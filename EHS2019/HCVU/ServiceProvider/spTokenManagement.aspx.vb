Imports System.Data.SqlClient
Imports System.Web.Security.AntiXss
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.Component.RSA_Manager
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.Token
Imports Common.Component.Token.TokenBLL
Imports Common.DataAccess
Imports Common.eHRIntegration.BLL
Imports Common.eHRIntegration.Model.Xml.eHRService
Imports Common.Encryption
Imports Common.Format
Imports Common.PCD.WebService.Interface
Imports Common.Validation
Imports CustomControls
Imports HCVU.AccountChangeMaintenance
Imports HCVU.TokenManagement
Imports Common.Component.Professional
Imports Common.PCD
Imports Common.Component.SchemeInformation
Imports Common.Component.Practice

Partial Public Class spTokenManagement
    Inherits BasePageWithControl

#Region "Constants"

    Private Class DetailPageCurrentAction
        Public Const NewEnrolment As Integer = 1
        Public Const ActivateToken As Integer = 2
        Public Const DeactivateToken As Integer = 3
        Public Const ReplaceToken As Integer = 4
        Public Const SchemeEnrolment As Integer = 6
    End Class

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Const _strValidationFailTitle As String = "ValidationFail"
    Private Const _strActionFailTitle As String = "UpdateFail"

    Private Enum EnrolmentAction
        ConfirmSchemeEnrolment
        Reject
        ReturnForAmendment
    End Enum
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

#End Region

    Private PrintOutClick As Boolean = False
    Private _udtBLLSearchResult As BaseBLL.BLLSearchResult
    Private _intSearchResultRowCount As Integer = 0

    Private Const SESS_FromTaskList As String = "010202_FromTaskList"

    Public Const SESS_TokenRecordResult As String = "TokenRecordResult"
    Public Const SESS_ERN As String = "Enrol_Ref_No"
    Public Const SESS_SPID As String = "SP_ID"
    Public Const SESS_Token_Status As String = "Token_Status"
    Public Const SESS_TokenSerialNo As String = "TokenSerialNo"
    Public Const SESS_IsShareToken As String = "010202_IsShareToken"
    Public Const SESS_IsShareTokenReplacement As String = "010202_IsShareTokenReplacement"
    Public Const SESS_ProjectReplacement As String = "010202_ProjectReplacement"
    Public Const SESS_ReplacementToken As String = "ReplacementToken"
    Public Const SESS_PPI_Status As String = "PPI_Status"
    Public Const SESS_Project As String = "Project"
    Public Const SESS_IsUniqueSearch As String = "010202_IsUniqueSearch"
    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
    ' --------------------------------------------------------------------------------------------------------------------------------
    Public Const SESS_SP As String = "010202_SP"
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Const SESS_PCDCheckAccountStatusResult As String = "010202_PCDCheckAccountStatusResult"
    Public Const SESS_PCDProfessionalChecked As String = "010202_PCDProfessional_Checked"
    Public Const SESS_PCDStatusChecked As String = "010202_PCDStatus_Checked"
    Public Const SESS_JoinPCDClick As String = "010202_JoinPCD_Click"
    Public Const SESS_PCDDelistedConfirmed As String = "010202_PCDDelisted_Confirmed"
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

    ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Const SESS_PCDSuspended As String = "010202_PCDSuspended"
    ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        FunctionCode = FunctCode.FUNT010202

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        If Not IsPostBack Then
            InitControlOnce()

            ResetMessageAndError()

            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Service Provider Token Management loaded")

            If Session("fromMain") = "Y" Then
                Session("fromMain") = Nothing

                Session(SESS_FromTaskList) = "Y"

                'Search Result
                Me.ddlStatus.SelectedIndex = 1
                ibtnSSearch_Click(Nothing, Nothing)

            End If

            ' Handle double post-back
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

        Else
            If Me.ucTypeOfPracticePopup.Showing Then
                Me.ModalPopupExtenderTypeOfPractice.Show()
            End If

        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not IsNothing(Session(spPrintFunction.SESS_PrintFunctionStatus)) Then
            Select Case Session(spPrintFunction.SESS_PrintFunctionStatus)
                Case PrintFunctionStatus.ActivePrintFunction
                    popupPrintSchemeEnrolmentLetter.Show()

                Case PrintFunctionStatus.ClosePrintFunction
                    popupPrintSchemeEnrolmentLetter.Hide()
                    Session.Remove(spPrintFunction.SESS_PrintFunctionStatus)
            End Select
        End If

        Select Case mvCore.GetActiveView.ID
            Case vInputSearch.ID
                'CRE17-008 (Remind Delist Practice) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If ViewState(TokenPopup_IsShow) Is Nothing AndAlso ViewState(TokenPopup_IsShow) = False Then
                If Not IsPopupShow() Then
                    ScriptManager1.SetFocus(Me.txtEnrolRefNo)
                    Me.pnlTokenManagement.DefaultButton = ibtnSSearch.ID
                End If
                'CRE17-008 (Remind Delist Practice) [End][Chris YIM]
            Case vSearchResult.ID
                ScriptManager1.SetFocus(Me.btnHidden)
                Me.pnlTokenManagement.DefaultButton = Me.btnHidden.ID
            Case vDetail.ID
                If Not PrintOutClick Then
                    Me.pnlTokenManagement.DefaultButton = ibtnDSave.ID
                Else
                    PrintOutClick = False
                End If
        End Select

    End Sub

    '

    Private Sub InitControlOnce()
        ' Text fields
        txtEnrolRefNo.Text = String.Empty
        txtSPID.Text = String.Empty
        txtSPHKID.Text = String.Empty
        txtSPName.Text = String.Empty
        txtPhone.Text = String.Empty
        txtTokenSerialNo.Text = String.Empty

        ' Bind Service Provider Status
        ddlStatus.Items.Clear()
        ddlStatus.DataSource = Status.GetDescriptionListFromDBEnumCode(ServiceProviderTokenStatus.ClassCode)
        ddlStatus.DataValueField = "Status_Value"
        ddlStatus.DataTextField = "Status_Description"
        ddlStatus.DataBind()
        ddlStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ' Bind Health Profession
        ddlSPHealthProf.Items.Clear()
        ddlSPHealthProf.DataSource = (New SPProfileBLL).GetHealthProf
        ddlSPHealthProf.DataValueField = "ServiceCategoryCode"
        ddlSPHealthProf.DataTextField = "ServiceCategoryDesc"
        ddlSPHealthProf.DataBind()
        ddlSPHealthProf.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))

        ' Bind Scheme
        ddlScheme.Items.Clear()
        ddlScheme.DataSource = (New SchemeBackOfficeBLL).GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
        ddlScheme.DataValueField = "SchemeCode"
        ddlScheme.DataTextField = "DisplayCode"
        ddlScheme.DataBind()
        ddlScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        Dim udtStaticDataBLL As New StaticDataBLL
        Dim udtStaticData As StaticDataModel

        ' Bind Token Issued By
        rblDTokenIssuedBy.Items.Clear()
        rblDTokenIssuedBy.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("TOKEN_ISSUE_BY")
        rblDTokenIssuedBy.DataValueField = "ItemNo"
        rblDTokenIssuedBy.DataTextField = "DataValue"
        rblDTokenIssuedBy.DataBind()

        ' Bind Is Share Token
        rblDIsShareToken.Items.Clear()
        rblDIsShareToken.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("YesNo")
        rblDIsShareToken.DataValueField = "ItemNo"
        rblDIsShareToken.DataTextField = "DataValue"
        rblDIsShareToken.DataBind()

        ' Bind Token Deactivate Reason
        rblDDeactivateReason.Items.Clear()
        udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKENDISABLE", 1)
        rblDDeactivateReason.Items.Insert(0, New ListItem(udtStaticData.DataValue, udtStaticData.ItemNo))
        udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKENDISABLE", 2)
        rblDDeactivateReason.Items.Insert(1, New ListItem(udtStaticData.DataValue, udtStaticData.ItemNo))
        udtStaticData = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKENDISABLE", 6)
        rblDDeactivateReason.Items.Insert(2, New ListItem(udtStaticData.DataValue, udtStaticData.ItemNo))

        ' Bind New Token Replacement Reason
        rblDRReplacementReason.Items.Clear()
        rblDRReplacementReason.DataSource = udtStaticDataBLL.GetStaticDataListByColumnName("TOKEN_REPLACE_REASON")
        rblDRReplacementReason.DataValueField = "ItemNo"
        rblDRReplacementReason.DataTextField = "DataValue"
        rblDRReplacementReason.DataBind()

        hfDCurrentAction.Value = String.Empty

        AddPreventMultiClick()

    End Sub

    Private Sub ResetMessageAndError()
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False

        ' Reset alert image
        imgDTokenIssuedByAlert.Visible = False
        imgDTokenSerialNoAlert.Visible = False
        imgDIsShareTokenAlert.Visible = False
        imgDDeactiveReasonAlert.Visible = False
        imgDRTokenSerialNoAlert.Visible = False
        imgDRReplacementReasonAlert.Visible = False

    End Sub

    Private Sub AddPreventMultiClick()
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnSSearch)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnSearchResultBack)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnDReturn)

        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnDManageTokenDeactivate)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnDManageTokenReplace)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnDSave)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnDCancel)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnDConfirmConfirm)
        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnDConfirmBack)

        MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnViewRecordReprint)

    End Sub

#End Region

