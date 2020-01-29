<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="login.aspx.vb" Inherits="HCVU.login" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="PageTitle" runat="server"></title>
    <base id="basetag" href="http://localhost/hcvu/" runat="server" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/DialogStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="JS/Common.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
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
            <cc2:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlPleaseWait"
                TargetControlID="btnHiddenPleaseWait" BackgroundCssClass="modalBackgroundTransparent" />

            <table style="width: 994px" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="AppEnvironment" style="background-position: top center; background-image: url(Images/master/banner_header.jpg); background-repeat: no-repeat; height: 100px; vertical-align: top">
                        <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                        <asp:ScriptManager ID="ScriptManager2" runat="server">
                        </asp:ScriptManager>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:UpdatePanel ID="UpdatePanel" runat="server">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 615px; position: relative; left: 22px">
                                    <tr>
                                        <td style="height: 55px">
                                            <asp:Panel ID="pnlNewsMessage" runat="server" CssClass="messageText" BorderWidth="1px" BorderColor="#666666" Width="750px">
                                                &nbsp;<asp:Label ID="lblNewsMessageTitle" runat="server" Text="<%$ Resources:Text, SystemMessage %>" Font-Underline="True" Font-Bold="True" Height="20px" Font-Size="12pt"></asp:Label>
                                                <asp:DataList ID="dlNewsMessage" runat="server" RepeatColumns="1" ShowFooter="False" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <table border="0" style="width: 100%">
                                                            <tr>
                                                                <td style="width: 120px" valign="top">
                                                                    <asp:Label ID="lblCreateDate" runat="server" Text='<%# Bind("CreateDtm") %>'></asp:Label>
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </asp:Panel>
                                            <br />
                                            <cc1:MessageBox ID="udcMessageBox" runat="server" Width="600px"></cc1:MessageBox>
                                        </td>
                                    </tr>
                                </table>

                                <table style="background-position: center center; background-image: url(Images/login/login_background.jpg); width: 750px; background-repeat: no-repeat; height: 280px"
                                    border="0">
                                    <tr>
                                        <td colspan="4" style="height: 20px"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" valign="middle" align="center">
                                            <table style="width: 100%" border="0">
                                                <tr>
                                                    <td style="width: 75px">&nbsp;</td>
                                                    <td align="center">
                                                        <asp:Label ID="lblSystemLogon" runat="server" CssClass="loginTitleText" Text="System Login"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 125px" rowspan="4"></td>
                                        <td class="loginLableText" align="left">
                                            <asp:Label ID="lblUsername" runat="server" Text="<%$ Resources:Text, LoginID %>" CssClass="loginLableText"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtUsername" runat="server" CssClass="loginLableText" Width="200px" onblur="convertToUpper(this)" MaxLength="20"></asp:TextBox>
                                            <asp:Image ID="imgUserNameAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                        </td>
                                        <td rowspan="4">
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td>
                                                        <asp:HyperLink ID="hylForgotUsername" runat="server" NavigateUrl="~/login.aspx" Visible="false" TabIndex="99">Forgot "Password"
                                                        </asp:HyperLink>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Image ID="imgToken" runat="server" ImageUrl="~/Images/others/token_2.png" Visible="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="loginLableText" align="left">
                                            <asp:Label ID="lblPassword" runat="server" Text="<%$Resources:Text, Password%>" CssClass="loginLableText"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="loginLableText" Width="200px" MaxLength="20"></asp:TextBox>
                                            <asp:Image ID="imgPasswordAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="loginLableText" align="left">
                                            <asp:Label ID="lblPinCode" runat="server" Text="<%$Resources:Text, PinNo%>" CssClass="loginLableText"></asp:Label></td>
                                        <td align="left">
                                            <asp:TextBox ID="txtPinCode" runat="server" TextMode="Password" CssClass="loginLableText" Width="200px" MaxLength="6"></asp:TextBox>
                                            <asp:Image ID="imgPinCodeAlert" runat="server" Visible="false" ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 30px"></td>
                                        <td style="height: 30px" align="left">
                                            <asp:ImageButton ID="ibtnLogin" runat="server" ImageUrl="~/Images/button/btn_login.png" AlternateText="Login" />
                                            <asp:ImageButton ID="btnExit" runat="server" ImageUrl="~/Images/button/btn_exit.png" AlternateText="Exit" OnClientClick="javascript:window.opener='X';window.open('','_parent','');window.close(); return false;" /></td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="height: 200px">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 10%"></td>
                                <td style="width: 85%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:ImageButton ID="ibtnUserManual" runat="server" ImageUrl="<%$Resources:ImageUrl, UserManualBtn%>" AlternateText="<%$Resources:AlternateText, UserManualBtn%>" OnClientClick="window.open('', '_blank', 'resizable=yes,status=yes,toolbar=no,location=no,scrollbars=yes,left=0,top=0'); return false;" /></td>
                                            <td align="center">
                                                <asp:ImageButton ID="ibtnWhatsNews" runat="server" ImageUrl="<%$Resources:ImageUrl, WhatsNewBtn%>" AlternateText="<%$Resources:AlternateText, WhatsNewBtn%>" OnClientClick="window.open('http://ha.home', '_blank', 'resizable=yes,status=yes,toolbar=no,location=no,scrollbars=yes,left=0,top=0'); return false;" Visible="false" /></td>
                                            <td align="center">
                                                <asp:ImageButton ID="ibtnFAQ" runat="server" ImageUrl="<%$Resources:ImageUrl, FAQsBtn%>" AlternateText="<%$Resources:AlternateText, FAQsBtn%>" OnClientClick="window.open('http://ha.home', '_blank', 'resizable=yes,status=yes,toolbar=no,location=no,scrollbars=yes,left=0,top=0'); return false;" /></td>
                                            <td align="center">
                                                <asp:ImageButton ID="ibtnContactUs" runat="server" ImageUrl="<%$Resources:ImageUrl, ContactUsBtn%>" AlternateText="<%$Resources:AlternateText, ContactUsBtn%>" OnClientClick="window.open('http://ha.home', '_blank', 'resizable=yes,status=yes,toolbar=no,location=no,scrollbars=yes,left=0,top=0'); return false;" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 5%"></td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="background-color: #d6d6d6" valign="top">
                        <table style="width: 98%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left">&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkBtnPrivacyPolicy" runat="server" CssClass="footerText" Text="<%$ Resources:Text, PrivacyPolicy %>"></asp:LinkButton>
                                    <asp:Label ID="lblseparator1" runat="server" CssClass="footerText" Text=" | "></asp:Label><asp:LinkButton ID="lnkBtnDisclaimer" runat="server" CssClass="footerText" Text="<%$ Resources:Text, ImportantNotices %>"></asp:LinkButton>
                                    <asp:Label ID="lblseparator2" runat="server" CssClass="footerText" Text=" | "></asp:Label><asp:LinkButton ID="lnkBtnSysMaint" runat="server" CssClass="footerText" Text="<%$ Resources:Text, SysMaint %>"></asp:LinkButton>
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
        </div>
    </form>
    <script type="text/javascript">

        function ResizeScreen() {
            var w = screen.availWidth || screen.width;
            var h = screen.availHeight || screen.height;

            window.moveTo(0, 0);
            window.resizeTo(w, h);
        }

        ResizeScreen();

    </script>
</body>
</html>
