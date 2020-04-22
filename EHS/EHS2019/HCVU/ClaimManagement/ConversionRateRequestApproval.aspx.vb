'CRE13-019-02 Extend HCVS to China [Chris YIM]
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.ExchangeRate
Imports Common.Component.HCVUUser
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation

Partial Public Class ConversionRateRequestApproval
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    Inherits BasePageWithControl
    'Inherits BasePageWithGridView
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

    Dim udtConversionRateBLL As ExchangeRateBLL
    Dim udtConversionRateModel As ExchangeRateModel

    Dim udtFormatter As Formatter

#Region "Private Class"

    Private Class ViewIndexConversionRateRequestApproval
        Public Const HiddenNotice As Integer = -1
        Public Const DisplayNotice As Integer = 0
    End Class

    Private Class ViewIndexPendingConversionRateRequest
        Public Const HiddenRecord As Integer = -1
        Public Const NoRecord As Integer = 0
        Public Const PendingAprrovalRecord As Integer = 1
        Public Const PendingDeleteApprovedRecord As Integer = 2

    End Class

    Private Class PopupButtonIndex
        Public Const ConfirmApprovePendingRequest As Integer = 1
        Public Const ConfirmRejectPendingRequest As Integer = 2
        Public Const ConfirmApproveToDeleteAprrovedRecord As Integer = 3
        Public Const ConfirmRejectToDeleteAprrovedRecord As Integer = 4

    End Class

    Private Class SESS
        Public Const SelectedPopupButtonIndex As String = "010411_SelectedPopupButtonIndex"

        Public Const PendingApprovalRequest As String = "010411_PendingApprovalRequest"
    End Class

    Private Class MessageBoxHeaderKey
        Public Const ValidationFail As String = "ValidationFail"
    End Class

    Private Class AuditLogDescription
        Public Const LOG00000 As String = "Conversion Rate Request Approval Page Load"

        Public Const LOG00001 As String = "Return click"

        Public Const LOG00002 As String = "Pending Approval Conversion Rate Request - Approve click"
        Public Const LOG00003 As String = "Pending Approval Conversion Rate Request - Reject click"
        Public Const LOG00004 As String = "Pending Approval Conversion Rate Request - Popup - Confirm click"
        Public Const LOG00005 As String = "Pending Approval Conversion Rate Request - Popup - Cancel click"
        Public Const LOG00006 As String = "Pending Approval Conversion Rate Request - Approve success"
        Public Const LOG00007 As String = "Pending Approval Conversion Rate Request - Reject success"

        Public Const LOG00008 As String = "Delete Approved Conversion Rate Request - Approve click"
        Public Const LOG00009 As String = "Delete Approved Conversion Rate Request - Reject click"
        Public Const LOG00010 As String = "Delete Approved Conversion Rate Request - Popup - Confirm click"
        Public Const LOG00011 As String = "Delete Approved Conversion Rate Request - Popup - Cancel click"
        Public Const LOG00012 As String = "Delete Approved Conversion Rate Request - Approve success"
        Public Const LOG00013 As String = "Delete Approved Conversion Rate Request - Reject success"

        Public Const LOG00014 As String = "Pending Approval Conversion Rate Request - Validation fail"
        Public Const LOG00015 As String = "Delete Approved Conversion Rate Request - Validation fail"

        Public Const LOG00016 As String = "Pending Approval Conversion Rate Request - Two-Tier Control block"
        Public Const LOG00017 As String = "Delete Approved Conversion Rate Request - Two-Tier Control block"
    End Class

#End Region


    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    ' -------------------------------------------------------------------------
#Region "SF Search"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean

    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult
        Return Nothing
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As Common.Component.BaseBLL.BLLSearchResult) As Integer

    End Function

    Protected Overrides Sub SF_ConfirmSearch_Click()

    End Sub

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()

    End Sub

