<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyRVP.ascx.vb"
    Inherits="HCVU.ucReadOnlyRVP" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>
<table id="tblRVP" runat="server" cellpadding="0" cellspacing="0">
    <tr style="height: 22px">
        <td id="tdCategory" runat="server" style="vertical-align: top">
            <asp:Label ID="lblCategoryText" runat="server" Text="<%$ Resources:Text, Category %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblRCHCodeText" runat="server" Text="<%$ Resources:Text, RCHCode %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblRCHCode" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblRCHNameText" runat="server" Text="<%$ Resources:Text, RCHName %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblRCHName" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
</table>

<cc1:ClaimVaccineReadOnly ID="udcReadOnlyVaccine" runat="server" />

<table style="border-collapse:collapse">
    <tr id="trRecipientCondition" runat="server" visible="false">
        <td style="vertical-align: top;width:205px;padding:0px">
            <asp:Label ID="lblRecipientConditionText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RecipientCondition %>">></asp:Label>
        </td>
        <td style="vertical-align: top;padding:0px">
            <asp:Label ID="lblRecipientCondition" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan ="2" style="height:2px;padding:0px" />
    </tr>
</table>