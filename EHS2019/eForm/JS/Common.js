// JScript File
function Upper(e,r)
{
    r.value = r.value.toUpperCase();
}


var bank_field_length=0;
function TabNext(obj,event,len,next_field)
{
    if (event == "down")
    {
        bank_field_length = obj.value.length;
    }
	else if (event == "up")
	{
	    if (obj.value.length != bank_field_length)
	    {
	        bank_field_length = obj.value.length;
	        if (bank_field_length == len)
	        {
	            next_field.focus();
	        }
	    }
	}
}

function formatHKID(textbox)
{   
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

function ltrim(stringToTrim) {
	//return stringToTrim.replace(/^\s+/,"");
	stringToTrim.value = stringToTrim.value.replace(/^\s+/,"");
}

function trim(stringToTrim) {
	
	return stringToTrim.replace(/^\s+|\s+$/g,"");

}

function changeInt(txt)
{
    if (trim(txt.value) != "")
    {
        txt.value = parseInt(txt.value, 10);
    }
}

function disableCopyPaste() {
  // current pressed key
  var pressedKey = String.fromCharCode(event.keyCode).toLowerCase();

  if (event.ctrlKey && (pressedKey == "c" || 
                        pressedKey == "v")) {
    // disable key press porcessing
    event.returnValue = false;
  }

} // onKeyDown


//  Check if the focus in at textbox
function textfieldfocus(e){
    var target = (e && e.target) || (event && event.srcElement);
    if (((target.type == 'text') || (target.type == 'textarea') || (target.type == 'password')) && ((target.readOnly == false)))
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

    //Disable backspace except textbox, textarea and password field
    if (code == 8 && textfieldfocus(e))
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
        
//Set Component Art ComboBox 
function ComboBox_onLoad(sender, e) 
{
 	//addEvent(document.getElementById(sender.get_id()+ '_Input'), 'blur', function () {ComboBox_onBlur(sender)}, false)
    document.getElementById(sender.DomElementId + "_TextBox").innerHTML = "<div style='width: " + (sender.Width - 23) + "px; overflow: hidden;'><nobr>" + sender.getSelectedItem().Text + "</nobr></div>";
}

function ComboBox_onBlur(obj) 
{
	var item = obj.findItemByProperty('Text', obj.get_text());
	if (item) 
	{
		obj.selectItem(item);
		obj.Postback();
        	}
	return true;
}

function addEvent(obj, evType, fn, useCapture)
{
	if (obj.addEventListener)
	{
		obj.addEventListener(evType, fn, useCapture);
		return true;
	}
	else if (obj.attachEvent)
	{
		var r = obj.attachEvent('on'+evType, fn);
		return r;
	}
}

//CRE17-013 (Extend bank account name to 300 chars) [Start][Chris YIM]
//-----------------------------------------------------------------------------------------
function LimitLength(obj, len, e) {
    var newlines = (obj.value.match(/\n/g) || []).length;
    if ((obj.value.length + newlines) > len) {

        //e.preventDefault();

        var temp = obj.value.substring(0, len);
        obj.value = "";
        obj.value = temp;

        return false;
    }
    else {
        return true;
    }
}
//CRE17-013 (Extend bank account name to 300 chars) [End][Chris YIM]