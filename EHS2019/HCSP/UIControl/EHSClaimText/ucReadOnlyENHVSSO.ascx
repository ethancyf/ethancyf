<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyENHVSSO.ascx.vb"
    Inherits="HCSP.UIControl.EHCClaimText.ucReadOnlyENHVSSO" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>
<table border="0" style="padding:0px;border-collapse:collapse;border-spacing:0px" class="textVersionTable">
    <tr>
        <td>
            <asp:Label ID="lblCategoryText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trPlaceOfVaccinationText" runat="server">
        <td>
            <asp:Label ID="lblPlaceOfVaccinationText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr id="trPlaceOfVaccination" runat="server">
        <td>
            <asp:Label ID="lblPlaceOfVaccination" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
</table>
<cc1:ClaimVaccineReadOnlyText ID="udcClaimVaccineReadOnlyText" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />
