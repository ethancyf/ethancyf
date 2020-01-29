<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyENHVSSO.ascx.vb" Inherits="HCVU.ucReadOnlyENHVSSO" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>
<table cellpadding="0" cellspacing="0">
    <tr style="height: 22px">
        <td id="tdCategory" runat="server" style="vertical-align: top">
            <asp:Label ID="lblCategoryText" runat="server" Text="<%$ Resources:Text, Category %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trPlaceOfVaccination" runat="server" style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblPlaceOfVaccinationText" runat="server" Text="<%$ Resources:Text, PlaceOfVaccination %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblPlaceOfVaccination" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
</table>

<cc1:ClaimVaccineReadOnly ID="udcReadOnlyVaccine" runat="server" />
