<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyRVPCOVID19.ascx.vb"
    Inherits="HCSP.ucReadOnlyRVPCOVID19" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>

<table style="border-collapse:collapse">
     <%--<tr id="trCategoryForCovid19" runat="server" visible="false">
        <td id="tdCategoryForCovid19" runat="server" class="tableCellStyle" style="vertical-align: top">
             <asp:Label ID="lblCategoryForCovid19Text" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblCategoryForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>--%>
    <tr>
        <td id="tdRCHCode" runat="server" valign="top" class="tableCellStyle">
            <asp:Label ID="lblRCHCodeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td id="tdRCHName" runat="server" valign="top" class="tableCellStyle">
            <asp:Label ID="lblRCHNameText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHName" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td id="tdCategoryForCovid19" runat="server" class="tableCellStyle" style="vertical-align: top">
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
