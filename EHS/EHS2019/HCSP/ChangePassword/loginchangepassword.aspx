<%@ Page Language="vb" MasterPageFile="~/MasterPageNonLogin.Master" AutoEventWireup="false"
    Codebehind="loginchangepassword.aspx.vb" Inherits="HCSP.loginchangepassword"
    Title="<%$ Resources:Title, ChangePassword %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<%--    <script type="text/javascript">
        function chkAccept_check(cbo, btn){
            if (cbo.checked){
                btn.style.cursor = "hand";
                btn.disabled = false;              
                if (btn.src.indexOf("btn_confirm_D.png") >= 0){
                    btn.src = "../Images/button/btn_confirm.png"
                }
                else{
                    btn.src = "../Images/button/btn_confirm_tw.png"
                }                
            }
            else{
                btn.style.cursor = "default";
               btn.disabled = true;
                if (btn.src.indexOf("btn_confirm.png") >= 0){
                    btn.src = "../Images/button/btn_confirm_D.png"
                }
                else{
                    btn.src = "../Images/button/btn_confirm_D_tw.png"
                }  
            }
        }
    </script>--%>

    <script type="text/javascript">
        function chkAccept_check(cbo, tdEnableConfirm, tdDisableConfirm){
            if (cbo.checked){
                tdEnableConfirm.style.display = "inline";
                tdDisableConfirm.style.display = "none";
            }
            else{
                tdEnableConfirm.style.display = "none";
                tdDisableConfirm.style.display = "inline";
            }
        }
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" border="0" style="width: 994px;">
                <tr style="background-image: url(../Images/master/background.jpg); background-position: bottom; background-repeat: repeat-x; height: 546px" valign="top">
                    <td style="width: 20px">
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 900px">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Image ID="imgHeaderClaimConfimation" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ChangePasswordBanner %>"
                                                    AlternateText="<%$ Resources:AlternateText, ChangePasswordBanner %>" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <cc2:MessageBox ID="udcMessageBox" runat="server" Width="780px" />
                                            </td>
                                        </tr>
                                        <tr style="height: 40px">
                                            <td colspan="2">
                                                <asp:Label ID="lblStatement" runat="server" CssClass="tableCaption" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <asp:Panel ID="pnlSPID" runat="server" Visible="false">
                                            <tr style="height: 35px">
                                                <td>
                                                    <asp:Label ID="lblSPIDText" Text="<%$Resources:Text, SPID%>" CssClass="tableTitle"
                                                        runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSPID" Text="" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <tr style="height: 35px">
                                            <td style="width: 180px">
                                                <asp:Label ID="lblUsernameText" Text="<%$Resources:Text, SPLoginAlias%>" CssClass="tableTitle"
                                                    runat="server"></asp:Label>
                                            </td>
                                            <td style="width: 620px">
                                                <asp:Label ID="lblUsername" Text="" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="height: 35px">
                                            <td>
                                                <asp:Label ID="lblOldPasswordText" Text="<%$Resources:Text, OldPassword%>" CssClass="tableTitle"
                                                    runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" MaxLength="20">
                                                </asp:TextBox>
                                                <asp:Image ID="imgOldPasswordAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                    Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                            </td>
                                        </tr>
                                        <tr style="height: 35px">
                                            <td>
                                                <asp:Label ID="lblNewPasswordText" Text="<%$Resources:Text, NewPassword%>" CssClass="tableTitle"
                                                    runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <table cellpadding="0" cellspacing="0" border="0s">
                                                    <tr>
                                                        <td style="width: 185px">
                                                            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" MaxLength="20">
                                                            </asp:TextBox>
                                                            <asp:Image ID="imgNewPasswordAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                        </td>
                                                        <td>
                                                            <table style="width: 300px" cellpadding="0">
                                                                <tr>
                                                                    <td colspan="5">
                                                                        <div id="progressBar" style="font-size: 1px; height: 10px; width: 290px; border: 1px solid white;" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="width: 300px">
                                                                    <td align="center" style="width: 30%">
                                                                        <span id="strength1"></span>
                                                                    </td>
                                                                    <td style="width: 5%">
                                                                        <span id="direction1"></span>
                                                                    </td>
                                                                    <td align="center" style="width: 30%">
                                                                        <span id="strength2"></span>
                                                                    </td>
                                                                    <td style="width: 5%">
                                                                        <span id="direction2"></span>
                                                                    </td>
                                                                    <td align="center" style="width: 30%">
                                                                        <span id="strength3"></span>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 35px">
                                            <td>
                                                <asp:Label ID="lblNewPasswordConfirmText" Text="<%$Resources:Text, ConfirmPW%>" CssClass="tableTitle"
                                                    runat="server"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNewPasswordConfirm" runat="server" TextMode="Password" MaxLength="20">
                                                </asp:TextBox>
                                                <asp:Image ID="imgNewPasswordConfirmAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                    Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="left">
                                                <p>
                                                    <br />
                                                    <asp:Label ID="lblWebPasswordTips" runat="server" Text="<%$ Resources:Text, WebPasswordTips %>">
                                                    </asp:Label>
                                                    <br />
                                                    <asp:Label ID="lblWebPasswordTips1" runat="server" Text="<%$ Resources:Text, WebPasswordTips1 %>">
                                                    </asp:Label><br />
                                                    &nbsp; &nbsp;&nbsp;<asp:Label ID="lblWebPasswordTips1a" runat="server" Text="<%$ Resources:Text, WebPasswordTips1a %>"></asp:Label><br />
                                                    &nbsp; &nbsp;&nbsp;<asp:Label ID="lblWebPasswordTips1b" runat="server" Text="<%$ Resources:Text, WebPasswordTips1b %>"></asp:Label><br />
                                                    &nbsp; &nbsp;&nbsp;<asp:Label ID="lblWebPasswordTips1c" runat="server" Text="<%$ Resources:Text, WebPasswordTips1c %>"></asp:Label><br />
                                                    &nbsp; &nbsp;&nbsp;<asp:Label ID="lblWebPasswordTips1d" runat="server" Text="<%$ Resources:Text, WebPasswordTips1d %>"></asp:Label><br />
                                                    <asp:Label ID="lblWebPasswordTips2" runat="server" Text="<%$ Resources:Text, WebPasswordTips2 %>">
                                                    </asp:Label><br />
                                                    <asp:Label ID="LabelWebPasswordTips3" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>">
                                                    </asp:Label>
                                                    <br />
                                                    <br />
                                            </td>
                                        </tr>
                                    </table>

                                    <cc1:FilteredTextBoxExtender ID="FilteredNewPassword" runat="server" TargetControlID="txtNewPassword"
                                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                    </cc1:FilteredTextBoxExtender>
                                    <cc1:FilteredTextBoxExtender ID="FilteredNewPasswordConfirm" runat="server" TargetControlID="txtNewPasswordConfirm"
                                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                    </cc1:FilteredTextBoxExtender>

                                    <asp:Panel ID="pnlAgreement" runat="server" Visible="true">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td align="left" style="width: 955px" bgcolor="#ffff99">
                                                    <table>
                                                        <tr style="height: 20px">
                                                            <td valign="top">
                                                                <asp:Label ID="lblAgreementText" runat="server" Text="<%$ Resources:Text, Agreement %>">
                                                                </asp:Label>:
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: justify">
                                                                <asp:Label ID="lblAgreement" runat="server" Text="<%$ Resources:Text, 1stLoginAgreement %>">
                                                                </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 25px">
                                                            <td valign="bottom">
                                                                <asp:CheckBox ID="chkAccept" runat="server" Text="<%$ Resources:Text, Accept %>" AutoPostBack="false"></asp:CheckBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 980px">
                                        <tr>
                                            <td align="right" id="tdDisableConfirm" runat="server" style="padding-right:2px;display:none">
                                                <asp:ImageButton ID="ibtnDisableConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmDisableBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" Enabled="false" />
                                            </td>
                                            <td align="right" id="tdConfirm" runat="server" style="padding-right:2px">
                                                <asp:ImageButton ID="ibtnConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" Enabled="true" />
                                            </td>
                                            <td align="left" id="tdExit" style="padding-left:2px">
                                                <asp:ImageButton ID="ibtnExit" runat="server" ImageUrl="<%$ Resources:ImageUrl, ExitBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ExitBtn %>" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
