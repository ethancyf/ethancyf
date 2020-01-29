<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcMeansOfInput.ascx.vb"
    Inherits="HCVU.udcMeansOfInput" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table>
    <asp:Panel ID="panMeansOfInput" runat="server">
        <tr>
            <td align="left" style="width: 150px; vertical-align: top">
                <asp:Label ID="lblMeansOfInput" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, MeansOfInput %>" />
            </td>
            <td align="left" style="width: 600px">
                <asp:DropDownList ID="ddlMeansOfInput" runat="server" Width="200px" />
                <asp:Image ID="imgErrorMeansOfInput" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" style="vertical-align: top" />
            </td>
        </tr>
    </asp:Panel>
</table>
