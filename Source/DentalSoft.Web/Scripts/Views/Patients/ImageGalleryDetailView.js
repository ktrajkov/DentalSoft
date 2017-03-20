function ImageGalleryDetailView(viewConfig) {
    this._viewConfig = viewConfig;
}

ImageGalleryDetailView.prototype = {

    start: function () {
        var that = this;
        this._intiContainer()
        this._initListView();
        this._initUploader();
        return this;
    },

    _intiContainer: function () {
        this._container = $(patientModelView._tabStrip.contentElement(patientModelView._tabStrip.select().index()));
    },

    _getImageGalleryContainer: function () {
        return this._container.find('.' + this._viewConfig.selectors.imageGalleryContainerId);
    },
    _getImageGallery: function () {
        return this._container.find('.' + this._viewConfig.selectors.imageGalleryId);
    },

    _loadImages: function () {
        var that = this;
        return dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: this._viewConfig.urls.imageList,
                    type: "POST",
                    data: {
                        Type: this._viewConfig.imageType,
                        PatientId: patientModelView._model.Id
                    },
                },
            },
            change: function (e) {
                that._onChangeImage(e);
            }
        });
    },
    _refreshImages: function () {
        this._getImageGallery().data("kendoListView").dataSource.read();
        // TODO: Check only for curent tab
        //  patientModelView._initTabsOfImages();
    },
    _onChangeImage:function(e){
        this._setImageTabIndicator(e.sender.data().length)
    },

    _setImageTabIndicator: function (imageCount) {
        var currentTab = patientModelView._tabStrip.select();
        var availableIndicator = currentTab.find('span.not-empty');
        if (imageCount > 0 && availableIndicator.length == 0) {
            currentTab.append("<span class='not-empty'></span>");

        } else if (imageCount == 0 && availableIndicator.length > 0) {
            availableIndicator.remove();
        }
    },

    /* ListView initialization */
    _initListView: function () {
        var that = this;
        this._getImageGallery().kendoListView({
            dataSource: this._loadImages(),
            template: kendo.template(this._container.find('#' + this._viewConfig.selectors.imageGalleryTamplate).html()),
            dataBound: function () {
                that._onListViewDataBound();
            }
        });
    },
    _onListViewDataBound:function(e){
        this._initZoomming();
        this._initDeleteButtons();
    },

    /* Zooming initialization */
    _initZoomming: function () {
        this._container.find('.' + this._viewConfig.selectors.images).glisse({
            changeSpeed: 550,
            speed: 500,
            effect: 'bounce',
            fullscreen: false
        });
    },

    /* Delete images initialization */
    _initDeleteButtons: function () {
        var that = this;
        this._container.find('.' + this._viewConfig.selectors.deleteButtons).bind('click', function (e) {
            that._onDeleteButtonClick(this);
        })
    },
    _onDeleteButtonClick:function(e){
        var imageSrc = $(e).prev().find('img').attr('src');
        var imageName = imageSrc.slice(imageSrc.lastIndexOf('/') + 1, imageSrc.length);
        this._deleteImage(imageName);
    },
    _deleteImage: function (imageName) {
        var that = this;
        $.ajax({
            url: this._viewConfig.urls.imageRemove,
            data: {
                fileName: imageName,
                patientId: patientModelView._model.Id,
                imageType: this._viewConfig.imageType
            },
            type: 'POST',
            success: function (data) {
                that._onDeleteImageSuccess(data);
            }
        });
    },
    _onDeleteImageSuccess:function(e){
        this._refreshImages();
    },



    /* Upload images initialization */
    _initUploader: function () {
        var that = this;
        kendo.ui.Upload.fn._supportsDrop = function () { return false; }
        this._container.find('.' + this._viewConfig.selectors.uploadButton).kendoUpload({
            enabled: true,
            async: {
                saveUrl: this._viewConfig.urls.imageSave,
                removeUrl: this._viewConfig.urls.imageRemove,
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
        this._container.find('.' + this._viewConfig.selectors.uploadButton).data("kendoUpload");
    },
    _onUpload: function (e) {
        e.data = {
            patientId: patientModelView._model.Id,
            imageType: this._viewConfig.imageType,
        };
    },
    _onUploadSuccess: function (e) {
        this._hideStatusUpload();
        this._refreshImages();

    },
    _hideStatusUpload: function () {
        this._getImageGalleryContainer().find('.k-upload').addClass('k-upload-empty');
        this._getImageGalleryContainer().find('.k-upload-status-total').css({ "display": "none" })
    },


    /* Data members and global variables */
    _viewConfig: null,
    _container: null,

}