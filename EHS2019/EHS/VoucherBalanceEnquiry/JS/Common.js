// JScript File

function Upper(e,r)
{
    r.value = r.value.toUpperCase();
}

    function openNewWin(link)
{
    var wi = screen.width;
    var he = screen.height;
    window.open(link, '', 'width='+wi+',height='+he*0.88+',location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
}

//  Check if the focus in at textbox
function textfieldfocus(e){
    var target = (e && e.target) || (event && event.srcElement);
    if (((target.type=='text') || (target.type=='password')) && ((target.readOnly==false)))
        return false;
    else
        return true;
}

// Check Keyboard event to Disable BackSpace and Ctrl-N
function check(e){
    var code;
    if (!e) var e = window.event;
    if (e.keyCode) code = e.keyCode;
    else if (e.which) code = e.which;
    
    if(e.altKey && code == 37)
    {
        return false;
    }

    if (code == 8&&textfieldfocus(e))
    {
       code = 0;
       //window.alert('Button Disabled');
       return false;
    }
    
    //Disable Ctrl-N to open new window
    if (e.ctrlKey && code == 78)
    {
       return false;
    }
                
    return true;
}
        

//      Check on every keyboard type
document.onkeydown = check;
document.onkeyup = check;
document.onkeypress = check;
        
//        Disable right click also
        document.oncontextmenu = function() { return false; }
        //document.ondragstart   = function() { return false; }
        document.onmousedown   = md;
        function md(e) 
        { 
          try { if (event.button==2||event.button==3) return false; }  
          catch (e) { if (e.which == 3) return false; } 
        }
        
function openNewWin(link)
{
    var wi = screen.width;
    var he = screen.height;
    window.open(link, '', 'width='+wi+',height='+he*0.88+',location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
}

function formatHKID(textbox) {
            textbox.value=textbox.value.toUpperCase();
            txt = textbox.value;
            var res="";
            if ((txt.length == 8) || (txt.length ==9))
            {  if ((txt.indexOf("(")<0) && (txt.indexOf(")")<0))
                {	
                    res=txt.substring(0,txt.length-1) + "(" + txt.substring(txt.length-1, txt.length) + ")";
                }
               else
                {
                    res = txt;
                }
                textbox.value=res;
            }
            return false;
        }
        
        
// Filter continuous "-" character in the textbox control
function filterDateInput(tb)
{
    var re = new RegExp(/(\-)(\-)+/g);
    var match = false;
    var text = tb.value;

    while (text.match(re))
    {
        match = true;
        text = text.replace(/[^0-9]*(\-)[^0-9]*(\-)+[^0-9]*/g,'-');
    }
    
    if (match)
    {
        var strPos = strPos = doGetCaretPosition(tb);
        
        tb.value = text;
        
        if (strPos && strPos >= 0 && strPos < text.length)
        {
            doSetCaretPosition(tb, strPos)
        }
    }
    
    return match;
}

// Filter continuous "-" character from release keyboard button (handle paste by ctrl-v)
function filterDateInputKeyUpHandler(tb, e)
{
    var KeyID = (window.event) ? event.keyCode : e.keyCode;

    // Ignore Tab Key
    if (KeyID == 9)
        return;

    filterDateInput(tb);
}

// Filter continuous "-" character from pressing the "-" button
function filterDateInputKeyDownHandler(tb, e)
{
    var KeyID = (window.event) ? event.keyCode : e.keyCode;
    var isValid = true;

    // "-" key
    if (KeyID == 109 || KeyID == 189) {
        var re = new RegExp(/(\-)(\-)+/g);
        var pos = doGetCaretPosition(tb);
        var temp = "";
        if (pos == 0) {
            temp = "-" + tb.value;
        }
        else {
            temp = tb.value.substring(0, pos) + "-" + tb.value.substring(pos)        
        }

        if (temp.match(re)) {
            isValid = false;
            
            if (window.event) 
                event.returnValue = false;
            else
                e.preventDefault();                 
        }
    }
    
    return isValid;
}

// Get the caret position for textbox control
function doGetCaretPosition (ctrl) {
	var CaretPos = 0;
	if (document.selection) 
	{
		ctrl.focus();
		var Sel = document.selection.createRange();
		var SelLength = document.selection.createRange().text.length;
		Sel.moveStart ('character', -ctrl.value.length);
		CaretPos = Sel.text.length - SelLength;
	}
	else if (ctrl.selectionStart || ctrl.selectionStart == '0')
	{
		CaretPos = ctrl.selectionStart;
	}
	return (CaretPos);
}

// Set the caret position for textbox control 
function doSetCaretPosition(ctrl, caretPos) {
    if(ctrl.createTextRange) 
    {
        var range = ctrl.createTextRange();
        range.move('character', caretPos);
        range.select();
    }
    else 
    {
        if(ctrl.selectionStart) 
        {
            ctrl.focus();
            ctrl.setSelectionRange(caretPos, caretPos);
        }
        else 
        {
            ctrl.focus();
        }
    }
}
