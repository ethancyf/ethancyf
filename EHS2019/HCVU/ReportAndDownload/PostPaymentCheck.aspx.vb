Imports System.IO
Imports System.Web.Security.AntiXss
Imports Common.ComFunction
Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.BaseBLL
Imports Common.Component.FileGeneration
Imports Common.Component.HCVUUser
Imports Common.Component.Practice
Imports Common.Component.ServiceProvider
Imports Common.DataAccess
Imports Common.Validation
Imports CustomControls
Imports Microsoft.Office.Interop

Partial Public Class PostPaymentCheck
    Inherits BasePageWithGridView

#Region "Private Class"

    Private Class SESS
        Public Const PostPaymentCheckList As String = "010704_PostPaymentCheckList"
        Public Const PostPaymentCheckCriteriaUC As String = "010704_PostPaymentCheckCriteriaUC"
        Public Const PPC0002_SPList As String = "010704_PPC0002_SPList"
        Public Const PPC0003_SPImportSuppList As String = "010704_PPC0003_SPImportSuppList"
        Public Const PPC0003_SPSuppList As String = "010704_PPC0003_SPSuppList"

    End Class

    Private Class VS
        Public Const P3ImportPopupMode As String = "P3ImportPopupMode"
        Public Const P3ImportFileName As String = "P3ImportFileName"
    End Class

    Private Class PostPaymentCheckReport
        Public Const PPC0001 As String = "PPC0001"
        Public Const PPC0002 As String = "PPC0002"
        Public Const PPC0003 As String = "PPC0003"
    End Class

    Private Class ImportSPListErrorType
        Public Const EmptyCell As String = "E"
        Public Const Duplicated As String = "D"
        Public Const NotFound As String = "N"
    End Class

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        FunctionCode = FunctCode.FUNT010704

        ' Check session expire
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser()

        If Not IsPostBack Then
            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Post Payment Check loaded")

            InitControlOnce()

        End If

        InitControlAlways()

    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Select Case mvPostPaymentCheck.GetActiveView.ID
            Case vPPC0002.ID
                ' Extend the table width if any multi-selection is selected
                Dim blnAtLeastOneSelected As Boolean = False

                For Each udtParameter As ParameterObject In ucP2CriteriaBase.GetParameterString
                    If udtParameter.ParamName = "Health Profession" OrElse udtParameter.ParamName = "Scheme" OrElse udtParameter.ParamName = "Transaction Status" Then
                        If udtParameter.ParamValue <> "---" AndAlso udtParameter.ParamValue <> "Any" AndAlso udtParameter.ParamValue <> String.Empty Then
                            blnAtLeastOneSelected = True
                            Exit For
                        End If
                    End If
                Next

                If blnAtLeastOneSelected Then
                    tblGenerationCriteria.Style("width") = "1250px"
                    tblGenerationCriteriaContent.Style("width") = "800px"
                Else
                    tblGenerationCriteria.Style("width") = "1000px"
                    tblGenerationCriteriaContent.Style("width") = "530px"
                End If

                ' Check whether Month and Year is selected
                trP2InsufficientTransaction.Visible = False

                For Each udtParameter As ParameterObject In ucP2CriteriaBase.GetParameterList
                    If udtParameter.ParamName = "Format of Date" Then
                        If udtParameter.ParamValue = "Month and Year" Then
                            trP2InsufficientTransaction.Visible = True
                        End If
                        Exit For
                    End If
                Next

                ' Reinit the disabled textboxes
                If cboP2TargetTranHighestClaim.Checked Then
                    txtP2TargetTranHighestClaim.Attributes.Remove("disabled")
                Else
                    txtP2TargetTranHighestClaim.Attributes("disabled") = "disabled"
                End If

                If cboP2TargetTranManualInput.Checked Then
                    txtP2TargetTranManualInput.Attributes.Remove("disabled")
                Else
                    txtP2TargetTranManualInput.Attributes("disabled") = "disabled"
                End If

                If cboP2TargetTranSmartICInput.Checked Then
                    txtP2TargetTranSmartICInput.Attributes.Remove("disabled")
                Else
                    txtP2TargetTranSmartICInput.Attributes("disabled") = "disabled"
                End If

            Case vPPC0003.ID
                If Not IsNothing(ViewState(VS.P3ImportPopupMode)) Then
                    mpeP3Import.Show()
                End If

        End Select

    End Sub

    '

    Private Sub InitControlOnce()
        pnlReportInfo.Visible = False

        mvPostPaymentCheck.SetActiveView(vReportList)

        ' Load Report List
        Dim dt As DataTable = (New StatisticsBLL).GetPostPaymentCheckListByUserID("P")
        Session(SESS.PostPaymentCheckList) = dt

        If dt.Rows.Count = 0 Then
            udcInfoMessageBox.Type = InfoMessageBoxType.Information
            udcInfoMessageBox.AddMessage("990000", "I", "00001")
            udcInfoMessageBox.BuildMessageBox()

            gvPostPaymentCheck.Visible = False

        Else
            gvPostPaymentCheck.DataSource = dt
            gvPostPaymentCheck.DataBind()

            gvPostPaymentCheck.SelectedIndex = -1
            gvPostPaymentCheck.Visible = True

        End If

    End Sub

    Private Sub InitControlAlways()
        If Not IsNothing(Session(SESS.PostPaymentCheckCriteriaUC)) Then
            Dim udtStatisticsModel As StatisticsModel = DirectCast(Session(SESS.PostPaymentCheckCriteriaUC), StatisticsModel)

            Select Case udtStatisticsModel.StatisticID
                Case PostPaymentCheckReport.PPC0001
                    ucP1CriteriaBase.Build(udtStatisticsModel.CriteriaSetup)
                Case PostPaymentCheckReport.PPC0002
                    ucP2CriteriaBase.Build(udtStatisticsModel.CriteriaSetup)
                Case PostPaymentCheckReport.PPC0003
                    ucP3CriteriaBase.Build(udtStatisticsModel.CriteriaSetup)
                Case Else
                    Throw New Exception(String.Format("PostPaymentCheck.InitControlAlways: Unexpected value (udtStatisticsModel.StatisticID={0})", udtStatisticsModel.StatisticID))
            End Select

        End If

    End Sub

