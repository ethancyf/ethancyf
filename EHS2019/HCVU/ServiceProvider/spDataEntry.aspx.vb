Imports System.Web.Security.AntiXss
Imports Common.ComObject
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.BankAcct
Imports Common.Component.ERNProcessed
Imports Common.Component.HCVUUser
Imports Common.Component.MedicalOrganization
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Professional
Imports Common.Component.Scheme
Imports Common.Component.SchemeInformation
Imports Common.Component.ServiceProvider
Imports Common.Component.Status
Imports Common.Component.UserRole
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports System.Data.SqlClient

Partial Public Class spDataEntry
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Inherits BasePageWithGridView
    Inherits BasePageWithControl
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    ' FunctionCode = FunctCode.FUNT010101

#Region "Fields"

    Private udtBankAcctBLL As New BankAcctBLL
    Private udtGeneralFunction As New GeneralFunction
    Private udtERNProcessedBLL As New ERNProcessedBLL
    Private udtFormatter As New Formatter
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtMedicalOrganizationBLL As New MedicalOrganizationBLL
    Private udtPracticeBLL As New PracticeBLL
    Private udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
    Private udtProfessionalBLL As New ProfessionalBLL
    Private udtSchemeInformationBLL As New SchemeInformationBLL
    Private udtServiceProviderBLL As New ServiceProviderBLL
    Private udtSPProfileBLL As New SPProfileBLL
    Private udtUserRoleBLL As New UserRoleBLL
    Private udtValidator As New Validator

    Private udtAuditLogEntry As AuditLogEntry
    Private SM As SystemMessage
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    Private _strERN As String = String.Empty
    Private _blnOverrideResultLimit As Boolean = False
    Private _blnUseHKIDSearch As Boolean
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

#End Region

#Region "Constants"

    Private Const ViewIndexSearchCriteria As Integer = 0
    Private Const ViewIndexSearchResult As Integer = 1
    Private Const ViewIndexSearchBatchResult As Integer = 2
    Private Const ViewIndexMigrateHCVS As Integer = 3
    Private Const ViewIndexMigrateIVSS As Integer = 4
    Private Const ViewIndexMigrateNeed As Integer = 5
    Private Const ViewIndexError As Integer = 6

    Private Const strFuncCode As String = FunctCode.FUNT010101

    Private Const strLinkToSPProfile As String = "~/ServiceProvider/spProfile.aspx"
    Private Const strLinkToSPMigration As String = "~/ServiceProvider/spMigration.aspx"

    Private Const strActionNew As String = "New"
    Private Const strActionSearch As String = "Search"
    Private Const strRecordStatusUnderAmendment = "U"
    Private Const strYes As String = "Y"
    Private Const strNo As String = "N"

    Private Const intUserRoleTypeSPMigration = 11

    Private Const intGvResultColumn_MergeCheckBox As Integer = 1
    Private Const intGvResultColumn_ServiceProviderID As Integer = 3
    Private Const intGvResultColumn_DateEntryProcessingTime As Integer = 5
    Private Const intGvResultColumn_Status As Integer = 9

    Private Const DefaultGridViewPageSize = 10
    Private Const ExtendedGridViewPageSize = 255

#End Region

#Region "Session Constants"

    Public Const SESS_DataEntryResult As String = "DataEntryResult"
    Public Const SESS_BackToDataEntryPage As String = "BackToDataEntryPage"

    Private Const SESS_Action As String = "EnrolAction"
    Private Const SESS_ERN As String = "Enrolment_Ref_No"
    Private Const SESS_TableLocation As String = "TableLocation"
    Private Const SESS_DataEntrySearchCriteria As String = "DataEntrySearchCriteria"
    Private Const SESS_MigrateHCVSERN As String = "MigrateHCVSERN"
    Private Const SESS_MigrateIVSSERN As String = "MigrateIVSSERN"
    Private Const SESS_MigrateIVSSSkip As String = "MigrateIVSSSkip"
    Private Const SESS_DataEntryMigrationSP As String = "DataEntryMigrationSP"
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    Private Const SESS_FromTaskList As String = "010101_FromTaskList"
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT010101

            ' Initialize the IVSS skip to "N"
            Session(SESS_MigrateIVSSSkip) = strNo

            ' Begin writing Audit Log
            udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Service Provider - Data Entry loaded")

            ' Bind Health Profession
            ddlSPHealthProf.DataSource = udtSPProfileBLL.GetHealthProf

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

            ' -----------------------------------------------------------------------------------------

            ddlSPHealthProf.DataValueField = "ServiceCategoryCode"
            ddlSPHealthProf.DataTextField = "ServiceCategoryDesc"

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            ddlSPHealthProf.DataBind()

            ' Bind Status
            ddlStatus.DataSource = GetDescriptionListFromDBEnumCode("SPDataEntryStatus")
            ddlStatus.DataValueField = "Status_Value"
            ddlStatus.DataTextField = "Status_Description"
            ddlStatus.DataBind()

            ' Bind Scheme
            ddlScheme.DataSource = udtSPProfileBLL.GetMasterScheme
            ddlScheme.DataValueField = "SchemeCode"
            ddlScheme.DataTextField = "DisplayCode"
            ddlScheme.DataBind()

            If Not Session(SESS_BackToDataEntryPage) Then
                Session.Remove(SESS_DataEntryResult)
                Session.Remove(SESS_DataEntrySearchCriteria)
            End If

            Session.Remove(SESS_BackToDataEntryPage)

            If Session("fromMain") = "Y" Then
                Session("fromMain") = Nothing

                If Session("SearchPage") = "N" Then
                    ' Search Result
                    Session("SearchPage") = Nothing
                    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    Session(SESS_FromTaskList) = "Y"
                    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
                    Me.ddlStatus.SelectedValue = "P  "
                    ibtnSearch_Click(Nothing, Nothing)
                Else
                    ' Unprocessed Enrolment
                    ddlStatus.SelectedValue = "U  "
                    Session("SearchPage") = Nothing
                End If

            End If

            If IsNothing(Session(SESS_DataEntryResult)) Then
                MultiViewDataEntry.ActiveViewIndex = ViewIndexSearchCriteria

                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Change default Status from "Unprocessed" to "Any"
                'ddlStatus.SelectedValue = "U  "
                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]
            Else
                Session.Remove(SESS_DataEntryResult)
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                _blnOverrideResultLimit = True
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
                ReloadSearchFromSession()

            End If

            ' Prevent multi-click
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnPopupMigrateHCVSConfirm)
            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnPopupMigrateIVSSConfirm)
            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnProceedMergeConfirm)

        End If

        If MultiViewDataEntry.ActiveViewIndex = ViewIndexSearchCriteria Then
            Session.Remove(SESS_DataEntryResult)
            Session.Remove(SESS_DataEntrySearchCriteria)
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case MultiViewDataEntry.ActiveViewIndex
            Case ViewIndexSearchCriteria
                'CRE17-008 (Remind Delist Practice) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If Not IsPopupShow() Then
                    ScriptManager1.SetFocus(txtEnrolRefNo)
                    pnlDataEntry.DefaultButton = ibtnSearch.ID
                End If
                'CRE17-008 (Remind Delist Practice) [End][Chris YIM]
            Case ViewIndexSearchResult
                ScriptManager1.SetFocus(btnHidden)
        End Select
    End Sub

