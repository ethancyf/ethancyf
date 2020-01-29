Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.HCVUUser
Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel

Partial Public Class reimbursementSecondAuthorization
    Inherits BasePageWithGridView

    Dim formater As New Common.Format.Formatter
    Private udtReimbursementBLL As ReimbursementBLL = New ReimbursementBLL("2nd Authorization")
    Dim criteria As Common.SearchCriteria.SearchCriteria

#Region "Constants"

    Const FUNCTION_CODE As String = FunctCode.FUNT010402
    Dim strUpdateFail As String = "UpdateFail"

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        ' Check session expire
        Call (New HCVUUserBLL).GetHCVUUser()

        If Not IsPostBack Then
            FunctionCode = FUNCTION_CODE
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Reimbursement Second Authorization Loaded")

            Dim intPageSize As Integer = (New GeneralFunction).GetPageSizeHCVU()

            gvGroupByScheme.PageSize = intPageSize
            gvGroupBySP.PageSize = intPageSize
            gvGroupByBankAccount.PageSize = intPageSize
            gvGroupByTransaction.PageSize = intPageSize

            Dim obj As New Common.SearchCriteria.SearchCriteria()
            fillSearchCondition(obj)
            LoadSummaryByFirstAuthorization()

            ibtnSConfirmSecondAuthorization.Enabled = False
            ibtnSConfirmSecondAuthorization.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmSecondAuthorizationDisableBtn")

        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If mvCore.GetActiveView.ID = vTransactionDetail.ID Then
            ibtnDPreviousRecord.Enabled = lbl_recordNo.Text <> "1"
            ibtnDNextRecord.Enabled = lbl_recordNo.Text <> lbl_recordMax.Text
        End If
    End Sub

    Private Sub LoadSummaryByFirstAuthorization()
        criteria = Session("Criteria")
        Dim dt As DataTable = udtReimbursementBLL.GetAuthorizationSummaryByFirstAuthorization(criteria, (New HCVUUserBLL).GetHCVUUser.UserID)

        Session("AuthorizationTxnListSummaryByFirstAuthorization") = dt

        If dt.Rows.Count > 0 Then
            gvGroupByScheme.PageIndex = 0
            gvGroupByScheme.Columns(8).Visible = IsRMBAvailable(dt)
            Me.GridViewDataBind(gvGroupByScheme, dt, "Display_Code", "ASC", False)

            mvCore.SetActiveView(vGroupByScheme)

        Else
            ' Message: No record found
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.AddMessage("990000", "I", "00001")
            udcInfoMessageBox.BuildMessageBox()

            mvCore.SetActiveView(vNoRecord)

        End If

    End Sub

#End Region

