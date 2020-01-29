<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyHSIVSS.ascx.vb"
    Inherits="HCSP.ucReadOnlyHSIVSS" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td runat="server" id="tdCategory" valign="top" class="tableCellStyle">
            <asp:Label ID="lblCategoryText" runat="server" CssClass="tableTitle" Width="160px"></asp:Label>
        </td>
        <td valign="top" class="tableCellStyle">
            <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
</table>
<asp:Panel ID="panPreConditions" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td runat="server" id="tdPreConditions" valign="top" class="tableCellStyle">
                <asp:Label ID="lblPreConditionsText" runat="server" CssClass="tableTitle" Width="160px"></asp:Label>
            </td>
            <td valign="top" class="tableCellStyle">
                <asp:Label ID="lblPreConditions" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
<cc1:ClaimVaccineReadOnly ID="udcClaimVaccineReadOnly" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />
