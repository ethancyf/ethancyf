<%@ Control Language="vb" AutoEventWireup="false" Codebehind="reportCriteriaSP.ascx.vb"
    Inherits="HCVU.reportCriteriaSP" %>
<table cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td style="width: 150px">
            <asp:Label ID="lblLabelText" runat="server" CssClass="tableTitle" Text="<LabelText>"></asp:Label></td>
        <td>
            <asp:DropDownList ID="ddlDropDown" runat="server" AppendDataBoundItems="False" Width="200px">
            </asp:DropDownList>
            <asp:Image ID="imgErrorDropDown" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" />
        </td>
    </tr>
    <tr style="height: 6px">
    </tr>
</table>
