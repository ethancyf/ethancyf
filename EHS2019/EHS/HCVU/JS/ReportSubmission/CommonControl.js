var objCalendarFrom;
var objCalendarTo;
var objSelectedDate;

var lblFromDateDummy;
var txtFromDate;
var btnFromDateDummy;
var btnFromDate;

var lblToDateDummy;
var txtToDate;
var btnToDateDummy;
var btnToDate;

function pageLoad() {
    //find behaviour ID instead of client ID
    objCalendarFrom = $find("calExFromDate_MY");
    objCalendarTo = $find("calExToDate_MY");

    if (objCalendarFrom != null && objCalendarTo != null) {
        ResetCalendarDelegate(objCalendarFrom);
        ResetCalendarDelegate(objCalendarTo);
    }
}

function ResetCalendarDelegate(calendar) {
    //we need to reset the original delegate of the month cell. 
    calendar._cell$delegates = {
        mouseover: Function.createDelegate(calendar, calendar._cell_onmouseover),
        mouseout: Function.createDelegate(calendar, calendar._cell_onmouseout),

        click: Function.createDelegate(calendar, function (e) {

            /// <summary>  
            /// Handles the click event of a cell 
            /// </summary> 
            /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param> 
            e.stopPropagation();
            e.preventDefault();

            if (!calendar._enabled) return;

            var target = e.target;
            var visibleDate = calendar._getEffectiveVisibleDate();

            Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");

            switch (target.mode) {
                case "prev":

                case "next":
                    calendar._visibleDate = target.date;
                    calendar.set_selectedDate(target.date);
                    break;
                case "title":
                    switch (calendar._mode) {
                        case "days": calendar._switchMode("months"); break;
                        case "months": calendar._switchMode("years"); break;
                    }
                    break;
                case "month":
                    //if the mode is month, then stop switching to day mode. 
                    if (target.month == visibleDate.getMonth()) {
                        //this._switchMode("days"); 
                    } else {
                        calendar._visibleDate = target.date;
                        //this._switchMode("days"); 
                    }
                    calendar.set_selectedDate(target.date);
                    calendar._blur.post(true);
                    calendar.raiseDateSelectionChanged();
                    break;
                case "year":
                    if (target.date.getFullYear() == visibleDate.getFullYear()) {
                        calendar._switchMode("months");
                    } else {
                        calendar._visibleDate = target.date;
                        calendar._switchMode("months");
                    }
                    break;
                    //          case "day":                 
                    //            this.set_selectedDate(target.date);                               
                    //            this._blur.post(true);                 
                    //            this.raiseDateSelectionChanged();                 
                    //            break;                 

                case "today":
                    calendar.set_selectedDate(target.date);
                    calendar._blur.post(true);
                    calendar.raiseDateSelectionChanged();
                    break;
            }
        })
    }
}

function RedefineHandler(calendar) {
    if (calendar._monthsBody) {
        //remove the old handler of each month body. 
        for (var i = 0; i < calendar._monthsBody.rows.length; i++) {
            var row = calendar._monthsBody.rows[i];
            for (var j = 0; j < row.cells.length; j++) {
                $common.removeHandlers(row.cells[j].firstChild, calendar._cell$delegates);
            }
        }

        //add the new handler of each month body. 

        for (var i = 0; i < calendar._monthsBody.rows.length; i++) {
            var row = calendar._monthsBody.rows[i];
            for (var j = 0; j < row.cells.length; j++) {
                $addHandlers(row.cells[j].firstChild, calendar._cell$delegates);
            }
        }
    }
}

function CalendarShownFromDate(sender, args) {
    //set the default mode to month 
    sender._switchMode("months", true);

    //RedefineHandler(objCalendarFrom);
    //RedefineHandler(objCalendarTo);
}

function CalendarHiddenFromDate(sender, args) {

    if (sender.get_selectedDate()) {

        objSelectedDate = sender.get_selectedDate();

        var selectedDate = new Date(objSelectedDate);

        var intMonth = selectedDate.getMonth() + 1;
        var intYear = selectedDate.getFullYear();

        if (!isNaN(intMonth) && !isNaN(intYear)) {
            lblFromDateDummy.value = null
            //if (intMonth < 10) {
            //    txtFromDateDummy.value = '0' + intMonth + '-' + intYear;
            //}
            //else {
            //    txtFromDateDummy.value = intMonth + '-' + intYear;
            //}
            var txtMonth = null
            switch (intMonth) {
                case 1:
                    txtMonth = "January"; break;
                case 2:
                    txtMonth = "February"; break;
                case 3:
                    txtMonth = "March"; break;
                case 4:
                    txtMonth = "April"; break;
                case 5:
                    txtMonth = "May"; break;
                case 6:
                    txtMonth = "June"; break;
                case 7:
                    txtMonth = "July"; break;
                case 8:
                    txtMonth = "August"; break;
                case 9:
                    txtMonth = "September"; break;
                case 10:
                    txtMonth = "October"; break;
                case 11:
                    txtMonth = "November"; break;
                case 12:
                    txtMonth = "December"; break;
            }

            if (txtMonth != null) {
                lblFromDateDummy.innerText = txtMonth + ", " + intYear;
            }
            else {
                lblFromDateDummy.innerText = "&nbsp;";
            }
        }
    }
}

