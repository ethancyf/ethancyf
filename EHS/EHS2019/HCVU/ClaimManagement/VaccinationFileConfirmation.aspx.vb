Imports System.Data.SqlClient
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.HCVUUser
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.StudentFile
Imports Common.DataAccess
Imports Common.Format

Partial Public Class VaccinationFileConfirmation ' 010416
    Inherits BasePageWithControl

#Region "Private Class"

    Private Class SESS
        Public Const SearchResultDT As String = "010416_SearchResultDT"
        Public Const DetailModel As String = "010416_StudentFileHeader"
        Public Const DetailEntryDT As String = "010416_StudentFileEntryDT"
    End Class

    Private Class ConfirmAction
        Public Const Upload As String = "UPLOAD"
        Public Const Rectify As String = "RECT"
        Public Const Claim As String = "CLAIM"
        Public Const ClaimReactivate As String = "CLAIMREACT"
        Public Const Remove As String = "REMOVE"

    End Class

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT010416

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, "[StdFileConfirmation] Page Loaded")

            InitControlOnce()
        Else
            Select Case mvCore.GetActiveView.ID
                Case vDetail.ID
                    AddHandler udcStudentFileDetail.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected

            End Select
        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    End Sub

    Private Sub InitControlOnce()
        ddlGAction.Items.Clear()
        ddlGAction.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
        ddlGAction.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ConfirmVaccinationFile"), ConfirmAction.Upload))
        ddlGAction.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ConfirmRectifiedVaccinationFile"), ConfirmAction.Rectify))
        ddlGAction.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ConfirmClaimCreation"), ConfirmAction.Claim))
        ddlGAction.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ConfirmClaimReactivation"), ConfirmAction.ClaimReactivate))
        ddlGAction.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ConfirmRemovalOfVaccinationFile"), ConfirmAction.Remove))

        ddlGAction.SelectedIndex = 0

        mvCore.SetActiveView(vGrid)

    End Sub

    '

    Protected Sub ddlGAction_SelectedIndexChanged(sender As Object, e As EventArgs)
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("SelectedValue", ddlGAction.SelectedValue)
        udtAuditLog.WriteStartLog(LogID.LOG00001, "[StdFileConfirmation] Index - Action change")

        BindGrid()

        udtAuditLog.WriteEndLog(LogID.LOG00002, "[StdFileConfirmation] Index - Action change success")

    End Sub

    Private Sub BindGrid()
        Dim dt As DataTable = Nothing
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID

        Select Case ddlGAction.SelectedValue
            Case String.Empty
                gvStudentFile.Visible = False

            Case ConfirmAction.Upload
                dt = (New StudentFileBLL).SearchStudentFile(String.Empty, String.Empty, String.Empty, String.Empty, _
                                                                     strUserID, String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, _
                                                                     Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload))


            Case ConfirmAction.Rectify
                dt = (New StudentFileBLL).SearchStudentFile(String.Empty, String.Empty, String.Empty, String.Empty, _
                                                                     strUserID, String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, _
                                                                     Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify))

            Case ConfirmAction.Claim
                dt = (New StudentFileBLL).SearchStudentFile(String.Empty, String.Empty, String.Empty, String.Empty, _
                                                                     strUserID, String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, _
                                                                     Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim))

            Case ConfirmAction.ClaimReactivate
                dt = (New StudentFileBLL).SearchStudentFile(String.Empty, String.Empty, String.Empty, String.Empty, _
                                                                     strUserID, String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, _
                                                                     Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx))

            Case ConfirmAction.Remove
                dt = (New StudentFileBLL).SearchStudentFile(String.Empty, String.Empty, String.Empty, String.Empty, _
                                                                     strUserID, String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, _
                                                                     Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove))
        End Select

        If Not IsNothing(dt) Then
            Session(SESS.SearchResultDT) = dt

            If dt.Rows.Count = 0 Then
                udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMessageBox.BuildMessageBox()

                gvStudentFile.Visible = False

            Else
                gvStudentFile.Visible = True

                Me.GridViewDataBind(gvStudentFile, dt, "Student_File_ID", "ASC", False)

            End If

        End If

    End Sub

    '

    Protected Sub gvStudentFile_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            ' Vaccination Date
            Dim lblGVaccinationDate As Label = e.Row.FindControl("lblGVaccinationDate")

            If IsDBNull(dr("Service_Receive_Dtm")) Then
                lblGVaccinationDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblGVaccinationDate.Text = udtFormatter.formatDisplayDate(dr("Service_Receive_Dtm"))
            End If

            ' Vaccination Report Generation Date
            Dim lblGVaccinationReportGenerationDate As Label = e.Row.FindControl("lblGVaccinationReportGenerationDate")

            If IsDBNull(dr("Final_Checking_Report_Generation_Date")) Then
                lblGVaccinationReportGenerationDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblGVaccinationReportGenerationDate.Text = udtFormatter.formatDisplayDate(dr("Final_Checking_Report_Generation_Date"))
            End If

            ' Subsidy / Dose to Inject
            Dim lblGDoseToInject As Label = e.Row.FindControl("lblGDoseToInject")

            If IsDBNull(dr("Subsidize_Code")) OrElse dr("Subsidize_Code").ToString.Trim() = String.Empty Then
                lblGDoseToInject.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblGDoseToInject.Text = String.Format("{0}<br><br>{1}", _
                                                        dr("SubsidizeDisplayName"), _
                                                        (New StaticDataBLL).GetStaticDataByColumnNameItemNo("StudentFileDoseToInject", dr("Dose")).DataValue)
            End If

            ' Upload By and Time
            Dim lblGUploadByAndTime As Label = e.Row.FindControl("lblGUploadByAndTime")
            lblGUploadByAndTime.Text = String.Format("{0}<br>{1}", dr("Upload_By"), udtFormatter.formatDateTime(dr("Upload_Dtm")))

            ' Status
            Dim lblGStatus As Label = e.Row.FindControl("lblGStatus")
            Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", dr("Record_Status"), lblGStatus.Text, String.Empty, String.Empty)

        End If

    End Sub

    Protected Sub gvStudentFile_PreRender(sender As Object, e As EventArgs)
        GridViewPreRenderHandler(sender, e, SESS.SearchResultDT)

    End Sub

    Protected Sub gvStudentFile_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If TypeOf e.CommandSource Is LinkButton Then
            Dim strStudentFileID As String = DirectCast(e.CommandSource, LinkButton).Text.Trim

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.AddDescripton("Student File ID", strStudentFileID)
            udtAuditLog.WriteStartLog(LogID.LOG00003, "[StdFileConfirmation] Result - Student File ID click")

            BuildDetail(strStudentFileID)

            udtAuditLog.WriteEndLog(LogID.LOG00004, "[StdFileConfirmation] Result - Student File ID click success")

        End If

    End Sub

    Protected Sub gvStudentFile_Sorting(sender As Object, e As GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS.SearchResultDT)

    End Sub

    Protected Sub gvStudentFile_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS.SearchResultDT)

    End Sub

    ' Detail

    Private Sub BuildDetail(strStudentFileID As String)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = Nothing
        Dim dt As DataTable = Nothing

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Select Case ddlGAction.SelectedValue
            Case ConfirmAction.Remove, ConfirmAction.ClaimReactivate
                udtStudentFile = udtStudentFileBLL.GetStudentFileHeader(strStudentFileID, blnWithEntry:=False)
                dt = udtStudentFileBLL.GetStudentFileEntrySearch(strStudentFileID)

            Case Else
                udtStudentFile = udtStudentFileBLL.GetStudentFileHeaderStaging(strStudentFileID, blnWithEntry:=False)
                dt = udtStudentFileBLL.GetStudentFileEntryStagingSearch(strStudentFileID)

                If IsNothing(udtStudentFile) Then
                    udtStudentFile = udtStudentFileBLL.GetStudentFileHeader(strStudentFileID, blnWithEntry:=False)
                    dt = udtStudentFileBLL.GetStudentFileEntrySearch(strStudentFileID)
                End If
        End Select
        ' CRE19-001 (VSS 2019) [End][Winnie]

        AddHandler udcStudentFileDetail.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected

        udcStudentFileDetail.Build(udtStudentFile, dt)

        Session(SESS.DetailModel) = udtStudentFile
        Session(SESS.DetailEntryDT) = dt

        ' Check access right
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim blnBlock As Boolean = False

        Select Case udtStudentFile.RecordStatusEnum
            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload
                If udtStudentFile.UploadBy = strUserID Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
                    blnBlock = True
                End If

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify
                If udtStudentFile.LastRectifyBy = strUserID Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)
                    blnBlock = True
                End If

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim
                If udtStudentFile.ClaimUploadBy = strUserID Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)
                    blnBlock = True
                End If

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove
                If udtStudentFile.RequestRemoveBy = strUserID Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006)
                    blnBlock = True
                End If

                ' CRE19-001 (VSS 2019) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx
                If udtStudentFile.RequestClaimReactivateBy = strUserID Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00011)
                    blnBlock = True
                End If
                ' CRE19-001 (VSS 2019) [End][Winnie]
        End Select


        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Check Status changed
        Select Case ddlGAction.SelectedValue
            Case ConfirmAction.Upload
                If udtStudentFile.RecordStatusEnum <> StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00014)
                    blnBlock = True
                End If

            Case ConfirmAction.Rectify
                If udtStudentFile.RecordStatusEnum <> StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00014)
                    blnBlock = True
                End If

            Case ConfirmAction.Claim
                If udtStudentFile.RecordStatusEnum <> StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00014)
                    blnBlock = True
                End If

            Case ConfirmAction.ClaimReactivate
                If udtStudentFile.RecordStatusEnum <> StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00014)
                    blnBlock = True
                End If

            Case ConfirmAction.Remove
                If udtStudentFile.RecordStatusEnum <> StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00014)
                    blnBlock = True
                End If

        End Select
        ' CRE19-001 (VSS 2019) [End][Winnie]


        If blnBlock Then
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.BuildMessageBox()

            ibtnDConfirm.Enabled = False
            ibtnDReject.Enabled = False
            ibtnDConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")
            ibtnDReject.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RejectDisableBtn")

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00005, "[StdFileConfirmation] Detail - Show message for cannot confirm/reject due to same user or status changed")

        Else
            ibtnDConfirm.Enabled = True
            ibtnDReject.Enabled = True
            ibtnDConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")
            ibtnDReject.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RejectBtn")

        End If

        mvCore.SetActiveView(vDetail)

    End Sub

    Protected Sub ibtnDBack_Click(sender As Object, e As ImageClickEventArgs)
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00006, "[StdFileConfirmation] Detail - Back click")

        mvCore.SetActiveView(vGrid)

        BindGrid()
    End Sub

    Protected Sub ibtnDConfirm_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00007, "[StdFileConfirmation] Detail - Confirm click")

        lblDPText.Text = Me.GetGlobalResourceObject("Text", "ConfirmQ")
        ibtnDPConfirmConfirm.Visible = True
        ibtnDPConfirmReject.Visible = False

        mpeDetailPopup.Show()

    End Sub

    Protected Sub ibtnDReject_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00008, "[StdFileConfirmation] Detail - Reject click")

        lblDPText.Text = Me.GetGlobalResourceObject("Text", "ConfirmRejectQ")
        ibtnDPConfirmConfirm.Visible = False
        ibtnDPConfirmReject.Visible = True

        mpeDetailPopup.Show()

    End Sub

    Public Sub ddlDClassName_DropDownListSelected(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim ddlClassName As DropDownList = DirectCast(sender, DropDownList)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udtAuditLog.AddDescripton("Class Name", ddlClassName.SelectedValue)
        udtAuditLog.WriteLog(LogID.LOG00018, "[StdFileConfirmation] Detail - Class Name select")
    End Sub

    ' Finish

    Protected Sub ibtnFReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00009, "[StdFileConfirmation] Finish - Return click")

        mvCore.SetActiveView(vGrid)

        BindGrid()

    End Sub

    ' Concurrent Update

    Protected Sub ibtnCUReturn_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00010, "[StdFileConfirmation] ConcurrentUpdate - Return click")

        Response.Redirect(Request.RawUrl)

    End Sub

    ' Popup

    Protected Sub ibtnDPCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00011, "[StdFileConfirmation] Popup - Cancel click")

    End Sub

    Protected Sub ibtnDPConfirmConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim dt As DataTable = Session(SESS.DetailEntryDT)
        udtStudentFile = udtStudentFile.Clone

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.AddDescripton("Action", ddlGAction.SelectedValue)
        udtAuditLog.WriteStartLog(LogID.LOG00012, "[StdFileConfirmation] Popup - Confirm click")

        Dim udtStudentFilePerm As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeader(udtStudentFile.StudentFileID, False)
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dtmNow As DateTime = DateTime.Now

        Dim udtDB As New Database

        Try
            udtDB.BeginTransaction()

            Select Case ddlGAction.SelectedValue
                Case ConfirmAction.Upload
                    udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload
                    udtStudentFile.FileConfirmBy = strUserID
                    udtStudentFile.FileConfirmDtm = dtmNow
                    udtStudentFile.UpdateBy = strUserID
                    udtStudentFile.UpdateDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtStudentFile, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "{FileID}", udtStudentFile.StudentFileID)

                Case ConfirmAction.Claim
                    udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim
                    udtStudentFile.FileConfirmBy = strUserID
                    udtStudentFile.FileConfirmDtm = dtmNow
                    udtStudentFile.UpdateBy = strUserID
                    udtStudentFile.UpdateDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtStudentFile, udtDB)

                    udtStudentFilePerm.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim
                    udtStudentFilePerm.UpdateBy = strUserID
                    udtStudentFilePerm.UpdateDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFilePerm, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "{FileID}", udtStudentFile.StudentFileID)

                Case ConfirmAction.Rectify
                    If dt.Rows.Count = 0 Then
                        ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        Select Case Formatter.StringToEnum(GetType(StudentFileHeaderModel.RecordStatusEnumClass), udtStudentFile.RequestRectifyStatus)
                            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration
                                udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration

                            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                                udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration

                            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim
                                ' For Report Gen Date remain unchanged, no need to gen report again
                                If udtStudentFilePerm.FinalCheckingReportGenerationDate = udtStudentFile.FinalCheckingReportGenerationDate Then
                                    udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim

                                Else
                                    udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                                End If

                            Case Else
                                Throw New NotImplementedException
                        End Select

                        udtStudentFile.RequestRectifyStatus = String.Empty
                        ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

                        udtStudentFile.UpdateBy = strUserID
                        udtStudentFile.UpdateDtm = dtmNow

                        udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtStudentFile, udtDB)

                        udtStudentFileBLL.MoveStudentFileHeaderStaging(udtStudentFile, udtDB)

                    Else
                        udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify
                        udtStudentFile.FileConfirmBy = strUserID
                        udtStudentFile.FileConfirmDtm = dtmNow
                        udtStudentFile.UpdateBy = strUserID
                        udtStudentFile.UpdateDtm = dtmNow

                        udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtStudentFile, udtDB)

                        udtStudentFilePerm.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify
                        udtStudentFilePerm.UpdateBy = strUserID
                        udtStudentFilePerm.UpdateDtm = dtmNow

                        udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFilePerm, udtDB)

                    End If

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "{FileID}", udtStudentFile.StudentFileID)

                Case ConfirmAction.Remove
                    udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.Removed
                    udtStudentFile.UpdateBy = strUserID
                    udtStudentFile.UpdateDtm = dtmNow
                    udtStudentFile.ConfirmRemoveBy = strUserID
                    udtStudentFile.ConfirmRemoveDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFile, udtDB)

                    ' CRE19-001 (VSS 2019) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' Remove all Student temp acct
                    RemoveStudentAccount(udtStudentFile, udtDB)
                    ' CRE19-001 (VSS 2019) [End][Winnie]

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002, "{FileID}", udtStudentFile.StudentFileID)

                    ' CRE19-001 (VSS 2019) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                Case ConfirmAction.ClaimReactivate
                    udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.Completed
                    udtStudentFile.UpdateBy = strUserID
                    udtStudentFile.UpdateDtm = dtmNow
                    udtStudentFile.ConfirmClaimReactivateBy = strUserID
                    udtStudentFile.ConfirmClaimReactivateDtm = dtmNow

                    udtStudentFileBLL.ReactivateClaim(udtStudentFile, udtDB)

                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFile, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00013, "{FileID}", udtStudentFile.StudentFileID)
                    ' CRE19-001 (VSS 2019) [End][Winnie]

            End Select

            udtDB.CommitTransaction()

            mvCore.SetActiveView(vFinish)

            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLog.WriteEndLog(LogID.LOG00013, "[StdFileConfirmation] Popup - Confirm click success")

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()

            If eSQL.Number = 50000 Then
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00014, "[StdFileConfirmation] Popup - Confirm click fail")

                mvCore.SetActiveView(vConcurrentUpdate)

                Return

            Else
                udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                udtAuditLog.WriteEndLog(LogID.LOG00014, "[StdFileConfirmation] Popup - Confirm click fail")

                Throw

            End If

        Catch ex As Exception
            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00014, "[StdFileConfirmation] Popup - Confirm click fail")

            udtDB.RollBackTranscation()

            Throw

        End Try

    End Sub

    Protected Sub ibtnDPConfirmReject_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        udtStudentFile = udtStudentFile.Clone

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.AddDescripton("Action", ddlGAction.SelectedValue)
        udtAuditLog.WriteStartLog(LogID.LOG00015, "[StdFileConfirmation] Popup - Reject click")

        Dim udtStudentFilePerm As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeader(udtStudentFile.StudentFileID, False)

        Dim udtDB As New Database
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dtmNow As DateTime = DateTime.Now

        Try
            udtDB.BeginTransaction()

            Select Case udtStudentFile.RecordStatusEnum
                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload
                    udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.Removed
                    udtStudentFile.UpdateBy = strUserID
                    udtStudentFile.UpdateDtm = dtmNow
                    udtStudentFile.ConfirmRemoveBy = strUserID
                    udtStudentFile.ConfirmRemoveDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtStudentFile, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00007, "{FileID}", udtStudentFile.StudentFileID)

                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify
                    ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    'udtStudentFilePerm.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                    udtStudentFilePerm.RecordStatus = udtStudentFilePerm.RequestRectifyStatus
                    udtStudentFilePerm.RequestRectifyStatus = String.Empty
                    ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

                    udtStudentFilePerm.UpdateBy = strUserID
                    udtStudentFilePerm.UpdateDtm = dtmNow

                    udtStudentFileBLL.DeleteStudentFileHeaderStaging(udtStudentFile, udtDB)
                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFilePerm, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00008, "{FileID}", udtStudentFile.StudentFileID)

                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim
                    udtStudentFilePerm.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim
                    udtStudentFilePerm.UpdateBy = strUserID
                    udtStudentFilePerm.UpdateDtm = dtmNow

                    udtStudentFileBLL.DeleteStudentFileHeaderStaging(udtStudentFile, udtDB)
                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFilePerm, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00009, "{FileID}", udtStudentFile.StudentFileID)

                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx
                    udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended
                    udtStudentFile.RequestClaimReactivateBy = String.Empty
                    udtStudentFile.RequestClaimReactivateDtm = Nothing
                    udtStudentFile.UpdateBy = strUserID
                    udtStudentFile.UpdateDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFile, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00012, "{FileID}", udtStudentFile.StudentFileID)

                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove
                    Select Case udtStudentFile.RequestRemoveFunction
                        Case "RECTIFICATION"
                            ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            If udtStudentFile.Precheck Then
                                udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration
                            Else
                                udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                            End If
                            ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

                        Case "CLAIMCREATION"
                            udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim
                        Case Else
                            Throw New NotImplementedException
                    End Select

                    udtStudentFile.RequestRemoveBy = String.Empty
                    udtStudentFile.RequestRemoveDtm = Nothing
                    udtStudentFile.RequestRemoveFunction = String.Empty
                    udtStudentFile.UpdateBy = strUserID
                    udtStudentFile.UpdateDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFile, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00010)

            End Select

            udtDB.CommitTransaction()

            mvCore.SetActiveView(vFinish)

            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLog.WriteEndLog(LogID.LOG00016, "[StdFileConfirmation] Popup - Reject click success")

        Catch eSql As SqlException
            udtDB.RollBackTranscation()

            If eSql.Number = 50000 Then
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00017, "[StdFileConfirmation] Popup - Reject click fail")

                mvCore.SetActiveView(vConcurrentUpdate)

                Return

            Else
                udtAuditLog.AddDescripton("SqlException", eSql.Message)
                udtAuditLog.WriteEndLog(LogID.LOG00017, "[StdFileConfirmation] Popup - Reject click fail")

                Throw

            End If

        Catch ex As Exception
            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00017, "[StdFileConfirmation] Popup - Reject click fail")

            udtDB.RollBackTranscation()

            Throw

        End Try

    End Sub

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As ServiceProviderModel
        Return Nothing
    End Function

#End Region

#Region "SF Search"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        Return Nothing
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer
        Return -1
    End Function

    Protected Overrides Sub SF_ConfirmSearch_Click()
    End Sub

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub

#End Region

#Region "Remove Vaccination File"
    ' CRE19-001 (VSS 2019) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub RemoveStudentAccount(ByVal udtStudentFile As StudentFileHeaderModel, ByVal udtDB As Database)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentList As StudentFileEntryModelCollection = udtStudentFileBLL.GetStudentFileEntry(udtStudentFile.StudentFileID)

        For Each udtStudent As StudentFileEntryModel In udtStudentList

            If udtStudent.TempVoucherAccID <> String.Empty Then

                Dim udteHSAccountMaintBLL As eHSAccountMaintBLL = New eHSAccountMaintBLL
                Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL

                Dim udtTempAccount As EHSAccountModel = Nothing
                udtTempAccount = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtStudent.TempVoucherAccID)

                If udtTempAccount IsNot Nothing Then

                    Dim udtTempPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = Nothing
                    udtTempPersonalInfo = udtTempAccount.EHSPersonalInformationList(0)

                    ' =====================================================
                    ' For case below will not remove existing temp account:
                    ' (1) Account being "Validated"
                    ' (2) Account being "Removed"
                    ' (3) Account being "Validating"
                    ' (4) Account with transaction
                    ' =====================================================
                    If udtTempAccount.RecordStatus <> EHSAccountModel.TempAccountRecordStatusClass.Validated _
                        AndAlso udtTempAccount.RecordStatus <> EHSAccountModel.TempAccountRecordStatusClass.Removed _
                        AndAlso Not udtTempPersonalInfo.Validating _
                        AndAlso udtTempAccount.TransactionID = String.Empty Then

                        Me.RemoveTempAcct(udtTempAccount, udtStudentFile.ConfirmRemoveBy, udtDB)

                    End If
                End If
            End If

        Next
        ' CRE19-001 (VSS 2019) [End][Winnie]

    End Sub

    ''' <summary>
    ''' Remove unused Temporary EHS Account
    ''' </summary>
    ''' <param name="udtEHSAccount"></param>
    ''' <param name="strUpdateBy"></param>
    ''' <param name="udtDB"></param>
    ''' <remarks>Simplified version of Back Office eHA remove temp acct</remarks>
    Private Sub RemoveTempAcct(ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, Optional udtDB As Database = Nothing)
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim udtEHSAccountBLL As New EHSAccountBLL
        Dim udtImmDBLL As New ImmD.ImmDBLL

        If IsNothing(udtDB) Then udtDB = New Database

        Dim dtmCurrent As DateTime = udtGeneralFunction.GetSystemDateTime

        ' Update Temp EHS Account Status to "D"
        udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccount, strUpdateBy, dtmCurrent)

        ' Delete related record in table "TempVoucherAccPendingVerify"
        udtImmDBLL.DeleteTempVRAcctInPendingTable(udtDB, udtEHSAccount.VoucherAccID)
    End Sub

#End Region

End Class
