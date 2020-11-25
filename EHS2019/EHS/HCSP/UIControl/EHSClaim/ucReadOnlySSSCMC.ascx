<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlySSSCMC.ascx.vb"
    Inherits="HCSP.ucReadOnlySSSCMC" %>

<table cellpadding="0" cellspacing="0" style="width: 850px" >
    <tr>
        <td colspan="3" style="padding-bottom:2px">
            <hr />
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblRegistrationFeeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SSSCMC_RegistrationFee%>" />
        </td>
        <td class="tableCellStyle" style="width:150px">
            <table>
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblRegistrationFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblRegistrationFee" runat="server" CssClass="tableText" />
                    </td>
                </tr>
            </table>
        </td>
        <td class="tableCellStyle">
            <asp:Label ID="lblRegistrationFeeRemark" runat="server" CssClass="tableText" />
        </td>
    </tr>
    <tr id="trRegistrationFeeWarning" runat="server">
        <td class="tableCellStyle" colspan="3">
            <div style="padding: 5px; border-width:1px; border-style:solid; border-color:red; background-color:rgba(255, 255, 153, 1);">
                <asp:Label ID="lblRegistrationFeeWarning" runat="server" CssClass="tableText" 
                    style="color:red;" Text="<%$ Resources:Text, SSSCMC_RegistrationFeeWarning%>" />
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="padding-bottom:2px">
            <hr />
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" colspan="3" style="padding-top:7px;padding-bottom:10px">
            <asp:Label ID="lblMedicalServiceInfoText" runat="server" CssClass="tableTitle" 
                style="text-decoration:underline;" Text="<%$ Resources:Text, SSSCMC_MedicalServiceInfo%>" />
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblConsultAndRegFeeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SSSCMC_ConsultAndSelfPaidFee%>" />
        </td>
        <td class="tableCellStyle" colspan="2" style="width:150px">
            <table style="display: block;">
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblConsultAndRegFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblConsultAndRegFee" runat="server" CssClass="tableText" />                        
                    </td>                                      
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblDrugFeeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SSSCMC_DrugFee%>" />
        </td>
        <td class="tableCellStyle" colspan="2" style="width:150px">
            <table style="display: block;">
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblDrugFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblDrugFee" runat="server" CssClass="tableText" />   
                    </td>                                          
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblInvestigationFeeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SSSCMC_InvestigationFee%>" />
        </td>
        <td class="tableCellStyle" colspan="2" style="width:150px">
            <table style="display: block;">
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblInvestigationFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblInvestigationFee" runat="server" CssClass="tableText" />   
                    </td>                                          
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblOtherFeeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SSSCMC_OtherFee%>" />
        </td>
        <td class="tableCellStyle" style="width:150px">
            <table style="display: block;">
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblOtherFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblOtherFee" runat="server" CssClass="tableText" />   
                    </td>                                          
                </tr>
            </table>
        </td>
        <td class="tableCellStyle">
            <table style="display: block;">
                <tr>
                    <td style="width:70px">
                        <asp:Label ID="lblOtherFeeRemarkText" runat="server" CssClass="tableText" Text="<%$ Resources:Text, SSSCMC_OtherFeeRemark%>" />
                    </td>
                    <td style="text-align:left;width:370px">
                        <asp:Label ID="lblOtherFeeRemark" runat="server" CssClass="tableText" />   
                    </td>     
                </tr>
            </table>         
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="width:200px" />
        <td class="tableCellStyle" colspan="2" style="width:200px">
            <table style="display: block;">
                <tr>
                    <td style="width:105px">
                        <hr />
                    </td>                                        
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblTotalAmountText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SSSCMC_TotalAmount%>" />
        </td>
        <td class="tableCellStyle" style="width:150px">
            <table>
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblTotalAmountRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblTotalAmount" runat="server" CssClass="tableText" />
                    </td>
                </tr>
            </table>
        </td>
        <td class="tableCellStyle">
            <asp:Label ID="lblTotalAmountRemark" runat="server" CssClass="tableText" Text="<%$ Resources:Text, SSSCMC_TotalAmountRemark%>" />
        </td>
    </tr>
<%--    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblActualTotalAmountText" runat="server" CssClass="tableTitle" Text="实际收取总服务费用<br>（ 1 至 4 项）" />
        </td>
        <td class="tableCellStyle" colspan="2" style="width:150px">
            <table>
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblActualTotalAmountRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblActualTotalAmount" runat="server" CssClass="tableText" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>--%>
    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblPaidFeeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SSSCMC_Reduction%>" />
        </td>
        <td class="tableCellStyle" style="width:150px">
            <table>
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblPaidFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblPaidFee" runat="server" CssClass="tableText" />
                    </td>
                </tr>
            </table>
        </td>
        <td class="tableCellStyle" rowspan="3">
            <table style="width:420px;border-color:black;border-style:dotted;border-width:3px;padding:10px">
                <tr>
                    <td style="width:300px">
                        <asp:Label ID="lblTotalSupportFee1Text" runat="server" CssClass="tableText" Text="<%$ Resources:Text, SSSCMC_TotalSupportFee1%>" />
                    </td>
                    <%--<td style="width:10px">
                        <asp:Label ID="lblTotalSupportRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>--%>
                    <td style="width:114px;text-align:right">
                        <asp:Label ID="lblTotalSupportFee" runat="server" CssClass="tableText" />
                    </td>
                </tr>
                <tr>
                    <td colspan ="2">
                        <asp:Label ID="lblTotalSupportFee2Text" runat="server" CssClass="tableText" Text="<%$ Resources:Text, SSSCMC_TotalSupportFee2%>" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblNetServiceFeeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SSSCMC_NetServiceFee%>" />
        </td>
        <td class="tableCellStyle" style="width:150px">
            <table style="border-color:black;border-top-style:solid;border-top-width:thin;border-bottom-style:double;border-bottom-width:medium">
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblNetServiceFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblNetServiceFee" runat="server" CssClass="tableText" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblCoPaymentFeeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SSSCMC_CoPaymentFee%>" />
        </td>
        <td class="tableCellStyle" style="width:150px">
            <table style="border-color:black;border-bottom-style:solid;border-bottom-width:thin">
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblCoPaymentFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblCoPaymentFee" runat="server" CssClass="tableText" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="padding-bottom:2px">
            <br />
            <hr />
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblSubsidyBeforeUseText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SSSCMC_SubsidyBeforeUse%>" />
        </td>
        <td class="tableCellStyle" colspan="2" style="width:150px">
            <table>
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblSubsidyBeforeUseRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblSubsidyBeforeUse" runat="server" CssClass="tableText" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblSubsidyUsedText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SSSCMC_SubsidyUsed%>" />
        </td>
        <td class="tableCellStyle" colspan="2" style="width:150px">
            <table>
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblSubsidyUsedRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblSubsidyUsed" runat="server" CssClass="tableText" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tableCellStyle" style="width:200px">
            <asp:Label ID="lblSubsidyAfterUseText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SSSCMC_SubsidyAfterUse%>" />
        </td>
        <td class="tableCellStyle" colspan="2" style="width:150px">
            <table style="border-color:black;border-top-style:solid;border-top-width:thin;border-bottom-style:double;border-bottom-width:medium">
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblSubsidyAfterUseRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblSubsidyAfterUse" runat="server" CssClass="tableText" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="padding-bottom:2px">
            <br />
            <hr />
        </td>
    </tr>
</table>

