<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyVoucherSlim.ascx.vb"
    Inherits="HCSP.UIControl.EHCClaimText.ucReadOnlyVoucherSlim" %>
<asp:Panel ID="panClaimDetailNormal" runat="server">
    <table cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr>
            <td>
                <asp:Label ID="lblRedeemAmountText" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblRedeemAmount" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panClaimDetail" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td>
                    <asp:Label ID="lblRedeemAmountDetailText" runat="server" CssClass="tableTitle" Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVoucherAvailText" runat="server" CssClass="tableTitle" Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVoucherAvail" runat="server" CssClass="tableText" Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVoucherRedeemText" runat="server" CssClass="tableTitle" Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVoucherRedeem" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVoucherRemainText" runat="server" CssClass="tableTitle" Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblVoucherRemain" runat="server" CssClass="tableText" Width="100%"></asp:Label></td>
            </tr>
        </tbody>
    </table>
</asp:Panel>