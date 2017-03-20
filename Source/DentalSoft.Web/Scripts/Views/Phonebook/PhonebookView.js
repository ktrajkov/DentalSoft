function PhonebookView(viewConfig) {
    this._viewConfig = viewConfig;
}
PhonebookView.prototype = {
    start: function () {
        var that = this;
        that._initListView();
        that._initGrid();

        return this;
    },
    /* Contact Grid  initialization */
    _initGrid: function () {
        var that = this;
        var contactsGrid = $("#" + this._viewConfig.selectors.contactsGrid).data("kendoGrid");
        contactsGrid.dataSource.transport.options.read.data = function (e) { that._additionalData(e); };
        contactsGrid.dataSource.transport.options.create.data = function (e) { that._additionalData(e); }
        contactsGrid.dataSource.transport.options.update.data = function (e) { that._additionalData(e); };
        contactsGrid.dataSource.transport.options.destroy.data = function (e) { that._additionalData(e); };
        contactsGrid.bind("dataBound", function (e) {
            that._onGridDataBound(e);
        })      
    },
    _onGridDataBound:function(e){
        if (!$('.' + this._viewConfig.selectors.splitterClass).data("kendoSplitter")) {
            this._initSplitter();
        }
    },
    _additionalData: function (e) {
        e.ContactCategory = this._currentContactCategory;
    },   

    /* Categories ListView  initialization */
    _initListView: function () {
        var that = this;
        var categoriesListView = $('#' + this._viewConfig.selectors.categoriesListView).data("kendoListView");
        categoriesListView.bind("change", function (e) {
            that._onListViewChange(e);
        });
        categoriesListView.select(categoriesListView.element.children().first());
        categoriesListView.trigger("change");
    },
    _onListViewChange:function(e){
        var categoriesListView = $('#' + this._viewConfig.selectors.categoriesListView).data('kendoListView');
        var selectedItemUid = categoriesListView.select().attr("data-uid");
        selectedModel = categoriesListView.dataSource.getByUid(selectedItemUid);
        this._currentContactCategory = selectedModel.Value;
        var grid = $("#" + this._viewConfig.selectors.contactsGrid).data("kendoGrid")
        grid.dataSource.read();
        if (selectedModel.Value !== '0') {
            grid.hideColumn("DentistInitials");
        } else {
            grid.showColumn("DentistInitials");
        }
    },

    _initSplitter: function () {
        $('.' + this._viewConfig.selectors.splitterClass).kendoSplitter({
            orientation: "horizontal",
            panes: [
                { collapsible: false, size: this._viewConfig.settings.splitterLeftPaneWidth, resizable: true },
                { collapsible: false, resizable: true },
            ]
        });
    },

    /* Data members and global variables */
    _viewConfig: null,
    _currentContactCategory: null,
}