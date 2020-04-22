<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucReadOnlyHCVSChina.ascx.vb" Inherits="HCVU.ucReadOnlyHCVSChina" %>
<table id="tblHCVSChina" runat="server" cellspacing="0" cellpadding="0" border="0">
    <tr>
        <td colspan="2" style="width: auto; vertical-align:top">
	        <table width="100%" cellpadding="0" cellspacing="0" border="0">
	            <tr>
                    <td style="width: 200px;vertical-align: top;padding-top: 0px">
                        <asp:Label ID="lblExchangeRateText" runat="server" Text="<%$ Resources:Text, VoucherConversionRate %>"></asp:Label></td>
                    <td style="vertical-align:top; padding-left: 4px">
                        <asp:Label ID="lblExchangeRate" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="width:auto; padding-top:4px">
	        <table width="100%" cellpadding="0" cellspacing="0" border="0">
	            <tr>
                    <td style="width: 200px;vertical-align: top;padding-top: 0px">
                        <asp:Label ID="lblRedeemAmountText" runat="server" Text="<%$ Resources:Text, RedeemAmount %>"></asp:Label></td>
                    <td style="vertical-align:top; padding-left: 4px">
                        <asp:Label ID="lblRedeemAmount" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
	<tr runat="server" id="trCoPaymentFee"> 
	    <td colspan="2" style="width: auto;padding-top: 4px">
	        <table width="100%" cellspacing="0" cellpadding="0">
	            <tr>
	                <td style="width: 200px;vertical-align: top">
                        <asp:Label ID="lblCoPaymentFeeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, CoPaymentFee %>"></asp:Label>
                    </td>  
                    <td style="vertical-align: top;padding-left: 4px">
                        <asp:Label ID="lblCoPaymentFee" runat="server" CssClass="tableText"></asp:Label>
                    </td>   
	            </tr>
	        </table>
	    </td>
    </tr>
    <tr runat="server" id="tr1"> 
	    <td colspan="2" style="width: auto;padding-top: 4px">
	        <table width="100%" cellspacing="0" cellpadding="0">
	            <tr>
	                <td style="width: 200px;vertical-align: top">
                        <asp:Label ID="lblPaymentTypeTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, PaymentType %>"></asp:Label>
                    </td>  
                    <td style="vertical-align: top;padding-left: 4px">
                        <asp:Label ID="lblPaymentType" runat="server" CssClass="tableText"></asp:Label>
                    </td>   
	            </tr>
	        </table>
	    </td>
    </tr>
    <tr>
        <td colspan="2" style="width:auto; padding-top:4px">
	        <table width="100%" cellpadding="0" cellspacing="0" border="0">
	            <tr>
	                <td style="width: 200px; vertical-align: top">
                        <asp:Label ID="lblReasonVisitText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ReasonVisit %>"></asp:Label>
                    </td>
                    <td style="vertical-align: top;padding-left: 3px">
                        <asp:Label ID="lblReasonVisitToBeProvided" runat="server" CssClass="tableText"></asp:Label>
                    </td>
	            </tr>
	        </table>
	    </td>        
    </tr>
    <tr>		
         <td style="vertical-align: top;padding-left: 0px;padding-top: 0px; width: auto" colspan="2">
            <div id="divReasonForVisit" runat="server" style="padding-top: 4px">
                <table cellpadding="1" cellspacing="0" id="tblReasonForVisit" runat="server">
                    <tr id="trPrincipal" runat="server">
                        <td style="width: 200px; vertical-align: top; border-style: solid; border-color: Gray; border-width: thin">
                               <asp:Label ID="lblPrimary" runat="server" Text="<%$ Resources:Text, PrincipalReasonForVisit %>"></asp:Label>
                        </td>
                        <td style="vertical-align: top;border-style: solid; border-color: Gray; border-width: thin; border-left-width: 0px">
                            <table cellpadding="1" cellspacing="0" border="0">
                                <tr>
                                    <td style="vertical-align: top">
                                        <table cellpadding="1" cellspacing="1" border="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblReasonVisitL1" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                          <%--  <tr>
                                                <td style="padding-left: 15px">
                                                    <asp:Label ID="lblReasonVisitL2" runat="server" CssClass="tableText" ></asp:Label>
                                                </td>
                                            </tr>--%>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trSecondary" runat="server">
                        <td valign="top" style="width: 200px;vertical-align: top;border-style: solid; border-color: Gray; border-width: thin; border-top-width: 0px">
                            <asp:Label ID="lblSecondary" runat="server" Text="<%$ Resources:Text, SecondaryReasonForVisit %>"></asp:Label>
                        </td>
                        <td style="vertical-align: top;border-style: solid; border-color: Gray; border-width: thin; border-left-width: 0px; border-top-width: 0px">
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
</table>