<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyPIDVSS.ascx.vb" Inherits="HCVU.ucReadOnlyPIDVSS" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>
<table id="tblPIDVSS" runat="server" cellpadding="0" cellspacing="0">
    <tr style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblDocumentaryProofTitle" runat="server" Text="<%$ Resources:Text, TypeOfDocumentaryProof %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblDocumentaryProof" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
</table>
<cc1:ClaimVaccineReadOnly ID="udcReadOnlyVaccine" runat="server" />