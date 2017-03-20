$.widget("DentalSoft.teethChartView", {
    options: {
        data: null,
        teethStatusChartId: null,
        handleClick: null,
    },

    /*Public API*/
    setCharts: function (data) {
        this._clear();
        this._setDefaultCharts();
        this._setCharts(data);
        this._checkAndShowDeciduousTeeth();
    },
    setImageIndicators: function (data) {
        this._setIndicatorForImages(data);
    },
    getSelectedToothNumbers: function () {
        return this._selectedToothNumbers;
    },
    setSelectedTeeth: function (toothNumbers) {
        this._removeActiveCells();
        this._setActiveCells(toothNumbers);
    },
    hasImage: function (toothNumber) {
        return this._getElement(toothNumber).find(this._selectors.chartContainer).hasClass("image-indicator");
    },
    clearSelectedTeeth: function () {
        this._removeActiveCells();
    },

    /* General functionality*/

    _create: function () {
        this.element.data("teethChartView", this);
        this._createTableWithPermanentTeeth();
        this._createRowsWithDeciduousTeeth();
        this._visibleDeciduousTeeth(false);
        this._creteCollapseNav();
        this._creteSelectRowButtons();
    },
    _clear: function () {
        $(this.element).find('td span').remove();
    },
    _createTableWithPermanentTeeth: function () {
        var that = this;
        var numbersOfTeeth = this._getNumbersOfTeeth();
        var html = '<table></table>';
        $(html).appendTo(this.element);
        this._createRowOfTable(numbersOfTeeth.PermanentTeeth.Top, "top");
        this._createRowOfTable(numbersOfTeeth.PermanentTeeth.Bottom, "bottom");
        $(this.element).find('table')
         .unbind('click', function (e) {
             that._handleClick(e)
         }).bind('click', function (e) {
             that._handleClick(e)
         });

    },
    _createRowsWithDeciduousTeeth: function () {
        var numbersOfTeeth = this._getNumbersOfTeeth();
        this._createRowOfTable(numbersOfTeeth.DeciduousTeeth.Top, "top");
        this._createRowOfTable(numbersOfTeeth.DeciduousTeeth.Bottom, "bottom");
    },
    _createRowOfTable: function (numbers, row) {
        var html = '<tr  class="' + row;
        if (numbers.length == 10) {
            html += ' deciduous-teeth"><td class="invisible"></td>' +
                '<td class="invisible"></td>';
        } else {
            html += ' permanent-teeth">';
        }
        for (var i = 0; i < numbers.length; i++) {
            html += '<td id="' + numbers[i] + '">' + numbers[i] + '</td>';

        }
        html += '</tr>';
        if (numbers.length == 10 && row === "top") {
            $(this.element).find('table').prepend(html);
        } else {
            $(this.element).find('table').append(html);
        }
    },

    _getDefaultChart: function (toothNumber) {
        var toothElement = this._getChartIntoHtml('root root-' + this._toothMap[toothNumber]);
        toothElement += this._getChartIntoHtml('crown crown-' + this._toothMap[toothNumber]);
        toothElement += this._getChartIntoHtml('crown crown-pos');
        return toothElement;
    },
    _setDefaultCharts: function () {
        var that = this;
        $(this.element).find('td').each(function (index, element) {
            var toothNumber = $(element).attr('id');
            $(element).append(that._getDefaultChart(toothNumber));
        })
    },
    _setCharts: function (data) {
        if (data != null && data.length > 0) {
            for (var i = data.length - 1; i >= 0; i--) {
                var operationData = {
                    Teeth: data[i].Teeth,
                    PositionText: data[i].PositionText.toLowerCase(),
                }
                if (typeof data[i].TreatmentToothChart != 'undefined' && data[i].TreatmentToothChart !== null) {
                    operationData = $.extend(operationData, data[i].TreatmentToothChart);
                }
                else if (typeof data[i].DiagnosisToothChart != 'undefined' && data[i].DiagnosisToothChart !== null) {
                    operationData = $.extend(operationData, data[i].DiagnosisToothChart);
                }
                this._setOperationCharts(operationData);
            }
        }
    },
    _setOperationCharts: function (data) {
        var teethLength = data.Teeth.length

        switch (teethLength) {
            case 0: {
                data.ToothNumber = "";
                this._setToothChart(data);
                break;
            }
            case 1: {
                data.ToothNumber = data.Teeth[0].Number;
                this._setToothChart(data);
                break;
            }
                //selected teeth
            default:
                for (var i = 0; i < teethLength; i++) {
                    if (data.Type === "crown") {
                        switch (i) {
                            case 0:
                                data.PartBridge = "leftPart center";
                                break;
                            case teethLength - 1:
                                data.PartBridge = "rightPart center";
                                break;
                            default:
                                data.PartBridge = "center";
                                break;
                        }
                    }
                    data.ToothNumber = data.Teeth[i].Number;
                    this._setToothChart(data)
                }
                break;
        }
    },
    _setToothChart: function (data) {
        var chart = '';
        if (data.ToothNumber && data.ToothNumber !== '') {
            var positionQuadrant = data.ToothNumber.toString()[0];
            data.quadrant = positionQuadrant > 4 ? (positionQuadrant - 4).toString() : positionQuadrant;
            data.sequence = data.ToothNumber.toString()[1];
            data.row = this._getRowByQuadrant(data.quadrant);
            data.element = this._getElement(data.ToothNumber);
            switch (data.Type) {
                case "partofcrown":
                    if (data.PositionText !== "") {
                        var reversePosition = this._reversePosition(data.PositionText, data.quadrant);
                        chart += 'partofcrown-' + reversePosition + '-' + data.Color;
                        if (!this._checkForAvailableChartBySelector(data.element, '.' + chart) ||
                            data.element.find('span').last().hasClass('partofcrown_grid-' + reversePosition + '-black')) {

                            this._removeChartsBySelector(data.element, 'span[class*="partofcrown-' + reversePosition + '-"]');
                            this._removeChartsBySelector(data.element, 'span[class*="partofcrown_grid-' + reversePosition + '-"]');
                            chart += ' crown';
                        }
                        else {
                            chart = "";
                        }
                    }
                    break;
                case "partofcrown_grid":
                    if (data.PositionText !== "") {
                        var reversePosition = this._reversePosition(data.PositionText, data.quadrant);
                        chart += data.Type + '-' + reversePosition + '-' + data.Color;
                        if (!this._checkForAvailableChartBySelector(data.element, '.' + chart) ||
                            data.element.find('span').last().hasClass('partofcrown-' + reversePosition + '-red')) {
                            chart += ' crown';
                        }
                        else {
                            chart = "";
                        }
                    }
                    break;
                case "partofcrownwithgrid":
                    if (data.PositionText !== "") {
                        var reversePosition = this._reversePosition(data.PositionText, data.quadrant);
                        chart = 'crown ' + 'partofcrown-' + reversePosition + '-' + data.Color;
                        this._appendChart(data.element, chart);
                        chart = 'crown partofcrown_grid-' + reversePosition + '-black';
                    }
                    break;
                case "crown":
                    this._removeChartsBySelector(data.element, this._selectors.missingTooth);
                    this._removeChartsBySelector(data.element, this._selectors.crown);
                    chart = 'crown ' + "crown-" + this._toothMap[data.ToothNumber] + "-";
                    if (!this._checkForAvailableChartBySelector(data.element, this._selectors.natRoot)) {
                        chart += 'cut-' + data.Color;
                        var implant = data.element.find(this._selectors.implant).first();
                        if (implant.length) {
                            implant.before(this._getChartIntoHtml(chart));
                        } else {
                            this._appendChart(data.element, chart);
                        }
                    } else {
                        chart += data.Color;
                        var radixanchor = data.element.find(this._selectors.radixanchor).first();
                        if (radixanchor.length) {
                            radixanchor.before(this._getChartIntoHtml(chart));
                        } else {
                            this._appendChart(data.element, chart)
                        }
                    }

                    if (data.PartBridge) {
                        data.element.append("<span class='bridge-indicator " + data.PartBridge + "'></span>")
                    }
                    chart = '';
                    break;

                case "removecrown":
                    if (data.PositionText === "") {
                        this._removeChartsBySelector(data.element, this._selectors.crown);
                    } else {
                        var reversePosition = this._reversePosition(data.PositionText, data.quadrant);
                        chart += 'crown partofcrown_grid-' + reversePosition + '-black';
                    }
                    break;

                case "missingcrown":
                    if (data.PositionText === "") {
                        this._removeChartsBySelector(data.element, this._selectors.crown);
                    }
                    break;
                case "crown_key":
                    $(data.element).append('<span class="partial-prosthesis crown crown-key ' + data.PositionText + '"></span>')
                    break;
                case "partofroot":
                    this._removeChartsBySelector(data.element, this._selectors.canal);
                    chart = 'root root-' + this._toothMap[data.ToothNumber] + "-canal-part-" + data.Color;
                    data.element.find(this._selectors.root).last().after(this._getChartIntoHtml(chart));
                    chart = '';
                    break;

                case "root":
                    this._removeChartsBySelector(data.element, this._selectors.canal);
                    chart = 'root ' + data.Type + '-' + this._toothMap[data.ToothNumber] + "-canal-" + data.Color;
                    var radixanchor = data.element.find(this._selectors.radixanchor).first();
                    if (radixanchor.length) {
                        radixanchor.before(this._getChartIntoHtml(chart));
                    } else {
                        data.element.find(this._selectors.root).last().after(this._getChartIntoHtml(chart));
                    }
                    chart = '';
                    break;

                case "radixanchor":
                    chart += 'root ' + data.Type + '-' + data.row;
                    var crown = data.element.find(this._selectors.crown).first();
                    if (crown.length) {
                        crown.after(this._getChartIntoHtml(chart));
                    } else {
                        data.element.find(this._selectors.root).last().after(this._getChartIntoHtml(chart));
                    }
                    chart = '';
                    break;

                case "removeradixanchor":
                    this._removeChartsBySelector(data.element, this._selectors.radixanchor);
                    this._removeChartsBySelector(data.element, this._selectors.canal);
                    chart = 'root root-' + this._toothMap[data.ToothNumber] + "-canal-part-black";
                    break;

                case "implant":
                    this._removeChartsBySelector(data.element, this._selectors.missingTooth);
                    this._removeChartsBySelector(data.element, this._selectors.crown);
                    this._removeChartsBySelector(data.element, this._selectors.root);

                    //check for molar
                    var toothSize = data.sequence >= 6 ? "big" : "small";
                    chart += 'root ' + data.Type + '-' + data.row + '-' + toothSize + '-' + data.Color;
                    break;
                case "missingtooth":
                    this._removeChartsBySelector(data.element, this._selectors.chartContainer);
                    chart += 'missingtooth ' + data.Type + '-' + data.row + '-' + data.Color;
                    break;

                case "partialprosthesis":
                    this._removeChartsBySelector(data.element, this._selectors.missingTooth);
                    this._removeChartsBySelector(data.element, this._selectors.crown);
                    chart = 'partial-prosthesis crown crown-' + this._toothMap[data.ToothNumber] + '-';
                    if (!this._checkForAvailableChartBySelector(data.element, this._selectors.natRoot)) {
                        chart += 'cut-';
                    }
                    chart += data.Color;
                    break;

                case "retinirantooth":
                    this._removeChartsBySelector(data.element, this._selectors.chartContainer);
                    chart += 'root ret-' + this._toothMap[data.ToothNumber];
                    break;

                case "outretinirantooth":
                    this._removeChartsBySelector(data.element, this._selectors.chartContainer);
                    $(data.element).append(this._getDefaultChart(this._toothMap[data.ToothNumber]));
                    break;
            }
            if (chart !== "") {
                this._appendChart(data.element, chart);
            }

        } else if (data.PositionText !== "") {
            var position = this._changePositionText(data.PositionText);
            switch (data.Type) {
                case "prosthesis":
                    this._removeChartsInPermanentTeeth(position, this._selectors.crown);
                    this._removeChartsInPermanentTeeth(position, this._selectors.missingTooth);
                    this._setProsthesis(position, data.Color);
                    break;
                case "removepartialprosthesis":
                    this._removeChartsInPermanentTeeth(position, this._selectors.partialProsthesis);
                    break;
                case "removeprosthesis":
                    this._removeChartsInPermanentTeeth(position, this._selectors.protesis);
                    break;
                default: { }
            }
        }
    },
    _setProsthesis: function (row, chartColor) {
        var that = this;
        $.each(this._getPermanentTeethByRow(row), function (index, element) {
            if (!that._checkForAvailableChartBySelector($(element), that._selectors.ret)) {
                var chart = 'protesis crown crown-' + that._toothMap[element.id] + '-';
                if (!that._checkForAvailableChartBySelector($(element), that._selectors.natRoot)) {

                    chart += 'cut-';
                }
                chart += chartColor;
                $(element).append(that._getChartIntoHtml(chart));
            }
        });
    },
    _appendChart: function (toothElement, chart) {
        var chartHtml = this._getChartIntoHtml(chart);
        toothElement.append(chartHtml);
    },

    _checkForAvailableChartBySelector: function (toothElement, selector) {
        return toothElement.find(selector).length > 0;
    },
    _getElement: function (toothNumber) {
        if (toothNumber !== '') {
            return $(this.element).find("td#" + toothNumber);
        }
    },
    _getRowBySelector: function (selector) {
        return $(this.element).find(selector);
    },

    _getChartIntoHtml: function (chart) {
        return '<span class="teeth-sprite ' + chart + '"></span>';
    },
    _getRowByToothNumber: function (toothNumber) {
        return this._getRowByQuadrant(parseInt(toothNumber / 10));
    },
    _getRowByQuadrant: function (quadrant) {
        return quadrant <= 2 || (quadrant >= 5 && quadrant <= 6) ? "top" : "bottom";
    },
    _getNumbersOfTeeth: function () {
        var numbers = {
            DeciduousTeeth: {
                Top: [],
                Bottom: []
            },
            PermanentTeeth: {
                Top: [],
                Bottom: []
            },
        }
        for (var i = 8; i > 0; i--) {
            numbers.PermanentTeeth.Top.push('1' + i);
            numbers.PermanentTeeth.Bottom.push('4' + i);
            if (i <= 5) {
                numbers.DeciduousTeeth.Top.push('5' + i);
                numbers.DeciduousTeeth.Bottom.push('8' + i);
            }
        }
        for (var y = 1; y <= 8; y++) {
            numbers.PermanentTeeth.Top.push('2' + y);
            numbers.PermanentTeeth.Bottom.push('3' + y);
            if (y <= 5) {
                numbers.DeciduousTeeth.Top.push('6' + y);
                numbers.DeciduousTeeth.Bottom.push('7' + y);
            }
        }
        return numbers;
    },

    _reversePosition: function (position, quadrant) {
        var reversePosition = position;
        switch (position) {
            case 'medioocclusalis':
                if (quadrant === '2' || quadrant == '3') {
                    reversePosition = 'distoocclusalis'
                }
                break;
            case 'distoocclusalis':
                if (quadrant === '2' || quadrant === '3') {
                    reversePosition = 'medioocclusalis'
                }
                break;
            case 'vestibularis':
                if (quadrant === '3' || quadrant === '4') {
                    reversePosition = 'oralis'
                }
                break;
            case 'oralis':
                if (quadrant === '3' || quadrant === '4') {
                    reversePosition = 'vestibularis'
                }
                break;
            default: { }
        }
        return reversePosition;

    },
    _changePositionText: function (text) {
        if (text === "superior") {
            return "top";
        } else if (text === "inferior") {
            return "bottom";
        }
    },

    /*Remove functionality*/
    _removeChartsBySelector: function (element, selector) {
        element.find(selector).remove();
    },

    /*Selecting cell functionality*/
    _setActiveCells: function (ids) {
        if (ids.length > 0) {
            for (var i = 0; i < ids.length; i++) {
                var element = this._getElement(ids[i]);;
                element.addClass('active');
                this._selectedToothNumbers.push(ids[i]);
            }
        }
    },
    _setActiveCellsBySelector: function (selector) {
        var ids = [];
        $(this.element).find(selector).each(function (index, element) {
            $(element).addClass('active');
            if (element.id !== "") {
                ids.push(element.id);
            }
        });
        this._selectedToothNumbers = this._selectedToothNumbers.concat(ids).unique();
    },
    _removeActiveCells: function () {
        $(this.element).find('td').removeClass('active');
        this._selectedToothNumbers.clear();
    },
    _removeActiveCellsBySelector: function (selector) {
        var that = this;
        var activeCells = $(this.element).find(selector + '.active').each(function (index, element) {
            $(element).removeClass('active');
            that._selectedToothNumbers.removeItem(element.id);
        });
    },

    /*Indicator for avaliable Images by tooth */
    _setIndicatorForImages: function (toothNumbers) {
        this._removeAllIndicatorsForImages();
        for (var i = 0; i < toothNumbers.length; i++) {
            var element = this._getElement(toothNumbers[i]);
            var position = this._getRowByToothNumber(toothNumbers[i]);
            element.append("<span class='image-indicator image-indicator-" + position + "'>.</span>");
        }

    },
    _removeAllIndicatorsForImages: function () {
        this.element.find(this._selectors.imageIndicator).remove();
    },

    /*Functionality for Deciduous Teeth */
    _creteCollapseNav: function () {
        var that = this;
        this._getRowBySelector(this._selectors.deciduousTeethTop).after('<div class="rel-cont"><span class="deciduous-teeth-nav top ' +
             this._icons.arrowDown + '"></span></div>');
        this._getRowBySelector(this._selectors.deciduousTeethBottom).before('<div class="rel-cont"><span class="deciduous-teeth-nav bottom ' +
            this._icons.arrowUp + '"></span></div>');
        $(this.element).find(this._selectors.deciduousTeethNav).bind('click', function (e) {
            that._handleDeciduousTeethNavClick(e);
        });

    },
    _creteSelectRowButtons: function () {
        var that = this;
        this._getRowBySelector(this._selectors.deciduousTeethTop).prepend('<span class="select-row-button top ' + this._icons.selectRow + '"></span>');
        this._getRowBySelector(this._selectors.deciduousTeethBottom).prepend('<span class="select-row-button bottom ' + this._icons.selectRow + '"></span>');
        $(this.element).find(this._selectors.selectRowButton).bind('click', function (e) {
            that._handleSelectRowButtonClick(e);
        });
    },
    _checkAndShowDeciduousTeeth: function () {
        this._checkAndShowDeciduousTeethByRow('top');
        this._checkAndShowDeciduousTeethByRow('bottom');
    },
    _checkAndShowDeciduousTeethByRow: function (row) {
        var deciduousTeeth = this._selectors.deciduousTeeth + '.' + row
        var missingToothSelector = deciduousTeeth + ' ' + this._selectors.missingTooth;
        if ($(missingToothSelector).length != 10) {
            this._visibleDeciduousTeeth(true, row);
        } else {
            this._visibleDeciduousTeeth(false, row);
            this._removeActiveCellsBySelector(deciduousTeeth + ' td');
            //  this._trigger("handleClick", null, { data: this._selectedToothNumbers });
        }

    },
    _visibleDeciduousTeeth: function (show, row) {
        var rowSelector = row ? '.' + row : '';
        var deciduousTeethSelector = $(this._selectors.deciduousTeeth + rowSelector);
        show ? deciduousTeethSelector.show() : deciduousTeethSelector.hide();
    },

    /*Functionality for Permanent Teeth*/
    _getPermanentTeethByRow: function (row) {
        return $(this.element).find(this._selectors.permanentTeeth + '.' + row + ' td');
    },
    _removeChartsInPermanentTeeth: function (row, selector) {
        var that = this;
        this._getPermanentTeethByRow(row).each(function (index, element) {
            that._removeChartsBySelector($(element), selector);
        });
    },

    /*Events*/
    _handleClick: function (element) {
        var tdElement = $(element.target).is('td') ? $(element.target) : $(element.target.parentElement);
        if ($(tdElement).is('td')) {
            var toothNumber = tdElement.attr('id');
            var indexOfToothNumber = this._selectedToothNumbers.indexOf(toothNumber);

            if (indexOfToothNumber >= 0) {
                tdElement.removeClass('active');
                this._selectedToothNumbers.splice(indexOfToothNumber, 1);

            } else {
                tdElement.addClass('active');
                this._selectedToothNumbers.push(toothNumber);
            }
            this._trigger("handleClick", null, {
                element: element,
                data: this._selectedToothNumbers,
            });
        }

    },
    _handleDeciduousTeethNavClick: function (element) {
        if ($(element.target).hasClass('top')) {
            $(this._selectors.deciduousTeethTop).toggle();
        } else {
            $(this._selectors.deciduousTeethBottom).toggle();
        }

    },
    _handleSelectRowButtonClick: function (element) {
        var selector = $(element.currentTarget).hasClass('top') ? this._selectors.deciduousTeethTop : this._selectors.deciduousTeethBottom;
        selector += ' td';
        this._setActiveCellsBySelector(selector);
        this._trigger("handleClick", null, { data: this._selectedToothNumbers });
    },

    _selectedToothNumbers: [],
    _selectors: {
        chartContainer: "span",
        crownPos: "span.crown-pos",
        root: "span.root",
        canal: "span[class*='-canal-']",
        natRoot: "span[class*='root-']",
        partOfRoot: "span[class*='partofroot-']",
        radixanchor: "span[class*='radixanchor-']",
        implant: "span[class*='implant-']",
        crown: "span.crown",
        ret: "span[class*='ret-']",
        partOfCrown: "span[class*='partofcrown-']",
        partOfCrown_Grid: "span[class*='partofcrown_grid-']",
        missingTooth: "span.missingtooth",
        partialProsthesis: "span.partial-prosthesis",
        protesis: "span.protesis",
        permanentTeeth: "tr.permanent-teeth",
        deciduousTeeth: "tr.deciduous-teeth",
        deciduousTeethTop: "tr.deciduous-teeth.top",
        deciduousTeethBottom: "tr.deciduous-teeth.bottom",
        deciduousTeethNav: "span.deciduous-teeth-nav",
        selectRowButton: '.select-row-button',
        imageIndicator: '.image-indicator',

    },
    _charts: {
        crown: 'crown',
        partOfCrown: 'partofcrown',
        partOfCrown_Grid: 'partofcrown_grid',
        partofCrownWithGrid: 'partofcrownwithgrid',
        missingCrown: 'missingcrown',
        root: 'root',
        partOfRoot: 'portofroot',
        radixanchor: 'radixanchor',
        implant: 'implant',
        crown_Key: 'crown_key',
        partialProsthesis: 'partialprosthesis',
        removeCrown: 'removecrown',
        removeRadixanchor: 'removeradixanchor',
        prosthesis: 'prosthesis',
        removePartialProsthesis: 'removepartialprosthesis',
        removeProsthesis: 'removeprosthesis'
    },
    _colors: {
        black: 'black',
        red: 'red',
    },

    _icons: {
        arrowUp: "glyphicon glyphicon-chevron-up",
        arrowDown: "glyphicon glyphicon-chevron-down",
        selectRow: 'glyphicon glyphicon-chevron-right',
    },


    _toothMap: {
        "18": 18, "17": 17, "16": 16, "15": 15, "14": 14, "13": 13, "12": 12, "11": 11, "21": 21, "22": 22, "23": 23, "24": 24, "25": 25,
        "26": 26, "27": 27, "28": 28, "38": 38, "37": 37, "36": 36, "35": 35, "34": 34, "33": 33, "32": 32,
        "31": 31, "41": 41, "42": 42, "43": 43, "44": 44, "45": 45, "46": 46, "47": 47, "48": 48, "55": 17, "54": 17,
        "53": 13, "52": 12, "51": 11, "61": 21, "62": 22, "63": 23, "64": 27, "65": 27, "75": 37, "74": 37,
        "73": 33, "72": 32, "71": 31, "81": 41, "82": 42, "83": 43, "84": 47, "85": 47,
    }
});
