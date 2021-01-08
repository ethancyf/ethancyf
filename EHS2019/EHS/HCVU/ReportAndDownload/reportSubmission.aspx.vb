Imports System.Web.Security.AntiXss
Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Component.HCVUUser

Partial Public Class reportSubmission
    Inherits BasePageWithGridView

#Region "Private Class"

    Private Class SESS
        Public Const ReportCriteriaUC As String = "010701_ReportCriteriaUC"
        Public Const ReportSubmissionList As String = "010701_ReportSubmissionList"
    End Class

#End Region

    ' FunctionCode = FunctCode.FUNT010701

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        FunctionCode = FunctCode.FUNT010701

        ' Check session expire
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser()

        If Not IsPostBack Then

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, "Report Submission")

            InitGridViewReportSubmission()

        End If

        If Not IsNothing(Session(SESS.ReportCriteriaUC)) Then
            ucReportCriteriaBase.Build(Session(SESS.ReportCriteriaUC))
        End If

    End Sub

    Private Sub InitGridViewReportSubmission()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Load Report List")

        ' Retrieve generate-able reports for this user
        Dim udtFileGenerationBLL As New FileGenerationBLL()
        'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Dim dtResult As DataTable = udtFileGenerationBLL.RetrieveFileGenerationListForReportSubmission((New HCVUUserBLL).GetHCVUUser().UserID)
        Dim dtResult As DataTable = udtFileGenerationBLL.RetrieveFileGenerationListForReportSubmission((New HCVUUserBLL).GetHCVUUser().UserID, SubmissionReportType.ReportSubmission)
        'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

        Session(SESS.ReportSubmissionList) = dtResult

        Me.gvReportSubmission.DataSource = dtResult
        Me.gvReportSubmission.DataBind()

        Me.gvReportSubmission.SelectedIndex = -1
        Me.ClearReportCriteria()

        If dtResult.Rows.Count = 0 Then
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcInfoMessageBox.AddMessage("990000", "I", "00001")
            Me.udcInfoMessageBox.BuildMessageBox()
        End If

        udtAuditLogEntry.AddDescripton("recordNum", dtResult.Rows.Count)
        udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Load Report List Successful")

    End Sub

#End Region

#Region "User Control Function"

    Private Sub SetupReportCriteria(ByVal strFileID As String)
        pnlReportCriteria.Visible = True

        Dim udtFileGenerationBLL As New FileGenerationBLL
        Dim udtFileGeneration As FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(strFileID)
        udtFileGeneration.UserControlList = udtFileGenerationBLL.RetrieveFileGenerationUserControl(strFileID)

        ' Report ID
        lblReportID.Text = udtFileGeneration.DisplayCode
        hfReportID.Value = strFileID

        ' Report Name
        lblReportName.Text = udtFileGeneration.FileName

        ' Report Criteria (user controls)
        Session(SESS.ReportCriteriaUC) = udtFileGeneration.UserControlList

        ucReportCriteriaBase.Build(udtFileGeneration.UserControlList)

    End Sub

    Private Sub ClearReportCriteria()
        pnlReportCriteria.Visible = False
        Session.Remove(SESS.ReportCriteriaUC)
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Me.ucReportCriteriaBase.Clear()
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
    End Sub

