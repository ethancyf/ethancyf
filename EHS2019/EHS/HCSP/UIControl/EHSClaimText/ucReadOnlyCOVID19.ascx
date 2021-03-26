<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyCOVID19.ascx.vb"
    Inherits="HCSP.UIControl.EHCClaimText.ucReadOnlyCOVID19" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc1" %>

<table style="border-collapse: collapse">
    <tr>
        <td>
            <asp:Label ID="lblCategoryTextForCovid19" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblCategoryForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
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
    <%--    
    <tr runat="server">
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

    <tr runat="server">
        <td>
            <asp:Label ID="lblDoseTextForCovid19" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblDoseForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>

    <tr>
        <td>
            <asp:Label ID="lblRemarksTextForCovid19" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblRemarksForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trJoinEHRSSText" runat="server">
        <td>
            <asp:Label ID="lblJoinEHRSSTextForCovid19" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr id="trJoinEHRSS" runat="server">
        <td>
            <asp:Label ID="lblJoinEHRSSForCovid19" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>

</table>

