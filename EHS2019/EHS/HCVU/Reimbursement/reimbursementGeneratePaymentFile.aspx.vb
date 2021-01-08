Imports Common.ComObject
Imports Common.Component
Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel
Imports Common.Format
Imports Common.Validation
Imports CustomControls
Imports HCVU.ReimbursementBLL
Imports Common.Component.HCVUUser
Imports Common.Component.UserRole

Partial Public Class reimbursementGeneratePaymentFile
    Inherits BasePageWithGridView

    ' FunctionCode = FunctCode.FUNT010408

#Region "Private Classes"

    Private Class AuditLogDescription
        Public Const Load As String = "Reimbursement Generate Payment File load" '00000
        Public Const GetAuthorizationSummaryStart As String = "Get Authorization Summary start"
        Public Const GetAuthorizationSummaryNoRecordFound As String = "Get Authorization Summary complete. No record found"
        Public Const GetAuthorizationSummarySuccessful As String = "Get Authorization Summary successful"
        Public Const GenerateClick As String = "Generate click"
        Public Const GenerateFail As String = "Generate fail" '00005
        Public Const GenerateSuccessful As String = "Generate successful"
        Public Const GenerateBackClick As String = "Generate Back click"
        Public Const GenerateConfirmClick As String = "Generate Confirm click"
        Public Const GenerateConfirmSuccessful As String = "Generate Confirm successful"
        Public Const ReturnClick As String = "Return click" '00010
    End Class

    Private Class ErrorMessageBoxHeaderKey
        Public Const ValidationFail As String = "ValidationFail"
    End Class

#End Region

#Region "Session Constants"

    Private Const SESS_RAuthorizationSummaryDataTable As String = "010408_RAuthorizationSummaryDataTable"
    Private Const SESS_NAuthorizationSummaryDataTable As String = "010408_NAuthorizationSummaryDataTable"
    Private Const SESS_NHAAuthorizationSummaryDataTable As String = "010408_NHAAuthorizationSummaryDataTable" ' CRE20-015 (Special Support Scheme) [Winnie]

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT010408

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditLogDescription.Load)

            GetAuthorizationSummary()
        End If

    End Sub

    Private Sub GetAuthorizationSummary()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, AuditLogDescription.GetAuthorizationSummaryStart)

        ' Get the latest Reimbursement ID with [Authorised_Status] = 'S' and [Record_Status] = 'A'
        Dim udtReimbursementBLL As New ReimbursementBLL
        Dim dtReimID As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(Nothing, ReimbursementStatus.StartReimbursement, ReimbursementAuthorisationStatus.Active, Nothing)

        If dtReimID.Rows.Count = 0 Then
            ' Message: No records found.
            udcInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoBox.BuildMessageBox()
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information

            udtAuditLogEntry.AddDescripton("StackTrace", "Could not find active Reimbursement ID")
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.GetAuthorizationSummaryNoRecordFound)

            mvCore.SetActiveView(vNoRecord)

            Return
        End If

        Dim strReimID As String = CStr(dtReimID.Rows(0)("Reimburse_ID")).Trim

        udtAuditLogEntry.AddDescripton("Reimbursement ID", strReimID)

        ' Use this Reimbursement ID to get records from [ReimbursementAuthorisation]
        Dim dtReimAuth As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(strReimID, Nothing, ReimbursementAuthorisationStatus.Active, Nothing)

        ' If there is only 1 row, that means the data table contains only the [Authorised_Status] = 'S' record
        If dtReimAuth.Rows.Count = 1 Then
            ' Message: No records found.
            udcInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoBox.BuildMessageBox()
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information

            udtAuditLogEntry.AddDescripton("StackTrace", "No authorizations have been held for this Reimbursement ID")
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.GetAuthorizationSummaryNoRecordFound)

            mvCore.SetActiveView(vNoRecord)

            Return
        End If

        ' Flatten the table
        Dim dtResult As New ReimbursementDataTable(dtReimAuth)

        ' If all held schemes are reimbursed, the reimbursement is already completed
        If dtResult.AllSchemeIsReimbursed Then
            ' Message: No records found.
            udcInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoBox.BuildMessageBox()
            udcInfoBox.Type = InfoMessageBoxType.Information

            udtAuditLogEntry.AddDescripton("StackTrace", "Current reimbursement is completed")
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.GetAuthorizationSummaryNoRecordFound)

            mvCore.SetActiveView(vNoRecord)

            Return

        End If

        ' Reimbursement ID
        mvCore.SetActiveView(vScheme)
        lblReimID.Text = strReimID


        ' --- Hide Show tab ---
        ' CRE20-015 (Special Support Scheme) [Start][Winnie]
        ' DH User: All tabs
        ' SSSCMC User: Only "No Payment File Required (HA)"
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser()
        Dim blnIsSSSCMCUser As Boolean = udtHCVUUserBLL.IsSSSCMCUser(udtHCVUUser)

        If blnIsSSSCMCUser Then
            tpRequire.Visible = False
            tpNoRequire.Visible = False
            tpNoRequireHA.Visible = True
        Else
            tpRequire.Visible = True
            tpNoRequire.Visible = True
            tpNoRequireHA.Visible = True
        End If
        ' CRE20-015 (Special Support Scheme) [End][Winnie]

        ' --- Payment File Required ---
        Dim dtR As ReimbursementDataTable = dtResult.FilterByReimbursementMode(EnumReimbursementMode.All)

        If dtR.AtLeastOneSchemeHold = False Then
            ' Message: No records found.
            udcRInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcRInfoBox.BuildMessageBox()
            udcRInfoBox.Type = CustomControls.InfoMessageBoxType.Information

            udtAuditLogEntry.AddDescripton("PaymentFileRequired", "No schemes have started reimbursement")

            mvR.SetActiveView(vRNoRecord)

        Else
            mvR.SetActiveView(vRContent)
            mvRPaymentDate.SetActiveView(vRPEnter)
            udcRInfoBox.Visible = False
            udcRErrorBox.Visible = False

            GridViewDataBind(gvR, dtR, "Display_Seq", "Asc", False)
            Session(SESS_RAuthorizationSummaryDataTable) = dtR

            If dtR.AllSchemeIsReimbursed Then
                ' Message: The payment file has been submitted for generation.
                udcRInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)
                udcRInfoBox.BuildMessageBox()
                udcRInfoBox.Type = InfoMessageBoxType.Information

                txtRPEPaymentDate.Visible = False
                imgRPEPaymentDate.Visible = False
                ibtnRPEPaymentDate.Visible = False
                lblRPEPaymentDate.Visible = True

                Dim strSchemeCode As String = String.Empty

                For Each dr As DataRow In dtR.Rows
                    If Not IsDBNull(dr("Reimbursed")) AndAlso dr("Reimbursed") = "Y" Then
                        strSchemeCode = dr("Scheme_Code")
                        Exit For
                    End If
                Next

                Dim dtmValueDate As DateTime = udtReimbursementBLL.GetBankIn(strReimID, strSchemeCode).Rows(0)("Value_Date")

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'lblRPEPaymentDate.Text = (New Formatter).formatDate(dtmValueDate, String.Empty)
                lblRPEPaymentDate.Text = (New Formatter).formatDisplayDate(dtmValueDate)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


                ibtnRPEGenerate.Enabled = False
                ibtnRPEGenerate.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "GeneratePaymentFileDisableBtn")

                udtAuditLogEntry.AddDescripton("PaymentFileRequired", String.Format("Reimbursement completed with payment date {0}", lblRPEPaymentDate.Text))

            ElseIf dtR.AbleToGenerateBankFile = False Then
                ' Message: The reimbursement process is still in progress. The payment file(s) cannot be generated at the moment.
                udcRInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
                udcRInfoBox.BuildMessageBox()
                udcRInfoBox.Type = InfoMessageBoxType.Information

                txtRPEPaymentDate.Visible = True
                ibtnRPEPaymentDate.Visible = True
                lblRPEPaymentDate.Visible = False
                imgRPEPaymentDate.Visible = False

                txtRPEPaymentDate.Enabled = False
                ibtnRPEPaymentDate.Enabled = False
                ibtnRPEGenerate.Enabled = False
                ibtnRPEGenerate.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "GeneratePaymentFileDisableBtn")

                txtRPEPaymentDate.Text = String.Empty

                udtAuditLogEntry.AddDescripton("PaymentFileRequired", "Reimbursement still in progress")

            Else
                txtRPEPaymentDate.Visible = True
                ibtnRPEPaymentDate.Visible = True
                lblRPEPaymentDate.Visible = False
                imgRPEPaymentDate.Visible = False

                txtRPEPaymentDate.Enabled = True
                ibtnRPEPaymentDate.Enabled = True
                ibtnRPEGenerate.Enabled = True
                ibtnRPEGenerate.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "GeneratePaymentFileBtn")

                txtRPEPaymentDate.Text = String.Empty

                udtAuditLogEntry.AddDescripton("PaymentFileRequired", "Ready to complete reimbursement")

            End If

        End If

        dtR = Nothing

        ' --- No Payment File Required ---
        Dim dtN As ReimbursementDataTable = dtResult.FilterByReimbursementMode(EnumReimbursementMode.FirstAuthAndSecondAuth)

        If dtN.AtLeastOneSchemeHold = False Then
            ' Message: No records found.
            udcNInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcNInfoBox.BuildMessageBox()
            udcNInfoBox.Type = CustomControls.InfoMessageBoxType.Information

            udtAuditLogEntry.AddDescripton("NoPaymentFileRequired", "No schemes have started reimbursement")

            mvN.SetActiveView(vNNoRecord)

        Else
            mvN.SetActiveView(vNContent)
            mvNPaymentDate.SetActiveView(vNPEnter)
            udcNInfoBox.Visible = False
            udcNErrorBox.Visible = False

            GridViewDataBind(gvN, dtN, "Display_Seq", "Asc", False)
            Session(SESS_NAuthorizationSummaryDataTable) = dtN

            If dtN.AllSchemeIsReimbursed Then
                ' Message: The reimbursement process for the following schemes is completed.
                udcNInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006)
                udcNInfoBox.BuildMessageBox()
                udcNInfoBox.Type = InfoMessageBoxType.Information

                txtNPEPaymentDate.Visible = False
                ibtnNPEPaymentDate.Visible = False
                lblNPEPaymentDate.Visible = True
                imgNPEPaymentDate.Visible = False

                Dim strSchemeCode As String = String.Empty

                For Each dr As DataRow In dtN.Rows
                    If Not IsDBNull(dr("Reimbursed")) AndAlso dr("Reimbursed") = "Y" Then
                        strSchemeCode = dr("Scheme_Code")
                        Exit For
                    End If
                Next

                Dim dtmValueDate As DateTime = udtReimbursementBLL.GetBankIn(strReimID, strSchemeCode).Rows(0)("Value_Date")

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'lblNPEPaymentDate.Text = (New Formatter).formatDate(dtmValueDate, String.Empty)
                lblNPEPaymentDate.Text = (New Formatter).formatDisplayDate(dtmValueDate)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                ibtnNPECompleteReimbursement.Enabled = False
                ibtnNPECompleteReimbursement.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CompleteReimbursementDisableBtn")

                udtAuditLogEntry.AddDescripton("NoPaymentFileRequired", String.Format("Reimbursement completed with payment date {0}", lblRPEPaymentDate.Text))

            ElseIf dtN.AbleToGenerateBankFile = False Then
                ' Message: The reimbursement process is still in progress.
                udcNInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)
                udcNInfoBox.BuildMessageBox()
                udcNInfoBox.Type = InfoMessageBoxType.Information

                txtNPEPaymentDate.Visible = True
                ibtnNPEPaymentDate.Visible = True
                lblNPEPaymentDate.Visible = False
                imgNPEPaymentDate.Visible = False

                txtNPEPaymentDate.Enabled = False
                ibtnNPEPaymentDate.Enabled = False
                ibtnNPECompleteReimbursement.Enabled = False
                ibtnNPECompleteReimbursement.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CompleteReimbursementDisableBtn")

                txtNPEPaymentDate.Text = String.Empty

                udtAuditLogEntry.AddDescripton("NoPaymentFileRequired", "Reimbursement still in progress")

            Else
                txtNPEPaymentDate.Visible = True
                ibtnNPEPaymentDate.Visible = True
                lblNPEPaymentDate.Visible = False
                imgNPEPaymentDate.Visible = False

                txtNPEPaymentDate.Enabled = True
                ibtnNPEPaymentDate.Enabled = True
                ibtnNPECompleteReimbursement.Enabled = True
                ibtnNPECompleteReimbursement.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CompleteReimbursementBtn")

                txtNPEPaymentDate.Text = String.Empty

                udtAuditLogEntry.AddDescripton("NoPaymentFileRequired", "Ready to complete reimbursement")

            End If

        End If

        dtN = Nothing
        ' --- No Payment File Required ---

        ' CRE20-015 (Special Support Scheme) [Start][Winnie]
        ' --- No Payment File Required (HA) ---
        Dim dtNHA As ReimbursementDataTable = dtResult.FilterByReimbursementMode(EnumReimbursementMode.HAFinance)

        If dtNHA.AtLeastOneSchemeHold = False Then
            ' Message: No records found.
            udcNHAInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcNHAInfoBox.BuildMessageBox()
            udcNHAInfoBox.Type = CustomControls.InfoMessageBoxType.Information

            udtAuditLogEntry.AddDescripton("NoPaymentFileRequired (HA)", "No schemes have started reimbursement")

            mvNHA.SetActiveView(vNHANoRecord)

        Else
            mvNHA.SetActiveView(vNHAContent)
            mvNHAPaymentDate.SetActiveView(vNHAPEnter)
            udcNHAInfoBox.Visible = False
            udcNHAErrorBox.Visible = False

            GridViewDataBind(gvNHA, dtNHA, "Display_Seq", "Asc", False)
            Session(SESS_NHAAuthorizationSummaryDataTable) = dtNHA

            If dtNHA.AllSchemeIsReimbursed Then
                ' Message: The reimbursement process for the following schemes is completed.
                udcNHAInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006)
                udcNHAInfoBox.BuildMessageBox()
                udcNHAInfoBox.Type = InfoMessageBoxType.Information

                txtNHAPEPaymentDate.Visible = False
                ibtnNHAPEPaymentDate.Visible = False
                lblNHAPEPaymentDate.Visible = True
                imgNHAPEPaymentDate.Visible = False

                Dim strSchemeCode As String = String.Empty

                For Each dr As DataRow In dtNHA.Rows
                    If Not IsDBNull(dr("Reimbursed")) AndAlso dr("Reimbursed") = "Y" Then
                        strSchemeCode = dr("Scheme_Code")
                        Exit For
                    End If
                Next

                Dim dtmValueDate As DateTime = udtReimbursementBLL.GetBankIn(strReimID, strSchemeCode).Rows(0)("Value_Date")

                lblNHAPEPaymentDate.Text = (New Formatter).formatDisplayDate(dtmValueDate)

                ibtnNHAPECompleteReimbursement.Enabled = False
                ibtnNHAPECompleteReimbursement.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CompleteReimbursementDisableBtn")

                udtAuditLogEntry.AddDescripton("NoPaymentFileRequired (HA)", String.Format("Reimbursement completed with payment date {0}", lblNHAPEPaymentDate.Text))

            ElseIf dtNHA.AbleToGenerateBankFile = False Then
                ' Message: The reimbursement process is still in progress.
                udcNHAInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)
                udcNHAInfoBox.BuildMessageBox()
                udcNHAInfoBox.Type = InfoMessageBoxType.Information

                txtNHAPEPaymentDate.Visible = True
                ibtnNHAPEPaymentDate.Visible = True
                lblNHAPEPaymentDate.Visible = False
                imgNHAPEPaymentDate.Visible = False

                txtNHAPEPaymentDate.Enabled = False
                ibtnNHAPEPaymentDate.Enabled = False
                ibtnNHAPECompleteReimbursement.Enabled = False
                ibtnNHAPECompleteReimbursement.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CompleteReimbursementDisableBtn")

                txtNHAPEPaymentDate.Text = String.Empty

                udtAuditLogEntry.AddDescripton("NoPaymentFileRequired (HA)", "Reimbursement still in progress")

            ElseIf blnIsSSSCMCUser = False Then
                ' Message: The reimbursement process can be proceeded by SSSCMC user only.
                udcNHAInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00007)
                udcNHAInfoBox.BuildMessageBox()
                udcNHAInfoBox.Type = InfoMessageBoxType.Information

                txtNHAPEPaymentDate.Visible = True
                ibtnNHAPEPaymentDate.Visible = True
                lblNHAPEPaymentDate.Visible = False
                imgNHAPEPaymentDate.Visible = False

                txtNHAPEPaymentDate.Enabled = False
                ibtnNHAPEPaymentDate.Enabled = False
                ibtnNHAPECompleteReimbursement.Enabled = False
                ibtnNHAPECompleteReimbursement.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CompleteReimbursementDisableBtn")

                txtNHAPEPaymentDate.Text = String.Empty

                udtAuditLogEntry.AddDescripton("NoPaymentFileRequired (HA)", String.Format("Reimbursement cannot be proceeded without SSSCMC user role with user {0}", udtHCVUUser.UserID))

            Else
                txtNHAPEPaymentDate.Visible = True
                ibtnNHAPEPaymentDate.Visible = True
                lblNHAPEPaymentDate.Visible = False
                imgNHAPEPaymentDate.Visible = False

                txtNHAPEPaymentDate.Enabled = True
                ibtnNHAPEPaymentDate.Enabled = True
                ibtnNHAPECompleteReimbursement.Enabled = True
                ibtnNHAPECompleteReimbursement.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CompleteReimbursementBtn")

                txtNHAPEPaymentDate.Text = String.Empty

                udtAuditLogEntry.AddDescripton("NoPaymentFileRequired (HA)", "Ready to complete reimbursement")

            End If

        End If

        dtNHA = Nothing
        ' --- No Payment File Required (HA) ---
        ' CRE20-015 (Special Support Scheme) [End][Winnie]


        udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDescription.GetAuthorizationSummarySuccessful)

    End Sub