#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '' Get HCVU User to check session expire
        GetCurrentUser()

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT010411

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

            mvConversionRateRequestApproval.ActiveViewIndex = ViewIndexConversionRateRequestApproval.DisplayNotice
            mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.NoRecord

            ResetLayout()
            GetPendingApprovalConversionRateRequest(udtAuditLog)
            udtAuditLog.WriteLog(LogID.LOG00000, AuditLogDescription.LOG00000)

            RenderingLayout()

        End If


    End Sub

    Private Function GetCurrentUser() As String
        Return (New HCVUUserBLL).GetHCVUUser.UserID
    End Function

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

#End Region

    Private Sub ClearSession()
        Session(SESS.SelectedPopupButtonIndex) = Nothing
        Session(SESS.PendingApprovalRequest) = Nothing
    End Sub

    Private Sub GetPendingApprovalConversionRateRequest(ByRef udtAuditLog As AuditLogEntry)
        Dim udtConversionRateModel As ExchangeRateModel

        udtConversionRateBLL = New ExchangeRateBLL
        udtFormatter = New Formatter

        If Session(SESS.PendingApprovalRequest) Is Nothing Then
            udtConversionRateModel = udtConversionRateBLL.CreateExchangeRateRequestModel(udtConversionRateBLL.GetPendingApprovalExchangeRateRequest())
            Session(SESS.PendingApprovalRequest) = udtConversionRateModel
        Else
            udtConversionRateModel = Session(SESS.PendingApprovalRequest)
        End If

        If Not udtConversionRateModel Is Nothing Then
            Select Case udtConversionRateModel.RecordType
                Case ExchangeRateModel.ERS_ACTION_I
                    mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.PendingAprrovalRecord

                    lblPendingApprovalEffectiveDate.Text = udtFormatter.formatDisplayDate(udtConversionRateModel.EffectiveDate)
                    lblPendingApprovalConversionRate.Text = HttpContext.GetGlobalResourceObject("Text", "ConversionRateFormula") & " " & udtConversionRateModel.ExchangeRate
                    lblPendingApprovalRequestType.Text = udtConversionRateModel.GetExchangeRateActionDisplayText()
                    lblPendingApprovalCreatedBy.Text = udtConversionRateModel.CreateBy
                    lblPendingApprovalCreatedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.CreateDtm, "") & ")"

                    udtAuditLog.AddDescripton("Conversion Rate Staging ID", udtConversionRateModel.ExchangeRateStagingID.ToString)
                Case ExchangeRateModel.ERS_ACTION_D
                    mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.PendingDeleteApprovedRecord

                    lblPendingDeleteApprovedConversionRateID.Text = udtConversionRateModel.ExchangeRateIDRefByStaging
                    lblPendingDeleteApprovedEffectiveDate.Text = udtFormatter.formatDisplayDate(udtConversionRateModel.EffectiveDate)
                    lblPendingDeleteApprovedConversionRate.Text = HttpContext.GetGlobalResourceObject("Text", "ConversionRateFormula") & " " & udtConversionRateModel.ExchangeRate
                    lblPendingDeleteApprovedRequestType.Text = udtConversionRateModel.GetExchangeRateActionDisplayText()
                    lblPendingDeleteApprovedCreatedBy.Text = udtConversionRateModel.CreateBy
                    lblPendingDeleteApprovedCreatedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.CreateDtm, "") & ")"
                    lblPendingDeleteApprovedBy.Text = udtConversionRateModel.ApproveBy
                    lblPendingDeleteApprovedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.ApproveDtm, "") & ")"
                    lblPendingDeleteApprovedRequestedBy.Text = udtConversionRateModel.DeleteBy
                    lblPendingDeleteApprovedRequestedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.DeleteDtm, "") & ")"

                    udtAuditLog.AddDescripton("Conversion Rate ID", udtConversionRateModel.ExchangeRateIDRefByStaging.ToString)
            End Select

        Else
            udtAuditLog.AddDescripton("No record found", String.Empty)
        End If

    End Sub

    Private Sub ibtnReturn_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnReturn.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)
        udtAuditLog.WriteLog(LogID.LOG00001, AuditLogDescription.LOG00001)

        udtAuditLog = Nothing
        udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)

        ResetLayout()
        ClearSession()
        GetPendingApprovalConversionRateRequest(udtAuditLog)
        RenderingLayout()

    End Sub

    Private Sub ResetLayout()
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        mvConversionRateRequestApproval.ActiveViewIndex = ViewIndexConversionRateRequestApproval.DisplayNotice
        mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.NoRecord
    End Sub

    Private Sub HideLayout()
        mvConversionRateRequestApproval.ActiveViewIndex = ViewIndexConversionRateRequestApproval.HiddenNotice
        mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.HiddenRecord
    End Sub

    Private Sub ResetAlert()
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False
    End Sub

    Private Sub RenderingLayout()
        Select Case mvConversionRateRequestApproval.ActiveViewIndex
            Case ViewIndexConversionRateRequestApproval.HiddenNotice
                ResetAlert()
                ibtnReturn.Enabled = True
                ibtnReturn.Visible = True
            Case ViewIndexConversionRateRequestApproval.DisplayNotice
                ibtnReturn.Enabled = False
                ibtnReturn.Visible = False
            Case Else

        End Select

        Select Case mvPendingConversionRateRequest.ActiveViewIndex
            Case ViewIndexPendingConversionRateRequest.NoRecord
                udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010411, SeverityCode.SEVI, LogID.LOG00004))
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

            Case ViewIndexPendingConversionRateRequest.PendingAprrovalRecord
                Dim blnResult As Boolean = True
                Dim udtAuditLog As AuditLogEntry

                'Handle Two Tier Control
                If Not IsValidUser() Then
                    ibtnApprovePendingRequest.Enabled = False
                    ibtnApprovePendingRequest.ImageUrl = HttpContext.GetGlobalResourceObject("ImageURL", "ApproveDisableBtn")
                    ibtnRejectPendingRequest.Enabled = False
                    ibtnRejectPendingRequest.ImageUrl = HttpContext.GetGlobalResourceObject("ImageURL", "RejectDisableBtn")

                    udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010411, SeverityCode.SEVI, LogID.LOG00005))
                    udcInfoMessageBox.BuildMessageBox()
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                    udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)
                    udtAuditLog.AddDescripton("message text", New SystemMessage(FunctCode.FUNT010411, SeverityCode.SEVI, LogID.LOG00005).GetMessage())
                    'udtAuditLog.AddDescripton("**********", New SystemMessage(FunctCode.FUNT010411, SeverityCode.SEVI, LogID.LOG00005).ConvertToResourceCode() & ";")
                    udtAuditLog.WriteLog(LogID.LOG00016, AuditLogDescription.LOG00016)

                    blnResult = False
                End If

                'Handle Effective Date
                If blnResult Then
                    If Not IsValidEffectiveDate() Then
                        ibtnApprovePendingRequest.Enabled = False
                        ibtnApprovePendingRequest.ImageUrl = HttpContext.GetGlobalResourceObject("ImageURL", "ApproveDisableBtn")

                        udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)

                        udcMessageBox.AddMessage(FunctCode.FUNT010411, SeverityCode.SEVE, LogID.LOG00001)
                        udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00014, AuditLogDescription.LOG00014)

                        blnResult = False
                    End If
                End If

            Case ViewIndexPendingConversionRateRequest.PendingDeleteApprovedRecord
                Dim blnResult As Boolean = True
                Dim udtAuditLog As AuditLogEntry

                'Handle Two Tier Control
                If Not IsValidUser() Then
                    ibtnApproveToDeleteApprovedRecord.Enabled = False
                    ibtnApproveToDeleteApprovedRecord.ImageUrl = HttpContext.GetGlobalResourceObject("ImageURL", "ApproveDisableBtn")
                    ibtnRejectToDeleteApprovedRecord.Enabled = False
                    ibtnRejectToDeleteApprovedRecord.ImageUrl = HttpContext.GetGlobalResourceObject("ImageURL", "RejectDisableBtn")

                    udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010411, SeverityCode.SEVI, LogID.LOG00005))
                    udcInfoMessageBox.BuildMessageBox()
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                    udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)
                    udtAuditLog.AddDescripton("message text", New SystemMessage(FunctCode.FUNT010411, SeverityCode.SEVI, LogID.LOG00005).GetMessage())
                    'udtAuditLog.AddDescripton("**********", New SystemMessage(FunctCode.FUNT010411, SeverityCode.SEVI, LogID.LOG00005).ConvertToResourceCode() & ";")
                    udtAuditLog.WriteLog(LogID.LOG00017, AuditLogDescription.LOG00017)

                    blnResult = False
                End If

                'Handle Effective Date
                If blnResult Then
                    If Not IsValidEffectiveDate() Then
                        ibtnApproveToDeleteApprovedRecord.Enabled = False
                        ibtnApproveToDeleteApprovedRecord.ImageUrl = HttpContext.GetGlobalResourceObject("ImageURL", "ApproveDisableBtn")

                        udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)

                        udcMessageBox.AddMessage(FunctCode.FUNT010411, SeverityCode.SEVE, LogID.LOG00002)
                        udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, AuditLogDescription.LOG00015)

                        blnResult = False
                    End If
                End If

            Case Else

        End Select
    End Sub


    Private Sub ApprovePendingRequest()
        Dim udtConversionRateModelPermanent As ExchangeRateModel
        Dim udtConversionRateModelStaging As ExchangeRateModel
        Dim udtConversionRateBLL As New ExchangeRateBLL
        Dim udtGeneralFunction As New GeneralFunction
        Dim strConversionRateID As String
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)
        Dim eSQL As String = Nothing

        udtConversionRateModel = Session(SESS.PendingApprovalRequest)

        'Generate ER_ID in Exchange Rate Model
        strConversionRateID = udtGeneralFunction.GenerateExchangeRateID()
        udtConversionRateModelPermanent = udtConversionRateBLL.CreateExchangeRateRecordModel(strConversionRateID, _
                                                                                          udtConversionRateModel.EffectiveDate, _
                                                                                          udtConversionRateModel.ExchangeRate, _
                                                                                          udtConversionRateModel.CreateBy, _
                                                                                          udtConversionRateModel.CreateDtm, _
                                                                                          GetCurrentUser(), _
                                                                                          Nothing)

        'Update Record Status, ER_ID and Approver in Exchange Rate Staging Model
        udtConversionRateModelStaging = udtConversionRateModel

        udtConversionRateModelStaging.ExchangeRateIDRefByStaging = strConversionRateID
        udtConversionRateModelStaging.ApproveBy = GetCurrentUser()
        udtConversionRateModelStaging.RecordStatus = ExchangeRateModel.ERS_RECORD_STATUS_A

        'Save the value of Exchange Rate Staging Model to DB Table [ConversionRateStaging]
        Try
            eSQL = udtConversionRateBLL.UpdateExchangeRateRecordStatusInStaging(udtConversionRateModelStaging)

            If eSQL Is Nothing Then
                'Get Approval DateTime from staging
                udtConversionRateModel = Nothing
                udtConversionRateModel = udtConversionRateBLL.CreateExchangeRateRequestModel(udtConversionRateBLL.GetApprovedExchangeRateRequest(strConversionRateID))

                'Copy Approval DateTime to Permanent
                udtConversionRateModelPermanent.ApproveDtm = udtConversionRateModel.ApproveDtm

                'Save the value of Exchange Rate Model to DB Table [ConversionRate]
                If Not udtConversionRateModelPermanent Is Nothing Then
                    udtConversionRateBLL.WriteExchangeRateRequestInPermanent(udtConversionRateModelPermanent)
                End If

                'Hide the view
                HideLayout()
                RenderingLayout()

                udtAuditLog.AddDescripton("Conversion Rate Staging ID", udtConversionRateModelStaging.ExchangeRateStagingID)
                udtAuditLog.AddDescripton("Conversion Rate ID", strConversionRateID)
                udtAuditLog.WriteLog(LogID.LOG00006, AuditLogDescription.LOG00006)

                'Message Box - Success
                udcInfoMessageBox.AddMessage(FunctCode.FUNT010411, SeverityCode.SEVI, LogID.LOG00002, "%s", strConversionRateID)
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            Else
                'Message Box - Fail
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL))
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00018, "Approve conversion rate request fail")
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub RejectPendingRequest()
        Dim udtConversionRateBLL As New ExchangeRateBLL
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)
        Dim eSQL As String = Nothing

        udtConversionRateModel = Session(SESS.PendingApprovalRequest)

        'Update Record Status in Exchange Rate Staging Model
        udtConversionRateModel.RejectBy = GetCurrentUser()
        udtConversionRateModel.RecordStatus = ExchangeRateModel.ERS_RECORD_STATUS_R

        Try
            'Save the value of Exchange Rate Staging Model to DB Table [ConversionRateStaging]
            eSQL = udtConversionRateBLL.UpdateExchangeRateRecordStatusInStaging(udtConversionRateModel)

            If eSQL Is Nothing Then
                'Hide the view
                HideLayout()
                RenderingLayout()

                udtAuditLog.AddDescripton("Conversion Rate Staging ID", udtConversionRateModel.ExchangeRateStagingID)
                udtAuditLog.WriteLog(LogID.LOG00007, AuditLogDescription.LOG00007)

                'Message Box - Success
                udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010411, SeverityCode.SEVI, LogID.LOG00001))
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            Else
                'Message Box - Fail
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL))
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "Reject conversion rate request fail")
            End If
        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ApproveToDeleteApprovedRecord()
        Dim udtConversionRateModelPermanent As ExchangeRateModel
        Dim udtConversionRateModelStaging As ExchangeRateModel
        Dim udtConversionRateBLL As New ExchangeRateBLL
        Dim udtGeneralFunction As New GeneralFunction
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)
        Dim dtActiveER As DataTable = Nothing
        Dim dtActiveERS As DataTable = Nothing
        Dim eSQL As String = Nothing

        udtConversionRateModel = Session(SESS.PendingApprovalRequest)

        'Get approved record
        dtActiveER = udtConversionRateBLL.GetApprovedExchangeRateRecord(ExchangeRateModel.ER_INFO_TYPE_N, udtConversionRateModel.ExchangeRateIDRefByStaging)

        'Get request of pending to delete approved record
        dtActiveERS = udtConversionRateBLL.GetPendingApprovalExchangeRateRequest(udtConversionRateModel.ExchangeRateIDRefByStaging)

        Try
            'Validation of concurrent update
            If dtActiveER.Rows.Count > 0 And dtActiveERS.Rows.Count > 0 Then

                'Create Exchange Rate Model
                udtConversionRateModelPermanent = udtConversionRateBLL.CreateExchangeRateRecordModel(dtActiveER)

                'Update Record Status and Approver in Exchange Rate Model
                udtConversionRateModelPermanent.UpdateBy = GetCurrentUser()
                udtConversionRateModelPermanent.RecordStatus = ExchangeRateModel.ER_RECORD_STATUS_D

                'Update Record Status and Approver in Exchange Rate Staging Model
                udtConversionRateModelStaging = udtConversionRateModel

                udtConversionRateModelStaging.ApproveBy = GetCurrentUser()
                udtConversionRateModelStaging.RecordStatus = ExchangeRateModel.ERS_RECORD_STATUS_A

                'Save the value of Exchange Rate Staging Model to DB Table [ConversionRateStaging]
                If Not udtConversionRateModelStaging Is Nothing Then
                    eSQL = udtConversionRateBLL.UpdateExchangeRateRecordStatusInStaging(udtConversionRateModelStaging)
                End If

                If eSQL Is Nothing Then
                    'Save the value of Exchange Rate Model to DB Table [ConversionRate]
                    If Not udtConversionRateModelPermanent Is Nothing Then
                        udtConversionRateBLL.UpdateExchangeRateRecordStatusInPermanent(udtConversionRateModelPermanent)
                    End If

                    'Hide the view
                    HideLayout()
                    RenderingLayout()

                    udtAuditLog.AddDescripton("Conversion Rate ID", udtConversionRateModel.ExchangeRateIDRefByStaging.ToString)
                    udtAuditLog.WriteLog(LogID.LOG00012, AuditLogDescription.LOG00012)

                    'Message Box - Success
                    udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010411, SeverityCode.SEVI, LogID.LOG00003))
                    udcInfoMessageBox.BuildMessageBox()
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                Else
                    'Message Box - Fail
                    udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL))
                    udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00020, "Approve to detele approved conversion rate record fail")
                End If
            Else
                'Message Box - Fail
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011))
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00020, "Approve to detele approved conversion rate record fail")
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub RejectToDeleteApprovedRecord()
        Dim udtConversionRateBLL As New ExchangeRateBLL
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)
        Dim eSQL As String = Nothing

        udtConversionRateModel = Session(SESS.PendingApprovalRequest)

        'Update Record Status in Exchange Rate Staging Model
        udtConversionRateModel.RejectBy = GetCurrentUser()
        udtConversionRateModel.RecordStatus = ExchangeRateModel.ERS_RECORD_STATUS_R

        Try
            'Save the value of Exchange Rate Staging Model to DB Table [ConversionRateStaging]
            eSQL = udtConversionRateBLL.UpdateExchangeRateRecordStatusInStaging(udtConversionRateModel)

            If eSQL Is Nothing Then
                'Hide the view
                HideLayout()
                RenderingLayout()

                udtAuditLog.AddDescripton("Conversion Rate ID", udtConversionRateModel.ExchangeRateIDRefByStaging.ToString)
                udtAuditLog.WriteLog(LogID.LOG00013, AuditLogDescription.LOG00013)

                'Message Box - Success
                udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010411, SeverityCode.SEVI, LogID.LOG00001))
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            Else
                'Message Box - Fail
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL))
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00021, "Reject to delete approved conversion rate record fail")
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub ibtnDialogConfirm_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnDialogConfirm.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)

        lblMsg.Text = sender.id.ToString()

        Select Case Session(SESS.SelectedPopupButtonIndex)
            Case PopupButtonIndex.ConfirmApprovePendingRequest
                udtAuditLog.WriteLog(LogID.LOG00004, AuditLogDescription.LOG00004)
                ApprovePendingRequest()
            Case PopupButtonIndex.ConfirmRejectPendingRequest
                udtAuditLog.WriteLog(LogID.LOG00004, AuditLogDescription.LOG00004)
                RejectPendingRequest()
            Case PopupButtonIndex.ConfirmApproveToDeleteAprrovedRecord
                udtAuditLog.WriteLog(LogID.LOG00010, AuditLogDescription.LOG00010)
                ApproveToDeleteApprovedRecord()
            Case PopupButtonIndex.ConfirmRejectToDeleteAprrovedRecord
                udtAuditLog.WriteLog(LogID.LOG00010, AuditLogDescription.LOG00010)
                RejectToDeleteApprovedRecord()
        End Select

    End Sub

    Private Sub ibtnDialogCancel_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnDialogCancel.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)

        Select Case Session(SESS.SelectedPopupButtonIndex)
            Case PopupButtonIndex.ConfirmApprovePendingRequest
                udtAuditLog.WriteLog(LogID.LOG00005, AuditLogDescription.LOG00005)
            Case PopupButtonIndex.ConfirmRejectPendingRequest
                udtAuditLog.WriteLog(LogID.LOG00005, AuditLogDescription.LOG00005)
            Case PopupButtonIndex.ConfirmApproveToDeleteAprrovedRecord
                udtAuditLog.WriteLog(LogID.LOG00011, AuditLogDescription.LOG00011)
            Case PopupButtonIndex.ConfirmRejectToDeleteAprrovedRecord
                udtAuditLog.WriteLog(LogID.LOG00011, AuditLogDescription.LOG00011)
        End Select

        RenderingLayout()

    End Sub

    Private Sub ibtnApprovePendingRequest_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnApprovePendingRequest.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)
        udtAuditLog.WriteLog(LogID.LOG00002, AuditLogDescription.LOG00002)

        lblMsg.Text = HttpContext.GetGlobalResourceObject("Text", "ConfirmApproveConversionRateRequest")
        Session(SESS.SelectedPopupButtonIndex) = PopupButtonIndex.ConfirmApprovePendingRequest
        ModalPopupExtenderConfirmApprovePendingRequest.Show()
    End Sub

    Private Sub ibtnRejectPendingRequest_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnRejectPendingRequest.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)
        udtAuditLog.WriteLog(LogID.LOG00003, AuditLogDescription.LOG00003)

        lblMsg.Text = HttpContext.GetGlobalResourceObject("Text", "ConfirmRejectConversionRateRequest")
        Session(SESS.SelectedPopupButtonIndex) = PopupButtonIndex.ConfirmRejectPendingRequest
        ModalPopupExtenderConfirmRejectPendingRequest.Show()
    End Sub

    Private Sub ibtnApproveToDeleteApprovedRecord_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnApproveToDeleteApprovedRecord.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)
        udtAuditLog.WriteLog(LogID.LOG00008, AuditLogDescription.LOG00008)

        lblMsg.Text = HttpContext.GetGlobalResourceObject("Text", "ConfirmApproveConversionRateRequest")
        Session(SESS.SelectedPopupButtonIndex) = PopupButtonIndex.ConfirmApproveToDeleteAprrovedRecord
        ModalPopupExtenderConfirmApproveToDeleteApprovedRecord.Show()
    End Sub

    Private Sub ibtnRejectToDeleteApprovedRecord_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnRejectToDeleteApprovedRecord.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010411, Me)
        udtAuditLog.WriteLog(LogID.LOG00009, AuditLogDescription.LOG00009)

        lblMsg.Text = HttpContext.GetGlobalResourceObject("Text", "ConfirmRejectConversionRateRequest")
        Session(SESS.SelectedPopupButtonIndex) = PopupButtonIndex.ConfirmRejectToDeleteAprrovedRecord
        ModalPopupExtenderConfirmRejectToDeleteApprovedRecord.Show()
    End Sub

    Private Function IsValidUser() As Boolean
        Dim udtConversionRateModel As ExchangeRateModel
        Dim blnResult As Boolean = True

        udtConversionRateModel = Session(SESS.PendingApprovalRequest)

        'Validation on Two Tier Control
        Select Case mvPendingConversionRateRequest.ActiveViewIndex
            Case ViewIndexPendingConversionRateRequest.PendingAprrovalRecord
                If udtConversionRateModel.CreateBy.Trim = GetCurrentUser().Trim Then
                    blnResult = False
                End If
            Case ViewIndexPendingConversionRateRequest.PendingDeleteApprovedRecord
                If udtConversionRateModel.DeleteBy.Trim = GetCurrentUser().Trim Then
                    blnResult = False
                End If
        End Select

        Return blnResult
    End Function

    Private Function IsValidEffectiveDate() As Boolean
        Dim udtConversionRateModel As ExchangeRateModel
        Dim blnResult As Boolean = True

        udtConversionRateModel = Session(SESS.PendingApprovalRequest)

        'Validation on Effective Date
        If udtConversionRateModel.EffectiveDate <= Today() Then
            blnResult = False
        End If

        Return blnResult
    End Function
End Class
