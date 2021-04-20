<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyRVPCOVID19.ascx.vb"
    Inherits="HCSP.ucReadOnlyRVPCOVID19" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>

<table style="border-collapse:collapse">
     <%--<tr>
        <td id="tdFirstRow" runat="server" class="tableCellStyle" style="vertical-align: top">
             <asp:Label ID="lblCategoryForCovid19Text" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblCategoryForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td id="tdFirstRow" runat="server" valign="top" class="tableCellStyle">
            <asp:Label ID="lblOutreachTypeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblOutreachType" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>--%>
    <asp:Panel ID="panRCHCode" runat="server">
    <tr>
        <td id="tdRecipientType" runat="server" valign="top" class="tableCellStyle">
            <asp:Label ID="lblRecipientTypeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRecipientType" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHCodeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHNameText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblRCHName" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    </asp:Panel>
    <asp:Panel ID="panOutreachCode" runat="server">
    <tr>
        <td id="tdOutreachCode" runat="server" class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblOutreachCodeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblOutreachCode" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblOutreachNameText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblOutreachName" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
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
    </asp:Panel>
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
