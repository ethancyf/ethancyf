<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcAgeRange.ascx.vb"
    Inherits="HCVU.udcAgeRange" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table>
    <asp:Panel ID="panAgeRange" runat="server">
        <tr>
            <asp:Panel ID="panMinAge" runat="server">
                <td align="left" style="width: 150px;">
                    <asp:Label ID="lblMinAge" runat="server" CssClass="tableTitle" Text="Age Range"></asp:Label>
                </td>
                <td style="width: 46px">
                    <asp:TextBox ID="txtMinAge" MaxLength="3" runat="server" Width="40px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                        onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                        onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="filterMinAge" runat="server" FilterType="Numbers"
                        TargetControlID="txtMinAge" Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </asp:Panel>
            <asp:Panel ID="panMaxAge" runat="server">
                <td style="padding-right: 10px; padding-left: 10px;">
                    <asp:Label ID="lblMaxAge" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, To_S %>"></asp:Label>
                </td>
                <td style="width: 46px">
                    <asp:TextBox ID="txtMaxAge" MaxLength="3" runat="server" Width="40px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                        onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                        onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="filterMaxAge" runat="server" FilterType="Numbers"
                        TargetControlID="txtMaxAge" Enabled="True">
                    </cc1:FilteredTextBoxExtender>                    
                </td>
            </asp:Panel>
            <asp:Panel ID="panErrorAge" runat="server">
                <td>
                    <asp:Image ID="imgErrorAge" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                        Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Style="vertical-align: top" />
                </td>
            </asp:Panel>
        </tr>
    </asp:Panel>
</table>
