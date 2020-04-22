<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPageNonLogin.master" CodeBehind="ForgotPassword.aspx.vb" Inherits="HCSP.ForgotPassword" 
    title="<%$ Resources:Title, ForgotPassword %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">              
                    &nbsp;
                    <asp:Image ID="imgHeaderAccountActivation" runat="server" AlternateText="<%$ Resources:AlternateText, ForgotPasswordBanner%>"
                        ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ForgotPasswordBanner%>" />
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>                 
                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                        <tr style="background-image: url(../Images/master/background.jpg); background-position: bottom; background-repeat: repeat-x; height: 546px" valign="top">
                            <td style="width: 10px">
                            </td>
                            <td style="width: 917px">             
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Panel ID="panStep1" runat="server" CssClass="highlightTimeline">
                                    <asp:Label ID="lblClaimVoucherStep1" runat="server" Text="<%$ Resources:Text, ForgotPasswordTimeLine1 %>"></asp:Label>&nbsp;</asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="panStep2" runat="server" CssClass="unhighlightTimeline">
                                    <asp:Label ID="lblClaimVoucherStep2" runat="server" Text="<%$ Resources:Text, ForgotPasswordTimeLine2 %>"></asp:Label>&nbsp;</asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="panStep3" runat="server" CssClass="unhighlightTimeline">
                                    <asp:Label ID="lblClaimVoucherStep3" runat="server" Text="<%$ Resources:Text, ForgotPasswordTimeLine3 %>"></asp:Label>&nbsp;</asp:Panel>
                            </td>
                            <td style="width: 5px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                    <cc2:MessageBox ID="udcErrorMessage" runat="server" />
                        <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" />
                                    
                                    
    <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="Step1" runat="server">
            <table style="width: 100%">
                <tr>
                    <td valign="top" colspan="4" style="width: 100%">
                    </td>                            
                </tr>
            </table>
            <br />
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td valign="top" style="width: 200px">
                        <asp:Label ID="lbl_accAct_SPID" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>" CssClass="tableTitle" Width="152px"></asp:Label></td>
                    <td valign="top">
                        &nbsp<asp:TextBox ID="txt_accAct_SPID" runat="server" MaxLength="8" Width="70px" BackColor="White" BorderColor="LightSteelBlue" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox><asp:Image
                            ID="img_err_spid" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" />
                        <cc1:FilteredTextBoxExtender ID="filteredit_spID" runat="server" FilterType="Numbers"
                            TargetControlID="txt_accAct_SPID">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                </tr>
                <tr valign="top">
                    <td valign="middle" style="width: 200px">
                        <asp:Label ID="lbl_accAct_tokenPIN" runat="server" Text="<%$ Resources:Text, PinNo %>" CssClass="tableTitle"></asp:Label></td>
                    <td valign="middle" style="height: 70px">
                        <table>
                            <tr>
                                <td valign="middle">
                        <asp:TextBox ID="txt_accAct_tokenPIN" runat="server" MaxLength="6" TextMode="Password" Width="70px" BackColor="White" BorderColor="LightSteelBlue" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox><asp:Image
                            ID="img_err_tokenPIN" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                            Visible="False" />
                                    <cc1:FilteredTextBoxExtender ID="filteredit_tokenPIN" runat="server" FilterType="Numbers"
                            TargetControlID="txt_accAct_tokenPIN" Enabled="True">
                                    </cc1:FilteredTextBoxExtender>
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
                    <td align="center">
                        <asp:ImageButton ID="btn_accAct_step1_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>"/></td>
                </tr>
            </table>
            
            </asp:View>
        
        <asp:View ID="Step2" runat="server" EnableViewState="False">
            <table style="width: 100%">
                        <tr>
                            <td valign="top" colspan="4">
                                <asp:Label ID="lblResetPasswordTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ResetPasswordStep2TitleText %>"></asp:Label></td>                            
                        </tr>
                        </table>
                        <br />
                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                        
                        <tr>
                            <td valign="top" style="width: 200px; height: 40px">
                                <asp:Label ID="lbl_accAct_newPW" runat="server" Text="<%$ Resources:Text, NewWebPasswordText %>" CssClass="tableTitle"></asp:Label>
                            </td>
                            <td valign="top">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:TextBox ID="txt_accAct_newPW" runat="server" TextMode="Password" MaxLength="20" Width="200px" BackColor="White" BorderColor="LightSteelBlue" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox><asp:Image ID="img_err_webNewPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" />
                                        </td>
                                        <td valign="top">
                                            <table style="width: 290px" cellpadding="0">
                                                <tr>
                                                    <td colspan="5" style="height: 28px">
                                                        <div id="progressBar" style="font-size: 1px; height: 10px; width: 290px; border: 1px solid white;" />
                                                    </td>
                                                </tr>
                                                <tr style="width: 290px">
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
                            <td valign="top" style="width: 200px; height: 36px;">
                                <asp:Label ID="lbl_accAct_confirmPW" runat="server" Text="<%$ Resources:Text, ConfirmWebPasswordText %>" CssClass="tableTitle"></asp:Label></td>
                            <td valign="top" style="height: 36px">
                                <asp:TextBox ID="txt_accAct_confirmPW" runat="server" TextMode="Password" MaxLength="20" Width="200px" BackColor="White" BorderColor="LightSteelBlue" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox><asp:Image
                                    ID="img_err_webConfirmPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
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
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px;" valign="top">
                                </td>
                                <td height="20" valign="top">
                                </td>
                            </tr>
                            <asp:Panel ID="panIVRSPassword" runat="server">
                            <tr>
                                <td style="width: 200px; height: 40px" valign="top">
                                <asp:Label ID="lblNEWIVRSPwText" runat="server" Text="<%$ Resources:Text, NewIVRSPasswordText %>" CssClass="tableTitle"></asp:Label></td>
                                <td valign="top">
                                <asp:TextBox ID="txt_accAct_newIVRPW" runat="server" TextMode="Password" MaxLength="6" Width="100px" BackColor="White" BorderColor="LightSteelBlue" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox><asp:Image
                                    ID="img_err_ivrsNewPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                    Visible="False" />
                                <cc1:FilteredTextBoxExtender ID="filteredit_IVRnewPw" runat="server" FilterType="Numbers"
                                    TargetControlID="txt_accAct_newIVRPW" Enabled="True">
                                </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px;height: 40px " valign="top">
                                <asp:Label ID="lblConfirmIVRSPWText" runat="server" Text="<%$ Resources:Text, ConfirmIVRSPasswordText %>" CssClass="tableTitle"></asp:Label></td>
                                <td valign="top">
                                <asp:TextBox ID="txt_accAct_confirmIVRPW" runat="server" TextMode="Password" MaxLength="6" Width="100px" BackColor="White" BorderColor="LightSteelBlue" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox><asp:Image
                                    ID="img_err_ivrsConfirmPW" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                    Visible="False" />
                                <cc1:FilteredTextBoxExtender ID="filteredit_IVRconfirmPw" runat="server" FilterType="Numbers"
                                    TargetControlID="txt_accAct_confirmIVRPW" Enabled="True">
                                </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:TextBox ID="txtIVRSEnabled" runat="server" Visible="true" style="display:none" BackColor="White" BorderColor="LightSteelBlue" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox></td>
                                <td valign="top">
                                    <asp:Label ID="lblIVRSPasswordTips" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips %>"></asp:Label><br />
                                    <asp:Label ID="lblIVRSPasswordTips1" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips1 %>"></asp:Label><br />
                                    <asp:Label ID="lblIVRSPasswordTips2" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips2 %>"></asp:Label>
                                </td>
                            </tr>
                            </asp:Panel>
                        </table>
                        
                                <br />
                                

                        
                        <table style="width: 100%">
                        <tr>
                            <td valign="top" align="center">
                                <asp:ImageButton ID="btn_accAct_step4_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>" /></td>
                        </tr>
                    </table>

                    <cc1:FilteredTextBoxExtender ID="FilteredNewPassword" runat="server" TargetControlID="txt_accAct_newPW"
                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                    </cc1:FilteredTextBoxExtender>
                    <cc1:FilteredTextBoxExtender ID="FilteredNewPasswordConfirm" runat="server" TargetControlID="txt_accAct_confirmPW"
                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                    </cc1:FilteredTextBoxExtender>
            <br />

        </asp:View>        
        <asp:View ID="Step3" runat="server">
            <table style="width: 100%">
                        <tr>
                            <td valign="top" colspan="4">
                                </td>                            
                        </tr>
                        <tr>
                            <td valign="top" colspan="4">
                                </td>                            
                        </tr>
                        
                        <tr>
                            <td valign="top" colspan="4">
                                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="<%$ Resources:Text, SecurityTipsTitle %>"
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
        
        <asp:View ID="StepIVRS" runat="server">
            <table style="width: 100%">
                        <tr>
                            <td valign="top" colspan="4">
                        <asp:Label ID="Label73" runat="server" Text="<%$ Resources:Text, AccActTimeLine4 %>" Font-Bold="True" Font-Size="Large" ForeColor="#FF8000"></asp:Label></td>                            
                        </tr>
                 </table>
                        
                                    
                <asp:Label ID="lbl_accAct_step5_desc" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccActLabel3 %>"></asp:Label><br />
                <br />
                <table style="width: 100%">
                  
                        <tr>
                            <td valign="top" style="width: 200px">
                                </td>
                            <td valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="width: 200px;">
                                </td>
                            <td valign="top">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top">
                            </td>
                        </tr>
                        </table>                        
                                <asp:Label ID="Label78" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips %>"></asp:Label><br />
                                <asp:Label ID="Label79" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips1 %>"></asp:Label><br /><asp:Label ID="Label80" runat="server" Text="<%$ Resources:Text, IVRSPasswordTips2 %>"></asp:Label>
                        <table style="width: 100%">                        
                        <tr>
                            <td valign="top" align="center">
                                <asp:ImageButton ID="btn_accAct_step5_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>" />
                                <asp:ImageButton ID="btn_skipIVRS" runat="server" ImageUrl="<%$ Resources:ImageUrl, SkipBtn %>" AlternateText="<%$ Resources:AlternateText, SkipBtn %>" /></td>
                        </tr>    
            </table>
            <br />
        </asp:View>  
        <asp:View ID="StepInvalidLink" runat="Server">
        </asp:View>     
    </asp:MultiView>
                        </td>
                        </tr>
                    </table>
            </ContentTemplate>
       </asp:UpdatePanel>                    
                    
</asp:Content>