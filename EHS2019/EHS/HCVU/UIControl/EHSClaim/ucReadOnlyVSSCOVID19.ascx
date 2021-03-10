<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyVSSCOVID19.ascx.vb" Inherits="HCVU.ucReadOnlyVSSCOVID19" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>

<table style="border-collapse:collapse">
     <%--<tr >
        <td id="tdCategoryForCovid19" runat="server" class="tableCellStyle" style="vertical-align: top">
             <asp:Label ID="lblCategoryForCovid19Text" runat="server" Text="<%$ Resources:Text, Category %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblCategoryForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>--%>
    <tr>
        <td id="tdCategoryForCovid19" runat="server" class="tableCellStyle" style="vertical-align: top">
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
