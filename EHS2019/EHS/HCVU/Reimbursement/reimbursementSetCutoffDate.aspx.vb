Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports HCVU.ReimbursementBLL

Partial Public Class reimbursementSetCutoffDate
    Inherits BasePageWithGridView

    ' FunctionCode = FunctCode.FUNT010407

#Region "Private Classes"

    Private Class ViewIndex
        Public Const UpdateError As Integer = 0
        Public Const InputDate As Integer = 1
        Public Const ConfirmDate As Integer = 2
        Public Const Allocated As Integer = 3
        Public Const Reset As Integer = 4
    End Class

    Private Class AuditLogDescription
        Public Const Load As String = "Reimbursement Set Cutoff Date load" '00000
        Public Const PresetCutoffDateStart As String = "Preset Cutoff Date start"
        Public Const PresetCutoffDateSuccessful As String = "Preset Cutoff Date successful"
        Public Const SetCutoffDateClick As String = "Set Cutoff Date click"
        Public Const SetCutoffDateSuccessful As String = "Set Cutoff Date successful"
        Public Const SetCutoffDateFail As String = "Set Cutoff Date fail" '00005
        Public Const ConfirmCutoffDateClick As String = "Confirm Cutoff Date click"
        Public Const ConfirmCutoffDateSuccessful As String = "Confirm Cutoff Date successful"
        Public Const ConfirmCutoffDateFail As String = "Confirm Cutoff Date fail"
        Public Const ConfirmCutoffDateBackClick As String = "Confirm Cutoff Date Back click"
        Public Const AllocatedReturnClick As String = "Allocated Return click" '00010
        Public Const ResetClick As String = "Reset click"
        Public Const ResetConfirmClick As String = "Reset Confirm click"
        Public Const ResetCutoffDateSuccessful As String = "Reset Cutoff Date successful"
        Public Const ResetCutoffDateFail As String = "Reset Cutoff Date fail"
        Public Const ResetCancelClick As String = "Reset Cancel click" '00015
        Public Const ResetReturnClick As String = "Reset Return click"
        Public Const ErrorBackClick As String = "Error Back click"
    End Class

    Private Class ErrorMessageBoxHeaderKey
        Public Const SearchFail As String = "SearchFail"
        Public Const ValidationFail As String = "ValidationFail"
    End Class

#End Region

#Region "Fields"

    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtReimbursementBLL As New ReimbursementBLL
    Private udtValidator As New Validator

#End Region

#Region "Session Constants"

    Private SESS_CurrentReimburseIDTSMP As String = "010407_CurrentReimburseIDTSMP"

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            ' Set function code
            FunctionCode = FunctCode.FUNT010407

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditLogDescription.Load)

            PresetCutoffDate()

            ' 2009-07-29 avoid double post back in firefox

            ' Browser: Firefox
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnResetConfirm)

        End If

    End Sub

