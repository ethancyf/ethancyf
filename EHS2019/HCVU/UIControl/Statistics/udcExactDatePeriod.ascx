<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcExactDatePeriod.ascx.vb"
    Inherits="HCVU.udcExactDatePeriod" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table>
    <asp:Panel ID="panExactDatePeriod" runat="server">
        <tr class ="ExactDatePeriod_tr">
            <asp:Panel ID="panFromDate" runat="server">
                <td align="left" style="width: 150px;" class ="ExactDatePeriod_LabelWidth">
                    <asp:Label ID="lblFromDate" runat="server" CssClass="tableTitle" Text="From"></asp:Label>
                </td>
                <td style="width: 110px" class ="ExactDatePeriod_FromDateWidth">
                    <asp:TextBox ID="txtFromDate_D" MaxLength="10" runat="server" Width="80px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                        onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                        onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="filterFromDate_D" runat="server" FilterType="Custom, Numbers"
                        TargetControlID="txtFromDate_D" ValidChars="-" Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                    <asp:ImageButton ID="btnFromDate_D" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />
                    <cc1:CalendarExtender ID="calExFromDate_D" CssClass="ajax_cal" runat="server" PopupButtonID="btnFromDate_D"
                        TargetControlID="txtFromDate_D" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                    </cc1:CalendarExtender>
                </td>
            </asp:Panel>
            <asp:Panel ID="panToDate" runat="server">
                <td style="padding-right: 10px; padding-left: 10px;" class ="ExactDatePeriod_LabelToWidth">
                    <asp:Label ID="lblToDate" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, To_S %>"></asp:Label>
                </td>
                <td style="width: 110px" class ="ExactDatePeriod_ToDateWidth">
                    <asp:TextBox ID="txtToDate_D" MaxLength="10" runat="server" Width="80px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                        onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                        onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="filterToDate_D" runat="server" FilterType="Custom, Numbers"
                        TargetControlID="txtToDate_D" ValidChars="-" Enabled="True">
                    </cc1:FilteredTextBoxExtender>
                    <asp:ImageButton ID="btnToDate_D" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />
                    <cc1:CalendarExtender ID="calExToDate_D" CssClass="ajax_cal" runat="server" PopupButtonID="btnToDate_D"
                        TargetControlID="txtToDate_D" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                    </cc1:CalendarExtender>                    
                </td>
            </asp:Panel>
            <asp:Panel ID="panErrorDate" runat="server">
                <td>
                    <asp:Image ID="imgErrorDate_D" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                        Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Style="vertical-align: top" />
                </td>
            </asp:Panel>
        </tr>
    </asp:Panel>
</table>
