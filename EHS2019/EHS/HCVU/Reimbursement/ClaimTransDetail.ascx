<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ClaimTransDetail.ascx.vb"
    Inherits="HCVU.ClaimTransDetail" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/DocType/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumentType"
    TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/EHSClaim/ucReadOnlyEHSClaim.ascx" TagName="ucReadOnlyEHSClaim"
    TagPrefix="uc2" %>

<asp:Panel ID="panMessageBox" runat="server" Width="950px">
    <cc2:MessageBox ID="udcErrorMessage" runat="server" Visible="False" Width="95%"></cc2:MessageBox>
    <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="95%"></cc2:InfoMessageBox>
</asp:Panel>
<table style="width: 1200px" cellpadding="0" cellspacing="0">
    <tr>
        <td style="width: 1%"></td>
        <td>
            <table style="width: 100%" cellpadding="1" cellspacing="0">
                <tr>
                    <td colspan="2" style="vertical-align: top">
                        <div class="headingText">
                            <asp:Label ID="lblVVoucherRecipientHeading" runat="server" Text="<%$ Resources:Text, VRInformation %>"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="vertical-align: top">
                        <uc1:ucReadOnlyDocumentType ID="udcReadOnlyDocumentType" runat="server"></uc1:ucReadOnlyDocumentType>
                    </td>
                </tr>
                <asp:Panel ID="panVaccinationRecord" runat="server">
                <tr>
                    <td class="headingText">
                        <asp:Label ID="lblCVaccinationRecordHeading" runat="server"
                            Text="<%$ Resources:Text, VaccinationRecordForCOVID19%>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="panNoVaccinationRecord" runat="server">
                            <table
                                style="border: 1px solid; padding: 0px; border-spacing: 1px; border-collapse: collapse; border-color: #CCCCCC; width: 930px">
                                <tr style="background-color: #f08000">
                                    <td
                                        style="width: 16px; vertical-align: middle; text-align: center; border: 1px solid; border-color: #61615b; height: 20px">&nbsp;
                                    </td>
                                    <td
                                        style="width: 125px; vertical-align: middle; text-align: center; border: 1px solid; border-color: #61615b">
                                        <asp:Label ID="lblCInjectionDate" runat="server" Style="color: white; font-size: medium"
                                            Text="<%$ Resources: Text, InjectionDate %>" />
                                    </td>
                                    <td
                                        style="width: 280px; vertical-align: middle; text-align: center; border: 1px solid; border-color: #61615b">
                                        <asp:Label ID="lblCVaccines" runat="server" Style="color: white; font-size: medium"
                                            Text="<%$ Resources: Text, Vaccines %>" />
                                    </td>
                                    <td
                                        style="width: 80px; vertical-align: middle; text-align: center; border: 1px solid; border-color: #61615b">
                                        <asp:Label ID="lblCDose" runat="server" Style="color: white; font-size: medium"
                                            Text="<%$ Resources: Text, DoseSeqShort %>" />
                                    </td>
                                    <td
                                        style="width: 230px; vertical-align: middle; text-align: center; border: 1px solid; border-color: #61615b">
                                        <asp:Label ID="lblCInformationProvider" runat="server"
                                            Style="color: white; font-size: medium"
                                            Text="<%$ Resources: Text, InformationProvider %>" />
                                    </td>
                                </tr>
                                <tr style="background-color: #F7F7DE">
                                    <td style="vertical-align: middle; border: 1px solid; border-color: #61615b; height: 20px"
                                        colspan="5">
                                        <asp:Label ID="lblCNoRecord" runat="server" CssClass="tableText"
                                            Style="font-size: medium; position: relative; left: 3px"
                                            Text="<%$ Resources:Text, NoCOVID19VaccinationRecordFound%>" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:GridView ID="gvCVaccinationRecord" runat="server" AutoGenerateColumns="False" Width="930px"
                            AllowSorting="True" AllowPaging="False" OnRowDataBound="gvCVaccinationRecord_RowDataBound"
                            OnPreRender="gvCVaccinationRecord_PreRender" OnSorting="gvCVaccinationRecord_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#f08000" />
                                    <ItemStyle Width="16px" VerticalAlign="Top" Font-Size="Medium"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="ServiceReceiveDtm"
                                    HeaderText="<%$ Resources: Text, InjectionDate %>">
                                    <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#f08000" />
                                    <ItemStyle Width="100px" VerticalAlign="Top" Font-Size="Medium"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCInjectionDate" runat="server" Text='<%# Bind("ServiceReceiveDtm") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="SubsidizeDesc" HeaderText="<%$ Resources: Text, Vaccines %>">
                                    <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#f08000" />
                                    <ItemStyle Width="355px" VerticalAlign="Top" Font-Size="Medium"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCVaccination" runat="server" Text='<%# Bind("SubsidizeDesc") %>'>
                                        </asp:Label>
                                        <asp:Label ID="lblCVaccinationChi" runat="server" Text='<%# Bind("SubsidizeDescChi") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="AvailableItemDesc"
                                    HeaderText="<%$ Resources: Text, DoseSeqShort %>">
                                    <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#f08000" />
                                    <ItemStyle Width="80px" VerticalAlign="Top" Font-Size="Medium"/>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCDose" runat="server" Text='<%# Bind("AvailableItemDesc") %>'></asp:Label>
                                        <asp:Label ID="lblCDoseChi" runat="server" Text='<%# Bind("AvailableItemDescChi") %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Provider"
                                    HeaderText="<%$ Resources: Text, InformationProvider %>">
                                    <HeaderStyle VerticalAlign="Top" Font-Size="Medium" BackColor="#f08000" />
                                    <ItemStyle Width="200px" VerticalAlign="Top" Font-Size="Medium"/>
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
                    <td style="padding-bottom: 10px"></td>                 
                </tr>
                </asp:Panel>
                <tr>
                    <td colspan="2" style="vertical-align: top">
                        <div class="headingText">
                            <asp:Label ID="lblTTransactionHeading" runat="server" Text="<%$ Resources:Text, TransactionInformation %>"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="vertical-align: top">
                        <table style="padding: 0; border-collapse: separate; border-spacing: 2px; width: 1150px">
                            <tr>
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblTTransactionNoText" runat="server" Text="<%$ Resources:Text, TransactionNo %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTTransactionNo" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblTTransactionNoTime" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trTConfirmTime" runat="server">
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTConfirmTimeText" runat="server" Text="<%$ Resources:Text, ConfirmedTime %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTConfirmTime" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trTTransactionTime" runat="server">
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTTransactionTimeText" runat="server" Text="<%$ Resources:Text, TransactionTime %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTTransactionTime" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTScheme" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblTSchemeStatus" runat="server" CssClass="tableText" ForeColor="red" Visible="false"></asp:Label>
                                    <asp:HiddenField ID="hfTScheme" runat="server" />
                                </td>
                            </tr>
                            <tr id="trTransactionStatus" runat="server">
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTTransactionStatusText" runat="server" Text="<%$ Resources:Text, TransactionStatus %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTTransactionStatus" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblReimbursementMethod" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblTAuthorizedStatus" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblInvalidationStatus" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTServiceDateText" runat="server" Text="<%$ Resources:Text, ServiceDate %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTServiceDate" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Image ID="imgTServiceDate" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                        ToolTip="<%$ Resources: Text, ServiceDateLaterThanDateOfDeath %>" /></td>
                            </tr>
                            <tr id="trServiceProvider" runat="server">
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTServiceProviderText" runat="server" Text="<%$ Resources:Text, ServiceProvider %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTServiceProvider" runat="server" CssClass="tableText"></asp:Label><asp:Label
                                        ID="lblTServiceProviderID" runat="server" Text="lblTServiceProviderID" CssClass="tableText"
                                        Visible="False"></asp:Label><asp:LinkButton ID="lnkTServiceProviderID" runat="server"
                                            CssClass="tableText" ForeColor="Blue" Visible="False">lnkTServiceProviderID</asp:LinkButton><asp:Label
                                                ID="lblCloseBracket" runat="server" Text=")" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblTServiceProviderStatus" runat="server" CssClass="tableText" ForeColor="red"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTPracticeText" runat="server" Text="<%$ Resources:Text, Practice %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTPractice" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblTPracticeStatus" runat="server" CssClass="tableText" ForeColor="red"></asp:Label></td>
                            </tr>
                            <tr id="trBankAccountNo" runat="Server">
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTBankAccountNoText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTBankAccountNo" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trServiceType" runat="Server">
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTServiceTypeText" runat="server" Text="<%$ Resources:Text, ServiceType %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTServiceType" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trReadOnlyEHSClaim" runat="Server">
                                <td colspan="2" style="vertical-align: top">
                                    <uc2:ucReadOnlyEHSClaim ID="udcReadOnlyEHSClaim" runat="server"></uc2:ucReadOnlyEHSClaim>
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="2" cellspacing="0">
                            <tr id="trContactNo" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblContactText" runat="server" Text="<%$ Resources:Text, ContactNo2 %>"></asp:Label>
                                </td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblContact" runat="server" CssClass="tableText"></asp:Label>&nbsp;
                                    <asp:Label ID="lblContactNoNotAbleSMS" runat="server" CssClass="tableText" Text="<%$ Resources:Text, NotAbleToReceiveSMS%>" style="color:red!important" />
                                </td>
                            </tr>
                            <tr id="trRemark" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblRemarkText" runat="server" Text="<%$ Resources:Text, Remarks %>"></asp:Label>
                                </td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblRemark" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trJoinEHRSS" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblJoinEHRSSText" runat="server" Text="<%$ Resources:Text, JoinEHRSS %>"></asp:Label>
                                </td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblJoinEHRSS" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trNonLocalRecoveredHistory" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblNonLocalRecoveredHistoryText" runat="server" Text="<%$ Resources:Text, NonLocalRecoveredHistory %>"></asp:Label>
                                </td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblNonLocalRecoveredHistory" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trTMeansOfInput" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblTMeansOfInputText" runat="server" Text="<%$ Resources: Text, MeansOfInput %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTMeansOfInput" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trTHAVaccination" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblTHAVaccinationText" runat="server" Text="<%$ Resources:Text, VaccinationRecordChecking %>" /></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTHAVaccination" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr id="trOCSSSCheckingResult" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblOCSSSCheckingResultText" runat="server" Text="<%$ Resources:Text, OCSSSCheckingResult %>" /></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblOCSSSCheckingResult" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr id="trTFirstAuthorization" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblTFirstAuthorizationText" runat="server" Text="<%$ Resources:Text, FirstAuthorizationBy %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTFirstAuthorizationBy" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblTFirstAuthorizationTime" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trTSecondAuthorization" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblTSecondAuthorizationText" runat="server" Text="<%$ Resources:Text, SecondAuthorizationBy %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTSecondAuthorizationBy" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblTSecondAuthorizationTime" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trTPaymentFileSubmitTime" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblTPaymentFileSubmitTimeText" runat="server" Text="<%$ Resources:Text, PaymentFileSubmittedBy %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTPaymentFileSubmitTime" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trTBankPaymentDay" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblTBankPaymentDayText" runat="server" Text="<%$ Resources:Text, BankPaymentDay %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTBankPaymentDay" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trTSuspendBy" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblTSuspendByText" runat="server" Text="<%$ Resources:Text, SuspendBy %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTSuspendBy" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trTSuspendReason" runat="server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblTSuspendReasonText" runat="server" Text="<%$ Resources:Text, SuspendReason %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTSuspendReason" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trVoidTransactionNo" runat="Server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblTVoidTransactionNoText" runat="server" Text="<%$ Resources:Text, VoidTranID %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTVoidTransactionNo" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblTVoidTransactionTime" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trVoidBy" runat="Server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblTVoidByText" runat="server" Text="<%$ Resources:Text, VoidBy %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTVoidBy" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trVoidReason" runat="Server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblTVoidReasonText" runat="server" Text="<%$ Resources:Text, VoidReason %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblTVoidReason" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trCreationReason" runat="Server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblCreationReasonText" runat="server" Text="<%$ Resources:Text, CreationReason %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblCreationReason" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trPaymentMethod" runat="Server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblPaymentMethodText" runat="server" Text="<%$ Resources:Text, PaymentSettlement %>"></asp:Label></td>
                                <td style="vertical-align: top">
                                    <asp:Label ID="lblPaymentMethod" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr id="trCreateBy" runat="Server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblCreateByText" runat="server" Text="<%$ Resources:Text, CreateBy %>"></asp:Label>
                                </td>
                                <td style="vertical-align: top; width: 670px">
                                    <asp:Label ID="lblCreateBy" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblCreateDtm" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trApprovalBy" runat="Server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblApprovalByText" runat="server" Text="<%$ Resources:Text, ApprovedBy %>"></asp:Label></td>
                                <td style="vertical-align: top; width: 670px">
                                    <asp:Label ID="lblApprovalBy" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblApprovalDtm" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trRejectBy" runat="Server">
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblRejectByText" runat="server" Text="<%$ Resources:Text, RejectedBy %>"></asp:Label></td>
                                <td style="vertical-align: top; width: 670px">
                                    <asp:Label ID="lblRejectBy" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblRejectDtm" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Panel ID="pnlWarning" runat="server" BorderStyle="NotSet" BorderWidth="1" BorderColor="orange"
    Width="900px">
    <table cellspacing="2">
        <tr>
            <td>
                <asp:Image ID="imgWarning" runat="server" ImageUrl="~/Images/others/warning.png"
                    AlternateText="<%$ Resources:Text, Warning %>" ImageAlign="AbsMiddle" />
                <asp:Label ID="lblWarning" runat="server" Text="<%$ Resources:Text, Warning %>" Font-Bold="true"></asp:Label>
                <asp:Panel ID="pnlWarningMessageList" runat="server" CssClass="tableText">
                </asp:Panel>
                <br />
                <table cellpadding="2" cellspacing="0">
                    <tr>
                        <td style="width: 20px"></td>
                        <td style="vertical-align: top; width: 185px">
                            <asp:Label ID="lblOverrideReasonText" runat="server" Text="<%$ Resources:Text, OverrideReason %>"></asp:Label></td>
                        <td style="vertical-align: top;">
                            <asp:Label ID="lblOverrideReason" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<table style="width: 100%" cellpadding="0" cellspacing="0">
    <tr>
        <td style="width: 1%"></td>
        <td>
            <table style="width: 100%" cellpadding="1" cellspacing="0">
                <tr>
                    <td>
                        <table cellpadding="2" cellspacing="0">
                            <tr>
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblInvalidTimeText" runat="server" Text="<%$ Resources:Text, InvalidationTime %>"></asp:Label></td>
                                <td style="vertical-align: top; width: 670px">
                                    <asp:Label ID="lblInvalidBy" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblInvalidTime" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top; width: 200px">
                                    <asp:Label ID="lblInvalidReasonText" runat="server" Text="<%$ Resources:Text, InvalidationReason %>"></asp:Label></td>
                                <td style="vertical-align: top; width: 670px">
                                    <asp:Label ID="lblInvalidReason" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
