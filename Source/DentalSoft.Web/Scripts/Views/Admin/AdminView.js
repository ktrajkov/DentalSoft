function AdminView(viewConfig) {
    this._viewConfig = viewConfig;
}

AdminView.prototype = {
    start: function () {
        var that = this;
        that._initMenu();
        that._initButtons();
        return this;
    },

    /* Controls and dataSource initialization */
    _initMenu: function(){
        $("#"+ this._viewConfig.selectors.menuId).kendoMenu();
    },
    _initButtons: function () {
        var that = this;
        $("#" + this._viewConfig.selectors.patientsButtonId).click(function (e) {
            location.href = that._viewConfig.urls.patints;
        });
        $("#" + this._viewConfig.selectors.diagnosesButtonId).click(function (e) {
            location.href = that._viewConfig.urls.diagnoses;
        });
        $("#" + this._viewConfig.selectors.treatmentsButtonId).click(function (e) {
            location.href = that._viewConfig.urls.treatments;
        });
    },
    

    /* Data members and global variables */
    _viewConfig: null,
}