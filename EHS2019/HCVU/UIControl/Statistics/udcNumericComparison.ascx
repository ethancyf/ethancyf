<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcNumericComparison.ascx.vb"
    Inherits="HCVU.udcNumericComparison" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<table>
    <asp:Panel ID="panInput" runat="server">
        <tr class="NumericComparison_tr">
            <td style="width: 150px;text-align:left;vertical-align:top" class="NumericComparison_LabelWidth">
                <asp:Label ID="lblText" runat="server" Text=""></asp:Label>
            </td>
            <td style="text-align:left; width: 600px; vertical-align:top" class="NumericComparison_ValueWidth">
                <asp:Label ID="lblCompareItem" runat="server" Text="" CssClass="Numeric_CompareItemLabel" />
                <asp:Label ID="lblOperator" runat="server" Text="" CssClass="Numeric_OperatorLabel" />
                <asp:TextBox ID="txtInput" runat="server" Width="50px" CssClass="NumericComparison_InputTextbox" />
                <cc1:FilteredTextBoxExtender ID="filterInput" runat="server" FilterType="Custom, Numbers" TargetControlID="txtInput" ValidChars="" Enabled="True" />
                <asp:Image ID="imgErrorInput" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" CssClass="NumericComparison_ErrorImage"
                                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" />
            </td>
        </tr>
    </asp:Panel>
</table>

