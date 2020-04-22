<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="udcPeriodBreakDown.ascx.vb" 
    Inherits="HCVU.udcPeriodBreakDown" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table>
    <asp:Panel ID="panPeriodBreakDown" runat="server">
        <tr>
            <td align="left" style="width: 150px; vertical-align: top">
                <asp:Label ID="lblPeriodBreakDown" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, BreakDownType %>" />
            </td>
            <td align="left" style="width: 600px">
                <asp:DropDownList ID="ddlPeriodBreakDown" runat="server" Width="200px" />
                <asp:Image ID="imgErrorPeriodBreakDown" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="vertical-align: top" />
            </td>
        </tr>
    </asp:Panel>
</table>