#Region "Abstract Method of [HCVU.BasePageWithControl]"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)
        Session(SESS_IsUniqueSearch) = Nothing
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As AuditLogEntry) As Boolean
        Return True
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        If blnOverrideResultLimit Then
            ShowSearchRecord(True, True)
        Else
            ShowSearchRecord(True)
        End If

        Return _udtBLLSearchResult
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer
        Return _intSearchResultRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)
        Session("Printed") = False
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        Dim enumSearchResult As SearchResultEnum

        If Not IsNothing(Session(SESS_FromTaskList)) Then
            If Session(SESS_FromTaskList) = "Y" Then
                Me.ddlStatus.SelectedIndex = 1
                Session(SESS_FromTaskList) = Nothing
            End If
        End If

        Try
            enumSearchResult = StartSearchFlow(FunctionCode, Nothing, udcMessageBox, Nothing, False, True)

        Catch ex As Exception
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                ' Audit Log has been handled locally

            Case Else
                Throw New Exception("Error: Class = [HCVU.spTokenManagement], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

        End Select
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub

#End Region

    Protected Sub ibtnSSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcCollapsibleSearchCriteriaReview.Collapsed = True
        udcCollapsibleSearchCriteriaReview.ClientState = "True"

        ResetMessageAndError()

        Dim enumSearchResult As SearchResultEnum = StartSearchFlow(FunctionCode, Nothing, udcMessageBox, Nothing)

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                ' Audit Log has been handled locally

            Case SearchResultEnum.ValidationFail
                ' No Validation
                Throw New Exception("Error: Class = [HCVU.spTokenManagement], Method = [ibtnSearch_Click], Message = The method - [SF_ValidateSearch] should not return [False]")

            Case SearchResultEnum.NoRecordFound
                ' Audit Log has been handled locally

            Case SearchResultEnum.OverResultList1stLimit_PopUp
                ' Audit Log has been handled locally

            Case SearchResultEnum.OverResultList1stLimit_Alert
                ' Audit Log has been handled locally

            Case SearchResultEnum.OverResultListOverrideLimit
                ' Audit Log has been handled locally

            Case Else
                Throw New Exception("Error: Class = [HCVU.spTokenManagement], Method = [ibtnSearch_Click], Message = The type of [EnumSqlErrorMessage] of [Common.Component.BaseBLL] mis-matched")

        End Select

    End Sub

    Private Sub ShowSearchRecord(ByVal blnWriteLog As Boolean, Optional ByVal blnOverrideResultLimit As Boolean = False)
        ' Init
        udcInfoMessageBox.Visible = False

        Dim strERN As String = String.Empty
        Me.txtEnrolRefNo.Text = AntiXssEncoder.HtmlEncode(UCase(Me.txtEnrolRefNo.Text), True)

        If Not txtEnrolRefNo.Text.Trim.Equals(String.Empty) Then
            If (New Validator).chkSystemNumber(txtEnrolRefNo.Text.Trim) Then
                strERN = Formatter.ReverseSystemNumber(txtEnrolRefNo.Text.Trim)
            Else
                strERN = txtEnrolRefNo.Text.Trim
            End If
        End If

        Dim udtAuditLogEntry As AuditLogEntry = Nothing

        If blnWriteLog Then
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("ERN", strERN)
            udtAuditLogEntry.AddDescripton("SPID", Me.txtSPID.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP HKID", Me.txtSPHKID.Text.Trim)
            udtAuditLogEntry.AddDescripton("SP Name", Me.txtSPName.Text.Trim)
            udtAuditLogEntry.AddDescripton("Phone", Me.txtPhone.Text.Trim)
            udtAuditLogEntry.AddDescripton("Profession", Me.ddlSPHealthProf.SelectedValue.Trim)
            udtAuditLogEntry.AddDescripton("Status", Me.ddlStatus.SelectedValue.Trim)
            udtAuditLogEntry.AddDescripton("Token Serial No", Me.txtTokenSerialNo.Text.Trim)
            udtAuditLogEntry.AddDescripton("Scheme Code", Me.ddlScheme.SelectedValue.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search", New AuditLogInfo(Me.txtSPID.Text.Trim, Me.txtSPHKID.Text.Trim, "", "", "", ""))
        End If

        _udtBLLSearchResult = (New TokenManagementBLL).TokenRecordSearch(FunctionCode, strERN, _
            Me.txtSPID.Text.Trim, (New Formatter).formatHKIDInternal(Me.txtSPHKID.Text.Trim), Me.txtSPName.Text.Trim, Me.txtPhone.Text.Trim, _
            Me.ddlSPHealthProf.SelectedValue.Trim, Me.ddlStatus.SelectedValue.Trim, Me.txtTokenSerialNo.Text.Trim, Me.ddlScheme.SelectedValue.Trim, _
            blnOverrideResultLimit)

        Dim dt As DataTable = Nothing

        Select Case _udtBLLSearchResult.SqlErrorMessage
            Case BaseBLL.EnumSqlErrorMessage.Normal
                dt = CType(_udtBLLSearchResult.Data, DataTable)
                _intSearchResultRowCount = dt.Rows.Count

            Case BaseBLL.EnumSqlErrorMessage.OverResultList1stLimit
                If Not IsNothing(udtAuditLogEntry) Then
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search failed")
                End If

                Return

            Case BaseBLL.EnumSqlErrorMessage.OverResultListOverrideLimit
                If Not IsNothing(udtAuditLogEntry) Then
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search failed")
                End If

                Return

            Case Else
                Throw New Exception("Error: Class = [HCVU.spTokenManagement], Method = [ShowSearchRecord], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

        End Select

        Session(SESS_TokenRecordResult) = dt

        If dt.Rows.Count = 0 Then
            ' No record found
            If mvCore.GetActiveView.ID = vInputSearch.ID Then
                udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                udcInfoMessageBox.Type = InfoMessageBoxType.Information
                udcInfoMessageBox.BuildMessageBox()

            End If

            mvCore.SetActiveView(vInputSearch)

            If Not IsNothing(udtAuditLogEntry) Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search failed: No Record")
            End If

        ElseIf dt.Rows.Count = 1 _
                AndAlso (txtSPHKID.Text.Trim <> String.Empty OrElse txtEnrolRefNo.Text.Trim <> String.Empty OrElse txtSPID.Text.Trim <> String.Empty) Then
            ' 1 record found with using key field search
            Session(SESS_IsUniqueSearch) = "Y"

            Dim dr As DataRow = dt.Rows(0)

            BuildDetail(dr("Enrolment_Ref_No").ToString.Trim, dr("Record_Status").ToString.Trim)

        Else
            lblResultERN.Text = FillAnyToEmptyString(txtEnrolRefNo.Text.Trim)
            lblResultSPID.Text = FillAnyToEmptyString(txtSPID.Text.Trim)
            lblResultSPHKID.Text = FillAnyToEmptyString(txtSPHKID.Text.Trim)
            lblResultSPName.Text = FillAnyToEmptyString(txtSPName.Text.Trim)
            lblResultPhone.Text = FillAnyToEmptyString(txtPhone.Text.Trim)
            lblResultTokenSerialNo.Text = FillAnyToEmptyString(txtTokenSerialNo.Text.Trim)
            lblResultHealthProf.Text = AntiXssEncoder.HtmlEncode(ddlSPHealthProf.SelectedItem.Text, True)
            lblResultStatus.Text = AntiXssEncoder.HtmlEncode(ddlStatus.SelectedItem.Text, True)
            lblResultScheme.Text = AntiXssEncoder.HtmlEncode(ddlScheme.SelectedItem.Text, True)

            Me.GridViewDataBind(gvResult, dt, "Enrolment_Ref_No", "ASC", False)
            mvCore.SetActiveView(vSearchResult)

            If Not IsNothing(udtAuditLogEntry) Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search successful")
            End If

        End If

    End Sub

    Private Function FillAnyToEmptyString(ByVal value As String) As String
        If IsNothing(value) OrElse value.Trim = String.Empty Then
            Return Me.GetGlobalResourceObject("Text", "Any")
        End If

        Return value
    End Function

    '

    Protected Sub gvResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim udtFormatter As New Formatter

            ' Enrolment Reference No.
            Dim lnkbtnERN As LinkButton = e.Row.FindControl("lnkbtnERN")
            lnkbtnERN.Text = udtFormatter.formatSystemNumber(lnkbtnERN.CommandArgument.Trim)

            ' Service Provider ID
            Dim lblRSPID As LinkButton = e.Row.FindControl("lblRSPID")

            If lblRSPID.Text.Trim = String.Empty Then
                lblRSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblRSPID.Enabled = False
            End If

            ' Service Provider HKIC No.
            Dim lblRSPHKID As Label = e.Row.FindControl("lblRSPHKID")
            lblRSPHKID.Text = udtFormatter.formatHKID(lblRSPHKID.Text.Trim, False)

            ' Service Provider Name
            Dim lblRCname As Label = e.Row.FindControl("lblRCname")
            lblRCname.Text = udtFormatter.formatChineseName(lblRCname.Text.Trim)

            ' Status
            Dim lblRStatus As Label = e.Row.FindControl("lblRStatus")
            Status.GetDescriptionFromDBCode(ServiceProviderTokenStatus.ClassCode, lblRStatus.Text, lblRStatus.Text, String.Empty)

        End If

    End Sub

    Protected Sub gvResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        ResetMessageAndError()

        If TypeOf e.CommandSource Is LinkButton Then
            Dim strERN As String = e.CommandArgument.ToString.Trim
            Dim row As GridViewRow = DirectCast(e.CommandSource, Control).NamingContainer
            Dim strStatus As String = DirectCast(row.FindControl("lblRStatusCode"), Label).Text.Trim

            BuildDetail(strERN, strStatus)

        End If

    End Sub

    Protected Sub gvResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, SESS_TokenRecordResult)
    End Sub

    Protected Sub gvResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_TokenRecordResult)
    End Sub

    Protected Sub gvResult_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, SESS_TokenRecordResult)
    End Sub

    '

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00031, "Back Click")

        mvCore.SetActiveView(vInputSearch)

        ResetMessageAndError()

    End Sub

    '

    Private Sub BuildDetail(strERN As String, strRowStatus As String)
        Session(SESS_ERN) = strERN

        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtServiceProviderBLL As New ServiceProviderBLL
        Dim udtSPProfileBLL As New SPProfileBLL

        ' Retrieve SP from DB
        ' New Enrolment, Scheme Enrolment -> Staging; Others -> Permanent
        If strRowStatus.Trim = ServiceProviderTokenStatus.SchemeEnrolment OrElse strRowStatus.Trim = ServiceProviderTokenStatus.NewEnrolment Then

            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Get SP and related record and save to session for Enrolment/Reject/Return for amendment action
            'udtSP = (New ServiceProviderBLL).GetServiceProviderStagingProfileByERN(strERN, New Database)
            If udtSPProfileBLL.GetServiceProviderProfile(strERN, TableLocation.Staging) Then
                udtSP = udtServiceProviderBLL.GetSP
            End If
        Else
            'udtSP = (New ServiceProviderBLL).GetServiceProviderPermanentProfileByERN(strERN, New Database)
            If udtSPProfileBLL.GetServiceProviderProfile(strERN, TableLocation.Permanent) Then
                udtSP = udtServiceProviderBLL.GetSP
            End If
            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]
        End If

        If IsNothing(udtSP) Then
            ' Message: The service provider status is updated by others.
            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00015)
            udcInfoMessageBox.BuildMessageBox()
            udcInfoMessageBox.Type = InfoMessageBoxType.Information

            mvCore.SetActiveView(vError)

            Return

        End If

        Session("TempSPmodel") = udtSP
        Session("Status") = strRowStatus

        mvCore.SetActiveView(vDetail)

        txtDTokenSerialNo.Text = String.Empty
        txtDRTokenSerialNo.Text = String.Empty
        rblDTokenIssuedBy.ClearSelection()
        rblDIsShareToken.ClearSelection()

        Dim dt As DataTable = Session(SESS_TokenRecordResult)
        Dim drs() As DataRow = dt.Select(String.Format("Enrolment_Ref_No = '{0}' AND Record_Status = '{1}'", strERN, strRowStatus))

        If drs.Length <> 1 Then
            Throw New Exception(String.Format("spTokenManagement.BuildDetail: Unexpected value (drs.Length={0})", drs.Length))
        End If

        Dim dr As DataRow = drs(0)

        Dim udtACMaintenanceBLL As New AccountChangeMaintenanceBLL
        Dim udtACMaintenance As AccountChangeMaintenanceModel = Nothing

        Dim blnPendingACMaintRecord As Boolean = False

        ' Write Audit Log
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ERN", strERN)
        udtAuditLogEntry.AddDescripton("SPID", dr("SP_ID"))
        udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Select")

        ' Service Provider Name
        If dr("SP_Chi_Name").ToString.Trim = String.Empty Then
            lblDSPNameEN.Text = dr("SP_Eng_Name").ToString.Trim
            lblDSPNameCH.Visible = False

        Else
            lblDSPNameEN.Text = dr.Item("SP_Eng_Name").ToString.Trim
            lblDSPNameCH.Text = String.Format(" ({0})", dr("SP_Chi_Name").ToString.Trim)
            lblDSPNameCH.Visible = True

        End If

        ' Service Provider ID
        Dim strSPID As String = IIf(dr("SP_ID").ToString.Trim = String.Empty, "N/A", dr("SP_ID").ToString.Trim)
        lblDSPID.Text = strSPID
        Session(SESS_SPID) = strSPID

        ' Service Provider Status
        Dim strSPStatus As String = dr("Record_Status").ToString.Trim
        Status.GetDescriptionFromDBCode(ServiceProviderTokenStatus.ClassCode, strSPStatus, lblDSPStatus.Text, String.Empty)

        ' Token Issued By
        Dim strProject As String = String.Empty
        Dim strTokenSerialNo As String = String.Empty
        Dim strIsShareToken As String = String.Empty

        strProject = dr("Project").ToString.Trim
        strTokenSerialNo = dr("Token_Serial_No").ToString.Trim
        strIsShareToken = dr("Is_Share_Token").ToString.Trim

        Session(SESS_Project) = strProject
        Session(SESS_TokenSerialNo) = strTokenSerialNo
        Session(SESS_IsShareToken) = strIsShareToken

        Dim udtStaticDataBLL As New StaticDataBLL
        lblDTokenIssuedBy.Text = CStr(udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKEN_ISSUE_BY", strProject).DataValue).Trim()

        ' Token Serial No.
        lblDTokenSerialNo.Text = TokenModel.DisplayTokenSerialNo(strTokenSerialNo, strProject, False, False, True)

        ' Is Share Token
        lblDIsShareToken.Text = CStr(udtStaticDataBLL.GetStaticDataByColumnNameItemNo("YesNo", strIsShareToken).DataValue).Trim()

        ' Token Status
        Dim strTokenStatus As String = dr("Token_Status").ToString.Trim
        Status.GetDescriptionFromDBCode(TokenStatus.ClassCode, strTokenStatus, lblDTokenStatus.Text, String.Empty)
        lblDTokenStatus.Visible = True
        lblDTokenNewStatus.Visible = False

        Session(SESS_Token_Status) = strTokenStatus
        Session(SESS_PPI_Status) = dr("Already_Joined_EHR").ToString.Trim

        Dim strReplacementTokenSerialNo As String = dr("Token_Serial_No_Replacement").ToString.Trim

        If strReplacementTokenSerialNo <> String.Empty Then
            Dim strProjectReplacement As String = String.Empty
            Dim strIsShareTokenReplacement As String = String.Empty

            strProjectReplacement = dr("Project_Replacement").ToString.Trim
            strIsShareTokenReplacement = dr("Is_Share_Token_Replacement").ToString.Trim

            Session(SESS_ProjectReplacement) = strProjectReplacement
            Session(SESS_IsShareTokenReplacement) = strIsShareTokenReplacement

            lblDRTokenSerialNo.Text = TokenModel.DisplayTokenSerialNo(strReplacementTokenSerialNo, strProjectReplacement, False, False, True)
            lblDRTokenIssuedBy.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKEN_ISSUE_BY", strProjectReplacement).DataValue
            lblDRIsShareToken.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("YesNo", strIsShareTokenReplacement).DataValue

            Session(SESS_ReplacementToken) = strReplacementTokenSerialNo

            Dim strLastReplacementReason As String = dr("Last_Replacement_Reason").ToString.Trim

            If strLastReplacementReason <> String.Empty Then
                If strProject = TokenProjectType.EHCVS Then
                    lblDRReplacementReason.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKEN_REPLACE_REASON", strLastReplacementReason).DataValue
                ElseIf strProject = TokenProjectType.EHR Then
                    lblDRReplacementReason.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TokenReplaceReasonEHR", strLastReplacementReason).DataValue
                Else
                    Throw New Exception(String.Format("spTokenManagement.BuildDetail: Unexpected value (strProject={0})", strProject))
                End If

            End If

        Else
            Session(SESS_ReplacementToken) = String.Empty
            Session(SESS_ProjectReplacement) = String.Empty
            Session(SESS_IsShareTokenReplacement) = String.Empty

        End If

        ' --- Init control (default the controls to Active SP + Active Token) ---

        ' Token panel
        panDExistingToken.GroupingText = String.Empty
        panDNewToken.Visible = False

        ' Token Issued By
        lblDTokenIssuedBy.Visible = True
        tblDTokenIssuedBy.Visible = False

        ' Token Serial No.
        lblDTokenSerialNo.Visible = True
        txtDTokenSerialNo.Visible = False
        ibtnDGetFromEHRSS.Visible = False
        ibtnDClear.Visible = False
        hfDObtainFromEHRSS.Value = String.Empty
        trDEHRSSReplacementToken.Visible = False
        trDUserDeclareEHRSSUser.Visible = False

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
        ' --------------------------------------------------------------------------------------------------------------------------------
        ' PCD
        trDPCDStatus.Visible = False
        trDPCDProfessional.Visible = False
        trDUserDeclareJoinPCD.Visible = False
        ibtnDJoinPCD.Visible = False
        ibtnDJoinPCDDisable.Visible = False
        Session(SESS_PCDCheckAccountStatusResult) = Nothing
        Session(SESS_PCDProfessionalChecked) = Nothing
        Session(SESS_PCDStatusChecked) = Nothing
        Session(SESS_JoinPCDClick) = Nothing
        Session(SESS_PCDDelistedConfirmed) = Nothing
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
        ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Session(SESS_PCDSuspended) = Nothing
        ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]

        ' Is Shared Token
        lblDIsShareToken.Visible = True
        tblDIsShareToken.Visible = False

        ' Token Status
        trDTokenStatus.Visible = True
        lblDTokenStatus.Visible = True

        ' New Enrolled Scheme
        trDNewEnrolledScheme.Visible = False

        ' Deactivate Reason
        trDDeactivationTime.Visible = False
        trDDeactivationApprovedBy.Visible = False
        trDDeactivateReason.Visible = False

        Select Case strRowStatus
            Case ServiceProviderTokenStatus.NewEnrolment
                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' --------------------------------------------------------------------------------------------------------------------------------
                trDNewEnrolledScheme.Visible = True
                lblDNewEnrolledScheme.Text = dr("Scheme_Code").ToString.Trim

                UpdatePCDStatus(udtSP)

                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
                lblDTokenIssuedBy.Visible = False
                tblDTokenIssuedBy.Visible = True

                If udtSP.AlreadyJoinEHR = YesNo.Yes Then trDUserDeclareEHRSSUser.Visible = True

                lblDTokenSerialNo.Visible = False
                txtDTokenSerialNo.Visible = True
                txtDTokenSerialNo.Enabled = True
                ibtnDGetFromEHRSS.Visible = True

                lblDIsShareToken.Visible = False
                tblDIsShareToken.Visible = True

                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                mvDButton.SetActiveView(vNewEnrolment)
                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                hfDCurrentAction.Value = DetailPageCurrentAction.NewEnrolment

            Case ServiceProviderTokenStatus.Active
                Select Case strTokenStatus
                    Case TokenStatus.Active
                        ' Check AccountChangeMaintenanceTable
                        udtACMaintenance = udtACMaintenanceBLL.GetRecordbyKeyValue(strSPID, SPAccountMaintenanceUpdTypeStatus.TokenDeactivate, String.Empty)

                        If Not IsNothing(udtACMaintenance) Then
                            ' Token Status
                            Dim strTokenPendingStatus As String = String.Empty
                            Status.GetDescriptionFromDBCode(TokenPendingStatus.Classcode, TokenPendingStatus.PendingDeactivate, strTokenPendingStatus, String.Empty)
                            lblDTokenStatus.Text += String.Format(" ({0})", strTokenPendingStatus)

                            ' Deactivate Reason
                            lblDDeactivateReason.Text = CStr(udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKENDISABLE", udtACMaintenance.TokenRemark).DataValue).Trim()
                            trDDeactivateReason.Visible = True
                            tblDDeactivateReason.Visible = False
                            lblDDeactivateReason.Visible = True

                            blnPendingACMaintRecord = True

                        End If

                        If Not IsNothing(udtACMaintenanceBLL.GetRecordbyKeyValue(strSPID, SPAccountMaintenanceUpdTypeStatus.SPSuspend, String.Empty)) _
                               OrElse Not IsNothing(udtACMaintenanceBLL.GetRecordbyKeyValue(strSPID, SPAccountMaintenanceUpdTypeStatus.SPDelist, String.Empty)) _
                               OrElse Not IsNothing(udtACMaintenanceBLL.GetRecordbyKeyValue(strSPID, SPAccountMaintenanceUpdTypeStatus.SPReactivate, String.Empty)) Then

                            blnPendingACMaintRecord = True

                        End If

                        If blnPendingACMaintRecord Then
                            ' Message: The service provider has record(s) pending for approval in "Account Change Confirmation". No token management action can be proceeded at the moment.
                            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00038)
                            udcInfoMessageBox.Type = InfoMessageBoxType.Information
                            udcInfoMessageBox.BuildMessageBox()

                            mvDButton.SetActiveView(vBackOnly)

                            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Select successful")

                        Else
                            mvDButton.SetActiveView(vManageToken)

                            ibtnDManageTokenReplace.Enabled = strProject = TokenProjectType.EHCVS
                            ibtnDManageTokenReplace.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnDManageTokenReplace.Enabled, "ReplaceTokenBtn", "ReplaceTokenDisableBtn"))

                            ' RSA
                            If (New TokenBLL).IsDisableAdminFeature Then
                                ' Message: Token server is temporarily unavailable. Please try again later.
                                udcInfoMessageBox.AddMessage(Me.FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)
                                udcInfoMessageBox.Type = InfoMessageBoxType.Information
                                udcInfoMessageBox.BuildMessageBox()

                                mvDButton.SetActiveView(vBackOnly)
                            End If

                            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Select successful")

                        End If

                    Case TokenStatus.Deactivated
                        trDDeactivationTime.Visible = True
                        trDDeactivationApprovedBy.Visible = True
                        trDDeactivateReason.Visible = True
                        tblDDeactivateReason.Visible = False
                        lblDDeactivateReason.Visible = True

                        ' Find the Deactivate Reason
                        Dim dtTDR As DataTable = (New TokenBLL).GetTokenSerialNoByUserID(strSPID, New Database)
                        Dim drTDR As DataRow = dtTDR.Rows(0)

                        Dim strTokenDeactivateBy As String = CStr(drTDR("Deactivate_By")).Trim

                        lblDDeactivationTime.Text = (New Formatter).formatDateTime(drTDR("Dtm"))
                        lblDDeactivationApprovedBy.Text = strTokenDeactivateBy

                        If strTokenDeactivateBy = "eHRSS" Then
                            lblDDeactivateReason.Text = Me.GetGlobalResourceObject("Text", "RequestedByEHRSS")
                        Else
                            lblDDeactivateReason.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKENDISABLE", drTDR("Remark")).DataValue.ToString.Trim
                        End If

                        ' Check AccountChangeMaintenanceTable
                        udtACMaintenance = udtACMaintenanceBLL.GetRecordbyKeyValue(strSPID, SPAccountMaintenanceUpdTypeStatus.TokenActivate, String.Empty)
                        If Not udtACMaintenance Is Nothing Then
                            strTokenSerialNo = udtACMaintenance.TokenSerialNo
                            strProject = udtACMaintenance.Project
                            strIsShareToken = IIf(udtACMaintenance.IsShareToken.Value, YesNo.Yes, YesNo.No)

                            Session(SESS_TokenSerialNo) = strTokenSerialNo
                            Session(SESS_Project) = strProject
                            Session(SESS_IsShareToken) = strIsShareToken

                            lblDTokenSerialNo.Text = TokenModel.DisplayTokenSerialNo(strTokenSerialNo, strProject, False, False, True)
                            lblDTokenIssuedBy.Text = CStr(udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKEN_ISSUE_BY", strProject).DataValue).Trim()
                            lblDIsShareToken.Text = CStr(udtStaticDataBLL.GetStaticDataByColumnNameItemNo("YesNo", strIsShareToken).DataValue).Trim()

                            ' Token Status
                            Dim strTokenPendingStatus As String = String.Empty
                            Status.GetDescriptionFromDBCode(TokenPendingStatus.Classcode, TokenPendingStatus.PendingReactivate, strTokenPendingStatus, "")
                            lblDTokenStatus.Text += String.Format(" ({0})", strTokenPendingStatus)

                            blnPendingACMaintRecord = True

                        ElseIf Not IsNothing(udtACMaintenanceBLL.GetRecordbyKeyValue(strSPID, SPAccountMaintenanceUpdTypeStatus.SPSuspend, String.Empty)) _
                                OrElse Not IsNothing(udtACMaintenanceBLL.GetRecordbyKeyValue(strSPID, SPAccountMaintenanceUpdTypeStatus.SPDelist, String.Empty)) _
                                OrElse Not IsNothing(udtACMaintenanceBLL.GetRecordbyKeyValue(strSPID, SPAccountMaintenanceUpdTypeStatus.SPReactivate, String.Empty)) Then
                            blnPendingACMaintRecord = True

                            If lblDTokenIssuedBy.Text = String.Empty Then
                                lblDTokenSerialNo.Text = Me.GetGlobalResourceObject("Text", "N/A")
                                lblDTokenSerialNo.Visible = True
                                lblDTokenIssuedBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
                                lblDTokenIssuedBy.Visible = True
                                lblDIsShareToken.Text = Me.GetGlobalResourceObject("Text", "N/A")
                                lblDIsShareToken.Visible = True

                            End If

                        End If

                        If blnPendingACMaintRecord Then
                            ' Message: The service provider has record(s) pending for approval in "Account Change Confirmation". No token management action can be proceeded at the moment.
                            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00038)
                            udcInfoMessageBox.Type = InfoMessageBoxType.Information
                            udcInfoMessageBox.BuildMessageBox()

                            mvDButton.SetActiveView(vBackOnly)
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Select successful")

                        Else
                            If (New TokenBLL).IsDisableAdminFeature Then
                                ' Message: Token server is temporarily unavailable. Please try again later.
                                udcInfoMessageBox.AddMessage(Me.FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)
                                udcInfoMessageBox.Type = InfoMessageBoxType.Information
                                udcInfoMessageBox.BuildMessageBox()

                                lblDTokenSerialNo.Text = Me.GetGlobalResourceObject("Text", "N/A")
                                lblDTokenIssuedBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
                                lblDIsShareToken.Text = Me.GetGlobalResourceObject("Text", "N/A")

                                mvDButton.SetActiveView(vBackOnly)

                            Else
                                lblDTokenIssuedBy.Visible = False
                                tblDTokenIssuedBy.Visible = True
                                rblDTokenIssuedBy.ClearSelection()

                                lblDTokenSerialNo.Visible = False
                                txtDTokenSerialNo.Visible = True
                                txtDTokenSerialNo.Enabled = True
                                ibtnDGetFromEHRSS.Visible = True

                                txtDTokenSerialNo.Text = String.Empty

                                lblDIsShareToken.Visible = False
                                tblDIsShareToken.Visible = True
                                rblDIsShareToken.ClearSelection()

                                mvDButton.SetActiveView(vSaveCancel)

                                hfDCurrentAction.Value = DetailPageCurrentAction.ActivateToken

                            End If

                            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Select successful")

                        End If

                    Case Else
                        Throw New Exception(String.Format("spTokenManagement.BuildDetail: Unexpected value (strRowStatus={0}, strTokenStatus={1})", _
                                                          strRowStatus, strTokenStatus))

                End Select

            Case ServiceProviderTokenStatus.Suspeneded
                If Not IsNothing(udtACMaintenanceBLL.GetRecordbyKeyValue(strSPID, SPAccountMaintenanceUpdTypeStatus.SPDelist, String.Empty)) _
                        OrElse Not IsNothing(udtACMaintenanceBLL.GetRecordbyKeyValue(strSPID, SPAccountMaintenanceUpdTypeStatus.SPReactivate, String.Empty)) Then
                    udcInfoMessageBox.Type = InfoMessageBoxType.Information
                    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00038)
                    udcInfoMessageBox.BuildMessageBox()

                End If

                udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Select successful")

                mvDButton.SetActiveView(vBackOnly)

            Case ServiceProviderTokenStatus.Delisted
                lblDTokenIssuedBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblDTokenSerialNo.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblDIsShareToken.Text = Me.GetGlobalResourceObject("Text", "N/A")

                Dim strTokenDeactivateReason As String = String.Empty

                Dim dtTDR As DataTable = (New TokenBLL).GetTokenSerialNoByUserID(strSPID, New Database)
                If dtTDR.Rows.Count > 0 Then
                    strTokenDeactivateReason = CStr(dtTDR.Rows(0)("Remark"))
                End If

                lblDDeactivateReason.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKENDISABLE", strTokenDeactivateReason).DataValue.ToString.Trim

                trDDeactivateReason.Visible = True
                lblDDeactivateReason.Visible = True
                tblDDeactivateReason.Visible = False

                udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Select successful")

                mvDButton.SetActiveView(vBackOnly)

            Case ServiceProviderTokenStatus.SchemeEnrolment
                lblDNewEnrolledScheme.Text = dr("Scheme_Code").ToString.Trim

                trDTokenStatus.Visible = False
                trDNewEnrolledScheme.Visible = True

                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' --------------------------------------------------------------------------------------------------------------------------------
                UpdatePCDStatus(udtSP)

                hfDCurrentAction.Value = DetailPageCurrentAction.SchemeEnrolment
                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

                mvDButton.SetActiveView(vSchemeEnrolment)

        End Select

        ' Replacement Token panel
        If strReplacementTokenSerialNo <> String.Empty _
                AndAlso (strRowStatus = ServiceProviderTokenStatus.Active OrElse strRowStatus = ServiceProviderTokenStatus.Suspeneded) Then
            panDExistingToken.GroupingText = Me.GetGlobalResourceObject("Text", "ExistingToken")
            panDNewToken.Visible = True

            txtDRTokenSerialNo.Visible = False
            rblDRReplacementReason.Visible = False

        End If

    End Sub

    '

    Protected Sub ibtnDBack_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00044, "Detail - Back Click")

        ResetMessageAndError()

        If Not IsNothing(Session(SESS_IsUniqueSearch)) AndAlso Session(SESS_IsUniqueSearch) = "Y" Then
            mvCore.SetActiveView(vInputSearch)

        Else
            ShowSearchRecord(False, True)

        End If


        'Return


        'If Not IsNothing(Session(SESS_BackPage)) Then
        '    If Not IsNothing(Session(SESS_IsBackPageSessionHoldLastPageAction)) AndAlso Session(SESS_IsBackPageSessionHoldLastPageAction) = True Then
        '        Select Case Session(SESS_BackPage).trim
        '            Case "Save"
        '                BuildDetail(Session(SESS_ERN), Nothing, Session(SESS_Enrolment_Status))
        '            Case "Deactivate"
        '                BuildDetail(Session(SESS_ERN), Nothing, Session(SESS_Enrolment_Status))
        '                ShowTokenSerialNo(True)
        '            Case "DeactivateAndReplace"
        '                BuildDetail(Session(SESS_ERN), Nothing, Session(SESS_Enrolment_Status))
        '                ShowTokenSerialNo(True)
        '        End Select
        '    Else
        '        Session(SESS_IsBackPageSessionHoldLastPageAction) = Nothing
        '        If Not IsNothing(Session(SESS_Token_UniqueSearch)) AndAlso Session(SESS_Token_UniqueSearch).ToString.Trim.Equals("Y") Then
        '            Session(SESS_Token_UniqueSearch) = Nothing
        '            mvCore.SetActiveView(vInputSearch)
        '        Else
        '            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        '            ' -------------------------------------------------------------------------
        '            'ShowSearchRecord(False)
        '            ShowSearchRecord(False, True)
        '            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
        '        End If
        '    End If

        '    Session(SESS_BackPage) = Nothing
        'Else
        '    Session(SESS_IsBackPageSessionHoldLastPageAction) = Nothing
        '    If Not IsNothing(Session(SESS_Token_UniqueSearch)) AndAlso Session(SESS_Token_UniqueSearch).ToString.Trim.Equals("Y") Then
        '        Session(SESS_Token_UniqueSearch) = Nothing
        '        mvCore.SetActiveView(vInputSearch)
        '    Else
        '        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        '        ' -------------------------------------------------------------------------
        '        'ShowSearchRecord(False)
        '        ShowSearchRecord(False, True)
        '        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
        '    End If
        'End If
    End Sub

    Protected Sub ibtnDReturn_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00043, "Return Click")

        InitControlOnce()

        ResetMessageAndError()

        mvCore.SetActiveView(vInputSearch)

    End Sub

    Protected Sub ibtnDErrorBack_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        ResetMessageAndError()

        ShowSearchRecord(False, True)

    End Sub

    '

    Protected Sub ibtnDManageTokenDeactivate_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00039, "Deactivate Token Click")

        ResetMessageAndError()

        rblDDeactivateReason.ClearSelection()

        ' Vision
        trDDeactivateReason.Visible = True
        lblDDeactivateReason.Visible = False
        tblDDeactivateReason.Visible = True
        mvDButton.SetActiveView(vSaveCancel)

        hfDCurrentAction.Value = DetailPageCurrentAction.DeactivateToken

    End Sub

    Protected Sub ibtnDManageTokenReplace_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00042, "Replace Token Click")

        ResetMessageAndError()

        ' Init values
        txtDRTokenSerialNo.Text = String.Empty
        rblDRReplacementReason.ClearSelection()

        ' Issued By and Is Share Token must be the same as Existing Token
        lblDRTokenIssuedBy.Text = lblDTokenIssuedBy.Text
        lblDRIsShareToken.Text = lblDIsShareToken.Text

        ' Visible
        panDExistingToken.GroupingText = Me.GetGlobalResourceObject("Text", "ExistingToken")
        panDNewToken.Visible = True
        lblDRTokenSerialNo.Visible = False
        txtDRTokenSerialNo.Visible = True
        lblDRReplacementReason.Visible = False
        rblDRReplacementReason.Visible = True

        mvDButton.SetActiveView(vSaveCancel)

        hfDCurrentAction.Value = DetailPageCurrentAction.ReplaceToken

    End Sub

    '

    Protected Sub ibtnDialogConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ResetMessageAndError()

        Dim strSPHKID As String = GetServiceProvider.HKID

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ERN", Session(SESS_ERN))
        udtAuditLogEntry.AddDescripton("SPID", IIf(Session(SESS_SPID) = "N/A", String.Empty, Session(SESS_SPID)))
        udtAuditLogEntry.AddDescripton("SP HKID", strSPHKID)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00019, "Get token from eHRSS")

        Dim udtInXml As InGeteHRTokenInfoXmlModel = Nothing

        Try
            udtInXml = (New eHRServiceBLL).GeteHRSSTokenInfo(strSPHKID)

        Catch ex As Exception
            ' Message: The token information cannot be obtained from eHRSS, please try again later.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006)
            udcMessageBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00021, String.Format("Get token from eHRSS fail (StackTrace={0})", ex.Message))

            ModalPopupExtenderConfirmConsent.Hide()

            Return

        End Try

        If udtInXml.ResultCodeEnum = eHRResultCode.R9002_UserNotFound Then
            ' Message: The service provider is not a registered user in eHRSS.
            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006)
            udcInfoMessageBox.Type = InfoMessageBoxType.Information
            udcInfoMessageBox.BuildMessageBox()

            Return

        End If

        If udtInXml.ResultCodeEnum = eHRResultCode.R1001_NoTokenAssigned OrElse udtInXml.ExistingTokenIssuer = TokenProjectType.EHCVS Then
            ' Message: The service provider is a registered user in eHRSS but is not assigned with an eHRSS token.
            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00007)
            udcInfoMessageBox.Type = InfoMessageBoxType.Information
            udcInfoMessageBox.BuildMessageBox()

            Return

        End If

        ' Fill the value
        rblDTokenIssuedBy.SelectedValue = TokenProjectType.EHR

        txtDTokenSerialNo.Text = udtInXml.ExistingTokenID
        txtDTokenSerialNo.Enabled = False

        ' Check if any replacement token
        If udtInXml.NewTokenID <> String.Empty Then
            txtDEHRSSReplacementToken.Text = udtInXml.NewTokenID
            trDEHRSSReplacementToken.Visible = True
            txtDEHRSSReplacementToken.Visible = True
            lblDEHRSSReplacementToken.Visible = False

        End If

        ibtnDGetFromEHRSS.Visible = False
        ibtnDClear.Visible = True

        rblDIsShareToken.SelectedValue = "Y"

        hfDObtainFromEHRSS.Value = YesNo.Yes

        ' Message: The token information is obtained from eHRSS successfully.
        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00008)
        udcInfoMessageBox.Type = InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

        chkGetFromEHRSS.Checked = False

        ModalPopupExtenderConfirmConsent.Hide()

    End Sub

    '

    Protected Sub ibtnDClear_Click(sender As Object, e As ImageClickEventArgs)
        udcInfoMessageBox.Clear()
        udcMessageBox.Clear()

        rblDTokenIssuedBy.ClearSelection()

        txtDTokenSerialNo.Text = String.Empty
        txtDTokenSerialNo.Enabled = True

        trDEHRSSReplacementToken.Visible = False

        ibtnDGetFromEHRSS.Visible = True
        ibtnDClear.Visible = False

        rblDIsShareToken.ClearSelection()

        hfDObtainFromEHRSS.Value = String.Empty

    End Sub

    '

    Protected Sub ibtnDSave_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00034, "Save Click")

        ResetMessageAndError()

        Dim udtStaticDataBLL As New StaticDataBLL

        Select Case hfDCurrentAction.Value
            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Change button from "Save" to "Proceed"
            'Case DetailPageCurrentAction.NewEnrolment
            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

            Case DetailPageCurrentAction.ActivateToken
                udtAuditLogEntry.AddDescripton("Token Issued By", rblDTokenIssuedBy.SelectedValue)
                udtAuditLogEntry.AddDescripton("Token Serial No.", txtDTokenSerialNo.Text)
                udtAuditLogEntry.AddDescripton("Is Share Token", rblDIsShareToken.SelectedValue)

                If hfDObtainFromEHRSS.Value = YesNo.Yes Then
                    rblDTokenIssuedBy.SelectedValue = TokenProjectType.EHR
                    rblDIsShareToken.SelectedValue = YesNo.Yes
                Else
                    rblDTokenIssuedBy.SelectedValue = TokenProjectType.EHCVS
                    rblDIsShareToken.SelectedValue = YesNo.No
                End If

                If ValidateInputToken() = False Then
                    udcMessageBox.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, LogID.LOG00041, "Save Click fail")

                Else
                    ' Message: Please confirm the following information.
                    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00021)
                    udcInfoMessageBox.Type = InfoMessageBoxType.Information
                    udcInfoMessageBox.BuildMessageBox()

                    ' Token Issused By
                    lblDTokenIssuedBy.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKEN_ISSUE_BY", rblDTokenIssuedBy.SelectedValue).DataValue.ToString.Trim

                    ' Token Serial No.
                    lblDTokenSerialNo.Text = TokenModel.DisplayTokenSerialNo(txtDTokenSerialNo.Text.Trim, rblDTokenIssuedBy.SelectedValue, False, False, True)
                    lblDEHRSSReplacementToken.Text = AntiXssEncoder.HtmlEncode(txtDEHRSSReplacementToken.Text, True)

                    ' Is Share Token
                    lblDIsShareToken.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("YesNo", rblDIsShareToken.SelectedValue).DataValue.ToString.Trim

                    ' Token New Status
                    Dim strTokenStatus As String = String.Empty
                    Dim strTokenPendingStatus As String = String.Empty
                    Status.GetDescriptionFromDBCode(TokenStatus.ClassCode, TokenStatus.Deactivated, strTokenStatus, String.Empty)
                    Status.GetDescriptionFromDBCode(TokenPendingStatus.Classcode, TokenPendingStatus.PendingReactivate, strTokenPendingStatus, String.Empty)
                    lblDTokenNewStatus.Text = String.Format("{0} ({1})", strTokenStatus, strTokenPendingStatus)

                    ' Vision
                    tblDTokenIssuedBy.Visible = False
                    lblDTokenIssuedBy.Visible = True
                    txtDTokenSerialNo.Visible = False
                    lblDTokenSerialNo.Visible = True
                    txtDEHRSSReplacementToken.Visible = False
                    lblDEHRSSReplacementToken.Visible = True
                    ibtnDGetFromEHRSS.Visible = False
                    ibtnDClear.Visible = False
                    tblDIsShareToken.Visible = False
                    lblDIsShareToken.Visible = True
                    lblDTokenStatus.Visible = False
                    lblDTokenNewStatus.Visible = True

                    mvDButton.SetActiveView(vConfirmBack)

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00040, "Save Click success")

                End If

            Case DetailPageCurrentAction.DeactivateToken
                udtAuditLogEntry.AddDescripton("Deactivate Reason", rblDDeactivateReason.SelectedValue)

                If rblDDeactivateReason.SelectedItem Is Nothing Then
                    udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00063)
                    udcMessageBox.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, LogID.LOG00041, "Save Click fail")

                    imgDDeactiveReasonAlert.Visible = True

                Else
                    ' Message: Please confirm the following information.
                    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00021)
                    udcInfoMessageBox.Type = InfoMessageBoxType.Information
                    udcInfoMessageBox.BuildMessageBox()

                    ' Token New Status
                    Dim strTokenStatus As String = String.Empty
                    Dim strTokenPendingStatus As String = String.Empty
                    Status.GetDescriptionFromDBCode(TokenStatus.ClassCode, Session(SESS_Token_Status), strTokenStatus, String.Empty)
                    Status.GetDescriptionFromDBCode(TokenPendingStatus.Classcode, TokenPendingStatus.PendingDeactivate, strTokenPendingStatus, String.Empty)
                    lblDTokenNewStatus.Text = String.Format("{0} ({1})", strTokenStatus, strTokenPendingStatus)

                    lblDDeactivateReason.Text = AntiXssEncoder.HtmlEncode(rblDDeactivateReason.SelectedItem.Text, True)

                    ' Vision
                    lblDTokenNewStatus.Visible = True
                    lblDTokenStatus.Visible = False
                    lblDDeactivateReason.Visible = True
                    tblDDeactivateReason.Visible = False
                    mvDButton.SetActiveView(vConfirmBack)

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00040, "Save Click success")

                End If

            Case DetailPageCurrentAction.ReplaceToken
                udtAuditLogEntry.AddDescripton("Token Serial No.", txtDRTokenSerialNo.Text)
                udtAuditLogEntry.AddDescripton("Replacement Reason", rblDRReplacementReason.SelectedValue)

                If Not ValidateReplaceToken() Then
                    udcMessageBox.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, LogID.LOG00041, "Save Click fail")

                Else
                    ' Message: Please confirm the following information.
                    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00021)
                    udcInfoMessageBox.Type = InfoMessageBoxType.Information
                    udcInfoMessageBox.BuildMessageBox()

                    lblDRTokenSerialNo.Text = txtDRTokenSerialNo.Text.Trim
                    lblDRReplacementReason.Text = AntiXssEncoder.HtmlEncode(rblDRReplacementReason.SelectedItem.Text, True)

                    Session(SESS_ProjectReplacement) = CStr(Session(SESS_Project))
                    Session(SESS_IsShareTokenReplacement) = CStr(Session(SESS_IsShareToken))

                    ' Vision
                    txtDRTokenSerialNo.Visible = False
                    lblDRTokenSerialNo.Visible = True
                    rblDRReplacementReason.Visible = False
                    lblDRReplacementReason.Visible = True

                    mvDButton.SetActiveView(vConfirmBack)

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00040, "Save Click success")

                End If

        End Select

    End Sub

    Protected Sub ibtnDCancel_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00035, "Cancel Click")

        ResetMessageAndError()

        Select Case hfDCurrentAction.Value
            Case DetailPageCurrentAction.NewEnrolment
                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Nothing here
                'ibtnDBack_Click(Nothing, Nothing)                
                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

            Case DetailPageCurrentAction.ActivateToken
                ibtnDBack_Click(Nothing, Nothing)

            Case DetailPageCurrentAction.DeactivateToken
                trDDeactivateReason.Visible = False

                mvDButton.SetActiveView(vManageToken)

            Case DetailPageCurrentAction.ReplaceToken
                If Session(SESS_ReplacementToken) <> String.Empty Then
                    lblDRTokenSerialNo.Visible = True
                    txtDRTokenSerialNo.Visible = False
                    lblDRReplacementReason.Visible = True
                    rblDRReplacementReason.Visible = False

                Else
                    panDExistingToken.GroupingText = String.Empty
                    panDNewToken.Visible = False

                End If

                mvDButton.SetActiveView(vManageToken)

            Case DetailPageCurrentAction.SchemeEnrolment
                ' Nothing here

        End Select

    End Sub

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Protected Sub ibtnDProceed_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00120, "Proceed Click")

        ResetMessageAndError()

        Select Case hfDCurrentAction.Value
            Case DetailPageCurrentAction.NewEnrolment
                udtAuditLogEntry.AddDescripton("Token Issued By", rblDTokenIssuedBy.SelectedValue)
                udtAuditLogEntry.AddDescripton("Token Serial No.", txtDTokenSerialNo.Text)
                udtAuditLogEntry.AddDescripton("Is Share Token", rblDIsShareToken.SelectedValue)

                Dim blnInvalid As Boolean = False

                ' Check Token
                If ValidateInputToken() = False Then
                    blnInvalid = True
                End If

                ' Check Join PCD for new enrolled scheme                
                If ValidatePCDStatus() = False Then
                    blnInvalid = True
                End If

                ' Reset PCD Warning
                Session(SESS_PCDProfessionalChecked) = Nothing
                Session(SESS_PCDStatusChecked) = Nothing

                If blnInvalid Then
                    udcMessageBox.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, LogID.LOG00122, "Proceed Click fail")
                Else
                    ' Success
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00121, "Proceed Click success")
                    ProcessNewEnrolment()
                End If

            Case Else
                ' Do Nothing

        End Select
    End Sub
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    '

    Protected Sub ibtnDConfirmConfirm_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        ResetMessageAndError()

        Dim strERN As String = Session(SESS_ERN)
        Dim strProject As String = Session(SESS_Project)
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser
        Dim udtDB As New Database
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Select Case hfDCurrentAction.Value
            Case DetailPageCurrentAction.NewEnrolment
                Try
                    Dim udtGeneralFunction As New GeneralFunction

                    ' Generate new SPID
                    Dim strNewSPID As String = udtGeneralFunction.generateSPID()
                    Session(SESS_SPID) = strNewSPID

                    Dim strActivationCode As String = udtGeneralFunction.generateAccountActivationCode()

                    Dim udtSP As ServiceProviderModel = (New ServiceProviderBLL).GetServiceProviderStagingProfileByERN(strERN, udtDB)
                    udtSP.SPID = strNewSPID
                    udtSP.UpdateBy = udtHCVUUser.UserID
                    udtSP.CreateBy = udtHCVUUser.UserID

                    Dim udtToken As New TokenModel

                    udtToken.UserID = strNewSPID
                    udtToken.UpdateBy = udtHCVUUser.UserID
                    udtToken.IssueBy = udtHCVUUser.UserID
                    udtToken.RecordStatus = TokenStatus.Active
                    udtToken.Project = rblDTokenIssuedBy.SelectedValue
                    udtToken.IsShareToken = IIf(rblDIsShareToken.SelectedValue = YesNo.Yes, True, False)
                    udtToken.TokenSerialNo = txtDTokenSerialNo.Text.Trim

                    If trDEHRSSReplacementToken.Visible Then
                        udtToken.TokenSerialNoReplacement = txtDEHRSSReplacementToken.Text.Trim
                        udtToken.ProjectReplacement = udtToken.Project
                        udtToken.IsShareTokenReplacement = udtToken.IsShareToken
                        udtToken.LastReplacementReason = "O" ' Hard code to be Others
                        udtToken.LastReplacementDtm = (New GeneralFunction).GetSystemDateTime
                        udtToken.LastReplacementActivateDtm = Nothing
                        udtToken.LastReplacementBy = "eHRSS"

                    End If

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' get PCD Last Check Status
                    Dim udtPCDAccountStatusResult As PCDCheckAccountStatusResult = Session(SESS_PCDCheckAccountStatusResult)
                    If trDPCDStatus.Visible AndAlso Not udtPCDAccountStatusResult Is Nothing Then

                        If udtPCDAccountStatusResult.AccountStatusCode <> PCDAccountStatus.ConnectionFail Then
                            udtSP.PCDAccountStatus = udtPCDAccountStatusResult.AccountStatusCode
                            udtSP.PCDEnrolmentStatus = udtPCDAccountStatusResult.EnrolmentStatusCode
                            udtSP.PCDProfessional = udtPCDAccountStatusResult.ProfID
                            udtSP.PCDStatusLastCheckDtm = udtPCDAccountStatusResult.LastCheckTime
                        End If

                    Else
                        ' SP not able to join PCD
                        udtSP.PCDAccountStatus = PCDAccountStatus.Unavailable
                        udtSP.PCDEnrolmentStatus = PCDEnrolmentStatus.Unavailable
                        udtSP.PCDProfessional = String.Empty
                        udtSP.PCDStatusLastCheckDtm = Nothing
                    End If
                    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

                    ' Write Audit Log
                    udtAuditLogEntry.AddDescripton("ERN", strERN)
                    udtAuditLogEntry.AddDescripton("SPID", strNewSPID)
                    udtAuditLogEntry.AddDescripton("Token Serial No", udtToken.TokenSerialNo)
                    udtAuditLogEntry.AddDescripton("Project Code", udtToken.Project)
                    udtAuditLogEntry.AddDescripton("Is Share Token", CStr(Session(SESS_IsShareToken)).Trim())
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00013, "Issue Token")

                    Dim udtSystemMessage As SystemMessage = (New TokenManagementBLL).CompleteTokenIssue(udtSP, udtToken, strActivationCode, udtDB)

                    mvDButton.SetActiveView(vCompleteNewEnrolment)

                    Dim blnSentActivationEmail As Boolean = False

                    If IsNothing(udtSystemMessage) Then
                        Try
                            Me.udcPrintFunction.PrepareThePanel(String.Empty, spPrintFunction.ActionPrintNewEnrolmentLetter, udtSP, True, FunctionCode)
                            Me.udcPrintFunction.SendActivationEmailWithoutPopup(strActivationCode)

                            blnSentActivationEmail = True

                        Catch ex As Exception
                            udtAuditLogEntry.AddDescripton("AddMailQueueError", ex.ToString())

                        End Try

                        lblDSPID.Text = udtSP.SPID
                        Status.GetDescriptionFromDBCode(ServiceProviderTokenStatus.ClassCode, ServiceProviderTokenStatus.Active, lblDSPStatus.Text, String.Empty)
                        Status.GetDescriptionFromDBCode(TokenStatus.ClassCode, TokenStatus.Active, lblDTokenStatus.Text, String.Empty)

                        If blnSentActivationEmail = True Then
                            udcInfoMessageBox.Type = InfoMessageBoxType.Complete
                            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
                            udcInfoMessageBox.BuildMessageBox()
                        Else
                            udcInfoMessageBox.Type = InfoMessageBoxType.Complete
                            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00010)
                            udcInfoMessageBox.BuildMessageBox()
                        End If

                        ibtnDNewEnrolmentSchemeEnrolmentPrint_Click(Nothing, Nothing)

                        udtAuditLogEntry.AddDescripton("ActivationEmail", blnSentActivationEmail.ToString())
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Issue Token successful")

                    Else
                        udcMessageBox.AddMessage(udtSystemMessage)
                        udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00015, "Issue Token failed")

                        lblDTokenStatus.Text = Me.GetGlobalResourceObject("Text", "NA")

                        mvDButton.SetActiveView(vComplete)

                    End If

                Catch eSQL As SqlException
                    If eSQL.Number = 50000 Then
                        udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message)
                        udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00015, "Issue Token failed")

                    Else
                        Throw

                    End If

                Catch ex As Exception
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
                    udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00015, "Issue Token failed: " + ex.Message)

                End Try

            Case DetailPageCurrentAction.ActivateToken
                Try
                    Dim udtACMaintenanceModel As New AccountChangeMaintenanceModel

                    udtACMaintenanceModel.SPID = lblDSPID.Text
                    udtACMaintenanceModel.UpdateBy = udtHCVUUser.UserID
                    udtACMaintenanceModel.DataInputBy = udtHCVUUser.UserID
                    udtACMaintenanceModel.RecordStatus = SPAccountMaintenanceRecordStatus.Active
                    udtACMaintenanceModel.TokenSerialNo = txtDTokenSerialNo.Text.Trim
                    udtACMaintenanceModel.Project = rblDTokenIssuedBy.SelectedValue
                    udtACMaintenanceModel.IsShareToken = IIf(rblDIsShareToken.SelectedValue = YesNo.Yes, True, False)
                    udtACMaintenanceModel.UpdType = SPAccountMaintenanceUpdTypeStatus.TokenActivate
                    udtACMaintenanceModel.SchemeCode = String.Empty

                    If trDEHRSSReplacementToken.Visible Then
                        udtACMaintenanceModel.TokenSerialNoReplacement = txtDEHRSSReplacementToken.Text.Trim
                        udtACMaintenanceModel.ProjectReplacement = udtACMaintenanceModel.Project
                        udtACMaintenanceModel.IsShareTokenReplacement = udtACMaintenanceModel.IsShareToken
                    End If

                    ' Write Audit Log
                    udtAuditLogEntry.AddDescripton("ERN", strERN)
                    udtAuditLogEntry.AddDescripton("SPID", udtACMaintenanceModel.SPID)
                    udtAuditLogEntry.AddDescripton("Token Serial No", udtACMaintenanceModel.TokenSerialNo)
                    udtAuditLogEntry.AddDescripton("Project Code", strProject.Trim)
                    udtAuditLogEntry.AddDescripton("Is Share Token", rblDIsShareToken.SelectedValue)

                    udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Activate Token")

                    ' Check RSA Server
                    Dim udtSystemMessage As SystemMessage = (New TokenBLL).IsUserIDAndTokenAvailable(udtACMaintenanceModel.SPID, udtACMaintenanceModel.TokenSerialNo)

                    If Not IsNothing(udtSystemMessage) Then
                        udcMessageBox.AddMessage(udtSystemMessage)
                        udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00009, "Activate Token failed")

                        Return

                    End If

                    Call (New AccountChangeMaintenanceBLL).AddRecord(udtACMaintenanceModel, udtDB)

                    ' Message: Token is pending activation.
                    udcInfoMessageBox.Type = InfoMessageBoxType.Complete
                    udcInfoMessageBox.AddMessage(FunctionCode, "I", MsgCode.MSG00002)
                    udcInfoMessageBox.BuildMessageBox()

                    mvDButton.SetActiveView(vComplete)

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Activate Token successful")

                Catch eSQL As SqlException
                    If eSQL.Number = 50000 Then
                        udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message)
                        udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00009, "Activate Token failed")
                    Else
                        Throw
                    End If

                Catch ex As Exception
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
                    udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00009, "Activate Token failed: " + ex.Message)

                End Try

            Case DetailPageCurrentAction.DeactivateToken
                Try
                    Dim udtACMaintenanceModel As New AccountChangeMaintenanceModel

                    udtACMaintenanceModel.SPID = Session(SESS_SPID)
                    udtACMaintenanceModel.UpdateBy = udtHCVUUser.UserID
                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    udtACMaintenanceModel.DataInputBy = udtHCVUUser.UserID
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]
                    udtACMaintenanceModel.RecordStatus = SPAccountMaintenanceRecordStatus.Active
                    udtACMaintenanceModel.UpdType = SPAccountMaintenanceUpdTypeStatus.TokenDeactivate
                    udtACMaintenanceModel.TokenRemark = rblDDeactivateReason.SelectedValue

                    ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]
                    ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'udtACMaintenanceModel.TokenSerialNo = GetTokenSerialNo(Session(SESS_TokenSerialNo), Project)
                    udtACMaintenanceModel.TokenSerialNo = CStr(Session(SESS_TokenSerialNo)).Trim()
                    udtACMaintenanceModel.Project = CStr(Session(SESS_Project)).Trim()
                    udtACMaintenanceModel.IsShareToken = IIf(CStr(Session(SESS_IsShareToken)).Trim().Equals(YesNo.Yes), True, False)
                    ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                    ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

                    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    If Not Session(SESS_ReplacementToken) = String.Empty Then
                        udtACMaintenanceModel.TokenSerialNoReplacement = CStr(Session(SESS_ReplacementToken)).Trim()
                        udtACMaintenanceModel.ProjectReplacement = CStr(Session(SESS_ProjectReplacement)).Trim()
                        udtACMaintenanceModel.IsShareTokenReplacement = IIf(CStr(Session(SESS_IsShareTokenReplacement)).Trim().Equals(YesNo.Yes), True, False)
                    End If
                    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

                    udtACMaintenanceModel.SchemeCode = String.Empty

                    ' Write Audit Log
                    udtAuditLogEntry.AddDescripton("ERN", strERN)
                    udtAuditLogEntry.AddDescripton("SPID", udtACMaintenanceModel.SPID)
                    udtAuditLogEntry.AddDescripton("Token Serial No", udtACMaintenanceModel.TokenSerialNo)
                    udtAuditLogEntry.AddDescripton("Project Code", strProject.Trim)
                    ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    udtAuditLogEntry.AddDescripton("Is Share Token", CStr(Session(SESS_IsShareToken)).Trim())
                    ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Deactivate Token")

                    Call (New AccountChangeMaintenanceBLL).AddRecord(udtACMaintenanceModel, udtDB)

                    ' Message: Token is pending deactivation.
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
                    udcInfoMessageBox.Type = InfoMessageBoxType.Complete
                    udcInfoMessageBox.BuildMessageBox()

                    mvDButton.SetActiveView(vComplete)

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Deactivate Token successful")

                Catch eSQL As SqlException
                    If eSQL.Number = 50000 Then
                        udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message)
                        udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00012, "Deactivate Token failed")

                    Else
                        Throw

                    End If

                Catch ex As Exception
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
                    udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00012, "Deactivate Token failed: " + ex.Message)

                End Try

            Case DetailPageCurrentAction.ReplaceToken
                Try
                    Dim strSPID As String = Session(SESS_SPID)

                    Dim udtToken As TokenModel = (New TokenBLL).GetTokenProfileByUserID(strSPID, Session(SESS_TokenSerialNo), udtDB)
                    udtToken.TokenSerialNoReplacement = txtDRTokenSerialNo.Text.Trim
                    udtToken.LastReplacementReason = rblDRReplacementReason.SelectedValue
                    udtToken.LastReplacementDtm = (New GeneralFunction).GetSystemDateTime
                    udtToken.LastReplacementActivateDtm = Nothing
                    udtToken.LastReplacementBy = udtHCVUUser.UserID
                    udtToken.UpdateBy = udtHCVUUser.UserID
                    udtToken.IsShareTokenReplacement = udtToken.IsShareToken
                    udtToken.ProjectReplacement = udtToken.Project

                    ' Write Audit Log
                    udtAuditLogEntry.AddDescripton("ERN", strERN)
                    udtAuditLogEntry.AddDescripton("SPID", strSPID)
                    udtAuditLogEntry.AddDescripton("Token Serial No (Existing)", udtToken.TokenSerialNo)
                    udtAuditLogEntry.AddDescripton("Project Code (Existing)", udtToken.Project)
                    udtAuditLogEntry.AddDescripton("Is Share Token (Existing)", IIf(udtToken.IsShareToken, YesNo.Yes, YesNo.No))
                    udtAuditLogEntry.AddDescripton("Token Serial No (New)", udtToken.TokenSerialNoReplacement)
                    udtAuditLogEntry.AddDescripton("Project Code (New)", udtToken.ProjectReplacement)
                    udtAuditLogEntry.AddDescripton("Is Share Token (New)", IIf(udtToken.IsShareTokenReplacement.Value, YesNo.Yes, YesNo.No))
                    udtAuditLogEntry.AddDescripton("Replacement Reason", udtToken.LastReplacementReason)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "Replace Token")

                    Dim udtSP As ServiceProviderModel = GetServiceProvider()
                    Dim udtSystemMessage As SystemMessage = (New TokenManagementBLL).ReplaceToken(udtToken, udtDB, udtSP)

                    If IsNothing(udtSystemMessage) Then
                        ' Message: New token assignment for token replacement is completed.
                        udcInfoMessageBox.Type = InfoMessageBoxType.Complete
                        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)
                        udcInfoMessageBox.BuildMessageBox()

                        mvDButton.SetActiveView(vCompleteReplaceToken)

                        ' Show Print button only when the Token is eHS
                        ibtnDCompleteReplaceTokenPrint.Visible = udtToken.ProjectReplacement = TokenProjectType.EHCVS

                        udtAuditLogEntry.WriteEndLog(LogID.LOG00017, "Replace Token successful")

                        ' TokenAction
                        Dim udtTokenBLL As New TokenBLL
                        Dim strActionDtm As String = eHRServiceBLL.GenerateTimestamp

                        udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, EnumTokenActionParty.NA, EnumTokenActionActionType.REPLACETOKEN, _
                                                   udtToken.UserID, udtToken.TokenSerialNo, udtToken.TokenSerialNoReplacement, _
                                                   udtToken.LastReplacementReason, False, EnumTokenActionActionResult.C, strActionDtm, Nothing, _
                                                   String.Empty, String.Empty, udtDB)

                        ' Notify eHRSS to replace token
                        If udtToken.IsShareToken Then
                            Dim eResult As TokenBLL.EnumTokenActionActionResult = Nothing
                            Dim strReferenceQueueID As String = String.Empty

                            Try
                                Dim udtInXml As InReplaceeHRSSTokenXmlModel = (New eHRServiceBLL).ReplaceeHRSSToken(udtSP.HKID, udtToken.TokenSerialNo, _
                                                                                udtToken.TokenSerialNoReplacement, udtToken.LastReplacementReason, _
                                                                                strActionDtm, strReferenceQueueID)

                                Select Case udtInXml.ResultCodeEnum
                                    Case eHRResultCode.R1000_Success
                                        eResult = EnumTokenActionActionResult.C
                                    Case eHRResultCode.R9999_UnexpectedFailure
                                        eResult = EnumTokenActionActionResult.F
                                    Case Else
                                        eResult = EnumTokenActionActionResult.R
                                End Select

                            Catch ex As Exception
                                ' Just ignore it
                                eResult = EnumTokenActionActionResult.F

                                udtAuditLogEntry.WriteLog(LogID.LOG00019, String.Format("Notify eHRSS Replace Token exception: {0}", ex.Message))

                            End Try

                            udtTokenBLL.AddTokenAction(EnumTokenActionParty.EHS, EnumTokenActionParty.EHR, EnumTokenActionActionType.NOTIFYREPLACETOKEN, _
                                                       udtToken.UserID, udtToken.TokenSerialNo, udtToken.TokenSerialNoReplacement, _
                                                       udtToken.LastReplacementReason, False, eResult, strActionDtm, DateTime.Now, strActionDtm, _
                                                       strReferenceQueueID, udtDB)

                        End If

                    Else
                        udcMessageBox.AddMessage(udtSystemMessage)
                        udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00018, "Replace Token failed")

                    End If

                Catch eSQL As SqlException
                    If eSQL.Number = 50000 Then
                        udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message)
                        udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00018, "Replace Token failed")

                    Else
                        Throw

                    End If

                Catch ex As Exception
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
                    udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00018, "Replace Token failed: " + ex.Message)

                End Try

        End Select

    End Sub

    Protected Sub ibtnDConfirmBack_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00035, "Back Click")

        ResetMessageAndError()

        Select Case hfDCurrentAction.Value
            Case DetailPageCurrentAction.NewEnrolment, DetailPageCurrentAction.ActivateToken
                lblDTokenIssuedBy.Visible = False
                tblDTokenIssuedBy.Visible = True

                lblDTokenSerialNo.Visible = False
                txtDTokenSerialNo.Visible = True

                lblDEHRSSReplacementToken.Visible = False
                txtDEHRSSReplacementToken.Visible = True

                lblDIsShareToken.Visible = False
                tblDIsShareToken.Visible = True

                lblDTokenNewStatus.Visible = False
                lblDTokenStatus.Visible = True

                If hfDObtainFromEHRSS.Value = YesNo.Yes Then
                    ibtnDGetFromEHRSS.Visible = False
                    ibtnDClear.Visible = True
                Else
                    ibtnDGetFromEHRSS.Visible = True
                    ibtnDClear.Visible = False

                End If

                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If Not Session(SESS_PCDCheckAccountStatusResult) Is Nothing Then
                    SetupJoinPCDButton()
                End If
                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If hfDCurrentAction.Value = DetailPageCurrentAction.NewEnrolment Then
                    mvDButton.SetActiveView(vNewEnrolment)
                Else
                    mvDButton.SetActiveView(vSaveCancel)
                End If
                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

            Case DetailPageCurrentAction.DeactivateToken
                lblDTokenNewStatus.Visible = False
                lblDTokenStatus.Visible = True
                lblDDeactivateReason.Visible = False
                tblDDeactivateReason.Visible = True

                mvDButton.SetActiveView(vSaveCancel)

            Case DetailPageCurrentAction.ReplaceToken
                lblDRTokenSerialNo.Visible = False
                txtDRTokenSerialNo.Visible = True
                lblDRReplacementReason.Visible = False
                rblDRReplacementReason.Visible = True

                mvDButton.SetActiveView(vSaveCancel)

        End Select

    End Sub

    '

    Private Function ValidateInputToken() As Boolean
        Dim blnValidatedSuccess As Boolean = True

        ResetMessageAndError()

        If (New Validator).IsEmpty(txtDTokenSerialNo.Text.Trim) Then
            ' Message: Please input "Token Serial No.".
            udcMessageBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00337)
            imgDTokenSerialNoAlert.Visible = True
            blnValidatedSuccess = False

        Else
            If Not IsNumeric(txtDTokenSerialNo.Text) Then
                ' Message: "Token Serial No." is invalid.
                udcMessageBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00341)
                imgDTokenSerialNoAlert.Visible = True
                blnValidatedSuccess = False

            End If

        End If

        Return blnValidatedSuccess

    End Function

    Private Function ValidateReplaceToken() As Boolean
        Dim blnValidatedSuccess As Boolean = True
        Dim udtValidator As New Validator

        ResetMessageAndError()

        ' Validate [txtbox_sysInfo_ReplacementTokenSerialNo]
        If udtValidator.IsEmpty(txtDRTokenSerialNo.Text.Trim()) Then
            udcMessageBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00337)
            imgDRTokenSerialNoAlert.Visible = True
            blnValidatedSuccess = False
        Else
            If Not IsNumeric(txtDRTokenSerialNo.Text) Then
                udcMessageBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00341)
                imgDRTokenSerialNoAlert.Visible = True
                blnValidatedSuccess = False
            End If

            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If lblDTokenSerialNo.Text.Trim.Equals(txtDRTokenSerialNo.Text.Trim) Then
                udcMessageBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00343)
                imgDRTokenSerialNoAlert.Visible = True
                blnValidatedSuccess = False
            End If
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
        End If

        ' Validate [rb_sysInfo_ReplaceReason]
        If rblDRReplacementReason.SelectedIndex = -1 Then
            udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00330)
            imgDRReplacementReasonAlert.Visible = True
            blnValidatedSuccess = False

        End If

        Return blnValidatedSuccess

    End Function

    '

    Private Sub ConfirmSchemeEnrolment()

        Dim udtSPBLL As New ServiceProviderBLL
        Dim udtDB As New Database
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim blnInvalid As Boolean = False

        ResetMessageAndError()

        Try
            Dim udtSP As ServiceProviderModel = udtSPBLL.GetServiceProviderStagingProfileByERN(Session(SESS_ERN), udtDB)

            ' Write Audit Log
            udtAuditLogEntry.WriteLog(LogID.LOG00117, "Confirm Scheme Enrolment")

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Check Join PCD for new enrolled scheme
            If ValidatePCDStatus() = False Then
                blnInvalid = True
            End If

            ' Reset PCD Warning
            Session(SESS_PCDProfessionalChecked) = Nothing
            Session(SESS_PCDStatusChecked) = Nothing

            If blnInvalid Then
                udcMessageBox.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, LogID.LOG00027, "Scheme Enrolment failed")
            Else
                ProcessSchemeEnrolment()
            End If

        Catch
            Throw

        End Try
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
    End Sub

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Protected Sub ibtnDConfirmSchemeEnrolment_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00131, "Confirm Scheme Enrolment Click")

        hfDEnrolmentAction.Value = EnrolmentAction.ConfirmSchemeEnrolment

        ucEnrolmentActionPopup.MessageText = (New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVQ, MsgCode.MSG00002)).GetMessage
        ModalPopupEnrolmentAction.PopupDragHandleControlID = ucEnrolmentActionPopup.Header.ClientID
        ModalPopupEnrolmentAction.Show()
    End Sub

    Protected Sub ibtnReject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00123, "Reject Click")

        hfDEnrolmentAction.Value = EnrolmentAction.Reject

        ucEnrolmentActionPopup.MessageText = (New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVQ, MsgCode.MSG00001)).GetMessage
        ModalPopupEnrolmentAction.PopupDragHandleControlID = ucEnrolmentActionPopup.Header.ClientID
        ModalPopupEnrolmentAction.Show()
    End Sub

    Protected Sub ibtnReturnForAmendment_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00132, "Return For Amendment Click")

        hfDEnrolmentAction.Value = EnrolmentAction.ReturnForAmendment

        ucEnrolmentActionPopup.MessageText = (New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVQ, MsgCode.MSG00003)).GetMessage
        ModalPopupEnrolmentAction.PopupDragHandleControlID = ucEnrolmentActionPopup.Header.ClientID
        ModalPopupEnrolmentAction.Show()
    End Sub

    Private Sub ucEnrolmentActionPopup_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucEnrolmentActionPopup.ButtonClick
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)        

        Select Case e
            Case ucNoticePopup.enumButtonClick.OK
                udtAuditLogEntry.WriteLog(LogID.LOG00133, "Confirmation Popup - Confirm Click")

                Select Case hfDEnrolmentAction.Value
                    Case EnrolmentAction.ConfirmSchemeEnrolment
                        ConfirmSchemeEnrolment()

                    Case EnrolmentAction.Reject
                        RejectEnrolment()

                    Case EnrolmentAction.ReturnForAmendment
                        ReturnForAmendment()

                End Select

            Case ucNoticePopup.enumButtonClick.Cancel
                udtAuditLogEntry.WriteLog(LogID.LOG00134, "Confirmation Popup - Cancel Click")
                ModalPopupEnrolmentAction.Hide()

        End Select

    End Sub

    Private Sub ReturnForAmendment()

        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
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

            udtSP = udtServiceProviderBLL.GetSP

            Dim dtErrorMessage As DataTable = New DataTable("ErrorMessage")

            If Me.CheckValidationClickReturnForAmendment(udtSP, dtErrorMessage) Then

                If udtSPAccountUpdateBLL.Exist AndAlso udtServiceProviderBLL.Exist Then
                    udtSPAccountUpdateModel = udtSPAccountUpdateBLL.GetSPAccountUpdate
                    udtSP = udtServiceProviderBLL.GetSP

                    udtAuditLogEntry.AddDescripton("ERN", udtSP.EnrolRefNo)
                    udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00124, "Return For Amendment")


                    If (New SPProfileBLL).ReturnForAmendmentFromUserE(udtSP.EnrolRefNo, udtHCVUUser.UserID, udtSPAccountUpdateModel.TSMP) Then
                        Dim strOld As String() = {"%s"}
                        Dim strNew As String() = {""}
                        Dim udtFormatter As New Formatter

                        If udtSP.SPID.Equals(String.Empty) Then
                            strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                        udtFormatter.formatSystemNumber(udtSP.EnrolRefNo) + "] "
                        Else
                            strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                        udtSP.SPID + "] "
                        End If

                        'Message: The record of {SP} is returned for amendment.
                        udcInfoMessageBox.Type = InfoMessageBoxType.Complete
                        udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00010, strOld, strNew)
                        udcInfoMessageBox.BuildMessageBox()

                        mvCore.SetActiveView(vCompletePage)

                        udtAuditLogEntry.WriteEndLog(LogID.LOG00125, "Return For Amendment success")
                    End If
                End If

            Else
                For Each drErrorMessage As DataRow In dtErrorMessage.Select()
                    If drErrorMessage.Item("IsReplace") Then
                        udcMessageBox.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"), drErrorMessage.Item("FindString"), drErrorMessage.Item("ReplaceString"))
                    Else
                        udcMessageBox.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"))
                    End If
                Next

                udcMessageBox.BuildMessageBox(_strValidationFailTitle, udtAuditLogEntry, LogID.LOG00126, "Return For Amendment abort")
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                udcMessageBox.AddMessage(New Common.ComObject.SystemMessage("990001", "D", strmsg))
                If udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                    udcMessageBox.Visible = False
                Else
                    udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00127, "Return for amendment fail")
                End If

            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    '' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub RejectEnrolment()

        ' Reject Enrolment

        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
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
            If udtSPAccountUpdateBLL.Exist AndAlso udtServiceProviderBLL.Exist Then
                udtSPAccountUpdateModel = udtSPAccountUpdateBLL.GetSPAccountUpdate
                udtSP = udtServiceProviderBLL.GetSP

                udtAuditLogEntry.AddDescripton("ERN", udtSP.EnrolRefNo)
                udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00128, "Reject:")


                If (New SPProfileBLL).RejectSPProfileFromUserE(udtSP.EnrolRefNo, udtHCVUUser.UserID, udtSPAccountUpdateModel.TSMP) Then
                    Dim strOld As String() = {"%s"}
                    Dim strNew As String() = {""}
                    Dim udtFormatter As New Formatter

                    If udtSP.SPID.Equals(String.Empty) Then
                        strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                    udtFormatter.formatSystemNumber(udtSP.EnrolRefNo) + "] "
                    Else
                        strNew(0) = udtSP.EnglishName + udtFormatter.formatChineseName(udtSP.ChineseName) + " [" + _
                                    udtSP.SPID + "] "
                    End If

                    ' Message: The request of {SP} is rejected.
                    udcInfoMessageBox.Type = InfoMessageBoxType.Complete
                    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00009, strOld, strNew)
                    udcInfoMessageBox.BuildMessageBox()

                    mvCore.SetActiveView(vCompletePage)

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00129, "Reject Successful")
                End If
            End If


        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                udcMessageBox.AddMessage(New Common.ComObject.SystemMessage("990001", "D", strmsg))

                If udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                    udcMessageBox.Visible = False
                Else
                    udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00130, "Reject Fail")
                End If
            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    '

    Protected Sub ibtnDNewEnrolmentSchemeEnrolmentPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strERN As String = Session(SESS_ERN)
        Dim strSPID As String = Session(SESS_SPID)
        Dim udtSP As ServiceProviderModel = Session("TempSPmodel")
        Dim strStatus As String = Session("Status")

        udcPrintFunction.Visible = False
        udcPrintFunction.PrepareThePanel("SchemeSelection", False, Nothing, False, FunctionCode)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Try
            Select Case strStatus
                Case ServiceProviderTokenStatus.NewEnrolment
                    udtAuditLogEntry.AddDescripton("ERN", strERN)
                    udtAuditLogEntry.AddDescripton("SPID", strSPID)
                    udtAuditLogEntry.AddDescripton("SP Token Status", ServiceProviderTokenStatus.NewEnrolment)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00028, "Print Letter Click")

                    'get new Token serial no and SPID
                    Dim udtSPPermanent As ServiceProviderModel = (New ServiceProviderBLL).GetServiceProviderBySPID(New Database, strSPID)
                    udtSPPermanent.TokenSerialNo = (New SPProfileBLL).GetTokenTokenSerialNoBySPID(udtSPPermanent.SPID)

                    udcPrintFunction.SPPermanent = True
                    udcPrintFunction.PrepareThePanel(String.Empty, spPrintFunction.ActionPrintNewEnrolmentLetter, udtSPPermanent, True, FunctionCode)
                    udcPrintFunction.PrintLetterWithoutPopup()

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00029, "Print Letter Click successful")

                Case ServiceProviderTokenStatus.SchemeEnrolment
                    udtAuditLogEntry.AddDescripton("ERN", strERN)
                    udtAuditLogEntry.AddDescripton("SPID", strSPID)
                    udtAuditLogEntry.AddDescripton("SP Token Status", ServiceProviderTokenStatus.SchemeEnrolment)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00028, "Print Letter Click")

                    If IsNothing(udtSP) Then
                        udtSP = (New ServiceProviderBLL).GetServiceProviderStagingProfileByERN(strERN, New Database)
                    End If

                    'Scheme enrolment
                    Me.udcPrintFunction.SPPermanent = False

                    'Send Email and Print Letter
                    popupPrintSchemeEnrolmentLetter.Show()

                    If Not IsNothing(Session("Printed")) AndAlso Session("Printed") <> True Then
                        udcPrintSchemeEnrolmentLetter.PrepareThePanel("SchemeSelection", spPrintFunction.ActionPrintAndSendSchemeEnrolment, udtSP, True, FunctionCode)
                        'Session("Printed") = True
                    Else
                        udcPrintSchemeEnrolmentLetter.PrepareThePanel("SchemeSelection", spPrintFunction.ActionPrintSchemeEnrolmentLetter, udtSP, True, FunctionCode)
                    End If

                    udcPrintSchemeEnrolmentLetter.SetDefaultSelectedScheme(udtSP)

                    udcPrintSchemeEnrolmentLetter.Activate()

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00029, "Print Letter Click successful")

                Case Else
                    Throw New Exception(String.Format("spTokenManagement.ibtnDNewEnrolmentSchemeEnrolmentPrint_Click: Unexpected value (strStatus={0})", strStatus))

            End Select

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("Exception", ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00030, "Print Letter Click failed")
            Throw

        End Try

    End Sub

    Protected Sub ibtnDCompleteReplaceTokenPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtSP As ServiceProviderModel = GetServiceProvider()

        udcPrintFunction.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ERN", Session(SESS_ERN))
        udtAuditLogEntry.AddDescripton("SPID", Session(SESS_SPID))
        udtAuditLogEntry.WriteStartLog(LogID.LOG00028, "Print Letter Click")

        Try
            udcPrintFunction.PrepareThePanel(String.Empty, spPrintFunction.ActionPrintTokenReplacementLetter, udtSP, True, FunctionCode)
            udcPrintFunction.PrintLetterWithoutPopup()

            udtAuditLogEntry.WriteEndLog(LogID.LOG00029, "Print Letter Click successful")

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00030, "Print Letter Click failed")
            Throw

        End Try

    End Sub

    '

