<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputVoucherSlim.ascx.vb"
    Inherits="HCSP.UIControl.EHCClaimText.ucInputVoucherSlim" %>
<table cellpadding="0" cellspacing="0" class="textVersionTable">
    <tr>
        <td>
            <asp:Label ID="lblAvailableVoucherText" runat="server" CssClass="tableTitle"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblAvailableVoucher" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblVoucherRedeemText" runat="server" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" class="textVersionTable" id="tblVoucherRedeem" runat="server">
                    <tr>
                        <td style="width:220px">
                            <asp:RadioButtonList ID="rbVoucherRedeem" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
                            </asp:RadioButtonList>
                        </td>
                        <td valign="bottom">
                            <asp:TextBox ID="txtVoucherRedeem" runat="server" AutoCompleteType="Disabled" AutoPostBack="True"
                                BackColor="Silver" Enabled="False" MaxLength="2" Width="26px"></asp:TextBox>
                            <asp:Label ID="ErrVoucherRedeem" runat="server" CssClass="validateFailText" Text="*"
                                Visible="False"></asp:Label>
                        </td>
                    </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="panClaimDetailNormal" runat="server">
                <table cellpadding="0" cellspacing="0" class="textVersionTable">
                    <tr>
                        <td>
                            <asp:Label ID="lblNoOfvoucherredeemed" runat="server" CssClass="tableText"></asp:Label><asp:Label
                                ID="lblNomralTotalAmount" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr id="trTotalAmountText" runat="server">
        <td>
            <asp:Label ID="lblTotalAmountText" runat="server" CssClass="tableTitle"></asp:Label></td>
    </tr>
    <tr id="trTotalAmount" runat="server">
        <td>
            <asp:Label ID="lblTotalAmount" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
</table>