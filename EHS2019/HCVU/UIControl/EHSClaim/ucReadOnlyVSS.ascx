<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyVSS.ascx.vb" Inherits="HCVU.ucReadOnlyVSS" %>
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
    <tr id="trDocumentaryProof" runat="server" style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblDocumentaryProofText" runat="server" Text="<%$ Resources:Text, TypeOfDocumentaryProof %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblDocumentaryProof" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trPIDInstitutionCode" runat="server" style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblPIDInstitutionCodeText" runat="server" Text="<%$ Resources:Text, PIDInstitutionCode %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblPIDInstitutionCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trPIDInstitutionName" runat="server" style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblPIDInstitutionNameText" runat="server" Text="<%$ Resources:Text, PIDInstitutionName %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblPIDInstitutionName" runat="server" CssClass="tableText"></asp:Label>
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