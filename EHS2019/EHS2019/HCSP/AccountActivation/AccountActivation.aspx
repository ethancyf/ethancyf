<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPageNonLogin.master" CodeBehind="AccountActivation.aspx.vb" Inherits="HCSP.AccountActivation" 
    title="<%$ Resources:Title, AccountActivation%>" %>
    
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
            
        .ChkEHRSSConsent {
            padding-left: 25px;
            padding-top: 5px;
            text-indent: -20px;
        }

    </style>

    <script type="text/javascript">
        // For popup of Get Username from eHRSS
        function changeEHRSSConsent(chkbox) {
            var val = chkbox.checked;

            if (val) {
                if (document.getElementById('<%=ibtnconsentconfirm.clientid %>') != null) {
                    document.getElementById('<%=ibtnconsentconfirm.clientid %>').style.display = "inline-block";
                }
                if (document.getElementById('<%=ibtnconsentconfirmdisable.clientid %>') != null) {
                    document.getElementById('<%=ibtnconsentconfirmdisable.clientid %>').style.display = "none";
                }
            }
            else {
                if (document.getElementById('<%=ibtnconsentconfirm.clientid %>') != null) {
                    document.getElementById('<%=ibtnconsentconfirm.clientid %>').style.display = "none";
                }
                if (document.getElementById('<%=ibtnconsentconfirmdisable.clientid %>') != null) {
                    document.getElementById('<%=ibtnconsentconfirmdisable.clientid %>').style.display = "inline-block";
                }
            }
            return false;
        }
    </script>

                    &nbsp;<asp:Image ID="imgHeaderAccountActivation" runat="server" AlternateText="<%$ Resources:AlternateText, AccountActivationBanner%>"
                        ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, AccountActivationBanner%>" />&nbsp;
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                    <table width="100%"  border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 10px">
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="panStep1" runat="server" CssClass="highlightTimeline">      
                                                <asp:Label ID="lblClaimVoucherStep1" runat="server" Text="<%$ Resources:Text, AccActTimeLine1 %>"></asp:Label>
                                              &nbsp;&nbsp;
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Panel ID="panStep2" runat="server" CssClass="unhighlightTimeline">
                                                <asp:Label ID="lblClaimVoucherStep2" runat="server" Text="<%$ Resources:Text, AccActTimeLine2 %>"></asp:Label>
                                                &nbsp;&nbsp;
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Panel ID="panStep3" runat="server" CssClass="unhighlightTimeline">
                                                <asp:Label ID="lblClaimVoucherStep3" runat="server" Text="<%$ Resources:Text, AccActTimeLine3 %>"></asp:Label>
                                                &nbsp;&nbsp;
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Panel ID="panStep4" runat="server" CssClass="unhighlightTimeline">
                                                <asp:Label ID="lblClaimVoucherStep4" runat="server" Text="<%$ Resources:Text,AccActTimeLine4 %>"></asp:Label>
                                                &nbsp;&nbsp;
                                            </asp:Panel>
                                        </td>                            
                                    </tr>
                                </table>
                                <cc2:MessageBox ID="udcErrorMessage" runat="server" Width="95%" />
                                <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="95%" />
                            </td>
                        </tr>
                        <tr style="background-image: url(../Images/master/background.jpg); background-position: bottom; background-repeat: repeat-x; height: 486px" valign="top">
                            <td style="width: 10px; height: 486px;">
                            </td>
                            <td style="height: 486px">
                                <asp:Label ID="lbl_accAct_status" runat="server" 
                                Text="<%$ Resources:Text, AccActStep1 %>" CssClass="tableCaption" Visible="False"></asp:Label>
                                <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="View1" runat="server">
                                        &nbsp;<asp:Label ID="lbl_accAct_step1_desc" runat="server" Text="<%$ Resources:Text, AccountInfo %>" CssClass="tableTitle" Visible="False"></asp:Label><br />
                                                    <table style="width: 100%">
                                                    <tr>
                                                        <td rowspan="3" style="width: 10px" valign="top">
                                                        </td>
                                                        <td valign="top" style="width: 200px">
                                                            &nbsp;<asp:Label ID="lbl_accAct_SPID" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>" CssClass="tableTitle" Width="152px"></asp:Label></td>
                                                        <td valign="top">
                                                            &nbsp<asp:TextBox ID="txt_accAct_SPID" runat="server" MaxLength="8" Width="70px"></asp:TextBox><asp:Image
                                                                ID="img_err_spid" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" />
                                                            <cc1:FilteredTextBoxExtender ID="filteredit_spID" runat="server" FilterType="Numbers"
                                                                TargetControlID="txt_accAct_SPID">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr valign="top">
                                                        <td valign="middle" style="width: 200px">
                                                            &nbsp;<asp:Label ID="lbl_accAct_tokenPIN" runat="server" Text="<%$ Resources:Text, PinNo %>" CssClass="tableTitle"></asp:Label></td>
                                                        <td valign="middle" style="height: 70px">
                                                            <table>
                                                                <tr>
                                                                    <td valign="middle">
                                                            <asp:TextBox ID="txt_accAct_tokenPIN" runat="server" MaxLength="6" TextMode="Password" Width="70px"></asp:TextBox><asp:Image
                                                                ID="img_err_tokenPIN" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                Visible="False" />
                                                                        <cc1:FilteredTextBoxExtender ID="filteredit_tokenPIN" runat="server" FilterType="Numbers"
                                                                TargetControlID="txt_accAct_tokenPIN" Enabled="True">
                                                                        </cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                    <td valign="top">
                                                                        &nbsp;&nbsp;
                                                            <asp:Image ID="Image1" runat="server" ImageUrl="<%$ Resources:ImageUrl, Token %>" AlternateText="<%$ Resources:AlternateText, Token %>" ImageAlign="Bottom"/></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                        <tr valign="top">                                
                                                            <td valign="middle" colspan="2" align="center">
                                                            <asp:ImageButton ID="btn_accAct_step1_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>"/></td>
                                                        </tr>
                                                </table>
                                        <br />
                                    </asp:View>
                                    <asp:View ID="View2" runat="server">
                                        &nbsp;<asp:Label ID="lbl_accAct_step2_desc" runat="server" Text="<%$ Resources:Text, AccActLabel2-1 %>" CssClass="tableTitle"></asp:Label><br />
                                        <br />
                                                        
                                                    <table style="width: 100%">
                                                    <tr>
                                                        <td rowspan="3" style="width: 10px" valign="top">
                                                        </td>
                                                        <td valign="top" style="width: 200px">
                                                            &nbsp;<asp:Label ID="Label20" runat="server" Text="<%$ Resources:Text, AccAlias %>" CssClass="tableTitle" Width="176px"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txt_accAct_HKID_prefix" runat="server" onChange="convertToUpper(this);" MaxLength="2" Width="25px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="filteredit_patshkid" runat="server" FilterType="UppercaseLetters, LowercaseLetters"
                                                                TargetControlID="txt_accAct_HKID_prefix">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:TextBox ID="txt_accAct_HKID_first3" runat="server" MaxLength="3" Width="30px"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="filteredit_HKIDfirst3" runat="server" FilterType="Numbers"
                                                                TargetControlID="txt_accAct_HKID_first3" Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:Label ID="Label18" runat="server" CssClass="tableTitle" Text="XXX (X)"></asp:Label><asp:Image
                                                                ID="img_err_hkid" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" style="width: 200px">
                                                            &nbsp;<asp:Label ID="Label22" runat="server" Text="<%$ Resources:Text, AccAlias %>" CssClass="tableTitle"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txt_accAct_phone" runat="server" MaxLength="10" Width="80px"></asp:TextBox><asp:Image
                                                                ID="img_err_contact" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                Visible="False" />
                                                            <cc1:FilteredTextBoxExtender ID="filteredit_contactno" runat="server" FilterType="Numbers"
                                                                TargetControlID="txt_accAct_phone">
                                                            </cc1:FilteredTextBoxExtender>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" style="width: 123px">
                                                        </td>
                                                        <td valign="top">
                                                            <asp:ImageButton ID="btn_accAct_step2_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>" /></td>
                                                    </tr>
                                                </table>
                                        <br />
                                    </asp:View>        
                                    <asp:View ID="View3" runat="server" EnableViewState="False">
                                                            
                                                        <asp:Label ID="lbl_accAct_step4_desc" runat="server" Text="<%$ Resources:Text, AccActLabel2-1 %>" CssClass="tableTitle" Visible="False"></asp:Label>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="82%">
                                                        <tr>
                                                            <td style="text-align: justify">
                                                            <asp:Label ID="lbl_accAct_step7_desc" runat="server" CssClass="tableText" Text="<%$ Resources:Text, AccActLabel2-2 %>"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                        <br />
                                        <table>
                                            <tr>
                                                <td style="width: 210px" valign="top">
                                                                <asp:Label ID="lbl_accAct_accAlias" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccAlias %>"></asp:Label></td>
                                                <td colspan="2" align="left" valign="top" style="width: 700px">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left" valign="top" >
                                                                <asp:TextBox ID="txt_accAlias" runat="server" Width="200px" onChange="convertToUpper(this);"></asp:TextBox><asp:Image
                                                                    ID="img_err_webalias" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                    Visible="False" ImageAlign="AbsMiddle" />
                                                            </td>
                                                            <td align="left" style="width: 10px" valign="top">
                                                            </td>
                                                            <td style="width:180px" valign="top">
                                                                <asp:ImageButton ID="btn_checkAvailability" runat="server" ImageUrl="<%$ Resources:ImageUrl, CheckAvailabilityBtn %>" AlternateText="<%$ Resources:AlternateText, CheckAvailabilityBtn %>" /></td>
                                                        </tr>
                                                        <tr id="trGetUsernameFromEHRSS" runat="server">
                                                            <td></td>
                                                            <td></td>
                                                            <td style="padding-top:4px;">
                                                                <asp:ImageButton ID="btnGetUsernameFromEHRSS" runat="server" ImageUrl="<%$ Resources: ImageUrl, GetUsernameFromEHRSSBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, GetUsernameFromEHRSSBtn %>" ImageAlign="AbsMiddle" OnClick="btnGetUsernameFromEHRSS_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 210px" valign="top">
                                                </td>
                                                <td colspan="2" align="left" valign="top">
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Text, UsernameTips %>"></asp:Label><br />
                                                    &nbsp;<asp:Label ID="Label8" runat="server" Text="<%$ Resources:Text, UsernameTips1 %>"></asp:Label><br />
                                                    &nbsp;<asp:Label ID="Label3" runat="server" Text="<%$ Resources:Text, UsernameTips2 %>"></asp:Label><br />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label4" runat="server" Text="<%$ Resources:Text, UsernameTips2a %>"></asp:Label><br />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label6" runat="server" Text="<%$ Resources:Text, UsernameTips2b %>"></asp:Label><br />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label7" runat="server" Text="<%$ Resources:Text, UsernameTips2c %>"></asp:Label><br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" valign="top">
                                                    <asp:ImageButton ID="btn_accAct_step3_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>" Visible="False" /><br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 210px; height: 40px;" valign="top">
                                                            <asp:Label ID="lbl_accAct_newPW" runat="server" Text="<%$ Resources:Text, NewPassword %>" CssClass="tableTitle"></asp:Label></td>
                                                <td colspan="2" style="height: 40px" align="left" valign="top">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="txt_accAct_newPW" runat="server" TextMode="Password" MaxLength="20" Width="200px"></asp:TextBox>
                                                                        <asp:Image ID="img_err_webNewPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" ImageAlign="Top" />
                                                                    </td>
                                                                    <td valign="top">
                                                                        <table style="width: 300px" cellpadding="0">
                                                                            <tr>
                                                                                <td colspan="5">
                                                                                    <div id="progressBar" style="font-size: 1px; height: 10px; width: 290px; border: 1px solid white;" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="width: 300px">
                                                                                <td align="center" style="width:30%">
                                                                                    <span id="strength1"></span>                                           
                                                                                </td>
                                                                                <td style="width:5%">
                                                                                    <span id="direction1"></span>
                                                                                </td>
                                                                                <td align="center" style="width:30%">
                                                                                    <span id="strength2"></span>
                                                                                </td>
                                                                                <td style="width:5%">
                                                                                    <span id="direction2"></span>
                                                                                </td>
                                                                                <td align="center" style="width:30%">
                                                                                    <span id="strength3"></span>
                                                                                </td>                                        
                                                                            </tr>
                                                                        </table>                                       
                                                                    </td>
                                                                </tr>
                                                            </table>                        
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 210px; height: 40px;" valign="top">
                                                            <asp:Label ID="lbl_accAct_confirmPW" runat="server" Text="<%$ Resources:Text, ConfirmPW %>" CssClass="tableTitle"></asp:Label></td>
                                                <td colspan="2" style="height: 40px" align="left" valign="top">
                                                            <asp:TextBox ID="txt_accAct_confirmPW" runat="server" TextMode="Password" MaxLength="20" Width="200px"></asp:TextBox>
                                                    <asp:Image
                                                                ID="img_err_webConfirmPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                Visible="False" ImageAlign="Top" /></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 210px" valign="top">
                                                </td>
                                                <td colspan="2" align="left" valign="top">
                                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Text, WebPasswordTips %>"></asp:Label><br />
                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Text, WebPasswordTips1 %>"></asp:Label><br />
                                                    &nbsp;
                                                            &nbsp; &nbsp;&nbsp;<asp:Label ID="Label32" runat="server" Text="<%$ Resources:Text, WebPasswordTips1a %>"></asp:Label><br />
                                                    &nbsp;
                                                            &nbsp; &nbsp;&nbsp;<asp:Label ID="Label74" runat="server" Text="<%$ Resources:Text, WebPasswordTips1b %>"></asp:Label><br />
                                                    &nbsp;
                                                            &nbsp; &nbsp;&nbsp;<asp:Label ID="Label75" runat="server" Text="<%$ Resources:Text, WebPasswordTips1c %>"></asp:Label><br />
                                                    &nbsp;
                                                            &nbsp; &nbsp;&nbsp;<asp:Label ID="Label76" runat="server" Text="<%$ Resources:Text, WebPasswordTips1d %>"></asp:Label><br />
                                                            <asp:Label ID="LabelWebPasswordTips2" runat="server" Text="<%$ Resources:Text, WebPasswordTips2 %>"></asp:Label><br />
                                                            <asp:Label ID="LabelWebPasswordTips3" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>"></asp:Label></td>
                                            </tr>
                                            <tr>

                                                <td colspan="3" align="center" valign="top">
                                                    <br />
                                                            <asp:ImageButton ID="btn_accAct_step4_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>" /></td>
                                            </tr>
                                        </table>
                                        <cc1:FilteredTextBoxExtender ID="FilteredAccUserName" runat="server" TargetControlID="txt_accAlias"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredAccNewPassword" runat="server" TargetControlID="txt_accAct_newPW"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredAccConfirmPassword" runat="server" TargetControlID="txt_accAct_confirmPW"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>

                                    </asp:View>
                                    <asp:View ID="View4" runat="server">
                                        &nbsp;<table border="0" cellpadding="0" cellspacing="0" width="82%">
                                            <tr>
                                                <td style="text-align: justify">
                                        <asp:Label ID="lbl_accAct_step5_desc" runat="server" CssClass="tableText" Text="<%$ Resources:Text, AccActLabel3 %>"></asp:Label></td>
                                            </tr>
                                        </table>
                                        <br />
                                            <table style="width: 100%">
                                              
                                                    <tr>
                                                        <td valign="top" style="width: 200px">
                                                            &nbsp;<asp:Label ID="Label15" runat="server" Text="<%$ Resources:Text, NewPassword %>" CssClass="tableTitle"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txt_accAct_newIVRPW" runat="server" TextMode="Password" MaxLength="6" Width="100px"></asp:TextBox><asp:Image
                                                                ID="img_err_ivrsNewPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                Visible="False" />
                                                            <cc1:FilteredTextBoxExtender ID="filteredit_IVRnewPw" runat="server" FilterType="Numbers"
                                                                TargetControlID="txt_accAct_newIVRPW" Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" style="width: 200px;">
                                                            &nbsp;<asp:Label ID="Label19" runat="server" Text="<%$ Resources:Text, ConfirmPW %>" CssClass="tableTitle"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:TextBox ID="txt_accAct_confirmIVRPW" runat="server" TextMode="Password" MaxLength="6" Width="100px"></asp:TextBox><asp:Image
                                                                ID="img_err_ivrsConfirmPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                Visible="False" />
                                                            <cc1:FilteredTextBoxExtender ID="filteredit_IVRconfirmPw" runat="server" FilterType="Numbers"
                                                                TargetControlID="txt_accAct_confirmIVRPW" Enabled="True">
                                                            </cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                <tr>
                                                    <td style="width: 200px" valign="top">
                                                    </td>
                                                    <td valign="top"><br/>
                                                        <asp:Label ID="Label78" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips %>"></asp:Label><br />
                                                        <asp:Label ID="Label79" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips1 %>"></asp:Label><br />
                                                        <asp:Label ID="Label80" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips2 %>"></asp:Label><br />        
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips3 %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                    <tr>
                                                        <td colspan="2" valign="top">
                                                        </td>
                                                    </tr>
                                                    </table>                        
                                                            
                                                    <table style="width: 100%">                        
                                                    <tr>
                                                        <td valign="top" align="center">
                                                            <asp:ImageButton ID="btn_accAct_step5_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>" />
                                                            <asp:ImageButton ID="btn_skipIVRS" runat="server" ImageUrl="<%$ Resources:ImageUrl, SkipBtn %>" AlternateText="<%$ Resources:AlternateText, SkipBtn %>" /></td>
                                                    </tr>    
                                        </table>
                                        <br />
                                    </asp:View>
                                    <asp:View ID="View5" runat="server">
                                        &nbsp;<asp:Label ID="lbl_accAct_step3_desc" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccAlias %>"></asp:Label><br /><br />
                                            
                                            
                                        <br />
                                    </asp:View>
                                    <asp:View ID="View6" runat="server">
                                        <table style="width: 85%">
                                                    <tr>
                                                        <td valign="top" colspan="4">
                                                            <asp:Label ID="lbl_accAct_step6_desc" runat="server" Text="<%$ Resources:Text, AccAlias %>" CssClass="tableTitle" Visible="False"></asp:Label></td>                            
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td valign="top" colspan="4">
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="<%$ Resources:Text, SecurityTipsTitle %>"></asp:Label>
                                                                        <asp:Label ID="Label28" runat="server" ForeColor="Red" Text="<%$ Resources:Text, SecurityTips %>"></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                            &nbsp;
                                                            </td>                            
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td valign="top" colspan="4">&nbsp
                                                            </td>                            
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" valign="top">                                &nbsp;<asp:Label ID="lbl_accAct_finalLink1" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, FinalLink1 %>"></asp:Label>&nbsp<asp:HyperLink ID="lbl_accAct_finalLink2" runat="server"  NavigateUrl="<%$ Resources:Text, FinalLink2 %>" Text="<%$ Resources:Text, FinalLink2 %>"></asp:HyperLink>&nbsp<asp:Label ID="lbl_accAct_finalLink3" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, FinalLink3 %>"></asp:Label></td>
                                                    </tr>                        
                                                </table>
                                        <br />
                                    </asp:View>
                                    <asp:View ID="View7" runat="server">
                                    
                                    </asp:View>
                                </asp:MultiView>
                            </td>
                        </tr>                   
                    </table>
                    <%-- Consent of Get Username from eHRSS--%>
                    <asp:Panel ID="panGetUsernameFromeHRSS" runat="server" Style="display: none;">
                        <asp:Panel ID="panGetUsernameFromeHRSSHeading" runat="server" Style="cursor: move;">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                                    <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                        <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, GetUsernameFromEHRSSTitle %>"></asp:Label></td>
                                    <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                                <td style="background-color: #ffffff">
                                    <table style="width: 100%">
                                        <tr>
                                            <td align="left" style="padding-left:10px; height: 60px" valign="middle">
                                                <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/information.png" /></td>
                                            <td style="height: 60px; text-align: justify">
                                                <asp:CheckBox ID="chkGetUsernameFromEHRSS" runat="server" Text="<%$ Resources: Text, GetUsernameFromEHRSSConsent %>"
                                                    Width="380px" class="ChkEHRSSConsent" onclick="changeEHRSSConsent(this);"></asp:CheckBox></td>
                                        </tr>
                                        <tr style="height: 10px"></tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:ImageButton ID="ibtnConsentCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnConsentCancel_Click" />
                                                <asp:ImageButton ID="ibtnConsentConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnConsentConfirm_Click" style="display:none" />
                                                <asp:ImageButton ID="ibtnConsentConfirmDisable" runat="server" 
                                                    ImageUrl="<%$ Resources:ImageUrl, ConfirmDisableBtn %>" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                    Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr style="height: 5px"></tr>
                                    </table>
                                </td>
                                <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                            </tr>
                            <tr>
                                <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                                <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                                <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                            </tr>
                        </table>
                        <asp:Button ID="btnGetUsernameFromEHRSSDummy" runat="server" Style="display: none" />
                    </asp:Panel>
                    <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmConsent" runat="server" TargetControlID="btnGetUsernameFromEHRSSDummy"
                        PopupControlID="panGetUsernameFromeHRSS" BehaviorID="mdlPopup" BackgroundCssClass="modalBackgroundTransparent"
                        DropShadow="False" RepositionMode="None"  PopupDragHandleControlID="panGetUsernameFromeHRSSHeading">
                    </cc1:ModalPopupExtender>
                    <%-- End of Consent of Get Username from eHRSS--%>
                        </ContentTemplate>
                </asp:UpdatePanel>

</asp:Content>