#End Region

    Protected Sub gvReportSubmission_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.ClearMessage()

        Me.gvReportSubmission.SelectedIndex = -1
        Me.ClearReportCriteria()

        Me.GridViewPageIndexChangingHandler(sender, e, SESS.ReportSubmissionList)
    End Sub

    Protected Sub gvReportSubmission_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, SESS.ReportSubmissionList)
    End Sub

    Protected Sub gvReportSubmission_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.ClearMessage()
        Me.gvReportSubmission.SelectedIndex = -1
        Me.GridViewSortingHandler(sender, e, SESS.ReportSubmissionList)
    End Sub

    Protected Sub gvReportSubmission_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ClearMessage()

        ' I-CRE16-003 Fix XSS [Start][Lawrence]
        Dim strFileID As String = AntiXssEncoder.HtmlEncode(DirectCast(gvReportSubmission.SelectedRow.FindControl("hfFileID"), HiddenField).Value, True)
        SetupReportCriteria(strFileID)
        ' I-CRE16-003 Fix XSS [End][Lawrence]
    End Sub

    Protected Sub gvReportSubmission_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        ' Enable Grid View Row Select
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Me.gvReportSubmission, "Select$" + e.Row.RowIndex.ToString(), False))
            e.Row.Style.Add("cursor", "hand")
        End If
    End Sub

    '

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.ClearMessage()
        Me.gvReportSubmission.SelectedIndex = -1
        Me.ClearReportCriteria()
        ' CRP11-008
        Me.MultiView1.SetActiveView(Me.View1)
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ClearMessage()

        Dim strReportID As String = hfReportID.Value
        Dim udtUserControlList As FileGenerationUserControlModelCollection = Session(SESS.ReportCriteriaUC)
        Dim strParameterString As String = ucReportCriteriaBase.GetParameterString(udtUserControlList)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        udtAuditLogEntry.AddDescripton("ReportID", strReportID)
        udtAuditLogEntry.AddDescripton("Parameters", strParameterString)
        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00004, "Submit report")

        Dim lstSysMsg As New List(Of SystemMessage)
        Dim lstSysMsgParam1 As New List(Of String)
        Dim lstSysMsgParam2 As New List(Of String)

        ' INT20-0055 (Fix concurrent browser submit in report submission) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Try
            ucReportCriteriaBase.ValidateCriteriaInput(strReportID, udtUserControlList, lstSysMsg, lstSysMsgParam1, lstSysMsgParam2)
        Catch ex As Exception
            Dim udtCommonAuditLogEntry As AuditLogEntry = New AuditLogEntry(Common.Component.FunctCode.FUNT029901)

            udtCommonAuditLogEntry.WriteLog(LogID.LOG00001, "Redirect to invalid access error page")

            Response.Redirect("~/ImproperAccess.aspx")
        End Try
        ' INT20-0055 (Fix concurrent browser submit in report submission) [End][Chris YIM]

        If lstSysMsg.Count > 0 Then
            ' Validation Fail
            For i As Integer = 0 To lstSysMsg.Count - 1
                If lstSysMsgParam1.Count - 1 >= i Then
                    If lstSysMsgParam2.Count - 1 >= i Then
                        Me.udcErrorMessage.AddMessage(lstSysMsg(i), New String() {"%s", "%t"}, New String() {lstSysMsgParam1(i).Trim, lstSysMsgParam2(i).Trim})
                    Else
                        Me.udcErrorMessage.AddMessage(lstSysMsg(i), New String() {"%s"}, New String() {lstSysMsgParam1(i).Trim})
                    End If

                Else
                    Me.udcErrorMessage.AddMessage(lstSysMsg(i))
                End If
            Next

            udtAuditLogEntry.AddDescripton("ReportID", strReportID)
            udtAuditLogEntry.AddDescripton("Parameters", ucReportCriteriaBase.GetParameterString(udtUserControlList))

            Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00006, "Submit report fail")

            Return

        End If

        ' Submit Report
        udtAuditLogEntry.AddDescripton("ReportID", strReportID)
        udtAuditLogEntry.AddDescripton("Parameters", strParameterString)

        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim blnSuccess As Boolean = SubmitReport( _
            strReportID, _
            ucReportCriteriaBase.GetCriteriaInput(udtUserControlList), _
            ucReportCriteriaBase.GetParameterList(udtUserControlList), _
            (New HCVUUserBLL).GetHCVUUser.UserID, _
            udtAuditLogEntry, _
            ucReportCriteriaBase.GetReportGenerationDate(udtUserControlList)
        )
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

        If blnSuccess Then
            InitGridViewReportSubmission()
            ' CRP11-008
            Me.MultiView1.SetActiveView(Me.ViewReturn)
        End If

    End Sub

    ''' <summary>
    ''' CRP11-008
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00007, "Return Click")
        ' CRE11-021 log the missed essential information [End]

        btnBack_Click(sender, e)
    End Sub
    '
    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Add [dtmScheduleGenDate]
    Private Function SubmitReport(ByVal strFileID As String, ByVal inputParam As StoreProcParamCollection, ByVal descParam As ParameterCollection, ByVal strUserID As String, ByVal udtAuditLogEntry As AuditLogEntry, ByVal dtmScheduleGenDate As DateTime?) As Boolean
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

        Dim blnSuccess As Boolean = False

        Dim udtDB As New Common.DataAccess.Database()
        Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
        Dim udtCommon As New Common.ComFunction.GeneralFunction()


        Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
        Dim udtFileGenerationQueueModel As New FileGeneration.FileGenerationQueueModel()
        Dim udtFileGenerationModel As FileGeneration.FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(udtDB, strFileID)

        ' CRE15-006 Rename of eHS [Start][Lawrence]
        Dim dicReplace As New Dictionary(Of String, String)

        For Each p As StoreProcParamObject In inputParam
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Select Case p.ParamDBType
                Case SqlDbType.Date, SqlDbType.DateTime
                    ' Format Date
                    dicReplace.Add(p.ParamName.Replace("@", String.Empty), CDate(p.ParamValue).ToString("yyyyMMdd"))

                Case Else
                    dicReplace.Add(p.ParamName.Replace("@", String.Empty), p.ParamValue)

            End Select            
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
        Next

        Dim strFileName As String = udtFileGenerationModel.OutputFileName(DateTime.Now, dicReplace)
        ' CRE15-006 Rename of eHS [End][Lawrence]

        Dim strDesc As String = ""
        For Each udtParam As ParameterObject In descParam
            If strDesc = "" Then
                strDesc = udtParam.ParamName + "(" + udtParam.ParamValue + ")"
            Else
                strDesc = strDesc + ", " + udtParam.ParamName + "(" + udtParam.ParamValue + ")"
            End If
        Next

        ' Add File Generation Queue
        udtFileGenerationQueueModel.GenerationID = udtCommon.generateFileSeqNo()
        udtFileGenerationQueueModel.FileID = udtFileGenerationModel.FileID
        udtFileGenerationQueueModel.InParm = udtParamFunction.GetSPParamString(inputParam)
        udtFileGenerationQueueModel.OutputFile = udtCommon.generateFileOutputPath(strFileName)
        udtFileGenerationQueueModel.Status = Common.Component.DataDownloadStatus.Pending
        udtFileGenerationQueueModel.FilePassword = ""
        udtFileGenerationQueueModel.RequestBy = strUserID

        Dim strFileDesc As String = udtFileGenerationModel.FileName + " - " + strFileName + Environment.NewLine
        If strDesc.Trim() <> "" Then
            strFileDesc = strFileDesc + " [" + strDesc + "]"
        End If
        udtFileGenerationQueueModel.FileDescription = strFileDesc

        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtFileGenerationQueueModel.ScheduleGenDtm = dtmScheduleGenDate
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

        Try
            udtDB.BeginTransaction()

            udtFileGenerationBLL.AddFileGenerationQueue(udtDB, udtFileGenerationQueueModel)
            Dim udtDataDownloadBLL As New DatadownloadBLL()
            udtDataDownloadBLL.InsertFileDownloadRecordsToUsersForFileGeneration(udtDB, udtFileGenerationQueueModel.GenerationID)

            udtDB.CommitTransaction()

            blnSuccess = True
        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Me.udcErrorMessage.AddMessage("990001", "D", eSQL.Message)
                Me.udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00006, "Submit report fail")

                Return False
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try

        If blnSuccess Then
            udtAuditLogEntry.AddDescripton("GenerationID", udtFileGenerationQueueModel.GenerationID)
            udtAuditLogEntry.AddDescripton("FileName", strFileName)
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00005, "Submit report successful")

            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            Me.udcInfoMessageBox.AddMessage("010701", "I", "00001", New String() {"%s"}, New String() {strFileName.Trim()})
            Me.udcInfoMessageBox.BuildMessageBox()
        End If
        Return blnSuccess
    End Function

    Private Sub ClearMessage()
        Me.udcErrorMessage.Clear()
        Me.udcInfoMessageBox.Clear()
    End Sub

#Region "Implement IWorkingData (CRE11-004)"

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
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
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

#End Region

End Class