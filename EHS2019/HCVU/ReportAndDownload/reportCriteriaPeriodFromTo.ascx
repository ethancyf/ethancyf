<%@ Control Language="vb" AutoEventWireup="false" Codebehind="reportCriteriaPeriodFromTo.ascx.vb"
    Inherits="HCVU.reportCriteriaPeriodFromTo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table style="width: 100%" cellpadding="0" cellspacing="0">
    <tr>
        <td style="width: 150px" align="left">
            <asp:Label ID="lblReportID" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, PeriodFrom %>"></asp:Label></td>
        <td style="width: 480px" align="left">
            (A)<asp:TextBox ID="txtPeriodFrom" MaxLength="10" runat="server" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="filterPeriodFromDate" runat="server" FilterType="Custom, Numbers"
                TargetControlID="txtPeriodFrom" ValidChars="-" Enabled="True">
            </cc1:FilteredTextBoxExtender>
            <asp:ImageButton ID="btnFromDate" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />
            <asp:Image ID="imgPeriodFromError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
            <cc1:CalendarExtender ID="calExFromDate" runat="server" PopupButtonID="btnFromDate"
                TargetControlID="txtPeriodFrom" Enabled="True">
            </cc1:CalendarExtender>
            <asp:Label ID="Label1" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, To %>"></asp:Label>
            (B)<asp:TextBox ID="txtPeriodTo" MaxLength="10" runat="server" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
            <cc1:FilteredTextBoxExtender ID="filterPeriodToDate" runat="server" FilterType="Custom, Numbers"
                TargetControlID="txtPeriodTo" ValidChars="-" Enabled="True">
            </cc1:FilteredTextBoxExtender>
            <asp:ImageButton ID="btnToDate" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />
            <asp:Image ID="imgPeriodToError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
            <cc1:CalendarExtender ID="calExToDate" runat="server" PopupButtonID="btnToDate" TargetControlID="txtPeriodTo"
                Enabled="True">
            </cc1:CalendarExtender>
        </td>
    </tr>
</table>
