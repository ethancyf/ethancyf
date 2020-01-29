Imports System.Web.Security.AntiXss
Imports System.Data
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel
Imports Common.Component.UserRole
Imports Common.SearchCriteria
Imports HCVU.ReimbursementBLL

Partial Public Class reimbursement_new
    Inherits BasePageWithGridView

    Dim formater As New Common.Format.Formatter
    Dim claimtran As New EHSTransaction.EHSTransactionBLL
    Dim criteria As Common.SearchCriteria.SearchCriteria

    Dim udtHCVUUser As HCVUUserModel
    Dim udtHCVUUserBLL As New HCVUUserBLL

#Region "Fields"

    Private udtReimbursementBLL As New ReimbursementBLL("1st Authorization")

#End Region

#Region "Constants"

    Const FUNCTION_CODE As String = FunctCode.FUNT010401
    Dim strUpdateFail As String = "UpdateFail"
    Dim strValidationFail As String = "ValidationFail"
    Const strfirstReimbCutoffDateTSMP As String = "firstReimbCutoffDateTSMP"

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            FunctionCode = FUNCTION_CODE
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Reimbursement First Authorization Loaded")

            mvCore.SetActiveView(vSelectScheme)

            Dim intPageSize As Integer = (New GeneralFunction).GetPageSizeHCVU()

            gvGroupByReimburseID.PageSize = intPageSize
            gvGroupBySP.PageSize = intPageSize
            gvGroupByBankAccount.PageSize = intPageSize
            gvGroupByTransaction.PageSize = intPageSize

            'Bind scheme dropdown list
            Dim udtUserRoleBLL As New UserRoleBLL
            Dim udtUserRoleCollection As UserRoleModelCollection
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtHCVUUser.UserRoleCollection = udtUserRoleBLL.GetUserRoleCollection(udtHCVUUser.UserID)
            udtUserRoleCollection = udtHCVUUser.UserRoleCollection

            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtSchemeClaimModelList As SchemeClaimModelCollection
            Dim udtSchemeClaimModelListFilter As New Scheme.SchemeClaimModelCollection
            udtSchemeClaimModelList = udtSchemeClaimBLL.getAllDistinctSchemeClaim()

            For Each li As SchemeClaimModel In udtSchemeClaimModelList
                For Each udtUserRoleModel As UserRoleModel In udtUserRoleCollection.Values
                    If udtUserRoleModel.SchemeCode.Trim.Equals(li.SchemeCode) Then
                        If Not udtSchemeClaimModelListFilter.Contains(li) Then udtSchemeClaimModelListFilter.Add(li)
                    End If
                Next
            Next

            ' CRE13-001 - EHAPP [Start][Koala]
            ' -------------------------------------------------------------------------------------
            udtSchemeClaimModelListFilter = udtSchemeClaimModelListFilter.FilterReimbursementAvailableScheme
            ' CRE13-001 - EHAPP [End][Koala]
            ddlScheme.DataSource = udtSchemeClaimModelListFilter
            ddlScheme.DataValueField = "SchemeCode"
            ddlScheme.DataTextField = "DisplayCode"
            ddlScheme.DataBind()

            'Get the cutoff date (if any)
            Dim dtReimID As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(Nothing, ReimbursementStatus.StartReimbursement, ReimbursementAuthorisationStatus.Active, Nothing)

            If dtReimID.Rows.Count = 0 Then
                ' "Reimbursement cutoff date" has not been set.
                udcInfoMessageBox.AddMessage(FUNCTION_CODE, SeverityCode.SEVI, MsgCode.MSG00004)
                udcInfoMessageBox.BuildMessageBox()
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                ibtnSearchAndHold.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchNHoldTranDisableBtn")
                ddlScheme.Enabled = False
                ibtnSearchAndHold.Enabled = False

                lblReimCutoffDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
                Return
            Else
                ' Check this Reimubursement ID (Cutoff Date) has been reimbursed
                Dim strReimburseID As String = CStr(dtReimID.Rows(0)("Reimburse_ID")).Trim

                Dim dt As ReimbursementDataTable = udtReimbursementBLL.GetReimbursementProgress(strReimburseID)

                If dt.AllSchemeIsReimbursed Then
                    udcInfoMessageBox.AddMessage(FUNCTION_CODE, SeverityCode.SEVI, MsgCode.MSG00004)
                    udcInfoMessageBox.BuildMessageBox()
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                    mvCore.SetActiveView(vSelectScheme)

                    Me.ibtnSearchAndHold.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchNHoldTranDisableBtn")
                    ddlScheme.Enabled = False
                    Me.ibtnSearchAndHold.Enabled = False

                    lblReimCutoffDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
                    Return
                End If

                Session(strfirstReimbCutoffDateTSMP) = dtReimID.Rows(0)("TSMP")

            End If

            Dim strReimID As String = CStr(dtReimID.Rows(0)("Reimburse_ID")).Trim
            Session("firstReimbursementID") = strReimID

            criteria = New Common.SearchCriteria.SearchCriteria
            Session("Criteria") = criteria

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'lblReimCutoffDate.Text = formater.formatDate(CStr(dtReimID.Rows(0)("Cutoff_Date")).Trim, Session("language"))
            lblReimCutoffDate.Text = formater.formatDisplayDate(CDate(CStr(dtReimID.Rows(0)("Cutoff_Date")).Trim))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            hfReimbursementID.Value = strReimID

            'CRE17-010 (OCSSS integration) [Start][Koala CHENG]
            '-----------------------------------------------------------------------------------------
            'Set dropdown list disabled if only 1 scheme
            If udtSchemeClaimModelListFilter.Count = 1 Then
                ddlScheme.SelectedIndex = 1
                ddlScheme.Enabled = False

                HandleButtonEnable()
            Else
                ddlScheme.SelectedIndex = 0
                ddlScheme.Enabled = True
            End If
            'CRE17-010 (OCSSS integration) [End][Koala CHENG]

            ' Browser: Firefox
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnRPrintReport)
            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnFirstAuthorizeConfirm)
            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnReleaseConfirm)

        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If mvCore.GetActiveView.ID = vTransactionDetail.ID Then
            ' Enable <Previous> and <Next> buttons in detail view
            ibtnDPreviousRecord.Enabled = lbl_recordNo.Text <> "1"
            ibtnDNextRecord.Enabled = lbl_recordNo.Text <> lbl_recordMax.Text
        End If
    End Sub

#End Region

#Region "Supporting Functions"

    Private Function IsRMBAvailable(ByVal strSchemeCode As String) As Boolean
        Return (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(strSchemeCode).ReimbursementCurrency = EnumReimbursementCurrency.HKDRMB
    End Function

#End Region

    Protected Sub ddlScheme_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False

        HandleButtonEnable()
    End Sub

    Private Sub HandleButtonEnable()
        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Scheme Code", ddlScheme.SelectedValue)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00041, "Select scheme")

        If ddlScheme.SelectedValue = "0" Then
            ibtnSearchAndHold.Enabled = False
            ibtnContinue.Enabled = False

            udtAuditLogEntry.WriteEndLog(LogID.LOG00042, "Select scheme success")

        Else
            Dim strSchemeCode As String = ddlScheme.SelectedValue.Trim
            Dim strDisplayCode As String = ddlScheme.SelectedItem.Text.Trim
            Dim udtSchemeClaimList As SchemeClaimModelCollection = (New SchemeClaimBLL).getAllDistinctSchemeClaim

            Dim aryReimbursed As New ArrayList

            For Each drR As DataRow In udtReimbursementBLL.GetReimbursementProgress(hfReimbursementID.Value).Rows
                If Not IsDBNull(drR("Reimbursed")) Then
                    aryReimbursed.Add(udtSchemeClaimList.Filter(drR("Scheme_Code")).ReimbursementMode.ToString)
                End If
            Next

            If aryReimbursed.Contains(udtSchemeClaimList.Filter(strSchemeCode).ReimbursementMode.ToString) Then
                ' Message: The bank payment file of the current reimbursement (Reimbursement ID {Reimburse_ID}) had been generated.
                '          The reimbursement process for the scheme "{Scheme}" cannot be started until the whole reimbursement process is completed.
                udcInfoMessageBox.AddMessage(Me.FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006, _
                                             New String() {"{Reimburse_ID}", "{Scheme}"}, New String() {hfReimbursementID.Value, strDisplayCode})
                udcInfoMessageBox.BuildMessageBox()

                ibtnSearchAndHold.Enabled = False
                ibtnContinue.Enabled = False

                udtAuditLogEntry.AddDescripton("StackTrace", "Reimbursement is complete for the reimbursement mode of this scheme")
                udtAuditLogEntry.WriteEndLog(LogID.LOG00043, "Select scheme fail")

            Else
                Dim dr As DataRow = udtReimbursementBLL.GetReimbursementProgress(hfReimbursementID.Value).FindByScheme(strSchemeCode)

                If Not IsDBNull(dr("First_Authorised_By")) Then
                    ' Message: The reimbursement is in progress.
                    udcInfoMessageBox.AddMessage(Me.FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005, "%s", strDisplayCode)
                    udcInfoMessageBox.BuildMessageBox()

                    ibtnSearchAndHold.Enabled = False
                    ibtnContinue.Enabled = False

                    udtAuditLogEntry.AddDescripton("StackTrace", "Reimbursement is in progress")
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00043, "Select scheme fail")

                ElseIf Not IsDBNull(dr("Hold_By")) Then
                    ' Show button Continue
                    ibtnSearchAndHold.Enabled = False
                    ibtnContinue.Enabled = True

                    udtAuditLogEntry.AddDescripton("Show Button", "Continue")
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00042, "Select scheme success")

                Else
                    ' Show button Search & Hold Transaction
                    ibtnSearchAndHold.Enabled = True
                    ibtnContinue.Enabled = False

                    udtAuditLogEntry.AddDescripton("Show Button", "SearchAndHold")
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00042, "Select scheme success")

                End If

            End If

        End If

        ibtnSearchAndHold.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnSearchAndHold.Enabled, "SearchNHoldTransactionBtn", "SearchNHoldTranDisableBtn"))
        ibtnContinue.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnContinue.Enabled, "ContinueBtn", "ContinueDisableBtn"))

    End Sub

    '

    Protected Sub mvCore_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Show or hide search criteria review
        panSearchCriteriaReview.Visible = False
        lblSCRReimbursementCutoffDateText.Visible = False
        lblSCRReimbursementCutoffDate.Visible = False
        lblSCRSchemeText.Visible = False
        lblSCRScheme.Visible = False
        lblSCRReimburseIDText.Visible = False
        lblSCRReimburseID.Visible = False
        lblSCRSPNameText.Visible = False
        lblSCRSPName.Visible = False
        lblSCRSPIDText.Visible = False
        lblSCRSPID.Visible = False
        lblSCRBankAccountText.Visible = False
        lblSCRBankAccount.Visible = False
        lblSCRPracticeText.Visible = False
        lblSCRPractice.Visible = False

        Select Case mvCore.GetActiveView.ID
            Case vSelectScheme.ID
                ' Nothing here

            Case vGroupByReimburseID.ID
                panSearchCriteriaReview.Visible = True
                lblSCRReimbursementCutoffDateText.Visible = True
                lblSCRReimbursementCutoffDate.Visible = True
                lblSCRSchemeText.Visible = True
                lblSCRScheme.Visible = True

            Case vGroupBySP.ID
                panSearchCriteriaReview.Visible = True
                lblSCRReimbursementCutoffDateText.Visible = True
                lblSCRReimbursementCutoffDate.Visible = True
                lblSCRSchemeText.Visible = True
                lblSCRScheme.Visible = True
                lblSCRReimburseIDText.Visible = True
                lblSCRReimburseID.Visible = True

            Case vGroupByBankAccount.ID
                panSearchCriteriaReview.Visible = True
                lblSCRReimbursementCutoffDateText.Visible = True
                lblSCRReimbursementCutoffDate.Visible = True
                lblSCRSchemeText.Visible = True
                lblSCRScheme.Visible = True
                lblSCRReimburseIDText.Visible = True
                lblSCRReimburseID.Visible = True
                lblSCRSPNameText.Visible = True
                lblSCRSPName.Visible = True
                lblSCRSPIDText.Visible = True
                lblSCRSPID.Visible = True

            Case vGroupByTransaction.ID
                panSearchCriteriaReview.Visible = True
                lblSCRReimbursementCutoffDateText.Visible = True
                lblSCRReimbursementCutoffDate.Visible = True
                lblSCRSchemeText.Visible = True
                lblSCRScheme.Visible = True
                lblSCRReimburseIDText.Visible = True
                lblSCRReimburseID.Visible = True
                lblSCRSPNameText.Visible = True
                lblSCRSPName.Visible = True
                lblSCRSPIDText.Visible = True
                lblSCRSPID.Visible = True
                lblSCRBankAccountText.Visible = True
                lblSCRBankAccount.Visible = True
                lblSCRPracticeText.Visible = True
                lblSCRPractice.Visible = True

            Case vTransactionDetail.ID
                ' Nothing here

        End Select

        ' Clear working data
        Select Case mvCore.GetActiveView.ID
            Case vTransactionDetail.ID
                ' Do Nothing (Keep working data)
            Case Else
                Me.ClearWorkingData()
        End Select

    End Sub

    '
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Protected Sub ibtnSearchAndHold_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim sm As Common.ComObject.SystemMessage
        Dim strReimbuserID As String
        Dim strSchemeDisplayCode As String = String.Empty

        ' Data validation
        udcErrorMessage.Visible = False
        imgAlertScheme.Visible = False

        If ddlScheme.SelectedIndex = 0 Then
            imgAlertScheme.Visible = True

            udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00161)
            udcErrorMessage.BuildMessageBox("ValidationFail")

            Return
        End If

        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        Try
            udtAuditLogEntry.AddDescripton("Reimbursement Cutoff Date", lblReimCutoffDate.Text)
            udtAuditLogEntry.AddDescripton("Scheme", Me.ddlScheme.SelectedValue)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search and Hold")

            Me.hfSchemeCode.Value = Me.ddlScheme.SelectedValue
            strSchemeDisplayCode = Me.ddlScheme.SelectedItem.Text.Trim

            strReimbuserID = Session("firstReimbursementID").ToString.Trim

            Session("SearchCriteria_Display") = Nothing
            Session("Criteria") = Nothing
            Session("AuthorizationTxnList4") = Nothing

            'Check cutoff date valid
            Dim dtReim As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(strReimbuserID, _
                                                                       ReimbursementStatus.StartReimbursement, ReimbursementAuthorisationStatus.Active, Nothing)

            Dim udtDB As New Common.DataAccess.Database
            If dtReim.Rows.Count = 0 Then
                'Throw a concurrent update exception from db
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, DBNull.Value)}
                udtDB.RunProc("proc_checkTSMP", params)
            Else
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, Session(strfirstReimbCutoffDateTSMP)), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, dtReim.Rows(0)("TSMP"))}
                udtDB.RunProc("proc_checkTSMP", params)
            End If

            Dim obj As New Common.SearchCriteria.SearchCriteria()

            fillSearchCondition(obj)

            Dim dt As DataTable = udtReimbursementBLL.GetAuthorizationSummary(criteria, DBNull.Value, strReimbuserID, Me.hfSchemeCode.Value.Trim)

            Session("AuthorizationTxnListSummary") = dt

            If dt.Rows.Count > 0 Then
                udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

                Try
                    udtDB.BeginTransaction()

                    udtReimbursementBLL.ReimbursementAuthorizationHold(udtHCVUUser.UserID, criteria.CutoffDate, Me.hfSchemeCode.Value.Trim, strReimbuserID, udtDB)

                    udtReimbursementBLL.GeneratePreAuthorizationCheckingFile(Me.hfSchemeCode.Value.Trim, strSchemeDisplayCode, strReimbuserID, udtDB)

                    udtDB.CommitTransaction()

                Catch ex As Exception
                    udtDB.RollBackTranscation()
                    Throw
                End Try

                gvGroupByReimburseID.Columns(6).Visible = IsRMBAvailable(hfSchemeCode.Value)
                Me.GridViewDataBind(gvGroupByReimburseID, dt, "reimburseID", "DESC", False)

                mvCore.SetActiveView(vGroupByReimburseID)

                ' Message: Search & hold transaction success. The pre-authorization checking file will be generated by the system within 1 hour.
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoMessageBox.AddMessage("990000", "I", "00028")

                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search and Hold successful")
            Else
                ' Message: No records found.
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMessageBox.AddMessage("990000", "I", "00001")

                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search and Hold fail")
            End If

        Catch eSQL As SqlException
            If eSQL.Number = 50000 Then
                sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                udcErrorMessage.AddMessage(sm)
                udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00003, "Search and Hold fail")
            Else
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search and Hold fail")
                Throw
            End If

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search and Hold fail")
            Throw
        End Try

        udcInfoMessageBox.BuildMessageBox()
    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    Protected Sub ibtnContinue_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00034, "Continue click")

        Dim strSchemeCode As String = ddlScheme.SelectedValue.Trim

        Dim dt As DataTable = udtReimbursementBLL.GetHoldAuthorizationSummary(strSchemeCode)

        Session("AuthorizationTxnListSummary") = dt

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'lblReimCutoffDate.Text = formater.formatDate(dt.Rows(0)("AuthorisedCutoffTime"), Session("Language"))
        lblReimCutoffDate.Text = formater.formatDisplayDate(dt.Rows(0)("AuthorisedCutoffTime"))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Dim udtSearchCriteria As New SearchCriteria
        fillSearchCondition(udtSearchCriteria)

        gvGroupByReimburseID.Columns(6).Visible = IsRMBAvailable(hfSchemeCode.Value)
        GridViewDataBind(gvGroupByReimburseID, dt, "reimburseID", "DESC", False)

        mvCore.SetActiveView(vGroupByReimburseID)

    End Sub

    '

    Private Sub fillSearchCondition(ByVal objCriteria As Common.SearchCriteria.SearchCriteria)
        criteria = New Common.SearchCriteria.SearchCriteria

        If lblReimCutoffDate.Text = String.Empty Then
            objCriteria.CutoffDate = Me.GetGlobalResourceObject("Text", "Any")
        Else
            objCriteria.CutoffDate = lblReimCutoffDate.Text
            criteria.CutoffDate = lblReimCutoffDate.Text & " 23:59:59"
        End If

        objCriteria.FromDate = ""

        lblSCRReimbursementCutoffDate.Text = objCriteria.CutoffDate
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
        lblSCRScheme.Text = AntiXssEncoder.HtmlEncode(ddlScheme.SelectedItem.Text, True)
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
        lblSCRSPName.Text = objCriteria.ServiceProviderName
        lblSCRSPID.Text = objCriteria.ServiceProviderID
        lblSCRBankAccount.Text = objCriteria.BankAcctNo
        lblSCRPractice.Text = objCriteria.Practice

        ' I-CRE16-003 Fix XSS [Start][Lawrence]
        hfSchemeCode.Value = AntiXssEncoder.HtmlEncode(ddlScheme.SelectedValue, True)
        ' I-CRE16-003 Fix XSS [End][Lawrence]

        Session("SearchCriteria_Display") = objCriteria

        Session("Criteria") = criteria
    End Sub

    '

    Protected Sub gvGroupByReimburseID_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblGTotalAmountRMB As Label = e.Row.FindControl("lblGTotalAmountRMB")
            lblGTotalAmountRMB.Text = formater.formatMoneyRMB(lblGTotalAmountRMB.Text, False)

        End If

    End Sub

    Protected Sub gvGroupByReimburseID_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Me.udcInfoMessageBox.BuildMessageBox()
        Me.udcErrorMessage.BuildMessageBox()
        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            lblSCRReimburseID.Text = CType(row.Cells(1).FindControl("lbtn_reimburseID"), LinkButton).Text

            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

            Try
                udtAuditLogEntry.AddDescripton("Reimbursement Cutoff Date", lblSCRReimbursementCutoffDate.Text)
                udtAuditLogEntry.AddDescripton("Reimbursement ID", lblSCRReimburseID.Text.Trim)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "Select Reimbursement ID")

                criteria = Session("Criteria")
                Dim dt As DataTable = udtReimbursementBLL.GetAuthorizationSummaryBySP(criteria, ReimbursementStatus.HoldForFirstAuthorisation, lblSCRReimburseID.Text, Me.hfSchemeCode.Value.Trim)

                Session("AuthorizationTxnListBySP") = dt

                gvGroupBySP.PageIndex = 0

                gvGroupBySP.Columns(5).Visible = IsRMBAvailable(hfSchemeCode.Value)
                Me.GridViewDataBind(gvGroupBySP, dt, "spNum", "ASC", False)

                mvCore.SetActiveView(vGroupBySP)

                udtAuditLogEntry.WriteEndLog(LogID.LOG00017, "Select Reimbursement ID successful")
            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then
                    Dim sm As Common.ComObject.SystemMessage
                    sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                    udcErrorMessage.AddMessage(sm)
                    udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00018, "Select Reimbursement ID fail")
                Else
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Select Reimbursement ID fail")
                    Throw eSQL
                End If
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Select Reimbursement ID fail")
                Throw ex
            End Try
        End If
    End Sub

    Protected Sub gvGroupByReimburseID_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, "AuthorizationTxnListSummary")
    End Sub

    Protected Sub gvGroupByReimburseID_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, "AuthorizationTxnListSummary")
    End Sub

    Protected Sub gvGroupByReimburseID_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, "AuthorizationTxnListSummary")
    End Sub

    Protected Sub ibtnRBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00036, "Back click")

        mvCore.SetActiveView(vSelectScheme)

        HandleButtonEnable()
    End Sub

    Protected Sub ibtnRPrintReport_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcErrorMessage.BuildMessageBox("UpdateFail")
        Me.udcInfoMessageBox.BuildMessageBox()
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        Dim db As New Common.DataAccess.Database

        Try
            Dim strReimbursementID As String
            Dim row As GridViewRow = CType(gvGroupByReimburseID.Rows(0), GridViewRow)
            strReimbursementID = CType(row.Cells(1).FindControl("lbtn_reimburseID"), LinkButton).Text.Trim

            udtAuditLogEntry.AddDescripton("Reimbursement ID", strReimbursementID)
            udtAuditLogEntry.AddDescripton("Cutoff Date", lblSCRReimbursementCutoffDate.Text)
            udtAuditLogEntry.AddDescripton("Scheme", ddlScheme.SelectedValue)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Print")

            'Pass the data to the Viewer by Session variables
            Session("RID") = strReimbursementID.Trim
            Session("strCutoffDate") = lblSCRReimbursementCutoffDate.Text.Trim
            Session("bWatermark") = "N"
            Session("DPAScheme") = Me.ddlScheme.SelectedValue
            ScriptManager.RegisterStartupScript(Me, Page.GetType, FUNCTION_CODE, "window.open('DPAViewer.aspx','','width=' + (screen.width/1.5) + ',height=' + (screen.width/2) + ',resizable=yes')", True)
            'ScriptManager.RegisterStartupScript(Me, Page.GetType, FUNCTION_CODE, "window.open('DPAViewer.aspx?RID=" & strReimbursementID.Trim & "&strCutoffDate=" & Me.lbl_searchReimbCutoffDate.Text.Trim & "&bWatermark=N','','width=' + (screen.width/1.5) + ',height=' + (screen.width/2) + ',resizable=yes')", True)

            udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Print successful")
            db.CommitTransaction()
            'Else
            'Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            'Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00003")
            'Me.udcInfoMessageBox.BuildMessageBox()
            'udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Print fail - request already submitted")
            'End If
        Catch ex As Exception
            db.RollBackTranscation()

            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00002")
            Me.udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00009, "Print fail")
        End Try
    End Sub

    Protected Sub ibtnRConfirmFirstAuthorization_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcErrorMessage.BuildMessageBox()
        Me.udcInfoMessageBox.BuildMessageBox()
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        Try
            udtAuditLogEntry.AddDescripton("Reimbursement Cutoff Date", lblSCRReimbursementCutoffDate.Text)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "First Authorization Click")
            Me.ModalPopupExtenderConfirmAuthorize.Show()
            udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "First Authorization Click successful")
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "First Authorization Click fail")
        End Try

    End Sub

    Protected Sub ibtnRRelease_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00037, "Release click")

        popupRelease.Show()
    End Sub

    Protected Sub ibtnFirstAuthorizeConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        Try
            udtAuditLogEntry.AddDescripton("Reimbursement Cutoff Date", lblSCRReimbursementCutoffDate.Text)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00013, "Confirm First Authorization")

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser
            udtReimbursementBLL.Authorize(Session("Criteria"), Trim(udtHCVUUser.UserID), Me.hfSchemeCode.Value.Trim)
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00001")
            Me.udcInfoMessageBox.BuildMessageBox()
            mvCore.SetActiveView(vComplete)

            udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Confirm First Authorization successful")
        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                udcErrorMessage.AddMessage(New SystemMessage("990001", "D", eSQL.Message))
                udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00015, "Confirm First Authorization fail")

                mvCore.SetActiveView(vError)
                Return

            Else
                udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Confirm First Authorization fail")
                Throw eSQL
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Confirm First Authorization fail")
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnFirstAuthorizeCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00039, "Confirmation Cancel Click")
        ' CRE11-021 log the missed essential information [End]

        Me.ModalPopupExtenderConfirmAuthorize.Hide()
    End Sub

    Protected Sub ibtnReleaseConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False

        Try
            udtAuditLogEntry.AddDescripton("Reimbursement Cutoff Date", lblSCRReimbursementCutoffDate.Text)
            udtAuditLogEntry.AddDescripton("Scheme", ddlScheme.SelectedValue.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Release Confirm click")

            'Hold the transaction for first reimbursement
            Dim strReimburseID As String
            Dim row As GridViewRow = CType(gvGroupByReimburseID.Rows(0), GridViewRow)
            strReimburseID = CType(row.Cells(1).FindControl("lbtn_reimburseID"), LinkButton).Text.Trim

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser
            udtReimbursementBLL.ReimbursementAuthorizationRelease(udtHCVUUser.UserID, hfSchemeCode.Value.Trim, strReimburseID)


            mvCore.SetActiveView(vSelectScheme)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Release Confirm successful")

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                udcErrorMessage.AddMessage(New SystemMessage("990001", "D", eSQL.Message))
                udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00006, "Release Confirm fail")

                mvCore.SetActiveView(vError)
                Return

            Else
                udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Release Confirm fail")
                Throw
            End If

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Release Confirm fail")
            Throw
        End Try

        HandleButtonEnable()

    End Sub

    Protected Sub ibtnReleaseCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00035, "Release Cancel click")
    End Sub

    '

    Protected Sub gvGroupBySP_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblGTotalAmountRMB As Label = e.Row.FindControl("lblGTotalAmountRMB")
            lblGTotalAmountRMB.Text = formater.formatMoneyRMB(lblGTotalAmountRMB.Text, False)
        End If

    End Sub

    Protected Sub gvGroupBySP_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)

        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

            'Update the search criteria
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            lblSCRSPID.Text = CType(row.Cells(1).FindControl("lbtn_transNum"), LinkButton).Text   'lbtn_bankAccNo
            lblSCRSPName.Text = AntiXssEncoder.HtmlEncode(row.Cells(2).Text, True)

            Dim dt As DataTable

            ' CRE11-004
            Dim objAuditLogInfo As New AuditLogInfo(Left(lblSCRSPID.Text.Trim, 8), Nothing, Nothing, Nothing, Nothing, Nothing)
            ' End CRE11-004

            Try
                udtAuditLogEntry.AddDescripton("Reimbursement Cutoff Date", lblSCRReimbursementCutoffDate.Text)
                udtAuditLogEntry.AddDescripton("Reimbursement ID", lblSCRReimburseID.Text)
                udtAuditLogEntry.AddDescripton("SP ID", lblSCRSPID.Text)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00019, "Select SP ID", objAuditLogInfo)

                criteria = Session("Criteria")
                criteria.ServiceProviderID = Left(lblSCRSPID.Text.Trim, 8)
                criteria.ServiceProviderName = lblSCRSPName.Text
                criteria.SPPracticeDisplaySeq = udtReimbursementBLL.ExtractPracticeDisplaySeqFromSPIDPracticeDisplaySeq(lblSCRSPID.Text.Trim)
                Session("Criteria") = criteria

                dt = udtReimbursementBLL.GetAuthorizationSummaryByBankAcct(criteria, "P", Me.hfSchemeCode.Value.Trim)
                Session("AuthorizationTxnList3") = dt
                gvGroupByBankAccount.PageIndex = 0
                gvGroupByBankAccount.Columns(5).Visible = IsRMBAvailable(hfSchemeCode.Value)
                Me.GridViewDataBind(gvGroupByBankAccount, dt, "bankAccount", "ASC", False)

                mvCore.SetActiveView(vGroupByBankAccount)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00020, "Select SP ID successful", objAuditLogInfo)

            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then
                    Dim sm As Common.ComObject.SystemMessage
                    sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                    udcErrorMessage.AddMessage(sm)
                    udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00021, "Select SP ID fail", objAuditLogInfo)
                Else
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Select SP ID fail", objAuditLogInfo)
                    Throw
                End If
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Select SP ID fail", objAuditLogInfo)
                Throw
            End Try
        End If
    End Sub

    Protected Sub gvGroupBySP_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, "AuthorizationTxnListBySP")
    End Sub

    Protected Sub gvGroupBySP_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, "AuthorizationTxnListBySP")
    End Sub

    Protected Sub gvGroupBySP_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, "AuthorizationTxnListBySP")
    End Sub

    Protected Sub ibtnBackToAuthorizePage_Click_Shared(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00038, "Back to Authorize Page Click")
        ' CRE11-021 log the missed essential information [End]

        'Fill the search criteria with default
        fillSearchCondition(CType(Session("SearchCriteria_Display"), Common.SearchCriteria.SearchCriteria))

        mvCore.SetActiveView(vGroupByReimburseID)

    End Sub

    '

    Protected Sub gvGroupByBankAccount_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

            ' CRE11-004
            Dim objAuditLogInfo As New AuditLogInfo(Left(lblSCRSPID.Text.Trim, 8), Nothing, Nothing, Nothing, Nothing, Nothing)
            ' End CRE11-004

            Try
                'Update the search criteria
                Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                lblSCRBankAccount.Text = CType(row.Cells(1).FindControl("lbtn_bankAccNo"), LinkButton).Text
                lblSCRPractice.Text = row.Cells(2).Text

                criteria = Session("Criteria")
                'criteria.BankAcctNo = Me.lbl_searchBankAcc.Text
                criteria.BankAcctNo = CType(row.Cells(1).FindControl("lblOriBank"), Label).Text
                criteria.Practice = lblSCRPractice.Text
                criteria.SPPracticeDisplaySeq = udtReimbursementBLL.ExtractPracticeDisplaySeqFromSPIDPracticeDisplaySeq(lblSCRSPID.Text.Trim)
                Session("Criteria") = criteria

                udtAuditLogEntry.AddDescripton("Reimbursement Cutoff Date", lblSCRReimbursementCutoffDate.Text)
                udtAuditLogEntry.AddDescripton("Reimbursement ID", lblSCRReimburseID.Text.Trim)
                udtAuditLogEntry.AddDescripton("SP ID", lblSCRSPID.Text)
                udtAuditLogEntry.AddDescripton("Bank Account", criteria.BankAcctNo)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00022, "Select Bank Account", objAuditLogInfo)

                Try
                    criteria = Session("Criteria")
                    Dim dt As DataTable = udtReimbursementBLL.GetAuthorizationSummaryByTxn(criteria, "P", Me.hfSchemeCode.Value.Trim)
                    Session("AuthorizationTxnList4") = dt

                    gvGroupByTransaction.PageIndex = 0
                    gvGroupByTransaction.Columns(10).Visible = IsRMBAvailable(hfSchemeCode.Value)
                    Me.GridViewDataBind(gvGroupByTransaction, dt, "transNum", "ASC", False)

                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then
                        Dim sm As Common.ComObject.SystemMessage
                        sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                        udcErrorMessage.AddMessage(sm)
                        udcErrorMessage.BuildMessageBox(strUpdateFail)
                    Else
                        Throw
                    End If

                Catch ex As Exception
                    Throw
                End Try

                Me.udcErrorMessage.Visible = False
                mvCore.SetActiveView(vGroupByTransaction)

                udtAuditLogEntry.WriteEndLog(LogID.LOG00023, "Save Bank Account successful", objAuditLogInfo)
            Catch eSQL As SqlClient.SqlException
                udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "Save Bank Account fail", objAuditLogInfo)
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "Save Bank Account fail", objAuditLogInfo)
            End Try
        End If
    End Sub

    Protected Sub gvGroupByBankAccount_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim ctrlBankAcc As LinkButton
            Dim ctrlOriBank As Label
            ctrlBankAcc = CType(e.Row.Cells(1).FindControl("lbtn_bankAccNo"), LinkButton)
            ctrlOriBank = CType(e.Row.Cells(1).FindControl("lblOriBank"), Label)

            ctrlBankAcc.Text = formater.maskBankAccount(ctrlOriBank.Text)

            Dim lblGTotalAmountRMB As Label = e.Row.FindControl("lblGTotalAmountRMB")
            lblGTotalAmountRMB.Text = formater.formatMoneyRMB(lblGTotalAmountRMB.Text, False)

        End If
    End Sub

    Protected Sub gvGroupByBankAccount_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, "AuthorizationTxnList3")
    End Sub

    Protected Sub gvGroupByBankAccount_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, "AuthorizationTxnList3")
    End Sub

    Protected Sub gvGroupByBankAccount_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, "AuthorizationTxnList3")
    End Sub

    Protected Sub ibtnBBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        mvCore.SetActiveView(vGroupBySP)
    End Sub

    '

    Protected Sub gvGroupByTransaction_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

            Try
                Me.udcErrorMessage.Visible = False

                Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)

                Dim dt As New DataTable
                dt = Session("AuthorizationTxnList4")

                criteria = Session("Criteria")
                criteria.TransNum = formater.formatSystemNumberReverse(Trim(CType(row.Cells(1).FindControl("lbtn_transNum"), LinkButton).Text))
                Session("Criteria") = criteria

                ' CRE11-004
                Dim udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = claimtran.LoadClaimTran(criteria.TransNum)
                Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransactionModel)
                ' End CRE11-004

                udtAuditLogEntry.AddDescripton("Reimbursement Cutoff Date", lblSCRReimbursementCutoffDate.Text)
                udtAuditLogEntry.AddDescripton("Reimbursement ID", lblSCRReimburseID.Text)
                udtAuditLogEntry.AddDescripton("SP ID", lblSCRSPID.Text)
                udtAuditLogEntry.AddDescripton("Bank Account", lblSCRBankAccount.Text)
                udtAuditLogEntry.AddDescripton("Transaction No", criteria.TransNum)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00025, "Select Transaction", objAuditLogInfo)

                Dim searchEngine As New SearchEngineBLL

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Me.ClaimTransDetail1.ShowHKICSymbol = True
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                Me.ClaimTransDetail1.LoadTranInfo(udtEHSTransactionModel, searchEngine.SearchSuspendHistory(criteria))

                Me.lbl_recordNo.Text = gvGroupByTransaction.PageIndex * gvGroupByTransaction.PageSize + row.RowIndex + 1
                Me.lbl_recordMax.Text = dt.Rows.Count

                mvCore.SetActiveView(vTransactionDetail)

                udtAuditLogEntry.WriteEndLog(LogID.LOG00026, "Select Transaction successful", objAuditLogInfo)

            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then
                    Dim sm As Common.ComObject.SystemMessage
                    sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                    udcErrorMessage.AddMessage(sm)
                    udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00027, "Select Transaction fail")
                Else
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00027, "Select Transaction fail")
                    Throw eSQL
                End If
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00027, "Select Transaction fail")
                Throw ex
            End Try
        End If
    End Sub

    Protected Sub gvGroupByTransaction_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.Header) Then

            'adding an attribute for onclick event on the check box in the header
            'and passing the ClientID of the Select All checkbox
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & _
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).ClientID & "')")

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ctrlTransNum As LinkButton
            ctrlTransNum = CType(e.Row.Cells(2).FindControl("lbtn_transNum"), LinkButton)
            ctrlTransNum.Text = formater.formatSystemNumber(ctrlTransNum.Text)

            ' Transaction Time
            Dim lblTransactionTime As Label = e.Row.FindControl("lblTransactionTime")
            lblTransactionTime.Text = formater.formatDateTime(lblTransactionTime.Text.Trim)

            Dim lblGTotalAmountRMB As Label = e.Row.FindControl("lblGTotalAmountRMB")
            lblGTotalAmountRMB.Text = formater.formatMoneyRMB(lblGTotalAmountRMB.Text, False)

        End If
    End Sub

    Protected Sub gvGroupByTransaction_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, "AuthorizationTxnList4")
    End Sub

    Protected Sub gvGroupByTransaction_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, "AuthorizationTxnList4")
    End Sub

    Protected Sub gvGroupByTransaction_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, "AuthorizationTxnList4")
    End Sub

    Protected Sub ibtnTBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ''Clear the bank search criteria and load the default
        'Dim sc As New Common.SearchCriteria.SearchCriteria
        'sc = CType(Session("SearchCriteria_Display"), Common.SearchCriteria.SearchCriteria)

        'lblSCRBankAccount.Text = sc.BankAcctNo
        'lblSCRPractice.Text = sc.Practice

        'If sc.BankAcctNo.ToString.Equals(Me.GetGlobalResourceObject("Text", "Any")) Then
        '    CType(Session("Criteria"), Common.SearchCriteria.SearchCriteria).BankAcctNo = Nothing
        'Else
        '    CType(Session("Criteria"), Common.SearchCriteria.SearchCriteria).BankAcctNo = sc.BankAcctNo
        'End If

        'If sc.Practice.ToString.Equals(Me.GetGlobalResourceObject("Text", "Any")) Then
        '    CType(Session("Criteria"), Common.SearchCriteria.SearchCriteria).Practice = Nothing
        'Else
        '    CType(Session("Criteria"), Common.SearchCriteria.SearchCriteria).Practice = sc.Practice
        'End If

        mvCore.SetActiveView(vGroupByBankAccount)

    End Sub

    '

    Protected Sub ibtnDBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Dim sc As New Common.SearchCriteria.SearchCriteria
        'sc = CType(Session("SearchCriteria_Display"), Common.SearchCriteria.SearchCriteria)

        'If sc.TransNum.ToString.Equals(Me.GetGlobalResourceObject("Text", "Any")) Then
        '    CType(Session("Criteria"), Common.SearchCriteria.SearchCriteria).TransNum = Nothing
        'Else
        '    CType(Session("Criteria"), Common.SearchCriteria.SearchCriteria).TransNum = sc.TransNum
        'End If

        mvCore.SetActiveView(vGroupByTransaction)

    End Sub

    Protected Sub ibtnDNextRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        Try
            If CInt(Me.lbl_recordNo.Text) < CInt(Me.lbl_recordMax.Text) Then
                Dim intActualIndex As Integer

                Dim ee As New System.Web.UI.WebControls.GridViewPageEventArgs(udtReimbursementBLL.GetPageIndexInRecordNavigation(gvGroupByTransaction.PageSize, CInt(Me.lbl_recordNo.Text) + 1))

                Me.GridViewPageIndexChangingHandler(gvGroupByTransaction, ee, "AuthorizationTxnList4")

                Dim criteria As New Common.SearchCriteria.SearchCriteria
                Dim dt As New DataTable
                dt = Session("AuthorizationTxnList4")
                intActualIndex = CInt(CType(gvGroupByTransaction.Rows(CInt(Me.lbl_recordNo.Text) - gvGroupByTransaction.PageSize * gvGroupByTransaction.PageIndex).Cells(0).FindControl("lblRecordNum"), Label).Text.Trim) - 1
                criteria.TransNum = formater.formatSystemNumberReverse(dt.Rows(intActualIndex)(2).ToString)

                udtAuditLogEntry.AddDescripton("Transaction No", criteria.TransNum)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00031, "Next Record")

                Dim udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = claimtran.LoadClaimTran(criteria.TransNum)
                Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransactionModel)
                Dim searchEngine As New SearchEngineBLL

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Me.ClaimTransDetail1.ShowHKICSymbol = True
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                Me.ClaimTransDetail1.LoadTranInfo(udtEHSTransactionModel, searchEngine.SearchSuspendHistory(criteria))

                Me.lbl_recordNo.Text = CInt(Me.lbl_recordNo.Text) + 1
                udtAuditLogEntry.WriteEndLog(LogID.LOG00032, "Next Record successful", objAuditLogInfo)
            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim sm As Common.ComObject.SystemMessage
                sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                udcErrorMessage.AddMessage(sm)
                udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00033, "Next Record fail")
            Else
                udtAuditLogEntry.WriteEndLog(LogID.LOG00033, "Next Record fail")
                Throw eSQL
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00033, "Next Record fail")
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnDPreviousRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        Try
            If CInt(Me.lbl_recordNo.Text) > 1 Then
                Dim intActualIndex As Integer

                Dim ee As New System.Web.UI.WebControls.GridViewPageEventArgs(udtReimbursementBLL.GetPageIndexInRecordNavigation(gvGroupByTransaction.PageSize, CInt(Me.lbl_recordNo.Text) - 1))

                Me.GridViewPageIndexChangingHandler(gvGroupByTransaction, ee, "AuthorizationTxnList4")
                Dim criteria As New Common.SearchCriteria.SearchCriteria
                Dim dt As New DataTable
                dt = Session("AuthorizationTxnList4")
                intActualIndex = CInt(CType(gvGroupByTransaction.Rows(CInt(Me.lbl_recordNo.Text) - (gvGroupByTransaction.PageSize * gvGroupByTransaction.PageIndex) - 2).Cells(0).FindControl("lblRecordNum"), Label).Text.Trim) - 1
                criteria.TransNum = formater.formatSystemNumberReverse(dt.Rows(intActualIndex)(2).ToString)

                udtAuditLogEntry.AddDescripton("Transaction No", criteria.TransNum)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00028, "Previous Record")

                Dim udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = claimtran.LoadClaimTran(criteria.TransNum)
                Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransactionModel)
                Dim searchEngine As New SearchEngineBLL

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Me.ClaimTransDetail1.ShowHKICSymbol = True
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                Me.ClaimTransDetail1.LoadTranInfo(udtEHSTransactionModel, searchEngine.SearchSuspendHistory(criteria))

                Me.lbl_recordNo.Text = CInt(Me.lbl_recordNo.Text) - 1
                udtAuditLogEntry.WriteEndLog(LogID.LOG00029, "Previous Record successful", objAuditLogInfo)
            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim sm As Common.ComObject.SystemMessage
                sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                udcErrorMessage.AddMessage(sm)
                udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00030, "Previous Record fail")
            Else
                udtAuditLogEntry.WriteEndLog(LogID.LOG00030, "Previous Record fail")
                Throw eSQL
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00030, "Previous Record fail")
            Throw ex
        End Try
    End Sub

    '

    Protected Sub ibtnCReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00040, "Return Click")
        ' CRE11-021 log the missed essential information [End]

        'Me.panel_searchCriteria.Visible = False
        'Me.MultiView1.ActiveViewIndex = 0
        Response.Redirect("reimbursementFirstAuthorization.aspx")
    End Sub

    '

    Protected Sub ibtnErrorBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False

        mvCore.SetActiveView(vSelectScheme)
        HandleButtonEnable()
    End Sub

#Region "Implement Working Data"

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        'Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = Session(SESS_EHSTransaction)
        'If Not IsNothing(udtEHSTransaction) Then
        '    Return udtEHSTransaction.EHSAcct
        'Else
        '    Return Nothing
        'End If
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        'Return Session(SESS_EHSTransaction)
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
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
    ''' <summary>
    ''' CRE11-004
    '''  Clear all working data
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub ClearWorkingData()
        MyBase.ClearWorkingData()
    End Sub

    ''' <summary>
    ''' CRE11-004
    ''' Build Audit Log Info Object
    ''' </summary>
    ''' <remarks></remarks>
    Private Function BuildAuditLogInfoWithTransaction(ByVal udtEHSTransactionModel As EHSTransaction.EHSTransactionModel) As AuditLogInfo
        Dim strSPID As String = udtEHSTransactionModel.ServiceProviderID
        Dim strSPDocNo As String = Nothing
        Dim strAccType As String = udtEHSTransactionModel.EHSAcct.AccountSourceString
        Dim strAccID As String = udtEHSTransactionModel.EHSAcct.VoucherAccID
        Dim udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtEHSTransactionModel.EHSAcct.getPersonalInformation(udtEHSTransactionModel.DocCode)
        Dim strDocCode As String = udtEHSTransactionModel.DocCode
        Dim strDocNo As String = udtEHSPersonalInformation.IdentityNum
        Dim objAuditLogInfo As New AuditLogInfo(strSPID, strSPDocNo, strAccType, strAccID, strDocCode, strDocNo)

        Return objAuditLogInfo
    End Function

#End Region

End Class