#End Region

#Region "Payment File Required"

    Protected Sub gvR_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim udtFormatter As New Formatter

            ' Hold Time
            Dim lblHoldTime As Label = CType(e.Row.FindControl("lblHoldTime"), Label)
            If lblHoldTime.Text.Trim = String.Empty Then
                lblHoldTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblHoldTime.Enabled = False
            Else
                lblHoldTime.Text = udtFormatter.convertDateTime(lblHoldTime.Text.Trim)
            End If

            ' Hold By
            Dim lblHoldBy As Label = CType(e.Row.FindControl("lblHoldBy"), Label)
            If lblHoldBy.Text.Trim = String.Empty Then
                lblHoldBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblHoldBy.Enabled = False
            End If

            ' First Authorized Time
            Dim lblFirstAuthTime As Label = CType(e.Row.FindControl("lblFirstAuthTime"), Label)
            If lblFirstAuthTime.Text.Trim = String.Empty Then
                lblFirstAuthTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblFirstAuthTime.Enabled = False
            Else
                lblFirstAuthTime.Text = udtFormatter.convertDateTime(lblFirstAuthTime.Text.Trim)
            End If

            ' First Authorized By
            Dim lblFirstAuthBy As Label = CType(e.Row.FindControl("lblFirstAuthBy"), Label)
            If lblFirstAuthBy.Text.Trim = String.Empty Then
                lblFirstAuthBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblFirstAuthBy.Enabled = False
            End If

            ' Second Authorized Time
            Dim lblSecondAuthTime As Label = CType(e.Row.FindControl("lblSecondAuthTime"), Label)
            If lblSecondAuthTime.Text.Trim = String.Empty Then
                lblSecondAuthTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblSecondAuthTime.Enabled = False
            Else
                lblSecondAuthTime.Text = udtFormatter.convertDateTime(lblSecondAuthTime.Text.Trim)
            End If

            ' Second Authorized By
            Dim lblSecondAuthBy As Label = CType(e.Row.FindControl("lblSecondAuthBy"), Label)
            If lblSecondAuthBy.Text.Trim = String.Empty Then
                lblSecondAuthBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblSecondAuthBy.Enabled = False
            End If

        End If
    End Sub

    Protected Sub gvR_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_RAuthorizationSummaryDataTable)
    End Sub

    Protected Sub gvR_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_RAuthorizationSummaryDataTable)
    End Sub

    '

    Protected Sub ibtnRPEGenerate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcRInfoBox.Visible = False
        udcRErrorBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Payment Day", txtRPEPaymentDate.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00004, AuditLogDescription.GenerateClick)

        ' Data validation
        imgRPEPaymentDate.Visible = False

        ' Format the input date
        Dim udtFormatter As New Formatter
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'txtRPEPaymentDate.Text = udtFormatter.formatDate(txtRPEPaymentDate.Text.Trim)
        txtRPEPaymentDate.Text = udtFormatter.formatInputDate(txtRPEPaymentDate.Text.Trim)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Dim udtSysMessage As SystemMessage = (New Validator).chkBankPaymentDate(FunctionCode, txtRPEPaymentDate.Text, Nothing)

        If Not IsNothing(udtSysMessage) Then
            udcRErrorBox.AddMessage(udtSysMessage)
            imgRPEPaymentDate.Visible = True
        End If

        If udcRErrorBox.GetCodeTable.Rows.Count <> 0 Then
            udcRErrorBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00005, AuditLogDescription.GenerateFail)

            Return
        End If

        lblRPCPaymentDate.Text = udtFormatter.convertDate(txtRPEPaymentDate.Text.Trim, String.Empty)

        udcRInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
        udcRInfoBox.BuildMessageBox()
        udcRInfoBox.Type = CustomControls.InfoMessageBoxType.Information

        udtAuditLogEntry.AddDescripton("Payment Day", lblRPCPaymentDate.Text)
        udtAuditLogEntry.WriteEndLog(LogID.LOG00006, AuditLogDescription.GenerateSuccessful)

        mvRPaymentDate.SetActiveView(vRPConfirm)

    End Sub

    Protected Sub ibtnRPCBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcRInfoBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00007, AuditLogDescription.GenerateBackClick)

        mvRPaymentDate.SetActiveView(vRPEnter)

    End Sub

    Protected Sub ibtnRPCConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00008, AuditLogDescription.GenerateConfirmClick)

        Dim intGenerated As Integer = 0
        Dim lstSchemeSubmitted As New List(Of String)

        Dim udtReimbursementBLL As New ReimbursementBLL
        Dim udtFormatter As New Formatter

        For Each r As GridViewRow In gvR.Rows
            If CType(r.FindControl("hfHoldBy"), HiddenField).Value = String.Empty Then Continue For

            Dim strSchemeCode As String = CType(r.FindControl("hfSchemeCode"), HiddenField).Value.Trim
            Dim strSchemeDisplayCode As String = CType(r.FindControl("lblSchemeCode"), Label).Text.Trim
            Dim strReimburseID As String = lblReimID.Text.Trim
            Dim dtmPaymentDate As DateTime = DateTime.ParseExact(lblRPCPaymentDate.Text.Trim, udtFormatter.DisplayDateFormat, Nothing)

            udtReimbursementBLL.GeneratePaymentFile(strSchemeCode, strSchemeDisplayCode, strReimburseID, dtmPaymentDate)

            lstSchemeSubmitted.Add(strSchemeCode)
            intGenerated += 1
        Next

        udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", String.Join(", ", lstSchemeSubmitted.ToArray))
        udcInfoBox.BuildMessageBox()
        udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete

        lblNoPaymentFileGenerated.Text = intGenerated
        lblNoPaymentFileGeneratedText.Visible = True
        lblNoPaymentFileGenerated.Visible = True

        udtAuditLogEntry.AddDescripton("No of Payment File", intGenerated)
        udtAuditLogEntry.AddDescripton("Scheme", String.Join(",", lstSchemeSubmitted.ToArray))
        udtAuditLogEntry.WriteEndLog(LogID.LOG00009, AuditLogDescription.GenerateConfirmSuccessful)

        mvCore.SetActiveView(vComplete)

    End Sub

#End Region

#Region "No Payment File Required"

    Protected Sub gvN_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim udtFormatter As New Formatter

            ' Hold Time
            Dim lblHoldTime As Label = CType(e.Row.FindControl("lblHoldTime"), Label)
            If lblHoldTime.Text.Trim = String.Empty Then
                lblHoldTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblHoldTime.Enabled = False
            Else
                lblHoldTime.Text = udtFormatter.convertDateTime(lblHoldTime.Text.Trim)
            End If

            ' Hold By
            Dim lblHoldBy As Label = CType(e.Row.FindControl("lblHoldBy"), Label)
            If lblHoldBy.Text.Trim = String.Empty Then
                lblHoldBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblHoldBy.Enabled = False
            End If

            ' First Authorized Time
            Dim lblFirstAuthTime As Label = CType(e.Row.FindControl("lblFirstAuthTime"), Label)
            If lblFirstAuthTime.Text.Trim = String.Empty Then
                lblFirstAuthTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblFirstAuthTime.Enabled = False
            Else
                lblFirstAuthTime.Text = udtFormatter.convertDateTime(lblFirstAuthTime.Text.Trim)
            End If

            ' First Authorized By
            Dim lblFirstAuthBy As Label = CType(e.Row.FindControl("lblFirstAuthBy"), Label)
            If lblFirstAuthBy.Text.Trim = String.Empty Then
                lblFirstAuthBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblFirstAuthBy.Enabled = False
            End If

            ' Second Authorized Time
            Dim lblSecondAuthTime As Label = CType(e.Row.FindControl("lblSecondAuthTime"), Label)
            If lblSecondAuthTime.Text.Trim = String.Empty Then
                lblSecondAuthTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblSecondAuthTime.Enabled = False
            Else
                lblSecondAuthTime.Text = udtFormatter.convertDateTime(lblSecondAuthTime.Text.Trim)
            End If

            ' Second Authorized By
            Dim lblSecondAuthBy As Label = CType(e.Row.FindControl("lblSecondAuthBy"), Label)
            If lblSecondAuthBy.Text.Trim = String.Empty Then
                lblSecondAuthBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblSecondAuthBy.Enabled = False
            End If

        End If
    End Sub

    Protected Sub gvN_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_NAuthorizationSummaryDataTable)
    End Sub

    Protected Sub gvN_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_NAuthorizationSummaryDataTable)
    End Sub

    '

    Protected Sub ibtnNPECompleteReimbursement_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcNInfoBox.Visible = False
        udcNErrorBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Payment Day", txtNPEPaymentDate.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00011, "Complete Reimbursement click")

        ' Data validation
        imgNPEPaymentDate.Visible = False

        ' Format the input date
        Dim udtFormatter As New Formatter
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'txtNPEPaymentDate.Text = udtFormatter.formatDate(txtNPEPaymentDate.Text.Trim)
        'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'txtNPEPaymentDate.Text = udtFormatter.formatInputDate(txtNPEPaymentDate.Text.Trim)
        Dim strNPEPaymentDate As String = IIf(udtFormatter.formatInputDate(txtNPEPaymentDate.Text.Trim) <> String.Empty, udtFormatter.formatInputDate(txtNPEPaymentDate.Text.Trim), txtNPEPaymentDate.Text.Trim)
        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Dim udtSysMessage As SystemMessage = (New Validator).chkInputDate(txtNPEPaymentDate.Text, True)
        Dim udtSysMessage As SystemMessage = (New Validator).chkInputDate(strNPEPaymentDate, True, False)

        If Not IsNothing(udtSysMessage) Then
            udcNErrorBox.AddMessage(udtSysMessage, "%s", Me.GetGlobalResourceObject("Text", "BankPaymentDay"))
            imgNPEPaymentDate.Visible = True
        End If

        If IsNothing(udtSysMessage) Then
            txtNPEPaymentDate.Text = strNPEPaymentDate
        End If
        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

        If udcNErrorBox.GetCodeTable.Rows.Count = 0 Then
            ' Bank Payment Date should not be earlier than Cutoff Date
            Dim dt As DataTable = (New ReimbursementBLL).GetReimbursementAuthorisationByIDStatus(Nothing, ReimbursementStatus.StartReimbursement, ReimbursementAuthorisationStatus.Active, Nothing)
            Dim dtmCutoffDate As DateTime = dt.Rows(0)("Cutoff_Date")

            If CDate(udtFormatter.convertDate(txtNPEPaymentDate.Text, String.Empty)).Subtract(dtmCutoffDate).Days < 0 Then
                ' Error: The "Bank Payment Date" should not be earlier than the Reimbursement Cutoff Date.
                udcNErrorBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00030))
                imgNPEPaymentDate.Visible = True
            End If

        End If

        If udcNErrorBox.GetCodeTable.Rows.Count <> 0 Then
            udcNErrorBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00013, "Complete Reimbursement click fail")

            Return
        End If

        lblNPCPaymentDate.Text = udtFormatter.convertDate(txtNPEPaymentDate.Text.Trim, String.Empty)

        udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
        udcInfoBox.BuildMessageBox()
        udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information

        udtAuditLogEntry.AddDescripton("Payment Day", lblNPCPaymentDate.Text)
        udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Complete Reimbursement click success")

        mvNPaymentDate.SetActiveView(vNPConfirm)

    End Sub

    Protected Sub ibtnNPCBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00014, "Complete Reimbursement - Back click")

        mvNPaymentDate.SetActiveView(vNPEnter)

    End Sub

    Protected Sub ibtnNPCConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00015, "Complete Reimbursement - Confirm click")

        Dim intGenerated As Integer = 0
        Dim strSchemeSubmitted As String = String.Empty

        Dim udtReimbursementBLL As New ReimbursementBLL
        Dim udtFormatter As New Formatter

        For Each r As GridViewRow In gvN.Rows
            If CType(r.FindControl("hfHoldBy"), HiddenField).Value = String.Empty Then Continue For

            Dim strSchemeCode As String = CType(r.FindControl("hfSchemeCode"), HiddenField).Value.Trim
            Dim strSchemeDisplayCode As String = CType(r.FindControl("lblSchemeCode"), Label).Text.Trim
            Dim strReimburseID As String = lblReimID.Text.Trim
            Dim dtmPaymentDate As DateTime = DateTime.ParseExact(lblNPCPaymentDate.Text.Trim, udtFormatter.DisplayDateFormat, Nothing)

            udtReimbursementBLL.GeneratePaymentFile(strSchemeCode, strSchemeDisplayCode, strReimburseID, dtmPaymentDate)

            strSchemeSubmitted += ", " + strSchemeCode
            intGenerated += 1
        Next

        If strSchemeSubmitted <> String.Empty Then strSchemeSubmitted = strSchemeSubmitted.Substring(2)

        udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", strSchemeSubmitted)
        udcInfoBox.BuildMessageBox()
        udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete

        lblNoPaymentFileGeneratedText.Visible = False
        lblNoPaymentFileGenerated.Visible = False

        udtAuditLogEntry.AddDescripton("Scheme", strSchemeSubmitted)
        udtAuditLogEntry.WriteEndLog(LogID.LOG00016, "Complete Reimbursement - Confirm click success")

        mvCore.SetActiveView(vComplete)

    End Sub

