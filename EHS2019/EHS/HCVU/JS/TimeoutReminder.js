// ------------------------------------------------------------------------------------
// JS Content same as HCSP, HCVU, eForm & PFC
// ------------------------------------------------------------------------------------

// Global Variables
var timer;
var lang;
var initialTime;
var displayTime;
var reminderID;
var reminderMessageID;
var reminderMessage;
var exccessSessionTime;
var exccessReminderTime;
var reminderPopuped;

// Execute ResetRemainTime() on every async postback
Sys.Application.add_init(AppInit);
 
function AppInit(sender) {
  Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ResetRemainTime);
  Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(ResetRemainTime);
}

function StartTimeoutReminder(_initialTime, _displayTime, _lang, _reminderID, _reminderMessageID) {
    initialTime = _initialTime;
    lang = _lang;
    reminderID = _reminderID;
    reminderMessageID = _reminderMessageID;
    displayTime = _displayTime;
    reminderPopuped = false;
    reminderMessage = document.getElementById(reminderMessageID).innerHTML;
    clearTimeout(timer);

    var now = Date.now();
    exccessSessionTime = now + (initialTime * 1000);
    exccessReminderTime = now + ((initialTime * 1000) - (displayTime * 60 * 1000));

    if (displayTime != 0 )
        CountDown();
}

function CountDown() {
    var currentTime = Date.now();

    if (exccessSessionTime <= currentTime) {
        if (lang == "")
            window.location = "../SessionTimeout.aspx";
        else
            window.location = "../" + lang + "/SessionTimeout.aspx";

        return;
    }

    if (exccessReminderTime <= currentTime && !reminderPopuped) {
        ShowReminder();
        reminderPopuped = true;
    }

    timer = setTimeout("CountDown()", 1000);
}

function KeepSessionAlive() {
    if (window.XMLHttpRequest) {
        req = new XMLHttpRequest();
    } else if (window.ActiveXObject) {
        req = new ActiveXObject("Microsoft.XMLHTTP");
    }

    req.open("POST", "../General/KeepSessionAlive.aspx?ts=" + Math.random().toString().substring(2), true);

    req.send();
}

function ShowReminder() {
    var elmMsg = document.getElementById(reminderMessageID);
    elmMsg.innerHTML = reminderMessage.replace("[TIME_OUT]", displayTime);
    elmMsg.innerHTML = elmMsg.innerHTML.replace("[MESSAGE_ALERT_TIME]", Clock.SystemTime);
    // CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
    //$find(reminderID).show();
    document.getElementById(reminderID).style.visibility = 'visible';
    // CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
}

function ReminderOK_Click() {
    KeepSessionAlive();
    ResetRemainTime();
    // CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
    document.getElementById(reminderID).style.visibility = 'hidden';
    // CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
}

function ResetRemainTime() {
    var now = Date.now();
    exccessSessionTime = now + (initialTime * 1000);
    exccessReminderTime = now + ((initialTime * 1000) - (displayTime * 60 * 1000));
    reminderPopuped = false;
}

function GetCurrentTime() {
    var d = new Date();
    var s;
 
    if (d.getHours() < 10) {
        s = "0" + d.getHours().toString();
    } else {
        s = d.getHours().toString();
    }
 
    s += ":"
 
    if (d.getMinutes() < 10) {
        s += "0" + d.getMinutes().toString();
    } else {
        s += d.getMinutes().toString();
    }
 
    s += ":"
 
    if (d.getSeconds() < 10) {
        s += "0" + d.getSeconds().toString();
    } else {
        s += d.getSeconds().toString();
    }
 
    return s;
}
