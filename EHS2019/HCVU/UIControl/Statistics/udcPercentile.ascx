<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="udcPercentile.ascx.vb"
    Inherits="HCVU.udcPercentile" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<table>
    <asp:Panel ID="panPercentile" runat="server">
        <tr class="Percentile_tr">
            <td align="left" style="width: 220px; vertical-align: top" class="Percentile_LabelWidth">
                <asp:Label ID="lblPercentileText" runat="server"></asp:Label>
            </td>            
            <asp:Panel ID="panP1" runat="server">
                <td style="width: 75px; vertical-align: top" class="Percentile_ValueWidth">
                    <asp:CheckBox ID="chkP1" runat="server" class="Percentile_Checkbox" />
                    <asp:TextBox ID="txtP1" MaxLength="3" runat="server" Width="30px"></asp:TextBox>
                    <asp:Label ID="lblPercentage1" runat="server" Text="%"></asp:Label>
                    <cc1:FilteredTextBoxExtender ID="filterP1" runat="server" FilterType="Numbers"
                        TargetControlID="txtP1" Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </asp:Panel>
            <asp:Panel ID="panP2" runat="server">
                <td style="width: 75px; vertical-align: top" class="Percentile_ValueWidth">
                    <asp:CheckBox ID="chkP2" runat="server" class="Percentile_Checkbox" />
                    <asp:TextBox ID="txtP2" MaxLength="3" runat="server" Width="30px"></asp:TextBox>
                    <asp:Label ID="lblPercentage2" runat="server" Text="%"></asp:Label>
                    <cc1:FilteredTextBoxExtender ID="filterP2" runat="server" FilterType="Numbers"
                        TargetControlID="txtP2" Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </asp:Panel>
            <asp:Panel ID="panP3" runat="server">
                <td style="width: 75px; vertical-align: top" class="Percentile_ValueWidth">
                    <asp:CheckBox ID="chkP3" runat="server" class="Percentile_Checkbox" />
                    <asp:TextBox ID="txtP3" MaxLength="3" runat="server" Width="30px"></asp:TextBox>
                    <asp:Label ID="lblPercentage3" runat="server" Text="%"></asp:Label>
                    <cc1:FilteredTextBoxExtender ID="filterP3" runat="server" FilterType="Numbers"
                        TargetControlID="txtP3" Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                </td>
            </asp:Panel>
            <td style="vertical-align:top;">
                <asp:Image ID="imgError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Style="vertical-align: top" />
            </td>
        </tr>
    </asp:Panel>
</table>