#Region "eHS and PCD integration (CRE12-001)"

    Protected Sub ibtnJoinPCD_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00110, "Join PCD button click")

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ResetMessageAndError()

        Dim udtSPBLL As New ServiceProviderBLL
        Dim udtSP As ServiceProviderModel

        If Not IsNothing(Session("TempSPmodel")) Then
            udtSP = Session("TempSPmodel")
        Else
            udtSP = udtSPBLL.GetServiceProviderBySPID(New Database, Session(SESS_SPID))
        End If

        ' Check Compulsory to join PCD for enrolling scheme
        If SPProfileBLL.IsJoinPCDCompulsory(udtSP.SchemeInfoList) Then
            Dim udtPCDAccountStatusResult As PCDCheckAccountStatusResult = Session(SESS_PCDCheckAccountStatusResult)

            If Not udtPCDAccountStatusResult Is Nothing Then
                Select Case udtPCDAccountStatusResult.AccountStatusCode
                    Case PCDAccountStatus.Delisted
                        ' Show PCD Warning Popup when SP Delisted in PCD
                        Session(SESS_JoinPCDClick) = YesNo.Yes
                        ShowPCDDelistedWarning()

                    Case Else
                        ' Show Join PCD Popup
                        ProcessJoinPCD()
                End Select
            End If
        Else
            ' Show Join PCD Popup
            ProcessJoinPCD()
        End If
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
    End Sub

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ''' <summary>
    ''' Check whether if any PCD warning exist, if yes, show popup, user need to confirm first to process next step
    ''' </summary>
    ''' <param name="udtSP"></param>
    ''' <returns>blnInvalid: True(Invalid), False(Valid) </returns>
    ''' <remarks></remarks>
    Private Function CheckPCDWarning(ByVal udtSP As ServiceProviderModel) As Boolean
        Dim blnJoinPCDCompulsory As Boolean = False
        Dim blnInvalid As Boolean = False
        Dim blnPCDProfessionalChecked As Boolean = False
        Dim blnPCDStatusChecked As Boolean = False

        ' PCD Warning Popup Confirm Click
        If Session(SESS_PCDProfessionalChecked) = YesNo.Yes Then
            blnPCDProfessionalChecked = True
        End If

        ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'If Session(SESS_PCDStatusChecked) = YesNo.Yes OrElse Session(SESS_PCDDelistedConfirmed) = YesNo.Yes Then
        If Session(SESS_PCDStatusChecked) = YesNo.Yes Then
            ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]
            blnPCDStatusChecked = True
        End If

        Session(SESS_JoinPCDClick) = YesNo.No

        ' get PCD Account Status from session
        Dim udtPCDAccountStatusResult As PCDCheckAccountStatusResult = Session(SESS_PCDCheckAccountStatusResult)

        If udtPCDAccountStatusResult Is Nothing Then
            ' Bypass PCD checking for non PCD profession
            Session(SESS_PCDProfessionalChecked) = YesNo.Yes
            Session(SESS_PCDStatusChecked) = YesNo.Yes
            blnInvalid = False

            Return blnInvalid
        End If

        blnJoinPCDCompulsory = SPProfileBLL.IsJoinPCDCompulsory(udtSP.SchemeInfoList)

        ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim blnSuspend As Boolean = False
        If Session(SESS_PCDSuspended) = YesNo.Yes Then
            blnSuspend = True
        End If
        ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]

        ' Check PCD Professional, show popup if contain any warning message
        If blnJoinPCDCompulsory Then

            If Not blnInvalid AndAlso Not blnPCDProfessionalChecked Then

                ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If Not blnSuspend Then
                    Dim strMessage As String = udtPCDAccountStatusResult.CheckPCDProfessional()
                    If strMessage <> String.Empty Then
                        blnInvalid = True
                        ShowPCDProfessionalWarning(strMessage)
                    End If
                End If
                ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]

                If Not blnInvalid Then
                    Session(SESS_PCDProfessionalChecked) = YesNo.Yes
                End If

            End If
        Else
            Session(SESS_PCDProfessionalChecked) = YesNo.Yes
        End If

        ' Check PCD Status
        If blnJoinPCDCompulsory Then

            If Not blnInvalid AndAlso Not blnPCDStatusChecked Then

                If udtPCDAccountStatusResult.AccountStatusCode = PCDAccountStatus.Delisted Then

                    ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If blnSuspend Then
                        ' Show warning for SP is suspended in PCD
                        ShowPCDSuspendedWarning()
                        blnInvalid = True
                    Else
                        ' Not to show delisted msg again which already checked and confirmed when Join PCD 
                        If Session(SESS_PCDDelistedConfirmed) <> YesNo.Yes Then
                            ' Show warning for SP is delisted in PCD and need to re-join PCD                      
                            ShowPCDDelistedWarning()
                            blnInvalid = True
                        End If
                    End If
                End If
                ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]

                If Not blnInvalid Then
                    Session(SESS_PCDStatusChecked) = YesNo.Yes
                End If
            End If
        Else
            Session(SESS_PCDStatusChecked) = YesNo.Yes
        End If

        Return blnInvalid
    End Function

    Private Sub ucPCDWarningPopup_Success_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ucPCDWarningPopup.Success_Click
        Select Case hfDPCDWarningPopupType.Value
            Case ucPCDWarningPopup.WarningType.Professional
                Session(SESS_PCDProfessionalChecked) = YesNo.Yes

            Case ucPCDWarningPopup.WarningType.Delisted
                Session(SESS_PCDStatusChecked) = YesNo.Yes
                If Session(SESS_JoinPCDClick) = YesNo.Yes Then
                    Session(SESS_PCDDelistedConfirmed) = YesNo.Yes
                End If

                ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
            Case ucPCDWarningPopup.WarningType.Suspended
                Session(SESS_PCDStatusChecked) = YesNo.Yes
                ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]

        End Select

        ' Confirm Click, continue process
        If Session(SESS_JoinPCDClick) = YesNo.Yes Then
            ' From "Join PCD" button
            ProcessJoinPCD()

        ElseIf Session(SESS_JoinPCDClick) = YesNo.No Then
            ' From "Save" / "Confirm Scheme Enrolment" button
            Select Case hfDCurrentAction.Value
                Case DetailPageCurrentAction.NewEnrolment
                    ProcessNewEnrolment()

                Case DetailPageCurrentAction.SchemeEnrolment
                    ProcessSchemeEnrolment()
            End Select
        End If

    End Sub

    Private Sub ucPCDWarningPopup_Failure_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ucPCDWarningPopup.Failure_Click
        Me.ModelPopupExtenderPCDWarning.Show()
    End Sub

    Private Sub ShowPCDDelistedWarning()
        hfDPCDWarningPopupType.Value = ucPCDWarningPopup.WarningType.Delisted

        Me.ucPCDWarningPopup.Build(ucPCDWarningPopup.WarningType.Delisted)
        Me.ModelPopupExtenderPCDWarning.PopupDragHandleControlID = Me.ucPCDWarningPopup.Header.ClientID
        Me.ModelPopupExtenderPCDWarning.Show()
    End Sub

    ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub ShowPCDSuspendedWarning()
        hfDPCDWarningPopupType.Value = ucPCDWarningPopup.WarningType.Suspended

        Me.ucPCDWarningPopup.Build(ucPCDWarningPopup.WarningType.Suspended)
        Me.ModelPopupExtenderPCDWarning.PopupDragHandleControlID = Me.ucPCDWarningPopup.Header.ClientID
        Me.ModelPopupExtenderPCDWarning.Show()
    End Sub
    ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]

    Private Sub ShowPCDProfessionalWarning(ByVal strWarningMessage As String)
        hfDPCDWarningPopupType.Value = ucPCDWarningPopup.WarningType.Professional

        Me.ucPCDWarningPopup.MessageText = strWarningMessage
        Me.ucPCDWarningPopup.Build(ucPCDWarningPopup.WarningType.Professional)
        Me.ModelPopupExtenderPCDWarning.PopupDragHandleControlID = Me.ucPCDWarningPopup.Header.ClientID
        Me.ModelPopupExtenderPCDWarning.Show()
    End Sub

    Private Sub ProcessNewEnrolment()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim blnResInValid As Boolean = False

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtSPBLL As New ServiceProviderBLL
        Dim udtSP As ServiceProviderModel = udtSPBLL.GetServiceProviderStagingProfileByERN(Session(SESS_ERN), New Database)

        ' Check if any PCD warning exists
        blnResInValid = Me.CheckPCDWarning(udtSP)

        If blnResInValid Then
            Return
        End If

        'udtAuditLogEntry.WriteStartLog(LogID.LOG00034, "Save Click")
        udtAuditLogEntry.WriteStartLog(LogID.LOG00118, "Process New Enrolment")
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

        Dim udtStaticDataBLL As New StaticDataBLL

        ' Message: Please confirm the following information.
        udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00021)
        udcInfoMessageBox.Type = InfoMessageBoxType.Information
        udcInfoMessageBox.BuildMessageBox()

        lblDTokenIssuedBy.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKEN_ISSUE_BY", rblDTokenIssuedBy.SelectedValue).DataValue.ToString.Trim
        lblDTokenSerialNo.Text = TokenModel.DisplayTokenSerialNo(txtDTokenSerialNo.Text.Trim, rblDTokenIssuedBy.SelectedValue, False, False, True)
        lblDEHRSSReplacementToken.Text = AntiXssEncoder.HtmlEncode(txtDEHRSSReplacementToken.Text, True)
        lblDIsShareToken.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("YesNo", rblDIsShareToken.SelectedValue).DataValue.ToString.Trim
        Status.GetDescriptionFromDBCode(TokenStatus.ClassCode, TokenStatus.Active, lblDTokenStatus.Text, String.Empty)

        ' Vision
        tblDTokenIssuedBy.Visible = False
        lblDTokenIssuedBy.Visible = True
        txtDTokenSerialNo.Visible = False
        lblDTokenSerialNo.Visible = True
        txtDEHRSSReplacementToken.Visible = False
        lblDEHRSSReplacementToken.Visible = True
        ibtnDGetFromEHRSS.Visible = False
        ibtnDClear.Visible = False
        tblDIsShareToken.Visible = False
        lblDIsShareToken.Visible = True

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ibtnDJoinPCD.Visible = False
        ibtnDJoinPCDDisable.Visible = False
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

        mvDButton.SetActiveView(vConfirmBack)

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'udtAuditLogEntry.WriteEndLog(LogID.LOG00040, "Save Click success")
        udtAuditLogEntry.WriteEndLog(LogID.LOG00119, "Process New Enrolment success")
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
    End Sub

    Private Sub ProcessSchemeEnrolment()
        Dim udtSPBLL As New ServiceProviderBLL
        Dim udtDB As New Database
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim blnInvalid As Boolean = False

        Try
            Dim udtSP As ServiceProviderModel = udtSPBLL.GetServiceProviderStagingProfileByERN(Session(SESS_ERN), udtDB)

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Check if any PCD warning exists
            blnInvalid = Me.CheckPCDWarning(udtSP)

            If blnInvalid Then
                Return
            End If
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

            ' Write Audit Log
            udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
            udtAuditLogEntry.AddDescripton("New Enrolled Scheme", lblDNewEnrolledScheme.Text.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00025, "Scheme Enrolment")

            udtSP.UpdateBy = udtHCVUUser.UserID
            udtSP.CreateBy = udtHCVUUser.UserID

            ' Complete the Scheme Enrolment

            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'Call (New TokenManagementBLL).CompleteSchemeEnrolment(udtSP, udtDB)
            Dim dtErrorMessage As DataTable = New DataTable("ErrorMessage")

            If Me.CheckValidationClickAccept(udtSP, dtErrorMessage) Then

                Call (New TokenManagementBLL).CompleteSchemeEnrolment(udtSP.EnrolRefNo, udtHCVUUser.UserID)
                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                lblDSPID.Text = udtSP.SPID

                ' If the SP is currently suspended, show a popup message
                Dim udtSPPermanent As ServiceProviderModel = udtSPBLL.GetServiceProviderBySPID(udtDB, udtSP.SPID)

                Status.GetDescriptionFromDBCode(ServiceProviderTokenStatus.ClassCode, udtSPPermanent.RecordStatus, lblDSPStatus.Text, String.Empty)

                If udtSPPermanent.RecordStatus = ServiceProviderStatus.Suspended Then
                    ucNewEnrolSuspendPopup.NoticeMode = ucNoticePopup.enumNoticeMode.Notification
                    ucNewEnrolSuspendPopup.ButtonMode = ucNoticePopup.enumButtonMode.OK
                    ucNewEnrolSuspendPopup.HeaderText = Me.GetGlobalResourceObject("Text", "Notification")

                    ' Message: New enrolled scheme(s) will be suspended as service provider is suspended. Please re-activate the scheme(s) & service provider if necessary.
                    ucNewEnrolSuspendPopup.MessageText = (New SystemMessage(FunctCode.FUNT010202, SeverityCode.SEVI, MsgCode.MSG00012)).GetMessage
                    ModalPopupNewEnrolSuspend.PopupDragHandleControlID = ucNewEnrolSuspendPopup.Header.ClientID
                    ModalPopupNewEnrolSuspend.Show()

                End If

                ' Message: Scheme Enrolment is completed.
                udcInfoMessageBox.Type = InfoMessageBoxType.Complete
                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00011)
                udcInfoMessageBox.BuildMessageBox()

                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ibtnDJoinPCD.Visible = False
                ibtnDJoinPCDDisable.Visible = False
                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

                mvDButton.SetActiveView(vCompleteSchemeEnrolment)

                udtAuditLogEntry.WriteEndLog(LogID.LOG00026, "Scheme Enrolment successful")

                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
            Else
                For Each drErrorMessage As DataRow In dtErrorMessage.Select()
                    If drErrorMessage.Item("IsReplace") Then
                        udcMessageBox.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"), drErrorMessage.Item("FindString"), drErrorMessage.Item("ReplaceString"))
                    Else
                        udcMessageBox.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"))
                    End If
                Next

                udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00027, "Scheme Enrolment failed")
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
            End If
            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]


        Catch eSQL As SqlException
            If eSQL.Number = 50000 Then
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message)
                udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00027, "Scheme Enrolment failed")

            Else
                Throw
            End If

        Catch ex As Exception
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
            udcMessageBox.BuildMessageBox(_strActionFailTitle, udtAuditLogEntry, LogID.LOG00027, "Scheme Enrolment failed: " + ex.Message)

        End Try
    End Sub
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
    ' --------------------------------------------------------------------------------------------------------------------------------
    Private Sub ProcessJoinPCD()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim udtSP As ServiceProviderModel
        Dim udtSPBLL As New ServiceProviderBLL

        If Not IsNothing(Session("TempSPmodel")) Then
            udtSP = Session("TempSPmodel")
        Else
            udtSP = udtSPBLL.GetServiceProviderBySPID(New Database, Session(SESS_SPID))
        End If

        ' Check SP exists in PCD (Save the variable to ServiceProviderBLL)
        'Dim udtSP As ServiceProviderModel = udtSPBLL.GetServiceProviderBySPID(New Database, Session(SESS_SPID))
        udtSPBLL.SaveToSession(udtSP)
        ucTypeOfPracticePopup.InvokePCD_CheckExist()

        Select Case Me.ucTypeOfPracticePopup.ExistPCDResult.ReturnCode
            Case PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.Available
                ' Display Type of Practice Popup
                udtAuditLogEntry.WriteLog(LogID.LOG00111, "Show Join PCD Popup")

                ucTypeOfPracticePopup.Reset()
                ucTypeOfPracticePopup.Showing = True
                ucTypeOfPracticePopup.Mode = ucTypeOfPracticeGrid.EnumMode.Transfer

                ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                'ucTypeOfPracticePopup.LoadPractice()
                Dim udtSPPermanent As ServiceProviderModel = Nothing

                If udtSP.SPID <> String.Empty Then
                    udtSPPermanent = (New ServiceProviderBLL).GetServiceProviderBySPID(New Database, udtSP.SPID)
                End If

                Dim udtPracticeList As PracticeModelCollection = udtSP.PracticeList.FilterByPCD(TableLocation.Staging, udtSPPermanent)

                ucTypeOfPracticePopup.LoadPractice(udtPracticeList)
                ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

                ModalPopupExtenderTypeOfPractice.PopupDragHandleControlID = ucTypeOfPracticePopup.Header.ClientID
                ModalPopupExtenderTypeOfPractice.Show()

            Case PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.ServiceProviderAlreadyExisted, _
                    PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.EnrolmentAlreadyExisted, _
                    PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.VerifiedEnrolmentAlreadyExisted
                ucTypeOfPracticePopup.Showing = False
                ModalPopupExtenderTypeOfPractice.Hide()

                ucNoticePopup.NoticeMode = HCVU.ucNoticePopUp.enumNoticeMode.Notification
                ucNoticePopup.MessageText = ucTypeOfPracticePopup.ExistPCDResult.SystemMessage.GetMessage
                ModalPopupExtenderNotice.PopupDragHandleControlID = ucNoticePopup.Header.ClientID
                ModalPopupExtenderNotice.Show()

            Case PCDCheckIsActiveSPResult.enumReturnCode.InvalidParameter, _
                    PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.DataValidationFail, _
                    PCDCheckIsActiveSPResult.enumReturnCode.ErrorAllUnexpected, _
                    PCDCheckIsActiveSPResult.enumReturnCode.CommunicationLinkError, _
                    PCDCheckIsActiveSPResult.enumReturnCode.AuthenticationFailed
                ucTypeOfPracticePopup.Showing = False
                ModalPopupExtenderTypeOfPractice.Hide()

                ucNoticePopup.NoticeMode = HCVU.ucNoticePopUp.enumNoticeMode.Custom
                ucNoticePopup.ButtonMode = HCVU.ucNoticePopUp.enumButtonMode.OK
                ucNoticePopup.IconMode = HCVU.ucNoticePopUp.enumIconMode.ExclamationIcon
                ucNoticePopup.HeaderText = Me.GetGlobalResourceObject("Text", "Notification")
                ucNoticePopup.MessageText = ucTypeOfPracticePopup.ExistPCDResult.SystemMessage.GetMessage
                ModalPopupExtenderNotice.PopupDragHandleControlID = ucNoticePopup.Header.ClientID
                ModalPopupExtenderNotice.Show()

            Case Else
                Throw New Exception(String.Format("[spTokenManagement] PCDCheckIsActiveSP unhandled return code ({0})", Me.ucTypeOfPracticePopup.ExistPCDResult.ReturnCode.ToString()))

        End Select

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        UpdatePCDStatus(udtSP)
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
    End Sub
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

    Protected Sub ucTypeOfPracticePopup_ButtonClick(ByVal e As ucTypeOfPracticePopup.enumButtonClick) Handles ucTypeOfPracticePopup.ButtonClick
        Select Case e
            Case ucTypeOfPracticePopup.enumButtonClick.Cancel
                ucTypeOfPracticePopup.Showing = False
                ModalPopupExtenderTypeOfPractice.Hide()

            Case ucTypeOfPracticePopup.enumButtonClick.CreatePCDAccount
                ' Show Notice Popup For JoinPCD
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00112, "Show Notice Popup")

                Me.ucTypeOfPracticePopup.Showing = False
                Me.ModalPopupExtenderTypeOfPractice.Hide()

                Select Case ucTypeOfPracticePopup.JoinPCDResult.ReturnCode
                    Case PCDUploadVerifiedEnrolmentResult.enumReturnCode.UploadedSuccessfully, _
                            PCDUploadVerifiedEnrolmentResult.enumReturnCode.ServiceProviderAlreadyExisted, _
                            PCDUploadVerifiedEnrolmentResult.enumReturnCode.EnrolmentAlreadyExisted
                        ucNoticePopup.NoticeMode = ucNoticePopup.enumNoticeMode.Notification

                    Case Else
                        ucNoticePopup.NoticeMode = ucNoticePopup.enumNoticeMode.Custom
                        ucNoticePopup.ButtonMode = ucNoticePopup.enumButtonMode.OK
                        ucNoticePopup.IconMode = ucNoticePopup.enumIconMode.ExclamationIcon
                        ucNoticePopup.HeaderText = Me.GetGlobalResourceObject("Text", "Notification")

                End Select

                ucNoticePopup.MessageText = ucTypeOfPracticePopup.JoinPCDResult.ReturnCodeDesc
                ModalPopupExtenderNotice.PopupDragHandleControlID = ucNoticePopup.Header.ClientID
                ModalPopupExtenderNotice.Show()

                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Update PCD Status
                Dim udtSP As ServiceProviderModel = Session("TempSPmodel")
                UpdatePCDStatus(udtSP)
                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

            Case ucTypeOfPracticePopup.enumButtonClick.ERN
                ' Show Enrolment Copy Popup
                Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
                udtAuditLogEntry.WriteLog(LogID.LOG00113, "Show Enrolment Copy Popup")

                ucTypeOfPracticePopup.Showing = True
                ModalPopupExtenderTypeOfPractice.Show()
                ModalPopupExtenderEnrolmentCopy.PopupDragHandleControlID = ucEnrolmentCopyPopup.Header.ClientID
                ModalPopupExtenderEnrolmentCopy.Show()
                ucEnrolmentCopyPopup.LoadRecord()

            Case Else
                Throw New Exception(String.Format("[spTokenManagement] Unhandled ucTypeOfPracticePopup button click ({0})", e.ToString()))

        End Select

    End Sub

    Protected Sub ucEnrolmentCopyPopup_ButtonClick(ByVal e As ucEnrolmentCopyPopup.enumButtonClick) Handles ucEnrolmentCopyPopup.ButtonClick
        Select Case e
            Case ucEnrolmentCopyPopup.enumButtonClick.Close
                ucTypeOfPracticePopup.Showing = True
                ModalPopupExtenderTypeOfPractice.Show()

            Case Else
                Throw New Exception(String.Format("[spTokenManagement] Unhandled ucEnrolmentCopyPopup button click ({0})", e.ToString()))

        End Select

    End Sub

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub UpdatePCDStatus(ByVal udtSP As ServiceProviderModel)
        Dim blnShowPCDStatus As Boolean = False
        Dim strPCDStatus As String = String.Empty

        ' Show PCD Status if the SP is able to join PCD

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtSPPerm As ServiceProviderModel = Nothing

        If udtSP.SPID <> String.Empty Then
            udtSPPerm = (New ServiceProviderBLL).GetServiceProviderBySPID(New Database, udtSP.SPID)
        End If

        If udtSP.PracticeList.FilterByPCD(TableLocation.Staging, udtSPPerm).Count > 0 Then
            'If udtSP.PracticeList.FilterByPCD.Count > 0 Then
            ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
            blnShowPCDStatus = True
        End If

        If blnShowPCDStatus Then
            trDPCDStatus.Visible = True
            trDPCDProfessional.Visible = True

            If udtSP.SPID.Equals(String.Empty) Then
                Select Case udtSP.JoinPCD
                    Case JoinPCDStatus.Yes
                        If udtSP.SPID = String.Empty Then
                            trDUserDeclareJoinPCD.Visible = True
                            lblDUserDeclareJoinPCD.Text = Me.GetGlobalResourceObject("Text", "JoinPCDDeclare_Y")
                        End If
                    Case JoinPCDStatus.Enrolled
                        If udtSP.SPID = String.Empty Then
                            trDUserDeclareJoinPCD.Visible = True
                            lblDUserDeclareJoinPCD.Text = Me.GetGlobalResourceObject("Text", "JoinPCDDeclare_E")
                        End If
                    Case JoinPCDStatus.No
                        If udtSP.SPID = String.Empty Then
                            trDUserDeclareJoinPCD.Visible = True
                            lblDUserDeclareJoinPCD.Text = Me.GetGlobalResourceObject("Text", "JoinPCDDeclare_N")
                        End If
                    Case JoinPCDStatus.NA
                        trDPCDStatus.Visible = False
                        trDPCDProfessional.Visible = False
                        trDUserDeclareJoinPCD.Visible = False

                End Select

            End If

            CheckPCDAccountStatus(udtSP)
        Else
            trDPCDStatus.Visible = False
            trDPCDProfessional.Visible = False
        End If

        SetupJoinPCDButton()

    End Sub

    Private Sub CheckPCDAccountStatus(ByVal udtSP As ServiceProviderModel)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        ' Check PCD Account Status
        Dim udtPCDWebService As PCDWebService = New PCDWebService(Me.FunctionCode)
        Dim udtPCDAccountStatusResult As PCDCheckAccountStatusResult = Nothing

        Try
            udtAuditLogEntry.AddDescripton("WebMethod", "PCDCheckAccountStatus")
            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00114, "CheckPCDAccountStatus Start")

            udtPCDAccountStatusResult = udtPCDWebService.PCDCheckAccountStatus(udtSP.HKID)

            udtAuditLogEntry.AddDescripton("ReturnCode", udtPCDAccountStatusResult.ReturnCode.ToString)
            udtAuditLogEntry.AddDescripton("MessageID", udtPCDAccountStatusResult.MessageID.ToString)

            Session(SESS_PCDCheckAccountStatusResult) = udtPCDAccountStatusResult

            ' Convert to status for display
            lblPCDStatus.Text = udtPCDAccountStatusResult.GetPCDStatusDesc()
            lblPCDProfessional.Text = udtPCDAccountStatusResult.GetPCDProfessionalDesc()

            ' Success
            If udtPCDAccountStatusResult.ReturnCode = WebService.Interface.PCDCheckAccountStatusResult.enumReturnCode.Success Then

                ' Update Service Provider's Join PCD Status
                If Not udtSP.SPID.Equals(String.Empty) Then
                    'Get VU User ID
                    Dim udtHCVUUser As HCVUUserModel
                    Dim udtHCVUUserBLL As New HCVUUserBLL
                    udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

                    Dim strMessage As String = String.Empty
                    Dim blnRes As Boolean = udtPCDAccountStatusResult.UpdateJoinPCDStatus(udtSP.SPID, udtHCVUUser.UserID, strMessage)
                    If Not blnRes Then
                        Throw New Exception(strMessage)
                    End If

                End If

                lblPCDStatus.ForeColor = Drawing.Color.Empty
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00115, "CheckPCDAccountStatus Success")
            Else
                lblPCDStatus.ForeColor = Drawing.Color.Red
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00116, "CheckPCDAccountStatus Fail")
            End If

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00116, "CheckPCDAccountStatus Fail")
            Throw
        End Try
    End Sub

    ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ''' <summary>
    ''' Check SP is Suspended in PCD
    ''' Suspended in PCD = PCD Account Status:Delisted + Check Available: SP Already Existed
    ''' </summary>
    ''' <param name="udtSP"></param>
    ''' <returns>The warning message</returns>
    ''' <remarks></remarks>    
    Private Function CheckPCDSuspended(ByVal udtSP As ServiceProviderModel) As Boolean
        Dim blnSuspended As Boolean = False

        Dim udtPCDAccountStatusResult As PCDCheckAccountStatusResult = Session(SESS_PCDCheckAccountStatusResult)

        Session(SESS_PCDSuspended) = Nothing

        If Not udtPCDAccountStatusResult Is Nothing Then
            ' === PCD Account Status ===
            Select Case udtPCDAccountStatusResult.AccountStatusCode
                Case PCDAccountStatus.Delisted

                    Dim udtSPBLL As New ServiceProviderBLL
                    udtSPBLL.SaveToSession(udtSP)
                    ucTypeOfPracticePopup.InvokePCD_CheckExist()

                    Select Case Me.ucTypeOfPracticePopup.ExistPCDResult.ReturnCode
                        Case PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.ServiceProviderAlreadyExisted
                            Session(SESS_PCDSuspended) = YesNo.Yes
                            blnSuspended = True
                        Case Else
                            'Nth to do

                    End Select
            End Select
        End If

        Return blnSuspended
    End Function
    ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]

    Private Sub SetupJoinPCDButton()
        ibtnDJoinPCD.Visible = False
        ibtnDJoinPCDDisable.Visible = False

        Dim udtPCDAccountStatusResult As PCDCheckAccountStatusResult = Session(SESS_PCDCheckAccountStatusResult)

        If Not udtPCDAccountStatusResult Is Nothing Then
            ' === PCD Account Status ===
            Select Case udtPCDAccountStatusResult.AccountStatusCode
                Case PCDAccountStatus.NotEnrolled, PCDAccountStatus.Delisted

                    ' === PCD Enrolment Status ===
                    Select Case udtPCDAccountStatusResult.EnrolmentStatusCode
                        Case PCDEnrolmentStatus.NA, PCDEnrolmentStatus.Unprocessed
                            ' Not Enrolled / Unprocessed in PCD
                            ' Enable <Join PCD>

                            ibtnDJoinPCD.Visible = True
                            ibtnDJoinPCDDisable.Visible = False

                        Case PCDEnrolmentStatus.Processing
                            ' Enrolment is processing in PCD
                            ' Disable <Join PCD>

                            ibtnDJoinPCD.Visible = False
                            ibtnDJoinPCDDisable.Visible = True
                    End Select
                Case PCDAccountStatus.Enrolled
                    ' Enrolled in PCD
                    ' Disable <Join PCD>

                    ibtnDJoinPCD.Visible = False
                    ibtnDJoinPCDDisable.Visible = True

                Case PCDAccountStatus.ConnectionFail
                    ' Unknown PCD status
                    ' Enable <Join PCD>

                    ibtnDJoinPCD.Visible = True
                    ibtnDJoinPCDDisable.Visible = False
            End Select

        Else
            ' Empty PCD status
            ' Disable <Join PCD>

            ibtnDJoinPCD.Visible = False
            ibtnDJoinPCDDisable.Visible = False
        End If
    End Sub

    Private Function ValidatePCDStatus() As Boolean
        Dim blnValidatedSuccess As Boolean = True

        Dim udtSPBLL As New ServiceProviderBLL
        Dim udtSP As ServiceProviderModel = udtSPBLL.GetServiceProviderStagingProfileByERN(Session(SESS_ERN), New Database)


        ' Check Compulsory to join PCD for enrolling scheme
        If SPProfileBLL.IsJoinPCDCompulsory(udtSP.SchemeInfoList) Then

            ' Check PCD Status
            Dim udtPCDAccountStatusResult As PCDCheckAccountStatusResult = Session(SESS_PCDCheckAccountStatusResult)

            If Not udtPCDAccountStatusResult Is Nothing Then


                ' ==== Check PCD Account Status ====
                Select Case udtPCDAccountStatusResult.AccountStatusCode

                    ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    Case PCDAccountStatus.NotEnrolled
                        ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]

                        ' ==== Check PCD Enrolment Status ====
                        Select Case udtPCDAccountStatusResult.EnrolmentStatusCode
                            Case PCDEnrolmentStatus.Unprocessed, PCDEnrolmentStatus.NA
                                ' No enrolment record / enrolment record is Unprocessed

                                ' Message: The service provider is not enrolled in PCD. Please click ¡§Join PCD¡¨ to join PCD first.
                                udcMessageBox.AddMessage(FunctCode.FUNT010202, SeverityCode.SEVE, MsgCode.MSG00018)
                                blnValidatedSuccess = False

                            Case PCDEnrolmentStatus.Processing
                                ' Processing in PCD
                                blnValidatedSuccess = True
                            Case Else
                                Throw New Exception(String.Format("Error: Unknown PCD Enrolment Status {0}", udtPCDAccountStatusResult.EnrolmentStatusCode))
                        End Select

                        ' CRE18-007 Override PCD Status to enroll VSS [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                    Case PCDAccountStatus.Delisted
                        Dim blnSuspended As Boolean = CheckPCDSuspended(udtSP)

                        ' ==== Check PCD Enrolment Status ====
                        Select Case udtPCDAccountStatusResult.EnrolmentStatusCode
                            Case PCDEnrolmentStatus.Unprocessed, PCDEnrolmentStatus.NA
                                ' No enrolment record / enrolment record is Unprocessed

                                ' Allow to proceed enrolment for SP 'Suspended' in PCD as the SP cannot rejoin PCD again
                                If blnSuspended Then
                                    blnValidatedSuccess = True
                                Else
                                    ' Message: The service provider is not enrolled in PCD. Please click ¡§Join PCD¡¨ to join PCD first.
                                    udcMessageBox.AddMessage(FunctCode.FUNT010202, SeverityCode.SEVE, MsgCode.MSG00018)
                                    blnValidatedSuccess = False
                                End If

                            Case PCDEnrolmentStatus.Processing
                                ' Processing in PCD
                                blnValidatedSuccess = True
                            Case Else
                                Throw New Exception(String.Format("Error: Unknown PCD Enrolment Status {0}", udtPCDAccountStatusResult.EnrolmentStatusCode))
                        End Select
                        ' CRE18-007 Override PCD Status to enroll VSS [End][Winnie]


                    Case PCDAccountStatus.Enrolled
                        ' Enrolled in PCD
                        blnValidatedSuccess = True

                    Case PCDAccountStatus.ConnectionFail
                        ' Message: PCD service is temporarily unavailable. Please try again later.

                        udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00318)
                        blnValidatedSuccess = False

                    Case Else
                        Throw New Exception(String.Format("Error: Unknown PCD Account Status {0}", udtPCDAccountStatusResult.AccountStatusCode))
                End Select

            End If

        Else
            ' Not compulsory to join PCD
            blnValidatedSuccess = True
        End If

        Return blnValidatedSuccess
    End Function
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
#End Region

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
        If IsNothing(Session("TempSPmodel")) Then
            Return Nothing
        Else
            Return CType(Session("TempSPmodel"), ServiceProviderModel)
        End If
    End Function

#End Region

#Region "Validation Checking for Enrolment"

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
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

#End Region


End Class
