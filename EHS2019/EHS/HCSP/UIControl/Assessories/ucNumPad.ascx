<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucNumPad.ascx.vb" Inherits="HCSP.ucNumPad" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
   
    
</script>

<table border="0" cellpadding="0" cellspacing="0">
    <%-- Header --%>
    <tr>
        <td colspan="3">
            <asp:Panel ID="panHeader" runat="server" Style="cursor: move;" Width="100%">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                        </td>
                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                            <asp:Label ID="lblTitle" runat="server" Text="<%$ Resources:Text, CoPaymentFeeCalculator%>"></asp:Label></td>
                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <%-- Content --%>
    <tr>
        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
            <table width="7px"><tr><td></td></tr></table>
        </td>
        <td style="background-color: #ffffff">
            <table cellpadding="1" cellspacing="0" style="width: 100%;">
                <tr>
                    <td>
                        <%------------------- Formula -----------------%>
                        <%---------------------------------------------%>
                        <table cellpadding="1" cellspacing="0" style="text-align: right; vertical-align: middle;
                            margin-right: 10px;">
                            <colgroup style="padding-left: 10px; text-align: left;" class="tableCellStyle" />
                            <colgroup style="padding-left: 10px; padding-right: 10px; text-align: left; font-size: 20px;"
                                class="tableCellStyle" />
                            <colgroup style="" class="tableCellStyle" />
                            <%-- Instruction --%>
                            <tr>
                                <td colspan="3" style="padding-bottom:5px">
                                    <asp:Label ID="lblInstruction" runat="server" CssClass="tableText" Text="<%$ Resources:Text, CoPaymentFeeCalculatorInstruction%>"  style="white-space:nowrap"/>
                                </td>
                            </tr>
                            <%-- Total Amount --%>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblTotalAmount" runat="server" CssClass="tableText" Text="<%$ Resources:Text, VoucherServiceFee%>" />
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDisplay" runat="server" MaxLength="6" Height="25px" Width="100px"
                                        Font-Size="20px" Style="text-align: right;" BackColor="#ECEBBD" Font-Names="Tahoma"
                                        ForeColor="#404040"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredDisplay" runat="server" FilterType="Numbers"
                                        TargetControlID="txtDisplay">
                                    </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <%-- Voucher Amount --%>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblVoucherAmount" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RedeemAmount%>" />
                                </td>
                                <td>
                                    -
                                </td>
                                <td align="right">
                                    <asp:Label ID="txtVoucherAmount" runat="server" Font-Names="Tahoma"  Font-Size="20px"></asp:Label>
                                </td>
                            </tr>
                            <%-- Result Separator --%>
                            <tr>
                                <td colspan="3">
                                    <hr />
                                </td>
                            </tr>
                            <%-- Co-Payment Fee --%>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblCoPaymentFee" runat="server" CssClass="tableText" Text="<%$ Resources:Text, CoPaymentFee%>" />
                                </td>
                                <td>
                                    =
                                </td>
                                <td>
                                    <asp:Label ID="txtCoPaymentFee" runat="server" Font-Names="Tahoma"  Font-Size="20px" Height="24px"></asp:Label>
                                </td>
                            </tr>
                            <%-- Button: Cancel, OK --%>
                            <tr>
                                <td colspan="3" align="center" style="padding-top: 10px;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr valign="top">
                                            <td>
                                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" />
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ibtnOK" runat="server" ImageUrl="<%$ Resources:ImageUrl, OKBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, OKBtn %>" />
                                                <asp:ImageButton ID="ibtnOKDisable" runat="server" ImageUrl="<%$ Resources:ImageUrl, OKDisableBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, OKDisableBtn %>" Style="display: none" OnClientClick="return false;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="background-color: lightgrey; border-left: thin solid gray">
                        <%--------------------- NumPad ----------------%>
                        <%---------------------------------------------%>
                        <table cellpadding="1" cellspacing="0" style="width: 100%;">
                            <%-- 7, 8, 9 --%>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ibtn7" runat="server" ImageUrl="<%$ Resources:ImageUrl, NumPad7Btn %>" AlternateText="<%$ Resources:AlternateText, NumPad7Btn %>"/></td>
                                <td>
                                    <asp:ImageButton ID="ibtn8" runat="server" ImageUrl="<%$ Resources:ImageUrl, NumPad8Btn %>" AlternateText="<%$ Resources:AlternateText, NumPad8Btn %>"/></td>
                                <td>
                                    <asp:ImageButton ID="ibtn9" runat="server" ImageUrl="<%$ Resources:ImageUrl, NumPad9Btn %>" AlternateText="<%$ Resources:AlternateText, NumPad9Btn %>"/></td>
                            </tr>
                            <%-- 4, 5, 6 --%>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ibtn4" runat="server" ImageUrl="<%$ Resources:ImageUrl, NumPad4Btn %>" AlternateText="<%$ Resources:AlternateText, NumPad4Btn %>"/></td>
                                <td>
                                    <asp:ImageButton ID="ibtn5" runat="server" ImageUrl="<%$ Resources:ImageUrl, NumPad5Btn %>" AlternateText="<%$ Resources:AlternateText, NumPad5Btn %>"/></td>
                                <td>
                                    <asp:ImageButton ID="ibtn6" runat="server" ImageUrl="<%$ Resources:ImageUrl, NumPad6Btn %>" AlternateText="<%$ Resources:AlternateText, NumPad6Btn %>"/></td>
                            </tr>
                            <%-- 1, 2, 3 --%>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ibtn1" runat="server" ImageUrl="<%$ Resources:ImageUrl, NumPad1Btn %>" AlternateText="<%$ Resources:AlternateText, NumPad1Btn %>"/></td>
                                <td>
                                    <asp:ImageButton ID="ibtn2" runat="server" ImageUrl="<%$ Resources:ImageUrl, NumPad2Btn %>" AlternateText="<%$ Resources:AlternateText, NumPad2Btn %>"/></td>
                                <td>
                                    <asp:ImageButton ID="ibtn3" runat="server" ImageUrl="<%$ Resources:ImageUrl, NumPad3Btn %>" AlternateText="<%$ Resources:AlternateText, NumPad3Btn %>"/></td>
                            </tr>
                            <%-- 0   , C --%>
                            <tr>
                                <td>
                                    <asp:ImageButton ID="ibtnC" runat="server" ImageUrl="<%$ Resources:ImageUrl, NumPadCBtn %>" AlternateText="<%$ Resources:AlternateText, NumPadCBtn %>"/></td>
                                <td colspan="2">
                                    <asp:ImageButton ID="ibtn0" runat="server" ImageUrl="<%$ Resources:ImageUrl, NumPad0Btn %>" AlternateText="<%$ Resources:AlternateText, NumPad0Btn %>"/></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
            <table width="7px"><tr><td></td></tr></table>
        </td>
    </tr>
    <tr>
        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
        </td>
        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
            height: 7px">
        </td>
        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
            height: 7px">
        </td>
    </tr>
</table>
