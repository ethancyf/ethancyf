<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcNumeric.ascx.vb"
    Inherits="HCVU.udcNumeric" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<table>
    <asp:Panel ID="panInput" runat="server">
        <tr class="PPC_Numeric_tr">
            <td align="left" style="width: 220px; vertical-align: top" class="PPC_Numeric_LabelWidth">
                <asp:Label ID="lblInputText" runat="server" Text=""></asp:Label>
            </td>
            <td align="left" style="width: 600px;" class="PPC_Numeric_ValueWidth">
                <asp:TextBox ID="txtInput" runat="server" Width="30px" CssClass="PPC_Numeric_Textbox"/>
                <cc1:FilteredTextBoxExtender ID="filterInput" runat="server" FilterType="Custom, Numbers"
                        TargetControlID="txtInput" ValidChars="" Enabled="True">
                </cc1:FilteredTextBoxExtender>

                <asp:Label ID="lblInputDesc" runat="server" Text="" CssClass="PPC_Numeric_Desc"></asp:Label>
                <asp:Image ID="imgErrorInput" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" CssClass="TextboxWithDesc_Alert"
                                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" />
            </td>
        </tr>
    </asp:Panel>
</table>

