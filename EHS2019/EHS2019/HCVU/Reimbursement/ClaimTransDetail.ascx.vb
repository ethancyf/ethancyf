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
Imports HCVU.BLL

Partial Public Class ClaimTransDetail
    Inherits System.Web.UI.UserControl

#Region "Fields"

    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtReasonForVisitBLL As New ReasonForVisitBLL
    Private udtReimbursementBLL As New ReimbursementBLL
    Private udtSchemeClaimBLL As New SchemeClaimBLL

    Private VS_EHSTRANSACTION As String = "VS_CLAIMCONTROL_EHSTRANSACTION"
    Private VS_SUSPENDHISTORY As String = "VS_CLAIMCONTROL_SUSPENDHISTORY"
    'Private VS_POPUPWARNINGMSG As String = "VS_CLAIMCONTROL_POPUPWARNINGMSG"

    Private _blnEnableVaccinationRecordChecking As Boolean = True

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private _blnEnableToShowHKICSymbol As Boolean = False
    Private _blnShowOCSSSCheckingResult As Boolean = True
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
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
    ' CRE19-026 (HCVS hotline service) [End][Winnie]
#End Region

#Region "Properties"
    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
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
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    Public Property EnableVaccinationRecordChecking() As Boolean
        Get
            Return _blnEnableVaccinationRecordChecking
        End Get
        Set(value As Boolean)
            _blnEnableVaccinationRecordChecking = value
        End Set
    End Property

    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
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
    ' CRE19-026 (HCVS hotline service) [End][Winnie]
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
            If udtEHSAccount.AccountSourceString = EHSAccountModel.SysAccountSourceClass.ValidateAccount Then
                udcReadOnlyDocumentType.OriginalAccID = udtEHSAccount.VoucherAccID
            ElseIf udtEHSAccount.AccountSourceString = EHSAccountModel.SysAccountSourceClass.SpecialAccount Then
                udcReadOnlyDocumentType.OriginalAccID = udtEHSAccount.VoucherAccID
            End If

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
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        udcReadOnlyDocumentType.ShowHKICSymbol = _blnEnableToShowHKICSymbol
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
        udcReadOnlyDocumentType.ShowAccountIDAsBtn = _blnEnableToShowAccountIDAsBtn
        udcReadOnlyDocumentType.ShowDateOfDeathBtn = _blnEnableToShowDateOfDeathBtn
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

        udcReadOnlyDocumentType.Build()

        ' =====================================================
        ' --- Transaction Information ---
        ' =====================================================

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]        
        'Override Reason Warning
        If _blnEnableToShowWarning = False OrElse IsNothing(udtEHSTransaction.WarningMessage) Then
            ' CRE19-026 (HCVS hotline service) [End][Winnie]
            Me.pnlWarning.Visible = False
        Else
            If udtEHSTransaction.WarningMessage.RuleResults.Count = 0 Then
                Me.pnlWarning.Visible = False
            Else
                Me.pnlWarning.Visible = True

                Me.lblOverrideReason.Text = udtEHSTransaction.OverrideReason

                'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                ' Fix display duplicate warning message 
                If pnlWarningMessageList.Controls.Count > 0 Then
                    pnlWarningMessageList.Controls.Clear()
                End If
                'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

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

        If IsNothing(udtEHSTransaction.TransactionID) OrElse udtEHSTransaction.TransactionID.Trim.Equals(String.Empty) Then
            Me.lblTTransactionNoText.Visible = False
            Me.lblTTransactionNo.Visible = False
            Me.lblTTransactionTime.Visible = False

        Else
            ' Transaction No.
            Me.lblTTransactionNoText.Visible = True
            Me.lblTTransactionNo.Visible = True
            Me.lblTTransactionTime.Visible = True

            lblTTransactionNo.Text = udtFormatter.formatSystemNumber(Trim(udtEHSTransaction.TransactionID))
            lblTTransactionTime.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm, String.Empty))
        End If


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
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'lblTServiceDate.Text = udtFormatter.formatDate(udtEHSTransaction.ServiceDate, String.Empty)
        lblTServiceDate.Text = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'CRE11-007
        imgTServiceDate.Visible = False
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        'If udtEHSAccount.DeathRecord.IsDead Then
        '    If udtEHSAccount.DeathRecord.IsDead(udtEHSTransaction.ServiceDate) Then
        If udtEHSAccount.Deceased Then
            Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSTransaction.DocCode)

            If udtPersonalInfo.IsDeceasedAsAt(EHSAccountModel.EHSPersonalInformationModel.DODCalMethodClass.FIRSTDAYOFMONTHYEAR, udtEHSTransaction.ServiceDate) Then
                imgTServiceDate.Visible = True
            End If
        End If
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

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

        ' ===== INT11-0003: Hyperlink to eHealth Account =====

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

        ' ===== End of INT11-0003 =====

        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
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
        'CRE15-004 (TIV and QIV) [End][Chris YIM]

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

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
        ' Bank Account No.
        If _blnEnableToShowBankAccountNo Then
            lblTBankAccountNo.Text = udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)
            DisplayBankAccountNo(True)
        Else
            DisplayBankAccountNo(False)
        End If
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

        ' Service Type
        lblTServiceType.Text = udtEHSTransaction.ServiceTypeDesc

        ' Scheme-related fields
        udcReadOnlyEHSClaim.Clear()


        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
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
                udcReadOnlyEHSClaim.BuildRVP()

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
                udcReadOnlyEHSClaim.BuildVSS()

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204
                udcReadOnlyEHSClaim.BuildENHVSSO()

            Case SchemeClaimModel.EnumControlType.PPP
                udcReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
                udcReadOnlyEHSClaim.Width = 204
                udcReadOnlyEHSClaim.BuildPPP()

        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        ' ===== CRE10-027: Means of Input =====

        ' Means of Input
        If (New GeneralFunction).CheckTurnOnMeansOfInput = GeneralFunction.EnumTurnOnStatus.Yes Then
            lblTMeansOfInputText.Visible = True
            lblTMeansOfInput.Visible = True
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            DisplayTMeansOfInput(True)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

            Status.GetDescriptionFromDBCode(EHSTransactionModel.MeansOfInputClass.ClassCode, IIf(udtEHSTransaction.CreateBySmartID, EHSTransactionModel.MeansOfInputClass.CardReader, EHSTransactionModel.MeansOfInputClass.Manual), lblTMeansOfInput.Text, String.Empty)

        Else
            lblTMeansOfInputText.Visible = False
            lblTMeansOfInput.Visible = False
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            DisplayTMeansOfInput(False)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]
        End If

        ' ===== End of CRE10-027 =====

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
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
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
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

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
        DisplayTFirstAuthorization(False)
        DisplayTSecondAuthorization(False)
        DisplayTPaymentFileSubmitTime(False)
        DisplayTBankPaymentDay(False)
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
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
                            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            'lblTBankPaymentDay.Text = udtFormatter.formatDate(dr("BankPaymentDtm"), String.Empty)
                            lblTBankPaymentDay.Text = udtFormatter.formatDisplayDate(dr("BankPaymentDtm"))
                            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
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

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
        ' Suspend Reason / Suspend By
        If _blnEnableToShowSuspendBy AndAlso udtEHSTransaction.RecordStatus = ClaimTransStatus.Suspended Then
            ' CRE19-026 (HCVS hotline service) [End][Winnie]

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

            ' CRE19-026 (HCVS hotline service) [Start][Winnie]
            ' ------------------------------------------------------------------------
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
            ' CRE19-026 (HCVS hotline service) [End][Winnie]


            ' CRE19-026 (HCVS hotline service) [Start][Winnie]
            ' ------------------------------------------------------------------------
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
            ' CRE19-026 (HCVS hotline service) [End][Winnie]

            ' CRE19-026 (HCVS hotline service) [Start][Winnie]
            ' ------------------------------------------------------------------------
            ' Create By
            If _blnEnableToShowCreateBy = False OrElse IsNothing(udtEHSTransaction.TransactionID) OrElse udtEHSTransaction.TransactionID.Trim.Equals(String.Empty) Then
                ' CRE19-026 (HCVS hotline service) [End][Winnie]
                lblCreateByText.Visible = False
                lblCreateBy.Visible = False
                lblCreateDtm.Visible = False
                'CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
                DisplayCreateBy(False)
                'CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]
            Else
                lblCreateByText.Visible = True
                lblCreateBy.Visible = True
                lblCreateDtm.Visible = True

                lblCreateBy.Text = udtEHSTransaction.CreateBy
                lblCreateDtm.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.CreateDate, String.Empty))

                'CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
                DisplayCreateBy(True)
                'CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]
            End If

        Else
            lblCreationReasonText.Visible = False
            lblCreationReason.Visible = False

            lblPaymentMethodText.Visible = False
            lblPaymentMethod.Visible = False

            lblCreateByText.Visible = False
            lblCreateBy.Visible = False
            lblCreateDtm.Visible = False

            'CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            DisplayCreationReason(False)
            DisplayPaymentMethod(False)
            DisplayCreateBy(False)
            'CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

        End If

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]        
        ' Approval By / Approval Dtm
        If _blnEnableToShowApprovalBy = False OrElse udtEHSTransaction.ApprovalBy.Trim.Equals(String.Empty) Then
            ' CRE19-026 (HCVS hotline service) [End][Winnie]
            Me.lblApprovalBy.Visible = False
            Me.lblApprovalByText.Visible = False

            Me.lblApprovalDtm.Visible = False

            'CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            DisplayApprovalBy(False)
            'CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]
        Else
            Me.lblApprovalBy.Visible = True
            Me.lblApprovalByText.Visible = True

            Me.lblApprovalDtm.Visible = True

            Me.lblApprovalBy.Text = udtEHSTransaction.ApprovalBy
            Me.lblApprovalDtm.Text = String.Format("({0})", udtFormatter.formatDateTime(udtEHSTransaction.ApprovalDate.Value, String.Empty))

            'CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            DisplayApprovalBy(True)
            'CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]
        End If


        ' CRE19-026 (HCVS hotline service) [Start][Winnie] 
        ' Reject By / Reject Dtm
        If _blnEnableToShowRejectBy = False OrElse udtEHSTransaction.RejectBy.Trim.Equals(String.Empty) Then
            ' CRE19-026 (HCVS hotline service) [End][Winnie]   
            Me.lblRejectBy.Visible = False
            Me.lblRejectByText.Visible = False

            Me.lblRejectDtm.Visible = False

            'CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            DisplayRejectBy(False)
            'CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]
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


        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' Void Transaction No. / Void Reason / Void By
        If _blnEnableToShowVoidTransactionNo AndAlso udtEHSTransaction.RecordStatus = ClaimTransStatus.Inactive Then
            ' CRE19-026 (HCVS hotline service) [End][Winnie]
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

            'CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            DisplayVoidTransactionNo(True)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

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


            'CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            DisplayVoidTransactionNo(False)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

        End If



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

    Private Function Replace(ByVal udtWarning As EHSClaim.EHSClaimBLL.EHSClaimBLL.RuleResult) As String

        Dim strOriMsg As String = udtWarning.ErrorMessage.GetMessage

        Dim arlOldChar As ArrayList
        Dim arlNewChar As ArrayList

        Dim udtSessionHandlerBLL As New BLL.SessionHandlerBLL
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


    'Protected Sub ibtnWarning_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnWarning.Click
    '    Dim udtEHSTransaction As EHSTransactionModel
    '    Dim dtSuspendHistory As DataTable
    '    Dim blnPopUpWarningMsg As Boolean

    '    udtEHSTransaction = CType(ViewState(VS_EHSTRANSACTION), EHSTransactionModel)
    '    dtSuspendHistory = CType(ViewState(VS_SUSPENDHISTORY), DataTable)
    '    blnPopUpWarningMsg = CBool(ViewState(VS_POPUPWARNINGMSG))

    '    Me.LoadTranInfo(udtEHSTransaction, dtSuspendHistory, blnPopUpWarningMsg)

    '    Me.ModalPopupExtenderWarningMessage.Show()
    'End Sub

    'Protected Sub ibtnWarningMessageClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnWarningMessageClose.Click

    '    Dim udtEHSTransaction As EHSTransactionModel
    '    Dim dtSuspendHistory As DataTable
    '    Dim blnPopUpWarningMsg As Boolean

    '    udtEHSTransaction = CType(ViewState(VS_EHSTRANSACTION), EHSTransactionModel)
    '    dtSuspendHistory = CType(ViewState(VS_SUSPENDHISTORY), DataTable)
    '    blnPopUpWarningMsg = CBool(ViewState(VS_POPUPWARNINGMSG))

    '    Me.LoadTranInfo(udtEHSTransaction, dtSuspendHistory, blnPopUpWarningMsg)

    '    Me.ModalPopupExtenderWarningMessage.Hide()
    'End Sub

    ' ===== INT11-0003: Hyperlink to eHealth Account =====

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
        Me.trCreateBy.Style.Item("display") = IIf(blnDisplay, "", "none")
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

#End Region

End Class
