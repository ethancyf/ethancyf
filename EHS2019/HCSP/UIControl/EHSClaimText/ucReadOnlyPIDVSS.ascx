<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyPIDVSS.ascx.vb"
    Inherits="HCSP.UIControl.EHCClaimText.ucReadOnlyPIDVSS" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>
<table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">   
    <tr>
        <td>
            <asp:Label ID="lblDocumentaryProofText" runat="server" CssClass="tableTitle"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblDocumentaryProof" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
</table>
<cc1:ClaimVaccineReadOnlyText ID="udcClaimVaccineReadOnlyText" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />
