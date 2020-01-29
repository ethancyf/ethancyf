<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputVoucherSlim.ascx.vb" Inherits="HCSP.ucInputVoucherSlim" %>
<%@ Register Src="~/UIControl/Assessories/ucNumPad.ascx" TagName="ucNumPad" TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc1" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
  
</script>
<table cellpadding="0" cellspacing="0" style="width: 753px">
    <tr id="trAvailableVoucher" runat="server">
        <td valign="top" style="width: 185px" class="tableCellStyle">
            <asp:Label ID="lblAvailableVoucherText" runat="server" CssClass="tableTitle" Width="160px"></asp:Label></td>
        <td valign="top" class="tableCellStyle">
            <asp:Label ID="lblAvailableVoucher" runat="server" CssClass="tableText">$0</asp:Label></td>
    </tr>
    <tr>
        <td valign="middle" style="width: 185px;" class="tableCellStyle">
            <asp:Label ID="lblVoucherRedeemText" runat="server" CssClass="tableTitle" Width="160px"></asp:Label></td>
        <td valign="top" class="tableCellStyle">
            <table id="tblVoucherRedeemRead" runat="server" cellpadding="0" cellspacing="0" style="display: none">
                <tr>
                    <td>
                        <asp:Label ID="lblVoucherRedeem" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
            <table id="tblVoucherRedeemWrite" runat="server" cellpadding="0" cellspacing="0" style="display: block;">
                <tr>
                    <td valign="top" id="cellVoucherRedeem" runat="server">
                        <asp:RadioButtonList ID="rbVoucherRedeem" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
                        </asp:RadioButtonList></td>
                    <td valign="top">
                        <asp:TextBox ID="txtVoucherRedeem" runat="server" AutoCompleteType="Disabled" AutoPostBack="false"
                            Enabled="true" MaxLength="2" Width="26px"></asp:TextBox></td>
                    <td valign="top">
                        <asp:Image ID="imgVoucherRedeemError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                    <td valign="middle">
                        <table id="tblTextInput" runat="server" cellpadding="0" cellspacing="0" style="display: block;">
                            <tr>
                                <td valign="middle">
                                    <span class="tableText">&nbsp;($&nbsp;</span></td>
                                <td valign="top">
                                    <asp:TextBox ID="txtTotalAmount" runat="server" Width="50px" AutoPostBack="false">50</asp:TextBox>
                                    <asp:Button ID="DummyNoticeTargetControl" runat="server" Style="display: none" />
                                    <cc1:ModalPopupExtender ID="ModalPopupExtenderNotice" runat="server" TargetControlID="DummyNoticeTargetControl"
                                        PopupControlID="panNotice" BehaviorID="mdlPopup1" BackgroundCssClass="modalBackgroundTransparent"
                                        DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
                                    </cc1:ModalPopupExtender>
                                </td>
                                <td valign="top">
                                    <asp:Image ID="imgVoucherAmountError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                                <td valign="middle">
                                    <span class="tableText">)</span>
                                </td>
                                <td valign="middle">
                                    <!-- Numeric Up Down -->
                                    <asp:Image ID="imgDn" runat="server" ImageUrl="<%$ Resources:ImageUrl, DownIBtn%>"  ToolTip="<%$ Resources:AlternateText, DownIBtn%>" AlternateText="<%$ Resources:AlternateText, DownIBtn%>"
                                        Width="15px" Height="15px" Style="cursor: pointer" />
                                    <asp:Image ID="imgUp" runat="server" ImageUrl="<%$ Resources:ImageUrl, UpIBtn%>"  ToolTip="<%$ Resources:AlternateText, UpIBtn%>" AlternateText="<%$ Resources:AlternateText, UpIBtn%>"
                                        Width="15px" Height="15px" Style="cursor: pointer" />
                                    <cc1:NumericUpDownExtender ID="nupVoucher" runat="server" Minimum="0" Maximum="5000"
                                        Step="50" TargetControlID="txtTotalAmount" TargetButtonUpID="imgUp" TargetButtonDownID="imgDn"
                                        ServiceDownMethod="" ServiceUpMethod="">
                                    </cc1:NumericUpDownExtender>
                                    <!-- Filter -->
                                    <cc1:FilteredTextBoxExtender ID="filtereditVoucherRedeem" runat="server" FilterType="numbers"
                                        TargetControlID="txtVoucherRedeem">
                                    </cc1:FilteredTextBoxExtender>
                                    <cc1:FilteredTextBoxExtender ID="filtereditTotalAmount" runat="server" FilterType="numbers"
                                        TargetControlID="txtTotalAmount">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Panel ID="panNotice" runat="server" Style="display: none" Width="500px">
    <uc1:ucNoticePopUp ID="ucNoticePopUp" runat="server" NoticeMode="Notification" ButtonMode="OK" />
</asp:Panel>
