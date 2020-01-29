<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyVoucherSlim.ascx.vb" Inherits="HCVU.ucReadOnlyVoucherSlim" %>
<table id="tblHCVS" runat="server" cellspacing="0" cellpadding="0" border="0">
    <tr>
        <td colspan="2" style="width: auto; padding-top: 4px">
	        <table width="100%" cellpadding="0" cellspacing="0" border="0">
	            <tr>
                    <td style="width: 200px;">
                        <asp:Label ID="lblRedeemAmountText" runat="server" Text="<%$ Resources:Text, RedeemAmount %>"></asp:Label></td>
                    <td style="padding-left: 3px">
                        <asp:Label ID="lblRedeemAmount" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>




