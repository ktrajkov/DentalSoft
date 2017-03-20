/// <reference path="kendo/kendo.all.min.js" />
function PlanView(viewConfig) {
    this._viewConfig = viewConfig;
}
PlanView.prototype = {
    start: function () {
        var that = this;
        this._loadSchedulerData();
        return this;
    },

    /* Scheduler initialization */
    _initScheduler: function (data) {
        var that = this;
        this._scheduler = $("#" + this._viewConfig.selectors.scheduler).kendoScheduler({
            date: new Date(),
            dateHeaderTemplate: kendo.template("<strong>#=kendo.toString(date, 'dd/MM')#</strong>"),
            currentTimeMarker: {
                updateInterval: 200
            },
            allDaySlot: false,
            workWeekStart: 1,
            workWeekEnd: 5,
            showWorkHours: true,
            workDayStart: new Date(this._viewConfig.settings.workDayStart),
            workDayEnd: new Date(this._viewConfig.settings.workDayEnd),
            height: this._viewConfig.settings.schedulerHeight,
            eventTemplate: $("#" + this._viewConfig.selectors.itemTemplate).html(),
            selectable: true,
            editable: {
                template: $("#" + this._viewConfig.selectors.editTemplate).html()
            },
            dataBinding: function (e) {
                that._onDataBinding(e);
            },
            dataBound: function (e) {
                that._onDataBound(e);
            },
            edit: function (e) {
                that._onEdit(e);
            },
            change: function (e) {
                that._onChange(e);
            },
            views: [
                {
                    type: "day",
                    title: this._viewConfig.texts.day,
                    selected: true,
                    selectedDateFormat: "{0:dd/MM/yyyy}"
                },
                {
                    type: "workWeek",
                    title: this._viewConfig.texts.workWeek,
                    selectedDateFormat: "{0:dd/MM/yyyy} - {1:dd/MM/yyyy}"
                },
                 {
                     type: "week",
                     title: this._viewConfig.texts.week,
                     selectedDateFormat: "{0:dd/MM/yyyy} - {1:dd/MM/yyyy}"
                 },
                 {
                     type: "month",
                     title: this._viewConfig.texts.month,

                 },
                 {
                     type: "agenda",
                     title: this._viewConfig.texts.agenda,
                     selectedDateFormat: "{0:dd/MM/yyyy} - {1:dd/MM/yyyy}"
                 },
                {
                    type: "timeline",
                    title: this._viewConfig.texts.timeline,
                    eventHeight: 50,
                    selectedDateFormat: "{0:dd/MM/yyyy}"
                }
            ],
            dataSource: {
                serverFiltering: true,
                data: data,
                transport: {
                    read: {
                        url: this._viewConfig.urls.planRead,
                        type: 'POST',
                        dataType: 'json',
                    },
                    update: {
                        url: this._viewConfig.urls.planUpdate,
                        type: 'POST',
                    },
                    create: {
                        url: this._viewConfig.urls.planUpdate,
                        type: 'POST',
                    },
                    destroy: {
                        url: this._viewConfig.urls.planDelete,
                        type: 'POST',
                        dataType: 'json'
                    },
                    parameterMap: function (options, operation) {
                        if (operation !== "read" && options) {
                            options.Start = options.Start.toISOString();
                            options.End = options.End.toISOString();
                            return options;
                        } else {
                            return that._getAdditionalData();
                        }
                    },
                },
                schema: {
                    data: 'Data',
                    model: {
                        id: "taskId",
                        fields: {
                            taskId: { from: "Id", type: "number" },
                            title: {
                                from: "Title", validation: {
                                    required: { message: "First name is required" }
                                }
                            },
                            phone: { from: "Phone" },
                            start: { type: "date", from: "Start" },
                            end: { type: "date", from: "End" },
                            description: { from: "Description" },
                            isAllDay: { type: "boolean", from: "IsAllDay" }
                        }
                    },
                },
            },

        }).data("kendoScheduler");
    },
    _loadSchedulerData: function () {
        var that = this;
        $.ajax({
            url: this._viewConfig.urls.planRead,
            type: 'POST',
            contentType: 'application/json',
            data: kendo.stringify(that._getAdditionalData()),
            success: function (response) {
                that._onLoadSuccess(response);
            }
        });
    },
    _onLoadSuccess: function (e) {
        this._initScheduler(e);
        this._initFilterModel();
        this._initControls();

    },
    _onChange: function (e) {
        if (e.events.length === 1) {
            this._keyboardCopyPlanningItem.toDateStart = null;
            if (this._keyboardCopyPlanningItem.isKeyPressed == false) {
                this._keyboardCopyPlanningItem.planningItem = e.events[0];

            }
            this._keyboardCopyPlanningItem.newPlanningItem = e.events[0];

        } else {
            this._keyboardCopyPlanningItem.toDateStart = e.start;
            this._keyboardCopyPlanningItem.toDateEnd = e.end;
        }
    },
    _dataSourceRequest: function (options, url, data) {
        var that = this;
        $.ajax({
            url: url,
            data: kendo.stringify(options.data),
            type: 'POST',
            contentType: 'application/json',
            success: function (response) {
                that._loadSchedulerData();
            }
        });
    },
    _onDataBound: function (e) {
        this._initItems();
        this._initBreakSlot();
    },
    _onDataBinding: function (e) {
        var viewTitle = this._scheduler.view().title;
        if (e.items) {
            $.each(e.items, function (index, value) {
                value.mode = viewTitle;
            })
        }
    },
    _onEdit: function (e) {
        var that = this;
        this._initEditModel(e);
        this._initDetailView();
    },

    _getAdditionalData: function () {
        var result;
        if (this._scheduler) {
            var view = this._scheduler.view();
            var startDate = new Date(view.startDate());
            var endDate = new Date(view.endDate());

            endDate.setHours(23, 59, 59, 999);
            result = {
                Start: kendo.toString(startDate, 'G'),
                End: kendo.toString(endDate, 'G'),
                PatientId: this._filterModel.PatientId,
                Status: this._filterModel.Status
            }
            if (this._filterModel.DentistIds.length === 1) {
                result.DentistIds = this._prepareArray(this._filterModel.DentistIds);
            }
        } else {
            result = {
                Start: kendo.toString(new Date().setStartOfTheDay(), 'G'),
                End: kendo.toString(new Date().setEndOfTheDay(), 'G'),
            }
        }
        return result;
    },
    _prepareArray: function (array) {
        var result = [];
        if (array && array.length) {
            $.each(array, function (index, value) {
                result.push(value);
            });
        }
        return result;
    },

    _initItems: function () {
        var events = this._scheduler.dataSource.view();
        var eventElement;
        var event;

        for (var idx = 0, length = events.length; idx < length; idx++) {
            event = events[idx];
            //get event element
            eventElement = this._scheduler.view().element.find("[data-uid=" + event.uid + "]");
            this._setVisualizationSettings(eventElement, event);

        }
    },
    _initBreakSlot: function () {
        var that = this;
        this._scheduler.view().table.find("td[role=gridcell]").each(function () {
            var element = $(this);
            var slot = that._scheduler.slotByElement(element);
            if (slot != null) {
                if (slot.startDate.timeNow() == that._viewConfig.settings.startBreak &&
                    slot.endDate.timeNow() == that._viewConfig.settings.endBreak) {
                    element.removeClass();
                    element.addClass("k-nonwork-hour");
                }
            }
        })
    },
    _setVisualizationSettings: function (element, eventModel) {
        if (eventModel.DentistId) {
            element.css("background-color", this._viewConfig.colors.byDentists[eventModel.DentistId].Background);
            element.css("color", this._viewConfig.colors.byDentists[eventModel.DentistId].Text);
        } else {
            element.css("background-color", this._viewConfig.colors.byDefault.Background);
            element.css("color", this._viewConfig.colors.byDefault.Text);
        }
        if (eventModel.StatusName) {
            element.remove(".status");
            var html = ('<span class="status glyphicon ');
            switch (eventModel.StatusName) {
                case 'Booked': {
                    html += 'glyphicon-pencil';
                    break;
                }
                case 'Done': {
                    html += 'glyphicon-ok';
                    break;
                }
                case 'Informed': {
                    html += 'glyphicon-share-alt';
                    break;
                }
                case 'Uninformed': {
                    html += 'glyphicon-remove';
                    break;
                }
                case 'Unbooked': {
                    element.css("background-color", this._viewConfig.colors.unbooked.Background);
                    element.css("color", this._viewConfig.colors.unbooked.Text);
                    html += 'glyphicon-phone-alt';
                    break;
                }
                case 'Reminder': {
                    element.css("background-color", this._viewConfig.colors.reminder.Background);
                    element.css("color", this._viewConfig.colors.reminder.Text);
                    html += 'glyphicon-phone-alt';
                    break;
                }
                case 'Completed': {
                    element.css("background-color", this._viewConfig.colors.completed.Background);
                    element.css("color", this._viewConfig.colors.completed.Text);
                    html += 'glyphicon-ok';
                    break;
                }

            }
            html += ('"></span>');
        }
        element.find('.plan-template').prepend(html);
    },

    /*Init Controls*/
    _initControls: function (e) {
        this._initFilterContainer();
        this._getDentist(this._initFilterByDentist);
        this._initFilterByPatientName();
        this._initFilterByStatus();
        this._initKeyboardFunctionality();

    },

    /*Init Filter*/
    _initFilterContainer: function () {
        var header = $('#' + this._viewConfig.selectors.filterScheduler);
        header.append('<div id="filter-container"></div>')
    },
    _getFilterContainer: function () {
        return $("#filter-container");
    },
    _initFilterModel: function () {
        var that = this;
        this._filterModel = kendo.observable({
            DentistIds: [],
            PatientId: '',
            Status: '',
        })
        this._filterModel.bind('change', function (e) {
            that._onFilterChanged(e);
        });
    },
    _onFilterChanged: function (e) {
        if (e.items || e.field === "PatientId" || e.field === "Status") {
            this._scheduler.dataSource.read();
        }
    },

    /*Init filter by Dentists*/
    _getDentist: function (successHandler) {
        var that = this;
        $.ajax({
            url: this._viewConfig.urls.dentistList,
            type: 'POST',
            contentType: 'application/json',
            success: function (e) {
                if (successHandler && typeof (successHandler) === 'function') {
                    successHandler.call(that, e);
                }
            }
        });
    },
    _initFilterByDentist: function (e) {
        var html = '<div id="dentists-container">';
        for (var i = 0; i < e.length; i++) {
            html += ' <label><input  type="checkbox" id="' + e[i].Value + '" value=' + e[i].Value + ' data-bind="checked: DentistIds">' +
                '<span class="k-button">' + e[i].Text + '</span></label>'
        }
        html += '</div>';
        this._getFilterContainer().append(html);
        this._filterModel.set("DentistIds", this._getIds(e));
        kendo.bind(this._getFilterContainer(), this._filterModel);
    },
    _getIds: function (items) {
        var ids = [];
        $.each(items, function (index, item) {
            ids.push(item.Value.toString());
        });
        return ids;
    },

    /*Init filter by Patient Name*/
    _initFilterByPatientName: function () {
        var html = '<span class="patient-container"><input id="search-patient" placeholder="' +
                            this._viewConfig.texts.searchPatientPlaceHolder + '"/></span>';
        this._getFilterContainer().append(html);
        this._initComboBox('search-patient', this._viewConfig.urls.patientList, this._onSearchPatientChanged);
    },
    _onSearchPatientChanged: function (e) {
        this._filterModel.set("PatientId", e.sender.value());
    },

    /*Init filter by Status*/
    _initFilterByStatus: function () {
        var that = this;
        var template = kendo.template($("#" + this._viewConfig.selectors.filterStatusTemplate).html());
        this._getFilterContainer().append(template);
        setTimeout(function () {
            var temp = that
            var statusDropDownList = that._getFilterContainer()
                    .find("#" + that._viewConfig.selectors.filterStatus).data("kendoDropDownList");
            statusDropDownList.bind("change", function (e) {
                temp._onSearchStatusChange(e);
            });
        }, 200);
    },
    _onSearchStatusChange: function (e) {
        this._filterModel.set("Status", e.sender.value());
    },

    /*Init Planning item  detail*/
    _initEditModel: function (e) {
        var uid = e.container.attr('data-uid');
        this._editModel = this._scheduler.dataSource.getByUid(uid);
    },
    _initDetailView: function () {
        this._initDentistDropDownList();
        this._initComboBox(this._viewConfig.selectors.patient, this._viewConfig.urls.patientList, this._onPatientComboBoxChanged);
        this._initStatusDropDownList();
        this._initNewPatientButton();
        this._initPatientButton();
    },
    _initPatientButton: function () {
        var that = this;
        $("#" + this._viewConfig.selectors.patientLink).bind("click", function (e) {
            if (that._editModel.PatientId) {
                window.location.href = that._viewConfig.urls.patientEdit + '?id=' + that._editModel.PatientId;
            }

        });
    },
    _initNewPatientButton: function () {
        var that = this;
        $("#" + this._viewConfig.selectors.newPatientButton).click(function (e) {
            that._onNewPatientClick(e);
        })
    },
    _initDentistDropDownList: function () {
        var that = this;
        $("#" + this._viewConfig.selectors.dentist).kendoDropDownList({
            optionLabel: this._viewConfig.texts.select,
            valuePrimitive: true,
            dataTextField: "Text",
            dataValueField: "Value",
            dataSource: {
                type: "json",
                transport: {
                    read: {
                        url: this._viewConfig.urls.dentistList,
                        type: "POST",
                        dataType: 'json',
                    },
                },
            },
        });
    },
    _initStatusDropDownList: function () {
        if (this._editModel.isNew()) {
            var statusDropDownList = $("#" + this._viewConfig.selectors.status).data("kendoDropDownList");

            if (this._editModel.start.getHours() < new Date(this._viewConfig.settings.workDayStartWithPatiens).getHours()) {
                this._editModel.set("Status", 5);
            } else {
                this._editModel.set("Status", 0);
            }
        }
    },
    _onPatientComboBoxChanged: function (e) {
        var that = this;
        $.ajax({
            url: this._viewConfig.urls.patientPlan,
            type: 'POST',
            dataType: 'json',
            data: { id: e.sender.value() },
            success: function (e) {

                that._editModel.set("DentistId", e.DentistId);
                that._editModel.set("phone", e.Phone);
            }
        });
    },
    _onNewPatientClick: function (e) {
        var newPatienWindow = this._getNewPatientWindow();
        if (newPatienWindow) {
            newPatienWindow.open();
        } else {
            this._initNewPatientForm(e);
            this._initNewPatientWindow(e);
        }
    },

    /*Init form for new patient*/
    _initNewPatientForm: function (e) {
        var that = this;
        var form = $("#" + this._viewConfig.selectors.newPatientForm);
        this._validator = form.kendoValidator().data("kendoValidator");

        form.find('#' + this._viewConfig.selectors.newPatientSaveButton).click(function (e) {
            that._onNewPatientSaveButtonClick(e);
        });

        var dentistDropDownList = form.find("#" + this._viewConfig.selectors.newPatientDentist).data("kendoDropDownList");
        dentistDropDownList.select(0);
    },
    _getNewPatientForm: function () {
        return $("#" + this._viewConfig.selectors.newPatientForm);
    },
    _getNewPatienModel: function () {
        return $("#" + this._viewConfig.selectors.newPatientForm + " input").first().get(0).kendoBindingTarget.source;
    },
    _onNewPatientSaveButtonClick: function (e) {
        e.preventDefault();
        if (this._validator.validate()) {
            this._createNewPatient();


        }
    },
    _createNewPatient: function () {
        var that = this;
        $.ajax({
            url: this._viewConfig.urls.patientCreate,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ PersonalDataModel: this._getNewPatientForm().serializeObject() }),

            success: function (response) {
                success_handler(response);
                that._onCreateNewPatientSuccess(response);
            },
            error: function (e) {
                error_handler(e);
            },
        });
    },
    _onCreateNewPatientSuccess: function (response) {
        this._getNewPatientWindow().close();
        $("#" + this._viewConfig.selectors.patient).data("kendoComboBox").dataSource.read();
        this._editModel.set("PatientId", response.Data[0].Id);
        this._editModel.set("DentistId", response.Data[0].PersonalDataModel.DentistId);
    },

    /*Init Window for new patient*/
    _initNewPatientWindow: function (e) {
        var that = this;
        var planningItemWindow = $('.k-window');
        var position = {
            top: planningItemWindow.position().top,
            left: planningItemWindow.position().left + planningItemWindow.width() + 1,
        }
        var newPatientWindow = $("#" + this._viewConfig.selectors.newPatientWindow);
        newPatientWindow.kendoWindow({
            title: this._viewConfig.texts.newPatient,
            position: position,
            actions: [
                "Close"
            ],
            modal: true,
            visible: true,
            close: function (e) {
                that._onNewPatientWindowClose(e);
            }

        });
        // newPatientWindow.data("kendoWindow").center().open();
    },
    _getNewPatientWindow: function () {
        return $("#" + this._viewConfig.selectors.newPatientWindow).data("kendoWindow");
    },
    _newPatientWindowDestroy: function () {
        this._getNewPatientWindow().destroy();
    },
    _onNewPatientWindowClose: function (e) {
        this._getNewPatientForm()[0].reset();
    },

    _initComboBox: function (selector, dataUrl, changeHandler) {
        var that = this;
        $("#" + selector).kendoComboBox({
            valuePrimitive: true,
            dataTextField: "Text",
            dataValueField: "Value",
            filter: "contains",
            suggest: true,
            dataSource: {
                type: "json",
                transport: {
                    read: {
                        url: dataUrl,
                        type: "POST",
                        dataType: 'json',
                    },

                },
            },
            change: function (e) {
                if (changeHandler && typeof (changeHandler) === 'function') {
                    changeHandler.call(that, e);
                }
            }
        });
    },

    /*Keyboard functionality*/
    _initKeyboardFunctionality: function () {
        var that = this;
        this._scheduler.element.on('keydown', function (event) {
            if (event.ctrlKey || event.metaKey) {
                switch (event.which) {
                    case 67: {
                        that._onCopyKeyDown();
                        break;
                    }
                    case 88: {
                        that._onCutKeyDown();
                        break;
                    }
                    case 86: {
                        that._onPasteKeyDown();
                        break;
                    }
                    default: break;
                }
            }
        });
    },
    _onCopyKeyDown: function (e) {
        this._keyboardCopyPlanningItem.mode = 'copy';
        this._keyboardCopyPlanningItem.isKeyPressed = true;

        $(this._scheduler.element).find('div [role=gridcell][data-uid=' + this._keyboardCopyPlanningItem.planningItem.uid + ']').removeClass('tile-cut tile-copy');
        $(this._scheduler.element).find('div [role=gridcell][data-uid=' + this._keyboardCopyPlanningItem.newPlanningItem.uid + ']').addClass('tile-copy');
        this._keyboardCopyPlanningItem.planningItem = this._keyboardCopyPlanningItem.newPlanningItem;
    },
    _onCutKeyDown: function (e) {
        this._keyboardCopyPlanningItem.mode = 'cut';
        this._keyboardCopyPlanningItem.isKeyPressed = true;

        $(this._scheduler.element).find('div [role=gridcell][data-uid=' + this._keyboardCopyPlanningItem.planningItem.uid + ']').removeClass('tile-cut tile-copy');
        $(this._scheduler.element).find('div [role=gridcell][data-uid=' + this._keyboardCopyPlanningItem.newPlanningItem.uid + ']').addClass('tile-cut');
        this._keyboardCopyPlanningItem.planningItem = this._keyboardCopyPlanningItem.newPlanningItem;
    },
    _onPasteKeyDown: function (e) {
        if (this._keyboardCopyPlanningItem.planningItem && this._keyboardCopyPlanningItem.toDateStart) {
            if (this._keyboardCopyPlanningItem.mode == 'cut') {
                this._cutFunctionality();

            } else if (this._keyboardCopyPlanningItem.mode == 'copy') {
                this._copyFunctionality();

            }
        }

    },

    _cutFunctionality: function () {
        this._recalculateDateTime(this._keyboardCopyPlanningItem.planningItem,
                      this._keyboardCopyPlanningItem.toDateStart,
                      this._keyboardCopyPlanningItem.toDateEnd);
        /* Check for same view */
        if (this._scheduler.dataSource.get(this._keyboardCopyPlanningItem.planningItem.id)) {
            var planningItem = this._scheduler.dataSource.get(this._keyboardCopyPlanningItem.planningItem.id);
            planningItem.set('start', this._keyboardCopyPlanningItem.toDateStart);
            planningItem.set('end', this._keyboardCopyPlanningItem.toDateEnd);
            this._scheduler.dataSource.sync();
        } else {
            this._copyFunctionality();
            this._deletePlanningItem(this._keyboardCopyPlanningItem.planningItem.id);
        }


        /* Reset clipboard after cut */
        this._keyboardCopyPlanningItem.planningItem = null;
        this._keyboardCopyPlanningItem.isKeyPressed = false;
    },
    _copyFunctionality: function () {
        this._CopyPlanningItem(this._keyboardCopyPlanningItem.planningItem, this._keyboardCopyPlanningItem.toDateStart)
    },

    _deletePlanningItem: function (planningItemId) {
        $.ajax({
            url: this._viewConfig.urls.planDelete,
            type: 'POST',
            dataType: 'json',
            data: {
                Id: planningItemId,
            },
        });
    },
    _CopyPlanningItem: function (item, startDate) {
        var endDate = new Date();
        this._recalculateDateTime(item, startDate, endDate);
        var newItem = {
            DentistId: item.DentistId,
            PatientId: item.PatientId,
            Status: item.Status,
            description: item.description,
            title: item.title,
            start: startDate,
            end: endDate,
        };
        this._scheduler.dataSource.pushCreate(newItem);
        this._scheduler.dataSource.sync();
        this._scheduler.refresh();
    },
    _onCopySuccess: function (e) {
        this._fixModel(this._scheduler.dataSource.options.schema.model.fields, e)
        e.start = e.Start;
        e.end = e.End;
        e.title = e.Title;
        this._scheduler.dataSource.pushCreate(e);
        this._scheduler.dataSource.sync();
        this._scheduler.refresh();
    },

    _fixModel: function (schemaModel, modelInstance) {
        var schemaField;
        for (var propertyName in modelInstance) {
            schemaField = schemaModel[propertyName];
            if (!schemaField) {
                /* If not found => check if field is used with "from" option */
                $.each(schemaModel, function (index, property) {
                    if (property.from === propertyName) {
                        schemaField = property;
                        return false;
                    }
                });
            }
            if (schemaField) {
                modelInstance[propertyName] = schemaField.parse(modelInstance[propertyName]);
            }
        }
    },
    _recalculateDateTime: function (planningItem, newStart, newEnd) {
        var diffTime = planningItem.end.getTime() - planningItem.start.getTime();
        if (this._scheduler.view().title === this._viewConfig.texts.viewMount) {
            newStart.setTimeByDate(planningItem.start);
        }
        newEnd.setTime(newStart.getTime() + diffTime);
    },

    /* Data members and global variables */
    _viewConfig: null,
    _editModel: null,
    _scheduler: null,
    _schedulerMode: null,
    _filterModel: null,
    _data: null,
    _newPatientValidator: null,
    _keyboardCopyPlanningItem: {
        planningItem: null,
        newPlanningItem: null,
        toDateStart: null,
        toDateEnd: null,
        isKeyPressed: false,
        mode: null,
    }
}