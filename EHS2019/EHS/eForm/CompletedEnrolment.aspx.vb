Imports Common.ComObject
Imports Common.Component
Imports Common.ComFunction
Imports Common.Component.ServiceProvider
Imports Common.Component.MedicalOrganization
Imports Common.Component.Practice
Imports Common.Component.ERNProcessed

Partial Public Class CompletedEnrolment
    'Inherits System.Web.UI.Page
    Inherits BasePage

    Private udtEFormBLL As eFormBLL = New eFormBLL
    Private Formatter As Common.Format.Formatter = New Common.Format.Formatter
    Private udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
    Private udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
    Private udtERNProcessedBLL As ERNProcessedBLL = New ERNProcessedBLL

    Private Const LocalFunctionCode As String = FunctCode.FUNT020101
    Private Const GlobalFunctionCode As String = FunctCode.FUNT990000
    Private Const DatabaseFunctionCode As String = FunctCode.FUNT990001

    Private strLanguage As String = String.Empty

    Private Const SESS_PrintOut As String = "PrintOutTable"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'CRE13-032 End of Support of XP and IE6 [Start][Karl]
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")
        'CRE13-032 End of Support of XP and IE6 [End][Karl]

        If IsNothing(Session("EnrolmentRefNo")) Then
            Response.Redirect("~/main.aspx")
        End If

        strLanguage = LCase(Session("language"))

        Dim strERN As New List(Of String)
        strERN = Session("EnrolmentRefNo")

        ' CRE12-001 eHS and PCD integration [Start][Tommy]

        Dim strPCD_ERN As String = Nothing
        If Not IsNothing(Session("PCD_ERN")) Then
            strPCD_ERN = Session("PCD_ERN")
        End If

        Dim strPCD_SubmissionTime As String = Nothing
        If Not IsNothing(Session("PCD_SubmissionTime")) Then
            strPCD_SubmissionTime = Session("PCD_SubmissionTime")
        End If

        Dim udtPCDSPModel As ServiceProviderModel = Nothing
        If Not IsNothing(Session("SP")) Then
            udtPCDSPModel = Session("SP")
        End If

        Dim blnIsJoinPCD As Boolean = False
        If Not IsNothing(Session("IsJoinPCD")) Then
            blnIsJoinPCD = Session("IsJoinPCD")
        End If

        'lblPCDERN.Text = strPCD_ERN

        If udtSPBLL.Exist Then
            Dim udtSP As ServiceProviderModel
            udtSP = udtSPBLL.GetSP

            Dim dtSP As DataTable = New DataTable
            dtSP.Columns.Add(New DataColumn("PCDERN"))

            Dim dr As DataRow
            dr = dtSP.NewRow
            dr(0) = strPCD_ERN
            dtSP.Rows.Add(dr)

            Me.gvPCDPrintOut.DataSource = dtSP
            Me.gvPCDPrintOut.DataBind()
            Me.pnlPCD.Visible = True

        End If

        ' CRE12-001 eHS and PCD integration [End][Tommy]

        If Not IsPostBack Then
            udtSPBLL.ClearSession()
            Session.RemoveAll()
            Session("language") = strLanguage
            Session("EnrolmentRefNo") = strERN

            ' CRE12-001 eHS and PCD integration [Start][Tommy]

            Session("PCD_ERN") = strPCD_ERN
            Session("PCD_SubmissionTime") = strPCD_SubmissionTime
            Session("SP") = udtPCDSPModel
            Session("IsJoinPCD") = blnIsJoinPCD

            ' CRE12-001 eHS and PCD integration [End][Tommy]

        End If

        Dim lnkBtnContactUs As LinkButton = New LinkButton
        lnkBtnContactUs = Master.FindControl("lnkBtnContactUs")
        lnkBtnContactUs.Visible = False

        Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "reconn key", KeepAlive())

        ibtnCompleteClose.OnClientClick = "showConfirm(this); return false;"

        Dim SM As Common.ComObject.SystemMessage
        SM = New Common.ComObject.SystemMessage(LocalFunctionCode, "Q", Common.Component.MsgCode.MSG00001)

        Me.lblMsg.Text = SM.GetMessage

        If Not IsNothing(strERN) Then
            Try
                If udtEFormBLL.GetServiceProviderProfile(strERN(0)) Then
                    If udtSPBLL.Exist Then
                        Dim udtSP As ServiceProviderModel
                        udtSP = udtSPBLL.GetSP
                        lblCompleteSubmissionDate.Text = Formatter.convertDateTime(udtSP.EnrolDate)

                        Dim dtPrintOut As DataTable

                        dtPrintOut = udtEFormBLL.PrintOutDataTableByERNList(strERN)

                        Session(SESS_PrintOut) = dtPrintOut
                        Me.gvPrintOut.DataSource = dtPrintOut
                        Me.gvPrintOut.DataBind()

                        ' CRE12-001 eHS and PCD integration [Start][Tommy]

                        pnlPCD.Visible = False
                        If blnIsJoinPCD Then
                            If Not IsNothing(strPCD_ERN) And Not strPCD_ERN = String.Empty Then
                                pnlPCD.Visible = True
                            End If
                        End If

                        ' CRE12-001 eHS and PCD integration [End][Tommy]

                        If udtSP.AlreadyJoinEHR = JoinEHRSSStatus.Yes Then
                            Me.gvPrintOut.Columns(4).Visible = True
                        Else
                            Me.gvPrintOut.Columns(4).Visible = False
                        End If

                    End If

                End If


            Catch ex As Exception
                Throw ex
            End Try

        End If

        InfoMessageBox1.AddMessage(LocalFunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
        InfoMessageBox1.BuildMessageBox()
        InfoMessageBox1.Type = CustomControls.InfoMessageBoxType.Complete


    End Sub

    Protected Function formatChineseString(ByVal strChineseString) As String
        Return Formatter.formatChineseName(strChineseString)
    End Function

    Protected Sub ibtnCompleteClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        'CRE13-032 End of Support of XP and IE6 [Start][Karl]
        'Session.RemoveAll()
        Me.Response.Redirect("~/thankyou.aspx")
        'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "javascript:window.opener = top;window.close();", True)
        'CRE13-032 End of Support of XP and IE6 [End][Karl]
    End Sub

    Private Sub gvPrintOut_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPrintOut.RowCommand
        Dim udtSp As ServiceProviderModel = Nothing
        Dim udtMO As MedicalOrganizationModelCollection = Nothing

        If udtSPBLL.Exist Then
            udtSp = udtSPBLL.GetSP

            If TypeOf e.CommandSource Is LinkButton Then
                If IsNothing(e.CommandArgument) OrElse e.CommandArgument.ToString.Equals(String.Empty) Then
                Else
                    Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

                    Dim strCommand() As String = e.CommandArgument.ToString.Split("|")
                    Dim strERN As String = strCommand(0)
                    Dim strReportType As String = strCommand(1)
                    Dim strReportLanguage As String = strCommand(2)

                    Session(ReportFunction.SessionName.udtServiceProvider) = Nothing

                    Dim udtTSP As New ServiceProviderModel
                    udtTSP = udtSPBLL.GetServiceProviderEnrolmentProfileByERN(strERN, udtEFormBLL.DB)

                    Session(ReportFunction.SessionName.udtServiceProvider) = udtSPBLL.GetServiceProviderEnrolmentProfileByERN(strERN, udtEFormBLL.DB)

                    Select Case strReportLanguage
                        Case "E"
                            Select Case strReportType
                                Case "App"
                                    udtAuditLogEntry.AddDescripton("ERN", strERN)
                                    udtAuditLogEntry.AddDescripton("Language", "English")
                                    udtAuditLogEntry.WriteLog(LogID.LOG00060, "Print Application Form")
                                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "EnrolmentFormScript", "javascript:openNewWin('PrintOut/DH_VSS/DH_VSS_RV.aspx');", True)
                                Case "EHRSS"
                                    udtAuditLogEntry.AddDescripton("ERN", strERN)
                                    udtAuditLogEntry.AddDescripton("Language", "English")
                                    udtAuditLogEntry.WriteLog(LogID.LOG00061, "Print Token Sharing Consent Form")

                                    Session(ReportFunction.SessionName.strApplicatName) = udtTSP.EnglishName

                                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "EnrolmentFormScript", "javascript:openNewWin('PrintOut/TokenSharingConsent/TokenSharingConsent_RV.aspx');", True)
                            End Select
                        Case "C"
                            Select Case strReportType
                                Case "App"
                                    udtAuditLogEntry.AddDescripton("ERN", strERN)
                                    udtAuditLogEntry.AddDescripton("Language", "Chinese")
                                    udtAuditLogEntry.WriteLog(LogID.LOG00060, "Print Application Form")
                                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "EnrolmentFormScript", "javascript:openNewWin('PrintOut/DH_VSS_CHI/DH_VSS_CHI_RV.aspx');", True)
                                Case "EHRSS"
                                    udtAuditLogEntry.AddDescripton("ERN", strERN)
                                    udtAuditLogEntry.AddDescripton("Language", "Chinese")
                                    udtAuditLogEntry.WriteLog(LogID.LOG00061, "Print Token Sharing Consent Form")
                                    If IsNothing(udtTSP.ChineseName) OrElse udtTSP.ChineseName.Equals(String.Empty) Then
                                        Session(ReportFunction.SessionName.strApplicatName) = udtTSP.EnglishName
                                    Else
                                        Session(ReportFunction.SessionName.strApplicatName) = udtTSP.ChineseName
                                    End If
                                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "EnrolmentFormScript", "javascript:openNewWin('PrintOut/TokenSharingConsent_CHI/TokenSharingConsent_CHI_RV.aspx');", True)
                            End Select
                    End Select

                End If

            End If

        End If

    End Sub

    Private Sub gvPrintOut_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPrintOut.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lblERNIndex As Label = e.Row.FindControl("lblERNIndex")

            Dim dt As DataTable
            dt = Session(SESS_PrintOut)

            If e.Row.RowIndex = 0 Then
                If udtSPBLL.Exist Then
                    e.Row.Cells(4).RowSpan = dt.Rows.Count
                    e.Row.Cells(5).RowSpan = dt.Rows.Count
                End If
                Dim lnkBtnEngEHRSS As LinkButton = e.Row.FindControl("lnkBtnEngEHRSS")
                Dim lnkBtnChiEHRSS As LinkButton = e.Row.FindControl("lnkBtnChiEHRSS")

                lnkBtnEngEHRSS.CommandArgument = lblERNIndex.Text.Trim + "|EHRSS|E"
                lnkBtnChiEHRSS.CommandArgument = lblERNIndex.Text.Trim + "|EHRSS|C"

                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim lnkBtnCheckList As LinkButton = e.Row.FindControl("lnkBtnCheckList")

                If strLanguage.Equals("zh-tw") Then
                    lnkBtnCheckList.Attributes.Add("onclick", "javascript:openNewWin('Doc/CheckList_CHI.pdf');return false;")
                Else
                    lnkBtnCheckList.Attributes.Add("onclick", "javascript:openNewWin('Doc/CheckList.pdf');return false;")
                End If
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
            Else

                e.Row.Cells.Remove(e.Row.Cells(5))
                e.Row.Cells.Remove(e.Row.Cells(4))
            End If

            Dim lnkBtnEngAppA As LinkButton = e.Row.FindControl("lnkBtnEngAppA")
            Dim lnkBtnChiAppA As LinkButton = e.Row.FindControl("lnkBtnChiAppA")

            lnkBtnEngAppA.CommandArgument = lblERNIndex.Text.Trim + "|App|E"
            lnkBtnChiAppA.CommandArgument = lblERNIndex.Text.Trim + "|App|C"

            lblERNIndex.Text = Formatter.formatSystemNumber(lblERNIndex.Text)



        End If
    End Sub

    Private Sub gvPCDPrintOut_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPCDPrintOut.RowCommand

        Dim udtSp As ServiceProviderModel = Nothing
        Dim udtMO As MedicalOrganizationModelCollection = Nothing

        If udtSPBLL.Exist Then
            udtSp = udtSPBLL.GetSP

            If TypeOf e.CommandSource Is LinkButton Then
                If IsNothing(e.CommandArgument) OrElse e.CommandArgument.ToString.Equals(String.Empty) Then
                Else
                    Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

                    Dim strCommand() As String = e.CommandArgument.ToString.Split("|")
                    Dim strReportType As String = strCommand(0)
                    Dim strReportLanguage As String = strCommand(1)

                    Dim strERN As New List(Of String)
                    strERN = Session("EnrolmentRefNo")

                    Session(ReportFunction.SessionName.udtServiceProvider) = udtEFormBLL.CombineSPModel(strERN)

                    Select Case strReportLanguage
                        Case "E"
                            Select Case strReportType
                                Case "App"

                                    For Each item As String In strERN
                                        udtAuditLogEntry.AddDescripton("ERN", item)
                                    Next
                                    udtAuditLogEntry.AddDescripton("Language", "English")
                                    udtAuditLogEntry.WriteLog(LogID.LOG00062, "Print PCD Enrolment Form")
                                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "PCDFormScript", "javascript:openNewWin('PCDEnrolmentForm.aspx?lang=en-us');", True)
                            End Select
                        Case "C"
                            Select Case strReportType
                                Case "App"

                                    For Each item As String In strERN
                                        udtAuditLogEntry.AddDescripton("ERN", item)
                                    Next
                                    udtAuditLogEntry.AddDescripton("Language", "Chinese")
                                    udtAuditLogEntry.WriteLog(LogID.LOG00062, "Print PCD Enrolment Form")
                                    ScriptManager.RegisterStartupScript(Me, Page.GetType, "PCDFormScript", "javascript:openNewWin('PCDEnrolmentForm.aspx?lang=zh-tw');", True)
                            End Select
                    End Select
                End If

            End If

        End If

    End Sub

    Private Sub gvPCDPrintOut_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPCDPrintOut.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lnkBtnPCDEnrolmentFormEng As LinkButton = e.Row.FindControl("lnkBtnPCDEnrolmentFormEng")
            lnkBtnPCDEnrolmentFormEng.CommandArgument = "App|E"
            Dim lnkBtnPCDEnrolmentFormChi As LinkButton = e.Row.FindControl("lnkBtnPCDEnrolmentFormChi")
            lnkBtnPCDEnrolmentFormChi.CommandArgument = "App|C"
        End If
    End Sub

    Public Function KeepAlive() As String

        Dim int_MilliSecondsTimeOut As Integer = (HttpContext.Current.Session.Timeout * 60000) - 30000
        Dim sScript As New StringBuilder
        sScript.Append("<script type='text/javascript'>" & vbNewLine)
        'Number Of Reconnects   
        sScript.Append("var count=0;" & vbNewLine)
        'Maximum reconnects Setting   
        sScript.Append("var max = 6;" & vbNewLine)
        sScript.Append("function Reconnect(){" & vbNewLine)
        sScript.Append("count++;" & vbNewLine)
        sScript.Append("var d = new Date();" & vbNewLine)
        sScript.Append("var curr_hour = d.getHours();" & vbNewLine)
        sScript.Append("var curr_min = d.getMinutes();" & vbNewLine)
        sScript.Append("if (count < max){" & vbNewLine)
        sScript.Append("window.status = 'Refreshed ' + count.toString() + ' time(s) [' + curr_hour + ':' + curr_min + ']';" & vbNewLine)
        sScript.Append("var img = new Image(1,1);" & vbNewLine)
        sScript.Append("img.src = 'reconnect.aspx';" & vbNewLine)

        sScript.Append("}" & vbNewLine)
        sScript.Append("}" & vbNewLine)
        sScript.Append("window.setInterval('Reconnect()'," & int_MilliSecondsTimeOut.ToString() & "); //Set to length required" & vbNewLine)
        sScript.Append("</script>")

        KeepAlive = sScript.ToString
    End Function

    ' CRE15-018 Remove PPIePR Enrolment [Start][Winnie]
    'Private Sub gvPPIePRPrintOut_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPPIePRPrintOut.RowCommand
    '    Dim udtSp As ServiceProviderModel = Nothing
    '    Dim udtMO As MedicalOrganizationModelCollection = Nothing

    '    If udtSPBLL.Exist Then
    '        udtSp = udtSPBLL.GetSP

    '        If TypeOf e.CommandSource Is LinkButton Then
    '            If IsNothing(e.CommandArgument) OrElse e.CommandArgument.ToString.Equals(String.Empty) Then
    '            Else
    '                Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode, Me)

    '                Dim strCommand() As String = e.CommandArgument.ToString.Split("|")
    '                Dim strReportType As String = strCommand(0)
    '                Dim strReportLanguage As String = strCommand(1)

    '                Dim strERN As New List(Of String)
    '                strERN = Session("EnrolmentRefNo")

    '                Session(ReportFunction.SessionName.udtServiceProvider) = udtEFormBLL.CombineSPModel(strERN)

    '                Select Case strReportLanguage
    '                    Case "E"
    '                        Select Case strReportType
    '                            Case "App"

    '                                For Each item As String In strERN
    '                                    udtAuditLogEntry.AddDescripton("ERN", item)
    '                                Next
    '                                udtAuditLogEntry.AddDescripton("Language", "English")
    '                                udtAuditLogEntry.WriteLog(LogID.LOG00062, "Print PPIePR Application Form")
    '                                ScriptManager.RegisterStartupScript(Me, Page.GetType, "PPIePRFormScript", "javascript:openNewWin('PrintOut/PPIePR/PPIePR_RV.aspx');", True)
    '                        End Select
    '                End Select
    '            End If

    '        End If

    '    End If
    'End Sub
    ' CRE15-018 Remove PPIePR Enrolment [End][Winnie]

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
        If IsNothing(Me.udtSPBLL.GetSP) Then
            Return Nothing
        Else
            Return Me.udtSPBLL.GetSP
        End If
    End Function

#End Region

End Class