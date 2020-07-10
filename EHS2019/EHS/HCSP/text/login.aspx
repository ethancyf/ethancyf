<%@ Page Language="vb" AutoEventWireup="false" Codebehind="login.aspx.vb" Inherits="HCSP.login1" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">    
    <title id="PageTitle" runat="server"></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/> 

    <script language="Javascript" src="../JS/ideasComboLib4Ra.js" type="text/javascript"></script>

        <script type="text/javascript">
        function checkIdeasComboClientSuccessEHS(_param) {
            var obj = document.getElementById("txtIDEASComboResult")
            obj.value = _param.result
        }

        function checkIdeasComboClientFailureEHS(_param) {
            var obj = document.getElementById("txtIDEASComboResult")
            obj.value = _param.result
        }

        function eHSSuccessCallbackFunc(IDEASComboVersion) { //Success get IDEAS Version
            //alert(IDEASComboVersion);
            var obj = document.getElementById("txtIDEASComboVersion")
            obj.value = IDEASComboVersion
        }

        function eHSFailCallbackFunc() { //IDEAS Combo not yet be installed
            //alert("eHS IDEAS Combo is not available.");
            var obj = document.getElementById("txtIDEASComboVersion")
            obj.value = ""
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="NonLoginPageKey" runat="server" Style="display: none" />
        <asp:TextBox ID="txtIDEASComboResult" runat="server" Style="display: none;" />
        <asp:TextBox ID="txtIDEASComboVersion" runat="server" Style="display: none;" />
        <asp:MultiView ID="mvLogin" runat="server">
            <asp:View ID="vLogin" runat="server">
                <div>
                    <table style="width: 100%; height: 20px;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblBannerText" runat="server" Text="E-Voucher System"></asp:Label></td>
                            <td align="right">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Button ID="lnkbtnTextOnlyVersion" runat="server" CssClass="languageText" SkinID="TextOnlyVersionLinkButton"
                                    PostBackUrl="~/login.aspx" />
                                <asp:Button ID="lnkbtnTradChinese" runat="server" SkinID="TextOnlyVersionLinkButton"
                                    Text="繁體" /><asp:Label ID="lblCurrentLanguageTradChinese" runat="server" Text="繁體"></asp:Label>
                                <asp:Button ID="lnkbtnEnglish" runat="server" Text="English" SkinID="TextOnlyVersionLinkButton" /><asp:Label
                                    ID="lblCurrentLanguageEnglish" runat="server" Text="English"></asp:Label>
                            </td>
                            <td align="right">
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="panSignin" runat="server" DefaultButton="btnLogin">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblFunctionInfo" runat="server" Text="System Login" Font-Underline="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <cc1:TextOnlyMessageBox ID="udcTextOnlyMessageBox" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRoleText" runat="server" Text="Account Type" Width="100%" Font-Bold="True"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSignInSP" runat="server" Text="Service Provider" SkinID="TextOnlyVersionLinkButton" /><asp:Label
                                        ID="lblSignInSP" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSignInDataEntry" runat="server" Text="Data Entry Account" SkinID="TextOnlyVersionLinkButton" /><asp:Label
                                        ID="lblSignInDataEntry" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblLoginAliasText" runat="server" Text="Service Provider ID/ Username"
                                        Font-Bold="True"></asp:Label><asp:Label ID="lblUserNameText" runat="server" Text="Login ID"
                                            Visible="False" Font-Bold="True"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtUserName" runat="server" Width="140px" MaxLength="20"></asp:TextBox><asp:Label
                                        ID="lblUserNameAlert" runat="server" Text="*" ForeColor="red" Visible="false"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPasswordText" runat="server" Text="Password" Font-Bold="True"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="140px" MaxLength="20"></asp:TextBox><asp:Label
                                        ID="lblPasswordAlert" runat="server" Text="*" ForeColor="red" Visible="false"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPinNoText" runat="server" Text="PIN Code" Font-Bold="True"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtPinNo" runat="server" TextMode="Password" Width="140px" MaxLength="6"></asp:TextBox><asp:Label
                                        ID="lblPinNoAlert" runat="server" Text="*" ForeColor="red" Visible="false"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSPIDText" runat="server" Text="Service Provider ID/ Username" Visible="False"
                                        Font-Bold="True"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtSPID" runat="server" Visible="False" Width="140px" MaxLength="20"></asp:TextBox><asp:Label
                                        ID="lblSPIDAlert" runat="server" Text="*" ForeColor="red" Visible="false"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnLogin" runat="server" Text="Login" Width="60px" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:HyperLink ID="hlContactUs" runat="server" NavigateUrl="~/text/login.aspx" Visible="False">Contact us</asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:View>
            <asp:View ID="vConfirmDeclare" runat="server">
                <table border="0" cellpadding="0" cellspacing="0" runat="server" >
                    <tr>
                        <td>
                            <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" runat="server">
                                <tr>
                                    <td style="width: 100%">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, eHealthSystem %>" Font-Underline="True" Font-Size="Medium"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 100%">
                                        <span style="font-size: 12pt;">
                                            <asp:Label ID="labelDescribeConcurrentAccess" runat="server" Text="<%$ Resources:Text, DescribeConcurrentAccess %>" />
                                            <asp:Label ID="labelExplainConcurrentAccess" runat="server" Text="<%$ Resources:Text, ExplainConcurrentAccess %>" />
                                            <asp:Label ID="labelConfirmConcurrentAccess" runat="server" Text="<%$ Resources:Text, ConfirmConcurrentAccess %>" />
                                        </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 4px">
                            <asp:Button ID="btnConfirmReturn" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                            <asp:Button ID="btnConfirmLogin" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn%>" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vNotification" runat="server">
                <asp:Table ID="tblNotification" runat="server" border="0" cellpadding="0" cellspacing="0">
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2" style="width: 100%">
                            <asp:Label ID="lblNoticeHeader" runat="server" Text="" Font-Size="Medium" Font-Underline="true" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2" style="width: 100%;padding-top:5px">
                            <asp:Label ID="lblNoticeMessage" runat="server" Text="" Font-Size="12pt" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <%--<asp:Panel ID="panDeclaration" runat="server" Visible="false">--%>
                    <asp:TableRow ID="trDeclaration">
                        <asp:TableCell style="width: 20px;padding-top:15px;padding-right:5px;vertical-align:top">
                            <asp:CheckBox ID="chkDeclaration" runat="server" Font-Size="12pt" AutoPostBack="true" style="position:relative;top:-2px"/>
                        </asp:TableCell>
                        <asp:TableCell style="width: auto;padding-top:15px">
                              <asp:Label ID="lblDeclaration" runat="server" Font-Size="12pt" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <%--</asp:Panel>--%>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2" style="padding-top: 4px">
                            <asp:Button ID="btnNoticeOK" runat="server" Text="" />
                            <asp:Button ID="btnProceed" runat="server" Text="" Visible="false" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:View>
            <asp:View ID="vReminderObsoleteOS" runat="server">
                <table border="0" cellpadding="0" cellspacing="0" runat="server" >
                    <tr>
                        <td>
                            <table id="tblReminderObsoleteOS" border="0" cellpadding="0" cellspacing="0" runat="server" width="240px">
                                <tr>
                                    <td style="width: 100%">
                                        <asp:Label ID="lblReminderObsoleteOSTitle" runat="server" Text="<%$ Resources:Text, eHealthSystem %>" Font-Underline="True" Font-Size="Medium"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Button ID="lbtnReminderObsoleteOSFullVersion" runat="server" CssClass="languageText" SkinID="TextOnlyVersionLinkButton"
                                            PostBackUrl="~/login.aspx" Text="Full Version"/>
                                        <asp:Button ID="lbtnReminderObsoleteOSTradChinese" runat="server" SkinID="TextOnlyVersionLinkButton"
                                            Text="繁體" /><asp:Label ID="lblReminderTradChinese" runat="server" Text="繁體"></asp:Label>
                                        <asp:Button ID="lbtnReminderObsoleteOSEnglish" runat="server" Text="English" SkinID="TextOnlyVersionLinkButton" /><asp:Label
                                            ID="lblReminderEnglish" runat="server" Text="English"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color:silver">
                                        <asp:Label ID="lblReminderObsoleteOSHeading" runat="server" Text="System Login" Font-Underline="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%">
                                        <span style="font-size: 12pt;">
                                            <asp:Label ID="lblReminderObsoleteOSContent" runat="server" Text="<%$ Resources:Text, ReminderWindowsVersion %>" />
                                        </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 4px">
                            <asp:Button ID="btnReminderObsoleteOSOK" runat="server" Text="<%$ Resources:AlternateText, OKBtn%>" />
                        </td>
                    </tr>
                </table>
            </asp:View>

        </asp:MultiView>
    </form>
</body>
</html>
