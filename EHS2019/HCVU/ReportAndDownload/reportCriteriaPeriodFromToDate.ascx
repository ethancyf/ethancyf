<%@ Control Language="vb" AutoEventWireup="false" Codebehind="reportCriteriaPeriodFromToDate.ascx.vb"
    Inherits="HCVU.reportCriteriaPeriodFromToDate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td style="width: 180px">
            <asp:Label ID="lblFromDateText" runat="server" CssClass="tableTitle" Text="<FromDateText>"></asp:Label></td>
        <td style="width: 260px">
            <asp:TextBox ID="txtFromDate" MaxLength="10" runat="server" Width="80px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="filterFromDate" runat="server" FilterType="Custom, Numbers"
                TargetControlID="txtFromDate" ValidChars="-" Enabled="True">
            </cc1:FilteredTextBoxExtender>
            <asp:ImageButton ID="btnFromDate" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />
            <cc1:CalendarExtender ID="calExFromDate" CssClass="ajax_cal" runat="server" PopupButtonID="btnFromDate" TodaysDateFormat="d MMMM, yyyy"
                TargetControlID="txtFromDate" Enabled="True">
            </cc1:CalendarExtender>
            <asp:TextBox ID="txtFromTime" runat="server" Width="40" MaxLength="5"></asp:TextBox>
            <asp:Label ID="lblFromTimeRemark" runat="server" Text="(HH:mm)"></asp:Label>
            <cc1:MaskedEditExtender ID="maskedFromTime" runat="server" Mask="99:99" MaskType="Time"
                UserTimeFormat="TwentyFourHour" TargetControlID="txtFromTime" PromptCharacter="_">
            </cc1:MaskedEditExtender>
            <asp:Image ID="imgErrorFromDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
        </td>
        <td style="padding-right: 10px; padding-left: 10px;">
            <asp:Label ID="lblToDateText" runat="server" CssClass="tableTitle" Text="<ToDateText>"></asp:Label></td>
        <td style="width: 260px">
            <asp:TextBox ID="txtToDate" MaxLength="10" runat="server" Width="80px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="filterToDate" runat="server" FilterType="Custom, Numbers"
                TargetControlID="txtToDate" ValidChars="-" Enabled="True">
            </cc1:FilteredTextBoxExtender>
            <asp:ImageButton ID="btnToDate" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />
            <cc1:CalendarExtender ID="calExToDate" CssClass="ajax_cal" runat="server" PopupButtonID="btnToDate" TargetControlID="txtToDate" TodaysDateFormat="d MMMM, yyyy"
                Enabled="True">
            </cc1:CalendarExtender>
            <asp:TextBox ID="txtToTime" runat="server" Width="40" MaxLength="5"></asp:TextBox>
            <asp:Label ID="lblToTimeRemark" runat="server" Text="(HH:mm)"></asp:Label>
            <cc1:MaskedEditExtender ID="maskedToTime" runat="server" Mask="99:99" MaskType="Time"
                UserTimeFormat="TwentyFourHour" TargetControlID="txtToTime" PromptCharacter="_">
            </cc1:MaskedEditExtender>
            <asp:Image ID="imgErrorToDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
        </td>
    </tr>
    <tr style="height: 6px">
    </tr>
</table>
