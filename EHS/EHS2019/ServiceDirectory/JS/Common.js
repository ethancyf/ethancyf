// JScript File

function Upper(e, r) {
    r.value = r.value.toUpperCase();
}

function openNewWin(link) {
    var wi = screen.width;
    var he = screen.height;
    window.open(link, '', 'width=' + wi + ',height=' + he * 0.88 + ',location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
}

// check / un-check child for district checkboxlist
function checkChild(e) {
    var controlID = e.id;
    controlID = controlID.replace('cb_area_', '');
    var cblChildList = document.getElementsByTagName('Input');
    for (var i = 0; i < cblChildList.length; i++) {
        var child = cblChildList[i];
        var name = child.id;
        if (name.indexOf("cbl_area_" + controlID) > -1) {
            child.checked = e.checked;
        }
    }
}

// check / un-check parent for the district checkbox
function checkParent(e) {
    var controlID = e.id;
    controlID = controlID.replace('cbl_area_', '');
    var cbParent = document.getElementById('cb_area_' + controlID);
    var cblChildList = document.getElementsByTagName('Input');
    var blnChecked = false;
    for (var i = 0; i < cblChildList.length; i++) {
        var child = cblChildList[i];
        var name = child.id;
        if (name.indexOf("cbl_area_" + controlID) > -1) {
            if (!child.checked) {
                cbParent.checked = false;
                return;
            }
        }
    }
    cbParent.checked = true;
}

// show/hide the filtering criteria
function ListOpenClose() {
    elementid = document.getElementById('ImgFilter');
    if (TblFilterCriteria.style.display.length == 0) {
        elementid.ImageURL = "~/Images/others/Expand.gif";
        TblFilterCriteria.style.display = "none";
    }
    else {
        elementid.ImageURL = "~/Images/others/Collapse.gif";
        TblFilterCriteria.style.display = "";
    }
}

//  Check if the focus in at textbox
function textfieldfocus(e) {
    var target = (e && e.target) || (event && event.srcElement);
    if (((target.type == 'text') || (target.type == 'password')) && ((target.readOnly == false)))
        return false;
    else
        return true;
}

// Check Keyboard event to Disable BackSpace and Ctrl-N
function check(e) {
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;

    if (e.altKey && code == 37) {
        //window.alert('disable alt+LEFT');    
        return false;
    }

    if (e.altKey && code == 39) {
        //window.alert('disable alt+RIGHT');    
        return false;
    }

    if (code == 8 && textfieldfocus(e)) {
        code = 0;
        //window.alert('disable backspace');
        return false;
    }

    //if (e.ctrlKey && code == 82) {
    //    //window.alert('disable Ctrl r');    
    //    return false;
    //}
    //
    //if (code == 116 && !textfieldfocus(e)) {
    //    //window.alert('disable F5');
    //    code = 0;
    //    return false;
    //}

    //Disable Ctrl-N to open new window
    if (e.ctrlKey && code == 78) {
        return false;
    }

    return true;
}

//INT16-0003 (Fix subsidy checkbox indent in SDIR) [Start][Chris YIM]
//-----------------------------------------------------------------------------------------
function selectValue() {
    var images = document.getElementsByTagName('IMG');
    var hyperlinks = document.getElementsByTagName('A');
    var inputs = document.getElementsByTagName('INPUT');
    var perfix = 'TreeViewServicen'
    var imageId
    var checkboxId
    var textId

    for (var i = 0; i < images.length; i++) {
        if (images[i].src.search("checkbox_D.png") > 0) {
            images[i].style.width = '0px';
        }
    }

    for (var i = 0; i < hyperlinks.length; i++) {
        if (hyperlinks[i].id.search(/TreeViewServicen[0-9]*i/i) != -1) {

            imageId = hyperlinks[i].id.substr(perfix.length, 1)

            for (var j = 0; j < inputs.length; j++) {
                if (inputs[j].id.search(/TreeViewServicen[0-9]*CheckBox/i) != -1) {

                    checkboxId = inputs[j].id.substr(perfix.length, 1);

                    if (imageId == checkboxId) {
                        inputs[j].disabled = true;
                        //textId = perfix.concat(checkboxId);
                        //document.getElementById(textId).disabled = true;
                    }
                }
            }

        }
    }
}
//INT16-0003 (Fix subsidy checkbox indent in SDIR) [End][Chris YIM]        


//      Check on every keyboard type
document.onkeydown = check;
//document.onkeyup = check;
//document.onkeypress = check;

//        Disable right click also

document.oncontextmenu = function () { return false; }
//document.ondragstart   = function() { return false; }
document.onmousedown = md;
function md(e) {
    try { if (event.button == 2 || event.button == 3) return false; }
    catch (e) { if (e.which == 3) return false; }
}

// CRE17-005 (Enhance EHCP list with search function)  (Marco)        
function CheckTextInput(alertMsg) {
    var lblServiceProviderName = document.getElementById('lblServiceProvider').innerHTML;
    var lblPracticeName = document.getElementById('lblPracticeName').innerHTML;
    var lblPracticeAddr = document.getElementById('lblPracticeAddr').innerHTML;
    var strServiceProviderName = document.getElementById('txtServiceProvider').value;
    var strPracticeName = document.getElementById('txtPracticeName').value;
    var strPracticeAddr = document.getElementById('txtPracticeAddr').value;
    //var chars = strServiceProviderName.trim().concat( strPracticeName.trim(), strPracticeAddr.trim());
    var lbl = "";

    var chkpass = true;
    if (CheckFullWidthChar(strServiceProviderName.trim()) == false) {
        lbl += lblServiceProviderName + ", ";
        chkpass = false;
    }
    if (CheckFullWidthChar(strPracticeName.trim()) == false) {
        lbl += lblPracticeName + ", ";
        chkpass = false;
    }
    if (CheckFullWidthChar(strPracticeAddr.trim()) == false) {
        lbl += lblPracticeAddr + ", ";
        chkpass = false;
    }

    if (chkpass) {
        return true;
    }
    else {
        lbl = lbl.substring(0, lbl.length - 2);
        lbl = alertMsg.replace("/s", lbl);
        alert(lbl);
        return false;
    }
}

function CheckFullWidthChar(chars) {
    var bln = true;

    for (var i = 0, l = chars.length; i < l; i++) {
        var b = chars[i];
        var c = chars[i].charCodeAt(0);

        //full width char code
        if (c >= 0xFF00 && c <= 0xFFEF) {
            bln = false;
            break;
        }
        else {
            //half width char code
        }
    }

    return bln;
}
