<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyEHAPP.ascx.vb" Inherits="HCVU.ucReadOnlyEHAPP" %>

<table id="tblEHAPP" runat="server" border="0" cellpadding="0" cellspacing="0">
    <tr style="height:22px">
        <td style="vertical-align:top; width:200px">
            <asp:Label ID="lblCoPaymentText" runat="server" Text="<%$ Resources:Text, EHAPP_CoPayment %>" CssClass="tableTitle"></asp:Label>
        </td>
        <td style="vertical-align:top; padding-left:3px">
            <asp:Label ID="lblCoPayment" runat="server" Text="" CssClass="tableText"></asp:Label>
        </td>
    </tr>
</table>
