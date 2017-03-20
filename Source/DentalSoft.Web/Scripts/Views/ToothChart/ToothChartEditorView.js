function ToothChartEditorView(viewConfig) {
    this._viewConfig = viewConfig;
}
ToothChartEditorView.prototype = {
    start: function () {
        var that = this;
        that._initGridEvents();
        return this;
    },

    /* Controls and dataSource initialization */
    _initGridEvents: function () {
        var that = this;
        var diagnosesGrid = $("#" + this._viewConfig.selectors.grid).data("kendoGrid");
        diagnosesGrid.bind("save", function (e) {
            that._onGridSave(e);
        });
        diagnosesGrid.bind("edit", function (e) {
            that._onGridEdit(e);
        });

    },

    _onGridEdit: function (e) {
        this._initDropDownEvents(e);
    },

    _onGridSave: function (e) {
        if (e.model.dirty) {
            var dropdowns = e.container.find("[data-role=dropdownlist]");
            var type = $(dropdowns[0]).data("kendoDropDownList").value();
            var color = $(dropdowns[1]).data("kendoDropDownList").value();
            if (type >= 0 || color >= 0) {
                e.model.set("ToothEditorModel",
               {
                   Type: type >= 0 ? type : null,
                   Color: color >= 0 ? color : null
               });
            } else
                e.model.set("ToothEditorModel", null);
        }
    },

    _initDropDownEvents: function (e) {
        var that = this;
        var dropdowns = e.container.find("[data-role=dropdownlist]");
        dropdowns.each(function (index, dropDown) {
            $(dropDown).data("kendoDropDownList").bind("select", function (e) {
                that._onDropDownSelect(e);
            });
        });

    },

    _onDropDownSelect: function (e) {
        if (e.item.val() !== "") {
            var diagnosesGrid = $("#" + this._viewConfig.selectors.grid).data("kendoGrid");
            var dataItem = diagnosesGrid.dataItem($(e.sender.wrapper).closest("tr"));
            if (!dataItem.ToothEditorModel) {
                dataItem.set("ToothEditorModel", {});
            }
            dataItem.dirty = true;
        }

    },

    /* Data members and global variables */
    _viewConfig: null,
};