<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoginChangePassword.aspx.vb" Inherits="HCVU.LoginChangePassword" Title="<%$ Resources:Title, ChangePassword %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title id="PageTitle" runat="server"></title>
    <meta http-equiv="Page-Exit" content="revealTrans(Duration=0,Transition=12)" />
    <link href="~/CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/MenuStyle.css" rel="stylesheet" type="text/css" />
    <link href="~/CSS/DialogStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../JS/Common.js"></script>
    <script type="text/javascript">
        function chkAccept_check() {
            if (document.all.chkAccept.checked) {
                document.getElementById("ibtnConfirm").style.cursor = "hand";
                document.getElementById("ibtnConfirm").disabled = false;
                document.getElementById("ibtnConfirm").src = "../Images/button/btn_confirm.png";
            }
            else {
                document.getElementById("ibtnConfirm").style.cursor = "default";
                document.getElementById("ibtnConfirm").disabled = true;
                document.getElementById("ibtnConfirm").src = "../Images/button/btn_confirm_D.png";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="ibtnConfirm">

        <div>
            <table border="0" cellpadding="0" cellspacing="0" style="background-image: url(../Images/master/banner_header.jpg); width: 994px; background-repeat: no-repeat; height: 100px"
                id="tblBanner" runat="server">
                <tr>
                    <td class="AppEnvironment" style="vertical-align: top">
                        <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>

            <asp:Button ID="btnHiddenPleaseWait" runat="server" Style="display: none;" />
            <asp:Panel ID="pnlPleaseWait" runat="server" Style="display: none; visibility: hidden">
                <table style="width: 150px; height: 150px; background-color: #ffffff" border="0"
                    cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="middle">
                            <asp:Image ID="imgLoading" runat="server" ImageUrl="<%$ Resources:ImageUrl, PleaseWait %>"
                                AlternateText="<%$ Resources:AlternateText, PleaseWait %>" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0">
                <ProgressTemplate>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlPleaseWait"
                TargetControlID="btnHiddenPleaseWait" BackgroundCssClass="modalBackgroundTransparent" />

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" style="background-position: bottom; background-image: url(../Images/master/body_background.jpg); width: 994px; background-repeat: repeat-x">
                        <tr>
                            <td style="width: 5px"></td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="height: 515px;" valign="top">
                                            <table cellpadding="0" cellspacing="0" style="width: 980px;" border="0">
                                                <tr>
                                                    <td style="height: 27px; width: 980px;" valign="top" colspan="2">
                                                        <asp:Image ID="imgHeader" runat="server" AlternateText="Voucher Account Maintenance"
                                                            ImageAlign="AbsMiddle" ImageUrl="~/Images/banner/ChangePassword/banner_changePassword.png" />

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Type="Complete" Width="780px" />
                                                        <cc2:MessageBox ID="udcMessageBox" runat="server" Width="780px" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width: 950px;" cellspacing="1" cellpadding="1" border="0">
                                                <tr style="height: 40px">
                                                    <td colspan="2" class="loginLableText" align="left">
                                                        <asp:Label ID="lblStatement" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 170px; height: 30px;" align="left">Login ID</td>
                                                    <td style="width: 780px; height: 30px;" align="left">
                                                        <asp:Label ID="lblUserName" runat="server" Text="UserA" CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">Old Password</td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtOldPassword" runat="server" MaxLength="20" TextMode="Password" Width="200px"></asp:TextBox>
                                                        <asp:Image ID="imgOldPasswordAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                    </td>
                                                </tr>
                                                <tr style="height: 35px">
                                                    <td align="left">
                                                        <asp:Label ID="lblNewPasswordText" runat="server" Text="<%$Resources:Text, NewPassword%>"></asp:Label></td>
                                                    <td align="left">
                                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 550px">
                                                            <tr>
                                                                <td style="width: 240px">
                                                                    <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="20" TextMode="Password" Width="200px"></asp:TextBox>
                                                                    <asp:Image ID="imgNewPasswordAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                                </td>
                                                                <td valign="top">
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
                                                <tr>
                                                    <td style="height: 30px" align="left">Confirm Password</td>
                                                    <td style="height: 31px" align="left">
                                                        <asp:TextBox ID="txtNewPasswordConfirm" runat="server" MaxLength="20" TextMode="Password" Width="200px"></asp:TextBox>
                                                        <asp:Image ID="imgNewPasswordConfirmAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <p>
                                                            <br />
                                                            <asp:Label ID="lblWebPasswordTips" runat="server" Text="<%$ Resources:Text, WebPasswordTips %>"></asp:Label>
                                                            <br />
                                                            <asp:Label ID="lblWebPasswordTips1" runat="server" Text="<%$ Resources:Text, WebPasswordTips1 %>"></asp:Label><br />
                                                            &nbsp; &nbsp;&nbsp;<asp:Label ID="lblWebPasswordTips1a" runat="server" Text="<%$ Resources:Text, WebPasswordTips1a %>"></asp:Label><br />
                                                            &nbsp; &nbsp;&nbsp;<asp:Label ID="lblWebPasswordTips1b" runat="server" Text="<%$ Resources:Text, WebPasswordTips1b %>"></asp:Label><br />
                                                            &nbsp; &nbsp;&nbsp;<asp:Label ID="lblWebPasswordTips1c" runat="server" Text="<%$ Resources:Text, WebPasswordTips1c %>"></asp:Label><br />
                                                            &nbsp; &nbsp;&nbsp;<asp:Label ID="lblWebPasswordTips1d" runat="server" Text="<%$ Resources:Text, WebPasswordTips1d %>"></asp:Label><br />
                                                            <asp:Label ID="lblWebPasswordTips2" runat="server" Text="<%$ Resources:Text, WebPasswordTips2 %>"></asp:Label><br />
                                                            <asp:Label ID="LabelWebPasswordTips3" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>"></asp:Label>
                                                            <br />
                                                            <br />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="pnlAgreement" runat="server" Visible="true">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td align="left" style="width: 955px" bgcolor="#ffff99">
                                                            <table>
                                                                <tr style="height: 20px">
                                                                    <td valign="top">
                                                                        <asp:Label ID="lblAgreementText" runat="server" Text="<%$ Resources:Text, Agreement %>"></asp:Label>:
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="text-align: justify">
                                                                        <asp:Label ID="lblAgreement" runat="server" Text="<%$ Resources:Text, 1stLoginAgreement %>"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 25px">
                                                                    <td valign="bottom">
                                                                        <asp:CheckBox ID="chkAccept" runat="server" Text="<%$ Resources:Text, Accept %>"></asp:CheckBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <table cellpadding="0" cellspacing="0" border="0" style="width: 980px">
                                                <tr>
                                                    <td align="center">
                                                        <asp:ImageButton ID="ibtnConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" Enabled="false" />
                                                        <asp:ImageButton ID="ibtnExit" runat="server" ImageUrl="<%$ Resources:ImageUrl, ExitBtn %>" AlternateText="<%$ Resources:AlternateText, ExitBtn %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="background-color: #dbdbdb" valign="top">
                                            <table style="width: 98%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td colspan="2" style="height: 2px">
                                                        <hr style="width: 98%; color: #ffffff; border-top-style: none; border-right-style: none; border-left-style: none; height: 1px; border-bottom-style: none;" />


                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblFooterTerms" runat="server" CssClass="footerText" Text="Terms & Conditions | Data Protection & Privacy Statement"></asp:Label>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Label ID="lblFooterCopyright" runat="server" CssClass="footerText" Text="© Copyright Hospital Authority . All rights reserved."
                                                            Visible="False"></asp:Label>
                                                        &nbsp; &nbsp; &nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <%--<cc1:FilteredTextBoxExtender ID="FilteredOldPassword" runat="server" TargetControlID="txtOldPassword"
                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                    </cc1:FilteredTextBoxExtender>--%>
                    <cc1:FilteredTextBoxExtender ID="FilteredNewPassword" runat="server" TargetControlID="txtNewPassword"
                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                    </cc1:FilteredTextBoxExtender>
                    <cc1:FilteredTextBoxExtender ID="FilteredNewPasswordConfirm" runat="server" TargetControlID="txtNewPasswordConfirm"
                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                    </cc1:FilteredTextBoxExtender>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </form>
</body>
</html>
