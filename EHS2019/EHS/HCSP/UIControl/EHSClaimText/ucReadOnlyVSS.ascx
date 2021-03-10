<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyVSS.ascx.vb"
    Inherits="HCSP.UIControl.EHCClaimText.ucReadOnlyVSS" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>
<table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
    <asp:Panel ID="panCategory" runat="server">
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
    </asp:Panel>
    <asp:Panel ID="panMainCategory" runat="server" Visible="false">
    <tr>
        <td>
            <asp:Label ID="lblMainCategoryText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblMainCategory" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    </asp:Panel>
    <asp:Panel ID="panSubCategory" runat="server">
    <tr>
        <td>
            <asp:Label ID="lblSubCategoryText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblSubCategory" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    </asp:Panel>

    <tr id="trDocumentaryProofText" runat="server">
        <td>
            <asp:Label ID="lblDocumentaryProofText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr id="trDocumentaryProof" runat="server">
        <td>
            <asp:Label ID="lblDocumentaryProof" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <asp:Panel ID="panVSSDAConfirm" runat="server" visible="false" >
    <tr id="trDocProofCSSA" runat="server">
 
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
    <tr><td>&nbsp;</td></tr>
    </asp:Panel>
    <tr id="trPIDInstitutionCodeText" runat="server">
        <td>
            <asp:Label ID="lblPIDInstitutionCodeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr id="trPIDInstitutionCode" runat="server">
        <td>
            <asp:Label ID="lblPIDInstitutionCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trPIDInstitutionNameText" runat="server">
        <td>
            <asp:Label ID="lblPIDInstitutionNameText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr id="trPIDInstitutionName" runat="server">
        <td>
            <asp:Label ID="lblPIDInstitutionName" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trPlaceOfVaccinationText" runat="server">
        <td>
            <asp:Label ID="lblPlaceOfVaccinationText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr id="trPlaceOfVaccination" runat="server">
        <td>
            <asp:Label ID="lblPlaceOfVaccination" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <asp:Panel ID="panCOVID19Vaccine" runat="server">
    <tr>
        <td>
            <asp:Label ID="lblVaccineTextForCovid19" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblVaccineForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblVaccineLotNumTextForCovid19" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblVaccineLotNumForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td >
            <asp:Label ID="lblDoseTextForCovid19" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td >
            <asp:Label ID="lblDoseForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td >
            <asp:Label ID="lblContactNoTextForCovid19" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td >
            <asp:Label ID="lblContactNoForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td >
            <asp:Label ID="lblRemarksTextForCovid19" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td >
            <asp:Label ID="lblRemarksForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trJoinEHRSSText" runat="server">
        <td >
            <asp:Label ID="lblJoinEHRSSTextForCovid19" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr  id="trJoinEHRSS" runat="server">
        <td >
            <asp:Label ID="lblJoinEHRSSForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    </asp:Panel>
</table>
<cc1:ClaimVaccineReadOnlyText ID="udcClaimVaccineReadOnlyText" runat="server" CssTableText="tableText"
    CssTableTitle="tableTitle" />