#End Region

    Protected Sub mvPostPaymentCheck_ActiveViewChanged(sender As Object, e As EventArgs)
        Select Case mvPostPaymentCheck.ActiveViewIndex
            Case mvPostPaymentCheck.Views.IndexOf(vReportList)
                pnlReportInfo.Visible = False
                gvPostPaymentCheck.SelectedIndex = -1
                CheckAvailableSlotToSubmitPPC()

            Case mvPostPaymentCheck.Views.IndexOf(vPPC0001),
                 mvPostPaymentCheck.Views.IndexOf(vPPC0002),
                 mvPostPaymentCheck.Views.IndexOf(vPPC0003)
                pnlReportInfo.Visible = True
                CheckAvailableSlotToSubmitPPC()

            Case mvPostPaymentCheck.Views.IndexOf(vReturn)
                pnlReportInfo.Visible = False

        End Select

    End Sub

    '

    Protected Sub gvPostPaymentCheck_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        udcInfoMessageBox.Visible = False
        CheckAvailableSlotToSubmitPPC()
        Me.gvPostPaymentCheck.SelectedIndex = -1
        Me.GridViewPageIndexChangingHandler(sender, e, SESS.PostPaymentCheckList)
    End Sub

    Protected Sub gvPostPaymentCheck_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, SESS.PostPaymentCheckList)
    End Sub

    Protected Sub gvPostPaymentCheck_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        udcInfoMessageBox.Visible = False
        CheckAvailableSlotToSubmitPPC()
        Me.gvPostPaymentCheck.SelectedIndex = -1
        Me.GridViewSortingHandler(sender, e, SESS.PostPaymentCheckList)
    End Sub

    Protected Sub gvPostPaymentCheck_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        udcInfoMessageBox.Visible = False
        Session(SESS.PostPaymentCheckCriteriaUC) = Nothing

        Dim strFileID As String = DirectCast(gvPostPaymentCheck.SelectedRow.FindControl("hfFileID"), HiddenField).Value

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReportID", strFileID)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00002, "Select report")

        Select Case strFileID
            Case PostPaymentCheckReport.PPC0001
                mvPostPaymentCheck.SetActiveView(Me.vPPC0001)

                InitPPC0001()

            Case PostPaymentCheckReport.PPC0002
                mvPostPaymentCheck.SetActiveView(Me.vPPC0002)

                InitPPC0002()

            Case PostPaymentCheckReport.PPC0003
                mvPostPaymentCheck.SetActiveView(Me.vPPC0003)

                InitPPC0003()

            Case Else
                Throw New NotImplementedException

        End Select

        udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Select report successful")

    End Sub

    Protected Sub gvPostPaymentCheck_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        ' Enable Grid View Row Select
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Me.gvPostPaymentCheck, "Select$" + e.Row.RowIndex.ToString(), False))
            e.Row.Style.Add("cursor", "hand")
        End If
    End Sub

    '

    Protected Sub ibtnReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00004, "Return Click")

        mvPostPaymentCheck.SetActiveView(vReportList)

    End Sub

    '

    Private Function SubmitReport(strFileID As String, strGenerationID As String, inputParam As StoreProcParamCollection, _
                                  Optional dtAdditionalParameter As DataTable = Nothing) As SystemMessage
        Dim udtDB As New Database
        Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
        Dim udtGeneralFunction As New GeneralFunction


        Dim udtFileGenerationBLL As New FileGenerationBLL
        Dim udtFileGenerationModel As FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(udtDB, strFileID)

        Dim dicReplace As New Dictionary(Of String, String)

        For Each p As StoreProcParamObject In inputParam
            dicReplace.Add(p.ParamName.Replace("@", String.Empty), p.ParamValue)
        Next

        Dim strFileName As String = udtFileGenerationModel.OutputFileName(DateTime.Now, dicReplace)

        Dim strDesc As String = String.Empty
        'Dim strParmName As String = String.Empty
        'Dim strParmDesc As String = String.Empty
        'Dim udtParm As ParameterObject

        'For i As Integer = 0 To descParam.Count - 1
        '    strParmName = String.Empty
        '    strParmDesc = String.Empty
        '    udtParm = Nothing

        '    If TypeOf descParam.Item(i) Is ParameterObjectList Then
        '        Dim udtParmList As ParameterObjectList = CType(descParam.Item(i), ParameterObjectList)

        '        For j As Integer = 1 To udtParmList.ParamValueList.Count
        '            strParmDesc = strParmDesc + udtParmList.ParamValueList.Item(j).ToString + ", "
        '        Next

        '        strParmName = udtParmList.ParamName
        '        strParmDesc = strParmDesc.Substring(0, strParmDesc.Length - 2)
        '    Else
        '        udtParm = CType(descParam.Item(i), ParameterObject)

        '        strParmName = udtParm.ParamName
        '    End If

        '    If strDesc = String.Empty Then
        '        If strParmDesc = String.Empty Then
        '            strDesc = strParmName + "(" + udtParm.ParamValue.ToString + ")"
        '        Else
        '            strDesc = strParmName + "(" + strParmDesc + ")"
        '        End If
        '    Else
        '        If strParmDesc = String.Empty Then
        '            strDesc = strDesc + ", " + strParmName + "(" + udtParm.ParamValue.ToString + ")"
        '        Else
        '            strDesc = strDesc + ", " + strParmName + "(" + strParmDesc + ")"
        '        End If
        '    End If
        'Next

        ' Add File Generation Queue
        Dim udtFileGenerationQueue As New FileGenerationQueueModel

        udtFileGenerationQueue.GenerationID = strGenerationID
        udtFileGenerationQueue.FileID = udtFileGenerationModel.FileID
        udtFileGenerationQueue.InParm = udtParamFunction.GetSPParamString(inputParam)
        udtFileGenerationQueue.OutputFile = udtGeneralFunction.generateFileOutputPath(strFileName)
        udtFileGenerationQueue.Status = DataDownloadStatus.Pending
        udtFileGenerationQueue.FilePassword = String.Empty
        udtFileGenerationQueue.RequestBy = (New HCVUUserBLL).GetHCVUUser.UserID

        Dim strFileDesc As String = udtFileGenerationModel.FileName + " - " + strFileName + Environment.NewLine
        If strDesc.Trim() <> "" Then
            strFileDesc = strFileDesc + " [" + strDesc + "]"
        End If
        udtFileGenerationQueue.FileDescription = strFileDesc

        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtFileGenerationQueue.ScheduleGenDtm = Nothing
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

        Try
            udtDB.BeginTransaction()

            ' Insert [FileGenerationQueue]
            udtFileGenerationBLL.AddFileGenerationQueue(udtDB, udtFileGenerationQueue)

            ' Insert [FileDownload]
            Call (New DatadownloadBLL).InsertFileDownloadRecordsToUsersForFileGeneration(udtDB, udtFileGenerationQueue.GenerationID)

            ' Insert [FileGenerationQueueAdditionalParameter] for the PPC0002/PPC0003 SP List
            If Not IsNothing(dtAdditionalParameter) Then
                Dim i As Integer = 1

                For Each dr As DataRow In dtAdditionalParameter.Rows
                    udtFileGenerationBLL.AddFileGenerationAdditionalParameter(udtDB, udtFileGenerationQueue.GenerationID, dr("Parm_Name"), i, dr("Parm_Value"))

                    i += 1

                Next

            End If

            udtDB.CommitTransaction()

        Catch eSQL As SqlClient.SqlException
            udtDB.RollBackTranscation()

            If eSQL.Number = 50000 Then
                Return New SystemMessage("990001", "D", eSQL.Message)
            Else
                Throw
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw

        End Try

        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002, New String() {"%s"}, New String() {strFileName.Trim()})
        udcInfoMessageBox.BuildMessageBox()

        Return Nothing

    End Function

#Region "PPC0001"

    Private Sub InitPPC0001()
        SetupReportCriteria(PostPaymentCheckReport.PPC0001, ucP1CriteriaBase)

    End Sub

    '

    Protected Sub ibtnP1Back_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReportID", hfReportID.Value)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00005, "Back click")

        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False

        mvPostPaymentCheck.SetActiveView(vReportList)

        udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Back click successful")

    End Sub

    Protected Sub ibtnP1Submit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False

        Dim strReportID As String = hfReportID.Value
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReportID", hfReportID.Value)

        For Each udtParameter As ParameterObject In ucP1CriteriaBase.GetParameterString
            If TypeOf udtParameter Is ParameterObjectList Then
                Dim lstValue As New List(Of String)

                For Each strValue As String In DirectCast(udtParameter, ParameterObjectList).ParamValueList
                    lstValue.Add(strValue)
                Next

                udtAuditLogEntry.AddDescripton(udtParameter.ParamName, String.Join(",", lstValue.ToArray))

            ElseIf TypeOf udtParameter Is ParameterObject Then
                udtAuditLogEntry.AddDescripton(udtParameter.ParamName, udtParameter.ParamValue)

            End If
        Next

        udtAuditLogEntry.WriteLog(LogID.LOG00007, "Submit click")

        ' Check if there is available slot for new PPC report
        If ContainAvailableSlotToSubmitPPCReport() = False Then
            udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
            udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00009, "Submit click fail")

            Return

        End If

        ' --- Validation ---

        Dim udtUserControlList As StatisticsModel = Session(SESS.PostPaymentCheckCriteriaUC)

        Dim lstSysMsg As New List(Of SystemMessage)
        Dim lstSysMsgParam1 As New List(Of String)
        Dim lstSysMsgParam2 As New List(Of String)

        ucP1CriteriaBase.ValidateCriteriaInput(strReportID, lstSysMsg, lstSysMsgParam1, lstSysMsgParam2)

        If lstSysMsg.Count > 0 Then
            ' Validation Fail
            For i As Integer = 0 To lstSysMsg.Count - 1
                If lstSysMsgParam1.Count - 1 >= i Then
                    If lstSysMsgParam2.Count - 1 >= i Then
                        udcErrorMessage.AddMessage(lstSysMsg(i), New String() {"%s", "%t"}, New String() {lstSysMsgParam1(i).Trim, lstSysMsgParam2(i).Trim})
                    Else
                        udcErrorMessage.AddMessage(lstSysMsg(i), New String() {"%s"}, New String() {lstSysMsgParam1(i).Trim})
                    End If

                Else
                    udcErrorMessage.AddMessage(lstSysMsg(i))
                End If
            Next

            udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00009, "Submit click fail")

            Return

        End If

        ' --- End of Validation ---

        Dim strGenerationID As String = (New GeneralFunction).generateFileSeqNo()

        Dim udtSystemMessage As SystemMessage = SubmitReport(strReportID, strGenerationID, ucP1CriteriaBase.GetCriteriaInput)

        If Not IsNothing(udtSystemMessage) Then
            udcErrorMessage.AddMessage(udtSystemMessage)
            udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00009, "Submit click fail")

            Return

        End If

        udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Submit click successful")

        mvPostPaymentCheck.SetActiveView(vReturn)

    End Sub

#End Region

