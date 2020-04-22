<%@ Control Language="vb" AutoEventWireup="false" Codebehind="reportCriteriaPeriodFromToDateTime.ascx.vb"
    Inherits="HCVU.reportCriteriaPeriodFromToDateTime" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td style="width: 200px" align="left">
            <asp:Label ID="lblInput01" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, EffectiveDateTimeFrom %>"></asp:Label></td>
        <td style="width: 250px" align="left">
            <asp:TextBox ID="txtPeriodFrom" MaxLength="10" runat="server" Width="80px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="filterPeriodFromDate" runat="server" FilterType="Custom, Numbers"
                TargetControlID="txtPeriodFrom" ValidChars="-" Enabled="True">
            </cc1:FilteredTextBoxExtender>
            <asp:ImageButton ID="btnFromDate" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />            
            <cc1:CalendarExtender ID="calExFromDate" runat="server" PopupButtonID="btnFromDate"
                TargetControlID="txtPeriodFrom" Enabled="True">
            </cc1:CalendarExtender>
            <asp:TextBox ID="txtFromTime" runat="server" Width="40" MaxLength="5"></asp:TextBox>(HH:mm)
            <cc1:MaskedEditExtender ID="maskedFromTime" runat="server" Mask="99:99" MaskType="Time" UserTimeFormat="TwentyFourHour" TargetControlID="txtFromTime" PromptCharacter="_"></cc1:MaskedEditExtender>
            <asp:Image ID="imgPeriodFromError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
        </td>
        <td style="width: 200px" align="left">
            <asp:Label ID="lblInput02" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, EffectiveDateTimeTo %>"></asp:Label></td>
        <td style="width: 250px" align="left">
            <asp:TextBox ID="txtPeriodTo" MaxLength="10" runat="server"  Width="80px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="filterPeriodToDate" runat="server" FilterType="Custom, Numbers"
                TargetControlID="txtPeriodTo" ValidChars="-" Enabled="True">
            </cc1:FilteredTextBoxExtender>
            <asp:ImageButton ID="btnToDate" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />            
            <cc1:CalendarExtender ID="calExToDate" runat="server" PopupButtonID="btnToDate" TargetControlID="txtPeriodTo"
                Enabled="True">
            </cc1:CalendarExtender>
            <asp:TextBox ID="txtToTime" runat="server" Width="40" MaxLength="5"></asp:TextBox>(HH:mm)
            <cc1:MaskedEditExtender ID="maskedToTime" runat="server" Mask="99:99" MaskType="Time" UserTimeFormat="TwentyFourHour" TargetControlID="txtToTime" PromptCharacter="_"></cc1:MaskedEditExtender>
            <asp:Image ID="imgPeriodToError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
        </td>
    </tr>
</table>
