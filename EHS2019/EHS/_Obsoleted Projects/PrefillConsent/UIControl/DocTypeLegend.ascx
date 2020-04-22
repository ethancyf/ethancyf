<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DocTypeLegend.ascx.vb" Inherits="PrefillConsent.DocTypeLegend" %>
<table>
    <tr>
        <td>
            <asp:Label ID="lblDocType" runat="server" Text="<%$ Resources:Text, preFillAllDoc %>"></asp:Label>
        </td>
    </tr>
</table>
<asp:GridView ID="gvDocType" runat="server" Width="600px" ShowHeader="false" GridLines="None" SkinID="GridViewNoBorder">
</asp:GridView>
