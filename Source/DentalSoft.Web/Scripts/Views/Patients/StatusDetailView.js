function StatusDetailView(viewConfig) {
    this._viewConfig = viewConfig;
}

StatusDetailView.prototype = {

    start: function () {
        var that = this;
        this._initContainerSelector();
        this._getDeseases();
        return this;
    },
    _initContainer: function () {
        this._initContent();
    },

    _initContent: function () {
        var statusIsIntraoral = this._getStatusName() === 'Intraoral';
        var categories = this._data[0].DeseaseCategories;
        for (var i = 0; i < categories.length; i++) {
            if (statusIsIntraoral) {
                this._initMultiSelect(categories[i]);
            }
            else {
                this._initCheckBoxesAndInfoes(categories[i]);
            }
        }
        statusIsIntraoral ? this._setClearFix() : null;
        this._initAdditionalInfo();
        this._initSaveButton();
    },

    /* Init Multiselects */
    _initMultiSelect: function (category) {
        this._container.append(this._getMultiSelectHtml(category));       
        this._setDataOfMultiSelect(category);
    },
    _getMultiSelectHtml: function (category) {
        return '<div class="multi-select-container"><label class="label-category">' + category.Name + '</label>' +
                    '<select id="MultiSelect' + category.Id + '"></select></div>';
    },
    _setClearFix:function(){
        this._container.append('<div class="clearfix"></div>');
    },

    _setDataOfMultiSelect: function (data) {
        $("#MultiSelect" + data.Id).kendoMultiSelect({
            animation: {
                open: {
                    effects: "zoom:in",
                    duration: 300
                }
            },
            autoClose: false,
            placeholder: this._viewConfig.texts.select,
            dataTextField: "Name",
            dataValueField: "Id",
            dataSource: {
                data: data.Deseases
            },
            value: $.map(patientModelView.getModel().Deseases, function (item) {
                return item.Id;
            }),
        });
    },
    _getIdsFromMultiSelets: function () {
        var multiSelects = this._container.find("select");
        var ids = [];
        multiSelects.each(function (index, element) {
            var items = $(element).data("kendoMultiSelect").dataItems();
            if (items.length > 0) {
                $(items).each(function (index, element) {
                    ids.push(element.Id);
                })
            }
        });
        return ids;
    },

    /* Init CheckBoxes */
    _initCheckBoxesAndInfoes: function (category) {
        this._container.append(this._getCheckBoxesAndInfoesHtml(category));
        this._setDataOfCheckBoxes();
        this._setDataOfAdditionalInfoesforDeseases();
    },
    _getCheckBoxesAndInfoesHtml: function (category) {
        var deseases = category.Deseases;
        var content = '<div class="category-container"><label class="label-category">' + category.Name + '</label>';
        for (var i = 0; i < deseases.length; i++) {
            content += this._getCheckBoxHtml(deseases[i])
            if (deseases[i].HasAdditionalInfo) {
                content += this._getAdditionalInfoHtml(deseases[i])               
            }
            content += '</div>';
        }
        content += '</div>';
        return content;

    },
    _getCheckBoxHtml: function (desease) {
        return '<div class="row desease-container"><input type="checkbox" id="Desease' + desease.Id +
                  '" class="k-checkbox" value=' + desease.Id + ' data-bind="checked: ids"/>' +
                  '<label class="k-checkbox-label" for="Desease' + desease.Id + '">' + desease.Name + '</label>';
    },
    _getAdditionalInfoHtml: function (desease) {
        return '<div class="desease-info-container"><textarea class="desease-info row k-textbox" id="DeseaseInfo_'
            + desease.Id + '" ></textarea></div>';
    },

    _setDataOfCheckBoxes: function () {
        var statusName = this._getStatusName();
        var ids = $.map(patientModelView.getModel().Deseases, function (item) {
            return item.Id.toString();
        })
        this._checkBoxesViewModel = kendo.observable({
            ids: ids
        });
        kendo.bind(this._container.find("input"), this._checkBoxesViewModel);
    },
    _getIdsFromCheckBoxes: function () {
        return this._checkBoxesViewModel.ids;
    },

    /* AdditionaInfo functions*/
    _initAdditionalInfo: function () {
        if (this._data[0].HasAdditionalInfo) {
            this._container.append('<div class="status-info-container"><textarea class="status-info row k-textbox"></textarea><div>');
            this._setDataOfAdditionalInfoForStatus();
        }
    },

    _setDataOfAdditionalInfoForStatus: function (e) {
        var that = this;
        var additionalInfoes = e != null ? e : patientModelView.getModel().AdditionalInfoes;
        var additionalInfoForStatus = additionalInfoes.filter(function (e) {
            return e.StatusId != null && e.StatusId === that._data[0].Id;
        });
        if (additionalInfoForStatus.length > 0) {
            var statusInfoElement = this._container.find('.status-info');
            if (additionalInfoForStatus[0].Info) {
                statusInfoElement.text(additionalInfoForStatus[0].Info);
            }          
            statusInfoElement.data('infoId', additionalInfoForStatus[0].Id)

        }
    },
    _setDataOfAdditionalInfoesforDeseases: function (e) {
        var additionalInfoes = e != null ? e : patientModelView.getModel().AdditionalInfoes
        var additionalInfoesForDeseases = additionalInfoes.filter(function (e) {
            return e.DeseaseId != null;
        });
        $(additionalInfoesForDeseases).each(function (index, element) {
            var deseaseInfoElement = $("#DeseaseInfo_" + element.DeseaseId);
            if (element.Info) {
                deseaseInfoElement.text(element.Info);
            }
            deseaseInfoElement.data('infoId', element.Id)
        })
    },
    _getAdditionalInfoes: function () {
        var that = this;
        var infoes = [];
        var patientId = patientModelView.getModel().Id;
        var statusElement = this._container.find('.status-info');
        if (statusElement.length > 0) {
            var statusInfoId = statusElement.data('infoId');
            var statusInfoText = statusElement.val();
            if (statusInfoId != null || statusInfoText != "") {
                infoes.push({
                    Id: statusInfoId,
                    PatientId: patientId,
                    StatusId: this._data[0].Id,
                    Info: statusInfoText
                });
            }
        }

        $(this._container.find('.desease-info')).each(function (index, element) {
            var jElement = $(element);
            var infoId = jElement.data('infoId');
            var infoText = jElement.val();
            if (infoId != null || infoText != "") {
                var deseiseElementId = jElement.attr('id');
                var deseaseId = deseiseElementId.substring(deseiseElementId.indexOf('_') + 1, deseiseElementId.length);
                infoes.push({
                    Id: infoId,
                    DeseaseId: deseaseId,
                    Info: infoText,
                    PatientId: patientId
                })
            }

        });
        return infoes;
    },

    /* Button initialization */  
    _initSaveButton: function () {
        var that = this;
        this._container.append('<div class="row status-save-button"><a class="k-button k-button-icontext k-primary" href="#">' +
            '<span class="k-icon k-update"></span>Save</a></div>');
        this._container.find('.status-save-button').click(function (e) {
            that._onSaveButtonClick();
        });
    },
    _onSaveButtonClick: function () {
        var ids = this._getStatusName() === "Intraoral" ? this._getIdsFromMultiSelets() : this._getIdsFromCheckBoxes();
        var infoes = this._getAdditionalInfoes();
        var data = {
            StatusName: this._getStatusName(),
            PatientId: patientModelView.getModel().Id,
            DeseasesIds: ids,
            AdditionalInfoes: infoes
        };
        this._saveModel(data);
    },

    _getDeseases: function () {
        var that = this;
        var dataSource = new kendo.data.DataSource({
            serverFiltering: true,
            transport: {
                read: {
                    type: "POST",
                    url: this._viewConfig.urls.deseaseList,
                    data: function () {
                        return { Name: that._getStatusName() }
                    }
                },
            },
            change: function (e) {
                that._data = e.sender.view();
                that._initContent();
            },
        });
        dataSource.fetch();
    },

    _saveModel: function (data) {
        var that = this;
        $.ajax({
            url: this._viewConfig.urls.deseasesUpdate,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(data),
            success: function (e) {
                that._onSaveSuccess(e);
            }
        });
    },
    _onSaveSuccess: function (e) {
        success_handler(e);
        this._setDataOfAdditionalInfoesforDeseases(e);
        this._setDataOfAdditionalInfoForStatus(e);
    },

    _getTabStrip: function () {
        return $("#" + this._viewConfig.selectors.patientTabstripId).data("kendoTabStrip");
    },
    _getStatusName: function () {
        return this._getTabStrip().select().data("status");
    },
    _initContainerSelector: function () {
        var container;
        var status = this._getStatusName();
        switch (status) {
            case "General": {
                this._container = $("#" + this._viewConfig.selectors.generalStatusContainer);
                break;
            };
            case "Extraoral": {
                this._container = $("#" + this._viewConfig.selectors.extraStatusContainer);
                break;
            };
            case "Intraoral": {
                this._container = $("#" + this._viewConfig.selectors.intraStatusContainer);
                break;
            };
        };
    },

    /* Data members and global variables */
    _viewConfig: null,
    _container: null,
    _data: null,
    _checkBoxesViewModel: null,
}