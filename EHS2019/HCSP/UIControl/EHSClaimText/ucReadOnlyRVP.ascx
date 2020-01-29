<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyRVP.ascx.vb"
    Inherits="HCSP.UIControl.EHCClaimText.ucReadOnlyRVP" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>
<table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
    <tr>
        <td>
            <asp:Panel ID="panClaimCategory" runat="server">
                <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                    <tr>
                        <td>
                            <asp:Label ID="lblCategoryText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblRCHCodeText" runat="server" CssClass="tableTitle"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblRCHCode" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblRCHNameText" runat="server" CssClass="tableTitle"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblRCHName" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
</table>
<cc1:ClaimVaccineReadOnlyText ID="udcClaimVaccineReadOnlyText" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />
