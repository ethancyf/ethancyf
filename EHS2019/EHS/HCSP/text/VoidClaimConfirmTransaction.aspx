<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/text/ClaimVoucher.Master"
    Codebehind="VoidClaimConfirmTransaction.aspx.vb" Inherits="HCSP.VoidClaimConfirmDetail"
    Title="<%$Resources:Title, ReimbursementClaimTransMgt%>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="../UIControl/ucInputDocumentType.ascx" TagName="ucInputDocumentType"
    TagPrefix="uc1" %>
<%@ Register Src="../UIControl/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc2" %>
<%@ Register Src="../UIControl/ucReadOnlyEHSClaim.ascx" TagName="ucReadOnlyEHSClaim"
    TagPrefix="uc3" %>
<%@ Register Src="../UIControl/ucInputEHSClaimText.ascx" TagName="ucInputDeferredClaimDetails"
    TagPrefix="uc4" %>
<%@ Register Src="../UIControl/EHSClaimText/ucReasonForVisit.ascx" TagName="ucReasonForVisit"
    TagPrefix="uc5" %>
<%@ OutputCache Duration="1" Location="None" VaryByParam="none" NoStore="true" %>
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
    <asp:Panel ID="panPersonalInformation" runat="server">
        <table id="tabVoucherDetail" runat="server" cellpadding="0" cellspacing="0" style="width: 201px">
            <tr>
                <td style="width: 100%" valign="top">
                    <asp:Label ID="lblAcctInfo" runat="server" Text="<%$ Resources:Text, AccountInfo %>"
                        CssClass="tableHeader"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 100%" valign="top">
                    <uc2:ucReadOnlyDocumnetType ID="udcTranDetailReadOnlyDocumnetType" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:MultiView ID="mvVoidTranDetail" runat="server" EnableTheming="True">
        <asp:View ID="vEnterVoidReason" runat="server">
            <asp:Panel ID="pnlEnterVoidReasonContainer" runat="server" DefaultButton="btnEnterVoidReasonConfirm">
                <table cellpadding="0" cellspacing="0" class="textVersionTable">
                    <tbody>
                        <tr>
                            <td valign="top">
                                <hr style="width: 100%" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonClaimInfo" runat="server" CssClass="tableHeader"
                                    Text="<%$ Resources:Text, ClaimInfo %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonTransNumText" runat="server" CssClass="tableTitle"
                                    Text="<%$ Resources:Text, TransactionNo %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonTransNum" runat="server" CssClass="tableText" ForeColor="Blue"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonTransDateText" runat="server" CssClass="tableTitle"
                                    Text="<%$ Resources:Text, TransactionDate %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonTransDate" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonSchemeText" runat="server" CssClass="tableTitle"
                                    Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonScheme" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonServiceDateText" runat="server" CssClass="tableTitle"
                                    Text="<%$ Resources:Text, ServiceDate %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonServiceDate" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonPracticeText" runat="server" CssClass="tableTitle"
                                    Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonPractice" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonBankAcctText" runat="server" CssClass="tableTitle"
                                    Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonBankAcct" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonServiceTypeText" runat="server" CssClass="tableTitle"
                                    Text="<%$ Resources:Text, ServiceType %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="lblEnterVoidReasonServiceType" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                    </tbody>
                </table>
                <uc3:ucReadOnlyEHSClaim ID="udcEnterVoidReasonReadOnlyEHSClaim" runat="server" />

                <asp:panel ID="panEnterVoidReasonRecipientCondition" runat="server" Visible="false">
                <table style="border-collapse: collapse; border-spacing:0px">
                    <tr>
                        <td style="vertical-align: top;width:205px;padding:0px">
                            <asp:Label ID="lblEnterVoidReasonRecipientConditionText" runat="server" CssClass="tableTitle"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;padding:0px">
                            <asp:Label ID="lblEnterVoidReasonRecipientCondition" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                </table>
                </asp:panel>

                <table cellpadding="0" cellspacing="0" style="width: 100%" runat="server" visible="true"
                    id="Table1">
                    <tr>
                        <td style="width: 100%">
                            <asp:Label ID="lblEnterVoidReasonVoidReasonText" runat="server" Text="<%$ Resources:Text, VoidReason %>"
                                Font-Bold="True"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 100%">
                            <asp:TextBox ID="txtEnterVoidReasonVoidReason" runat="server" MaxLength="255" CssClass="textChi"></asp:TextBox><asp:Label
                                ID="lblEnterVoidReasonVoidReasonError" runat="server" ForeColor="Red" Text="*"
                                Visible="False"></asp:Label></td>
                    </tr>
                </table>
                <asp:Button ID="btnEnterVoidReasonCancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn %>" />
                <asp:Button ID="btnEnterVoidReasonConfirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn %>" />
            </asp:Panel>
        </asp:View>
        <asp:View ID="vConfirmVoidReason" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tbody>
                    <tr>
                        <td valign="top">
                            <hr style="width: 100%" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonClaimInfo" runat="server" CssClass="tableHeader"
                                Text="<%$ Resources:Text, ClaimInfo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonTransNumText" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, TransactionNo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonTransNum" runat="server" CssClass="tableText"
                                ForeColor="Blue"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonTransDateText" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, TransactionDate %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonTransDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonSchemeText" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonScheme" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonServiceDateText" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, ServiceDate %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonServiceDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonPracticeText" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonPractice" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonBankAcctText" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonBankAcct" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonServiceTypeText" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, ServiceType %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblConfirmVoidReasonServiceType" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
            <uc3:ucReadOnlyEHSClaim ID="udcConfirmVoidReasonBankReadOnlyEHSClaim" runat="server" />

            <asp:panel ID="panConfirmVoidReasonRecipientCondition" runat="server" Visible="false">
            <table style="border-collapse: collapse; border-spacing:0px">
                <tr>
                    <td style="vertical-align: top;width:205px;padding:0px">
                        <asp:Label ID="lblConfirmVoidReasonRecipientConditionText" runat="server" CssClass="tableTitle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;padding:0px">
                        <asp:Label ID="lblConfirmVoidReasonRecipientCondition" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
            </asp:panel>

            <table cellpadding="0" cellspacing="0" style="width: 100%" runat="server" visible="true"
                id="Table3">
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblConfirmVoidReasonVoidReasonText" runat="server" Font-Bold="True"
                            Text="<%$ Resources:Text, VoidReason %>"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%">
                        <asp:Label ID="lblConfirmVoidReasonVoidReason" runat="server"></asp:Label></td>
                </tr>
            </table>
            <asp:Button ID="btnConfirmVoidReasonBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn %>" />
            <asp:Button ID="btnConfirmVoidReasonConfirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn %>" /></asp:View>
        <asp:View ID="vCompleteVoid" runat="server">
            <table id="Table2" cellpadding="0" cellspacing="0" style="width: 100%" runat="server"
                visible="true">
                <tr>
                    <td style="width: 100%; height: 19px;" valign="top">
                        <asp:Label ID="lblCompleteVoidTranIDText" runat="server" Text="<%$ Resources:Text, VoidTranID %>"
                            Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; height: 19px;" valign="top">
                        <table bgcolor="#ffff80" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top">
                                    <asp:Label ID="lblCompleteVoidTranID" runat="server" ForeColor="Blue" BackColor="Transparent"
                                        Font-Bold="True"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%" valign="top">
                        <asp:Label ID="lblCompleteVoidDtmText" runat="server" Text="<%$ Resources:Text, VoidDtm %>"
                            Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%" valign="top">
                        <asp:Label ID="lblCompleteVoidDtm" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; height: 19px;" valign="top">
                        <asp:Label ID="lblCompleteVoidByText" runat="server" Text="<%$ Resources:Text, VoidBy %>"
                            Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%" valign="top">
                        <asp:Label ID="lblCompleteVoidBy" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%" valign="top">
                        <asp:Label ID="lblCompletVoidReasonText" runat="server" Font-Bold="True" Text="<%$ Resources:Text, VoidReason %>"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%" valign="top">
                        <asp:Label ID="lblCompleteVoidReason" runat="server"></asp:Label></td>
                </tr>
            </table>
            <asp:Button ID="btnCompleteReturn" runat="server" Text="<%$ Resources:AlternateText, ReturnBtn %>" /></asp:View>
        <asp:View ID="vComfirmMessage" runat="server">
            <asp:Label ID="lblConfirmMessageText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ConfirmBoxTitle %>"
                Width="100%"></asp:Label><br />
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <table id="tblConfirmBoxContainer" runat="server" cellpadding="0" cellspacing="0"
                            class="tableRemark">
                            <tr>
                                <td>
                                    <asp:Label ID="lblConfirmMessageContentText" runat="server" CssClass="tableText"
                                        Text="<%$ Resources:Text, CancelAlert %>"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 4px">
                        <asp:Button ID="lblConfirmMessageBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" />
                        <asp:Button ID="lblConfirmMessageConfirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn%>" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vRemarks" runat="server">
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
                        <asp:Button ID="btnViewRemarkBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn%>" /></td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vInternalError" runat="server">
            <asp:Button ID="btnInternalErrorBack" runat="server" Text="<%$ Resources:AlternateText, ReturnBtn %>" />
        </asp:View>
        <asp:View ID="vViewTransactionDetail" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tbody>
                    <tr>
                        <td valign="top">
                            <hr style="width: 100%" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label1" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, ClaimInfo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label2" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionNo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblViewTransactionDetailTransactionNo" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>                  
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label4" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionDate %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblViewTransactionDetailTransactionDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label6" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblViewTransactionDetailScheme" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label33" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblViewTransactionDetailTransactionStatus" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>                      
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label8" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceDate %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblViewTransactionDetailServiceDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label10" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblViewTransactionDetailPractice" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                 <%--   <!-- contact Number -->
                    <tr ID="trContactNumText" runat="server" style="display:none">
                        <td valign="top">
                            <asp:Label ID="Label34" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ContactNo2 %>"></asp:Label></td>
                    </tr>
                    <tr  ID="trContactNum" runat="server" style="display:none">
                        <td valign="top">
                            <asp:Label ID="lblContactNum" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>--%>

                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label12" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblViewTransactionDetailBankAccountNo" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label14" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceType %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblViewTransactionDetailServiceType" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
            <uc3:ucReadOnlyEHSClaim ID="udcClaimTransactionDetailReadOnlyEHSClaim" runat="server" />

            <asp:panel ID="panDetailRecipientCondition" runat="server" Visible="false">
            <table style="border-collapse: collapse; border-spacing:0px">
                <tr>
                    <td style="vertical-align: top;width:205px;padding:0px">
                        <asp:Label ID="lblDetailRecipientConditionText" runat="server" CssClass="tableTitle"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;padding:0px">
                        <asp:Label ID="lblDetailRecipientCondition" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
            </asp:panel>

            <asp:Button ID="btnClaimTransactionDetailBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn %>" />
            <asp:Button ID="btnClaimTransactionDetailVoid" runat="server" Text="<%$ Resources:AlternateText, VoidBtn %>" />
            <asp:Button ID="btnClaimTransactionDetailModify" runat="server" Text="<%$ Resources:AlternateText, ModifyBtn %>" />
        </asp:View>
        <asp:View ID="vModifyTransaction" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tbody>
                    <tr>
                        <td valign="top">
                            <hr style="width: 100%" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label3" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, ClaimInfo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label5" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionNo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionTransactionNo" runat="server" CssClass="tableText"
                                ForeColor="Blue"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label7" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionDate %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionTransactionDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label9" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionScheme" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionTransactionStatusText" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionTransactionStatus" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>                    
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label11" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceDate %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionServiceDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label13" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionPractice" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label15" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionBankAccountNo" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label16" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceType %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionServiceType" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
            <uc4:ucInputDeferredClaimDetails ID="ucInputDeferredClaimDetails_Modify" runat="server" />
            <asp:Button ID="btnModifyTransactionBack" runat="server" Text="<%$ Resources:AlternateText, CancelBtn %>" />
            <asp:Button ID="btnModifyTransactionNext" runat="server" Text="<%$ Resources:AlternateText, NextBtn %>" />
        </asp:View>
        <asp:View ID="vModifyTransactionConfirm" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tbody>
                    <tr>
                        <td valign="top">
                            <hr style="width: 100%" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label17" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, ClaimInfo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label18" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionNo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConfirmTransactionNo" runat="server" CssClass="tableText"
                                ForeColor="Blue"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label19" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionDate %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConfirmTransactionDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label20" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConfirmScheme" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
            <table cellpadding="0" cellspacing="0" class="textVersionTable" runat="server" id="tblModifyTransactionConfirmTransactionStatus">
                <tbody>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConfirmTransactionStatusText" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConfirmTransactionStatus" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tbody>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label21" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceDate %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConfirmServiceDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label22" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConfirmPractice" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label23" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConfirmBankAccountNo" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label24" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceType %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConfirmServiceType" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
            <uc3:ucReadOnlyEHSClaim ID="udcModifyTransactionConfirmReadOnlyEHSClaim" runat="server" />
            <asp:Button ID="btnModifyTransactionConfirmBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn %>" />
            <asp:Button ID="btnModifyTransactionConfirmSave" runat="server" Text="<%$ Resources:AlternateText, SaveBtn %>" />
            <asp:Button ID="btnModifyTransactionConfirmConfirm" runat="server" Text="<%$ Resources:AlternateText, SaveAndConfirmBtn %>" />
        </asp:View>
        <asp:View ID="vModifyTransactionConpleted" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tbody>
                    <tr>
                        <td valign="top">
                            <hr style="width: 100%" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label25" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, ClaimInfo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label26" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionNo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConpletedTransactionNo" runat="server" CssClass="tableText"
                                ForeColor="Blue"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label27" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionDate %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConpletedTransactionDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label28" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConpletedScheme" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label35" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConpletedTransactionStatus" runat="server" CssClass="tableText" ></asp:Label></td>
                    </tr>                    
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label29" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceDate %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConpletedServiceDate" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label30" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConpletedPractice" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label31" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConpletedBankAccountNo" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="Label32" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceType %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Label ID="lblModifyTransactionConpletedServiceType" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </tbody>
            </table>
            <uc3:ucReadOnlyEHSClaim ID="udcModifyTransactionConpletedReadOnlyEHSClaim" runat="server" />
            <asp:Button ID="btnModifyTransactionCompletedReturn" runat="server" Text="<%$ Resources:AlternateText, ReturnBtn %>" />
        </asp:View>
        <asp:View ID="vReasonForVisit" runat="server">
            <table cellpadding="0" cellspacing="0" class="textVersionTable">
                <tr>
                    <td>
                        <uc5:ucReasonForVisit runat="server" ID="udcReasonForVisit">
                        </uc5:ucReasonForVisit>
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
