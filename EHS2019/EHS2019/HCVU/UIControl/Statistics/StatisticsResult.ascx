<%@ Control Language="vb" AutoEventWireup="false" Codebehind="StatisticsResult.ascx.vb"
    Inherits="HCVU.StatisticsResult" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/ProfessionLegend.ascx" TagName="ucProfessionLegend"
    TagPrefix="uc4" %>
<%@ Register Src="~/UIControl/DistrictLegend.ascx" TagName="ucDistrictLegend" TagPrefix="uc5" %>

<table cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td align="left" style="width: 180px; vertical-align: top">
            <asp:Label ID="lblParameterTitle" runat="server" CssClass="tableTitle" />
        </td>
        <td align="left" style="width: 580px">
            <asp:Label ID="lblParameterValue" runat="server" CssClass="tableText" />
        </td>
    </tr>
    <tr style="height: 4px">
    </tr>
</table>
