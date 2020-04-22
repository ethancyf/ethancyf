<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyPPP.ascx.vb"
    Inherits="HCSP.ucReadOnlyPPP" %>
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
    <tr id="trSchoolCode" runat="server">
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblSchoolCodeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblSchoolCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trSchoolName" runat="server">
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblSchoolNameText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblSchoolName" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
</table>

<cc1:ClaimVaccineReadOnly ID="udcClaimVaccineReadOnly" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />
