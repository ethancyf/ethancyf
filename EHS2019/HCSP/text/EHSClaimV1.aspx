<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/text/ClaimVoucher.Master"
    Codebehind="EHSClaimV1.aspx.vb" Inherits="HCSP.Text.EHSClaimV1" Title="<%$ Resources:Title, Claim %>" %>

<%@ Register Src="../UIControl/VaccinationRecordText/ucVaccinationRecord.ascx" TagName="ucVaccinationRecord"
    TagPrefix="uc8" %>
<%@ Register Src="../UIControl/EHSClaimText/ucReasonForVisit.ascx" TagName="ucReasonForVisit"
    TagPrefix="uc7" %>
<%@ Register Src="../UIControl/ucInputTips.ascx" TagName="ucInputTips" TagPrefix="uc6" %>
<%@ Register Src="../UIControl/ucReadOnlyEHSClaim.ascx" TagName="ucReadOnlyEHSClaim"
    TagPrefix="uc5" %>
<%@ Register Src="../UIControl/ucInputEHSClaimText.ascx" TagName="ucInputEHSClaim"
    TagPrefix="uc4" %>
<%@ Register Src="../UIControl/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc3" %>
<%@ Register Src="../UIControl/DocTypeText/ucClaimSearch.ascx" TagName="ucClaimSearch"
    TagPrefix="uc2" %>
