<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyPIDVSS.ascx.vb"
    Inherits="HCSP.ucReadOnlyPIDVSS" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>

<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td runat="server" id="tdDocumentaryProof" valign="top" class="tableCellStyle">
            <asp:Label ID="lblDocumentaryProofText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td valign="top" class="tableCellStyle">
            <asp:Label ID="lblDocumentaryProof" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
</table>
<cc1:ClaimVaccineReadOnly ID="udcClaimVaccineReadOnly" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />
