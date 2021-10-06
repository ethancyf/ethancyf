Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.ClaimRules
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.ClaimTrans
Imports Common.Component.DataEntryUser
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.ReasonForVisit
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.UserAC
Imports Common.Component.VoucherRecipientAccount
Imports Common.Component.VoucherScheme
Imports Common.Format
Imports Common.Validation
Imports HCSP.BLL
Imports Common.WebService.Interface
Imports Common.Component.VoucherInfo

Partial Public Class VoidClaimConfirmDetail
    Inherits TextOnlyBasePage

    Public Class AublitLogDescription
        Public Const VoidTransaction As String = "Void Transaction"
        Public Const VoidTransactionSuccess As String = "Void Transaction Success"
        Public Const VoidTransactionFail As String = "Void Transaction Fail"
        Public Const ConfirmVoidTransaction As String = "Confirm Void Transaction"
        Public Const ConfirmVoidTransactionSuccess As String = "Complete Confirm Void Transaction"
        Public Const ConfirmVoidTransactionFail As String = "Confirm Void Transaction Fail"
        Public Const ReturntoVoidOtherTransaction As String = "Return to Void Other Transaction"
    End Class

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Class HighRiskOption
        Public Const HIGHRISK As String = "HIGHRISK"
        Public Const NOHIGHRISK As String = "NOHIGHRISK"
    End Class
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    Private udtClaimVoucherBLL As ClaimVoucherBLL
    Private udfMessagBox As CustomControls.TextOnlyMessageBox
    Private udtTransactionMaintenance As TransactionMaintenanceBLL
    Private udtFormatter As Formatter = New Formatter
    Private _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
    Private _strValidationFail As String = "ValidationFail"
    Private _blnIsRequireHandlePageRefresh As Boolean = False
    Private _udtGeneralFunction As New GeneralFunction

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Private udtEHSTransactionBLL As New EHSTransactionBLL
    Private _udtClaimCategoryBLL As New ClaimCategoryBLL()
    Private _udtEHSClaimBLL As BLL.EHSClaimBLL = New BLL.EHSClaimBLL()
    Private _udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
    Public Const FunctCode As String = Common.Component.FunctCode.FUNT020303
    Private udtTransactionMaintenanceBLL As New TransactionMaintenanceBLL

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Private _udtSchemeClaimBLL As New SchemeClaimBLL()

    Private Enum ActiveViewIndex
        EnterVoidReason = 0
        ConfirmVoidReason = 1
        CompleteVoid = 2
        ConfirmMessage = 3
        Remark = 4
        InternalError = 5

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        ViewClaimTransactionDetail = 6
        ModifyTransaction = 7
        ModifyTransactionConfirm = 8
        ModifyTransactionCompleted = 9
        ReasonForVisit = 10

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        Me.FunctionCode = FunctCode
        Me.udcReasonForVisit.FunctionCode = Me.FunctionCode

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Dim udtEHSTransaction As EHSTransactionModel = Nothing

        'Get Current USer Account for check Session Expired
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC

        udtEHSTransaction = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)

        'Transaction can be nothing, if the current page is internal error and pressed return button -> session removed
        If udtEHSTransaction Is Nothing Then
            Return
        End If

        'By Default, Void and Modify Transaction are not enabled
        btnClaimTransactionDetailVoid.Visible = False
        btnClaimTransactionDetailModify.Visible = False

        If udtTransactionMaintenanceBLL.CheckTransactionVoidable(udtEHSTransaction) Then
            btnClaimTransactionDetailVoid.Visible = True
        End If

        If udtTransactionMaintenanceBLL.CheckTransactionEditable(udtEHSTransaction, udtUserAC) Then
            btnClaimTransactionDetailModify.Visible = True
        End If

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Me.SetupEHSAccount(udtEHSTransaction)
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]


        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblTitle), Label).Text = Me.GetGlobalResourceObject("Text", "EVoucherSystem")

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.lblSubTitle), Label).Text = Me.GetGlobalResourceObject("Text", "ClaimTransactionManagement")

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        tblModifyTransactionConfirmTransactionStatus.Visible = True
        If Not Me.IsPostBack Then

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ViewClaimTransactionDetail
            Me.SetupClaimTransactionDetail(udtEHSTransaction, False)

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Else
            CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = Me.GetStepText()
            Select Case Me.mvVoidTranDetail.ActiveViewIndex
                Case ActiveViewIndex.EnterVoidReason

                    Me.SetupEnterVoidReason(udtEHSTransaction, False)

                Case ActiveViewIndex.ConfirmVoidReason

                    Me.SetupConfirmVoidReason(udtEHSTransaction, False)

                Case ActiveViewIndex.CompleteVoid
                    'Me.SetupEHSAccount(udtEHSTransaction.EHSAcct, udtEHSTransaction.DocCode)
                    Me.SetupCompleteVoid(udtEHSTransaction, False)
                    Me.BuildMenu()

                Case ActiveViewIndex.ConfirmMessage
                    'Me.SetupEHSAccount(udtEHSTransaction.EHSAcct, udtEHSTransaction.DocCode)
                    CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = String.Empty
                    Me.SetupConfirmMessage()

                Case ActiveViewIndex.Remark
                    'CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = Me.GetGlobalResourceObject("Text", "VoidClaimStep2")
                    Me.ShowVaccineRemarkText()

                Case ActiveViewIndex.InternalError
                    Me.udcMsgBoxErr.Clear()
                    Me.SetupInternalError()

                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                    ' -----------------------------------------------------------------------------------------

                Case ActiveViewIndex.ViewClaimTransactionDetail

                    Me.SetupClaimTransactionDetail(udtEHSTransaction, False)

                Case ActiveViewIndex.ModifyTransaction

                    Me.SetupModifyTransaction(udtEHSTransaction, False)

                Case ActiveViewIndex.ModifyTransactionConfirm
                    tblModifyTransactionConfirmTransactionStatus.Visible = False
                    Me.SetupModifyTransactionConfirm(udtEHSTransaction, False)

                Case ActiveViewIndex.ModifyTransactionCompleted

                    Me.SetupModifyTransaction(udtEHSTransaction, False)
                    Me.SetupModifyTransactionCompleted(udtEHSTransaction, False)

                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
                Case ActiveViewIndex.ReasonForVisit
                    Me.SetupModifyTransaction(udtEHSTransaction, False)
                    Me.panPersonalInformation.Visible = False
            End Select
        End If
    End Sub

    Private Sub mvVoidTranDetail_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvVoidTranDetail.ActiveViewChanged
        CType(Me.Master.FindControl(ClaimVoucherMaster.ControlName.LblClaimVoucherStep), Label).Text = Me.GetStepText()
        ' Hide error icon when view changed
        Me.lblEnterVoidReasonVoidReasonError.Visible = False

        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        tblModifyTransactionConfirmTransactionStatus.Visible = True
        Select Case Me.mvVoidTranDetail.ActiveViewIndex

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            Case ActiveViewIndex.ViewClaimTransactionDetail
                Me.udcMsgBoxErr.Clear()

                Me.SetupClaimTransactionDetail(udtEHSTransaction, True)
                Me.SetupReasonForVisit()

            Case ActiveViewIndex.ModifyTransaction
                Me.udcMsgBoxErr.Clear()

                Me.SetupModifyTransaction(udtEHSTransaction, True)

            Case ActiveViewIndex.ModifyTransactionConfirm
                Me.udcMsgBoxErr.Clear()
                tblModifyTransactionConfirmTransactionStatus.Visible = False
                Me.SetupModifyTransactionConfirm(udtEHSTransaction, True)

            Case ActiveViewIndex.ModifyTransactionCompleted
                Me.udcMsgBoxErr.Clear()
                Me.SetupModifyTransaction(udtEHSTransaction, True)
                Me.SetupModifyTransactionCompleted(udtEHSTransaction, True)

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            Case ActiveViewIndex.EnterVoidReason
                Me.udcMsgBoxErr.Clear()
                Me.SetupEnterVoidReason(udtEHSTransaction, True)

            Case ActiveViewIndex.ConfirmVoidReason
                Me.udcMsgBoxErr.Clear()
                Me.SetupConfirmVoidReason(udtEHSTransaction, True)

            Case ActiveViewIndex.CompleteVoid
                Me.SetupCompleteVoid(udtEHSTransaction, True)
                Me.BuildMenu()

            Case ActiveViewIndex.ConfirmMessage
                Me.udcMsgBoxErr.Clear()
                Me.SetupConfirmMessage()

            Case ActiveViewIndex.Remark
                Me.udcMsgBoxErr.Clear()
                Me.ShowVaccineRemarkText()

            Case ActiveViewIndex.InternalError
                Me.udcMsgBoxErr.Clear()
                Me.SetupInternalError()

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                ' -----------------------------------------------------------------------------------------

            Case ActiveViewIndex.ReasonForVisit
                Me.SetupReasonForVisit()
                Me.panPersonalInformation.Visible = False
                ' Generate Dynamic Control
                'Me.BuildInputEHSClaimControl()

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        End Select

    End Sub

    '' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    '' -----------------------------------------------------------------------------------------

    '' Return Boolean flag indicate the Scheme is available or not:  Return True if available
    'Private Function BuildInputEHSClaimControl() As Boolean

    '    Dim udtSchemeClaim As SchemeClaimModel = Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctionCode)
    '    Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctionCode)

    '    Return BuildInputEHSClaimControl(udtSchemeClaim, udtEHSAccount)

    'End Function

    '' Return Boolean flag indicate the Scheme is available or not
    'Private Function BuildInputEHSClaimControl(ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSAccount As EHSAccountModel, Optional ByVal udtClaimCategorys As ClaimCategoryModelCollection = Nothing) As Boolean
    '    Dim blnIsAvailable As Boolean = False
    '    Dim udtFormatter As Formatter = New Formatter
    '    Dim strServiceDate As String
    '    Dim dtmServiceDate As DateTime
    '    'udtClaimCategory Must have value
    '    Dim udtClaimCategory As ClaimCategoryModel = Me._udtSessionHandler.ClaimCategoryGetFromSession(FunctionCode)
    '    Dim udtTransaction As EHSTransactionModel = Nothing
    '    If IsNothing(Me._udtSessionHandler.EHSTransactionGetFromSession(FunctionCode)) Then
    '        udtTransaction = New EHSTransactionModel
    '        udtTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
    '        Me._udtSessionHandler.EHSTransactionSaveToSession(udtTransaction, FunctionCode)
    '    Else
    '        udtTransaction = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctionCode)
    '    End If


    '    ' Build control only when the scheme have subsidize available
    '    If Not udtSchemeClaim Is Nothing AndAlso Not udtSchemeClaim.SubsidizeGroupClaimList Is Nothing AndAlso Not udtEHSAccount Is Nothing Then
    '        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    '        Select Case udtSchemeClaim.ControlType
    '            'CRE13-019-02 Extend HCVS to China [Start][Karl]
    '            Case SchemeClaimModel.EnumControlType.VOUCHER ', SchemeClaimModel.EnumControlType.VOUCHERCHINA
    '                'CRE13-019-02 Extend HCVS to China [End][Karl]
    '                If Not udtEHSAccount.AvailableVoucher.HasValue Then
    '                    Dim udtEHSTransactionBLL As New EHSTransactionBLL()

    '                    udtEHSAccount.AvailableVoucher = udtEHSTransactionBLL.getAvailableVoucher(Me._udtSessionHandler.SchemeSelectedGetFromSession(FunctionCode), udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode))
    '                    Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
    '                End If

    '                If udtEHSAccount.AvailableVoucher.Value > 0 Then
    '                    blnIsAvailable = True
    '                End If

    '                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '                '-----------------------------------------------------------------------------------------
    '            Case SchemeClaimModel.EnumControlType.EVSS, SchemeClaimModel.EnumControlType.CIVSS, SchemeClaimModel.EnumControlType.HSIVSS, _
    '                SchemeClaimModel.EnumControlType.RVP, SchemeClaimModel.EnumControlType.PIDVSS, SchemeClaimModel.EnumControlType.VSS
    '                'CRE16-002 (Revamp VSS) [End][Chris YIM]
    '                Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()

    '                'if not Vaccine -> get the vaccination list
    '                If udtEHSClaimVaccine Is Nothing OrElse Not udtEHSClaimVaccine.SchemeCode.Equals(udtSchemeClaim.SchemeCode) Then

    '                    ' Get Service Date before search the vaccine
    '                    strServiceDate = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode).ServiceDate

    '                    'One case to change Not udtClaimCategory Is Nothing: claim for same patient will not have udtClaimCategory
    '                    If (udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.HSIVSS) OrElse _
    '                        udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.RVP) OrElse _
    '                        udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.VSS)) AndAlso Not udtClaimCategory Is Nothing Then

    '                        'udtClaimCategorys may not is nothing, passed by function parameter
    '                        If udtClaimCategorys Is Nothing Then
    '                            Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)

    '                            'If scheme is HSIVSS, RVP or VSS, retrieve Claim Category
    '                            udtClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation.DOB, udtPersonalInformation.ExactDOB, dtmServiceDate, udtPersonalInformation.Gender)
    '                        End If

    '                        'Assing Claim Category List to HINSS and RVP control
    '                        Me.ucInputDeferredClaimDetails_Modify.ClaimCategorys = udtClaimCategorys

    '                        'Category must have value -> catagory selected before step 2a
    '                        udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, True, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode), udtClaimCategory.CategoryCode)
    '                    Else
    '                        'Default, no category
    '                        udtEHSClaimVaccine = Me._udtEHSClaimBLL.SearchEHSClaimVaccine(udtSchemeClaim, udtEHSAccount.SearchDocCode, udtEHSAccount, dtmServiceDate, False, GetVaccinationRecordFromSession(udtEHSAccount, udtSchemeClaim.SchemeCode))
    '                    End If

    '                    Me._udtSessionHandler.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine)
    '                Else

    '                    If udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.HSIVSS) OrElse udtSchemeClaim.ControlType.Equals(SchemeClaimModel.EnumControlType.RVP) Then

    '                        If udtClaimCategorys Is Nothing Then
    '                            Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)

    '                            'If scheme is HSIVSS, RVP or VSS, retrieve Claim Category
    '                            udtClaimCategorys = Me._udtClaimCategoryBLL.getDistinctCategoryByScheme(udtSchemeClaim, udtPersonalInformation.DOB, udtPersonalInformation.ExactDOB, dtmServiceDate, udtPersonalInformation.Gender)
    '                        End If

    '                        'Assing Claim Category List to HSIVSS and RVP control
    '                        Me.ucInputDeferredClaimDetails_Modify.ClaimCategorys = udtClaimCategorys
    '                    End If
    '                End If

    '                'Check if no vaccine is avaliable for the recipient -> change "noAvailableForClaim" to false
    '                For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
    '                    If udtEHSClaimSubsidize.Available Then
    '                        blnIsAvailable = True
    '                        Exit For
    '                    End If
    '                Next
    '                Me.ucInputDeferredClaimDetails_Modify.EHSClaimVaccine = udtEHSClaimVaccine
    '        End Select
    '        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

    '        Me.ucInputDeferredClaimDetails_Modify.IsSupportedDevice = Me._udtSessionHandler.IsMobileDeviceGetFromSession
    '        Me.ucInputDeferredClaimDetails_Modify.AvaliableForClaim = blnIsAvailable
    '        Me.ucInputDeferredClaimDetails_Modify.TextOnlyVersion = True
    '        Me.ucInputDeferredClaimDetails_Modify.CurrentPractice = Me._udtSessionHandler.PracticeDisplayGetFromSession(FunctionCode)
    '        Me.ucInputDeferredClaimDetails_Modify.SchemeType = udtSchemeClaim.SchemeCode.Trim()
    '        Me.ucInputDeferredClaimDetails_Modify.EHSAccount = udtEHSAccount
    '        Me.ucInputDeferredClaimDetails_Modify.EHSTransaction = udtTransaction
    '        Me.ucInputDeferredClaimDetails_Modify.ServiceDate = dtmServiceDate
    '        Me.ucInputDeferredClaimDetails_Modify.SetRebuildRequired()
    '        Me.ucInputDeferredClaimDetails_Modify.Built()

    '        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
    '        ' Post Build: set the property that require the control is created
    '        Select Case udtSchemeClaim.ControlType
    '            'CRE13-019-02 Extend HCVS to China [Start][Karl]
    '            Case SchemeClaimModel.EnumControlType.VOUCHER ', SchemeClaimModel.EnumControlType.VOUCHERCHINA
    '                'CRE13-019-02 Extend HCVS to China [End][Karl]
    '                Dim udcInputHCVS As UIControl.EHCClaimText.ucInputHCVS = CType(ucInputDeferredClaimDetails_Modify.GetHCVSControl(), UIControl.EHCClaimText.ucInputHCVS)
    '        End Select
    '        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
    '    End If

    '    Return blnIsAvailable

    'End Function

    '' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Function GetVaccinationRecordFromSession(ByVal udtEHSAccount As EHSAccountModel, Optional ByVal strSchemeCode As String = "") As TransactionDetailVaccineModelCollection
        Dim udtVaccinationBLL As New VaccinationBLL
        Dim udtEHSTransactionBLL As New EHSTransactionBLL
        Dim udtSession As New BLL.SessionHandler

        Dim htRecordSummary As Hashtable = Nothing
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        Dim udtHAVaccineResult As Common.WebService.Interface.HAVaccineResult = Nothing
        Dim udtDHVaccineResult As Common.WebService.Interface.DHVaccineResult = Nothing
        Dim udtHAVaccineResultSession As Common.WebService.Interface.HAVaccineResult = udtSession.CMSVaccineResultGetFromSession(FunctionCode)
        Dim udtDHVaccineResultSession As Common.WebService.Interface.DHVaccineResult = udtSession.CIMSVaccineResultGetFromSession(FunctionCode)

        Dim udtVaccineResultBag As New VaccineResultCollection
        udtVaccineResultBag.DHVaccineResult = udtDHVaccineResult
        udtVaccineResultBag.HAVaccineResult = udtHAVaccineResult

        Dim udtVaccineResultBagSession As New VaccineResultCollection
        udtVaccineResultBagSession.DHVaccineResult = udtDHVaccineResultSession
        udtVaccineResultBagSession.HAVaccineResult = udtHAVaccineResultSession

        udtVaccinationBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, udtVaccineResultBag, htRecordSummary, _udtAuditLogEntry, strSchemeCode, udtVaccineResultBagSession)

        udtSession.CMSVaccineResultSaveToSession(udtVaccineResultBag.HAVaccineResult, FunctionCode)
        udtSession.CIMSVaccineResultSaveToSession(udtVaccineResultBag.DHVaccineResult, FunctionCode)

        udtTranDetailVaccineList.Sort(TransactionDetailVaccineModelCollection.enumSortBy.ServiceDate, SortDirection.Descending)

        Return udtTranDetailVaccineList
    End Function
    ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]


    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)

        ' handle if page is refreshed
        If _blnIsRequireHandlePageRefresh = True Then
            HandlePageRefreshed()
        End If

        MyBase.OnPreRender(e)
    End Sub

    Protected Sub MasterPage_MenuChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me._udtSessionHandler.EHSTransactionSearchByNoRemoveFromSession(FunctCode)
        Me._udtSessionHandler.TextOnlyVersionSearchTypeRemoveFromSession()
        Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)
    End Sub

#Region "Enter Void Reason"

    Private Sub btnEnterVoidReasonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnterVoidReasonCancel.Click
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        ' Cancel Void will back to Claim transaction Management detail page
        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ViewClaimTransactionDetail
        'Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ConfirmMessage
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
    End Sub

    Private Sub btnEnterVoidReasonConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnterVoidReasonConfirm.Click
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(Common.Component.FunctCode.FUNT020302, Me)

        Me.lblEnterVoidReasonVoidReasonError.Visible = False

        AuditLogEnterVoidReasonStart(udtAuditLogEntry, Me.txtEnterVoidReasonVoidReason.Text.Trim())

        udtEHSTransaction.VoidReason = Me.txtEnterVoidReasonVoidReason.Text.Trim()

        If Not udtEHSTransaction.VoidReason.Equals(String.Empty) Then
            Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)
            Me._udtSessionHandler.EHSTransactionOrginalSaveToSession(udtEHSTransaction, FunctCode)
            AuditLogEnterVoidReasonComplete(udtAuditLogEntry, udtEHSTransaction)

            Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ConfirmVoidReason
        Else
            Me.lblEnterVoidReasonVoidReasonError.Visible = True
            Me.udcMsgBoxErr.AddMessage(New Common.ComObject.SystemMessage("020302", "E", "00005"))
            Me.udcMsgBoxErr.BuildMessageBox(Me._strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00012, "Enter Void Reason Failed")
        End If
    End Sub

    Private Sub SetupEnterVoidReason(ByVal udtEHSTransaction As EHSTransactionModel, ByVal activeViewChanged As Boolean)
        Dim udtFormatter As Formatter = New Formatter()
        Dim udtEHSClaimBLL As EHSClaimBLL = New EHSClaimBLL()

        'If Not activeViewChanged Then
        '    Me.SetupEHSAccount(udtEHSTransaction.EHSAcct, udtEHSTransaction.DocCode)
        'End If

        Me.panPersonalInformation.Visible = True
        Me.udcMsgBoxInfo.BuildMessageBox()

        Me.lblEnterVoidReasonTransNum.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
        Me.lblEnterVoidReasonTransDate.Text = udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'Me.lblEnterVoidReasonServiceDate.Text = udtFormatter.formatDate(udtEHSTransaction.ServiceDate)
        Me.lblEnterVoidReasonServiceDate.Text = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Me.lblEnterVoidReasonPractice.Text = udtEHSTransaction.PracticeName

        If Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
            Me.lblEnterVoidReasonPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeNameChi, udtEHSTransaction.PracticeID)
            Me.lblEnterVoidReasonPractice.CssClass = "tableTextChi"
        Else
            Me.lblEnterVoidReasonPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeName, udtEHSTransaction.PracticeID)
            Me.lblEnterVoidReasonPractice.CssClass = "tableText"
        End If


        Me.lblEnterVoidReasonScheme.Text = Me.GetSchemeCodeDescription(udtEHSTransaction.SchemeCode)
        Me.lblEnterVoidReasonBankAcct.Text = udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)
        If Me._udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            Me.lblEnterVoidReasonServiceType.Text = udtEHSTransaction.ServiceTypeDesc_Chi
        Else
            Me.lblEnterVoidReasonServiceType.Text = udtEHSTransaction.ServiceTypeDesc
        End If

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblEnterVoidReasonRecipientConditionText.Text = Me.GetGlobalResourceObject("Text", "RecipientCondition")

        Me.panEnterVoidReasonRecipientCondition.Visible = False
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' For vaccine only
        Dim udtSchemeClaimModel As SchemeClaimModel = (New SchemeClaimBLL).getAllSchemeClaim_WithSubsidizeGroup().Filter(udtEHSTransaction.SchemeCode)
        If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
            udcEnterVoidReasonReadOnlyEHSClaim.EHSClaimVaccine = udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
        End If
        ' CRE19-006 (DHC) [End][Winnie]

        Me.udcEnterVoidReasonReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
        Me.udcEnterVoidReasonReadOnlyEHSClaim.SchemeCode = udtEHSTransaction.SchemeCode
        Me.udcEnterVoidReasonReadOnlyEHSClaim.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Normal
        Me.udcEnterVoidReasonReadOnlyEHSClaim.TextOnlyVersion = True
        Me.udcEnterVoidReasonReadOnlyEHSClaim.Built()

        ' Recipient Condition
        setupDisplayRecipientCondition(udtEHSTransaction, panEnterVoidReasonRecipientCondition, lblEnterVoidReasonRecipientCondition)

        'Contact
        setupDisplayContactNo(udtEHSTransaction, panEnterVoidReasonContactNo, lblEnterVoidReasonContactNo)

        'Remarks
        setupDisplayRemarks(udtEHSTransaction, panEnterVoidReasonRemarks, lblEnterVoidReasonRemarks)

    End Sub

#End Region

#Region "Confirm Void Reason"

    Private Sub btnConfirmVoidReasonBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirmVoidReasonBack.Click
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.EnterVoidReason
    End Sub

    Private Sub btnConfirmVoidReasonConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirmVoidReasonConfirm.Click
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        Dim udtEHSTransactionBLL As EHSTransactionBLL = New EHSTransactionBLL
        Dim udtDataEntryUser As DataEntryUser.DataEntryUserModel = Nothing
        Dim udtSP As ServiceProvider.ServiceProviderModel = Nothing
        Dim isValid As Boolean = True
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(Common.Component.FunctCode.FUNT020302, Me)

        AuditLogConfirmVoidReasonStart(udtAuditLogEntry)

        Me._udtSessionHandler.CurrentUserGetFromSession(udtSP, udtDataEntryUser)

        If udtDataEntryUser Is Nothing Then
            udtEHSTransaction.VoidUser = udtSP.SPID
            udtEHSTransaction.VoidByDataEntry = String.Empty
        Else
            udtEHSTransaction.VoidUser = udtDataEntryUser.SPID
            udtEHSTransaction.VoidByDataEntry = udtDataEntryUser.DataEntryAccount
        End If


        Me.udtTransactionMaintenance = New TransactionMaintenanceBLL()


        Try
            isValid = Me.udtTransactionMaintenance.OnVoid(udtEHSTransaction)

            'Catch udtTransactionVoidException As TransactionMaintenanceBLL.TransactionVoidSqlException
            '    If udtTransactionVoidException.SystemMessage Is Nothing Then
            '        AuditLogConfirmVoidReasonFailed(udtAuditLogEntry, udtTransactionVoidException.Message)

            '        Throw New Exception(udtTransactionVoidException.Message)
            '    Else
            '        Me._udtSessionHandler.EHSTransactionRemoveFromSession(FunctCode)
            '        AuditLogConfirmVoidReasonFailed(udtAuditLogEntry, String.Format("Confirm Void Reason Failed with SystemMessage : {0}", udtTransactionVoidException.SystemMessage.MessageCode))

            '    End If

            '    isValid = False
        Catch eSQL As SqlClient.SqlException
            Me._udtSessionHandler.EHSTransactionRemoveFromSession(FunctCode)
            AuditLogConfirmVoidReasonFailed(udtAuditLogEntry, String.Format("Confirm Void Reason Failed with SystemMessage : {0}", eSQL.Message))
            isValid = False
        Catch ex As Exception
            Throw ex
        End Try

        If isValid Then

            udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(Formatter.ReverseSystemNumber(udtEHSTransaction.TransactionID))

            AuditLogConfirmVoidReasonComplete(udtAuditLogEntry, udtEHSTransaction)

            Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)
            Me._udtSessionHandler.EHSTransactionOrginalSaveToSession(udtEHSTransaction, FunctCode)
            Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.CompleteVoid
        Else
            Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.InternalError

        End If
    End Sub

    Private Sub SetupConfirmVoidReason(ByVal udtEHSTransaction As EHSTransactionModel, ByVal activeViewChanged As Boolean)
        Dim udtFormatter As Formatter = New Formatter()
        Dim udtEHSClaimBLL As EHSClaimBLL = New EHSClaimBLL()

        'If Not activeViewChanged Then
        '    Me.SetupEHSAccount(udtEHSTransaction.EHSAcct, udtEHSTransaction.DocCode)
        'End If

        Me.panPersonalInformation.Visible = True
        Me.udcMsgBoxInfo.BuildMessageBox()

        Me.lblConfirmVoidReasonTransNum.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
        Me.lblConfirmVoidReasonTransDate.Text = udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'Me.lblConfirmVoidReasonServiceDate.Text = udtFormatter.formatDate(udtEHSTransaction.ServiceDate)
        Me.lblConfirmVoidReasonServiceDate.Text = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


        If Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
            Me.lblConfirmVoidReasonPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeNameChi, udtEHSTransaction.PracticeID)
            Me.lblConfirmVoidReasonPractice.CssClass = "tableTextChi"
        Else
            Me.lblConfirmVoidReasonPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeName, udtEHSTransaction.PracticeID)
            Me.lblConfirmVoidReasonPractice.CssClass = "tableText"
        End If


        Me.lblConfirmVoidReasonScheme.Text = Me.GetSchemeCodeDescription(udtEHSTransaction.SchemeCode)
        Me.lblConfirmVoidReasonBankAcct.Text = udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)
        If Me._udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            Me.lblConfirmVoidReasonServiceType.Text = udtEHSTransaction.ServiceTypeDesc_Chi
        Else
            Me.lblConfirmVoidReasonServiceType.Text = udtEHSTransaction.ServiceTypeDesc
        End If
        Me.lblConfirmVoidReasonVoidReason.Text = udtEHSTransaction.VoidReason

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblConfirmVoidReasonRecipientConditionText.Text = Me.GetGlobalResourceObject("Text", "RecipientCondition")

        Me.panConfirmVoidReasonRecipientCondition.Visible = False
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        ' Scheme-related fields
        'CRE13-001 EHAPP [Start][Karl]
        '--------------------------------------------------------------------------
        'If udtEHSTransaction.SchemeCode <> "HCVS" Then
        If udtEHSTransaction.SchemeCode <> Common.Component.Scheme.SchemeClaimModel.HCVS AndAlso udtEHSTransaction.SchemeCode <> Common.Component.Scheme.SchemeClaimModel.EHAPP Then
            'CRE13-001 EHAPP [End][Karl]
            Me.udcConfirmVoidReasonBankReadOnlyEHSClaim.EHSClaimVaccine = udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
        End If
        Me.udcConfirmVoidReasonBankReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
        Me.udcConfirmVoidReasonBankReadOnlyEHSClaim.SchemeCode = udtEHSTransaction.SchemeCode
        Me.udcConfirmVoidReasonBankReadOnlyEHSClaim.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Normal
        Me.udcConfirmVoidReasonBankReadOnlyEHSClaim.TextOnlyVersion = True
        Me.udcConfirmVoidReasonBankReadOnlyEHSClaim.Built()

        ' Recipient Condition
        setupDisplayRecipientCondition(udtEHSTransaction, panConfirmVoidReasonRecipientCondition, lblConfirmVoidReasonRecipientCondition)

        'Contact
        setupDisplayContactNo(udtEHSTransaction, panConfirmVoidReasonContactNo, lblConfirmVoidReasonContactNo)

        'Remarks
        setupDisplayRemarks(udtEHSTransaction, panConfirmVoidReasonRemarks, lblConfirmVoidReasonRemarks)

    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Private Sub SetupClaimTransactionDetail(ByVal udtEHSTransaction As EHSTransactionModel, ByVal activeViewChanged As Boolean)
        Dim udtFormatter As Formatter = New Formatter()
        Dim udtEHSClaimBLL As EHSClaimBLL = New EHSClaimBLL()

        'If Not activeViewChanged Then
        '    Me.SetupEHSAccount(udtEHSTransaction.EHSAcct, udtEHSTransaction.DocCode)
        'End If

        ' CRE20-022 (Immu record)  [Start][Martin]
        ' Display setting for COVID-19
        If IsClaimCOVID19(udtEHSTransaction) Then
            Label8.Text = Me.GetGlobalResourceObject("Text", "InjectionDate")
        End If
        ' CRE20-022 (Immu record)  [End][Martin]



        Me.panPersonalInformation.Visible = True
        Me.udcMsgBoxInfo.BuildMessageBox()

        Me.lblViewTransactionDetailTransactionNo.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
        Me.lblViewTransactionDetailTransactionStatus.Text = Me.GetTransactionStatusText(udtEHSTransaction.RecordStatus)
        FormatTransactionStatus(Me.lblViewTransactionDetailTransactionStatus, udtEHSTransaction.RecordStatus)
        Me.lblViewTransactionDetailTransactionDate.Text = udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm)
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.lblDetailRecipientConditionText.Text = Me.GetGlobalResourceObject("Text", "RecipientCondition")

        Me.panDetailRecipientCondition.Visible = False
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'Me.lblViewTransactionDetailServiceDate.Text = udtFormatter.formatDate(udtEHSTransaction.ServiceDate)
        Me.lblViewTransactionDetailServiceDate.Text = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
            Me.lblViewTransactionDetailPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeNameChi, udtEHSTransaction.PracticeID)
            Me.lblViewTransactionDetailPractice.CssClass = "tableTextChi"
        Else
            Me.lblViewTransactionDetailPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeName, udtEHSTransaction.PracticeID)
            Me.lblViewTransactionDetailPractice.CssClass = "tableText"
        End If

        Me.lblViewTransactionDetailScheme.Text = Me.GetSchemeCodeDescription(udtEHSTransaction.SchemeCode)

        Dim udtTranTAF As TransactionAdditionalFieldModel = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID(TransactionAdditionalFieldModel.AdditionalFieldType.ClinicType)

        If Not IsNothing(udtTranTAF) AndAlso udtTranTAF.AdditionalFieldValueCode = "N" Then
            lblViewTransactionDetailScheme.Text += String.Format("<br>({0})", Me.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting"))
        End If

        Me.lblViewTransactionDetailBankAccountNo.Text = udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)
        If Me._udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            Me.lblViewTransactionDetailServiceType.Text = udtEHSTransaction.ServiceTypeDesc_Chi
        Else
            Me.lblViewTransactionDetailServiceType.Text = udtEHSTransaction.ServiceTypeDesc
        End If



        ' Scheme-related fields

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' For vaccine only
        Dim udtSchemeClaimModel As SchemeClaimModel = (New SchemeClaimBLL).getAllSchemeClaim_WithSubsidizeGroup().Filter(udtEHSTransaction.SchemeCode)
        If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
            Me.udcClaimTransactionDetailReadOnlyEHSClaim.EHSClaimVaccine = udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
        End If
        ' CRE19-006 (DHC) [End][Winnie]

        Me.udcClaimTransactionDetailReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
        Me.udcClaimTransactionDetailReadOnlyEHSClaim.SchemeCode = udtEHSTransaction.SchemeCode
        Me.udcClaimTransactionDetailReadOnlyEHSClaim.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Normal
        Me.udcClaimTransactionDetailReadOnlyEHSClaim.TextOnlyVersion = True
        Me.udcClaimTransactionDetailReadOnlyEHSClaim.Built()

        ' Recipient Condition
        setupDisplayRecipientCondition(udtEHSTransaction, panDetailRecipientCondition, lblDetailRecipientCondition)

        'Contact
        setupDisplayContactNo(udtEHSTransaction, panDetailContactNo, lblDetailContactNo)

        'Remarks
        setupDisplayRemarks(udtEHSTransaction, panDetailRemarks, lblDetailRemarks)

    End Sub

    Private Sub FormatTransactionStatus(ByRef lblTransactionStatus As Label, ByVal strTransactionStatus As String)
        If strTransactionStatus = Common.Component.ClaimTransStatus.Incomplete Then
            lblTransactionStatus.CssClass = "tableTextForInComplete"
        Else
            lblTransactionStatus.CssClass = "tableText"
        End If
    End Sub

    Private Sub btnClaimTransactionDetailBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClaimTransactionDetailBack.Click
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        If Me._udtSessionHandler.EHSTransactionListGetFromSession(FunctCode) IsNot Nothing Then
            RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.SelectTransation)
        Else

            Me.BackToVoidClaimSearch()
        End If

    End Sub

    Private Sub btnModifyTransactionBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModifyTransactionBack.Click
        ' Reload Transaction
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        udtEHSTransaction = Me.udtEHSTransactionBLL.LoadClaimTran(udtEHSTransaction.TransactionID)
        Me._udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransaction, FunctCode)
        Me._udtSessionHandler.EHSTransactionOrginalSaveToSession(udtEHSTransaction, FunctCode)

        Me.ucInputDeferredClaimDetails_Modify.Clear()
        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ViewClaimTransactionDetail
    End Sub

    Private Sub btnModifyTransactionModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModifyTransactionNext.Click

        Dim isValid As Boolean = True
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        Dim udtEHSTransactionOrginal As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionOrginalGetFromSession(FunctCode)
        Dim udcInputHCVS As UIControl.EHCClaimText.ucInputHCVS = Me.ucInputDeferredClaimDetails_Modify.GetHCVSControl()
        Dim udtValidator As Validator = New Validator()

        Dim systemMessage As SystemMessage
        Dim strMsgParam As String = String.Empty

        CType(udcInputHCVS.FindControl("ErrCoPaymentFee"), Label).Visible = False

        If isValid Then
            systemMessage = udtValidator.chkCoPaymentFee(udcInputHCVS.CoPaymentFee, strMsgParam)
            If systemMessage IsNot Nothing Then
                CType(udcInputHCVS.FindControl("ErrCoPaymentFee"), Label).Visible = True
                If Me.udcMsgBoxErr IsNot Nothing Then
                    If strMsgParam <> String.Empty Then
                        Me.udcMsgBoxErr.AddMessage(systemMessage, New String() {"%d"}, New String() {strMsgParam})
                    Else
                        Me.udcMsgBoxErr.AddMessage(systemMessage)
                    End If
                End If
                isValid = False
            End If
        End If

        If isValid Then
            'If Existing Transaction has Copayment Fee and New Transaction does not have Copayment Fee, then show error
            If udcInputHCVS.CoPaymentFee.ToString = String.Empty Then
                If udtEHSTransaction.TransactionAdditionFields.CoPaymentFee.HasValue Then
                    Me.udcMsgBoxErr.AddMessage("990000", "E", "00309")
                    CType(udcInputHCVS.FindControl("ErrCoPaymentFee"), Label).Visible = True
                    isValid = False
                End If
            End If
        End If

        CType(udcInputHCVS.FindControl("ErrVisitReason"), Label).Visible = False
        If isValid Then
            'If Existing Transaction has Reason For Visit and New Transaction does not have Reason For Visit, then show error
            If udtEHSTransaction.TransactionAdditionFields.ReasonForVisitCount = 0 Then
                If udtEHSTransactionOrginal IsNot Nothing Then
                    If udtEHSTransactionOrginal.TransactionAdditionFields IsNot Nothing Then
                        If udtEHSTransactionOrginal.TransactionAdditionFields.ReasonForVisitCount > 0 Then
                            Me.udcMsgBoxErr.AddMessage("990000", "E", "00273")
                            CType(udcInputHCVS.FindControl("ErrVisitReason"), Label).Visible = True
                            isValid = False
                        End If
                    End If
                End If
            End If
        End If

        If isValid Then
            Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ModifyTransactionConfirm
        Else
            Me.udcMsgBoxErr.BuildMessageBox()
        End If

    End Sub

    Private Sub btnClaimTransactionDetailVoid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClaimTransactionDetailVoid.Click
        Me.txtEnterVoidReasonVoidReason.Text = String.Empty
        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.EnterVoidReason
    End Sub

    Private Sub btnClaimTransactionDetailModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClaimTransactionDetailModify.Click
        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ModifyTransaction
    End Sub

    Private Sub btnModifyTransactionConfirmBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModifyTransactionConfirmBack.Click
        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ModifyTransaction
    End Sub

    Private Sub btnModifyTransactionConfirmSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModifyTransactionConfirmSave.Click
        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ModifyTransactionCompleted
    End Sub

    Private Sub btnModifyTransactionConfirmConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModifyTransactionConfirmConfirm.Click
        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ModifyTransactionCompleted
    End Sub

    Private Sub btnModifyTransactionCompletedReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnModifyTransactionCompletedReturn.Click
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        Me.BackToVoidClaimSearch()
    End Sub

    Private Sub SetupModifyTransaction(ByVal udtEHSTransaction As EHSTransactionModel, ByVal activeViewChanged As Boolean)
        Dim udtFormatter As Formatter = New Formatter()
        Dim udtEHSClaimBLL As EHSClaimBLL = New EHSClaimBLL()
        Dim isValid As Boolean = True

        Me.panPersonalInformation.Visible = True
        Me.udcMsgBoxInfo.BuildMessageBox()

        Me.lblModifyTransactionTransactionNo.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
        Me.lblModifyTransactionTransactionDate.Text = udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'Me.lblModifyTransactionServiceDate.Text = udtFormatter.formatDate(udtEHSTransaction.ServiceDate)
        Me.lblModifyTransactionServiceDate.Text = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
            Me.lblModifyTransactionPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeNameChi, udtEHSTransaction.PracticeID)
            Me.lblModifyTransactionPractice.CssClass = "tableTextChi"
        Else
            Me.lblModifyTransactionPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeName, udtEHSTransaction.PracticeID)
            Me.lblModifyTransactionPractice.CssClass = "tableText"
        End If

        Me.lblModifyTransactionScheme.Text = Me.GetSchemeCodeDescription(udtEHSTransaction.SchemeCode)
        Me.lblModifyTransactionBankAccountNo.Text = udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)
        If Me._udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            Me.lblModifyTransactionServiceType.Text = udtEHSTransaction.ServiceTypeDesc_Chi
        Else
            Me.lblModifyTransactionServiceType.Text = udtEHSTransaction.ServiceTypeDesc
        End If

        lblModifyTransactionTransactionStatus.Text = Me.GetTransactionStatusText(udtEHSTransaction.RecordStatus)
        FormatTransactionStatus(Me.lblModifyTransactionTransactionStatus, udtEHSTransaction.RecordStatus)
        ' Scheme-related fields
        'CRE13-001 EHAPP [Start][Karl]
        '--------------------------------------------------------------------------
        'If udtEHSTransaction.SchemeCode <> "HCVS" Then
        If udtEHSTransaction.SchemeCode <> Common.Component.Scheme.SchemeClaimModel.HCVS AndAlso udtEHSTransaction.SchemeCode <> Common.Component.Scheme.SchemeClaimModel.EHAPP Then
            'CRE13-001 EHAPP [End][Karl]
            Me.ucInputDeferredClaimDetails_Modify.EHSClaimVaccine = udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
        End If

        Me.ucInputDeferredClaimDetails_Modify.EHSTransaction = udtEHSTransaction
        Me.ucInputDeferredClaimDetails_Modify.SchemeType = udtEHSTransaction.SchemeCode
        Me.ucInputDeferredClaimDetails_Modify.TextOnlyVersion = True
        Me.ucInputDeferredClaimDetails_Modify.Built()
        ucInputDeferredClaimDetails_Modify.SetRebuildRequired()

    End Sub

    Private Sub SetupModifyTransactionConfirm(ByVal udtEHSTransaction As EHSTransactionModel, ByVal activeViewChanged As Boolean)
        Dim udtFormatter As Formatter = New Formatter()
        Dim udtEHSClaimBLL As EHSClaimBLL = New EHSClaimBLL()
        Dim udcInputHCVS As UIControl.EHCClaimText.ucInputHCVS = Me.ucInputDeferredClaimDetails_Modify.GetHCVSControl()

        'If Not activeViewChanged Then
        '    Me.SetupEHSAccount(udtEHSTransaction.EHSAcct, udtEHSTransaction.DocCode)
        'End If

        Me.panPersonalInformation.Visible = True
        Me.udcMsgBoxInfo.BuildMessageBox()

        Me.lblModifyTransactionConfirmTransactionNo.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
        Me.lblModifyTransactionConfirmTransactionDate.Text = udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'Me.lblModifyTransactionConfirmServiceDate.Text = udtFormatter.formatDate(udtEHSTransaction.ServiceDate)
        Me.lblModifyTransactionConfirmServiceDate.Text = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
            Me.lblModifyTransactionConfirmPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeNameChi, udtEHSTransaction.PracticeID)
            Me.lblModifyTransactionConfirmPractice.CssClass = "tableTextChi"
        Else
            Me.lblModifyTransactionConfirmPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeName, udtEHSTransaction.PracticeID)
            Me.lblModifyTransactionConfirmPractice.CssClass = "tableText"
        End If

        lblModifyTransactionConfirmTransactionStatus.Text = Me.GetTransactionStatusText(udtEHSTransaction.RecordStatus)
        FormatTransactionStatus(Me.lblModifyTransactionConfirmTransactionStatus, udtEHSTransaction.RecordStatus)

        If udcInputHCVS IsNot Nothing Then
            Dim udtTransactAdditionfield As TransactionAdditionalFieldModel = New TransactionAdditionalFieldModel()

            Dim strSchemeCode As String = String.Empty
            Dim strSchemeSeq As String = String.Empty
            Dim strSubsidizeCode As String = String.Empty

            If udtEHSTransaction.TransactionAdditionFields Is Nothing Then udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()
            Dim udtDetail As TransactionDetailModel = Nothing
            For Each udtTemp As TransactionDetailModel In udtEHSTransaction.TransactionDetails
                If udtTemp.SchemeCode.Trim = udtEHSTransaction.SchemeCode.Trim Then
                    udtDetail = udtTemp
                    Exit For
                End If
            Next
            strSchemeCode = udtDetail.SchemeCode.Trim
            strSchemeSeq = udtDetail.SchemeSeq
            strSubsidizeCode = udtDetail.SubsidizeCode.Trim

            If Me._udtGeneralFunction.IsCoPaymentFeeEnabled(udtEHSTransaction.ServiceDate) Then

                udtEHSTransaction.TransactionAdditionFields.RemoveCoPaymentFee()
                If udcInputHCVS.CoPaymentFee.Trim <> String.Empty Then
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.CoPaymentFee
                    udtTransactAdditionfield.AdditionalFieldValueCode = udcInputHCVS.CoPaymentFee
                    udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                    udtTransactAdditionfield.SchemeCode = strSchemeCode
                    udtTransactAdditionfield.SchemeSeq = strSchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = strSubsidizeCode
                    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
                End If
            End If
        End If


        Me.lblModifyTransactionConfirmScheme.Text = Me.GetSchemeCodeDescription(udtEHSTransaction.SchemeCode)
        Me.lblModifyTransactionConfirmBankAccountNo.Text = udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)
        If Me._udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            Me.lblModifyTransactionConfirmServiceType.Text = udtEHSTransaction.ServiceTypeDesc_Chi
        Else
            Me.lblModifyTransactionConfirmServiceType.Text = udtEHSTransaction.ServiceTypeDesc
        End If

        ' Scheme-related fields
        'CRE13-001 EHAPP [Start][Karl]
        '--------------------------------------------------------------------------
        'If udtEHSTransaction.SchemeCode <> "HCVS" Then
        If udtEHSTransaction.SchemeCode <> Common.Component.Scheme.SchemeClaimModel.HCVS AndAlso udtEHSTransaction.SchemeCode <> Common.Component.Scheme.SchemeClaimModel.EHAPP Then
            'CRE13-001 EHAPP [End][Karl]
            Me.udcModifyTransactionConfirmReadOnlyEHSClaim.EHSClaimVaccine = udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
        End If
        Me.udcModifyTransactionConfirmReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
        Me.udcModifyTransactionConfirmReadOnlyEHSClaim.SchemeCode = udtEHSTransaction.SchemeCode
        Me.udcModifyTransactionConfirmReadOnlyEHSClaim.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Normal
        Me.udcModifyTransactionConfirmReadOnlyEHSClaim.TextOnlyVersion = True
        Me.udcModifyTransactionConfirmReadOnlyEHSClaim.Built()
        ' -------------------------------------

        ' Rebuild scheme modify control
        'CRE13-001 EHAPP [Start][Karl]
        '--------------------------------------------------------------------------
        'If udtEHSTransaction.SchemeCode <> "HCVS" Then
        If udtEHSTransaction.SchemeCode <> Common.Component.Scheme.SchemeClaimModel.HCVS AndAlso udtEHSTransaction.SchemeCode <> Common.Component.Scheme.SchemeClaimModel.EHAPP Then
            'CRE13-001 EHAPP [End][Karl]
            Me.ucInputDeferredClaimDetails_Modify.EHSClaimVaccine = udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
        End If
        Me.ucInputDeferredClaimDetails_Modify.EHSTransaction = udtEHSTransaction
        Me.ucInputDeferredClaimDetails_Modify.SchemeType = udtEHSTransaction.SchemeCode
        Me.ucInputDeferredClaimDetails_Modify.TextOnlyVersion = True
        Me.ucInputDeferredClaimDetails_Modify.Built()
        ucInputDeferredClaimDetails_Modify.SetRebuildRequired()
        ' -------------------------------------

        Me.btnModifyTransactionConfirmSave.Visible = False
        Me.btnModifyTransactionConfirmConfirm.Visible = False
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            If Me.ucInputDeferredClaimDetails_Modify.IsIncomplete(udtEHSTransaction) Then
                Me.btnModifyTransactionConfirmSave.Visible = True
            Else
                Me.btnModifyTransactionConfirmConfirm.Visible = True
            End If
        Else
            Me.btnModifyTransactionConfirmSave.Visible = True
        End If
    End Sub

    Private Sub SetupModifyTransactionCompleted(ByVal udtEHSTransaction As EHSTransactionModel, ByVal activeViewChanged As Boolean)
        Dim udtEHSClaimBLL As EHSClaimBLL = New EHSClaimBLL()

        Dim udtFormatter As Formatter = New Formatter()

        Me.panPersonalInformation.Visible = False

        Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Complete
        'show complete message

        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim strAC_ID As String = String.Empty

        Dim udtServiceProvider As ServiceProviderModel = Nothing
        Dim udtDataEntry As DataEntryUserModel = Nothing
        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            udtServiceProvider = udtUserAC
            strAC_ID = udtServiceProvider.SPID
        Else
            udtDataEntry = udtUserAC
            strAC_ID = udtDataEntry.DataEntryAccount
        End If

        udtEHSTransaction.UpdateBy = strAC_ID
        udtEHSTransaction.UpdateDate = Now()

        Dim udtDB As Common.DataAccess.Database
        Try

            udtDB = New Common.DataAccess.Database

            udtDB.BeginTransaction()
            Me.udtEHSTransactionBLL.UpdateEHSTransaction(udtDB, udtEHSTransaction)
            udtEHSTransaction = Me.udtEHSTransactionBLL.LoadClaimTran(udtEHSTransaction.TransactionID, False, False, udtDB)

            udtEHSTransaction.UpdateBy = strAC_ID
            udtEHSTransaction.UpdateDate = Now()

            ' Save transaction 
            ' --------------------------------------------------------------------------------
            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                ' if SP
                ' --------------------------------------------------------------------------------
                'If udtEHSTransaction.ServiceDate >= New Date(2012, 1, 1) And (udtEHSTransaction.CoPaymentFee = String.Empty Or udtEHSTransaction.TransactionAdditionFields.Count = 0) Then
                '    ' Defer
                '    udcInfoMsgBox.AddMessage("020301", "I", "00003")
                'ElseIf udtEHSTransaction.ServiceDate < New Date(2012, 1, 1) And (udtEHSTransaction.TransactionAdditionFields.Count = 0) Then

                Me.SetupModifyTransaction(udtEHSTransaction, False)
                If Me.ucInputDeferredClaimDetails_Modify.IsIncomplete(udtEHSTransaction) Then
                    ' Defer
                    Me.udcMsgBoxInfo.AddMessage("020301", "I", "00003")
                Else

                    ' Complete Information
                    'If udtEHSTransaction.TempVoucherAccID = String.Empty Then
                    '    ' Valicated Account
                    '    Me.udtEHSTransactionBLL.UpdateEHSTransactionStatus(udtEHSTransaction.TransactionID, EHSTransactionModel.TransRecordStatusClass.Active, udtEHSTransaction.UpdateBy, udtEHSTransaction.UpdateDate, udtEHSTransaction.TSMP)
                    'Else
                    '    ' Temp Account
                    '    Me.udtEHSTransactionBLL.UpdateEHSTransactionStatus(udtEHSTransaction.TransactionID, EHSTransactionModel.TransRecordStatusClass.PendingVRValidate, udtEHSTransaction.UpdateBy, udtEHSTransaction.UpdateDate, udtEHSTransaction.TSMP)
                    'End If

                    Me.udtEHSTransactionBLL.UpdateEHSTransactionStatus(udtEHSTransaction.TransactionID, EHSTransactionModel.TransRecordStatusClass.Pending, udtEHSTransaction.UpdateBy, udtEHSTransaction.UpdateDate, udtEHSTransaction.TSMP, udtDB)
                    'Dim udtRecordConfirmationBLL As New RecordConfirmationBLL
                    'udtRecordConfirmationBLL.UpdateTransactionStatus(udtEHSTransaction.TransactionID, udtEHSTransaction.UpdateDate, udtEHSTransaction.UpdateBy, EHSTransactionModel.TransRecordStatusClass.Pending, udtEHSTransaction.TSMP, udtDB)

                    ' Confirm transaction
                    Dim udtRecordConfirmationBLL As New RecordConfirmationBLL
                    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Dim dtmClaimConfirmationDate As DateTime = udtRecordConfirmationBLL.ConfirmTransaction(udtEHSTransaction.ServiceProviderID, udtEHSTransaction.TransactionID, udtDB)
                    Dim dtmClaimConfirmationDate As DateTime = udtRecordConfirmationBLL.ConfirmTransaction(udtEHSTransaction.ServiceProviderID, udtEHSTransaction.TransactionID, Me.SubPlatform, udtDB)
                    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                    Me.udcMsgBoxInfo.AddMessage("020301", "I", "00004") ' Claim completed and confirmed. Please refer to the following information. (TBC)
                End If

            Else
                ' if Data Entry
                ' --------------------------------------------------------------------------------
                'If udtEHSTransaction.ServiceDate >= New Date(2012, 1, 1) And (udtEHSTransaction.CoPaymentFee = String.Empty Or udtEHSTransaction.TransactionAdditionFields.Count = 0) Then
                '    ' Defer
                '    udcInfoMsgBox.AddMessage("020301", "I", "00003")
                'ElseIf udtEHSTransaction.ServiceDate < New Date(2012, 1, 1) And (udtEHSTransaction.TransactionAdditionFields.Count = 0) Then
                If Me.ucInputDeferredClaimDetails_Modify.IsIncomplete(udtEHSTransaction) Then
                    ' Defer
                    Me.udcMsgBoxInfo.AddMessage("020301", "I", "00003")
                Else
                    ' Complete Information, pending SP to confirm
                    Me.udtEHSTransactionBLL.UpdateEHSTransactionStatus(udtEHSTransaction.TransactionID, EHSTransactionModel.TransRecordStatusClass.Pending, udtEHSTransaction.UpdateBy, udtEHSTransaction.UpdateDate, udtEHSTransaction.TSMP, udtDB)
                    'Dim udtRecordConfirmationBLL As New RecordConfirmationBLL
                    'udtRecordConfirmationBLL.UpdateTransactionStatus(udtEHSTransaction.TransactionID, udtEHSTransaction.UpdateDate, udtEHSTransaction.UpdateBy, EHSTransactionModel.TransRecordStatusClass.Pending, udtEHSTransaction.TSMP, udtDB)
                    Me.udcMsgBoxInfo.AddMessage("020301", "I", "00005") ' Claim completed. Please refer to the following information. (TBC)
                End If

            End If
            udtDB.CommitTransaction()
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
        'If udtEHSTransaction.EHSAcct.RecordStatus = Common.Component.EHSAccount.EHSAccountModel.TempAccountRecordStatusClass.Removed Then
        '    If Not udtEHSTransaction.EHSAcct.OriginalAccID Is Nothing AndAlso Not udtEHSTransaction.EHSAcct.OriginalAccID.Equals(String.Empty) Then
        '        Me.udcMsgBoxInfo.AddMessage(New Common.ComObject.SystemMessage("020302", "I", "00001"), New String() {"%s"}, New Object() {udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim())})
        '    Else
        '        Me.udcMsgBoxInfo.AddMessage(New Common.ComObject.SystemMessage("020302", "I", "00002"), New String() {"%r", "%s"}, New Object() {udtFormatter.formatSystemNumber(udtEHSTransaction.EHSAcct.VoucherAccID.Trim()), udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim())})
        '    End If
        'Else
        '    Me.udcMsgBoxInfo.AddMessage(New Common.ComObject.SystemMessage("020302", "I", "00001"), New String() {"%s"}, New Object() {udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim())})
        'End If

        Me.udcMsgBoxInfo.BuildMessageBox()

        Me.lblModifyTransactionConpletedTransactionNo.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.TransactionID)
        Me.lblModifyTransactionConpletedTransactionDate.Text = udtFormatter.formatDateTime(udtEHSTransaction.TransactionDtm)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'Me.lblModifyTransactionConpletedServiceDate.Text = udtFormatter.formatDate(udtEHSTransaction.ServiceDate)
        Me.lblModifyTransactionConpletedServiceDate.Text = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
            Me.lblModifyTransactionConpletedPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeNameChi, udtEHSTransaction.PracticeID)
            Me.lblModifyTransactionConpletedPractice.CssClass = "tableTextChi"
        Else
            Me.lblModifyTransactionConpletedPractice.Text = String.Format("{0} ({1})", udtEHSTransaction.PracticeName, udtEHSTransaction.PracticeID)
            Me.lblModifyTransactionConpletedPractice.CssClass = "tableText"
        End If

        Me.lblModifyTransactionConpletedScheme.Text = Me.GetSchemeCodeDescription(udtEHSTransaction.SchemeCode)
        Me.lblModifyTransactionConpletedBankAccountNo.Text = udtFormatter.maskBankAccount(udtEHSTransaction.BankAccountNo)
        If Me._udtSessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
            Me.lblModifyTransactionConpletedServiceType.Text = udtEHSTransaction.ServiceTypeDesc_Chi
        Else
            Me.lblModifyTransactionConpletedServiceType.Text = udtEHSTransaction.ServiceTypeDesc
        End If

        'Reload Transaction from DB to get the latest Transaction Status
        udtEHSTransaction = udtEHSTransactionBLL.LoadClaimTran(udtEHSTransaction.TransactionID)

        lblModifyTransactionConpletedTransactionStatus.Text = Me.GetTransactionStatusText(udtEHSTransaction.RecordStatus)
        FormatTransactionStatus(Me.lblModifyTransactionConpletedTransactionStatus, udtEHSTransaction.RecordStatus)

        ' Scheme-related fields
        'CRE13-001 EHAPP [Start][Karl]
        '--------------------------------------------------------------------------
        'If udtEHSTransaction.SchemeCode <> "HCVS" Then
        If udtEHSTransaction.SchemeCode <> Common.Component.Scheme.SchemeClaimModel.HCVS AndAlso udtEHSTransaction.SchemeCode <> Common.Component.Scheme.SchemeClaimModel.EHAPP Then
            'CRE13-001 EHAPP [End][Karl]
            Me.udcModifyTransactionConpletedReadOnlyEHSClaim.EHSClaimVaccine = udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
        End If
        Me.udcModifyTransactionConpletedReadOnlyEHSClaim.EHSTransaction = udtEHSTransaction
        Me.udcModifyTransactionConpletedReadOnlyEHSClaim.SchemeCode = udtEHSTransaction.SchemeCode
        Me.udcModifyTransactionConpletedReadOnlyEHSClaim.Mode = ucReadOnlyEHSClaim.ReadOnlyEHSClaimMode.Normal
        Me.udcModifyTransactionConpletedReadOnlyEHSClaim.TextOnlyVersion = True
        Me.udcModifyTransactionConpletedReadOnlyEHSClaim.Built()

    End Sub

#End Region

    ' Event
    Private Sub udcReasonForVisit_ReturnButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles udcReasonForVisit.ReturnButtonClickForCTM
        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ModifyTransaction
    End Sub

    Private Sub udcReasonForVisit_ConfirmButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles udcReasonForVisit.ConfirmButtonClickForCTM
        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ModifyTransaction
    End Sub

    Private Function GetTransactionStatusText(ByVal strRecordStatus As String) As String
        Dim strTransactionStatusText As String
        Select Case strRecordStatus
            Case Common.Component.ClaimTransStatus.Incomplete
                strTransactionStatusText = Me.GetGlobalResourceObject("Text", "Incomplete")
            Case Common.Component.ClaimTransStatus.Active
                strTransactionStatusText = Me.GetGlobalResourceObject("Text", "ReadytoReimburse")
            Case Common.Component.ClaimTransStatus.Inactive
                strTransactionStatusText = Me.GetGlobalResourceObject("Text", "Voided")
            Case Common.Component.ClaimTransStatus.Pending
                strTransactionStatusText = Me.GetGlobalResourceObject("Text", "PendingConfirmation")
            Case Common.Component.ClaimTransStatus.PendingVRValidate
                strTransactionStatusText = Me.GetGlobalResourceObject("Text", "PendingVoucherAccountValidation")
            Case Common.Component.ClaimTransStatus.Reimbursed
                strTransactionStatusText = Me.GetGlobalResourceObject("Text", "Reimbursed")
            Case Common.Component.ClaimTransStatus.Suspended
                strTransactionStatusText = Me.GetGlobalResourceObject("Text", "ClaimSuspended")
            Case Common.Component.ClaimTransStatus.ManualReimbursedClaim
                strTransactionStatusText = Me.GetGlobalResourceObject("Text", "ManualReimbursedClaim")
                ' CRE13-001 - EHAPP [Start][Koala]
                ' -------------------------------------------------------------------------------------
            Case Common.Component.ClaimTransStatus.Joined
                strTransactionStatusText = Me.GetGlobalResourceObject("Text", "Joined")
                ' CRE13-001 - EHAPP [End][Koala]
            Case Else
                strTransactionStatusText = "Unclassified"
        End Select

        Return strTransactionStatusText
    End Function

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

#Region "Complete Void "

    Private Sub btnCompleteReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCompleteReturn.Click
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        Me.BackToVoidClaimSearch()
    End Sub

    Private Sub SetupCompleteVoid(ByVal udtEHSTransaction As EHSTransactionModel, ByVal activeViewChanged As Boolean)
        Dim udtFormatter As Formatter = New Formatter()

        Me.panPersonalInformation.Visible = False

        Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Complete
        'show complete message
        If udtEHSTransaction.EHSAcct.RecordStatus = Common.Component.EHSAccount.EHSAccountModel.TempAccountRecordStatusClass.Removed Then
            If Not udtEHSTransaction.EHSAcct.OriginalAccID Is Nothing AndAlso Not udtEHSTransaction.EHSAcct.OriginalAccID.Equals(String.Empty) Then
                Me.udcMsgBoxInfo.AddMessage(New Common.ComObject.SystemMessage("020302", "I", "00001"), New String() {"%s"}, New String() {udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim())})
            Else
                Me.udcMsgBoxInfo.AddMessage(New Common.ComObject.SystemMessage("020302", "I", "00002"), New String() {"%r", "%s"}, New String() {udtFormatter.formatSystemNumber(udtEHSTransaction.EHSAcct.VoucherAccID.Trim()), udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim())})
            End If
        Else
            Me.udcMsgBoxInfo.AddMessage(New Common.ComObject.SystemMessage("020302", "I", "00001"), New String() {"%s"}, New String() {udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim())})
        End If

        Me.udcMsgBoxInfo.BuildMessageBox()

        Me.lblCompleteVoidTranID.Text = udtFormatter.formatSystemNumber(udtEHSTransaction.VoidTranNo.Trim())
        Me.lblCompleteVoidBy.Text = udtEHSTransaction.VoidUser
        Me.lblCompleteVoidDtm.Text = udtFormatter.convertDateTime(udtEHSTransaction.VoidDate)
        Me.lblCompleteVoidReason.Text = udtEHSTransaction.VoidReason

        If activeViewChanged Then
            AuditLogCompleteVoid(udtEHSTransaction)
        End If

    End Sub

#End Region

#Region "comfirm Message"

    Protected Sub lblConfirmMessageConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblConfirmMessageConfirm.Click
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        Me.BackToVoidClaimSearch()
    End Sub

    Protected Sub lblConfirmMessageBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblConfirmMessageBack.Click
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.EnterVoidReason
    End Sub

    Private Sub SetupConfirmMessage()
        Me.panPersonalInformation.Visible = False
    End Sub

#End Region

#Region "Remarks"

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Handle remark click in Transaction Detail
    Private Sub udcReasonReadOnlyEHSClaim_VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles udcEnterVoidReasonReadOnlyEHSClaim.VaccineRemarkClicked, udcConfirmVoidReasonBankReadOnlyEHSClaim.VaccineRemarkClicked, udcClaimTransactionDetailReadOnlyEHSClaim.VaccineRemarkClicked
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        ' Update Session Index if the value is different
        Me._udtSessionHandler.EHSClaimStepsSaveToSession(FunctCode, Me.mvVoidTranDetail.ActiveViewIndex)
        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.Remark

    End Sub
    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

    ' Event
    Private Sub btnViewRemarkBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewRemarkBack.Click
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        ' Cancel Clicked, move to the previous View
        Dim intView As Integer = Me._udtSessionHandler.EHSClaimStepsGetFromSession(FunctCode)

        Me.mvVoidTranDetail.ActiveViewIndex = intView

    End Sub

    Private Sub ShowVaccineRemarkText()

        Dim tblRemark As Table = New Table
        Dim tr As TableRow
        Dim td As TableCell

        Me.panPersonalInformation.Visible = False

        ' Table Property
        tblRemark.CellPadding = 0
        tblRemark.CellSpacing = 0
        tblRemark.CssClass = "textVersionTable"

        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        Dim udtEHSClaimBLL As EHSClaimBLL = New EHSClaimBLL()
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = udtEHSClaimBLL.ConstructEHSClaimVaccineModel(udtEHSTransaction.SchemeCode, udtEHSTransaction)
        If Not udtEHSClaimVaccine Is Nothing AndAlso Not udtEHSClaimVaccine.SubsidizeList Is Nothing Then
            For Each udtEHSClaimSubsidizeItem As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList

                If tblRemark.Controls.Count > 0 Then
                    ' Add Seperator
                    tr = New TableRow()
                    td = New TableCell()
                    td.Controls.Add(New HtmlControls.HtmlGenericControl("HR"))
                    tr.Controls.Add(td)
                    tblRemark.Controls.Add(tr)
                End If

                ' Subsidy Title (Bold)
                tr = New TableRow()
                td = New TableCell()
                td.CssClass = "tableTitle"
                td.Text = Me.GetGlobalResourceObject("Text", "Vaccine")
                tr.Controls.Add(td)
                tblRemark.Controls.Add(tr)

                ' Subsidize Code (Regular Text)
                tr = New TableRow()
                td = New TableCell()
                td.CssClass = "tableText"
                td.Text = udtEHSClaimSubsidizeItem.SubsidizeDisplayCode
                tr.Controls.Add(td)
                tblRemark.Controls.Add(tr)

                'CRE16-026 (Add PCV13) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If udtEHSClaimSubsidizeItem.SchemeCode <> SchemeClaimModel.RVP Then
                    ' Subsidize Remark (Regular Text)
                    tr = New TableRow()
                    td = New TableCell()
                    td.CssClass = "tableText"
                    td.Text = IIf(_udtSessionHandler.Language() = CultureLanguage.English, FormatSubsidyRemark(udtEHSClaimSubsidizeItem.Remark), FormatSubsidyRemark(udtEHSClaimSubsidizeItem.RemarkChi))
                    tr.Controls.Add(td)
                    tblRemark.Controls.Add(tr)
                End If
                'CRE16-026 (Add PCV13) [End][Chris YIM]

                ' Subsidize Admount Header (Bold)
                tr = New TableRow()
                td = New TableCell()
                td.CssClass = "tableTitle"
                'CRE16-026 (Add PCV13) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                td.Text = IIf(udtEHSClaimSubsidizeItem.SchemeCode = SchemeClaimModel.RVP, HttpContext.GetGlobalResourceObject("Text", "InjectionCost"), HttpContext.GetGlobalResourceObject("Text", "SubsidyAmount"))
                'CRE16-026 (Add PCV13) [End][Chris YIM]
                tr.Controls.Add(td)
                tblRemark.Controls.Add(tr)

                ' Subsidize Admount (Regular Text)
                tr = New TableRow()
                td = New TableCell()
                td.CssClass = "tableText"
                td.Text = String.Format("${0}", Convert.ToInt32(udtEHSClaimSubsidizeItem.Amount))
                tr.Controls.Add(td)
                tblRemark.Controls.Add(tr)
            Next
        End If

        ' Put the dynamic control into the remark place holder
        Me.plRemarkContainer.Controls.Add(tblRemark)

    End Sub

    Private Function FormatSubsidyRemark(ByVal strSubsidizeItemRemark As String) As String
        ' Separator According to "SearchEHSClaimVaccine" in "EHSClaimBLL"
        Dim strRemarks As String() = strSubsidizeItemRemark.Replace("<br>", "|").Split("|")
        Dim strResult As StringBuilder = New StringBuilder()

        For Each strRemark As String In strRemarks
            If strResult.Length > 0 Then
                strResult.AppendFormat("<br/>")
            End If

            If strRemark.Length > 0 Then
                If strRemark.StartsWith("(") AndAlso strRemark.EndsWith(")") Then
                    strResult.Append(strRemark)
                Else
                    strResult.AppendFormat("({0})", strRemark)
                End If
            End If
        Next

        Return strResult.ToString()

    End Function

