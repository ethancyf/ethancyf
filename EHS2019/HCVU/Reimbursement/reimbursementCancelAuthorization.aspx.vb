Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.HCVUUser
Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel
Imports Common.Format
Imports Common.SearchCriteria
Imports Common.Validation
Imports HCVU.ReimbursementBLL

Partial Public Class reimbursement_void
    Inherits BasePageWithGridView

#Region "Private Class"

    Private Class ViewIndex
        Public Const NoRecord As Integer = 0
        Public Const TransactionSummary As Integer = 1
        Public Const ConfirmCancel As Integer = 2
        Public Const Complete As Integer = 3
        Public Const DrillSPID As Integer = 4
        Public Const DrillBankAccount As Integer = 5
        Public Const DrillTransaction As Integer = 6
        Public Const TransactionDetail As Integer = 7
    End Class

    Private Class DrillLevel
        Public Const None As Integer = 0
        Public Const SPID As Integer = 1
        Public Const BankAccount As Integer = 2
        Public Const Transaction As Integer = 3
    End Class

#End Region

#Region "Fields"

    Private udtEHSTransactionBLL As New EHSTransactionBLL
    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtReimbursementBLL As New ReimbursementBLL
    Private udtSearchEngineBLL As New SearchEngineBLL
    Private udtValidator As New Validator

#End Region

#Region "Session Constants"

    Private Const SESS_ReimCancelAuthTransactionDrillCriteria As String = "ReimCancelAuthTransactionDrillCriteria"
    Private Const SESS_ReimCancelAuthTransactionSummaryDataTable As String = "ReimCancelAuthTransactionSummaryDataTable"
    Private Const SESS_ReimCancelAuthTransactionSummarySelectedRowDataTable As String = "ReimCancelAuthTransactionSummarySelectedRowDataTable"
    Private Const SESS_ReimCancelAuthTransactionDrillSPIDDataTable As String = "ReimCancelAuthTransactionDrillSPIDDataTable"
    Private Const SESS_ReimCancelAuthTransactionDrillBankAccountDataTable As String = "ReimCancelAuthTransactionDrillBankAccountDataTable"
    Private Const SESS_ReimCancelAuthTransactionDrillTransactionDataTable As String = "ReimCancelAuthTransactionDrillTransactionDataTable"

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT010406

            udcErrorBox.Visible = False
            udcInfoBox.Visible = False
            BuildDrillCriteriaReview(DrillLevel.None)

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Reimbursement Cancel Authorization Loaded")

            ' Set the page size of the gridviews
            Dim strParaValue As String = String.Empty
            ' CRE11-007
            Dim intPageSize As Integer = udtGeneralFunction.GetPageSizeHCVU

            gvTransactionSummary.PageSize = intPageSize
            gvDrillSPID.PageSize = intPageSize
            gvDrillBankAccount.PageSize = intPageSize
            gvDrillTransaction.PageSize = intPageSize

            GetTransactionSummary()

            EnableCancelAuthorization(False)

        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.TransactionDetail Then EnablePreviousNextButton()
    End Sub

    Private Sub GetTransactionSummary()
        hfReimID.Value = String.Empty

        ' Get the current Reimbursement ID
        Dim dtReim As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(Nothing, ReimbursementStatus.StartReimbursement, ReimbursementAuthorisationStatus.Active, Nothing)

        If dtReim.Rows.Count = 0 Then
            udcInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoBox.BuildMessageBox()
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information

            MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.NoRecord

        Else
            BuildTransactionSummary(CStr(dtReim.Rows(0)("Reimburse_ID")).Trim)

        End If

    End Sub

    Private Sub BuildTransactionSummary(ByVal strReimID As String)
        Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

        Dim dtSummary As DataTable = udtReimbursementBLL.GetReimbursementCancelAuthorizationTransactionSummary(strReimID, strUserID)

        Session(SESS_ReimCancelAuthTransactionSummaryDataTable) = dtSummary

        If dtSummary.Rows.Count = 0 Then
            udcInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoBox.BuildMessageBox()
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information

            MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.NoRecord

        Else
            GridViewDataBind(gvTransactionSummary, dtSummary, "Display_Code", "ASC", False)

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            gvTransactionSummary.Columns(8).Visible = IsRMBAvailable(dtSummary)
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            hfReimID.Value = strReimID

            MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.TransactionSummary

        End If

    End Sub

    Private Sub EnablePreviousNextButton()
        ibtnDetailPrevious.Enabled = lblCurrentRecordNo.Text <> "1"
        ibtnDetailNext.Enabled = lblCurrentRecordNo.Text <> lblMaxRecordNo.Text
    End Sub

#End Region

#Region "Supporting Functions"

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
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
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

