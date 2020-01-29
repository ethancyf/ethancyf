<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputTokenPopup.ascx.vb"
    Inherits="HCSP.ucInputTokenPopup" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
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
                            <asp:Label ID="lblTitle" runat="server" Text="Undefine"></asp:Label></td>
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
            <table width="7px">
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </td>
        <td style="background-color: #ffffff">
            <table cellpadding="5" cellspacing="0" style="width: 100%;">
                <tr>
                    <td>
                        <%-------------------  Body   -----------------%>
                        <%---------------------------------------------%>
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <cc2:MessageBox ID="udcTMessageBox" runat="server" />
                                    <table style="width: 100%" border="0">
                                        <tr style="vertical-align: top">
                                            <td colspan="2">
                                                <asp:Label ID="lblTMessage" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top">
                                            <td style="padding-top: 25px; text-align: right; width: 180px">
                                                <asp:TextBox ID="txtTPasscode" runat="server" Width="100px" MaxLength="6" TextMode="Password"></asp:TextBox>
                                                <asp:Image ID="imgTPasscode" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Visible="False" />
                                            </td>
                                            <td>
                                                <asp:Image ID="imgTPasscodeHelp" runat="server" ImageUrl="<%$ Resources: ImageUrl, TokenPassCode %>"
                                                    AlternateText="<%$ Resources: AlternateText, Token %>" ToolTip="<%$ Resources: AlternateText, Token %>" />
                                            </td>
                                        </tr>
                                        <tr style="height: 10px">
                                        </tr>
                                        <tr>
                                            <td style="text-align: center" colspan="2">
                                                <asp:ImageButton ID="ibtnTCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, CancelBtn %>" ToolTip="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnTCancel_Click" />
                                                <asp:ImageButton ID="ibtnTConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" ToolTip="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnTConfirm_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
            <table width="7px">
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
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
