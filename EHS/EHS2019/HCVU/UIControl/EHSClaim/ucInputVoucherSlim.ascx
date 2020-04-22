<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputVoucherSlim.ascx.vb"
    Inherits="HCVU.ucInputVoucherSlim" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc2" %>
<table cellpadding="0" cellspacing="0" style="width: 753px">
    <tr style="height: 25px">
        <td valign="top" style="width: 200px" class="tableCellStyle">
            <asp:Label ID="lblAvailableVoucherText" runat="server" CssClass="tableTitle" Width="160px"></asp:Label></td>
        <td valign="top"  class="tableCellStyle" style="padding-left: 3px">
            <asp:Label ID="lblAvailableVoucher" runat="server" CssClass="tableText">$0</asp:Label></td>
    </tr>
    <tr>
        <td valign="top" style="width: 200px" class="tableCellStyle">
            <asp:Label ID="lblVoucherRedeemText" runat="server" CssClass="tableTitle" Width="160px"></asp:Label>
        </td>
        <td valign="top">
            <table cellpadding="0" cellspacing="0">
                <tbody>
                    <tr>
                        <td valign="top" id="cellVoucherRedeem" runat="server">
                            <asp:RadioButtonList ID="rbVoucherRedeem" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
                            </asp:RadioButtonList></td>
                        <td valign="top">
                            <asp:TextBox ID="txtVoucherRedeem" runat="server" AutoCompleteType="Disabled" AutoPostBack="True"
                                BackColor="Silver" Enabled="False" MaxLength="2" Width="26px"></asp:TextBox></td>
                        <td valign="middle">
                            <span class="tableText">&nbsp;</span></td>
                        <td valign="middle">
                            <asp:Label ID="lblTotalAmount" runat="server" CssClass="tableText">$0</asp:Label></td>
                        <td style="width: 760px" valign="top">
                            <asp:Image ID="imgVoucherRedeemError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                            <cc1:FilteredTextBoxExtender ID="filtereditVoucherRedeem" runat="server" FilterType="Numbers"
                                TargetControlID="txtVoucherRedeem">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
</table>