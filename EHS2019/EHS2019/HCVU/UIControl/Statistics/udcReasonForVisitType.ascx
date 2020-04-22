<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcReasonForVisitType.ascx.vb"
    Inherits="HCVU.udcReasonForVisitType" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table>
    <asp:Panel ID="panReasonForVisit" runat="server">
        <tr>
            <td align="left" style="width: 150px; vertical-align: top">
                <asp:Label ID="lblReasonForVisit" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ReasonVisit %>" />
            </td>
            <td align="left" style="width: 600px">
                <asp:DropDownList ID="ddlReasonForVisit" runat="server" Width="200px" />
                <asp:Image ID="imgErrorReasonForVisit" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="vertical-align: top" />
            </td>
        </tr>
    </asp:Panel>
</table>