#End Region

#Region "No Payment File Required (HA)"

    Protected Sub gvNHA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim udtFormatter As New Formatter

            ' Hold Time
            Dim lblHoldTime As Label = CType(e.Row.FindControl("lblHoldTime"), Label)
            If lblHoldTime.Text.Trim = String.Empty Then
                lblHoldTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblHoldTime.Enabled = False
            Else
                lblHoldTime.Text = udtFormatter.convertDateTime(lblHoldTime.Text.Trim)
            End If

            ' Hold By
            Dim lblHoldBy As Label = CType(e.Row.FindControl("lblHoldBy"), Label)
            If lblHoldBy.Text.Trim = String.Empty Then
                lblHoldBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblHoldBy.Enabled = False
            End If

            ' First Authorized Time
            Dim lblFirstAuthTime As Label = CType(e.Row.FindControl("lblFirstAuthTime"), Label)
            If lblFirstAuthTime.Text.Trim = String.Empty Then
                lblFirstAuthTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblFirstAuthTime.Enabled = False
            Else
                lblFirstAuthTime.Text = udtFormatter.convertDateTime(lblFirstAuthTime.Text.Trim)
            End If

            ' First Authorized By
            Dim lblFirstAuthBy As Label = CType(e.Row.FindControl("lblFirstAuthBy"), Label)
            If lblFirstAuthBy.Text.Trim = String.Empty Then
                lblFirstAuthBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblFirstAuthBy.Enabled = False
            End If

            ' Second Authorized Time
            Dim lblSecondAuthTime As Label = CType(e.Row.FindControl("lblSecondAuthTime"), Label)
            If lblSecondAuthTime.Text.Trim = String.Empty Then
                lblSecondAuthTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblSecondAuthTime.Enabled = False
            Else
                lblSecondAuthTime.Text = udtFormatter.convertDateTime(lblSecondAuthTime.Text.Trim)
            End If

            ' Second Authorized By
            Dim lblSecondAuthBy As Label = CType(e.Row.FindControl("lblSecondAuthBy"), Label)
            If lblSecondAuthBy.Text.Trim = String.Empty Then
                lblSecondAuthBy.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblSecondAuthBy.Enabled = False
            End If

        End If
    End Sub

    Protected Sub gvNHA_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_NHAAuthorizationSummaryDataTable)
    End Sub

    Protected Sub gvNHA_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_NHAAuthorizationSummaryDataTable)
    End Sub

    '

    Protected Sub ibtnNHAPECompleteReimbursement_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcNHAInfoBox.Visible = False
        udcNHAErrorBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Payment Day", txtNHAPEPaymentDate.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00011, "Complete Reimbursement click")

        ' Data validation
        imgNHAPEPaymentDate.Visible = False

        ' Format the input date
        Dim udtFormatter As New Formatter

        Dim strNHAPEPaymentDate As String = IIf(udtFormatter.formatInputDate(txtNHAPEPaymentDate.Text.Trim) <> String.Empty, udtFormatter.formatInputDate(txtNHAPEPaymentDate.Text.Trim), txtNHAPEPaymentDate.Text.Trim)

        Dim udtSysMessage As SystemMessage = (New Validator).chkInputDate(strNHAPEPaymentDate, True, False)

        If Not IsNothing(udtSysMessage) Then
            udcNHAErrorBox.AddMessage(udtSysMessage, "%s", Me.GetGlobalResourceObject("Text", "BankPaymentDay"))
            imgNHAPEPaymentDate.Visible = True
        End If

        If IsNothing(udtSysMessage) Then
            txtNHAPEPaymentDate.Text = strNHAPEPaymentDate
        End If

        If udcNHAErrorBox.GetCodeTable.Rows.Count = 0 Then
            ' Bank Payment Date should not be earlier than Cutoff Date
            Dim dt As DataTable = (New ReimbursementBLL).GetReimbursementAuthorisationByIDStatus(Nothing, ReimbursementStatus.StartReimbursement, ReimbursementAuthorisationStatus.Active, Nothing)
            Dim dtmCutoffDate As DateTime = dt.Rows(0)("Cutoff_Date")

            If CDate(udtFormatter.convertDate(txtNHAPEPaymentDate.Text, String.Empty)).Subtract(dtmCutoffDate).Days < 0 Then
                ' Error: The "Bank Payment Date" should not be earlier than the Reimbursement Cutoff Date.
                udcNHAErrorBox.AddMessage(New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00030))
                imgNHAPEPaymentDate.Visible = True
            End If

        End If

        If udcNHAErrorBox.GetCodeTable.Rows.Count <> 0 Then
            udcNHAErrorBox.BuildMessageBox(ErrorMessageBoxHeaderKey.ValidationFail, udtAuditLogEntry, LogID.LOG00013, "Complete Reimbursement click fail")

            Return
        End If

        lblNHAPCPaymentDate.Text = udtFormatter.convertDate(txtNHAPEPaymentDate.Text.Trim, String.Empty)

        udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
        udcInfoBox.BuildMessageBox()
        udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information

        udtAuditLogEntry.AddDescripton("Payment Day", lblNHAPCPaymentDate.Text)
        udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Complete Reimbursement click success")

        mvNHAPaymentDate.SetActiveView(vNHAPConfirm)

    End Sub

    Protected Sub ibtnNHAPCBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00014, "Complete Reimbursement - Back click")

        mvNHAPaymentDate.SetActiveView(vNHAPEnter)

    End Sub

    Protected Sub ibtnNHAPCConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00015, "Complete Reimbursement - Confirm click")

        Dim intGenerated As Integer = 0
        Dim strSchemeSubmitted As String = String.Empty

        Dim udtReimbursementBLL As New ReimbursementBLL
        Dim udtFormatter As New Formatter

        For Each r As GridViewRow In gvNHA.Rows
            If CType(r.FindControl("hfHoldBy"), HiddenField).Value = String.Empty Then Continue For

            Dim strSchemeCode As String = CType(r.FindControl("hfSchemeCode"), HiddenField).Value.Trim
            Dim strSchemeDisplayCode As String = CType(r.FindControl("lblSchemeCode"), Label).Text.Trim
            Dim strReimburseID As String = lblReimID.Text.Trim
            Dim dtmPaymentDate As DateTime = DateTime.ParseExact(lblNHAPCPaymentDate.Text.Trim, udtFormatter.DisplayDateFormat, Nothing)

            udtReimbursementBLL.GeneratePaymentFile(strSchemeCode, strSchemeDisplayCode, strReimburseID, dtmPaymentDate)

            strSchemeSubmitted += ", " + strSchemeCode
            intGenerated += 1
        Next

        If strSchemeSubmitted <> String.Empty Then strSchemeSubmitted = strSchemeSubmitted.Substring(2)

        udcInfoBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", strSchemeSubmitted)
        udcInfoBox.BuildMessageBox()
        udcInfoBox.Type = CustomControls.InfoMessageBoxType.Complete

        lblNoPaymentFileGeneratedText.Visible = False
        lblNoPaymentFileGenerated.Visible = False

        udtAuditLogEntry.AddDescripton("Scheme", strSchemeSubmitted)
        udtAuditLogEntry.WriteEndLog(LogID.LOG00016, "Complete Reimbursement - Confirm click success")

        mvCore.SetActiveView(vComplete)

    End Sub

#End Region

    Protected Sub ibtnReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00010, AuditLogDescription.ReturnClick)

        GetAuthorizationSummary()
    End Sub

#Region "Implement Working Data"

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

#End Region

End Class
