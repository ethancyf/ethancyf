Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.HCVUUser
Imports Common.Component.ReasonForVisit
Imports Common.Component.Scheme
Imports Common.Format
Imports Common.Component.ClaimCategory
Imports Common.Component.StaticData
Imports Common.ComObject
Imports Common.WebService.Interface
Imports HCVU.BLL
Imports Common.Component.COVID19
Imports Common.Component.SchemeDetails

Partial Public Class ClaimTransDetail
    Inherits BaseControlWithGridView
    'Inherits System.Web.UI.UserControl

#Region "Fields"
    Private Const SESS_ClaimCOVID19_VaccineRecord As String = "ClaimTransDetail_ClaimCOVID19_VaccineRecord"  ' CRE20-0022 (Immu record) [Martin]

    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtReasonForVisitBLL As New ReasonForVisitBLL
    Private udtReimbursementBLL As New ReimbursementBLL
    Private udtSchemeClaimBLL As New SchemeClaimBLL
    Private udtEHSClaimBLL As New EHSClaimBLL  ' CRE20-0022 (Immu record) [Martin]
    Private udtSessionHandlerBLL As New BLL.SessionHandlerBLL

    Private VS_EHSTRANSACTION As String = "VS_CLAIMCONTROL_EHSTRANSACTION"
    Private VS_SUSPENDHISTORY As String = "VS_CLAIMCONTROL_SUSPENDHISTORY"
    'Private VS_POPUPWARNINGMSG As String = "VS_CLAIMCONTROL_POPUPWARNINGMSG"

    Private _blnEnableVaccinationRecordChecking As Boolean = True

    Private _blnEnableToShowHKICSymbol As Boolean = False
    Private _blnShowOCSSSCheckingResult As Boolean = True

    Private _blnEnableToShowAccountIDAsBtn As Boolean = True ' Default Show Btn
    Private _blnEnableToShowDateOfDeathBtn As Boolean = True ' Default Show Btn

    Private _blnEnableToShowBankAccountNo As Boolean = True
    Private _blnEnableToShowReimbursePayment As Boolean = True ' First Authorization / Second Authorization / Payment File Submission Time / Bank Payment Day
    Private _blnEnableToShowSuspendBy As Boolean = True
    Private _blnEnableToShowCreateBy As Boolean = True
    Private _blnEnableToShowCreationReason As Boolean = True
    Private _blnEnableToShowPaymentMethod As Boolean = True
    Private _blnEnableToShowApprovalBy As Boolean = True
    Private _blnEnableToShowRejectBy As Boolean = True
    Private _blnEnableToShowVoidTransactionNo As Boolean = True
    Private _blnEnableToShowWarning As Boolean = True

    Private _strClaimTransDetailFunctionCode As String = String.Empty ' CRE20-0022 (Immu record) [Martin]

    Private _blnShowContactNoNotAbleToSMS As Boolean = False

#End Region

#Region "Properties"
    Public Property ShowHKICSymbol() As Boolean
        Get
            Return _blnEnableToShowHKICSymbol
        End Get
        Set(value As Boolean)
            _blnEnableToShowHKICSymbol = value
        End Set
    End Property

    Public Property ShowOCSSSCheckingResult() As Boolean
        Get
            Return _blnShowOCSSSCheckingResult
        End Get
        Set(value As Boolean)
            _blnShowOCSSSCheckingResult = value
        End Set
    End Property

    Public Property EnableVaccinationRecordChecking() As Boolean
        Get
            Return _blnEnableVaccinationRecordChecking
        End Get
        Set(value As Boolean)
            _blnEnableVaccinationRecordChecking = value
        End Set
    End Property

    Public WriteOnly Property ShowAccountIDAsBtn() As Boolean
        Set(ByVal value As Boolean)
            _blnEnableToShowAccountIDAsBtn = value
        End Set
    End Property

    Public WriteOnly Property ShowDateOfDeathBtn() As Boolean
        Set(ByVal value As Boolean)
            _blnEnableToShowDateOfDeathBtn = value
        End Set

    End Property

    ' CRE20-0022 (Immu record) [Start][Martin]
    Public WriteOnly Property FunctionCode() As String
        Set(ByVal value As String)
            _strClaimTransDetailFunctionCode = value
        End Set

    End Property

    Public WriteOnly Property ShowContactNoNotAbleToSMS() As Boolean
        Set(ByVal value As Boolean)
            _blnShowContactNoNotAbleToSMS = value
        End Set

    End Property
    ' CRE20-0022 (Immu record) [End][Martin]
#End Region

