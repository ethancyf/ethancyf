<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyVSS.ascx.vb" Inherits="HCVU.ucReadOnlyVSS" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>
<table cellpadding="0" cellspacing="0">
    <tr style="height: 22px">
        <td id="tdCategory" runat="server" style="vertical-align: top"   >
            <asp:Label ID="lblCategoryText" runat="server" Text="<%$ Resources:Text, Category %>"></asp:Label>
        </td>
        <td style="vertical-align: top" > 
            <!--- CRE20-009 prevent the text oversize the screen [Start][Nichole]-->
            <div style="max-width:700px">
            <asp:Label ID="lblCategory" runat="server" CssClass="tableText"></asp:Label>
            </div>
            <!--- CRE20-009 prevent the text oversize the screen [End][Nichole]-->
        </td>
    </tr>
    <tr id="trDocumentaryProof" runat="server" style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblDocumentaryProofText" runat="server" Text="<%$ Resources:Text, TypeOfDocumentaryProof %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <!--- CRE20-009 prevent the text oversize the screen [Start][Nichole]-->
             <div style="max-width:700px">
            <asp:Label ID="lblDocumentaryProof" runat="server" CssClass="tableText"></asp:Label>
                 </div>
            <!--- CRE20-009 prevent the text oversize the screen [End][Nichole]-->
        </td>
    </tr>
    <!---CRE20-009 VSS DA with CSSA [Start][Nichole] -->
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
                            <td>
                            <div style="max-width:650px">
                            <asp:Label ID="lblDocProofCSSA" runat="server" CssClass="tableText"  Text="<%$ Resources:Text, ProvidedInfoCSSA %>"></asp:Label>
                            </div>
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
                            <asp:CheckBox ID="chkDocProofAnnex" runat="server" Enabled="false"  checked="true" />
                            </td>
                            <td>
                            <div style="max-width:650px">
                            <asp:Label ID="lblDocProofAnnex" runat="server" CssClass="tableText"  Text="<%$ Resources:Text, ProvidedInfoAnnex %>"></asp:Label>
                            </div>
                            </td>
                        </tr>
            </table>
        </td>
    </tr>
    <tr><td colspan="2">&nbsp;</td></tr>
    </asp:Panel>
    <!--CRE20-009 VSS DA with CSSA [End][Nichole]-->
    <tr id="trPIDInstitutionCode" runat="server" style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblPIDInstitutionCodeText" runat="server" Text="<%$ Resources:Text, PIDInstitutionCode %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblPIDInstitutionCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trPIDInstitutionName" runat="server" style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblPIDInstitutionNameText" runat="server" Text="<%$ Resources:Text, PIDInstitutionName %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblPIDInstitutionName" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trPlaceOfVaccination" runat="server" style="height: 22px">
        <td style="vertical-align: top">
            <asp:Label ID="lblPlaceOfVaccinationText" runat="server" Text="<%$ Resources:Text, PlaceOfVaccination %>"></asp:Label>
        </td>
        <td style="vertical-align: top">
            <asp:Label ID="lblPlaceOfVaccination" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
</table>

<cc1:ClaimVaccineReadOnly ID="udcReadOnlyVaccine" runat="server" />

<table style="border-collapse:collapse">
    <tr id="trRecipientCondition" runat="server" visible="false">
        <td style="vertical-align: top;width:205px;padding:0px">
            <asp:Label ID="lblRecipientConditionText" runat="server" Text="<%$ Resources:Text, RecipientCondition %>">></asp:Label>
        </td>
        <td style="vertical-align: top;padding:0px">
            <asp:Label ID="lblRecipientCondition" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan ="2" style="height:2px;padding:0px" />
    </tr>
</table>

<table style="border-collapse:collapse">
    <tr id="trContactNo"  runat="server" visible="false">
        <td id="tdContactNo" runat="server" class="tableCellStyle" style="vertical-align: top;width:205px;padding:0px">
            <asp:Label ID="lblContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo2 %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top;padding:0px; height:23px;">
            <asp:Label ID="lblContactNo" runat="server" CssClass="tableText"></asp:Label>
            <asp:Label ID="lblContactNoNotAbleSMS" runat="server" CssClass="tableText" Text="<%$ Resources:Text, NotAbleToReceiveSMS%>" style="color:red!important" visible="false" />
        </td>
    </tr>
     <tr id="trRemarks" runat="server" visible="false">
        <td id="tdRemarks" runat="server" class="tableCellStyle" style="vertical-align: top;width:205px;padding:0px">
            <asp:Label ID="lblRemarksText" runat="server" Text="<%$ Resources:Text, Remarks %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top;padding:0px; height:23px;">
            <asp:Label ID="lblRemarks" runat="server" CssClass="tableText"></asp:Label>            
        </td>
    </tr>
</table>