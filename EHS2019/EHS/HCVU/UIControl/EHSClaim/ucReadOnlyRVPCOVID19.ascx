<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyRVPCOVID19.ascx.vb" Inherits="HCVU.ucReadOnlyRVPCOVID19" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>

<table style="border-collapse:collapse">
     <%--<tr>
        <td id="tdFirstRow" runat="server" class="tableCellStyle" style="vertical-align: top">
             <asp:Label ID="lblCategoryForCovid19Text" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Category %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblCategoryForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td id="tdFirstRow" runat="server" valign="top" class="tableCellStyle">
            <asp:Label ID="lblOutreachTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, OutreachType %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblOutreachType" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>--%>
    <asp:Panel ID="panRCHCode" runat="server">
    <tr>
        <td id="tdRecipientType" runat="server" valign="top" class="tableCellStyle">
            <asp:Label ID="lblRecipientTypeText" runat="server" Text="<%$ Resources:Text, RecipientType %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRecipientType" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHCodeText" runat="server" Text="<%$ Resources:Text, RCHCode %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHNameText" runat="server" Text="<%$ Resources:Text, RCHName %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHName" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    </asp:Panel>
    <asp:Panel ID="panOutreachCode" runat="server">
    <tr>
        <td id="tdOutreachCode" runat="server" class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblOutreachCodeText" runat="server" Text="<%$ Resources:Text, OutreachCode %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblOutreachCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblOutreachNameText" runat="server" Text="<%$ Resources:Text, OutreachName %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblOutreachName" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
             <asp:Label ID="lblMainCategoryForCovid19Text" runat="server" Text="<%$ Resources:Text, Category %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblMainCategoryForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trSubCategory" runat="server">
        <td class="tableCellStyle" style="vertical-align: top">
             <asp:Label ID="lblSubCategoryForCovid19Text" runat="server" Text="<%$ Resources:Text, SubCategory %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblSubCategoryForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblVaccineForCovid19Text" runat="server" Text="<%$ Resources:Text, Vaccines %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblVaccineForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblVaccineLotNumForCovid19Text" runat="server" Text="<%$ Resources:Text, VaccineLotNumber %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblVaccineLotNumForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top;padding-bottom:0px!important">
            <asp:Label ID="lblDoseSeqForCovid19Text" runat="server" Text="<%$ Resources:Text, DoseSeq %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top;padding-bottom:0px!important">
            <asp:Label ID="lblDoseSeqForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
   
    <cc1:ClaimVaccineReadOnly ID="udcReadOnlyVaccine" runat="server" visible="false"/>
</table>
 
    <!--CRE20-0XX (Immu record) [End][Raiman] -->
