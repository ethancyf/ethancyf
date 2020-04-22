<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPageNonLogin.Master"
    Codebehind="DataEntryRecoverLogin.aspx.vb" Inherits="HCSP.DataEntryRecoverLogin"
    title="<%$ Resources:Title, RecoverLogin %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%"  border="0" cellpadding="0" cellspacing="0">
                <tr style="background-image: url(../Images/master/background.jpg); background-position: bottom;
                    background-repeat: repeat-x; height: 546px" valign="top">
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td colspan="2" style="width: 950px">
                                    <asp:Image ID="imgHeaderRecoverLogin" runat="server" AlternateText="<%$ Resources:AlternateText, RecoverLoginBanner%>"
                                        ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, RecoverLoginBanner%>" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20px">
                                </td>
                                <td>
                                    <table style="width: 100%">
                                        <tr>
                                            <td valign="top" colspan="4">
                                                <asp:Label ID="lbl_successMsg" runat="server" CssClass="tableText" Text="<%$ Resources:Text, DataEntryRecoverLoginMsg %>"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td valign="top" colspan="4">
                                                &nbsp
                                            </td>
                                        </tr>
                                    </table>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 62px">
                                                <asp:ImageButton ID="btn_backToLogin" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" />
                                            </td>
                                            <td align="center">
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
