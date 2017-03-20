function PatientModelView(viewConfig) {
    this._viewConfig = viewConfig;
}

PatientModelView.prototype = {
    start: function () {
      
        if (this._viewConfig.isPopUp) {
            this._initPatientPopup();  
        } else {
            this._model = kendo.observable(this._viewConfig.model);
            this._initIndicatorForTabsOfImages();
        }        
        this._initTabs();
        return this;
    },
    getModel: function () {
        return this._model;
    },
    setModel:function(model){
        this._model = model;
    },
    isPopup:function(){
        return this._viewConfig.isPopUp;
    },
    getTabStrip: function () {
        return this._tabStrip;
    },
    showAllTabs:function(){
        var items = this._tabStrip.items();
        this._tabStrip.enable(items, true);
    },   

    /* Popup initialization and event handlers */
    _initPatientPopup: function () {
        var that = this;
        this._getPatientGrid().one("edit", function (e) {
            that._onGridEdit(e);
        });
        this._getPatientGrid().one("save", function (e) {
            that._onGridSave(e);
        });
    },
    _onPopupOpen: function () {
        var that = this;
        $(".k-window-titlebar").remove();
        $("div.k-window").addClass("patient-popup");
        $(".k-window .k-window-titlebar").remove();
        $('.k-grid-update').remove();
        $(".k-popup-edit-form").data("kendoWindow").one("close", function (e) {
            that._onPopupClose(e);
        });
    },
    _onPopupClose: function (e) {
        this._getPatientGrid().dataSource.read();
    },

    /*Grid evens handlers*/
    _onGridEdit: function (e) {
        this._model = e.model;
        this._onPopupOpen();
        this._initIndicatorForTabsOfImages();
        this._initTabsVisibility(e);
    },
    _onGridSave: function (e) {
        this._initTabsVisibility(e);
    },

    /* Tabs initialization*/
    _initTabs: function () {
        var that = this;
        var patientTabstripId = '#' + this._viewConfig.selectors.patientTabstripId;
        this._tabStrip = $(patientTabstripId).kendoTabStrip({
            show: function (e) {
                if ($(e.item).hasClass(that._viewConfig.selectors.galleryTabs)) {
                    that._initGalleryTab(e);
                } else if ($(e.item).hasClass(that._viewConfig.selectors.statusTabs)) {
                    that._initStatusTab(e);
                }
            }
        }).data("kendoTabStrip");
    },
    _initTabsVisibility: function (e) {
        var items = this._tabStrip.items();
        if (e.model.isNew() && !e.model.dirty) {
            this._tabStrip.disable(items);
            var personalTab = this._tabStrip.tabGroup.children().eq(1);
            this._tabStrip.enable(personalTab, true);
            this._tabStrip.select(personalTab);

        } else {
            this._tabStrip.enable(items);
        }
    },

    /* Gallery tabs*/
    _initGalleryTab: function (e) {
        var imageType = $(e.item).data("type");
        if (!$(e.contentElement).find("." + this._viewConfig.selectors.imageGalleryId).data("kendoListView")) {
            new ImageGalleryDetailView({
                selectors: {
                    imageGalleryContainerId: this._viewConfig.selectors.imageGalleryContainerId,
                    imageGalleryId: this._viewConfig.selectors.imageGalleryId,
                    imageGalleryTamplate: this._viewConfig.selectors.imageGalleryTamplate,
                    patientTabstripId: this._viewConfig.selectors.patientTabstripId,
                    images: this._viewConfig.selectors.images,
                    deleteButtons: this._viewConfig.selectors.deleteButtons,
                    uploadButton: this._viewConfig.selectors.uploadButton,
                },
                imageType: imageType,
                urls: {
                    imageList: this._viewConfig.urls.imageList,
                    imageSave: this._viewConfig.urls.imageSave,
                    imageRemove: this._viewConfig.urls.imageRemove,
                },
                texts: {
                    uploadButton: this._viewConfig.texts.uploadButton,
                }
            }).start();
        }
    },
    _initIndicatorForTabsOfImages: function () {
        if (this._model.HasCTImages) {
            this._setNotEmptyTabIndicator(this._viewConfig.imageType.CT)
        }
        if (this._model.HasOPGImages) {
            this._setNotEmptyTabIndicator(this._viewConfig.imageType.OPG)
        }
        if (this._model.HasXrayImages) {
            this._setNotEmptyTabIndicator(this._viewConfig.imageType.Xray)
        }
        if (this._model.HasExtraoralImages) {
            this._setNotEmptyTabIndicator(this._viewConfig.imageType.Extraoral)
        }
        if (this._model.HasIntraoralImages) {
            this._setNotEmptyTabIndicator(this._viewConfig.imageType.Intraoral)
        }
    },
    _setNotEmptyTabIndicator: function (type) {
        var tab = $("." + this._viewConfig.selectors.galleryTabs + "[data-type=" + type + "]");
        $(tab).append("<span class='not-empty'></span>")
    },

    /* Status tabs*/
    _initStatusTab: function (e) {
        if ($(e.contentElement).find("div").length === 1) {
            new StatusDetailView({
                selectors: {
                    patientTabstripId: this._viewConfig.selectors.patientTabstripId,
                    generalStatusContainer: this._viewConfig.selectors.generalStatusContainer,
                    extraStatusContainer: this._viewConfig.selectors.extraStatusContainer,
                    intraStatusContainer: this._viewConfig.selectors.intraStatusContainer,
                },
                urls: {
                    deseaseList: this._viewConfig.urls.deseaseList,
                    deseasesUpdate: this._viewConfig.urls.deseasesUpdate,
                },
                texts: {
                    select: this._viewConfig.texts.select,
                }
            }).start();
        }
    },

    _getPatientGrid: function () {
        return $("#" + this._viewConfig.selectors.patientGridId).data("kendoGrid");
    },

    /* Data members and global variables */
    _viewConfig: null,
    _tabStrip: null,
    _model: null,
}