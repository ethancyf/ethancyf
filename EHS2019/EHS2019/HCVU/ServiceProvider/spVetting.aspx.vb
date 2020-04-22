Imports System.Web.Security.AntiXss
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.Component.ServiceProvider
Imports Common.Component.Token
Imports Common.Component.VoucherScheme
Imports Common.ComObject
Imports Common.Format
Imports Common.Validation
Imports Common.Component.Status
Imports Common.Component.Scheme
Imports Common.PCD
Imports Common.Component.Professional
Imports Common.PCD.WebService.Interface


Partial Public Class spVetting
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Inherits BasePageWithGridView
    Inherits BasePageWithControl
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

#Region "Fields"

    Private udtServiceProviderBLL As New ServiceProviderBLL
    Private udtSPProfileBLL As New SPProfileBLL
    Private udtSPVerificationBLL As New ServiceProviderVerificationBLL
    Private udtAuditLogEntry As AuditLogEntry

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private udtProfessionalBLL As New ProfessionalBLL
    Private udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]

    Private SM As SystemMessage
    Private udtFormatter As New Formatter
    Private udtValidator As New Validator

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    Private _strERN As String = String.Empty
    Private _blnOverrideResultLimit As Boolean = False
    Private _blnKeyFieldSearch As Boolean = False
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
#End Region

