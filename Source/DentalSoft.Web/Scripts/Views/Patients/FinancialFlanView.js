function FinancialPlanView(viewConfig) {
    this._viewConfig = viewConfig;
}

FinancialPlanView.prototype = {
    start: function () {
        var that = this;
        setTimeout(function () {
            that._initForm();
        }, 100);
        return this;
    },

    _initForm: function () {
        var that = this;
        this._bindForm();
        var form = $("#" + this._viewConfig.selectors.formId);
        form.submit(function (e) {
            that._onFormSubmit(e);
        });
    },
    _onFormSubmit: function (e) {
        var that = this;
        var model = patientModelView.getModel();
        if (!model.FinancialPlanModel) {
            model.FinancialPlanModel = { "Text": $("#" + this._viewConfig.selectors.financialPlanEditor).data("kendoEditor").value() };
        }
        e.preventDefault();
        $.ajax({
            url: that._viewConfig.urls.patientSave,
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(model),
            success: function (e) {
                that._onSubmitSuccess(e);
            },
            error: function (e) {
                error_handler(e);
            }
        });
    },

    _onSubmitSuccess: function (e) {
        success_handler(e);
        patientModelView.setModel(kendo.observable(e.Data[0]));
        this._bindForm();
    },
    _bindForm: function () {
        var form = $("#" + this._viewConfig.selectors.formId);
        kendo.bind(form, patientModelView.getModel().FinancialPlanModel);
    },
    _viewConfig: null,
}