#End Region

    Private Sub BackToCancelAuthorizePage()
        BuildDrillCriteriaReview(DrillLevel.None)
        MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.TransactionSummary
    End Sub

    Private Sub BuildDrillCriteriaReview(ByVal intDrillLevel As Integer, Optional ByVal strReimID As String = Nothing, Optional ByVal udtSearchCriteria As SearchCriteria = Nothing, Optional ByVal strBankAccountNoMasked As String = Nothing)
        Select Case intDrillLevel
            Case DrillLevel.None
                pnlDrillCriteriaReview.Visible = False

            Case DrillLevel.SPID
                pnlDrillCriteriaReview.Visible = True

                If Not IsNothing(udtSearchCriteria) Then
                    'lblRSchemeCode.Text = udtSearchCriteria.SchemeCode
                    lblRFirstAuthDtm.Text = udtSearchCriteria.FirstAuthorizedDate
                    lblRFirstAuthBy.Text = udtSearchCriteria.FirstAuthorizedBy

                    If Not IsNothing(udtSearchCriteria.SecondAuthorizedDate) Then
                        lblRSecondAuthDtm.Text = udtSearchCriteria.SecondAuthorizedDate
                        lblRSecondAuthBy.Text = udtSearchCriteria.SecondAuthorizedBy

                        lblRSecondAuthDtm.Visible = True
                        lblRSecondAuthDtmText.Visible = True
                        lblRSecondAuthBy.Visible = True
                        lblRSecondAuthByText.Visible = True

                    Else
                        lblRSecondAuthDtm.Visible = False
                        lblRSecondAuthDtmText.Visible = False
                        lblRSecondAuthBy.Visible = False
                        lblRSecondAuthByText.Visible = False

                    End If
                End If

                If Not IsNothing(strReimID) Then lblRReimID.Text = strReimID

                lblRSchemeCode.Visible = True
                lblRSchemeCodeText.Visible = True

                lblRReimID.Visible = True
                lblRReimIDText.Visible = True

                lblRFirstAuthDtm.Visible = True
                lblRFirstAuthDtmText.Visible = True

                lblRFirstAuthBy.Visible = True
                lblRFirstAuthByText.Visible = True

                lblRSPID.Visible = False
                lblRSPIDText.Visible = False
                lblRSPName.Visible = False
                lblRSPNameText.Visible = False
                lblRBankAcct.Visible = False
                lblRBankAcctText.Visible = False
                lblRPractice.Visible = False
                lblRPracticeText.Visible = False

            Case DrillLevel.BankAccount
                lblRSchemeCode.Visible = True
                lblRSchemeCodeText.Visible = True
                lblRReimID.Visible = True
                lblRReimIDText.Visible = True
                lblRFirstAuthDtm.Visible = True
                lblRFirstAuthDtmText.Visible = True
                lblRFirstAuthBy.Visible = True
                lblRFirstAuthByText.Visible = True

                If Not IsNothing(udtSearchCriteria) Then
                    lblRSPID.Text = udtSearchCriteria.ServiceProviderID
                    lblRSPName.Text = udtSearchCriteria.ServiceProviderName
                End If

                lblRSPID.Visible = True
                lblRSPIDText.Visible = True

                lblRSPName.Visible = True
                lblRSPNameText.Visible = True

                lblRBankAcct.Visible = False
                lblRBankAcctText.Visible = False
                lblRPractice.Visible = False
                lblRPracticeText.Visible = False

            Case DrillLevel.Transaction
                lblRSchemeCode.Visible = True
                lblRSchemeCodeText.Visible = True
                lblRReimID.Visible = True
                lblRReimIDText.Visible = True
                lblRFirstAuthDtm.Visible = True
                lblRFirstAuthDtmText.Visible = True
                lblRFirstAuthBy.Visible = True
                lblRFirstAuthByText.Visible = True
                lblRSPID.Visible = True
                lblRSPIDText.Visible = True
                lblRSPName.Visible = True
                lblRSPNameText.Visible = True

                If Not IsNothing(strBankAccountNoMasked) Then lblRBankAcct.Text = strBankAccountNoMasked
                If Not IsNothing(udtSearchCriteria) Then lblRPractice.Text = udtSearchCriteria.Practice

                lblRBankAcct.Visible = True
                lblRBankAcctText.Visible = True

                lblRPractice.Visible = True
                lblRPracticeText.Visible = True

        End Select

    End Sub

    Private Sub EnableCancelAuthorization(ByVal blnEnable As Boolean)
        If ibtnCancelAuthorization.Enabled = blnEnable Then Return

        pnlReason.Visible = blnEnable

        ibtnCancelAuthorization.Enabled = blnEnable
        ibtnCancelAuthorization.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(blnEnable, "CancelAuthorizationBtn", "CancelAuthorizationDisableBtn"))
    End Sub

    '

    Protected Sub gvTransactionSummary_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.Header) Then
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & _
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).ClientID & "')")
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gvTransactionSummary, "Select$" + e.Row.RowIndex.ToString(), False))
            e.Row.Style.Add("cursor", "hand")

            ' Authorization Time
            Dim lbtnAuthorisedDtm As LinkButton = e.Row.FindControl("lbtnAuthorisedDtm")
            lbtnAuthorisedDtm.Text = udtFormatter.convertDateTime(lbtnAuthorisedDtm.Text.Trim)
            Dim lblAuthorisedDtm As Label = e.Row.FindControl("lblAuthorisedDtm")
            lblAuthorisedDtm.Text = udtFormatter.convertDateTime(lblAuthorisedDtm.Text.Trim)

            ' Total Amount ($)
            Dim lblTotalAmount As Label = e.Row.FindControl("lblTotalAmount")
            lblTotalAmount.Text = Format(CDbl(lblTotalAmount.Text), "#,###")

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            ' Amount Claimed (RMB)
            Dim lblTotalAmountRMB As Label = e.Row.FindControl("lblTotalAmountRMB")
            Dim strScheme As String = DirectCast(e.Row.FindControl("hfSchemeCode"), HiddenField).Value

            If (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(strScheme).ReimbursementCurrency = EnumReimbursementCurrency.HKDRMB Then
                lblTotalAmountRMB.Text = udtFormatter.formatMoneyRMB(lblTotalAmountRMB.Text, False)
            Else
                lblTotalAmountRMB.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblTotalAmountRMB.Enabled = False
            End If
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            ' Transaction Status
            Dim lblAuthorisedStatus As Label = e.Row.FindControl("lblAuthorisedStatus")
            Status.GetDescriptionFromDBCode(ReimbursementStatus.ClassCode, lblAuthorisedStatus.Text.Trim, lblAuthorisedStatus.Text, String.Empty)

        End If
    End Sub

    Protected Sub gvTransactionSummary_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        gvTransactionSummary.SelectedIndex = -1
        EnableCancelAuthorization(False)

        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT") Or e.CommandName.ToUpper.Equals("SELECT")) Then
            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

            Try
                Dim r As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)

                Dim strSchemeCode As String = CType(r.FindControl("hfSchemeCode"), HiddenField).Value.Trim
                lblRSchemeCode.Text = CType(r.FindControl("lbtnSchemeCode"), LinkButton).Text.Trim
                Dim strReimburseID As String = CType(r.FindControl("lbtnReimID"), LinkButton).Text.Trim
                Dim strAuthDtm As String = CType(r.FindControl("lbtnAuthorisedDtm"), LinkButton).Text.Trim
                Dim strAuthBy As String = CType(r.FindControl("lblAuthorisedBy"), Label).Text.Trim
                Dim strAuthStatus As String = CType(r.FindControl("hfAuthorisedStatus"), HiddenField).Value.Trim

                ' Set the authorised status to hidden field for the usage in the "deeper" drill-down grid views
                hfAuthorisedStatus.Value = strAuthStatus

                ' Get the cutoff date with the Reimbursement ID
                Dim strCutoffDate As String = CStr(udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(strReimburseID, ReimbursementStatus.StartReimbursement, ReimbursementAuthorisationStatus.Active, Nothing).Rows(0)("CutOff_Date")).Trim

                udtAuditLogEntry.AddDescripton("ReimbursementID", strReimburseID)
                udtAuditLogEntry.AddDescripton("Scheme", strSchemeCode)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Select Cancel Authorization")

                Dim udtSearchCriteria As New SearchCriteria
                udtSearchCriteria.CutoffDate = strCutoffDate
                udtSearchCriteria.SchemeCode = strSchemeCode

                If strAuthStatus = ReimbursementStatus.FirstAuthorised Then
                    udtSearchCriteria.FirstAuthorizedDate = strAuthDtm
                    udtSearchCriteria.FirstAuthorizedBy = strAuthBy
                    udtSearchCriteria.SecondAuthorizedDate = Nothing
                    udtSearchCriteria.SecondAuthorizedBy = Nothing

                Else
                    ' Get the First Authorised record
                    Dim dt As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(strReimburseID, ReimbursementStatus.FirstAuthorised, ReimbursementAuthorisationStatus.Active, strSchemeCode)
                    If dt.Rows.Count = 1 Then
                        Dim dr As DataRow = dt.Rows(0)
                        udtSearchCriteria.FirstAuthorizedDate = udtFormatter.convertDateTime(CStr(dr("Authorised_Dtm")).Trim)
                        udtSearchCriteria.FirstAuthorizedBy = CStr(dr("Authorised_By")).Trim
                    End If

                    udtSearchCriteria.SecondAuthorizedDate = strAuthDtm
                    udtSearchCriteria.SecondAuthorizedBy = strAuthBy
                End If

                Session(SESS_ReimCancelAuthTransactionDrillCriteria) = udtSearchCriteria

                BuildDrillCriteriaReview(DrillLevel.SPID, strReimburseID, udtSearchCriteria)

                Dim dtSPID As DataTable = udtReimbursementBLL.GetAuthorizationSummaryBySP(udtSearchCriteria, strAuthStatus, strReimburseID, strSchemeCode, strAuthStatus = ReimbursementStatus.FirstAuthorised)

                Session(SESS_ReimCancelAuthTransactionDrillSPIDDataTable) = dtSPID

                ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                gvDrillSPID.PageIndex = 0
                gvDrillSPID.Columns(6).Visible = IsRMBAvailable(strSchemeCode)
                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
                GridViewDataBind(gvDrillSPID, dtSPID, "spNum", "ASC", False)

                udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Select Cancel Authorization successful")
                MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.DrillSPID

            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Select Cancel Authorization fail")
                Throw ex
            End Try

        End If
    End Sub

    Protected Sub gvTransactionSummary_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_ReimCancelAuthTransactionSummaryDataTable)
    End Sub

    Protected Sub gvTransactionSummary_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If gvTransactionSummary.SelectedIndex = -1 Then
            EnableCancelAuthorization(False)
            Return
        End If

        EnableCancelAuthorization(True)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Try
            Dim r As GridViewRow = gvTransactionSummary.SelectedRow
            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            Dim strReimbursementID As String = CType(r.FindControl("lbtnReimID"), LinkButton).Text.Trim
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
            Dim strScheme As String = CType(r.FindControl("hfSchemeCode"), HiddenField).Value.Trim

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            udtAuditLogEntry.AddDescripton("ReimbursementID", strReimbursementID)
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
            udtAuditLogEntry.AddDescripton("Scheme", strScheme)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Highlight Cancel Authorization")

            Dim dtSummary As DataTable = Session(SESS_ReimCancelAuthTransactionSummaryDataTable)
            Dim dtSelected As DataTable = dtSummary.Clone

            For Each drSummary As DataRow In dtSummary.Select("Scheme_Code = '" + strScheme + "'")
                dtSelected.ImportRow(drSummary)
            Next

            dtSelected.Columns.Add("Index")
            dtSelected.Columns.Add("AuthorisedStatusReal")

            For Each drConfirm As DataRow In dtSelected.Rows
                drConfirm("Index") = CType(r.FindControl("lblIndex"), Label).Text
                drConfirm("AuthorisedStatusReal") = CType(r.FindControl("hfAuthorisedStatus"), HiddenField).Value
            Next

            Session(SESS_ReimCancelAuthTransactionSummarySelectedRowDataTable) = dtSelected

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            ' Determine whether cancelling this scheme will cause the reimbursement to complete
            If IsFinalScheme(strReimbursementID, strScheme) Then
                If (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(strScheme).ReimbursementMode = EnumReimbursementMode.All Then
                    udcErrorBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003), "%s", strReimbursementID)
                Else
                    udcErrorBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004), "%s", strReimbursementID)
                End If

                udcErrorBox.BuildMessageBox("Note")

            End If

            gvConfirmCancel.Columns(8).Visible = IsRMBAvailable(dtSelected)
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            gvConfirmCancel.DataSource = dtSelected
            gvConfirmCancel.DataBind()

            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Highlight Cancel Authorization successful")

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Highlight Cancel Authorization fail")
            Throw ex
        End Try

    End Sub

    Protected Sub gvTransactionSummary_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_ReimCancelAuthTransactionSummaryDataTable)
    End Sub

    Protected Sub gvTransactionSummary_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS_ReimCancelAuthTransactionSummaryDataTable)
    End Sub

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    Private Function IsFinalScheme(ByVal strReimbursementID As String, ByVal strSchemeCode As String) As Boolean
        Dim udtReimbursementBLL As New ReimbursementBLL

        Dim dt As ReimbursementDataTable = udtReimbursementBLL.GetReimbursementProgress(strReimbursementID)

        Dim dtPaymentFile As ReimbursementDataTable = dt.FilterByReimbursementMode(EnumReimbursementMode.All)
        Dim dtNoPaymentFile As ReimbursementDataTable = dt.FilterByReimbursementMode(EnumReimbursementMode.FirstAuthAndSecondAuth)

        ' If neither section has completed the reimbursement, no need to perform further checking
        If dtPaymentFile.AllSchemeIsReimbursed = False AndAlso dtNoPaymentFile.AllSchemeIsReimbursed = False Then
            Return False
        End If

        If (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(strSchemeCode).ReimbursementMode = EnumReimbursementMode.All Then
            If dtPaymentFile.HoldSchemeCount = 1 _
                    AndAlso Not IsDBNull(dtPaymentFile.FindByScheme(strSchemeCode)("Hold_By")) _
                    AndAlso dtNoPaymentFile.AllSchemeIsReimbursed Then
                Return True
            End If

        Else
            If dtNoPaymentFile.HoldSchemeCount = 1 _
                    AndAlso Not IsDBNull(dtNoPaymentFile.FindByScheme(strSchemeCode)("Hold_By")) _
                    AndAlso dtPaymentFile.AllSchemeIsReimbursed Then
                Return True
            End If

        End If

        Return False

    End Function
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

    '

    Protected Sub ibtnCancelAuthorization_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcErrorBox.Visible = False
        imgAlertReason.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Try
            udtAuditLogEntry.AddDescripton("ReimbursementID", hfReimID.Value)
            udtAuditLogEntry.AddDescripton("CancelReason", txtReason.Text)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Cancel Authorization Click")

            If udtValidator.IsEmpty(txtReason.Text.Trim) Then
                imgAlertReason.Visible = True

                udcErrorBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
                udcErrorBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00006, "Cancel Authorization Click fail")

            Else
                lblConfirmReason.Text = txtReason.Text.Trim

                udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
                udcInfoBox.BuildMessageBox()
                udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information

                MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.ConfirmCancel

                udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Cancel Authorization Click successful")
            End If

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Cancel Authorization Click fail")
            Throw ex
        End Try

    End Sub

    '

    Protected Sub gvConfirmCancel_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Authorization Time
            Dim lblAuthorisedDtm As Label = e.Row.FindControl("lblAuthorisedDtm")
            lblAuthorisedDtm.Text = udtFormatter.convertDateTime(lblAuthorisedDtm.Text.Trim)

            ' Total Amount ($)
            Dim lblTotalAmount As Label = e.Row.FindControl("lblTotalAmount")
            lblTotalAmount.Text = Format(CDbl(lblTotalAmount.Text), "#,###")

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            ' Amount Claimed (RMB)
            Dim lblTotalAmountRMB As Label = e.Row.FindControl("lblTotalAmountRMB")
            lblTotalAmountRMB.Text = udtFormatter.formatMoneyRMB(lblTotalAmountRMB.Text, False)
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            ' Transaction Status
            Dim lblAuthorisedStatus As Label = e.Row.FindControl("lblAuthorisedStatus")
            Status.GetDescriptionFromDBCode(ReimbursementStatus.ClassCode, lblAuthorisedStatus.Text.Trim, lblAuthorisedStatus.Text, String.Empty)

        End If
    End Sub

    Protected Sub ibtnReasonBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim dtSelected As DataTable = Session(SESS_ReimCancelAuthTransactionSummarySelectedRowDataTable)
        Dim drSelected As DataRow = dtSelected.Rows(0)

        Dim strReimID As String = CStr(drSelected("Reimburse_ID")).Trim
        Dim strScheme As String = CStr(drSelected("Scheme_Code")).Trim

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReimbursementID", strReimID)
        udtAuditLogEntry.AddDescripton("Scheme", strScheme)
        udtAuditLogEntry.WriteLog(LogID.LOG00036, "Cancel Authorization - Back Click")
        ' CRE11-021 log the missed essential information [End]

        udcErrorBox.Visible = False
        udcInfoBox.Visible = False

        MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.TransactionSummary
    End Sub

    Protected Sub ibtnReasonConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Try
            Dim dtSelected As DataTable = Session(SESS_ReimCancelAuthTransactionSummarySelectedRowDataTable)
            Dim drSelected As DataRow = dtSelected.Rows(0)

            Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
            Dim strVoidRemark As String = txtReason.Text.Trim
            Dim strReimID As String = CStr(drSelected("Reimburse_ID")).Trim
            Dim strScheme As String = CStr(drSelected("Scheme_Code")).Trim

            udtAuditLogEntry.AddDescripton("ReimbursementID", strReimID)
            udtAuditLogEntry.AddDescripton("Scheme", strScheme)
            udtAuditLogEntry.AddDescripton("CancelReason", txtReason.Text)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Confirm Cancel Authorization")

            udtReimbursementBLL.VoidAuthorization(strUserID, strVoidRemark, strReimID, strScheme)

            udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoBox.BuildMessageBox()

            udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Confirm Cancel Authorization successful")
            MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.Complete

        Catch ex As Exception
            udcErrorBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
            udcErrorBox.BuildMessageBox("ValidationFail")

            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Confirm Cancel Authorization fail")
            MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.Complete

        End Try

    End Sub

    '

    Protected Sub gvDrillSPID_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Amount Claimed (RMB)
            Dim lblTotalAmountRMB As Label = e.Row.FindControl("lblTotalAmountRMB")
            lblTotalAmountRMB.Text = udtFormatter.formatMoneyRMB(lblTotalAmountRMB.Text, False)

        End If
    End Sub

    Protected Sub gvDrillSPID_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Try
                Dim r As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)

                Dim strSPID As String = CType(r.FindControl("lbtn_spID"), LinkButton).Text.Trim
                Dim strSPName As String = r.Cells(3).Text.Trim

                ' CRE11-004
                Dim objAuditLogInfo As New AuditLogInfo(Left(strSPID, 8), Nothing, Nothing, Nothing, Nothing, Nothing)
                ' End CRE11-004

                udtAuditLogEntry.AddDescripton("ReimbursementID", lblRReimID.Text)
                udtAuditLogEntry.AddDescripton("SPID", strSPID)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00022, "Select SPID", objAuditLogInfo)

                Dim udtSearchCriteria As SearchCriteria = Session(SESS_ReimCancelAuthTransactionDrillCriteria)
                udtSearchCriteria.ServiceProviderID = strSPID
                udtSearchCriteria.ServiceProviderName = strSPName
                udtSearchCriteria.SPPracticeDisplaySeq = udtReimbursementBLL.ExtractPracticeDisplaySeqFromSPIDPracticeDisplaySeq(strSPID)
                Session(SESS_ReimCancelAuthTransactionDrillCriteria) = udtSearchCriteria

                BuildDrillCriteriaReview(DrillLevel.BankAccount, Nothing, udtSearchCriteria)

                Dim dt As DataTable = udtReimbursementBLL.GetAuthorizationSummaryByBankAcct(udtSearchCriteria, hfAuthorisedStatus.Value, udtSearchCriteria.SchemeCode, hfAuthorisedStatus.Value = ReimbursementStatus.FirstAuthorised)

                Session(SESS_ReimCancelAuthTransactionDrillBankAccountDataTable) = dt

                ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                gvDrillBankAccount.PageIndex = 0
                gvDrillBankAccount.Columns(6).Visible = IsRMBAvailable(udtSearchCriteria.SchemeCode)
                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

                GridViewDataBind(gvDrillBankAccount, dt, "bankAccount", "ASC", False)

                udtAuditLogEntry.WriteEndLog(LogID.LOG00023, "Select SPID successful", objAuditLogInfo)

                MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.DrillBankAccount

            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "Select SPID fail")
                Throw ex
            End Try

        End If
    End Sub

    Protected Sub gvDrillSPID_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_ReimCancelAuthTransactionDrillSPIDDataTable)
    End Sub

    Protected Sub gvDrillSPID_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_ReimCancelAuthTransactionDrillSPIDDataTable)
    End Sub

    Protected Sub gvDrillSPID_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS_ReimCancelAuthTransactionDrillSPIDDataTable)
    End Sub

    Protected Sub ibtnSPBackToAuth_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReimbursementID", lblRReimID.Text)
        udtAuditLogEntry.AddDescripton("Scheme", lblRSchemeCode.Text)
        udtAuditLogEntry.WriteLog(LogID.LOG00028, "SPID Back to Authorization Click")
        ' CRE11-021 log the missed essential information [End]

        BackToCancelAuthorizePage()
    End Sub

    Protected Sub ibtnSPBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        BuildDrillCriteriaReview(DrillLevel.None)
        MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.TransactionSummary
    End Sub

    '

    Protected Sub gvDrillBankAccount_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ctrlBankAcc As LinkButton
            Dim ctrlOriBank As Label
            ctrlBankAcc = CType(e.Row.Cells(1).FindControl("lbtn_bankAccNo"), LinkButton)
            ctrlOriBank = CType(e.Row.Cells(1).FindControl("lblOriBank"), Label)

            ctrlBankAcc.Text = udtFormatter.maskBankAccount(ctrlOriBank.Text)

            ' Amount Claimed (RMB)
            Dim lblTotalAmountRMB As Label = e.Row.FindControl("lblTotalAmountRMB")
            lblTotalAmountRMB.Text = udtFormatter.formatMoneyRMB(lblTotalAmountRMB.Text, False)

        End If
    End Sub

    Protected Sub gvDrillBankAccount_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then

            ' CRE11-004
            Dim objAuditLogInfo As New AuditLogInfo(Left(lblRSPID.Text, 8), Nothing, Nothing, Nothing, Nothing, Nothing)
            ' End CRE11-004

            Try
                Dim r As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)

                Dim strBankAccountNo As String = CType(r.FindControl("lbtn_bankAccNo"), LinkButton).Text.Trim
                Dim strPractice As String = r.Cells(3).Text

                Dim udtSearchCriteria As SearchCriteria = Session(SESS_ReimCancelAuthTransactionDrillCriteria)
                udtSearchCriteria.BankAcctNo = CType(r.Cells(1).FindControl("lblOriBank"), Label).Text
                udtSearchCriteria.Practice = strPractice
                udtSearchCriteria.SPPracticeDisplaySeq = udtReimbursementBLL.ExtractPracticeDisplaySeqFromSPIDPracticeDisplaySeq(lblRSPID.Text.Trim)
                Session(SESS_ReimCancelAuthTransactionDrillCriteria) = udtSearchCriteria

                BuildDrillCriteriaReview(DrillLevel.Transaction, Nothing, udtSearchCriteria, strBankAccountNo)


                udtAuditLogEntry.AddDescripton("ReimbursementID", lblRReimID.Text.Trim)
                udtAuditLogEntry.AddDescripton("SPID", lblRSPID.Text)
                udtAuditLogEntry.AddDescripton("BankAccountNo", udtSearchCriteria.BankAcctNo)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00025, "Select Bank Account", objAuditLogInfo)

                Dim dtTrans As DataTable = udtReimbursementBLL.GetAuthorizationSummaryByTxn(udtSearchCriteria, hfAuthorisedStatus.Value, udtSearchCriteria.SchemeCode, hfAuthorisedStatus.Value = ReimbursementStatus.FirstAuthorised)

                ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                gvDrillTransaction.PageIndex = 0
                gvDrillTransaction.Columns(10).Visible = IsRMBAvailable(udtSearchCriteria.SchemeCode)
                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

                GridViewDataBind(gvDrillTransaction, dtTrans, "transNum", "ASC", False)

                Session(SESS_ReimCancelAuthTransactionDrillTransactionDataTable) = dtTrans

                udtAuditLogEntry.WriteEndLog(LogID.LOG00026, "Select Bank Account successful", objAuditLogInfo)

                MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.DrillTransaction

            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00027, "Select Bank Account fail", objAuditLogInfo)
                Throw ex
            End Try

        End If
    End Sub

    Protected Sub gvDrillBankAccount_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_ReimCancelAuthTransactionDrillBankAccountDataTable)
    End Sub

    Protected Sub gvDrillBankAccount_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_ReimCancelAuthTransactionDrillBankAccountDataTable)
    End Sub

    Protected Sub gvDrillBankAccount_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS_ReimCancelAuthTransactionDrillBankAccountDataTable)
    End Sub

    Protected Sub ibtnBankBackToAuth_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReimbursementID", lblRReimID.Text)
        udtAuditLogEntry.AddDescripton("Scheme", lblRSchemeCode.Text)
        udtAuditLogEntry.WriteLog(LogID.LOG00030, "Bank Back to Authorization Click")
        ' CRE11-021 log the missed essential information [End]

        BackToCancelAuthorizePage()
    End Sub

    Protected Sub ibtnBankBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReimbursementID", lblRReimID.Text)
        udtAuditLogEntry.AddDescripton("Scheme", lblRSchemeCode.Text)
        udtAuditLogEntry.WriteLog(LogID.LOG00031, "Bank Back Click")
        ' CRE11-021 log the missed essential information [End]

        BuildDrillCriteriaReview(DrillLevel.SPID)
        MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.DrillSPID
    End Sub

    '

    Protected Sub gvDrillTransaction_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If (e.Row.RowType = DataControlRowType.Header) Then

            'adding an attribute for onclick event on the check box in the header
            'and passing the ClientID of the Select All checkbox

            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & _
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).ClientID & "')")

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ctrlTransNum As LinkButton
            ctrlTransNum = CType(e.Row.Cells(2).FindControl("lbtn_transNum"), LinkButton)
            ctrlTransNum.Text = udtFormatter.formatSystemNumber(ctrlTransNum.Text)

            ' Transaction Time
            Dim lblTransactionTime As Label = e.Row.FindControl("lblTransactionTime")
            lblTransactionTime.Text = udtFormatter.formatDateTime(lblTransactionTime.Text.Trim)

            ' Amount Claimed (RMB)
            Dim lblTotalAmountRMB As Label = e.Row.FindControl("lblTotalAmountRMB")
            lblTotalAmountRMB.Text = udtFormatter.formatMoneyRMB(lblTotalAmountRMB.Text, False)

        End If
    End Sub

    Protected Sub gvDrillTransaction_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Try
                Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)

                Dim dt As New DataTable
                dt = Session(SESS_ReimCancelAuthTransactionDrillTransactionDataTable)

                Dim udtSearchCriteria As SearchCriteria = Session(SESS_ReimCancelAuthTransactionDrillCriteria)
                udtSearchCriteria.TransNum = udtFormatter.formatSystemNumberReverse(CType(row.Cells(1).FindControl("lbtn_transNum"), LinkButton).Text)
                Session(SESS_ReimCancelAuthTransactionDrillCriteria) = udtSearchCriteria

                Dim udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(udtSearchCriteria.TransNum)
                Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransactionModel)

                udtAuditLogEntry.AddDescripton("Transaction No", udtSearchCriteria.TransNum)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00013, "Select Transaction", objAuditLogInfo)

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                udcClaimTransDetail.ShowHKICSymbol = True
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]
                udcClaimTransDetail.LoadTranInfo(udtEHSTransactionModel, udtSearchEngineBLL.SearchSuspendHistory(udtSearchCriteria))

                lblCurrentRecordNo.Text = gvDrillTransaction.PageIndex * gvDrillTransaction.PageSize + row.RowIndex + 1
                lblMaxRecordNo.Text = dt.Rows.Count

                pnlDrillCriteriaReview.Visible = False

                udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Select Transaction successful", objAuditLogInfo)

                MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.TransactionDetail

            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Select Transaction fail")
                Throw ex
            End Try

        End If
    End Sub

    Protected Sub gvDrillTransaction_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS_ReimCancelAuthTransactionDrillTransactionDataTable)
    End Sub

    Protected Sub gvDrillTransaction_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_ReimCancelAuthTransactionDrillTransactionDataTable)
    End Sub

    Protected Sub gvDrillTransaction_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_ReimCancelAuthTransactionDrillTransactionDataTable)
    End Sub

    Protected Sub ibtnTransBackToAuth_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtSearchCriteria As SearchCriteria = Session(SESS_ReimCancelAuthTransactionDrillCriteria)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReimbursementID", lblRReimID.Text)
        udtAuditLogEntry.AddDescripton("Scheme", lblRSchemeCode.Text)
        udtAuditLogEntry.AddDescripton("BankAccountNo", Me.lblRBankAcct.Text)
        udtAuditLogEntry.AddDescripton("TransactionID", udtSearchCriteria.TransNum)
        udtAuditLogEntry.WriteLog(LogID.LOG00034, "Transaction Back to Authorization Click")
        ' CRE11-021 log the missed essential information [End]

        BackToCancelAuthorizePage()
    End Sub

    Protected Sub ibtnTransBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtSearchCriteria As SearchCriteria = Session(SESS_ReimCancelAuthTransactionDrillCriteria)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReimbursementID", lblRReimID.Text)
        udtAuditLogEntry.AddDescripton("Scheme", lblRSchemeCode.Text)
        udtAuditLogEntry.AddDescripton("BankAccountNo", Me.lblRBankAcct.Text)
        udtAuditLogEntry.AddDescripton("TransactionID", udtSearchCriteria.TransNum)
        udtAuditLogEntry.WriteLog(LogID.LOG00035, "Transaction Back Click")
        ' CRE11-021 log the missed essential information [End]

        BuildDrillCriteriaReview(DrillLevel.BankAccount)
        MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.DrillBankAccount
    End Sub

    '

    Protected Sub ibtnDetailBackToAuth_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReimbursementID", lblRReimID.Text)
        udtAuditLogEntry.AddDescripton("Scheme", lblRSchemeCode.Text)
        udtAuditLogEntry.AddDescripton("BankAccountNo", Me.lblRBankAcct.Text)
        udtAuditLogEntry.WriteLog(LogID.LOG00032, "Detail Back to Authorization Click")
        ' CRE11-021 log the missed essential information [End]

        BackToCancelAuthorizePage()
    End Sub

    Protected Sub ibtnDetailBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("ReimbursementID", lblRReimID.Text)
        udtAuditLogEntry.AddDescripton("Scheme", lblRSchemeCode.Text)
        udtAuditLogEntry.AddDescripton("BankAccountNo", Me.lblRBankAcct.Text)
        udtAuditLogEntry.WriteLog(LogID.LOG00033, "Detail Back Click")
        ' CRE11-021 log the missed essential information [End]

        pnlDrillCriteriaReview.Visible = True
        MultiViewReimCancelAuthorization.ActiveViewIndex = ViewIndex.DrillTransaction
    End Sub

    Protected Sub ibtnDetailNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If CInt(lblCurrentRecordNo.Text) >= CInt(lblMaxRecordNo.Text) Then Return

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim ee As New System.Web.UI.WebControls.GridViewPageEventArgs(udtReimbursementBLL.GetPageIndexInRecordNavigation(gvDrillTransaction.PageSize, CInt(lblCurrentRecordNo.Text) + 1))

        GridViewPageIndexChangingHandler(gvDrillTransaction, ee, SESS_ReimCancelAuthTransactionDrillTransactionDataTable)

        Dim dt As DataTable = Session(SESS_ReimCancelAuthTransactionDrillTransactionDataTable)
        Dim intActualIndex As Integer = CInt(CType(gvDrillTransaction.Rows(CInt(lblCurrentRecordNo.Text) - gvDrillTransaction.PageSize * gvDrillTransaction.PageIndex).Cells(0).FindControl("lblRecordNum"), Label).Text.Trim) - 1
        Dim strTransNum As String = udtFormatter.formatSystemNumberReverse(dt.Rows(intActualIndex)(2).ToString)

        udtAuditLogEntry.AddDescripton("Transaction No", strTransNum)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00019, "Next record")

        Try
            Dim udtSearchCriteria As New SearchCriteria
            udtSearchCriteria.TransNum = strTransNum

            Dim udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(strTransNum)
            Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransactionModel)

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            udcClaimTransDetail.ShowHKICSymbol = True
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]
            udcClaimTransDetail.LoadTranInfo(udtEHSTransactionModel, udtSearchEngineBLL.SearchSuspendHistory(udtSearchCriteria))

            lblCurrentRecordNo.Text = CInt(lblCurrentRecordNo.Text) + 1

            udtAuditLogEntry.WriteEndLog(LogID.LOG00020, "Next record successful", objAuditLogInfo)

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Next record fail")
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnDetailPrevious_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If CInt(lblCurrentRecordNo.Text) = 1 Then Return

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Dim ee As New System.Web.UI.WebControls.GridViewPageEventArgs(udtReimbursementBLL.GetPageIndexInRecordNavigation(gvDrillTransaction.PageSize, CInt(lblCurrentRecordNo.Text) - 1))

        GridViewPageIndexChangingHandler(gvDrillTransaction, ee, SESS_ReimCancelAuthTransactionDrillTransactionDataTable)

        Dim dt As DataTable = Session(SESS_ReimCancelAuthTransactionDrillTransactionDataTable)
        Dim intActualIndex As Integer = CInt(CType(gvDrillTransaction.Rows(CInt(lblCurrentRecordNo.Text) - (gvDrillTransaction.PageSize * gvDrillTransaction.PageIndex) - 2).Cells(0).FindControl("lblRecordNum"), Label).Text.Trim) - 1
        Dim strTransNum As String = udtFormatter.formatSystemNumberReverse(dt.Rows(intActualIndex)(2).ToString)

        udtAuditLogEntry.AddDescripton("Transaction No", strTransNum)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "Previous record")

        Try
            Dim udtSearchCriteria As New SearchCriteria
            udtSearchCriteria.TransNum = strTransNum

            Dim udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(strTransNum)
            Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransactionModel)

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            udcClaimTransDetail.ShowHKICSymbol = True
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]
            udcClaimTransDetail.LoadTranInfo(udtEHSTransactionModel, udtSearchEngineBLL.SearchSuspendHistory(udtSearchCriteria))

            lblCurrentRecordNo.Text = CInt(lblCurrentRecordNo.Text) - 1

            udtAuditLogEntry.WriteEndLog(LogID.LOG00017, "Previous record successful", objAuditLogInfo)

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Previous record fail")
            Throw ex
       
        End Try
    End Sub

    '

    Protected Sub ibtnReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Response.Redirect("reimbursementCancelAuthorization.aspx")
    End Sub

    ''' <summary>
    ''' CRE11-004
    ''' Handle working data on view change, clear working data if no use
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MultiViewReimCancelAuthorization_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MultiViewReimCancelAuthorization.ActiveViewChanged

        Select Case MultiViewReimCancelAuthorization.ActiveViewIndex
            Case ViewIndex.TransactionDetail
                ' Do Nothing (Keep working data)
            Case Else
                Me.ClearWorkingData()
        End Select
    End Sub
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
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
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
   
End Class