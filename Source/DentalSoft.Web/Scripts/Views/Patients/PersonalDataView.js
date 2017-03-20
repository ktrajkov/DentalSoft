function PersonalDatatView(viewConfig) {
    this._viewConfig = viewConfig;
}

PersonalDatatView.prototype = {
    start: function () {
        var that = this;
        setTimeout(function () {
            that._initAddressComboBoxes();
            that._initForm();
        }, 100);
        return this;
    },

    /*Init Form*/
    _initForm: function () {
        var that = this;
        this._bindForm();
        var form = $("#" + this._viewConfig.selectors.formId);
        this._validator = form.kendoValidator().data("kendoValidator");
        this._initDentistDropdown();
        form.submit(function (e) {
            that._onFormSubmit(e);
        });
    },
    _onFormSubmit: function (e) {
        var that = this;
        e.preventDefault();
        that._setDentistIdIntoModel();
        if (that._validator.validate()) {
            $.ajax({
                url: that._viewConfig.urls.patientSave,
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(patientModelView.getModel()),
                success: function (e) {
                    that._onSubmitSuccess(e);
                },
                error: function (e) {
                    error_handler(e);
                }
            });
        }
    },
    _onSubmitSuccess: function (e) {
        success_handler(e);
        patientModelView.setModel(kendo.observable(e.Data[0]));
        this._bindForm();
        patientModelView.showAllTabs();
    },
    _bindForm: function () {
        var form = $("#" + this._viewConfig.selectors.formId);
        var model = patientModelView.getModel();
        if(!model.Id){
            model.set('PersonalDataModel.NextMedicalCheckUp', this._viewConfig.settings.nextMedicalCheckUp);
        }
        kendo.bind(form, model.PersonalDataModel);
    },
    _setDentistIdIntoModel: function () {
        var dentistId = $("#" + this._viewConfig.selectors.dentistId).data("kendoDropDownList").value();
        patientModelView.getModel().set("PersonalDataModel.DentistId", dentistId);
    },

    /* ComboBoxes initialization */
    _initAddressComboBoxes: function () {
        var that = this;
        regionComboBox = $("#" + that._viewConfig.selectors.regionId).kendoComboBox({
            suggest: true,
            valuePrimitive: true,
            dataTextField: "Text",
            dataValueField: "Value",
            filter: "contains",
            dataSource: {
                type: "json",
                serverFiltering: true,
                transport: {
                    read: {
                        url: this._viewConfig.urls.regionList,
                        type: "POST",
                        dataType: 'json',
                        data: function () {
                            return { name: $("#" + that._viewConfig.selectors.regionId).data("kendoComboBox").input.val() }
                        }
                    },

                },
            },
            dataBound: function (e) {
                that._initFilterFor(that._viewConfig.selectors.municipalityId);
                that._initFilterFor(that._viewConfig.selectors.locationId);
            }
        });

        municipalityComboBox = $("#" + that._viewConfig.selectors.municipalityId).kendoComboBox({
            autoBind: true,
            cascadeFrom: that._viewConfig.selectors.regionId,
            dataTextField: "Text",
            dataValueField: "Value",
            filter: "contains",
            valuePrimitive: true,
            suggest: true,
            dataSource: {
                type: "json",
                serverFiltering: true,
                transport: {
                    dataType: 'json',
                    read: {
                        type: "POST",
                        url: this._viewConfig.urls.municipalityList,
                        dataType: 'json',
                    },
                    parameterMap: function (obj, action) {
                        return that._getFilterFor(that._viewConfig.selectors.municipalityId, that._viewConfig.selectors.regionId)
                    }
                },
            },
        });

        locationComboBox = $("#" + that._viewConfig.selectors.locationId).kendoComboBox({
            autoBind: true,
            cascadeFrom: that._viewConfig.selectors.municipalityId,
            dataTextField: "Text",
            dataValueField: "Value",
            filter: "contains",
            suggest: true,
            valuePrimitive: true,
            dataSource: {
                type: "json",
                serverFiltering: true,
                transport: {
                    read: {
                        type: "POST",
                        url: this._viewConfig.urls.locationList,
                        dataType: 'json',
                    },
                    parameterMap: function (obj, action) {
                        return that._getFilterFor(that._viewConfig.selectors.locationId, that._viewConfig.selectors.municipalityId)
                    }
                },
            }
        }).data("kendoComboBox");
    },
    _initFilterFor: function (selector) {
        var comboBox = $("#" + selector).data("kendoComboBox");
        var inputVal = comboBox.input.val();
        if (!isNaN(inputVal) && inputVal !== "") {
            comboBox.enable(true);
            comboBox.dataSource.read();
        }
    },
    _getFilterFor: function (selector, selectorCascadeFrom) {
        var comboBox = $("#" + selector).data("kendoComboBox");
        var inputVal = comboBox.input.val();

        if (!isNaN(inputVal) && inputVal !== "") {
            obj = {};
            obj.id = inputVal;
        } else {
            var cascadeComboBox = $("#" + selectorCascadeFrom).data("kendoComboBox");
            obj = {};
            if (selector === this._viewConfig.selectors.municipalityId) {
                obj.regionId = cascadeComboBox.dataItem().Value;
            } else {
                obj.municipalityId = cascadeComboBox.dataItem().Value;
            }
            obj.name = inputVal;
        }
        return obj;
    },

    /* Dropdown initialization */
    _initDentistDropdown: function () {
        if (patientModelView.getModel().Id) {
            return;
        }
        var dropdown = $("#" + this._viewConfig.selectors.dentistId).data("kendoDropDownList");
        dropdown.select(0);
    },

    /* Data members and global variables */
    _viewConfig: null,
    _validator: null,
}