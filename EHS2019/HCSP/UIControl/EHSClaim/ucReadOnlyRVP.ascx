<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyRVP.ascx.vb"
    Inherits="HCSP.ucReadOnlyRVP" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td class="tableCellStyle" valign="top">
            <asp:Panel ID="panClaimCategory" runat="server">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td runat="server" id="tdCategory">
                            <asp:Label ID="lblCategoryText" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td runat="server" id="tdRCHCode" valign="top" class="tableCellStyle">
            <asp:Label ID="lblRCHCodeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td valign="top" class="tableCellStyle">
            <asp:Label ID="lblRCHCode" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td runat="server" id="tdRCHName" valign="top" class="tableCellStyle">
            <asp:Label ID="lblRCHNameText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td valign="top" class="tableCellStyle">
            <asp:Label ID="lblRCHName" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
</table>

<cc1:ClaimVaccineReadOnly ID="udcClaimVaccineReadOnly" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />

<table style="border-collapse:collapse">
    <tr>
        <td colspan ="2" style="height:5px;padding:0px" />
    </tr>
    <tr id="trRecipientCondition" runat="server">
        <td style="vertical-align: top;width:205px;padding:0px">
            <asp:Label ID="lblRecipientConditionText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td style="vertical-align: top;padding:0px">
            <asp:Label ID="lblRecipientCondition" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
</table>