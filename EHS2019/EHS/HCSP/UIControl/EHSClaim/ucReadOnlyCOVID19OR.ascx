<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyCOVID19OR.ascx.vb"
    Inherits="HCSP.ucReadOnlyCOVID19OR" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>

<table style="border-collapse:collapse">
     <%--<tr style="display:none">
        <td runat="server" class="tableCellStyle" style="vertical-align: top">
             <asp:Label ID="lblCategoryForCovid19Text" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblCategoryForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>--%>
    <tr>
        <td id="tdOutreachCode" runat="server" valign="top" class="tableCellStyle">
            <asp:Label ID="lblOutreachCodeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblOutreachCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td id="tdOutreachName" runat="server" valign="top" class="tableCellStyle">
            <asp:Label ID="lblOutreachNameText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblOutreachName" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td id="tdCategoryForCovid19" runat="server" class="tableCellStyle" style="vertical-align: top">
             <asp:Label ID="lblMainCategoryForCovid19Text" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblMainCategoryForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trSubCategory" runat="server">
        <td class="tableCellStyle" style="vertical-align: top">
             <asp:Label ID="lblSubCategoryForCovid19Text" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblSubCategoryForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblVaccineForCovid19Text" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblVaccineForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblVaccineLotNumForCovid19Text" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblVaccineLotNumForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblDoseForCovid19Text" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblDoseForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>

</table>
