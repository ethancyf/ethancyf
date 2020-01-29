<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="reportCriteriaPeriodType.ascx.vb" Inherits="HCVU.reportCriteriaPeriodType" %>

<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td style="width:180px">
            <asp:Label ID="lblPeriodTypeText" runat="server" Text="<PeriodTypeText>" CssClass="tableTitle"></asp:Label>
        </td>
        <td>
            <asp:RadioButtonList ID="rdolPeriodType" runat="server" AppendDataBoundItems="false" RepeatDirection="Horizontal">
            </asp:RadioButtonList>
        </td>
        <td>
            <asp:Image ID="imgErrorPeriodType" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                       ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false" />
        </td>
    </tr>
    <tr style="height:6px">
    </tr>
</table>
