<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPageNonLogin.master" CodeBehind="RecoverLogin.aspx.vb" Inherits="HCSP.RecoverLogin" 
    title="<%$ Resources:Title, RecoverLogin %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc1" %>

 <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script type="text/javascript">

       function changeIVRSPWD(chkbox) {

            var val = chkbox.checked
            if (val) {
                document.getElementById('<%=trIVRSPassword.ClientID%>').disabled = false;
                document.getElementById('<%=txt_Step3_newIVRPW.ClientID%>').disabled = false;
                document.getElementById('<%=txt_Step3_confirmIVRPW.ClientID%>').disabled = false;
                document.getElementById('<%=txt_Step3_newIVRPW.ClientID%>').removeAttribute("readOnly");
                document.getElementById('<%=txt_Step3_confirmIVRPW.ClientID%>').removeAttribute("readOnly");
                if (document.getElementById('<%=img_err_ivrsNewPW.ClientID%>') != null) {
                    document.getElementById('<%=img_err_ivrsNewPW.ClientID%>').style["display"] = "inline-block";
                }
                if (document.getElementById('<%=img_err_ivrsConfirmPW.ClientID%>') != null) {
                    document.getElementById('<%=img_err_ivrsConfirmPW.ClientID %>').style["display"] = "inline-block";
                }
            }
            else {
                document.getElementById('<%=trIVRSPassword.ClientID%>').disabled = true;
                document.getElementById('<%=txt_Step3_newIVRPW.ClientID%>').disabled = true;
                document.getElementById('<%=txt_Step3_confirmIVRPW.ClientID%>').disabled = true;
                document.getElementById('<%=txt_Step3_newIVRPW.ClientID%>').value = '';
                document.getElementById('<%=txt_Step3_confirmIVRPW.ClientID%>').value = '';
                if (document.getElementById('<%=img_err_ivrsNewPW.ClientID%>') != null) {
                    document.getElementById('<%=img_err_ivrsNewPW.ClientID %>').style["display"] = "none";
                }
                if (document.getElementById('<%=img_err_ivrsConfirmPW.ClientID%>') != null) {
                    document.getElementById('<%=img_err_ivrsConfirmPW.ClientID %>').style["display"] = "none";
                }
            }
            return false;
        }

    </script>

        &nbsp;
        <asp:Image ID="imgHeaderRecoverLogin" runat="server" AlternateText="<%$ Resources:AlternateText, RecoverLoginBanner%>"
            ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, RecoverLoginBanner%>" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">   
            <ContentTemplate>         
                <table style="width: 100%"  border="0" cellpadding="0" cellspacing="0">
                    <tr style="background-image: url(../Images/master/background.jpg); background-position: bottom; background-repeat: repeat-x; height: 546px" valign="top">
                        <td style="width: 10px">
                            </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr style="height:50px">
                                    <td>
                                        <asp:Panel ID="panStep1" runat="server" CssClass="highlightTimeline">
                                            <asp:Label ID="lblRecoverLoginTimeLine1" runat="server" Text="<%$ Resources:Text, RecoverLoginTimeLine1 %>"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Panel>
                                    </td>
                                    <td>
                                        <asp:Panel ID="panStep2" runat="server" CssClass="unhighlightTimeline">
                                            <asp:Label ID="lblRecoverLoginTimeLine2" runat="server" Text="<%$ Resources:Text, RecoverLoginTimeLine2 %>"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Panel>
                                    </td>                                    
                                    <td>
                                        <asp:Panel ID="panStep3" runat="server" CssClass="unhighlightTimeline">
                                            <asp:Label ID="lblRecoverLoginTimeLine3" runat="server" Text="<%$ Resources:Text, RecoverLoginTimeLine3 %>"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Panel>
                                    </td>
                                    <td>
                                        <asp:Panel ID="panStep4" runat="server" CssClass="unhighlightTimeline">
                                            <asp:Label ID="lblRecoverLoginTimeLine4" runat="server" Text="<%$ Resources:Text, RecoverLoginTimeLine4 %>"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</asp:Panel>
                                    </td>                                    
                                    <td style="width: 5px">
                                        &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>                                         
                            <cc2:MessageBox ID="udcErrorMessage" runat="server" />
                            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" />
                                                                       
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                          <tr>
                                    <td>                                    
                                        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                                            <asp:View ID="Step1" runat="server">
                                                <table style="width: 100%" cellspacing="0">
                                                    <tr>
                                                        <td valign="top" style="width: 950px">
                                                            <asp:Label ID="lblStep1TitleText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RecoverLoginStep1TitleText %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td valign="top" style="width: 200px; height: 30px">
                                                            <asp:Label ID="lbl_step1_SPID" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>" CssClass="tableTitle" Width="152px"></asp:Label></td>
                                                        <td valign="top">
                                                            &nbsp<asp:TextBox ID="txt_step1_SPID" runat="server" MaxLength="8" Width="70px"></asp:TextBox>
                                                            <asp:Image ID="img_err_spid" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                Style="position: absolute" Visible="False" />
                                                            <cc1:FilteredTextBoxExtender ID="filteredit_spID" runat="server" FilterType="Numbers"
                                                                TargetControlID="txt_step1_SPID"></cc1:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px" valign="top">
                                                            <asp:Label ID="lbl_step1_RegEmail" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RegisteredEmail %>"></asp:Label></td>
                                                        <td valign="top">
                                                            &nbsp<asp:TextBox ID="txt_step1_RegEmail" runat="server" MaxLength="50" Width="200px"></asp:TextBox><asp:Image ID="imgEmailAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Style="position: absolute" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr valign="top">
                                                        <td valign="middle" style="width: 200px">
                                                            <asp:Label ID="lbl_step1_tokenPIN" runat="server" Text="<%$ Resources:Text, PinNo %>" CssClass="tableTitle"></asp:Label></td>
                                                        <td valign="middle" style="height: 70px">
                                                            <table>
                                                                <tr>
                                                                    <td valign="middle">
                                                            <asp:TextBox ID="txt_step1_tokenPIN" runat="server" MaxLength="6" TextMode="Password" Width="70px"></asp:TextBox><asp:Image
                                                                ID="img_err_tokenPIN" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                Visible="False" />
                                                                        <cc1:FilteredTextBoxExtender ID="filteredit_tokenPIN" runat="server" FilterType="Numbers"
                                                                TargetControlID="txt_step1_tokenPIN" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                                    </td>
                                                                    <td valign="top">
                                                                        &nbsp; &nbsp;
                                                            <asp:Image ID="Image1" runat="server" ImageUrl="<%$ Resources:ImageUrl, Token %>" AlternateText="<%$ Resources:AlternateText, Token %>" ImageAlign="Bottom"/></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>                
                                                </table>                                                             
                                                <table style="width:100%">
                                                    <tr>
                                                        <td style="width: 250px">
                                                            <asp:ImageButton ID="btn_step1_cancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"/>
                                                        </td>
                                                        <td align="center">
                                                            <asp:ImageButton ID="btn_step1_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"/></td>
                                                    </tr>
                                                </table>                                                    
                                            </asp:View>
                                            <asp:View ID="Step2a" runat="server">
                                                <asp:Panel ID="panVerifyCode" runat="server" Visible="True" >
                                                   <table style="width:100%">                                                        
                                                        <tr>
                                                            <td valign="top" style="width: 950px">
                                                                <asp:Label ID="lblStep2aTitleText" runat="server" Width="100%" CssClass="tableTitle" 
                                                                    Text="<%$ Resources:Text, RecoverLoginStep2aTitleText %>"></asp:Label>
                                                            </td>                            
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <table style="width:100%">
                                                        <tr>
                                                            <td style="width: 230px;"  valign="top">
                                                                <asp:Label ID="lbl_step2a_VerCode" runat="server" CssClass="tableTitle"  Text="<%$ Resources:Text, VerificationCode %>"></asp:Label></td>
                                                            <td style="width: 280px"  valign="top">
                                                                <asp:TextBox ID="txt_step2a_VerCode" runat="server" MaxLength="7" Width="100px"></asp:TextBox>
                                                                <asp:Label ID="lbl_step2a_VerCode_RefTime" runat="server" CssClass="tableTitle" Text=""></asp:Label>
                                                                <asp:Image ID="imgVerCodeAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                   Style="position: absolute" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                            </td>
                                                            <td>                                                                
                                                                <asp:ImageButton ID="btn_step2a_Resend" runat="server" ImageUrl="<%$ Resources:ImageUrl, ResendBtn %>" AlternateText="<%$ Resources:AlternateText, ResendBtn %>"/>                                                                
                                                            </td>
                                                        </tr>
                                                        <tr id="trHKIC_Step2a" runat="server" visible ="false">
                                                            <td valign="top">                                                                
                                                                <asp:Label ID="lbl_step2a_RegHKICText" runat="server" Text="<%$ Resources:Text, SPHKID %>"
                                                                    CssClass="tableTitle"></asp:Label></td>
                                                            <td align="left" valign="top" colspan="2">
                                                                <asp:TextBox ID="txt_step2a_RegHKIDPrefix" runat="server" MaxLength="2" Width="28px" onblur="convertToUpper(this)"></asp:TextBox>
                                                                <asp:TextBox ID="txt_step2a_RegHKID" runat="server" Width="50px" MaxLength="6"></asp:TextBox>
                                                                (
                                                                <asp:TextBox ID="txt_step2a_RegHKIDdigit" runat="server" MaxLength="1" Width="14px" onblur="convertToUpper(this)"></asp:TextBox>
                                                                )
                                                                <asp:Image ID="imgHKIDAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height:60px">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table style="width:100%">
                                                        <tr>
                                                            <td style="width: 250px">
                                                                <asp:ImageButton ID="btn_step2a_cancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"/>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="btn_step2a_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>"/>
                                                            </td>
                                                        </tr>
                                                    </table> 
                                                </asp:Panel>
                                            </asp:View>
                                            <asp:View ID="Step2b" runat="server">                                                                                           
                                                <asp:Panel ID="panVerifyToken" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td valign="top" style="width: 950px;">
                                                                <asp:Label ID="lblStep2bTitleText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RecoverLoginStep2bTitleText %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <table style="width:100%">
                                                        <tr>
                                                            <td style="width: 230px; height:30px"  valign="top" >
                                                                <asp:Label ID="Label3" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, VerificationCode %>"></asp:Label>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="lblVerCodePass" runat="server" Font-Bold="true" Font-Size="16px" Text="<%$ Resources:Text, Pass %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trHKIC_Step2b" runat="server" visible ="false">                                                            
                                                            <td>                                                                
                                                                <asp:Label ID="lblHKIC" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SPHKID %>"></asp:Label>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="lblHKICPass" runat="server" Font-Bold="true" Font-Size="16px" Text="<%$ Resources:Text, Pass %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr valign="top">
                                                            <td valign="middle">
                                                                <asp:Label ID="lbl_Step2b_tokenPIN" runat="server" Text="<%$ Resources:Text, PinNo %>" CssClass="tableTitle"></asp:Label></td>
                                                            <td valign="middle" style="height: 40px">
                                                                <table>
                                                                    <tr>
                                                                        <td valign="middle">
                                                                            <asp:TextBox ID="txt_Step2b_tokenPIN" runat="server" MaxLength="6" TextMode="Password" Width="70px"></asp:TextBox><asp:Image
                                                                                ID="img_err_Step2b_tokenPIN" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                                Visible="False" />
                                                                            <cc1:FilteredTextBoxExtender ID="filteredit_Step2b_tokenPIN" runat="server" FilterType="Numbers"
                                                                                TargetControlID="txt_Step2b_tokenPIN" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                        <td valign="top">
                                                                            &nbsp; &nbsp;
                                                                            <asp:Image ID="Image4" runat="server" ImageUrl="<%$ Resources:ImageUrl, Token %>" AlternateText="<%$ Resources:AlternateText, Token %>" ImageAlign="Bottom"/>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>                    
                                                <table style="width:100%">
                                                    <tr>
                                                        <td style="width: 250px">
                                                            <asp:ImageButton ID="btn_step2b_cancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"/>
                                                        </td>
                                                        <td align="center">
                                                            <asp:ImageButton ID="btn_step2b_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>" />
                                                        </td>
                                                    </tr>
                                                </table>  
                                            </asp:View>       
                                            <!-- Change Password -->
                                            <asp:View ID="Step3" runat="server">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td valign="top" style="width: 950px;">
                                                            <asp:Label ID="lblStep3TitleText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RecoverLoginStep3TitleText %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 190px" colspan="3">
                                                            <asp:Label ID="lbl_Step3_ChgWebPW" runat="server" Style="font-weight: bold; font-size: 14px" Text="<%$ Resources:Text, ChgWebPWD %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>                                                   
                                                <table style="width:80% ; border:1px solid silver; padding:10px"  >
                                                    <tr>
                                                        <td valign="top" style="width: 200px; height: 40px">
                                                            <asp:Label ID="lbl_Step3_newPW" runat="server" Text="<%$ Resources:Text, NewWebPasswordText %>" CssClass="tableTitle"></asp:Label>
                                                        </td>
                                                        <td valign="top">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="txt_Step3_newPW" runat="server" TextMode="Password" MaxLength="20" Width="200px"></asp:TextBox>
                                                                        <asp:Image ID="img_err_webNewPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" />
                                                                    </td>
                                                                    <td valign="top">
                                                                        <table cellpadding="0">
                                                                            <tr>
                                                                                <td colspan="5" style="height: 18px; width: 230px;">
                                                                                    <div id="progressBar" style="font-size: 1px; height: 10px; border: 1px solid white;" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
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
                                                        <td valign="top" style="width: 200px; height: 24px;">
                                                            <asp:Label ID="lbl_Step3_confirmPW" runat="server" Text="<%$ Resources:Text, ConfirmWebPasswordText %>" CssClass="tableTitle"></asp:Label></td>
                                                        <td valign="top" style="height: 24px">
                                                            <asp:TextBox ID="txt_Step3_confirmPW" runat="server" TextMode="Password" MaxLength="20" Width="200px"></asp:TextBox>
                                                            <asp:Image ID="img_err_webConfirmPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                Visible="False" />                                    
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px" valign="top">
                                                        </td>
                                                        <td valign="top">
                                                            <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Text, WebPasswordTips %>"></asp:Label><br />
                                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:Text, WebPasswordTips1 %>"></asp:Label><br />
                                                        &nbsp; &nbsp;&nbsp;<asp:Label ID="Label32" runat="server" Text="<%$ Resources:Text, WebPasswordTips1a %>"></asp:Label><br />
                                                        &nbsp; &nbsp;&nbsp;<asp:Label ID="Label74" runat="server" Text="<%$ Resources:Text, WebPasswordTips1b %>"></asp:Label><br />
                                                        &nbsp; &nbsp;&nbsp;<asp:Label ID="Label75" runat="server" Text="<%$ Resources:Text, WebPasswordTips1c %>"></asp:Label><br />
                                                        &nbsp; &nbsp;&nbsp;<asp:Label ID="Label76" runat="server" Text="<%$ Resources:Text, WebPasswordTips1d %>"></asp:Label><br />
                                                        <asp:Label ID="Label77" runat="server" Text="<%$ Resources:Text, WebPasswordTips2 %>"></asp:Label><br />
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>                                                                                            
                                                <table style="width:100%;" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <asp:Panel ID="panIVRSPassword" runat="server">
                                                                <table style="width:100%">
                                                                   <tr>                                                               
                                                                        <td style="width: 190px;" colspan="3">
                                                                            <asp:Label ID="lbl_Step3_ChgIVRSPW" runat="server" Visible="False" Style="font-weight: bold; font-size: 14px" Text="<%$ Resources:Text, ChgIVRSPassword %>"></asp:Label>
                                                                            <asp:CheckBox ID="chkChgIVRSPwd" runat="server" Text="<%$ Resources:Text, ChgIVRSPassword %>"
                                                                                Visible="False" onclick="changeIVRSPWD(this)" Style="font-weight: bold" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table id="trIVRSPassword" runat="server" style="width:80% ; border:1px solid silver; padding:10px">
                                                                    <tr>
                                                                        <td style="width: 200px; height: 40px" valign="top">                                                            
                                                                            <asp:Label ID="lblNEWIVRSPwText" runat="server" Text="<%$ Resources:Text, NewIVRSPasswordText %>" CssClass="tableTitle"></asp:Label></td>
                                                                        <td valign="top">
                                                                            <asp:TextBox ID="txt_Step3_newIVRPW" runat="server" TextMode="Password" MaxLength="6" Width="100px" ></asp:TextBox>
                                                                            <asp:Image ID="img_err_ivrsNewPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                                Visible="False" />
                                                                            <cc1:FilteredTextBoxExtender ID="filteredit_IVRnewPw" runat="server" FilterType="Numbers"
                                                                                TargetControlID="txt_Step3_newIVRPW" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 200px;height: 24px " valign="top">
                                                                            <asp:Label ID="lblConfirmIVRSPWText" runat="server" Text="<%$ Resources:Text, ConfirmIVRSPasswordText %>" CssClass="tableTitle"></asp:Label></td>
                                                                        <td valign="top">
                                                                            <asp:TextBox ID="txt_Step3_confirmIVRPW" runat="server" TextMode="Password" MaxLength="6" Width="100px"></asp:TextBox>
                                                                            <asp:Image ID="img_err_ivrsConfirmPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                                Visible="False" />
                                                                            <cc1:FilteredTextBoxExtender ID="filteredit_IVRconfirmPw" runat="server" FilterType="Numbers"
                                                                                TargetControlID="txt_Step3_confirmIVRPW" Enabled="True"></cc1:FilteredTextBoxExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 200px" valign="top">                                                                            
                                                                        </td>
                                                                        <td valign="top">
                                                                            <asp:Label ID="lblIVRSPasswordTips" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips %>"></asp:Label><br />
                                                                            <asp:Label ID="lblIVRSPasswordTips1" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips1 %>"></asp:Label><br />
                                                                            <asp:Label ID="lblIVRSPasswordTips2" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips2 %>"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>                                                    
                                                </table>
                                                <br />
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 250px">
                                                            <asp:ImageButton ID="btn_step3_cancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"/>
                                                        </td>
                                                        <td valign="top" align="center">
                                                            <asp:ImageButton ID="btn_step3_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>" /></td>
                                                    </tr>
                                                </table>
                                            <cc1:FilteredTextBoxExtender ID="FilteredNewPassword" runat="server" TargetControlID="txt_Step3_newPW"
                                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;"></cc1:FilteredTextBoxExtender>
                                            <cc1:FilteredTextBoxExtender ID="FilteredNewPasswordConfirm" runat="server" TargetControlID="txt_Step3_confirmPW"
                                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;"></cc1:FilteredTextBoxExtender>
                                        </asp:View>
                                        <!-- Complete -->
                                        <asp:View ID="Step4" runat="server">
                                            <table style="width: 100%">
                                                        <tr>
                                                            <td valign="top" style="width: 950px;">
                                                                </td>                            
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" colspan="4">
                                                                </td>                            
                                                        </tr>
                        
                                                        <tr>
                                                            <td valign="top" colspan="4">
                                                                <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="<%$ Resources:Text, SecurityTipsTitle %>"
                                                                    Width="120px"></asp:Label>
                                                                <asp:Label ID="Label28" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SecurityTips %>" ForeColor="Red"></asp:Label>
                                                                </td>                            
                                                        </tr>
                        
                                                        <tr>
                                                            <td valign="top" colspan="4">&nbsp
                                                                </td>                            
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" valign="top">                                
                                                                <asp:Label ID="lbl_accAct_finalLink1" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, FinalLink1 %>"></asp:Label>&nbsp<asp:HyperLink ID="lbl_accAct_finalLink2" runat="server"  NavigateUrl="<%$ Resources:Text, FinalLink2 %>" Text="<%$ Resources:Text, FinalLink2 %>"></asp:HyperLink>&nbsp<asp:Label ID="lbl_accAct_finalLink3" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, FinalLink3 %>"></asp:Label></td>
                                                        </tr>                        
                                                    </table>
                                            <br />
                                        </asp:View>
                                    </asp:MultiView>
                                </td>
                            </tr>
                         </table>
                        </td>  
                    </tr>
                </table>
            <asp:Panel Style="display: none" ID="panPopupConfirmCancel" runat="server" Width="500px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpConfirm" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" 
                                 MessageText="<%$ Resources:Text, ConfirmCancelRecoverLogin %>"/>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupConfirmCancel" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupConfirmCancel" PopupControlID="panPopupConfirmCancel"
                PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupConfirmCancel" runat="server"></asp:Button>
              </ContentTemplate>
           </asp:UpdatePanel>
</asp:Content>

