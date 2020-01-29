<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DocTypeLegend.ascx.vb" Inherits="HCVU.DocTypeLegend" %>
<table>
    <tr>
        <td class="headingText">
            <asp:Label ID="lblDocType" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
    </tr>
</table>
<asp:GridView ID="gvDocType" runat="server" Width="570px">
</asp:GridView>