function ResetCalendarFromDate(objFromDateDummyID) {
    var objFromDateDummy = document.getElementById(objFromDateDummyID);

    if (objCalendarFrom != null && objFromDateDummy != null) {
        //alert(objCalendarFrom.get_selectedDate());
        var selectedDate = new Date(objCalendarFrom.get_selectedDate());

        var intMonth = selectedDate.getMonth() + 1;
        var intYear = selectedDate.getFullYear();

        if (!isNaN(intMonth) && !isNaN(intYear) && intYear != "1970") {
            objFromDateDummy.innerText = "&nbsp;"

            var txtMonth = null
            switch (intMonth) {
                case 1:
                    txtMonth = "January"; break;
                case 2:
                    txtMonth = "February"; break;
                case 3:
                    txtMonth = "March"; break;
                case 4:
                    txtMonth = "April"; break;
                case 5:
                    txtMonth = "May"; break;
                case 6:
                    txtMonth = "June"; break;
                case 7:
                    txtMonth = "July"; break;
                case 8:
                    txtMonth = "August"; break;
                case 9:
                    txtMonth = "September"; break;
                case 10:
                    txtMonth = "October"; break;
                case 11:
                    txtMonth = "November"; break;
                case 12:
                    txtMonth = "December"; break;
            }

            if (txtMonth != null) {
                objFromDateDummy.innerText = txtMonth + ", " + intYear;
            }
            else {
                objFromDateDummy.innerText = "&nbsp;";
            }
        }
    }
}


function CalendarShownToDate(sender, args) {
    //set the default mode to month 
    sender._switchMode("months", true);

    //RedefineHandler(objCalendarFrom);
    //RedefineHandler(objCalendarTo);
}

function CalendarHiddenToDate(sender, args) {

    if (sender.get_selectedDate()) {

        objSelectedDate = sender.get_selectedDate();

        var selectedDate = new Date(objSelectedDate);

        var intMonth = selectedDate.getMonth() + 1;
        var intYear = selectedDate.getFullYear();

        if (!isNaN(intMonth) && !isNaN(intYear)) {
            lblToDateDummy.value = null
            //if (intMonth < 10) {
            //    txtToDateDummy.value = '0' + intMonth + '-' + intYear;
            //}
            //else {
            //    txtToDateDummy.value = intMonth + '-' + intYear;
            //}
            var txtMonth = null
            switch (intMonth) {
                case 1:
                    txtMonth = "January"; break;
                case 2:
                    txtMonth = "February"; break;
                case 3:
                    txtMonth = "March"; break;
                case 4:
                    txtMonth = "April"; break;
                case 5:
                    txtMonth = "May"; break;
                case 6:
                    txtMonth = "June"; break;
                case 7:
                    txtMonth = "July"; break;
                case 8:
                    txtMonth = "August"; break;
                case 9:
                    txtMonth = "September"; break;
                case 10:
                    txtMonth = "October"; break;
                case 11:
                    txtMonth = "November"; break;
                case 12:
                    txtMonth = "December"; break;
            }

            if (txtMonth != null) {
                lblToDateDummy.innerText = txtMonth + ", " + intYear;
            }
            else {
                lblToDateDummy.innerText = "&nbsp;";
            }
        }
    }
}

function ResetCalendarToDate(objToDateDummyID) {
    var objToDateDummy = document.getElementById(objToDateDummyID);

    if (objCalendarTo != null && objToDateDummy != null) {
        var selectedDate = new Date(objCalendarTo.get_selectedDate());

        var intMonth = selectedDate.getMonth() + 1;
        var intYear = selectedDate.getFullYear();

        if (!isNaN(intMonth) && !isNaN(intYear) && intYear != "1970") {
            objToDateDummy.innerText = "&nbsp;"

            var txtMonth = null
            switch (intMonth) {
                case 1:
                    txtMonth = "January"; break;
                case 2:
                    txtMonth = "February"; break;
                case 3:
                    txtMonth = "March"; break;
                case 4:
                    txtMonth = "April"; break;
                case 5:
                    txtMonth = "May"; break;
                case 6:
                    txtMonth = "June"; break;
                case 7:
                    txtMonth = "July"; break;
                case 8:
                    txtMonth = "August"; break;
                case 9:
                    txtMonth = "September"; break;
                case 10:
                    txtMonth = "October"; break;
                case 11:
                    txtMonth = "November"; break;
                case 12:
                    txtMonth = "December"; break;
            }

            if (txtMonth != null) {
                objToDateDummy.innerText = txtMonth + ", " + intYear;
            }
            else {
                objToDateDummy.innerText = "&nbsp;";
            }
        }
    }
}

