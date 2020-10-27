<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyVSS.ascx.vb"
    Inherits="HCSP.ucReadOnlyVSS" %>
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
    <tr id="trDocumentaryProof" runat="server">
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblDocumentaryProofText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblDocumentaryProof" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
     
    <!---CRE20-009 VSS DA With CSSA [Start][Nichole] -->
    <asp:Panel ID="panVSSDAConfirm" runat="server" visible="false" >
    <tr id="trDocProofCSSA" runat="server">
        <td class="tableCellStyle" style="vertical-align: top">
            
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
             <table border="0" style="border-collapse: collapse; border-spacing:0px">
             <tr>
                <td style="width: 5px; vertical-align:top ">
                <asp:CheckBox ID="chkDocProofCSSA" runat="server" Enabled="false" checked="true" />
                </td>
                 <td><div style="max-width:650px">
                    <asp:Label ID="lblDocProofCSSA" runat="server" CssClass="tableText"></asp:Label></div>
                    </td>
                </tr>
             </table>
        </td>
    </tr>
    <tr id="trDocProofAnnex" runat="server">
        <td class="tableCellStyle" style="vertical-align: top">
            
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
             <table border="0" style="border-collapse: collapse; border-spacing:0px">
             <tr>
                <td style="width: 5px; vertical-align:top ">
            <asp:CheckBox ID="chkDocProofAnnex" runat="server" Enabled="false"  checked="true" /> </td>
                 <td><div style="max-width:650px">
            <asp:Label ID="lblDocProofAnnex" runat="server" CssClass="tableText"></asp:Label></div> </td>
                </tr>
             </table>
        </td>
    </tr>
    <tr><td colspan="2">&nbsp;</td></tr>
    </asp:Panel>
    <!--CRe20-009 VSS DA With CSSA [End][Nichole]-->
    <tr id="trPIDInstitutionCode" runat="server">
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblPIDInstitutionCodeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblPIDInstitutionCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trPIDInstitutionName" runat="server">
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblPIDInstitutionNameText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblPIDInstitutionName" runat="server" CssClass="tableText"></asp:Label>
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