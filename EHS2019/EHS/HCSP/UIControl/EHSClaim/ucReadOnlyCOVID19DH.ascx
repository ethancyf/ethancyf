<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyCOVID19DH.ascx.vb"
    Inherits="HCSP.ucReadOnlyCOVID19DH" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>


<!--CRE20-0XX (Immu record) [Start][Raiman] -->
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
 <%--    
    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblContraindicationTextForCovid19" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">

            <table border="0" style="border-collapse: collapse; border-spacing:0px">
             <tr>
                <td style="width: 5px; vertical-align:top ">
                <asp:CheckBox ID="chkContraindicationForCovid19" runat="server" Enabled="false" checked="true" />
                </td>
                 <td><div style="max-width:650px">
                    <asp:Label ID="chkContraindicationTextForCovid19" runat="server" CssClass="tableText"></asp:Label></div>
                    </td>
                </tr>
             </table>
        </td>
    </tr>--%>

    <tr>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblDoseForCovid19Text" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="vertical-align: top">
            <asp:Label ID="lblDoseForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>

</table>
    <!--CRE20-0XX (Immu record) [End][Raiman] -->