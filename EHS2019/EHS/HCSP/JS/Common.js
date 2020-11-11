// JScript File
function convertToUpper(textbox) {
    textbox.value = textbox.value.toUpperCase();
}

function showHKICHelp(lang) {   
    var l;

    switch (lang) {
        case 'zh-cn':
            l = 'CN';
            break;
        default:
            l = '';
    }

    newwindow = window.open('../../../SampleHKID' + l + '.htm', 'Sample', 'toolbar=no, height=420, width=800, menubar=no, resizable=no, location=no, status=no');
    newwindow.focus();    
    return false;
}


function showADOPCHelp(lang) {
    newwindow = window.open('../../../SampleADOPC.htm', 'Sample', 'toolbar=no, height=700, width=750, menubar=no, resizable=no, location=no, status=no, scrollbars=yes')
    newwindow.focus();
    return false;
}

function showDocIHelp(lang) {
    newwindow = window.open('../../../SampleDI.htm', 'Sample', 'toolbar=no, height=500, width=660, menubar=no, resizable=no, location=no, status=no')
    newwindow.focus();
    return false;
}

function showREPMTHelp(lang) {
    newwindow = window.open('../../../SampleREPMT.htm', 'Sample', 'toolbar=no, height=500, width=660, menubar=no, resizable=no, location=no, status=no')
    newwindow.focus();
    return false;
}

function showHKBCHelp(lang) {
    var l;

    switch (lang) {
        case 'zh-cn':
            l = 'CN';
            break;
        default:
            l = '';
    }
    newwindow = window.open('../../../SampleHKBC' + l + '.htm', 'Sample', 'toolbar=no, height=590, width=450, menubar=no, resizable=no, location=no, status=no,')
    newwindow.focus();
    return false;
}

function showVISAHelp(lang) {
    newwindow = window.open('../../../SampleVISA.htm', 'Sample', 'toolbar=no, height=500, width=920, menubar=no, resizable=no, location=no, status=no,')
    newwindow.focus();
    return false;
}

function showUpdateNow(lang) {
    var language;

    switch (lang) {
        case 'en-us':
            language = 'EN';
            break;
        case 'zh-tw':
            language = 'ZH';
            break;
        default:
            language = 'EN';
    }

    newwindow = window.open('../' + language + '/ReadSmartIDTips.htm', 'ReadSmartIDTips', 'toolbar=no, height=300, width=800, menubar=no, resizable=yes, location=no, status=no');
    newwindow.focus();
    return false;
}

/*
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
*/