#Region "Constants"
    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Const _strValidationFailTitle As String = "ValidationFail"
    Private Const _strActionFailTitle As String = "UpdateFail"
    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

    Private Const SearchCriteria As Integer = 0
    Private Const SearchResult As Integer = 1
    Private Const Details As Integer = 2
    Private Const Complete As Integer = 3

    Private Const strFuncCode As String = FunctCode.FUNT010102

    Private Const SESS_VettingResult As String = "VettingResult"

    Private Const SESS_KeyFieldSearch As String = "Vetting_KeyFeildSearch"
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    Private Const SESS_FromTaskList As String = "010102_FromTaskList"
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Const SESS_PCDCheckAccountStatusResult As String = "010102_PCDCheckAccountStatusResult"
    Private Const SESS_PCDProfessionalChecked As String = "010102_PCDProfessional_Checked"
    Private Const SESS_PCDStatusChecked As String = "010102_PCDStatus_Checked"
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Enum EnrolmentAction
        Accept
        Defer
        Reject
        ReturnForAmendment
    End Enum
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)

        ' Shortcut from home page
        If Not IsPostBack Then
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            FunctionCode = FunctCode.FUNT010102
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, "Service Provider - Vetting loaded")

            ' Bind Status
            ddlStatus.DataSource = GetDescriptionListFromDBEnumCode("SPVettingStatus")
            ddlStatus.DataValueField = "Status_Value"
            ddlStatus.DataTextField = "Status_Description"
            ddlStatus.DataBind()

            ' Default the "Status" selection to Active
            ddlStatus.SelectedValue = "E"

            If Session("fromMain") = "Y" Then
                Session("fromMain") = Nothing
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                Session(SESS_FromTaskList) = "Y"
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
                ddlStatus.SelectedValue = "E"
                ibtnSearch_Click(Nothing, Nothing)
            End If

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

            ' -----------------------------------------------------------------------------------------

            ' Bind Health Profession
            ddlSPHealthProf.DataSource = udtSPProfileBLL.GetHealthProf
            ddlSPHealthProf.DataValueField = "ServiceCategoryCode"
            If Session("language") = "zh-tw" Then
                ddlSPHealthProf.DataTextField = "ServiceCategoryDescChi"
            Else
                ddlSPHealthProf.DataTextField = "ServiceCategoryDesc"
            End If
            ddlSPHealthProf.DataBind()

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            ' Bind Scheme
            ddlScheme.DataSource = udtSPProfileBLL.GetMasterScheme
            ddlScheme.DataValueField = "SchemeCode"
            ddlScheme.DataTextField = "DisplayCode"
            ddlScheme.DataBind()
            'ddlScheme.DataSource = udtSPProfileBLL.GetScheme
            'ddlScheme.DataValueField = "ItemNo"
            'If Session("language") = "zh-tw" Then
            '    ddlScheme.DataTextField = "DataValueChi"
            'Else
            '    ddlScheme.DataTextField = "DataValue"
            'End If
            'ddlScheme.DataBind()

            ' Handle double post-back
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            'MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnDialogConfirm)

        End If

        'SM = New SystemMessage("990000", "Q", "00001")

        'ibtnReject.OnClientClick = "showConfirm(this,'" + SM.GetMessage + "'); return false;"

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case Me.MultiViewSPVetting.ActiveViewIndex
            Case SearchCriteria
                'CRE17-008 (Remind Delist Practice) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If Not IsPopupShow() Then
                    ScriptManager1.SetFocus(Me.txtEnrolRefNo)
                    pnlSPVetting.DefaultButton = ibtnSearch.ID
                End If
                'CRE17-008 (Remind Delist Practice) [End][Chris YIM]
            Case SearchResult
                ScriptManager1.SetFocus(Me.btnHidden)
                pnlSPVetting.DefaultButton = ibtnSearchResultBack.ID

        End Select

        If MultiViewSPVetting.ActiveViewIndex <> Details Then
            ibtnExistingSPProfile.Style.Item("DISPLAY") = "none"
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
        Return udtSPProfileBLL.VettingSearch(strFuncCode, _strERN, txtSPID.Text.Trim, udtFormatter.formatHKIDInternal(txtSPHKID.Text.Trim), _
                                txtSPName.Text.Trim, txtPhone.Text.Trim, ddlSPHealthProf.SelectedValue.Trim, _
                                ddlStatus.SelectedValue.Trim, ddlScheme.SelectedValue.Trim, blnOverrideResultLimit)
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer
        Dim dt As New DataTable
        Dim intRowCount As Integer
        Dim blnShowResultList As Boolean = False

        dt = CType(udtBLLSearchResult.Data, DataTable)
        intRowCount = dt.Rows.Count

        If dt.Rows.Count = 0 Then
            blnShowResultList = False
            ' No record found
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, "Search failed: No Record")

            CompleteMsgBox.AddMessage(New SystemMessage("990000", "I", "00001"))
            CompleteMsgBox.BuildMessageBox()
            CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

        ElseIf dt.Rows.Count = 1 Then
            If _blnKeyFieldSearch Then
                ShowSPProfile(CStr(dt.Rows(0).Item("Enrolment_Ref_No")).Trim)
                blnShowResultList = False
            Else
                blnShowResultList = True
            End If

        Else
            blnShowResultList = True
        End If

        If blnShowResultList Then
            FillSearchCriteria()

            Me.GridViewDataBind(gvResult, dt, "Enter_Confirm_Dtm", "ASC", False)

            Session(SESS_VettingResult) = dt

            MultiViewSPVetting.ActiveViewIndex = SearchResult
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search completed")
        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        If Not IsNothing(Session(SESS_FromTaskList)) Then
            If Session(SESS_FromTaskList) = "Y" Then
                ddlStatus.SelectedValue = "E  "
                Session(SESS_FromTaskList) = Nothing
            End If
        End If

        _blnOverrideResultLimit = True
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

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Dim dt As New DataTable
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)

        Try
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'Dim strERN As String = String.Empty
            _strERN = String.Empty
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            txtEnrolRefNo.Text = UCase(txtEnrolRefNo.Text.Trim)
            txtSPHKID.Text = UCase(txtSPHKID.Text.Trim)

            If Not txtEnrolRefNo.Text = String.Empty Then
                If udtValidator.chkSystemNumber(txtEnrolRefNo.Text) Then
                    _strERN = Formatter.ReverseSystemNumber(txtEnrolRefNo.Text)
                Else
                    _strERN = txtEnrolRefNo.Text
                End If
            End If

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'Dim blnKeyFieldSearch As Boolean = False
            'Dim blnShowResultList As Boolean = False
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Session.Remove(SESS_KeyFieldSearch)

            If Not txtEnrolRefNo.Text.Trim.Equals(String.Empty) OrElse Not txtSPID.Text.Trim.Equals(String.Empty) OrElse Not txtSPHKID.Text.Trim.Equals(String.Empty) Then
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'blnKeyFieldSearch = True
                'Session(SESS_KeyFieldSearch) = blnKeyFieldSearch
                _blnKeyFieldSearch = True
                Session(SESS_KeyFieldSearch) = _blnKeyFieldSearch
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            End If

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'udtAuditLogEntry.AddDescripton("ERN", strERN)
            udtAuditLogEntry.AddDescripton("ERN", _strERN)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            udtAuditLogEntry.AddDescripton("SPID", txtSPID.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP HKID", txtSPHKID.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP Name", txtSPName.Text.Trim)
            udtAuditLogEntry.AddDescripton("Phone", txtPhone.Text.Trim)
            udtAuditLogEntry.AddDescripton("Profession", ddlSPHealthProf.SelectedValue.Trim)
            udtAuditLogEntry.AddDescripton("Status", ddlStatus.SelectedValue.Trim)
            udtAuditLogEntry.AddDescripton("Scheme", ddlScheme.SelectedValue.Trim)
            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00001, "Search", New AuditLogInfo(txtSPID.Text.Trim, txtSPHKID.Text.Trim, "", "", "", ""))

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            Dim enumSearchResult As SearchResultEnum

            If IsNothing(sender) AndAlso _blnOverrideResultLimit = True Then
                enumSearchResult = StartSearchFlow(strFuncCode, udtAuditLogEntry, msgBox, Nothing, False, True)
            Else
                enumSearchResult = StartSearchFlow(strFuncCode, udtAuditLogEntry, msgBox, Nothing)
            End If

            Select Case enumSearchResult
                Case SearchResultEnum.Success
                    ' Audit Log has been handled locally

                Case SearchResultEnum.ValidationFail
                    ' No Validation
                    Throw New Exception("Error: Class = [HCVU.spVetting], Method = [ibtnSearch_Click], Message = The method - [SF_ValidateSearch] should not return [False]")

                Case SearchResultEnum.NoRecordFound
                    ' Audit Log has been handled locally

                Case SearchResultEnum.OverResultList1stLimit_PopUp
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")

                Case SearchResultEnum.OverResultList1stLimit_Alert
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")

                Case SearchResultEnum.OverResultListOverrideLimit
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")

                Case Else
                    Throw New Exception("Error: Class = [HCVU.spVetting], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

            End Select

            'dt = udtSPProfileBLL.VettingSearch(strERN, txtSPID.Text.Trim, udtFormatter.formatHKIDInternal(txtSPHKID.Text.Trim), _
            '                                    txtSPName.Text.Trim, txtPhone.Text.Trim, ddlSPHealthProf.SelectedValue.Trim, _
            '                                    ddlStatus.SelectedValue.Trim, ddlScheme.SelectedValue.Trim)

            'If dt.Rows.Count = 0 Then
            'blnShowResultList = False
            ' No record found
            'udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, "Search failed: No Record")

            'CompleteMsgBox.AddMessage(New SystemMessage("990000", "I", "00001"))
            'CompleteMsgBox.BuildMessageBox()
            'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

            'ElseIf dt.Rows.Count = 1 Then
            'If blnKeyFieldSearch Then
            'ShowSPProfile(CStr(dt.Rows(0).Item("Enrolment_Ref_No")).Trim)
            'blnShowResultList = False
            'Else
            'blnShowResultList = True
            'End If

            'Else
            'blnShowResultList = True
            'End If

            'If blnShowResultList Then
            'FillSearchCriteria()

            'Me.GridViewDataBind(gvResult, dt, "Enter_Confirm_Dtm", "ASC", False)

            'Session(SESS_VettingResult) = dt

            'MultiViewSPVetting.ActiveViewIndex = SearchResult
            'udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search completed")
            'End If

        Catch eSQL As SqlClient.SqlException
            'If eSQL.Number = 50000 Then
            'msgBox.AddMessage(New SystemMessage("990001", "D", eSQL.Message))

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

    'Protected Sub btnSpDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim blnShowMessage As Boolean = False

    '    Dim strOld As String() = {"%s"}
    '    Dim strNew As String() = {""}
    '    Dim strMessageCode As String = String.Empty

    '    If BindSPSummaryView() Then
    '        Dim strProgress As String = udtSPProfileBLL.GetEnrolmentProcessStatus(hfERN.Value.Trim)

    '        If Not strProgress = String.Empty AndAlso Not strProgress = "00004" Then
    '            blnShowMessage = True
    '            strMessageCode = strProgress

    '            If udtServiceProviderBLL.GetSP.SPID.Equals(String.Empty) Then
    '                strNew(0) = udtServiceProviderBLL.GetSP.EnglishName + udtFormatter.formatChineseName(udtServiceProviderBLL.GetSP.ChineseName) + _
    '                                " [" + udtFormatter.formatSystemNumber(udtServiceProviderBLL.GetSP.EnrolRefNo) + "] "
    '            Else
    '                strNew(0) = udtServiceProviderBLL.GetSP.EnglishName + udtFormatter.formatChineseName(udtServiceProviderBLL.GetSP.ChineseName) + _
    '                                " [" + udtServiceProviderBLL.GetSP.SPID + "] "
    '            End If
    '        End If

    '    Else
    '        blnShowMessage = True
    '        strMessageCode = "00015"

    '    End If

    '    If blnShowMessage Then
    '        CompleteMsgBox.AddMessage("990000", "I", strMessageCode, strOld, strNew)
    '        CompleteMsgBox.BuildMessageBox()
    '        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

    '        MultiViewSPVetting.ActiveViewIndex = Complete

    '    End If

    'End Sub

    Private Sub ShowSPProfile(ByVal strERN As String)
        Dim blnShowMessage As Boolean = False

        Dim strOld As String() = {"%s"}
        Dim strNew As String() = {""}
        Dim strMessageCode As String = String.Empty

        If BindSPSummaryView(strERN) Then
            Dim strProgress As String = udtSPProfileBLL.GetEnrolmentProcessStatus(strERN)

            If strProgress Is Nothing Then strProgress = String.Empty

            If Not strProgress = String.Empty AndAlso Not strProgress = "00004" Then
                blnShowMessage = True
                strMessageCode = strProgress

                If udtServiceProviderBLL.GetSP.SPID.Equals(String.Empty) Then
                    strNew(0) = udtServiceProviderBLL.GetSP.EnglishName + udtFormatter.formatChineseName(udtServiceProviderBLL.GetSP.ChineseName) + _
                                    " [" + udtFormatter.formatSystemNumber(udtServiceProviderBLL.GetSP.EnrolRefNo) + "] "
                Else
                    strNew(0) = udtServiceProviderBLL.GetSP.EnglishName + udtFormatter.formatChineseName(udtServiceProviderBLL.GetSP.ChineseName) + _
                                    " [" + udtServiceProviderBLL.GetSP.SPID + "] "
                End If
            End If

        Else
            blnShowMessage = True
            'strMessageCode = "00017"
            strMessageCode = "00184"
        End If

        If blnShowMessage Then
            'CompleteMsgBox.AddMessage("990000", "I", strMessageCode, strOld, strNew)
            'CompleteMsgBox.BuildMessageBox()
            'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

            'MultiViewSPVetting.ActiveViewIndex = Complete
            Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
            SM = New Common.ComObject.SystemMessage("990000", "E", "00184")
            msgBox.AddMessage(SM)
            msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, "Search Fail.")
            MultiViewSPVetting.ActiveViewIndex = Complete
        End If
    End Sub

    Private Function BindSPSummaryView(ByVal strERN As String) As Boolean
        Dim blnAbleToBind As Boolean = False

        If Not strERN = String.Empty Then
            Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)

            blnAbleToBind = udtSPProfileBLL.GetServiceProviderProfile(strERN, TableLocation.Staging)

            If blnAbleToBind Then
                If udtServiceProviderBLL.Exist Then
                    Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP

                    udtAuditLogEntry.AddDescripton("ERN", strERN)
                    udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00005, "Select")

                    If udtSP.SPID Is Nothing OrElse udtSP.SPID.Trim = String.Empty Then
                        SpSummaryView1.buildSpProfileObject(udtSP, TableLocation.Staging, True)
                    Else
                        SpSummaryView1.buildSpProfileObject(udtSP, TableLocation.Staging, udtSPProfileBLL.GetServiceProviderPermanentProfile(strERN))
                    End If

                    If udtSP.SPID = String.Empty Then
                        ibtnExistingSPProfile.Style.Item("DISPLAY") = "none"
                    Else
                        ibtnExistingSPProfile.Style.Item("DISPLAY") = "block"
                    End If

                    SpSummaryView1.DisplayRecordStatus(True, TableLocation.Staging)

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' Show PCD Status if the SP able to join PCD
                    Dim blnShowPCDStatus As Boolean = False
                    Dim strPCDStatus As String = String.Empty
                    Dim udtProfessionalList As ProfessionalModelCollection

                    Session(SESS_PCDCheckAccountStatusResult) = Nothing

                    If udtProfessionalBLL.Exist Then
                        udtProfessionalList = udtProfessionalBLL.GetProfessionalCollection

                        For Each udtProfessionalModel As ProfessionalModel In udtProfessionalList.Values
                            If udtProfessionalModel.RecordStatus = ProfessionalStagingStatus.Active OrElse _
                                udtProfessionalModel.RecordStatus = ProfessionalStagingStatus.Existing Then

                                If udtProfessionalModel.Profession.AllowJoinPCD Then
                                    blnShowPCDStatus = True
                                    Exit For
                                End If
                            End If

                        Next
                    End If

                    If blnShowPCDStatus Then

                        ' Check PCD Account Status
                        Dim udtPCDWebService As PCDWebService = New PCDWebService(Me.FunctionCode)
                        Dim udtPCDResult As WebService.Interface.PCDCheckAccountStatusResult = Nothing

                        Try
                            udtAuditLogEntry.AddDescripton("WebMethod", "PCDCheckAccountStatus")
                            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00025, "CheckPCDAccountStatus Start")

                            udtPCDResult = udtPCDWebService.PCDCheckAccountStatus(udtSP.HKID)

                            udtAuditLogEntry.AddDescripton("ReturnCode", udtPCDResult.ReturnCode.ToString)
                            udtAuditLogEntry.AddDescripton("MessageID", udtPCDResult.MessageID.ToString)

                            Session(SESS_PCDCheckAccountStatusResult) = udtPCDResult

                            strPCDStatus = udtPCDResult.GetPCDStatusDesc()

                            ' Success
                            If udtPCDResult.ReturnCode = WebService.Interface.PCDCheckAccountStatusResult.enumReturnCode.Success Then

                                ' Update Service Provider's Join PCD Status
                                If Not udtSP.SPID.Equals(String.Empty) Then
                                    'Get VU User ID
                                    Dim udtHCVUUser As HCVUUserModel
                                    Dim udtHCVUUserBLL As New HCVUUserBLL
                                    udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

                                    Dim strMessage As String = String.Empty
                                    Dim blnRes As Boolean = udtPCDResult.UpdateJoinPCDStatus(udtSP.SPID, udtHCVUUser.UserID, strMessage)
                                    If Not blnRes Then
                                        Throw New Exception(strMessage)
                                    End If
                                End If

                                SpSummaryView1.DisplayPCDStatus(strPCDStatus, udtPCDResult.ProfID, False)
                                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00026, "CheckPCDAccountStatus Success")
                            Else
                                ' Connection Fail
                                SpSummaryView1.DisplayPCDStatus(strPCDStatus, udtPCDResult.ProfID, True)
                                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00027, "CheckPCDAccountStatus Fail")
                            End If

                        Catch ex As Exception
                            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00027, "CheckPCDAccountStatus Fail")
                            Throw
                        End Try
                    End If
                    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

                    MultiViewSPVetting.ActiveViewIndex = Details

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If udtSP.SPID = String.Empty Then
                        ' Show eHRSS Q&A
                        Select Case udtSP.AlreadyJoinEHR
                            Case JoinEHRSSStatus.Yes
                                panEHRSS.Visible = True
                                lblHadJoinEHRSS.Text = Me.GetGlobalResourceObject("Text", "Yes")

                            Case JoinEHRSSStatus.No
                                panEHRSS.Visible = True
                                lblHadJoinEHRSS.Text = Me.GetGlobalResourceObject("Text", "No")

                            Case JoinEHRSSStatus.NA
                                panEHRSS.Visible = False
                        End Select

                        ' Show PCD Q&A
                        Select Case udtSP.JoinPCD
                            Case JoinPCDStatus.Yes
                                panPCD.Visible = True
                                lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "Yes")

                            Case JoinPCDStatus.Enrolled
                                panPCD.Visible = True
                                lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "No_JoinedPCD")

                            Case JoinPCDStatus.No
                                panPCD.Visible = True
                                lblWillJoinPCD.Text = Me.GetGlobalResourceObject("Text", "No_NotJoinPCD")

                            Case JoinEHRSSStatus.NA
                                panPCD.Visible = False
                        End Select

                    Else
                        ' Hide eHRSS and PCD question for existing SP
                        panEHRSS.Visible = False
                        panPCD.Visible = False
                    End If
                    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
                End If

                If udtSPProfileBLL.IsAbleToVet Then
                    ' Able to vet the SP
                    ibtnAccept.Enabled = True
                    ibtnReject.Enabled = True
                    ibtnReturnForAmendment.Enabled = True

                    ibtnAccept.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AcceptBtn")
                    ibtnReject.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RejectBtn")
                    ibtnReturnForAmendment.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReturnForAmendmentBtn")

                    If udtSPVerificationBLL.Exist Then
                        If udtSPVerificationBLL.GetSPVerification.RecordStatus = ServiceProviderVerificationStatus.Defer Then
                            ibtnDefer.Enabled = False
                            ibtnDefer.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "DeferDisabledBtn")
                        Else
                            ibtnDefer.Enabled = True
                            ibtnDefer.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "DeferBtn")
                        End If

                    End If

                Else
                    ibtnAccept.Enabled = False
                    ibtnDefer.Enabled = False
                    ibtnReject.Enabled = False
                    ibtnReturnForAmendment.Enabled = False

                    ibtnAccept.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AcceptDisabledBtn")
                    ibtnDefer.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "DeferDisabledBtn")
                    ibtnReject.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RejectDisabledBtn")
                    ibtnReturnForAmendment.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReturnForAmendmentDisabledBtn")

                    CompleteMsgBox.AddMessage(New SystemMessage("990000", "I", "00012"))
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

                End If

                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00006, "Select completed")

            Else
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00019, "Select fail")
            End If

        End If

        Return blnAbleToBind

    End Function

    Protected Function GetHealthProfName(ByVal strHealthProfCode As String) As String
        If IsNothing(strHealthProfCode) OrElse strHealthProfCode = String.Empty Then Return String.Empty

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

        ' -----------------------------------------------------------------------------------------

        If Session("language") = "zh-tw" Then
            Return udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDescChi
        Else
            Return udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDesc
        End If

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

        Return Nothing

    End Function

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        udtAuditLogEntry.WriteLog(LogID.LOG00020, "Back Click")
        ' CRE11-021 log the missed essential information [End]

        udtServiceProviderBLL.ClearSession()
        'msgBox.Visible = False
        If Not IsNothing(Session(SESS_KeyFieldSearch)) Then
            If CBool(Session(SESS_KeyFieldSearch)) Then
                MultiViewSPVetting.ActiveViewIndex = SearchCriteria
                CompleteMsgBox.Visible = False
            End If
        Else
            MultiViewSPVetting.ActiveViewIndex = SearchCriteria
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            _blnOverrideResultLimit = True
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            ibtnSearch_Click(Nothing, Nothing)
            CompleteMsgBox.Visible = False
        End If

        If MultiViewSPVetting.ActiveViewIndex = SearchCriteria Then
            'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            msgBox.Visible = False
            'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
            ResetSearchCriteria()
        End If

    End Sub

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtSPProfileBLL.ClearSession()
        MultiViewSPVetting.ActiveViewIndex = SearchCriteria
    End Sub

    Private Sub ResetSearchCriteria()
        txtEnrolRefNo.Text = String.Empty
        txtSPID.Text = String.Empty
        txtSPHKID.Text = String.Empty
        txtSPName.Text = String.Empty
        txtPhone.Text = String.Empty
        ddlSPHealthProf.SelectedValue = String.Empty
        ddlStatus.SelectedValue = String.Empty
        ddlScheme.SelectedValue = String.Empty
    End Sub

    Private Sub AcceptEnrolment()

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Re-check PCD status 
        Session(SESS_PCDProfessionalChecked) = Nothing
        Session(SESS_PCDStatusChecked) = Nothing

        ProcessAccept()
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

    End Sub

    Private Sub DeferEnrolment()
        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)

        Try
            If udtSPProfileBLL.DeferSPProfile() Then
                Dim strOld As String() = {"%s"}
                Dim strNew As String() = {""}

                If udtServiceProviderBLL.Exist() Then
                    Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP

                    udtAuditLogEntry.AddDescripton("ERN", udtSP.EnrolRefNo)
                    udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Defer")

                    strNew(0) = udtServiceProviderBLL.GetSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + _
                                " [" + udtFormatter.formatSystemNumber(udtSP.EnrolRefNo) + "] "
                End If

                CompleteMsgBox.AddMessage("990000", "I", "00008", strOld, strNew)
                CompleteMsgBox.BuildMessageBox()
                CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete

                MultiViewSPVetting.ActiveViewIndex = Complete
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00011, "Defer successful")

            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                msgBox.AddMessage(New SystemMessage("990001", "D", eSQL.Message))

                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                Else
                    msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00012, "Defer failed")
                End If

            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex

        End Try

    End Sub

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Protected Sub ibtnAccept_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry.WriteLog(LogID.LOG00024, "Accept Click")

        hfEnrolmentAction.Value = EnrolmentAction.Accept

        ucEnrolmentActionPopup.MessageText = (New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVQ, MsgCode.MSG00005)).GetMessage
        ModalPopupEnrolmentAction.PopupDragHandleControlID = ucEnrolmentActionPopup.Header.ClientID
        ModalPopupEnrolmentAction.Show()
    End Sub

    Protected Sub ibtnDefer_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry.WriteLog(LogID.LOG00028, "Defer Click")

        hfEnrolmentAction.Value = EnrolmentAction.Defer

        ucEnrolmentActionPopup.MessageText = (New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVQ, MsgCode.MSG00004)).GetMessage
        ModalPopupEnrolmentAction.PopupDragHandleControlID = ucEnrolmentActionPopup.Header.ClientID
        ModalPopupEnrolmentAction.Show()
    End Sub

    Protected Sub ibtnReject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry.WriteLog(LogID.LOG00021, "Reject Click")

        hfEnrolmentAction.Value = EnrolmentAction.Reject

        ucEnrolmentActionPopup.MessageText = (New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVQ, MsgCode.MSG00001)).GetMessage
        ModalPopupEnrolmentAction.PopupDragHandleControlID = ucEnrolmentActionPopup.Header.ClientID
        ModalPopupEnrolmentAction.Show()
    End Sub

    Protected Sub ibtnReturnForAmendment_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry.WriteLog(LogID.LOG00029, "Return For Amendment Click")

        hfEnrolmentAction.Value = EnrolmentAction.ReturnForAmendment

        ucEnrolmentActionPopup.MessageText = (New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVQ, MsgCode.MSG00003)).GetMessage
        ModalPopupEnrolmentAction.PopupDragHandleControlID = ucEnrolmentActionPopup.Header.ClientID
        ModalPopupEnrolmentAction.Show()
    End Sub

    Private Sub ucEnrolmentActionPopup_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucEnrolmentActionPopup.ButtonClick
        Select Case e
            Case ucNoticePopup.enumButtonClick.OK
                udtAuditLogEntry.WriteLog(LogID.LOG00030, "Confirmation Popup - Confirm Click")

                Select Case hfEnrolmentAction.Value
                    Case EnrolmentAction.Accept
                        AcceptEnrolment()

                    Case EnrolmentAction.Defer
                        DeferEnrolment()

                    Case EnrolmentAction.Reject
                        RejectEnrolment()

                    Case EnrolmentAction.ReturnForAmendment
                        ReturnForAmendment()

                End Select

            Case ucNoticePopup.enumButtonClick.Cancel
                udtAuditLogEntry.WriteLog(LogID.LOG00031, "Confirmation Popup - Cancel Click")
                ModalPopupEnrolmentAction.Hide()

        End Select
    End Sub
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    Private Sub ReturnForAmendment()
        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        Try
            Dim udtSPAccountUpdateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
            Dim udtSPAccountUpdateModel As SPAccountUpdateModel = Nothing

            Dim udtSPVerificationBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL
            Dim udtSPVerificationModel As ServiceProviderVerificationModel = Nothing

            Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
            Dim udtSP As ServiceProviderModel = Nothing

            Dim udtHCVUUser As HCVUUserModel
            Dim udtHCVUUserBLL As New HCVUUserBLL
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------           
            udtSP = udtServiceProviderBLL.GetSP
            'Dim udtSPPermenant As ServiceProviderModel
            'udtSPPermenant = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(udtSP.EnrolRefNo, New Common.DataAccess.Database)

            'Dim udtSPStaging As ServiceProviderModel
            'udtSPStaging = udtServiceProviderBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, New Common.DataAccess.Database)

            'Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
            'If Not udtSPProfileBLL.CheckUnsynchronizeRecord(udtSPStaging, udtSPPermenant) Then
            Dim dtErrorMessage As DataTable = New DataTable("ErrorMessage")

            If CheckValidationClickReturnForAmendment(udtSP, dtErrorMessage) Then
                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

                If udtSPAccountUpdateBLL.Exist AndAlso udtSPVerificationBLL.Exist AndAlso udtServiceProviderBLL.Exist Then
                    udtSPAccountUpdateModel = udtSPAccountUpdateBLL.GetSPAccountUpdate
                    udtSPVerificationModel = udtSPVerificationBLL.GetSPVerification
                    udtSP = udtServiceProviderBLL.GetSP

                    udtAuditLogEntry.AddDescripton("ERN", udtSP.EnrolRefNo)
                    udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
                    udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00016, "Return for amendment:")


                    If udtSPProfileBLL.ReturnForAmendmentFromUserB(udtSP.EnrolRefNo, udtHCVUUser.UserID, udtSPAccountUpdateModel.TSMP, udtSPVerificationModel.TSMP) Then
                        Dim strOld As String() = {"%s"}
                        Dim strNew As String() = {""}


                        If udtSP.SPID.Equals(String.Empty) Then
                            strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                        udtFormatter.formatSystemNumber(udtSP.EnrolRefNo) + "] "
                        Else
                            strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                        udtSP.SPID + "] "
                        End If


                        CompleteMsgBox.AddMessage("990000", "I", "00010", strOld, strNew)
                        CompleteMsgBox.BuildMessageBox()
                        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                        MultiViewSPVetting.ActiveViewIndex = Complete
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00017, "Return for amendment Successful.")
                    End If
                End If
                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
            Else
                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'msgBox.AddMessage(FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, "%s", "")
                For Each drErrorMessage As DataRow In dtErrorMessage.Select()
                    If drErrorMessage.Item("IsReplace") Then
                        msgBox.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"), drErrorMessage.Item("FindString"), drErrorMessage.Item("ReplaceString"))
                    Else
                        msgBox.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"))
                    End If
                Next
                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                msgBox.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00023, "Return For Amendment abort")
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
            End If
            'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                msgBox.AddMessage(SM)
                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                Else
                    'msgBox.BuildMessageBox("UpdateFail")
                    msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00018, "Return for amendment Fail.")
                End If

                'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub gvResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResult.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, "VettingResult")
    End Sub

    Protected Sub gvResult_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.PreRender
        Me.GridViewPreRenderHandler(sender, e, "VettingResult")
    End Sub

    Private Sub gvResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvResult.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then
            Dim r As GridViewRow = CType(e.CommandSource, LinkButton).NamingContainer
            Dim strERN As String = CType(r.FindControl("lnkbtnERN"), LinkButton).CommandArgument.Trim

            '------------------------------------------------------------------------------
            'Check whether the record has been proceeded by others 
            Dim dt As DataTable
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'dt = udtSPProfileBLL.VettingSearch(strERN, "", "", _
            '                        "", "", "", _
            '                        "", "")

            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

            udtBLLSearchResult = udtSPProfileBLL.VettingSearch(strFuncCode, strERN, "", "", _
                                                "", "", "", _
                                                "", "", True, True)

            dt = CType(udtBLLSearchResult.Data, DataTable)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            If dt.Rows.Count = 0 Then
                Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
                SM = New Common.ComObject.SystemMessage("990000", "E", "00184")
                msgBox.AddMessage(SM)
                msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00004, "Search Fail.")
                MultiViewSPVetting.ActiveViewIndex = Complete
            Else
                'msgBox.Visible = False
                ShowSPProfile(strERN)
            End If
        End If
    End Sub

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
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

            ' Data Entry Confirm Time
            Dim lblRRequestDtm As Label = CType(e.Row.FindControl("lblRRequestDtm"), Label)
            If Not lblRRequestDtm.Text.Trim.Equals(String.Empty) Then
                lblRRequestDtm.Text = udtFormatter.convertDateTime(lblRRequestDtm.Text.Trim)
            End If

            ' Service Provider HKIC No.
            Dim lblRSPHKID As Label = CType(e.Row.FindControl("lblRSPHKID"), Label)
            lblRSPHKID.Text = udtFormatter.formatHKID(lblRSPHKID.Text, False)

            ' Service Provider Name
            Dim lblRCname As Label = CType(e.Row.FindControl("lblRCname"), Label)
            lblRCname.Text = udtFormatter.formatChineseName(lblRCname.Text.Trim)

            ' Status
            Dim lblRStatus As Label = CType(e.Row.FindControl("lblRStatus"), Label)
            Status.GetDescriptionFromDBCode(ServiceProviderVerificationStatus.ClassCode, lblRStatus.Text.Trim, lblRStatus.Text, String.Empty)

        End If
    End Sub

    Protected Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        Me.GridViewSortingHandler(sender, e, "VettingResult")
    End Sub

    Protected Sub ibtnExistingSPProfile_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtSP As ServiceProviderModel
        ' Dim udtToken As TokenModel

        Try
            udtSP = udtSPProfileBLL.GetServiceProviderPermanentProfileNoSession(udtServiceProviderBLL.GetSP.EnrolRefNo)
            'udtToken = udtSPProfileBLL.GetTokenModelBySPID(udtSP.SPID)
            udcExistingSPProfile.buildSpProfileObject(udtSP, TableLocation.Permanent)
            udcExistingSPProfile.DisplayRecordStatus(False, TableLocation.Permanent)
            Me.ModalPopupExtenderSPProfile.Show()
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Protected Sub ibtnExistingSPProfileClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ModalPopupExtenderSPProfile.Hide()
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

        If txtPhone.Text.Trim.Equals(String.Empty) Then
            lblResultPhone.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultPhone.Text = txtPhone.Text.Trim
        End If

        If ddlSPHealthProf.SelectedValue.Trim.Equals(String.Empty) Then
            lblResultHealthProf.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            lblResultHealthProf.Text = GetHealthProfName(ddlSPHealthProf.SelectedValue)
        End If

        If ddlStatus.SelectedValue.Trim.Equals(String.Empty) Then
            lblResultStatus.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
            lblResultStatus.Text = AntiXssEncoder.HtmlEncode(ddlStatus.SelectedItem.Text, True)
            ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
        End If

        If ddlScheme.SelectedValue.Trim.Equals(String.Empty) Then
            lblResultScheme.Text = Me.GetGlobalResourceObject("Text", "Any")
        Else
            ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
            lblResultScheme.Text = AntiXssEncoder.HtmlEncode(ddlScheme.SelectedItem.Text, True)
            ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
        End If
    End Sub

    Private Sub RejectEnrolment()

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        Dim udtSPAccountUpdateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
        Dim udtSPAccountUpdateModel As SPAccountUpdateModel = Nothing

        Dim udtSPVerificationBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL
        Dim udtSPVerificationModel As ServiceProviderVerificationModel = Nothing

        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSP As ServiceProviderModel = Nothing

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Try
            If udtSPAccountUpdateBLL.Exist AndAlso udtSPVerificationBLL.Exist AndAlso udtServiceProviderBLL.Exist Then
                udtSPAccountUpdateModel = udtSPAccountUpdateBLL.GetSPAccountUpdate
                udtSPVerificationModel = udtSPVerificationBLL.GetSPVerification
                udtSP = udtServiceProviderBLL.GetSP

                udtAuditLogEntry.AddDescripton("ERN", udtSP.EnrolRefNo)
                udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00013, "Reject:")


                If udtSPProfileBLL.RejectSPProfileFromUserB(udtSP.EnrolRefNo, udtHCVUUser.UserID, udtSPAccountUpdateModel.TSMP, udtSPVerificationModel.TSMP) Then
                    Dim strOld As String() = {"%s"}
                    Dim strNew As String() = {""}

                    If udtSP.SPID.Equals(String.Empty) Then
                        strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                    udtFormatter.formatSystemNumber(udtSP.EnrolRefNo) + "] "
                    Else
                        strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                    udtSP.SPID + "] "
                    End If


                    CompleteMsgBox.AddMessage("990000", "I", "00009", strOld, strNew)
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                    MultiViewSPVetting.ActiveViewIndex = Complete
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00014, "Reject Successful.")
                End If
            End If


        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                msgBox.AddMessage(SM)
                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                Else
                    'msgBox.BuildMessageBox("UpdateFail")
                    msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00015, "Reject Fail.")
                End If

                'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function CheckValidationClickAccept(ByRef udtSP As ServiceProviderModel, ByRef dtErrorMessage As DataTable) As Boolean
        Dim blnRes As Boolean = True
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtSPProfileBLL As New SPProfileBLL()
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

        AddDataColumnErrorMessage(dtErrorMessage)

        Dim drErrorMessage As DataRow

        '1. Check token whether is existed in SP when SP's email address is changed.
        If udtSPProfileBLL.CheckChangeEmailWithoutToken(udtSP) Then
            drErrorMessage = dtErrorMessage.NewRow()
            drErrorMessage.ItemArray = New Object() {FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00344, False, "", ""}
            dtErrorMessage.Rows.Add(drErrorMessage)
            blnRes = False
        End If

        '2. Check SP profile whether is synchronized between staging and permanent.
        If udtSPProfileBLL.CheckUnsynchronizeRecord(udtSP) Then
            drErrorMessage = dtErrorMessage.NewRow()
            drErrorMessage.ItemArray = New Object() {FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, True, "%s", ""}
            dtErrorMessage.Rows.Add(drErrorMessage)
            blnRes = False
        End If

        Return blnRes
    End Function

    Private Function CheckValidationClickReturnForAmendment(ByRef udtSP As ServiceProviderModel, ByRef dtErrorMessage As DataTable) As Boolean
        Dim blnRes As Boolean = True
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtSPProfileBLL As New SPProfileBLL()
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

        AddDataColumnErrorMessage(dtErrorMessage)

        Dim drErrorMessage As DataRow

        '1. Check SP profile whether is synchronized between staging and permanent.
        If udtSPProfileBLL.CheckUnsynchronizeRecord(udtSP) Then
            drErrorMessage = dtErrorMessage.NewRow()
            drErrorMessage.ItemArray = New Object() {FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, True, "%s", ""}
            dtErrorMessage.Rows.Add(drErrorMessage)
            blnRes = False
        End If

        Return blnRes
    End Function

    Sub AddDataColumnErrorMessage(ByRef dtErrorMessage As DataTable)

        Dim dcErrorMessage As DataColumn = New DataColumn("FunctionCode", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("SeverityCode", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("MessageCode", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("IsReplace", GetType(System.Boolean))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("FindString", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("ReplaceString", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

    End Sub
    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub ProcessAccept()
        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)

        Dim udtSPAccountUpdateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
        Dim udtSPAccountUpdateModel As SPAccountUpdateModel = Nothing

        Dim udtSPVerificationBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL
        Dim udtSPVerificationModel As ServiceProviderVerificationModel = Nothing

        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtSPStaging As ServiceProviderModel = Nothing

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Dim blnPCDProfessionalChecked As Boolean = False
        Dim blnPCDStatusChecked As Boolean = False

        Dim blnResInValid As Boolean = False
        
        ' PCD Warning Popup Confirm Click
        If Session(SESS_PCDProfessionalChecked) = YesNo.Yes Then
            blnPCDProfessionalChecked = True
        End If

        If Session(SESS_PCDStatusChecked) = YesNo.Yes Then
            blnPCDStatusChecked = True
        End If

        ' PCD Checking
        If blnPCDProfessionalChecked = False OrElse blnPCDStatusChecked = False Then
            udtSP = udtServiceProviderBLL.GetSP
            udtSPStaging = udtServiceProviderBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, New Common.DataAccess.Database)

            blnResInValid = Me.CheckPCD(udtSPStaging)
        End If

        If blnResInValid Then
            Return
        End If

        ' Complete Accept
        Try
            If udtSPAccountUpdateBLL.Exist AndAlso udtSPVerificationBLL.Exist AndAlso udtServiceProviderBLL.Exist Then
                udtSPAccountUpdateModel = udtSPAccountUpdateBLL.GetSPAccountUpdate
                udtSPVerificationModel = udtSPVerificationBLL.GetSPVerification
                udtSP = udtServiceProviderBLL.GetSP

                udtAuditLogEntry.AddDescripton("ERN", udtSP.EnrolRefNo)
                udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00007, "Accept:")

                '<1> Pass to Bank Account Verification
                '<2> Pass to Scheme Enrolment
                '<3> Application Accept

                If udtSPAccountUpdateModel.UpdateBankAcct Then
                    '<1> Pass to Bank Account Verification

                    If udtSPProfileBLL.VetSPProfile(SPAccountUpdateProgressStatus.BankAcctVerification) Then
                        Dim strOld As String() = {"%s"}
                        Dim strNew As String() = {""}


                        If udtSP.SPID.Equals(String.Empty) Then
                            strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                        udtFormatter.formatSystemNumber(udtSP.EnrolRefNo) + "] "
                        Else
                            strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                        udtSP.SPID + "] "
                        End If

                        CompleteMsgBox.AddMessage("990000", "I", "00005", strOld, strNew)
                        CompleteMsgBox.BuildMessageBox()
                        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                        MultiViewSPVetting.ActiveViewIndex = Complete
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00008, "Accept Successful.")
                    End If

                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                ElseIf udtSPAccountUpdateModel.SchemeConfirm Then
                    '<2> Pass to Scheme Enrolment

                    If udtSPProfileBLL.VetSPProfile(SPAccountUpdateProgressStatus.WaitingForSchemeEnrolment) Then
                        Dim strOld As String() = {"%s"}
                        Dim strNew As String() = {""}


                        If udtSP.SPID.Equals(String.Empty) Then
                            strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                        udtFormatter.formatSystemNumber(udtSP.EnrolRefNo) + "] "
                        Else
                            strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                        udtSP.SPID + "] "
                        End If

                        CompleteMsgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00046, strOld, strNew)
                        CompleteMsgBox.BuildMessageBox()
                        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                        MultiViewSPVetting.ActiveViewIndex = Complete
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00008, "Accept Successful.")
                    End If

                    ' ----------------------------------------------------------------------------------------
                    ' Remove partial accept flow (Pass to Scheme Enrolment to complete whole enrolment)
                    ' ----------------------------------------------------------------------------------------
                    ''1. Get the new enrolled scheme (Master Scheme Code)
                    ''2. Pass to Token / Scheme Management to complete the new scheme enrolment

                    ''1 Start
                    'Dim alEnrolledSchemeCode As New ArrayList
                    'Dim alNewSchemeCode As New ArrayList

                    'Dim alEnrolledSuspendedSchemeCode As New ArrayList

                    'Dim udtSPM As ServiceProviderModel
                    'udtSPM = udtServiceProviderBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, New Common.DataAccess.Database)

                    ''INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                    'Dim dtErrorMessage As DataTable = New DataTable("ErrorMessage")

                    'If CheckValidationClickAccept(udtSP, dtErrorMessage) Then
                    '    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]


                    '    If Not IsNothing(udtSPM) AndAlso Not IsNothing(udtSPM.SchemeInfoList) Then
                    '        For Each udtSchemeInfoModel As SchemeInformation.SchemeInformationModel In udtSPM.SchemeInfoList.Values
                    '            If udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Existing) Then
                    '                If Not alEnrolledSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                    '                    alEnrolledSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                    '                End If

                    '                'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [Start][Karl]
                    '                If Not alEnrolledSuspendedSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                    '                    alEnrolledSuspendedSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                    '                End If
                    '                'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [End][Karl]

                    '            ElseIf udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Active) Then
                    '                If Not alNewSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                    '                    alNewSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                    '                End If

                    '                'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [Start][Karl]
                    '            ElseIf (udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Suspended) Or _
                    '            udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.SuspendedPendingDelist) Or _
                    '            udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.SuspendedPendingReactivate)) Then
                    '                If Not alEnrolledSuspendedSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                    '                    alEnrolledSuspendedSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                    '                End If
                    '                'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [End][Karl]

                    '            Else
                    '                'Do nothing
                    '            End If
                    '        Next
                    '    End If

                    '    If udtSPProfileBLL.PartiallyAcceptSPProfileFromUserBBySchemeEnrolment(udtSP.EnrolRefNo, udtHCVUUser.UserID, udtSPAccountUpdateModel.TSMP, udtSPVerificationModel.TSMP, alEnrolledSchemeCode) Then
                    '        'Successful
                    '        Dim strOld As String() = {"%s", "%n"}
                    '        Dim strNew As String() = {"", ""}

                    '        Dim udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL

                    '        Dim strEnrolledSchemeCode As String = String.Empty
                    '        Dim i As Integer

                    '        For i = 0 To alEnrolledSuspendedSchemeCode.Count - 1

                    '            Dim strTemp As String = String.Empty
                    '            strTemp = udtSchemeBackOfficeBLL.GetSchemeBackOfficeDisplayCodeFromSchemeCode(alEnrolledSuspendedSchemeCode(i).Trim)

                    '            If strEnrolledSchemeCode.Equals(String.Empty) Then
                    '                strEnrolledSchemeCode = strTemp
                    '            Else
                    '                strEnrolledSchemeCode = strEnrolledSchemeCode & ", " & strTemp
                    '            End If
                    '        Next

                    '        strNew(0) = strEnrolledSchemeCode

                    '        Dim strNewSchemeCode As String = String.Empty
                    '        For i = 0 To alNewSchemeCode.Count - 1
                    '            Dim strTemp As String = String.Empty

                    '            'strTemp = udtSchemeBLL.getExternalSchemeCodeFromMasterSchemeCode(alNewSchemeCode(i).Trim)
                    '            strTemp = udtSchemeBackOfficeBLL.GetSchemeBackOfficeDisplayCodeFromSchemeCode(alNewSchemeCode(i).Trim)
                    '            If strNewSchemeCode.Equals(String.Empty) Then
                    '                strNewSchemeCode = strTemp
                    '            Else
                    '                strNewSchemeCode = strNewSchemeCode & ", " & strTemp
                    '            End If
                    '        Next

                    '        strNew(1) = strNewSchemeCode

                    '        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                    '        CompleteMsgBox.AddMessage("990000", "I", "00016", strOld, strNew)
                    '        CompleteMsgBox.BuildMessageBox()
                    '        MultiViewSPVetting.ActiveViewIndex = Complete
                    '        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00008, "Accept Successful (Scheme Enrolment).")
                    '    End If
                    '    '1 End

                    '    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                    '    '-----------------------------------------------------------------------------------------
                    'Else
                    '    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '    '-----------------------------------------------------------------------------------------
                    '    'msgBox.AddMessage(FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, "%s", "")
                    '    For Each drErrorMessage As DataRow In dtErrorMessage.Select()
                    '        If drErrorMessage.Item("IsReplace") Then
                    '            msgBox.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"), drErrorMessage.Item("FindString"), drErrorMessage.Item("ReplaceString"))
                    '        Else
                    '            msgBox.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"))
                    '        End If
                    '    Next
                    '    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    '    msgBox.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00022, "Accept abort")
                    '    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
                    'End If
                    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
                    ' ----------------------------------------------------------------------------------------

                Else
                    '<3> Application Accept
                    Dim alEnrolledSchemeCode As New ArrayList
                    Dim alNewSchemeCode As New ArrayList

                    Dim udtSPM As ServiceProviderModel
                    udtSPM = udtServiceProviderBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, New Common.DataAccess.Database)

                    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Dim dtErrorMessage As DataTable = New DataTable("ErrorMessage")

                    If CheckValidationClickAccept(udtSP, dtErrorMessage) Then
                        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
                        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                        If Not IsNothing(udtSPM) AndAlso Not IsNothing(udtSPM.SchemeInfoList) Then
                            For Each udtSchemeInfoModel As SchemeInformation.SchemeInformationModel In udtSPM.SchemeInfoList.Values
                                If udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Existing) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.ActivePendingDelist) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.ActivePendingSuspend) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Suspended) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.SuspendedPendingDelist) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.SuspendedPendingReactivate) Then

                                    If Not alEnrolledSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                                        alEnrolledSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                                    End If
                                ElseIf udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Active) Then
                                    If Not alNewSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                                        alNewSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                                    End If
                                Else
                                    'Do nothing
                                End If
                            Next
                        End If

                        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        'If udtSPProfileBLL.AcceptSPProfileFromUserB(udtSP.EnrolRefNo, udtHCVUUser.UserID, udtSPAccountUpdateModel.TSMP, udtSPVerificationModel.TSMP, alEnrolledSchemeCode) Then
                        If udtSPProfileBLL.AcceptVettingAndCompleteApplication(udtSP.EnrolRefNo, udtHCVUUser.UserID, udtSPAccountUpdateModel.TSMP, alEnrolledSchemeCode) Then
                            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]                            

                            Dim strOld As String() = {"%s"}
                            Dim strNew As String() = {""}

                            strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                        udtSP.SPID + "] "

                            CompleteMsgBox.AddMessage("990000", "I", "00011", strOld, strNew)
                            CompleteMsgBox.BuildMessageBox()
                            CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                            MultiViewSPVetting.ActiveViewIndex = Complete
                            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00008, "Accept Successful.")
                        End If

                        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                    Else
                        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        'msgBox.AddMessage(FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, "%s", "")
                        For Each drErrorMessage As DataRow In dtErrorMessage.Select()
                            If drErrorMessage.Item("IsReplace") Then
                                msgBox.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"), drErrorMessage.Item("FindString"), drErrorMessage.Item("ReplaceString"))
                            Else
                                msgBox.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"))
                            End If
                        Next
                        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                        msgBox.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, Common.Component.LogID.LOG00022, "Accept abort")
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
                    End If
                    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
                End If

            End If


        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                msgBox.AddMessage(SM)
                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                Else
                    msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00009, "Accept Fail.")
                End If

            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' Check whether the PCD status is valid to the Join PCD question
    ''' </summary>
    ''' <param name="udtSP"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckPCD(ByVal udtSP As ServiceProviderModel) As Boolean
        Dim blnJoinPCDCompulsory As Boolean = False
        Dim blnInvalid As Boolean = False

        Dim blnPCDProfessionalChecked As Boolean = False
        Dim blnPCDStatusChecked As Boolean = False

        ' PCD Warning Popup Confirm Click
        If Session(SESS_PCDProfessionalChecked) = YesNo.Yes Then
            blnPCDProfessionalChecked = True
        End If

        If Session(SESS_PCDStatusChecked) = YesNo.Yes Then
            blnPCDStatusChecked = True
        End If

        blnJoinPCDCompulsory = SPProfileBLL.IsJoinPCDCompulsory(udtSP.SchemeInfoList)

        Dim udtPCDAccountStatusResult As PCDCheckAccountStatusResult = Session(SESS_PCDCheckAccountStatusResult)

        If blnJoinPCDCompulsory AndAlso Not udtPCDAccountStatusResult Is Nothing Then
            Select Case udtPCDAccountStatusResult.AccountStatusCode
                Case PCDAccountStatus.NotEnrolled, PCDAccountStatus.Enrolled, PCDAccountStatus.Delisted

                    ' Check PCD Professional, show popup if contain any warning message
                    If Not blnInvalid AndAlso Not blnPCDProfessionalChecked Then
                        Dim strMessage As String = udtPCDAccountStatusResult.CheckPCDProfessional()

                        If strMessage <> String.Empty Then
                            blnInvalid = True
                            ShowPCDProfessionalWarning(strMessage)
                        Else
                            Session(SESS_PCDProfessionalChecked) = YesNo.Yes
                        End If
                    End If

                    If Not blnInvalid AndAlso Not blnPCDStatusChecked Then
                        If udtPCDAccountStatusResult.AccountStatusCode = PCDAccountStatus.Delisted Then
                            ' Show warning for SP is delisted in PCD and need to re-join PCD                        
                            ShowPCDDelistedWarning()
                            blnInvalid = True
                        Else
                            Session(SESS_PCDStatusChecked) = YesNo.Yes
                        End If
                    End If

                Case PCDAccountStatus.ConnectionFail
                    ' PCD service is temporarily unavailable. Please try again later.
                    ShowPCDConnectionFail()
                    blnInvalid = True

            End Select

        Else
            ' Bypass PCD checking for non PCD profession or SP not compulsory to join PCD
            Session(SESS_PCDProfessionalChecked) = YesNo.Yes
            Session(SESS_PCDStatusChecked) = YesNo.Yes
            blnInvalid = False
        End If

        Return blnInvalid
    End Function

    Private Sub ucPCDWarningPopup_Success_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ucPCDWarningPopup.Success_Click
        ' Confirm Click, continue process
        Select Case hfPCDWarningPopupType.Value
            Case ucPCDWarningPopup.WarningType.Professional
                Session(SESS_PCDProfessionalChecked) = YesNo.Yes

            Case ucPCDWarningPopup.WarningType.Delisted
                Session(SESS_PCDStatusChecked) = YesNo.Yes

        End Select

        ProcessAccept()
    End Sub

    Private Sub ucPCDWarningPopup_Failure_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ucPCDWarningPopup.Failure_Click
        Me.ModelPopupExtenderPCDWarning.Show()
    End Sub

    Private Sub ShowPCDDelistedWarning()
        hfPCDWarningPopupType.Value = ucPCDWarningPopup.WarningType.Delisted

        Me.ucPCDWarningPopup.Build(ucPCDWarningPopup.WarningType.Delisted)
        Me.ModelPopupExtenderPCDWarning.PopupDragHandleControlID = Me.ucPCDWarningPopup.Header.ClientID
        Me.ModelPopupExtenderPCDWarning.Show()
    End Sub

    Private Sub ShowPCDProfessionalWarning(ByVal strWarningMessage As String)
        hfPCDWarningPopupType.Value = ucPCDWarningPopup.WarningType.Professional

        Me.ucPCDWarningPopup.MessageText = strWarningMessage
        Me.ucPCDWarningPopup.Build(ucPCDWarningPopup.WarningType.Professional)
        Me.ModelPopupExtenderPCDWarning.PopupDragHandleControlID = Me.ucPCDWarningPopup.Header.ClientID
        Me.ModelPopupExtenderPCDWarning.Show()
    End Sub

    Private Sub ShowPCDConnectionFail()
        Me.udtAuditLogEntry.WriteLog(LogID.LOG01136, "Show PCD Connection Fail Popup")

        Me.ucNoticePopup.NoticeMode = HCVU.ucNoticePopUp.enumNoticeMode.Custom
        Me.ucNoticePopup.ButtonMode = HCVU.ucNoticePopUp.enumButtonMode.OK
        Me.ucNoticePopup.IconMode = HCVU.ucNoticePopUp.enumIconMode.ExclamationIcon
        Me.ucNoticePopup.HeaderText = Me.GetGlobalResourceObject("Text", "Notification")
        Me.ucNoticePopup.MessageText = Me.GetGlobalResourceObject("Text", "PCDServiceUnavailable")
        Me.ModalPopupExtenderNotice.PopupDragHandleControlID = Me.ucNoticePopup.Header.ClientID
        Me.ModalPopupExtenderNotice.Show()
    End Sub
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

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