#Region "Supporting Functions"

    Private Function IsRMBAvailable(ByVal dt As DataTable) As Boolean
        Dim udtSchemeClaimList As SchemeClaimModelCollection = (New SchemeClaimBLL).getAllDistinctSchemeClaim

        For Each dr As DataRow In dt.Rows
            If udtSchemeClaimList.Filter(dr("Scheme_Code")).ReimbursementCurrency = EnumReimbursementCurrency.HKDRMB Then
                Return True
            End If
        Next

        Return False

    End Function

    Private Function IsRMBAvailable(ByVal strSchemeCode As String) As Boolean
        Return (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(strSchemeCode).ReimbursementCurrency = EnumReimbursementCurrency.HKDRMB
    End Function

#End Region

    Protected Sub mvCore_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Show or hide search criteria review
        panSearchCriteriaReview.Visible = False

        lblSCRSchemeText.Visible = False
        lblSCRScheme.Visible = False
        lblSCRReimbursementIDText.Visible = False
        lblSCRReimbursementID.Visible = False
        lblSCRFirstAuthorizedTimeText.Visible = False
        lblSCRFirstAuthorizedTime.Visible = False
        lblSCRFirstAuthorizedByText.Visible = False
        lblSCRFirstAuthorizedBy.Visible = False
        lblSCRSPNameText.Visible = False
        lblSCRSPName.Visible = False
        lblSCRSPIDText.Visible = False
        lblSCRSPID.Visible = False
        lblSCRBankAccountText.Visible = False
        lblSCRBankAccount.Visible = False
        lblSCRPracticeText.Visible = False
        lblSCRPractice.Visible = False

        Select Case mvCore.GetActiveView.ID
            Case vGroupByScheme.ID
                ' Nothing here

            Case vGroupBySP.ID
                panSearchCriteriaReview.Visible = True
                lblSCRSchemeText.Visible = True
                lblSCRScheme.Visible = True
                lblSCRReimbursementIDText.Visible = True
                lblSCRReimbursementID.Visible = True
                lblSCRFirstAuthorizedTimeText.Visible = True
                lblSCRFirstAuthorizedTime.Visible = True
                lblSCRFirstAuthorizedByText.Visible = True
                lblSCRFirstAuthorizedBy.Visible = True

            Case vGroupByBankAccount.ID
                panSearchCriteriaReview.Visible = True
                lblSCRSchemeText.Visible = True
                lblSCRScheme.Visible = True
                lblSCRReimbursementIDText.Visible = True
                lblSCRReimbursementID.Visible = True
                lblSCRFirstAuthorizedTimeText.Visible = True
                lblSCRFirstAuthorizedTime.Visible = True
                lblSCRFirstAuthorizedByText.Visible = True
                lblSCRFirstAuthorizedBy.Visible = True
                lblSCRSPNameText.Visible = True
                lblSCRSPName.Visible = True
                lblSCRSPIDText.Visible = True
                lblSCRSPID.Visible = True

            Case vGroupByTransaction.ID
                panSearchCriteriaReview.Visible = True
                lblSCRSchemeText.Visible = True
                lblSCRScheme.Visible = True
                lblSCRReimbursementIDText.Visible = True
                lblSCRReimbursementID.Visible = True
                lblSCRFirstAuthorizedTimeText.Visible = True
                lblSCRFirstAuthorizedTime.Visible = True
                lblSCRFirstAuthorizedByText.Visible = True
                lblSCRFirstAuthorizedBy.Visible = True
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

    Private Sub fillSearchCondition(ByVal objCriteria As Common.SearchCriteria.SearchCriteria)
        criteria = New Common.SearchCriteria.SearchCriteria

        'If Me.txt_serviceProvider.Text = "" Then
        '    objCriteria.ServiceProviderName = "Any"
        'Else
        '    objCriteria.ServiceProviderName = Me.txt_serviceProvider.Text
        '    criteria.ServiceProviderName = Me.txt_serviceProvider.Text
        'End If

        'If Me.txt_SPID.Text = "" Then
        '    objCriteria.ServiceProviderID = "Any"
        'Else
        '    objCriteria.ServiceProviderID = Me.txt_SPID.Text
        '    criteria.ServiceProviderID = Me.txt_SPID.Text
        'End If

        'If Me.txt_bankAccount.Text = "" Then
        '    objCriteria.BankAcctNo = "Any"
        'Else
        '    objCriteria.BankAcctNo = Me.txt_bankAccount.Text
        '    criteria.BankAcctNo = Me.txt_bankAccount.Text
        'End If

        'objCriteria.Practice = "Any"

        'If Me.ddl_serviceType.SelectedItem.Text = "Any" Then
        '    objCriteria.HealthProf = "Any"
        'Else
        '    objCriteria.HealthProf = Me.ddl_serviceType.SelectedItem.Text
        '    criteria.HealthProf = Me.ddl_serviceType.SelectedItem.Text
        'End If

        'objCriteria.TransStatus = ClaimTransStatus.FirstAuthorised
        'criteria.TransStatus = ClaimTransStatus.SecondAuthorised

        'If Me.txt_transNum.Text = "" Then
        '    objCriteria.TransNum = "Any"
        'Else
        '    objCriteria.TransNum = Me.txt_transNum.Text
        '    criteria.TransNum = Me.txt_transNum.Text
        'End If

        'If Me.txt_todate.Text = "" Then
        '    objCriteria.CutoffDate = "Any"
        'Else
        '    'objCriteria.CutoffDate = ConvertInputDateToDisplayDate(Me.txt_todate.Text)
        '    'criteria.CutoffDate = ConvertInputDateToDisplayDate(Me.txt_todate.Text)
        '    criteria.CutoffDate = Now.ToString
        'End If

        'objCriteria.FromDate = ""

        ''objCriteria.FirstAuthorizedBy = "Any"
        ''objCriteria.FirstAuthorizedDate = "Any"

        'If Me.txt_VRhkid.Text = "" Then
        '    objCriteria.VoucherRecipientHKIC = "Any"
        'Else
        '    objCriteria.VoucherRecipientHKIC = Me.txt_VRhkid.Text
        '    criteria.VoucherRecipientHKIC = Me.txt_VRhkid.Text
        'End If

        'If Me.txt_VRName.Text = "" Then
        '    objCriteria.VoucherRecipientName = "Any"
        'Else
        '    objCriteria.VoucherRecipientName = Me.txt_VRName.Text
        '    criteria.VoucherRecipientName = Me.txt_VRName.Text
        'End If

        'Me.lbl_searchReimbCutoffDate.Text = objCriteria.CutoffDate
        'Me.lbl_searchSPID.Text = objCriteria.ServiceProviderID
        'Me.lbl_searchSPName.Text = objCriteria.ServiceProviderName
        'Me.lbl_searchBankAcc.Text = objCriteria.BankAcctNo
        'Me.lbl_searchPractice.Text = objCriteria.Practice
        'Me.lbl_searchV1By.Text = objCriteria.FirstAuthorizedBy
        'Me.lbl_searchV1Date.Text = objCriteria.FirstAuthorizedDate
        'Me.lbl_searchServiceType.Text = objCriteria.HealthProf
        'Me.lbl_searchTranNum.Text = objCriteria.TransNum
        'Me.lbl_searchVRHKIC.Text = objCriteria.VoucherRecipientHKIC
        'Me.lbl_searchVRName.Text = objCriteria.VoucherRecipientName
        'Me.panel_searchCriteria.Visible = True
        'Session("SearchCriteria_Display") = objCriteria

        criteria.CutoffDate = Now.ToString
        criteria.TransStatus = ReimbursementStatus.SecondAuthorised
        Session("Criteria") = criteria
        'criteria.TransStatus = "1st Authorized"
    End Sub

    '

    Protected Sub gvGroupByScheme_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If Not (e.CommandName.Equals("Select") Or e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Me.udcErrorMessage.BuildMessageBox()
            Me.udcInfoMessageBox.BuildMessageBox()

            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

            Try
                Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                lblSCRFirstAuthorizedTime.Text = CType(row.FindControl("lbl_v1Date"), Label).Text.Trim
                lblSCRFirstAuthorizedBy.Text = CType(row.FindControl("lbl_v1By"), Label).Text.Trim
                lblSCRReimbursementID.Text = CType(row.FindControl("lbtn_reimburseID"), LinkButton).Text.Trim
                lblSCRScheme.Text = CType(row.FindControl("lbl_SchemeCode"), Label).Text.Trim
                hfSchemeCode.Value = CType(row.FindControl("hfSchemeCode"), HiddenField).Value.Trim

                udtAuditLogEntry.AddDescripton("Reimbursement ID", lblSCRReimbursementID.Text.Trim)
                udtAuditLogEntry.AddDescripton("Scheme", hfSchemeCode.Value)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Select First Authorization")

                Dim criteria As New Common.SearchCriteria.SearchCriteria
                criteria = Session("Criteria")
                criteria.FirstAuthorizedBy = lblSCRFirstAuthorizedBy.Text
                criteria.FirstAuthorizedDate = lblSCRFirstAuthorizedTime.Text
                Session("Criteria") = criteria

                Dim dt As DataTable = udtReimbursementBLL.GetAuthorizationSummaryBySP(criteria, ReimbursementStatus.FirstAuthorised, lblSCRReimbursementID.Text.Trim, Me.hfSchemeCode.Value, True)

                Session("AuthorizationTxnListBySPID") = dt

                Session("Criteria") = criteria

                gvGroupBySP.PageIndex = 0
                gvGroupBySP.Columns(6).Visible = IsRMBAvailable(hfSchemeCode.Value)
                Me.GridViewDataBind(gvGroupBySP, dt, "spNum", "ASC", False)

                mvCore.SetActiveView(vGroupBySP)

                udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Select First Authorization successful")
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Select First Authorization fail")
                Throw
            End Try
        End If
    End Sub

    Protected Sub gvGroupByScheme_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(sender, "Select$" + e.Row.RowIndex.ToString(), False))
            e.Row.Style.Add("cursor", "hand")

            ' First Authorized Time
            Dim lbl_v1Date As Label = e.Row.FindControl("lbl_v1Date")
            lbl_v1Date.Text = formater.formatDateTime(lbl_v1Date.Text.Trim)

            ' Amount Claimed RMB
            Dim strScheme As String = DirectCast(e.Row.FindControl("hfSchemeCode"), HiddenField).Value
            Dim lblGAmountClaimedRMB As Label = e.Row.FindControl("lblGAmountClaimedRMB")

            If IsRMBAvailable(strScheme) Then
                lblGAmountClaimedRMB.Text = formater.formatMoneyRMB(lblGAmountClaimedRMB.Text, False)
            Else
                lblGAmountClaimedRMB.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblGAmountClaimedRMB.Enabled = False
            End If

        End If
    End Sub

    Protected Sub gvGroupByScheme_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.udcInfoMessageBox.BuildMessageBox()
        Me.udcErrorMessage.BuildMessageBox()

        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        Try
            Dim row As GridViewRow = gvGroupByScheme.SelectedRow

            lblSCRReimbursementID.Text = CType(row.FindControl("lbtn_reimburseID"), LinkButton).Text.Trim
            lblSCRFirstAuthorizedTime.Text = CType(row.FindControl("lbtn_v1Date"), LinkButton).Text.Trim
            lblSCRFirstAuthorizedBy.Text = CType(row.FindControl("lbl_v1By"), Label).Text.Trim
            lblSCRScheme.Text = CType(row.FindControl("lbl_SchemeCode"), Label).Text.Trim
            hfSchemeCode.Value = CType(row.FindControl("hfSchemeCode"), HiddenField).Value.Trim

            udtAuditLogEntry.AddDescripton("Reimbursement ID", lblSCRReimbursementID.Text.Trim)
            udtAuditLogEntry.AddDescripton("Scheme", lblSCRScheme.Text)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Highlight First Authorization")

            If (New HCVUUserBLL).GetHCVUUser.UserID <> lblSCRFirstAuthorizedBy.Text.Trim Then
                ibtnSConfirmSecondAuthorization.Enabled = True
                ibtnSConfirmSecondAuthorization.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmSecondAuthorizationBtn")

                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Highlight First Authorization successful")

            Else
                ibtnSConfirmSecondAuthorization.Enabled = False
                ibtnSConfirmSecondAuthorization.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmSecondAuthorizationDisableBtn")

                ' Message: You have authorized this batch before
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00002")
                udcInfoMessageBox.BuildMessageBox()

                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Highlight First Authorization successful")

            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Highlight First Authorization fail")
            Throw ex
        End Try
    End Sub

    Protected Sub gvGroupByScheme_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, "AuthorizationTxnListSummaryByFirstAuthorization")
    End Sub

    Protected Sub gvGroupByScheme_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, "AuthorizationTxnListSummaryByFirstAuthorization")
        gvGroupByScheme.SelectedIndex = -1
        ibtnSConfirmSecondAuthorization.Enabled = False
        ibtnSConfirmSecondAuthorization.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmSecondAuthorizationDisableBtn")
        udcInfoMessageBox.BuildMessageBox()
        udcErrorMessage.BuildMessageBox()
    End Sub

    Protected Sub gvGroupByScheme_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, "AuthorizationTxnListSummaryByFirstAuthorization")
        gvGroupByScheme.SelectedIndex = -1
        ibtnSConfirmSecondAuthorization.Enabled = False
        ibtnSConfirmSecondAuthorization.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmSecondAuthorizationDisableBtn")
        udcInfoMessageBox.BuildMessageBox()
        udcErrorMessage.BuildMessageBox()
    End Sub

    Protected Sub ibtnSConfirmSecondAuthorization_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        Try
            Dim r As GridViewRow = gvGroupByScheme.SelectedRow

            udtAuditLogEntry.AddDescripton("Reimbursement ID", DirectCast(r.FindControl("lbtn_reimburseID"), LinkButton).Text.Trim)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Second Authorization Click")

            Dim dt As DataTable
            Dim dr As DataRow
            dt = New DataTable()

            dt.Columns.Add(New DataColumn("lineNum", GetType(Integer)))
            dt.Columns.Add(New DataColumn("reimburseID", GetType(String)))
            dt.Columns.Add(New DataColumn("v1Date", GetType(String)))
            dt.Columns.Add(New DataColumn("v1By", GetType(String)))
            dt.Columns.Add(New DataColumn("noTran", GetType(String)))
            dt.Columns.Add(New DataColumn("noSP", GetType(String)))
            'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'dt.Columns.Add(New DataColumn("totalAmount", GetType(Single)))
            dt.Columns.Add(New DataColumn("totalAmount", GetType(Decimal)))
            'dt.Columns.Add(New DataColumn("totalAmountRMB", GetType(Single)))
            dt.Columns.Add(New DataColumn("totalAmountRMB", GetType(Decimal)))
            'I-CRP15-002 (Fix the display problem of total voucher amount (RMB) in reimbursement UI) [End][Chris YIM]

            dt.Columns.Add(New DataColumn("Scheme_Code", GetType(String)))
            dt.Columns.Add(New DataColumn("Display_Code", GetType(String)))

            dr = dt.NewRow
            dr("lineNum") = 1
            dr("reimburseID") = DirectCast(r.FindControl("lbtn_reimburseID"), LinkButton).Text.Trim
            dr("v1By") = DirectCast(r.FindControl("lbl_v1By"), Label).Text.Trim
            dr("v1Date") = DirectCast(r.FindControl("lbtn_v1Date"), LinkButton).Text.Trim
            dr("noTran") = r.Cells(5).Text
            dr("noSP") = r.Cells(6).Text
            dr("totalAmount") = r.Cells(7).Text

            If IsRMBAvailable(DirectCast(r.FindControl("hfSchemeCode"), HiddenField).Value) Then
                dr("totalAmountRMB") = DirectCast(r.FindControl("lblGAmountClaimedRMB"), Label).Text
            End If

            dr("Display_Code") = DirectCast(r.FindControl("lbl_SchemeCode"), Label).Text.Trim
            dr("Scheme_Code") = DirectCast(r.FindControl("hfSchemeCode"), HiddenField).Value.Trim
            dt.Rows.Add(dr)

            gvConfirmSecondAuthorization.Columns(8).Visible = IsRMBAvailable(dt)

            gvConfirmSecondAuthorization.DataSource = dt
            gvConfirmSecondAuthorization.DataBind()

            ' Message: Please confirm the following information
            udcInfoMessageBox.AddMessage(FUNCTION_CODE, SeverityCode.SEVI, MsgCode.MSG00003)
            udcInfoMessageBox.BuildMessageBox()
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

            mvCore.SetActiveView(vConfirmSecondAuthorization)

            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Second Authorization Click successful")

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Second Authorization Click fail")
            Throw
        End Try

    End Sub

    '

    Protected Sub gvGroupBySP_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Amount Claimed RMB
            Dim lblGAmountClaimedRMB As Label = e.Row.FindControl("lblGAmountClaimedRMB")
            lblGAmountClaimedRMB.Text = formater.formatMoneyRMB(lblGAmountClaimedRMB.Text, False)

        End If
    End Sub

    Protected Sub gvGroupBySP_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

            Try
                ' Update the search criteria
                Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                lblSCRSPID.Text = CType(row.Cells(1).FindControl("lbtn_spID"), LinkButton).Text
                lblSCRSPName.Text = row.Cells(3).Text

                Dim objAuditLogInfo As New AuditLogInfo(Left(lblSCRSPID.Text.Trim, 8), Nothing, Nothing, Nothing, Nothing, Nothing)

                udtAuditLogEntry.AddDescripton("Reimbursement ID", lblSCRReimbursementID.Text.Trim)
                udtAuditLogEntry.AddDescripton("SPID", lblSCRSPID.Text)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "Select SPID", objAuditLogInfo)

                criteria = Session("Criteria")
                criteria.ServiceProviderID = Left(lblSCRSPID.Text.Trim, 8)
                criteria.SPPracticeDisplaySeq = udtReimbursementBLL.ExtractPracticeDisplaySeqFromSPIDPracticeDisplaySeq(lblSCRSPID.Text.Trim)
                criteria.ServiceProviderName = lblSCRSPName.Text

                Dim dt As DataTable = udtReimbursementBLL.GetAuthorizationSummaryByBankAcct(criteria, ReimbursementStatus.FirstAuthorised, Me.hfSchemeCode.Value.Trim, True)

                Session("AuthorizationTxnListByBank") = dt

                Session("Criteria") = criteria

                gvGroupByBankAccount.PageIndex = 0
                gvGroupByBankAccount.Columns(6).Visible = IsRMBAvailable(hfSchemeCode.Value)
                Me.GridViewDataBind(gvGroupByBankAccount, dt, "bankAccount", "ASC", False)

                mvCore.SetActiveView(vGroupByBankAccount)

                udtAuditLogEntry.WriteEndLog(LogID.LOG00017, "Select SPID successful", objAuditLogInfo)

            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Select SPID fail")
                Throw ex
            End Try
        End If
    End Sub

    Protected Sub gvGroupBySP_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, "AuthorizationTxnListBySPID")
    End Sub

    Protected Sub gvGroupBySP_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, "AuthorizationTxnListBySPID")
    End Sub

    Protected Sub gvGroupBySP_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, "AuthorizationTxnListBySPID")
    End Sub

    Protected Sub ibtnBackToAuthorizePage_Click_Shared(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00034, "Back to Authorize Page Click")
        ' CRE11-021 log the missed essential information [End]

        mvCore.SetActiveView(vGroupByScheme)

    End Sub

    '

    Protected Sub gvGroupByBankAccount_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

            Dim objAuditLogInfo As New AuditLogInfo(Left(lblSCRSPID.Text.Trim, 8), Nothing, Nothing, Nothing, Nothing, Nothing)

            Try
                ' Update the search criteria
                Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                lblSCRBankAccount.Text = CType(row.Cells(1).FindControl("lbtn_bankAccNo"), LinkButton).Text
                lblSCRPractice.Text = row.Cells(3).Text

                criteria = Session("Criteria")
                criteria.BankAcctNo = CType(row.Cells(1).FindControl("lblOriBank"), Label).Text
                criteria.Practice = lblSCRPractice.Text
                criteria.SPPracticeDisplaySeq = udtReimbursementBLL.ExtractPracticeDisplaySeqFromSPIDPracticeDisplaySeq(lblSCRSPID.Text.Trim)

                udtAuditLogEntry.AddDescripton("Reimbursement ID", lblSCRReimbursementID.Text.Trim)
                udtAuditLogEntry.AddDescripton("SPID", lblSCRSPID.Text)
                udtAuditLogEntry.AddDescripton("Bank Account", criteria.BankAcctNo)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00019, "Select Bank Account", objAuditLogInfo)

                Dim dt As DataTable = udtReimbursementBLL.GetAuthorizationSummaryByTxn(criteria, ReimbursementStatus.FirstAuthorised, Me.hfSchemeCode.Value.Trim, True)

                Session("Criteria") = criteria
                Session("AuthorizationTxnList") = dt

                gvGroupByTransaction.PageIndex = 0
                gvGroupByTransaction.Columns(10).Visible = IsRMBAvailable(hfSchemeCode.Value)
                Me.GridViewDataBind(gvGroupByTransaction, dt, "transNum", "ASC", False)

                mvCore.SetActiveView(vGroupByTransaction)

                udtAuditLogEntry.WriteEndLog(LogID.LOG00020, "Select Bank Account successful", objAuditLogInfo)

            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Select Bank Account fail")
                Throw
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

            Dim lblGAmountClaimedRMB As Label = e.Row.FindControl("lblGAmountClaimedRMB")
            lblGAmountClaimedRMB.Text = formater.formatMoneyRMB(lblGAmountClaimedRMB.Text, False)

        End If
    End Sub

    Protected Sub gvGroupByBankAccount_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, "AuthorizationTxnListByBank")
    End Sub

    Protected Sub gvGroupByBankAccount_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, "AuthorizationTxnListByBank")
    End Sub

    Protected Sub gvGroupByBankAccount_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, "AuthorizationTxnListByBank")
    End Sub

    Protected Sub ibtnBBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00032, "Back to Summary By SPID Click")
        ' CRE11-021 log the missed essential information [End]

        mvCore.SetActiveView(vGroupBySP)

    End Sub

    '

    Protected Sub gvGroupByTransaction_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

            Try
                Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)

                Dim dt As New DataTable
                dt = Session("AuthorizationTxnList")

                criteria = Session("Criteria")
                criteria.TransNum = formater.formatSystemNumberReverse(CType(row.Cells(1).FindControl("lbtn_transNum"), LinkButton).Text)
                Session("Criteria") = criteria

                Dim udtEHSTransactionModel As EHSTransactionModel = (New EHSTransactionBLL).LoadClaimTran(criteria.TransNum)
                Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransactionModel)

                udtAuditLogEntry.AddDescripton("Transaction No", criteria.TransNum)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00022, "Select Transaction", objAuditLogInfo)

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Me.ClaimTransDetail1.ShowHKICSymbol = True
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                Me.ClaimTransDetail1.LoadTranInfo(udtEHSTransactionModel, (New SearchEngineBLL).SearchSuspendHistory(criteria))
                Me.lbl_recordNo.Text = gvGroupByTransaction.PageIndex * gvGroupByTransaction.PageSize + row.RowIndex + 1
                Me.lbl_recordMax.Text = dt.Rows.Count

                mvCore.SetActiveView(vTransactionDetail)

                udtAuditLogEntry.WriteEndLog(LogID.LOG00023, "Select Transaction successful", objAuditLogInfo)
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "Select Transaction fail")
                Throw
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

            Dim lblGAmountClaimedRMB As Label = e.Row.FindControl("lblGAmountClaimedRMB")
            lblGAmountClaimedRMB.Text = formater.formatMoneyRMB(lblGAmountClaimedRMB.Text, False)

        End If
    End Sub

    Protected Sub gvGroupByTransaction_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, "AuthorizationTxnList")
    End Sub

    Protected Sub gvGroupByTransaction_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, "AuthorizationTxnList")
    End Sub

    Protected Sub gvGroupByTransaction_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, "AuthorizationTxnList")
    End Sub

    Protected Sub ibtnTBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00033, "Back to Summary By Bank Click")
        ' CRE11-021 log the missed essential information [End]

        mvCore.SetActiveView(vGroupByBankAccount)

    End Sub

    '

    Protected Sub ibtnDNextRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        If CInt(Me.lbl_recordNo.Text) < CInt(Me.lbl_recordMax.Text) Then
            Dim intActualIndex As Integer

            Dim ee As New System.Web.UI.WebControls.GridViewPageEventArgs(udtReimbursementBLL.GetPageIndexInRecordNavigation(gvGroupByTransaction.PageSize, CInt(Me.lbl_recordNo.Text) + 1))

            Me.GridViewPageIndexChangingHandler(gvGroupByTransaction, ee, "AuthorizationTxnList")

            Dim criteria As New Common.SearchCriteria.SearchCriteria
            Dim dt As New DataTable
            dt = Session("AuthorizationTxnList")

            intActualIndex = CInt(CType(gvGroupByTransaction.Rows(CInt(Me.lbl_recordNo.Text) - gvGroupByTransaction.PageSize * gvGroupByTransaction.PageIndex).Cells(0).FindControl("lblRecordNum"), Label).Text.Trim) - 1
            criteria.TransNum = formater.formatSystemNumberReverse(dt.Rows(intActualIndex)(2).ToString)

            udtAuditLogEntry.AddDescripton("Transaction No", criteria.TransNum)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00025, "Next record")

            Try
                Dim udtEHSTransactionBLL As New EHSTransactionBLL
                Dim udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(criteria.TransNum)
                Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransactionModel)

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Me.ClaimTransDetail1.ShowHKICSymbol = True
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                Me.ClaimTransDetail1.LoadTranInfo(udtEHSTransactionBLL.LoadClaimTran(criteria.TransNum), (New SearchEngineBLL).SearchSuspendHistory(criteria))
                Me.lbl_recordNo.Text = CInt(Me.lbl_recordNo.Text) + 1
                udtAuditLogEntry.WriteEndLog(LogID.LOG00026, "Next record successful", objAuditLogInfo)
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00027, "Next record fail")
                Throw ex
            End Try
        End If

    End Sub

    Protected Sub ibtnDPreviousRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        If CInt(Me.lbl_recordNo.Text) > 1 Then
            Dim intActualIndex As Integer

            Dim ee As New System.Web.UI.WebControls.GridViewPageEventArgs(udtReimbursementBLL.GetPageIndexInRecordNavigation(gvGroupByTransaction.PageSize, CInt(Me.lbl_recordNo.Text) - 1))

            Me.GridViewPageIndexChangingHandler(gvGroupByTransaction, ee, "AuthorizationTxnList")

            Dim criteria As New Common.SearchCriteria.SearchCriteria
            Dim dt As New DataTable
            dt = Session("AuthorizationTxnList")
            intActualIndex = CInt(CType(gvGroupByTransaction.Rows(CInt(Me.lbl_recordNo.Text) - (gvGroupByTransaction.PageSize * gvGroupByTransaction.PageIndex) - 2).Cells(0).FindControl("lblRecordNum"), Label).Text.Trim) - 1
            criteria.TransNum = formater.formatSystemNumberReverse(dt.Rows(intActualIndex)(2).ToString)

            udtAuditLogEntry.AddDescripton("Transaction No", criteria.TransNum)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00028, "Previous record")

            Try
                Dim searchEngine As New SearchEngineBLL
                Dim udtEHSTransactionBLL As New EHSTransactionBLL
                Dim udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(criteria.TransNum)
                Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransactionModel)

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Me.ClaimTransDetail1.ShowHKICSymbol = True
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                Me.ClaimTransDetail1.LoadTranInfo(udtEHSTransactionBLL.LoadClaimTran(criteria.TransNum), searchEngine.SearchSuspendHistory(criteria))
                Me.lbl_recordNo.Text = CInt(Me.lbl_recordNo.Text) - 1
                udtAuditLogEntry.WriteEndLog(LogID.LOG00029, "Previous record successful", objAuditLogInfo)
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00030, "Previous record fail")
                Throw ex
            End Try

        End If

    End Sub

    Protected Sub ibtnDBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        mvCore.SetActiveView(vGroupByTransaction)
    End Sub

    '

    Protected Sub gvConfirmSecondAuthorization_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' First Authorized Time
            Dim lblFirstAuthorizedTime As Label = e.Row.FindControl("lblFirstAuthorizedTime")
            lblFirstAuthorizedTime.Text = formater.formatDateTime(lblFirstAuthorizedTime.Text.Trim)

            ' Amount Claimed RMB
            Dim strScheme As String = DirectCast(e.Row.FindControl("hfSchemeCode"), HiddenField).Value

            If IsRMBAvailable(strScheme) Then
                Dim lblGAmountClaimedRMB As Label = e.Row.FindControl("lblGAmountClaimedRMB")
                lblGAmountClaimedRMB.Text = formater.formatMoneyRMB(lblGAmountClaimedRMB.Text, False)
            End If

        End If
    End Sub

    Protected Sub ibtnCBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False

        mvCore.SetActiveView(vGroupByScheme)

    End Sub

    Protected Sub ibtnCConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        CType(Session("Criteria"), Common.SearchCriteria.SearchCriteria).FirstAuthorizedBy = lblSCRFirstAuthorizedBy.Text
        CType(Session("Criteria"), Common.SearchCriteria.SearchCriteria).FirstAuthorizedDate = lblSCRFirstAuthorizedTime.Text

        Try
            udtAuditLogEntry.AddDescripton("Reimbursement ID", lblSCRReimbursementID.Text)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Confirm Second Authorization")

            If udtReimbursementBLL.ReimbursementSecondAuthorization((New HCVUUserBLL).GetHCVUUser.UserID, lblSCRReimbursementID.Text, hfSchemeCode.Value.Trim) Then
                ' Message: Second Authorization is successful
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00001")
                udcInfoMessageBox.Visible = True

                mvCore.SetActiveView(vComplete)

                udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Confirm Second Authorization successful")
            Else
                Dim sm As New SystemMessage(FUNCTION_CODE, "E", "00002")
                udcErrorMessage.AddMessage(sm)

                udcErrorMessage.BuildMessageBox(strUpdateFail)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Confirm Second Authorization fail")
                mvCore.SetActiveView(vComplete)

            End If
        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                udcErrorMessage.AddMessage(New SystemMessage("990001", "D", eSQL.Message))
                udcErrorMessage.BuildMessageBox(strUpdateFail)

                udcInfoMessageBox.Visible = False
                mvCore.SetActiveView(vError)
                Return

            Else
                udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Confirm Second Authorization fail")
                Throw eSQL
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Confirm Second Authorization fail")
            Throw ex
        End Try
    End Sub

    '

    Protected Sub ibtnCompleteReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Response.Redirect("reimbursementSecondAuthorization.aspx")
    End Sub

    '

    Protected Sub ibtnErrorBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcErrorMessage.Visible = False
        udcInfoMessageBox.Visible = False

        LoadSummaryByFirstAuthorization()
        mvCore.SetActiveView(vGroupByScheme)

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