#End Region

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
#Region "Abstract Method of [HCVU.BasePageWithControl]"
    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)
        _blnUseHKIDSearch = txtSPHKID.Text.Trim <> String.Empty
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As AuditLogEntry) As Boolean
        Return True
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        ' ----- Search the result list from user input -----
        Return udtSPProfileBLL.DataEntrySearch(strFuncCode, _strERN, txtSPID.Text.Trim, udtFormatter.formatHKIDInternal(txtSPHKID.Text.Trim), _
                                                    txtSPName.Text.Trim, txtPhone.Text.Trim, ddlSPHealthProf.SelectedValue.Trim, _
                                                    ddlStatus.SelectedValue.Trim, ddlScheme.SelectedValue.Trim, blnOverrideResultLimit)

    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer
        Dim dt As DataTable
        Dim intRowCount As Integer

        dt = CType(udtBLLSearchResult.Data, DataTable)
        intRowCount = dt.Rows.Count

        ' Reset the result gridview
        ibtnNewEnrolment.Visible = False
        ibtnProceed.Visible = False

        panSearchCriteriaReview.Visible = True
        panSearchCriteriaProcessing.Visible = False
        panSearchCriteriaEnrolled.Visible = False

        Dim blnKeyFieldSearch As Boolean = txtEnrolRefNo.Text <> String.Empty OrElse txtSPID.Text <> String.Empty OrElse txtSPHKID.Text <> String.Empty

        Select Case intRowCount
            Case 0
                ' ----- 1. No Record Found -----
                Session(SESS_MigrateHCVSERN) = String.Empty

                If txtSPHKID.Text.Trim <> String.Empty AndAlso Not udtSPProfileBLL.IsHKIDExistingInServiceProviderStagingPermanentByHKID(txtSPHKID.Text.Trim) Then
                    ' ----- 1.1 HKIC is inputted as a search key AND the HKIC No. is neither an enrolled SP nor under processing -----
                    ' -----     (a) Enable "New Enrolment" Button -----
                    ' -----     (b) Show no corresponding HKIC found message -----

                    FillSearchCriteria()
                    gvResult.Visible = False
                    ibtnNewEnrolment.Visible = True
                    panHKIDNoRecord.Visible = True

                    MultiViewDataEntry.ActiveViewIndex = ViewIndexSearchResult

                Else
                    ' ----- 1.2 HKIC is not inputted as a search key -----
                    ' -----     (a) Display No Record Found in the information box -----

                    CompleteMsgBox.AddMessage(New SystemMessage("990000", "I", "00001"))
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

                    MultiViewDataEntry.ActiveViewIndex = ViewIndexSearchCriteria

                End If

            Case 1
                ' ----- 2. Only One Record Found -----
                Dim dr As DataRow = dt.Rows(0)

                Session(SESS_MigrateHCVSERN) = CStr(dr("Enrolment_Ref_No")).Trim

                ' First check for Migration records, because if HKID is used as a search criterion, it will be handled outside the Select statements
                If Not _blnUseHKIDSearch Then
                    'If Session(SESS_TurnOnDataMigration) = strYes Then
                    '    If SearchMigrateNeed(CStr(dr("SP_HKID")).Trim) Then Return intRowCount
                    '    If SearchMigrateHCVS(CStr(dr("SP_HKID")).Trim) Then Return intRowCount
                    'End If

                    'If Session(SESS_TurnOnIVSSDataMigration) = strYes Then
                    '    If SearchMigrateIVSS(CStr(dr("SP_HKID")).Trim) Then Return intRowCount
                    'End If
                End If

                ' -----     (a) Check the Record whether is in Unprocessed or not -----
                If CStr(dr("Table_Location")).Trim = TableLocation.Enrolment AndAlso txtEnrolRefNo.Text.Trim = String.Empty Then
                    ' The record is unprocessed
                    ' Enable "New Enrolment" Button when not using HKID as search key
                    FillSearchCriteria()
                    ListResult(dt, True, False)

                Else
                    SearchUnprocessedRecord(dr("Enrolment_Ref_No"), dr("Table_Location"), dr("SP_HKID"), blnKeyFieldSearch)
                End If

            Case Else
                ' ----- 3. More than one record found, list all results -----
                Dim dtNonEnrolment As DataTable = DataTableFilter(dt, "Table_Location <> '" + TableLocation.Enrolment + "'")

                If dtNonEnrolment.Rows.Count = 0 Then
                    ' If all records are unprocessed
                    ListResult(dt, True, False)
                Else
                    ' At least one record is "Enrolled" or "Processing"
                    If blnKeyFieldSearch Then
                        Dim drNonEnrolment As DataRow = dtNonEnrolment.Rows(0)
                        Session(SESS_MigrateHCVSERN) = CStr(drNonEnrolment("Enrolment_Ref_No")).Trim
                        SearchUnprocessedRecord(drNonEnrolment("Enrolment_Ref_No"), drNonEnrolment("Table_Location"), drNonEnrolment("SP_HKID"), True)
                    Else
                        ListResult(dt, False, False)
                    End If

                End If

        End Select

        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        If Not IsNothing(Session(SESS_FromTaskList)) Then
            If Session(SESS_FromTaskList) = "Y" Then
                Me.ddlStatus.SelectedValue = "P  "
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

        Try
            If Not IsNothing(sender) Then Session(SESS_MigrateIVSSSkip) = strNo

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            _strERN = String.Empty
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            txtEnrolRefNo.Text = UCase(AntiXssEncoder.HtmlEncode(txtEnrolRefNo.Text, True))
            ' I-CRE16-003 Fix XSS [End][Lawrence]
            txtSPHKID.Text = UCase(txtSPHKID.Text.Trim)

            If txtEnrolRefNo.Text.Trim <> String.Empty Then
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

            If IsNothing(Session(SESS_DataEntrySearchCriteria)) OrElse Session(SESS_DataEntrySearchCriteria).ToString.Split("^").Length <> 2 Then
                Session(SESS_DataEntrySearchCriteria) = txtEnrolRefNo.Text.Trim + "|" + _
                                                        txtSPID.Text.Trim + "|" + _
                                                        txtSPHKID.Text.Trim + "|" + _
                                                        txtSPName.Text.Trim + "|" + _
                                                        txtPhone.Text.Trim + "|" + _
                                                        ddlSPHealthProf.SelectedValue.Trim + "|" + _
                                                        ddlStatus.SelectedValue + "|" + _
                                                        ddlScheme.SelectedValue.Trim
            End If

            udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
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
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search", New AuditLogInfo(txtSPID.Text.Trim, txtSPHKID.Text.Trim, "", "", "", ""))

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            Dim enumSearchResult As SearchResultEnum

            If IsNothing(sender) AndAlso _blnOverrideResultLimit = True Then
                enumSearchResult = StartSearchFlow(strFuncCode, udtAuditLogEntry, msgBox, Nothing, True, True)
            Else
                enumSearchResult = StartSearchFlow(strFuncCode, udtAuditLogEntry, msgBox, Nothing)
            End If

            Select Case enumSearchResult
                Case SearchResultEnum.Success
                    ' Audit Log has been handled locally

                Case SearchResultEnum.ValidationFail
                    ' This case has been handled locally

                Case SearchResultEnum.NoRecordFound
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search completed. No record found")

                Case SearchResultEnum.OverResultList1stLimit_PopUp
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")

                Case SearchResultEnum.OverResultList1stLimit_Alert
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")

                Case SearchResultEnum.OverResultListOverrideLimit
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00004, "Search failed")

                Case Else
                    Throw New Exception("Error: Class = [HCVU.spDataEntry], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

            End Select

            'Dim blnUseHKIDSearch As Boolean = txtSPHKID.Text.Trim <> String.Empty

            ' If HKIC No. is used as a search criterion, search for Migration records
            'If blnUseHKIDSearch Then
            'If Session(SESS_TurnOnDataMigration) = strYes Then
            'If SearchMigrateNeed(txtSPHKID.Text.Trim) Then Return
            'If SearchMigrateHCVS(txtSPHKID.Text.Trim) Then Return
            'End If

            'If Session(SESS_TurnOnIVSSDataMigration) = strYes Then
            'If SearchMigrateIVSS(txtSPHKID.Text.Trim) Then Return
            'End If
            'End If

            ' ----- Search the result list from user input -----
            'Dim dt As DataTable = udtSPProfileBLL.DataEntrySearch(strERN, txtSPID.Text.Trim, udtFormatter.formatHKIDInternal(txtSPHKID.Text.Trim), _
            '                                                        txtSPName.Text.Trim, txtPhone.Text.Trim, ddlSPHealthProf.SelectedValue.Trim, _
            '                                                        ddlStatus.SelectedValue.Trim, ddlScheme.SelectedValue.Trim)

            ' Reset the result gridview
            'ibtnNewEnrolment.Visible = False
            'ibtnProceed.Visible = False

            'panSearchCriteriaReview.Visible = True
            'panSearchCriteriaProcessing.Visible = False
            'panSearchCriteriaEnrolled.Visible = False

            'Dim blnKeyFieldSearch As Boolean = txtEnrolRefNo.Text <> String.Empty OrElse txtSPID.Text <> String.Empty OrElse txtSPHKID.Text <> String.Empty

            'Select Case dt.Rows.Count
            'Case 0
            ' ----- 1. No Record Found -----
            'Session(SESS_MigrateHCVSERN) = String.Empty

            'If txtSPHKID.Text.Trim <> String.Empty AndAlso Not udtSPProfileBLL.IsHKIDExistingInServiceProviderStagingPermanentByHKID(txtSPHKID.Text.Trim) Then
            ' ----- 1.1 HKIC is inputted as a search key AND the HKIC No. is neither an enrolled SP nor under processing -----
            ' -----     (a) Enable "New Enrolment" Button -----
            ' -----     (b) Show no corresponding HKIC found message -----

            'FillSearchCriteria()
            'gvResult.Visible = False
            'ibtnNewEnrolment.Visible = True
            'panHKIDNoRecord.Visible = True

            'MultiViewDataEntry.ActiveViewIndex = ViewIndexSearchResult

            'Else
            ' ----- 1.2 HKIC is not inputted as a search key -----
            ' -----     (a) Display No Record Found in the information box -----



            'CompleteMsgBox.AddMessage(New SystemMessage("990000", "I", "00001"))
            'CompleteMsgBox.BuildMessageBox()
            'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

            'MultiViewDataEntry.ActiveViewIndex = ViewIndexSearchCriteria

            'End If

            'udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search completed. No record found")

            'Case 1
            ' ----- 2. Only One Record Found -----
            'Dim dr As DataRow = dt.Rows(0)

            'Session(SESS_MigrateHCVSERN) = CStr(dr("Enrolment_Ref_No")).Trim

            ' First check for Migration records, because if HKID is used as a search criterion, it will be handled outside the Select statements
            'If Not blnUseHKIDSearch Then
            'If Session(SESS_TurnOnDataMigration) = strYes Then
            'If SearchMigrateNeed(CStr(dr("SP_HKID")).Trim) Then Return
            'If SearchMigrateHCVS(CStr(dr("SP_HKID")).Trim) Then Return
            'End If

            'If Session(SESS_TurnOnIVSSDataMigration) = strYes Then
            'If SearchMigrateIVSS(CStr(dr("SP_HKID")).Trim) Then Return
            'End If
            'End If

            ' -----     (a) Check the Record whether is in Unprocessed or not -----
            'If CStr(dr("Table_Location")).Trim = TableLocation.Enrolment AndAlso txtEnrolRefNo.Text.Trim = String.Empty Then
            ' The record is unprocessed
            ' Enable "New Enrolment" Button when not using HKID as search key
            'FillSearchCriteria()
            'ListResult(dt, True, False)

            'Else
            'SearchUnprocessedRecord(dr("Enrolment_Ref_No"), dr("Table_Location"), dr("SP_HKID"), blnKeyFieldSearch)
            'End If

            'Case Else
            ' ----- 3. More than one record found, list all results -----
            'Dim dtNonEnrolment As DataTable = DataTableFilter(dt, "Table_Location <> '" + TableLocation.Enrolment + "'")

            'If dtNonEnrolment.Rows.Count = 0 Then
            ' If all records are unprocessed
            'ListResult(dt, True, False)
            'Else
            ' At least one record is "Enrolled" or "Processing"
            'If blnKeyFieldSearch Then
            'Dim drNonEnrolment As DataRow = dtNonEnrolment.Rows(0)
            'Session(SESS_MigrateHCVSERN) = CStr(drNonEnrolment("Enrolment_Ref_No")).Trim
            'SearchUnprocessedRecord(drNonEnrolment("Enrolment_Ref_No"), drNonEnrolment("Table_Location"), drNonEnrolment("SP_HKID"))
            'Else
            'ListResult(dt, False, False)
            'End If

            'End If

            'End Select

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

        Catch ex As Exception
            Throw ex

        End Try
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    End Sub

    Private Sub ReloadSearchFromSession()
        Dim arySearchCriteria As String()

        If Session(SESS_DataEntrySearchCriteria).ToString.Split("^").Length > 1 Then
            arySearchCriteria = Session(SESS_DataEntrySearchCriteria).ToString.Split("^")(0).Split("|")
        Else
            arySearchCriteria = Session(SESS_DataEntrySearchCriteria).ToString.Split("|")
        End If

        txtEnrolRefNo.Text = arySearchCriteria(0)
        txtSPID.Text = arySearchCriteria(1)
        txtSPHKID.Text = arySearchCriteria(2)
        txtSPName.Text = arySearchCriteria(3)
        txtPhone.Text = arySearchCriteria(4)
        ddlSPHealthProf.SelectedValue = arySearchCriteria(5)
        ddlStatus.SelectedValue = arySearchCriteria(6)
        ddlScheme.SelectedValue = arySearchCriteria(7)

        ibtnSearch_Click(Nothing, Nothing)

        arySearchCriteria = Session(SESS_DataEntrySearchCriteria).ToString.Split("^")

        If arySearchCriteria.Length = 1 Then
            'CompleteMsgBox.Visible = False

            'If MultiViewDataEntry.ActiveViewIndex = SearchCriteria Then
            '    ResetSearchCriteria()
            'End If
        Else
            '---------------------------------------------
            ' Start of INT11-0011
            '---------------------------------------------
            Dim arySelectCriteria As String() = arySearchCriteria(1).Split("|")

            Dim blnAbleToFind As Boolean = False
            blnAbleToFind = udtSPProfileBLL.GetServiceProviderProfile(arySelectCriteria(0), arySelectCriteria(1))

            If blnAbleToFind Then
                SearchUnprocessedRecord(arySelectCriteria(0), arySelectCriteria(1), arySelectCriteria(2), True)
            Else
                arySelectCriteria = arySearchCriteria(0).Split("|")
                txtEnrolRefNo.Text = arySelectCriteria(0)
                txtSPID.Text = arySelectCriteria(1)
                txtSPHKID.Text = arySelectCriteria(2)
                txtSPName.Text = arySelectCriteria(3)
                txtPhone.Text = arySelectCriteria(4)
                ddlSPHealthProf.SelectedValue = arySelectCriteria(5)
                ddlStatus.SelectedValue = arySelectCriteria(6)
                ddlScheme.SelectedValue = arySelectCriteria(7)

                ibtnSearch_Click(Nothing, Nothing)
            End If
            '---------------------------------------------
            ' End of INT11-0011
            '--------------------------------------------
        End If
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

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

            ' -----------------------------------------------------------------------------------------

            lblResultHealthProf.Text = udtSPProfileBLL.GetHealthProfName(ddlSPHealthProf.SelectedValue).ServiceCategoryDesc

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

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

    Private Sub ListResult(ByVal dt As DataTable, Optional ByVal blnShowNewEnrolment As Boolean = False, Optional ByVal blnShowPnlHKIC As Boolean = False, Optional ByVal blnSameHKIC As Boolean = False)
        FillSearchCriteria()

        gvResult.Visible = True
        ibtnNewEnrolment.Visible = blnShowNewEnrolment
        panHKIDNoRecord.Visible = blnShowPnlHKIC

        If blnSameHKIC Then
            gvResult.Columns(intGvResultColumn_MergeCheckBox).Visible = True
            gvResult.Columns(intGvResultColumn_ServiceProviderID).Visible = False
            gvResult.Columns(intGvResultColumn_DateEntryProcessingTime).Visible = False
            gvResult.Columns(intGvResultColumn_Status).Visible = False
            gvResult.PageSize = ExtendedGridViewPageSize
        Else
            gvResult.Columns(intGvResultColumn_MergeCheckBox).Visible = False
            gvResult.Columns(intGvResultColumn_ServiceProviderID).Visible = True
            gvResult.Columns(intGvResultColumn_DateEntryProcessingTime).Visible = True
            gvResult.Columns(intGvResultColumn_Status).Visible = True
            gvResult.PageSize = DefaultGridViewPageSize
        End If

        Session(SESS_DataEntryResult) = dt
        Me.GridViewDataBind(gvResult, dt, "Enrolment_Dtm", "ASC", False)

        MultiViewDataEntry.ActiveViewIndex = ViewIndexSearchResult

        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search completed")

    End Sub

    Private Sub RedirectToSPProfileFromSearch(ByVal strERN As String, ByVal strTableLocation As String)
        Session(SESS_Action) = strActionSearch
        Session(SESS_ERN) = strERN
        Session(SESS_TableLocation) = strTableLocation

        Response.Redirect(strLinkToSPProfile)
    End Sub

    '

    Private Function SearchCompleteMOPractice(ByVal strERN As String, ByVal strTableLocation As String) As Boolean
        Dim udtDB As New Database

        ' Add the rules that if no practice in the model, then it is count as complete model ==> Don't force user to go to migration page
        Select Case strTableLocation
            Case TableLocation.Staging
                Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderStagingProfileByERN(strERN, udtDB)

                If IsNothing(udtSP.PracticeList) OrElse udtSP.PracticeList.Count = 0 Then Return True
                If IsNothing(udtSP.MOList) OrElse udtSP.MOList.Count = 0 Then Return False

                For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
                    If udtPractice.MODisplaySeq = 0 Then Return False
                Next

                Return True

            Case TableLocation.Permanent
                Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(strERN, udtDB)

                If IsNothing(udtSP.PracticeList) OrElse udtSP.PracticeList.Count = 0 Then Return True
                If IsNothing(udtSP.MOList) OrElse udtSP.MOList.Count = 0 Then Return False

                For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
                    If udtPractice.MODisplaySeq = 0 Then Return False
                Next

                Return True

        End Select

        Return Nothing

    End Function

    Private Sub SearchUnprocessedRecord(ByVal strERN As String, ByVal strTableLocation As String, ByVal strHKID As String, Optional ByVal blnKeyFieldSearch As Boolean = True)
        ' Find the HKIC No. from the ERN
        If strHKID = String.Empty Then
            udtSPProfileBLL.GetServiceProviderHKID(strERN, String.Empty, strHKID, Nothing)
        Else
            strHKID = udtFormatter.formatHKIDInternal(strHKID)
        End If

        ' Use this HKIC No. to search records
        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Dim dtKeySearch As DataTable = udtSPProfileBLL.DataEntrySearch(String.Empty, _
        '                                                                String.Empty, strHKID, String.Empty, String.Empty, _
        '                                                                String.Empty, String.Empty, String.Empty)

        Dim dtKeySearch As DataTable
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

        udtBLLSearchResult = udtSPProfileBLL.DataEntrySearch(strFuncCode, String.Empty, _
                                                             String.Empty, strHKID, String.Empty, String.Empty, _
                                                             String.Empty, String.Empty, String.Empty, True, True)

        dtKeySearch = CType(udtBLLSearchResult.Data, DataTable)

        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        If dtKeySearch.Rows.Count = 1 Then
            ' If only one record
            Dim arySearchCriteria As String() = Session(SESS_DataEntrySearchCriteria).ToString.Split("^")
            If arySearchCriteria.Length > 1 Then
                Session(SESS_DataEntrySearchCriteria) = arySearchCriteria(0)
            End If

            If blnKeyFieldSearch Then
                RedirectToSPProfileFromSearch(strERN, strTableLocation)
            Else
                ListResult(dtKeySearch, False, False)
            End If

            Return
        End If

        ' Get the non-enrolment records (Processing and Enrolled)
        Dim dtNonEnrolment As DataTable = DataTableFilter(dtKeySearch, "Table_Location <> '" + TableLocation.Enrolment + "'")

        If dtNonEnrolment.Rows.Count = 0 Then
            ' If no unprocessed records, redirect to SP Profile
            Dim arySearchCriteria As String() = Session(SESS_DataEntrySearchCriteria).ToString.Split("^")
            If arySearchCriteria.Length > 1 Then
                Session(SESS_DataEntrySearchCriteria) = arySearchCriteria(0)
            End If

            RedirectToSPProfileFromSearch(strERN, strTableLocation)
            Return
        End If

        Dim drNonEnrolment As DataRow = dtNonEnrolment.Rows(0)

        Select Case drNonEnrolment("Table_Location")
            Case TableLocation.Staging
                FillSearchCriteriaProcessing(drNonEnrolment)
            Case TableLocation.Permanent
                FillSearchCriteriaEnrolled(drNonEnrolment)
        End Select

        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("ERN", drNonEnrolment("Enrolment_Ref_No"))
        udtAuditLogEntry.AddDescripton("SPID", drNonEnrolment("SP_ID"))
        udtAuditLogEntry.AddDescripton("HKID", drNonEnrolment("SP_HKID"))
        udtAuditLogEntry.AddDescripton("TableLocation", drNonEnrolment("Table_Location"))
        If Not IsNothing(drNonEnrolment("SP_ID")) AndAlso drNonEnrolment("SP_ID").ToString().Trim = "" Then
            udtAuditLogEntry.WriteStartLog(LogID.LOG00074, "Merge record found", New Common.ComObject.AuditLogInfo("", drNonEnrolment("SP_HKID"), "", "", "", ""))
        Else
            udtAuditLogEntry.WriteStartLog(LogID.LOG00074, "Merge record found", New Common.ComObject.AuditLogInfo(drNonEnrolment("SP_ID"), drNonEnrolment("SP_HKID"), "", "", "", ""))
        End If
        ListResult(DataTableFilter(dtKeySearch, "Table_Location = '" + TableLocation.Enrolment + "'"), False, False, True)

        HighlightAndCheckMatchLine(strERN)

    End Sub

    '

    Private Function DataTableFilter(ByVal dt As DataTable, ByVal strExpression As String) As DataTable
        Dim dtRes As DataTable = dt.Clone

        For Each dr As DataRow In dt.Select(strExpression)
            dtRes.ImportRow(dr)
        Next

        Return dtRes

    End Function

    '

    Protected Sub ibtnNewEnrolment_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Session(SESS_Action) = strActionNew
        Session.Remove(SESS_ERN)
        Session.Remove(SESS_TableLocation)

        Response.Redirect(strLinkToSPProfile)

    End Sub

    '

    Private Sub FillSearchCriteriaProcessing(ByVal dr As DataRow)
        panSearchCriteriaReview.Visible = False
        panSearchCriteriaProcessing.Visible = True

        If CStr(dr("SP_ID")).Trim = String.Empty Then
            ' Enrolment Reference No.
            lblSameHKICPERNSPIDText.Text = Me.GetGlobalResourceObject("Text", "EnrolRefNo")
            lnkSameHKICPERNSPID.Text = udtFormatter.formatSystemNumber(CStr(dr("Enrolment_Ref_No")).Trim)
        Else
            ' Service Provider ID
            lblSameHKICPERNSPIDText.Text = Me.GetGlobalResourceObject("Text", "SPID")
            lnkSameHKICPERNSPID.Text = CStr(dr("SP_ID")).Trim
        End If

        lnkSameHKICPERNSPID.CommandArgument = CStr(dr("Enrolment_Ref_No")).Trim + "|" + CStr(dr("Table_Location")).Trim


        ' SPID (if any)
        hfSameHKICPSPID.Value = CStr(dr("SP_ID")).Trim

        ' Service Provider HKIC No.
        lblSameHKICPSPHKIC.Text = udtFormatter.formatHKID(CStr(dr("SP_HKID")).Trim, False)
        hfSameHKICPSPHKIC.Value = CStr(dr("SP_HKID")).Trim

        ' Service Provider Name
        lblSameHKICPSPName.Text = CStr(dr("SP_Eng_Name")).Trim
        Dim strSPChiName As String = CStr(dr("SP_Chi_Name")).Trim
        If strSPChiName <> String.Empty Then
            lblSameHKICPSPName.Text += " (" + strSPChiName + ")"
        End If

        ' Status
        lblSameHKICPStatus.Text = ConvertStatusCodeToValue(CStr(dr("Table_Location")).Trim)

        ' Show the Proceed button
        ibtnProceed.Visible = True

    End Sub

    Private Sub FillSearchCriteriaEnrolled(ByVal dr As DataRow)
        panSearchCriteriaReview.Visible = False
        panSearchCriteriaEnrolled.Visible = True

        ' Service Provider ID
        lnkSameHKICESPID.Text = CStr(dr("SP_ID")).Trim
        lnkSameHKICESPID.CommandArgument = CStr(dr("Enrolment_Ref_No")).Trim + "|" + CStr(dr("Table_Location")).Trim

        ' Service Provider HKIC No.
        lblSameHKICESPHKIC.Text = udtFormatter.formatHKID(CStr(dr("SP_HKID")).Trim, False)
        hfSameHKICPSPHKIC.Value = CStr(dr("SP_HKID")).Trim

        ' Service Provider Name
        lblSameHKICESPName.Text = CStr(dr("SP_Eng_Name")).Trim
        Dim strSPChiName As String = CStr(dr("SP_Chi_Name")).Trim
        If strSPChiName <> String.Empty Then
            lblSameHKICESPName.Text += " (" + strSPChiName + ")"
        End If

        ' Status
        lblSameHKICEStatus.Text = ConvertStatusCodeToValue(CStr(dr("Table_Location")).Trim)

        ' Show the Proceed button
        ibtnProceed.Visible = True

    End Sub

    Private Sub HighlightAndCheckMatchLine(ByVal strERN As String)
        For Each r As GridViewRow In gvResult.Rows
            If CType(r.FindControl("lnkbtnERN"), LinkButton).CommandArgument = strERN Then
                r.BackColor = Drawing.Color.MistyRose
                CType(r.FindControl("cboMerge"), CheckBox).Checked = True
                Return
            End If
        Next
    End Sub

    '

    Private Sub ShowMigrateNeed(ByVal dr As DataRow)
        If CStr(dr("SP_ID")).Trim = String.Empty Then
            ' Enrolment Reference No.
            lblMigrateNeedERNSPIDText.Text = Me.GetGlobalResourceObject("Text", "EnrolRefNo")
            lblMigrateNeedERNSPID.Text = udtFormatter.formatSystemNumber(CStr(dr("Enrolment_Ref_No")).Trim).Trim
        Else
            ' Service Provider ID
            lblMigrateNeedERNSPIDText.Text = Me.GetGlobalResourceObject("Text", "SPID")
            lblMigrateNeedERNSPID.Text = CStr(dr("SP_ID")).Trim
        End If

        ' Service Provider Name
        lblMigrateNeedSPName.Text = CStr(dr("SP_Eng_Name")).Trim
        lblMigrateNeedSPNameChi.Text = udtFormatter.formatChineseName(CStr(dr("SP_Chi_Name")).Trim)

        ' Service Provider HKIC No.
        lblMigrateNeedSPHKID.Text = udtFormatter.formatHKID(CStr(dr("SP_HKID")).Trim, False).Trim

        ' Check the User Role is valid to go to Data Migration page
        Dim blnValidUser As Boolean = False
        For Each udtUserRole As UserRoleModel In udtUserRoleBLL.GetUserRoleCollection(udtHCVUUserBLL.GetHCVUUser.UserID).Values
            If udtUserRole.RoleType = intUserRoleTypeSPMigration Then
                blnValidUser = True
                Exit For
            End If
        Next

        lblMigrateNeedDataMigration.Visible = blnValidUser
        lblMigrateNeedDataMigrationNoRight.Visible = Not blnValidUser
        ibtnMigrateNeedDataMigration.Enabled = blnValidUser
        ibtnMigrateNeedDataMigration.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnMigrateNeedDataMigration.Enabled, "DataMigrationBtn", "DataMigrationDisableBtn"))

        MultiViewDataEntry.ActiveViewIndex = ViewIndexMigrateNeed

        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("ERN", CStr(dr("Enrolment_Ref_No")).Trim)
        udtAuditLogEntry.AddDescripton("ValidToAccessDataMigration", IIf(blnValidUser, "Y", "N"))
        udtAuditLogEntry.WriteStartLog(LogID.LOG00075, "Data migration needed")

    End Sub

    Private Sub ShowMigrateHCVS(ByVal dr As DataRow)
        Session(SESS_MigrateHCVSERN) = CStr(dr("Enrolment_Ref_No")).Trim

        If CStr(dr("SP_ID")).Trim = String.Empty Then
            ' Enrolment Reference No.
            lblMigrateHCVSERNSPIDText.Text = Me.GetGlobalResourceObject("Text", "EnrolRefNo")
            lblMigrateHCVSERNSPID.Text = udtFormatter.formatSystemNumber(CStr(dr("Enrolment_Ref_No")).Trim).Trim
        Else
            ' Service Provider ID
            lblMigrateHCVSERNSPIDText.Text = Me.GetGlobalResourceObject("Text", "SPID")
            lblMigrateHCVSERNSPID.Text = CStr(dr("SP_ID")).Trim
        End If

        ' Service Provider Name
        lblMigrateHCVSSPName.Text = CStr(dr("SP_Eng_Name")).Trim
        lblMigrateHCVSSPNameChi.Text = udtFormatter.formatChineseName(CStr(dr("SP_Chi_Name")).Trim)

        ' Service Provider HKIC No.
        lblMigrateHCVSHKID.Text = udtFormatter.formatHKID(CStr(dr("SP_HKID")).Trim, False).Trim

        MultiViewDataEntry.ActiveViewIndex = ViewIndexMigrateHCVS

        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("ERN", CStr(dr("Enrolment_Ref_No")).Trim)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00076, "HCVS migration record found")

    End Sub

    Private Sub ShowMigrateIVSS(ByVal dr As DataRow)
        Dim strERN As String = CStr(dr("Enrolment_Ref_No")).Trim
        Session(SESS_MigrateIVSSERN) = strERN

        ' Enrolment Reference No.
        lblMigrateIVSSERN.Text = udtFormatter.formatSystemNumber(strERN).Trim

        ' Service Provider Name
        lblMigrateIVSSSPName.Text = CStr(dr("SP_Eng_Name")).Trim
        lblMigrateIVSSSPNameChi.Text = udtFormatter.formatChineseName(CStr(dr("SP_Chi_Name")).Trim)

        ' Service Provider HKIC No.
        lblMigrateIVSSHKID.Text = udtFormatter.formatHKID(CStr(dr("SP_HKID")).Trim, False).Trim

        MultiViewDataEntry.ActiveViewIndex = ViewIndexMigrateIVSS

        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.AddDescripton("ERN", strERN)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00077, "IVSS migration record found")

    End Sub

    '

    Protected Sub btnSpDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    '

    Protected Sub lnkSameHKIC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim aryArgument As String() = CType(sender, LinkButton).CommandArgument.Split("|")
        RedirectToSPProfileFromSearch(aryArgument(0), aryArgument(1))
    End Sub

    ' Merge Popup

    Protected Sub ibtnProceed_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00078, "Merge Proceed", New Common.ComObject.AuditLogInfo(Nothing, hfSameHKICPSPHKIC.Value.Trim, Nothing, Nothing, Nothing, Nothing))

        msgBox.Visible = False

        ' Check exactly one checkbox is checked
        Dim intCboChecked As Integer = 0

        Dim strERN As String = String.Empty

        For Each r As GridViewRow In gvResult.Rows
            If CType(r.FindControl("cboMerge"), CheckBox).Checked Then
                intCboChecked += 1
                strERN = CType(r.FindControl("lnkbtnERN"), LinkButton).CommandArgument.Trim
            End If
        Next

        If intCboChecked <> 1 Then
            msgBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00006))
            msgBox.BuildMessageBox("ValidationFail")

            Return
        End If

        Select Case True
            Case panSearchCriteriaProcessing.Visible
                hfProceed.Value = lnkSameHKICPERNSPID.CommandArgument + "|" + strERN + "|" + hfSameHKICPSPID.Value.Trim
            Case panSearchCriteriaEnrolled.Visible
                hfProceed.Value = lnkSameHKICESPID.CommandArgument + "|" + strERN + "|" + lnkSameHKICESPID.Text.Trim
        End Select

        popupProceed.Show()

    End Sub

    Protected Sub ibtnProceedMergeConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim aryTargetERN As String() = hfProceed.Value.Split("|")

        Dim strOldERN As String = aryTargetERN(0).Trim
        Dim strOldTableLocation As String = aryTargetERN(1).Trim
        Dim strNewERN As String = aryTargetERN(2).Trim

        Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID
        Dim udtDB As New Database

        Dim strHKIC As String = hfSameHKICPSPHKIC.Value.Trim

        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00079, "Merge Confirm Click", New Common.ComObject.AuditLogInfo(Nothing, strHKIC, Nothing, Nothing, Nothing, Nothing))

        Dim udtOldSP As ServiceProviderModel = Nothing
        Dim udtOldProfList As ProfessionalModelCollection = Nothing
        Dim udtNewSP As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderEnrolmentProfileByERNInHCVU(strNewERN, udtDB)
        Dim udtNewProfList As ProfessionalModelCollection = Nothing
        Dim udtSchemeListToAdd As SchemeInformationModelCollection = Nothing
        Dim arySchemeCodeListToActive As ArrayList = Nothing
        Dim arySchemeCodeListToActiveTSMP As ArrayList = Nothing

        If IsNothing(udtNewSP) Then
            msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
            msgBox.BuildMessageBox("UpdateFail")

            MultiViewDataEntry.ActiveViewIndex = ViewIndexError

            udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
            udtAuditLogEntry.AddDescripton("NewERN", strNewERN)
            udtAuditLogEntry.WriteLog(LogID.LOG00083, "Merge failed: Cannot get new SP", New Common.ComObject.AuditLogInfo(Nothing, strHKIC, Nothing, Nothing, Nothing, Nothing))
        Else

            Select Case strOldTableLocation
                Case TableLocation.Permanent
                    udtOldSP = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(strOldERN, udtDB)
                    udtOldProfList = udtProfessionalBLL.GetProfessinalListFromPermanentBySPID(udtOldSP.SPID, udtDB)
                Case TableLocation.Staging
                    udtOldSP = udtServiceProviderBLL.GetServiceProviderStagingProfileByERN(strOldERN, udtDB)
                    udtOldProfList = udtProfessionalBLL.GetProfessinalListFromStagingByERN(udtOldSP.EnrolRefNo, udtDB)
            End Select

            udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
            udtAuditLogEntry.AddDescripton("OldERN", strOldERN)
            udtAuditLogEntry.AddDescripton("OldSPID", udtOldSP.SPID)
            udtAuditLogEntry.AddDescripton("OldHKID", udtOldSP.HKID)
            udtAuditLogEntry.AddDescripton("OldTableLocation", strOldTableLocation)
            udtAuditLogEntry.AddDescripton("NewERN", strNewERN)
            udtAuditLogEntry.AddDescripton("NewHKID", udtNewSP.HKID)

            udtAuditLogEntry.WriteStartLog(LogID.LOG00081, "Merge starts", New Common.ComObject.AuditLogInfo(udtOldSP.SPID, udtOldSP.HKID, Nothing, Nothing, Nothing, Nothing))

            Try
                udtSPProfileBLL.MergeSPModel(udtOldSP, udtOldProfList, udtNewSP, udtNewProfList, udtSchemeListToAdd, arySchemeCodeListToActive, arySchemeCodeListToActiveTSMP)

                udtDB.BeginTransaction()

                udtSPProfileBLL.WriteEnrolmentToStagingAndRemove(udtNewSP.EnrolRefNo, strUserID, udtDB)

                udtSPProfileBLL.MergeSPToDatabase(udtOldSP, strOldTableLocation = TableLocation.Permanent, udtNewSP, udtNewProfList, udtSchemeListToAdd, arySchemeCodeListToActive, arySchemeCodeListToActiveTSMP, udtDB)

                ' Delete the Enrolment record
                udtServiceProviderBLL.DeleteServiceProviderEnrolmentProfile(udtNewSP.EnrolRefNo, udtDB)

                ' Write to ERNProcessed
                Dim udtERNProcessed As New ERNProcessedModel(udtOldSP.EnrolRefNo, udtOldSP.SPID, strUserID, Nothing, udtNewSP.EnrolRefNo)
                udtERNProcessedBLL.AddERNProcessedToStaging(udtERNProcessed, udtDB)

                udtDB.CommitTransaction()

                udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00082, "Merge successful", New Common.ComObject.AuditLogInfo(udtOldSP.SPID, udtOldSP.HKID, Nothing, Nothing, Nothing, Nothing))

            Catch eSQL As SqlClient.SqlException
                udtDB.RollBackTranscation()

                If eSQL.Number = 50000 Then
                    msgBox.AddMessage(New Common.ComObject.SystemMessage("990001", "D", eSQL.Message))
                    If msgBox.GetCodeTable.Rows.Count = 0 Then
                        msgBox.Visible = False
                    Else
                        msgBox.BuildMessageBox("Merge failed", udtAuditLogEntry, LogID.LOG00083, "Merge failed", New Common.ComObject.AuditLogInfo(udtOldSP.SPID, udtOldSP.HKID, Nothing, Nothing, Nothing, Nothing))
                    End If
                Else
                    Throw eSQL
                End If

            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex

            End Try

            msgBox.Visible = False

            Session.Remove(SESS_DataEntrySearchCriteria)
            Session.Remove(SESS_DataEntryResult)
            RedirectToSPProfileFromSearch(aryTargetERN(0), TableLocation.Staging)
        End If

    End Sub

    ' Migrate Need

    Protected Sub ibtnMigrateNeedDataMigration_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        'udtAuditLogEntry.AddDescripton("HKIC", lblMigrateNeedSPHKID.Text.Trim)
        'udtAuditLogEntry.WriteLog(LogID.LOG00084, "Data Migration Click and redirect to Data Migration Page")
        'Session(spMigration.SESS_SPMigrationSearchCriteria) = lblMigrateNeedSPHKID.Text.Trim
        'Response.Redirect(strLinkToSPMigration)
    End Sub

    ' Migrate HCVS

    Protected Sub ibtnMigrateHCVSPreview_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Dim strERN As String = Session(SESS_MigrateHCVSERN)
        'Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID
        'Dim udtDB As Database = New Database

        'udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        ''udtAuditLogEntry.WriteLog(LogID.LOG00085, "HCVS migration preview click")

        'Try
        '    Dim blnRequireConvertToStaging As Boolean

        '    udtAuditLogEntry.AddDescripton("ERN", strERN)
        '    udtAuditLogEntry.WriteStartLog(LogID.LOG00085, "HCVS migration preview click and starts to load")

        '    Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderStagingByERN(strERN, udtDB)
        '    Dim udtOrigSP As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderStagingByERN(strERN, udtDB)
        '    If Not IsNothing(udtSP) Then
        '        Dim udtPracticeBLL As New PracticeBLL
        '        udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)
        '        udtOrigSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)
        '        udtSP.MOList = Nothing
        '        udtOrigSP.MOList = Nothing
        '    End If

        '    If IsNothing(udtSP) Then
        '        ' SP is in permanent
        '        udtSP = udtSPProfileBLL.GetServiceProviderPermanentProfileNoSession(strERN)
        '        udtOrigSP = udtSPProfileBLL.GetServiceProviderPermanentProfileNoSession(strERN)
        '        udtSP.MOList = Nothing
        '        udtOrigSP.MOList = Nothing
        '        blnRequireConvertToStaging = True
        '    Else
        '        ' SP is in staging
        '        blnRequireConvertToStaging = False
        '    End If

        '    udtSPProfileBLL.UpdateSPModelAfterHCVSDataMigrationForPreviewPurpose(udtFormatter.formatHKIDInternal(lblMigrateHCVSHKID.Text), _
        '                                                                            strERN, strUserID, udtSP, udtDB, blnRequireConvertToStaging)

        '    udtSP.RecordStatus = ServiceProviderStatus.Active
        '    udtSP.UnderModification = "Y"

        '    BuildHCVSPreview(udtSP)

        '    'udcSPMigrationPreview.buildSpProfileObject(udtSP, TableLocation.Staging, udtOrigSP)
        '    'udcSPMigrationPreview.DisplayRecordStatus(True, TableLocation.Staging)

        '    popupMigrateHCVSPreview.Show()
        '    udtAuditLogEntry.WriteEndLog(LogID.LOG00086, "HCVS migration preview successfully loaded")
        'Catch ex As Exception
        '    Throw ex

        'End Try

    End Sub

    Protected Sub ibtnMigrateHCVSMigrate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        'udtAuditLogEntry.AddDescripton("ERN", Session(SESS_MigrateHCVSERN).ToString.Trim)
        'udtAuditLogEntry.WriteLog(LogID.LOG00087, "HCVS migration migrate click")

        'popupMigrateHCVS.Show()
    End Sub

    Protected Sub ibtnPopupMigrateHCVSConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Dim udtDB As New Database
        'Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        ''udtAuditLogEntry.WriteLog(LogID.LOG00088, "HCVS migration migrate - confirm click")

        'Try
        '    Dim strERN As String = Session(SESS_MigrateHCVSERN)
        '    Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID

        '    udtDB.BeginTransaction()
        '    udtAuditLogEntry.AddDescripton("ERN", strERN)
        '    udtAuditLogEntry.WriteStartLog(LogID.LOG00088, "HCVS migration migrate - confirm click and execute")

        '    Dim udtExistingSP As ServiceProviderModel = udtSPProfileBLL.GetServiceProviderStagingProfileNoSessionForMigration(strERN, udtDB)

        '    If IsNothing(udtExistingSP) Then
        '        udtExistingSP = udtSPProfileBLL.GetServiceProviderPermanentProfile(strERN)

        '        ' Set ERN to Schemes
        '        For Each udtExistingScheme As SchemeInformationModel In udtExistingSP.SchemeInfoList.Values
        '            udtExistingScheme.EnrolRefNo = udtExistingSP.EnrolRefNo
        '        Next

        '        ' Set ERN to MOs
        '        For Each udtExistingMO As MedicalOrganizationModel In udtExistingSP.MOList.Values
        '            udtExistingMO.EnrolRefNo = udtExistingSP.EnrolRefNo
        '        Next

        '        ' Set ERN to Practices
        '        For Each udtExistingPracticeModel As PracticeModel In udtExistingSP.PracticeList.Values
        '            udtExistingPracticeModel.EnrolRefNo = udtExistingSP.EnrolRefNo

        '            If Not IsNothing(udtExistingPracticeModel.BankAcct) Then udtExistingPracticeModel.BankAcct.EnrolRefNo = udtExistingSP.EnrolRefNo

        '            If Not IsNothing(udtExistingPracticeModel.Professional) Then udtExistingPracticeModel.Professional.EnrolRefNo = udtExistingSP.EnrolRefNo

        '            If Not IsNothing(udtExistingPracticeModel.PracticeSchemeInfoList) Then
        '                For Each udtExistingPracticeScheme As PracticeSchemeInfoModel In udtExistingPracticeModel.PracticeSchemeInfoList.Values
        '                    udtExistingPracticeScheme.EnrolRefNo = udtExistingSP.EnrolRefNo
        '                Next
        '            End If
        '        Next

        '        If Not IsNothing(udtExistingSP) Then
        '            ' Save the SP to session, then to Staging table
        '            udtServiceProviderBLL.SaveToSession(udtExistingSP)
        '            udtSPProfileBLL.SaveSessionObjectToStagingWithinTransition(TableLocation.Permanent, udtDB)

        '            'Remove the MO record in permanent (if any)
        '            udtSPProfileBLL.DeleteMedicalOraganizationPermanentWithinTransition(udtExistingSP.SPID, udtDB)

        '            'Take SP model from staging
        '            udtExistingSP = udtSPProfileBLL.GetServiceProviderStagingProfileNoSessionForMigration(udtExistingSP.EnrolRefNo, udtDB)
        '        End If

        '    Else
        '        If Not IsNothing(udtExistingSP.SPID) Then
        '            'Remove the MO record in permanent (if any)
        '            udtSPProfileBLL.DeleteMedicalOraganizationPermanentWithinTransition(udtExistingSP.SPID, udtDB)
        '        End If

        '    End If
        '    '------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        '    udtSPProfileBLL.HCVSRecordDataMigration(udtExistingSP.HKID, udtExistingSP.EnrolRefNo, udtExistingSP.SPID, strUserID, udtExistingSP.PracticeList, udtDB)
        '    'Dim recordTSMP As Byte() = udtSPMigrationBLL.GetSPMigrationRecordTSMP("", udtExistingSP.HKID, "", udtDB)
        '    Dim recordTSMP As Byte() = HttpContext.Current.Session(SPMigrationBLL.SESS_SPMigrationTSMP)
        '    udtSPMigrationBLL.UpdateSPMigrationStatusWithDBsupplied(udtExistingSP.HKID, SPMigrationStatus.Processed, udtExistingSP.EnrolRefNo, recordTSMP, udtDB)

        '    udtDB.CommitTransaction()
        '    udtAuditLogEntry.WriteEndLog(LogID.LOG00089, "HCVS migration migrate successful")
        '    '------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        '    Session(SESS_DataEntrySearchCriteria) = "||" + lblMigrateHCVSHKID.Text + "|||||"
        '    Session.Remove(SESS_DataEntryResult)


        'Catch eSQL As SqlClient.SqlException
        '    udtDB.RollBackTranscation()
        '    If eSQL.Number = 50000 Then
        '        msgBox.AddMessage(New SystemMessage("990001", "D", eSQL.Message))

        '        If msgBox.GetCodeTable.Rows.Count = 0 Then
        '            msgBox.Visible = False
        '        Else
        '            msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00090, "HCVS migration migrate failed")
        '        End If

        '    Else
        '        udtAuditLogEntry.WriteEndLog(LogID.LOG00090, "HCVS migration migrate failed")
        '        Throw eSQL
        '    End If

        'Catch ex As Exception
        '    udtAuditLogEntry.WriteEndLog(LogID.LOG00090, "HCVS migration migrate failed")
        '    udtDB.RollBackTranscation()
        '    Throw ex

        'End Try

        'ReloadSearchFromSession()
    End Sub

    Protected Sub gvMigrateHCVSMO_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    ' Email Address
        '    Dim lblMOEmail As Label = CType(e.Row.FindControl("lblMOEmail"), Label)
        '    If lblMOEmail.Text.Trim = String.Empty Then
        '        lblMOEmail.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '    End If

        '    ' Fax No.
        '    Dim lblMOFax As Label = CType(e.Row.FindControl("lblMOFax"), Label)
        '    If lblMOFax.Text.Trim = String.Empty Then
        '        lblMOFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '    End If
        'End If
    End Sub

    Protected Sub gvMigrateHCVSPracticeBank_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    ' Convert Medical Organization No. to Medical Organization Name
        '    Dim lblPracticeMO As Label = CType(e.Row.FindControl("lblPracticeMO"), Label)
        '    Dim intMODisplaySeq As Integer = CInt(lblPracticeMO.Text.Trim)

        '    For Each udtMO As MedicalOrganization.MedicalOrganizationModel In CType(Session(SESS_DataEntryMigrationSP), ServiceProviderModel).MOList.Values
        '        If udtMO.DisplaySeq.Value = intMODisplaySeq Then
        '            lblPracticeMO.Text = udtMO.DisplaySeqMOName
        '            Exit For
        '        End If
        '    Next
        'End If
    End Sub

    ' Migrate IVSS

    Protected Sub ibtnMigrateIVSSPreview_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Try
        '    Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID

        '    Dim strHCVSLocation As String = String.Empty
        '    Dim udtSPModel_HCVS As ServiceProviderModel = Nothing
        '    Dim udtSPModel_IVSS As ServiceProviderModel = Nothing
        '    Dim udtNewProfessionalList As ProfessionalModelCollection = Nothing
        '    Dim udtNewSchemeInfoList As SchemeInformationModelCollection = Nothing
        '    Dim udtSPCompletedModel As ServiceProviderModel = Nothing
        '    Dim dtProfessionalVer As DataTable = Nothing
        '    udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        '    udtAuditLogEntry.AddDescripton("HKIC", lblMigrateIVSSHKID.Text)
        '    udtAuditLogEntry.WriteLog(LogID.LOG00091, "IVSS migration preview click")

        '    Dim strHCVSERN As String = GetERNFromHKID(udtFormatter.formatHKIDInternal(lblMigrateIVSSHKID.Text))

        '    udtAuditLogEntry.AddDescripton("HCVS ERN", strHCVSERN)
        '    udtAuditLogEntry.AddDescripton("IVSS ERN", Session(SESS_MigrateIVSSERN).ToString())
        '    udtAuditLogEntry.WriteStartLog(LogID.LOG00092, "IVSS migration preview starts to load")

        '    udtSPCompletedModel = udtSPProfileBLL.HCVSIVSSRecordDataMigrationPreview(strHCVSERN, Session(SESS_MigrateIVSSERN), strUserID, udtSPModel_HCVS, udtSPModel_IVSS, udtNewProfessionalList, udtNewSchemeInfoList, strHCVSLocation, dtProfessionalVer)

        '    If IsNothing(udtSPModel_HCVS) Then
        '        ' udcSPMigrationPreview.buildSpProfileObject(udtSPCompletedModel, TableLocation.Staging)
        '    Else
        '        If strHCVSLocation.Trim.Equals(TableLocation.Permanent) Then
        '            udtSPModel_HCVS = udtSPProfileBLL.GetServiceProviderPermanentProfile(strHCVSERN)
        '            '     udcSPMigrationPreview.buildSpProfileObject(udtSPCompletedModel, TableLocation.Staging, udtSPModel_HCVS)
        '        ElseIf strHCVSLocation.Trim.Equals(TableLocation.Staging) Then
        '            'If in staging, need to get back the hcvs model in permanent for compairson
        '            If udtSPModel_HCVS.SPID.Trim.Equals(String.Empty) Then
        '                'Not yet completed the enrolment, should be a new enrolment even after migration
        '                '          udcSPMigrationPreview.buildSpProfileObject(udtSPCompletedModel, TableLocation.Staging)
        '            Else
        '                'Have SPID, so must have record in permanent
        '                udtSPModel_HCVS = udtSPProfileBLL.GetServiceProviderPermanentProfile(strHCVSERN)
        '                '           udcSPMigrationPreview.buildSpProfileObject(udtSPCompletedModel, TableLocation.Staging, udtSPModel_HCVS)
        '            End If

        '        End If

        '    End If

        '    'udcSPMigrationPreview.DisplayRecordStatus(True, TableLocation.Staging)

        '    ' Add this part for preview purpose
        '    BuildIVSSPreview(udtServiceProviderBLL.GetServiceProviderStagingProfileByERN_FromIVSS(Session(SESS_MigrateIVSSERN), New Database, strUserID))
        '    udtAuditLogEntry.WriteEndLog(LogID.LOG00093, "IVSS migration preview successfully loaded ")
        '    popupMigrateIVSSPreview.Show()

        'Catch ex As Exception
        '    Throw ex

        'End Try

    End Sub

    Protected Sub ibtnMigrateIVSSMigrate_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        'udtAuditLogEntry.AddDescripton("HKIC", lblMigrateIVSSHKID.Text)
        'udtAuditLogEntry.WriteLog(LogID.LOG00094, "IVSS migration migrate click")
        'popupMigrateIVSS.Show()
    End Sub

    Protected Sub ibtnMigrateIVSSSkip_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        'udtAuditLogEntry.AddDescripton("HKIC", lblMigrateIVSSHKID.Text)
        'udtAuditLogEntry.WriteLog(LogID.LOG00101, "IVSS migration migrate skip click")

        'Session(SESS_MigrateIVSSSkip) = strYes
        'ReloadSearchFromSession()
    End Sub

    Protected Sub ibtnPopupMigrateIVSSConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID

        'Dim strHCVSLocation As String = String.Empty
        'Dim udtSPModel_HCVS As ServiceProviderModel = Nothing
        'Dim udtSPModel_IVSS As ServiceProviderModel = Nothing
        'Dim udtNewProfessionalList As ProfessionalModelCollection = Nothing
        'Dim udtNewSchemeInfoList As SchemeInformationModelCollection = Nothing
        'Dim dtProfessionalVer As DataTable = Nothing
        'udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        'udtAuditLogEntry.AddDescripton("HKIC", lblMigrateIVSSHKID.Text)
        'udtAuditLogEntry.WriteLog(LogID.LOG00095, "IVSS migration migrate - confirm click")

        'Dim strHCVSERN As String = GetERNFromHKID(udtFormatter.formatHKIDInternal(lblMigrateIVSSHKID.Text))

        ''udtAuditLogEntry.AddDescripton("HKIC", udtSPModel_HCVS.HKID)
        'udtAuditLogEntry.AddDescripton("HCVS ERN", strHCVSERN)
        'udtAuditLogEntry.AddDescripton("IVSS ERN", Session(SESS_MigrateIVSSERN).ToString.Trim)
        'udtAuditLogEntry.WriteStartLog(LogID.LOG00096, "IVSS migration migrate (Prepare Data Model)")

        'udtSPProfileBLL.HCVSIVSSRecordDataMigrationPreview(strHCVSERN, Session(SESS_MigrateIVSSERN).ToString.Trim, strUserID, udtSPModel_HCVS, udtSPModel_IVSS, udtNewProfessionalList, udtNewSchemeInfoList, strHCVSLocation, dtProfessionalVer)

        'udtAuditLogEntry.WriteEndLog(LogID.LOG00097, "IVSS migration migrate (Prepare Data Model) successful")

        'udtSPProfileBLL.HCVSIVSSRecordDataMigration(udtSPModel_HCVS, udtSPModel_IVSS, udtNewProfessionalList, udtNewSchemeInfoList, strUserID, strHCVSLocation, dtProfessionalVer, udtAuditLogEntry)

        'Session(SESS_DataEntrySearchCriteria) = "||" + lblMigrateIVSSHKID.Text.Trim + "|||||"
        'Session.Remove(SESS_DataEntryResult)

        'ReloadSearchFromSession()
    End Sub

    Private Function GetERNFromHKID(ByVal strHKID As String) As String
        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Dim dt As DataTable = udtSPProfileBLL.DataEntrySearch(strFuncCode, True, String.Empty, String.Empty, strHKID, String.Empty, String.Empty, _
        '                                                          String.Empty, String.Empty, String.Empty)

        Dim dt As DataTable
        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

        udtBLLSearchResult = udtSPProfileBLL.DataEntrySearch(strFuncCode, String.Empty, String.Empty, strHKID, String.Empty, String.Empty, _
                                                             String.Empty, String.Empty, String.Empty, True, True)

        dt = CType(udtBLLSearchResult.Data, DataTable)
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        dt = DataTableFilter(dt, "Table_Location <> '" + TableLocation.Enrolment + "'")

        If dt.Rows.Count = 0 Then
            Return String.Empty
        Else
            Return CStr(dt.Rows(0)("Enrolment_Ref_No")).Trim
        End If

    End Function

    Protected Sub gvMigrateIVSSMO_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    ' Email Address
        '    Dim lblMOEmail As Label = CType(e.Row.FindControl("lblMOEmail"), Label)
        '    If lblMOEmail.Text.Trim = String.Empty Then
        '        lblMOEmail.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '    End If

        '    ' Fax No.
        '    Dim lblMOFax As Label = CType(e.Row.FindControl("lblMOFax"), Label)
        '    If lblMOFax.Text.Trim = String.Empty Then
        '        lblMOFax.Text = Me.GetGlobalResourceObject("Text", "N/A")
        '    End If
        'End If
    End Sub

    Protected Sub gvMigrateIVSSPracticeBank_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Then
        '    ' Convert Medical Organization No. to Medical Organization Name
        '    Dim lblPracticeMO As Label = CType(e.Row.FindControl("lblPracticeMO"), Label)
        '    Dim intMODisplaySeq As Integer = CInt(lblPracticeMO.Text.Trim)

        '    For Each udtMO As MedicalOrganization.MedicalOrganizationModel In CType(Session(SESS_DataEntryMigrationSP), ServiceProviderModel).MOList.Values
        '        If udtMO.DisplaySeq.Value = intMODisplaySeq Then
        '            lblPracticeMO.Text = udtMO.DisplaySeqMOName
        '            Exit For
        '        End If
        '    Next
        'End If
    End Sub

    '

    Protected Sub ibtnSearchBatchProceed_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim lnkbtnResultBatchERN As LinkButton

        Dim strSuffix As String = String.Empty

        For Each gvr As GridViewRow In Me.gvBatchResult.Rows
            Dim chkSelect As CheckBox
            Dim hfERNSuffix As HiddenField
            chkSelect = CType(gvr.FindControl("chkSelect"), CheckBox)
            hfERNSuffix = CType(gvr.FindControl("hfERNSuffix"), HiddenField)
            lnkbtnResultBatchERN = CType(gvr.FindControl("lnkbtnResultBatchERN"), LinkButton)
            If chkSelect.Checked Then
                strSuffix = strSuffix + "," + hfERNSuffix.Value.Trim
            End If
        Next

        Me.txtSuffix.Text = strSuffix.Substring(1)


        If panEnrolledSP.Visible Then
            Me.txtTSPID.Text = lblEnrolledSPID.Text.Trim
        End If

    End Sub

    '

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        Session(SESS_MigrateIVSSSkip) = strNo

        If Session(SESS_DataEntrySearchCriteria).ToString.Split("^").Length = 2 Then
            Session(SESS_DataEntrySearchCriteria) = Session(SESS_DataEntrySearchCriteria).ToString.Split("^")(0)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            _blnOverrideResultLimit = True
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            ReloadSearchFromSession()
        Else
            udtSPProfileBLL.ClearSession()
            MultiViewDataEntry.ActiveViewIndex = ViewIndexSearchCriteria
        End If

    End Sub

    Protected Sub ibtnSearchResultBatchBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtSPProfileBLL.ClearSession()
        MultiViewDataEntry.ActiveViewIndex = ViewIndexSearchCriteria
    End Sub

    '

    Private Sub ResetSearchCriteria()
        txtEnrolRefNo.Text = String.Empty
        txtSPID.Text = String.Empty
        txtSPHKID.Text = String.Empty
        txtSPName.Text = String.Empty
        txtPhone.Text = String.Empty
        ddlSPHealthProf.SelectedValue = String.Empty
        ddlStatus.SelectedValue = String.Empty
        ddlScheme.SelectedValue = String.Empty

        txtERN.Text = String.Empty
        txtSuffix.Text = String.Empty
        txtAction.Text = String.Empty

    End Sub

    '

    Private Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            ' Enrolment Reference No.
            Dim lnkbtnERN As LinkButton = CType(e.Row.FindControl("lnkbtnERN"), LinkButton)
            lnkbtnERN.Text = udtFormatter.formatSystemNumber(lnkbtnERN.CommandArgument.Trim)

            ''ERN_Remark Icon only display when record with spid
            'Dim imgInfo As Image = CType(e.Row.FindControl("imgInfo"), Image)
            'If lnkbtnRSPID.Text.Trim.Equals(String.Empty) Then
            '    If imgInfo.AlternateText.Trim.Equals(String.Empty) Then
            '        imgInfo.Visible = False
            '    Else
            '        imgInfo.Visible = True

            '        Dim strERNRemark() As String
            '        Dim strAlternateText As String = String.Empty
            '        strERNRemark = imgInfo.AlternateText.Trim.Split(",")

            '        For i As Integer = 0 To strERNRemark.Length - 1
            '            strAlternateText = strAlternateText + ", " + udtFormatter.formatSystemNumber(strERNRemark(i))
            '        Next

            '        If strAlternateText.Length > 2 Then
            '            strAlternateText = strAlternateText.Substring(2)
            '        End If
            '        imgInfo.AlternateText = "The enrolment merges with " + strAlternateText
            '    End If
            'Else
            '    imgInfo.Visible = False
            'End If

            ' Service Provider ID
            Dim lnkbtnRSPID As LinkButton = CType(e.Row.FindControl("lnkbtnRSPID"), LinkButton)
            If lnkbtnRSPID.Text.Trim.Equals(String.Empty) Then
                lnkbtnRSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lnkbtnRSPID.CommandArgument = String.Empty
                lnkbtnRSPID.Enabled = False
            End If

            ' Enrolment Time
            Dim lblREnrolDtm As Label = CType(e.Row.FindControl("lblREnrolDtm"), Label)
            If Not lblREnrolDtm.Text.Trim.Equals(String.Empty) Then
                lblREnrolDtm.Text = udtFormatter.convertDateTime(lblREnrolDtm.Text.Trim)
            End If

            ' Data Entry Processing Time
            Dim lblRProcessingDtm As Label = CType(e.Row.FindControl("lblRProcessingDtm"), Label)
            If Not lblRProcessingDtm.Text.Trim.Equals(String.Empty) Then
                lblRProcessingDtm.Text = udtFormatter.convertDateTime(lblRProcessingDtm.Text.Trim)
            End If

            ' Service Provider HKIC No.
            Dim lblRSPHKID As Label = CType(e.Row.FindControl("lblRSPHKID"), Label)
            lblRSPHKID.Text = udtFormatter.formatHKID(lblRSPHKID.Text, False)

            ' Service Provider Name
            Dim lblRCname As Label = CType(e.Row.FindControl("lblRCname"), Label)
            lblRCname.Text = udtFormatter.formatChineseName(lblRCname.Text.Trim)

            ' Status
            Dim lblRStatus As Label = CType(e.Row.FindControl("lblRStatus"), Label)
            lblRStatus.Text = ConvertStatusCodeToValue(lblRStatus.Text)

            ' Scheme Name
            Dim lblRScheme As Label = CType(e.Row.FindControl("lblRScheme"), Label)

            If lblRScheme.Text.Trim.Equals(String.Empty) Then
                lblRScheme.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

        End If
    End Sub

    Private Sub gvResult_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvResult.PreRender
        Me.GridViewPreRenderHandler(sender, e, "DataEntryResult")
    End Sub

    Private Sub gvResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvResult.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, "DataEntryResult")
    End Sub

    Private Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvResult.Sorting
        Me.GridViewSortingHandler(sender, e, "DataEntryResult")
    End Sub

    Private Sub gvResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvResult.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then
            Dim r As GridViewRow = CType(e.CommandSource, LinkButton).NamingContainer
            Dim strERN As String = CType(r.FindControl("lnkbtnERN"), LinkButton).CommandArgument.Trim
            Dim strTableLocation As String = CType(r.FindControl("hfRStatus"), HiddenField).Value.Trim
            Dim strHKID As String = CType(r.FindControl("lblRSPHKID"), Label).Text.Trim

            If Not gvResult.Columns(intGvResultColumn_MergeCheckBox).Visible Then
                If Session(SESS_DataEntrySearchCriteria).ToString.Split("^").Length > 1 Then
                    Session(SESS_DataEntrySearchCriteria) = Session(SESS_DataEntrySearchCriteria).ToString.Split("^")(0) + "^" + strERN + "|" + strTableLocation + "|" + strHKID
                Else
                    Session(SESS_DataEntrySearchCriteria) += "^" + strERN + "|" + strTableLocation + "|" + strHKID
                End If

                'If Session(SESS_TurnOnDataMigration) = strYes Then
                '    Session(SESS_MigrateHCVSERN) = strERN
                '    If SearchMigrateNeed(strHKID) Then Return
                '    If SearchMigrateHCVS(strHKID) Then Return
                'End If

                'If Session(SESS_TurnOnIVSSDataMigration) = strYes Then
                '    Session(SESS_MigrateHCVSERN) = strERN
                '    If SearchMigrateIVSS(strHKID) Then Return
                'End If

            End If

            If gvResult.Columns(intGvResultColumn_MergeCheckBox).Visible Then
                RedirectToSPProfileFromSearch(strERN, strTableLocation)
            Else
                SearchUnprocessedRecord(strERN, strTableLocation, strHKID, True)
            End If

        End If
    End Sub

    '

    Protected Sub gvResult_UncheckOther(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cboMerge As CheckBox = CType(sender, CheckBox)
        If cboMerge.Checked = False Then Return

        For Each r As GridViewRow In gvResult.Rows
            If Not r.Equals(CType(cboMerge.NamingContainer, GridViewRow)) Then
                CType(r.FindControl("cboMerge"), CheckBox).Checked = False
            End If
        Next
    End Sub

    '

    Private Sub gvBatchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvBatchResult.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            Dim strERN As String = String.Empty
            Dim strSuffix As String = String.Empty

            ' Retrieve the row that contains the linkbutton clicked by the user from the Rows collection.
            Dim row As GridViewRow = CType(e.CommandSource, LinkButton).NamingContainer 'gvResult.Rows(index)

            Me.txtERN.Text = e.CommandArgument
            Me.txtSuffix.Text = CType(row.FindControl("hfERNSuffix"), HiddenField).Value.Trim
            Me.txtAction.Text = "View"
            Me.txtTableLocation.Text = "E"

            Dim strClickScript As String

            strClickScript = "<script>document.getElementById('" + Me.btnSpDetails.ClientID + "').click();</script>"

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "ClickScript", strClickScript, False)


        End If
    End Sub

    Private Sub gvBatchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBatchResult.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lnkbtnResultBatchERN As LinkButton = CType(e.Row.FindControl("lnkbtnResultBatchERN"), LinkButton)
            Dim hfERNSuffix As HiddenField = CType(e.Row.FindControl("hfERNSuffix"), HiddenField)
            Dim lblResultBatchEnrolDtm As Label = CType(e.Row.FindControl("lblResultBatchEnrolDtm"), Label)
            Dim lblResultBatchSPHKID As Label = CType(e.Row.FindControl("lblResultBatchSPHKID"), Label)

            Dim chkbox As CheckBox = CType(e.Row.FindControl("chkSelect"), CheckBox)

            lnkbtnResultBatchERN.Text = udtFormatter.formatSystemNumber(lnkbtnResultBatchERN.CommandArgument.Trim) + "-" + hfERNSuffix.Value.Trim

            If Not lblResultBatchEnrolDtm.Text.Trim.Equals(String.Empty) Then
                lblResultBatchEnrolDtm.Text = udtFormatter.convertDateTime(lblResultBatchEnrolDtm.Text.Trim)
            End If

            lblResultBatchSPHKID.Text = udtFormatter.formatHKID(lblResultBatchSPHKID.Text, False)

            If lnkbtnResultBatchERN.CommandArgument.Trim.Equals(txtERN.Text.Trim) AndAlso hfERNSuffix.Value.Trim.Equals(Me.txtSuffix.Text.Trim) Then
                e.Row.BackColor = Drawing.Color.Azure
                chkbox.Checked = True
                chkbox.Attributes.Add("onclick", "return false;")
            End If


        End If
    End Sub

    '

    Private Function ConvertStatusCodeToValue(ByVal strCode As String) As String
        Select Case strCode
            Case "E"
                Return "Unprocessed"
            Case "S"
                Return "Processing"
            Case "P"
                Return "Enrolled"
        End Select

        Return Nothing

    End Function

    ' Used in .aspx

    Protected Function formatAddress(ByVal udtAddressModel As Common.Component.Address.AddressModel) As String
        Return udtFormatter.formatAddress(udtAddressModel)
    End Function

    Protected Function formatChiAddress(ByVal udtAddressModel As Common.Component.Address.AddressModel) As String
        Return udtFormatter.formatAddressChi(udtAddressModel)
    End Function

    Protected Function formatChineseString(ByVal strChineseString) As String
        Return udtFormatter.formatChineseName(strChineseString)
    End Function

    Protected Function GetHealthProfName(ByVal strHealthProfCode As String) As String
        If IsNothing(strHealthProfCode) OrElse strHealthProfCode = String.Empty Then Return String.Empty

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

        ' -----------------------------------------------------------------------------------------

        Return udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDesc

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

    End Function

    Protected Function GetPracticeTypeName(ByVal strPracticeCode As String) As String
        If IsNothing(strPracticeCode) OrElse strPracticeCode = String.Empty Then Return String.Empty
        Return udtSPProfileBLL.GetPracticeTypeName(strPracticeCode).DataValue
    End Function

    Protected Sub ibtnErrorBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        Session.Remove(SESS_DataEntryResult)
        Session.Remove(SESS_DataEntrySearchCriteria)

        udtSPProfileBLL.ClearSession()

        MultiViewDataEntry.ActiveViewIndex = ViewIndexSearchCriteria
        ResetSearchCriteria()

    End Sub

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

End Class