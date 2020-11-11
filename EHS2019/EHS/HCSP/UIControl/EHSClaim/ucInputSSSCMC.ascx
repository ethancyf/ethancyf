<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputSSSCMC.ascx.vb"
    Inherits="HCSP.ucInputSSSCMC" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
</script>

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
                    <td style="width:26px">
                        <asp:Label ID="lblConsultAndRegFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:70px;text-align:right">
                        <asp:TextBox ID="txtConsultAndRegFee" runat="server" Width="64px" AutoPostBack="false" style="text-align:right" MaxLength="8"/>                        
                        <cc1:FilteredTextBoxExtender ID="fteConsultAndRegFee" runat="server" FilterType="numbers, custom"
                            ValidChars="." TargetControlID="txtConsultAndRegFee">
                        </cc1:FilteredTextBoxExtender>  
                    </td> 
                    <td style="vertical-align:top">
                        &nbsp<asp:Image ID="imgConsultAndRegFeeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
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
                    <td style="width:26px">
                        <asp:Label ID="lblDrugFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:70px;text-align:right">
                        <asp:TextBox ID="txtDrugFee" runat="server" Width="64px" AutoPostBack="false" style="text-align:right" MaxLength="8"/>                        
                        <cc1:FilteredTextBoxExtender ID="fteDrugFee" runat="server" FilterType="numbers, custom"
                            ValidChars="." TargetControlID="txtDrugFee">
                        </cc1:FilteredTextBoxExtender>  
                    </td> 
                    <td style="vertical-align:top">
                        &nbsp<asp:Image ID="imgDrugFeeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
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
                    <td style="width:26px">
                        <asp:Label ID="lblInvestigationFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:70px;text-align:right">
                        <asp:TextBox ID="txtInvestigationFee" runat="server" Width="64px" AutoPostBack="false" style="text-align:right" MaxLength="8"/>                        
                        <cc1:FilteredTextBoxExtender ID="fteInvestigationFee" runat="server" FilterType="numbers, custom"
                            ValidChars="." TargetControlID="txtInvestigationFee">
                        </cc1:FilteredTextBoxExtender>  
                    </td> 
                    <td style="vertical-align:top">
                        &nbsp<asp:Image ID="imgInvestigationFeeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
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
                    <td style="width:26px">
                        <asp:Label ID="lblOtherFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:70px;text-align:right">
                        <asp:TextBox ID="txtOtherFee" runat="server" Width="64px" AutoPostBack="false" style="text-align:right" MaxLength="8"/>                        
                        <cc1:FilteredTextBoxExtender ID="fteOtherFee" runat="server" FilterType="numbers, custom"
                            ValidChars="." TargetControlID="txtOtherFee">
                        </cc1:FilteredTextBoxExtender>  
                    </td> 
                    <td style="vertical-align:top">
                        &nbsp<asp:Image ID="imgOtherFeeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
                    </td>                                          
                </tr>
            </table>
        </td>
        <td class="tableCellStyle">
            <table style="display: block;">
                <tr>
                    <td>
                        <asp:Label ID="lblOtherFeeRemarkText" runat="server" CssClass="tableText" Text="<%$ Resources:Text, SSSCMC_OtherFeeRemark%>" />
                    </td>
                    <td style="text-align:right">
                        <asp:TextBox ID="txtOtherFeeRemarkText" runat="server" Width="200px" AutoPostBack="false" MaxLength="255" /> 
                    </td> 
                    <td style="vertical-align:top">
                        &nbsp<asp:Image ID="imgOtherFeeRemarkError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
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
        <td class="tableCellStyle" style="width:200px;padding-bottom:8px">
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
                        <asp:Label ID="lblTotalSupportFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
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
            <table style="border-color:black;border-top-style:solid;border-top-width:thin;border-bottom-style:double;border-bottom-width:medium;display:inline-block">
                <tr>
                    <td style="width:10px">
                        <asp:Label ID="lblNetServiceFeeRMB" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>" />
                    </td>
                    <td style="width:84px;text-align:right">
                        <asp:Label ID="lblNetServiceFee" runat="server" CssClass="tableText" />
                    </td>
                </tr>
            </table>
            <div style="display:inline-block;position:relative;top:-6px">&nbsp<asp:Image ID="ImageNetServiceFeeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></div>
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


