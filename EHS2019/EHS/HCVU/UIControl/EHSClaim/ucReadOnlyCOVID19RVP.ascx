<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyCOVID19RVP.ascx.vb" Inherits="HCVU.ucReadOnlyCOVID19RVP" %>
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
    <tr style="height: 22px">
        <td id="tdRCHCode" runat="server" class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHCodeText" runat="server" Text="<%$ Resources:Text, RCHCode %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHCode" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr style="height: 22px">
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHNameText" runat="server" Text="<%$ Resources:Text, RCHName %>"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHName" runat="server" CssClass="tableText"></asp:Label></td>
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
