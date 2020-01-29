<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="reimbursementSetCutoffDate.aspx.vb" Inherits="HCVU.reimbursementSetCutoffDate"
    Title="<%$ Resources:Title, ReimbursementSetCutoffDate %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="conMain" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="smScriptManager" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMain" runat="server">
        <ContentTemplate>
            <asp:Image ID="imgReimSetCutoffDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReimbursementSetCutoffDateBanner %>"
                AlternateText="<%$ Resources:AlternateText, ReimbursementSetCutoffDateBanner %>">
            </asp:Image>
            <asp:Panel ID="panMain" runat="server" Width="100%">
                <cc2:MessageBox ID="udcErrorBox" runat="server" Visible="False" Width="95%"></cc2:MessageBox>
                <cc2:InfoMessageBox ID="udcInfoBox" runat="server" Visible="False" Width="95%"></cc2:InfoMessageBox>
                <asp:MultiView ID="MultiViewReimSetCutoffDate" runat="server" ActiveViewIndex="1">
                    <asp:View ID="ViewUpdateError" runat="server">
                        <table>
                            <tr>
                                <td style="padding-top: 10px">
                                    <asp:ImageButton ID="ibtnErrorBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnErrorBack_Click" /></td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewInputDate" runat="server">
                        <table width="90%">
                            <tr style="height: 30px">
                                <td style="width: 25%; vertical-align: top">
                                    <asp:Label ID="lblCutoffDateText" runat="server" Text="<%$ Resources:Text, ReimbursementCutoffDate %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:TextBox ID="txtCutoffDate" runat="server" Width="70px" MaxLength="10" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                    <asp:Label ID="lblCutoffDate" runat="server" CssClass="tableText" Visible="False"></asp:Label>
                                    <asp:ImageButton ID="ibtnCutoffDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"></asp:ImageButton>
                                    <asp:Image ID="imgAlertCutoffDate" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                        AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                    <cc1:CalendarExtender ID="calCutoffDate" CssClass="ajax_cal" runat="server" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" TargetControlID="txtCutoffDate" PopupButtonID="ibtnCutoffDate" />
                                    <cc1:FilteredTextBoxExtender ID="filtereditCutoffDate" runat="server" FilterType="Custom, Numbers" TargetControlID="txtCutoffDate" ValidChars="-" />
                                </td>
                            </tr>
                            <tr style="height: 30px">
                                <td style="width: 25%; vertical-align: top">
                                    <asp:Label ID="lblReimIDText" runat="server" Text="<%$ Resources:Text, ReimbursementID %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblReimID" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:HiddenField ID="hfReimID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: center; padding-top: 10px">
                                    <asp:ImageButton ID="ibtnSet" runat="server" ImageUrl="<%$ Resources:ImageUrl, SetBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, SetBtn %>" OnClick="ibtnSet_Click" />
                                    <asp:ImageButton ID="ibtnReset" runat="server" Enabled="False" ImageUrl="<%$ Resources:ImageUrl, ResetDateDisableBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, ResetDateBtn %>" OnClick="ibtnReset_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewConfirmDate" runat="server">
                        <table width="90%">
                            <tr style="height: 30px">
                                <td style="width: 25%; vertical-align: top">
                                    <asp:Label ID="lblCutoffDateConfirmText" runat="server" Text="<%$ Resources:Text, ReimbursementCutoffDate %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblCutoffDateConfirm" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 30px">
                                <td style="width: 25%; vertical-align: top">
                                    <asp:Label ID="lblReimIDConfirmText" runat="server" Text="<%$ Resources:Text, ReimbursementID %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblReimIDConfirm" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 10%; text-align: left; padding-top: 10px">
                                                <asp:ImageButton ID="ibtnBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnBack_Click" />
                                            </td>
                                            <td style="text-align: center; padding-top: 10px">
                                                <asp:ImageButton ID="ibtnConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnConfirm_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewAllocated" runat="server">
                        <table width="90%">
                            <tr style="height: 30px">
                                <td style="width: 25%; vertical-align: top">
                                    <asp:Label ID="lblCutoffDateAllocatedText" runat="server" Text="<%$ Resources:Text, ReimbursementCutoffDate %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblCutoffDateAllocated" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 30px">
                                <td style="width: 25%; vertical-align: top">
                                    <asp:Label ID="lblReimIDAllocatedText" runat="server" Text="<%$ Resources:Text, ReimbursementID %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblReimIDAllocated" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 10%; text-align: left; padding-top: 10px">
                                    <asp:ImageButton ID="ibtnReturn" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, ReturnBtn %>" OnClick="ibtnReturn_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewReset" runat="server">
                        <table width="90%">
                            <tr>
                                <td style="width: 10%; padding-top: 10px">
                                    <asp:ImageButton ID="ibtnResetReturn" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, ReturnBtn %>" OnClick="ibtnResetReturn_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
            <%-- Popup for Confirm Reset (reset cutoff date) --%>
            <asp:Panel ID="panReset" runat="server" Style="display: none;">
                <asp:Panel ID="panResetHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblConfirmReset" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgConfirmReset" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblConfirmResetConfirm" runat="server" Text="<%$ Resources:Text, ReimbursementSetCutoffDateResetConfirm %>"
                                            Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ibtnResetConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnResetConfirm_Click" />
                                        <asp:ImageButton ID="ibtnResetCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnResetCancel_Click" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
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
            </asp:Panel>
            <asp:Button ID="btnHiddenReset" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupReset" runat="server" TargetControlID="btnHiddenReset"
                PopupControlID="panReset" PopupDragHandleControlID="panResetHeading" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll">
            </cc1:ModalPopupExtender>
            <%-- End of Popup for Confirm Reset --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
