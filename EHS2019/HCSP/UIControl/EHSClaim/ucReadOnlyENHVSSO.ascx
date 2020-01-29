<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyENHVSSO.ascx.vb"
    Inherits="HCSP.ucReadOnlyENHVSSO" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>

<table style="border-collapse:collapse">
    <tr>
        <td id="tdCategory" runat="server" class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblCategoryText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trPlaceOfVaccination" runat="server">
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblPlaceOfVaccinationText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblPlaceOfVaccination" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>

</table>

<cc1:ClaimVaccineReadOnly ID="udcClaimVaccineReadOnly" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />

