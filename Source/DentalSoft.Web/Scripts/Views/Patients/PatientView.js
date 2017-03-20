function PatientView(viewConfig) {
    this._viewConfig = viewConfig;
}
    
PatientView.prototype = {
    start: function () {
        var that = this;
        that._initGridEvents();
       
        return this;
    },
    
    /* Controls and dataSource initialization */
    _initGridEvents :function(){
        var patientGrid = $("#" + this._viewConfig.selectors.patientGridId).data("kendoGrid");
        patientGrid.dataSource.bind("requestEnd", this._onRequestEnd)
    },

    /* Grid event handlers */
    _onRequestEnd: function (e) {
        if (e.type !== "read") {
            var grid = $('#PatientsGrid').data("kendoGrid");
            grid.one("dataBinding", function (ev) {
                ev.preventDefault();
            });                  
        }       
    },

    /* Data members and global variables */
    _viewConfig: null,
}