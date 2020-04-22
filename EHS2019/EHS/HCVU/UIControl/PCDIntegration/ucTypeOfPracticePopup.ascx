<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucTypeOfPracticePopup.ascx.vb"
    Inherits="HCVU.ucTypeOfPracticePopup" %>
<%@ Register Src="ucTypeOfPracticeGrid.ascx" TagName="ucTypeOfPracticeGrid" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<table border="0" cellpadding="0" cellspacing="0">
    <!-- Header -->
    <tr>
        <td colspan="3">
            <asp:Panel ID="panHeader" runat="server" Style="cursor: move;" Width="100%">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                        </td>
                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                            <asp:Label ID="lblTitle" runat="server" Text="<%$ Resources:Text, JoinPCD %>"></asp:Label></td>
                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <!-- Content -->
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
                        <cc2:MessageBox ID="udcTMessageBox" runat="server" />
                        <!--------------------------------------------->
                        <table cellpadding="1" cellspacing="0" style="vertical-align: middle; margin-right: 10px;">
                            <!-- Heading -->
                            <!-- Service Provider Detail -->
                            <tr>
                                <td colspan="3" style="padding-left: 10px;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 200px">
                                                <asp:Label ID="lblERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnkBtnERN" runat="server" CssClass="tableText" ForeColor="Blue" />
                                                <asp:Label ID="lblERN" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px">
                                                <asp:Label ID="lblSPNameText" runat="server" Text="<% $Resources:Text, ServiceProviderName %>"
                                                    CssClass="tableTitle"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSPEName" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:Label ID="lblSPCName" runat="server" CssClass="TextChi"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px">
                                                <asp:Label ID="lblHKICText" runat="server" Text="<% $Resources:Text, HKID %>" CssClass="tableTitle"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblHKIC" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px" valign="top">
                                                <asp:Label ID="lblSPAddressText" runat="server" Text="<% $Resources:Text, SPAddress%>"
                                                    CssClass="tableTitle"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSPAddress" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td style="width: 200px">
                                                <asp:Label ID="lblSPPhoneText" runat="server" Text="<%$ Resources:Text, ContactNo %>"
                                                    CssClass="tableTitle"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSPPhone" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px">
                                                <asp:Label ID="lblSPEmailText" runat="server" Text="<% $Resources:Text, Email %>"
                                                    CssClass="tableTitle"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSPEmail" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <!-- Practice List -->
                            <tr>
                                <td style="padding-left: 10px; padding-top: 10px;">
                                    <asp:Panel ID="panTypeOfPractice" runat="server" Width="100%" Height="400px" ScrollBars="Auto">
                                        <table cellpadding="1" cellspacing="0" width="98%" style="background-color:Gray">
                                            <tr>
                                                <td>
                                                    <uc1:ucTypeOfPracticeGrid ID="ucTypeOfPracticeGrid" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <!-- Button: Cancel, Join PCD -->
                            <tr>
                                <td colspan="3" align="center" style="padding-top: 10px;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr valign="top">
                                            <td>
                                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" ToolTip="<%$ Resources:AlternateText, CancelBtn %>" />&nbsp;
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ibtnJoinPCD" runat="server" ImageUrl="<%$ Resources:ImageUrl, JoinPCDBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, JoinPCDBtn_Short %>" ToolTip="<%$ Resources:AlternateText, JoinPCDBtn_Short %>" />
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