#Region "PPC0002"

    Private Sub InitPPC0002()
        txtP2SPID.Text = String.Empty
        txtP2SPID.Enabled = True
        imgP2SPIDError.Visible = False
        ibtnP2SPIDSearch.Enabled = True
        ibtnP2SPIDSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
        ibtnP2SPIDClear.Enabled = False
        ibtnP2SPIDClear.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")
        ddlP2Practice.Items.Clear()
        ddlP2Practice.Enabled = False
        imgP2PracticeError.Visible = False
        ibtnP2SPAdd.Enabled = False
        ibtnP2SPAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSDisableBtn")

        Dim dt As DataTable = ConstructPPC0002SPPracticeListDataTable()

        Session(SESS.PPC0002_SPList) = dt
        gvP2SPList.DataSource = dt
        gvP2SPList.DataBind()

        SetupReportCriteria(PostPaymentCheckReport.PPC0002, ucP2CriteriaBase)

        Dim udtGeneralFunction As New GeneralFunction
        Dim strParmValue1 As String = String.Empty
        Dim strParmValue2 As String = String.Empty

        ' Targeted Number of Transaction
        udtGeneralFunction.getSystemParameter("PPC0002_TargetNumberOfTransactionRange", strParmValue1, strParmValue2)
        lblP2TargetTranRemark.Text = String.Format("( {0} - {1} )", strParmValue1, strParmValue2)
        txtP2TargetTran.MaxLength = strParmValue2.Length
        txtP2TargetTran.Text = String.Empty
        imgP2TargetTranError.Visible = False

        ' Number of Transactions with the Highest Amount Claimed
        udtGeneralFunction.getSystemParameter("PPC0002_NumberOfTransactionWithHighestClaimRange", strParmValue1, strParmValue2)
        lblP2TargetTranHighestClaimRemark.Text = String.Format("( {0} - {1} )", strParmValue1, strParmValue2)
        cboP2TargetTranHighestClaim.Checked = False
        cboP2TargetTranHighestClaim.Attributes("onclick") = String.Format("javascript: CheckboxTextboxRelation(this, '{0}');", txtP2TargetTranHighestClaim.ClientID)
        txtP2TargetTranHighestClaim.MaxLength = strParmValue2.Length
        txtP2TargetTranHighestClaim.Text = String.Empty
        imgP2TargetTranHighestClaimError.Visible = False

        ' Number of Manual Input
        udtGeneralFunction.getSystemParameter("PPC0002_NumberOfManualInputRange", strParmValue1, strParmValue2)
        lblP2TargetTranManualInputRemark.Text = String.Format("( {0} - {1} )", strParmValue1, strParmValue2)
        cboP2TargetTranManualInput.Checked = False
        cboP2TargetTranManualInput.Attributes("onclick") = String.Format("javascript: CheckboxTextboxRelation(this, '{0}');", txtP2TargetTranManualInput.ClientID)
        txtP2TargetTranManualInput.MaxLength = strParmValue2.Length
        txtP2TargetTranManualInput.Text = String.Empty
        imgP2TargetTranManualInputError.Visible = False

        ' Number of Smart IC Input
        udtGeneralFunction.getSystemParameter("PPC0002_NumberOfSmartICInputRange", strParmValue1, strParmValue2)
        lblP2TargetTranSmartICInputRemark.Text = String.Format("( {0} - {1} )", strParmValue1, strParmValue2)
        cboP2TargetTranSmartICInput.Checked = False
        cboP2TargetTranSmartICInput.Attributes("onclick") = String.Format("javascript: CheckboxTextboxRelation(this, '{0}');", txtP2TargetTranSmartICInput.ClientID)
        txtP2TargetTranSmartICInput.MaxLength = strParmValue2.Length
        txtP2TargetTranSmartICInput.Text = String.Empty
        imgP2TargetTranSmartICInputError.Visible = False

        ' Insufficient transaction
        cboP2InsuffTran.Checked = False

    End Sub

    Protected Sub ibtnP2SPIDSearch_Click(sender As Object, e As ImageClickEventArgs)
        ' Init
        udcInfoMessageBox.Visible = False
        udcErrorMessage.Visible = False
        imgP2SPIDError.Visible = False
        ucP2CriteriaBase.SetErrorComponentVisibility(False)
        imgP2TargetTranError.Visible = False
        imgP2TargetTranHighestClaimError.Visible = False
        imgP2TargetTranManualInputError.Visible = False
        imgP2TargetTranSmartICInputError.Visible = False

        ddlP2Practice.Enabled = False
        ibtnP2SPAdd.Enabled = False
        ibtnP2SPAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSDisableBtn")

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ServiceProviderID", txtP2SPID.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00015, "Selection of Service Provider - Search click")

        Dim strSPID As String = txtP2SPID.Text.Trim

        ' --- Validation ---

        Dim blnError As Boolean = False

        ' Empty
        If strSPID = String.Empty Then
            udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
            imgP2SPIDError.Visible = True
            blnError = True
        End If

        ' Valid format
        If blnError = False Then
            Dim udtSystemMessage As SystemMessage = (New Validator).chkSPID(strSPID)

            If Not IsNothing(udtSystemMessage) Then
                udcErrorMessage.AddMessage(udtSystemMessage)
                imgP2SPIDError.Visible = True
                blnError = True
            End If

        End If

        ' Exist
        If blnError = False Then
            ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
            'Dim udtSearchResult As BLLSearchResult = (New ServiceProviderBLL).GetServiceProviderMaintenanceSearch(Me.FunctionCode, _
            '                                            String.Empty, strSPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, _
            '                                            New Database, False, False)
            Dim udtSearchResult As BLLSearchResult = (New ServiceProviderBLL).GetServiceProviderMaintenanceSearch(Me.FunctionCode, _
                                                        String.Empty, strSPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, _
                                                        New Database, False, False)
            ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

            Dim dt As DataTable = udtSearchResult.Data

            If dt.Rows.Count = 0 Then
                udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011)
                imgP2SPIDError.Visible = True
                blnError = True

            End If

        End If

        ' Check already max 10 service providers and you are not in the list
        If blnError = False Then
            Dim dtSPList As DataTable = Session(SESS.PPC0002_SPList)
            Dim dtDistinctSPList As DataTable = dtSPList.DefaultView.ToTable(True, "SPID")
            Dim intMaxNumberOfSP As Integer = CInt((New GeneralFunction).getSystemParameterValue1("PPC0002_SelectionOfSPMaxNumber"))

            If dtDistinctSPList.Rows.Count = intMaxNumberOfSP AndAlso dtDistinctSPList.Select(String.Format("SPID = '{0}'", strSPID)).Length = 0 Then
                udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017, _
                                           New String() {"{PPC0002_SelectionOfSPMaxNumber}"}, New String() {intMaxNumberOfSP.ToString})
                imgP2SPIDError.Visible = True
                blnError = True

            End If

        End If

        ' Any error?
        If udcErrorMessage.GetCodeTable.Rows.Count > 0 Then
            udcErrorMessage.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00017, "Selection of Service Provider - Search click fail")

            Return
        End If

        ' --- End of Validation ---

        Dim dtPractice As DataTable = (New PracticeBLL).getRawAllPracticeBankAcct(strSPID)

        ' Save the practice count before filter
        hfP2PracticeCount.Value = dtPractice.Rows.Count

        ' Filter the added practice
        Dim drs As DataRow() = Session(SESS.PPC0002_SPList).Select(String.Format("SPID = '{0}'", strSPID))

        For Each drPractice As DataRow In dtPractice.Rows
            For Each dr As DataRow In drs
                If drPractice("PracticeID") = dr("PracticeID") Then
                    drPractice.Delete()
                    Exit For
                End If
            Next
        Next

        dtPractice.AcceptChanges()

        ' Break if no practice can be added
        If dtPractice.Rows.Count = 0 Then
            udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016)
            udcErrorMessage.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00017, "Selection of Service Provider - Search click fail")
            imgP2SPIDError.Visible = True

            Return

        End If

        ddlP2Practice.Items.Clear()

        ddlP2Practice.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

        If dtPractice.Rows.Count > 1 Then
            ddlP2Practice.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "All"), "ALL"))
        End If

        For Each dr As DataRow In dtPractice.Select(String.Empty, "PracticeID")
            ' Place the PracticeStatus in the value also for later use
            ' Text = PracticeName(PracticeID); Value = PracticeID,PracticeStatus
            ddlP2Practice.Items.Add(New ListItem(String.Format("{0}({1})", dr("Practice_Name"), dr("PracticeID")), _
                                                 String.Format("{0},{1}", dr("PracticeID"), dr("Practice_Status"))))
        Next

        txtP2SPID.Enabled = False
        ibtnP2SPIDSearch.Enabled = False
        ibtnP2SPIDSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchDisableSBtn")
        ibtnP2SPIDClear.Enabled = True
        ibtnP2SPIDClear.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearSBtn")
        ddlP2Practice.Enabled = True
        ibtnP2SPAdd.Enabled = True
        ibtnP2SPAdd.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "AddSBtn")

        udtAuditLogEntry.AddDescripton("PracticeAvailableToAdd", dtPractice.Rows.Count)
        udtAuditLogEntry.WriteEndLog(LogID.LOG00016, "Selection of Service Provider - Search click successful")

    End Sub

    Protected Sub ibtnP2SPIDClear_Click(sender As Object, e As ImageClickEventArgs)
        udcInfoMessageBox.Visible = False
        udcErrorMessage.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00018, "Selection of Service Provider - Clear click")

        txtP2SPID.Text = String.Empty
        txtP2SPID.Enabled = True
        ibtnP2SPIDSearch.Enabled = True
        ibtnP2SPIDSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
        ibtnP2SPIDClear.Enabled = False
        ibtnP2SPIDClear.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")
        ddlP2Practice.Items.Clear()
        ddlP2Practice.Enabled = False
        imgP2PracticeError.Visible = False
        ibtnP2SPAdd.Enabled = False
        ibtnP2SPAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSDisableBtn")

        txtP2SPID.Focus()

        udtAuditLogEntry.WriteEndLog(LogID.LOG00019, "Selection of Service Provider - Clear click successful")

    End Sub

    '

    Protected Sub ibtnP2SPAdd_Click(sender As Object, e As ImageClickEventArgs)
        ' Init
        udcErrorMessage.Visible = False
        imgP2PracticeError.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ServiceProviderID", txtP2SPID.Text)
        udtAuditLogEntry.AddDescripton("Practice", ddlP2Practice.SelectedItem.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00020, "Selection of Service Provider - Add click")

        ' --- Validation ---

        ' Empty
        If ddlP2Practice.SelectedIndex = 0 Then
            udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
            imgP2PracticeError.Visible = True
        End If

        If udcErrorMessage.GetCodeTable.Rows.Count > 0 Then
            udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00022, "Selection of Service Provider - Add click fail")

            Return
        End If

        ' --- End of Validation ---

        Dim dt As DataTable = Session(SESS.PPC0002_SPList)

        If ddlP2Practice.SelectedValue = "ALL" Then
            ' Add all items
            For i As Integer = 2 To ddlP2Practice.Items.Count - 1
                Dim dr As DataRow = dt.NewRow
                dr("SPID") = txtP2SPID.Text
                dr("PracticeID") = ddlP2Practice.Items(i).Value.Split(",")(0)
                dr("PracticeName") = ddlP2Practice.Items(i).Text
                dr("PracticeStatus") = ddlP2Practice.Items(i).Value.Split(",")(1)

                dt.Rows.Add(dr)

            Next

        Else
            ' Add single item
            Dim dr As DataRow = dt.NewRow
            dr("SPID") = txtP2SPID.Text
            dr("PracticeID") = ddlP2Practice.SelectedItem.Value.Split(",")(0)
            dr("PracticeName") = ddlP2Practice.SelectedItem.Text
            dr("PracticeStatus") = ddlP2Practice.SelectedItem.Value.Split(",")(1)

            dt.Rows.Add(dr)

        End If

        SortSPList()
        ShowSPList()

        ' Check whether all practice have been added
        If dt.Select(String.Format("SPID = '{0}'", AntiXssEncoder.HtmlEncode(txtP2SPID.Text, True))).Length = hfP2PracticeCount.Value Then
            ' All practices have been added
            ibtnP2SPIDClear_Click(Nothing, Nothing)

        Else
            ' Remove the added one
            ibtnP2SPIDSearch_Click(Nothing, Nothing)

        End If

        udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Selection of Service Provider - Add click successful")

    End Sub

    Private Sub SortSPList()
        Dim dt As DataTable = Session(SESS.PPC0002_SPList)
        Dim i As Integer = 0
        Dim strPreviousSPID As String = String.Empty

        For Each dr As DataRow In dt.Select(String.Empty, "SPID")
            If dr("SPID") <> strPreviousSPID Then
                i += 1
                strPreviousSPID = dr("SPID")
            End If

            dr("Index") = i

        Next

        Dim dv As DataView = dt.DefaultView
        dv.Sort = "SPID, PracticeID"
        dt = dv.ToTable

    End Sub

    Private Sub ShowSPList()
        Dim dt As DataTable = Session(SESS.PPC0002_SPList)

        gvP2SPList.DataSource = dt
        gvP2SPList.DataBind()

        ' Merge cell
        Dim strPreviousSPID As String = String.Empty
        Dim intPreviousRowIndex As Integer = -1

        For Each gvr As GridViewRow In gvP2SPList.Rows
            Dim strSPID As String = DirectCast(gvr.FindControl("lblGSPID"), Label).Text

            If strSPID = strPreviousSPID Then
                If gvP2SPList.Rows(intPreviousRowIndex).Cells(0).RowSpan <= 1 Then
                    gvP2SPList.Rows(intPreviousRowIndex).Cells(0).RowSpan = 2
                    gvP2SPList.Rows(intPreviousRowIndex).Cells(1).RowSpan = 2
                Else
                    gvP2SPList.Rows(intPreviousRowIndex).Cells(0).RowSpan += 1
                    gvP2SPList.Rows(intPreviousRowIndex).Cells(1).RowSpan += 1
                End If

                gvr.Cells(0).Visible = False
                gvr.Cells(1).Visible = False

            Else
                strPreviousSPID = strSPID
                intPreviousRowIndex = gvr.RowIndex

            End If

        Next

    End Sub

    '

    Protected Sub gvP2SPList_RowDataBound(sender As System.Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Status
            Dim lblGPracticeStatus As Label = e.Row.FindControl("lblGPracticeStatus")
            Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, lblGPracticeStatus.Text, lblGPracticeStatus.Text, String.Empty)

        End If
    End Sub

    Protected Sub gvP2SPList_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim gvr As GridViewRow = gvP2SPList.Rows(e.RowIndex)

        Dim strSPID As String = DirectCast(gvr.FindControl("lblGSPID"), Label).Text
        Dim strPracticeID As String = DirectCast(gvr.FindControl("hfGPracticeID"), HiddenField).Value

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("SPID", strSPID)
        udtAuditLogEntry.AddDescripton("PracticeID", strPracticeID)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00023, "Selection of Service Provider - Delete click")

        Dim dtSPList As DataTable = Session(SESS.PPC0002_SPList)

        For Each dr As DataRow In dtSPList.Select(String.Format("SPID = '{0}'", strSPID))
            If dr("PracticeID") = strPracticeID Then
                dr.Delete()
                Exit For
            End If
        Next

        dtSPList.AcceptChanges()

        SortSPList()
        ShowSPList()

        ' Update the drop down list if the same SP is locked in
        If txtP2SPID.Text.Trim = strSPID AndAlso ddlP2Practice.Items.Count <> 0 Then
            ibtnP2SPIDSearch_Click(Nothing, Nothing)
        End If

        udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "Selection of Service Provider - Delete click successful")

    End Sub

    '

    Protected Sub ibtnP2Back_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReportID", hfReportID.Value)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Back click")

        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False

        mvPostPaymentCheck.SetActiveView(vReportList)

        udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Back click successful")

    End Sub

    Protected Sub ibtnP2Submit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcErrorMessage.Clear()
        udcInfoMessageBox.Visible = False
        imgP2SPIDError.Visible = False
        imgP2TargetTranError.Visible = False
        imgP2TargetTranHighestClaimError.Visible = False
        imgP2TargetTranManualInputError.Visible = False
        imgP2TargetTranSmartICInputError.Visible = False

        Dim strReportID As String = hfReportID.Value
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReportID", hfReportID.Value)

        For Each udtParameter As ParameterObject In ucP2CriteriaBase.GetParameterString
            If TypeOf udtParameter Is ParameterObjectList Then
                Dim lstValue As New List(Of String)

                For Each strValue As String In DirectCast(udtParameter, ParameterObjectList).ParamValueList
                    lstValue.Add(strValue)
                Next

                udtAuditLogEntry.AddDescripton(udtParameter.ParamName, String.Join(",", lstValue.ToArray))

            ElseIf TypeOf udtParameter Is ParameterObject Then
                udtAuditLogEntry.AddDescripton(udtParameter.ParamName, udtParameter.ParamValue)

            End If
        Next

        udtAuditLogEntry.WriteStartLog(LogID.LOG00012, "Submit click")

        ' Check if there is available slot for new PPC report
        If ContainAvailableSlotToSubmitPPCReport() = False Then
            udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
            udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00014, "Submit click fail")

            Return
        End If

        ' --- Validation ---

        ' Service Provider List
        If DirectCast(Session(SESS.PPC0002_SPList), DataTable).Rows.Count = 0 Then
            udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004)
            imgP2SPIDError.Visible = True
        End If

        ' User Controls
        Dim lstSysMsg As New List(Of SystemMessage)
        Dim lstSysMsgParam1 As New List(Of String)
        Dim lstSysMsgParam2 As New List(Of String)

        ucP2CriteriaBase.ValidateCriteriaInput(strReportID, lstSysMsg, lstSysMsgParam1, lstSysMsgParam2)

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

        End If

        ' Targeted Number of Transaction
        Dim lstSystemMessage As New List(Of SystemMessage)
        Dim lstOldChar As New List(Of String)
        Dim lstNewChar As New List(Of String)
        Dim blnAllNumberValid As Boolean = True

        ValidatePPC0002TargetNumberOfTransaction(txtP2TargetTran.Text.Trim,
                                                 Me.GetGlobalResourceObject("Text", "TargetedNumberOfTransaction"), _
                                                 "PPC0002_TargetNumberOfTransactionRange", _
                                                 lstSystemMessage, lstOldChar, lstNewChar)

        If lstSystemMessage.Count > 0 Then
            For i As Integer = 0 To lstSystemMessage.Count - 1
                udcErrorMessage.AddMessage(lstSystemMessage(i), New String() {lstOldChar(i)}, New String() {lstNewChar(i)})
            Next

            imgP2TargetTranError.Visible = True
            blnAllNumberValid = False

        End If

        ' Number of Transactions with the Highest Amount Claimed
        If cboP2TargetTranHighestClaim.Checked Then
            lstSystemMessage.Clear()
            lstOldChar.Clear()
            lstNewChar.Clear()

            ValidatePPC0002TargetNumberOfTransaction(txtP2TargetTranHighestClaim.Text.Trim,
                                                     Me.GetGlobalResourceObject("Text", "NumberOfTransactionWithTheHighestAmountClaim"), _
                                                     "PPC0002_NumberOfTransactionWithHighestClaimRange", _
                                                     lstSystemMessage, lstOldChar, lstNewChar)

            If lstSystemMessage.Count > 0 Then
                For i As Integer = 0 To lstSystemMessage.Count - 1
                    udcErrorMessage.AddMessage(lstSystemMessage(i), New String() {lstOldChar(i)}, New String() {lstNewChar(i)})
                Next

                imgP2TargetTranHighestClaimError.Visible = True
                blnAllNumberValid = False

            End If

        End If

        ' Number of Manual Input 
        If cboP2TargetTranManualInput.Checked Then
            lstSystemMessage.Clear()
            lstOldChar.Clear()
            lstNewChar.Clear()

            ValidatePPC0002TargetNumberOfTransaction(txtP2TargetTranManualInput.Text.Trim,
                                                     Me.GetGlobalResourceObject("Text", "NumberOfManualInput"), _
                                                     "PPC0002_NumberOfManualInputRange", _
                                                     lstSystemMessage, lstOldChar, lstNewChar)

            If lstSystemMessage.Count > 0 Then
                For i As Integer = 0 To lstSystemMessage.Count - 1
                    udcErrorMessage.AddMessage(lstSystemMessage(i), New String() {lstOldChar(i)}, New String() {lstNewChar(i)})
                Next

                imgP2TargetTranManualInputError.Visible = True
                blnAllNumberValid = False

            End If

        End If

        ' Number of Smart IC Input 
        If cboP2TargetTranSmartICInput.Checked Then
            lstSystemMessage.Clear()
            lstOldChar.Clear()
            lstNewChar.Clear()

            ValidatePPC0002TargetNumberOfTransaction(txtP2TargetTranSmartICInput.Text.Trim,
                                                     Me.GetGlobalResourceObject("Text", "NumberOfSmartICInput"), _
                                                     "PPC0002_NumberOfSmartICInputRange", _
                                                     lstSystemMessage, lstOldChar, lstNewChar)

            If lstSystemMessage.Count > 0 Then
                For i As Integer = 0 To lstSystemMessage.Count - 1
                    udcErrorMessage.AddMessage(lstSystemMessage(i), New String() {lstOldChar(i)}, New String() {lstNewChar(i)})
                Next

                imgP2TargetTranSmartICInputError.Visible = True
                blnAllNumberValid = False

            End If

        End If

        ' Sum of the three should not be greater than parent
        If blnAllNumberValid Then
            Dim intSum As Integer = 0

            If cboP2TargetTranHighestClaim.Checked Then intSum += CInt(txtP2TargetTranHighestClaim.Text)
            If cboP2TargetTranManualInput.Checked Then intSum += CInt(txtP2TargetTranManualInput.Text)
            If cboP2TargetTranSmartICInput.Checked Then intSum += CInt(txtP2TargetTranSmartICInput.Text)

            If intSum > CInt(txtP2TargetTran.Text) Then
                udcErrorMessage.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015))

                imgP2TargetTranError.Visible = True
                If cboP2TargetTranHighestClaim.Checked Then imgP2TargetTranHighestClaimError.Visible = True
                If cboP2TargetTranManualInput.Checked Then imgP2TargetTranManualInputError.Visible = True
                If cboP2TargetTranSmartICInput.Checked Then imgP2TargetTranSmartICInputError.Visible = True

            End If

        End If

        ' Final
        If udcErrorMessage.GetCodeTable.Rows.Count > 0 Then
            udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00014, "Submit click fail")

            Return

        End If

        ' --- End of Validation ---

        ' Add additional parameters not in user controls
        Dim strGenerationID As String = (New GeneralFunction).generateFileSeqNo()
        Dim udtSProcParmList As StoreProcParamCollection = ucP2CriteriaBase.GetCriteriaInput

        ' SP List
        Dim dicSPList As New Dictionary(Of String, List(Of String))

        For Each dr As DataRow In DirectCast(Session(SESS.PPC0002_SPList), DataTable).Rows
            If dicSPList.ContainsKey(dr("SPID")) = False Then
                dicSPList.Add(dr("SPID"), New List(Of String))
            End If

            dicSPList(dr("SPID")).Add(dr("PracticeID"))

        Next

        ' Additional Parameter
        Dim dtAP As DataTable = ConstructAdditionalParameterDataTable()
        Dim drAP As DataRow = Nothing

        For Each kvp As KeyValuePair(Of String, List(Of String)) In dicSPList
            drAP = dtAP.NewRow
            drAP("Parm_Name") = "SPPracticeList"
            drAP("Parm_Value") = String.Format("{0}:{1}", kvp.Key, String.Join(",", kvp.Value.ToArray))

            dtAP.Rows.Add(drAP)

        Next

        udtSProcParmList.AddParam("@SPList", SqlDbType.VarChar, 80, String.Format("AdditionalParameter:{0}:SPPracticeList", strGenerationID))

        ' Target Number of Transaction
        udtSProcParmList.AddParam("@TargetNoOfTransaction", SqlDbType.Int, 8, txtP2TargetTran.Text)
        udtSProcParmList.AddParam("@NoOfHighestAmountClaimed", SqlDbType.Int, 8, IIf(txtP2TargetTranHighestClaim.Text = String.Empty, -1, txtP2TargetTranHighestClaim.Text))
        udtSProcParmList.AddParam("@NoOfManualInput", SqlDbType.Int, 8, IIf(txtP2TargetTranManualInput.Text = String.Empty, -1, txtP2TargetTranManualInput.Text))
        udtSProcParmList.AddParam("@NoOfSmartICInput", SqlDbType.Int, 8, IIf(txtP2TargetTranSmartICInput.Text = String.Empty, -1, txtP2TargetTranSmartICInput.Text))

        Dim blnIncludePreviousMonth As Boolean = False

        If trP2InsufficientTransaction.Visible AndAlso cboP2InsuffTran.Checked Then
            blnIncludePreviousMonth = True
        End If

        udtSProcParmList.AddParam("@IncludePreviousMonth", SqlDbType.Bit, 1, blnIncludePreviousMonth)

        ' Submit Report
        Dim udtSystemMessage As SystemMessage = SubmitReport(strReportID, strGenerationID, udtSProcParmList, dtAP)
        
        If Not IsNothing(udtSystemMessage) Then
            udcErrorMessage.AddMessage(udtSystemMessage)
            udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00014, "Submit click fail")

            Return

        End If

        udtAuditLogEntry.WriteEndLog(LogID.LOG00013, "Submit click successful")

        mvPostPaymentCheck.SetActiveView(vReturn)

    End Sub

