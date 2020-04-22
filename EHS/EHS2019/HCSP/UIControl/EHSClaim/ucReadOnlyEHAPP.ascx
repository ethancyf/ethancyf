<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyEHAPP.ascx.vb" Inherits="HCSP.ucReadOnlyEHAPP" %>

<asp:Panel ID="pnlReadOnlyEHAPPDetails" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" style="width:185px">
                <asp:Label ID="lblCoPaymentText" runat="server" Text="" CssClass="tableTitle" Height="25px" Width="160px"></asp:Label>
            </td>
            <td valign="top">
                <asp:Label ID="lblCoPayment" runat="server" Text="" CssClass="tableText"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Panel>
