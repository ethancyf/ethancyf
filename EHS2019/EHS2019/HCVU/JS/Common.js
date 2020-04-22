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

function checkPassword(password, passRuleCountMin, passRuleCountMax, ctrlLabelPoor, ctrlLabelModerate, ctrlLabelStrong, ctrlProgressBarID, strStrengthPoorMsg, strStrengthModerateMsg, strStrengthStrongMsg, ctrldir1, ctrldir2) { 
    var strengthBarPoor = document.getElementById(ctrlLabelPoor);
    var strengthBarModerate = document.getElementById(ctrlLabelModerate);
    var strengthBarStrong = document.getElementById(ctrlLabelStrong);
    var capitalRegex    = new RegExp("[A-Z]");
    var smallRegex      = new RegExp("[a-z]");
    var numberRegex     = new RegExp("[0-9]");
    var specialRegex    = new RegExp("[^A-Za-z0-9]");
    var score = 0;
 
    var percentage;
    var progressBar = document.getElementById(ctrlProgressBarID); 
    var dir1 = document.getElementById(ctrldir1); 
    var dir2 = document.getElementById(ctrldir2); 
 
    if (password.length==0) 
    {
        score = 0;
        strengthBarPoor.innerHTML = ''; 
        strengthBarModerate.innerHTML = ''; 
        strengthBarStrong.innerHTML = ''; 
        dir1.innerHTML = '';
        dir2.innerHTML = '';
        percentage = 0;     
    } 
    else 
    {
        strengthBarPoor.innerHTML = strStrengthPoorMsg; //'Poor>>Moderate>>Strong'; //'Poor';
        strengthBarModerate.innerHTML = strStrengthModerateMsg;
        strengthBarStrong.innerHTML = strStrengthStrongMsg;
        dir1.innerHTML = '>>';
        dir2.innerHTML = '>>';
       
        if ((capitalRegex.test(password)) && (score < 100))
        {
            if ((password.length >= 8) && (password.length <= 20))
            {
                score += 1;
            }
            else
            {
                score += 0;
            }
        }        

        if ((smallRegex.test(password)) && (score < 100))
        {
            if ((password.length >= 8) && (password.length <= 20))
            {
                score += 1;
            }
            else
            {
                score += 0;
            }           
        }
        
        if ((numberRegex.test(password)) && (score < 100))
        {
            if ((password.length >= 8) && (password.length <= 20))
            {
                score += 1;
            }
            else
            {
                score += 0;
            }
        }
        
        if ((specialRegex.test(password)) && (score < 100))
        {
            if ((password.length >= 8) && (password.length <= 20))
            {
                score += 1;
            }
            else
            {
                score += 0;
            }
        }

        if (score < passRuleCountMin) 
        {            
            percentage = 0.33;
            progressBar.style.backgroundColor = "red";
        }
        else if (score == passRuleCountMin || score == passRuleCountMax) 
        {        
            percentage = 0.67;
            progressBar.style.backgroundColor = "orange";
        }
        else
        {
            percentage = 1;
            progressBar.style.backgroundColor = "#3bce08";
        }
    }
    
    progressBar.style.width = percentage*100 + "%";
}

function convertToUpper(textbox) {
    textbox.value = textbox.value.toUpperCase();
}


 function SelectAllCheckboxes(chkbox)
    {    
	    var elm=chkbox.form.elements;
	    for(i=0;i<elm.length;i++)
	    if(elm[i].type=="checkbox" && elm[i].id!=chkbox.id)
        {
		    if(elm[i].checked!=chkbox.checked) elm[i].click();
        }
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
    if (((target.type == 'text') || (target.type == 'password') || (target.type == 'textarea')) && ((target.readOnly == false)))
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
    
    
//Set Component Art ComboBox 

function ComboBox_onChange(sender, e)
{
    document.getElementById(sender.DomElementId + "_TextBox").innerHTML = "<div style='width: " + (sender.Width - 23) + "px; overflow: hidden;'><nobr>" + sender.getSelectedItem().Text + "</nobr></div>";
}

function ComboBox_onLoad(sender, e) 
{
 	addEvent(document.getElementById(sender.get_id()+ '_Input'), 'blur', function () {ComboBox_onBlur(sender)}, false)
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

function openNewWin(link)
{
    var wi = screen.width;
    var he = screen.height;
    window.open(link, '', 'width='+wi+',height='+he*0.88+',location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
}

function openNewHTML(link)
{
    var wi = screen.width;
    var he = screen.height;
    window.open(link, '', 'width='+wi+',height='+he*0.88+',location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
}

// 'CRE12-001 eHS and PCD integration [Start][Tony]

function openNewModalWin(link, winname)
{
    var wi = screen.width;
    var he = screen.height;
    //window.showModalDialog(link, winname, 'dialogWidth='+wi+',dialogHeight='+he*0.88+',center=yes,scroll=yes,status=no,resizable=yes');
    window.showModalDialog(link, winname, 'dialogWidth='+wi+'px;dialogHeight='+he*0.88+'px;resizable=yes;status=no');
    //window.open(link, winname, 'width='+wi+',height='+he*0.88+',location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0,modal=yes', false);
}


// 'CRE12-001  eHS and PCD integration [End][Tony]

function autoTab(txtbox, tabToTxtbox, len)
{
    var agent  = navigator.userAgent.toLowerCase();
    var isNN = (agent.indexOf("netscape")!=-1);
    var isOpera = (agent.indexOf("opera")!=-1);
    var isIE = (agent.indexOf("msie") != -1);

    var keyCode;
      var filter;

      if (isOpera | isIE)
      {
         keyCode = event.keyCode;
         filter = [0,8,9,16,17,18,37,38,39,40,46];
      }
      else if (isNN)
      {
         keyCode = event.which;
         filter = [0,8,9];
      }

      if(txtbox.value.length >= len && !containsElement(filter,keyCode))
      {
         txtbox.value = txtbox.value.slice(0, len);
         tabToTxtbox.focus();
      }


      function containsElement(arr, ele)
      {

         var found = false, index = 0;

         while(!found && index < arr.length)

            if(arr[index] == ele)
               found = true;
            else
               index++;

         return found;
      }

      return true;
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

var hidReasonForVisitCount;
    var ibtnP_add;
    var ibtnAdd_S1;
    var ibtnRemove_S1;
    var trReasonForVisitS;
    var tblReasonForVistS1;
    var ddlReasonVisitFirst_S1;
    var ddlReasonVisitSecond_S1;
    var cddReasonVisitFirst_S1;
    var cddReasonVisitSecond_S1;
    var imgVisitReasonError_S1;
    var ddlReasonVisitFirst_S2;
    var ddlReasonVisitFirst_S3;
    
    var ddlReasonVisitFirst;
    
    //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]      
    var cddReasonVisitSecond;
    //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]   
    
    function ReasonForVisitInitialComplete(){
        var blnComplete = true;
        
        var obj = document.getElementById(ddlReasonVisitFirst);
        
        if (obj != null){
            if (obj.length <= 1 || obj.disabled == true){
                blnComplete = false;
            }
        }
        
        var obj = document.getElementById(ddlReasonVisitFirst_S3);
        
        if (obj != null){
            if (obj.length <= 1 || obj.disabled == true){
                blnComplete = false;
            }
        }
        
        var obj = document.getElementById(ddlReasonVisitFirst_S2);
        
        if (obj != null){
            if (obj.length <= 1 || obj.disabled == true){
                blnComplete = false;
            }
        }
        
        var obj = document.getElementById(ddlReasonVisitFirst_S1);
        
        if (obj != null){
            if (obj.length <= 1 || obj.disabled == true){
                blnComplete = false;
            }
        }
                
        return blnComplete;
    }
    
    function AddReasonForVisit(){
        var hidCount = document.getElementById(hidReasonForVisitCount);

        if (document.getElementById(ddlReasonVisitFirst).disabled == false){
            if (hidCount.value == "0"){
                //document.getElementById(ibtnP_add).style.display="none";
            } else {
                
                document.getElementById(String(ibtnAdd_S1).substring(0, ibtnAdd_S1.length -1) + (parseInt(hidCount.value) )).style.display="none";
                //document.getElementById(String(ibtnRemove_S1).substring(0, ibtnRemove_S1.length -1) + (parseInt(hidCount.value) )).style.display="none";
            }

            hidCount.value = parseInt(hidCount.value) + 1;
            
            //var tr = document.getElementById(trReasonForVisitS);
            //tr.style.display = "block";
            
            var tbl = document.getElementById(String(tblReasonForVistS1).substring(0, tblReasonForVistS1.length -1) + (parseInt(hidCount.value)));
            tbl.style.display = "block";
        }
        
    }
    
     function RemoveReasonForVisit(removeIndex){
        var hidCount = document.getElementById(hidReasonForVisitCount);
                
        var ddl_L1_A;
        var ddl_L1_B;
        
        var ddl_L2_A;
        var ddl_L2_B;
        
        var img_Err_A;
        var img_Err_B;
        
        var bLast = true;
        for (i=removeIndex + 1;i<=3;i++){
            tbl = document.getElementById(String(tblReasonForVistS1).substring(0, tblReasonForVistS1.length -1) + i);
            if (tbl.style.display=="block" || tbl.style.display==""){
                // Mark user is not pressing the last secondary
                bLast = false; 
                
                // Copy the next secondary to current secondary
                // ------------------------------------------------------
                // Secondary Level 1
                ddl_L1_A = $find(String(cddReasonVisitFirst_S1).substring(0, cddReasonVisitFirst_S1.length -1) + i);
                ddl_L1_B = $find(String(cddReasonVisitFirst_S1).substring(0, cddReasonVisitFirst_S1.length -1) + parseInt(i-1));
                                
                //ddl_L1_B.value = ddl_L1_A.value;
                ddl_L1_B.set_SelectedValue(ddl_L1_A.get_SelectedValue() , ddl_L1_A.get_SelectedValue());
                ddl_L1_B._onParentChange(false); 
                
                // Secondary Level 2
                ddl_L2_A = $find(String(cddReasonVisitSecond_S1).substring(0, cddReasonVisitSecond_S1.length -1) + i);
                ddl_L2_B = $find(String(cddReasonVisitSecond_S1).substring(0, cddReasonVisitSecond_S1.length -1) + parseInt(i-1));
                //ddl_L2_B.value = ddl_L2_A.value;
                ddl_L2_B.set_SelectedValue(ddl_L2_A.get_SelectedValue() , ddl_L2_A.get_SelectedValue);
                ddl_L2_B._onParentChange(null, true);
                $get(String(ddlReasonVisitSecond_S1).substring(0, ddlReasonVisitSecond_S1.length - 1) + parseInt(i - 1)).selectedIndex = ddl_L2_A.get_SelectedValue();
                
                // Secondary Error Image
                img_err_A = document.getElementById(String(imgVisitReasonError_S1).substring(0, imgVisitReasonError_S1.length -1) + i);
                img_err_B = document.getElementById(String(imgVisitReasonError_S1).substring(0, imgVisitReasonError_S1.length -1) + parseInt(i-1));
                img_err_B.style.display = img_err_A.style.display;
                img_err_A.style.display = "none";

                // Clear the next secondary
                // ------------------------------------------------------
                ddl_L1_A.set_SelectedValue("","");
                ddl_L1_A._onParentChange(false); 

            }
        }
        
            
        if (parseInt(hidCount.value) > 1){
            // Hide the last secondary (Maintain at least one visible)
            var tbl = document.getElementById(String(tblReasonForVistS1).substring(0, tblReasonForVistS1.length -1) + (parseInt(hidCount.value)));
            tbl.style.display = "none";
        
            // Show Add button at the last secondary (if not meet maximum)
            document.getElementById(String(ibtnAdd_S1).substring(0, ibtnAdd_S1.length -1) + (parseInt(hidCount.value) -1)).style.display="block";
            hidCount.value = parseInt(hidCount.value) - 1;
        }
        
        if (bLast){
            
            // Clear current pressing secondary, if it is the last secondary
            // ------------------------------------------------------
            // Secondary Level 1
            ddl_L1_A = $find(String(cddReasonVisitFirst_S1).substring(0, cddReasonVisitFirst_S1.length -1) + removeIndex);
            
            // Secondary Error Image
            img_err_A = document.getElementById(String(imgVisitReasonError_S1).substring(0, imgVisitReasonError_S1.length -1) + + removeIndex);
            img_err_A.style.display = "none";
            
             //alert(ddl_L1_A);
            //if (ddl_L1_A != null) {
            if (removeIndex == 1){
                $get(ddlReasonVisitFirst_S1).selectedIndex = 0;
                $get(ddlReasonVisitSecond_S1).selectedIndex = 0;    
                $get(ddlReasonVisitSecond_S1).disabled = true;
            }
                ddl_L1_A.set_SelectedValue("","");
                ddl_L1_A._onParentChange(false); 
                
            //}
            
        } 
    }
    
     function RemoveReasonForVisitWithoutL2(removeIndex) {

         var hidCount = document.getElementById(hidReasonForVisitCount);

         var ddl_L1_A;
         var ddl_L1_B;

         var img_Err_A;
         var img_Err_B;

         var bLast = true;
         for (i = removeIndex + 1; i <= 3; i++) {
             tbl = document.getElementById(String(tblReasonForVistS1).substring(0, tblReasonForVistS1.length - 1) + i);
             if (tbl.style.display == "block" || tbl.style.display == "") {
                 // Mark user is not pressing the last secondary
                 bLast = false;

                 // Copy the next secondary to current secondary
                 // ------------------------------------------------------
                 // Secondary Level 1
                 ddl_L1_A = $find(String(cddReasonVisitFirst_S1).substring(0, cddReasonVisitFirst_S1.length - 1) + i);
                 ddl_L1_B = $find(String(cddReasonVisitFirst_S1).substring(0, cddReasonVisitFirst_S1.length - 1) + parseInt(i - 1));

                 //ddl_L1_B.value = ddl_L1_A.value;
                 ddl_L1_B.set_SelectedValue(ddl_L1_A.get_SelectedValue(), ddl_L1_A.get_SelectedValue());
                 ddl_L1_B._onParentChange(false);

                 // Secondary Error Image
                 img_err_A = document.getElementById(String(imgVisitReasonError_S1).substring(0, imgVisitReasonError_S1.length - 1) + i);
                 img_err_B = document.getElementById(String(imgVisitReasonError_S1).substring(0, imgVisitReasonError_S1.length - 1) + parseInt(i - 1));
                 img_err_B.style.display = img_err_A.style.display;
                 img_err_A.style.display = "none";

                 // Clear the next secondary
                 // ------------------------------------------------------
                 ddl_L1_A.set_SelectedValue("", "");
                 ddl_L1_A._onParentChange(false);

             }
         }


         if (parseInt(hidCount.value) > 1) {
             // Hide the last secondary (Maintain at least one visible)
             var tbl = document.getElementById(String(tblReasonForVistS1).substring(0, tblReasonForVistS1.length - 1) + (parseInt(hidCount.value)));
             tbl.style.display = "none";

             // Show Add button at the last secondary (if not meet maximum)
             document.getElementById(String(ibtnAdd_S1).substring(0, ibtnAdd_S1.length - 1) + (parseInt(hidCount.value) - 1)).style.display = "block";
             hidCount.value = parseInt(hidCount.value) - 1;
         }

         if (bLast) {
             // Clear current pressing secondary, if it is the last secondary
             // ------------------------------------------------------
             // Secondary Level 1
             ddl_L1_A = $find(String(cddReasonVisitFirst_S1).substring(0, cddReasonVisitFirst_S1.length - 1) + removeIndex);

             // Secondary Error Image
             img_err_A = document.getElementById(String(imgVisitReasonError_S1).substring(0, imgVisitReasonError_S1.length - 1) + +removeIndex);
             img_err_A.style.display = "none";

             //alert(ddl_L1_A);
             //if (ddl_L1_A != null) {
             if (removeIndex == 1) {
                 $get(ddlReasonVisitFirst_S1).selectedIndex = 0;
                 //$get(ddlReasonVisitSecond_S1).selectedIndex = 0;
                 //$get(ddlReasonVisitSecond_S1).disabled = true;
             }
             ddl_L1_A.set_SelectedValue("", "");
             ddl_L1_A._onParentChange(false);

             //}

         }
     }


    function ClearSecondaryReasonForVisit(){
        var hidCount = document.getElementById(hidReasonForVisitCount);
        
        var ddl_L1;
        
        ddl_L1 = $get(ddlReasonVisitFirst_S1);
        
        if (document.getElementById(ddlReasonVisitFirst).selectedIndex == 0){

            // Register CascadeDropDownExtender Populated event to disable drop down after populate
            //$find(cddReasonVisitFirst_S1).add_populated(onPopulated_DisableDropDown);
            
            for (var i= parseInt(hidCount.value); i > 0; i--){
                RemoveReasonForVisit(i);
            }


            $find(cddReasonVisitFirst_S1).disabled = true;
            //$get(ibtnAdd_S1).disabled = true;

        } else {
            $find(cddReasonVisitFirst_S1).disabled = false;
            //ddl_L1.disabled = false;
            //$get(ibtnAdd_S1).disabled = false;
            
            //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]        
            var cddSecond = $find(cddReasonVisitSecond);
            cddSecond.set_SelectedValue("","");
            cddSecond._onParentChange(false);
            //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        }
    }
    
    //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    function ClearReasonVisitSecond(index) 
    {
        var ddl;
        var ccd;

        var tbl = document.getElementById(String(tblReasonForVistS1).substring(0, tblReasonForVistS1.length -1) + index);
        if (tbl.style.display=="block" || tbl.style.display=="")
        {
            ddl = document.getElementById(String(ddlReasonVisitSecond_S1).substring(0, ddlReasonVisitSecond_S1.length -1) + index);
            ccd = $find(String(cddReasonVisitSecond_S1).substring(0, cddReasonVisitSecond_S1.length -1) + index);

            if (ddl.selectedIndex != 0)
            {
                ccd.set_SelectedValue("","");
                ccd._onParentChange(false);
            }
        }
    }
    //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
    
    function onPopulated_DisableDropDown(sender, args)
    {
        $get(ddlReasonVisitFirst_S1).disabled = true;
    } 
    
    function addEvent(obj, evType, fn){ 
         if (obj.addEventListener){ 
                obj.addEventListener(evType, fn, false); 
                return true; 
         } else if (obj.attachEvent){ 
               var r = obj.attachEvent("on"+evType, fn); 
                return r; 
         } else { 
                return false; 
         } 
    }
    
    //INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
    //-----------------------------------------------------------------------------------------
    function ResetScrollPosition()
    {
        setTimeout("window.scrollTo(0,0)", 0);
    }
    //INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

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