#Region "Function"

    Public Sub LoadTranInfo(ByVal udtEHSTransaction As EHSTransactionModel, ByVal dtSuspendHistory As DataTable, Optional ByVal blnShowSPStatus As Boolean = False, Optional ByVal blnShowPracticeStatus As Boolean = False, Optional ByVal blnShowAccountStatus As Boolean = False)

        ViewState(VS_EHSTRANSACTION) = udtEHSTransaction
        ViewState(VS_SUSPENDHISTORY) = dtSuspendHistory
        'ViewState(VS_POPUPWARNINGMSG) = blnPopupWarningMsg

        ' Init
        lblTServiceProviderStatus.Text = String.Empty
        lblTPracticeStatus.Text = String.Empty
        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        lblTSchemeStatus.Visible = False
        lblTSchemeStatus.Text = String.Empty
        'CRE15-004 (TIV and QIV) [End][Chris YIM]

        udcErrorMessage.Clear()
        udcInfoMessageBox.Clear()

        ' =====================================================
        ' --- eHealth Account Information ---
        ' =====================================================

        Dim udtEHSAccount As EHSAccountModel = udtEHSTransaction.EHSAcct
        udcReadOnlyDocumentType.Clear()
        udcReadOnlyDocumentType.Vertical = False
        udcReadOnlyDocumentType.Width = HCVU.ucReadOnlyDocumnetType.DEFAULT_WIDTH
        udcReadOnlyDocumentType.Width2 = HCVU.ucReadOnlyDocumnetType.DEFAULT_WIDTH2
        udcReadOnlyDocumentType.DocumentType = udtEHSTransaction.DocCode
        udcReadOnlyDocumentType.MaskIdentityNo = True

        If udtEHSTransaction.Invalidation = EHSTransactionModel.InvalidationStatusClass.Invalidated Then
            '---> Invalidated
            udcReadOnlyDocumentType.IsInvalidAccount = True
            udcReadOnlyDocumentType.Vertical = True
            udcReadOnlyDocumentType.Width2 = 560
            udcReadOnlyDocumentType.EHSPersonalInformation = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSTransaction.DocCode)
            udcReadOnlyDocumentType.OriginalAccType = udtEHSAccount.AccountSourceString

            'Remind that the original account is retrieved instead of invalid account
            '--> Reason : personal data stored in invalidpersonalinformation may not same as that of personalinformation / specialpersonalinformation

            ' INT20-0014 (Fix unable to open invalidated PPP transaction) [Start][Winnie]
            ' ---------------------------------------------------------------------------
            Select Case udtEHSAccount.AccountSourceString
                Case EHSAccountModel.SysAccountSourceClass.ValidateAccount,
                    EHSAccountModel.SysAccountSourceClass.TemporaryAccount,
                    EHSAccountModel.SysAccountSourceClass.SpecialAccount
                    udcReadOnlyDocumentType.OriginalAccID = udtEHSAccount.VoucherAccID
            End Select

            'If udtEHSAccount.AccountSourceString = EHSAccountModel.SysAccountSourceClass.ValidateAccount Then
            '    udcReadOnlyDocumentType.OriginalAccID = udtEHSAccount.VoucherAccID
            'ElseIf udtEHSAccount.AccountSourceString = EHSAccountModel.SysAccountSourceClass.SpecialAccount Then
            '    udcReadOnlyDocumentType.OriginalAccID = udtEHSAccount.VoucherAccID
            'End If
            ' INT20-0014 (Fix unable to open invalidated PPP transaction) [End][Winnie]

            'Change titles
            lblInvalidTimeText.Text = Me.GetGlobalResourceObject("Text", "InvalidatedBy")
            lblInvalidReasonText.Text = Me.GetGlobalResourceObject("Text", "InvalidationReason")

            'Invalid Reason
            lblInvalidReasonText.Visible = True
            lblInvalidReason.Visible = True

            Dim udtStaticDataBLL As New StaticDataBLL()
            Dim udtStaticDataModel As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TransactionInvalidationType", udtEHSTransaction.TransactionInvalidation.InvalidationType)
            If udtEHSTransaction.TransactionInvalidation.InvalidationRemark.Trim <> "" Then
                lblInvalidReason.Text = udtStaticDataModel.DataValue.ToString().Trim() + " (" + udtEHSTransaction.TransactionInvalidation.InvalidationRemark.Trim + ")"
            Else
                lblInvalidReason.Text = udtStaticDataModel.DataValue.ToString().Trim()
            End If
            'Invalidation Time
            lblInvalidTimeText.Visible = True
            lblInvalidTime.Visible = True
            lblInvalidBy.Visible = True

            lblInvalidTime.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.TransactionInvalidation.UpdateDate, String.Empty))
            lblInvalidBy.Text = udtEHSTransaction.TransactionInvalidation.UpdateBy

            udcReadOnlyDocumentType.ShowAccountID = False
        ElseIf udtEHSTransaction.Invalidation = EHSTransactionModel.InvalidationStatusClass.PendingInvalidation Then
            '---> Pending invalidation

            udcReadOnlyDocumentType.IsInvalidAccount = False
            udcReadOnlyDocumentType.EHSPersonalInformation = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSTransaction.DocCode)

            'Change titles
            lblInvalidTimeText.Text = Me.GetGlobalResourceObject("Text", "MarkPendingInvalidationBy")
            lblInvalidReasonText.Text = Me.GetGlobalResourceObject("Text", "InvalidationReason")

            'Invalidation Reason
            lblInvalidReasonText.Visible = True
            lblInvalidReason.Visible = True

            Dim udtStaticDataBLL As New StaticDataBLL()
            Dim udtStaticDataModel As StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TransactionInvalidationType", udtEHSTransaction.TransactionInvalidation.InvalidationType)
            If udtEHSTransaction.TransactionInvalidation.InvalidationRemark.Trim <> "" Then
                lblInvalidReason.Text = udtStaticDataModel.DataValue.ToString().Trim() + " (" + udtEHSTransaction.TransactionInvalidation.InvalidationRemark.Trim + ")"
            Else
                lblInvalidReason.Text = udtStaticDataModel.DataValue.ToString().Trim()
            End If
            'Invalidation Time
            lblInvalidTimeText.Visible = True
            lblInvalidTime.Visible = True
            lblInvalidBy.Visible = True

            lblInvalidTime.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.TransactionInvalidation.UpdateDate, String.Empty))
            lblInvalidBy.Text = udtEHSTransaction.TransactionInvalidation.UpdateBy

            udcReadOnlyDocumentType.ShowAccountID = True
        Else
            udcReadOnlyDocumentType.IsInvalidAccount = False
            udcReadOnlyDocumentType.EHSPersonalInformation = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSTransaction.DocCode)

            lblInvalidReasonText.Visible = False
            lblInvalidReason.Visible = False
            lblInvalidTimeText.Visible = False
            lblInvalidTime.Visible = False
            lblInvalidBy.Visible = False

            udcReadOnlyDocumentType.ShowAccountID = True

        End If
        udcReadOnlyDocumentType.DocumentType = udtEHSTransaction.DocCode
        udcReadOnlyDocumentType.EHSAccountModel = udtEHSAccount

        If blnShowAccountStatus Then
            udcReadOnlyDocumentType.ShowAccountStatus = True
        End If

        udcReadOnlyDocumentType.ShowHKICSymbol = _blnEnableToShowHKICSymbol
        udcReadOnlyDocumentType.ShowAccountIDAsBtn = _blnEnableToShowAccountIDAsBtn
        udcReadOnlyDocumentType.ShowDateOfDeathBtn = _blnEnableToShowDateOfDeathBtn

        udcReadOnlyDocumentType.Build()

        ' =====================================================
        ' --- Transaction Information ---
        ' =====================================================

        'Wordings for display in COVID19
        If IsClaimCOVID19(udtEHSTransaction) Then
            lblTTransactionHeading.Text = Me.GetGlobalResourceObject("Text", "VaccineInfo")
            lblTServiceDateText.Text = Me.GetGlobalResourceObject("Text", "InjectionDate")
        End If

        'Override Reason Warning
        If _blnEnableToShowWarning = False OrElse IsNothing(udtEHSTransaction.WarningMessage) OrElse _strClaimTransDetailFunctionCode = FunctCode.FUNT010421 Then
            Me.pnlWarning.Visible = False
        Else
            If udtEHSTransaction.WarningMessage.RuleResults.Count = 0 Then
                Me.pnlWarning.Visible = False
            Else
                Me.pnlWarning.Visible = True

                Me.lblOverrideReason.Text = udtEHSTransaction.OverrideReason

                ' Fix display duplicate warning message 
                If pnlWarningMessageList.Controls.Count > 0 Then
                    pnlWarningMessageList.Controls.Clear()
                End If

                For Each udtWarning As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult In udtEHSTransaction.WarningMessage.RuleResults
                    Dim listStart As New Literal
                    Dim listEnd As New Literal
                    listStart.Text = "<ul style='margin-top:5px; margin-bottom:5px;'><li>"
                    listEnd.Text = "</li></ul>"

                    Dim strMessage As New Literal

                    pnlWarningMessageList.Controls.Add(listStart)
                    strMessage.Text = udtWarning.MessageDescription + " [" + udtWarning.ErrorMessage.FunctionCode + "-" + udtWarning.ErrorMessage.SeverityCode + "-" + udtWarning.ErrorMessage.MessageCode + "]"
                    pnlWarningMessageList.Controls.Add(strMessage)
                    pnlWarningMessageList.Controls.Add(listEnd)
                Next

            End If

        End If

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If IsNothing(udtEHSTransaction.TransactionID) OrElse udtEHSTransaction.TransactionID.Trim.Equals(String.Empty) Then
            Me.lblTTransactionNoText.Visible = False
            Me.lblTTransactionNo.Visible = False
            Me.lblTTransactionNoTime.Visible = False

        Else
            ' Transaction No.
            Me.lblTTransactionNoText.Visible = True
            Me.lblTTransactionNo.Visible = True
            Me.lblTTransactionNoTime.Visible = True

            lblTTransactionNo.Text = udtFormatter.formatSystemNumber(Trim(udtEHSTransaction.TransactionID))
            lblTTransactionNoTime.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm, String.Empty))
        End If
        ' CRE20-0022 (Immu record) [End][Chris YIM]


        ' Confirmed Time
        Dim strTransactionStatus As String = udtEHSTransaction.RecordStatus

        If udtEHSTransaction.ManualReimburse Then
            trTConfirmTime.Visible = False

        Else
            trTConfirmTime.Visible = True

            If udtEHSTransaction.ConfirmDate.HasValue Then
                lblTConfirmTime.Text = udtFormatter.formatDateTime(udtEHSTransaction.ConfirmDate, String.Empty)
            Else
                lblTConfirmTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

        End If


        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ' Transaction Time
        If IsClaimCOVID19(udtEHSTransaction) Then
            trTTransactionTime.Visible = True
            lblTTransactionTime.Text = udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm, String.Empty)
        Else
            trTTransactionTime.Visible = False
        End If

        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' Scheme
        lblTScheme.Text = GetDisplayCodeFromSchemeCode(udtEHSTransaction.SchemeCode)
        hfTScheme.Value = udtEHSTransaction.SchemeCode

        Dim udtTranTAF As TransactionAdditionalFieldModel = Nothing

        If Not IsNothing(udtEHSTransaction.TransactionAdditionFields) Then
            udtTranTAF = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ClinicType)
        End If

        If Not IsNothing(udtTranTAF) AndAlso udtTranTAF.AdditionalFieldValueCode = "N" Then
            lblTScheme.Text += String.Format(" ({0})", Me.GetGlobalResourceObject("Text", "NonClinic"))
        End If

        ' Transaction Status
        If IsNothing(udtEHSTransaction.TransactionID) OrElse udtEHSTransaction.TransactionID.Trim.Equals(String.Empty) Then
            Me.lblTTransactionStatusText.Visible = False
            Me.lblTTransactionStatus.Visible = False
        Else
            Me.lblTTransactionStatusText.Visible = True
            Me.lblTTransactionStatus.Visible = True
            Status.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, strTransactionStatus, lblTTransactionStatus.Text, String.Empty)
        End If

        'Invalidation Status
        If udtEHSTransaction.Invalidation = EHSTransactionModel.InvalidationStatusClass.Invalidated Or udtEHSTransaction.Invalidation = EHSTransactionModel.InvalidationStatusClass.PendingInvalidation Then
            Dim strInvalidationStatus As String = ""
            Status.GetDescriptionFromDBCode(EHSTransactionModel.InvalidationStatusClass.ClassCode, udtEHSTransaction.Invalidation, strInvalidationStatus, String.Empty)
            'lblTTransactionStatus.Text = lblTTransactionStatus.Text.Trim + " (" + strRemarkStatus + ")"
            lblInvalidationStatus.Visible = True
            lblInvalidationStatus.ForeColor = Drawing.Color.Blue
            lblInvalidationStatus.Text = " (" + strInvalidationStatus + ")"
        Else
            lblInvalidationStatus.Visible = False
        End If

        ' Service Date
        lblTServiceDate.Text = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate)

        imgTServiceDate.Visible = False

        If udtEHSAccount.Deceased Then
            Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSTransaction.DocCode)

            If udtPersonalInfo.IsDeceasedAsAt(EHSAccountModel.EHSPersonalInformationModel.DODCalMethodClass.FIRSTDAYOFMONTHYEAR, udtEHSTransaction.ServiceDate) Then
                imgTServiceDate.Visible = True
            End If
        End If

        ' Service Provider
        lblTServiceProvider.Text = String.Format("{0} (", udtEHSTransaction.ServiceProviderName)

        If blnShowSPStatus Then
            Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL

            Dim udtSP As ServiceProvider.ServiceProviderModel

            udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(udtEHSTransaction.ServiceProviderID, New Common.DataAccess.Database)

            If Not udtSP.RecordStatus.Trim.Equals(ServiceProviderStatus.Active) Then
                Dim strSPStatus As String = String.Empty
                Status.GetDescriptionFromDBCode(ServiceProviderStatus.ClassCode, udtSP.RecordStatus, strSPStatus, String.Empty)
                Me.lblTServiceProviderStatus.Text = " (" + strSPStatus + ")"
            End If
        End If

        ' Service Provider ID (Access Right)
        If (New GeneralFunction).CheckTurnOnHyperlinkToEHealthAccount = GeneralFunction.EnumTurnOnStatus.Yes AndAlso IsAllow(FUNCTION_CODE_SERVICE_PROVIDER_ENQUIRY) Then
            lnkTServiceProviderID.Text = udtEHSTransaction.ServiceProviderID
            lnkTServiceProviderID.CommandArgument = GetURL(FUNCTION_CODE_SERVICE_PROVIDER_ENQUIRY)

            lnkTServiceProviderID.Visible = True
            lblTServiceProviderID.Visible = False

        Else
            lblTServiceProviderID.Text = udtEHSTransaction.ServiceProviderID

            lblTServiceProviderID.Visible = True
            lnkTServiceProviderID.Visible = False

        End If

        If blnShowPracticeStatus Then
            Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfo.PracticeSchemeInfoBLL
            Dim udtPracticeSchemeInfoModelCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPIDPracticeDisplaySeq(udtEHSTransaction.ServiceProviderID, udtEHSTransaction.PracticeID, New Common.DataAccess.Database)
            Dim udtResPracticeSchemeInfoModelCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtPracticeSchemeInfoModelCollection.FilterByPracticeScheme(udtEHSTransaction.PracticeID, udtEHSTransaction.SchemeCode)

            If Not udtResPracticeSchemeInfoModelCollection Is Nothing Then
                For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtResPracticeSchemeInfoModelCollection.Values
                    'If one of practice scheme is delisted, the label "delisted" will be shown.
                    If udtPracticeSchemeInfoModel.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary Or _
                        udtPracticeSchemeInfoModel.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary Then
                        Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, PracticeSchemeInfoStatus.Delisted, Me.lblTSchemeStatus.Text, String.Empty)
                        lblTSchemeStatus.Text = "(" + lblTSchemeStatus.Text + ")"
                        lblTSchemeStatus.Visible = True
                    End If
                Next
            End If
        End If

        ' Practice
        lblTPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeName, udtEHSTransaction.PracticeID.ToString.Trim)
        If blnShowPracticeStatus Then
            Dim udtPracticeBLL As New Practice.PracticeBLL
            ' Get Practice Information
            Dim dtPractice As DataTable = udtPracticeBLL.getRawAllPracticeBankAcct(udtEHSTransaction.ServiceProviderID)

            Practice.PracticeBLL.ConcatePracticeDisplayColumn(dtPractice, Practice.PracticeBLL.PracticeDisplayType.Practice)

            Dim udtPracticeDisplayList As Practice.PracticeBLL.PracticeDisplayModelCollection
            Dim udtPracticeDisplay As Practice.PracticeBLL.PracticeDisplayModel

            udtPracticeDisplayList = udtPracticeBLL.convertPractice(dtPractice)
            udtPracticeDisplay = udtPracticeDisplayList.Filter(udtEHSTransaction.PracticeID)

            If Not udtPracticeDisplay.PracticeStatus.Trim.Equals(PracticeStatus.Active) Then
                Dim strPracticeStatus As String = String.Empty
                Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, udtPracticeDisplay.PracticeStatus, strPracticeStatus, String.Empty)
                Me.lblTPracticeStatus.Text = " (" + strPracticeStatus + ")"
            End If
        End If

        ' Bank Account No.
        If _blnEnableToShowBankAccountNo Then
            lblTBankAccountNo.Text = udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)
            DisplayBankAccountNo(True)
        Else
            DisplayBankAccountNo(False)
        End If

        ' Service Type
        lblTServiceType.Text = udtEHSTransaction.ServiceTypeDesc

        ' Scheme-related fields
        udcReadOnlyEHSClaim.Clear()

        ' Read-only EHS Claim Control
        DisplayReadOnlyEHSClaim(True)

        ' Contact No.
        DisplayContactNo(False)

        ' Remarks
        DisplayRemarks(False)

        ' Join EHRSS
        DisplayJoinEHRSS(False)

        ' Non-Local Recovery History
        DisplayNonLocalRecoveredHistory(False)

        'Vacc Record
        panVaccinationRecord.Visible = False


        Select Case udtSchemeClaimBLL.ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode)
            Case SchemeClaimModel.EnumControlType.VOUCHER
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204
                udcReadOnlyEHSClaim.BuildHCVS()

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204
                udcReadOnlyEHSClaim.BuildHCVSChina()

            Case SchemeClaimModel.EnumControlType.CIVSS
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.BuildCIVSS()

            Case SchemeClaimModel.EnumControlType.EVSS
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.BuildEVSS()

            Case SchemeClaimModel.EnumControlType.HSIVSS
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204
                udcReadOnlyEHSClaim.BuildHSIVSS()

            Case SchemeClaimModel.EnumControlType.RVP
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204

                If IsClaimCOVID19(udtEHSTransaction) Then
                    If _strClaimTransDetailFunctionCode = FunctCode.FUNT010421 Then
                        'Reprint not show subsidize amount
                        udcReadOnlyEHSClaim.BuildRVPCOVID19(False)
                        DisplayCOVID19VaccinationRecord(udtEHSTransaction, udtEHSAccount)
                    Else
                        udcReadOnlyEHSClaim.BuildRVPCOVID19(True)
                    End If

                    'Contact No.
                    If (udtEHSTransaction.TransactionAdditionFields.RecipientType IsNot Nothing AndAlso _
                        udtEHSTransaction.TransactionAdditionFields.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                        udtEHSTransaction.TransactionAdditionFields.RecipientType <> String.Empty) _
                        OrElse _
                        (udtEHSTransaction.TransactionAdditionFields.OutreachType IsNot Nothing AndAlso _
                        udtEHSTransaction.TransactionAdditionFields.OutreachType = TYPE_OF_OUTREACH.OTHER) Then

                        DisplayContactNo(True)
                        FillContactNo(udtEHSTransaction)
                    Else
                        DisplayContactNo(False)
                    End If

                    DisplayJoinEHRSS(False)

                    'Remark
                    DisplayRemarks(True)
                    FillRemarks(udtEHSTransaction)

                    'Join EHRSS
                    If (udtEHSTransaction.TransactionAdditionFields.RecipientType IsNot Nothing AndAlso _
                        udtEHSTransaction.TransactionAdditionFields.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                        udtEHSTransaction.TransactionAdditionFields.RecipientType <> String.Empty) _
                        OrElse _
                        (udtEHSTransaction.TransactionAdditionFields.OutreachType IsNot Nothing AndAlso _
                        udtEHSTransaction.TransactionAdditionFields.OutreachType = TYPE_OF_OUTREACH.OTHER) Then

                        If COVID19.COVID19BLL.DisplayJoinEHRSSForReadOnly(udtEHSAccount, udtEHSTransaction.DocCode) Then
                            DisplayJoinEHRSS(True)
                            FillJoinEHRSS(udtEHSTransaction)
                        Else
                            DisplayJoinEHRSS(False)
                        End If

                    Else
                        DisplayJoinEHRSS(False)
                    End If

                    ' Non-Local Recovered History
                    DisplayNonLocalRecoveredHistory(True)
                    FillNonLocalRecoveredHistory(udtEHSTransaction)

                Else
                    udcReadOnlyEHSClaim.BuildRVP()

                End If

            Case SchemeClaimModel.EnumControlType.EHAPP
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.BuildEHAPP()

            Case SchemeClaimModel.EnumControlType.PIDVSS
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204
                udcReadOnlyEHSClaim.BuildPIDVSS()

            Case SchemeClaimModel.EnumControlType.VSS
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204

                If IsClaimCOVID19(udtEHSTransaction) Then
                    If _strClaimTransDetailFunctionCode = FunctCode.FUNT010421 Then
                        'Reprint not show subsidize amount
                        udcReadOnlyEHSClaim.BuildVSSCOVID19(False)
                        DisplayCOVID19VaccinationRecord(udtEHSTransaction, udtEHSAccount)
                    Else
                        udcReadOnlyEHSClaim.BuildVSSCOVID19(True)
                    End If

                    'Contact No.
                    DisplayContactNo(True)
                    FillContactNo(udtEHSTransaction)

                    'Remark
                    DisplayRemarks(True)
                    FillRemarks(udtEHSTransaction)

                    'Join EHRSS
                    If COVID19.COVID19BLL.DisplayJoinEHRSSForReadOnly(udtEHSAccount, udtEHSTransaction.DocCode) Then
                        DisplayJoinEHRSS(True)
                        FillJoinEHRSS(udtEHSTransaction)
                    Else
                        DisplayJoinEHRSS(False)
                    End If

                    'Non-Local Recovered History
                    DisplayNonLocalRecoveredHistory(True)
                    FillNonLocalRecoveredHistory(udtEHSTransaction)

                Else
                    udcReadOnlyEHSClaim.ShowContactNoNotAbleToSMS = _blnShowContactNoNotAbleToSMS
                    udcReadOnlyEHSClaim.BuildVSS()

                    DisplayContactNo(False)
                    DisplayRemarks(False)
                    DisplayJoinEHRSS(False)

                End If

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204
                udcReadOnlyEHSClaim.BuildENHVSSO()

            Case SchemeClaimModel.EnumControlType.PPP
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204
                udcReadOnlyEHSClaim.BuildPPP()

                ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.SSSCMC
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204
                udcReadOnlyEHSClaim.BuildSSSCMC()
                ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

                ' CRE20-0XX (Immu record) [Start][Raiman Chong]
                ' ---------------------------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.COVID19
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204
                udcReadOnlyEHSClaim.BuildCOVID19()

                If IsClaimCOVID19(udtEHSTransaction) Then
                    If _strClaimTransDetailFunctionCode = FunctCode.FUNT010421 Then
                        DisplayCOVID19VaccinationRecord(udtEHSTransaction, udtEHSAccount)
                    End If

                    'Contact No.
                    DisplayContactNo(False)

                    'Remark
                    DisplayRemarks(True)
                    FillRemarks(udtEHSTransaction)

                    'Join EHRSS
                    If COVID19.COVID19BLL.DisplayJoinEHRSSForReadOnly(udtEHSAccount, udtEHSTransaction.DocCode) Then
                        DisplayJoinEHRSS(True)
                        FillJoinEHRSS(udtEHSTransaction)
                    Else
                        DisplayJoinEHRSS(False)
                    End If

                    'Non-Local Recovered History
                    DisplayNonLocalRecoveredHistory(True)
                    FillNonLocalRecoveredHistory(udtEHSTransaction)

                Else
                    DisplayContactNo(False)
                    DisplayRemarks(False)
                    DisplayJoinEHRSS(False)
                End If
                ' CRE20-0XX (Immu record) [End][Raiman Chong]

            Case SchemeClaimModel.EnumControlType.COVID19RVP
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204
                udcReadOnlyEHSClaim.BuildCOVID19RVP()

                If IsClaimCOVID19(udtEHSTransaction) Then
                    If _strClaimTransDetailFunctionCode = FunctCode.FUNT010421 Then
                        DisplayCOVID19VaccinationRecord(udtEHSTransaction, udtEHSAccount)
                    End If

                    'Contact No.
                    If udtEHSTransaction.TransactionAdditionFields.RecipientType IsNot Nothing AndAlso _
                        udtEHSTransaction.TransactionAdditionFields.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                        udtEHSTransaction.TransactionAdditionFields.RecipientType <> String.Empty Then

                        DisplayContactNo(True)
                        FillContactNo(udtEHSTransaction)
                    Else
                        DisplayContactNo(False)
                    End If

                    'Remark
                    DisplayRemarks(True)
                    FillRemarks(udtEHSTransaction)

                    'Join EHRSS
                    If udtEHSTransaction.TransactionAdditionFields.RecipientType IsNot Nothing AndAlso _
                        udtEHSTransaction.TransactionAdditionFields.RecipientType <> RECIPIENT_TYPE.RESIDENT AndAlso _
                        udtEHSTransaction.TransactionAdditionFields.RecipientType <> String.Empty Then

                        If COVID19.COVID19BLL.DisplayJoinEHRSSForReadOnly(udtEHSAccount, udtEHSTransaction.DocCode) Then
                            DisplayJoinEHRSS(True)
                            FillJoinEHRSS(udtEHSTransaction)
                        Else
                            DisplayJoinEHRSS(False)
                        End If
                    Else
                        DisplayJoinEHRSS(False)
                    End If

                    'Non-Local Recovered History
                    DisplayNonLocalRecoveredHistory(True)
                    FillNonLocalRecoveredHistory(udtEHSTransaction)

                Else
                    DisplayContactNo(False)
                    DisplayRemarks(False)
                    DisplayJoinEHRSS(False)
                End If

            Case SchemeClaimModel.EnumControlType.COVID19OR
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204
                udcReadOnlyEHSClaim.BuildCOVID19OR()

                If IsClaimCOVID19(udtEHSTransaction) Then
                    If _strClaimTransDetailFunctionCode = FunctCode.FUNT010421 Then
                        DisplayCOVID19VaccinationRecord(udtEHSTransaction, udtEHSAccount)
                    End If

                    'Contact No.
                    DisplayContactNo(True)
                    FillContactNo(udtEHSTransaction)

                    'Remark
                    DisplayRemarks(True)
                    FillRemarks(udtEHSTransaction)

                    'Join EHRSS
                    If COVID19.COVID19BLL.DisplayJoinEHRSSForReadOnly(udtEHSAccount, udtEHSTransaction.DocCode) Then
                        DisplayJoinEHRSS(True)
                        FillJoinEHRSS(udtEHSTransaction)
                    Else
                        DisplayJoinEHRSS(False)
                    End If

                    'Non-Local Recovered History
                    DisplayNonLocalRecoveredHistory(True)
                    FillNonLocalRecoveredHistory(udtEHSTransaction)

                Else
                    DisplayContactNo(False)
                    DisplayRemarks(False)
                    DisplayJoinEHRSS(False)
                End If

        End Select

        ' Means of Input
        If (New GeneralFunction).CheckTurnOnMeansOfInput = GeneralFunction.EnumTurnOnStatus.Yes Then
            lblTMeansOfInputText.Visible = True
            lblTMeansOfInput.Visible = True
            DisplayTMeansOfInput(True)

            Status.GetDescriptionFromDBCode(EHSTransactionModel.MeansOfInputClass.ClassCode, IIf(udtEHSTransaction.CreateBySmartID, EHSTransactionModel.MeansOfInputClass.CardReader, EHSTransactionModel.MeansOfInputClass.Manual), lblTMeansOfInput.Text, String.Empty)

        Else
            lblTMeansOfInputText.Visible = False
            lblTMeansOfInput.Visible = False
            DisplayTMeansOfInput(False)
        End If

        ' HA Vaccination Status
        Dim strHAVaccinationRecord As String = udtEHSTransaction.HAVaccineRefStatus
        If strHAVaccinationRecord.Length > 3 Then strHAVaccinationRecord = strHAVaccinationRecord.Substring(0, 3)

        ' DH Vaccination Status
        Dim strDHVaccinationRecord As String = udtEHSTransaction.DHVaccineRefStatus
        If strDHVaccinationRecord.Length > 3 Then strDHVaccinationRecord = strDHVaccinationRecord.Substring(0, 3)

        If Not _blnEnableVaccinationRecordChecking Or _
            (strHAVaccinationRecord = HAVaccinationRecordStatus.OldRecord And _
                (strDHVaccinationRecord = HAVaccinationRecordStatus.OldRecord Or strDHVaccinationRecord = String.Empty)) Then
            lblTHAVaccinationText.Visible = False
            lblTHAVaccination.Visible = False
            DisplayTHAVaccination(False)

        Else
            lblTHAVaccinationText.Visible = True
            lblTHAVaccination.Visible = True
            DisplayTHAVaccination(True)

            Dim strHAStatus As String = String.Empty
            Dim strDHStatus As String = String.Empty
            Dim strHA As String = String.Empty
            Dim strDH As String = String.Empty

            If strHAVaccinationRecord <> HAVaccinationRecordStatus.OldRecord Then
                Status.GetDescriptionFromDBCode(HAVaccinationRecordStatus.ClassCode, strHAVaccinationRecord, strHAStatus, String.Empty)
                strHA = GetGlobalResourceObject("Text", "HospitalAuthority")
                lblTHAVaccination.Text = String.Format("{0} - {1}", strHA, strHAStatus)
            End If

            If strDHVaccinationRecord <> HAVaccinationRecordStatus.OldRecord And strDHVaccinationRecord <> String.Empty Then
                Status.GetDescriptionFromDBCode(HAVaccinationRecordStatus.ClassCode, strDHVaccinationRecord, strDHStatus, String.Empty)
                strDH = GetGlobalResourceObject("Text", "DepartmentOfHealth")
                lblTHAVaccination.Text = String.Format("{0} - {1}", strDH, strDHStatus)
            End If

            If strHAVaccinationRecord <> HAVaccinationRecordStatus.OldRecord And strDHVaccinationRecord <> HAVaccinationRecordStatus.OldRecord And strDHVaccinationRecord <> String.Empty Then
                lblTHAVaccination.Text = String.Format("{0} - {1}<br />{2} - {3}", strHA, strHAStatus, strDH, strDHStatus)
            End If

        End If

        ' OCSSS Checking Result
        If _blnShowOCSSSCheckingResult AndAlso _
            (udtEHSTransaction.DocCode = DocTypeCode.HKIC And Common.OCSSS.OCSSSServiceBLL.EnableHKICSymbolInput(udtEHSTransaction.SchemeCode)) Then
            If udtEHSTransaction.OCSSSRefStatus Is Nothing Then
                lblOCSSSCheckingResult.Text = HttpContext.GetGlobalResourceObject("Text", "NA")
            Else
                Select Case udtEHSTransaction.OCSSSRefStatus.Trim
                    Case "V"
                        lblOCSSSCheckingResult.Text = HttpContext.GetGlobalResourceObject("Text", "OCSSSResultValid")
                    Case "C"
                        lblOCSSSCheckingResult.Text = HttpContext.GetGlobalResourceObject("Text", "OCSSSResultConnectionFailed")
                    Case "N"
                        lblOCSSSCheckingResult.Text = HttpContext.GetGlobalResourceObject("Text", "NA")
                    Case Else
                        lblOCSSSCheckingResult.Text = HttpContext.GetGlobalResourceObject("Text", "NA")
                End Select
            End If

            DisplayOCSSSCheckingResult(True)
        Else
            DisplayOCSSSCheckingResult(False)
        End If
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' Reimbursement Status
        Me.lblTAuthorizedStatus.Visible = False

        If IsNothing(udtEHSTransaction.TransactionID) OrElse udtEHSTransaction.TransactionID.Trim.Equals(String.Empty) Then
            Me.lblTAuthorizedStatus.Visible = False

        Else
            If udtEHSTransaction.RecordStatus <> EHSTransactionModel.TransRecordStatusClass.Reimbursed Then
                Me.lblTAuthorizedStatus.Visible = True

                If udtEHSTransaction.AuthorisedStatus = String.Empty Then
                    lblTAuthorizedStatus.Text = String.Empty

                Else
                    Dim strAuthorisedStatus As String = udtEHSTransaction.AuthorisedStatus

                    If strAuthorisedStatus = ReimbursementStatus.Reimbursed Then strAuthorisedStatus = AuthorizedDisplayStatus.PaymentFileSubmitted



                    Status.GetDescriptionFromDBCode(ReimbursementStatus.ClassCode, strAuthorisedStatus, lblTAuthorizedStatus.Text, String.Empty)
                    lblTAuthorizedStatus.Text = "(" + lblTAuthorizedStatus.Text + ")"

                End If

            End If

        End If

        'Reimbursement Method
        If udtEHSTransaction.RecordStatus = EHSTransactionModel.TransRecordStatusClass.Reimbursed Then
            Me.lblReimbursementMethod.Visible = True

            Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
            If udtEHSTransaction.ManualReimburse Then
                Me.lblReimbursementMethod.Text = String.Format("({0})", udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ReimbursementMethod", "O").DataValue.Trim)
            Else
                Me.lblReimbursementMethod.Text = String.Format("({0})", udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ReimbursementMethod", "I").DataValue.Trim)
            End If
        Else
            Me.lblReimbursementMethod.Visible = False
        End If

        ' First Authorization / Second Authorization / Payment File Submission Time / Bank Payment Day
        lblTFirstAuthorizationText.Visible = False
        lblTFirstAuthorizationTime.Visible = False
        lblTFirstAuthorizationBy.Visible = False

        lblTSecondAuthorizationText.Visible = False
        lblTSecondAuthorizationTime.Visible = False
        lblTSecondAuthorizationBy.Visible = False

        lblTPaymentFileSubmitTimeText.Visible = False
        lblTPaymentFileSubmitTime.Visible = False
        lblTBankPaymentDayText.Visible = False
        lblTBankPaymentDay.Visible = False

        DisplayTFirstAuthorization(False)
        DisplayTSecondAuthorization(False)
        DisplayTPaymentFileSubmitTime(False)
        DisplayTBankPaymentDay(False)

        If _blnEnableToShowReimbursePayment Then
            ' CRE19-026 (HCVS hotline service) [End][Winnie]

            If Not udtEHSTransaction.ManualReimburse Then
                Select Case udtEHSTransaction.AuthorisedStatus
                    Case String.Empty, ReimbursementStatus.HoldForFirstAuthorisation
                        ' Nothing here

                    Case ReimbursementStatus.FirstAuthorised
                        lblTFirstAuthorizationTime.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.FirstAuthorisedDate, String.Empty))
                        lblTFirstAuthorizationBy.Text = udtEHSTransaction.FirstAuthorisedBy

                        lblTFirstAuthorizationText.Visible = True
                        lblTFirstAuthorizationTime.Visible = True
                        lblTFirstAuthorizationBy.Visible = True

                        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
                        DisplayTFirstAuthorization(True)
                        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

                    Case ReimbursementStatus.SecondAuthorised
                        lblTFirstAuthorizationTime.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.FirstAuthorisedDate, String.Empty))
                        lblTFirstAuthorizationBy.Text = udtEHSTransaction.FirstAuthorisedBy
                        lblTSecondAuthorizationTime.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.SecondAuthorisedDate, String.Empty))
                        lblTSecondAuthorizationBy.Text = udtEHSTransaction.SecondAuthorisedBy

                        lblTFirstAuthorizationText.Visible = True
                        lblTFirstAuthorizationTime.Visible = True
                        lblTFirstAuthorizationBy.Visible = True

                        lblTSecondAuthorizationText.Visible = True
                        lblTSecondAuthorizationTime.Visible = True
                        lblTSecondAuthorizationBy.Visible = True

                        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
                        DisplayTFirstAuthorization(True)
                        DisplayTSecondAuthorization(True)
                        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

                    Case ReimbursementStatus.Reimbursed
                        lblTFirstAuthorizationTime.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.FirstAuthorisedDate, String.Empty))
                        lblTFirstAuthorizationBy.Text = udtEHSTransaction.FirstAuthorisedBy
                        lblTSecondAuthorizationTime.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.SecondAuthorisedDate, String.Empty))
                        lblTSecondAuthorizationBy.Text = udtEHSTransaction.SecondAuthorisedBy

                        Dim dt As DataTable = udtReimbursementBLL.GetReimbursementDetailByTransactionID(udtEHSTransaction.TransactionID)
                        If dt.Rows.Count = 1 Then
                            Dim dr As DataRow = dt.Rows(0)
                            lblTPaymentFileSubmitTime.Text = String.Format("{0} ({1})", CStr(dr("PaymentFileSubmitBy")).Trim, udtFormatter.formatDateTime(dr("PaymentFileSubmitDtm"), String.Empty))
                            lblTBankPaymentDay.Text = udtFormatter.formatDisplayDate(dr("BankPaymentDtm"))

                        Else
                            lblTPaymentFileSubmitTime.Text = String.Empty
                            lblTBankPaymentDay.Text = String.Empty

                        End If

                        lblTFirstAuthorizationText.Visible = True
                        lblTFirstAuthorizationTime.Visible = True
                        lblTFirstAuthorizationBy.Visible = True

                        lblTSecondAuthorizationText.Visible = True
                        lblTSecondAuthorizationTime.Visible = True
                        lblTSecondAuthorizationBy.Visible = True

                        lblTPaymentFileSubmitTimeText.Visible = True
                        lblTPaymentFileSubmitTime.Visible = True
                        lblTBankPaymentDayText.Visible = True
                        lblTBankPaymentDay.Visible = True

                        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
                        DisplayTFirstAuthorization(True)
                        DisplayTSecondAuthorization(True)
                        DisplayTPaymentFileSubmitTime(True)
                        DisplayTBankPaymentDay(True)
                        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

                End Select
            End If
        End If

        ' Suspend Reason / Suspend By
        If _blnEnableToShowSuspendBy AndAlso udtEHSTransaction.RecordStatus = ClaimTransStatus.Suspended Then

            lblTSuspendReasonText.Visible = True
            lblTSuspendReason.Visible = True
            lblTSuspendByText.Visible = True
            lblTSuspendBy.Visible = True

            If dtSuspendHistory.Rows.Count > 0 Then
                Dim i As Integer = dtSuspendHistory.Rows.Count - 1
                lblTSuspendReason.Text = dtSuspendHistory.Rows(i)("Remark").ToString.Trim
                lblTSuspendBy.Text = dtSuspendHistory.Rows(i)("Update_By").ToString.Trim
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            DisplayTSuspendBy(True)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

        Else
            lblTSuspendReasonText.Visible = False
            lblTSuspendReason.Visible = False
            lblTSuspendByText.Visible = False
            lblTSuspendBy.Visible = False

            'CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            DisplayTSuspendBy(False)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

        End If

        'Creation Reason / Payment Method / Override Reason
        If udtEHSTransaction.ManualReimburse Then

            Dim udtStaticDataBLL As New StaticDataBLL

            ' Creation Reason
            If _blnEnableToShowCreationReason Then
                lblCreationReasonText.Visible = True
                lblCreationReason.Visible = True

                Dim strRemarks As String = String.Empty

                strRemarks = udtEHSTransaction.CreationRemarks.Trim

                If Not strRemarks.Equals(String.Empty) Then
                    strRemarks = "(" + strRemarks + ")"
                End If

                lblCreationReason.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ClaimCreationReason", udtEHSTransaction.CreationReason).DataValue + " " + strRemarks
                DisplayCreationReason(True)
            Else
                DisplayCreationReason(False)
            End If

            ' Payment Method
            If _blnEnableToShowPaymentMethod Then
                lblPaymentMethodText.Visible = True
                lblPaymentMethod.Visible = True

                Dim strPaymentRemarks As String = String.Empty
                strPaymentRemarks = udtEHSTransaction.PaymentRemarks.Trim

                If Not strPaymentRemarks.Equals(String.Empty) Then
                    strPaymentRemarks = "(" + strPaymentRemarks + ")"
                End If

                lblPaymentMethod.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ReimbursementPaymentMethod", udtEHSTransaction.PaymentMethod).DataValue + " " + strPaymentRemarks
                DisplayPaymentMethod(True)

            Else
                DisplayPaymentMethod(False)
            End If

            ' Create By
            If _blnEnableToShowCreateBy = False OrElse IsNothing(udtEHSTransaction.TransactionID) OrElse udtEHSTransaction.TransactionID.Trim.Equals(String.Empty) Then
                DisplayCreateBy(False)
            Else
                lblCreateBy.Text = udtEHSTransaction.CreateBy
                lblCreateDtm.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.CreateDate, String.Empty))

                DisplayCreateBy(True)
            End If

        Else
            lblCreationReasonText.Visible = False
            lblCreationReason.Visible = False

            lblPaymentMethodText.Visible = False
            lblPaymentMethod.Visible = False

            DisplayCreationReason(False)
            DisplayPaymentMethod(False)
            DisplayCreateBy(False)

        End If

        ' Approval By / Approval Dtm
        If _blnEnableToShowApprovalBy = False OrElse udtEHSTransaction.ApprovalBy.Trim.Equals(String.Empty) Then
            ' CRE19-026 (HCVS hotline service) [End][Winnie]
            Me.lblApprovalBy.Visible = False
            Me.lblApprovalByText.Visible = False

            Me.lblApprovalDtm.Visible = False

            DisplayApprovalBy(False)

        Else
            Me.lblApprovalBy.Visible = True
            Me.lblApprovalByText.Visible = True

            Me.lblApprovalDtm.Visible = True

            Me.lblApprovalBy.Text = udtEHSTransaction.ApprovalBy
            Me.lblApprovalDtm.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.ApprovalDate.Value, String.Empty))

            DisplayApprovalBy(True)

        End If

        ' Reject By / Reject Dtm
        If _blnEnableToShowRejectBy = False OrElse udtEHSTransaction.RejectBy.Trim.Equals(String.Empty) Then
            Me.lblRejectBy.Visible = False
            Me.lblRejectByText.Visible = False

            Me.lblRejectDtm.Visible = False

            DisplayRejectBy(False)

        Else
            Me.lblRejectBy.Visible = True
            Me.lblRejectByText.Visible = True

            Me.lblRejectDtm.Visible = True

            Me.lblRejectBy.Text = udtEHSTransaction.RejectBy
            Me.lblRejectDtm.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.RejectDate.Value, String.Empty))

            'CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            DisplayRejectBy(True)
            'CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]
        End If

        ' Void Transaction No. / Void Reason / Void By
        If _blnEnableToShowVoidTransactionNo AndAlso udtEHSTransaction.RecordStatus = ClaimTransStatus.Inactive Then
            lblTVoidTransactionNoText.Visible = True
            lblTVoidTransactionNo.Visible = True
            lblTVoidTransactionTime.Visible = True
            lblTVoidReasonText.Visible = True
            lblTVoidReason.Visible = True
            lblTVoidByText.Visible = True
            lblTVoidBy.Visible = True
            trVoidReason.Visible = True
            trVoidBy.Visible = True
            trVoidReason.Visible = True

            lblTVoidTransactionNo.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo)
            lblTVoidTransactionTime.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.VoidDate, String.Empty))
            lblTVoidReason.Text = udtEHSTransaction.VoidReason

            If udtEHSTransaction.VoidByDataEntry <> String.Empty Then
                lblTVoidBy.Text = String.Format("{0} ({1})", udtEHSTransaction.VoidByDataEntry, udtEHSTransaction.VoidUser.Trim)
            Else
                lblTVoidBy.Text = udtEHSTransaction.VoidUser
            End If

            DisplayVoidTransactionNo(True)

        Else
            lblTVoidTransactionNoText.Visible = False
            lblTVoidTransactionNo.Visible = False
            lblTVoidTransactionTime.Visible = False
            lblTVoidReasonText.Visible = False
            lblTVoidReason.Visible = False
            lblTVoidByText.Visible = False
            lblTVoidBy.Visible = False

            trVoidReason.Visible = False
            trVoidBy.Visible = False
            trVoidReason.Visible = False

            DisplayVoidTransactionNo(False)

        End If

        Select Case _strClaimTransDetailFunctionCode
            Case FunctCode.FUNT010421
                trTConfirmTime.Style.Add("display", "none")
                trTTransactionTime.Style.Remove("display")
                trTransactionStatus.Style.Add("display", "none")
                trServiceProvider.Style.Add("display", "none")
                trBankAccountNo.Style.Add("display", "none")
                trServiceType.Style.Add("display", "none")
                trTMeansOfInput.Style.Add("display", "none")
                trTHAVaccination.Style.Add("display", "none")
                trOCSSSCheckingResult.Style.Add("display", "none")
                trTFirstAuthorization.Style.Add("display", "none")
                trTSecondAuthorization.Style.Add("display", "none")
                trTPaymentFileSubmitTime.Style.Add("display", "none")
                trTBankPaymentDay.Style.Add("display", "none")
                trTSuspendBy.Style.Add("display", "none")
                trTSuspendReason.Style.Add("display", "none")
                trVoidTransactionNo.Style.Add("display", "none")
                trVoidBy.Style.Add("display", "none")
                trVoidReason.Style.Add("display", "none")
                trCreationReason.Style.Add("display", "none")
                trPaymentMethod.Style.Add("display", "none")
                trCreateBy.Style.Add("display", "none")
                trApprovalBy.Style.Add("display", "none")
                trRejectBy.Style.Add("display", "none")

            Case Else
                'Show default
                trTConfirmTime.Style.Remove("display")
                trTTransactionTime.Style.Add("display", "none")

        End Select


    End Sub

    Private Function GetDisplayCodeFromSchemeCode(ByVal strSchemeCode As String) As String
        For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimBLL.getAllDistinctSchemeClaim()
            If udtSchemeClaim.SchemeCode.Trim = strSchemeCode.Trim Then Return udtSchemeClaim.DisplayCode.Trim
        Next

        Return String.Empty

    End Function

    Public Sub ClearDocumentType()
        udcReadOnlyDocumentType.Clear()
        ViewState(VS_EHSTRANSACTION) = Nothing
        ViewState(VS_SUSPENDHISTORY) = Nothing
        'ViewState(VS_POPUPWARNINGMSG) = Nothing
    End Sub

    Public Sub ClearEHSClaim()
        udcReadOnlyEHSClaim.Clear()
        ViewState(VS_EHSTRANSACTION) = Nothing
        ViewState(VS_SUSPENDHISTORY) = Nothing
        'ViewState(VS_POPUPWARNINGMSG) = Nothing
    End Sub

    Public Sub ClearVaccineRecord()
        Session(SESS_ClaimCOVID19_VaccineRecord) = Nothing
    End Sub

    Private Function Replace(ByVal udtWarning As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult) As String

        Dim strOriMsg As String = udtWarning.ErrorMessage.GetMessage

        Dim arlOldChar As ArrayList
        Dim arlNewChar As ArrayList

        Dim strLang As String = udtSessionHandlerBLL.Language

        If strLang.Trim.ToLower.Equals("zh-tw") Then
            arlOldChar = udtWarning.MessageVariableNameChiArrayList
            arlNewChar = udtWarning.MessageVariableValueChiArrayList
        Else
            arlOldChar = udtWarning.MessageVariableNameArrayList
            arlNewChar = udtWarning.MessageVariableValueArrayList
        End If

        Dim strOldCharList() As String = arlOldChar.ToArray
        Dim strNewCharList() As String = arlNewChar.ToArray


        Dim i As Integer
        For i = 0 To strOldCharList.Length - 1
            If Not strOldCharList(i) Is Nothing AndAlso Not strNewCharList(i) Is Nothing Then
                strOriMsg = strOriMsg.Replace(strOldCharList(i), strNewCharList(i))
            End If
        Next

        Return strOriMsg
    End Function

    Protected Sub lnkTServiceProviderID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkTServiceProviderID.Click
        ' MenuControlBLL.ToServiceProviderDetail.Redirect(Me.lnkTServiceProviderID.Text)

        Me.Session(eHSAccountEnquiry.SESSION_REDIRECT_PARAMETER) = Me.lnkTServiceProviderID.Text
        Me.Session(eHSAccountEnquiry.SESSION_REDIRECT_SOURCE) = spEnquiry.REDIRECT_NAME

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        RedirectHandler.ToURL(lnkTServiceProviderID.CommandArgument)
        ' CRE19-026 (HCVS hotline service) [End][Winnie]
    End Sub

    Private Const FUNCTION_CODE_SERVICE_PROVIDER_ENQUIRY As String = Common.Component.FunctCode.FUNT010204

    ''' <summary>
    ''' Get corresponding page URL by function code
    ''' </summary>
    ''' <param name="strFunctionCode">Function Code</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetURL(ByVal strFunctionCode As String) As String
        Dim udtMenuBLL As New Component.Menu.MenuBLL
        Dim drs() As DataRow = udtMenuBLL.GetMenuItemTable.Select(String.Format("Function_Code='{0}'", strFunctionCode))
        If drs.Length = 0 Then Return String.Empty

        ' Record_Status must 'A' if get from cache
        Return drs(0)("URL")
    End Function

    ''' <summary>
    ''' Check access right on function code by current logined user (Cached HCVUUser model)
    ''' </summary>
    ''' <param name="strFunctionCode">Function Code</param>
    ''' <param name="udtHCVUUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsAllow(ByVal strFunctionCode As String, Optional ByVal udtHCVUUser As HCVUUserModel = Nothing)
        Dim udtHCVUUserBLL As New HCVUUserBLL

        If udtHCVUUser Is Nothing Then
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser()
        End If

        Return udtHCVUUser.AccessRightCollection.Item(strFunctionCode).Allow

    End Function

    ' ===== End of INT11-0003 =====


    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]

    Private Sub DisplayTMeansOfInput(ByVal blnDisplay As Boolean)
        Me.trTMeansOfInput.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    Private Sub DisplayTHAVaccination(ByVal blnDisplay As Boolean)
        Me.trTHAVaccination.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub DisplayOCSSSCheckingResult(ByVal blnDisplay As Boolean)
        Me.trOCSSSCheckingResult.Style.Item("display") = IIf(blnDisplay, "", "none")
        Me.lblOCSSSCheckingResultText.Visible = blnDisplay
        Me.lblOCSSSCheckingResult.Visible = blnDisplay
    End Sub
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    Private Sub DisplayTFirstAuthorization(ByVal blnDisplay As Boolean)
        Me.trTFirstAuthorization.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    Private Sub DisplayTSecondAuthorization(ByVal blnDisplay As Boolean)
        Me.trTSecondAuthorization.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    Private Sub DisplayTPaymentFileSubmitTime(ByVal blnDisplay As Boolean)
        Me.trTPaymentFileSubmitTime.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    Private Sub DisplayTBankPaymentDay(ByVal blnDisplay As Boolean)
        Me.trTBankPaymentDay.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    Private Sub DisplayTSuspendBy(ByVal blnDisplay As Boolean)
        Me.trTSuspendBy.Style.Item("display") = IIf(blnDisplay, "", "none")
        Me.trTSuspendReason.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    Private Sub DisplayCreateBy(ByVal blnDisplay As Boolean)
        If blnDisplay Then
            Me.trCreateBy.Style.Remove("display")
        Else
            Me.trCreateBy.Style.Add("display", "none")
        End If
    End Sub

    Private Sub DisplayCreationReason(ByVal blnDisplay As Boolean)
        Me.trCreationReason.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    Private Sub DisplayPaymentMethod(ByVal blnDisplay As Boolean)
        Me.trPaymentMethod.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    Private Sub DisplayApprovalBy(ByVal blnDisplay As Boolean)
        Me.trApprovalBy.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    Private Sub DisplayRejectBy(ByVal blnDisplay As Boolean)
        Me.trRejectBy.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub

    Private Sub DisplayVoidTransactionNo(ByVal blnDisplay As Boolean)
        Me.trVoidTransactionNo.Style.Item("display") = IIf(blnDisplay, "", "none")
        Me.trVoidReason.Style.Item("display") = IIf(blnDisplay, "", "none")
        Me.trVoidBy.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Private Sub DisplayBankAccountNo(ByVal blnDisplay As Boolean)
        Me.trBankAccountNo.Style.Item("display") = IIf(blnDisplay, "", "none")
    End Sub
    ' CRE19-026 (HCVS hotline service) [End][Winnie]

    Private Sub DisplayReadOnlyEHSClaim(ByVal blnDisplay As Boolean)
        If blnDisplay Then
            Me.trReadOnlyEHSClaim.Style.Remove("display")
        Else
            Me.trReadOnlyEHSClaim.Style.Add("display", "none")
        End If

    End Sub

    Private Sub DisplayContactNo(ByVal blnDisplay As Boolean)
        If blnDisplay Then
            Me.trContactNo.Style.Remove("display")
        Else
            Me.trContactNo.Style.Add("display", "none")
        End If

    End Sub


    Private Sub DisplayRemarks(ByVal blnDisplay As Boolean)
        If blnDisplay Then
            Me.trRemark.Style.Remove("display")
        Else
            Me.trRemark.Style.Add("display", "none")
        End If

    End Sub

    Private Sub DisplayJoinEHRSS(ByVal blnDisplay As Boolean)
        If blnDisplay Then
            Me.trJoinEHRSS.Style.Remove("display")
        Else
            Me.trJoinEHRSS.Style.Add("display", "none")
        End If

    End Sub

    Private Sub DisplayNonLocalRecoveredHistory(ByVal blnDisplay As Boolean)
        If blnDisplay Then
            Me.trNonLocalRecoveredHistory.Style.Remove("display")
        Else
            Me.trNonLocalRecoveredHistory.Style.Add("display", "none")
        End If

    End Sub

    Private Function IsClaimCOVID19(ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
        Dim udtTranDetailList As TransactionDetailModelCollection = udtEHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

        If udtTranDetailList.Count > 0 Then
            Return True
        End If

        Return False

    End Function

    Private Sub FillContactNo(ByVal udtEHSTransaction As EHSTransactionModel)
        If udtEHSTransaction.TransactionAdditionFields.ContactNo IsNot Nothing AndAlso udtEHSTransaction.TransactionAdditionFields.ContactNo <> String.Empty Then
            lblContact.Text = udtEHSTransaction.TransactionAdditionFields.ContactNo

            If udtEHSTransaction.TransactionAdditionFields.Mobile IsNot Nothing AndAlso udtEHSTransaction.TransactionAdditionFields.Mobile = YesNo.Yes Then
                lblContact.Text = lblContact.Text & " (" & GetGlobalResourceObject("Text", "Mobile") & ")"
            End If

            If _blnShowContactNoNotAbleToSMS Then
                Select Case Left(udtEHSTransaction.TransactionAdditionFields.ContactNo, 1)
                    Case "2", "3"
                        lblContactNoNotAbleSMS.Visible = True
                    Case Else
                        lblContactNoNotAbleSMS.Visible = False
                End Select
            Else
                lblContactNoNotAbleSMS.Visible = False
            End If

        Else
            lblContact.Text = GetGlobalResourceObject("Text", "NotProvided")
        End If

    End Sub

    Private Sub FillRemarks(ByVal udtEHSTransaction As EHSTransactionModel)
        If udtEHSTransaction.TransactionAdditionFields.Remarks IsNot Nothing AndAlso udtEHSTransaction.TransactionAdditionFields.Remarks <> String.Empty Then
            lblRemark.Text = udtEHSTransaction.TransactionAdditionFields.Remarks
        Else
            lblRemark.Text = GetGlobalResourceObject("Text", "NotProvided")
        End If

    End Sub

    Private Sub FillJoinEHRSS(ByVal udtEHSTransaction As EHSTransactionModel)
        If udtEHSTransaction.TransactionAdditionFields.JoinEHRSS IsNot Nothing AndAlso udtEHSTransaction.TransactionAdditionFields.JoinEHRSS <> String.Empty Then
            lblJoinEHRSS.Text = IIf(udtEHSTransaction.TransactionAdditionFields.JoinEHRSS = YesNo.Yes, _
                                       GetGlobalResourceObject("Text", "Yes"), _
                                       GetGlobalResourceObject("Text", "No"))

        Else
            lblJoinEHRSS.Text = GetGlobalResourceObject("Text", "NA")
        End If

    End Sub

    Private Sub FillNonLocalRecoveredHistory(ByVal udtEHSTransaction As EHSTransactionModel)
        If udtEHSTransaction.TransactionAdditionFields.NonLocalRecoveredHistory IsNot Nothing AndAlso _
            udtEHSTransaction.TransactionAdditionFields.NonLocalRecoveredHistory <> String.Empty Then

            lblNonLocalRecoveredHistory.Text = IIf(udtEHSTransaction.TransactionAdditionFields.NonLocalRecoveredHistory = YesNo.Yes, _
                                                   GetGlobalResourceObject("Text", "Yes"), _
                                                   GetGlobalResourceObject("Text", "No"))

        Else
            lblNonLocalRecoveredHistory.Text = GetGlobalResourceObject("Text", "NA")
        End If

    End Sub

    Private Sub FillCreateBy(ByVal udtEHSTransaction As EHSTransactionModel)
        lblCreateBy.Text = udtEHSTransaction.CreateBy
        'Label2.Text = udtEHSTransaction.CreateBy
    End Sub


    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Function CheckVaccinationRecordForReprint(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtTranDetailVaccineList As TransactionDetailVaccineModelCollection) As Boolean
        Dim blnValid As Boolean = True
        Dim udtVaccinationRecordList As TransactionDetailVaccineModelCollection = Nothing
        Dim udtResTranDetailVaccineModel_FirstDose As TransactionDetailVaccineModel = Nothing
        Dim udtResTranDetailVaccineModel_SecondDose As TransactionDetailVaccineModel = Nothing
        Dim udtResTranDetailVaccineModel_ThirdDose As TransactionDetailVaccineModel = Nothing

        Dim intCount_FirstDose As Integer = 0
        Dim intCount_SecondDose As Integer = 0
        Dim intCount_ThirdDose As Integer = 0

        Dim udtVaccinationCardRecord As New VaccinationCardRecordModel()

        udtVaccinationRecordList = udtTranDetailVaccineList.FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

        If udtVaccinationRecordList.Count > 0 Then

            ' CRE20-023-59 (Immu record - 3rd Dose) [Start][Winnie SUEN]
            ' -------------------------------------------------------------
            For Each udtVaccinationRecord As TransactionDetailVaccineModel In udtVaccinationRecordList
                
                ' ====== 1st Dose ======
                If udtVaccinationRecord.AvailableItemCode.Trim.Trim().ToUpper() = SchemeDetails.SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
                    intCount_FirstDose = intCount_FirstDose + 1

                    If udtResTranDetailVaccineModel_FirstDose Is Nothing Then
                        udtResTranDetailVaccineModel_FirstDose = udtVaccinationRecord
                    Else
                        If udtResTranDetailVaccineModel_FirstDose.ServiceReceiveDtm < udtVaccinationRecord.ServiceReceiveDtm Then
                            udtResTranDetailVaccineModel_FirstDose = udtVaccinationRecord
                        End If
                    End If
                End If

                ' ====== 2nd Dose ======
                If udtVaccinationRecord.AvailableItemCode.Trim.Trim().ToUpper() = SchemeDetails.SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                    intCount_SecondDose = intCount_SecondDose + 1

                    If udtResTranDetailVaccineModel_SecondDose Is Nothing Then
                        udtResTranDetailVaccineModel_SecondDose = udtVaccinationRecord
                    Else
                        If udtResTranDetailVaccineModel_SecondDose.ServiceReceiveDtm < udtVaccinationRecord.ServiceReceiveDtm Then
                            udtResTranDetailVaccineModel_SecondDose = udtVaccinationRecord
                        End If
                    End If
                End If

                ' ====== 3rd Dose ======
                If udtVaccinationRecord.AvailableItemCode.Trim.Trim().ToUpper() = SchemeDetails.SubsidizeItemDetailsModel.DoseCode.ThirdDOSE Then
                    intCount_ThirdDose = intCount_ThirdDose + 1

                    If udtResTranDetailVaccineModel_ThirdDose Is Nothing Then
                        udtResTranDetailVaccineModel_ThirdDose = udtVaccinationRecord
                    Else
                        If udtResTranDetailVaccineModel_ThirdDose.ServiceReceiveDtm < udtVaccinationRecord.ServiceReceiveDtm Then
                            udtResTranDetailVaccineModel_ThirdDose = udtVaccinationRecord
                        End If
                    End If
                End If

            Next

            ' Validation

            ' ====== Duplicate Dose ======
            If blnValid Then
                If intCount_FirstDose > 1 OrElse intCount_SecondDose > 1 OrElse intCount_ThirdDose > 1 Then
                    blnValid = False
                End If
            End If

            ' ====== Dose Sequence ======
            If blnValid Then
                Dim dtmLatestDate As Date = Nothing

                If udtResTranDetailVaccineModel_FirstDose IsNot Nothing Then
                    If dtmLatestDate = Nothing Then
                        dtmLatestDate = udtResTranDetailVaccineModel_FirstDose.ServiceReceiveDtm
                    Else
                        If dtmLatestDate <= udtResTranDetailVaccineModel_FirstDose.ServiceReceiveDtm Then
                            dtmLatestDate = udtResTranDetailVaccineModel_FirstDose.ServiceReceiveDtm
                        Else
                            blnValid = False
                        End If
                    End If
                End If

                If udtResTranDetailVaccineModel_SecondDose IsNot Nothing Then
                    If dtmLatestDate = Nothing Then
                        dtmLatestDate = udtResTranDetailVaccineModel_SecondDose.ServiceReceiveDtm
                    Else
                        If dtmLatestDate <= udtResTranDetailVaccineModel_SecondDose.ServiceReceiveDtm Then
                            dtmLatestDate = udtResTranDetailVaccineModel_SecondDose.ServiceReceiveDtm
                        Else
                            blnValid = False
                        End If
                    End If
                End If

                If udtResTranDetailVaccineModel_ThirdDose IsNot Nothing Then
                    If dtmLatestDate = Nothing Then
                        dtmLatestDate = udtResTranDetailVaccineModel_ThirdDose.ServiceReceiveDtm
                    Else
                        If dtmLatestDate <= udtResTranDetailVaccineModel_ThirdDose.ServiceReceiveDtm Then
                            dtmLatestDate = udtResTranDetailVaccineModel_ThirdDose.ServiceReceiveDtm
                        Else
                            blnValid = False
                        End If
                    End If
                End If
            End If

            If blnValid Then

                If udtResTranDetailVaccineModel_FirstDose IsNot Nothing Then
                    udtVaccinationCardRecord.FirstDose = New VaccinationCardDoseRecordModel(udtResTranDetailVaccineModel_FirstDose)
                End If

                If udtResTranDetailVaccineModel_SecondDose IsNot Nothing Then
                    udtVaccinationCardRecord.SecondDose = New VaccinationCardDoseRecordModel(udtResTranDetailVaccineModel_SecondDose)
                End If

                If udtResTranDetailVaccineModel_ThirdDose IsNot Nothing Then
                    udtVaccinationCardRecord.ThirdDose = New VaccinationCardDoseRecordModel(udtResTranDetailVaccineModel_ThirdDose)
                End If

            End If

            'Save vaccine detail for reprint vaccination card
            udtSessionHandlerBLL.ClaimCOVID19VaccinationCardSaveToSession(udtVaccinationCardRecord, _strClaimTransDetailFunctionCode)

            ' CRE20-023-59 (Immu record - 3rd Dose) [End][Winnie SUEN]

        End If

        Return blnValid

    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub DisplayCOVID19VaccinationRecord(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel)
        Dim blnValid As Boolean = True
        Dim dtVaccineRecord As DataTable = Nothing

        'Set doc type from transaction
        Dim udtCloneEHSAccount As EHSAccountModel = New EHSAccountModel(udtEHSAccount)
        Dim udtCloneEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = Nothing

        If udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSTransaction.DocCode) IsNot Nothing Then
            udtCloneEHSPersonalInformation = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSTransaction.DocCode).Clone()
        Else
            udtCloneEHSPersonalInformation = udtEHSAccount.EHSPersonalInformationList(0).Clone()
        End If

        udtCloneEHSAccount.SetPersonalInformation(udtCloneEHSPersonalInformation)

        'Get vaccination record
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = GetVaccinationRecordFromSession(udtCloneEHSAccount, udtEHSTransaction.SchemeCode)

        'Format the vaccination record for display
        dtVaccineRecord = TransactionDetailListToCOVID19DataTable(udtTranDetailVaccineList)

        'Bind the gridview
        BuildCOVID19VaccinationRecordGrid(dtVaccineRecord)

        'Check the vaccination record status
        blnValid = CheckVaccinationRecordStatus(udtCloneEHSAccount, udtEHSTransaction.SchemeCode)

        EnableVaccinationRecordChecking = False

        'Find the nearest EHS vaccination record for vaccination card
        If blnValid Then
            blnValid = CheckVaccinationRecordForReprint(udtEHSTransaction, udtTranDetailVaccineList)
        End If

        'Save whether is valid for reprint 
        udtSessionHandlerBLL.ClaimCOVID19ValidReprintSaveToSession(blnValid, _strClaimTransDetailFunctionCode)

    End Sub

    ' CRE20-0022 (Immu record) [End][Chris YIM]

#End Region

    ' CRE20-0022 (Immu record) [Start][Martin]
#Region "Vaccination Record"
    Private Sub BuildCOVID19VaccinationRecordGrid(ByRef dtVaccineRecord As DataTable)
        Session(SESS_ClaimCOVID19_VaccineRecord) = dtVaccineRecord
        panVaccinationRecord.Visible = True

        gvCVaccinationRecord.RowStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F7F7DE")

        If dtVaccineRecord.Rows.Count > 0 Then
            gvCVaccinationRecord.DataSource = dtVaccineRecord
            gvCVaccinationRecord.DataBind()
            panNoVaccinationRecord.Visible = False
            gvCVaccinationRecord.Visible = True
        Else
            panNoVaccinationRecord.Visible = True
            gvCVaccinationRecord.Visible = False
            gvCVaccinationRecord.Dispose()
        End If

    End Sub



    Private Function CheckVaccinationRecordStatus(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As Boolean
        Dim blnValid As Boolean = True
        Dim blnHAError As Boolean = False
        Dim blnHANotMatch As Boolean = False
        Dim blnDHError As Boolean = False
        Dim blnDHNotMatch As Boolean = False
        Dim udtSystemMessage As SystemMessage = Nothing
        Dim udtSystemMessageList As New List(Of SystemMessage)


        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        Dim udtVaccineResultBag As New VaccineResultCollection

        udtVaccineResultBag = udtEHSClaimBLL.GetVaccinationRecord(udtEHSAccount,
                                                                   udtTranDetailVaccineList, _
                                                                   _strClaimTransDetailFunctionCode, _
                                                                   New AuditLogEntry(_strClaimTransDetailFunctionCode), _
                                                                   strSchemeCode)

        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            Select Case udtVaccineResultBag.HAReturnStatus
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00052))
                    blnHANotMatch = True
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00471))
                    blnHAError = True
            End Select
        End If

        If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
            Select Case udtVaccineResultBag.DHReturnStatus
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.OK
                    If udtVaccineResultBag.DHVaccineResult.SingleClient.ReturnClientCIMSCode = DHTransaction.DHClientModel.ReturnCIMSCode.AllDemographicMatch_PartialRecord Then
                        If udtVaccineResultBag.DHVaccineResult.GetNoOfValidVaccine > 0 Then
                            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00051))
                            blnDHNotMatch = True
                        End If
                    End If
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.DemographicNotMatch
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00053))
                    blnDHNotMatch = True
                Case VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail
                    udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00472))
                    blnDHError = True
            End Select
        End If

        If blnHANotMatch And blnDHNotMatch Then
            udtSystemMessageList.Clear()
            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00054))
        End If

        If blnHAError And blnDHError Then
            udtSystemMessageList.Clear()
            udtSystemMessageList.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00473))
        End If

        For Each udtSystemMessage In udtSystemMessageList
            If Not udtSystemMessage Is Nothing Then
                Select Case udtSystemMessage.SeverityCode
                    Case SeverityCode.SEVI
                        udcInfoMessageBox.AddMessage(udtSystemMessage)
                        udcInfoMessageBox.BuildMessageBox()
                    Case SeverityCode.SEVE
                        udcErrorMessage.AddMessage(udtSystemMessage)
                        udcErrorMessage.BuildMessageBox("ConnectionFail")
                        blnValid = False
                    Case Else
                        'Not to show MessageBox
                End Select
            End If
        Next

        Return blnValid

    End Function

    Private Function TransactionDetailListToCOVID19DataTable(ByVal udtTranDetailVaccineList As TransactionDetailVaccineModelCollection) As DataTable
        Dim dtVaccineRecord As New DataTable

        ' Columns
        With dtVaccineRecord.Columns
            .Add("ServiceReceiveDtm", GetType(Date))
            .Add("SubsidizeDesc", GetType(String))
            .Add("SubsidizeDescChi", GetType(String))
            .Add("AvailableItemDesc", GetType(String))
            .Add("AvailableItemDescChi", GetType(String))
            .Add("Provider", GetType(String))
            .Add("Remark", GetType(String))
        End With

        ' Convert each TransactionDetailModel to datarow
        For Each udtTranDetailVaccine As TransactionDetailVaccineModel In udtTranDetailVaccineList
            If udtTranDetailVaccine.SubsidizeItemCode = SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19 Then
                Dim drVaccineRecord As DataRow = dtVaccineRecord.NewRow

                drVaccineRecord("ServiceReceiveDtm") = udtTranDetailVaccine.ServiceReceiveDtm
                drVaccineRecord("SubsidizeDesc") = udtTranDetailVaccine.SubsidizeDesc
                drVaccineRecord("SubsidizeDescChi") = udtTranDetailVaccine.SubsidizeDescChi
                drVaccineRecord("AvailableItemDesc") = udtTranDetailVaccine.AvailableItemDesc
                drVaccineRecord("AvailableItemDescChi") = udtTranDetailVaccine.AvailableItemDescChi

                If udtTranDetailVaccine.SchemeCode = Common.Component.Scheme.SchemeClaimModel.RVP Then
                    drVaccineRecord("Provider") = TransactionDetailVaccineModel.ProviderClass.RVP
                Else
                    drVaccineRecord("Provider") = udtTranDetailVaccine.Provider
                End If
                drVaccineRecord("Remark") = udtTranDetailVaccine.RecordType

                dtVaccineRecord.Rows.Add(drVaccineRecord)
            End If
        Next

        ' Sort the datatable
        Dim dtResult As DataTable = dtVaccineRecord.Clone

        For Each dr As DataRow In dtVaccineRecord.Select(String.Empty, "ServiceReceiveDtm DESC")
            dtResult.ImportRow(dr)
        Next

        Return dtResult

    End Function

    Public Function GetVaccinationRecordFromSession(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As TransactionDetailVaccineModelCollection
        Dim udtVaccinationBLL As New VaccinationBLL
        Dim udtEHSTransactionBLL As New EHSTransactionBLL

        Dim htRecordSummary As Hashtable = Nothing
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = New Common.WebService.Interface.HAVaccineResult(Common.WebService.Interface.HAVaccineResult.enumReturnCode.Error)
        Dim udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult = New Common.WebService.Interface.DHVaccineResult(Common.WebService.Interface.DHVaccineResult.enumReturnCode.UnexpectedError)
        Dim udtHAVaccineResultSession As Common.WebService.Interface.HAVaccineResult = udtSessionHandlerBLL.CMSVaccineResultGetFromSession(_strClaimTransDetailFunctionCode)
        Dim udtDHVaccineResultSession As Common.WebService.Interface.DHVaccineResult = udtSessionHandlerBLL.CIMSVaccineResultGetFromSession(_strClaimTransDetailFunctionCode)

        'If Me.CheckFromVaccinationRecordEnquiry Then
        '    If udtSessionHandlerBLL.CMSVaccineResultGetFromSession(Common.Component.FunctCode.FUNT020801) IsNot Nothing Then
        '        udtHAVaccineResultSession = udtSessionHandlerBLL.CMSVaccineResultGetFromSession(Common.Component.FunctCode.FUNT020801)
        '        udtSessionHandlerBLL.CMSVaccineResultSaveToSession(udtHAVaccineResultSession, FunctionCode)
        '        udtSessionHandlerBLL.CMSVaccineResultRemoveFromSession(Common.Component.FunctCode.FUNT020801)
        '    End If

        '    If udtSessionHandlerBLL.CIMSVaccineResultGetFromSession(Common.Component.FunctCode.FUNT020801) IsNot Nothing Then
        '        udtDHVaccineResultSession = udtSessionHandlerBLL.CIMSVaccineResultGetFromSession(Common.Component.FunctCode.FUNT020801)
        '        udtSessionHandlerBLL.CIMSVaccineResultSaveToSession(udtDHVaccineResultSession, FunctionCode)
        '        udtSessionHandlerBLL.CIMSVaccineResultRemoveFromSession(Common.Component.FunctCode.FUNT020801)
        '    End If
        'End If

        Dim udtVaccineResultBag As New VaccineResultCollection
        udtVaccineResultBag.DHVaccineResult = udtDHVaccineResult
        udtVaccineResultBag.HAVaccineResult = udtHAVaccineResult

        Dim udtVaccineResultBagSession As New VaccineResultCollection
        udtVaccineResultBagSession.DHVaccineResult = udtDHVaccineResultSession
        udtVaccineResultBagSession.HAVaccineResult = udtHAVaccineResultSession

        udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtVaccineResultBag, htRecordSummary, New AuditLogEntry(_strClaimTransDetailFunctionCode), strSchemeCode, udtVaccineResultBagSession)

        udtSessionHandlerBLL.CMSVaccineResultSaveToSession(udtVaccineResultBag.HAVaccineResult, _strClaimTransDetailFunctionCode)
        udtSessionHandlerBLL.CIMSVaccineResultSaveToSession(udtVaccineResultBag.DHVaccineResult, _strClaimTransDetailFunctionCode)

        udtTranDetailVaccineList.Sort(TransactionDetailVaccineModelCollection.enumSortBy.ServiceDate, SortDirection.Descending)

        Return udtTranDetailVaccineList
    End Function

    Protected Sub gvCVaccinationRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim udtFormatter As New Formatter
            Dim blnIsChinese As Boolean = (LCase(Session("language")) = CultureLanguage.TradChinese)

            ' Injection Date
            Dim lblCInjectionDate As Label = e.Row.FindControl("lblCInjectionDate")
            If blnIsChinese Then
                lblCInjectionDate.Text = udtFormatter.formatDisplayDate(CDate(lblCInjectionDate.Text.Trim), CultureLanguage.TradChinese)
            Else
                lblCInjectionDate.Text = udtFormatter.formatDisplayDate(CDate(lblCInjectionDate.Text.Trim), CultureLanguage.English)
            End If

            ' Vaccination
            Dim lblGVaccination As Label = e.Row.FindControl("lblCVaccination")
            Dim lblGVaccinationChi As Label = e.Row.FindControl("lblCVaccinationChi")

            lblGVaccination.Visible = Not blnIsChinese
            lblGVaccinationChi.Visible = blnIsChinese

            ' Dose
            Dim lblGDose As Label = e.Row.FindControl("lblCDose")
            Dim lblGDoseChi As Label = e.Row.FindControl("lblCDoseChi")

            lblGDose.Visible = Not blnIsChinese
            lblGDoseChi.Visible = blnIsChinese

            ' Information Provider
            Dim lblGProvider As Label = e.Row.FindControl("lblCProvider")

            If lblGProvider.Text.ToUpper.Trim = TransactionDetailVaccineModel.ProviderClass.Private Then
                'Enrolled Doctors (eHS(S))
                lblGProvider.Text = GetGlobalResourceObject("Text", "eHSSubsidize")
            Else
                'Hospital Authority, Department of Health, Residential Care Home (eHS(S))
                If blnIsChinese Then
                    Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.ProviderClass.ClassCode, lblGProvider.Text.Trim, String.Empty, lblGProvider.Text)
                Else
                    Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.ProviderClass.ClassCode, lblGProvider.Text.Trim, lblGProvider.Text, String.Empty)
                End If
            End If

            ' Remarks
            Dim lblCRemark As Label = e.Row.FindControl("lblCRemark")
            If blnIsChinese Then
                Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.RecordTypeClass.ClassCode, lblCRemark.Text.Trim, String.Empty, lblCRemark.Text)
            Else
                Status.GetDescriptionFromDBCode(TransactionDetailVaccineModel.RecordTypeClass.ClassCode, lblCRemark.Text.Trim, lblCRemark.Text, String.Empty)
            End If

        End If
    End Sub

    Protected Sub gvCVaccinationRecord_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_ClaimCOVID19_VaccineRecord)
    End Sub

    Protected Sub gvCVaccinationRecord_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_ClaimCOVID19_VaccineRecord)
    End Sub

#End Region
    ' CRE20-0022 (Immu record) [End][Martin]

End Class