function ChangeFromDateMMYYYYToDDMMYYYY(objTxtFromDateDummy, objTxtFromDate, objBtnFromDateDummy, objBtnFromDate) {
    if (FromDateInit(objTxtFromDateDummy, objTxtFromDate, objBtnFromDateDummy, objBtnFromDate)) {
        btnFromDateDummy.style.display = "none";
        btnFromDate.style.display = "inline";
    }

    var pattern = new RegExp("^(0[1-9]|1[0-2])-?20\\d{2}$", "g");
    if (pattern.test(lblFromDateDummy.value)) {
        txtFromDate.value = null
        var objDate = new Date();

        if (lblFromDateDummy.value.length == 6) {
            txtFromDate.value = '01-' + lblFromDateDummy.value.substr(0, 2) + '-' + lblFromDateDummy.value.substr(2, 4);
            objDate.setFullYear(lblFromDateDummy.value.substr(2, 4), lblFromDateDummy.value.substr(0, 2) - 1, 1);
        }
        else {
            txtFromDate.value = '01-' + lblFromDateDummy.value;
            objDate.setFullYear(lblFromDateDummy.value.substr(3, 4), lblFromDateDummy.value.substr(0, 2) - 1, 1);
        }

        objCalendarFrom.show();
        objCalendarFrom.set_selectedDate(objDate);
        objCalendarFrom.hide();
    }
    else {
        objCalendarFrom.show();
        objCalendarFrom.set_selectedDate(null);
        objCalendarFrom.hide();
    }
}

function FromDateInit(objTxtFromDateDummy, objTxtFromDate, objBtnFromDateDummy, objBtnFromDate) {
    lblFromDateDummy = document.getElementById(objTxtFromDateDummy);
    txtFromDate = document.getElementById(objTxtFromDate);
    btnFromDateDummy = document.getElementById(objBtnFromDateDummy);
    btnFromDate = document.getElementById(objBtnFromDate);

    if (lblFromDateDummy == null || txtFromDate == null || btnFromDateDummy == null || btnFromDate == null) {
        return false;
    }
    else {
        return true;
    }
}

function ChangeToDateMMYYYYToDDMMYYYY(objTxtToDateDummy, objTxtToDate, objBtnToDateDummy, objBtnToDate) {
    if (ToDateInit(objTxtToDateDummy, objTxtToDate, objBtnToDateDummy, objBtnToDate)) {
        btnToDateDummy.style.display = "none";
        btnToDate.style.display = "inline";
    }

    var patternTo = new RegExp("^(0[1-9]|1[0-2])-?20\\d{2}$", "g");
    if (patternTo.test(lblToDateDummy.value)) {
        txtToDate.value = null
        var objDate = new Date();

        if (lblToDateDummy.value.length == 6) {
            txtToDate.value = '01-' + lblToDateDummy.value.substr(0, 2) + '-' + lblToDateDummy.value.substr(2, 4);
            objDate.setFullYear(lblToDateDummy.value.substr(2, 4), lblToDateDummy.value.substr(0, 2) - 1, 1);
        }
        else {
            txtToDate.value = '01-' + lblToDateDummy.value;
            objDate.setFullYear(lblToDateDummy.value.substr(3, 4), lblToDateDummy.value.substr(0, 2) - 1, 1);
        }

        objCalendarTo.show();
        objCalendarTo.set_selectedDate(objDate);
        objCalendarTo.hide();
    }
    else {
        objCalendarTo.show();
        objCalendarTo.set_selectedDate(null);
        objCalendarTo.hide();
    }
}

function ToDateInit(objTxtToDateDummy, objTxtToDate, objBtnToDateDummy, objBtnToDate) {
    lblToDateDummy = document.getElementById(objTxtToDateDummy);
    txtToDate = document.getElementById(objTxtToDate);
    btnToDateDummy = document.getElementById(objBtnToDateDummy);
    btnToDate = document.getElementById(objBtnToDate);

    if (lblToDateDummy == null || txtToDate == null || btnToDateDummy == null || btnToDate == null) {
        return false;
    }
    else {
        return true;
    }
}

function CalendarFromDateClick() {
    btnFromDateDummy.style.display = "none";
    btnFromDate.style.display = "inline";
    btnFromDate.style.outline = "none";

    if (!objCalendarFrom.get_isOpen()) {

        objCalendarFrom.show();
        objCalendarFrom.focus();
    }
    else {
        objCalendarFrom.hide();
    }
}

function CalendarToDateClick() {
    btnToDateDummy.style.display = "none";
    btnToDate.style.display = "inline";
    btnToDate.style.outline = "none";

    if (!objCalendarTo.get_isOpen()) {

        objCalendarTo.show();
        objCalendarTo.focus();
    }
    else {
        objCalendarTo.hide();
    }
}

function RemoveButtonMouseIn(img) {
    img.src = img.src.replace("RemoveX", "RemoveX_red");
}

function RemoveButtonMouseOut(img) {
    img.src = img.src.replace("RemoveX_red", "RemoveX");
}

function CheckboxTextboxRelation(chk, textboxId) {
    var txt = document.getElementById(textboxId);

    if (chk.checked) {
        txt.disabled = '';
    } else {
        txt.value = '';
        txt.disabled = 'disabled';
    }
}