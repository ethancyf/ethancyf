<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="EHSAccountCreationV1.aspx.vb" Inherits="HCSP.EHSAccountCreationV1"
    Title="<%$ Resources:Title, Claim %>" %>

<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc3" %>
<%@ Register Src="../UIControl/ChooseCCCode.ascx" TagName="ChooseCCCode" TagPrefix="uc4" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="../UIControl/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc3" %>
<%@ Register Src="../UIControl/ucInputDocumentType.ascx" TagName="ucInputDocumentType"
    TagPrefix="uc2" %>
<%@ Register Src="../PracticeSelection.ascx" TagName="PracticeSelection" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Popup Panel Section ----------------------------------------------------------------------->
            <!--Practice Selection ----------------------------------------------------------------------->
            <asp:Panel Style="display: none" ID="panModalPopupPracticeSelection" runat="server"
                BorderStyle="Solid" BackColor="#E0E0E0" BorderWidth="1px">
                <div style="padding: 10px 10px 10px 10px; overflow: auto;">
                    <cc3:PracticeRadioButtonGroup runat="server" ID="udcPracticeRadioButtonGroup" HeaderText="<%$ Resources:Text, SelectPractice %>"
                        HeaderTextCss="tableText" PracticeRadioButtonCss="tableText" PracticeTextCss="tableTextChi"
                        HeaderCss="" SchemeLabelCss="tableTitle" SelectButtonURL="~/Images/button/icon_button/btn_Arrow_to_Right.png"
                        MaskBankAccountNo="True" PanelHeight="400" ShowCloseButton="True"/>
                </div>
            </asp:Panel>
            <!--Practice Selection End ----------------------------------------------------------------------->
            <!--Confirm Message Pop Up Box Section ----------------------------------------------------------------------->
            <asp:Panel ID="panPopupConfirmCancel" runat="server" Style="display: none">
                <asp:Panel ID="panConfirmCancelHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblPopupConfirmCancelTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="right" style="width: auto; height: 42px" valign="middle">
                                        <asp:Image ID="imgPopupConfirmCancelMessage" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="left" style="width: 95%; height: 42px">
                                        <asp:Label ID="lblPopupConfirmCancelMessage" runat="server" Font-Bold="True" Text="<%$ Resources:Text, CancelAlert %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="btnPopupConfirmCancelCancel" runat="server" AlternateText="<%$ Resources:AlternateText, NoBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, NoBtn %>" />
                                        <asp:ImageButton ID="btnPopupConfirmCancelConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, YesBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, YesBtn %>" /></td>
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
            <!--Confirm Message Pop Up Box Section End----------------------------------------------------------------------->
            <!--confirm Modify----------------------------------------------------------------------->
            <asp:Panel Style="display: none" ID="panPopupConfirmModify" runat="server">
                <asp:Panel ID="panPopupConfirmModifyHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblPopupConfirmModifyTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 9px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="right" style="width: auto; height: 42px" valign="middle">
                                        <asp:Image ID="imgPopupConfirmModifyMessage" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="left" style="width: 95%; height: 42px">
                                        <asp:Label ID="lblPopupConfirmModifyMessage" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ModifyTempAccDisclaim %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        &nbsp;<asp:ImageButton ID="btnPopupConfirmModifyNo" runat="server" AlternateText="<%$ Resources:AlternateText, NoBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, NoBtn %>" />
                                        <asp:ImageButton ID="btnPopupConfirmModifyYes" runat="server" AlternateText="<%$ Resources:AlternateText, YesBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, YesBtn %>" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 9px; height: 7px">
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
            <!--confirm Modify End----------------------------------------------------------------------->
            <!--confirm Only Message Box -------------------------------------------------------------------------->
            <asp:Panel ID="panPopupConfirmOnly" runat="server" Style="display: none">
                <asp:Panel ID="panPopupConfirmOnlyHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblPopupConfirmOnlyTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="right" style="width: auto; height: 42px" valign="middle">
                                        <asp:Image ID="imgPopupConfirmOnlyMessage" runat="server" ImageUrl="~/Images/others/exclamation.png" /></td>
                                    <td align="left" style="width: 95%; height: 42px">
                                        <asp:Label ID="lblPopupConfirmOnlyMessage" runat="server" Font-Bold="True"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="btnPopupConfirmOnlyConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" /></td>
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
            <!--confirm Only Message Box End----------------------------------------------------------------------->
            <!--Chinese CCCode -------------------------------------------------------------------------->
            <asp:Panel Style="display: none" ID="panChooseCCCode" runat="server">
                <asp:Panel ID="panChooseCCCodeHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 350px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Text, ChooseCCCodeHeading %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table style="width: 350px" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                            </td>
                            <td style="background-color: #ffffff" align="center">
                                <uc4:ChooseCCCode ID="udcChooseCCCode" runat="server"></uc4:ChooseCCCode>
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
                    </tbody>
                </table>
            </asp:Panel>

             <asp:Panel Style="display: none; z-index:10003;" ID="panConfirmSelectPractice" runat="server" Width="600px">
                <uc5:ucNoticePopUp ID="ucNoticePopUpConfirmSelectPractice" runat="server" NoticeMode="ExclamationConfirmation" ButtonMode="ConfirmCancel" 
                    MessageText="<%$ Resources:Text, SelectPracticePopup %>"  />
            </asp:Panel>






            <!--Chinese CCCode End----------------------------------------------------------------------->
            <!-- Popup Panel Section End ----------------------------------------------------------------------->
            <!--ModalPopupExtender ----------------------------------------------------------------------->
            <cc1:ModalPopupExtender ID="ModalPopupPracticeSelection" runat="server" BackgroundCssClass="modalBackground"
                TargetControlID="btnModalPopupPracticeSelection" PopupControlID="panModalPopupPracticeSelection"
                RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupPracticeSelection" runat="server">
            </asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupConfirmCancel" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupConfirmCancel" PopupControlID="panPopupConfirmCancel"
                PopupDragHandleControlID="panConfirmCancelHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupConfirmCancel" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupConfirmModify" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupConfirmModify" PopupControlID="panPopupConfirmModify"
                PopupDragHandleControlID="panPopupConfirmModifyHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupConfirmModify" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderChooseCCCode" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnChooseCCCode" PopupControlID="panChooseCCCode" RepositionMode="None"
                PopupDragHandleControlID="panChooseCCCodeHeading">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnChooseCCCode" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupConfirmOnly" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupConfirmOnly" PopupControlID="panPopupConfirmOnly"
                PopupDragHandleControlID="panPopupConfirmOnlyHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>

            <%-- Popup for Select Practice Confirmation in covid-19 program --%>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmSelectPractice" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupConfirmSelectPractice" PopupControlID="panConfirmSelectPractice"
                PopupDragHandleControlID="panConfirmSelectPracticeHeading" RepositionMode="None" BehaviorID="panConfirmSelectPractice" DropShadow="False">
            </cc1:ModalPopupExtender>
          <asp:Button Style="display: none" ID="btnModalPopupConfirmSelectPractice" runat="server"></asp:Button>
            <%-- End of Popup for Select Practice Confirmation in covid-19 program  --%>

            <asp:Button ID="btnModalPopupConfirmOnly" runat="server" Style="display: none" /><!-- ModalPopupExtender Create End -----------------------------------------------------------------------><table
                cellpadding="0" cellspacing="0" style="width: 100%">
                <tbody>
                    <tr>
                        <td style="height: 50px" valign="middle">
                            <asp:Image ID="imgHeaderClaimVoucher" runat="server" AlternateText="<%$ Resources:AlternateText, ClaimVoucherBanner %>"
                                ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ClaimVoucherBanner %>" /></td>
                        <td align="right" style="width: 120px; height: 50px" valign="middle">
                        </td>
                    </tr>
                </tbody>
            </table>
            <!-- Heading Section ----------------------------------------------------------------------->
            <!-- Heading Section End ----------------------------------------------------------------------->
            <!-- TimeLine Section ----------------------------------------------------------------------->
            <asp:Panel ID="panClaimTimeline" runat="server">
                <table cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td>
                                <asp:Panel ID="panClaimTimelineStep1" runat="server" CssClass="highlightTimeline">
                                    <asp:Label ID="lblClaimVoucherStep1" runat="server" Text="<%$ Resources:Text, ClaimStep1 %>"></asp:Label></asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="panClaimTimelineStep1a" runat="server" CssClass="unhighlightTimeline">
                                    &nbsp;
                                    <asp:Label ID="lblClaimVoucherStep1a" runat="server" Text="<%$ Resources:Text, ClaimStep2 %>"></asp:Label></asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="panClaimTimelineStep1b" runat="server" CssClass="unhighlightTimeline">
                                    &nbsp;
                                    <asp:Label ID="lblClaimVoucherStep1b" runat="server" Text="<%$ Resources:Text, ClaimStep3 %>"></asp:Label></asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <asp:Panel ID="panAccountCreationTimeline" runat="server">
                <table cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td width="50">
                            </td>
                            <td>
                                <asp:Panel ID="panAccountCreationTimelineStep1a" runat="server" CssClass="highlightTimelineLast"
                                    Width="100%">
                                    <asp:Label ID="lblVRACreationStep1a" runat="server" Text="<%$ Resources:Text, EHAStep1 %>"></asp:Label></asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="panAccountCreationTimelineStep1b" runat="server" CssClass="unhighlightTimelineTest"
                                    Width="100%">
                                    &nbsp;
                                    <asp:Label ID="lblVRACreationStep1b" runat="server" Text="<%$ Resources:Text, EHAStep2 %>"></asp:Label></asp:Panel>
                            </td>
                            <td>
                                <asp:Panel ID="panAccountCreationTimelineStep1c" runat="server" CssClass="unhighlightTimelineTest"
                                    Width="100%">
                                    &nbsp;
                                    <asp:Label ID="lblVRACreationStep1c" runat="server" Text="<%$ Resources:Text, EHAStep3 %>"></asp:Label></asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <!-- TimeLine Section End ----------------------------------------------------------------------->
            <!-- Message Box----------------------------------------------------------------------->
            <cc2:InfoMessageBox ID="udcInfoMsgBox" runat="server" Width="99%" />
            <cc2:MessageBox ID="udcMsgBoxErr" runat="server" Width="99%" />
            <!-- Message Box End ----------------------------------------------------------------------->
            <!-- Content Section ----------------------------------------------------------------------->
            <!-- Hidden Field ----------------------------------------------------------------------->
            <!-- Hidden Field End----------------------------------------------------------------------->
            <asp:MultiView ID="mvAccountCreation" runat="server">
                <!-- step1a1 View (Get Consent)----------------------------------------------------------------------->
                <asp:View ID="vStep1a1" runat="server">
                    <table style="width: 770px" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td valign="middle" class="eHSTableCaption">
                                    <asp:Label ID="lblstep1a1HeaderText" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="eHSTableHeading" valign="top">
                                    <asp:Label ID="lblstep1a1AcctInfoText" runat="server" Text="<%$ Resources:Text, SearchInfo %>"></asp:Label></td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <table cellspacing="0" cellpadding="0">
                                        <tbody>
                                            <tr>
                                                <td style="vertical-align: top; width: 200px">
                                                    <asp:Label ID="lblstep1a1DocumentTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>"
                                                        CssClass="tableTitle" Height="25px"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblstep1a1DocumentType" runat="server" CssClass="tableText"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblstep1a1HKIDText" runat="server" CssClass="tableTitle" Height="25px"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblstep1a1HKID" runat="server" CssClass="tableText"></asp:Label></td>
                                            </tr>
                                            <tr id="Trstep1a1ECSerialNo" runat="server">
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblstep1a1ECSerialNoText" runat="server" Text="<%$ Resources:Text, ECSerialNo %>"
                                                        CssClass="tableTitle" Height="25px"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblstep1a1ECSerialNo" runat="server" CssClass="tableText"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblstep1a1DOBText" runat="server" Text="<%$ Resources:Text, DOB %>"
                                                        CssClass="tableTitle" Height="25px"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblstep1a1DOB" runat="server" CssClass="tableText"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblstep1a1ENameText" runat="server" Text="<%$ Resources: Text, EnglishName %>"
                                                        CssClass="tableTitle" Height="25px"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblstep1a1EName" runat="server" CssClass="tableText"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblstep1a1GenderText" runat="server" Text="<%$ Resources: Text, Gender %>"
                                                        CssClass="tableTitle" Height="25px"></asp:Label></td>
                                                <td style="vertical-align: top">
                                                    <asp:Label ID="lblstep1a1Gender" runat="server" CssClass="tableText"></asp:Label></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 750px; padding-top: 20px">
                                <asp:Label ID="lblstep1a1DisclaimerNotice" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                    </table>
                    <table style="width: 100%" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td align="center" style="width: 200px; height: 33px" valign="bottom">
                                    &nbsp;
                                </td>
                                <td valign="bottom" align="left" style="height: 33px">
                                    <asp:ImageButton ID="btnstep1a1Back" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn%>"></asp:ImageButton>&nbsp;
                                    <asp:ImageButton ID="btnstep1a1CreateAccount" runat="server" AlternateText="<%$ Resources:AlternateText, CreateAccBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CreateAccBtn%>"></asp:ImageButton></td>
                            </tr>
                        </tbody>
                    </table>
                </asp:View>
                <!-- step1a1 View End ------------------------------------------------------------------------------------------------------------>
                <!-- Step1a2 View (Get Consent)----------------------------------------------------------------------->
                <asp:View ID="vStep1a2" runat="server">
                    <table cellpadding="0" cellspacing="0" style="width: 770px">
                        <tr>
                            <td style="height: 25px" valign="middle" class="eHSTableCaption">
                                <asp:Label ID="lblStep1a2HeaderText" runat="server" Text="<%$ Resources:Text, ConfirmAcc %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="eHSTableHeading" valign="top">
                                <asp:Label ID="lblStep1a2AcctInfoText" runat="server" Text="<%$ Resources:Text, AccountInfo%>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <uc3:ucReadOnlyDocumnetType ID="udcStep1a2ReadOnlyDocumnetType" runat="server"></uc3:ucReadOnlyDocumnetType>
                            </td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0" style="width: 750px">
                        <tr>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" style="padding-bottom: 10px; width: 750px">
                                <asp:Label ID="lblStep1a2TempAccDisclaimerText" runat="server" CssClass="tableText"
                                    Text="<%$ Resources:Text, ConfirmTempAccDisclaim %>"></asp:Label></td>
                        </tr>
                    </table>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 200px" valign="top">
                            </td>
                            <td valign="top">
                                <asp:ImageButton ID="btnStep1a2Cancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn%>"
                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn%>" />
                                <asp:ImageButton ID="btnStep1a2Confirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn%>"
                                    ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn%>" />
                                <asp:ImageButton ID="btnStep1a2Modify" runat="server" AlternateText="<%$ Resources:AlternateText, ModifyBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, ModifyBtn%>" /></td>
                        </tr>
                    </table>
                </asp:View>
                <!-- Step1a2 View End ------------------------------------------------------------------------------------------------------------>
                <!-- Step1b1 View (Enter Detail)----------------------------------------------------------------------->
                <asp:View ID="vStep1b1" runat="server">
                    <table style="width: 772px" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td style="height: 25px" valign="middle" class="eHSTableCaption">
                                    <asp:Label ID="lblStep1b1HeaderText" runat="server" Text="<%$ Resources:Text, VRACreationEnterDtl %>"></asp:Label></td>
                            </tr>
                        </tbody>
                    </table>
                    <table cellspacing="0" cellpadding="0" width="850">
                        <tbody>
                            <tr>
                                <td valign="top" style="width: 150px">
                                    <asp:Label ID="lblStep1b1CurrentPracticeText" runat="server" Text="<%$ Resources:Text, CurrentPractice %>"
                                        CssClass="tableTitle" Width="150px"></asp:Label></td>
                                <td style="width: 650px;" valign="top">
                                    <asp:Label ID="lblStep1b1CurrentPractice" runat="server" CssClass="tableTextChi"></asp:Label>
                                    <asp:ImageButton ID="btnStep1b1ChangePractice" runat="server" AlternateText="<%$ Resources:AlternateText, ChangePracticeBtn %>"
                                        ImageUrl="~/Images/button/icon_button/btn_change_scheme.png" ImageAlign="top"></asp:ImageButton></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding-bottom: 5px" valign="top" width="850">
                                    <asp:Label ID="lblStep1b1InputInfoText" runat="server" CssClass="tableText"></asp:Label>&nbsp;<asp:ImageButton
                                        ID="btnStep1b1InputTips" runat="server" ImageAlign="AbsMiddle" /></td>
                            </tr>
                        </tbody>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td valign="top">
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <uc2:ucInputDocumentType ID="udcStep1b1InputDocumentType" runat="server"></uc2:ucInputDocumentType>
                                        </td>
                                    </tr>
                                </table>
                                <table cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td style="width: 200px; padding-top: 15px;">
                                            </td>
                                            <td style="width: 400px; padding-top: 10px;">
                                                <asp:ImageButton ID="btnStep1b1Cancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn%>"
                                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn%>"></asp:ImageButton>
                                                <asp:ImageButton ID="btnStep1b1Next" runat="server" AlternateText="<%$ Resources:AlternateText, NextBtn%>"
                                                    ImageUrl="<%$ Resources:ImageUrl, NextBtn%>"></asp:ImageButton></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td valign="top" style="padding-right: 10px">
                                <asp:Image ID="imgDocTips" runat="server" /></td>
                        </tr>
                    </table>
                </asp:View>
                <!-- Step1b1 View End ------------------------------------------------------------------------------------>
                <!-- Step1bs View (Confirm Detail)----------------------------------------------------------------------->
                <asp:View ID="vStep1b2" runat="server">
                    <table cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td style="height: 25px" class="eHSTableCaption">
                                    <asp:Label ID="lblStep1b2HeaderText" runat="server" Text="<%$ Resources:Text, VRAConfirmTempAcc %>"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="eHSTableHeading" valign="top">
                                    <asp:Label ID="lblStep1b2CreationAcctInfotext" runat="server" Text="<%$ Resources:Text, AccountInfo %>"></asp:Label></td>
                            </tr>
                        </tbody>
                    </table>
                    <uc3:ucReadOnlyDocumnetType ID="udcStep1b2ReadOnlyDocumnetType" runat="server"></uc3:ucReadOnlyDocumnetType>
                    <table cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td style="width: 200px;">
                                </td>
                                <td style="width: 500px;" class="checkboxStyle">
                                    <asp:CheckBox ID="chkStep1b2Declare" TabIndex="1" runat="server" Text="<%$ Resources:Text, ProvidedInfoTrueVAForm%>"
                                        AutoPostBack="True"></asp:CheckBox></td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                </td>
                                <td style="width: 500px; padding-top: 10px;">
                                    <asp:ImageButton ID="btnStep1b2Cancel" runat="server" />
                                    <asp:ImageButton ID="btnStep1b2Back" runat="server" />
                                    <asp:ImageButton ID="btnStep1b2Confirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, ConfirmDisableBtn%>" EnableTheming="False"
                                        Enabled="False"></asp:ImageButton></td>
                            </tr>
                        </tbody>
                    </table>
                </asp:View>
                <!-- Step1b2 View End ------------------------------------------------------------------------------------>
                <!-- Step1c View (Complete Account Creation)----------------------------------------------------------------------->
                <asp:View ID="vStep1c" runat="server">
                    <table cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td class="eHSTableHeading" valign="top">
                                    <asp:Label ID="lblStep1cAcctInfo" runat="server" Text="<%$ Resources:Text, AccountInfo%>"></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <uc3:ucReadOnlyDocumnetType ID="udcStep1cReadOnlyDocumnetType" runat="server"></uc3:ucReadOnlyDocumnetType>
                    <table cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td style="width: 230px">
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnStep1cProceedToClaim" runat="server" AlternateText="<%$ Resources:AlternateText, ProceedClaimBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, ProceedClaimBtn%>"></asp:ImageButton>&nbsp;<asp:ImageButton
                                            ID="btnStep1cNextCreation" runat="server" AlternateText="<%$ Resources:AlternateText, NextCreationBtn%>"
                                            ImageUrl="<%$ Resources:ImageUrl, NextCreationBtn%>"></asp:ImageButton>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:View>
                <!-- Step1c View End ------------------------------------------------------------------------------------>
                <asp:View ID="vStep1b3" runat="server">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="height: 25px" valign="middle" class="eHSTableCaption">
                                <asp:Label ID="lblStep1b3HeaderText" runat="server" Text="<%$ Resources:Text, ConfirmAcc %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="eHSTableHeading" valign="top">
                                <asp:Label ID="lblStep1b3CreationAcctInfotext" runat="server" Text="<%$ Resources:Text, AccountInfo %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <uc2:ucInputDocumentType ID="udcStep1b3InputDocumentType" runat="server"></uc2:ucInputDocumentType>
                            </td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td style="width: 200px;">
                                </td>
                                <td style="width: 500px;">
                                    <asp:Panel ID="panStep1b3Declare" runat="server">
                                        <asp:CheckBox ID="chkStep1b3Declare" runat="server" AutoPostBack="True" TabIndex="1"
                                            Width="580px" CssClass="checkboxStyle" /></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                </td>
                                <td style="width: 500px; padding-top: 10px;">
                                    <asp:ImageButton ID="btnStep1b3Cancel" runat="server" />
                                    <asp:ImageButton ID="btnStep1b3Confirm" runat="server" /></td>
                            </tr>
                        </tbody>
                    </table>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
