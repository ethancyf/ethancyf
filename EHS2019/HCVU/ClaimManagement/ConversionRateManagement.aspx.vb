'CRE13-019-02 Extend HCVS to China [Chris YIM]
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.HCVUUser
Imports Common.Component.ExchangeRate
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation

Partial Public Class ConversionRateManagement
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    Inherits BasePageWithControl
    'Inherits BasePageWithGridView
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]
    ' FunctionCode = FunctCode.FUNT010304

    Dim udtConversionRateBLL As ExchangeRateBLL

    Dim udtFormatter As Formatter
    Dim udtGeneralFunction As GeneralFunction

#Region "Private Class"

    Private Class ViewIndexConversionRateManagement
        Public Const HiddenNotice As Integer = -1
        Public Const DisplayNotice As Integer = 0
    End Class

    Private Class ViewIndexCurrentConversionRateInfo
        Public Const HiddenRecord As Integer = -1
        Public Const NoRecord As Integer = 0
        Public Const CurrentRecord As Integer = 1
    End Class

    Private Class ViewIndexNextConversionRateInfo
        Public Const HiddenRecord As Integer = -1
        Public Const NoRecord As Integer = 0
        Public Const NextRecord As Integer = 1
    End Class

    Private Class ViewIndexPendingConversionRateRequest
        Public Const HiddenRecord As Integer = -1
        Public Const NoRecord As Integer = 0
        Public Const CreateRecord As Integer = 1
        Public Const ConfirmRecord As Integer = 2
        Public Const PendingAprrovalRecord As Integer = 3
        Public Const PendingDeleteApprovedRecord As Integer = 4

    End Class

    Private Class SESS
        Public Const InputedConversionRate As String = "010410_InputedConversionRate"
        Public Const InputedEffectiveDate As String = "010410_InputedEffectiveDate"

        Public Const CurrentConversionRate As String = "010410_CurrentConversionRate"
        Public Const NextConversionRate As String = "010410_NextConversionRate"
        Public Const PendingApprovalRequest As String = "010410_PendingApprovalRequest"

        Public Const MessageBoxForValidationOfConcurrentUpdate As String = "010410_MessageBoxForValidationOfConcurrentUpdate"

    End Class

    Private Class MessageBoxHeaderKey
        Public Const ValidationFail As String = "ValidationFail"
    End Class

    Private Class AuditLogDescription
        Public Const LOG00000 As String = "Conversion Rate Management Page Load"

        Public Const LOG00001 As String = "Create Conversion Rate click"
        Public Const LOG00002 As String = "Create Conversion Rate - Save fail"
        Public Const LOG00003 As String = "Create Conversion Rate - Save click"
        Public Const LOG00004 As String = "Create Conversion Rate - Cancel click"
        Public Const LOG00005 As String = "Create Conversion Rate - Confirm click"
        Public Const LOG00006 As String = "Create Conversion Rate - Back click"
        Public Const LOG00007 As String = "Create Conversion Rate - Creation success"
        Public Const LOG00008 As String = "Return click"

        Public Const LOG00009 As String = "Pending Approval Conversion Rate Request - Delete click"
        Public Const LOG00010 As String = "Pending Approval Conversion Rate Request - Popup - Confirm click"
        Public Const LOG00011 As String = "Pending Approval Conversion Rate Request - Popup - Cancel click"
        Public Const LOG00012 As String = "Pending Approval Conversion Rate Request - Delete success"

        Public Const LOG00013 As String = "Delete Approved Conversion Rate Request - Delete click"
        Public Const LOG00014 As String = "Delete Approved Conversion Rate Request - Popup - Confirm click"
        Public Const LOG00015 As String = "Delete Approved Conversion Rate Request - Popup - Cancel click"
        Public Const LOG00016 As String = "Delete Approved Conversion Rate Request - Submission success"

        Public Const LOG00017 As String = "Cancel click and back to Conversion Rate Management main page"
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

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Me.udcInfoMessageBox.Visible = False
        Me.udcMessageBox.Visible = False

        '' Get HCVU User to check session expire
        GetCurrentUser()

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT010410
            '    Me.Session(SESS.DeathRecordResultOverridded) = False
            '    Me.Session(SESS.DeathRecordResultCnt) = 0

            Dim udtAuditLog = New AuditLogEntry(FunctionCode, Me)

            ResetLayout()
            ClearSession()
            GetCurrentConversionRate(udtAuditLog)
            GetNextConversionRate(udtAuditLog)
            GetPendingApprovalConversionRateRequest(udtAuditLog)
            RenderingLayout()

            udtAuditLog.WriteLog(LogID.LOG00000, AuditLogDescription.LOG00000)

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
        Session(SESS.InputedConversionRate) = Nothing
        Session(SESS.InputedEffectiveDate) = Nothing
        Session(SESS.CurrentConversionRate) = Nothing
        Session(SESS.NextConversionRate) = Nothing
        Session(SESS.PendingApprovalRequest) = Nothing
    End Sub

    Private Sub ibtnCreateConversionRate_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnCreateConversionRate.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)
        udtAuditLog.WriteLog(LogID.LOG00001, AuditLogDescription.LOG00001)

        mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.CreateRecord

        RenderingLayout()

    End Sub

    Private Sub ibtnCancel_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnCancel.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)
        udtAuditLog.WriteLog(LogID.LOG00004, AuditLogDescription.LOG00004)

        mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.NoRecord

        udtAuditLog = Nothing
        udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)

        ClearSession()
        GetCurrentConversionRate(udtAuditLog)
        GetNextConversionRate(udtAuditLog)
        GetPendingApprovalConversionRateRequest(udtAuditLog)
        RenderingLayout()

    End Sub

    Private Sub ibtnSave_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSave.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)

        If IsValidInput() Then
            Dim udtFormattor As New Formatter

            udtAuditLog.AddDescripton(GetGlobalResourceObject("text", "EffectiveDate"), txtEffectiveDate.Text)
            udtAuditLog.AddDescripton(GetGlobalResourceObject("text", "ConversionRate"), txtConversionRate.Text)
            udtAuditLog.WriteLog(LogID.LOG00003, AuditLogDescription.LOG00003)

            mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.ConfirmRecord

            RenderingLayout()
        Else
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00002, AuditLogDescription.LOG00002)
        End If

    End Sub

    Private Sub ibtnBack_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnBack.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)
        udtAuditLog.WriteLog(LogID.LOG00006, AuditLogDescription.LOG00006)

        mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.CreateRecord

        RenderingLayout()

    End Sub

    Private Sub ibtnConfirm_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnConfirm.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)
        udtAuditLog.AddDescripton(GetGlobalResourceObject("text", "EffectiveDate"), lblConfirmEffectiveDate.Text)
        udtAuditLog.AddDescripton(GetGlobalResourceObject("text", "ConversionRate"), formatConversionRate(txtConversionRate.Text))
        udtAuditLog.WriteLog(LogID.LOG00005, AuditLogDescription.LOG00005)

        Dim udtConversionRateBLL As New ExchangeRateBLL
        Dim udtConversionRateModel As ExchangeRateModel
        Dim eSQL As String = Nothing
        Dim dt As DataTable

        'Get pending approval request when status is 'P'
        dt = udtConversionRateBLL.GetPendingApprovalExchangeRateRequest()

        'Validation whether has creation as before to prevent concurrent update
        If dt.Rows.Count = 0 Then
            udtConversionRateModel = udtConversionRateBLL.CreateExchangeRateRequestModel(String.Empty, _
                                                                                     txtEffectiveDate.Text, _
                                                                                     formatConversionRate(txtConversionRate.Text), _
                                                                                     ExchangeRateModel.ERS_ACTION_I, _
                                                                                     GetCurrentUser())

            eSQL = udtConversionRateBLL.WriteExchangeRateRequestInStaging(udtConversionRateModel)

            If eSQL Is Nothing Then
                HideLayout()
                RenderingLayout()

                udtAuditLog.AddDescripton("Conversion Rate Staging ID", udtConversionRateModel.ExchangeRateStagingID.ToString)
                udtAuditLog.WriteLog(LogID.LOG00007, AuditLogDescription.LOG00007)

                'Message Box - Success
                udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010410, SeverityCode.SEVI, LogID.LOG00002))
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            Else
                'Message Box - Fail, with custom message description
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010410, SeverityCode.SEVE, eSQL))
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00017, "Create conversion rate fail")
                trConfirmCreation.Style.Add("display", "none")
                trCancelBackToHome.Style.Add("display", "initial")
            End If
        Else
            'Message Box - Fail, with custom message description
            udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010410, SeverityCode.SEVE, MsgCode.MSG00006))
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00017, "Create conversion rate fail")
            trConfirmCreation.Style.Add("display", "none")
            trCancelBackToHome.Style.Add("display", "initial")
        End If


    End Sub

    Private Sub ibtnReturn_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnReturn.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)
        udtAuditLog.WriteLog(LogID.LOG00008, AuditLogDescription.LOG00008)

        udtAuditLog = Nothing
        udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)

        ResetLayout()
        ClearSession()
        GetCurrentConversionRate(udtAuditLog)
        GetNextConversionRate(udtAuditLog)
        GetPendingApprovalConversionRateRequest(udtAuditLog)
        RenderingLayout()

    End Sub

    Private Sub GetCurrentConversionRate(ByRef udtAuditLog As AuditLogEntry)
        Dim udtConversionRateModel As ExchangeRateModel

        udtConversionRateBLL = New ExchangeRateBLL
        udtFormatter = New Formatter

        If Session(SESS.CurrentConversionRate) Is Nothing Then
            udtConversionRateModel = udtConversionRateBLL.CreateExchangeRateRecordModel(udtConversionRateBLL.GetApprovedExchangeRateRecord(ExchangeRateModel.ER_INFO_TYPE_T))
            Session(SESS.CurrentConversionRate) = udtConversionRateModel
        Else
            udtConversionRateModel = Session(SESS.CurrentConversionRate)
        End If

        If Not udtConversionRateModel Is Nothing Then
            mvCurrentConversionRateInfo.ActiveViewIndex = ViewIndexCurrentConversionRateInfo.CurrentRecord

            lblCurrentConversionRateID.Text = udtConversionRateModel.ExchangeRateID
            lblCurrentEffectiveDate.Text = udtFormatter.formatDisplayDate(udtConversionRateModel.EffectiveDate)
            lblCurrentConversionRate.Text = HttpContext.GetGlobalResourceObject("Text", "ConversionRateFormula") & " " & formatConversionRate(udtConversionRateModel.ExchangeRate)
            lblCurrentConversionRateCreateBy.Text = udtConversionRateModel.CreateBy
            lblCurrentConversionRateCreateDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.CreateDtm, "") & ")"
            lblCurrentConversionRateApprovedBy.Text = udtConversionRateModel.ApproveBy
            lblCurrentConversionRateApprovedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.ApproveDtm, "") & ")"

            udtAuditLog.AddDescripton("Current Conversion Rate", udtConversionRateModel.ExchangeRateID.ToString)
        Else
            udtAuditLog.AddDescripton("Current Conversion Rate", String.Empty)
        End If

    End Sub

    Private Sub GetNextConversionRate(ByRef udtAuditLog As AuditLogEntry)
        Dim udtConversionRateModel As ExchangeRateModel

        udtConversionRateBLL = New ExchangeRateBLL
        udtFormatter = New Formatter

        If Session(SESS.NextConversionRate) Is Nothing Then
            udtConversionRateModel = udtConversionRateBLL.CreateExchangeRateRecordModel(udtConversionRateBLL.GetApprovedExchangeRateRecord(ExchangeRateModel.ER_INFO_TYPE_N))
            Session(SESS.NextConversionRate) = udtConversionRateModel
        Else
            udtConversionRateModel = Session(SESS.NextConversionRate)
        End If

        If Not udtConversionRateModel Is Nothing Then
            mvNextConversionRateInfo.ActiveViewIndex = ViewIndexNextConversionRateInfo.NextRecord

            lblNextConversionRateID.Text = udtConversionRateModel.ExchangeRateID
            lblNextEffectiveDate.Text = udtFormatter.formatDisplayDate(udtConversionRateModel.EffectiveDate)
            lblNextConversionRate.Text = HttpContext.GetGlobalResourceObject("Text", "ConversionRateFormula") & " " & formatConversionRate(udtConversionRateModel.ExchangeRate)
            lblNextConversionRateCreateBy.Text = udtConversionRateModel.CreateBy
            lblNextConversionRateCreateDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.CreateDtm, "") & ")"
            lblNextConversionRateApprovedBy.Text = udtConversionRateModel.ApproveBy
            lblNextConversionRateApprovedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.ApproveDtm, "") & ")"

            udtAuditLog.AddDescripton("Next Conversion Rate", udtConversionRateModel.ExchangeRateID.ToString)
        Else
            udtAuditLog.AddDescripton("Next Conversion Rate", String.Empty)
        End If

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
                    lblPendingApprovalConversionRate.Text = HttpContext.GetGlobalResourceObject("Text", "ConversionRateFormula") & " " & formatConversionRate(udtConversionRateModel.ExchangeRate)
                    lblPendingApprovalRequestType.Text = udtConversionRateModel.GetExchangeRateActionDisplayText()
                    lblPendingApprovalCreatedBy.Text = udtConversionRateModel.CreateBy
                    lblPendingApprovalCreatedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.CreateDtm, "") & ")"

                    udtAuditLog.AddDescripton("Pending Approval Conversion Rate Request", udtConversionRateModel.ExchangeRateStagingID.ToString)
                Case ExchangeRateModel.ERS_ACTION_D
                    mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.PendingDeleteApprovedRecord

                    lblPendingDeleteApprovedConversionRateID.Text = udtConversionRateModel.ExchangeRateIDRefByStaging
                    lblPendingDeleteApprovedEffectiveDate.Text = udtFormatter.formatDisplayDate(udtConversionRateModel.EffectiveDate)
                    lblPendingDeleteApprovedConversionRate.Text = HttpContext.GetGlobalResourceObject("Text", "ConversionRateFormula") & " " & formatConversionRate(udtConversionRateModel.ExchangeRate)
                    lblPendingDeleteApprovedRequestType.Text = udtConversionRateModel.GetExchangeRateActionDisplayText()
                    lblPendingDeleteApprovedCreatedBy.Text = udtConversionRateModel.CreateBy
                    lblPendingDeleteApprovedCreatedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.CreateDtm, "") & ")"
                    lblPendingDeleteApprovedBy.Text = udtConversionRateModel.ApproveBy
                    lblPendingDeleteApprovedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.ApproveDtm, "") & ")"
                    lblPendingDeleteApprovedRequestedBy.Text = udtConversionRateModel.DeleteBy
                    lblPendingDeleteApprovedRequestedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.DeleteDtm, "") & ")"

                    udtAuditLog.AddDescripton("Pending Approval Conversion Rate Request", udtConversionRateModel.ExchangeRateIDRefByStaging.ToString)
            End Select

        Else
            udtAuditLog.AddDescripton("Pending Approval Conversion Rate Request", String.Empty)
        End If

    End Sub

    Private Sub ResetLayout()
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        mvConversionRateManagement.ActiveViewIndex = ViewIndexConversionRateManagement.DisplayNotice
        mvCurrentConversionRateInfo.ActiveViewIndex = ViewIndexCurrentConversionRateInfo.NoRecord
        mvNextConversionRateInfo.ActiveViewIndex = ViewIndexNextConversionRateInfo.NoRecord
        mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.NoRecord
    End Sub

    Private Sub HideLayout()
        mvConversionRateManagement.ActiveViewIndex = ViewIndexConversionRateManagement.HiddenNotice
        mvCurrentConversionRateInfo.ActiveViewIndex = ViewIndexCurrentConversionRateInfo.HiddenRecord
        mvNextConversionRateInfo.ActiveViewIndex = ViewIndexNextConversionRateInfo.HiddenRecord
        mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.HiddenRecord
    End Sub

    Private Sub ResetAlert()
        imgEffectiveDateAlert.Visible = False
        imgConversionRateAlert.Visible = False
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False
    End Sub

    Private Sub RenderingLayout()

        Select Case mvConversionRateManagement.ActiveViewIndex
            Case ViewIndexConversionRateManagement.HiddenNotice
                ibtnReturn.Enabled = True
                ibtnReturn.Visible = True
            Case ViewIndexConversionRateManagement.DisplayNotice
                ibtnReturn.Enabled = False
                ibtnReturn.Visible = False
            Case Else

        End Select

        Select Case mvCurrentConversionRateInfo.ActiveViewIndex
            Case ViewIndexCurrentConversionRateInfo.NoRecord

            Case ViewIndexCurrentConversionRateInfo.CurrentRecord

            Case Else

        End Select

        Select Case mvNextConversionRateInfo.ActiveViewIndex
            Case ViewIndexNextConversionRateInfo.NoRecord

            Case ViewIndexNextConversionRateInfo.NextRecord
                If mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.PendingDeleteApprovedRecord Then
                    trDeleteApprovedRecord.Visible = False
                    ibtnDeleteApprovedRecord.Enabled = False
                    ibtnDeleteApprovedRecord.Visible = False

                    Dim udtNextConversionRateModel As ExchangeRateModel
                    Dim udtPendingApprovalConversionRateModel As ExchangeRateModel
                    udtNextConversionRateModel = Session(SESS.NextConversionRate)
                    udtPendingApprovalConversionRateModel = Session(SESS.PendingApprovalRequest)

                    If udtNextConversionRateModel.ExchangeRateID = udtPendingApprovalConversionRateModel.ExchangeRateIDRefByStaging Then
                        trNextConversionRateRemark.Visible = True
                        lblDeleteApprovedConversionRateAsterisk.Visible = True
                    End If
                Else
                    trDeleteApprovedRecord.Visible = True
                    ibtnDeleteApprovedRecord.Enabled = True
                    ibtnDeleteApprovedRecord.Visible = True

                    Session(SESS.MessageBoxForValidationOfConcurrentUpdate) = Nothing
                End If

            Case Else

        End Select

        Select Case mvPendingConversionRateRequest.ActiveViewIndex
            Case ViewIndexPendingConversionRateRequest.NoRecord
                If mvNextConversionRateInfo.ActiveViewIndex = ViewIndexNextConversionRateInfo.NextRecord Then
                    ibtnCreateConversionRate.Enabled = False
                    ibtnCreateConversionRate.Visible = False
                Else
                    ibtnCreateConversionRate.Enabled = True
                    ibtnCreateConversionRate.Visible = True
                End If

            Case ViewIndexPendingConversionRateRequest.CreateRecord
                ResetAlert()
                txtConversionRate.Text = String.Empty
                txtEffectiveDate.Text = String.Empty

                Status.GetDescriptionFromDBCode(ExchangeRateModel.STATUS_DATA_CLASS_ERS_ACTION, ExchangeRateModel.ERS_ACTION_I, lblCreateRequestType.Text, String.Empty)
                calEffectiveDate.StartDate = DateAdd(DateInterval.Day, 1, Today)

                If Not Session(SESS.InputedEffectiveDate) Is Nothing Then
                    txtEffectiveDate.Text = Session(SESS.InputedEffectiveDate)
                End If

                If Not Session(SESS.InputedConversionRate) Is Nothing Then
                    txtConversionRate.Text = Left(Session(SESS.InputedConversionRate), 6)
                End If

            Case ViewIndexPendingConversionRateRequest.ConfirmRecord
                Dim udtFormattor As New Formatter

                udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVI, LogID.LOG00021))
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                Session(SESS.InputedEffectiveDate) = txtEffectiveDate.Text
                lblConfirmEffectiveDate.Text = udtFormattor.convertDate(udtFormattor.formatInputDate(txtEffectiveDate.Text), "")
                Session(SESS.InputedConversionRate) = formatConversionRate(txtConversionRate.Text)
                lblConfirmConversionRate.Text = HttpContext.GetGlobalResourceObject("Text", "ConversionRateFormula") & " " & formatConversionRate(txtConversionRate.Text)

                Status.GetDescriptionFromDBCode(ExchangeRateModel.STATUS_DATA_CLASS_ERS_ACTION, ExchangeRateModel.ERS_ACTION_I, lblConfirmRequestType.Text, String.Empty)

                trConfirmCreation.Style.Add("display", "initial")
                trCancelBackToHome.Style.Add("display", "none")

            Case ViewIndexPendingConversionRateRequest.PendingAprrovalRecord
                udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010410, SeverityCode.SEVI, LogID.LOG00001))
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                Session(SESS.MessageBoxForValidationOfConcurrentUpdate) = Nothing

            Case ViewIndexPendingConversionRateRequest.PendingDeleteApprovedRecord
                udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010410, SeverityCode.SEVI, LogID.LOG00001))
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                Session(SESS.MessageBoxForValidationOfConcurrentUpdate) = Nothing

            Case Else

        End Select

    End Sub

    Private Sub DeletePendingRequest()
        Dim udtConversionRateBLL As New ExchangeRateBLL
        Dim udtConversionRateModel As ExchangeRateModel
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)
        Dim eSQL As String = Nothing

        udtConversionRateModel = Session(SESS.PendingApprovalRequest)

        udtConversionRateModel.DeleteBy = GetCurrentUser()
        udtConversionRateModel.RecordStatus = ExchangeRateModel.ERS_RECORD_STATUS_D

        eSQL = udtConversionRateBLL.UpdateExchangeRateRecordStatusInStaging(udtConversionRateModel)

        If eSQL Is Nothing Then
            HideLayout()
            RenderingLayout()

            udtAuditLog.AddDescripton("Conversion Rate Staging ID", udtConversionRateModel.ExchangeRateStagingID.ToString)
            udtAuditLog.WriteLog(LogID.LOG00012, AuditLogDescription.LOG00012)

            'Message Box - Success
            udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010410, SeverityCode.SEVI, LogID.LOG00003))
            udcInfoMessageBox.BuildMessageBox()
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete

            Session(SESS.MessageBoxForValidationOfConcurrentUpdate) = Nothing
        Else
            'Message Box - Fail
            Dim sm As SystemMessage = New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL)
            udcMessageBox.AddMessage(sm)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00018, "Delete conversion rate request fail")

            Session(SESS.MessageBoxForValidationOfConcurrentUpdate) = sm
        End If
    End Sub

    Private Sub DeleteApprovedRecord()
        Dim udtConversionRateModelPermanent As ExchangeRateModel
        Dim udtConversionRateModelStaging As ExchangeRateModel
        Dim strEffectiveDate As String
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)
        Dim eSQL As String = Nothing

        udtConversionRateBLL = New ExchangeRateBLL
        udtGeneralFunction = New GeneralFunction
        udtFormatter = New Formatter

        udtConversionRateModelPermanent = Session(SESS.NextConversionRate)

        strEffectiveDate = udtFormatter.formatInputDate(Day(udtConversionRateModelPermanent.EffectiveDate) & "-" & _
                                                        Month(udtConversionRateModelPermanent.EffectiveDate) & "-" & _
                                                        Year(udtConversionRateModelPermanent.EffectiveDate))

        udtConversionRateModelStaging = udtConversionRateBLL.CreateExchangeRateRequestModel(String.Empty, _
                                                                                 strEffectiveDate, _
                                                                                 CStr(udtConversionRateModelPermanent.ExchangeRate), _
                                                                                 ExchangeRateModel.ERS_ACTION_D, _
                                                                                 GetCurrentUser(), _
                                                                                 udtConversionRateModelPermanent.ExchangeRateID)

        eSQL = udtConversionRateBLL.WriteExchangeRateRequestInStaging(udtConversionRateModelStaging)

        If eSQL Is Nothing Then
            HideLayout()
            RenderingLayout()

            udtAuditLog.AddDescripton("Conversion Rate ID", udtConversionRateModelPermanent.ExchangeRateID.ToString)
            udtAuditLog.WriteLog(LogID.LOG00016, AuditLogDescription.LOG00016)

            'Message Box - Success
            udcInfoMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT010410, SeverityCode.SEVI, LogID.LOG00004))
            udcInfoMessageBox.BuildMessageBox()
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete

            Session(SESS.MessageBoxForValidationOfConcurrentUpdate) = Nothing
        Else
            'Message Box - Fail
            Dim sm As SystemMessage = New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL)
            udcMessageBox.AddMessage(sm)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "Delete approved conversion rate record fail")

            Session(SESS.MessageBoxForValidationOfConcurrentUpdate) = sm
        End If
    End Sub

    Private Sub ibtnDialogConfirm_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnDialogConfirm.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)

        If mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.PendingAprrovalRecord Then
            udtAuditLog.WriteLog(LogID.LOG00010, AuditLogDescription.LOG00010)
            DeletePendingRequest()
        End If

        If mvNextConversionRateInfo.ActiveViewIndex = ViewIndexNextConversionRateInfo.NextRecord Then
            udtAuditLog.WriteLog(LogID.LOG00014, AuditLogDescription.LOG00014)
            DeleteApprovedRecord()
        End If

    End Sub

    Private Sub ibtnDialogCancel_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnDialogCancel.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)

        If mvPendingConversionRateRequest.ActiveViewIndex = ViewIndexPendingConversionRateRequest.PendingAprrovalRecord Then
            udtAuditLog.WriteLog(LogID.LOG00011, AuditLogDescription.LOG00011)
        End If

        If mvNextConversionRateInfo.ActiveViewIndex = ViewIndexNextConversionRateInfo.NextRecord Then
            udtAuditLog.WriteLog(LogID.LOG00015, AuditLogDescription.LOG00015)
        End If

        'Retain message box when popup displays
        If Not Session(SESS.MessageBoxForValidationOfConcurrentUpdate) Is Nothing Then
            Dim sm As SystemMessage = Session(SESS.MessageBoxForValidationOfConcurrentUpdate)
            udcMessageBox.AddMessage(sm)
            RenderingLayout()
            udcInfoMessageBox.Visible = False
            udcMessageBox.Visible = True
            Session(SESS.MessageBoxForValidationOfConcurrentUpdate) = sm
        Else
            RenderingLayout()
        End If

    End Sub

    Private Sub ibtnDeletePendingRequest_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnDeletePendingRequest.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)
        udtAuditLog.WriteLog(LogID.LOG00009, AuditLogDescription.LOG00009)

        lblMsg.Text = HttpContext.GetGlobalResourceObject("Text", "ConfirmDeleteConversionRate")
        ModalPopupExtenderConfirmDeletePendingRequest.Show()

        'Retain message box when popup displays
        If Not Session(SESS.MessageBoxForValidationOfConcurrentUpdate) Is Nothing Then
            Dim sm As SystemMessage = Session(SESS.MessageBoxForValidationOfConcurrentUpdate)
            udcMessageBox.AddMessage(sm)
            RenderingLayout()
            udcInfoMessageBox.Visible = False
            udcMessageBox.Visible = True
            Session(SESS.MessageBoxForValidationOfConcurrentUpdate) = sm
        Else
            RenderingLayout()
        End If
    End Sub

    Private Sub ibtnDeleteApprovedRecord_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnDeleteApprovedRecord.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)
        udtAuditLog.WriteLog(LogID.LOG00013, AuditLogDescription.LOG00013)

        lblMsg.Text = HttpContext.GetGlobalResourceObject("Text", "ConfirmDeleteApprovedConversionRateQ")
        ModalPopupExtenderConfirmDeleteApprovedRecord.Show()

        'Retain message box when popup displays
        If Not Session(SESS.MessageBoxForValidationOfConcurrentUpdate) Is Nothing Then
            Dim sm As SystemMessage = Session(SESS.MessageBoxForValidationOfConcurrentUpdate)
            udcMessageBox.AddMessage(sm)
            RenderingLayout()
            udcInfoMessageBox.Visible = False
            udcMessageBox.Visible = True
            Session(SESS.MessageBoxForValidationOfConcurrentUpdate) = sm
        Else
            RenderingLayout()
        End If

    End Sub

    Private Function IsValidInput() As Boolean
        Dim blnResultEffectiveDate As Boolean = True
        Dim blnResultConversionRate As Boolean = True
        Dim udtFormatter As New Formatter
        Dim udtGeneralFunction As New GeneralFunction
        Dim udtValidator As New Validator
        Dim udtSysMessage As SystemMessage

        ResetAlert()

        'Validation on Effective Date
        If udtValidator.IsEmpty(txtEffectiveDate.Text.Trim) Then
            udcMessageBox.AddMessage(FunctCode.FUNT010410, SeverityCode.SEVE, LogID.LOG00001)
            blnResultEffectiveDate = False
        End If

        If blnResultEffectiveDate = True Then
            'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'udtSysMessage = udtValidator.chkInputDate(txtEffectiveDate.Text, True)
            udtSysMessage = udtValidator.chkInputDate(txtEffectiveDate.Text, True, False)
            'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

            If Not udtSysMessage Is Nothing Then
                udcMessageBox.AddMessage(udtSysMessage, "%s", HttpContext.GetGlobalResourceObject("Text", "EffectiveDate"))
                blnResultEffectiveDate = False
            End If
        End If

        If blnResultEffectiveDate = True Then
            If udtFormatter.convertDate(txtEffectiveDate.Text, "") <= Today() Then
                udcMessageBox.AddMessage(FunctCode.FUNT010410, SeverityCode.SEVE, LogID.LOG00003)
                blnResultEffectiveDate = False
            End If
        End If

        'Validation on Exchange Rate
        If udtValidator.IsEmpty(txtConversionRate.Text.Trim) Then
            udcMessageBox.AddMessage(FunctCode.FUNT010410, SeverityCode.SEVE, LogID.LOG00002)
            blnResultConversionRate = False
        End If

        If blnResultConversionRate = True Then
            Dim arrStrRate As String() = txtConversionRate.Text.Trim.Split(".")
            If arrStrRate.Length > 2 Then
                udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, LogID.LOG00029, "%s", HttpContext.GetGlobalResourceObject("Text", "ConversionRate"))
                blnResultConversionRate = False
            End If
        End If

        If blnResultConversionRate = True Then
            If CDec(txtConversionRate.Text.Trim) <= 0 Then
                udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, LogID.LOG00029, "%s", HttpContext.GetGlobalResourceObject("Text", "ConversionRate"))
                blnResultConversionRate = False
            End If
        End If

        If blnResultConversionRate = True Then
            Dim strConversionRateValueUpperLimit = udtGeneralFunction.getSystemParameter("ConversionRateValueUpperLimit")

            If CDec(txtConversionRate.Text.Trim) > CDec(strConversionRateValueUpperLimit) Then
                udcMessageBox.AddMessage(FunctCode.FUNT010410, SeverityCode.SEVE, LogID.LOG00004, "%s", strConversionRateValueUpperLimit)
                blnResultConversionRate = False
            End If
        End If

        If blnResultConversionRate = True Then
            Dim strConversionRateValueDecimalPlace As String = udtGeneralFunction.getSystemParameter("ConversionRateValueDecimalPlace")

            Dim arrStrRate As String() = txtConversionRate.Text.Trim.Split(".")
            If arrStrRate.Length = 2 Then
                If arrStrRate(1).Length > CInt(strConversionRateValueDecimalPlace) Then
                    udcMessageBox.AddMessage(FunctCode.FUNT010410, SeverityCode.SEVE, LogID.LOG00005, "%s", strConversionRateValueDecimalPlace)
                    blnResultConversionRate = False
                End If
            End If
        End If

        'Display Alert Image
        If Not blnResultEffectiveDate Then
            imgEffectiveDateAlert.Visible = True
        End If

        If Not blnResultConversionRate Then
            imgConversionRateAlert.Visible = True
        End If

        Return (blnResultEffectiveDate And blnResultConversionRate)
    End Function

    Private Function formatConversionRate(ByVal strConversionRate As String) As String
        Dim udtGeneralFunction As New GeneralFunction
        Dim strResult As String = String.Empty
        Dim strZero As String = String.Empty
        Dim arrStrRate As String() = strConversionRate.Trim.Split(".")

        Dim strConversionRateValueDecimalPlace As String = udtGeneralFunction.getSystemParameter("ConversionRateValueDecimalPlace")

        For intLength As Integer = 0 To CInt(strConversionRateValueDecimalPlace) - 1
            strZero += "0"
        Next

        Select Case arrStrRate.Length
            Case 1
                strResult = Int(arrStrRate(0)).ToString + "." + strZero
            Case 2
                strResult = Int(arrStrRate(0)).ToString + "." + Left(arrStrRate(1) + strZero, CInt(strConversionRateValueDecimalPlace))
            Case Else
                strResult = strConversionRate
        End Select

        Return strResult
    End Function

    Private Sub ibtnCancelBackToHome_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnCancelBackToHome.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)
        udtAuditLog.WriteLog(LogID.LOG00017, AuditLogDescription.LOG00017)

        udtAuditLog = Nothing
        udtAuditLog = New AuditLogEntry(FunctCode.FUNT010410, Me)

        ResetLayout()
        ClearSession()
        GetCurrentConversionRate(udtAuditLog)
        GetNextConversionRate(udtAuditLog)
        GetPendingApprovalConversionRateRequest(udtAuditLog)
        RenderingLayout()
    End Sub
End Class
