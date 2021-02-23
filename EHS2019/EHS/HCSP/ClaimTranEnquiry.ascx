<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ClaimTranEnquiry.ascx.vb"
    Inherits="HCSP.ClaimTranEnquiry" %>
<%@ Register Src="~/UIControl/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumentType"
    TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/ucReadOnlyEHSClaim.ascx" TagName="ucReadOnlyEHSClaim"
    TagPrefix="uc2" %>
<%@ Register Src="~/UIControl/ucInputEHSClaim.ascx" TagName="ucInputEHSClaim"
    TagPrefix="uc4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/SchemeLegend.ascx" TagName="SchemeLegend" TagPrefix="uc3" %>
<table style="width: 950px" cellpadding="0" cellspacing="0">
    <tr>
        <td class="tableHeading" colspan="2" style="vertical-align: top">
            <asp:Label ID="lblAccountInfo" runat="server" Text="<%$ Resources:Text, AccountInfo %>"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="vertical-align: top">
            <uc1:ucReadOnlyDocumentType ID="udcReadOnlyDocumentType" runat="server"></uc1:ucReadOnlyDocumentType>
        </td>
    </tr>
    <tr style="height: 0px">
        <td colspan="2"></td>
    </tr>
    <tr>
        <td class="tableHeading" colspan="2" style="vertical-align: top">
            <asp:Label ID="lblClaimInfo" runat="server" Text="<%$ Resources:Text, ClaimInfo %>"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr id="trTransactionNo" runat="server">
                    <td style="vertical-align: top; width: 205px" class="tableCellStyle">
                        <asp:Label ID="lblTransactionNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionNo %>"></asp:Label></td>
                    <td style="vertical-align: top;">
                        <asp:Label ID="lblTransactionNo" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblTransactionDate" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblTransactionDate_Chi" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr id="trConfirmTime" runat="server">
                    <td style="vertical-align: top" class="tableCellStyle">
                        <asp:Label ID="lblConfirmTimeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ConfirmedTime %>"></asp:Label></td>
                    <td style="vertical-align: top">
                        <asp:Label ID="lblConfirmTime" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblConfirmTime_Chi" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td style="vertical-align: top; width: 205px" class="tableCellStyle">
                        <asp:Label ID="lblSchemeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                    <td style="vertical-align: top;" class="tableCellStyle">
                        <asp:Label ID="lblScheme" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblScheme_Chi" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblScheme_CN" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr id="trTransactionStatus" runat="server">
                    <td style="vertical-align: top" class="tableCellStyle">
                        <asp:Label ID="lblTransactionStatusText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label></td>
                    <td style="vertical-align: top">
                        <asp:Label ID="lblTransactionStatus" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblTransactionStatus_Chi" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblTransactionStatus_CN" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblReimbursementMethod" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblReimbursementMethod_Chi" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblReimbursementMethod_CN" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblInvalidationStatus" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblInvalidationStatus_Chi" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblInvalidationStatus_CN" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top" class="tableCellStyle">
                        <asp:Label ID="lblServiceDateText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceDate %>"></asp:Label></td>
                    <td style="vertical-align: top">
                        <asp:Label ID="lblServiceDate" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblServiceDate_Chi" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr style="display: none">
                    <td style="vertical-align: top" class="tableCellStyle">
                        <asp:Label ID="lblSPText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceProvider %>"></asp:Label></td>
                    <td style="vertical-align: top">
                        <asp:Label ID="lblSP" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblSP_Chi" runat="server" CssClass="tableTextChi"></asp:Label></td>
                </tr>
                <tr id="trPractice" runat="server">
                    <td style="vertical-align: top" class="tableCellStyle">
                        <asp:Label ID="lblPracticeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                    <td align="left" style="vertical-align: top">
                        <asp:Label ID="lblPractice" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblPractice_Chi" runat="server" CssClass="tableTextChi"></asp:Label></td>
                </tr>
                <%--<tr id="trContactNo" runat="server"  style="display: none">
                    <td style="vertical-align: top" class="tableCellStyle">
                        <asp:Label ID="lblContactText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ContactNo2 %>"></asp:Label></td>
                    <td style="vertical-align: top">
                        <asp:Label ID="lblContact" runat="server" CssClass="tableText"></asp:Label>
                </tr>--%>
                <tr id="trBankAcct" runat="server">
                    <td style="vertical-align: top" class="tableCellStyle">
                        <asp:Label ID="lblBankAcctText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                    <td style="vertical-align: top">
                        <asp:Label ID="lblBankAcct" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr id="trServiceType" runat="server">
                    <td style="vertical-align: top" class="tableCellStyle">
                        <asp:Label ID="lblServiceTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceType %>"></asp:Label></td>
                    <td style="vertical-align: top">
                        <asp:Label ID="lblServiceType" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblServiceType_Chi" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblServiceType_CN" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
            </table>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">
                        <uc2:ucReadOnlyEHSClaim ID="udcReadOnlyEHSClaim" runat="server"></uc2:ucReadOnlyEHSClaim>
                        <uc4:ucInputEHSClaim ID="udcInputEHSClaim" runat="server" Visible="false"></uc4:ucInputEHSClaim>
                    </td>
                </tr>
                <tr style="height: 2px"></tr>
            </table>

            <asp:Panel ID="panRecipinetContactInfo" runat="server">
            <table style="width: 100%;border-spacing:0;padding:0px;border-collapse:collapse">
                <%--<tr>
                    <td class="eHSTableHeading" colspan="2">
                        <asp:Label ID="lblStep3aRecipinetContactInfoHeading" runat="server" Text="<%$ Resources:Text, RecipientContactInformation%>" />
                    </td>
                </tr>--%>
                <tr id="trContactNo" runat="server" visible="false">
                    <td style="height:25px;width: 205px;vertical-align:top">
                        <asp:Label ID="lblContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo2%>" CssClass="tableTitle" Width="160px" Style="position:relative;left:-1px"/>
                    </td>
                    <td style="height:25px;vertical-align:top">
                        <asp:Label ID="lblContactNo" runat="server" CssClass="tableText" Style="position:relative;left:-2px"/>
                    </td>
                </tr>
                <%--<tr>
                    <td style="padding-bottom:10px">
                    </td>
                </tr>--%>
                <tr>
                    <td style="height:25px;width: 205px;vertical-align:top">
                        <asp:Label ID="lblRemarkText" runat="server" Text="<%$ Resources:Text, Remarks%>" CssClass="tableTitle" Width="160px" Style="position:relative;left:-1px"/>
                    </td>
                    <td style="height:25px;vertical-align:top">
                        <asp:Label ID="lblRemark" runat="server" CssClass="tableText" Style="position:relative;left:-2px"/>
                    </td>
                </tr>
                <asp:Panel ID="panJoinEHRSS" runat="server">
                <tr>
                    <td style="height:25px;width: 205px;vertical-align:top">
                        <asp:Label ID="lblJoinEHRSSText" runat="server" Text="<%$ Resources:Text, JoinEHRSS%>" CssClass="tableTitle" Width="160px" Style="position:relative;left:-1px"/>
                    </td>
                    <td style="height:25px;vertical-align:top">
                        <asp:Label ID="lblJoinEHRSS" runat="server" CssClass="tableText" Style="position:relative;left:-2px"/>
                    </td>
                </tr>
                </asp:Panel>
            </table>
            </asp:Panel>

            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr id="trCreateBy" runat="server">
                    <td style="vertical-align: top; width: 205px" class="tableCellStyle">
                        <asp:Label ID="lblCreateByText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, CreateBy %>"></asp:Label></td>
                    <td align="left" style="vertical-align: top;">
                        <asp:Label ID="lblVia" runat="server" Text="" ForeColor="Brown" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblVia_Chi" runat="server" Text='' ForeColor="Brown" CssClass="tableText"></asp:Label>
                        <asp:HiddenField ID="hdnIsUpload" runat="server"></asp:HiddenField>
                        <asp:Label ID="lblCreateBy" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label Visible="false" ID="lblCreateBy_Chi" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label Visible="false" ID="lblCreateBy_CN" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
                <tr id="trPaymentMethod" runat="server">
                    <td style="vertical-align: top; width: 205px" class="tableCellStyle">
                        <asp:Label ID="lblPaymentMethodText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, PaymentSettlement %>"></asp:Label></td>
                    <td align="left" style="vertical-align: top">
                        <asp:Label ID="lblPaymentMethod" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblPaymentMethod_Chi" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblPaymentMethod_CN" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
            <table style="width: 100%" cellpadding="0" cellspacing="0">
                <tr id="trVoidNo" runat="server" style="display: none">
                    <td style="vertical-align: top; width: 205px" class="tableCellStyle">
                        <asp:Label ID="lblVoidNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, VoidTranID %>"></asp:Label></td>
                    <td style="vertical-align: top;">
                        <asp:Label ID="lblVoidNo" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblVoidDate" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblVoidDate_Chi" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr id="trVoidBy" runat="server" style="display: none">
                    <td style="vertical-align: top" class="tableCellStyle">
                        <asp:Label ID="lblVoidByText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, VoidBy %>"></asp:Label></td>
                    <td style="vertical-align: top">
                        <asp:Label ID="lblVoidBy" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblVoidBy_Chi" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr id="trVoidReason" runat="server" style="display: none">
                    <td style="vertical-align: top; width: 205px" class="tableCellStyle">
                        <asp:Label ID="lblVoidReasonText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, VoidReason %>"></asp:Label></td>
                    <td style="vertical-align: top">
                        <asp:Label ID="lblVoidReason" runat="server" CssClass="tableTextChi"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

