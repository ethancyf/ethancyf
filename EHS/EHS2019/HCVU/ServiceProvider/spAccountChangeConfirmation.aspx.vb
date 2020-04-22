Imports System.Web.Security.AntiXss
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.ComObject.SystemMessage
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.SchemeInformation
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.UserAC
Imports Common.Component.VoucherScheme
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports HCVU.AccountChangeMaintenance

' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

Imports Common.Component.Token

' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

Partial Public Class sp_AccountChangeConfirmation
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    ' -------------------------------------------------------------------------
    Inherits BasePageWithControl
    'Inherits BasePageWithGridView

    Private Class AuditLogDescription
        Public Const SearchSuccessful As String = "Search successful"
        Public Const SearchFail As String = "Search failed"
        Public Const SearchCompleteNoRecordFound As String = "Search failed: No record found"
    End Class

    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]


#Region "Fields"

    Private udtAccountChangeMaintenanceBLL As New AccountChangeMaintenanceBLL
    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtPracticeBLL As New PracticeBLL
    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
    'Private udtSchemeBLL As New SchemeBLL
    Private udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
    Private udtServiceProviderBLL As New ServiceProviderBLL
    Private udtSPAccountUpdateBLL As New SPAccountUpdateBLL
    Private udtStaticDataBLL As New StaticDataBLL
    Private udtValidator As New Validator

    Private SM As SystemMessage
    Private udtAuditLogEntry As AuditLogEntry

#End Region

#Region "Constants"

    Private Const ViewIndexSearchCriteria As Integer = 0
    Private Const ViewIndexSearchResultLevelOne As Integer = 1
    Private Const ViewIndexSearchResultLevelTwo As Integer = 2
    Private Const ViewIndexComplete As Integer = 3

    Private Const HtmlLineBreak As String = "<br />"

#End Region

#Region "Session Constants"

    Private Const SESS_FromMain As String = "fromMain"
    Private Const SESS_AccountChangeConfirmationLevelOneRecord As String = "AccountChangeConfirmationLevelOneRecord"
    Private Const SESS_AccountChangeConfirmationLevelTwoRecord As String = "AccountChangeConfirmationLevelTwoRecord"
    Private Const SESS_AccountChangeConfirmationSearchCriteria As String = "SESS_AccountChangeConfirmationSearchCriteria"     ' CRE12-014 - Relax 500 rows limit in back office platform

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        ' Shortcut from home page
        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT010203
            ResetControls()

            ' Write Audit Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Service Provider Account Change Confirmation loaded")

            If Session(SESS_FromMain) = "Y" Then
                Session(SESS_FromMain) = Nothing
                ' Search Result
                ddlChangeAccount.SelectedValue = ""
                ibtnSearch_Click(Nothing, Nothing)
            End If
        End If

        ReRenderPage()
        ResetBox()
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case MultiViewAccountChangeConfirmation.ActiveViewIndex
            Case ViewIndexSearchCriteria
                ScriptManager1.SetFocus(ibtnSearch)
                pnlAccountChangeConfirmation.DefaultButton = ibtnSearch.ID
            Case ViewIndexSearchResultLevelOne
                ScriptManager1.SetFocus(btnHidden)
                pnlAccountChangeConfirmation.DefaultButton = btnHidden.ID
            Case ViewIndexSearchResultLevelTwo
                ScriptManager1.SetFocus(btnHidden)
                pnlAccountChangeConfirmation.DefaultButton = btnHidden.ID
            Case ViewIndexComplete
                ScriptManager1.SetFocus(btnHidden)
                pnlAccountChangeConfirmation.DefaultButton = btnHidden.ID
        End Select
    End Sub

    Private Sub ReRenderPage()
        If MultiViewAccountChangeConfirmation.ActiveViewIndex = ViewIndexSearchResultLevelTwo Then
            If gvRecordLevelTwo.Rows.Count > 0 Then
                Me.GridViewDataBind(gvRecordLevelTwo, Session(SESS_AccountChangeConfirmationLevelTwoRecord), CStr(ViewState("SortExpression")), CStr(ViewState("SortDirection")), False)
            End If
        End If
    End Sub

    Private Sub ResetBox()
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False
    End Sub

    Private Sub ResetControls()
        BindAction()
        GridViewUncheckAllCheckbox()
        AddPreventMultiClick()
    End Sub

    Private Sub BindAction()
        Dim i As Integer = 1

        For Each udtStaticDataModel As StaticDataModel In udtStaticDataBLL.GetStaticDataListByColumnName(StaticDataColumnName.SPAction)
            ddlChangeAccount.Items.Insert(i, New ListItem(udtStaticDataModel.DataValue, udtStaticDataModel.ItemNo))
            i = i + 1
        Next
    End Sub

    Private Sub GridViewUncheckAllCheckbox()
        If Not IsNothing(gvRecordLevelOne.HeaderRow) Then
            CType(gvRecordLevelOne.HeaderRow.Cells.Item(1).FindControl("chkselectall"), CheckBox).Checked = False
        End If

        For Each r As GridViewRow In gvRecordLevelOne.Rows
            CType(r.FindControl("chkselect"), CheckBox).Checked = False
        Next
    End Sub

    Private Sub AddPreventMultiClick()
        MyBase.preventMultiImgClick(Me.ClientScript, ibtnSearch)
        MyBase.preventMultiImgClick(Me.ClientScript, ibtnLevelOneConfirm)
        MyBase.preventMultiImgClick(Me.ClientScript, ibtnLevelOneReject)
        MyBase.preventMultiImgClick(Me.ClientScript, ibtnLevelOneBack)
        MyBase.preventMultiImgClick(Me.ClientScript, ibtnLevelTwoConfirm)
        MyBase.preventMultiImgClick(Me.ClientScript, ibtnLevelTwoReject)
        MyBase.preventMultiImgClick(Me.ClientScript, ibtnLevelTwoBack)
        MyBase.preventMultiImgClick(Me.ClientScript, ibtnReturn)
    End Sub

#End Region

    Private Sub gv_RowDataBound(ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim r As DataRow = CType(e.Row.DataItem, DataRowView).Row

            ' Service Provider Name
            Dim lblSP_Chi_Name As Label = CType(e.Row.FindControl("lblSP_Chi_Name"), Label)
            lblSP_Chi_Name.Text = udtFormatter.formatChineseName(lblSP_Chi_Name.Text.Trim)

            ' Action
            r("ActionDescription") = CStr(udtStaticDataBLL.GetStaticDataByColumnNameItemNo(StaticDataColumnName.SPAction, CStr(r("Upd_Type"))).DataValue)

            If CStr(r("Upd_Type")) = SPAccountMaintenanceUpdTypeStatus.SPDelist _
                    OrElse CStr(r("Upd_Type")) = SPAccountMaintenanceUpdTypeStatus.PracticeDelist Then
                Dim strDelistStatus As String = String.Empty
                Status.GetDescriptionFromDBCode(DelistStatus.ClassCode, CStr(r("Delist_Status")), strDelistStatus, "")
                r("ActionDescription") += " (" + strDelistStatus + ")"
            End If

            Dim lblAction As Label = CType(e.Row.FindControl("lblAction"), Label)
            lblAction.Text = CStr(r("ActionDescription"))

            ' Information
            r("Information") = String.Empty
            If Not CInt(r("SP_Practice_Display_Seq")) = 0 Then
                r("Information") += Me.GetGlobalResourceObject("Text", "PracticeName") + ": " + udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(CStr(r("SP_ID")), New Database)(CInt(r("SP_Practice_Display_Seq"))).PracticeName + HtmlLineBreak
            End If

            If Not CStr(r("Scheme_Code")).Trim = String.Empty Then
                r("Information") += Me.GetGlobalResourceObject("Text", "SchemeName") + ": " + udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(CStr(r("Scheme_Code")).Trim).DisplayCode.Trim
            End If

            Dim lblInformation As Label = CType(e.Row.FindControl("lblInformation"), Label)

            ' Remark
            If CStr(r("Remark")) = String.Empty Then
                r("Remark") = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim imgIsShareToken As Image = CType(e.Row.FindControl("imgIsShareToken"), Image)

            imgIsShareToken.ToolTip = String.Empty
            imgIsShareToken.Visible = False
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

            If Not CStr(r("Token_Serial_No")).Trim = String.Empty Then

                ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'r("Information") = Me.GetGlobalResourceObject("Text", "TokenSerialNo") + ": " + TokenModel.DisplayTokenSerialNo(CStr(r("Token_Serial_No")).Trim, CStr(r("Project")).Trim)

                If CStr(r("Project")).Trim = TokenProjectType.EHCVS Then
                    r("Information") = Me.GetGlobalResourceObject("Text", "TokenSerialNo") + ": " + TokenModel.DisplayTokenSerialNo(CStr(r("Token_Serial_No")).Trim, CStr(r("Project")).Trim, False, False, True)
                Else
                    r("Information") = Me.GetGlobalResourceObject("Text", "TokenSerialNo") + ": " + TokenModel.DisplayTokenSerialNo(CStr(r("Token_Serial_No")).Trim, CStr(r("Project")).Trim, False, True, True)
                End If

                If r("Is_Share_Token") = YesNo.Yes Then
                    imgIsShareToken.ToolTip = HttpContext.GetGlobalResourceObject("AlternateText", "ShareToken").ToString
                    imgIsShareToken.Style.Add("vertical-align", "middle")
                    imgIsShareToken.Visible = True
                Else
                    imgIsShareToken.Visible = False
                End If

                Dim lblInformationTokenReplacement As Label = CType(e.Row.FindControl("lblInformationTokenReplacement"), Label)
                Dim imgIsShareTokenReplacement As Image = CType(e.Row.FindControl("imgIsShareTokenReplacement"), Image)

                If Not CStr(r("Token_Serial_No_Replacement")).Trim = String.Empty Then

                    If CStr(r("Project")).Trim = TokenProjectType.EHCVS Then
                        lblInformationTokenReplacement.Text = Me.GetGlobalResourceObject("Text", "ReplacementTokenSerialNo") + ": " + TokenModel.DisplayTokenSerialNo(CStr(r("Token_Serial_No_Replacement")).Trim, CStr(r("Project")).Trim, False, False, True)
                    Else
                        lblInformationTokenReplacement.Text = Me.GetGlobalResourceObject("Text", "ReplacementTokenSerialNo") + ": " + TokenModel.DisplayTokenSerialNo(CStr(r("Token_Serial_No_Replacement")).Trim, CStr(r("Project")).Trim, False, True, True)
                    End If

                    lblInformationTokenReplacement.Visible = True

                    If r("Is_Share_Token_Replacement") = YesNo.Yes Then
                        imgIsShareTokenReplacement.ToolTip = HttpContext.GetGlobalResourceObject("AlternateText", "ShareToken").ToString
                        imgIsShareTokenReplacement.Style.Add("vertical-align", "middle")
                        imgIsShareTokenReplacement.Visible = True
                    End If

                Else
                    lblInformationTokenReplacement.Visible = False
                    imgIsShareTokenReplacement.Visible = False
                End If

                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

                ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

                If Not CStr(r("Token_Remark")).Trim = String.Empty Then
                    r("Remark") = udtStaticDataBLL.GetStaticDataByColumnNameItemNo(StaticDataColumnName.TokenDisable, CStr(r("Token_Remark"))).DataValue
                End If
            End If

            Dim lblRemark As Label = CType(e.Row.FindControl("lblRemark"), Label)
            lblRemark.Text = CStr(r("Remark"))

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

            lblInformation.Text = CStr(r("Information"))

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

        End If
    End Sub

    ' Search
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    ' -------------------------------------------------------------------------

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult
        If blnOverrideResultLimit Then
            Return GetAccount(Session(SESS_AccountChangeConfirmationSearchCriteria), True)
        Else
            Return GetAccount()
        End If
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As Common.Component.BaseBLL.BLLSearchResult) As Integer

        Dim dtLevelOne As DataTable
        Dim intRowCount As Integer

        Try
            dtLevelOne = CType(udtBLLSearchResult.Data, DataTable)

        Catch ex As Exception
            Throw

        End Try

        intRowCount = dtLevelOne.Rows.Count

        If intRowCount > 0 Then
            MultiViewAccountChangeConfirmation.ActiveViewIndex = ViewIndexSearchResultLevelOne
            dtLevelOne.Columns.Add("Information")
            dtLevelOne.Columns.Add("ActionDescription")
            Session(SESS_AccountChangeConfirmationLevelOneRecord) = dtLevelOne

            Me.GridViewDataBind(gvRecordLevelOne, dtLevelOne, "SP_ID", "ASC", False)
        Else

        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_ConfirmSearch_Click()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim enumSearchResult As SearchResultEnum

        'write start log
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Action", ddlChangeAccount.SelectedValue)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search")

        Try
            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMessageBox, udcInfoMessageBox, False, True)

        Catch eSQL As SqlClient.SqlException

            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException")
            udtAuditLogEntry.AddDescripton("Message", eSQL.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)
            Throw

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLogEntry.AddDescripton("Message", ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.SearchSuccessful)

            Case Else
                Throw New Exception("Error: Class = [HCVU.sp_AccountChangeConfirmation], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

        End Select
    End Sub
    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub
    Protected Overrides Sub SF_CancelSearch_Click()

    End Sub

    Private Function GetAccount(Optional ByVal UpdateType As String = Nothing, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
        If IsNothing(UpdateType) Then
            Session(SESS_AccountChangeConfirmationSearchCriteria) = ddlChangeAccount.SelectedValue
        End If

        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

        udtBLLSearchResult = udtAccountChangeMaintenanceBLL.GetRecordbyUpdType(FunctionCode, blnOverrideResultLimit, Session(SESS_AccountChangeConfirmationSearchCriteria))

        Return udtBLLSearchResult

    End Function


    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]
    ' -------------------------------------------------------------------------
    Protected Sub ibtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Visible = False

        Try
            'Write Start Log
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Action", ddlChangeAccount.SelectedValue)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search")


            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Karl L]
            ' -------------------------------------------------------------------------
            Dim enumSearchResult As SearchResultEnum

            Try
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMessageBox, udcInfoMessageBox, False)

            Catch eSQL As SqlClient.SqlException

                udtAuditLogEntry.AddDescripton("StackTrace", "Unknown SqlException")
                udtAuditLogEntry.AddDescripton("Message", eSQL.Message)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)
                Throw

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception")
                udtAuditLogEntry.AddDescripton("Message", ex.Message)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchFail)
                Throw

            End Try

            Select Case enumSearchResult
                Case SearchResultEnum.Success

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.SearchSuccessful)

                Case SearchResultEnum.ValidationFail
                    'Audit Log has been handled in [SF_ValidateSearch] method

                Case SearchResultEnum.NoRecordFound
                    MultiViewAccountChangeConfirmation.ActiveViewIndex = ViewIndexSearchCriteria
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, "I", MsgCode.MSG00001)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.SearchCompleteNoRecordFound)

                Case SearchResultEnum.OverResultList1stLimit_PopUp
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, SF_AuditLogDescription.SearchFail_Over1stLimit)

                Case SearchResultEnum.OverResultList1stLimit_Alert
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, SF_AuditLogDescription.SearchFail_Over1stLimit)

                Case SearchResultEnum.OverResultListOverrideLimit
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, SF_AuditLogDescription.SearchFail_OverOverrideLimit)

                Case Else
                    Throw New Exception("Error: Class = [HCVU.sp_AccountChangeConfirmation], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

            End Select

            ' Dim dtLevelOne As DataTable = udtAccountChangeMaintenanceBLL.GetRecordbyUpdType(ddlChangeAccount.SelectedValue)

            'If dtLevelOne.Rows.Count > 0 Then
            '    MultiViewAccountChangeConfirmation.ActiveViewIndex = ViewIndexSearchResultLevelOne

            '    dtLevelOne.Columns.Add("Information")
            '    dtLevelOne.Columns.Add("ActionDescription")
            '    Session(SESS_AccountChangeConfirmationLevelOneRecord) = dtLevelOne

            '    Me.GridViewDataBind(gvRecordLevelOne, dtLevelOne, "SP_ID", "ASC", False)

            '    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search successful")

            'Else
            '    MultiViewAccountChangeConfirmation.ActiveViewIndex = ViewIndexSearchCriteria

            '    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            '    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, "I", MsgCode.MSG00001)

            '    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search failed: No record found")
            'End If

            udcInfoMessageBox.BuildMessageBox()

            'Catch eSQL As SqlClient.SqlException
            '    If eSQL.Number = 50000 Then
            '        udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, "D", eSQL.Message))

            '        If udcMessageBox.GetCodeTable.Rows.Count = 0 Then
            '            udcMessageBox.Visible = False
            '        Else
            '            udcMessageBox.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00003, "Search failed")
            '        End If

            '    Else
            '        Throw eSQL
            '    End If

            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Karl L]


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    '

    Protected Sub gvRecordLevelOne_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        gv_RowDataBound(e)
    End Sub

    Protected Sub gvRecordLevelOne_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_AccountChangeConfirmationLevelOneRecord)
    End Sub

    Protected Sub gvRecordLevelOne_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, SESS_AccountChangeConfirmationLevelOneRecord)
    End Sub

    Protected Sub gvRecordLevelOne_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_AccountChangeConfirmationLevelOneRecord)
    End Sub

    ' Level One Confirm / Reject

    Protected Sub ibtnLevelOneConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' --- Validation ---

        ' Check no checkboxes selected
        Dim udtSystemMessage As SystemMessage = udtValidator.chkGridSelectedNothing(gvRecordLevelOne, "chkSelect", 1)

        If Not IsNothing(udtSystemMessage) Then
            udcMessageBox.AddMessage(udtSystemMessage)
            udcMessageBox.BuildMessageBox("ValidationFail")

            Return

        End If

        ' Activate token involving EHR cannot be perform with multiple records
        Dim blnInvolveEHR As Boolean = False
        Dim intSelectedRow As Integer = 0

        For Each gvr As GridViewRow In gvRecordLevelOne.Rows
            If DirectCast(gvr.FindControl("chkSelect"), CheckBox).Checked Then
                intSelectedRow += 1

                If DirectCast(gvr.FindControl("lblUpd_Type"), Label).Text.Trim = SPAccountMaintenanceUpdTypeStatus.TokenActivate _
                        AndAlso Not IsNothing(DirectCast(gvr.FindControl("hfProject"), HiddenField).Value) _
                        AndAlso DirectCast(gvr.FindControl("hfProject"), HiddenField).Value.Trim = TokenProjectType.EHR Then
                    blnInvolveEHR = True
                End If

            End If
        Next

        If intSelectedRow > 1 AndAlso blnInvolveEHR Then
            udcMessageBox.AddMessage(FunctionCode, "E", MsgCode.MSG00004)
            udcMessageBox.BuildMessageBox("ValidationFail")

            Return

        End If

        ' --- End of Validation ---

        ibtnLevelTwoConfirm.Visible = True
        ibtnLevelTwoReject.Visible = False
        lblConfirm.Text = Me.GetGlobalResourceObject("Text", "ConfirmChange")

        Dim strSPError As String = String.Empty
        Dim strSPErrorFiltered As String = String.Empty

        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Change: Remove the auto/manual rejection on amendment when delisting Scheme/Practice Scheme

        ' Block the contradiction case e.g Delist Action + Under Amendment
        Dim strMessageCode As String = OutstandingRecordMessageCode(strSPError)

        ' strMessageCode = [Empty] or [4] or [12]

        ' SP is delisted (due to concurrent update): MSG00004 = The Service Provider %s is delisted. Please reject the selected record.
        If strMessageCode = MsgCode.MSG00004 Then
            strSPErrorFiltered = RemoveDuplicatedSPID(strSPError)
            udcMessageBox.AddMessage(New SystemMessage(FunctionCode, "I", MsgCode.MSG00004), "%s", strSPErrorFiltered.TrimEnd(" ").TrimEnd(","))
            udcMessageBox.BuildMessageBox("Note")
            Return
        End If

        ' SP is under amendment with delist action: 
        'MSG00012 = The record(s) of SP cannot be proceeded because the profile(s) is/are under amendment. Please complete or reject the amendment before confirm the "Delist" action.
        If strMessageCode = MsgCode.MSG00012 Then
            strSPErrorFiltered = RemoveDuplicatedSPID(strSPError)
            udcMessageBox.AddMessage(New SystemMessage(FunctionCode, "I", MsgCode.MSG00012), "%s", strSPErrorFiltered.TrimEnd(" ").TrimEnd(","))
            udcMessageBox.BuildMessageBox("Note")
            Return
        End If

        '' No batch update: MsgCode.MSG00003 = The record(s) of %s cannot proceed in batch update. Please do it individually.
        'If strMessageCode = MsgCode.MSG00003 Then
        '    strSPErrorFiltered = RemoveDuplicatedSPID(strSPError)
        '    udcMessageBox.AddMessage(New SystemMessage(FunctionCode, "I", MsgCode.MSG00003), "%s", strSPErrorFiltered.TrimEnd(" ").TrimEnd(","))
        '    udcMessageBox.BuildMessageBox("Note")
        '    Return
        'End If

        ' ShowLevelTwoRecord = [0] or [1]
        Dim intLevelTwoMsgCode As Integer = ShowLevelTwoRecord(strSPError)

        strSPErrorFiltered = RemoveDuplicatedSPID(strSPError)

        Select Case intLevelTwoMsgCode
            Case 0
                ' Nothing here
            Case 1
                'MSG00006: The service provider %s has other account change confirmation request(s). Please either (1) complete other account change confirmation request(s) before confirm the "Delist" action <B>OR</B> (2) press "Confirm" to proceed the delist action and other account change confirmation request(s) will be rejected automatically.
                udcMessageBox.AddMessage(New SystemMessage(FunctionCode, "I", MsgCode.MSG00006), "%s", strSPErrorFiltered.TrimEnd(" ").TrimEnd(","))
                udcMessageBox.BuildMessageBox("Note")

                ' Below message are removed as it should be already block on the previous page
                'Case 5
                '    'MSG00005: The service provider %s is under amendment. Please either (1) complete the amendment before confirm the "Delist" action <B>OR</B> (2) press "Confirm" to proceed the delist action and the amendment will be rejected automatically.
                '    udcMessageBox.AddMessage(New SystemMessage(FunctionCode, "I", MsgCode.MSG00005), "%s", strSPErrorFiltered.TrimEnd(" ").TrimEnd(","))
                '    udcMessageBox.BuildMessageBox("Note")
                'Case 6
                '    'MSG00007: The service provider %s is under amendment and has other account change confirmation request(s). Please either (1) complete the amendment and account change confirmation request(s) before confirm the "Delist" action <B>OR</B> (2) press "Confirm" to proceed the delist action and all the amendment record and the account change request(s) will be rejected automatically.
                '    udcMessageBox.AddMessage(New SystemMessage(FunctionCode, "I", MsgCode.MSG00007), "%s", strSPErrorFiltered.TrimEnd(" ").TrimEnd(","))
                '    udcMessageBox.BuildMessageBox("Note")
                'Case 8
                '    'MSG00008: The service provider %s is under amendment. Please either (1) complete the amendment before confirm the "Delist" action <B>OR</B> (2) press "Confirm" to proceed the delist action and then reject the amendment manually.
                '    udcMessageBox.AddMessage(New SystemMessage(FunctionCode, "I", MsgCode.MSG00008), "%s", strSPErrorFiltered.TrimEnd(" ").TrimEnd(","))
                '    udcMessageBox.BuildMessageBox("Note")
                'Case 9
                '    'MSG00009: The service provider %s is under amendment and has other account change confirmation request(s). Please either (1) complete the amendment and account change confirmation request(s) before confirm the "Delist" action <B>OR</B> (2) press "Confirm" to proceed the delist action (other account change confirmation request(s) will be rejected automatically) and then reject the amendment manually.
                '    udcMessageBox.AddMessage(New SystemMessage(FunctionCode, "I", MsgCode.MSG00009), "%s", strSPErrorFiltered.TrimEnd(" ").TrimEnd(","))
                '    udcMessageBox.BuildMessageBox("Note")

        End Select
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    End Sub

    Protected Sub ibtnLevelOneReject_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtValidator As New Validator
        Dim udtSystemMessage As SystemMessage = udtValidator.chkGridSelectedNothing(gvRecordLevelOne, "chkSelect", 1)

        If Not udtSystemMessage Is Nothing Then
            udcMessageBox.AddMessage(udtSystemMessage)
            udcMessageBox.BuildMessageBox("ValidationFail")

        Else
            ibtnLevelTwoConfirm.Visible = False
            ibtnLevelTwoReject.Visible = True
            lblConfirm.Text = Me.GetGlobalResourceObject("Text", "ConfirmReject")

            'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim strSPError As String = String.Empty
            'ShowLevelTwoRecord()
            ShowLevelTwoRecord(strSPError)
            'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]


        End If
    End Sub

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Re-write function to simplify the checking logic: Delist + Under Amendment -> Block
    Private Function OutstandingRecordMessageCode(ByRef strSPError As String) As String
        Dim strReturn As String = String.Empty
        Dim dtLevelOne As DataTable = Session(SESS_AccountChangeConfirmationLevelOneRecord)
        Dim udtDB As New Database

        For Each r As GridViewRow In gvRecordLevelOne.Rows
            If CType(r.FindControl("chkSelect"), CheckBox).Checked Then
                Dim strAction As String = CType(r.FindControl("lblUpd_Type"), Label).Text
                Dim strSPID As String = CType(r.FindControl("lblSPID"), Label).Text
                Dim strSystemDtm As String = CType(r.FindControl("lblSystem_Dtm"), Label).Text
                Dim strSchemeCode As String = AntiXssEncoder.HtmlEncode(CType(r.FindControl("hfSchemeCode"), HiddenField).Value, True)

                For Each dr As DataRow In dtLevelOne.Select("Upd_Type = '" + strAction + "' and SP_ID = '" + strSPID + "' and Scheme_Code = '" + AntiXssEncoder.HtmlEncode(strSchemeCode, True) + "'")
                    If Convert.ToString(dr.Item("System_Dtm")) = strSystemDtm Then
                        Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, strSPID)

                        Select Case strAction
                            Case SPAccountMaintenanceUpdTypeStatus.SPDelist, SPAccountMaintenanceUpdTypeStatus.PracticeDelist
                                ' Delist Scheme/Practice Scheme + SP is under amendment -> Block
                                If udtSP.UnderModification = "Y" Then
                                    strSPError += strSPID + ", "
                                    strReturn = MsgCode.MSG00012
                                End If

                            Case Else
                                ' SP is delisted (actually this won't happen in normal flow, it must be caused by concurrent update)
                                If udtSP.RecordStatus = ServiceProviderStatus.Delisted Then
                                    strSPError = strSPID + ", "
                                    Return MsgCode.MSG00004
                                End If
                        End Select

                    End If
                Next

            End If
        Next

        Return strReturn
    End Function
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Private Function OutstandingRecordMessageCode(ByRef strSPError As String) As String
    '    Dim strReturn As String = String.Empty
    '    Dim dtLevelOne As DataTable = Session(SESS_AccountChangeConfirmationLevelOneRecord)
    '    Dim udtDB As New Database

    '    Dim count As Integer = 0

    '    Dim strTargetAction As String = String.Empty
    '    Dim strTargetSPID As String = String.Empty
    '    Dim strTargetSystemDtm As String = String.Empty
    '    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
    '    '-----------------------------------------------------------------------------------------
    '    Dim strPracticeDisplaySeq As String = String.Empty
    '    Dim strSchemeCode As String = String.Empty
    '    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

    '    For Each r As GridViewRow In gvRecordLevelOne.Rows
    '        If CType(r.FindControl("chkSelect"), CheckBox).Checked Then
    '            count += 1

    '            strTargetAction = CType(r.FindControl("lblUpd_Type"), Label).Text
    '            strTargetSPID = CType(r.FindControl("lblSPID"), Label).Text
    '            strTargetSystemDtm = CType(r.FindControl("lblSystem_Dtm"), Label).Text
    '            strPracticeDisplaySeq = CType(r.FindControl("hfPracticeDisplaySeq"), HiddenField).Value
    '            ' I-CRE16-003 Fix XSS [Start][Lawrence]
    '            strSchemeCode = AntiXssEncoder.HtmlEncode(CType(r.FindControl("hfSchemeCode"), HiddenField).Value, True)
    '            ' I-CRE16-003 Fix XSS [End][Lawrence]

    '        End If

    '    Next

    '    If count = 1 Then
    '        For Each dr As DataRow In dtLevelOne.Select("Upd_Type = '" + strTargetAction + "' and SP_ID = '" + strTargetSPID + "' and Scheme_Code = '" + AntiXssEncoder.HtmlEncode(strSchemeCode, True) + "'")
    '            If Convert.ToString(dr.Item("System_Dtm")) = strTargetSystemDtm Then
    '                Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, strTargetSPID)

    '                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
    '                '-----------------------------------------------------------------------------------------
    '                ' SP is delisted (actually this won't happen in normal flow, it must be caused by concurrent update)
    '                If udtSP.RecordStatus = ServiceProviderStatus.Delisted Then
    '                    'strSPError = strTargetSPID
    '                    strSPError = strTargetSPID + ", "
    '                    Return MsgCode.MSG00004
    '                End If

    '                ' Delist Scheme:
    '                ' (1) All other schemes are delisted + SP is under amendment -> Auto reject
    '                ' (2) All other practice scheme are delisted + adding new practice scheme under same practice -> Auto reject
    '                ' (3) Adding a new practice scheme which is conflicting with the delisting one -> Auto reject
    '                ' (4) Updating the practice scheme which is conflicting with the delisting one -> Manual reject (the staging record will not be proceeded)
    '                If strTargetAction = SPAccountMaintenanceUpdTypeStatus.SPDelist AndAlso udtSP.UnderModification = "Y" Then

    '                    '(1)
    '                    ' Check if this is the last scheme to delisted
    '                    Dim intNonDelistedScheme As Integer = 0

    '                    For Each udtScheme As SchemeInformationModel In udtSP.SchemeInfoList.Values
    '                        If udtScheme.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary _
    '                                AndAlso udtScheme.RecordStatus <> SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary Then
    '                            intNonDelistedScheme += 1

    '                            If intNonDelistedScheme > 1 Then Exit For
    '                        End If
    '                    Next

    '                    ' After delisting this scheme, the whole SP will be delisted and the Staging record will be rejected
    '                    If intNonDelistedScheme = 1 Then
    '                        'strSPError = strTargetSPID
    '                        strSPError = strTargetSPID + ", "
    '                        Return MsgCode.MSG00005
    '                    End If

    '                    Dim udtPractice As PracticeModel = New PracticeModel
    '                    udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByERN(udtSP.EnrolRefNo, udtDB)

    '                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    '                    ' ----------------------------------------------------------------------------------------                        
    '                    '(2)
    '                    For Each udtPracticePerm As PracticeModel In udtSP.PracticeList.Values

    '                        ' if all other Practice Scheme are delisted, practice will be delisted also
    '                        Dim intNonDelistedPracticeScheme As Integer = 0

    '                        For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPracticePerm.PracticeSchemeInfoList.Values
    '                            If udtPracticeScheme.RecordStatus <> SPMaintenanceDisplayStatus.DelistedVoluntary _
    '                                    AndAlso udtPracticeScheme.RecordStatus <> SPMaintenanceDisplayStatus.DelistedInvoluntary Then

    '                                intNonDelistedPracticeScheme += 1
    '                            End If

    '                            If intNonDelistedPracticeScheme > 1 Then Exit For
    '                        Next

    '                        If intNonDelistedPracticeScheme = 1 Then
    '                            ' Auto reject the amendment if adding a new practice scheme where the practice is to be delisted
    '                            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
    '                                If udtPracticeSchemeInfo.PracticeDisplaySeq = udtPracticePerm.DisplaySeq AndAlso _
    '                                    udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Active Then

    '                                    strSPError = strTargetSPID + ", "
    '                                    Return MsgCode.MSG00005
    '                                End If
    '                            Next
    '                        End If
    '                    Next
    '                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    '                    ' INT14-0022 - Auto reject the amendment if adding a new practice scheme which is conflicting with the delisting one [Start][Lawrence]
    '                    '(3)
    '                    For Each udtPracticeSchemeStag As PracticeSchemeInfoModel In udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByERN(udtSP.EnrolRefNo, udtDB).Values
    '                        If udtPracticeSchemeStag.SchemeCode = CStr(dr.Item("Scheme_Code")).Trim AndAlso udtPracticeSchemeStag.RecordStatus = PracticeSchemeInfoStagingStatus.Active Then
    '                            strSPError = strTargetSPID + ", "
    '                            Return MsgCode.MSG00005
    '                        End If
    '                    Next
    '                    ' INT14-0022 - Auto reject the amendment if adding a new practice scheme which is conflicting with the delisting one [End][Lawrence]

    '                    '(4)
    '                    ' Check if this is the practice scheme to delisted
    '                    Dim intAmendedPracticeScheme As Integer = 0

    '                    For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
    '                        If udtPracticeSchemeInfo.SchemeCode = CStr(dr.Item("Scheme_Code")).Trim AndAlso _
    '                            udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Update Then

    '                            intAmendedPracticeScheme += 1

    '                            If intAmendedPracticeScheme > 0 Then Exit For
    '                        End If
    '                    Next

    '                    ' After delisting this practice scheme, the staging record will not be proceeded.
    '                    If intAmendedPracticeScheme > 0 Then
    '                        strSPError = strTargetSPID + ", "
    '                        Return MsgCode.MSG00008
    '                    End If
    '                    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

    '                End If

    '                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
    '                '-----------------------------------------------------------------------------------------
    '                ' Delist Practice Scheme:
    '                ' (1) All other practice scheme are delisted + adding new practice scheme under same practice -> Auto reject
    '                ' (2) Updating the practice scheme which is conflicting with the delisting one -> Manual reject (the staging record will not be proceeded)
    '                If strTargetAction = SPAccountMaintenanceUpdTypeStatus.PracticeDelist AndAlso udtSP.UnderModification = "Y" Then

    '                    Dim udtPractice As PracticeModel = New PracticeModel
    '                    udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByERN(udtSP.EnrolRefNo, udtDB)

    '                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    '                    ' ----------------------------------------------------------------------------------------
    '                    '(1)
    '                    For Each udtPracticePerm As PracticeModel In udtSP.PracticeList.Values

    '                        ' Check all other Practice Scheme are delisted
    '                        Dim intNonDelistedPracticeScheme As Integer = 0

    '                        For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPracticePerm.PracticeSchemeInfoList.Values
    '                            If udtPracticeScheme.RecordStatus <> SPMaintenanceDisplayStatus.DelistedVoluntary _
    '                                    AndAlso udtPracticeScheme.RecordStatus <> SPMaintenanceDisplayStatus.DelistedInvoluntary Then

    '                                intNonDelistedPracticeScheme += 1
    '                            End If

    '                            If intNonDelistedPracticeScheme > 1 Then Exit For
    '                        Next

    '                        If intNonDelistedPracticeScheme = 1 Then
    '                            ' Auto reject the amendment if adding a new practice scheme where the practice is to be delisted
    '                            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
    '                                If udtPracticeSchemeInfo.PracticeDisplaySeq = udtPracticePerm.DisplaySeq AndAlso _
    '                                    udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Active Then

    '                                    strSPError = strTargetSPID + ", "
    '                                    Return MsgCode.MSG00005
    '                                End If
    '                            Next
    '                        End If
    '                    Next
    '                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    '                    '(2)
    '                    ' Check if this is the practice scheme to delisted
    '                    Dim intAmendedPracticeScheme As Integer = 0

    '                    For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
    '                        If udtPracticeSchemeInfo.SchemeCode = CStr(dr.Item("Scheme_Code")).Trim AndAlso _
    '                            udtPracticeSchemeInfo.PracticeDisplaySeq = CInt(strPracticeDisplaySeq) AndAlso _
    '                            udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Update Then

    '                            intAmendedPracticeScheme += 1

    '                            If intAmendedPracticeScheme > 0 Then Exit For
    '                        End If
    '                    Next

    '                    ' After delisting this practice scheme, the staging record will not be proceeded.
    '                    If intAmendedPracticeScheme > 0 Then
    '                        strSPError = strTargetSPID + ", "
    '                        Return MsgCode.MSG00008
    '                    End If
    '                End If
    '                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
    '            End If
    '        Next

    '    Else
    '        For Each r As GridViewRow In gvRecordLevelOne.Rows
    '            If CType(r.FindControl("chkSelect"), CheckBox).Checked Then
    '                Dim strAction As String = CType(r.FindControl("lblUpd_Type"), Label).Text
    '                Dim strSPID As String = CType(r.FindControl("lblSPID"), Label).Text
    '                Dim strSystemDtm As String = CType(r.FindControl("lblSystem_Dtm"), Label).Text
    '               ' I-CRE16-003 Fix XSS [Start][Lawrence]
    '                strSchemeCode = AntiXssEncoder.HtmlEncode(CType(r.FindControl("hfSchemeCode"), HiddenField).Value, True)
    '                ' I-CRE16-003 Fix XSS [End][Lawrence]

    '                For Each dr As DataRow In dtLevelOne.Select("Upd_Type = '" + strAction + "' and SP_ID = '" + strSPID + "' and Scheme_Code = '" + AntiXssEncoder.HtmlEncode(strSchemeCode, True) + "'")
    '                    If Convert.ToString(dr.Item("System_Dtm")) = strSystemDtm Then
    '                        Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, strSPID)

    '                        Select Case strAction
    '                            Case SPAccountMaintenanceUpdTypeStatus.SPDelist, SPAccountMaintenanceUpdTypeStatus.PracticeDelist
    '                                If udtSP.UnderModification = "Y" Then
    '                                    strSPError += strSPID + ", "
    '                                    strReturn = "00003"
    '                                End If
    '                            Case Else
    '                                If udtSP.RecordStatus = ServiceProviderStatus.Delisted Then
    '                                    strSPError += strSPID + ", "
    '                                    strReturn = "00003"
    '                                End If
    '                        End Select

    '                    End If
    '                Next

    '            End If
    '        Next

    '    End If

    '    Return strReturn

    'End Function
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    Private Function ShowLevelTwoRecord(ByRef strSPError As String) As Integer
        MultiViewAccountChangeConfirmation.ActiveViewIndex = ViewIndexSearchResultLevelTwo

        ' Clone the structure
        Dim dtLevelOne As DataTable = Session(SESS_AccountChangeConfirmationLevelOneRecord)
        Dim dtLevelTwo As DataTable = dtLevelOne.Clone

        For Each r As GridViewRow In gvRecordLevelOne.Rows
            If CType(r.FindControl("chkSelect"), CheckBox).Checked Then
                Dim strAction As String = CType(r.FindControl("lblUpd_Type"), Label).Text.Trim
                Dim strSPID As String = CType(r.FindControl("lblSPID"), Label).Text.Trim
                Dim strSystemDtm As String = CType(r.FindControl("lblSystem_Dtm"), Label).Text.Trim
                Dim strPracticeDisplaySeq As String = CType(r.FindControl("hfPracticeDisplaySeq"), HiddenField).Value.Trim
                Dim strSchemeCode As String = CType(r.FindControl("hfSchemeCode"), HiddenField).Value.Trim

                For Each dr As DataRow In dtLevelOne.Select("Upd_Type = '" + strAction + _
                                                            "' and SP_ID = '" + strSPID + _
                                                            "' and SP_Practice_Display_Seq = '" + strPracticeDisplaySeq + _
                                                            "' and Scheme_Code = '" + strSchemeCode + "'")
                    If Convert.ToString(dr.Item("System_Dtm")) = strSystemDtm Then
                        dtLevelTwo.ImportRow(dr)

                        ' Write Audit Log
                        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
                        udtAuditLogEntry.AddDescripton("SPID", strSPID)
                        udtAuditLogEntry.AddDescripton("Action", strAction)
                        udtAuditLogEntry.AddDescripton("System_Dtm", strSystemDtm)
                        udtAuditLogEntry.AddDescripton("PracticeDisplaySeq", strPracticeDisplaySeq)
                        udtAuditLogEntry.AddDescripton("Scheme", strSchemeCode)
                        udtAuditLogEntry.WriteLog(LogID.LOG00004, "Select", New AuditLogInfo(strSPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

                    End If

                    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    strSPError += strSPID + ", "
                    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
                Next
            End If
        Next

        If dtLevelTwo.Rows.Count > 0 Then
            Session(SESS_AccountChangeConfirmationLevelTwoRecord) = dtLevelTwo
            Me.GridViewDataBind(gvRecordLevelTwo, dtLevelTwo, CStr(ViewState("SortExpression")), CStr(ViewState("SortDirection")), False)

            Dim dtLevelTwoAutoReject As DataTable = SearchPracticeSchemeChange(dtLevelTwo)
            If dtLevelTwoAutoReject.Rows.Count > 0 Then
                Return 1
            End If

        End If

        Return 0

    End Function

    Private Function SearchPracticeSchemeChange(ByVal dt As DataTable) As DataTable
        Dim aryPracticeAction As String() = New String() {"DP", "SP", "RP"}

        Dim dtRes As DataTable = dt.Clone

        For Each dr As DataRow In dt.Rows
            If CStr(dr("Upd_Type")).Trim <> "D" Then Continue For

            For Each strPracticeAction As String In aryPracticeAction
                For Each r As DataRow In udtAccountChangeMaintenanceBLL.GetRecordDataTableByKeyValue(CStr(dr("SP_ID")).Trim, strPracticeAction).Rows
                    If CStr(dr("Scheme_Code")).Trim = CStr(r("Scheme_Code")).Trim AndAlso Not LevelTwoRecordExist(dt, r) Then dtRes.ImportRow(r)
                Next
            Next
        Next

        Return dtRes

    End Function

    Private Function LevelTwoRecordExist(ByVal dt As DataTable, ByVal r As DataRow) As Boolean
        For Each dr As DataRow In dt.Rows
            If CStr(dr("SP_ID")).Trim = CStr(r("SP_ID")).Trim _
                AndAlso CStr(dr("Upd_Type")).Trim = CStr(r("Upd_Type")).Trim _
                AndAlso CStr(dr("SP_Practice_Display_Seq")).Trim = CStr(r("SP_Practice_Display_Seq")).Trim _
                AndAlso CStr(dr("Scheme_Code")).Trim = CStr(r("Scheme_Code")).Trim Then Return True
        Next

        Return False

    End Function

    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function RemoveDuplicatedSPID(ByVal strSPError As String) As String

        Dim strSPErrorFiltered As String = String.Empty
        Dim arrayErrorSPID As Array = Split(strSPError.TrimEnd(" ").TrimEnd(","), ", ")

        Dim dtErrorSPID As DataTable = New DataTable("ErrorSPID")
        Dim dcErrorSPID As DataColumn = New DataColumn("SPID", GetType(System.String))
        dtErrorSPID.Columns.Add(dcErrorSPID)

        Dim drErrorSPID As DataRow

        For ct As Integer = 0 To arrayErrorSPID.Length - 1
            drErrorSPID = dtErrorSPID.NewRow()
            drErrorSPID.ItemArray = New Object() {arrayErrorSPID(ct)}
            dtErrorSPID.Rows.Add(drErrorSPID)
        Next

        Dim dvErrorSPID As DataView = New DataView(dtErrorSPID)
        Dim dtResultErrorSPID As DataTable = dvErrorSPID.ToTable(True, "SPID")

        For Each drResultErrorSPID As DataRow In dtResultErrorSPID.Select()
            strSPErrorFiltered += drResultErrorSPID.Item("SPID") + ", "
        Next

        Return strSPErrorFiltered
    End Function
    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

    '

    Protected Sub gvRecordLevelTwo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        gv_RowDataBound(e)
    End Sub

    Protected Sub gvRecordLevelTwo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_AccountChangeConfirmationLevelTwoRecord)
    End Sub

    Protected Sub gvRecordLevelTwo_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, SESS_AccountChangeConfirmationLevelTwoRecord)
    End Sub

    Protected Sub gvRecordLevelTwo_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_AccountChangeConfirmationLevelTwoRecord)
    End Sub

    ' Level Two Confirm / Reject

    Protected Sub ibtnLevelTwoConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim dtmConfirmationDate As DateTime = udtGeneralFunction.GetSystemDateTime()

        Dim intNumRecord As Integer = 0
        Dim strSPError As String = String.Empty
        Dim udtSystemMessageOut As SystemMessage = Nothing

        Try
            udtAccountChangeMaintenanceBLL.UpdateConfirmRecord(udtHCVUUserBLL.GetHCVUUser.UserID, SortAccountChangeConfirmationList(Session(SESS_AccountChangeConfirmationLevelTwoRecord)), _
                                                                intNumRecord, strSPError, udtSystemMessageOut)

            MultiViewAccountChangeConfirmation.ActiveViewIndex = ViewIndexComplete

            If strSPError = String.Empty Then
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoMessageBox.AddMessage(FunctionCode, "I", MsgCode.MSG00001)
                udcInfoMessageBox.BuildMessageBox()
            Else
                Dim strSPErrorFiltered As String = String.Empty
                strSPErrorFiltered = RemoveDuplicatedSPID(strSPError)

                If Not IsNothing(udtSystemMessageOut) Then
                    udcMessageBox.AddMessage(udtSystemMessageOut)
                Else
                    udcMessageBox.AddMessage(New SystemMessage(FunctionCode, "E", MsgCode.MSG00001), "%s", strSPErrorFiltered.TrimEnd(" ").TrimEnd(","))
                End If

                udcMessageBox.BuildMessageBox("UpdateFail")

            End If

            lblConfirmationDate.Text = udtFormatter.formatDateTime(dtmConfirmationDate)
            lblNoOfConfirmed.Text = intNumRecord

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                udcMessageBox.AddMessage(New SystemMessage(FunctCode.FUNT990001, "D", eSQL.Message))

                If udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                    udcMessageBox.Visible = False
                Else
                    udcMessageBox.BuildMessageBox("UpdateFail")
                End If
            Else
                Throw eSQL
            End If

            lblConfirmationDate.Text = udtFormatter.formatDateTime(dtmConfirmationDate)
            lblNoOfConfirmed.Text = intNumRecord

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Protected Sub ibtnLevelTwoReject_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim dtRecordSelected As DataTable = CType(Session(SESS_AccountChangeConfirmationLevelTwoRecord), DataTable)

        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser

        Dim udtGeneralFunction As GeneralFunction = New GeneralFunction
        Dim dtmConfirmationDate As DateTime = udtGeneralFunction.GetSystemDateTime()

        Try
            Dim udtAccountChangeMaintenanceBLL As New AccountChangeMaintenance.AccountChangeMaintenanceBLL
            udtAccountChangeMaintenanceBLL.UpdateRejectRecord(udtHCVUUser.UserID, dtRecordSelected)

            MultiViewAccountChangeConfirmation.ActiveViewIndex = ViewIndexComplete

            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.AddMessage(FunctionCode, "I", MsgCode.MSG00002)
            udcInfoMessageBox.BuildMessageBox()

            Dim udtFormatter As New Formatter
            lblConfirmationDate.Text = udtFormatter.formatDateTime(dtmConfirmationDate)
            lblNoOfConfirmed.Text = dtRecordSelected.Rows.Count

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                udcMessageBox.AddMessage(New Common.ComObject.SystemMessage(FunctCode.FUNT990001, "D", eSQL.Message))

                If udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                    udcMessageBox.Visible = False
                Else
                    udcMessageBox.BuildMessageBox("UpdateFail")
                End If
            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    '

    Private Function SortAccountChangeConfirmationList(ByVal dt As DataTable) As DataTable
        Dim aryConfirmationOrder As String() = New String() {"AT", "DT", "DP", "SP", "RP", "D", "S", "R"}

        ' Clone the structure
        Dim dtRes As DataTable = dt.Clone

        For Each strOrder As String In aryConfirmationOrder
            For Each dr As DataRow In dt.Rows
                If CStr(dr("Upd_Type")).Trim = strOrder Then
                    dtRes.ImportRow(dr)
                End If
            Next
        Next

        If dt.Rows.Count <> dtRes.Rows.Count Then
            Throw New Exception("Error in sorting Account Change Confirmation data table")
        End If

        Return dtRes

    End Function

    ' Back and Return

    Protected Sub ibtnLevelOneBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00032, "Level One Back Click")
        ' CRE11-021 log the missed essential information [End]

        MultiViewAccountChangeConfirmation.ActiveViewIndex = ViewIndexSearchCriteria
        GridViewUncheckAllCheckbox()
    End Sub

    Protected Sub ibtnLevelTwoBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00033, "Level Two Back Click")
        ' CRE11-021 log the missed essential information [End]

        MultiViewAccountChangeConfirmation.ActiveViewIndex = ViewIndexSearchResultLevelOne
        ddlChangeAccount.ClearSelection()
        GridViewUncheckAllCheckbox()
    End Sub

    Protected Sub ibtnReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00034, "Return Click")
        ' CRE11-021 log the missed essential information [End]

        MultiViewAccountChangeConfirmation.ActiveViewIndex = ViewIndexSearchCriteria
        udcInfoMessageBox.Visible = False
        GridViewUncheckAllCheckbox()
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
        Throw New Exception("Explicit passing the SPID is required for the WriteLog function, please check all your WriteLog functions! <tmk791>")
    End Function

#End Region

End Class