/* DataSource error */
function DataSourceError(message) {
    this.message = (message || "");
}
DataSourceError.prototype = new Error();
DataSourceError.errorName = "DataSourceError";
DataSourceError.prototype.name = DataSourceError.errorName;


/* Handles Kendo Grid DataSource errors */
function error_handler(e) {
    if (typeof e.preventDefault === "function") {
        e.preventDefault();
    }

    var error;

    /* Check for invalid ModelState */
    if (e.errors) {
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                value.errors.forEach(function (error) {
                    throw new DataSourceError(error);
                });
            }
        });
    }
    else {
        try {
            var responseObject = e.xhr ? $.parseJSON(e.xhr.responseText) : $.parseJSON(e.responseText)
            /* Check if it is an error upon deletion */
            if (responseObject.Action && $.inArray(responseObject.Action.toLowerCase(), dentalSoft.actionNames !== -1)) {
                // e.sender.cancelChanges();
            }
            error = responseObject.Message;
        }
        catch (err) {
            error = e.xhr.responseText;
        }
        throw new DataSourceError(error);
    }
}


function onKendoGridRequestEnd(e) {
    if (e.type !== "read") {
        if (e.response && e.response.Errors == null) {
            dentalSoft.showNotification("Успешен запис", "success");
        }
    }
}



/* Handles Kendo Grid DataSource successes */
function success_handler(e) {
    if (e.Errors) {
        var errs = e.Errors[Object.keys(e.Errors)[0]].errors;
        if (errs.length > 0) {
            throw new DataSourceError(errs[0]);
        }
    } else {
        dentalSoft.showNotification("Успешен запис", "success");
    }

}

var dentalSoft = {

    getMessage: function (trace, msg) {
        if (!msg && trace && trace.message) {
            msg = trace.message;
        }
        return msg;
    },
    logError: function (msg, url, trace) {
        if (!msg && !trace) {
            return;
        }

        if (!this.logErrorUrl) {
            return;
        }

        var url = url || document.location;

        // format output
        var out = "\n message: " + msg;
        out += "\n stack: " + (trace && trace.stack ? trace.stack : "N/A");
        out += "\n document path: '" + url + "'.";

        // send error message
        $.ajax({
            type: 'POST',
            url: this.logErrorUrl,
            data: { message: encodeURIComponent(out) }
        });
    },

    showDialog: function (message) {
        alert(message);
    },
    showNotification: function (message, type) {
        $("#" + this.notificationId).data("kendoNotification").show(message, type);
    },
    showError: function (message) {
        console.log(message);
    },
    initNotificationDialog: function () {
        var popupNotification = $("#" + this.notificationId).kendoNotification(
                            {
                                show: function onShow(e) {
                                    if (!$("." + e.sender._guid)[1]) {
                                        var element = e.element.parent(),
                                            eWidth = element.width(),
                                            eHeight = element.height(),
                                            wWidth = $(window).width(),
                                            wHeight = $(window).height(),
                                            newTop, newLeft;

                                        newLeft = Math.floor(wWidth / 2 - eWidth / 2);
                                        newTop = Math.floor(wHeight / 2 - eHeight / 2);

                                        e.element.parent().css({ top: newTop, left: newLeft });
                                    }
                                },
                                autoHideAfter: 1000,
                                width: 500,
                                hideOnClick: true,
                                animation: {
                                    open: {
                                        effects: "slideIn:left"
                                    },
                                    close: {
                                        effects: "slideIn:left",
                                        reverse: true
                                    }
                                },
                                position: {
                                    pinned: true,
                                    top: 20,
                                    left: null,
                                    bottom: null,
                                    right: 20
                                }
                            }).data("kendoNotification");
    },

    notificationId: 'popupNotification',
    actionNames: ["delete"],
    logErrorUrl: "/Error/LogJavaScriptError",

    startUp: function () {
        var that = this;

        /* Attach global handler for runtime errors.  */
        window.onerror = function (msg, url, lineNumber, column, trace) {
            msg = that.getMessage(trace, msg);
            if (msg.indexOf(DataSourceError.errorName) == -1) {
                that.logError(msg, url, trace);
                that.showError(msg);
            }
            else {
                var array = msg.split(':');
                if (array.length == 2) {
                    msg = msg.split(':')[1];
                }
                that.showNotification(msg, 'error');
            }
        };

        /* Attach handler for ajax errors.  */
        $.ajaxSetup({
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (XMLHttpRequest && XMLHttpRequest.responseJSON && XMLHttpRequest.responseJSON.Message) {
                    that.showDialog(XMLHttpRequest.responseJSON.Message);
                }
                else {
                    var message = "N/A";
                    if (errorThrown && errorThrown.message) {
                        message = errorThrown.message;
                    }
                    else if (XMLHttpRequest.responseText) {
                        try {
                            var response = JSON.parse(XMLHttpRequest.responseText);
                            if (response && response.Message) {
                                message = response.Message;
                            }
                        }
                        catch (ex) {
                        }
                    }
                    that.showError(message);
                }
            },

        });

        this.initNotificationDialog();
    }
};

$(function () {
    dentalSoft.startUp();
});


if (!Array.prototype.clear) {
    Array.prototype.clear = function () {
        while (this.length > 0) {
            this.pop();
        }
    };
}

if (!Array.prototype.removeItem) {
    Array.prototype.removeItem = function (item) {
        var index = this.indexOf(item);
        if (index >= 0) {
            this.splice(index, 1);
        }        
    };
}
if (!Array.prototype.unique) {
    Array.prototype.unique = function () {
        var a = this.concat();
        for (var i = 0; i < a.length; ++i) {
            for (var j = i + 1; j < a.length; ++j) {
                if (a[i] === a[j])
                    a.splice(j--, 1);
            }
        }

        return a;
    };
}



Date.prototype.timeNow = function () {
    var h = (this.getHours() < 10 ? '0' : '') + this.getHours();
    var m = (this.getMinutes() < 10 ? '0' : '') + this.getMinutes();
    return h + ':' + m;
}

if (!Date.prototype.setTimeByDate) {
    Date.prototype.setTimeByDate = function (date) {
        this.setHours(date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds());
    };
}

Date.prototype.setStartOfTheDay = function () {
    this.setHours(00, 00, 00, 000);
    return this;
};

Date.prototype.setEndOfTheDay = function () {
    this.setHours(23, 59, 59, 999);
    return this;
};


Date.prototype.setTimeByDate = function (date) {
    this.setHours(date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds());
};


$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
