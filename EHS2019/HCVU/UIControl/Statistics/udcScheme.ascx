<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcScheme.ascx.vb"
    Inherits="HCVU.udcScheme" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table>
    <asp:Panel ID="panScheme" runat="server">
        <tr>
            <td align="left" style="width: 150px; vertical-align: top">
                <asp:Label ID="lblScheme" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>" />
            </td>
            <td align="left" style="width: 600px;">
                <asp:DropDownList ID="ddlScheme" runat="server" Width="200px" AppendDataBoundItems="True" />
                <asp:Image ID="imgErrorScheme" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="vertical-align: top" />
            </td>
        </tr>
    </asp:Panel>
</table>