#End Region

#Region "Internal Error"

    Private Sub SetupInternalError()
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        panPersonalInformation.Visible = False

        If udtEHSTransaction Is Nothing Then
            Me.udcMsgBoxInfo.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcMsgBoxInfo.AddMessage(New SystemMessage("990001", "D", "00011"))
            Me.udcMsgBoxInfo.BuildMessageBox()
        End If

    End Sub

    Protected Sub btnInternalErrorBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInternalErrorBack.Click
        If (MyBase.IsPageRefreshed) Then
            _blnIsRequireHandlePageRefresh = True
            Return
        End If

        Me.BackToVoidClaimSearch()
    End Sub

#End Region

#Region "Other function"
    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub SetupEHSAccount(ByVal udtEHSTransaction As EHSTransactionModel)
        If udtEHSTransaction.DocCode.Trim = DocTypeModel.DocTypeCode.HKIC Then
            udcTranDetailReadOnlyDocumnetType.SetEnableToShowHKICSymbol = True
            udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).HKICSymbol = udtEHSTransaction.HKICSymbol
        End If

        Me.udcTranDetailReadOnlyDocumnetType.DocumentType = udtEHSTransaction.DocCode.Trim
        Me.udcTranDetailReadOnlyDocumnetType.EHSAccount = udtEHSTransaction.EHSAcct
        Me.udcTranDetailReadOnlyDocumnetType.Vertical = False
        Me.udcTranDetailReadOnlyDocumnetType.MaskIdentityNo = True
        Me.udcTranDetailReadOnlyDocumnetType.ShowAccountRefNo = False
        Me.udcTranDetailReadOnlyDocumnetType.ShowTempAccountNotice = False
        Me.udcTranDetailReadOnlyDocumnetType.ShowAccountCreationDate = False
        Me.udcTranDetailReadOnlyDocumnetType.Mode = ucInputDocTypeBase.BuildMode.Modification
        Me.udcTranDetailReadOnlyDocumnetType.TableTitleWidth = 200
        Me.udcTranDetailReadOnlyDocumnetType.TextOnlyVersion = True
        Me.udcTranDetailReadOnlyDocumnetType.Built()
    End Sub
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    Private Sub BackToVoidClaimSearch()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        Me._udtSessionHandler.EHSClaimSessionRemove(FunctCode)
        RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.SearchTransation)

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    End Sub

    Private Function GetSchemeCodeDescription(ByVal strSchemeCode As String) As String
        Dim udtSchemeClaimBLL As Scheme.SchemeClaimBLL = New Scheme.SchemeClaimBLL()

        'INT14-0017 Fix HSCP Claim Trans Management search for expired scheme [Start][Karl]
        'For Each udtSchemeClaim As Scheme.SchemeClaimModel In udtSchemeClaimBLL.getAllEffectiveSchemeClaim()
        For Each udtSchemeClaim As Scheme.SchemeClaimModel In udtSchemeClaimBLL.getAllDistinctSchemeClaim()
            'INT14-0017 Fix HSCP Claim Trans Management search for expired scheme [End][Karl]
            If udtSchemeClaim.SchemeCode.Trim = strSchemeCode.Trim() Then
                If Me._udtSessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
                    Return udtSchemeClaim.SchemeDescChi
                Else
                    Return udtSchemeClaim.SchemeDesc
                End If
            End If
        Next
        Return String.Empty
    End Function

    Private Sub BuildMenu()
        Dim masterPage As ClaimVoucherMaster = CType(Me.Master, ClaimVoucherMaster)
        masterPage.BuildMenu(FunctCode, Me._udtSessionHandler.Language())
        AddHandler masterPage.MenuChanged, AddressOf MasterPage_MenuChanged
    End Sub

    Private Function GetStepText() As String
        Select Case Me.mvVoidTranDetail.ActiveViewIndex
            Case ActiveViewIndex.EnterVoidReason, ActiveViewIndex.ConfirmVoidReason
                Return Me.GetGlobalResourceObject("Text", "VoidClaimStep2")
            Case ActiveViewIndex.CompleteVoid
                Return Me.GetGlobalResourceObject("Text", "VoidClaimStep3")
            Case Else
                Return String.Empty
        End Select
    End Function

    ' Handle Page Refresh
    Private Sub HandlePageRefreshed()

        Me.BackToVoidClaimSearch()

    End Sub

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Sub setupDisplayRecipientCondition(ByVal udtEHSTransaction As EHSTransactionModel, ByRef panRecipientCondition As Panel, ByRef lblRecipientCondition As Label)
        Dim strRecipientCondition As String = String.Empty

        If Not udtEHSTransaction.HighRisk Is Nothing Then
            Select Case udtEHSTransaction.HighRisk
                Case YesNo.Yes
                    strRecipientCondition = HighRiskOption.HIGHRISK
                Case YesNo.No
                    strRecipientCondition = HighRiskOption.NOHIGHRISK
            End Select
        End If

        'Display Recipient Condition
        If strRecipientCondition <> String.Empty Then

            panRecipientCondition.Visible = True

            Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("VSS_RECIPIENTCONDITION", strRecipientCondition)

            Select Case _udtSessionHandler.Language
                Case CultureLanguage.TradChinese
                    lblRecipientCondition.Text = udtStaticDataModel.DataValueChi
                Case CultureLanguage.SimpChinese
                    lblRecipientCondition.Text = udtStaticDataModel.DataValueCN
                Case Else
                    lblRecipientCondition.Text = udtStaticDataModel.DataValue
            End Select

        End If

    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    Sub setupDisplayContactNo(ByVal udtEHSTransaction As EHSTransactionModel, ByRef panContact As Panel, ByRef lblContact As Label)
        panContact.Visible = False

        'ContactNo
        Dim strContactNo As String = udtEHSTransaction.TransactionAdditionFields.ContactNo
        If strContactNo IsNot Nothing AndAlso strContactNo <> String.Empty Then
            panContact.Visible = True
            lblContact.Text = strContactNo
        Else
            panContact.Visible = False
        End If
    End Sub

    Sub setupDisplayRemarks(ByVal udtEHSTransaction As EHSTransactionModel, ByRef panRemarks As Panel, ByRef lblRemarks As Label)
        panRemarks.Visible = False

        'Remarks
        Dim strRemarks As String = udtEHSTransaction.TransactionAdditionFields.Remarks
        If strRemarks Is Nothing Then
            panRemarks.Visible = False
        Else
            panRemarks.Visible = True
            If strRemarks <> String.Empty Then
                lblRemarks.Text = strRemarks
            ElseIf strRemarks = String.Empty Then
                lblRemarks.Text = GetGlobalResourceObject("Text", "NotProvided")
            End If
        End If

    End Sub


#End Region

