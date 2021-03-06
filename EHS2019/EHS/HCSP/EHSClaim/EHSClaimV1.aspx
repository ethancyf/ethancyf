<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="EHSClaimV1.aspx.vb" Inherits="HCSP.EHSClaimV1" Title="<%$ Resources:Title, Claim %>"
    EnableEventValidation="False" %>
<%@ Register Src="../UIControl/OutreachListSearch.ascx" TagName="OutreachListSearch"
    TagPrefix="uc11" %>
<%@ Register Src="../UIControl/ucVaccinationRecordProvider.ascx" TagName="ucVaccinationRecordProvider"
    TagPrefix="uc10" %>
<%@ Register Src="../UIControl/VaccinationRecord/ucVaccinationRecord.ascx" TagName="VaccinationRecord"
    TagPrefix="uc9" %>
<%@ Register Src="../UIControl/SchemeDocTypeLegend.ascx" TagName="SchemeDocTypeLegend"
    TagPrefix="uc8" %>
<%@ Register Src="../UIControl/SchemeLegend.ascx" TagName="SchemeLegend" TagPrefix="uc7" %>
<%@ Register Src="../UIControl/RVPHomeListSearch.ascx" TagName="RVPHomeListSearch"
    TagPrefix="uc6" %>
<%@ Register Src="../PrintFormOptionSelection.ascx" TagName="PrintFormOptionSelection"
    TagPrefix="uc5" %>
<%@ Register Src="../UIControl/ucReadOnlyEHSClaim.ascx" TagName="ucReadOnlyEHSClaim"
    TagPrefix="uc4" %>
<%@ Register Src="../UIControl/ucInputEHSClaim.ascx" TagName="ucInputEHSClaim" TagPrefix="uc3" %>
<%@ Register Src="../UIControl/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc2" %>
<%@ Register Src="../UIControl/DocType/ucClaimSearch.ascx" TagName="ucClaimSearch"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc3" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc1" %>

