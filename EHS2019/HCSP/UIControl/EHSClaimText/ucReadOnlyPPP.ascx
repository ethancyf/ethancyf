<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyPPP.ascx.vb"
    Inherits="HCSP.UIControl.EHCClaimText.ucReadOnlyPPP" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>
<table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
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
    <tr id="trSchoolCodeText" runat="server">
        <td>
            <asp:Label ID="lblSchoolCodeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr id="trSchoolCode" runat="server">
        <td>
            <asp:Label ID="lblSchoolCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trSchoolNameText" runat="server">
        <td>
            <asp:Label ID="lblSchoolNameText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr id="trSchoolName" runat="server">
        <td>
            <asp:Label ID="lblSchoolName" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
</table>
<cc1:ClaimVaccineReadOnlyText ID="udcClaimVaccineReadOnlyText" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />
