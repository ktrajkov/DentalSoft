$.widget("DentalSoft.imageSlider", {
    options: {
        data: null,
        height: null,
        width: null,
        imgWidth: null,
        selectors: {
            container: 'image-slider',
            prev: 'control-prev',
            next: 'control-next',
            deleteButton:'delete-button',   
        },
        //Events
        deleteImage: null,
    },
    setData: function (data) {
        this.options.data = data;
        this.clear();
        this._create();
    },

    clear: function () {
        this._getContainer().remove();
    },

    _create: function () {
        this.element.data("imageSlider", this);
        if (this.options.data.length > 0) {
            this._createContainer();
            this._setMainOptions();
            this._setNavigatorOptions();
            this._initSlider();
            this._initButtons();
        }     
    },
    _createContainer: function () {
        var data = new Date().getTime();
        var html = '<div class="' + this.options.selectors.container + '">' +
            '<a href="#" class="' + this.options.selectors.prev + '"><</a><ul>';
        for (var i = 0; i < this.options.data.length; i++) {
            html += '<li><img src="/' + this.options.data[i] + '?' + data + '" data-glisse-big="/' +
                this.options.data[i] + '?' + data + '" class="' + this.options.selectors.image + '" ></li>';
        }
        html += '</ul><a href="#" hidden class="' + this.options.selectors.next + '">></a>'+            
           '<div class="k-button '+this.options.selectors.deleteButton +'">'+
           '<span class="glyphicon glyphicon-trash" aria-hidden="true"></span></div></div>';
        $(html).appendTo(this.element);      
    },  

    _setMainOptions: function () {
        var liSelector = this._getContainer().find('li');
        liSelector.css({ width: this.options.width, height: this.options.height });
        liSelector.css({ 'line-height': this.options.height });
        liSelector.find('img').css({ width: this.options.imgWidth });
    },
    _setNavigatorOptions: function () {
        var that = this;
        this._showPrevNavigator(false);

        if (this.options.data.length == 1) {
            this._showNextNavigator(false);
        }

        this._getPrevNavigator().bind('click', function (e) {
            that._moveLeft(e);
        });

        this._getNextNavigator().bind('click', function (e) {
            that._moveRight(e);
        });
    },
    _initSlider: function () {
        this._currentItem = 1;
        var selector = this._getContainer()
        var slideCount = selector.find('li').length;
        var slideWidth = this.options.width;
        var slideHeight = this.options.height;
        var sliderUlWidth = slideCount * slideWidth;

        selector.css({ width: slideWidth, height: slideHeight });

        selector.find('ul').css({ width: sliderUlWidth });




    },

    //Buttons initialization
    _initButtons: function () {
        var that = this;
        this._getDeleteButton().click(function (e) {
            that._trigger("deleteImage", null, {
                element: e,
                data: that._getCurrentImageName(),
            });
        })
    },

    _getPrevNavigator: function () {
        return $('a.' + this.options.selectors.prev);
    },
    _getNextNavigator: function () {
        return $('a.' + this.options.selectors.next);
    },
    _showNextNavigator: function (visible) {
        visible ? this._getNextNavigator().css({ visibility: 'visible' }) : this._getNextNavigator().css({ visibility: 'hidden' });
    },
    _showPrevNavigator: function (visible) {
        visible ? this._getPrevNavigator().css({ visibility: 'visible' }) : this._getPrevNavigator().css({ visibility: 'hidden' });
    },
    _getDeleteButton:function(){
        return $('div.' + this.options.selectors.deleteButton);
    },
    _getCurrentImageName: function () {
        var imageSrc = $(this._getContainer().find('img')[this._currentItem-1]).attr('src');
        return imageSrc.slice(imageSrc.lastIndexOf('/') + 1, imageSrc.lastIndexOf('?'));
    },

    _moveRight: function (e) {
        var that = this;
        that._currentItem++;
        that._showPrevNavigator(true);
        if (that._currentItem == that.options.data.length) {
            that._showNextNavigator(false);

        }
        var selector = that._getContainer();
        selector.find('ul').animate({
            left: '-=' + that.options.width + 'px'
        }, 200);
    },
    _moveLeft: function (e) {
        var that = this;
        that._currentItem--;
        that._showNextNavigator(true);
        if (that._currentItem == 1) {
            that._showPrevNavigator(false);
        }
        var selector = that._getContainer();
        selector.find('ul').animate({
            left: '+=' + that.options.width + 'px'
        }, 200);
    },   

    _getContainer: function () {
        return $('.' + this.options.selectors.container);
    },

    _currentItem: null,

});