#End Region

    Private Sub PresetCutoffDate()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, AuditLogDescription.PresetCutoffDateStart)

        Dim dtReim As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(Nothing, ReimbursementStatus.StartReimbursement, _
                                                                                                ReimbursementAuthorisationStatus.Active, Nothing)

        If dtReim.Rows.Count = 0 Then
            DeduceDefaultCutoffDate()

            udtAuditLogEntry.AddDescripton("Deduce new Cutoff Date", "Y")
            udtAuditLogEntry.AddDescripton("Cutoff Date", txtCutoffDate.Text)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.PresetCutoffDateSuccessful)

        Else
            udtAuditLogEntry.AddDescripton("Previous active Reimbursement ID found", "Y")

            Dim drReim As DataRow = dtReim.Rows(0)
            Dim strReimID As String = CStr(drReim("Reimburse_ID")).Trim

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            Dim dt As ReimbursementDataTable = udtReimbursementBLL.GetReimbursementProgress(strReimID)

            ' First check if this Reimbursement ID has been reimbursed
            If dt.AllSchemeIsReimbursed Then
                DeduceDefaultCutoffDate()

                udtAuditLogEntry.AddDescripton("Previous active Reimbursement ID reimbursed", "Y")
                udtAuditLogEntry.AddDescripton("Deduce new Cutoff Date", "Y")
                udtAuditLogEntry.AddDescripton("Cutoff Date", txtCutoffDate.Text)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.PresetCutoffDateSuccessful)

                Return
            End If

            For Each dr As DataRow In dt.Rows
                If Not IsDBNull(dr("Hold_By")) Then
                    txtCutoffDate.Text = Convert.ToDateTime(drReim("CutOff_Date")).ToString(udtFormatter.EnterDateFormat)
                    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

                    lblReimID.Text = strReimID
                    hfReimID.Value = strReimID

                    ' Message: Reimbursement process has been started. No update is allowed.
                    udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
                    udcInfoBox.BuildMessageBox()
                    udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information

                    EnableSetCutoffDate(False, False)

                    udtAuditLogEntry.AddDescripton("Previous active Reimbursement ID started reimbursement", "Y")
                    udtAuditLogEntry.AddDescripton("Reimbursement ID", strReimID)
                    udtAuditLogEntry.AddDescripton("Cutoff Date", txtCutoffDate.Text)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.PresetCutoffDateSuccessful)

                    Return
                End If
            Next

            txtCutoffDate.Text = Convert.ToDateTime(drReim("CutOff_Date")).ToString(udtFormatter.EnterDateFormat)
            lblReimID.Text = strReimID
            hfReimID.Value = strReimID
            Session(SESS_CurrentReimburseIDTSMP) = drReim("TSMP")

            EnableSetCutoffDate(False, True)

            udtAuditLogEntry.AddDescripton("Reimbursement ID", strReimID)
            udtAuditLogEntry.AddDescripton("Cutoff Date", txtCutoffDate.Text)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.PresetCutoffDateSuccessful)

        End If
    End Sub

    Private Sub DeduceDefaultCutoffDate()
        txtCutoffDate.Text = udtReimbursementBLL.DeduceDefaultCutoffDate()
        lblReimID.Text = Me.GetGlobalResourceObject("Text", "N/A")
        hfReimID.Value = String.Empty
        Session(SESS_CurrentReimburseIDTSMP) = Nothing

        EnableSetCutoffDate(True, False)
    End Sub

    Private Sub EnableSetCutoffDate(ByVal blnEnableSet As Boolean, ByVal blnEnableReset As Boolean)
        ibtnSet.Enabled = blnEnableSet
        ibtnSet.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(blnEnableSet, "SetBtn", "SetDisableBtn"))

        ibtnReset.Enabled = blnEnableReset
        ibtnReset.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(blnEnableReset, "ResetDateBtn", "ResetDateDisableBtn"))

        lblCutoffDate.Visible = Not blnEnableSet
        txtCutoffDate.Visible = blnEnableSet
        ibtnCutoffDate.Visible = blnEnableSet

        If lblCutoffDate.Visible Then lblCutoffDate.Text = udtFormatter.convertDate(txtCutoffDate.Text.Trim, String.Empty)

    End Sub

    '

    Protected Sub ibtnSet_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoBox.Visible = False
        udcErrorBox.Visible = False

        imgAlertCutoffDate.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Cutoff Date Entered", txtCutoffDate.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00003, AuditLogDescription.SetCutoffDateClick)

        ' Format the input date
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'txtCutoffDate.Text = udtFormatter.formatDate(txtCutoffDate.Text.Trim)
        txtCutoffDate.Text = udtFormatter.formatInputDate(txtCutoffDate.Text.Trim)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        ' Data validation
        Dim udtSystemMessage As SystemMessage = udtValidator.chkInputDateIsValidReimbursementCutoffDate(FunctionCode, txtCutoffDate.Text)
        If Not IsNothing(udtSystemMessage) Then
            udcErrorBox.AddMessage(udtSystemMessage)
            imgAlertCutoffDate.Visible = True
        End If

        If udcErrorBox.GetCodeTable.Rows.Count = 0 Then
            lblCutoffDateConfirm.Text = udtFormatter.convertDate(txtCutoffDate.Text.Trim, String.Empty)
            lblReimIDConfirm.Text = Me.GetGlobalResourceObject("Text", "N/A")

            udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
            udcInfoBox.BuildMessageBox()
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information

            MultiViewReimSetCutoffDate.ActiveViewIndex = ViewIndex.ConfirmDate

            udtAuditLogEntry.AddDescripton("Cutoff Date", lblCutoffDateConfirm.Text)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, AuditLogDescription.SetCutoffDateSuccessful)

        Else
            udcErrorBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00004, AuditLogDescription.SetCutoffDateFail)

        End If

    End Sub

    Protected Sub ibtnReset_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00011, AuditLogDescription.ResetClick)

        popupReset.Show()
    End Sub

    Protected Sub ibtnResetConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Reimbursement ID", hfReimID.Value)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00012, AuditLogDescription.ResetConfirmClick)

        Dim dtCheck As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(hfReimID.Value, Nothing, _
                                                                                                ReimbursementAuthorisationStatus.Active, Nothing)

        For Each drCheck As DataRow In dtCheck.Rows
            If drCheck("Authorised_Status") = ReimbursementStatus.HoldForFirstAuthorisation _
                    OrElse drCheck("Authorised_Status") = ReimbursementStatus.FirstAuthorised _
                    OrElse drCheck("Authorised_Status") = ReimbursementStatus.SecondAuthorised Then
                udcErrorBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011))
                udcErrorBox.BuildMessageBox("UpdateFail")

                udtAuditLogEntry.AddDescripton("StackTrace", "Updated by others: The Reimbursement ID has started reimbursement")
                udtAuditLogEntry.WriteEndLog(LogID.LOG00014, AuditLogDescription.ResetCutoffDateFail)

                MultiViewReimSetCutoffDate.ActiveViewIndex = ViewIndex.UpdateError

                Return
            End If
        Next

        If hfReimID.Value <> String.Empty Then
            Try
                udtReimbursementBLL.UpdateReimbursementAuthorisationByReimbursementID(hfReimID.Value, ReimbursementAuthorisationStatus.Voided, _
                                                                    udtHCVUUserBLL.GetHCVUUser.UserID.Trim, Session(SESS_CurrentReimburseIDTSMP))
            Catch ex As Exception
                udcErrorBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, ex.Message))
                udcErrorBox.BuildMessageBox("UpdateFail")

                udtAuditLogEntry.AddDescripton("StackTrace", "SqlException: Timestamp is incorrect")
                udtAuditLogEntry.WriteEndLog(LogID.LOG00014, AuditLogDescription.ResetCutoffDateFail)

                MultiViewReimSetCutoffDate.ActiveViewIndex = ViewIndex.UpdateError

                Return
            End Try
        End If

        udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)
        udcInfoBox.BuildMessageBox()
        udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete

        udtAuditLogEntry.WriteEndLog(LogID.LOG00013, AuditLogDescription.ResetCutoffDateSuccessful)

        MultiViewReimSetCutoffDate.ActiveViewIndex = ViewIndex.Reset

    End Sub

    Protected Sub ibtnResetCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00015, AuditLogDescription.ResetCancelClick)
    End Sub

    '

    Protected Sub ibtnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00018, "Back Click")
        ' CRE11-021 log the missed essential information [End]

        udcInfoBox.Visible = False
        udcErrorBox.Visible = False

        MultiViewReimSetCutoffDate.ActiveViewIndex = ViewIndex.InputDate

        'Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Cutoff Date", lblCutoffDateConfirm.Text)
        udtAuditLogEntry.WriteLog(LogID.LOG00009, AuditLogDescription.ConfirmCutoffDateBackClick)

        PresetCutoffDate()
    End Sub

    Protected Sub ibtnConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Cutoff Date", lblCutoffDateConfirm.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00006, AuditLogDescription.ConfirmCutoffDateClick)

        ' Generate a new Reimbursement ID
        Dim strReimID As String = udtGeneralFunction.generateBankInNo

        Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
        Dim dtmCutoff As DateTime = DateTime.ParseExact(lblCutoffDateConfirm.Text, udtFormatter.DisplayDateFormat, Nothing)
        Dim udtDB As New Database

        ' If a previous 'S' record is found, reject the update
        Dim dtCheck As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(Nothing, ReimbursementStatus.StartReimbursement, _
                                                                                                ReimbursementAuthorisationStatus.Active, Nothing)
        If dtCheck.Rows.Count <> 0 Then
            If udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(CStr(dtCheck.Rows(0)("Reimburse_ID")).Trim, ReimbursementStatus.Reimbursed, _
                                                                                                ReimbursementAuthorisationStatus.Active, Nothing).Rows.Count = 0 Then
                udcErrorBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011))
                udcErrorBox.BuildMessageBox("UpdateFail")

                udtAuditLogEntry.AddDescripton("StackTrace", "Previous ReimbursementStatus.StartReimbursement found: The cutoff date has been set by others")
                udtAuditLogEntry.WriteEndLog(LogID.LOG00008, AuditLogDescription.ConfirmCutoffDateFail)

                MultiViewReimSetCutoffDate.ActiveViewIndex = ViewIndex.UpdateError

                Return
            End If
        End If

        ' CRE17-004 Generate a new DPAR on EHCP basis [Start][Dickson]
        ' Insert into [ReimbursementAuthorisation]
        udtReimbursementBLL.InsertReimbursementAuthorisation(udtDB, ReimbursementAuthorisationStatus.Active, strUserID, _
                                                                ReimbursementStatus.StartReimbursement, strReimID, strUserID, dtmCutoff, _
                                                                ReimbursementAuthorisationSchemeCode.All, ReimbursementVerificationCaseAvailable.Available)
        ' CRE17-004 Generate a new DPAR on EHCP basis [End][Dickson]

        lblCutoffDateAllocated.Text = lblCutoffDateConfirm.Text
        lblReimIDAllocated.Text = strReimID

        udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
        udcInfoBox.BuildMessageBox()
        udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete

        udtAuditLogEntry.AddDescripton("Cutoff Date to database", dtmCutoff)
        udtAuditLogEntry.AddDescripton("New Reimbursement ID", strReimID)
        udtAuditLogEntry.WriteEndLog(LogID.LOG00007, AuditLogDescription.ConfirmCutoffDateSuccessful)

        MultiViewReimSetCutoffDate.ActiveViewIndex = ViewIndex.Allocated

    End Sub

    '

    Protected Sub ibtnReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoBox.Visible = False
        udcErrorBox.Visible = False

        MultiViewReimSetCutoffDate.ActiveViewIndex = ViewIndex.InputDate

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00010, AuditLogDescription.AllocatedReturnClick)

        PresetCutoffDate()
    End Sub

    Protected Sub ibtnResetReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoBox.Visible = False
        udcErrorBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00016, AuditLogDescription.ResetReturnClick)

        MultiViewReimSetCutoffDate.ActiveViewIndex = ViewIndex.InputDate

        PresetCutoffDate()
    End Sub

    '

    Protected Sub ibtnErrorBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcErrorBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00017, AuditLogDescription.ErrorBackClick)

        MultiViewReimSetCutoffDate.ActiveViewIndex = ViewIndex.InputDate

        PresetCutoffDate()
    End Sub

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve Document Code which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function
    ''' <summary>
    ''' CRE11-004
    '''  Clear all working data
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub ClearWorkingData()
        MyBase.ClearWorkingData()
    End Sub
End Class
