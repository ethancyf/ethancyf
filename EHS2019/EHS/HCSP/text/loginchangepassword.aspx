<%@ Page Language="vb" AutoEventWireup="false"  MasterPageFile="~/CSRFMasterPage.Master" Codebehind="loginchangepassword.aspx.vb"
    Inherits="HCSP.loginchangepassword1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<asp:Content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>   
            <meta name="viewport" content="width=device-width, initial-scale=1.0"/>

            <div>
                <table style="width: 100%;" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblBannerText" runat="server" Text="<%$Resources:Text, EVoucherSystem%>"></asp:Label></td>
                        <td align="right">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:LinkButton ID="lnkbtnTradChinese" runat="server">繁體</asp:LinkButton>
                            <asp:LinkButton ID="lnkbtnEnglish" runat="server" Enabled="False">English</asp:LinkButton></td>
                        <td align="right">
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFunctionInfo" runat="server" Text="<%$Resources:Text, ChangePassword%>"
                                            Font-Underline="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <cc1:TextOnlyMessageBox ID="udcTextOnlyMessageBox" runat="server"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:Label ID="lblStatement" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <asp:Panel ID="pnlSPID" runat="server" Visible="true">
                                    <tr>
                                        <td style="width: 100%;">
                                            <asp:Label ID="lblSPIDText" Text="<%$Resources:Text, SPID%>" runat="server" CssClass="tableCaption"
                                                Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%;">
                                            <asp:Label ID="lblSPID" Text="12345613" runat="server" CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:Label ID="lblUsernameText" Text="<%$Resources:Text, SPLoginAlias%>" CssClass="tableTitle"
                                            runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:Label ID="lblUsername" Text="HAITADMIN" runat="server" CssClass="tableText"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:Label ID="lblOldPasswordText" Text="<%$Resources:Text, OldPassword%>" CssClass="tableTitle"
                                            runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
                                        <asp:Label ID="lblOldPasswordAlert" runat="server" Text="*" ForeColor="red" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:Label ID="lblNewPasswordText" Text="<%$Resources:Text, NewPassword%>" CssClass="tableTitle"
                                            runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
                                        <asp:Label ID="lblNewPasswordAlert" runat="server" Text="*" ForeColor="red" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:Label ID="lblNewPasswordConfirmText" Text="<%$Resources:Text, ConfirmPW%>" CssClass="tableTitle"
                                            runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:TextBox ID="txtNewPasswordConfirm" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
                                        <asp:Label ID="lblNewPasswordConfirmAlert" runat="server" Text="*" ForeColor="red"
                                            Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 100%">
                                        <br />
                                        <asp:Label ID="lblWebPasswordTips" runat="server" Text="<%$ Resources:Text, WebPasswordTips %>"
                                            Font-Bold="True"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 100%;">
                                        <asp:Label ID="lblWebPasswordTips1" runat="server" Text="<%$ Resources:Text, WebPasswordTips1 %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 100%">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                </td>
                                                <td style="width: 100%">
                                                    <asp:Label ID="lblWebPasswordTips1a" runat="server" Text="<%$ Resources:Text, WebPasswordTips1a %>"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15px">
                                                </td>
                                                <td style="width: 100%">
                                                    <asp:Label ID="lblWebPasswordTips1b" runat="server" Text="<%$ Resources:Text, WebPasswordTips1b %>"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15px">
                                                </td>
                                                <td style="width: 100%">
                                                    <asp:Label ID="lblWebPasswordTips1c" runat="server" Text="<%$ Resources:Text, WebPasswordTips1c %>"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15px">
                                                </td>
                                                <td style="width: 100%">
                                                    <asp:Label ID="lblWebPasswordTips1d" runat="server" Text="<%$ Resources:Text, WebPasswordTips1d %>"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 100%">
                                        <asp:Label ID="lblWebPasswordTips2" runat="server" Text="<%$ Resources:Text, WebPasswordTips2 %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 100%">
                                        <asp:Label ID="LabelWebPasswordTips3" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            <asp:Panel ID="pnlAgreement" runat="server" Visible="true">
                                <br />
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td align="left" style="width: 100%">
                                            <asp:Label ID="lblAgreement" runat="server" Text="<%$ Resources:Text, Agreement %>"></asp:Label>:
                                            <br />
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, 1stLoginAgreement %>"
                                                Width="100%"></asp:Label>
                                            <asp:CheckBox ID="chkAccept" runat="server" Text="<%$ Resources:Text, Accept %>"></asp:CheckBox><asp:Label
                                                ID="lblAcceptAlert" runat="server" Text="*" ForeColor="red" Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;">
                            <asp:Button ID="btnLogin" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn %>"
                                Width="60px" />
                            <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:AlternateText, ExitBtn %>"
                                Width="60px" /></td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
