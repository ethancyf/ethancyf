Imports System.Web.Security.AntiXss
Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports HCVU.BLL

Partial Public Class spEnquiry
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Inherits BasePageWithGridView
    Inherits BasePageWithControl
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

#Region "Fields"

    Private udtSPProfileBLL As New SPProfileBLL
    Private udtServiceProviderBLL As New ServiceProviderBLL

    Private udtFormatter As New Formatter
    Private udtValidator As New Validator

    Private SM As SystemMessage
    Private udtAuditLogEntry As AuditLogEntry

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    Private _strERN As String = String.Empty
    Private _blnKeyFieldSearch As Boolean = False
    Private _blnShowResultList As Boolean = False
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    Private Const ViewIndexSearchCriteria As Integer = 0
    Private Const ViewIndexSearchResult As Integer = 1
    Private Const ViewIndexDetail As Integer = 2
    Private Const ViewIndexErrorPage As Integer = 3

    Private Const SESS_SearchResultList As String = "SPEnquiry_SearchResultList"
    Private Const SESS_KeyFieldSearch As String = "SPEnquiry_KeyFeildSearch"

    Public Const SESSION_REDIRECT_PARAMETER As String = "PageRedirectorBLL.Parameter"
    Public Const SESSION_REDIRECT_SOURCE As String = "PageRedirectorBLL.Source"
    Public Const REDIRECT_NAME As String = "eHSAccountEnquiry"
#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Me.ibtnBack.Visible = True

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT010204

            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Service Provider Enquiry loaded")

            ' Bind Health professiona information
            ddlSPHealthProf.DataSource = udtSPProfileBLL.GetHealthProf

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

            ' -----------------------------------------------------------------------------------------

            ddlSPHealthProf.DataValueField = "ServiceCategoryCode"
            If Session("language") = "zh-tw" Then
                ddlSPHealthProf.DataTextField = "ServiceCategoryDescChi"
            Else
                ddlSPHealthProf.DataTextField = "ServiceCategoryDesc"
            End If

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            ddlSPHealthProf.DataBind()

            ' Bind scheme information
            ddlScheme.DataSource = udtSPProfileBLL.GetMasterScheme
            ddlScheme.DataValueField = "SchemeCode"
            ddlScheme.DataTextField = "DisplayCode"
            ddlScheme.DataBind()
            'ddlScheme.DataSource = udtSPProfileBLL.GetScheme
            'ddlScheme.DataValueField = "ItemNo"
            'ddlScheme.DataTextField = "DataValue"
            'ddlScheme.DataBind()

            Me.hfIsRedirect.Value = False
            If Me.Session(SESSION_REDIRECT_SOURCE) IsNot Nothing AndAlso Me.Session(SESSION_REDIRECT_SOURCE) = REDIRECT_NAME Then
                Me.hfIsRedirect.Value = True
                Me.BindSPSummaryView(CType(Me.Session(SESSION_REDIRECT_PARAMETER), String))
                Me.Session(SESSION_REDIRECT_PARAMETER) = Nothing
                Me.Session(SESSION_REDIRECT_SOURCE) = Nothing
            End If
        End If

        If Me.hfIsRedirect.Value = True Then
            Me.ibtnBack.Visible = False
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If MultiViewEnquiry.ActiveViewIndex <> ViewIndexDetail Then
            ibtnAmendedRecord.Visible = False
        End If

        If MultiViewEnquiry.ActiveViewIndex = ViewIndexSearchCriteria Then
            pnlEnquiry.DefaultButton = ibtnSearch.ID
        End If

    End Sub

#End Region

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
#Region "Abstract Method of [HCVU.BasePageWithControl]"
    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As AuditLogEntry) As Boolean
        Return True
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        'Return udtSPProfileBLL.EnquirySearch(FunctionCode, _strERN, _
        '                    txtSPID.Text.Trim, _
        '                    udtFormatter.formatHKIDInternal(txtSPHKID.Text.Trim), _
        '                    txtSPName.Text.Trim, _
        '                    txtPhone.Text.Trim, _
        '                    ddlSPHealthProf.SelectedValue.Trim, _
        '                    ddlScheme.SelectedValue.Trim, blnOverrideResultLimit)
        Return udtSPProfileBLL.EnquirySearch(FunctionCode, _strERN, _
                            txtSPID.Text.Trim, _
                            udtFormatter.formatHKIDInternal(txtSPHKID.Text.Trim), _
                            txtSPName.Text.Trim, _
                            txtSPChiName.Text.Trim, _
                            txtPhone.Text.Trim, _
                            ddlSPHealthProf.SelectedValue.Trim, _
                            ddlScheme.SelectedValue.Trim, blnOverrideResultLimit)
        ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer
        Dim dt As DataTable
        Dim intRowCount As Integer

        Try
            dt = CType(udtBLLSearchResult.Data, DataTable)

        Catch ex As Exception
            Throw

        End Try

        intRowCount = dt.Rows.Count

        Select Case intRowCount
            Case 0
                ' No record found
                _blnShowResultList = False

            Case 1
                If _blnKeyFieldSearch Then
                    BindSPSummaryView(CStr(dt.Rows(0).Item("Enrolment_Ref_No")).Trim, CStr(dt.Rows(0).Item("SP_ID")).Trim, CStr(dt.Rows(0).Item("Table_Location")).Trim)
                    _blnShowResultList = False
                Else
                    _blnShowResultList = True
                End If

            Case Else
                _blnShowResultList = True

        End Select

        If _blnShowResultList Then
            FillSearchCriteria()
            gvResult.DataSource = dt
            gvResult.DataBind()

            Session(SESS_SearchResultList) = dt

            Me.GridViewDataBind(gvResult, dt, "Enrolment_Ref_No", "ASC", False)

            MultiViewEnquiry.ActiveViewIndex = ViewIndexSearchResult
        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        ibtnSearch_Click(Nothing, Nothing)
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub
#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        CompleteMsgBox.Visible = False
        msgBox.Visible = False

        ' CRE12-015 Add the respective practice number in “Practice” in the functions under “Reimbursement” in eHS [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------------------------------------------
        ' Implement Collapsible Search Criteria Review
        udcCollapsibleSearchCriteriaReview.Collapsed = True
        udcCollapsibleSearchCriteriaReview.ClientState = "True"
        ' CRE12-015 Add the respective practice number in “Practice” in the functions under “Reimbursement” in eHS [End][Tommy L]

        Try
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'Dim strERN As String = String.Empty
            _strERN = String.Empty
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            If txtEnrolRefNo.Text.Trim <> String.Empty Then
                ' I-CRE16-003 Fix XSS [Start][Lawrence]
                txtEnrolRefNo.Text = UCase(AntiXssEncoder.HtmlEncode(txtEnrolRefNo.Text, True))
                ' I-CRE16-003 Fix XSS [End][Lawrence]

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                If udtValidator.chkSystemNumber(txtEnrolRefNo.Text.Trim) Then
                    'strERN = Formatter.ReverseSystemNumber(txtEnrolRefNo.Text.Trim)
                    _strERN = Formatter.ReverseSystemNumber(txtEnrolRefNo.Text.Trim)
                Else
                    'strERN = txtEnrolRefNo.Text.Trim
                    _strERN = txtEnrolRefNo.Text.Trim
                End If
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            End If

            txtSPHKID.Text = UCase(txtSPHKID.Text.Trim)

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'Dim blnKeyFieldSearch As Boolean = False
            'Dim blnShowResultList As Boolean = False
            _blnKeyFieldSearch = False
            _blnShowResultList = False
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Session.Remove(SESS_KeyFieldSearch)

            If txtEnrolRefNo.Text.Trim <> String.Empty OrElse txtSPID.Text.Trim <> String.Empty OrElse txtSPHKID.Text.Trim <> String.Empty Then
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'blnKeyFieldSearch = True
                'Session(SESS_KeyFieldSearch) = blnKeyFieldSearch
                _blnKeyFieldSearch = True
                Session(SESS_KeyFieldSearch) = _blnKeyFieldSearch
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            End If

            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'udtAuditLogEntry.AddDescripton("ERN", strERN)
            udtAuditLogEntry.AddDescripton("ERN", _strERN)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            udtAuditLogEntry.AddDescripton("SPID", txtSPID.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP HKID", txtSPHKID.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP Name", txtSPName.Text.Trim)
            ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
            udtAuditLogEntry.AddDescripton("SP ChiName", txtSPChiName.Text.Trim)
            ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]
            udtAuditLogEntry.AddDescripton("Phone", txtPhone.Text.Trim)
            udtAuditLogEntry.AddDescripton("Profession", ddlSPHealthProf.SelectedValue.Trim)
            udtAuditLogEntry.AddDescripton("Scheme", ddlScheme.SelectedValue.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search", New AuditLogInfo(txtSPID.Text.Trim, txtSPHKID.Text.Trim, "", "", "", ""))

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            Dim enumSearchResult As SearchResultEnum

            If IsNothing(sender) Then
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, CompleteMsgBox, False, True)
            Else
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, msgBox, CompleteMsgBox)
            End If

            Select Case enumSearchResult
                Case SearchResultEnum.Success
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search completed")

                Case SearchResultEnum.ValidationFail
                    ' No Validation
                    Throw New Exception("Error: Class = [HCVU.spEnquiry], Method = [ibtnSearch_Click], Message = The method - [SF_ValidateSearch] should not return [False]")

                Case SearchResultEnum.NoRecordFound
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search completed. No record found")

                Case SearchResultEnum.OverResultList1stLimit_PopUp
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")

                Case SearchResultEnum.OverResultList1stLimit_Alert
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")

                Case SearchResultEnum.OverResultListOverrideLimit
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")

                Case Else
                    Throw New Exception("Error: Class = [HCVU.spEnquiry], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

            End Select

            'Dim dt As DataTable = udtSPProfileBLL.EnquirySearch(strERN, _
            '                                    txtSPID.Text.Trim, _
            '                                    udtFormatter.formatHKIDInternal(txtSPHKID.Text.Trim), _
            '                                    txtSPName.Text.Trim, _
            '                                    txtPhone.Text.Trim, _
            '                                    ddlSPHealthProf.SelectedValue.Trim, _
            '                                    ddlScheme.SelectedValue.Trim)

            'Select Case dt.Rows.Count
            'Case 0
            ' No record found
            'blnShowResultList = False
            'CompleteMsgBox.AddMessage(New Common.ComObject.SystemMessage("990000", "I", "00001"))
            'CompleteMsgBox.BuildMessageBox()
            'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

            'udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search completed. No record found")

            'Case 1
            'If blnKeyFieldSearch Then
            'BindSPSummaryView(CStr(dt.Rows(0).Item("Enrolment_Ref_No")).Trim, CStr(dt.Rows(0).Item("SP_ID")).Trim, CStr(dt.Rows(0).Item("Table_Location")).Trim)
            'blnShowResultList = False
            'Else
            'blnShowResultList = True
            'End If

            'Case Else
            'blnShowResultList = True

            'End Select

            'If blnShowResultList Then
            'FillSearchCriteria()
            'gvResult.DataSource = dt
            'gvResult.DataBind()

            'Session(SESS_SearchResultList) = dt

            'Me.GridViewDataBind(gvResult, dt, "Enrolment_Ref_No", "ASC", False)

            'MultiViewEnquiry.ActiveViewIndex = ViewIndexSearchResult

            'udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search completed")

            'End If

        Catch eSQL As SqlClient.SqlException
            'If eSQL.Number = 50000 Then
            'msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, "D", eSQL.Message))

            'If msgBox.GetCodeTable.Rows.Count = 0 Then
            'msgBox.Visible = False
            'Else
            'msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, "Search failed")
            'End If

            'Else
            Throw eSQL
            'End If
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub FillSearchCriteria()
        If txtEnrolRefNo.Text.Trim.Equals(String.Empty) Then
            lblResultERN.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultERN.Text = txtEnrolRefNo.Text.Trim
        End If

        If txtSPID.Text.Trim.Equals(String.Empty) Then
            lblResultSPID.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultSPID.Text = txtSPID.Text.Trim
        End If

        If txtSPHKID.Text.Trim.Equals(String.Empty) Then
            lblResultSPHKID.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultSPHKID.Text = txtSPHKID.Text.Trim
        End If

        If txtSPName.Text.Trim.Equals(String.Empty) Then
            lblResultSPName.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultSPName.Text = txtSPName.Text.Trim
        End If

        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        If txtSPChiName.Text.Trim.Equals(String.Empty) Then
            lblResultSPChiName.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultSPChiName.Text = txtSPChiName.Text.Trim
        End If
        ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

        If txtPhone.Text.Trim.Equals(String.Empty) Then
            lblResultPhone.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultPhone.Text = txtPhone.Text.Trim
        End If

        If ddlSPHealthProf.SelectedValue.Trim.Equals(String.Empty) Then
            lblResultHealthProf.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

            ' -----------------------------------------------------------------------------------------

            lblResultHealthProf.Text = udtSPProfileBLL.GetHealthProfName(ddlSPHealthProf.SelectedValue).ServiceCategoryDesc

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        End If

        If ddlScheme.SelectedValue.Trim.Equals(String.Empty) Then
            lblResultScheme.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultScheme.Text = ddlScheme.SelectedItem.Text.Trim
        End If
    End Sub


    Private Sub BindSPSummaryView(ByVal strSPID As String)
        ResetSearchCriteria()
        txtSPID.Text = strSPID
        ibtnSearch_Click(Me.ibtnSearch, New ImageClickEventArgs(0, 0))

    End Sub

    Private Sub BindSPSummaryView(ByVal strERN As String, ByVal strSPID As String, ByVal strTableLocation As String)
        If IsNothing(strERN) OrElse strERN.Trim = String.Empty Then Return
        If IsNothing(strTableLocation) OrElse strTableLocation.Trim = String.Empty Then Return

        Dim blnNotFind As Boolean = False

        Dim udtSP As ServiceProviderModel = Nothing
        Dim strSPHKID As String = String.Empty

        If strTableLocation.Trim.Equals(TableLocation.Permanent) Then
            udtSP = udtSPProfileBLL.GetServiceProviderPermanentProfileWithMaintenance(strSPID)

            If IsNothing(udtSP) Then
                blnNotFind = True
            Else
                ibtnAmendedRecord.Visible = udtSP.UnderModification <> String.Empty
            End If
        Else
            If udtSPProfileBLL.GetServiceProviderProfile(strERN, strTableLocation) Then
                If udtServiceProviderBLL.Exist Then
                    udtSP = udtServiceProviderBLL.GetSP
                    strSPHKID = udtSP.HKID
                Else
                    blnNotFind = True
                End If
            Else
                blnNotFind = True
            End If

            ibtnAmendedRecord.Visible = False
        End If

        ' Write Audit Log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ERN", strERN)
        udtAuditLogEntry.AddDescripton("SPID", strSPID)
        udtAuditLogEntry.WriteLog(LogID.LOG00005, "Select", New AuditLogInfo(IIf(strSPHKID <> String.Empty, Nothing, strSPID), strSPHKID, String.Empty, String.Empty, String.Empty, String.Empty))

        If blnNotFind Then
            msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
            msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00004, "Select failed: Service Provider object not found")

            Me.MultiViewEnquiry.ActiveViewIndex = ViewIndexErrorPage
        Else
            ' Call the function in spSummaryView.aspx.vb
            SpSummaryView1.buildSpProfileObject(udtSP, strTableLocation, True)
            SpSummaryView1.DisplayRecordStatus(False, strTableLocation)

            MultiViewEnquiry.ActiveViewIndex = ViewIndexDetail
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Select completed")
        End If

    End Sub

    '

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00007, "Search Result Back Click")
        ' CRE11-021 log the missed essential information [End]

        udtSPProfileBLL.ClearSession()
        MultiViewEnquiry.ActiveViewIndex = ViewIndexSearchCriteria
    End Sub

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00008, "Detail Back Click")
        ' CRE11-021 log the missed essential information [End]

        udtServiceProviderBLL.ClearSession()
        If Not IsNothing(Session(SESS_KeyFieldSearch)) Then
            If CBool(Session(SESS_KeyFieldSearch)) Then
                MultiViewEnquiry.ActiveViewIndex = ViewIndexSearchCriteria
                CompleteMsgBox.Visible = False
            End If
        Else
            MultiViewEnquiry.ActiveViewIndex = ViewIndexSearchCriteria
            ibtnSearch_Click(Nothing, Nothing)
            CompleteMsgBox.Visible = False
        End If

        If MultiViewEnquiry.ActiveViewIndex = ViewIndexSearchCriteria Then
            ResetSearchCriteria()
        End If

        Me.hfIsRedirect.Value = False
    End Sub

    Protected Sub ibtnReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ibtnBack_Click(Me, e)
    End Sub

    Protected Sub ibtnErrorBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        ResetSearchCriteria()

        udtSPProfileBLL.ClearSession()
        Me.MultiViewEnquiry.ActiveViewIndex = ViewIndexSearchCriteria

    End Sub


    Private Sub ResetSearchCriteria()
        txtEnrolRefNo.Text = String.Empty
        txtSPID.Text = String.Empty
        txtSPHKID.Text = String.Empty
        txtSPName.Text = String.Empty
        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        txtSPChiName.Text = String.Empty
        ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]
        txtPhone.Text = String.Empty
        ddlSPHealthProf.SelectedValue = String.Empty
        ddlScheme.SelectedValue = String.Empty
    End Sub

    '

    Private Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Enrolment Reference No.
            Dim lnkbtnERN As LinkButton = CType(e.Row.FindControl("lnkbtnERN"), LinkButton)
            lnkbtnERN.Text = udtFormatter.formatSystemNumber(lnkbtnERN.CommandArgument.Trim)

            ' Service Provider ID
            Dim lnkbtnRSPID As LinkButton = CType(e.Row.FindControl("lnkbtnRSPID"), LinkButton)
            If lnkbtnRSPID.Text.Trim.Equals(String.Empty) Then
                lnkbtnRSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lnkbtnRSPID.CommandArgument = String.Empty
                lnkbtnRSPID.Enabled = False
            End If

            ' Service Provider HKIC No.
            Dim lblRSPHKID As Label = CType(e.Row.FindControl("lblRSPHKID"), Label)
            lblRSPHKID.Text = udtFormatter.formatHKID(lblRSPHKID.Text, False)

            ' Service Provider Name
            Dim lblRCname As Label = CType(e.Row.FindControl("lblRCname"), Label)
            lblRCname.Text = udtFormatter.formatChineseName(lblRCname.Text.Trim)

            ' Status
            Dim lblRStatus As Label = CType(e.Row.FindControl("lblRStatus"), Label)
            Status.GetDescriptionFromDBCode(SPEnquiryDisplayStatus.ClassCode, lblRStatus.Text.Trim, lblRStatus.Text, String.Empty)

            ' Request Date time (invisible)
            Dim lblRRequestDtm As Label = CType(e.Row.FindControl("lblRRequestDtm"), Label)
            If Not lblRRequestDtm.Text.Trim = String.Empty Then
                lblRRequestDtm.Text = udtFormatter.convertDateTime(lblRRequestDtm.Text.Trim)
            End If

        End If
    End Sub

    Private Sub gvResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResult.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_SearchResultList)
    End Sub

    Private Sub gvResult_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_SearchResultList)
    End Sub

    Private Sub gvResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvResult.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then
            Dim r As GridViewRow = CType(e.CommandSource, LinkButton).NamingContainer
            Dim strERN As String = CType(r.FindControl("lnkbtnERN"), LinkButton).CommandArgument.Trim
            Dim strSPID As String = CType(r.FindControl("lnkbtnRSPID"), LinkButton).Text.Trim
            Dim strTableLocation As String = CType(r.FindControl("hfRTableLocation"), HiddenField).Value.Trim

            If strERN.Trim = String.Empty Then strERN = CType(r.FindControl("lnkbtnRSPID"), LinkButton).CommandArgument.Trim
            If strSPID.Trim = Me.GetGlobalResourceObject("Text", "N/A") Then strSPID = String.Empty

            BindSPSummaryView(strERN, strSPID, strTableLocation)
        End If
    End Sub

    Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_SearchResultList)
    End Sub

    '

    Protected Sub ibtnAmendedRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim udtAmendingSP As ServiceProviderModel = udtSPProfileBLL.GetServiceProviderStagingProfileNoSession(udtServiceProviderBLL.GetSP.EnrolRefNo)

            If Not IsNothing(udtAmendingSP) Then
                udcExistingSPProfile.buildSpProfileObject(udtAmendingSP, TableLocation.Staging)
                udcExistingSPProfile.DisplayRecordStatus(True, TableLocation.Staging)
                ModalPopupExtenderSPProfile.Show()

            Else
                CompleteMsgBox.AddMessage("990000", "I", "00015")
                CompleteMsgBox.BuildMessageBox()
                CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnExistingSPProfileClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ModalPopupExtenderSPProfile.Hide()
    End Sub

    ' Used in .aspx

    Protected Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String, ByVal strArea As String) As String
        Return udtFormatter.formatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrict, strArea)
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
        If IsNothing(Me.udtServiceProviderBLL.GetSP) Then
            Return Nothing
        Else
            Return Me.udtServiceProviderBLL.GetSP
        End If
    End Function

#End Region
End Class