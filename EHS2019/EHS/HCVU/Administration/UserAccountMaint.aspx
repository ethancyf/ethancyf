<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="UserAccountMaint.aspx.vb" Inherits="HCVU.UserAccountMaint" 
    title="<%$ Resources:Title, UserAccountMaint %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../JS/Common.js"></script>
    <script type="text/javascript">
    
        var hfRoleScrollTop_id = "<%=hfRoleScrollTop.ClientID%>";
        var pnlRole_id = "<%=pnlRole.ClientID%>";
        var hfFuncAccessRightScrollTop_id = "<%=hfFuncAccessRightScrollTop.ClientID%>";
        var pnlFuncAccessRight_id = "<%=pnlFuncAccessRight.ClientID%>";
        var hfFileGenerationRightScrollTop_id = "<%=hfFileGenerationRightScrollTop.ClientID%>";
        var pnlFileGenerationRight_id = "<%=pnlFileGenerationRight.ClientID%>";
        var panConfirmMsg_id = "<%=panConfirmMsg.ClientID%>";
        var hfFileGenerationRightScrollTop_id = "<%=hfFileGenerationRightScrollTop.ClientID%>";
        
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
    
        function pnlRoleOnScroll(){
            var obj = event.srcElement;
            document.getElementById(hfRoleScrollTop_id).value = obj.scrollTop;
        }
        
        function pnlRoleScroll(){
            var y = document.getElementById(hfRoleScrollTop_id).value;
            if (y != ''){
                setTimeout("document.getElementById(pnlRole_id).scrollTop = document.getElementById(hfRoleScrollTop_id).value", 1);
            }
        }
        
        function pnlRoleScrollReset(){
            setTimeout("document.getElementById(pnlRole_id).scrollTop = 0", 1);
        }
        
        function pnlFuncAccessRightOnScroll(){
            var obj = event.srcElement;
            document.getElementById(hfFuncAccessRightScrollTop_id).value = obj.scrollTop;        
        }
        
        function pnlFuncAccessRightScroll(){
            var y = document.getElementById(hfFuncAccessRightScrollTop_id).value;
            if (y != ''){
                setTimeout("document.getElementById(pnlFuncAccessRight_id).scrollTop = document.getElementById(hfFuncAccessRightScrollTop_id).value", 1);
            }
        }
        
        function pnlFuncAccessRightScrollReset(){
           setTimeout("document.getElementById(pnlFuncAccessRight_id).scrollTop = 0", 1);
        }
        
        function pnlFileGenerationRightOnScroll(){
            var obj = event.srcElement;
            document.getElementById(hfFileGenerationRightScrollTop_id).value = obj.scrollTop;        
        }
        
        function pnlFileGenerationRightScroll(){
            var y = document.getElementById(hfFileGenerationRightScrollTop_id).value;
            if (y != ''){
                setTimeout("document.getElementById(pnlFileGenerationRight_id).scrollTop = document.getElementById(hfFileGenerationRightScrollTop_id).value", 1);
            }
        }
        
        function pnlFileGenerationRightScrollReset(){
            setTimeout("document.getElementById(pnlFileGenerationRight_id).scrollTop = 0", 1);
        }
              
        function showResetPasswordConfirm(){
            var h = 122;
            var w = 600;
            var cw;
            var ch;
            
            var sx;
	        var sy;
	        
	        if (document.all){
	            cw = parseFloat(document.documentElement.clientWidth);
                ch = parseFloat(document.documentElement.clientHeight);
                //alert(document.body.scrollTop);
                sx = parseFloat(document.documentElement.scrollLeft);
	            sy = parseFloat(document.documentElement.scrollTop);
	        }
	        else{
	            cw = parseFloat(window.innerWidth);
                ch = parseFloat(window.innerHeight);
                //alert(document.body.scrollTop);
                //alert(document.documentElement.scrollTop);
                //document.body.scrollTop = 10;
                sx = parseFloat(document.documentElement.scrollLeft);
	            sy = parseFloat(document.documentElement.scrollTop);
	        }
	        
            var top = ch / 2 - 61 + sy / 2;
            var left = cw / 2 - 300 + sx / 2;
            document.getElementById(panConfirmMsg_id).style.visibility = 'visible';
            document.getElementById(panConfirmMsg_id).style.top = top + "px";
            //alert(document.getElementById(panConfirmMsg_id).style.top);
            document.getElementById(panConfirmMsg_id).style.left = left + "px";
            document.getElementById('ifmConfirmMsg').style.visibility = 'visible';
            document.getElementById('ifmConfirmMsg').style.top = top;
            document.getElementById('ifmConfirmMsg').style.left = left;
            
            document.getElementById('ifmTop').style.visibility = 'visible';
            document.getElementById('ifmRight').style.visibility = 'visible';
            document.getElementById('ifmBottom').style.visibility = 'visible';
            
            return false;
        }
        
        function hiddenResetPasswordConfirm(){
            document.getElementById(panConfirmMsg_id).style.visibility = 'hidden';
            document.getElementById('ifmConfirmMsg').style.visibility = 'hidden';
             
            document.getElementById('ifmTop').style.visibility = 'hidden';
            document.getElementById('ifmRight').style.visibility = 'hidden';
            document.getElementById('ifmBottom').style.visibility = 'hidden';
            
            return false;
        }
        
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfRoleScrollTop" runat="server" />
            <asp:HiddenField ID="hfFuncAccessRightScrollTop" runat="server" />
            <asp:HiddenField ID="hfFileGenerationRightScrollTop" runat="server" />
            <table cellpadding="0" cellspacing="0" style="width: 980px" border="0">
                <tr>
                    <td style="height: 60px; width: 980px;" valign="top"  colspan="2">
                        <asp:Image ID="imgHeader" runat="server" AlternateText="<%$Resources:AlternateText, UserAcctMaintBanner%>"
                            ImageAlign="AbsMiddle" ImageUrl="<%$Resources:ImageUrl, UserAcctMaintBanner%>" />
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Type="Complete" Width="780px" />
                        <cc2:MessageBox ID="udcMessageBox" runat="server" Width="780px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 488px" valign="top">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                        <tr style="height: 28px">
                                            <td>
                                                <asp:Label ID="lblUserListText" runat="server" Text="<%$Resources:Text, UserList%>" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:CheckBox ID="chkDisplayInactive" Text="<%$Resources:Text, InactiveUsers%>" runat="server" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddlUserList" Visible="true" runat="server" Width="380px" AutoPostBack="true" DataTextField="Display_Text" DataValueField="User_ID" Enabled="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 15px">
                                <td>
                                </td>
                            </tr>
                        </table> 
                        <asp:Label ID="lblUserInfoText" runat="server" Text="<%$Resources:Text, UserInfo%>" Font-Bold="true"></asp:Label>
                        <table style="width: 450px">
                            <tr style="height: 24px">
                                <td style="width: 120px">
                                    <asp:Label ID="lblLoginIDText" runat="server" Text="<%$Resources:Text, LoginID%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLoginID" runat="server" Visible="false" onblur="convertToUpper(this)" MaxLength="20"></asp:TextBox>
                                    <asp:Label ID="lblLoginID" runat="server"></asp:Label>
                                    <asp:Image ID="imgLoginIDAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>" style="position: absolute;"/>
                                </td>
                            </tr>
                            <tr style="height: 24px">
                                <td>
                                    <asp:Label ID="lblUserNameText" runat="server" Text="<%$Resources:Text, EnglishName%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox MaxLength="48" ID="txtSurname" Width="105px" runat="server" Visible="false" onblur="convertToUpper(this)" ToolTip="<%$Resources:Text, Surname%>"></asp:TextBox>
                                    <asp:Label ID="lblUserNameComma" runat="server" Text="," CssClass="tableText" Visible="false"></asp:Label>
                                    <asp:TextBox MaxLength="48" ID="txtGivenname" runat="server" Visible="false" onblur="convertToUpper(this)" ToolTip="<%$Resources:Text, Givenname%>"></asp:TextBox>
                                    <asp:TextBox ID="txtUserName" runat="server" Visible="false" onblur="convertToUpper(this)" ToolTip=""></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="filteredUserName" runat="server" TargetControlID="txtUserName"
                                        FilterType="Custom, LowercaseLetters, UppercaseLetters " ValidChars=",'.- ">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:Label ID="lblUserName" runat="server"></asp:Label>
                                    <asp:Image ID="imgUserNameAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>" style="position: absolute;"/>
                                </td>
                            </tr>
                            <!--Golden Add-->
                            <tr style="height: 24px">
                                <td>
                                    <asp:Label ID="lblChineseNameText" runat="server" Text="<%$Resources:Text, NameInChinese%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChineseName" runat="server" Visible="false" MaxLength="6"></asp:TextBox>        
                                    <asp:Label ID="lblChineseName" runat="server"></asp:Label>
                                    <asp:Image ID="imgChineseNameAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Style="position: absolute;" />
                                </td>
                            </tr>
                            <tr style="height: 24px">
                                <td>
                                    <asp:Label ID="lblHKIDText" runat="server" Text="<%$Resources:Text, HKID%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHKID" runat="server" Visible="false" MaxLength="11" onChange="formatHKID(this);"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="filteredHKID" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                        TargetControlID="txtHKID" ValidChars='()'>
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:Label ID="lblHKID" runat="server"></asp:Label>
                                    <asp:Image ID="imgHKIDAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>" style="position: absolute;"/>
                                </td>
                            </tr>
                            <!--Golden Add-->
                            <tr style="height: 24px">
                                <td>
                                    <asp:Label ID="lblGenderText" runat="server" Text="<%$Resources:Text, Gender%>"></asp:Label>
                                </td>
                                <td>                                    
                                    <asp:RadioButtonList id="rdlGender" runat="server" Visible="false" RepeatDirection="Horizontal" Style="position:relative;left:-7px;top:-2px;display:inline-block" >
                                    </asp:RadioButtonList>
                                    <asp:Label ID="lblGender" runat="server" Visible="false"></asp:Label>
                                    <asp:Image ID="imgGenderAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Style="position: absolute;" />
                                </td>
                            </tr>
                            <tr style="height: 24px">
                                <td>
                                    <asp:Label ID="lblContactNoText" runat="server" Text="<%$Resources:Text, ContactNo2%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContactNo" runat="server" Visible="false" MaxLength="20"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="filtertxtContactNo" runat="server" TargetControlID="txtContactNo"
                                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:Label ID="lblContactNo" runat="server" Visible="false"></asp:Label>
                                    <asp:Image ID="imgContactNoAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Style="position: absolute;" />
                                </td>
                            </tr>
                            <tr style="height: 24px">
                                <td>
                                    <asp:Label ID="lblEffectiveDateText" runat="server" Text="<%$Resources:Text, EffectiveDate%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEffectiveDate" runat="server" Visible="false" MaxLength="10" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="filterEffectiveDate" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEffectiveDate" ValidChars="-" />
                                    <asp:ImageButton ID="ibtnEffectiveDate" runat="server" Visible="false" AlternateText='<%$ Resources:AlternateText, CalenderBtn %>' ImageUrl='<%$ Resources:ImageUrl, CalenderBtn %>' />
                                    <cc1:CalendarExtender ID="calExtEffectiveDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnEffectiveDate" TargetControlID="txtEffectiveDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" />
                                    <asp:Label ID="lblEffectiveDate" runat="server"></asp:Label>
                                    <asp:Image ID="imgEffectiveDateAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>" style="position: absolute;"/>
                                </td>
                            </tr>
                            <tr style="height: 24px">
                                <td>
                                    <asp:Label ID="lblExpiryDateText" runat="server" Text="<%$Resources:Text, ExpiryDate%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExpiryDate" runat="server" Visible="false" MaxLength="10" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="filteredExpiryDate" runat="server" FilterType="Custom, Numbers" TargetControlID="txtExpiryDate" ValidChars="-" />
                                    <asp:ImageButton ID="ibtnExpiryDate" runat="server" Visible="false" AlternateText='<%$ Resources:AlternateText, CalenderBtn %>' ImageUrl='<%$ Resources:ImageUrl, CalenderBtn %>' />
                                    <cc1:CalendarExtender ID="calExtExpiryDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnExpiryDate" TargetControlID="txtExpiryDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" />
                                    <asp:Label ID="lblExpiryDate" runat="server"></asp:Label>
                                    <asp:Image ID="imgExpiryDateAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>" style="position: absolute;"/>
                                </td>
                            </tr>
                            <tr style="height: 24px">
                                <td>
                                    <asp:Label ID="lblTokenSNText" runat="server" Text="<%$Resources:Text, TokenSN%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTokenSN" runat="server" Visible="false" MaxLength="20"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTokenSN" runat="server" TargetControlID="txtTokenSN"
                                        FilterType="Numbers">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:Label ID="lblTokenSN" runat="server"></asp:Label>
                                    <asp:Image ID="imgTokenSNAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>" style="position: absolute;"/>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="ibtnDeActivateToken" runat="server" ImageUrl="<%$Resources:ImageUrl, DeactivateTokenSBtn%>" Visible="false" />
                                </td>
                            </tr>
                            <tr style="height: 24px">
                                <td>
                                    <asp:Label ID="lblSuspendedText" runat="server" Text="<%$Resources:Text, LoginStatus%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkSuspended" runat="server" Enabled="false" Text="<%$Resources:Text, Suspended%>" />
                                </td>
                            </tr>
                            <tr style="height: 24px">
                                <td>
                                    <asp:Label ID="lblAccountLocked" runat="server" Text="<%$Resources:Text, AccountLocked%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkAccountLocked" runat="server" Enabled="false" Text="<%$Resources:Text, Locked%>" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                
                                </td>
                            </tr>
                        </table>
                        
                        <table cellpadding="0" cellspacing="0" border="0">                            
                            <tr>
                                <td>
                                    <asp:Label ID="lblSchemeNameText" runat="server" Text="<%$Resources:Text, SchemeName %>" Font-Bold="true" Height="25px"></asp:Label>
                                    <span style="position: relative; top: -4px;">
                                        <asp:Image ID="imgSchemeAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>" style="position: absolute;"/>
                                    </span>
                                    <asp:Panel ID="panScheme" runat="server" BorderWidth="1px" ScrollBars="Auto" Height="50px" Width="380px">
                                        <asp:CheckBoxList ID="ckbScheme" runat="server" Enabled="false" RepeatDirection="Vertical">                                           
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr style="height: 28px">
                                <td valign="middle">                                    
                                    <asp:Label ID="lblRoleText" runat="server" Text="<%$Resources:Text, UserRole%>" Font-Bold="true"></asp:Label>
                                    <span style="position: relative; top: -4px;">
                                        <asp:Image ID="imgRoleAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>" style="position: absolute;"/>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlRole" runat="server" BorderWidth="1px" ScrollBars="Auto" Height="71px" Width="380px">
                                        <asp:CheckBoxList ID="chklRole" runat="server" Enabled="false" AutoPostBack="true" DataValueField="Role_Type" DataTextField="Role_Description">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" style="width: 500px">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr style="height: 28px">
                                <td>
                                    <asp:Label ID="lblFuncAccessRightText" runat="server" Text="<%$Resources:Text, AccessRightOnFunctions%>" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                <asp:Panel ID="pnlFuncAccessRight" runat="server" BorderWidth="1px" Height="250px" Width="510px" ScrollBars="auto">
                                    <asp:CheckBoxList ID="chklFuncAccessRight" runat="server" Enabled="false" DataTextField="Description" DataValueField="Function_Code">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                </td>
                            </tr>
                            <tr style="height: 15px">
                                <td></td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr style="height: 28px">
                                <td>
                                    <asp:Label ID="lblRptAccessRightText" runat="server" Text="<%$Resources:Text, AccessRightOnReports%>" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                <asp:Panel ID="pnlFileGenerationRight" runat="server" BorderWidth="1px" Height="222px" Width="510px" ScrollBars="auto">
                                    <asp:CheckBoxList id="chklFileGenerationRight" runat="server" Enabled="false" DataTextField="FileName" DataValueField="FileID">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:ImageButton ID="ibtnEdit" runat="server" ImageUrl="<%$Resources:ImageUrl, EditBtn%>" />
                        <asp:ImageButton ID="ibtnNew" runat="server" ImageUrl="<%$Resources:ImageUrl, NewBtn%>" />
                        <asp:ImageButton ID="ibtnResetPassword" runat="server" ImageUrl="<%$Resources:ImageUrl, ResetPasswordBtn%>" AlternateText="<%$Resources:AlternateText, ResetPasswordBtn%>" OnClientClick="" />
                        <asp:ImageButton ID="ibtnSave" runat="server" ImageUrl="<%$Resources:ImageUrl, SaveBtn%>" Visible="false" />
                        <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="<%$Resources:ImageUrl, CancelBtn%>" Visible="false" />
                    </td>
                </tr>
            </table>
            
            <iframe id="ifmTop" style="position: absolute; top: 0; left: 0; width: 990px; height: 200px; visibility: hidden; background-color: #FFFFFF; filter: alpha(opacity=0); opacity: 0; border: none 0 white;" src="javascript:false;"></iframe>
            <iframe id="ifmRight" style="position: absolute; top: 0px; left: 400px; width: 590px; height: 660px; visibility: hidden; background-color: #FFFFFF; filter: alpha(opacity=0); opacity: 0; border: none 0 white;" src="javascript:false;"></iframe>
            <iframe id="ifmBottom" style="position: absolute; top: 250px; left: 0px; width: 990px; height: 410px; visibility: hidden; background-color: #FFFFFF; filter: alpha(opacity=0); opacity: 0; border: none 0 white;" src="javascript:false;"></iframe>
            
            <iframe id="ifmConfirmMsg" style="top: 0px; left: 0px; position: absolute; width: 600px; height: 122px; visibility: hidden; background-color: #FFFFFF; filter: alpha(opacity=0); opacity: 0; border: none 0 white;" src="javascript:false;"></iframe>
                        
            <asp:Panel ID="panConfirmMsg" runat="server" Style="top: 0px; left: 0px; visibility: hidden; position: absolute;">
                <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: default;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                        </td>
                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                            <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                        </td>
                    </tr>
                    </table></asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px; background-color: #ffffff">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 9px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Text="Confirm to reset password?"></asp:Label></td>
                                    <td style="width: 40px">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnDialogConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" />
                                        <asp:ImageButton ID="ibtnDialogCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClientClick=""/></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 9px; height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                            height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                            height: 7px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
            <script type="text/javascript">
           
                function bodyOnScroll(){
                    if(document.getElementById('ifmConfirmMsg').style.visibility == 'visible'){
                        setTimeout('showResetPasswordConfirm()', 1);
                    }
                }
                //document.documentElement.onscroll = bodyOnScroll;
                window.onscroll = bodyOnScroll;     
            </script>
                                                                
        </ContentTemplate>        
    </asp:UpdatePanel>
    
    
</asp:Content>
