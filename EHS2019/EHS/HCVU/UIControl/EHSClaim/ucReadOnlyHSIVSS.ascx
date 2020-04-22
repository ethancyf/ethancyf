<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyHSIVSS.ascx.vb"
    Inherits="HCVU.ucReadOnlyHSIVSS" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>
<table id="tblHSIVSS" runat="server" cellpadding="0" cellspacing="0">
    <tr style="height: 22px">
        <td id="tdCategory" runat="server" style="vertical-align: top">
            <asp:Label ID="lblCategoryText" runat="server" Text="<%$ Resources:Text, Category %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trPreConditions" runat="server">
        <td style="vertical-align: top">
            <asp:Label ID="lblPreConditionText" runat="server" Text="<%$ Resources:Text, PreConditions %>"></asp:Label></td>
        <td style="vertical-align: top">
            <asp:Label ID="lblPreCondition" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
</table>
<cc1:ClaimVaccineReadOnly ID="udcReadOnlyVaccine" runat="server" />
