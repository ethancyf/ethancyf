<%@ Control Language="vb" AutoEventWireup="false" Codebehind="reportCriteriaScheme.ascx.vb"
    Inherits="HCVU.reportCriteriaScheme" %>
<table cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td style="width: 180px">
            <asp:Label ID="lblSchemeText" runat="server" CssClass="tableTitle" Text="<SchemeText>"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlScheme" runat="server" AppendDataBoundItems="False" Width="200px">
            </asp:DropDownList>
            <asp:Image ID="imgErrorScheme" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" />
        </td>
    </tr>
    <tr style="height: 6px">
    </tr>
</table>
