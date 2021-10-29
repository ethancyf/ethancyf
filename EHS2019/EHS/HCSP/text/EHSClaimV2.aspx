<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/text/ClaimVoucher.Master"
    Codebehind="EHSClaimV2.aspx.vb" Inherits="HCSP.EHSClaimV2" Title="<%$ Resources:Title, Claim %>" %>

<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc2" %>
<%@ Register Src="../UIControl/VaccinationRecordText/ucVaccinationRecord.ascx" TagName="ucVaccinationRecord"
    TagPrefix="uc7" %>
<%@ Register Src="../UIControl/ucInputTips.ascx" TagName="ucInputTips" TagPrefix="uc6" %>
<%@ Register Src="../UIControl/ucReadOnlyEHSClaim.ascx" TagName="ucReadOnlyEHSClaim"
    TagPrefix="uc5" %>
<%@ Register Src="../UIControl/ucInputEHSClaim.ascx" TagName="ucInputEHSClaim" TagPrefix="uc4" %>
<%@ Register Src="../UIControl/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc3" %>
<%@ Register Src="../UIControl/DocTypeText/ucClaimSearch.ascx" TagName="ucClaimSearch"
    TagPrefix="uc2" %>
<%@ Register Src="../PrintFormOptionSelection.ascx" TagName="PrintFormOptionSelection"
    TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <cc1:TextOnlyMessageBox ID="udcMsgBoxErr" runat="server"/>
            </td>
        </tr>
        <tr>
            <td>
                <cc1:TextOnlyInfoMessageBox ID="udcMsgBoxInfo" runat="server"/>
            </td>
        </tr>
    </table>
    <asp:MultiView ID="mvEHSClaim" runat="server">
        <asp:View ID="vViewTranDetail" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailAcctInfo" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, AccountInfo %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <uc3:ucReadOnlyDocumnetType ID="udcViewTranDetailOnlyDocumnetType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr id="hrViewTranDetailAcctInfo" class="textVersionTable" />
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td class="eHSTableHeading">
                        <asp:Label ID="lblViewTranDetailClaimInfo" runat="server" CssClass="tableHeader"
                            Text="<%$ Resources:Text, ClaimInfo %>"></asp:Label></td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" class="textVersionTable" runat="server" id="tblViewTranDetailScheme">
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblViewTranDetailSchemeText" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblViewTranDetailScheme" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
                <tr id="trNonClinicSetting" runat="server" style="display:none">
                    <td valign="top">
                        <asp:Label ID="lblViewNonClinicSetting" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" class="textVersionTable" runat="server" id="tblViewTranDetailPractice">
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblViewTranDetailPracticeText" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblViewTranDetailPractice" runat="server" CssClass="tableTextChi"></asp:Label></td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" class="textVersionTable" runat="server" id="tblViewTranDetailServiceDate">
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblViewTranDetailServiceDateText" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, ServiceDate %>"></asp:Label></td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblViewTranDetailServiceDate" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable" runat="server"
                id="tblViewTranDetailCategory">
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailCategoryText" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, Category %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailCategory" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable" runat="server"
                id="tblViewTranDetailPreConditions">
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailPreConditionsText" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, PreConditions %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailPreConditions" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
            </table>

            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable" runat="server"
                id="tblViewTranDetailDocumentaryProof">
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailDocumentaryProofText" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, TypeOfDocumentaryProof %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailDocumentaryProof" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
            </table>
           
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable" runat="server"
                id="tblViewTranDetailRCHCode">
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailRCHCodeText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailRCHCode" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailRCHNameText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailRCHName" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable" runat="server"
                id="tblViewTranDetailPlaceVaccination">
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailPlaceVaccinationText" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, PlaceOfVaccination %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblViewTranDetailPlaceVaccination" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
            </table>
            <cc2:ClaimVaccineReadOnlyText ID="udcViewTranDetailVaccineReadOnly" runat="server"
                CssTableText="tableText" CssTableTitle="tableTitle" />
            <asp:Button ID="btnViewTranDetailReturn" runat="server" Text="<%$ Resources:AlternateText, ReturnBtn%>" />

        </asp:View>
        <asp:View ID="vConfirmDetail" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmDetailAcctInfo" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, AccountInfo %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <uc3:ucReadOnlyDocumnetType ID="udcConfirmDetailReadOnlyDocumnetType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr id="hrConfirmDetailAcctInfo" class="textVersionTable" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmDetailClaimInfo" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, ConfirmClaimInformation %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmDetailSchemeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmDetailScheme" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr id="trConfirmNonClinicSetting" runat="server" style="display:none">
                    <td valign="top">
                        <asp:Label ID="lblConfirmNonClinicSetting" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmDetailServiceDateText" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, ServiceDate %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmDetailServiceDate" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmDetailPracticeText" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmDetailPractice" runat="server" CssClass="tableTextChi"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmDetailBankAcctText" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmDetailBankAcct" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmDetailServiceTypeText" runat="server" CssClass="tableTitle"
                            Text="<%$ Resources:Text, ServiceType %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblConfirmDetailServiceType" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <uc5:ucReadOnlyEHSClaim ID="udcConfirmDetailReadOnlyEHSClaim" runat="server" />
                    </td>
                </tr>
                <asp:panel ID="panConfirmDetailRecipientCondition" runat="server" Visible="false">
                <tr>
                    <td style="vertical-align: top;width:205px;padding:0px">
                        <asp:Label ID="lblConfirmDetailRecipientConditionText" runat="server" CssClass="tableTitle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;padding:0px">
                        <asp:Label ID="lblConfirmDetailRecipientCondition" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
                </asp:panel>
                <tr>
                    <td style="vertical-align: top;padding:0px">
                        <asp:Label ID="lblConfirmDetailContactNoText" runat="server" CssClass="tableTitle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;padding:0px">
                        <asp:Label ID="lblConfirmDetailContactNo" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;padding:0px">                        
                        <asp:Label ID="lblContactNoNotAbleSMS" runat="server" CssClass="tableText" Text="<%$ Resources:Text, NotAbleToReceiveSMS%>" style="color:red!important" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;padding:0px">
                        <asp:Label ID="lblConfirmDetailRemarksText" runat="server" CssClass="tableTitle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;padding:0px">
                        <asp:Label ID="lblConfirmDetailRemarks" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Panel ID="panConfirmDetailPrintClaimConsentForm" runat="server">
                            <asp:Panel ID="panlblConfirmDetailPrintConsent" runat="server">
                                <table id="tabPrintConsentForm" runat="server" border="0" cellpadding="0" cellspacing="0"
                                    class="textVersionTable">
                                    <tr>
                                        <td valign="top">
                                            <asp:Button ID="btnConfirmDetailPrintClaimConsentForm" runat="server" TabIndex="3"
                                                Text="<%$ Resources:AlternateText, VRAPrintClaimConsentFormBtn%>" Width="260px" /><asp:Label
                                                    ID="lblConfirmDetailPrintFormError" runat="server" CssClass="validateFailText"
                                                    Text="*" Visible="false"></asp:Label>
                                            <asp:HiddenField ID="hfCurrentPrintOption" runat="server"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButtonList ID="rbConfirmDetailPrintClaimConsentFormLanguage" runat="server"
                                                            RepeatDirection="Horizontal">
                                                        </asp:RadioButtonList></td>
                                                    <td>
                                                        <asp:Label ID="ClaimPrintOptionError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panConfirmDetailPerprintFormNotice" runat="server">
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
                            <div class="textVersionTable" style="padding-bottom: 4px; padding-top: 4px">
                                <asp:Button ID="btnConfirmDetailChangePrintOption" runat="server" Text="<%$ Resources:AlternateText, ChangePrintOption%>" /></div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnConfirmDetailBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                        <asp:Button ID="btnConfirmDetailConfirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn%>" />
                        <asp:Button ID="btnConfirmDetailAdhocPrintConsentForm" runat="server" Text="<%$ Resources:AlternateText, VRAPrintClaimConsentFormBtn %>" /></td>
                </tr>
            </table>
            <div style="padding-top: 4px">
                &nbsp;&nbsp;
            </div>
        </asp:View>
        <asp:View ID="vCompleteClaim" runat="server">
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblCompleteClaimAccInfo" runat="server" CssClass="tableHeader"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <uc3:ucReadOnlyDocumnetType ID="udcCompleteClaimReadOnlyDocumnetType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr id="hrCompleteClaimAccInfo" class="textVersionTable" />
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tbody>
                    <tr>
                        <td class="eHSTableHeading">
                            <asp:Label ID="lblCompleteClaimClaimInfo" runat="server" CssClass="tableHeader"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimTransNumText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCompleteClaimTransNum" runat="server" CssClass="tableText" ForeColor="Blue"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label align="top" ID="lblStep3PrefixTransNum" runat="server" CssClass="tableText" ForeColor="Black" Font-Strikeout="true"></asp:Label>
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
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimTransDateText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimTransDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimSchemeText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimScheme" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr id="trCompleteNonClinicSetting" runat="server" style="display:none">
                        <td valign="top">
                            <asp:Label ID="lblCompleteNonClinicSetting" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimServiceDateText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimServiceDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimPracticeText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimPractice" runat="server" CssClass="tableTextChi"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimBankAccText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimBankAcc" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimServiceTypeText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblCompleteClaimServiceType" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
            <uc5:ucReadOnlyEHSClaim ID="udcCompleteClaimReadOnlyEHSClaim" runat="server" />
            <asp:panel ID="panCompleteRecipientCondition" runat="server" Visible="false">
            <table style="border-collapse: collapse; border-spacing:0px">
                <tr>
                    <td style="vertical-align: top;width:205px;padding:0px">
                        <asp:Label ID="lblCompleteRecipientConditionText" runat="server" CssClass="tableTitle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;padding:0px">
                        <asp:Label ID="lblCompleteRecipientCondition" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
            </asp:panel>
            <asp:panel ID="panCompleteVSS" runat="server" Visible="false">
            <table style="border-collapse: collapse; border-spacing:0px">
                <tr>
                    <td style="vertical-align: top;width:205px;padding:0px">
                        <asp:Label ID="lblCompleteVSSContactNoText" runat="server" CssClass="tableTitle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;padding:0px">
                        <asp:Label ID="lblCompleteVSSContactNo" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;width:205px;padding:0px">
                        <asp:Label ID="lblCompleteVSSRemarksText" runat="server" CssClass="tableTitle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;padding:0px">
                        <asp:Label ID="lblCompleteVSSRemarks" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
            </asp:panel>

            <asp:Button ID="btnCompleteClaimNextClaim" runat="server" Width="80px" />
            <asp:Button ID="btnCompleteClaimClaimForSamePatient" runat="server" Width="155px" />
        </asp:View>
        <asp:View ID="vServiceDate" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblServiceDateAccInfoText" runat="server" CssClass="tableHeader"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblServiceDateAccInfoENameText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblServiceDateAccInfoEName" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnServiceDateViewDetail" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnServiceDateViewVaccinationRecord" runat="server" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td valign="top">
                        <hr id="hrServiceDateView" class="textVersionTable" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblServiceDateClaimInfoTitle" runat="server" CssClass="tableHeader"
                            Text="<%$ Resources:Text, InputClaimInformation %>"></asp:Label></td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="lblServiceDateText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:TextBox ID="txtServiceDate" runat="server" MaxLength="10" Width="65px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                            onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                            onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                        <asp:Label ID="lblServiceDateError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label></td>
                </tr>
            </table>
            <asp:Button ID="btnServiceDateCancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
            <asp:Button ID="btnServiceDateNext" runat="server" Text="<%$ Resources: AlternateText, NextBtn %>" /></asp:View>
        <asp:View ID="vCategory" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblCategoryAccInfoText" runat="server" CssClass="tableHeader"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblCategoryAccInfoENameText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblCategoryAccInfoEName" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnCategoryViewDetail" runat="server" /></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnCategoryViewVaccinationRecord" runat="server" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td valign="top">
                        <hr id="hrCategoryView" class="textVersionTable" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblCategoryClaimInfoTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, InputClaimInformation %>"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblCategoryText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Category%>"></asp:Label>
                        <asp:Label ID="lblCategoryError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:RadioButtonList ID="rbCategory" runat="server" Width="100%">
                        </asp:RadioButtonList></td>
                </tr>
            </table>
            <asp:Button ID="btnCategoryBack" runat="server" Text="<%$ Resources: AlternateText, BackBtn %>" />
            <asp:Button ID="btnCategoryCancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
            <asp:Button ID="btnCategoryNext" runat="server" Text="<%$ Resources: AlternateText, NextBtn%>" />
        </asp:View>
        <asp:View ID="vMedicalCondition" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblMedicalConditionAccInfoText" runat="server" CssClass="tableHeader"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblMedicalConditionAccInfoENameText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblMedicalConditionAccInfoEName" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnMedicalConditionViewDetail" runat="server" /></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnMedicalConditionViewVaccinationRecord" runat="server" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td valign="top">
                        <hr id="Hr1" class="textVersionTable" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblMedicalConditionClaimInfoTitle" runat="server" CssClass="tableHeader"
                            Text="<%$ Resources:Text, InputClaimInformation %>"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblMedicalConditionText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, PreConditions%>"></asp:Label>
                        <asp:Label ID="lblMedicalConditionError" runat="server" ForeColor="Red" Text="*"
                            Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:RadioButtonList ID="rbMedicalCondition" runat="server" Width="100%">
                        </asp:RadioButtonList></td>
                </tr>
            </table>
            <asp:Button ID="btnMedicalConditionBack" runat="server" Text="<%$ Resources: AlternateText, BackBtn %>" />
            <asp:Button ID="btnMedicalConditionCancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
            <asp:Button ID="btnMedicalConditionNext" runat="server" Text="<%$ Resources: AlternateText, NextBtn%>" />
        </asp:View>
        <asp:View ID="RCHCode" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblECHCodeAccInfoText" runat="server" CssClass="tableHeader"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblECHCodeAccInfoENameText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblECHCodeAccInfoEName" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnRCHCodeViewDetail" runat="server" /></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnRCHCodeViewVaccinationRecord" runat="server" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td style="width: 100%" valign="top">
                        <hr id="Hr2" class="textVersionTable" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblRCHCodeClaimInfoTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, InputClaimInformation %>"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblRCHCodeText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:TextBox ID="txtRCHCode" runat="server" MaxLength="6" Width="65px"></asp:TextBox><asp:Label
                            ID="lblRCHCodeError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnRCHCodeBack" runat="server" Text="<%$ Resources: AlternateText, BackBtn %>" />
            <asp:Button ID="btnRCHCodeCancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn %>" />
            <asp:Button ID="btnRCHCodeNext" runat="server" Text="<%$ Resources: AlternateText, NextBtn %>" /></asp:View>
        <asp:View ID="vVaccine" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblVaccineAccInfoText" runat="server" CssClass="tableHeader"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblVaccineAccInfoENameText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblVaccineAccInfoEName" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnVaccineViewDetail" runat="server" /></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnVaccineViewVaccinationRecord" runat="server" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td valign="top">
                        <hr id="Hr3" class="textVersionTable" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; height: 19px">
                        <asp:Label ID="lblVaccineClaimInfoTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, InputClaimInformation %>"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblVaccineText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <cc2:ClaimVaccineInputText ID="udcVaccineClaimVaccineInputText" runat="server" CssTableText="tableText"
                            CssTableTitle="tableTitle" />
                    </td>
                </tr>
                <asp:Panel ID="panVSSRecipientCondition" runat="server" Visible="false">
                <tr>
                    <td style="width: 100%; height: 19px">
                        <asp:Label ID="lblRecipientConditionTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, RecipientCondition %>" />
                        &nbsp;&nbsp;
                        <asp:linkbutton ID="lBtnRecipientConditionHelp" runat="server" SkinID="TextOnlyVersionLinkButton" style="color:blue;font-family:Arial;font-size:13px" Text="<%$ Resources:Text, Remarks %>" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <table style="border-collapse: collapse; border-spacing:0px">
                            <tr>
                                <td style="width:auto">
                                    <asp:RadioButtonList ID="rblRecipientCondition" runat="server" AutoPostBack="true" Visible="false" Enabled="false" RepeatColumns ="2" Width="260px" 
                                        style="position:relative;left:-5px">
                                    </asp:RadioButtonList>
                                    <asp:CheckBox ID="chkRecipientCondition" runat="server" AutoPostBack="true" Visible="false" Enabled="false" Width="260px"
                                        style="position:relative;left:-2px">
                                    </asp:CheckBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblRecipientConditionError" runat="server"  EnableViewState="true" ForeColor="Red" Text="*" Visible="False" 
                                         style="position:relative;left:-10px"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </asp:Panel>
                <asp:Panel ID="panVSSContactNo" runat="server" Visible="false">
                <tr>
                    <td style="width: 100%; height: 19px; padding-top:5px">
                        <asp:Label ID="lblContactNoText" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, ContactNo2 %>" />                        
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:TextBox ID="txtContactNo" runat="server" MaxLength="8" Style="position: relative; left: -1px" Width="100px" />
                        <asp:Label ID="lblContactNoError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>                        
                        <cc3:FilteredTextBoxExtender ID="fteContactNo" runat="server" TargetControlID="txtContactNo"
                            FilterType="Numbers" />                       
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblContactNoRecommendation" runat="server" Text="<%$ Resources:Text, ProvideContactNoWithSMS%>" Style="font-size: 14px;"></asp:Label>
                    </td>
                </tr>
                </asp:Panel>
                <asp:Panel ID="panVSSRemark" runat="server" Visible="false">
                    <tr>
                    <td style="width: 100%; height: 19px; padding-top:5px">
                        <asp:Label ID="lblRemarksText" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, Remarks %>" />                        
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:TextBox ID="txtRemarks" runat="server" MaxLength="100" Style="position: relative; left: -1px" Width="230px" />
                        <asp:Label ID="lblRemarksError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>                        
                        <cc3:FilteredTextBoxExtender ID="fteRemarks" runat="server" TargetControlID="txtRemarks"
                                      FilterMode="InvalidChars"  InvalidChars="|\&quot;" />                       
                    </td>
                </tr>
                </asp:Panel>
                <tr>
                    <td style="padding-top:10px"></td>
                </tr>
            </table>
            <asp:Button ID="btnVaccineBack" runat="server" Text="<%$ Resources: AlternateText, BackBtn %>" />
            <asp:Button ID="btnVaccineCancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn %>" />
            <asp:Button ID="btnVaccineNext" runat="server" Text="<%$ Resources: AlternateText, NextBtn %>" /></asp:View>
        <asp:View ID="vInternalError" runat="server">
            <asp:Button ID="btnViewInternalErrorReturn" runat="server" Text="<%$ Resources:AlternateText, ReturnBtn%>" />
        </asp:View>
        <asp:View ID="vConfirmBox" runat="server">
            <asp:Label ID="lblConfirmOnlyBoxTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ConfirmBoxTitle %>"
                Width="100%"></asp:Label><br />
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <table id="tblConfirmBoxContainer" runat="server" cellpadding="0" cellspacing="0">
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
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblPrintOptionTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:AlternateText, ChangePrintOption%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc1:PrintFormOptionSelection ID="udtPrintOptionSelection" runat="server" PrintOptionTableWidth="100%" />
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
                        <asp:Button ID="btnAddHocPrintSelectionBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
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
                        <asp:Button ID="btnViewRemarkBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vSelectPractice" runat="server">
            <asp:Panel ID="panStepSelectPractice" runat="server" DefaultButton="btnStepSelectPracticeSelect">
                <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                    <tr>
                        <td>
                            <asp:Label ID="lblStepSelectPracticePracticeText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <cc2:PracticeRadioButtonGroupText ID="ucSelectPracticeRadioButtonGroupText" runat="server"
                                CssClass="tableTextChi">
                            </cc2:PracticeRadioButtonGroupText>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnStepSelectPracticeSelect" runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:View>
        <asp:View ID="vSelectScheme" runat="server">
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
                        <cc2:SchemeRadioButtonGroupText ID="ucSchemeRadioButtonGroupText" runat="server">
                        </cc2:SchemeRadioButtonGroupText>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnStepSelectSchemeBack" runat="server" />
                        <asp:Button ID="btnStepSelectSchemeSelect" runat="server" /></td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vVaccinationRecord" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblVaccinationRecordAccInfoText" runat="server" CssClass="tableHeader"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblVaccinationRecordAccInfoENameText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblVaccinationRecordAccInfoEName" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnVaccinationRecordViewDetail" runat="server" />
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
                        <uc7:ucvaccinationrecord id="udcVaccinationRecord" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnVRContinue" runat="server" Text="<%$ Resources: AlternateText, ContinueBtn %>" />
            <asp:Button ID="btnVRReturn" runat="server" Text="<%$ Resources: AlternateText, ReturnBtn %>" />
        </asp:View>
        <asp:View ID="DocumentaryProof" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblDocumentaryProofAccInfoText" runat="server" CssClass="tableHeader"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblDocumentaryProofAccInfoENameText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblDocumentaryProofAccInfoEName" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnDocumentaryProofViewDetail" runat="server" /></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnDocumentaryProofViewVaccinationRecord" runat="server" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td valign="top">
                        <hr id="hrDocumentaryProofView" class="textVersionTable" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblDocumentaryProofClaimInfoTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, InputClaimInformation %>"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblDocumentaryProofText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TypeOfDocumentaryProof%>"></asp:Label>
                        <asp:Label ID="lblDocumentaryProofError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                         <!---CRE20-009 Set autopostback to true for active the selectchanged function [Start][Nichole] -->
                        <asp:RadioButtonList ID="rbDocumentaryProof" runat="server" Width="100%" AutoPostBack="true" Visible="false"/>
                         <!---CRE20-009 Set autopostback to true for active the selectchanged function [End][Nichole] -->
                        <asp:checkbox ID="chkDocumentaryProof" runat="server" Visible="false"/>
                        <asp:hiddenfield ID="hfDocumentaryProof" runat="server" Visible="false"/>
                    </td>
                </tr>
                <!--CRE20-009 add checkbox CSSA and Annex for CSSA Claim [Start][Nichole] -->
                <asp:Panel ID="panVSSDAConfirm" runat="server" visible="false" >
                <tr>
                    <td style="width: 100%"><br />
                         <table border="0" style="border-collapse: collapse; border-spacing:0px">
                         <tr> <td style="width:20px">&nbsp;</td>
                             <td style="width: 5px; vertical-align:top ">
                                <asp:checkbox ID="chkDocProofCSSA" runat="server" />
                             </td>
                             <td>
                                <asp:Label ID="lblDocProofCSSA" runat="server"   AssociatedControlId="chkDocProofCSSA"  CssClass="tableText"  Text="<%$ Resources:Text,  ProvidedInfoCSSA%>"></asp:Label>
                                <asp:Label ID="lblVSSDAConfirmCSSAError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                            </td>
                         </tr>
                         </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                         <table border="0" style="border-collapse: collapse; border-spacing:0px">
                         <tr><td style="width:20px">&nbsp;</td>
                             <td style="width: 5px; vertical-align:top ">
                                <asp:checkbox ID="chkDocProofAnnex" runat="server"  />
                             </td>
                             <td>
                                <asp:Label ID="lblDocProofAnnex" runat="server"  AssociatedControlId="chkDocProofAnnex"   CssClass="tableText"  Text="<%$ Resources:Text,  ProvidedInfoAnnex%>"></asp:Label>
                                <asp:Label ID="lblVSSDAConfirmAnnexError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                            </td>
                         </tr>
                             <tr><td colspan="2">&nbsp;</td></tr>
                         </table>
                    </td>
                </tr>
                </asp:Panel> 
                 <!--CRE20-009 add checkbox CSSA and Annex for CSSA Claim [End][Nichole] -->
            </table>
            <asp:Button ID="btnDocumentaryProofBack" runat="server" Text="<%$ Resources: AlternateText, BackBtn %>" />
            <asp:Button ID="btnDocumentaryProofCancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
            <asp:Button ID="btnDocumentaryProofNext" runat="server" Text="<%$ Resources: AlternateText, NextBtn%>" />
        </asp:View>
        <asp:View ID="PlaceVaccination" runat="server">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblPlaceVaccinationAccInfoText" runat="server" CssClass="tableHeader"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblPlaceVaccinationAccInfoENameText" runat="server" CssClass="tableTitle"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblPlaceVaccinationAccInfoEName" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnPlaceVaccinationViewDetail" runat="server" /></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Button ID="btnPlaceVaccinationViewVaccinationRecord" runat="server" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td valign="top">
                        <hr id="hrPlaceVaccinationView" class="textVersionTable" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblPlaceVaccinationClaimInfoTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, InputClaimInformation %>"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblPlaceVaccinationText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, PlaceOfVaccination%>"></asp:Label>
                        <asp:Label ID="lblPlaceVaccinationError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:RadioButtonList ID="rbPlaceVaccination" runat="server" Width="100%" AutoPostBack="true">
                        </asp:RadioButtonList></td>
                </tr>
                <asp:Panel ID="panPlaceVaccinationOther" runat="server" Visible ="false">
                <tr>
                    <td style="width: 100%">
                        <div style="margin-left:22px;height:25px;vertical-align:top">
                            <asp:Label ID="lblPlaceVaccinationOther" runat="server" CssClass="tableText" Text="<%$ Resources:Text, PlaceOfVaccinationOtherPleaseSpecify%>" Enabled="false"></asp:Label>
                        </div>
                        <div style="margin-left:22px">
                            <asp:textbox ID="txtPlaceVaccinationOther" runat="server" Width="270px" AutoPostBack="false" MaxLength="255" Enabled="false"/>
                        </div>
                    </td>
                </tr>
                </asp:Panel>
            </table>
            <asp:Button ID="btnPlaceVaccinationBack" runat="server" Text="<%$ Resources: AlternateText, BackBtn %>" />
            <asp:Button ID="btnPlaceVaccinationCancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
            <asp:Button ID="btnPlaceVaccinationNext" runat="server" Text="<%$ Resources: AlternateText, NextBtn%>" />
        </asp:View>
        <asp:View ID="vRecipientConditionHelp" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblRecipientConditionHeading" runat="server" CssClass="tableHeader"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divRecipientConditionHelpTitle" runat="server" style="width:100%;margin: 2px 2px 0px 2px"></div>                                                            
                        <div id="divRecipientConditionHelpContent" runat="server" style="width:95%;margin: 2px 2px 4px 2px"></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnViewRecipientConditionBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vSubsidizeDisabledDetail" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <asp:Label ID="lblSubsidizeDisabledDetailHeading" runat="server" CssClass="tableHeader"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<div id="divSubsidizeDisabledDetailTitle" runat="server" style="width:100%;margin: 2px 2px 0px 2px"></div>--%>                                                            
                        <div id="divSubsidizeDisabledDetailContent" runat="server" style="width:100%;margin: 2px 2px 4px 2px"></div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnViewSubsidizeDisabledDetailBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>

    </asp:MultiView>
    <asp:HiddenField ID="hfEHSClaimTokenNum" runat="server" EnableViewState="true"></asp:HiddenField>
</asp:Content>