<%@ Register Src="~/UIControl/IDEASCombo/IDEASCombo.ascx" TagName="IDEASCombo" TagPrefix="uc11" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc11:IDEASCombo ID="ucIDEASCombo" runat="server" />
            <asp:Panel Style="display: none" ID="panPopupConfirmCancel" runat="server" Width="500px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpConfirm" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo"
                    MessageText="<%$ Resources:Text, CancelAlert %>" />
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panPopupDuplicateClaimAlert" runat="server" Width="500px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpDuplicateClaimAlert" runat="server" NoticeMode="Custom" IconMode="ExclamationIcon"
                    ButtonMode="ProceedNotProceed" HeaderText="<%$ Resources:Text, DuplicateClaimAlertTitle %>" MessageText="<%$ Resources:Text, DuplicateClaimAlertMessage %>" />
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panPopupExclamationConfirmationBox" runat="server" Width="700px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpExclamationConfirm" runat="server" NoticeMode="ExclamationConfirmation" ButtonMode="ConfirmCancel"
                    MessageText="" />
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panPopupExclamationErrorBox" runat="server" Width="500px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpExclamationError" runat="server" NoticeMode="Custom" IconMode="ExclamationIcon" ButtonMode="Close"
                    MessageText="" />
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panPopupExclamationImportantReminder" runat="server" Width="600px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpExclamationImportantReminder" runat="server" NoticeMode="Custom" IconMode="ExclamationIcon" ButtonMode="Close"
                    MessageText="" />
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panPopupExclamationDemographicReminder" runat="server" Width="600px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpExclamationDemographicReminder" runat="server" NoticeMode="Custom" IconMode="ExclamationIcon" ButtonMode="Close"
                    MessageText="" />
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panPopupExclamationImportantReminderWithReason" runat="server" Width="700px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpExclamationImportantReminderWithReason" runat="server" NoticeMode="Custom" IconMode="ExclamationIcon" ButtonMode="ConfirmCancelWithReason"
                    MessageText="" />
            </asp:Panel>
            <asp:Panel Style="display: none;" ID="panPopupPracticeSelection" runat="server" BorderStyle="Solid"
                BackColor="#E0E0E0" BorderWidth="1px">
                <div style="padding: 10px 10px 10px 10px; overflow: auto;">
                    <cc3:PracticeRadioButtonGroup runat="server" ID="udcPopupPracticeRadioButtonGroup"
                        HeaderText="<%$ Resources:Text, SelectPractice%>" HeaderTextCss="tableText" PracticeRadioButtonCss="tableText"
                        PracticeTextCss="tableTextChi" HeaderCss="" SchemeLabelCss="tableTitle" SelectButtonURL="~/Images/button/icon_button/btn_Arrow_to_Right.png"
                        MaskBankAccountNo="True" PanelHeight="400" ShowCloseButton="True"    />
                </div>
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panPopupPrintOptionSelection" runat="server"
                Width="600px" BorderStyle="Solid" BackColor="#E0E0E0" BorderWidth="1px">
                <table style="width: 100%" cellspacing="0" cellpadding="0">
                    <tbody>
                        <tr>
                            <td>
                                <uc5:PrintFormOptionSelection ID="udtPrintOptionSelection" runat="server"></uc5:PrintFormOptionSelection>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:ImageButton ID="btnPopupPrintOptionSelectionCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn%>"
                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn%>"></asp:ImageButton>
                                <asp:ImageButton ID="btnPopupPrintOptionSelectionSelect" runat="server" AlternateText="<%$ Resources:AlternateText, SelectBtn%>"
                                    ImageUrl="<%$ Resources:ImageUrl, SelectBtn%>"></asp:ImageButton>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panPopupOutreachListSearch" runat="server">
                <asp:Panel Style="cursor: move" ID="panOutreachListHeading" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 980px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px;">
                                <asp:Label ID="lblOutreachSearchTitle" runat="server" Text="<%$ Resources:Text, Search %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table style="width: 980px" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                            <td style="background-color: #ffffff; padding: 5px 5px 5px 5px" align="left">
                                <asp:Panel ID="panOutreachRecord" runat="server">
                                    <uc11:OutreachListSearch ID="udcOutreachSearch" runat="server"></uc11:OutreachListSearch>
                                </asp:Panel>
                            </td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                            <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                                <asp:ImageButton ID="btnPopupOutreachListSearchCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"></asp:ImageButton>
                                <asp:ImageButton ID="btnPopupOutreachListSearchSelect" runat="server" AlternateText="<%$ Resources:AlternateText, SelectBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, SelectBtn %>" />
                            </td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                            <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                            <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panPopupRVPHomeListSearch" runat="server">
                <asp:Panel Style="cursor: move" ID="panRCHSearchHomeListHeading" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 980px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px;">
                                <asp:Label ID="lblRCHSearchFormTitle" runat="server" Text="<%$ Resources:Text, Search %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table style="width: 980px" cellspacing="0" cellpadding="0" border="0">
                    <tbody>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                            <td style="background-color: #ffffff; padding: 5px 5px 5px 5px" align="left">
                                <asp:Panel ID="panRCHRecord" runat="server">
                                    <uc6:RVPHomeListSearch ID="udcRVPHomeListSearch" runat="server"></uc6:RVPHomeListSearch>
                                </asp:Panel>
                            </td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                            <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                                <asp:ImageButton ID="btnPopupRVPHomeListSearchCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"></asp:ImageButton>
                                <asp:ImageButton ID="btnPopupRVPHomeListSearchSelect" runat="server" AlternateText="<%$ Resources:AlternateText, SelectBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, SelectBtn %>" />
                            </td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                            <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                            <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panSchemeLegend" runat="server">
                <asp:Panel ID="panSchemeLegendHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblSchemeLegnedHeading" runat="server" Text="<%$ Resources:Text, Legend %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panSchemeLegnedContent" runat="server" ScrollBars="vertical" Height="330px">
                                <uc7:SchemeLegend ID="udcSchemeLegend" runat="server"></uc7:SchemeLegend>
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="btnSchemeLegnedClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panAddHocPrintSelection" runat="server">
                <asp:Panel ID="panAddHocPrintSelectionHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 250px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblAddHocPrintSelectionHeading" runat="server" Text="<%$ Resources:Text, PrintConsentForm %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 250px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" colspan="2">
                                        <div style="text-align: left" id="dvAdhocPrintFull" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAdhocPrintFull" runat="server" CssClass="tableText" Text="<%$ Resources:Text, FullVersionPrintOut %>"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rbAdhocPrintFullLang1" GroupName="AdhocPrintSelection" runat="server" CssClass="tableText" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rbAdhocPrintFullLang2" GroupName="AdhocPrintSelection" runat="server" CssClass="tableText" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rbAdhocPrintFullLang3" GroupName="AdhocPrintSelection" runat="server" CssClass="tableText" /></td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 10px"></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="text-align: left" id="dvAdhocPrintCondense" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAdhocPrintCondense" runat="server" CssClass="tableText" Text="<%$ Resources:Text, CondensedVersionPrintOut %>"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rbAdhocPrintCondenseLang1" GroupName="AdhocPrintSelection" runat="server" CssClass="tableText" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rbAdhocPrintCondenseLang2" GroupName="AdhocPrintSelection" runat="server" CssClass="tableText" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rbAdhocPrintCondenseLang3" GroupName="AdhocPrintSelection" runat="server" CssClass="tableText" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="btnAddHocPrintSelectionCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn%>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn%>" />
                                        <asp:ImageButton ID="btnAddHocPrintSelection" runat="server" AlternateText="<%$ Resources:AlternateText, PrintBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, PrintBtn %>" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panSchemeDocTypeLegend" runat="server">
                <asp:Panel ID="panSchemeDocTypeLegendHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 920px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblSchemeDocTypeLegnedHeading" runat="server" Text="<%$ Resources:Text, AcceptedDocList %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 920px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panSchemeDocTypeLegnedContent" runat="server" ScrollBars="Both" Width="890px"  
                                Height="550px" >
                                <uc8:SchemeDocTypeLegend ID="udcSchemeDocTypeLegend" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="btnSchemeDocTypeLegnedClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panVaccinationRecord" runat="server">
                <asp:Panel ID="panVaccinationRecordHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 980px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblVaccinationRecordHeading" runat="server" Text="<%$ Resources:Text, VaccinationRecord %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 980px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panVaccinationRecordContent" runat="server" ScrollBars="Auto" Height="508px">
                                <uc9:VaccinationRecord ID="udcVaccinationRecord" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="checkboxStyle" style="width: 930px">
                                        <asp:Label ID="lblDisclaimer" runat="server" Text="<%$ Resources:Text, Disclaimer %>"
                                            Style="text-decoration: underline"></asp:Label>
                                        <asp:ImageButton ID="ibtnInfo" runat="server" ImageUrl="<%$ Resources: ImageUrl, Infobtn %>"
                                            AlternateText="<%$ Resources: Text, VaccinationRecordProvider %>" OnClick="ibtnInfo_Click" />
                                        <br />
                                        <asp:Label ID="lblNotCompleteVaccinationRecord" runat="server" Text="<%$ Resources:Text, NotCompleteVaccinationRecord %>"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="btnVaccinationRecordClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>"
                                AlternateText="<%$ Resources:AlternateText, CloseBtn %>" OnClick="btnVaccinationRecordClose_Click" />
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panSubsidizeDisabledRemark" runat="server">
                <asp:Panel ID="panSubsidizeDisabledRemarkHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 520px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblpanSubsidizeDisabledDetailsHeading" runat="server" /></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 520px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panSubsidizeDisabledRemarkContent" runat="server" ScrollBars="None">
                                <%--<div id="divSubsidizeDisabledDetailsTitle" runat="server" style="width:480px;margin: 14px 2px 0px 2px"></div>--%>
                                <div id="divSubsidizeDisabledDetailsContent" runat="server" style="width: 480px; margin: 6px 2px 2px 2px"></div>
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="btnSubsidizeDisabledRemarkClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panRecipientConditionHelp" runat="server">
                <asp:Panel ID="panRecipientConditionHelpHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 660px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblpanRecipientConditionHelpHeading" runat="server" /></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 660px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panRecipientConditionHelpContent" runat="server" ScrollBars="None">
                                <div id="divRecipientConditionHelpTitle" runat="server" style="width: 620px; margin: 14px 2px 0px 2px"></div>
                                <div id="divRecipientConditionHelpContent" runat="server" style="width: 620px; margin: 6px 2px 2px 2px"></div>
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="btnRecipientConditionHelpClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel Style="display: none" ID="panOtherVaccinationRecordRemark" runat="server">
                <asp:Panel ID="panOtherVaccinationRecordRemarkHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 660px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblpanOtherVaccinationRecordRemarkHeading" runat="server" Text="<%$ Resources:Text, Remarks %>" />
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 660px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panOtherVaccinationRecordRemarkContent" runat="server" ScrollBars="None">
                                <div id="divOtherVaccinationRecordRemarkTitle" runat="server" style="width: 620px; margin: 14px 2px 0px 2px">
                                </div>
                                <div id="divOtherVaccinationRecordRemark" runat="server" style="width: 620px; margin: 6px 2px 2px 2px">
                                    <asp:Label ID="lblpanOtherVaccinationRecordRemarkContent" runat="server" Text="<%$ Resources:Text, VaccinationOtherRecordForCOVID19Remark %>" />                                    
                                </div>
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="btnOtherVaccinationRecordRemarkClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel Style="display: none" ID="panDischargeRecordRemark" runat="server">
                <asp:Panel ID="panDischargeRecordRemarkHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 660px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblpanDischargeRecordRemarkHeading" runat="server" Text="<%$ Resources:Text, Remarks %>" />
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 660px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panDischargeRecordRemarkContent" runat="server" ScrollBars="None">
                                <div id="divDischargeRecordRemarkTitle" runat="server" style="width: 620px; margin: 14px 2px 0px 2px">
                                </div>
                                <div id="divDischargeRecordRemark" runat="server" style="width: 620px; margin: 6px 2px 2px 2px">
                                    <asp:Label ID="lblpanDischargeRecordRemarkContent" runat="server" Text="<%$ Resources:Text, DischargeRecordForCOVID19Remark %>" />                                    
                                </div>
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="btnDischargeRecordRemarkClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel Style="display: none; z-index:10003;" ID="panConfirmSelectPractice" runat="server" Width="600px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpConfirmSelectPractice" runat="server" NoticeMode="ExclamationConfirmation" ButtonMode="ConfirmCancel" 
                    MessageText="<%$ Resources:Text, SelectPracticePopup %>"  />
            </asp:Panel>



            <cc1:ModalPopupExtender ID="ModalPopupConfirmCancel" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnStep2aCancel" PopupControlID="panPopupConfirmCancel"
                PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupConfirmCancel" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModelPopupDuplicateClaimAlert" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModelPopupDuplicateClaimAlert" PopupControlID="panPopupDuplicateClaimAlert"
                PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModelPopupDuplicateClaimAlert" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupPracticeSelection" runat="server" BackgroundCssClass="modalBackground"
                TargetControlID="btnModalPopupPracticeSelection" PopupControlID="panPopupPracticeSelection"
                RepositionMode="None" BehaviorID="panPopupPracticeSelection">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupPracticeSelection" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupPrintOptionSelection" runat="server" BackgroundCssClass="modalBackground"
                TargetControlID="btnModalPopupPrintOptionSelection" PopupControlID="panPopupPrintOptionSelection"
                RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupPrintOptionSelection" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExclamationConfirmationBox" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupExclamationConfirmationBox" PopupControlID="panPopupExclamationConfirmationBox"
                PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupExclamationErrorBox" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExclamationErrorBox" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupExclamationErrorBox" PopupControlID="panPopupExclamationErrorBox"
                PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupExclamationImportantReminder" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExclamationImportantReminder" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupExclamationImportantReminder" PopupControlID="panPopupExclamationImportantReminder"
                PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupExclamationDemographicReminder" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExclamationDemographicReminder" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupExclamationDemographicReminder" PopupControlID="panPopupExclamationDemographicReminder"
                PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupExclamationImportantReminderWithReason" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExclamationImportantReminderWithReason" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupExclamationImportantReminderWithReason" PopupControlID="panPopupExclamationImportantReminderWithReason"
                PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupExclamationConfirmationBox" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderRVPHomeListSearch" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupRVPHomeListSearch" PopupControlID="panPopupRVPHomeListSearch"
                PopupDragHandleControlID="panRCHSearchHomeListHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupRVPHomeListSearch" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderOutreachListSearch" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupOutreachListSearch" PopupControlID="panPopupOutreachListSearch"
                PopupDragHandleControlID="panOutreachListHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupOutreachListSearch" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderSchemeLegned" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupSchemeLegned" PopupControlID="panSchemeLegend"
                PopupDragHandleControlID="panSchemeLegendHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupSchemeLegned" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderSchemeDocTypeLegend" runat="server"
                BackgroundCssClass="modalBackgroundTransparent" TargetControlID="btnModalPopupSchemeDocTypeLegend"
                PopupControlID="panSchemeDocTypeLegend" PopupDragHandleControlID="panSchemeDocTypeLegendHeading"
                RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupSchemeDocTypeLegend" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderAddHocPrintSelection" runat="server"
                BackgroundCssClass="modalBackgroundTransparent" TargetControlID="btnModalPopupAddHocPrintSelection"
                PopupControlID="panAddHocPrintSelection" PopupDragHandleControlID="panAddHocPrintSelectionHeading"
                RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button ID="btnModalPopupAddHocPrintSelection" runat="server" Style="display: none" />
            <%-- Popup for Vaccination Record --%>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderVaccinationRecord" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupVaccinationRecord" PopupControlID="panVaccinationRecord"
                PopupDragHandleControlID="panVaccinationRecordHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button ID="btnModalPopupVaccinationRecord" runat="server" Style="display: none" />
            <%-- End of Popup for Vaccination Record --%>
            <%-- Popup for Subsidize Disabled Remark --%>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderSubsidizeDisabledRemark" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupSubsidizeDisabledRemark" PopupControlID="panSubsidizeDisabledRemark"
                PopupDragHandleControlID="panSubsidizeDisabledRemarkHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button ID="btnModalPopupSubsidizeDisabledRemark" runat="server" Style="display: none" />
            <%-- End of Popup for Subsidize Disabled Remark --%>

            <%-- Popup for Recipient Condition Help --%>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderRecipientConditionHelp" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupRecipientConditionHelp" PopupControlID="panRecipientConditionHelp"
                PopupDragHandleControlID="panRecipientConditionHelpHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button ID="btnModalPopupRecipientConditionHelp" runat="server" Style="display: none" />
            <%-- End of Popup for Recipient Condition Help --%>

            <%-- Popup for Other Vaccination Record Remark --%>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderOtherVaccinationRecordRemark" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupOtherVaccinationRecordRemark" PopupControlID="panOtherVaccinationRecordRemark" BehaviorID="mpeOtherVaccinationRecordRemark"
                PopupDragHandleControlID="" OkControlID="btnOtherVaccinationRecordRemarkClose" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <%--<asp:Button ID="btnModalPopupOtherVaccinationRecordRemark" runat="server" Style="display: none" >
            </asp:Button>--%>
            <%-- End of Popup for Other Vaccination Record Remark --%>

             <%-- Popup for Discharge Record Remark --%>
            <cc1:ModalPopupExtender ID="ModalPopupDischargeRecordRemark" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupDischargeRecordRemark" PopupControlID="panDischargeRecordRemark" BehaviorID="mpeDischargeRecordRemark"
                PopupDragHandleControlID="" OkControlID="btnDischargeRecordRemarkClose" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <%--<asp:Button ID="btnModalPopupDischargeRecordRemark" runat="server" Style="display: none" >
            </asp:Button>--%>
            <%-- End of Popup for Other Vaccination Record Remark --%>

            <%-- Popup for Select Practice Confirmation in covid-19 program --%>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmSelectPractice" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupConfirmSelectPractice" PopupControlID="panConfirmSelectPractice"
                PopupDragHandleControlID="panConfirmSelectPracticeHeading" RepositionMode="None" BehaviorID="panConfirmSelectPractice" DropShadow="False">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupConfirmSelectPractice" runat="server"></asp:Button>
            <%-- End of Popup for Select Practice Confirmation in covid-19 program  --%>


            <table style="width: 965px; height: 78px" cellspacing="0" cellpadding="0">
                <tbody>
                    <tr>
                        <td style="height: 50px" valign="middle">
                            <asp:Image ID="imgHeaderClaimVoucher" runat="server" AlternateText="<%$ Resources:AlternateText, ClaimVoucherBanner %>"
                                ImageUrl="<%$ Resources:ImageUrl, ClaimVoucherBanner %>" ImageAlign="AbsMiddle"></asp:Image></td>
                        <td style="padding-right: 10px; width: 120px" valign="top" align="right" rowspan="2">
                            <asp:Image ID="imgSchemeLogo" runat="server" Width="102px" Height="78px"></asp:Image></td>
                    </tr>
                    <tr style="height: 28px">
                        <td valign="middle">
                            <asp:Panel ID="panClaimValidatedTimeline" runat="server">
                                <table cellspacing="0" cellpadding="0" border="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="panClaimValidatedTimelineStep1" runat="server" CssClass="highlightTimelineLast">
                                                    <asp:Label ID="lblClaimValidatedStep1" runat="server" Text="<%$ Resources:Text, ClaimStep1 %>"></asp:Label>
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="panClaimValidatedTimelineStep2" runat="server" CssClass="unhighlightTimeline">
                                                    &nbsp;
                                                    <asp:Label ID="lblClaimValidatedStep2" runat="server" Text="<%$ Resources:Text, ClaimStep2 %>"></asp:Label>
                                                </asp:Panel>
                                            </td>
                                            <td>
                                                <asp:Panel ID="panClaimValidatedTimelineStep3" runat="server" CssClass="unhighlightTimeline">
                                                    &nbsp;
                                                    <asp:Label ID="lblClaimValidatedStep3" runat="server" Text="<%$ Resources:Text, ClaimStep3 %>"></asp:Label>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>
            <cc2:MessageBox ID="udcMsgBoxErr" runat="server" Width="99%"></cc2:MessageBox>
            <cc2:InfoMessageBox ID="udcMsgBoxInfo" runat="server" Width="99%" Visible="False"></cc2:InfoMessageBox>
            <asp:HiddenField runat="server" ID="hfEHSClaimTokenNum" EnableViewState="true" />
            <asp:MultiView ID="mvEHSClaim" runat="server" OnActiveViewChanged="mvEHSClaim_ActiveViewChanged">
                <asp:View ID="vStep1" runat="server">
                    <table cellspacing="0" cellpadding="0">
                        <tr style="height: 25px">
                            <td valign="middle" class="eHSTableCaption">
                                <asp:Label ID="lblStep1SearchAccountText" runat="server" Text="<%$ Resources:Text, SearchTempVRAcct %>"></asp:Label></td>
                        </tr>
                    </table>
                    <table cellspacing="0" cellpadding="0">
                        <tr style="height: 25px">
                            <td style="width: 175px; height: 28px" valign="top">
                                <asp:Label ID="lblStep1PracticeText" Width="175px" runat="server" Text="<%$ Resources:Text, Practice %>"
                                    CssClass="tableTitle"></asp:Label></td>
                            <td style="height: 28px" valign="top">
                                <asp:Label ID="lblStep1Practice" runat="server"></asp:Label>
                                <asp:ImageButton ID="btnStep1ChangePractice" runat="server" ImageUrl="~/Images/button/icon_button/btn_change_scheme.png"
                                    ImageAlign="top" /></td>
                        </tr>
                        <tr style="height: 25px">
                            <td style="width: 175px;" valign="top">
                                <asp:Label ID="lblStep1SchemeText" Width="175px" runat="server" Text="<%$ Resources:Text, Scheme %>"
                                    CssClass="tableTitle"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlStep1Scheme" runat="server" Width="430px" AutoPostBack="true">
                                </asp:DropDownList><asp:Label ID="lblStep1SchemeSelectedText" CssClass="tableText"
                                    runat="server" Text="Elderly Health Care Voucher Scheme"></asp:Label>
                            </td>
                        </tr>
                        <asp:Panel ID="panNonClinicSettingStep1" runat="server" Visible="false">
                            <tr style="height: 25px">
                                <td style="padding-bottom: 10px;" valign="middle" width="175px"></td>
                                <td valign="middle" style="padding-bottom: 10px">
                                    <asp:Label ID="lblNonClinicSettingStep1" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr style="min-height:120px">
                            <td style="padding-bottom: 10px; vertical-align: top">
                                <cc2:DocumentTypeRadioButtonGroup ID="udcStep1DocumentTypeRadioButtonGroup" runat="server"
                                    HeaderCss="eHSTableHeading" AutoPostBack="true" HeaderText="<%$ Resources:Text, DocumentType%>"
                                    LegendImageURL="<%$ Resources:ImageUrl, Infobtn%>" LegendImageALT="<%$ Resources:Text, AcceptedDocList%>" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-bottom: 10px">
                                <asp:Label ID="lblSearchECAcctInputSearch" runat="server" Text="<%$ Resources:Text, InputECSearchAccount %>"
                                    CssClass="tableText"></asp:Label>&nbsp;<asp:ImageButton ID="btnStep1InputTips" Visible="false"
                                        runat="server" ImageAlign="AbsMiddle" />
                                <asp:ImageButton ID="btnStep1ReadSmartIDTips" Visible="false"
                                    runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources: ImageUrl, ReadSmartIDTipsBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ReadSmartIDTipsBtn %>" />

                            </td>
                        </tr>
                    </table>
                    <uc1:ucClaimSearch ID="udcClaimSearch" runat="server"></uc1:ucClaimSearch>
                </asp:View>
                <asp:View ID="vStep2a" runat="server">
                    <table cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td valign="middle" class="eHSTableCaption">
                                    <asp:Label ID="lblStep2aHeaderText" runat="server" Text="<%$ Resources:Text, EnterDetails %>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="eHSTableHeading" style="vertical-align:top;width:200px">
                                    <asp:Label ID="lblStep2aAcctInfo" runat="server" Text="<%$ Resources:Text, AccountInfo%>"></asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton ID="ibtnVaccinationRecord" runat="server" ImageUrl="<%$ Resources: ImageUrl, VaccinationRecordBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, VaccinationRecordBtn %>" OnClick="ibtnVaccinationRecord_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <asp:Panel ID="panStep2aReminder" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0" width="800">
                            <tr>
                                <td style="background-attachment: inherit; background-image: url(../Images/others/reminder_topleft.png); width: 30px; background-repeat: no-repeat; height: 30px"></td>
                                <td style="background-image: url(../Images/others/reminder_topmiddle.png); width: 740px; background-repeat: repeat-x; height: 30px"></td>
                                <td style="background-image: url(../Images/others/reminder_topright.png); width: 30px; background-repeat: no-repeat; height: 30px"></td>
                            </tr>
                            <tr>
                                <td style="background-image: url(../Images/others/reminder_left.png); width: 30px; background-repeat: repeat-y"></td>
                                <td style="line-height: 20px; background-color: #f9f9f9; width: 740px;">
                                    <asp:Label ID="lblStep2aReminder" runat="server" CssClass="tableText"></asp:Label></td>
                                <td style="background-image: url(../Images/others/reminder_right.png); width: 30px; background-repeat: repeat-y"></td>
                            </tr>
                            <tr>
                                <td style="background-image: url(../Images/others/reminder_bottomleft.png); width: 30px; background-repeat: no-repeat; height: 30px"></td>
                                <td style="background-image: url(../Images/others/reminder_bottommiddle.png); width: 740px; background-repeat: repeat-x; height: 30px"></td>
                                <td style="background-image: url(../Images/others/reminder_bottomright.png); width: 30px; background-repeat: no-repeat; height: 30px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td colspan="2">
                                    <uc2:ucReadOnlyDocumnetType ID="udcStep2aReadOnlyDocumnetType" runat="server"></uc2:ucReadOnlyDocumnetType>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr width="100%" />
                                </td>
                            </tr>
                    </table>

                    <asp:Panel ID="panStep2aMedicalExemptionRecord" runat="server">
                        <table cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td class="eHSTableHeading">
                                    <asp:Label ID="lblMedicalExemptionRecordHeading" runat="server" Text="<%$ Resources:Text, MedicalExemptionRecordsHeading%>" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="panMedicalExemptionRecord" runat="server">
                                        <table style="border:0px solid;padding:0px;border-spacing:2px;border-collapse:separate;border-color:#CCCCCC;width:930px">
                                            <tr style="background-color:#70AD47">
                                                <td style="width:125px;vertical-align:middle;text-align:center;border:0px solid;border-color:transparent">
                                                    <asp:Label ID="lblNoMedicalExemptionRecordDOI" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, DateOfIssue %>" />
                                                </td>
                                                <td style="width:125px;vertical-align:middle;text-align:center;border:0px solid;border-color:transparent">
                                                    <asp:Label ID="lblNoMedicalExemptionRecordIssuer" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, Issuer %>" />
                                                </td>
                                                <td style="width:125px;vertical-align:middle;text-align:center;border:0px solid;border-color:transparent">
                                                    <asp:Label ID="lblNoMedicalExemptionRecordValidUntil" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, ValidUntil %>" />
                                                </td>
                                            </tr>
                                            <tr style="background-color:#D5E3CF">
                                                <td style="vertical-align:middle;border:0px solid;border-color:transparent;height:20px" colspan="3">
                                                    <asp:Label ID="lblNoMedicalExemptionRecord" runat="server" CssClass="tableText" style="font-size:medium;position:relative;left:3px"
                                                        Text="<%$ Resources:Text, NoRecordsFound%>" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:GridView ID="gvMedicalExemptionRecord" runat="server" AutoGenerateColumns="False" Width="930px" BorderColor="Transparent"
                                        AllowSorting="True"  AllowPaging="False"  OnRowDataBound="gvMedicalExemptionRecord_RowDataBound"
                                        OnPreRender="gvMedicalExemptionRecord_PreRender" OnSorting="gvMedicalExemptionRecord_Sorting">
                                        <Columns>
                                            <asp:TemplateField SortExpression="DateOfIssueDtmSorting" HeaderText="<%$ Resources: Text, DateOfIssue %>">
                                                <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#70AD47" BorderColor="Transparent"/>                                  
                                                <ItemStyle Width="125px" VerticalAlign="Top" Font-Size="Medium" BorderColor="Transparent" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMedicalExemptionRecordDOI" runat="server" Text='<%# Bind("DateOfIssueDtm")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ServiceProviderSorting" HeaderText="<%$ Resources: Text, Issuer %>">
                                                <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#70AD47" BorderColor="Transparent"/>
                                                <ItemStyle Width="280px" VerticalAlign="Top" Font-Size="Medium" BorderColor="Transparent" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMedicalExemptionRecordSP" runat="server" Text='<%# Bind("ServiceProviderEng")%>' />
                                                    <asp:Label ID="lblMedicalExemptionRecordSPChi" runat="server" Text='<%# Bind("ServiceProviderChi")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ValidUntilDtmSorting" HeaderText="<%$ Resources: Text, ValidUntil %>">
                                                <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#70AD47" BorderColor="Transparent"/>
                                                <ItemStyle Width="280px" VerticalAlign="Top" Font-Size="Medium" BorderColor="Transparent" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMedicalExemptionRecordValidUntil" runat="server" Text='<%# Bind("ValidUntilDtm")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <AlternatingRowStyle BackColor="#D5E3CF" ForeColor="Black"  />                              
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-bottom:10px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="panStep2aOtherVaccinationRecord" runat="server">
                        <table cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td class="eHSTableHeading">
                                    <asp:Label ID="lblCOtherVaccinationRecordHeading" runat="server" Text="<%$ Resources:Text, VaccinationOtherRecordForCOVID19%>" />
                                    <asp:Button ID="btnModalPopupOtherVaccinationRecordRemark" runat="server" BorderWidth="0" BorderStyle="None" Width="17px" Height="17px"
                                        AlternateText="<%$ Resources:Text, Remarks%>" 
                                        style="vertical-align:top;position:relative;top:2px;background-color: transparent;background-image:url('../Images/others/info.png');background-repeat: no-repeat;"
                                        onmouseover="this.style.cursor='pointer'" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="panNoOtherVaccinationRecord" runat="server">
                                        <table style="border:0px solid;padding:0px;border-spacing:2px;border-collapse:separate;border-color:#CCCCCC;width:930px">
                                            <tr style="background-color:#5b9bd5">
                                                <td style="width:125px;vertical-align:middle;text-align:center;border:0px solid;border-color:transparent">
                                                    <asp:Label ID="lblCOtherInjectionDate" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, InjectionDate %>" />
                                                </td>
                                                <td style="width:280px;vertical-align:middle;text-align:center;border:0px solid;border-color:transparent">
                                                    <asp:Label ID="lblCOtherVaccines" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, Vaccines %>" />
                                                </td>
                                            </tr>
                                            <tr style="background-color:#d2deef">
                                                <td style="vertical-align:middle;border:0px solid;border-color:transparent;height:20px" colspan="2">
                                                    <asp:Label ID="lblCNoOtherRecord" runat="server" CssClass="tableText" style="font-size:medium;position:relative;left:3px"
                                                        Text="<%$ Resources:Text, NoVaccinationRecordFound%>" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:GridView ID="gvCOtherVaccinationRecord" runat="server" AutoGenerateColumns="False" Width="930px" BorderColor="Transparent"
                                        AllowSorting="True"  AllowPaging="False"  OnRowDataBound="gvCOtherVaccinationRecord_RowDataBound"
                                        OnPreRender="gvCOtherVaccinationRecord_PreRender" OnSorting="gvCOtherVaccinationRecord_Sorting">
                                        <Columns>
                                            <asp:TemplateField SortExpression="ServiceReceiveDtmSorting" HeaderText="<%$ Resources: Text, InjectionDate %>">
                                                <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#5b9bd5" BorderColor="Transparent"/>                                  
                                                <ItemStyle Width="125px" VerticalAlign="Top" Font-Size="Medium" BorderColor="Transparent" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCOtherInjectionDate" runat="server" Text='<%# Bind("ServiceReceiveDtm") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="SubsidizeDescSorting" HeaderText="<%$ Resources: Text, Vaccines %>">
                                                <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#5b9bd5" BorderColor="Transparent"/>
                                                <ItemStyle Width="280px" VerticalAlign="Top" Font-Size="Medium" BorderColor="Transparent" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCOtherVaccination" runat="server" Text='<%# Bind("SubsidizeDesc")%>' />
                                                    <asp:Label ID="lblCOtherVaccinationChi" runat="server" Text='<%# Bind("SubsidizeDescChi") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <AlternatingRowStyle BackColor="#BFE4FF" ForeColor="Black"  />
                                        
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-bottom:20px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="panStep2aC19VaccinationRecord" runat="server">
                        <table cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td class="eHSTableHeading">
                                    <asp:Label ID="lblCC19VaccinationRecordHeading" runat="server" Text="<%$ Resources:Text, VaccinationRecordForCOVID19%>" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="panNoC19VaccinationRecord" runat="server">
                                        <table style="border:1px solid;padding:0px;border-spacing:1px;border-collapse:collapse;border-color:#CCCCCC;width:930px">
                                            <tr style="background-color:#f08000">
                                                <td style="width:16px;vertical-align:middle;text-align:center;border:1px solid;border-color:#61615b;height:20px"">
                                                    &nbsp;
                                                </td>
                                                <td style="width:125px;vertical-align:middle;text-align:center;border:1px solid;border-color:#61615b">
                                                    <asp:Label ID="lblCInjectionDate" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, InjectionDate %>" />
                                                </td>
                                                <td style="width:280px;vertical-align:middle;text-align:center;border:1px solid;border-color:#61615b">
                                                    <asp:Label ID="lblCVaccines" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, Vaccines %>" />
                                                </td>
                                                <td style="width:80px;vertical-align:middle;text-align:center;border:1px solid;border-color:#61615b">
                                                    <asp:Label ID="lblCDose" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, DoseSeqShort %>" />
                                                </td>
                                                <td style="width:230px;vertical-align:middle;text-align:center;border:1px solid;border-color:#61615b">
                                                    <asp:Label ID="lblCInformationProvider" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, InformationProvider %>" />
                                                </td>
                                                <td style="vertical-align:middle;text-align:center;border:1px solid;border-color:#61615b">
                                                    <asp:Label ID="lblCRemarks" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, Remarks %>" />
                                                </td>
                                            </tr>
                                            <tr style="background-color:#F7F7DE">
                                                <td style="vertical-align:middle;border:1px solid;border-color:#61615b;height:20px" colspan="6">
                                                    <asp:Label ID="lblCNoRecord" runat="server" CssClass="tableText" style="font-size:medium;position:relative;left:3px"
                                                        Text="<%$ Resources:Text, NoCOVID19VaccinationRecordFound%>" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:GridView ID="gvCC19VaccinationRecord" runat="server" AutoGenerateColumns="False" Width="930px"
                                        AllowSorting="True"  AllowPaging="False"  OnRowDataBound="gvCC19VaccinationRecord_RowDataBound"
                                        OnPreRender="gvCC19VaccinationRecord_PreRender" OnSorting="gvCC19VaccinationRecord_Sorting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#f08000"/>
                                                <ItemStyle Width="16px" VerticalAlign="Top" Font-Size="Medium" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ServiceReceiveDtmSorting" HeaderText="<%$ Resources: Text, InjectionDate %>">
                                                <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#f08000" />
                                                <ItemStyle Width="125px" VerticalAlign="Top" Font-Size="Medium" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCInjectionDate" runat="server" Text='<%# Bind("ServiceReceiveDtm") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="SubsidizeDescSorting" HeaderText="<%$ Resources: Text, Vaccines %>">
                                                <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#f08000" />
                                                <ItemStyle Width="280px" VerticalAlign="Top" Font-Size="Medium" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCVaccination" runat="server" Text='<%# Bind("SubsidizeDesc") %>'></asp:Label>
                                                    <asp:Label ID="lblCVaccinationChi" runat="server" Text='<%# Bind("SubsidizeDescChi") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="AvailableItemDesc" HeaderText="<%$ Resources: Text, DoseSeqShort %>">
                                                <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#f08000" />
                                                <ItemStyle Width="80px" VerticalAlign="Top" Font-Size="Medium" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCDose" runat="server"></asp:Label>
                                                    <asp:Label ID="lblCDoseChi" runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Provider" HeaderText="<%$ Resources: Text, InformationProvider %>">
                                                <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#f08000" />
                                                <ItemStyle Width="230px" VerticalAlign="Top" Font-Size="Medium" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCProvider" runat="server" Text='<%# Bind("Provider") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Remark" HeaderText="<%$ Resources: Text, Remarks %>">
                                                <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#f08000" />
                                                <ItemStyle VerticalAlign="Top" Font-Bold="true" Font-Size="Medium"/>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCRemark" runat="server" Text='<%# Bind("Remark") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-bottom:20px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="panStep2aDischargeRecord" runat="server" Visible="false">
                        <table cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td class="eHSTableHeading">
                                    <asp:Label ID="lblCDischargeRecordHeading" runat="server" Text="<%$ Resources: Text, DischargeRecordForCOVID19 %>" />
                                    <asp:Button ID="btnModalPopupDischargeRecordRemark" runat="server" BorderWidth="0" BorderStyle="None" Width="17px" Height="17px"
                                        AlternateText="<%$ Resources:Text, Remarks%>" 
                                        style="vertical-align:top;position:relative;top:2px;background-color: transparent;background-image:url('../Images/others/info.png');background-repeat: no-repeat;"
                                        onmouseover="this.style.cursor='pointer'" Visible ="false"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="border:1px solid;padding:0px;border-spacing:1px;border-collapse:collapse;border-color:#CCCCCC;width:930px">
                                        <tr style="background-color:#70ae47">
                                            <td style="width:465px;vertical-align:middle;text-align:center;border:1px solid;border-color:#61615b">
                                                <asp:Label ID="lblCDischargeDateText" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, HospitalDischargeDate %>" />
                                            </td>
                                            <td style="vertical-align:middle;text-align:center;border:1px solid;border-color:#61615b">
                                                <asp:Label ID="lblCDischargeRemarkText" runat="server" style="color:white;font-size:medium" Text="<%$ Resources: Text, Remarks %>" />
                                            </td>
                                        </tr>
                                        <tr style="background-color:white">
                                            <td style="vertical-align:middle;border:1px solid;border-color:#61615b;height:20px">
                                                <asp:Label ID="lblCDischargeDate" runat="server" CssClass="tableText" style="font-size:medium;position:relative;left:3px" />
                                            </td>
                                            <td style="vertical-align:middle;border:1px solid;border-color:#61615b;height:20px">
                                                <asp:Label ID="lblCDischargeRemark" runat="server" CssClass="tableText" style="font-size:medium;position:relative;left:3px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-bottom:20px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <table cellspacing="0" cellpadding="0" border="0" style="width: 100%">
                        <tr>
                            <td>
                                <table cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td class="eHSTableHeading" colspan="2">
                                            <asp:Label ID="lblStep2aClaimInfo" runat="server" Text="<%$ Resources:Text, ClaimInfo%>"></asp:Label></td>
                                    </tr>
                                </table>
                                <table cellspacing="0" cellpadding="0" border="0">
                                    <tbody>
                                        <tr>
                                            <td height="25" style="width: 205px" valign="top">
                                                <asp:Label ID="lblStep2aPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>"
                                                    CssClass="tableTitle" Width="160px"></asp:Label></td>
                                            <td valign="top" height="25">
                                                <asp:Label ID="lblStep2aPractice" runat="server" CssClass="tableTitleChi"></asp:Label>
                                                <asp:ImageButton ID="btnStep2aChangePractice" runat="server" AlternateText="<%$ Resources:AlternateText, ChangePracticeBtn %>"
                                                    ImageUrl="~/Images/button/icon_button/btn_change_scheme.png" ImageAlign="Top"></asp:ImageButton><asp:TextBox
                                                        ID="hfStep2aPractice" runat="server" Visible="false"></asp:TextBox></td>
                                        </tr>
                                    </tbody>
                                </table>
                                <asp:Panel ID="panStep2aClaimDetaila" runat="server" Width="100%">
                                    <table style="width: 660px" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="width: 205px; height: 25px" valign="top">
                                                <asp:Label ID="lblStep2aSchemeText" runat="server" CssClass="tableTitle" Width="205px"></asp:Label></td>
                                            <td style="vertical-align: top">
                                                <asp:Label ID="lblStep2aSchemeSelectedText" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:DropDownList ID="ddlStep2aScheme" runat="server" Width="430px" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <asp:Panel ID="panNonClinicSettingStep2a" runat="server" Visible="false">
                                            <tr style="height: 25px">
                                                <td style="padding-bottom: 10px;" valign="middle" width="205"></td>
                                                <td valign="middle" style="padding-bottom: 10px">
                                                    <asp:Label ID="lblNonClinicSettingStep2a" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                        </asp:Panel>
                                        <tr>
                                            <td style="width: 205px; height: 23px;" valign="top" class="tableCellStyle">
                                                <asp:Label ID="lblStep2aServiceDateText" runat="server" CssClass="tableTitle" Width="205px" Style="top: 1px; position: relative;"></asp:Label></td>
                                            <td style="vertical-align: top">
                                                <table style="border-collapse: collapse; border-spacing: 0px 0px; margin: 0px">
                                                    <tr>
                                                        <td style="vertical-align: top; padding-left: 0px">
                                                            <asp:TextBox ID="txtStep2aServiceDate" runat="server" Width="71px" Height="15" ForeColor="DimGray"
                                                                MaxLength="10" AutoPostBack="True" OnTextChanged="txtStep2aServiceDate_TextChanged"
                                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                                onblur="filterDateInput(this);"></asp:TextBox>
                                                            <cc1:CalendarExtender runat="server" ID="Step2aCalendarExtenderServiceDate" CssClass="ajax_cal" PopupPosition="BottomLeft" TargetControlID="txtStep2aServiceDate"
                                                                PopupButtonID="btnStep2aServiceDateCal" Format="dd-MM-yyyy"></cc1:CalendarExtender>
                                                            <cc1:FilteredTextBoxExtender ID="filtereditStep2aDtlServiceDate" runat="server" TargetControlID="txtStep2aServiceDate"
                                                                FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                                            <asp:Label ID="lblStep2aServiceDate" runat="server" CssClass="tableText"></asp:Label>
                                                        </td>
                                                        <td style="padding-left: 5px; vertical-align: central">
                                                            <asp:ImageButton ID="btnStep2aServiceDateCal" runat="server" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                                                                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" ImageAlign="AbsMiddle" Style="vertical-align: top"></asp:ImageButton>
                                                        </td>
                                                        <td style="padding-left: 5px; vertical-align: top">
                                                            <asp:Image ID="imgStep2aServiceDateError" runat="server" Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="AbsMiddle" Style="vertical-align: top"></asp:Image>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                            <td style="max-width: 34%">
                                <asp:Panel ID="panNotice" runat="server" Width="93%" Height="100px" BorderColor="#007FC2" BorderWidth="2px" BorderStyle="solid">
                                    <table border="0" style="width: 100%; height: 100%; border-collapse: collapse;" cellspacing="0">
                                        <tr style="height: 15%;">
                                            <td width="40%" align="center" bgcolor="#007FC2">
                                                <!--empty-->
                                            </td>
                                            <td width="20%" align="center" bgcolor="#007FC2">
                                                <asp:Label ID="lblNoticeTitle" runat="server" ForeColor="white" Font-Bold="true" Text="<%$ Resources:Text, Notice %>" Font-Size="11pt"></asp:Label>
                                            </td>
                                            <td width="20%" align="center" bgcolor="#007FC2">
                                                <!--empty-->
                                            </td>
                                            <td width="20%" align="center" bgcolor="#007FC2" style="vertical-align: top">
                                                <asp:Panel ID="panNoticeNew" runat="server" Width="100%" Height="100%">
                                                    <div style="background-image: url(../Images/others/NoticeNewbg.gif); width: 100%;">
                                                        <asp:Label ID="lblNew" runat="server" ForeColor="Red" Font-Bold="true" Text="<%$ Resources:Text, NewNotice %>" Font-Size="11pt"></asp:Label>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr style="height: 75%;" bgcolor="#FFFFFF">
                                            <td colspan="4" align="left" valign="middle">
                                                <asp:Literal ID="ltlNoticeContent" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="panStep2aClaimDetailb" runat="server" Width="100%">
                        <table cellspacing="0" cellpadding="0" border="0">
                            <tbody>
                                <tr>
                                    <td>
                                        <uc3:ucInputEHSClaim ID="udcStep2aInputEHSClaim" runat="server" Visible="true"></uc3:ucInputEHSClaim>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="panStep2aRecipinetContactInfo" runat="server">
                        <table cellspacing="0" cellpadding="0" border="0">
                            <tr id="trStep2aContactNo" runat="server" visible="false">
                                <td height="25" style="width: 205px;vertical-align:top;padding-top:5px">
                                    <asp:Label ID="lblStep2aContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo2%>" CssClass="tableTitle" Width="160px" />
                                </td>
                                <td height="25" style="vertical-align:top;padding-top:3px">
                                    <asp:textbox ID="txtStep2aContactNo" runat="server" MaxLength="8" style="position:relative;left:-1px" Width="100px"/>
                                    <asp:Image ID="imgStep2aContactNoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" visible="false" />
                                    <asp:Label ID="lblStep2aContactNoRecommendation" runat="server" Text="<%$ Resources:Text, ProvideContactNoWithSMS%>" style="font-size:14px"></asp:Label>
                                    <cc1:FilteredTextBoxExtender ID="fteStep2aContactNo" runat="server" TargetControlID="txtStep2aContactNo"
                                                                FilterType="Numbers" />
                                </td>
                                <td height="25" style="vertical-align:top;padding-top:3px;padding-left:5px">
                                    <asp:Label ID="lblStep2aMobile" runat="server" Text="<%$ Resources:Text, Mobile%>" CssClass="tableTitle" style="position:relative;top:-1px" Visible="false"/>
                                    <asp:checkbox ID="chkStep2aMobile" runat="server" AutoPostBack="false" style="position:relative;left:-1px" Visible="false"/>
                                </td>
                            </tr>
                            <tr id="trStep2aRemark" runat="server" visible="false">
                                <td height="25" style="width: 205px;vertical-align:top;padding-top:2px">
                                    <asp:Label ID="lblStep2aRemarkText" runat="server" Text="<%$ Resources:Text, Remarks%>" CssClass="tableTitle" Width="160px" />
                                </td>
                                <td height="25" style="vertical-align:top;padding-top:3px" colspan="2">
                                    <asp:textbox ID="txtStep2aRemark" runat="server" MaxLength="100" style="position:relative;left:-1px" Width="660px"/>
                                    <asp:Image ID="imgStep2aRemarkError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" visible="false" />
                                    <cc1:FilteredTextBoxExtender ID="fteStep2aRemark" runat="server" TargetControlID="txtStep2aRemark"
                                      FilterMode="InvalidChars"  InvalidChars="|\&quot;"></cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="panStep2aPrintClaimConsentForm" runat="server">
                        <table cellspacing="0" cellpadding="0" border="0" style="min-height: 50px">
                            <tr>
                                <td style="width: 200px;padding-right: 5px"></td>
                                <td>
                                    <asp:Panel ID="panlblStep2aPrintConsent" runat="server">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:ImageButton ID="btnStep2aPrintClaimConsentForm" runat="server" AlternateText="<%$ Resources:AlternateText, VRAPrintClaimConsentFormBtn%>"
                                                        ImageUrl="<%$ Resources:ImageUrl, VRAPrintClaimConsentFormBtn%>" />
                                                    <asp:HiddenField ID="hfStep2aCurrentPrintOption" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rbStep2aPrintClaimConsentFormLanguage" runat="server" RepeatDirection="Horizontal" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="panStep2aPerprintFormNotice" runat="server">
                                        <table cellspacing="0" cellpadding="0" border="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        <img id="imgStep2aPerprintFormNotice" src="../Images/others/ExclamationMark%20.gif" alt="" />&nbsp;</td>
                                                    <td>
                                                        <asp:Label ID="lblStep2aPerprintFormNotice" runat="server" Text="<%$ Resources:Text, ReminPreprintOption %>"
                                                            ForeColor="Black"/>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td valign="top" rowspan="2" style="padding-bottom: 5px; padding-left: 5px;">
                                    <asp:ImageButton ID="btnStep2aChangePrintOption" runat="server" AlternateText="<%$ Resources:AlternateText, ChangePrintOption%>"
                                        ImageUrl="~/Images/button/icon_button/btn_change_print_option.png" ImageAlign="AbsMiddle"></asp:ImageButton></td>
                                <td rowspan="2" style="vertical-align:middle">
                                    <asp:Panel ID="panStep2aAdhocPrint" runat="server">
                                        <asp:ImageButton ID="btnStep2aPrintAdhocClaimConsentForm" runat="server" />
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="panStep2aDeclareClaim" runat="server">
                        <div style="padding-top:3px"></div>
                        <table cellspacing="0" cellpadding="0" border="0" style="position:relative;left:-1px">
                            <tr id="trStep2aDeclareClaim" runat="server">
                                <td style="width: 930px; min-height: 48px; vertical-align: top;background-color:white!important" class="checkboxStyle">
                                    <asp:label ID="lblStep2aDeclareClaim1" runat="server" AssociatedControlID="chkStep2aDeclareClaim" Text="<%$ Resources:Text, VerificationCheckList %>" />
                                    <table style="width:100%;border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                                        <tr style="min-height :20px;background-color:#fff894">
                                            <td style="vertical-align:top;width:30px;padding-top:10px;padding-bottom:5px">
                                                <asp:CheckBox ID="chkStep2aDeclareClaim" runat="server" AutoPostBack="false" style="position:relative;left:2px;top:3px;-ms-transform:scale(1.5)"/>
                                            </td>
                                            <td style="text-align:left;padding-top:10px;padding-bottom:5px">
                                                <asp:label ID="lblStep2aDeclareClaim2" AssociatedControlID="chkStep2aDeclareClaim" runat="server" Text="<%$ Resources:Text, VerificationConfirmStatement %>" />
                                            </td>
                                        </tr>
                                        <asp:Panel ID="panStep2aDeclareJoineHRSS" runat="server">
                                        <tr style="min-height :4px">
                                            <td></td>
                                        </tr>             
                                        <tr id="trStep2aDeclareJoineHRSS" runat="server" style="min-height :20px;background-color:#fff894">
                                            <td style="vertical-align:top;width:30px;padding-top:5px;padding-bottom:5px">
                                                <asp:CheckBox ID="chkStep2aDeclareJoineHRSS" runat="server" AutoPostBack="false" style="position:relative;left:2px;top:3px;-ms-transform:scale(1.5)"/>
                                            </td>
                                            <td style="text-align:left;padding-top:5px;padding-bottom:5px">
                                                <asp:label ID="lblStep2aDeclareJoineHRSS" AssociatedControlID="chkStep2aDeclareJoineHRSS" runat="server" Text="<%$ Resources:Text, VerificationConfirmJoineHRSS %>" />
                                            </td>
                                        </tr> 
                                        </asp:Panel>                                                                                                  
                                    </table>
                                </td>
                                <td style="vertical-align:top;padding-left:5px">
                                    <asp:ImageButton ID="imgStep2aDeclareClaimError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" visible="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="panStep2aDeclareNonLocalRecoveredHistory" runat="server">
                        <div style="padding-top:10px"></div>
                        <table cellspacing="0" cellpadding="0" border="0" style="position:relative;left:-1px">
                            <tr id="trStep2aDeclareNonLocalRecoveredHistory" runat="server">
                                <td style="width: 930px; min-height: 48px; vertical-align: top;background-color:white!important" class="checkboxStyle">
                                    <asp:label ID="lblStep2aDeclareNonLocalRecoveredHistory" runat="server" 
                                        AssociatedControlID="chkStep2aDeclareNonLocalRecoveredHistory" 
                                        Text="<%$ Resources:Text, NonLocalRecoveredHistoryDeclare %>" />
                                    <table style="width:100%;border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                                        <tr style="min-height :20px">
                                            <td style="vertical-align:top;width:30px;padding-top:10px;padding-bottom:5px">
                                                <asp:CheckBox ID="chkStep2aDeclareNonLocalRecoveredHistory" runat="server" AutoPostBack="false" style="position:relative;left:2px;top:3px;-ms-transform:scale(1.5)"/>
                                            </td>
                                            <td style="text-align:left;padding-top:10px;padding-bottom:5px">
                                                <asp:label ID="lblStep2aDeclareNonLocalRecoveredHistoryContent" 
                                                    AssociatedControlID="chkStep2aDeclareNonLocalRecoveredHistory" runat="server" 
                                                    Text="<%$ Resources:Text, NonLocalRecoveredHistoryContent %>" />
                                            </td>
                                        </tr>                                                                                             
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="panStep2aMedicalExemptionJoinEHRSS" runat="server">
                        <div style="padding-top:3px"></div>
                        <table cellspacing="0" cellpadding="0" border="0" style="position:relative;left:-1px">
                            <tr>
                                <td style="width: 930px; min-height: 48px; vertical-align: top;background-color:white!important" class="checkboxStyle">
                                    <table style="width:100%;border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">        
                                        <tr id="trStep2aMedicalExemptionJoinEHRSS" runat="server" style="min-height :20px;background-color:#fff894">
                                            <td style="vertical-align:top;width:30px;padding-top:5px;padding-bottom:5px">
                                                <asp:CheckBox ID="chkStep2aMedicalExemptionJoinEHRSS" runat="server" AutoPostBack="false" style="position:relative;left:2px;top:3px;-ms-transform:scale(1.5)"/>
                                            </td>
                                            <td style="text-align:left;padding-top:5px;padding-bottom:5px">
                                                <asp:label ID="lblStep2aMedicalExemptionJoinEHRSS_1" AssociatedControlID="chkStep2aMedicalExemptionJoinEHRSS" runat="server" Text="<%$ Resources:Text, MedicalExemptionsJoinEHRSS_1 %>" />
                                                <br />
                                                <asp:label ID="lblStep2aMedicalExemptionJoinEHRSS_2" AssociatedControlID="chkStep2aMedicalExemptionJoinEHRSS" runat="server" Text="<%$ Resources:Text, MedicalExemptionsJoinEHRSS_2 %>" />
                                                <asp:textbox ID="txtStep2aMedicalExemptionContactNo" runat="server" MaxLength="8" style="position:relative;left:-1px" Width="100px"/>
                                                <asp:Image ID="imgStep2aMedicalExemptionContactNoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" visible="false" />                                                
                                                <asp:label ID="lblStep2aMedicalExemptionJoinEHRSS_3" AssociatedControlID="chkStep2aMedicalExemptionJoinEHRSS" runat="server" Text="<%$ Resources:Text, MedicalExemptionsJoinEHRSS_3 %>" />
                                                <cc1:FilteredTextBoxExtender ID="fteStep2aMedicalExemptionContactNo" runat="server"
                                                     TargetControlID="txtStep2aMedicalExemptionContactNo" FilterType="Numbers" />
                                            </td>
                                        </tr>                                                                                             
                                    </table>
                                </td>
                                <td style="vertical-align:top;padding-left:5px">
                                    <asp:Image ID="imgStep2aMedicalExemptionJoinEHRSSError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" visible="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                    <table cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td style="width: 200px; padding-top: 10px;padding-right: 5px"></td>
                                <td style="padding-top: 10px">
                                    <asp:ImageButton ID="btnStep2aCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn%>" 
                                        OnClientClick="return ReasonForVisitInitialComplete();"></asp:ImageButton>
                                    <asp:ImageButton ID="btnStep2aClaim" runat="server" AlternateText="<%$ Resources:AlternateText, ClaimBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, ClaimBtn %>" OnClientClick="return ReasonForVisitInitialComplete();"></asp:ImageButton>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:View>
                <asp:View ID="vStep2b" runat="server">
                    <table cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td valign="middle" class="eHSTableCaption">
                                    <asp:Label ID="lblStep2bText" runat="server" Text="<%$ Resources:Text,ConfirmDetail %>"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="height: 20px" class="eHSTableHeading">
                                    <asp:Label ID="lblStep2bAcctInfo" runat="server" Text="<%$ Resources:Text, AccountInfo%>"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <uc2:ucReadOnlyDocumnetType ID="udcStep2bReadOnlyDocumnetType" runat="server"></uc2:ucReadOnlyDocumnetType>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td class="eHSTableHeading" valign="top" colspan="2">
                                    <asp:Label ID="lblStep2bClaimInfo" runat="server" Text="<%$ Resources:Text, ClaimInfo%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableCellStyle" style="width: 205px" valign="top">
                                    <asp:Label ID="lblStep2bSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"
                                        CssClass="tableTitle" Width="160px"></asp:Label></td>
                                <td class="tableCellStyle" valign="top">
                                    <asp:Label ID="lblStep2bScheme" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <asp:Panel ID="panNonClinicSettingStep2b" runat="server" Visible="false">
                                <tr style="height: 25px">
                                    <td class="tableCellStyle" style="width: 205px" valign="top"></td>
                                    <td class="tableCellStyle" valign="top">
                                        <asp:Label ID="lblNonClinicSettingStep2b" runat="server" CssClass="tableText" Style="position: relative; top: -1px"></asp:Label>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr>
                                <td style="width: 205px;" valign="top" class="tableCellStyle">
                                    <asp:Label ID="lblStep2bServiceDateText" runat="server" Text="<%$ Resources:Text, ServiceDate %>"
                                        CssClass="tableTitle" Width="160px"></asp:Label></td>
                                <td valign="top" class="tableCellStyle">
                                    <asp:Label ID="lblStep2bServiceDate" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 205px;" valign="top" class="tableCellStyle">
                                    <asp:Label ID="lblStep2bPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>"
                                        CssClass="tableTitle" Width="160px"></asp:Label></td>
                                <td valign="top" class="tableCellStyle">
                                    <asp:Label ID="lblStep2bPractice" runat="server" CssClass="tableTextChi"></asp:Label></td>
                            </tr>
                            <tr id="trStep2bBankAcct" runat="server">
                                <td class="tableCellStyle" style="width: 205px" valign="top">
                                    <asp:Label ID="lblStep2bBankAcctText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, BankAccountNo %>"
                                        Width="160px"></asp:Label></td>
                                <td class="tableCellStyle" valign="top">
                                    <asp:Label ID="lblStep2bBankAcct" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trStep2bServiceType" runat="server">
                                <td class="tableCellStyle" style="width: 205px" valign="top">
                                    <asp:Label ID="lblStep2bServiceTypeText" runat="server" CssClass="tableTitle"
                                        Text="<%$ Resources:Text, ServiceType %>" Width="160px"></asp:Label></td>
                                <td class="tableCellStyle" valign="top">
                                    <asp:Label ID="lblStep2bServiceType" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                        </tbody>
                    </table>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <uc4:ucReadOnlyEHSClaim ID="udcStep2bReadOnlyEHSClaim" runat="server"></uc4:ucReadOnlyEHSClaim>
                            </td>
                        </tr>
                    </table>

                    <asp:Panel ID="panStep2bRecipinetContactInfo" runat="server">
                        <table cellspacing="0" cellpadding="0" border="0">
                            <tr id="trStep2bContactNo" runat="server" visible="false">
                                <td height="25" style="width: 205px;vertical-align:top;">
                                    <asp:Label ID="lblStep2bContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo2%>" CssClass="tableTitle" Width="160px" />
                                </td>
                                <td height="25" style="vertical-align:top;">
                                    <asp:Label ID="lblStep2bContactNo" runat="server" CssClass="tableText" style="position:relative;left:1px"/>&nbsp;
                                    <asp:Label ID="lblStep2bContactNoNotAbleSMS" runat="server" CssClass="tableText" Text="<%$ Resources:Text, NotAbleToReceiveSMS%>" style="color:red!important" />
                                </td>
                            </tr>
                            <asp:Panel ID="panStep2bRemark" runat="server">
                            <tr>
                                <td height="25" style="width: 205px;vertical-align:top;">
                                    <asp:Label ID="lblStep2bRemarkText" runat="server" Text="<%$ Resources:Text, Remarks%>" CssClass="tableTitle" Width="160px" />
                                </td>
                                <td height="25" style="vertical-align:top;">
                                    <asp:Label ID="lblStep2bRemark" runat="server" CssClass="tableText" style="position:relative;left:1px"/>
                                </td>
                            </tr>
                            </asp:Panel>
                            <asp:Panel ID="panStep2bJoinEHRSS" runat="server">
                            <tr>
                                <td height="25" style="width: 205px;vertical-align:top;">
                                    <asp:Label ID="lblStep2bJoinEHRSSText" runat="server" Text="<%$ Resources:Text, JoinEHRSS%>" CssClass="tableTitle" Width="160px" />
                                </td>
                                <td height="25" style="vertical-align:top;">
                                    <asp:Label ID="lblStep2bJoinEHRSS" runat="server" CssClass="tableText" style="position:relative;left:1px"/>
                                </td>
                            </tr>
                            </asp:Panel>
                            <asp:Panel ID="panStep2bNonLocalRecoveredHistory" runat="server">
                            <tr>
                                <td height="25" style="width: 205px;vertical-align:top;">
                                    <asp:Label ID="lblStep2bNonLocalRecoveredHistoryText" runat="server" Text="<%$ Resources:Text, NonLocalRecoveredHistory%>" CssClass="tableTitle" />
                                </td>
                                <td height="25" style="vertical-align:top;">
                                    <asp:Label ID="lblStep2bNonLocalRecoveredHistory" runat="server" CssClass="tableText" style="position:relative;left:1px"/>
                                </td>
                            </tr>
                            </asp:Panel>
                        </table>
                    </asp:Panel>

                    <asp:Panel ID="panStep2bPrintClaimConsentForm" runat="server">
                        <table cellspacing="0" cellpadding="0" border="0" style="min-height: 50px">
                            <tbody>
                                <tr>
                                    <td style="width: 200px;padding-right: 5px"></td>
                                    <td>
                                        <asp:Panel ID="panlblStep2bPrintConsent" runat="server">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:ImageButton ID="btnStep2bPrintClaimConsentForm" runat="server" AlternateText="<%$ Resources:AlternateText, VRAPrintClaimConsentFormBtn%>"
                                                            ImageUrl="<%$ Resources:ImageUrl, VRAPrintClaimConsentFormBtn%>" />
                                                        <asp:HiddenField ID="hfCurrentPrintOption" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rbStep2bPrintClaimConsentFormLanguage" runat="server" RepeatDirection="Horizontal">
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="panStep2bPerprintFormNotice" runat="server">
                                            <table cellspacing="0" cellpadding="0" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <img id="imgStep2bPerprintFormNotice" src="../Images/others/ExclamationMark%20.gif" />&nbsp;</td>
                                                        <td>
                                                            <asp:Label ID="lblStep2bPerprintFormNotice" runat="server" Text="<%$ Resources:Text, ReminPreprintOption %>"
                                                                ForeColor="Black"></asp:Label></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td valign="top" rowspan="2" style="padding-bottom: 5px; padding-left: 5px;">
                                        <asp:ImageButton ID="btnStep2bChangePrintOption" runat="server" AlternateText="<%$ Resources:AlternateText, ChangePrintOption%>"
                                            ImageUrl="~/Images/button/icon_button/btn_change_print_option.png" ImageAlign="AbsMiddle"></asp:ImageButton></td>
                                    <td rowspan="2" align="top">
                                        <asp:Panel ID="panStep2bAdhocPrint" runat="server">
                                            <asp:ImageButton ID="btnStep2bPrintAdhocClaimConsentForm" runat="server" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px;padding-right: 5px"></td>
                                    <td></td>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>
                    <table cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr id="trStep2bDeclareClaim" runat="server">
                                <td style="width: 200px; padding-right: 5px; height: 16px"></td>
                                <td style="width: 550px; height: 16px" class="checkboxStyle">
                                    <asp:CheckBox ID="chkStep2bDeclareClaim" TabIndex="1" runat="server" Text="<%$ Resources:Text, ProvidedInfoTrueClaimSP%>"
                                        AutoPostBack="True">
                                    </asp:CheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-bottom: 5px; width: 200px; padding-right: 5px; padding-top: 5px; height: 16px"></td>
                                <td style="padding-bottom: 5px; padding-top: 10px">
                                    <asp:ImageButton ID="btnStep2bBack" TabIndex="2" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn%>"></asp:ImageButton>&nbsp;
                                    <asp:ImageButton
                                            ID="btnStep2bConfirm" TabIndex="4" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn%>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmDisableBtn%>" Enabled="False"></asp:ImageButton>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:View>
                <asp:View ID="vStep3" runat="server">
                    <table cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td class="eHSTableHeading">
                                    <asp:Label ID="lblStep3AcctInfoText" runat="server" Text="<%$ Resources:Text, AccountInfo %>"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="padding-top: 4px">
                                    <uc2:ucReadOnlyDocumnetType ID="udcStep3ReadOnlyDocumnetType" runat="server"></uc2:ucReadOnlyDocumnetType>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td class="eHSTableHeading" valign="top" colspan="2">
                                    <asp:Label ID="lblStep3ClaimInfo" runat="server" Text="<%$ Resources:Text, ClaimInfo %>"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="tableCellStyle" style="width: 205px; padding: 10px 0px 3px 0px;" valign="top">
                                    <asp:Label ID="lblStep3TransNumText" runat="server" Text="<%$ Resources:Text, TransactionNo %>"
                                        CssClass="tableTitle" Width="160px"></asp:Label></td>
                                <td class="tableCellStyle" valign="middle">
                                    <table cellspacing="0" cellpadding="0">
                                        <tbody>
                                            <tr>
                                                <td class="" valign="middle" style="padding: 10px 0px 3px 0px;" height="auto">
                                                    <asp:Label ID="lblStep3TransNum" runat="server" CssClass="tableText" ForeColor="Blue" Height="16px"></asp:Label>
                                                </td>
                                                <td class="" valign="middle" style="padding: 10px 0px 3px 0px;">
                                                    <asp:Label ID="lblStep3PrefixTransNum" runat="server" CssClass="tableText" ForeColor="Black" Font-Strikeout="true"></asp:Label>
                                                </td>
                                                <td class="" width="10px" valign="middle"></td>
                                                <td class="" valign="middle" style="padding: 6px 0px 3px 0px;">
                                                    <asp:ImageButton ID="ibtnStep3ViewLatestTransactionID" AlternateText="<%$ Resources:AlternateText, ViewLatestTransactionIDBtn%>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ViewLatestTransactionIDBtn%>" runat="server" Visible="true" />
                                                </td>
                                                <td class="" valign="middle" style="padding: 5px 0px 0px 0px;">
                                                    <asp:Label ID="lblHTMLRightPointArrow" runat="server" Text="<%$ Resources:Text, HTMLRightPointArrow %>" ForeColor="Blue"
                                                        CssClass="tableText" Visible="false" Font-Size="16"></asp:Label>
                                                </td>
                                                <td width="6px" valign="middle"></td>
                                                <td class="" valign="middle" style="padding: 10px 0px 3px 0px;">
                                                    <asp:Label ID="lblLatestTransactionID" CssClass="tableText" runat="server" Text="Label" ForeColor="Blue" Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>

                                    <asp:Label ID="lblStep3TransactionIDUpdateNoticeBeforeViewLatest" runat="server" color="red" CssClass="tableText" ForeColor="Red" Text="<%$ Resources:Text, TransactionIDUpdateNoticeBeforeViewLatest %>" Visible="false"></asp:Label>
                                    <asp:Label ID="lblStep3TransactionIDUpdateNoticeAfterViewLatest" runat="server" color="red" CssClass="tableText" ForeColor="Red" Text="<%$ Resources:Text, TransactionIDUpdateNoticeAfterViewLatest %>" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableCellStyle" style="width: 205px" valign="top">
                                    <asp:Label ID="lblStep3TransDateText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionDate %>" Width="160px"></asp:Label>
                                </td>
                                <td class="tableCellStyle" valign="top">
                                    <asp:Label ID="lblStep3TransDate" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableCellStyle" style="width: 205px;" valign="top">
                                    <asp:Label ID="lblStep3SchemeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>" Width="160px"></asp:Label>
                                </td>
                                <td class="tableCellStyle" valign="top">
                                    <asp:Label ID="lblStep3Scheme" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <asp:Panel ID="panNonClinicSettingStep3" runat="server" Visible="false">
                                <tr>
                                    <td class="tableCellStyle" style="width: 205px;" valign="top"></td>
                                    <td class="tableCellStyle" valign="top">
                                        <asp:Label ID="lblNonClinicSettingStep3" runat="server" CssClass="tableText" Style="position: relative; top: -1px"></asp:Label>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <tr id="trStep3TransactionStatus" runat="server">
                                <td class="tableCellStyle" style="vertical-align: top">
                                    <asp:Label ID="lblTransactionStatusText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label>
                                </td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTransactionStatus" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableCellStyle" style="width: 205px;" valign="top">
                                    <asp:Label ID="lblStep3ServiceDateText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceDate %>" Width="160px"></asp:Label>
                                </td>
                                <td class="tableCellStyle" valign="top">
                                    <asp:Label ID="lblStep3ServiceDate" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableCellStyle" style="width: 205px;" valign="top">
                                    <asp:Label ID="lblStep3PracticeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Practice %>" Width="160px"></asp:Label>
                                </td>
                                <td class="tableCellStyle" valign="top">
                                    <asp:Label ID="lblStep3Practice" runat="server" CssClass="tableTextChi"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trStep3BankAcct" runat="server">
                                <td class="tableCellStyle" valign="top">
                                    <asp:Label ID="lblStep3BankAcctText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, BankAccountNo %>" Width="160px"></asp:Label>
                                </td>
                                <td align="left" class="tableCellStyle" valign="top">
                                    <asp:Label ID="lblStep3BankAcct" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trStep3ServiceType" runat="server">
                                <td class="tableCellStyle" valign="top">
                                    <asp:Label ID="lblStep3ServiceTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceType %>" Width="160px"></asp:Label>
                                </td>
                                <td align="left" class="tableCellStyle" valign="top">
                                    <asp:Label ID="lblStep3ServiceType" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <uc4:ucReadOnlyEHSClaim ID="udcStep3ReadOnlyEHSClaim" runat="server" />

                    <asp:Panel ID="panStep3RecipinetContactInfo" runat="server">
                        <table cellspacing="0" cellpadding="0" border="0">
                            <%--<tr>
                                <td class="eHSTableHeading" colspan="2">
                                    <asp:Label ID="lblStep3aRecipinetContactInfoHeading" runat="server" Text="<%$ Resources:Text, RecipientContactInformation%>" />
                                </td>
                            </tr>--%>
                            <tr id="trStep3ContactNo" runat="server" visible="false">
                                <td height="25" style="width: 205px;vertical-align:top">
                                    <asp:Label ID="lblStep3ContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo2%>" CssClass="tableTitle" Width="160px" />
                                </td>
                                <td height="25" style="vertical-align:top">
                                    <asp:Label ID="lblStep3ContactNo" runat="server" CssClass="tableText"/>
                                </td>
                            </tr>
                            <%--<tr>
                                <td style="padding-bottom:10px">
                                </td>
                            </tr>--%>
                            <asp:Panel ID="panStep3Remark" runat="server">
                            <tr>
                                <td height="25" style="width: 205px;vertical-align:top">
                                    <asp:Label ID="lblStep3RemarkText" runat="server" Text="<%$ Resources:Text, Remarks%>" CssClass="tableTitle" Width="160px" />
                                </td>
                                <td height="25" style="vertical-align:top">
                                    <asp:Label ID="lblStep3Remark" runat="server" CssClass="tableText"/>
                                </td>
                            </tr>
                            </asp:Panel>
                            <asp:Panel ID="panStep3JoinEHRSS" runat="server">
                            <tr>
                                <td height="25" style="width: 205px;vertical-align:top;">
                                    <asp:Label ID="lblStep3JoinEHRSSText" runat="server" Text="<%$ Resources:Text, JoinEHRSS%>" CssClass="tableTitle" Width="160px" />
                                </td>
                                <td height="25" style="vertical-align:top;">
                                    <asp:Label ID="lblStep3JoinEHRSS" runat="server" CssClass="tableText"/>
                                </td>
                            </tr>
                            </asp:Panel>
                            <asp:Panel ID="panStep3NonLocalRecoveredHistory" runat="server">
                            <tr>
                                <td height="25" style="width: 205px;vertical-align:top;">
                                    <asp:Label ID="lblStep3NonLocalRecoveredHistoryText" runat="server" Text="<%$ Resources:Text, NonLocalRecoveredHistory%>" CssClass="tableTitle" />
                                </td>
                                <td height="25" style="vertical-align:top;">
                                    <asp:Label ID="lblStep3NonLocalRecoveredHistory" runat="server" CssClass="tableText" style="position:relative;left:1px"/>
                                </td>
                            </tr>
                            </asp:Panel>
                        </table>
                    </asp:Panel>

                    <table cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <tr>
                                <td style="height: 4px" />
                            </tr>
                            <tr>
                                <td style="width: 205px; padding-top: 5px" />
                                <td style="padding-top: 5px">
                                    <asp:ImageButton ID="btnStep3Reprint" runat="server" Visible = "false" AlternateText="<%$ Resources:AlternateText, ReprintVaccinationRecordBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, ReprintVaccinationRecordBtn%>"></asp:ImageButton>
                                    <asp:ImageButton ID="btnStep3NextClaim" runat="server" AlternateText="<%$ Resources:AlternateText, NextClaimBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, NextClaimBtn%>"></asp:ImageButton>
                                    <asp:ImageButton ID="btnStep3ClaimForSamePatient" runat="server" AlternateText="<%$ Resources:AlternateText, ClaimForSamePatientBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, ClaimForSamePatientBtn%>"></asp:ImageButton>
                                     <!--AlternateText=""-->
                                     <asp:ImageButton ID="btnStep3ClaimClose" runat="server" AlternateText="<%$ Resources:AlternateText, ClaimForCloseBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, ClaimForCloseBtn%>"></asp:ImageButton>
                                          

                                </td>
                            </tr>
                        </tbody>
                    </table>

                </asp:View>
                <asp:View ID="vSelectPractice" runat="server">
                    <cc3:PracticeRadioButtonGroup runat="server" ID="PracticeRadioButtonGroup" HeaderText="<%$ Resources:Text, SelectPractice%>"
                        HeaderTextCss="tableText" PracticeRadioButtonCss="tableText" PracticeTextCss="tableTextChi"
                        SchemeLabelCss="tableTitle" SelectButtonURL="~/Images/button/icon_button/btn_Arrow_to_Right.png"
                        MaskBankAccountNo="True" ShowCloseButton="False"/>
                </asp:View>
                <asp:View ID="vEHSClaimError" runat="server">
                    <asp:ImageButton ID="btnInternalErrorBack" TabIndex="2" runat="server"></asp:ImageButton>
                </asp:View>
            </asp:MultiView>
            <%-- Popup for Vaccination Record Provider --%>
            <asp:Panel Style="display: none" ID="panVaccinationRecordProvider" runat="server">
                <asp:Panel ID="panVaccinationRecordProviderHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 900px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblVaccinationRecordProviderHeading" runat="server" Text="<%$ Resources:Text, VaccinationRecordProvider %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 900px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panVaccinationRecordProviderContent" runat="server" ScrollBars="Auto"
                                Height="420px">
                                <uc10:ucVaccinationRecordProvider ID="ucVaccinationRecordProvider" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="ibtnVaccinationRecordProviderClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>"
                                AlternateText="<%$ Resources:AlternateText, CloseBtn %>" OnClick="ibtnVaccinationRecordProviderClose_Click" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderVaccinationRecordProvider" runat="server"
                BackgroundCssClass="modalBackgroundTransparent" TargetControlID="btnModalPopupVaccinationRecordProvider"
                PopupControlID="panVaccinationRecordProvider" PopupDragHandleControlID="panVaccinationRecordProviderHeading"
                RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupVaccinationRecordProvider" runat="server"></asp:Button>
            <%-- End of Popup for Vaccination Record Provider --%>
        </ContentTemplate>
    </asp:UpdatePanel>
   
    <script type="text/javascript" language="javascript">
        $(function () {
            $(document).on('change', "[id$='ddlCCategoryCovid19']", function () {
                disableChkStep2aDeclareClaim()
            });

            $(document).on('change', "[id$='ddlCVaccineBrandCovid19']", function () {
                disableChkStep2aDeclareClaim()
            });

            $(document).on('change', "[id$='ddlCVaccineLotNoCovid19']", function () {
                disableChkStep2aDeclareClaim()
            });

            $(document).on('change', "[id$='ddlCDoseCovid19']", function () {
                disableChkStep2aDeclareClaim()

            });

        });

        const disableChkStep2aDeclareClaim = function () {
            //if ($("[id$='btnStep2aPrintClaimConsentForm']").length > 0) $("[id$='chkStep2aDeclareClaim']").attr('disabled', true);
            $("[id$='chkStep2aDeclareClaim']").prop("checked", false).change();
        }

    </script> 
</asp:Content>
