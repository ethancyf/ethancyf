<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyVoucherSlim.ascx.vb"
    Inherits="HCSP.ucReadOnlyVoucherSlim" %>
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