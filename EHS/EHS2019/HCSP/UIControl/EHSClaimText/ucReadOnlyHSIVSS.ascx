<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyHSIVSS.ascx.vb"
    Inherits="HCSP.UIControl.EHCClaimText.ucReadOnlyHSIVSS" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <asp:Label ID="lblCategoryText" runat="server" CssClass="tableTitle"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
</table>
<asp:Panel ID="panPreConditions" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr>
            <td>
                <asp:Label ID="lblPreConditionsText" runat="server" CssClass="tableTitle"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblPreConditions" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
<cc1:ClaimVaccineReadOnlyText ID="udcClaimVaccineReadOnlyText" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />
