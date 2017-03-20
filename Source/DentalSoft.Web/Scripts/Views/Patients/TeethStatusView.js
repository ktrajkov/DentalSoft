function TeethStatusView(viewConfig) {
    this._viewConfig = viewConfig;
}

TeethStatusView.prototype = {
    start: function () {
        var that = this;
        this._initGrid();
        this._initGridEvents();
        this._initTeethStatusChart();
        this._initToothImages();
        this._initToothImagesUploader();
        //check for init parent model
        setTimeout(function () {
            that._attachTabStripEvents();         
        }, 100);
       
        return this;
    },

    /*Tabstrip events*/
    _attachTabStripEvents: function () {
        patientModelView.getTabStrip().bind('show', this._onTabStripShow.bind(this))
    },
    _onTabStripShow: function (e) {
        if ($(e.item).hasClass(this._viewConfig.selectors.teethStatusTab)) {
            this._operationGrid.dataSource.read();
        }
    },

    /* Grid events and initialization */
    _initGrid: function () {
        this._operationGrid = $("#" + this._viewConfig.selectors.operationGrid).data("kendoGrid");
        this._operationGrid.dataSource.options.serverFiltering = false;
        this._initOperationDate();
        this._initCheckBoxForFilter();
        this._filterCheckBoxValue(false);
        this._initClearSelectedButton();
        this._initAddNewRowButton();
    },
    _initOperationDate: function () {
        this._getGridHeader().append('<span><input id="operationDateTime"/></span>');
        $("#operationDateTime").kendoDateTimePicker({
            format: 'dd.MM.yyyy',
            value: new Date()
        });


    },
    _initPatientName: function () {
        if (this._getGridHeader().find('#patient-name-container').length === 0) {
            var html = '<div id="patient-name-container"><span data-bind="text: FirstName"></span> <span data-bind="text: LastName"></span></div>';          
            this._getGridHeader().append(html);
            kendo.bind($("#patient-name-container"), patientModelView.getModel().PersonalDataModel)
        }
    },
    _getGridHeader: function () {
        return this._operationGrid.element.find(".k-grid-toolbar");
    },
    _getEditRow: function () {
        return this._operationGrid.element.find('.k-grid-edit-row');
    },
    _setModelAtFirstPosition: function (model) {
        var grid = this._operationGrid
        var index = grid.dataSource.indexOf(model);
        if (index !== 0) {
            grid.dataSource.remove(model);
            grid.dataSource.insert(0, model);
            grid.editRow(grid.tbody.find(">tr:first"));
        }
    },
    _openDiagnosisDropDownList: function () {
        var diagnosisDropDownList = $("#" + this._viewConfig.selectors.diagnosisId).data("kendoComboBox");
        diagnosisDropDownList.open();
        diagnosisDropDownList.focus();
    },
    _getOperationGridData:function(){
        return this._operationGrid.dataSource.data();
    },

    /*Init Buttons*/
    _initAddNewRowButton: function () {
        var that = this;
        $('.k-grid-add').bind("click", function (e) {
            that._onAddNewRowClick();
        });
    },
    _initClearSelectedButton: function () {
        var that = this;
        var buttonHtml = '<span id="clear-selected-button" class="k-button text-center" aria-hidden="true">' +
                     '<i class="glyphicon glyphicon-erase"></i></span>';
        this._getGridHeader().append(buttonHtml);
        $('#clear-selected-button').bind("click", function (e) {
            that._onClearSelectedButton();
        });
    },
    _onAddNewRowClick: function (e) {
        this._clearFilters();
        this._filterCheckBoxValue(false);
        this._filterCheckBoxDisabled(true);
    },
    _onClearSelectedButton: function (e) {
        this._teethChartView.clearSelectedTeeth();
        var editRow = this._getEditRow();
        if (editRow.length > 0) {
            editRow.find("#" + this._viewConfig.selectors.toothId).val("");
        } else {
            this._filterGridByTeeth([]);
        }

    },

    /*Grid events*/
    _initGridEvents: function () {
        var that = this;
        var operationGrid = that._operationGrid;
        operationGrid.bind("dataBound", function (e) {
            that._onGridDataBound(e);
        });
        operationGrid.bind("save", function (e) {
            that._onGridSave(e);
        });
        operationGrid.bind("edit", function (e) {
            that._onGridEdit(e);
        });
        operationGrid.bind("change", function (e) {
            that._onGridSelected(e);
        });
        operationGrid.bind("cancel", function (e) {
            that._onGridCancel(e);
        });
        operationGrid.dataSource.transport.options.read.data = function (e) { that._additionalData(e); };
        setTimeout(function () {          //waiting for binding        
            operationGrid.dataSource.read();
        }, 100);

    },
    _onGridDataBound: function (e) {
        var firstRow = this._operationGrid.tbody.find(">tr:first");
        if (firstRow.length === 0) {
            this._teethChartView.setCharts();
        } else {
            var item = this._operationGrid.dataItem(firstRow);
            if (item != null && !item.isNew()) {
                this._teethChartView.setCharts(e.sender.dataSource.data());
            }
        }
        this._loadTeethWithImage();
        this._initPatientName();
    },
    _onGridSave: function (e) {
        var diagnosisId = $("#" + this._viewConfig.selectors.diagnosisId).data("kendoComboBox").value();
        var treatmentId = $("#" + this._viewConfig.selectors.treatmentId).data("kendoComboBox").value();
        var treatmentDate = $("#" + this._viewConfig.selectors.treatmentDateTimeId).data("kendoDateTimePicker");
        if (!treatmentId) {
            treatmentDate.value("");
        }
        var diagnosisDate = $("#" + this._viewConfig.selectors.diagnosisDateTimeId).data("kendoDateTimePicker").value();
        var dentistId = $("#" + this._viewConfig.selectors.dentistId).data("kendoDropDownList").value();
        var position = $("#" + this._viewConfig.selectors.positionId).data("kendoDropDownList").value();
        e.model.set("Teeth", this._teethChartView.getSelectedToothNumbers().map(function (element) {
            return { Number: element };
        }));
        e.model.set("DiagnosisId", diagnosisId);
        e.model.set("TreatmentId", treatmentId >= 0 ? treatmentId : null);
        e.model.set("DiagnosisDateTime", diagnosisDate);
        e.model.set("TreatmentDateTime", treatmentDate.value());
        e.model.set("Position", position);
        e.model.set("DentistId", dentistId);
        e.model.set("PatientId", patientModelView.getModel().Id);

        this._filterCheckBoxDisabled(false);
    },
    _onGridSelected: function (e) {
        var selectedItem = e.sender.dataItem(e.sender.select());
        var gridData = e.sender.dataSource.data();
        var indexOfSelectedItem = gridData.indexOf(selectedItem)
        var filteredData = gridData.slice(indexOfSelectedItem, gridData.length);
        this._teethChartView.setCharts(filteredData);
    },
    _onGridEdit: function (e) {
        var that = this;
        if (e.model.isNew()) {
            this._setModelAtFirstPosition(e.model);
        }
        var teethTextBox = $(e.container).find("#" + this._viewConfig.selectors.toothId);
        if (!e.model.Id) {
            var selectedTeeth = this._teethChartView.getSelectedToothNumbers();
            teethTextBox.val(selectedTeeth.toString());
            this._openDiagnosisDropDownList();
        } else {
            var toothNumbers = e.model.Teeth.map(function (e) { return e.Number.toString() });
            this._teethChartView.setSelectedTeeth(toothNumbers);
            teethTextBox.val(toothNumbers.toString());
        }
        $(e.container).find('td').on('keypress', function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) { //Enter keycode
                that._operationGrid.saveRow();
            }
        });
    },
    _onGridCancel: function (e) {
        this._filterCheckBoxDisabled(false);
    },

    /* Filter initialization and events*/
    _filterGridByTeeth: function (teeth) {
        this._clearFilters();
        if (teeth.length > 0) {
            this._operationGrid.dataSource.filter({
                logic: 'or',
                filters: [{
                    field: 'Teeth',
                    operator: function (item) {
                        var result = item.filter(function (n) {
                            return teeth.indexOf(n.Number.toString()) != -1
                        });
                        return result.length > 0;
                    },
                    value: ""
                }]
            })
        } else {
            this._operationGrid.dataSource.read();
        }
    },
    _clearFilters: function () {
        this._operationGrid.dataSource._filter = null;
    },
    _initCheckBoxForFilter: function () {
        var that = this;
        this._getGridHeader().append('<div class="filter-teeth-checkbox"><input type="checkbox" id="filterByTeeth" class="k-checkbox" checked="checked" >' +
          '<label class="k-checkbox-label" for="filterByTeeth">' + this._viewConfig.texts.filterByTeeth + '</label></div>');
        $(".filter-teeth-checkbox").bind("click", function (e) {
            that._onFilterCheckBoxClick(e);
        });
    },
    _filterCheckBoxValue: function (value) {
        if (value !== undefined) {
            $('#filterByTeeth').prop('checked', value);
        } else {
            return $('#filterByTeeth').is(":checked");
        }

    },
    _filterCheckBoxDisabled: function (value) {
        $("#filterByTeeth").prop("disabled", value);
    },
    _onFilterCheckBoxClick: function (e) {
        if (this._filterCheckBoxValue()) {
            this._filterGridByTeeth(this._teethChartView.getSelectedToothNumbers());
        } else {
            this._clearFilters();
        }
    },

    _additionalData: function (e) {
        e.PatientId = patientModelView.getModel().Id;
    },

    /* Teeth status chart initialization and events */
    _initTeethStatusChart: function () {
        var that = this;
        var element = $('#' + this._viewConfig.selectors.teethStatusChartId);
        element.teethChartView({
            data: that._operationGrid.dataSource.data(),
            handleClick: function (e, args) {
                that._onTeethStatusHandleClick(e, args);
            },
            teethStatusChartId: that._viewConfig.selectors.teethStatusChartId
        });
        that._teethChartView = element.data('teethChartView');
    },
    _onTeethStatusHandleClick: function (e, args) {
        var editRow = $("#" + this._viewConfig.selectors.operationGrid + " .k-grid-edit-row");
        var teethTextBox = editRow.find("#" + this._viewConfig.selectors.toothId);
        if (editRow.length > 0) {
            teethTextBox.val(args.data.toString());
        } else if (this._filterCheckBoxValue()) {
            this._filterGridByTeeth(args.data);
        }

        if (args.data.length === 1) {    // Has one selected tooth
            this._showUploader(true);
            if (this._teethChartView.hasImage(args.data[0])) {
                this._loadToothImages(args.data[0]);
            }
        } else {
            this._showUploader(false);
            this._toothImages.clear();
        }
    },

    _loadTeethWithImage: function () {
        var patientId = patientModelView.getModel().Id;
        if (patientId) {
            var that = this;
            $.ajax({
                url: this._viewConfig.urls.teethWithImage,
                data: {
                    PatientId: patientId
                },
                type: 'POST',
                success: function (data) {
                    that._onLoadTeethWithImageSuccess(data);
                }
            });
        }
    },
    _onLoadTeethWithImageSuccess: function (e) {
        this._teethChartView.setImageIndicators(e.map(function (e) { return e.ToothNumber }));
    },

    /* Tooth images initialization */
    _initToothImages: function () {
        var that = this;
        var element = $('#' + this._viewConfig.selectors.toothImagesId);
        element.imageSlider({
            data: [],
            width: this._viewConfig.settings.imageSliderWidth,
            height: this._viewConfig.settings.imageSliderHeight,
            imgWidth: this._viewConfig.settings.imageSliderImgWidth,
            selectors: {
                image: this._viewConfig.selectors.toothImage
            },
            deleteImage: function (e, args) {
                that._onDeleteImage(e, args);
            },
        });
        that._toothImages = element.data('imageSlider');


    },
    _initZoomming: function () {
        $('#' + this._viewConfig.selectors.toothImagesId).find('.' + this._viewConfig.selectors.toothImage).glisse({
            changeSpeed: 550,
            speed: 500,
            effect: 'bounce',
            fullscreen: false
        });
    },
    _loadToothImages: function (toothNumber) {
        var that = this;
        $.ajax({
            url: this._viewConfig.urls.imageList,
            data: {
                ToothNumber: toothNumber,
                Type: 'Tooth',
                PatientId: patientModelView.getModel().Id
            },
            type: 'POST',
            success: function (data) {
                that._onLoadToothImagesSuccess(data);
            }
        });
    },
    _onLoadToothImagesSuccess: function (e) {
        var images = $(e).map(function () { return this.ImageUrl; });
        this._toothImages.setData(images);
        this._initZoomming();
    },
    _onDeleteImage: function (e, args) {
        var that = this;
        $.ajax({
            url: this._viewConfig.urls.imageRemove,
            data: {
                fileName: args.data,
                patientId: patientModelView.getModel().Id,
                imageType: "Tooth"
            },
            type: 'POST',
            success: function (data) {
                that._onDeleteSuccess(data);
            }
        });
    },
    _onDeleteSuccess: function (e) {
        this._loadToothImages(this._teethChartView.getSelectedToothNumbers()[0]);
        this._initZoomming();
        this._loadTeethWithImage();
    },


    /* Upload images initialization and  events */
    _initToothImagesUploader: function () {
        var that = this;
        kendo.ui.Upload.fn._supportsDrop = function () { return false; }
        $('#' + this._viewConfig.selectors.toothImagesUploadId).kendoUpload({
            enabled: false,
            async: {
                saveUrl: this._viewConfig.urls.imageSave,
                removeUrl: this._viewConfig.urls.imageRemove
            },
            upload: function (e) {
                that._onUpload(e);
            },
            success: function (e) {
                that._onUploadSuccess(e);
            },
            error: function (e) {
                that._hideStatusUpload();
            },
            localization: {
                select: this._viewConfig.texts.uploadButton,
                statusUploaded: "",
                headerStatusUploading: "",
                headerStatusUploaded: ""
            },
            showFileList: false
        });
        this._uploader = $('#' + this._viewConfig.selectors.toothImagesUploadId).data("kendoUpload");
    },
    _onUpload: function (e) {
        e.data = {
            patientId: patientModelView.getModel().Id,
            imageType: this._viewConfig.imageType.tooth,
            toothNumber: this._teethChartView.getSelectedToothNumbers()[0]
        };
    },
    _onUploadSuccess: function (e) {
        this._hideStatusUpload();
        if (this._teethChartView.getSelectedToothNumbers().length === 1) {
            this._loadToothImages(this._teethChartView.getSelectedToothNumbers()[0]);
        }
        this._initZoomming();
        this._loadTeethWithImage();
    },
    _showUploader: function (visible) {
        visible ? this._uploader.enable() : this._uploader.disable();
    },
    _hideStatusUpload: function () {
        $('#' + this._viewConfig.selectors.toothImagesUploadContainerId)
                  .find('.k-upload').addClass('k-upload-empty');
    },

    /* Data members and global variables */
    _viewConfig: null,
    _operationGrid: null,
    _teethChartView: null,
    _toothImages: null,
    _uploader: null,
}