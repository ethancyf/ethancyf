<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyHCVS.ascx.vb"
    Inherits="HCSP.ucReadOnlyHCVS" %>

<asp:Panel ID="panDHCRelatedService" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr id="trDHCRelatedService" runat="server">
            <td class="tableCellStyle" valign="top">
                <asp:Label ID="lblDHCRelatedServiceText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DHCRelatedService%>" />
            </td>
            <td valign="top" class="tableCellStyle">
                    <asp:Label ID="lblDHCRelatedService" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblDHCRelatedServiceName" runat="server" CssClass="tableText"></asp:Label><!-- CRE20-006 DHC Intergation -->
            </td>   
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panClaimDetailNormal" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td class="tableCellStyle" style="width: 185px"
                valign="top">
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
<table cellpadding="0" cellspacing="0">
    <tr>
        <td class="tableCellStyle" style="width: 185px; height: auto;">
            <asp:Label ID="lblReasonVisitText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
        <td class="tableCellStyle" style="height: auto;">
            <div style="display: block">
                <asp:Label ID="lblReasonForVisit" runat="server" CssClass="tableText" ForeColor="Red"></asp:Label></div>
        </td>
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
                        <td class="tableCellStyle">
                            <asp:Label ID="lblRedeemAmountDetailText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" style="width: 600px; border-collapse: collapse; border: 1px solid gray">
                    <tbody>
                        <tr>
                            <td align="center" style="width: 200px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherAvailText" runat="server" CssClass="tableText"></asp:Label></td>
                            <td align="center" style="width: 200px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherRedeemText" runat="server" CssClass="tableText"></asp:Label></td>
                            <td align="center" style="width: 200px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherRemainText" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center" style="width: 200px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherAvail" runat="server" CssClass="tableText"></asp:Label></td>
                            <td align="center" style="width: 200px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherRedeem" runat="server" CssClass="tableText"></asp:Label></td>
                            <td align="center" style="width: 200px; height: 25px; border-collapse: collapse; border: 1px solid gray">
                                <asp:Label ID="lblVoucherRemain" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>