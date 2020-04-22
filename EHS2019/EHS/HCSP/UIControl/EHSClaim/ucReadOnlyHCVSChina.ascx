<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyHCVSChina.ascx.vb"
    Inherits="HCSP.ucReadOnlyHCVSChina" %>
<table cellpadding="0" cellspacing="0">
    <tr>
        <td class="tableCellStyle" style="width: 185px" valign="top">
            <asp:Label ID="lblExchangeRateText" runat="server" CssClass="tableTitle"></asp:Label></td>
        <td valign="top">
            <asp:Label ID="lblExchangeRate" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
</table>
<asp:Panel ID="panClaimDetailNormal" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td class="tableCellStyle" style="width: 185px" valign="top">
                <asp:Label ID="lblRedeemAmountText" runat="server" CssClass="tableTitle"></asp:Label></td>
            <td valign="top">
                <asp:Label ID="lblRedeemAmount" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
<table id="tblCoPaymentFee" runat="server" cellpadding="0" cellspacing="0">
    <tr>
        <td class="tableCellStyle" style="width: 185px">
            <asp:Label ID="lblCoPaymentFeeText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle">
            <div style="display: block">
                <asp:Label ID="lblCoPaymentFee" runat="server" CssClass="tableText"></asp:Label></div>
        </td>
    </tr>
</table>
<table id="tblPaymentType" runat="server" cellpadding="0" cellspacing="0">
    <tr>
        <td class="tableCellStyle" style="width: 185px">
            <asp:Label ID="lblPaymentTypeTitle" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle">
            <div style="display: block">
                <asp:Label ID="lblPaymentType" runat="server" CssClass="tableText"></asp:Label></div>
        </td>
    </tr>
</table>

<table cellpadding="0" cellspacing="0">
    <tr>
        <td class="tableCellStyle" style="width: 185px; height: auto; vertical-align:top;">
            <asp:Label ID="lblReasonVisitText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="height: auto;">
            <div style="display: block">
                <asp:Label ID="lblReasonForVisit" runat="server" CssClass="tableText" ForeColor="Red"></asp:Label></div>
        </td>
<%--        <td>
   
        </td>--%>
    </tr>
</table>
 <div style="display: block; padding: 0px 0px 5px 0px">
    <!-- 1 column -->
    <table  cellpadding ="0" cellspacing ="0" width="900px">
    <tr><td><asp:PlaceHolder ID="phReasonForVisitSecondary" runat="server"></asp:PlaceHolder>
    </td></tr></table>
    <table id="tblReasonForVisit" runat="server"></table>
    </div>
<asp:Panel ID="panClaimDetail" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="900px">
        <tr>
            <td>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="tableCellStyle" colspan="2">
                            <asp:Label ID="lblRedeemAmountDetailText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" style="width: 500px; border-collapse: collapse; border: 1px solid gray">
                    <tbody>
                        <tr>
                            <td align="center" style="width: 200px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                </td>
                            <td align="center" style="width: 100px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblVoucherAvailText" runat="server" CssClass="tableText"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Image ID="imgExclaimLowProfile" runat="server" ImageUrl="<%$ Resources:ImageUrl, AfterRedeemRemarkIcon %>" ImageAlign="AbsMiddle"></asp:Image>
                                        </td>
                                    </tr>
                                    </table>
                            </td>
                            <td align="center" style="width: 100px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherRedeemText" runat="server" CssClass="tableText"></asp:Label></td>
                            <td align="center" style="width: 100px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherRemainText" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
<%--                        <tr>
                           <td align="left" style="width: 200px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblClaimSummaryTitleRMB" runat="server" CssClass="tableTitle"></asp:Label></td>
                            <td align="center" style="width: 100px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherAvailRMB" runat="server" CssClass="tableText"></asp:Label></td>
                            <td align="center" style="width: 100px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherRedeemRMB" runat="server" CssClass="tableText"></asp:Label></td>
                            <td align="center" style="width: 100px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherRemainRMB" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>--%>
                        <tr>
                           <td align="left" style="width: 200px; height: 25px; border-collapse: collapse; border: 1px solid gray; padding-left: 5px;">
                                <asp:Label ID="lblClaimSummaryTitleHKD" runat="server" CssClass="tableTitle"></asp:Label></td>
                            <td align="center" style="width: 100px; height: 25px; border-collapse: collapse; border: 1px solid gray">                                
                                <asp:Label ID="lblVoucherAvailHKD" runat="server" CssClass="tableText"></asp:Label></td>
                            <td align="center" style="width: 100px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherRedeemHKD" runat="server" CssClass="tableText"></asp:Label></td>
                            <td align="center" style="width: 100px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherRemainHKD" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>