<%@ Register Src="../PrintFormOptionSelection.ascx" TagName="PrintFormOptionSelection"
    TagPrefix="uc1" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <cc1:TextOnlyMessageBox ID="udcMsgBoxErr" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <cc1:TextOnlyInfoMessageBox ID="udcMsgBoxInfo" runat="server" />
            </td>
        </tr>
    </table>
    <asp:MultiView ID="mvEHSClaim" runat="server">
        <asp:View ID="vStep1" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblStep1SearchAccountText" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, InputSearchInformation %>"></asp:Label></td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblStep1PracticeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Practice %>"></asp:Label>
                        <asp:Button ID="btnStep1ChangePractice" runat="server" SkinID="TextOnlyVersionLinkButton" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStep1Practice" runat="server" CssClass="tableTextChi"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStep1SchemeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                        <asp:Button ID="btnStep1ChangeScheme" runat="server" SkinID="TextOnlyVersionLinkButton" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStep1Scheme" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <asp:panel ID="panNonClinicSetting" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="lblNonClinicSetting" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
                </asp:panel>
                <tr>
                    <td>
                        <asp:Label ID="lblStep1DocTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DocumentType %>"></asp:Label>
                        <asp:Button ID="btnStep1ChangeDocType" runat="server" SkinID="TextOnlyVersionLinkButton" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStep1DocType" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="panStep1Submit" DefaultButton="btnStep1GO">
                <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                    <tr>
                        <td>
                            <uc2:ucClaimSearch ID="udcStep1ClaimSearch" runat="server"></uc2:ucClaimSearch>
                        </td>
                    </tr>
                    <tr style="padding: 0px 0px 5px 0px">
                        <td>
                            <asp:Button ID="btnStep1GO" runat="server" Text="<%$ Resources:AlternateText, SearchBtn %>" /><br />
                            <asp:Button ID="btnStep1ReadOldSmartID" runat="server" Text="<%$ Resources:AlternateText, ReadOldCardAndSearchBtn %>" /><br />
                            <asp:Button ID="btnStep1ReadNewSmartID" runat="server" Text="<%$ Resources:AlternateText, ReadNewCardAndSearchBtn %>" />
                            <asp:Label ID="lblReadCardAndSearchNA" runat="server" Text="<%$ Resources:Text, ReadCardAndSearchTextOnlyNA %>"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
        </asp:View>
        <asp:View ID="vStep2a" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblStep2aAcctInfo" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, AccountInfo %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" id="tblStep2aReminderContainer" runat="server"
                            class="tableRemark">
                            <tr>
                                <td>
                                    <asp:Label ID="lblStep2aReminder" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc3:ucReadOnlyDocumnetType ID="udcStep2aReadOnlyDocumnetType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnStep2aViewVaccinationRecord" runat="server" Text="<%$ Resources:AlternateText, ViewVaccinationRecordBtn %>" />
                    </td>
                </tr>
                <tr>
                    <td class="SeparationLine">
                        <hr id="hrStep2aAcctInfo" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="panStep2aInput" runat="server" DefaultButton="btnStep2aClaim">
                <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                    <tr>
                        <td>
                            <asp:Label ID="lbllblStep2aClaimInfo" runat="server" Text="<%$ Resources:Text, InputClaimInformation %>"
                                CssClass="tableHeader"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStep2aPracticeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Practice %>"></asp:Label>
                            <asp:Button ID="btnStep2aChangePractice" runat="server" Text="<%$ Resources: AlternateText, Change %>"
                                SkinID="TextOnlyVersionLinkButton" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStep2aPractice" runat="server" CssClass="tableTextChi" Width="100%"></asp:Label></td>
                    </tr>
                </table>
                <asp:Panel ID="panStep2aClaimDetail" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                        <tr>
                            <td>
                                <asp:Label ID="lblStep2aSchemeText" runat="server" CssClass="tableTitle"></asp:Label>
                                <asp:Button ID="btnStep2aChangeScheme" runat="server" Text="<%$ Resources: AlternateText, Change %>"
                                    SkinID="TextOnlyVersionLinkButton" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblStep2aSchemeSelectedText" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblStep2aServiceDateText" runat="server" CssClass="tableTitle"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtStep2aServiceDate" runat="server" ForeColor="DimGray" MaxLength="10"
                                    AutoPostBack="True" Width="65px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                    onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                    onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox><asp:Label
                                        ID="lblStep2aServiceDateError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                                <asp:Label ID="lblStep2aServiceDate" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <uc4:ucInputEHSClaim ID="udcStep2aInputEHSClaim" runat="server" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table cellpadding="0" cellspacing="0" class="textVersionTable">
                    <tr>
                        <td style="padding-top: 4px">
                            <asp:Button ID="btnStep2aCancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
                            <asp:Button ID="btnStep2aClaim" runat="server" Text="<%$ Resources:AlternateText, ClaimBtn%>" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vStep2b" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblStep2bAcctInfo" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, AccountInfo %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <uc3:ucReadOnlyDocumnetType ID="udcStep2bReadOnlyDocumnetType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <hr id="hrStep2bAcctInfo" width="100%" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lbllblStep2bClaimInfo" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, ConfirmClaimInformation %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStep2bSchemeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStep2bScheme" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbllblStep2bServiceDateText" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, ServiceDate %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStep2bServiceDate" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbllblStep2bPracticeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStep2bPractice" runat="server" CssClass="tableTextChi"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStep2bBankAcctText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStep2bBankAcct" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStep2bServiceTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceType %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStep2bServiceType" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <uc5:ucReadOnlyEHSClaim ID="udcStep2bReadOnlyEHSClaim" runat="server" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Panel ID="panStep2bPrintClaimConsentForm" runat="server">
                            <asp:Panel ID="panlblStep2bPrintConsent" runat="server">
                                <table id="tabPrintConsentForm" runat="server" border="0" cellpadding="0" cellspacing="0"
                                    class="textVersionTable">
                                    <tr>
                                        <td valign="top">
                                            <asp:Button ID="btnStep2bPrintClaimConsentForm" runat="server" TabIndex="3" Text="<%$ Resources:AlternateText, VRAPrintClaimConsentFormBtn%>"
                                                Width="260px" /><asp:Label runat="server" ID="lblStep2bPrintFormError" CssClass="validateFailText"
                                                    Text="*" Visible="false" />
                                            <asp:HiddenField ID="hfCurrentPrintOption" runat="server"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButtonList ID="rbStep2bPrintClaimConsentFormLanguage" runat="server" RepeatDirection="Horizontal">
                                                        </asp:RadioButtonList></td>
                                                    <td>
                                                        <asp:Label ID="ClaimPrintOptionError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panStep2bPerprintFormNotice" runat="server">
                                <table id="tblConfirmDtlPrePrintForm" runat="server" cellpadding="0" cellspacing="0"
                                    class="textVersionTable">
                                    <tr>
                                        <td>
                                            <table cellpadding="2" cellspacing="0" class="tableRemark">
                                                <tr>
                                                    <td>
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblConfirmDtlPerprintFormNoticeExclamation" runat="server" Font-Bold="True"
                                                                        Font-Size="X-Large" ForeColor="Blue" Text="!" Width="15px"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblConfirmDtlPerprintFormNotice" runat="server" Text="<%$ Resources:Text, ReminPreprintOption %>"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <div style="padding-top: 4px; padding-bottom: 4px;" class="textVersionTable">
                                <asp:Button ID="btnStep2bChangePrintOption" runat="server" Text="<%$ Resources:AlternateText, ChangePrintOption%>" /></div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnStep2bBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                        <asp:Button ID="btnStep2bConfirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn%>" />
                        <asp:Button ID="btnStep2bAdhocPrintConsentForm" runat="server" Text="<%$ Resources:AlternateText, VRAPrintClaimConsentFormBtn %>" /></td>
                </tr>
            </table>
            <div style="padding-top: 4px">
                &nbsp;&nbsp;
            </div>
        </asp:View>
        <asp:View ID="vStep3" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblStep3AcctInfo" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, AccountInfo %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <uc3:ucReadOnlyDocumnetType ID="udcStep3ReadOnlyDocumnetType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" valign="top">
                        <hr id="hrStep3AcctInfo" width="100%" />
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tbody>
                    <tr>
                        <td class="eHSTableHeading">
                            <asp:Label ID="lblStep3ClaimInfo" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, ClaimInfo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3TransNumText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionNo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3TransNum" runat="server" CssClass="tableText" ForeColor="Blue"></asp:Label>
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblStep3PrefixTransNum" runat="server" CssClass="tableText" ForeColor="Black" Font-Strikeout="true"></asp:Label>
                                    </td>
                                    <td width="10px">
                                    </td>
                                    <td>
                                        <asp:Button ID="btnStep3ViewLatestTransactionID" runat="server" CssClass="tableText" Text="<%$ Resources:AlternateText, ViewLatestTransactionIDBtn%>" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblHTMLRightPointArrow" runat="server" Text="<%$ Resources:Text, HTMLRightPointArrow %>" ForeColor="Blue"
                                                    CssClass="tableTitle" Visible="false" Font-Size="18px"></asp:Label>
                                    </td>
                                    <td width="10px">
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLatestTransactionID" runat="server" CssClass="tableText" Text="Label" Visible="false" ForeColor="Blue"></asp:Label>
                                    </td>
                                </tr>
                                </tbody>
                            </table>
                            <asp:Label ID="lblStep3TransactionIDUpdateNoticeBeforeViewLatest" runat="server" color="red" CssClass="tableText" ForeColor="Red" Text="<%$ Resources:Text, TransactionIDUpdateNoticeBeforeViewLatest %>" Visible="false"></asp:Label>
                            <asp:Label ID="lblStep3TransactionIDUpdateNoticeAfterViewLatest" runat="server" color="red" CssClass="tableText" ForeColor="Red" Text="<%$ Resources:Text, TransactionIDUpdateNoticeAfterViewLatest %>" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3TransDateText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionDate %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3TransDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3SchemeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3Scheme" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3TransStatusText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3TransStatus" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3ServiceDateText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceDate %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3ServiceDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3PracticeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3Practice" runat="server" CssClass="tableTextChi"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3BankAcctText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3BankAcct" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3ServiceTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceType %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblStep3ServiceType" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
            <uc5:ucReadOnlyEHSClaim ID="udcStep3ReadOnlyEHSClaim" runat="server" />
            <asp:Button ID="btnStep3NextClaim" runat="server" Text="<%$ Resources:AlternateText, NextClaimBtn%>"
                Width="80px" />
            <asp:Button ID="btnStep3ClaimForSamePatient" runat="server" Text="<%$ Resources:AlternateText, ClaimForSamePatientBtn%>"
                Width="160px" />
        </asp:View>
        <asp:View ID="vSelectPractice" runat="server">
            <asp:Panel runat="server" ID="panStepSelectPractice" DefaultButton="btnStepSelectPracticeGO">
                <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                    <tr>
                        <td>
                            <asp:Label ID="lblStepSelectPracticePracticeText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <cc2:PracticeRadioButtonGroupText runat="server" ID="ucSelectPracticeRadioButtonGroupText"
                                CssClass="tableTextChi" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnStepSelectPracticeGO" runat="server" Text="" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vSelectScheme" runat="server">
            <asp:Panel runat="server" ID="panStepSelectScheme" DefaultButton="btnStepSelectSchemeGO">
                <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                    <tr>
                        <td>
                            <asp:Label ID="lblStepSelectSchemePracticeText" runat="server" CssClass="tableTitle"></asp:Label>
                            <asp:Button ID="btnStepSelectSchemeChangePractice" runat="server" SkinID="TextOnlyVersionLinkButton" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStepSelectSchemePractice" runat="server" CssClass="tableTextChi"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStepSelectSchemeScheme" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <cc2:SchemeRadioButtonGroupText runat="server" ID="ucSchemeRadioButtonGroupText" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnStepSelectSchemeCancel" runat="server" />
                            <asp:Button ID="btnStepSelectSchemeGO" runat="server" Text="" /></td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vSelectDocType" runat="server">
            <asp:Panel runat="server" ID="panStepSelectDocType" DefaultButton="btnStepSelectDocTypeSelect">
                <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                    <tr>
                        <td>
                            <asp:Label ID="lblStepSelectDocTypePracticeText" runat="server" CssClass="tableTitle"></asp:Label>
                            <asp:Button ID="btnStepSelectDocTypeChangePractice" runat="server" SkinID="TextOnlyVersionLinkButton" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStepSelectDocTypePractice" runat="server" CssClass="tableTextChi"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStepSelectDocTypeSchemeText" runat="server" CssClass="tableTitle"></asp:Label>
                            <asp:Button ID="btnStepSelectDocTypeChangeScheme" runat="server" SkinID="TextOnlyVersionLinkButton" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStepSelectDocTypeScheme" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStepSelectDocTypeDocTypeText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:DocumentTypeRadioButtonGroupText runat="server" ID="ucDocumentTypeRadioButtonGroupText" PracticeTextCss="tableText" AutoPostBack="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnStepSelectDocTypeSelect" runat="server" Text="" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vInternalError" runat="server">
            <asp:Button ID="btnViewInternalErrorReturn" runat="server" Text="<%$ Resources:AlternateText, ReturnBtn%>" />
        </asp:View>
        <asp:View ID="vConfirmBox" runat="server">
            <asp:Label ID="lblConfirmOnlyBoxTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label><br />
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" id="tblConfirmBoxContainer" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lblConfirmBoxMessage" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 4px">
                        <asp:Button ID="btnConfirmBoxBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                        <asp:Button ID="btnConfirmBoxConfirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vPrintOption" runat="server">
            <table cellspacing="0" cellpadding="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblPrintOptionTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:AlternateText, ChangePrintOption%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc1:PrintFormOptionSelection ID="udtPrintOptionSelection" runat="server" PrintOptionTableWidth="100%"></uc1:PrintFormOptionSelection>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnPrintOptionSelectionBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                        <asp:Button ID="btnPrintOptionSelectionSelect" runat="server" Text="<%$ Resources:AlternateText, SelectBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vAddHocPrint" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblAdhocPrintTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, PrintConsentForm %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <table id="tbAdhocPrintFull" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lblAdhocPrintFull" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, FullVersionPrintOut %>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbAdhocPrintFullLang1" GroupName="AdhocPrintSelection" runat="server" CssClass="tableText"/>
                                    <asp:RadioButton ID="rbAdhocPrintFullLang2" GroupName="AdhocPrintSelection" runat="server" CssClass="tableText"/>
                                    <asp:RadioButton ID="rbAdhocPrintFullLang3" GroupName="AdhocPrintSelection" runat="server" CssClass="tableText"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="tbAdhocPrintCondense" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lblAdhocPrintCondense" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, CondensedVersionPrintOut %>"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rbAdhocPrintCondenseLang1" GroupName="AdhocPrintSelection" runat="server" CssClass="tableText"/>
                                    <asp:RadioButton ID="rbAdhocPrintCondenseLang2" GroupName="AdhocPrintSelection" runat="server" CssClass="tableText"/>
                                    <asp:RadioButton ID="rbAdhocPrintCondenseLang3" GroupName="AdhocPrintSelection" runat="server" CssClass="tableText"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnAddHocPrintSelectionBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn %>" />
                        <asp:Button ID="btnAddHocPrintSelection" runat="server" Text="<%$ Resources:AlternateText, PrintBtn %>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vRemark" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblRemarksHeader" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, Remarks %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:PlaceHolder ID="plRemarkContainer" runat="server"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnViewRemarkReturn" runat="server" Text="<%$ Resources:AlternateText, ReturnBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vInputTip" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblInputTipTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, InputTips %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc6:ucInputTips runat="server" ID="ucInputTipsControl"></uc6:ucInputTips>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnInputTipReturn" runat="server" Text="<%$ Resources:AlternateText, BackBtn %>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vReasonForVisit" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <uc7:ucReasonForVisit runat="server" ID="udcReasonForVisit"></uc7:ucReasonForVisit>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vCategory" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblSelectCategoryHeader" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Category%>"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:RadioButtonList ID="rbCategory" runat="server" Width="100%">
                        </asp:RadioButtonList></td>
                </tr>
            </table>
            <asp:Button ID="btnSelectCategoryConfirm" runat="server" Text="<%$ Resources:AlternateText, SelectBtn %>" />
        </asp:View>
        <asp:View ID="vVaccinationRecord" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblVaccinationRecordAccInfoText" runat="server" CssClass="tableHeader"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <uc3:ucReadOnlyDocumnetType ID="udcVaccinationRecordReadOnlyDocumnetType" runat="server" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td valign="top">
                        <hr class="textVersionTable" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <uc8:ucVaccinationRecord ID="udcVaccinationRecord" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnVRContinue" runat="server" Text="<%$ Resources: AlternateText, ContinueBtn %>" />
            <asp:Button ID="btnVRReturn" runat="server" Text="<%$ Resources: AlternateText, ReturnBtn %>" />
        </asp:View>
    </asp:MultiView>
    <asp:HiddenField ID="hfEHSClaimTokenNum" runat="server" EnableViewState="true" />
</asp:Content>