#End Region

#Region "PPC0003"

    Private Sub InitPPC0003()
        Session.Remove(SESS.PPC0003_SPImportSuppList)
        Session(SESS.PPC0003_SPSuppList) = ConstructPPC0003ImportSPListDataTable()

        imgP3PercentageOfSPRangeError.Visible = False

        lblP3NoOfRecordImport.Text = Me.GetGlobalResourceObject("Text", "NImported").ToString _
                                       .Replace("{NoOfRecord}", DirectCast(Session(SESS.PPC0003_SPSuppList), DataTable).Rows.Count)
        ibtnP3Import.Visible = True
        ibtnP3View.Visible = False
        ibtnP3Clear.Visible = False

        Dim strLowerLimit As String = String.Empty
        Dim strUpperLimit As String = String.Empty
        Call (New GeneralFunction).getSystemParameter("PPC0003_PercentageOfSPRange", strLowerLimit, strUpperLimit)

        txtP3PercentageOfSP.MaxLength = strUpperLimit.Length
        txtP3PercentageOfSP.Width = 12 * strUpperLimit.Length
        txtP3PercentageOfSP.Text = String.Empty

        lblP3PercentageOfSPRange.Text = Me.GetGlobalResourceObject("Text", "PPC0003PercentageOfSPUnit").ToString _
                                          .Replace("{LowerLimit}", strLowerLimit).Replace("{UpperLimit}", strUpperLimit)

        SetupReportCriteria(PostPaymentCheckReport.PPC0003, ucP3CriteriaBase)

    End Sub

    Protected Sub ibtnP3Import_Click(sender As Object, e As ImageClickEventArgs)
        udcP3ImportMessageBox.Visible = False
        udcP3ImportInfoMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00030, "Supplementary List Import click")

        mvP3Import.SetActiveView(vP3ImportUploadFile)
        imgP3IUFile.Visible = False

        ibtnP3IVImportFile.Visible = True
        ibtnP3IVConfirm.Visible = False
        ibtnP3IVCancel.Visible = True
        ibtnP3IVClose.Visible = False

        ViewState(VS.P3ImportPopupMode) = "A"

    End Sub

    Protected Sub ibtnP3View_Click(sender As Object, e As ImageClickEventArgs)
        udcP3ImportMessageBox.Visible = False
        udcP3ImportInfoMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00031, "Supplementary List View click")

        trP3IVFile.Visible = False

        Dim dt As DataTable = Session(SESS.PPC0003_SPSuppList)

        lblP3IVNoOfRecordText.Text = Me.GetGlobalResourceObject("Text", "NoOfRecords")
        lblP3IVNoOfRecord.Text = dt.Rows.Count
        lblP3IVNoOfRecord.ForeColor = Nothing

        gvP3IVList.DataSource = dt
        gvP3IVList.DataBind()
        gvP3IVList.Columns(3).Visible = False

        ibtnP3IVImportFile.Visible = False
        ibtnP3IVConfirm.Visible = False
        ibtnP3IVCancel.Visible = False
        ibtnP3IVClose.Visible = True

        mvP3Import.SetActiveView(vP3ImportViewRecord)

        ViewState(VS.P3ImportPopupMode) = "A"

    End Sub

    Protected Sub ibtnP3Clear_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00032, "Supplementary List Clear click")

        Session(SESS.PPC0003_SPSuppList) = ConstructPPC0002SPPracticeListDataTable()

        lblP3NoOfRecordImport.Text = Me.GetGlobalResourceObject("Text", "NImported").ToString _
                                       .Replace("{NoOfRecord}", DirectCast(Session(SESS.PPC0003_SPSuppList), DataTable).Rows.Count)

        ibtnP3Import.Visible = True
        ibtnP3View.Visible = False
        ibtnP3Clear.Visible = False

    End Sub

    '

    Private Function ReadExcel(ByVal xlsWorkBook As Excel.Workbook, ByRef intLastExcelRow As Integer) As DataTable
        Dim xlsWorkSheet As Excel.Worksheet = xlsWorkBook.Worksheets(1)
        Dim intFailRecord As Integer = 0

        ' Initialize the result datatable
        Dim dt As DataTable = ConstructPPC0003ImportSPListDataTable()
        Dim dr As DataRow = Nothing

        ' Read rows starting from row 1
        Dim range As Excel.Range = xlsWorkSheet.UsedRange
        Dim cell As Excel.Range = Nothing
        Dim intLastReadRow As Integer = -1
        intLastExcelRow = -1

        ' Find the last used row in column 1
        For r As Integer = 1 To range.Rows.Count
            cell = range.Cells(r, 1)

            If Not IsNothing(cell.Value) AndAlso cell.Value.ToString.Trim <> String.Empty Then
                intLastReadRow = r
                If cell.Row > intLastExcelRow Then intLastExcelRow = cell.Row
            End If
        Next

        ' Break if number of rows over limit (the threshold value should add 1 (500->501) to include the header)
        If intLastExcelRow > CInt((New GeneralFunction).getSystemParameterValue1("PPC0003_ImportSPSuppListMaxRecord")) + 1 Then
            Return dt
        End If

        Dim blnContainHeader As Boolean = False

        For r As Integer = 1 To intLastReadRow
            cell = range.Cells(r, 1)

            ' Skip the row if it is not SPID (only for cell A1)
            If cell.Row = 1 AndAlso Not IsNothing(cell.Value) AndAlso cell.Value.ToString.Trim <> String.Empty _
                    AndAlso (New Regex("^\d+$")).IsMatch(cell.Value.ToString.Trim) = False Then
                blnContainHeader = True
                Continue For
            End If

            ' Add the row to datatable
            dr = dt.NewRow

            dr("Row_No") = cell.Row

            If IsNothing(cell.Value) OrElse cell.Value.ToString.Trim = String.Empty Then
                dr("SPID") = String.Empty
                dr("SP_Name") = String.Empty
                dr("Fail_Type") = ImportSPListErrorType.EmptyCell

            Else
                dr("SPID") = cell.Value.ToString.Trim
                dr("SP_Name") = String.Empty
                dr("Fail_Type") = String.Empty

            End If

            dt.Rows.Add(dr)

        Next

        ' --- Validation ---

        ' Duplicate records
        Dim lstDuplicateSPID As New List(Of String)

        For Each dr In dt.DefaultView.ToTable(True, "SPID").Rows
            If dr("SPID") = String.Empty Then Continue For

            If dt.Compute("COUNT(SPID)", String.Format("SPID = '{0}'", dr("SPID"))) > 1 Then
                lstDuplicateSPID.Add(dr("SPID"))
            End If
        Next

        ' Find SP Name
        Dim udtServiceProviderBLL As New ServiceProviderBLL
        Dim udtDB As New Database
        Dim dtSearchResult As DataTable = Nothing
        Dim drSearchResult As DataRow = Nothing

        Dim i As Integer = 1
        Dim regSPID As New Regex("^\d{8}$")

        For Each dr In dt.Rows
            If dr("SPID") = String.Empty Then Continue For

            If regSPID.IsMatch(dr("SPID")) = False Then
                dr("Fail_Type") = ImportSPListErrorType.NotFound

            Else
                dtSearchResult = udtServiceProviderBLL.GetServiceProviderBySPID(dr("SPID"), udtDB)

                If dtSearchResult.Rows.Count = 0 Then
                    dr("Fail_Type") = ImportSPListErrorType.NotFound

                Else
                    drSearchResult = dtSearchResult.Rows(0)

                    If drSearchResult("SP_Chi_Name").ToString <> String.Empty Then
                        dr("SP_Name") = String.Format("{0} ({1})", drSearchResult("SP_Eng_Name"), drSearchResult("SP_Chi_Name"))
                    Else
                        dr("SP_Name") = drSearchResult("SP_Eng_Name")
                    End If

                End If

            End If

            ' Duplicated will override NotFound
            If lstDuplicateSPID.Contains(dr("SPID")) Then
                dr("Fail_Type") = ImportSPListErrorType.Duplicated
            End If

        Next

        ' Supplement the starting rows if they are empty
        If dt.Rows.Count > 0 Then
            Dim intFirstExcelRow As Integer = CInt(dt.Compute("MIN(Row_No)", String.Empty))

            For j As Integer = 1 To intFirstExcelRow - 1
                If j = 1 AndAlso blnContainHeader Then Continue For

                ' Add the row to datatable
                dr = dt.NewRow

                dr("Row_No") = j
                dr("SPID") = String.Empty
                dr("SP_Name") = String.Empty
                dr("Fail_Type") = ImportSPListErrorType.EmptyCell

                dt.Rows.Add(dr)

            Next

        End If

        ' Return
        Return dt

    End Function

    '

    Protected Sub gvP3IVList_RowDataBound(sender As System.Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Validation
            Dim lblGValidation As Label = e.Row.FindControl("lblGValidation")

            Select Case lblGValidation.Text
                Case ImportSPListErrorType.EmptyCell
                    lblGValidation.Text = Me.GetGlobalResourceObject("Text", "EmptyRow")
                Case ImportSPListErrorType.Duplicated
                    lblGValidation.Text = Me.GetGlobalResourceObject("Text", "Duplicated")
                Case ImportSPListErrorType.NotFound
                    lblGValidation.Text = Me.GetGlobalResourceObject("Text", "NotFound")
            End Select

        End If
    End Sub

    '

    Protected Sub ibtnP3IVImportFile_Click(sender As Object, e As ImageClickEventArgs)
        udcP3ImportMessageBox.Visible = False
        imgP3IUFile.Visible = False
        Session.Remove(SESS.PPC0003_SPImportSuppList)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("FileName", flP3IUFile.FileName)
        udtAuditLogEntry.WriteLog(LogID.LOG00033, "Supplementary List - Import File click")

        ' -- Validation --

        If flP3IUFile.FileName = String.Empty Then
            imgP3IUFile.Visible = True
            udcP3ImportMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005)

            udtAuditLogEntry.AddDescripton("StackTrace", "flP3IUFile.FileName = String.Empty")

        End If

        If udcP3ImportMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcP3ImportMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00035, "Supplementary List - Import File click fail")
            Return
        End If

        ' Save the file to application server
        Dim strUploadDirectory As String = GetSuppListFileUploadDirectory(Session.SessionID)
        Dim strUploadPath As String = strUploadDirectory + flP3IUFile.FileName.Trim

        Dim xlsApp As Excel.Application = Nothing
        Dim xlsWorkBook As Excel.Workbook = Nothing
        Dim xlsWorkSheet As Excel.Worksheet = Nothing

        Try
            flP3IUFile.PostedFile.SaveAs(strUploadPath)

            ' Try to open the file

            ' CRE16-020 - Excel Upgrade 2007 to 2013 [Start][Marco]
            'xlsApp = New Excel.ApplicationClass
            xlsApp = New Excel.Application
            ' CRE16-020 - Excel Upgrade 2007 to 2013 [End][Marco]

            xlsWorkBook = xlsApp.Workbooks.Open(strUploadPath, 0, False, 5, String.Empty)

            ' Read the Excel
            xlsApp.DisplayAlerts = False

            Dim intLastExcelRow As Integer = -1
            Dim blnFirstRowContainValue As Boolean = False
            Dim dt As DataTable = ReadExcel(xlsWorkBook, intLastExcelRow)

            xlsWorkBook.Close()

            Dim strMaxRecord As String = (New GeneralFunction).getSystemParameterValue1("PPC0003_ImportSPSuppListMaxRecord")

            If intLastExcelRow > CInt(strMaxRecord) + 1 OrElse _
                    dt.Rows.Count > CInt(strMaxRecord) Then
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("LastExcelRowContainData = {0}, TotalRowImported = {1}", intLastExcelRow, dt.Rows.Count))

                udcP3ImportMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, _
                                                 New String() {"{PPC0003_ImportSPSuppListMaxRecord}"}, New String() {strMaxRecord})
                udcP3ImportMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00035, "Supplementary List - Import File click fail")

                Return

            End If

            If dt.Rows.Count = 0 Then
                udtAuditLogEntry.AddDescripton("StackTrace", "No data rows in the Excel file")

                udcP3ImportMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006)
                udcP3ImportMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00035, "Supplementary List - Import File click fail")

                Return

            End If

            ' Process data
            trP3IVFile.Visible = True
            lblP3IVFile.Text = flP3IUFile.PostedFile.FileName

            If dt.Select("Fail_Type <> ''").Length > 0 Then
                ' Contains invalid records
                lblP3IVNoOfRecordText.Text = Me.GetGlobalResourceObject("Text", "NoOfInvalidRecords")

                Dim dv As DataView = dt.DefaultView
                dv.RowFilter = "Fail_Type <> ''"
                Dim dtFail As DataTable = dv.ToTable

                lblP3IVNoOfRecord.Text = dtFail.Rows.Count
                lblP3IVNoOfRecord.ForeColor = Drawing.Color.Red

                gvP3IVList.DataSource = dtFail
                gvP3IVList.DataBind()
                gvP3IVList.Columns(3).Visible = True

                ibtnP3IVImportFile.Visible = False
                ibtnP3IVConfirm.Visible = True
                ibtnP3IVConfirm.Enabled = False
                ibtnP3IVConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")

                udcP3ImportMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009)
                udcP3ImportMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00035, "Supplementary List - Import File click fail")

            Else
                ' All records are fine
                lblP3IVNoOfRecordText.Text = Me.GetGlobalResourceObject("Text", "NoOfRecords")
                lblP3IVNoOfRecord.Text = dt.Rows.Count
                lblP3IVNoOfRecord.ForeColor = Nothing

                gvP3IVList.DataSource = dt
                gvP3IVList.DataBind()
                gvP3IVList.Columns(3).Visible = False

                ibtnP3IVImportFile.Visible = False
                ibtnP3IVConfirm.Visible = True
                ibtnP3IVConfirm.Enabled = True
                ibtnP3IVConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")

                udcP3ImportInfoMessageBox.Type = InfoMessageBoxType.Information
                udcP3ImportInfoMessageBox.AddMessage(Me.FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
                udcP3ImportInfoMessageBox.BuildMessageBox()

                udtAuditLogEntry.AddDescripton("NoOfRecord", dt.Rows.Count)
                udtAuditLogEntry.WriteLog(LogID.LOG00034, "Supplementary List - Import File click successful")

            End If

            mvP3Import.SetActiveView(vP3ImportViewRecord)

            Session(SESS.PPC0003_SPImportSuppList) = dt

            ' --- End of Validation ---

        Catch exCom As System.Runtime.InteropServices.COMException
            udtAuditLogEntry.AddDescripton("StackTrace", "COMException: Error in opening file")
            udtAuditLogEntry.AddDescripton("Message", exCom.Message)

            udcP3ImportMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007)
            udcP3ImportMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00035, "Supplementary List - Import File click fail")

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Exception: Unknown error")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)

            udcP3ImportMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008)
            udcP3ImportMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00035, "Supplementary List - Import File click fail")

        Finally
            If Not IsNothing(xlsWorkSheet) Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkSheet)
                xlsWorkSheet = Nothing
            End If

            If Not IsNothing(xlsWorkBook) Then
                Try
                    xlsWorkBook.Close()
                Catch ex As Exception
                End Try

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkBook)
                xlsWorkBook = Nothing
            End If

            If Not IsNothing(xlsApp) Then
                xlsApp.Workbooks.Close()
                xlsApp.Quit()
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsApp)
                xlsApp = Nothing
            End If

            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()

            ' Remove the directory
            RemoveFileUploadDirectory(strUploadDirectory)

        End Try
    End Sub

    Protected Sub ibtnP3IVConfirm_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00036, "Supplementary List - Confirm click")

        Session(SESS.PPC0003_SPSuppList) = Session(SESS.PPC0003_SPImportSuppList)
        Session.Remove(SESS.PPC0003_SPImportSuppList)
        ViewState.Remove(VS.P3ImportPopupMode)

        lblP3NoOfRecordImport.Text = Me.GetGlobalResourceObject("Text", "NImported").ToString _
                                       .Replace("{NoOfRecord}", DirectCast(Session(SESS.PPC0003_SPSuppList), DataTable).Rows.Count)

        ibtnP3Import.Visible = False
        ibtnP3View.Visible = True
        ibtnP3Clear.Visible = True

    End Sub

    Protected Sub ibtnP3IVCancel_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00037, "Supplementary List - Cancel click")

        ViewState.Remove(VS.P3ImportPopupMode)

    End Sub

    Protected Sub ibtnP3IVClose_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00038, "Supplementary List - Close click")

        ViewState.Remove(VS.P3ImportPopupMode)

    End Sub

    '

    Protected Sub ibtnP3Back_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReportID", hfReportID.Value)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00025, "Back click")

        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False

        mvPostPaymentCheck.SetActiveView(vReportList)

        udtAuditLogEntry.WriteEndLog(LogID.LOG00026, "Back click successful")

    End Sub

    Protected Sub ibtnP3Submit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcErrorMessage.Clear()
        udcInfoMessageBox.Visible = False
        imgP3PercentageOfSPRangeError.Visible = False

        Dim strReportID As String = hfReportID.Value
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReportID", hfReportID.Value)

        For Each udtParameter As ParameterObject In ucP3CriteriaBase.GetParameterString
            If TypeOf udtParameter Is ParameterObjectList Then
                Dim lstValue As New List(Of String)

                For Each strValue As String In DirectCast(udtParameter, ParameterObjectList).ParamValueList
                    lstValue.Add(strValue)
                Next

                udtAuditLogEntry.AddDescripton(udtParameter.ParamName, String.Join(",", lstValue.ToArray))

            ElseIf TypeOf udtParameter Is ParameterObject Then
                udtAuditLogEntry.AddDescripton(udtParameter.ParamName, udtParameter.ParamValue)

            End If
        Next

        udtAuditLogEntry.WriteStartLog(LogID.LOG00027, "Submit click")

        ' Check if there is available slot for new PPC report
        If ContainAvailableSlotToSubmitPPCReport() = False Then
            udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
            udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00029, "Submit click fail")

            Return
        End If

        ' --- Validation ---

        ' (b) Percentage of Service Provider 
        Dim blnValidPercentageSP As Boolean = True

        If txtP3PercentageOfSP.Text = String.Empty Then
            udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00012)
            imgP3PercentageOfSPRangeError.Visible = True
            blnValidPercentageSP = False

        End If

        If blnValidPercentageSP Then
            If IsNumeric(txtP3PercentageOfSP.Text.Trim) = False Then
                udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013)
                imgP3PercentageOfSPRangeError.Visible = True
                blnValidPercentageSP = False
            End If

        End If

        If blnValidPercentageSP Then
            Dim strLowerLimit As String = String.Empty
            Dim strUpperLimit As String = String.Empty

            Call (New GeneralFunction).getSystemParameter("PPC0003_PercentageOfSPRange", strLowerLimit, strUpperLimit)

            If CInt(txtP3PercentageOfSP.Text.Trim) < CInt(strLowerLimit) OrElse CInt(txtP3PercentageOfSP.Text.Trim) > CInt(strUpperLimit) Then
                udcErrorMessage.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014)
                imgP3PercentageOfSPRangeError.Visible = True
                blnValidPercentageSP = False
            End If

        End If

        ' User Controls

        Dim lstSysMsg As New List(Of SystemMessage)
        Dim lstSysMsgParam1 As New List(Of String)
        Dim lstSysMsgParam2 As New List(Of String)

        ucP3CriteriaBase.ValidateCriteriaInput(strReportID, lstSysMsg, lstSysMsgParam1, lstSysMsgParam2)

        If lstSysMsg.Count > 0 Then
            ' Validation Fail
            For i As Integer = 0 To lstSysMsg.Count - 1
                If lstSysMsgParam1.Count - 1 >= i Then
                    If lstSysMsgParam2.Count - 1 >= i Then
                        udcErrorMessage.AddMessage(lstSysMsg(i), New String() {"%s", "%t"}, New String() {lstSysMsgParam1(i).Trim, lstSysMsgParam2(i).Trim})
                    Else
                        udcErrorMessage.AddMessage(lstSysMsg(i), New String() {"%s"}, New String() {lstSysMsgParam1(i).Trim})
                    End If

                Else
                    udcErrorMessage.AddMessage(lstSysMsg(i))
                End If
            Next

        End If

        If udcErrorMessage.GetCodeTable.Rows.Count > 0 Then
            udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00029, "Submit click fail")

            Return

        End If

        ' --- End of Validation ---

        ' Add additional parameters not in user controls
        Dim udtSProcParmList As StoreProcParamCollection = ucP3CriteriaBase.GetCriteriaInput
        Dim strGenerationID As String = (New GeneralFunction).generateFileSeqNo()

        ' (a) Supplementary List
        udtSProcParmList.AddParam("@SPList", SqlDbType.VarChar, 80, String.Format("AdditionalParameter:{0}:SPList", strGenerationID))

        ' (b) Percentage of Service Provider
        udtSProcParmList.AddParam("@PercentageOfSP", SqlDbType.Int, 8, txtP3PercentageOfSP.Text.Trim)

        ' Additional Parameter
        Dim dtAP As DataTable = ConstructAdditionalParameterDataTable()
        Dim drAP As DataRow = Nothing

        For Each dr As DataRow In DirectCast(Session(SESS.PPC0003_SPSuppList), DataTable).Rows
            drAP = dtAP.NewRow
            drAP("Parm_Name") = "SPList"
            drAP("Parm_Value") = dr("SPID")

            dtAP.Rows.Add(drAP)

        Next

        ' Submit Report
        Dim udtSystemMessage As SystemMessage = SubmitReport(strReportID, strGenerationID, udtSProcParmList, dtAP)
        
        If Not IsNothing(udtSystemMessage) Then
            udcErrorMessage.AddMessage(udtSystemMessage)
            udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00029, "Submit click fail")

            Return

        End If

        udtAuditLogEntry.WriteEndLog(LogID.LOG00028, "Submit click successful")

        mvPostPaymentCheck.SetActiveView(vReturn)

    End Sub

