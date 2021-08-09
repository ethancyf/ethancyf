<%@ Control Language="vb" AutoEventWireup="false" Codebehind="eHealthAccountDeathRecordMaint.ascx.vb"
    Inherits="HCVU.eHealthAccountDeathRecordMaint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/DocType/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumentType"
    TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/Token/ucInputToken.ascx" TagName="ucInputToken" TagPrefix="uc2" %>
<cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" />
<cc2:MessageBox ID="udcMessageBox" runat="server" />
<asp:Panel ID="panCore" runat="server">
    <asp:MultiView ID="mvCore" runat="server">
        <asp:View ID="vSearchCriteria" runat="server">
            <%--[S]--%>
            <table>
                <tr style="height: 1px">
                    <td style="width: 160px">
                    </td>
                    <td style="width: 240px">
                    </td>
                    <td style="width: 160px">
                    </td>
                    <td style="width: 240px">
                    </td>
                </tr>
                <tr style="height: 25px">
                    <td>
                        <asp:Label ID="lblSDocumentNoText" runat="server" Text="<%$ Resources: Text, HKID %>"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSDocumentNo" runat="server" Width="100" onChange="convertToUpper(this)"
                            MaxLength="20"></asp:TextBox>
                        <asp:Image ID="imgSDocumentNo" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                            AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Visible="False" />
                    </td>
                </tr>
                <tr style="height: 25px">
                </tr>
                <tr>
                    <td style="text-align: center" colspan="4">
                        <asp:ImageButton ID="ibtnSSearch" runat="server" ImageUrl="<%$ Resources: ImageUrl, SearchBtn %>"
                            AlternateText="<%$ Resources: AlternateText, SearchBtn %>" OnClick="ibtnSSearch_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vDetail" runat="server">
            <%--[D]--%>
            <table>
                <tr style="height: 5px">
                </tr>
                <tr>
                    <td colspan="4">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <div class="headingText">
                                        <asp:Label ID="lblDDeathRecordInformation" runat="server" Text="<%$ Resources: Text, DeathRecordInformation %>"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkDMaskDocumentNo" runat="server" Text="<%$ Resources: Text, MaskIdentityDocumentNo %>"
                                        AutoPostBack="True" OnCheckedChanged="chkDMaskDocumentNo_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 205px; padding-left: 4px">
                        <asp:Label ID="lblDDocumentNoText" runat="server" Text="<%$ Resources: Text, HKID %>"></asp:Label>
                    </td>
                    <td style="width: 205px;">
                        <asp:Label ID="lblDDocumentNo" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                    <td style="width: 205px;">
                        <asp:Label ID="lblDNameText" runat="server" Text="<%$ Resources: Text, Name %>"></asp:Label>
                    </td>
                    <td style="width: 205px;">
                        <asp:Label ID="lblDName" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="padding-left: 4px">
                        <asp:Label ID="lblDDateOfDeathText" runat="server" Text="<%$ Resources: Text, DateOfDeath %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDDateOfDeath" runat="server" CssClass="tableText"></asp:Label>
                        <asp:HiddenField ID="hfDDateOfDeathType" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblDDateOfRegistrationText" runat="server" Text="<%$ Resources: Text, DateOfRegistration %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDDateOfRegistration" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
                <tr style="height: 10px">
                </tr>
                <tr style="height: 10px">
                </tr>
            </table>
            <br />
            <br />
            <table>
                <tr>
                    <td style="width: 400px">
                        <asp:ImageButton ID="ibtnDBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                            AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                    </td>
                    <td>
                        <asp:ImageButton ID="ibtnDRemove" runat="server" ImageUrl="<%$ Resources: ImageUrl, RemoveBtn %>"
                            AlternateText="<%$ Resources: AlternateText, RemoveBtn %>" OnClick="ibtnDRemove_Click" />
                        <cc2:CustomImageButton ID="ibtnDManagement" runat="server" ImageUrl="<%$ Resources: ImageUrl, ManagementBtn %>"
                            ImageUrlDisable="<%$ Resources: ImageUrl, ManagementDisableBtn %>" AlternateText="<%$ Resources: AlternateText, ManagementBtn %>"
                            ShowRedirectImage="False"  />
                    </td>
                </tr>
            </table>
        </asp:View>
        &nbsp;&nbsp;
        <asp:View ID="vRemoveComplete" runat="server">
            <%--[AF]--%>
            <table>
                <tr>
                    <td style="padding-top: 10px">
                        <asp:ImageButton ID="ibtnRCReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                            AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnRCReturn_Click" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
    <%-- Pop up for Remove Entry --%>
    <asp:Button ID="btnHiddenSRemoveFile" runat="server" Style="display: none" />
    <cc1:ModalPopupExtender ID="popupSRemoveFile" runat="server" TargetControlID="btnHiddenSRemoveFile"
        PopupControlID="panSRemoveFile" BackgroundCssClass="modalBackgroundTransparent"
        DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panSRemoveFileHeading">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="panSRemoveFile" runat="server" Style="display: none">
        <asp:Panel ID="panSRemoveFileHeading" runat="server" Style="cursor: move">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                <tr>
                    <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                    </td>
                    <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                        color: #ffffff; background-repeat: repeat-x; height: 35px">
                        <asp:Label ID="lblSRemoveFileHeader" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>">
                        </asp:Label></td>
                    <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
            <tr>
                <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y">
                </td>
                <td style="background-color: #FFFFFF">
                    <table style="width: 100%">
                        <tr>
                            <td align="left" style="width: 40px; height: 42px" valign="middle">
                                <asp:Image ID="imgPopupSRemoveFile" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                            <td align="center" style="height: 42px">
                                <asp:Label ID="lblPopupSRemoveFileText" runat="server" Font-Bold="True" Text="<%$ Resources: Text, ConfirmToRemoveDeathRecordQ %>">
                                </asp:Label></td>
                            <td style="width: 40px">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3">
                                <asp:ImageButton ID="ibtnPopupSRemoveFileConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnPopupSRemoveFileConfirm_Click" />
                                <asp:ImageButton ID="ibtnPopupSRemoveFileCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnPopupSRemoveFileCancel_Click" /></td>
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
    <%-- End of Pop up for Remove Entry --%>
    <%-- Pop up for Unmask --%>
    <asp:Button ID="btnHiddenUnmask" runat="server" Style="display: none" />
    <cc1:ModalPopupExtender ID="popupUnmask" runat="server" TargetControlID="btnHiddenUnmask"
        PopupControlID="panUnmask" BackgroundCssClass="modalBackgroundTransparent" DropShadow="False"
        RepositionMode="None">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="panUnmask" runat="server" Style="display: none">
        <uc2:ucInputToken ID="udcPUInputToken" runat="server"></uc2:ucInputToken>
    </asp:Panel>
    <%-- End of Pop up for Unmask --%>
</asp:Panel>