#Region "Audit Log"

    'Select Transaction : LOG00010 : Start
    Public Sub AuditLogEnterVoidReasonStart(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strVoidReason As String)
        udtAuditLogEntry.AddDescripton("Void Reason", strVoidReason)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00010, "Enter Void Reason start")
    End Sub

    'Select Transaction : LOG00011 : Complete
    Public Sub AuditLogEnterVoidReasonComplete(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel)
        udtAuditLogEntry.AddDescripton("Void Reason", udtEHSTransaction.VoidReason)

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00011, "Enter Void Reason Complete")

    End Sub

    'Select Transaction : LOG00012 : Enter Void Reason Failed

    'Select Transaction : LOG00013 : Start
    Public Sub AuditLogConfirmVoidReasonStart(ByRef udtAuditLogEntry As AuditLogEntry)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00013, "Confirma Void Reason start")
    End Sub

    'Select Transaction : LOG00014 : Complete
    Public Sub AuditLogConfirmVoidReasonComplete(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtEHSTransaction As EHSTransactionModel)
        udtAuditLogEntry.AddDescripton("Transaction No", udtEHSTransaction.TransactionID)
        udtAuditLogEntry.AddDescripton("Transaction Date", udtEHSTransaction.TransactionDtm)
        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSTransaction.DocCode)
        udtAuditLogEntry.AddDescripton("Create By", udtEHSTransaction.CreateBy)
        udtAuditLogEntry.AddDescripton("Update By", udtEHSTransaction.UpdateBy)
        udtAuditLogEntry.AddDescripton("Date Entry By", udtEHSTransaction.DataEntryBy)
        udtAuditLogEntry.AddDescripton("Service Date", udtEHSTransaction.ServiceDate)
        udtAuditLogEntry.AddDescripton("Void Transaction No.", udtEHSTransaction.VoidTranNo)
        udtAuditLogEntry.AddDescripton("Void Date", udtEHSTransaction.VoidDate)
        udtAuditLogEntry.AddDescripton("Void User", udtEHSTransaction.VoidUser)
        udtAuditLogEntry.AddDescripton("Void DataEntry", udtEHSTransaction.VoidByDataEntry)
        udtAuditLogEntry.AddDescripton("Void Reason", udtEHSTransaction.VoidReason)

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode)
            Case SchemeClaimModel.EnumControlType.VOUCHER
                udtAuditLogEntry = EHSClaimBasePage.AuditLogHCVS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                'No text version

            Case SchemeClaimModel.EnumControlType.EVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogEVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.CIVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogCIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.HSIVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogHSIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.RVP
                udtAuditLogEntry = EHSClaimBasePage.AuditLogRVP(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PIDVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogPIDVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                udtAuditLogEntry = EHSClaimBasePage.AuditLogENHVSSO(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PPP
                'No text version

                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                ' --------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.COVID19, SchemeClaimModel.EnumControlType.COVID19RVP, SchemeClaimModel.EnumControlType.COVID19OR
                'No text version
                ' CRE20-0022 (Immu record) [End][Winnie SUEN]

        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]


        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00014, "Enter Void Reason Complete")

    End Sub

    'Select Transaction : LOG00015 : Confirm Void Reason Failed
    Public Sub AuditLogConfirmVoidReasonFailed(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strMessage As String)
        udtAuditLogEntry.AddDescripton("Description", strMessage)
        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00015, "Confirma Void Reason Failed")
    End Sub


    'Complete Void Transaction : LOG00016 
    Public Sub AuditLogCompleteVoid(ByVal udtEHSTransaction As EHSTransactionModel)
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(Common.Component.FunctCode.FUNT020302, Me)
        udtAuditLogEntry.AddDescripton("Transaction No", udtEHSTransaction.TransactionID)
        udtAuditLogEntry.AddDescripton("Transaction Date", udtEHSTransaction.TransactionDtm)
        udtAuditLogEntry.AddDescripton("Doc Code", udtEHSTransaction.DocCode)
        udtAuditLogEntry.AddDescripton("Create By", udtEHSTransaction.CreateBy)
        udtAuditLogEntry.AddDescripton("Update By", udtEHSTransaction.UpdateBy)
        udtAuditLogEntry.AddDescripton("Date Entry By", udtEHSTransaction.DataEntryBy)
        udtAuditLogEntry.AddDescripton("Service Date", udtEHSTransaction.ServiceDate)
        udtAuditLogEntry.AddDescripton("Void Transaction No.", udtEHSTransaction.VoidTranNo)
        udtAuditLogEntry.AddDescripton("Void Date", udtEHSTransaction.VoidDate)
        udtAuditLogEntry.AddDescripton("Void User", udtEHSTransaction.VoidUser)
        udtAuditLogEntry.AddDescripton("Void DataEntry", udtEHSTransaction.VoidByDataEntry)
        udtAuditLogEntry.AddDescripton("Void Reason", udtEHSTransaction.VoidReason)

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode)
            Case SchemeClaimModel.EnumControlType.VOUCHER
                udtAuditLogEntry = EHSClaimBasePage.AuditLogHCVS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VOUCHERCHINA
                'no text version

            Case SchemeClaimModel.EnumControlType.EVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogEVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.CIVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogCIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.HSIVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogHSIVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.RVP
                udtAuditLogEntry = EHSClaimBasePage.AuditLogRVP(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PIDVSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogPIDVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.VSS
                udtAuditLogEntry = EHSClaimBasePage.AuditLogVSS(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.ENHVSSO
                udtAuditLogEntry = EHSClaimBasePage.AuditLogENHVSSO(udtAuditLogEntry, udtEHSTransaction)

            Case SchemeClaimModel.EnumControlType.PPP
                'no text version

                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
                ' --------------------------------------------------------------------------------------
            Case SchemeClaimModel.EnumControlType.COVID19, SchemeClaimModel.EnumControlType.COVID19RVP, SchemeClaimModel.EnumControlType.COVID19OR
                'No text version
                ' CRE20-0022 (Immu record) [End][Winnie SUEN]

        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00016, "Complete Void Transaction")

    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Protected Sub ucInputDeferredClaimDetails_Modify_SelectReasonForVisitClick(ByVal sender As Object, ByVal e As EventArgs) Handles ucInputDeferredClaimDetails_Modify.SelectReasonForVisitClicked
        Me.udcMsgBoxErr.Clear()
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        If udtGeneralFunction.IsCoPaymentFeeEnabled(Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode).ServiceDate) Then
            Me.udcReasonForVisit.Mode(UIControl.EHCClaimText.ucReasonForVisit.EnumMode.AfterCopaymentFeeEnabled)
        Else
            Me.udcReasonForVisit.Mode(UIControl.EHCClaimText.ucReasonForVisit.EnumMode.BeforeCopaymentFeeEnabled)
        End If
        Me.udcReasonForVisit.Build()
        Me.mvVoidTranDetail.ActiveViewIndex = ActiveViewIndex.ReasonForVisit

    End Sub

    ' Function
    Private Sub SetupReasonForVisit()

        Dim udtSchemeClaim As SchemeClaimModel = _udtSessionHandler.SchemeSelectedGetFromSession(FunctionCode)
        Dim udtEHSAccount As EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)

        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtSelectedPracticeDisplay As PracticeDisplayModel = _udtSessionHandler.PracticeDisplayGetFromSession(FunctionCode)
        ' CRE19-003 (Opt voucher capping) [End][Winnie]

        Dim blnIsAvailable As Boolean = False

        ' Build control only when the scheme have subsidize available
        If Not udtSchemeClaim Is Nothing AndAlso Not udtSchemeClaim.SubsidizeGroupClaimList Is Nothing AndAlso Not udtEHSAccount Is Nothing Then

            Select Case udtSchemeClaim.ControlType

                Case SchemeClaimModel.EnumControlType.VOUCHER ', SchemeClaimModel.EnumControlType.VOUCHERCHINA
                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    If udtEHSAccount.VoucherInfo Is Nothing Then
                        'Dim udtEHSTransactionBLL As New EHSTransactionBLL()

                        Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                                   VoucherInfoModel.AvailableQuota.Include)

                        udtVoucherInfo.GetInfo(_udtSessionHandler.SchemeSelectedGetFromSession(FunctionCode), _
                                               udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode), _
                                               udtSelectedPracticeDisplay.ServiceCategoryCode)

                        udtEHSAccount.VoucherInfo = udtVoucherInfo

                        _udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
                    End If

                    If udtEHSAccount.VoucherInfo.GetAvailableVoucher() > 0 Then
                        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        Dim udtVoucherQuota As VoucherQuotaModel = udtEHSAccount.VoucherInfo.VoucherQuotalist.Filter(udtSelectedPracticeDisplay.ServiceCategoryCode)

                        If Not udtVoucherQuota Is Nothing Then
                            If udtVoucherQuota.AvailableQuota > 0 Then
                                blnIsAvailable = True
                            End If
                        Else
                            blnIsAvailable = True
                        End If
                        ' CRE19-003 (Opt voucher capping) [End][Winnie]?
                    End If
                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]
            End Select

        End If


        If blnIsAvailable Then
            Me.udcReasonForVisit.IsSupportedDevice = _udtSessionHandler.IsMobileDeviceGetFromSession
            Me.udcReasonForVisit.AvaliableForClaim = blnIsAvailable
            Me.udcReasonForVisit.CurrentPractice = _udtSessionHandler.PracticeDisplayGetFromSession(FunctionCode)
            Me.udcReasonForVisit.EHSAccount = udtEHSAccount
            Me.udcReasonForVisit.EHSTransaction = _udtSessionHandler.EHSTransactionGetFromSession(FunctionCode)
            Me.udcReasonForVisit.Build()
            'Else
            '    Me.ClearReasonForVisit()
        End If

    End Sub


    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Private Sub ClearReasonForVisit()
        Me.udcReasonForVisit.Clear()
    End Sub

#End Region

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        If Not IsNothing(udtEHSTransaction) Then
            Return udtEHSTransaction.DocCode
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Dim udtEHSTransaction As EHSTransactionModel = Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
        If Not IsNothing(udtEHSTransaction) Then
            Return udtEHSTransaction.EHSAcct
        Else
            Return Nothing
        End If
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Me._udtSessionHandler.EHSTransactionGetFromSession(FunctCode)
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

    Public Overrides Sub ClearWorkingData()
        MyBase.ClearWorkingData()
        Me._udtSessionHandler.EHSTransactionRemoveFromSession(FunctCode)
    End Sub

#End Region

    ' CRE20-0XX (Immu record)  [Start][Raiman]
    ' -----------------------------------------------------------------------------------------
    Public Function IsClaimCOVID19(udtEHSTransaction) As Boolean

        Dim udtTranDetailList As TransactionDetailModelCollection = udtEHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

        If udtTranDetailList.Count > 0 Then
            Return True
        End If

        Return False

    End Function
    ' CRE20-0XX (Immu record)  [End][Raiman]
    ' -----------------------------------------------------------------------------------------

End Class