Imports Common.Encryption
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.ComObject
Imports Common.Component.FileGeneration

Partial Public Class Datadownload
    Inherits BasePageWithGridView
    'Inherits System.Web.UI.Page

    Dim dt As DataTable
    Dim mode As String
    Dim src As String
    Dim transIndex As Integer
    Dim oriStatus As String
    Dim udtcomfunct As New Common.ComFunction.GeneralFunction
    Dim strDownloadList As String = "strDownloadList"
    Dim strWindowWasherPath As String = "WindowWasherPath"
    Dim udcValidator As New Common.Validation.Validator
    Dim udcDataDownloadBll As New DatadownloadBLL
    Dim udtHCVUUser As HCVUUserModel
    Dim udtHCVUUserBLL As New HCVUUserBLL
    Dim udcFormater As New Common.Format.Formatter
    Const COMMON_FUNCTION_CODE As String = "990000"
    Const FUNCTION_CODE As String = "010702"

#Region "Private Class"
    Public Class SESS
        Public Const DictionaryTimestampPath As String = "010702_Dictionary_Timestamp_Path"
    End Class

    Public Class QueryString
        Public Const TimeStamp As String = "TS"
    End Class
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "File and Report Download")

            Me.rbMyFolder.Checked = True
            Me.lbl_RecycleBinNote.Visible = False
            Me.lbl_KeepFilePeriodNote.Visible = Not Me.lbl_RecycleBinNote.Visible
            Me.ibtn_undelete.Visible = False
            Me.btn_delete_disabled.Visible = False

            Me.hfSelectedIndex.Text = String.Empty

            'CRE14-00XX - Retain sorting other and page index after download report  [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            LoadEnqDownloadGrid(IIf(Me.rbMyFolder.Checked, FileDownloadStatus.NotDownloadYet, FileDownloadStatus.Deleted), True)
            'CRE14-00XX - Retain sorting other and page index after download report  [End][Chris YIM]

            Dim strvalue1 As String = String.Empty
            Dim strvalue2 As String = String.Empty

            udtcomfunct.getSystemParameter("PasswordRuleNumber", strvalue1, strvalue2)

            Me.txt_accAct_newPW.Attributes.Add("onKeyUp", "checkPassword(this.value,'" & CInt(strvalue2.Trim) & "', '" & CInt(strvalue2.Trim) & "', 'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")
            'Me.txt_accAct_newPW.Attributes.Add("onKeyUp", "checkPassword(this.value,'strength1','strength2','strength3','progressBar', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "', '" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "','direction2','direction1');")
            Me.lbl_RecycleBinNote.Visible = False
            Me.lbl_KeepFilePeriodNote.Visible = Not Me.lbl_RecycleBinNote.Visible

            Dim strOCX_codebase As String
            Dim strapppath As String
            Dim strhostname As String
            Dim strOCX_version As String
            Dim strhttp As String

            Dim strvalue As String = String.Empty
            udtcomfunct.getSystemParameter("SiteHttp", strvalue, String.Empty)
            strhttp = strvalue

            strvalue = String.Empty
            udtcomfunct.getSystemParameter("OCXAppPath", strvalue, String.Empty)
            strapppath = strvalue

            strvalue = String.Empty
            udtcomfunct.getSystemParameter("OCXDatadownloadVer", strvalue, String.Empty)
            strOCX_version = strvalue

            'strhostname = "localhost" 'System.Configuration.ConfigurationSettings.AppSettings("HostName")
            strhostname = Request.Url.Host
            'strOCX_version = "1,0,0,0" 'System.Configuration.ConfigurationSettings.AppSettings("DataDownloadOCXVer")

            strOCX_codebase = "<OBJECT id='DataDownload' style='DISPLAY: none' "
            strOCX_codebase += "codeBase='" + strhttp + "://" + strhostname + strapppath + "OCX/EVSDataDownload.CAB#Version=" + strOCX_version + "' "
            strOCX_codebase += "classid=CLSID:21289723-36E9-46CA-AE00-1F979770F910 viewastext> " + vbCrLf
            strOCX_codebase += "<PARAM NAME='_ExtentX' VALUE='0'>" + vbCrLf
            strOCX_codebase += "<PARAM NAME='_ExtentY' VALUE='6350'>" + vbCrLf
            strOCX_codebase += "</OBJECT>" + vbCrLf

            Me.RegisterStartupScript("clientScript", strOCX_codebase)
            'ScriptManager.RegisterStartupScript(Me, GetType(Page), "DataDownloadObject", strOCX_codebase, True)


            strvalue = String.Empty
            udtcomfunct.getSystemParameter("WindowWasherPath", strvalue, String.Empty)
            Session(strWindowWasherPath) = strvalue.Replace("\", "?")
            Me.RegisterStartupScript("clientScriptCheckOCX", "<script language='javascript'>CheckOCXExists('" & Session(strWindowWasherPath) & "');</script>")

            ' Browser: Firefox
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnDialogConfirm)

        End If
    End Sub

    Private Sub LoadEnqDownloadGrid(ByVal strDownloadStatus As String, ByVal blnPageIndexReset As Boolean)

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("File Status", strDownloadStatus)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Load File List")

        Try
            Dim dt, dtall As DataTable
            Dim dr, drall As DataRow
            Dim i As Integer

            dt = New DataTable()
            dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
            dt.Columns.Add(New DataColumn("fileIconURL", GetType(String)))
            dt.Columns.Add(New DataColumn("reportNum", GetType(String)))
            dt.Columns.Add(New DataColumn("reportName", GetType(String)))
            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            dt.Columns.Add(New DataColumn("DisplayCode", GetType(String)))
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
            dt.Columns.Add(New DataColumn("submissionDate", GetType(DateTime)))
            dt.Columns.Add(New DataColumn("submittedBy", GetType(String)))
            dt.Columns.Add(New DataColumn("status", GetType(String)))
            dt.Columns.Add(New DataColumn("syspassword", GetType(String)))
            dt.Columns.Add(New DataColumn("SysEncryptedFileName", GetType(String)))
            dt.Columns.Add(New DataColumn("FileType", GetType(String)))
            dt.Columns.Add(New DataColumn("OutputFile", GetType(String)))
            dt.Columns.Add(New DataColumn("GenerationID", GetType(String)))
            dt.Columns.Add(New DataColumn("DownloadStatus", GetType(String)))
            dt.Columns.Add(New DataColumn("Recipient", GetType(String)))
            dt.Columns.Add(New DataColumn("FileDescription", GetType(String)))

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            dtall = udcDataDownloadBll.GetDownloadListByFileDownloadStatus(udtHCVUUser.UserID, strDownloadStatus)

            If dtall.Rows.Count = 0 Then
                Me.panel_Folder.Visible = False
                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcInfoMessageBox.AddMessage(COMMON_FUNCTION_CODE, "I", "00001")
                Me.panel_searchCriteria.Visible = False
                Me.gvDataDownloadFolder.Visible = False
                Me.btn_delete_disabled.Visible = False
                Me.ibtn_delete.Visible = True
            Else
                Me.panel_Folder.Visible = True
                For i = 0 To dtall.Rows.Count - 1
                    drall = CType(dtall.Rows(i), DataRow)
                    dr = dt.NewRow()
                    dr("lineNum") = i + 1
                    'If Trim(drall("status")).Equals(DataDownloadStatus.Completed) Then
                    '    dr("fileIconURL") = ResolveUrl(HttpContext.GetGlobalResourceObject("ImageUrl", "ReadyDownloadBtn"))
                    'Else
                    '    dr("fileIconURL") = ResolveUrl(HttpContext.GetGlobalResourceObject("ImageUrl", "ProcessingBtn"))
                    'End If

                    dr("reportNum") = drall("reportNum")
                    dr("reportName") = drall("reportName")
                    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                    dr("DisplayCode") = drall("Display_Code")
                    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
                    dr("submissionDate") = Trim(drall("submissionDate"))
                    dr("submittedBy") = drall("submittedBy")
                    dr("status") = drall("status")
                    dr("syspassword") = drall("syspassword")
                    dr("SysEncryptedFileName") = drall("SysEncryptedFileName")
                    dr("FileType") = drall("FileType")
                    dr("OutputFile") = drall("OutputFile")
                    dr("GenerationID") = drall("GenerationID")
                    dr("DownloadStatus") = drall("DownloadStatus")
                    dr("Recipient") = drall("Recipient")
                    dr("FileDescription") = drall("FileDescription")
                    dt.Rows.Add(dr)
                Next

                Session(strDownloadList) = dt

                'CRE14-00XX - Retain sorting other and page index after download report  [Start][Chris YIM]
                '----------------------------------------------------------------------------------------- 
                If blnPageIndexReset Then
                    Me.GridViewDataBind(Me.gvDataDownloadFolder, dt, "submissionDate", "DESC", False)
                Else
                    Me.GridViewDataBind(Me.gvDataDownloadFolder, dt)
                End If
                'CRE14-00XX - Retain sorting other and page index after download report  [End][Chris YIM]

                'dv = New DataView(dt)
                'Me.gvDataDownloadFolder.DataSource = dv
                'Me.gvDataDownloadFolder.DataBind()

                Me.gvDataDownloadFolder.Visible = True
            End If

            Me.udcInfoMessageBox.BuildMessageBox()
            udtAuditLogEntry.AddDescripton("File Status", strDownloadStatus)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Load File List successful")

            Dim strFileGenerateID As String = Session("FileGenerateID")

            If Not IsNothing(strFileGenerateID) Then
                Session.Remove("FileGenerateID")

                For Each gvr As GridViewRow In gvDataDownloadFolder.Rows
                    If DirectCast(gvr.FindControl("lblGenerationID"), Label).Text = strFileGenerateID Then
                        CustomDownload(gvr)
                        Exit For
                    End If

                    If gvr.RowIndex >= 9 Then Exit For

                Next

            End If

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("File Status", strDownloadStatus)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Load File List fail")
        End Try
    End Sub

    Private Sub CustomDownload(row As GridViewRow)
        '' Convert the row index stored in the CommandArgument
        '' property to an Integer.

        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim dtDownloadList As DataTable
        Dim drDownloadList() As DataRow
        Dim drSelectedRow As DataRow
        Dim lblGenerationID As Label


        lblGenerationID = CType(row.Cells(0).FindControl("lblGenerationID"), Label)

        If lblGenerationID Is Nothing Then
            Throw New Exception(String.Format("Generation ID is nothing."))
        End If

        dtDownloadList = CType(Session(strDownloadList), DataTable)

        drDownloadList = dtDownloadList.Select("GenerationID = '" + lblGenerationID.Text.Trim + "'")

        If drDownloadList.Length <> 1 Then
            Throw New Exception(String.Format("Concurrent Download Error: No available result is found by Generation ID {0}.", lblGenerationID.Text.Trim))
        End If

        drSelectedRow = drDownloadList(0)
        Me.hfSelectedIndex.Text = lblGenerationID.Text.Trim

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("Generation_ID", drSelectedRow("GenerationID").trim)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Select File")

        Try
            Dim strFileName As String = drSelectedRow("SysEncryptedFileName").trim
            strFileName = strFileName.Substring(strFileName.IndexOf("\") + 1)
            Me.lbl_SelectedReportName.Text = "(" & drSelectedRow("reportName").trim & " - " & strFileName & ")"
            '' Retrieve the row that contains the button clicked 
            '' by the user from the Rows collection.


            Me.gvDataDownloadFolder.SelectedIndex = Convert.ToInt32(row.RowIndex)

            If udcValidator.IsEmpty(Me.HiddenField1.Text) Then
                Me.panel_searchCriteria.Visible = True
                Me.btn_delete_disabled.Visible = True
                Me.ibtn_delete.Visible = False
                Me.ScriptManager1.SetFocus(Me.btn_Download)
                Me.ScriptManager1.SetFocus(Me.txt_accAct_newPW)
                'Clear All Check box selected
                Dim i As Integer
                Dim cb As CheckBox
                For i = 0 To Me.gvDataDownloadFolder.Rows.Count - 1
                    Dim row2 = gvDataDownloadFolder.Rows(i)
                    cb = CType(row2.Cells(0).FindControl("chk_selected"), CheckBox)
                    cb.Checked = False
                Next
            Else
                Me.panel_searchCriteria.Visible = False
                Me.btn_delete_disabled.Visible = False
                Me.ibtn_delete.Visible = True
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", Me.HiddenField1.Text)
            End If

            ''Re-check the download folder exists or not
            'Dim strvalue As String = String.Empty
            'udtcomfunct.getSystemParameter("WindowWasherPath", strvalue, String.Empty)
            'Session(strWindowWasherPath) = strvalue.Replace("\", "?")
            'Me.RegisterStartupScript("clientScriptCheckOCX2", "<script language='javascript'>CheckOCXExists('" & Session(strWindowWasherPath) & "');</script>")

            Me.udcInfoMessageBox.BuildMessageBox()
            udtAuditLogEntry.AddDescripton("Generation_ID", drSelectedRow("GenerationID").trim)
            Me.udcErrorMessage.BuildMessageBox("DownloadFail", udtAuditLogEntry, LogID.LOG00006, "Select File Fail")

            udtAuditLogEntry.AddDescripton("Generation_ID", drSelectedRow("GenerationID").trim)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Select File end")
        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("Generation_ID", drSelectedRow("GenerationID").trim)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Select File fail")
        End Try

    End Sub

    Protected Sub btn_Return_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Me.panel_searchCriteria.Visible = False
        If e.CommandName = "DataDownload" And Me.rbMyFolder.Checked Then

            '' Convert the row index stored in the CommandArgument
            '' property to an Integer.
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = Me.gvDataDownloadFolder.Rows(index)

            'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim dtDownloadList As DataTable
            Dim drDownloadList() As DataRow
            Dim drSelectedRow As DataRow
            Dim lblGenerationID As Label


            lblGenerationID = CType(row.Cells(0).FindControl("lblGenerationID"), Label)

            If lblGenerationID Is Nothing Then
                Throw New Exception(String.Format("Generation ID is nothing."))
            End If

            dtDownloadList = CType(Session(strDownloadList), DataTable)

            drDownloadList = dtDownloadList.Select("GenerationID = '" + lblGenerationID.Text.Trim + "'")

            If drDownloadList.Length <> 1 Then
                Throw New Exception(String.Format("Concurrent Download Error: No available result is found by Generation ID {0}.", lblGenerationID.Text.Trim))
            End If

            drSelectedRow = drDownloadList(0)
            Me.hfSelectedIndex.Text = lblGenerationID.Text.Trim

            'add audit log
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.AddDescripton("Generation_ID", drSelectedRow("GenerationID").trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Select File")

            Try
                Dim strFileName As String = drSelectedRow("SysEncryptedFileName").trim
                strFileName = strFileName.Substring(strFileName.IndexOf("\") + 1)
                Me.lbl_SelectedReportName.Text = "(" & drSelectedRow("reportName").trim & " - " & strFileName & ")"
                '' Retrieve the row that contains the button clicked 
                '' by the user from the Rows collection.


                Me.gvDataDownloadFolder.SelectedIndex = Convert.ToInt32(e.CommandArgument)

                If udcValidator.IsEmpty(Me.HiddenField1.Text) Then
                    Me.panel_searchCriteria.Visible = True
                    Me.btn_delete_disabled.Visible = True
                    Me.ibtn_delete.Visible = False
                    Me.ScriptManager1.SetFocus(Me.btn_Download)
                    Me.ScriptManager1.SetFocus(Me.txt_accAct_newPW)
                    'Clear All Check box selected
                    Dim i As Integer
                    Dim cb As CheckBox
                    For i = 0 To Me.gvDataDownloadFolder.Rows.Count - 1
                        Dim row2 = gvDataDownloadFolder.Rows(i)
                        cb = CType(row2.Cells(0).FindControl("chk_selected"), CheckBox)
                        cb.Checked = False
                    Next
                Else
                    Me.panel_searchCriteria.Visible = False
                    Me.btn_delete_disabled.Visible = False
                    Me.ibtn_delete.Visible = True
                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", Me.HiddenField1.Text)
                End If

                ''Re-check the download folder exists or not
                'Dim strvalue As String = String.Empty
                'udtcomfunct.getSystemParameter("WindowWasherPath", strvalue, String.Empty)
                'Session(strWindowWasherPath) = strvalue.Replace("\", "?")
                'Me.RegisterStartupScript("clientScriptCheckOCX2", "<script language='javascript'>CheckOCXExists('" & Session(strWindowWasherPath) & "');</script>")

                Me.udcInfoMessageBox.BuildMessageBox()
                udtAuditLogEntry.AddDescripton("Generation_ID", drSelectedRow("GenerationID").trim)
                Me.udcErrorMessage.BuildMessageBox("DownloadFail", udtAuditLogEntry, LogID.LOG00006, "Select File Fail")

                udtAuditLogEntry.AddDescripton("Generation_ID", drSelectedRow("GenerationID").trim)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Select File end")
            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("Generation_ID", drSelectedRow("GenerationID").trim)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Select File fail")
            End Try
            'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [End][Chris YIM]
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ctrlEnabled, ctrlDisabled As ImageButton
            Dim ctrlSubmissionDtm, ctrlActualIndex As Label
            Dim intLineNum As Integer
            ctrlEnabled = CType(e.Row.Cells(2).FindControl("lbtn_ReadyDownload"), ImageButton)
            ctrlDisabled = CType(e.Row.Cells(2).FindControl("lbtn_Processing"), ImageButton)
            ctrlSubmissionDtm = CType(e.Row.Cells(5).FindControl("lblSubmissionDtm"), Label)
            ctrlActualIndex = CType(e.Row.Cells(0).FindControl("lblRecordNum"), Label)
            'intLineNum = e.Row.Cells(0).Text
            intLineNum = ctrlActualIndex.Text.Trim
            dt = Session(strDownloadList)

            If dt.Rows(intLineNum - 1)("status").Equals(DataDownloadStatus.Completed) Then
                'If dt.Rows((Me.gvDataDownloadFolder.PageIndex * Me.gvDataDownloadFolder.PageSize) + e.Row.RowIndex)("status").Equals(DataDownloadStatus.Completed) Then
                ctrlEnabled.Visible = True
                ctrlDisabled.Visible = False

                'ctrlEnabled.CommandArgument = (Me.gvDataDownloadFolder.PageIndex * Me.gvDataDownloadFolder.PageSize) + e.Row.RowIndex
                ctrlEnabled.CommandArgument = e.Row.RowIndex
            Else
                ctrlEnabled.Visible = False
                ctrlDisabled.Visible = True
            End If

            'ctrlSubmissionDtm.Text = udcFormater.convertDateTime(Trim(ctrlSubmissionDtm.Text), "E")
            ctrlSubmissionDtm.Text = udcFormater.convertDateTime(ctrlSubmissionDtm.Text)

            'e.Row.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(Me.gvDataDownloadFolder, "Select$" + e.Row.RowIndex.ToString(), False))
            'e.Row.Style.Add("cursor", "hand")
        End If

        If (e.Row.RowType = DataControlRowType.Header) Then
            'adding an attribute for onclick event on the check box in the header
            'and passing the ClientID of the Select All checkbox
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & _
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).ClientID & "')")
        End If
    End Sub

    'Protected Sub GridView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If Me.rbMyFolder.Checked Then
    '        dt = Session(strDownloadList)
    '        Me.txt_accAct_newPW.Text = ""
    '        Me.txt_accAct_confirmPW.Text = ""

    '        'Clear All Check box selected
    '        Dim i As Integer
    '        Dim cb As CheckBox
    '        For i = 0 To Me.gvDataDownloadFolder.Rows.Count - 1
    '            Dim row = gvDataDownloadFolder.Rows(i)
    '            cb = CType(row.Cells(0).FindControl("chk_selected"), CheckBox)
    '            cb.Checked = False
    '        Next

    '        Dim intRealIndex As Integer
    '        intRealIndex = (Me.gvDataDownloadFolder.PageIndex * Me.gvDataDownloadFolder.PageSize) + gvDataDownloadFolder.SelectedIndex

    '        If udcValidator.IsEmpty(Me.HiddenField1.Text) Then
    '            If Trim(dt.Rows(intRealIndex)("status")).Equals(DataDownloadStatus.Completed) Then
    '                Me.lbl_SelectedReportName.Text = "(" & Me.gvDataDownloadFolder.SelectedRow.Cells(4).Text & ")"
    '                Me.panel_searchCriteria.Visible = True
    '                Me.btn_delete_disabled.Visible = True
    '                Me.ibtn_delete.Visible = False
    '            Else
    '                Me.panel_searchCriteria.Visible = False
    '                Me.btn_delete_disabled.Visible = False
    '                Me.ibtn_delete.Visible = True
    '            End If
    '        Else
    '            Me.panel_searchCriteria.Visible = False
    '            Me.btn_delete_disabled.Visible = False
    '            Me.ibtn_delete.Visible = True
    '            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", Me.HiddenField1.Text)
    '        End If
    '    End If

    '    Me.udcInfoMessageBox.BuildMessageBox()
    '    Me.udcErrorMessage.BuildMessageBox()
    'End Sub

    Protected Sub btn_Download_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strSrcFilePath As String
        Dim strOutputFilename As String
        Dim strGenerationID As String
        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim drDownloadList() As DataRow
        Dim drSelectedRow As DataRow

        dt = Session(strDownloadList)
        Me.udcInfoMessageBox.BuildMessageBox()

        'Dim lblRecordNum As Label = CType(Me.gvDataDownloadFolder.Rows(Me.gvDataDownloadFolder.SelectedIndex).FindControl("lblRecordNum"), Label)

        'If Not lblRecordNum Is Nothing Then
        'Dim intRecordNum As Integer = CInt(lblRecordNum.Text) - 1
        'If Me.hfSelectedIndex.Text.Trim <> Session(SESS.GenerationID) Then
        '    Throw New Exception(String.Format("Concurrent Download Error: Generation ID in client-side is {0}; Generation ID in server-side is {1}.", hfSelectedIndex.Text, Session(SESS.GenerationID)))
        'End If
        'End If

        drDownloadList = dt.Select("GenerationID = '" + Me.hfSelectedIndex.Text.Trim + "'")

        If drDownloadList.Length <> 1 Then
            Throw New Exception(String.Format("Concurrent Download Error: No available result is found by Generation ID {0}.", Me.hfSelectedIndex.Text.Trim))
        End If

        drSelectedRow = drDownloadList(0)

        strOutputFilename = Trim(drSelectedRow("OutputFile"))

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        strGenerationID = Trim(drSelectedRow("GenerationID"))
        udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Download")

        Try
            If Not udcValidator.IsEmpty(Me.txt_accAct_newPW.Text) Then
                'If Not udcValidator.IsEmpty(Me.txt_accAct_confirmPW.Text) Then
                If udcValidator.ValidateFileDownloadPassword(Me.txt_accAct_newPW.Text) Then
                    'If Not udcValidator.ChkIsIdenticial(Me.txt_accAct_newPW.Text, Me.txt_accAct_confirmPW.Text) Then
                    '    Me.MessageBox.AddMessage(COMMON_FUNCTION_CODE, "E", "00059")
                    'Else
                    'Determine the source file path

                    'Get the temp folder path
                    Dim strTempFolderPath As String = udtcomfunct.generateTempFolderPath(Me.Session.SessionID)

                    Select Case Trim(drSelectedRow("reportNum"))
                        Case DataDownloadFileID.BankPaymentFile
                            Dim strvalue As String = String.Empty
                            udtcomfunct.getSystemParameter("BankFileStoragePath", strvalue, String.Empty)
                            strSrcFilePath = strvalue

                            'Proceed to download
                            If DownloadTxtFile(strSrcFilePath & strTempFolderPath & "\", Trim(drSelectedRow("SysEncryptedFileName")), Me.txt_accAct_newPW.Text, Trim(drSelectedRow("GenerationID"))) Then
                                If udcValidator.IsEmpty(Me.HiddenField1.Text) Then
                                    'UpdateFileDownloadedStatus()
                                    'Session(strSelectedIndex) = -1
                                    'Me.gvDataDownloadFolder.SelectedIndex = -1
                                    'Me.panel_searchCriteria.Visible = False
                                    'Me.btn_delete_disabled.Visible = False
                                    'Me.ibtn_delete.Visible = True
                                    'Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                                    'Me.udcInfoMessageBox.AddMessage("990000", "I", "00013", "%s", Session(strWindowWasherPath).ToString.Trim.Replace("?", "\"))
                                    'udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Download successful")
                                Else
                                    Me.hfSelectedIndex.Text = String.Empty
                                    Me.gvDataDownloadFolder.SelectedIndex = -1
                                    Me.panel_searchCriteria.Visible = False
                                    Me.btn_delete_disabled.Visible = False
                                    Me.ibtn_delete.Visible = True
                                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", Me.HiddenField1.Text)
                                    udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                                End If
                            Else    'General Error
                                Me.hfSelectedIndex.Text = String.Empty
                                Me.panel_searchCriteria.Visible = False
                                Me.btn_delete_disabled.Visible = False
                                Me.ibtn_delete.Visible = True
                                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                                udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                            End If
                        Case DataDownloadFileID.BoardAndCouncil
                            Dim strvalue As String = String.Empty
                            udtcomfunct.getSystemParameter("BNCFileStoragePath", strvalue, String.Empty)
                            strSrcFilePath = strvalue

                            'Proceed to download                    
                            If DownloadExcelFile(Trim(drSelectedRow("syspassword")), strSrcFilePath & strTempFolderPath & "\", Trim(drSelectedRow("SysEncryptedFileName")), Me.txt_accAct_newPW.Text, Trim(drSelectedRow("GenerationID"))) Then
                                If udcValidator.IsEmpty(Me.HiddenField1.Text) Then
                                    'UpdateFileDownloadedStatus()
                                    'Session(strSelectedIndex) = -1
                                    'Me.gvDataDownloadFolder.SelectedIndex = -1
                                    'Me.panel_searchCriteria.Visible = False
                                    'Me.btn_delete_disabled.Visible = False
                                    'Me.ibtn_delete.Visible = True
                                    'Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                                    'Me.udcInfoMessageBox.AddMessage("990000", "I", "00013", "%s", Session(strWindowWasherPath).ToString.Trim.Replace("?", "\"))
                                    'udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Download successful")
                                Else
                                    Me.hfSelectedIndex.Text = String.Empty
                                    Me.gvDataDownloadFolder.SelectedIndex = -1
                                    Me.panel_searchCriteria.Visible = False
                                    Me.btn_delete_disabled.Visible = False
                                    Me.ibtn_delete.Visible = True
                                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", Me.HiddenField1.Text)
                                    udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                                End If
                            Else    'General Error
                                Me.hfSelectedIndex.Text = String.Empty
                                Me.panel_searchCriteria.Visible = False
                                Me.btn_delete_disabled.Visible = False
                                Me.ibtn_delete.Visible = True
                                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                                udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                            End If
                        Case DataDownloadFileID.SuperDownload
                            Dim strvalue As String = String.Empty
                            udtcomfunct.getSystemParameter("SuperDownloadStoragePath", strvalue, String.Empty)
                            strSrcFilePath = strvalue

                            'Proceed to download                    
                            If DownloadExcelFile(Trim(drSelectedRow("syspassword")), strSrcFilePath & strTempFolderPath & "\", Trim(drSelectedRow("SysEncryptedFileName")), Me.txt_accAct_newPW.Text, Trim(drSelectedRow("GenerationID"))) Then
                                If udcValidator.IsEmpty(Me.HiddenField1.Text) Then
                                    'UpdateFileDownloadedStatus()
                                    'Session(strSelectedIndex) = -1
                                    'Me.gvDataDownloadFolder.SelectedIndex = -1
                                    'Me.panel_searchCriteria.Visible = False
                                    'Me.btn_delete_disabled.Visible = False
                                    'Me.ibtn_delete.Visible = True
                                    'Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                                    'Me.udcInfoMessageBox.AddMessage("990000", "I", "00013", "%s", Session(strWindowWasherPath).ToString.Trim.Replace("?", "\"))
                                    'udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Download successful")
                                Else
                                    Me.hfSelectedIndex.Text = String.Empty
                                    Me.gvDataDownloadFolder.SelectedIndex = -1
                                    Me.panel_searchCriteria.Visible = False
                                    Me.btn_delete_disabled.Visible = False
                                    Me.ibtn_delete.Visible = True
                                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", Me.HiddenField1.Text)
                                    udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                                End If
                            Else
                                Me.hfSelectedIndex.Text = String.Empty
                                Me.panel_searchCriteria.Visible = False
                                Me.btn_delete_disabled.Visible = False
                                Me.ibtn_delete.Visible = True
                                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                                udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                            End If
                        Case DataDownloadFileID.EnrolmentDownload
                            Dim strvalue As String = String.Empty
                            udtcomfunct.getSystemParameter("EnrolmentDownloadStoragePath", strvalue, String.Empty)
                            strSrcFilePath = strvalue

                            'Proceed to download                    
                            If DownloadExcelFile(Trim(drSelectedRow("syspassword")), strSrcFilePath & strTempFolderPath & "\", Trim(drSelectedRow("SysEncryptedFileName")), Me.txt_accAct_newPW.Text, Trim(drSelectedRow("GenerationID"))) Then
                                If udcValidator.IsEmpty(Me.HiddenField1.Text) Then
                                    'UpdateFileDownloadedStatus()
                                    'Session(strSelectedIndex) = -1
                                    'Me.gvDataDownloadFolder.SelectedIndex = -1
                                    'Me.panel_searchCriteria.Visible = False
                                    'Me.btn_delete_disabled.Visible = False
                                    'Me.ibtn_delete.Visible = True
                                    'Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                                    'Me.udcInfoMessageBox.AddMessage("990000", "I", "00013", "%s", Session(strWindowWasherPath).ToString.Trim.Replace("?", "\"))
                                    'udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Download successful")
                                Else
                                    Me.hfSelectedIndex.Text = String.Empty
                                    Me.gvDataDownloadFolder.SelectedIndex = -1
                                    Me.panel_searchCriteria.Visible = False
                                    Me.btn_delete_disabled.Visible = False
                                    Me.ibtn_delete.Visible = True
                                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", Me.HiddenField1.Text)
                                    udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                                End If
                            Else
                                Me.hfSelectedIndex.Text = String.Empty
                                Me.panel_searchCriteria.Visible = False
                                Me.btn_delete_disabled.Visible = False
                                Me.ibtn_delete.Visible = True
                                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                                udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                            End If
                        Case Else
                            Select Case Trim(drSelectedRow("FileType"))
                                Case DataDownloadFileType.PDF
                                    Dim strvalue As String = String.Empty
                                    udtcomfunct.getSystemParameter("PdfReportStoragePath", strvalue, String.Empty)
                                    strSrcFilePath = strvalue

                                    'Proceed to download                    
                                    If DownloadPdfFile(strSrcFilePath & strTempFolderPath & "\", Trim(drSelectedRow("SysEncryptedFileName")), Me.txt_accAct_newPW.Text, Trim(drSelectedRow("GenerationID"))) Then
                                        If udcValidator.IsEmpty(Me.HiddenField1.Text) Then
                                            'UpdateFileDownloadedStatus()
                                            'Session(strSelectedIndex) = -1
                                            'Me.gvDataDownloadFolder.SelectedIndex = -1
                                            'Me.panel_searchCriteria.Visible = False
                                            'Me.btn_delete_disabled.Visible = False
                                            'Me.ibtn_delete.Visible = True
                                            'Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                                            'Me.udcInfoMessageBox.AddMessage("990000", "I", "00013", "%s", Session(strWindowWasherPath).ToString.Trim.Replace("?", "\"))
                                            'udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Download successful")
                                        Else
                                            Me.hfSelectedIndex.Text = String.Empty
                                            Me.panel_searchCriteria.Visible = False
                                            Me.btn_delete_disabled.Visible = False
                                            Me.ibtn_delete.Visible = True
                                            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                                            udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                                        End If
                                    Else
                                        Me.hfSelectedIndex.Text = String.Empty
                                        Me.panel_searchCriteria.Visible = False
                                        Me.btn_delete_disabled.Visible = False
                                        Me.ibtn_delete.Visible = True
                                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                                        udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                        udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                                    End If
                                    ' CRE13-016 - Upgrade to excel 2007 [Start][Tommy L]
                                    ' -----------------------------------------------------------------------------------------
                                    'Case DataDownloadFileType.Excel
                                Case DataDownloadFileType.XLS, DataDownloadFileType.XLSX
                                    ' CRE13-016 - Upgrade to excel 2007 [End][Tommy L]
                                    Dim strvalue As String = String.Empty
                                    udtcomfunct.getSystemParameter("SuperDownloadStoragePath", strvalue, String.Empty)
                                    strSrcFilePath = strvalue

                                    'Proceed to download                    
                                    If DownloadExcelFile(Trim(drSelectedRow("syspassword")), strSrcFilePath & strTempFolderPath & "\", Trim(drSelectedRow("SysEncryptedFileName")), Me.txt_accAct_newPW.Text, Trim(drSelectedRow("GenerationID"))) Then
                                        If udcValidator.IsEmpty(Me.HiddenField1.Text) Then
                                            'UpdateFileDownloadedStatus()
                                            'Session(strSelectedIndex) = -1
                                            'Me.gvDataDownloadFolder.SelectedIndex = -1
                                            'Me.panel_searchCriteria.Visible = False
                                            'Me.btn_delete_disabled.Visible = False
                                            'Me.ibtn_delete.Visible = True
                                            'Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                                            'Me.udcInfoMessageBox.AddMessage("990000", "I", "00013", "%s", Session(strWindowWasherPath).ToString.Trim.Replace("?", "\"))
                                            'udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Download successful")
                                        Else
                                            Me.hfSelectedIndex.Text = String.Empty
                                            Me.gvDataDownloadFolder.SelectedIndex = -1
                                            Me.panel_searchCriteria.Visible = False
                                            Me.btn_delete_disabled.Visible = False
                                            Me.ibtn_delete.Visible = True
                                            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", Me.HiddenField1.Text)
                                            udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                                        End If
                                    Else
                                        Me.hfSelectedIndex.Text = String.Empty
                                        Me.panel_searchCriteria.Visible = False
                                        Me.btn_delete_disabled.Visible = False
                                        Me.ibtn_delete.Visible = True
                                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                                        udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                        udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                                    End If
                                Case DataDownloadFileType.Text
                                    Dim strvalue As String = String.Empty
                                    udtcomfunct.getSystemParameter("BankFileStoragePath", strvalue, String.Empty)
                                    strSrcFilePath = strvalue

                                    'Proceed to download                    
                                    If DownloadTxtFile(strSrcFilePath & strTempFolderPath & "\", Trim(drSelectedRow("SysEncryptedFileName")), Me.txt_accAct_newPW.Text, Trim(drSelectedRow("GenerationID"))) Then    'Trim(dt.Rows(Session(strSelectedIndex))("syspassword")),
                                        If udcValidator.IsEmpty(Me.HiddenField1.Text) Then
                                            'UpdateFileDownloadedStatus()
                                            'Session(strSelectedIndex) = -1
                                            'Me.gvDataDownloadFolder.SelectedIndex = -1
                                            'Me.panel_searchCriteria.Visible = False
                                            'Me.btn_delete_disabled.Visible = False
                                            'Me.ibtn_delete.Visible = True
                                            'Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                                            'Me.udcInfoMessageBox.AddMessage("990000", "I", "00013", "%s", Session(strWindowWasherPath).ToString.Trim.Replace("?", "\"))
                                            'udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Download successful")
                                        Else
                                            Me.hfSelectedIndex.Text = String.Empty
                                            Me.panel_searchCriteria.Visible = False
                                            Me.btn_delete_disabled.Visible = False
                                            Me.ibtn_delete.Visible = True
                                            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                                            udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                                        End If
                                    Else
                                        Me.hfSelectedIndex.Text = String.Empty
                                        Me.panel_searchCriteria.Visible = False
                                        Me.btn_delete_disabled.Visible = False
                                        Me.ibtn_delete.Visible = True
                                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                                        udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                        udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                                    End If
                                Case Else
                                    Dim strvalue As String = String.Empty
                                    udtcomfunct.getSystemParameter("BankFileStoragePath", strvalue, String.Empty)
                                    strSrcFilePath = strvalue

                                    'Proceed to download                    
                                    If DownloadTxtFile(strSrcFilePath & strTempFolderPath & "\", Trim(drSelectedRow("SysEncryptedFileName")), Me.txt_accAct_newPW.Text, Trim(drSelectedRow("GenerationID"))) Then    'Trim(dt.Rows(Session(strSelectedIndex))("syspassword")),
                                        If udcValidator.IsEmpty(Me.HiddenField1.Text) Then
                                            'UpdateFileDownloadedStatus()
                                            'Session(strSelectedIndex) = -1
                                            'Me.gvDataDownloadFolder.SelectedIndex = -1
                                            'Me.panel_searchCriteria.Visible = False
                                            'Me.btn_delete_disabled.Visible = False
                                            'Me.ibtn_delete.Visible = True
                                            'Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                                            'Me.udcInfoMessageBox.AddMessage("990000", "I", "00013", "%s", Session(strWindowWasherPath).ToString.Trim.Replace("?", "\"))
                                            'udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Download successful")
                                        Else
                                            Me.hfSelectedIndex.Text = String.Empty
                                            Me.panel_searchCriteria.Visible = False
                                            Me.btn_delete_disabled.Visible = False
                                            Me.ibtn_delete.Visible = True
                                            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                                            udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                                        End If
                                    Else
                                        Me.hfSelectedIndex.Text = String.Empty
                                        Me.panel_searchCriteria.Visible = False
                                        Me.btn_delete_disabled.Visible = False
                                        Me.ibtn_delete.Visible = True
                                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
                                        udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                                        udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                                    End If
                            End Select
                    End Select
                Else
                    udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                    Me.udcErrorMessage.AddMessage(COMMON_FUNCTION_CODE, "E", "00057")
                End If
            Else
                Me.hfSelectedIndex.Text = String.Empty
                Me.panel_searchCriteria.Visible = False
                Me.btn_delete_disabled.Visible = False
                Me.ibtn_delete.Visible = True
                udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
                Me.udcErrorMessage.AddMessage(COMMON_FUNCTION_CODE, "E", "00043")
            End If

            udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
            Me.udcErrorMessage.BuildMessageBox("DownloadFail", udtAuditLogEntry, LogID.LOG00009, "Download Fail" & Session("PathError"))
            Me.udcInfoMessageBox.BuildMessageBox()
        Catch ex As Exception
            Me.hfSelectedIndex.Text = String.Empty
            Me.panel_searchCriteria.Visible = False
            Me.btn_delete_disabled.Visible = False
            Me.ibtn_delete.Visible = True
            udtAuditLogEntry.AddDescripton("DownloadFailException", ex.ToString)
            udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
            ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            udtAuditLogEntry.AddDescripton("Generation_ID", strGenerationID)
            Me.udcErrorMessage.BuildMessageBox("DownloadFail", udtAuditLogEntry, LogID.LOG00009, "Download Fail" & Session("PathError"))
            Me.udcInfoMessageBox.BuildMessageBox()
            ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
        End Try
        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [End][Chris YIM]

    End Sub

    Private Sub UpdateFileDownloadedStatus()
        'Update the FileDownloadStatus
        dt = Session(strDownloadList)
        Dim alSelectedIndex As New ArrayList

        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim drDownloadList() As DataRow
        Dim drSelectedRow As DataRow
        drDownloadList = dt.Select("GenerationID = '" + Me.hfSelectedIndex.Text.Trim + "'")

        If drDownloadList.Length <> 1 Then
            Throw New Exception(String.Format("Concurrent Download Error: No available result is found by Generation ID {0}.", Me.hfSelectedIndex.Text.Trim))
        End If

        drSelectedRow = drDownloadList(0)

        alSelectedIndex.Add(drSelectedRow("lineNum") - 1)
        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [End][Chris YIM]

        Me.udcDataDownloadBll.UpdateMultipleFileDownloadStatus(dt, alSelectedIndex, FileDownloadStatus.Downloaded)
        'CRE14-00XX - Retain sorting other and page index after download report  [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        LoadEnqDownloadGrid(IIf(Me.rbMyFolder.Checked, FileDownloadStatus.NotDownloadYet, FileDownloadStatus.Deleted), False)
        'CRE14-00XX - Retain sorting other and page index after download report  [End][Chris YIM]
    End Sub

    Private Function DownloadTxtFile(ByVal strFullFilePath As String, ByVal strFileName As String, ByVal strUserPassword As String, ByVal strGenerationID As String) As Boolean 'ByVal strSystemPassword As String, 
        Dim bResult As Boolean = False
        Dim strPathPart As String
        Dim strZipFilename As String
        Dim strTxtFilename As String
        Dim intFileLength As Integer

        If Not System.IO.Directory.Exists(strFullFilePath) Then
            System.IO.Directory.CreateDirectory(strFullFilePath)
        End If

        strPathPart = strFullFilePath
        'strZipFilename = strFileName
        'strTxtFilename = strZipFilename.ToUpper.Replace(".ZIP", ".txt")
        strTxtFilename = strFileName
        strZipFilename = strTxtFilename & ".exe" 'strTxtFilename.ToUpper.Replace(".TXT", ".exe")

        Try
            'Get the file from Db and write to physical file
            If SaveDBByteToFile(strGenerationID, strFullFilePath & strFileName, intFileLength) Then

                'Remark by Clark start <<
                'udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

                'Dim file As IO.FileInfo = New IO.FileInfo(strFullFilePath & strFileName)
                'Do While Not file.Length = intFileLength
                '    'System.IO.File.WriteAllText("C:\Temp\WriteFileLog.txt", "FileLength:" & file.Length.ToString)
                'Loop
                ''strOriFilename = Encrypt.Decrypt7zFileWithPassword(strSystemPassword, strSrcFileName, strSrcFileTempPath)
                'If Encrypt.DecryptWinzipWithPassword(strSystemPassword, strPathPart, strTxtFilename, strZipFilename) Then

                '    'Delete the zip file with system password encrypted
                '    Do While Not bDeleteFileOK(strPathPart & strZipFilename)
                '        System.IO.File.WriteAllText("C:\Temp\TempLog0.txt", "Wait for Decrypt=" & Now)
                '        System.Threading.Thread.Sleep(1000)
                '    Loop

                '    System.IO.File.WriteAllText("C:\Temp\TempLog-.txt", "After DeleteFile=" & Now)

                '    'Encrypt.Encrypt7zFileWithPassword(strUserPassword, strOriFilename, strOutputFileName)

                If Encrypt.EncryptWinRAR(strUserPassword, strPathPart, strTxtFilename) Then
                    'End If
                    'If Encrypt.EncryptWinzipWithPassword(strUserPassword, strPathPart, strTxtFilename, strZipFilename) Then
                    'Delete the text file
                    Do While Not bDeleteFileOK(strPathPart & strTxtFilename)
                        System.Threading.Thread.Sleep(1000)
                    Loop

                    Do While Not System.IO.File.Exists(strPathPart & strZipFilename)
                        System.Threading.Thread.Sleep(1000)
                    Loop

                    'udtHCVUUser = udtHCVUUserBLL.GetHCVUUser
                    'Call OCX object
                    CallOCX(strPathPart & "\" & strZipFilename, strZipFilename)

                    bResult = True
                Else
                    bResult = False
                End If
                'Else
                '    bResult = False
                '    System.IO.File.WriteAllText("C:\Temp\TempLog2.txt", HttpContext.Current.Session("ErrorCode"))
                'End If
                'Remark by Clark end >>
                'Call OCX object
                'CallOCX(strPathPart & "\" & strZipFilename, strZipFilename)
                'bResult = True
            Else
                bResult = False
            End If

            Return bResult
        Catch ex As Exception
            bResult = False
            Throw ex
        End Try
    End Function

    Private Function DownloadExcelFile(ByVal strSystemPassword As String, ByVal strSrcFilePath As String, ByVal strSrcFileName As String, ByVal strUserPassword As String, ByVal strGenerationID As String) As Boolean
        Dim strFilename As String
        Dim bResult As Boolean = False
        Dim intFileLength As Integer

        Try
            strFilename = strSrcFileName

            If Not System.IO.Directory.Exists(strSrcFilePath) Then
                System.IO.Directory.CreateDirectory(strSrcFilePath)
            End If

            strSrcFileName = strSrcFilePath & strSrcFileName

            'Get the file from Db and write to physical file
            If SaveDBByteToFile(strGenerationID, strSrcFileName, intFileLength) Then
                udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

                Dim file As IO.FileInfo = New IO.FileInfo(strSrcFileName)
                Do While Not file.Length = intFileLength

                Loop

                ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'If Encrypt.DecryptExcel(strSystemPassword, strSrcFileName) Then
                If Encrypt.Excel_ChangePassword(strSystemPassword, strUserPassword, strSrcFileName) Then
                    'Encrypt.EncryptExcel(strUserPassword, strSrcFileName)
                    ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]

                    'Call OCX object to download the file in strSrcFileTempPath
                    CallOCX(strSrcFileName, strFilename)
                    Me.HiddenField1.Text = ""
                    bResult = True
                Else
                    ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'bResult = False
                    Throw New Exception("Error: Class = [HCVU.Datadownload], Method = [DownloadExcelFile], Message = Method - [Excel_ChangePassword] return [False]")
                    ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
                End If
            Else
                ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'bResult = False
                Throw New Exception("Error: Class = [HCVU.Datadownload], Method = [DownloadExcelFile], Message = Method - [SaveDBByteToFile] return [False]")
                ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
            End If

            Return bResult
        Catch ex As Exception
            bResult = False
            Throw ex
        End Try
    End Function

    Private Function DownloadPdfFile(ByVal strFullFilePath As String, ByVal strFileName As String, ByVal strUserPassword As String, ByVal strGenerationID As String) As Boolean
        Dim bResult As Boolean = False
        Dim strPathPart As String
        Dim intFileLength As Integer

        If Not System.IO.Directory.Exists(strFullFilePath) Then
            System.IO.Directory.CreateDirectory(strFullFilePath)
        End If

        strPathPart = strFullFilePath

        Try
            'Get the file from Db and write to physical file
            If SaveDBByteToFile(strGenerationID, strFullFilePath & strFileName, intFileLength) Then
                If Encrypt.EncryptPDF(strUserPassword, strFullFilePath, strFileName) Then
                    'Call OCX object
                    CallOCX(strPathPart & "\" & strFileName, strFileName)

                    bResult = True
                Else
                    bResult = False
                End If
            Else
                bResult = False
            End If

            Return bResult
        Catch ex As Exception
            bResult = False
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Copy the system encrypted file to the temp folder
    ''' </summary>
    ''' <param name="strSrcFileName"></param>
    ''' <param name="strDestPath">Consists of the temp path from system parameter + userid</param>
    ''' <param name="strDestFileName"></param>
    ''' <remarks></remarks>
    Private Function CopyFileToTempFolder(ByVal strSrcFileName As String, ByVal strDestPath As String, ByVal strDestFileName As String) As String
        Try
            If Not System.IO.Directory.Exists(strDestPath) Then
                System.IO.Directory.CreateDirectory(strDestPath)
            End If
            System.IO.File.Move(strSrcFileName, strDestPath & "\" & strDestFileName)
            'System.IO.File.Copy(strSrcFileName, strDestPath & "\" & strDestFileName, True)
            'System.IO.File.Delete(strSrcFileName)
            Return ""
        Catch ex As Exception
            Return "00003"
        End Try

    End Function

    Protected Sub ibtn_delete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.panel_searchCriteria.Visible = False
        Me.btn_delete_disabled.Visible = False
        Me.ibtn_delete.Visible = True

        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Delete click")

        If IsSelectNothing() Then
            Me.udcErrorMessage.AddMessage("990000", "E", "00023")
            Me.MultiView1.ActiveViewIndex = 0
            udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Delete click fail")
        Else
            udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Delete click successful")
            Me.ModalPopupExtenderConfirmDelete.Show()
        End If
        Me.udcInfoMessageBox.BuildMessageBox()
        Me.udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00012, "Delete File Fail")
    End Sub

    Private Function IsSelectNothing() As Boolean

        Dim cb As CheckBox

        For Each row As GridViewRow In Me.gvDataDownloadFolder.Rows
            cb = CType(row.Cells(0).FindControl("chk_selected"), CheckBox)
            If cb.Checked = True Then
                Return False
            End If
        Next

        Return True
    End Function

    ' CRE15-016 (Randomly genereate the valid claim transaction) [Start][Winnie]
    Private Function IsAllowDelete(ByVal dt As DataTable, ByVal alSelectedIndex As ArrayList) As Boolean
        Dim dr As DataRow
        Dim i As Integer
        Dim blnAllowDelete As Boolean = True
        Dim udtFileGenerationBLL As New Common.Component.FileGeneration.FileGenerationBLL
        Dim intReportCount As Integer = 0 ' No. of PPC report add back to pending gen queue list

        Try

            For i = 0 To alSelectedIndex.Count - 1
                dr = CType(dt.Rows(alSelectedIndex(i)), DataRow)

                If dr("status").Equals(DataDownloadStatus.Completed) Then
                    Continue For
                End If

                Dim udtFileGenerationModel As FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(Trim(dr.Item("reportNum")))

                ' Involve Post Payment Report
                If udtFileGenerationModel.ShowForGeneration.Equals(SubmissionReportType.PostPaymentCheck) Then
                    Dim dtDownloadList As DataTable = udcDataDownloadBll.GetDownloadListByGenID(Trim(dr.Item("GenerationID")), FileDownloadStatus.NotDownloadYet)

                    ' No user is waiting
                    If dtDownloadList.Rows.Count = 0 Then
                        intReportCount += 1
                    End If
                End If
            Next

            ' Check if total post payment report pending for generation is exceed limit
            If intReportCount > 0 Then
                Dim intPendingGenCount As Integer = 0
                Dim strPPCGenLimit As String = String.Empty
                Dim intPPCGenLimit As Integer = 0

                udtcomfunct.getSystemParameter("PPCReport_PendingGenLimit", strPPCGenLimit, String.Empty)

                If Integer.TryParse(strPPCGenLimit, intPPCGenLimit) AndAlso intPPCGenLimit > 0 Then

                    intPendingGenCount = udtFileGenerationBLL.GetFileGenerationQueueToRun_PPCCount()

                    If intReportCount + intPendingGenCount > intPPCGenLimit Then
                        blnAllowDelete = False
                    End If
                End If
            End If

            Return blnAllowDelete

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    ' CRE15-016 (Randomly genereate the valid claim transaction) [End][Winnie]

    Protected Sub rbMyFolder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.panel_searchCriteria.Visible = False
        Me.btn_delete_disabled.Visible = False
        Me.ibtn_delete.Visible = True
        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.hfSelectedIndex.Text = String.Empty
        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [End][Chris YIM]
        'CRE14-00XX - Retain sorting other and page index after download report  [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        LoadEnqDownloadGrid(IIf(Me.rbMyFolder.Checked, FileDownloadStatus.NotDownloadYet, FileDownloadStatus.Deleted), True)
        'CRE14-00XX - Retain sorting other and page index after download report  [End][Chris YIM]
        Me.udcErrorMessage.BuildMessageBox()
        Me.ibtn_delete.Visible = True
        Me.lbl_RecycleBinNote.Visible = False
        Me.lbl_KeepFilePeriodNote.Visible = Not Me.lbl_RecycleBinNote.Visible
        Me.ibtn_undelete.Visible = False
        Me.gvDataDownloadFolder.SelectedIndex = -1
    End Sub

    Protected Sub rbRecycleBin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.panel_searchCriteria.Visible = False
        Me.btn_delete_disabled.Visible = False
        Me.ibtn_delete.Visible = True
        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.hfSelectedIndex.Text = String.Empty
        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [End][Chris YIM]
        'CRE14-00XX - Retain sorting other and page index after download report  [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        LoadEnqDownloadGrid(IIf(Me.rbMyFolder.Checked, FileDownloadStatus.NotDownloadYet, FileDownloadStatus.Deleted), True)
        'CRE14-00XX - Retain sorting other and page index after download report  [End][Chris YIM]
        Me.udcErrorMessage.BuildMessageBox()
        Me.ibtn_delete.Visible = False
        Me.lbl_RecycleBinNote.Visible = True
        Me.lbl_KeepFilePeriodNote.Visible = Not Me.lbl_RecycleBinNote.Visible
        Me.ibtn_undelete.Visible = True
        Me.gvDataDownloadFolder.SelectedIndex = -1
    End Sub

    Protected Sub ibtnDialogCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00025, "Delete Cancel Click")
        ' CRE11-021 log the missed essential information [End]

        Me.ModalPopupExtenderConfirmDelete.Hide()
    End Sub

    Protected Sub btn_confirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim selectedRow As New ArrayList

        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "Confirm delete File Start")

        Try
            dt = Session(strDownloadList)
            Me.GetAllSelectedRowIndex(selectedRow)
            Me.udcDataDownloadBll.UpdateMultipleFileDownloadStatus(dt, selectedRow, FileDownloadStatus.Deleted)
            Me.panel_searchCriteria.Visible = False
            Me.btn_delete_disabled.Visible = False
            Me.ibtn_delete.Visible = True
            'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me.hfSelectedIndex.Text = String.Empty
            'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [End][Chris YIM]
            'CRE14-00XX - Retain sorting other and page index after download report  [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            LoadEnqDownloadGrid(IIf(Me.rbMyFolder.Checked, FileDownloadStatus.NotDownloadYet, FileDownloadStatus.Deleted), False)
            'CRE14-00XX - Retain sorting other and page index after download report  [End][Chris YIM]
            Me.udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00018, "Confirm delete File fail")
            udtAuditLogEntry.WriteEndLog(LogID.LOG00017, "Confirm delete File successful")
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Confirm delete File fail")
            Throw ex
        End Try

    End Sub

    Private Sub GetAllSelectedRowIndex(ByRef selectedRow As ArrayList)

        Dim cb As CheckBox
        Dim i As Integer

        i = 0
        For Each row As GridViewRow In Me.gvDataDownloadFolder.Rows
            cb = CType(row.Cells(1).FindControl("chk_selected"), CheckBox)
            If cb.Checked = True Then
                selectedRow.Add((Me.gvDataDownloadFolder.PageIndex * Me.gvDataDownloadFolder.PageSize) + i)
            End If
            i = i + 1
        Next
    End Sub

    Protected Sub ibtn_undelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.panel_searchCriteria.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00013, "Undelete File Start")

        If IsSelectNothing() Then
            Me.udcErrorMessage.AddMessage("990000", "E", "00023")
            Me.MultiView1.ActiveViewIndex = 0
        Else
            Dim selectedRow As New ArrayList

            Try
                dt = Session(strDownloadList)
                Me.GetAllSelectedRowIndex(selectedRow)

                ' CRE15-016 (Randomly genereate the valid claim transaction) [Start][Winnie]
                If IsAllowDelete(dt, selectedRow) Then
                    Me.udcDataDownloadBll.UpdateMultipleFileDownloadStatus(dt, selectedRow, FileDownloadStatus.NotDownloadYet)

                    Me.panel_searchCriteria.Visible = False
                    'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Me.hfSelectedIndex.Text = String.Empty
                    'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [End][Chris YIM]
                    'CRE14-00XX - Retain sorting other and page index after download report  [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    LoadEnqDownloadGrid(IIf(Me.rbMyFolder.Checked, FileDownloadStatus.NotDownloadYet, FileDownloadStatus.Deleted), False)
                    'CRE14-00XX - Retain sorting other and page index after download report  [End][Chris YIM]
                    Me.udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00015, "Undelete File fail")
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Undelete File Successful")

                Else
                    Me.udcErrorMessage.AddMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005)
                    Me.MultiView1.ActiveViewIndex = 0
                End If
                ' CRE15-016 (Randomly genereate the valid claim transaction) [End][Winnie]


            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Undelete File fail")
                Throw ex
            End Try
        End If
        Me.udcInfoMessageBox.BuildMessageBox()
        Me.udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00015, "Undelete File fail")
    End Sub

    Protected Sub gvDataDownloadFolder_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, strDownloadList)
    End Sub

    Protected Sub gvDataDownloadFolder_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.gvDataDownloadFolder.SelectedIndex = -1
        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.btn_delete_disabled.Visible = False
        Me.ibtn_delete.Visible = True
        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [End][Chris YIM]

        Me.GridViewPageIndexChangingHandler(sender, e, strDownloadList)
    End Sub

    Protected Sub gvDataDownloadFolder_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.gvDataDownloadFolder.SelectedIndex = -1
        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Me.btn_delete_disabled.Visible = False
        Me.ibtn_delete.Visible = True
        'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [End][Chris YIM]

        Me.GridViewSortingHandler(sender, e, strDownloadList)
    End Sub

    Private Sub CallOCX(ByVal strSrcFile As String, ByVal strOutputFileName As String)
        'Dim strvalue As String = String.Empty
        'udtcomfunct.getSystemParameter("DownloadFileWorkerPath", strvalue, String.Empty)

        'I-CRE16-002 (Fix Path Traversal) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'strSrcFile = strSrcFile.Replace("\", "|")
        'I-CRE16-002 (Fix Path Traversal) [End][Chris YIM]

        Dim url As String

        Dim strhttp As String = String.Empty
        Dim strapppath As String = String.Empty
        Dim strvalue As String = String.Empty
        udtcomfunct.getSystemParameter("SiteHttp", strvalue, String.Empty)
        strhttp = strvalue

        strvalue = String.Empty
        udtcomfunct.getSystemParameter("OCXAppPath", strvalue, String.Empty)
        strapppath = strvalue

        'I-CRE16-002 (Fix Path Traversal) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim dictTSPath As Dictionary(Of String, String)

        If Session(SESS.DictionaryTimestampPath) Is Nothing Then
            dictTSPath = New Dictionary(Of String, String)
        Else
            dictTSPath = Session(SESS.DictionaryTimestampPath)
        End If

        Dim lngTimeStamp As Long
        Dim strTimeStamp As String
        lngTimeStamp = DateTime.Now.Subtract(New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)).TotalMilliseconds
        strTimeStamp = lngTimeStamp.ToString

        dictTSPath.Add(strTimeStamp, strSrcFile)
        Session(SESS.DictionaryTimestampPath) = dictTSPath

        ''url = strvalue & "?FileDownloadPath=" & strSrcFile
        'url = strhttp & "://" & Request.Url.Host & strapppath & "ReportAndDownload/DownloadFileWorker.aspx?FileDownloadPath=" & strSrcFile
        url = strhttp & "://" & Request.Url.Host & strapppath & "ReportAndDownload/DownloadFileWorker.aspx?" & QueryString.TimeStamp & "=" & strTimeStamp
        'I-CRE16-002 (Fix Path Traversal) [End][Chris YIM]

        'strSrcFile = strSrcFile.Replace("\\", "|")
        'strSrcFile = Request.Url.ToString        

        ScriptManager.RegisterStartupScript(Me, GetType(Page), "callocxscript", "setTimeout(" + Chr(34) + "DownloadFile('" & url & "', '" & strOutputFileName & "', '" & Session(strWindowWasherPath).ToString & "')" + Chr(34) + ", 1)", True)
    End Sub

    Private Function SaveDBByteToFile(ByVal strGenerationID As String, ByVal strOutputFilePath As String, ByRef intFileLength As Integer) As Boolean
        Dim udcFileQueueModel As New FileGeneration.FileGenerationQueueModel()
        Dim udcFileQueueBll As New FileGeneration.FileGenerationBLL

        udcFileQueueModel = udcFileQueueBll.GetFileContent(strGenerationID)

        Try
            System.IO.File.WriteAllBytes(strOutputFilePath, udcFileQueueModel.FileContent)
            intFileLength = udcFileQueueModel.FileContent.Length
            Session("PathError") = String.Empty
            Return True
        Catch ex As Exception
            'add log
            'Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            'udtAuditLogEntry.AddDescripton("Err", ex.Message)
            'udtAuditLogEntry.WriteLog("99999", "Download Fail With exception")
            Session("PathError") = ": Path=" & strOutputFilePath & " " & ex.ToString()
            Return False
        End Try
    End Function

    Protected Sub btn_back_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.panel_searchCriteria.Visible = False
        Me.btn_delete_disabled.Visible = False
        Me.ibtn_delete.Visible = True
        Me.gvDataDownloadFolder.SelectedIndex = -1
        Me.udcErrorMessage.BuildMessageBox()
    End Sub

    Private Function bDeleteFileOK(ByVal strFilePath As String) As Boolean
        Try
            'Delete the zip file with system password encrypted
            If System.IO.File.Exists(strFilePath) Then
                System.IO.File.Delete(strFilePath)
            End If
            If Not System.IO.File.Exists(strFilePath) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try

    End Function

    Protected Sub btn_HiddenSuccessBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HiddenSuccessBtn.Click
        ' CRE11-004    
        'Dim udtAuditLogEntry As AuditLogEntry = Session("DatadownloadAuditLogObj")
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(FUNCTION_CODE, Me)
        If udcValidator.IsEmpty(Me.HiddenField1.Text) Then
            UpdateFileDownloadedStatus()
            'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me.hfSelectedIndex.Text = String.Empty
            'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [End][Chris YIM]

            Me.gvDataDownloadFolder.SelectedIndex = -1
            Me.panel_searchCriteria.Visible = False
            Me.btn_delete_disabled.Visible = False
            Me.ibtn_delete.Visible = True
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            Me.udcInfoMessageBox.AddMessage("990000", "I", "00013", "%s", Session(strWindowWasherPath).ToString.Trim.Replace("?", "\"))
            udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Download successful")
            Me.udcInfoMessageBox.BuildMessageBox()
        Else
            'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me.hfSelectedIndex.Text = String.Empty
            'INT16-0012 (Fix problem of concurrent download in HCVU Download Report and Datafile) [End][Chris YIM]
            Me.gvDataDownloadFolder.SelectedIndex = -1
            Me.panel_searchCriteria.Visible = False
            Me.btn_delete_disabled.Visible = False
            Me.ibtn_delete.Visible = True
            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", Me.HiddenField1.Text)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Download fail")
            Me.udcErrorMessage.BuildMessageBox("DownloadFail", udtAuditLogEntry, LogID.LOG00009, "Download Fail Code " & Me.HiddenField1.Text.Trim)
            Me.HiddenField1.Text = ""
        End If
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