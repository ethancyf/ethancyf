<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyCOVID19MEC.ascx.vb" Inherits="HCVU.ucReadOnlyCOVID19MEC" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>

<style>
    .ulReasonText{
        list-style-position: outside;
        /* For Chrome & Firefox */
        padding-inline-start: 15px;
        margin-block-start: 0px;
        /* For IE */
        margin: 0px;
        padding-left: 15px;
    }
</style>
    <table style="border-collapse:collapse; width: 1000px">
        <tr id="trPart1Title" runat="server">
            <td colspan="2" id="tdCategoryForCovid19" runat="server" class="tableCellStyle" style="vertical-align: top; font-weight: bold; padding-top: 10px;">
                <asp:Label ID="lblPartIForExemptionText" runat="server" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr id="trPart1Reason" runat="server">
              <td class="tableCellStyle" style="vertical-align: top; width: 205px">
                <asp:Label ID="lblPartIReasonForExemptionText" runat="server"></asp:Label>
            </td>
            <td class="tableCellStyle" style="vertical-align: top">
                <ul class="ulReasonText">
                <asp:Label ID="lblPartIForExemption" runat="server" CssClass="tableTitle"></asp:Label>
                </ul>
            </td>
        </tr>
        <tr id="trPart2Title" runat="server">
            <td class="tableCellStyle" colspan="2" style="vertical-align: top; font-weight: bold; padding-top: 10px;">
               <asp:Label ID="lblPartIIForExemptionText" runat="server" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr id="trPart2Brand1Title" runat="server">
            <td class="tableCellStyle" colspan="2" style="vertical-align: top; font-weight: bold">
               <asp:Label ID="lblComirnatyForExemptionText" runat="server"></asp:Label>
            </td>
        </tr>
        <tr id="trPart2Brand1Reason" runat="server">
            <td class="tableCellStyle" style="vertical-align: top;">
                <asp:Label ID="lblComirnatyReasonForExemptionText" runat="server"></asp:Label>
            </td>
            <td class="tableCellStyle" style="vertical-align: top">
                <ul class="ulReasonText">
                    <asp:Label ID="lblComirnatyReasonForExemption" runat="server" CssClass="tableTitle"></asp:Label>
                </ul>
            </td>
        </tr>
        <tr id="trPart2Brand2Title" runat="server">
            <td class="tableCellStyle" colspan="2" style="vertical-align: top; font-weight: bold">
                <asp:Label ID="lblCoronaVacForExemptionText" runat="server"></asp:Label>
            </td>
        </tr>
        <tr id="trPart2Brand2Reason" runat="server">
            <td class="tableCellStyle" style="vertical-align: top">
                <asp:Label ID="lblCoronaVacReasonForExemptionText" runat="server"></asp:Label>
            </td>
            <td class="tableCellStyle" style="vertical-align: top">
                <ul class="ulReasonText">
                    <asp:Label ID="lblCoronaVacReasonForExemption" runat="server" CssClass="tableTitle"></asp:Label>
                </ul>
            </td>
        </tr>
        <%-- %><tr>
            <td class="tableCellStyle" style="vertical-align: top">
                <asp:Label ID="lblIssueForExemptionText" runat="server"></asp:Label>
            </td>
            <td class="tableCellStyle" style="vertical-align: top">
                <asp:Label ID="lblIssueForExemption" runat="server" CssClass="tableText"></asp:Label>
            </td>
        </tr>--%>
        <tr>
            <td class="tableCellStyle" style="vertical-align: top">
                <asp:Label ID="lblValidForExemptionText" runat="server"></asp:Label>
            </td>
            <td class="tableCellStyle" style="vertical-align: top">
                <asp:Label ID="lblValidForExemption" runat="server" CssClass="tableText"></asp:Label>
            </td>
        </tr>
<%--        <tr>
            <td class="tableCellStyle" style="vertical-align: top">
              <asp:Label ID="lblJoinForExemptionText" runat="server"></asp:Label>
            </td>
            <td class="tableCellStyle" style="vertical-align: top">
                <asp:Label ID="lblJoinForExemption" runat="server" CssClass="tableText"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="tableCellStyle" style="vertical-align: top">
              <asp:Label ID="lblContactForExemptionText" runat="server"></asp:Label>
            </td>
            <td class="tableCellStyle" style="vertical-align: top">
                <asp:Label ID="lblContactForExemption" runat="server" CssClass="tableText"></asp:Label>
            </td>
        </tr>--%>

</table>
 
    <!--CRE20-0XX (Immu record) [End][Raiman] -->