function formatHKID(textbox) {
    this.UpperIndentityNo(textbox);
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

function formatVISA(textbox)
{
    this.UpperIndentityNo(textbox);
    txt = textbox.value;
    var res="";
    if (txt.length == 14) 
    {  
        if ((txt.indexOf("-")<0) && (txt.indexOf("(")<0) && (txt.indexOf(")")<0))
        {
            res=txt.substring(0,4) + "-" + txt.substring(4, 11) + "-" + txt.substring(11, 13) + "(" + txt.substring(13, 14) + ")";
        }
    
        textbox.value=res;
    }
    return false;
}

function UpperIndentityNo(textbox)
{
    textbox.value=textbox.value.toUpperCase();
    return false;
}

function formatReferenceNo(textbox) {
    textbox.value=textbox.value.toUpperCase();
    txt = textbox.value;
    var res="";
    if (txt.length==14) {
        if (txt.indexOf("(")<0 && txt.indexOf(")")<0) {
            res = txt.substring(0,txt.length-1) + "(" + txt.substring(txt.length-1, txt.length) + ")";
        }else {
            res = txt;
        }
        textbox.value=res;
    }
    return false;
}

function clearPassword(ctrlLabelPoor, ctrlLabelModerate, ctrlLabelStrong, ctrlProgressBarID, ctrldir1, ctrldir2)
{
    var strengthBarPoor = document.getElementById(ctrlLabelPoor);
    var strengthBarModerate = document.getElementById(ctrlLabelModerate);
    var strengthBarStrong = document.getElementById(ctrlLabelStrong);
    var score = 0;
    var percentage;
    var progressBar = document.getElementById(ctrlProgressBarID); 
    var dir1 = document.getElementById(ctrldir1); 
    var dir2 = document.getElementById(ctrldir2); 
    
    score = 0;
    strengthBarPoor.innerHTML = ''; 
    strengthBarModerate.innerHTML = ''; 
    strengthBarStrong.innerHTML = ''; 
    dir1.innerHTML = '';
    dir2.innerHTML = '';
    percentage = 0;
    
    progressBar.style.width = percentage*100 + "%";
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


//  Check if the focus in at textbox
function textfieldfocus(e){
    var target = (e && e.target) || (event && event.srcElement);
    if (((target.type=='text') || (target.type=='password')) && ((target.readOnly==false)))
        return false;
    else
        return true;
}

 function openNewHTML(link)
{
    var wi = screen.width;
    var he = screen.height;
    
    if (isMobileClient())
    {
        window.open(link, '', 'location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
    }else{
        window.open(link, '', 'width='+wi+',height='+he*0.88+',location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
    }
    //window.open(link, '', 'resizable=yes,status=yes,toolbar=no,location=no,scrollbars=yes,left=0,top=0,width=500,height=500');
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
       // document.ondragstart   = function() { return false; }
        document.onmousedown   = md;
        function md(e) 
        { 
          try { if (event.button==2||event.button==3) return false; }  
          catch (e) { if (e.which == 3) return false; } 
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
    
    // CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
    //  - for Record Confirmation select All check box to not 
    //     choose "Include Incomplete Claim"
     function SelectAllCheckboxesByGridId(chkbox)
    {   
        
        var bln = chkbox.checked;
        var table = chkbox.parentNode.parentNode.parentNode.parentNode;
        // 4 levels in object hierarchy: input -> cell -> row -> tablebody -> table
 
	    for (var i = 0, row; row = table.rows[i]; i++) {
	       for (var j = 0, col; col = row.cells[j]; j++) {
		    var elm = col.children;
		    for(k=0;k<elm.length;k++)
	    	    if(elm[k].type=="checkbox" && elm[k].id != chkbox.id)
        	    {
		            if (elm[k].checked != bln) elm[k].click();
        	    }  
	       }  
	    }
    }    
    // CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]    
    
    
function openNewWin(link)
{
    var wi = screen.width;
    var he = screen.height;
    
    if (isMobileClient())
    {
        window.open(link, '', 'location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
    }else{
        window.open(link, '', 'width='+wi+',height='+he*0.88+',location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
    }
    
}

// This OpenNewWin2 Function Only for HCSP
function openNewWin2(link)
{
    var blnOpen = true;
    if (opener && !opener.closed){
        try{
            if (opener.opener && opener.opener.location.href==self.location.href && opener.name && opener.name=='eHS_HCSP'){
                opener.focus();
                blnOpen = false;
            }
        }catch(err)
        {
        }        
    }
    if (blnOpen)
    {
        var wi = screen.width;
        var he = screen.height;
        var myWin;
        if (isMobileClient())
        {
            myWin = window.open(link, '', 'location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
        }else{
            myWin = window.open(link, '', 'width='+wi+',height='+he*0.88+',location=no,directories=no,menubar=no,toolbar=no,scrollbars=yes,status=yes,resizable=yes,left=0,top=0');
        }
            
        myWin.name = 'eHS_HCSP';
        opener = myWin;
    }   
}
 
 
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

// Detect client device, if mobile then return true;otherwise return false
        function isMobileClient() {
            var mobileClients = [
	            "midp",
	            "240x320",
	            "blackberry",
	            "netfront",
	            "nokia",
	            "panasonic",
	            "portalmmm",
	            "sharp",
	            "sie-",
	            "sonyericsson",
	            "symbian",
	            "windows ce",
	            "benq",
	            "mda",
	            "mot-",
	            "opera mini",
	            "philips",
	            "pocket pc",
	            "sagem",
	            "samsung",
	            "sda",
	            "sgh-",
	            "vodafone",
	            "xda",
	            "iphone",
                "ipad",
	            "android"
            ];

            userAgent = navigator.userAgent.toLowerCase();
            for (var i in mobileClients) {
                if (userAgent.indexOf(mobileClients[i]) != -1) {
                    return true;
                }
            }
            return false;
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
                // $get(ddlReasonVisitSecond_S1).selectedIndex = 0;
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
        
        if ( $find(cddReasonVisitFirst_S1) == null) {
            return;
        }
        
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


    function VoucherAmountAdd(elmAmount, intValue){
        intValue = parseInt(intValue, 10);
        
        if (parseInt(elmAmount.value, 10) == 0 || elmAmount.value == '')
        {
            elmAmount.value = Math.abs(intValue);
        } else {
            elmAmount.value = parseInt(elmAmount.value) + intValue;
        }
        
        if (parseInt(elmAmount.value, 10) <= Math.abs(intValue))
        {
            elmAmount.value = Math.abs(intValue);
        }
        
        //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        elmAmount.value = parseInt(elmAmount.value, 10) - (parseInt(elmAmount.value, 10) % intValue);
        //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        
        elmAmount.onchange();
    }
    
     var VoucherChangedFirstAmountOnChange = -1;
     
     function VoucherChanged(sender, elmRadioVoucher, elmVoucher, elmAmount, iAvailableVoucher , elmNumPadVoucherAmount, 
                                     elmNoticePopup, elmNoticePopupMsg, strInvalidVoucherAmount, intVoucherPrice){
        var radioMax = 5;
        //var intVoucherPrice = 50;
        var elm;
        var numOfVoucher = 0;
        if (sender == elmAmount){

                //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]  
                if ( parseInt(elmAmount.value, 10) % intVoucherPrice > 0 )
                //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                {
                    elmNoticePopup.hide();
                    elmNoticePopupMsg.innerHTML = strInvalidVoucherAmount.replace("%1", elmAmount.value);
                    elmNoticePopup.show();
                }

                
                numOfVoucher = parseInt(parseInt(elmAmount.value, 10) / intVoucherPrice);
                if (numOfVoucher > parseInt(iAvailableVoucher)) {
                    numOfVoucher = iAvailableVoucher;
                    elmAmount.value = iAvailableVoucher * intVoucherPrice;
                }
                
                if (numOfVoucher > radioMax){
                    if (elmRadioVoucher != null) {
                        elm = document.getElementById(elmRadioVoucher.id + "_" + radioMax );
                        if (elm != null){
                            elm.checked = true;
                            if (elmVoucher != null)
                            {
                                elmVoucher.value = numOfVoucher;
                                elmVoucher.readonly = false;
                                elmVoucher.disabled = false;
                                elmVoucher.style.backgroundColor  = 'white';
                            }
                        }
                    }
                } else {
                    if (elmRadioVoucher != null) {
                        if ( numOfVoucher > 0) {
                            elm = document.getElementById(elmRadioVoucher.id + "_" + (numOfVoucher - 1) )
                            if (elm != null)
                            {
                                elm.checked = true;
                            }
                            else
                            {
                                elmAmount.value = (numOfVoucher - 1) * intVoucherPrice;
                            }

                            if (elmVoucher != null)
                            {
                                elmVoucher.value = '';
                                elmVoucher.readonly = true;
                                elmVoucher.disabled = true;
                                elmVoucher.style.backgroundColor  = 'inactivecaptiontext';
                            }
                        }
                    }
                }
            
        } else if (sender == elmVoucher){
            if (parseInt(elmVoucher.value, 10) == 0 || elmVoucher.value == '') {
                elmAmount.value = '';
                /*elmVoucher.value = 1;
                elmAmount.value = intVoucherPrice;*/
                elmNoticePopupMsg.innerHTML = strInvalidVoucherAmount.replace("%1", elmAmount.value);
                elmNoticePopup.show();
            } else {
                elmAmount.value = parseInt(elmVoucher.value) * intVoucherPrice;
            }
        } else if (sender == elmRadioVoucher){
            var elmItem;
            var blnHandled = false;
            //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]  
            if (elmAmount != null)
            {
                for(var i = 0; i < 5; i++) {
                    elmItem = document.getElementById(elmRadioVoucher.id + "_" + i);
                    
		            if(elmItem.checked) {
		           
	                    elmAmount.value = (i+1) * intVoucherPrice;
	                    if (elmVoucher != null) {
	                        elmVoucher.value = '';
    		                
	                        elmVoucher.disabled = true;
	                        elmVoucher.readonly = true;
	                        elmVoucher.style.backgroundColor  = 'inactivecaptiontext';
	                    }
	                    blnHandled = true;
	                    break;
	               
		            }
	            }
	            if (!blnHandled) {
	                elmAmount.value = '';
                     if (elmVoucher != null) {
                        elmVoucher.value = '';
                        elmVoucher.readonly = false;
                        elmVoucher.disabled = false;
                        elmVoucher.style.backgroundColor  = 'white';
                     }
	            }
	        }
	        //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]  
	    }
	    //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen] 
	    if (elmNumPadVoucherAmount != null)
	    {
	        elmNumPadVoucherAmount.innerHTML = elmAmount.value;
	        try {
	        NumPacCalc(); // ucNumPad dynamic function
	        } catch (err) {alert(err.desctiption);};
	    }
	    //     CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen] 
   }
   
   // For ucNumPad.ascx
     var txtDisplayID;
     
     function roundNumber(num, dec) {
	    var result = Math.round(num*Math.pow(10,dec))/Math.pow(10,dec);
	    return result;
    }
    
  function SetNumPadRedeemVoucher(elmAmount, elmNumPadVoucherAmount){
      if (elmNumPadVoucherAmount != null)
	         {
	        elmNumPadVoucherAmount.innerHTML = elmAmount.value;
	             try {
	                    NumPacCalc(); // ucNumPad dynamic function
	                 } catch (err) {alert(err.desctiption);};
	         }       
                                     
  }

  function ResetScrollPosition() {
      setTimeout("window.scrollTo(0,0)", 0);
  }