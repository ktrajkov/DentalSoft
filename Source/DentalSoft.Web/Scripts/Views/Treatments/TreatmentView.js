function TreatmentView(viewConfig) {
    this._viewConfig = viewConfig;
}
TreatmentView.prototype = {
    start: function () {
        var that = this;
        that._initButtons();
        that._initListView();
        that._initGrid();
        return this;
    },

    /* Controls and dataSource initialization */
    _initGrid: function () {
        var that = this;
        var treatmentGrid = $("#" + this._viewConfig.selectors.treatmentGridId).data("kendoGrid");   
        treatmentGrid.dataSource.transport.options.read.data = function (e) { that._additionalData(e);};
        treatmentGrid.dataSource.transport.options.create.data = function (e) { that._additionalData(e); }
        treatmentGrid.dataSource.transport.options.update.data = function (e) { that._additionalData(e); };
        treatmentGrid.dataSource.transport.options.destroy.data = function (e) { that._additionalData(e); };
    },

    _additionalData: function (e) {
        e.DiagnosisId = this._currentDiagnosisId;
    },

    _initButtons: function () {
    },

    _initListView: function () {
        var that = this;
        var tabStripSelector = $('#' + this._viewConfig.selectors.diagnosesTabId);
        tabStripSelector.kendoListView({
            template: kendo.template($("#" + this._viewConfig.selectors.templateId).html()),
            selectable: 'single',
            dataSource: {
                type: "json",
                serverFiltering: true,
                transport: {
                    read: {
                        url: this._viewConfig.urls.diagnoses,
                        type: "POST",
                        dataType: 'json',
                    },
                },
                schema: {
                    data: "Data"
                },
            },
            change: function () {
                var listViewSelector = $('#' + that._viewConfig.selectors.diagnosesTabId).data('kendoListView');
                var selectedItemUid = listViewSelector.select().attr("data-uid");
                selectedModel = listViewSelector.dataSource.getByUid(selectedItemUid);
                that._currentDiagnosisId = selectedModel.Id;
                $("#" + that._viewConfig.selectors.treatmentGridId).data("kendoGrid").dataSource.read();
            },
            dataBound: function () {
                var listViewSelector = $('#' + that._viewConfig.selectors.diagnosesTabId).data('kendoListView');
                listViewSelector.select(listViewSelector.element.children().first());

                that._initSplitter();
            }
        })
    },

    _initSplitter: function () {
        $('.' + this._viewConfig.selectors.splitterClass).kendoSplitter({
            orientation: "horizontal",
            panes: [
                { collapsible: false, size: this._viewConfig.settings.splitterLeftPaneWidth },
                { collapsible: false},
            ]
        });
    },

    /* Data members and global variables */
    _currentDiagnosisId: null,
    _viewConfig: null,
}