#End Region

#Region "User Control Function"

    Private Sub SetupReportCriteria(ByVal strFileID As String, ByVal ucStatisticsCriteriaBase As StatisticsCriteriaBase)
        ucStatisticsCriteriaBase.Visible = True

        Dim udtFileGenerationBLL As New FileGenerationBLL
        Dim udtFileGeneration As FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(strFileID)

        Dim udtStatisticsBLL As New StatisticsBLL
        Dim udtStatisticsModel As StatisticsModel

        udtStatisticsModel = udtStatisticsBLL.GetStatisticsByStatisticsID(strFileID)

        ' Post Payment Check Criteria (user controls)
        Session(SESS.PostPaymentCheckCriteriaUC) = udtStatisticsModel
        ucStatisticsCriteriaBase.Build(udtStatisticsModel.CriteriaSetup)

        ' Report ID
        lblReportID.Text = udtFileGeneration.DisplayCode
        hfReportID.Value = strFileID

        ' Report Name
        lblReportName.Text = udtFileGeneration.FileName

    End Sub

#End Region

#Region "Supporting Functions"

    Private Sub CheckAvailableSlotToSubmitPPC()
        If Not ContainAvailableSlotToSubmitPPCReport() Then
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.AddMessage(Me.FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMessageBox.BuildMessageBox()

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00001, "Show message for number of Post Payment Check report has exceeded limit")

        End If
    End Sub

    Private Function ContainAvailableSlotToSubmitPPCReport() As Boolean
        Dim intPPCGenLimit As Integer = CInt((New GeneralFunction).getSystemParameterValue1("PPCReport_PendingGenLimit"))

        If (New FileGenerationBLL).GetFileGenerationQueueToRun_PPCCount() >= intPPCGenLimit Then
            Return False
        End If

        Return True

    End Function

    '

    Private Sub ValidatePPC0002TargetNumberOfTransaction(strInput As String, strLabelText As String, strParmName As String, _
                                                         ByRef lstSystemMessage As List(Of SystemMessage), _
                                                         ByRef lstOldChar As List(Of String), ByRef lstNewChar As List(Of String))
        ' Empty?
        If strInput = String.Empty Then
            lstSystemMessage.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028))
            lstOldChar.Add("%s")
            lstNewChar.Add(strLabelText)

            Return

        End If

        ' Is numeric?
        If IsNumeric(txtP2TargetTran.Text.Trim) = False Then
            lstSystemMessage.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00365))
            lstOldChar.Add("%s")
            lstNewChar.Add(strLabelText)

            Return

        End If

        ' Within range?
        Dim udtGeneralFunction As New GeneralFunction
        Dim strParmValue1 As String = String.Empty
        Dim strParmValue2 As String = String.Empty

        udtGeneralFunction.getSystemParameter(strParmName, strParmValue1, strParmValue2)

        If CInt(strInput) < CInt(strParmValue1) OrElse CInt(strInput) > CInt(strParmValue2) Then
            lstSystemMessage.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00366))
            lstOldChar.Add("%s")
            lstNewChar.Add(strLabelText)

            Return

        End If

    End Sub

    '

    Public Function GetSuppListFileUploadDirectory(ByVal strSessionID As String) As String
        Dim udtGeneralFunction As New GeneralFunction

        Dim strDirectory As String = udtGeneralFunction.getSystemParameterValue1("PPCFileUploadPath").Trim

        If Not strDirectory.EndsWith("\") Then strDirectory += "\"

        If Not Directory.Exists(strDirectory) Then Directory.CreateDirectory(strDirectory)

        strDirectory += udtGeneralFunction.generateTempFolderPath(strSessionID)

        Dim intSuffix As Integer = 0

        While True
            If Directory.Exists(String.Format("{0}{1}", strDirectory, intSuffix.ToString)) Then
                intSuffix += 1

                If intSuffix >= 100 Then
                    ' Loop for 100 times and cannot find an unique directory, there must be something wrong
                    Throw New Exception("GetSuppListFileUploadDirectory: intSuffix >= 100")
                End If

            Else
                Directory.CreateDirectory(String.Format("{0}{1}", strDirectory, intSuffix.ToString))
                Return String.Format("{0}{1}{2}", strDirectory, intSuffix.ToString, "\")

            End If

        End While

        Return Nothing

    End Function

    Public Sub RemoveFileUploadDirectory(ByVal strDirectory As String)
        If Directory.Exists(strDirectory) Then Directory.Delete(strDirectory, True)
    End Sub

    '

    Private Function ConstructPPC0002SPPracticeListDataTable()
        Dim dt As New DataTable

        dt.Columns.Add("Index", GetType(Integer))
        dt.Columns.Add("SPID", GetType(String))
        dt.Columns.Add("PracticeID", GetType(String))
        dt.Columns.Add("PracticeName", GetType(String))
        dt.Columns.Add("PracticeStatus", GetType(String))

        Return dt

    End Function

    Private Function ConstructPPC0003ImportSPListDataTable()
        Dim dt As New DataTable

        dt.Columns.Add("Row_No", GetType(Integer))
        dt.Columns.Add("SPID", GetType(String))
        dt.Columns.Add("SP_Name", GetType(String))
        dt.Columns.Add("Fail_Type", GetType(String))

        Return dt

    End Function

    Private Function ConstructAdditionalParameterDataTable() As DataTable
        Dim dt As New DataTable

        dt.Columns.Add("Parm_Name", GetType(String))
        dt.Columns.Add("Parm_Value", GetType(String))

        Return dt

    End Function

#End Region

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
