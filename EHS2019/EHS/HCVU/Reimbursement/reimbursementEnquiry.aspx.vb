Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.ClaimTrans
Imports Common.Component.HCVUUser
Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel
Imports Common.ComObject
Imports Common.Format
Imports Common.SearchCriteria
Imports Common.Validation
Imports HCVU.ReimbursementBLL

Partial Public Class reimbursement_enquiry
    Inherits BasePageWithGridView

#Region "Private Class"

    Private Class ViewIndex
        Public Const FillCriteria As Integer = 0
        Public Const PaymentFileListing As Integer = 1
        Public Const DrillSPID As Integer = 2
        Public Const DrillBankAccount As Integer = 3
        Public Const DrillTransaction As Integer = 4
        Public Const TransactionDetail As Integer = 5
    End Class

    Private Class DrillLevel
        Public Const None As Integer = 0
        Public Const PaymentFile As Integer = 1
        Public Const SPID As Integer = 2
        Public Const BankAccount As Integer = 3
        Public Const Transaction As Integer = 4
    End Class

#End Region

#Region "Fields"

    Private udtClaimTransBLL As New EHSTransaction.EHSTransactionBLL
    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtReimbursementBLL As New ReimbursementBLL
    Private udtSearchEngineBLL As New SearchEngineBLL
    Private udtValidator As New Validator

#End Region

#Region "Session Constants"

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    Private Const SESS_ReimbursementEnquiryPaymentFileRequiredDataTable As String = "SESS_ReimbursementEnquiryPaymentFileRequiredDataTable"
    Private Const SESS_ReimbursementEnquiryNoPaymentFileRequiredDataTable As String = "SESS_ReimbursementEnquiryNoPaymentFileRequiredDataTable"
    Private Const SESS_ReimbursementEnquiryNoPaymentFileRequiredHADataTable As String = "SESS_ReimbursementEnquiryNoPaymentFileRequiredHADataTable" 'CRE20-015 (Special Support Scheme) [Martin]
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
    Private Const SESS_ReimbursementEnquirySearchCriteria As String = "ReimbursementEnquirySearchCriteria"
    Private Const SESS_ReimbursementEnquiryDrillSPIDDataTable As String = "ReimbursementEnquiryDrillSPIDDataTable"
    Private Const SESS_ReimbursementEnquiryDrillBankAccountDataTable As String = "ReimbursementEnquiryDrillBankAccountDataTable"
    Private Const SESS_ReimbursementEnquiryDrillTransactionDataTable As String = "ReimbursementEnquiryDrillTransactionDataTable"

#End Region

#Region "Constants"
    Dim strExportReportID As String = "eHSM0012" ' CRE17-004 Generate a new DPAR on EHCP basis [Dickson]
#End Region


#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const DPAR_Download_Click As String = "[Detailed Payment Analysis Report] Download - Download click"
        Public Const DPAR_Download_Success As String = "[Detailed Payment Analysis Report] Download - Download success"
        Public Const DPAR_Download_Fail As String = "[Detailed Payment Analysis Report] Download - Download fail"
    End Class
#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT010405

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Reimbursement Enquiry Loaded")

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            InitControlOnce()
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
            ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]
            ' CRE17-004 Generate a new DPAR on EHCP basis [Start][Martin]
            Dim strvalue1 As String = String.Empty
            Dim strvalue2 As String = String.Empty
            Dim udtcomfunct As New Common.ComFunction.GeneralFunction

            udtcomfunct.getSystemParameter("PasswordRuleNumber", strvalue1, strvalue2)
            Me.txtNewPassword.Attributes.Remove("onKeyUp")
            Me.txtNewPassword.Attributes.Add("onKeyUp", "checkPassword(this.value," & _
                                                            "'" & CInt(strvalue2.Trim) & "'," & _
                                                            "'" & CInt(strvalue2.Trim) & "'," & _
                                                            "'strength1','strength2','strength3','progressBar'," & _
                                                            "'" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "'," & _
                                                            "'" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "'," & _
                                                            "'" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "'," & _
                                                            "'direction2','direction1');")
            ' CRE17-004 Generate a new DPAR on EHCP basis [End][Martin]
        Else
            If MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.TransactionDetail Then
                LoadDetail(hfCurrentDetailTransactionNo.Value)
            End If
            ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.TransactionDetail Then EnablePreviousNextButton()
    End Sub

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    Private Sub InitControlOnce()
        pnlSearchCriteriaReview.Visible = False

        Session(SESS_ReimbursementEnquiryPaymentFileRequiredDataTable) = Nothing
        Session(SESS_ReimbursementEnquiryNoPaymentFileRequiredDataTable) = Nothing
        Session(SESS_ReimbursementEnquiryNoPaymentFileRequiredHADataTable) = Nothing
        Session(SESS_ReimbursementEnquirySearchCriteria) = Nothing
        Session(SESS_ReimbursementEnquiryDrillSPIDDataTable) = Nothing
        Session(SESS_ReimbursementEnquiryDrillBankAccountDataTable) = Nothing
        Session(SESS_ReimbursementEnquiryDrillTransactionDataTable) = Nothing

        Dim intPageSize As Integer = udtGeneralFunction.GetPageSizeHCVU()

        gvPaymentFile.PageSize = intPageSize
        gvDrillSPID.PageSize = intPageSize
        gvDrillBankAccount.PageSize = intPageSize
        gvDrillTransaction.PageSize = intPageSize

        Dim udtSearchCriteria = New SearchCriteria
        Session(SESS_ReimbursementEnquirySearchCriteria) = udtSearchCriteria

        BindControl()

        InitCurrentReimbursement()

    End Sub

    Private Sub InitCurrentReimbursement()
        ' Get the latest Reimbursement ID with [Authorised_Status] = 'S' and [Record_Status] = 'A'
        Dim udtReimbursementBLL As New ReimbursementBLL
        Dim dtReimID As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(Nothing, ReimbursementStatus.StartReimbursement, ReimbursementAuthorisationStatus.Active, Nothing)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        If dtReimID.Rows.Count = 0 Then
            ' Message: No records found.
            udcInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoBox.BuildMessageBox()
            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information

            udtAuditLogEntry.AddDescripton("StackTrace", "Could not find active Reimbursement ID")
            udtAuditLogEntry.WriteLog(LogID.LOG00031, "No current reimbursement in progress")

            tpCurrent.Visible = False
            tcCore.ActiveTab = tpPrevious

            Return
        End If

        Dim strReimbursementID As String = CStr(dtReimID.Rows(0)("Reimburse_ID")).Trim

        ' Use this Reimbursement ID to get records from [ReimbursementAuthorisation]
        Dim dtReimAuth As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(strReimbursementID, Nothing, ReimbursementAuthorisationStatus.Active, Nothing)

        ' Flatten the table
        Dim dtResult As New ReimbursementDataTable(dtReimAuth)

        ' If all held schemes are reimbursed, the reimbursement is already completed
        If dtResult.AllSchemeIsReimbursed Then
            udtAuditLogEntry.AddDescripton("StackTrace", String.Format("The latest reimbursement {0} is completed", strReimbursementID))
            udtAuditLogEntry.WriteLog(LogID.LOG00031, "No current reimbursement in progress")

            tpCurrent.Visible = False
            tcCore.ActiveTab = tpPrevious

            ' Message: No reimbursement is in progress.
            udcDInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
            udcDInfoMessageBox.BuildMessageBox()
            udcDInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

            Return

        End If

        tpDisable.Visible = False
        tcCore.ActiveTab = tpCurrent

        udtAuditLogEntry.AddDescripton("Reimbursement ID", strReimbursementID)

        ' Reimbursement ID
        lblCReimbursementID.Text = strReimbursementID

        ' CRE20-015-02 (Special Support Scheme) [Start][Martin]
        'Only display "No Payment File Required (HA)" if HA user
        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser
        If udtHCVUUserBLL.IsSSSCMCUser(udtHCVUUser) Then
            panlCP.Visible = False
            panlCN.Visible = False
        End If
        ' CRE20-015-02 (Special Support Scheme) [End][Martin]


        ' --- Payment File Required ---
        Dim dtR As ReimbursementDataTable = dtResult.FilterByReimbursementMode(EnumReimbursementMode.All)

        GridViewDataBind(gvCP, dtR, "Display_Seq", "Asc", False)
        Session(SESS_ReimbursementEnquiryPaymentFileRequiredDataTable) = dtR

        If dtR.AtLeastOneSchemeHold = False Then
            ' Message: No records found.
            lblCPStatus.Text = Me.GetGlobalResourceObject("Text", "NotYetStart")
            lblCPBankPaymentDate.Text = Me.GetGlobalResourceObject("Text", "N/A")

        Else
            If dtR.AllSchemeIsReimbursed Then
                lblCPStatus.Text = Me.GetGlobalResourceObject("Text", "Completed")

                Dim strSchemeCode As String = String.Empty

                For Each dr As DataRow In dtR.Rows
                    If Not IsDBNull(dr("Reimbursed")) AndAlso dr("Reimbursed") = "Y" Then
                        strSchemeCode = dr("Scheme_Code")
                        Exit For
                    End If
                Next

                Dim dtmValueDate As DateTime = udtReimbursementBLL.GetBankIn(strReimbursementID, strSchemeCode).Rows(0)("Value_Date")

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'lblCPBankPaymentDate.Text = udtFormatter.formatDate(dtmValueDate, String.Empty)
                lblCPBankPaymentDate.Text = udtFormatter.formatDisplayDate(dtmValueDate)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ElseIf dtR.AbleToGenerateBankFile = False Then
                lblCPStatus.Text = Me.GetGlobalResourceObject("Text", "InProgress")
                lblCPBankPaymentDate.Text = Me.GetGlobalResourceObject("Text", "N/A")

            Else
                lblCPStatus.Text = Me.GetGlobalResourceObject("Text", "InProgress")
                lblCPBankPaymentDate.Text = Me.GetGlobalResourceObject("Text", "N/A")

            End If

        End If

        dtR = Nothing

        ' --- No Payment File Required ---
        Dim dtN As ReimbursementDataTable = dtResult.FilterByReimbursementMode(EnumReimbursementMode.FirstAuthAndSecondAuth)

        GridViewDataBind(gvCN, dtN, "Display_Seq", "Asc", False)
        Session(SESS_ReimbursementEnquiryNoPaymentFileRequiredDataTable) = dtN

        If dtN.AtLeastOneSchemeHold = False Then
            lblCNStatus.Text = Me.GetGlobalResourceObject("Text", "NotYetStart")
            lblCNBankPaymentDate.Text = Me.GetGlobalResourceObject("Text", "N/A")

        Else
            If dtN.AllSchemeIsReimbursed Then
                lblCNStatus.Text = Me.GetGlobalResourceObject("Text", "Completed")

                Dim strSchemeCode As String = String.Empty

                For Each dr As DataRow In dtN.Rows
                    If Not IsDBNull(dr("Reimbursed")) AndAlso dr("Reimbursed") = "Y" Then
                        strSchemeCode = dr("Scheme_Code")
                        Exit For
                    End If
                Next

                Dim dtmValueDate As DateTime = udtReimbursementBLL.GetBankIn(strReimbursementID, strSchemeCode).Rows(0)("Value_Date")

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'lblCNBankPaymentDate.Text = udtFormatter.formatDate(dtmValueDate, String.Empty)
                lblCNBankPaymentDate.Text = udtFormatter.formatDisplayDate(dtmValueDate)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ElseIf dtN.AbleToGenerateBankFile = False Then
                lblCNStatus.Text = Me.GetGlobalResourceObject("Text", "InProgress")
                lblCNBankPaymentDate.Text = Me.GetGlobalResourceObject("Text", "N/A")

            Else
                lblCNStatus.Text = Me.GetGlobalResourceObject("Text", "InProgress")
                lblCNBankPaymentDate.Text = Me.GetGlobalResourceObject("Text", "N/A")

            End If

        End If

        dtN = Nothing

        ' CRE20-015-02 (Special Support Scheme) [Start][Martin]
        ' --- No Payment File Required (HA) ---
        Dim dtHA As ReimbursementDataTable = dtResult.FilterByReimbursementMode(EnumReimbursementMode.HAFinance)

        GridViewDataBind(gvHA, dtHA, "Display_Seq", "Asc", False)
        Session(SESS_ReimbursementEnquiryNoPaymentFileRequiredHADataTable) = dtHA

        If dtHA.AtLeastOneSchemeHold = False Then
            lblHAStatus.Text = Me.GetGlobalResourceObject("Text", "NotYetStart")
            lblHABankPaymentDate.Text = Me.GetGlobalResourceObject("Text", "N/A")

        Else
            If dtHA.AllSchemeIsReimbursed Then
                lblHAStatus.Text = Me.GetGlobalResourceObject("Text", "Completed")

                Dim strSchemeCode As String = String.Empty

                For Each dr As DataRow In dtHA.Rows
                    If Not IsDBNull(dr("Reimbursed")) AndAlso dr("Reimbursed") = "Y" Then
                        strSchemeCode = dr("Scheme_Code")
                        Exit For
                    End If
                Next

                Dim dtmValueDate As DateTime = udtReimbursementBLL.GetBankIn(strReimbursementID, strSchemeCode).Rows(0)("Value_Date")


                lblHABankPaymentDate.Text = udtFormatter.formatDisplayDate(dtmValueDate)

            ElseIf dtHA.AbleToGenerateBankFile = False Then
                lblHAStatus.Text = Me.GetGlobalResourceObject("Text", "InProgress")
                lblHABankPaymentDate.Text = Me.GetGlobalResourceObject("Text", "N/A")

            Else
                lblHAStatus.Text = Me.GetGlobalResourceObject("Text", "InProgress")
                lblHABankPaymentDate.Text = Me.GetGlobalResourceObject("Text", "N/A")

            End If

        End If


        dtHA = Nothing

        udtAuditLogEntry.AddDescripton("PaymentFileRequired", String.Format("Status: {0}, BankPaymentDate: {1}", lblCPStatus.Text, lblCPBankPaymentDate.Text))
        udtAuditLogEntry.AddDescripton("NoPaymentFileRequired", String.Format("Status: {0}, BankPaymentDate: {1}", lblCNStatus.Text, lblCNBankPaymentDate.Text))
        udtAuditLogEntry.AddDescripton("NoPaymentFileRequired(HA)", String.Format("Status: {0}, BankPaymentDate: {1}", lblHAStatus.Text, lblHABankPaymentDate.Text))
        udtAuditLogEntry.WriteLog(LogID.LOG00030, "Current reimbursement is in progress")
        ' CRE20-015-02 (Special Support Scheme) [End][Martin]
    End Sub
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

    Private Sub EnablePreviousNextButton()
        ibtnDetailPrevious.Enabled = lblCurrentRecordNo.Text <> "1"
        ibtnDetailNext.Enabled = lblCurrentRecordNo.Text <> lblMaxRecordNo.Text
    End Sub

#End Region

    Private Sub BackToAuthorizatePage()
        pnlSearchCriteriaReview.Visible = False
        MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.FillCriteria
    End Sub

    Private Sub BuildSearchCriteriaReview(ByVal intDrillLevel As Integer, Optional ByVal strReimburseID As String = Nothing, Optional ByVal udtSearchCriteria As SearchCriteria = Nothing, Optional ByVal strBankAccountNoMasked As String = Nothing)
        Select Case intDrillLevel
            Case DrillLevel.None
                pnlSearchCriteriaReview.Visible = False

            Case DrillLevel.PaymentFile
                pnlSearchCriteriaReview.Visible = True

                lblRPaymentFileDateFromText.Visible = Not rbtnLastFile.Checked
                lblRPaymentFileDateFrom.Visible = Not rbtnLastFile.Checked
                lblRPaymentFileDateToText.Visible = Not rbtnLastFile.Checked
                lblRPaymentFileDateTo.Visible = Not rbtnLastFile.Checked
                lblRSchemeCodeText.Visible = False
                lblRSchemeCode.Visible = False
                lblRReimburseIDText.Visible = False
                lblRReimburseID.Visible = False
                lblRSPNameText.Visible = False
                lblRSPName.Visible = False
                lblRSPIDText.Visible = False
                lblRSPID.Visible = False
                lblRBankAccountText.Visible = False
                lblRBankAccount.Visible = False
                lblRPracticeText.Visible = False
                lblRPractice.Visible = False

            Case DrillLevel.SPID
                If Not IsNothing(udtSearchCriteria) Then lblRSchemeCode.Text = udtSearchCriteria.SchemeCode
                If Not IsNothing(strReimburseID) Then lblRReimburseID.Text = strReimburseID

                lblRPaymentFileDateFromText.Visible = Not rbtnLastFile.Checked
                lblRPaymentFileDateFrom.Visible = Not rbtnLastFile.Checked
                lblRPaymentFileDateToText.Visible = Not rbtnLastFile.Checked
                lblRPaymentFileDateTo.Visible = Not rbtnLastFile.Checked
                lblRSchemeCodeText.Visible = True
                lblRSchemeCode.Visible = True
                lblRReimburseIDText.Visible = True
                lblRReimburseID.Visible = True

                lblRSPNameText.Visible = False
                lblRSPName.Visible = False
                lblRSPIDText.Visible = False
                lblRSPID.Visible = False
                lblRBankAccountText.Visible = False
                lblRBankAccount.Visible = False
                lblRPracticeText.Visible = False
                lblRPractice.Visible = False

            Case DrillLevel.BankAccount
                If Not IsNothing(udtSearchCriteria) Then
                    lblRSPID.Text = udtSearchCriteria.ServiceProviderID
                    lblRSPName.Text = udtSearchCriteria.ServiceProviderName
                End If

                If Not IsNothing(udtSearchCriteria) Then
                    lblRBankAccount.Text = udtSearchCriteria.BankAcctNo
                    lblRPractice.Text = udtSearchCriteria.Practice
                End If

                lblRPaymentFileDateFromText.Visible = Not rbtnLastFile.Checked
                lblRPaymentFileDateFrom.Visible = Not rbtnLastFile.Checked
                lblRPaymentFileDateToText.Visible = Not rbtnLastFile.Checked
                lblRPaymentFileDateTo.Visible = Not rbtnLastFile.Checked
                lblRSchemeCodeText.Visible = True
                lblRSchemeCode.Visible = True
                lblRReimburseIDText.Visible = True
                lblRReimburseID.Visible = True
                lblRSPNameText.Visible = True
                lblRSPName.Visible = True
                lblRSPIDText.Visible = True
                lblRSPID.Visible = True

                lblRBankAccountText.Visible = False
                lblRBankAccount.Visible = False
                lblRPracticeText.Visible = False
                lblRPractice.Visible = False

            Case DrillLevel.Transaction
                If Not IsNothing(strBankAccountNoMasked) Then lblRBankAccount.Text = strBankAccountNoMasked
                If Not IsNothing(udtSearchCriteria) Then lblRPractice.Text = udtSearchCriteria.Practice

                lblRPaymentFileDateFromText.Visible = Not rbtnLastFile.Checked
                lblRPaymentFileDateFrom.Visible = Not rbtnLastFile.Checked
                lblRPaymentFileDateToText.Visible = Not rbtnLastFile.Checked
                lblRPaymentFileDateTo.Visible = Not rbtnLastFile.Checked
                lblRSchemeCodeText.Visible = True
                lblRSchemeCode.Visible = True
                lblRReimburseIDText.Visible = True
                lblRReimburseID.Visible = True
                lblRSPNameText.Visible = True
                lblRSPName.Visible = True
                lblRSPIDText.Visible = True
                lblRSPID.Visible = True
                lblRBankAccountText.Visible = True
                lblRBankAccount.Visible = True
                lblRPracticeText.Visible = True
                lblRPractice.Visible = True

        End Select

    End Sub

    Private Sub BindControl()
        ' Last 3 files
        ddlLastFile.DataSource = udtReimbursementBLL.GetLatestThreeReimbursementID(udtHCVUUserBLL.GetHCVUUser.UserID.Trim)
        ddlLastFile.DataTextField = "text"
        ddlLastFile.DataValueField = "value"
        ddlLastFile.DataBind()

        If ddlLastFile.Items.Count = 1 Then
            rbtnLastFile.Enabled = False
            ddlLastFile.Enabled = False
            rbtnSubmissionDate.Checked = True
        Else
            rbtnLastFile.Enabled = True
            ddlLastFile.Enabled = True
            rbtnLastFile.Checked = True
        End If

        ' Payment File Submission Date From / To
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'txtSubmissionDateFrom.Text = udtFormatter.formatEnterDate(FormatDateTime(DateAdd("m", -1, Now), DateFormat.GeneralDate))
        'txtSubmissionDateTo.Text = udtFormatter.formatEnterDate(Now)
        txtSubmissionDateFrom.Text = udtFormatter.formatInputTextDate(CDate(FormatDateTime(DateAdd("m", -1, Now), DateFormat.GeneralDate)))
        txtSubmissionDateTo.Text = udtFormatter.FormatInputTextDate(Now)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

    End Sub

#Region "Current Reimbursement"

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    Protected Sub gvCP_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
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

    Protected Sub gvCP_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_ReimbursementEnquiryPaymentFileRequiredDataTable)
    End Sub

    Protected Sub gvCP_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_ReimbursementEnquiryPaymentFileRequiredDataTable)
    End Sub

    '

    Protected Sub gvCN_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
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


    Protected Sub gvCN_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_ReimbursementEnquiryNoPaymentFileRequiredDataTable)
    End Sub

    Protected Sub gvCN_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_ReimbursementEnquiryNoPaymentFileRequiredDataTable)
    End Sub

    'CRE20-015 (Special Support Scheme) [Start][Martin]
    Protected Sub gvHA_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
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

    Protected Sub gvHA_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_ReimbursementEnquiryNoPaymentFileRequiredHADataTable)
    End Sub

    Protected Sub gvHA_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_ReimbursementEnquiryNoPaymentFileRequiredHADataTable)
    End Sub
    'CRE20-015 (Special Support Scheme) [End][Martin]

#End Region

    Protected Sub ddlLastFile_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        udcInfoBox.Visible = False
        udcErrorBox.Visible = False
        imgAlertSubmissionDateFrom.Visible = False
        imgAlertSubmissionDateTo.Visible = False

        rbtnLastFile.Checked = True

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Try
            udtAuditLogEntry.AddDescripton("Reimbursement ID", ddlLastFile.SelectedValue)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Select Last 3 Reimbursement")

            If ddlLastFile.SelectedIndex <> 0 Then
                ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                BuildPaymentFileGrid(ddlLastFile.SelectedValue, String.Empty, String.Empty, udtHCVUUserBLL.GetHCVUUser.UserID)
                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

                BuildSearchCriteriaReview(DrillLevel.PaymentFile)

                MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.PaymentFileListing

            Else
                MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.FillCriteria
            End If

            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Select Last 3 Reimbursement successful")

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Select Last 3 Reimbursement fail")
            Throw ex
        End Try

    End Sub

    Protected Sub ibtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoBox.Visible = False
        udcErrorBox.Visible = False
        imgAlertSubmissionDateFrom.Visible = False
        imgAlertSubmissionDateTo.Visible = False

        rbtnLastFile.Checked = False
        rbtnSubmissionDate.Checked = True

        Dim sm1, sm2 As Common.ComObject.SystemMessage
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        Try
            udtAuditLogEntry.AddDescripton("Submission Date From", txtSubmissionDateFrom.Text.Trim)
            udtAuditLogEntry.AddDescripton("Submission Date To", txtSubmissionDateTo.Text)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Search Reimbursement")

            'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ' Format input date
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'txtSubmissionDateFrom.Text = udtFormatter.formatDate(txtSubmissionDateFrom.Text.Trim)
            'txtSubmissionDateTo.Text = udtFormatter.formatDate(txtSubmissionDateTo.Text.Trim)
            'txtSubmissionDateFrom.Text = udtFormatter.formatInputDate(txtSubmissionDateFrom.Text.Trim)
            'txtSubmissionDateTo.Text = udtFormatter.formatInputDate(txtSubmissionDateTo.Text.Trim)
            Dim strSubmissionDateFrom = IIf(udtFormatter.formatInputDate(txtSubmissionDateFrom.Text.Trim) <> String.Empty, udtFormatter.formatInputDate(txtSubmissionDateFrom.Text.Trim), txtSubmissionDateFrom.Text.Trim)
            Dim strSubmissionDateTo = IIf(udtFormatter.formatInputDate(txtSubmissionDateTo.Text.Trim) <> String.Empty, udtFormatter.formatInputDate(txtSubmissionDateTo.Text.Trim), txtSubmissionDateTo.Text.Trim)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            'If Not udtValidator.IsEmpty(txtSubmissionDateFrom.Text) Then
            '    txtSubmissionDateFrom.Text = udtFormatter.formatSearchDate(txtSubmissionDateFrom.Text)
            'End If

            'If Not udtValidator.IsEmpty(Me.txtSubmissionDateTo.Text) Then
            '    txtSubmissionDateTo.Text = udtFormatter.formatSearchDate(txtSubmissionDateTo.Text)
            'End If

            'sm1 = udtValidator.chkInputDate(FunctionCode, txtSubmissionDateFrom.Text, Nothing)
            sm1 = udtValidator.chkInputDate(strSubmissionDateFrom, True, True)

            If Not IsNothing(sm1) Then
                'udcErrorBox.AddMessage(sm1)
                udcErrorBox.AddMessage(sm1, "%s", rbtnSubmissionDate.Text + " From")
                imgAlertSubmissionDateFrom.Visible = True
            End If

            'sm2 = udtValidator.chkInputDate(FunctionCode, Me.txtSubmissionDateTo.Text, "00001", "00002", "00003")
            sm2 = udtValidator.chkInputDate(strSubmissionDateTo, True, True)

            If Not IsNothing(sm2) Then
                'udcErrorBox.AddMessage(sm2)
                udcErrorBox.AddMessage(sm2, "%s", rbtnSubmissionDate.Text + " To")
                imgAlertSubmissionDateTo.Visible = True
            End If

            If IsNothing(sm1) And IsNothing(sm2) Then
                'sm1 = udtValidator.chkInputValidFromDateCutoffDate(FunctionCode, "00004", udtFormatter.convertDate(txtSubmissionDateFrom.Text, "E"), udtFormatter.convertDate(Me.txtSubmissionDateTo.Text, "E"))
                sm1 = udtValidator.chkInputValidFromDateCutoffDate(FunctionCode, "00004", udtFormatter.convertDate(strSubmissionDateFrom, "E"), udtFormatter.convertDate(strSubmissionDateTo, "E"))

                If Not IsNothing(sm1) Then
                    udcErrorBox.AddMessage(sm1)
                    imgAlertSubmissionDateFrom.Visible = True
                    MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.FillCriteria

                Else
                    BuildSearchCriteriaReview(DrillLevel.PaymentFile)

                    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                    'BuildPaymentFileGrid(String.Empty, txtSubmissionDateFrom.Text, txtSubmissionDateTo.Text, udtHCVUUserBLL.GetHCVUUser.UserID)
                    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
                    BuildPaymentFileGrid(String.Empty, strSubmissionDateFrom, strSubmissionDateTo, udtHCVUUserBLL.GetHCVUUser.UserID)

                    'lblRPaymentFileDateFrom.Text = udtFormatter.convertDate(txtSubmissionDateFrom.Text, String.Empty)
                    'lblRPaymentFileDateTo.Text = udtFormatter.convertDate(txtSubmissionDateTo.Text, String.Empty)
                    lblRPaymentFileDateFrom.Text = udtFormatter.convertDate(strSubmissionDateFrom, String.Empty)
                    lblRPaymentFileDateTo.Text = udtFormatter.convertDate(strSubmissionDateTo, String.Empty)

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Search Reimbursement successful")
                End If

                txtSubmissionDateFrom.Text = strSubmissionDateFrom
                txtSubmissionDateTo.Text = strSubmissionDateTo
            End If
            'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

            udcErrorBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00006, "Search Reimbursement fail")
            udcInfoBox.BuildMessageBox()

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Search Reimbursement fail")
            Throw ex
        End Try
    End Sub

    Private Sub BuildPaymentFileGrid(ByVal strReimbursementID As String, ByVal strFromDate As String, ByVal strToDate As String, ByVal strUserID As String)
        Dim dt As DataTable = Nothing

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        If rbtnLastFile.Checked Then
            ' Search by Reimbursment ID
            dt = udtReimbursementBLL.GetReimbursementPaymentFileLists(strReimbursementID, Nothing, Nothing, strUserID)
        Else
            ' Search by Submission Date
            dt = udtReimbursementBLL.GetReimbursementPaymentFileLists(String.Empty, udtFormatter.convertDate(strFromDate, String.Empty), udtFormatter.convertDate(strToDate, String.Empty) + " 23:59:59", strUserID)
        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        If dt.Rows.Count = 0 Then
            MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.FillCriteria
            BuildSearchCriteriaReview(DrillLevel.None)

            udcInfoBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
        Else
            ' CRE20-015-02 (Special Support Scheme) [Start][Winnie]
            gvPaymentFile.Columns(5).Visible = udtReimbursementBLL.IsHKDAvailable(dt) OrElse udtReimbursementBLL.IsHKDRMBAvailable(dt)
            gvPaymentFile.Columns(6).Visible = udtReimbursementBLL.IsRMBAvailable(dt) OrElse udtReimbursementBLL.IsHKDRMBAvailable(dt)
            ' CRE20-015-02 (Special Support Scheme) [End][Winnie]

            GridViewDataBind(gvPaymentFile, dt, "reimburseID", "DESC", False)

            Session("temp_dataGrid_enq") = dt

            gvPaymentFile.Visible = True
            MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.PaymentFileListing
            pnlSearchCriteriaReview.Visible = True

        End If

    End Sub

    '

    Protected Sub gvPaymentFile_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            ' Payment File
            Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(dr("Scheme_Code"))

            Dim lblCompletionTime As Label = e.Row.FindControl("lblCompletionTime")

            If udtSchemeClaim.ReimbursementMode = EnumReimbursementMode.All Then

                If lblCompletionTime.Text.Trim = String.Empty Then
                    e.Row.FindControl("img_processing").Visible = True
                    e.Row.FindControl("img_download").Visible = False
                Else
                    e.Row.FindControl("img_processing").Visible = False
                    e.Row.FindControl("img_download").Visible = True
                End If

                e.Row.FindControl("lblPaymentFileNA").Visible = False

            Else
                e.Row.FindControl("img_processing").Visible = False
                e.Row.FindControl("img_download").Visible = False

            End If
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            ' File Submission Time
            Dim lbl_submissionDate As Label = e.Row.FindControl("lbl_submissionDate")
            lbl_submissionDate.Text = udtFormatter.formatDateTime(lbl_submissionDate.Text.Trim)

            ' CRE20-015-02 (Special Support Scheme) [Start][Winnie]
            ' Amount Claimed (HKD) 
            Dim lblAmountClaimed As Label = e.Row.FindControl("lblAmountClaimed")

            If udtSchemeClaim.ReimbursementCurrency = EnumReimbursementCurrency.HKD OrElse _
                udtSchemeClaim.ReimbursementCurrency = EnumReimbursementCurrency.HKDRMB Then
                lblAmountClaimed.Text = udtFormatter.formatMoney(lblAmountClaimed.Text, False)

            Else
                lblAmountClaimed.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblAmountClaimed.Enabled = False
            End If

            ' Amount Claimed(RMB)
            Dim lblAmountClaimedRMB As Label = e.Row.FindControl("lblAmountClaimedRMB")

            If udtSchemeClaim.ReimbursementCurrency = EnumReimbursementCurrency.RMB OrElse _
                udtSchemeClaim.ReimbursementCurrency = EnumReimbursementCurrency.HKDRMB Then
                lblAmountClaimedRMB.Text = udtFormatter.formatMoneyRMB(lblAmountClaimedRMB.Text, False)

            Else                
                lblAmountClaimedRMB.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblAmountClaimedRMB.Enabled = False
            End If
            ' CRE20-015-02 (Special Support Scheme) [End][Winnie]

            ' Completion Time
            If lblCompletionTime.Text.Trim = String.Empty Then
                lblCompletionTime.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblCompletionTime.Enabled = False
            Else
                lblCompletionTime.Text = udtFormatter.formatDateTime(lblCompletionTime.Text.Trim)
            End If

            ' Detailed Payment Analysis Report
            ' CRE20-015-02 (Special Support Scheme) [Start][Winnie]
            If dr("Verification_Case_Available") = ReimbursementVerificationCaseAvailable.Available _
                AndAlso udtSchemeClaim.ReimbursementMode <> EnumReimbursementMode.HAFinance Then
                ' CRE20-015-02 (Special Support Scheme) [End][Winnie]
                e.Row.FindControl("div_EHCP").Visible = True
                e.Row.FindControl("div_Practice").Visible = True
            Else
                e.Row.FindControl("div_EHCP").Visible = False
                e.Row.FindControl("div_Practice").Visible = True
            End If
            ' CRE17-004 Generate a new DPAR on EHCP basis [End][Dickson]


        End If
    End Sub

    Protected Sub gvPaymentFile_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        udcErrorBox.Visible = False
        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

            Dim r As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)

            Dim strReimburseID As String = CType(r.FindControl("lbtn_reimburseID"), LinkButton).Text.Trim
            Dim strSchemeCode As String = CType(r.FindControl("hfSchemeCode"), HiddenField).Value.Trim
            Dim strPaymentFileTime As String = CType(r.Cells(2).FindControl("lbtn_submissionDate"), LinkButton).Text.Trim

            ' CRE17-004 Generate a new DPAR on EHCP basis [Start][Dickson]
            If e.CommandName.Equals("ReprintDPAReportEHCP") Then
                Try
                    udtAuditLogEntry.AddDescripton("Reimbursement ID", strReimburseID)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00019, "Reprint DPAR ECHP Basis Report")

                    ' Get the cutoff date from [ReimbursementAuthorisation] having [Authorised_Status] = 'R'
                    Dim dt As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(strReimburseID, ReimbursementStatus.Reimbursed, ReimbursementAuthorisationStatus.Active, strSchemeCode)
                    Dim strCutoffDate As String = udtFormatter.convertDate(dt.Rows(0)("CutOff_Date"))

                    ' Pass the data to the Viewer by Session variables
                    Session("RID") = strReimburseID
                    Session("strCutoffDate") = strCutoffDate
                    Session("bWatermark") = "Y"
                    Session("DPAScheme") = strSchemeCode
                    Session("ReportSelected") = DPAReportType.EHCP

                    ' CRE17-004 Generate a new DPAR on EHCP basis [Start][Martin]
                    Me.mpeDownload.Show()
                    lblReportType.Text = "Detailed Payment Analysis Report (on EHCP Basis)"
                    txtNewPassword.Focus()
                    udcDownloadErrorMessage.Clear()
                    'ScriptManager.RegisterStartupScript(Me, Page.GetType, FunctionCode, "window.open('DPAViewer.aspx','','width=' + (screen.width/1.5) + ',height=' + (screen.width/2) + ',resizable=yes')", True)
                    ' CRE17-004 Generate a new DPAR on EHCP basis [End][Martin]

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00020, "Reprint DPAR ECHP Basis Report successful")

                Catch ex As Exception
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Reprint DPAR ECHP Basis Report fail")
                    Throw ex
                End Try
            ElseIf e.CommandName.Equals("ReprintDPAReportPractice") Then
                Try
                    udtAuditLogEntry.AddDescripton("Reimbursement ID", strReimburseID)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00019, "Reprint DPAR Practice Basis Report")

                    ' Get the cutoff date from [ReimbursementAuthorisation] having [Authorised_Status] = 'R'
                    Dim dt As DataTable = udtReimbursementBLL.GetReimbursementAuthorisationByIDStatus(strReimburseID, ReimbursementStatus.Reimbursed, ReimbursementAuthorisationStatus.Active, strSchemeCode)
                    Dim strCutoffDate As String = udtFormatter.convertDate(dt.Rows(0)("CutOff_Date"))

                    ' Pass the data to the Viewer by Session variables
                    Session("RID") = strReimburseID
                    Session("strCutoffDate") = strCutoffDate
                    Session("bWatermark") = "Y"
                    Session("DPAScheme") = strSchemeCode
                    Session("ReportSelected") = DPAReportType.Practice

                    ' CRE17-004 Generate a new DPAR on EHCP basis [Start][Martin]
                    Me.mpeDownload.Show()
                    lblReportType.Text = "Detailed Payment Analysis Report (on Practice Basis)"
                    txtNewPassword.Focus()
                    udcDownloadErrorMessage.Clear()
                    ' ScriptManager.RegisterStartupScript(Me, Page.GetType, FunctionCode, "window.open('DPAViewer.aspx','','width=' + (screen.width/1.5) + ',height=' + (screen.width/2) + ',resizable=yes')", True)
                    ' CRE17-004 Generate a new DPAR on EHCP basis [End][Martin]

                    udtAuditLogEntry.WriteEndLog(LogID.LOG00020, "Reprint DPAR Practice Basis Report successful")

                Catch ex As Exception
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Reprint DPAR Practice Basis Report fail")
                    Throw ex
                End Try
                ' CRE17-004 Generate a new DPAR on EHCP basis [End][Dickson]
            Else
                Try
                    Dim udtSearchCriteria As SearchCriteria = Session(SESS_ReimbursementEnquirySearchCriteria)
                    udtSearchCriteria.SchemeCode = strSchemeCode
                    Session(SESS_ReimbursementEnquirySearchCriteria) = udtSearchCriteria

                    BuildSearchCriteriaReview(DrillLevel.SPID, strReimburseID, udtSearchCriteria)

                    udtAuditLogEntry.AddDescripton("ReimbursementID", strReimburseID)
                    udtAuditLogEntry.AddDescripton("Scheme", strSchemeCode)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Select Payment File")

                    Dim dt As DataTable = udtReimbursementBLL.GetAuthorizationSummaryBySP_PaymentFile(strReimburseID, strSchemeCode)

                    Session(SESS_ReimbursementEnquiryDrillSPIDDataTable) = dt

                    If dt.Rows.Count > 0 Then
                        gvDrillSPID.PageIndex = 0
                        ' CRE20-015-02 (Special Support Scheme) [Start][Winnie]
                        gvDrillSPID.Columns(5).Visible = udtReimbursementBLL.IsHKDAvailable(udtSearchCriteria.SchemeCode) OrElse udtReimbursementBLL.IsHKDRMBAvailable(udtSearchCriteria.SchemeCode)
                        gvDrillSPID.Columns(6).Visible = udtReimbursementBLL.IsRMBAvailable(udtSearchCriteria.SchemeCode) OrElse udtReimbursementBLL.IsHKDRMBAvailable(udtSearchCriteria.SchemeCode)
                        ' CRE20-015-02 (Special Support Scheme) [End][Winnie]

                        GridViewDataBind(gvDrillSPID, dt, "spNum", "ASC", False)
                        gvDrillSPID.Visible = True
                    End If

                    MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.DrillSPID
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Select Payment successful")

                Catch ex As Exception
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Select Payment fail")
                    Throw ex
                End Try
            End If
        End If
    End Sub

    Protected Sub gvPaymentFile_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, "temp_dataGrid_enq")
    End Sub

    Protected Sub gvPaymentFile_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, "temp_dataGrid_enq")
    End Sub

    Protected Sub gvPaymentFile_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, "temp_dataGrid_enq")
    End Sub

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00028, "Back Click")
        ' CRE11-021 log the missed essential information [End]

        BuildSearchCriteriaReview(DrillLevel.None)
        MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.FillCriteria

        rbtnLastFile.Checked = True
        ddlLastFile.SelectedIndex = 0
        rbtnSubmissionDate.Checked = False
    End Sub

    '

    Protected Sub gvDrillSPID_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' Amount Claimed (RMB)
            Dim lblAmountClaimedRMB As Label = e.Row.FindControl("lblAmountClaimedRMB")
            lblAmountClaimedRMB.Text = udtFormatter.formatMoneyRMB(lblAmountClaimedRMB.Text, False)

        End If
    End Sub

    Protected Sub gvDrillSPID_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Try
                Dim r As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
                Dim strSPID As String = CType(r.FindControl("lbtn_spID"), LinkButton).Text.Trim
                Dim strSPName As String = r.Cells(3).Text.Trim
                Dim strReimburseID As String = lblRReimburseID.Text.Trim

                ' CRE11-004
                Dim objAuditLogInfo As New AuditLogInfo(Left(strSPID, 8), Nothing, Nothing, Nothing, Nothing, Nothing)
                ' End CRE11-004

                udtAuditLogEntry.AddDescripton("Reimbursement ID", strReimburseID)
                udtAuditLogEntry.AddDescripton("SPID", strSPID)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00022, "Select SPID", objAuditLogInfo)

                Dim udtSearchCriteria As SearchCriteria = Session(SESS_ReimbursementEnquirySearchCriteria)
                udtSearchCriteria.ServiceProviderID = strSPID
                udtSearchCriteria.SPPracticeDisplaySeq = udtReimbursementBLL.ExtractPracticeDisplaySeqFromSPIDPracticeDisplaySeq(strSPID)
                udtSearchCriteria.ServiceProviderName = strSPName
                Session(SESS_ReimbursementEnquirySearchCriteria) = udtSearchCriteria

                BuildSearchCriteriaReview(DrillLevel.BankAccount, Nothing, udtSearchCriteria)

                Dim dt As DataTable = udtReimbursementBLL.GetAuthorizationSummaryByBankAcct_PaymentFile(udtSearchCriteria, strReimburseID, udtSearchCriteria.SchemeCode)
                Session(SESS_ReimbursementEnquiryDrillBankAccountDataTable) = dt

                gvDrillBankAccount.PageIndex = 0
                ' CRE20-015-02 (Special Support Scheme) [Start][Winnie]
                gvDrillBankAccount.Columns(5).Visible = udtReimbursementBLL.IsHKDAvailable(udtSearchCriteria.SchemeCode) OrElse udtReimbursementBLL.IsHKDRMBAvailable(udtSearchCriteria.SchemeCode)
                gvDrillBankAccount.Columns(6).Visible = udtReimbursementBLL.IsRMBAvailable(udtSearchCriteria.SchemeCode) OrElse udtReimbursementBLL.IsHKDRMBAvailable(udtSearchCriteria.SchemeCode)
                ' CRE20-015-02 (Special Support Scheme) [End][Winnie]

                GridViewDataBind(gvDrillBankAccount, dt, "bankAccount", "ASC", False)

                MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.DrillBankAccount

                udtAuditLogEntry.WriteEndLog(LogID.LOG00023, "Select SPID successful", objAuditLogInfo)

            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "Select SPID fail")
                Throw ex
            End Try
        End If
    End Sub

    Protected Sub gvDrillSPID_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, SESS_ReimbursementEnquiryDrillSPIDDataTable)
    End Sub

    Protected Sub gvDrillSPID_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, SESS_ReimbursementEnquiryDrillSPIDDataTable)
    End Sub

    Protected Sub gvDrillSPID_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_ReimbursementEnquiryDrillSPIDDataTable)
        Me.pnlSearchCriteriaReview.Visible = True
    End Sub

    Protected Sub ibtnSPIDBackToAuth_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        BackToAuthorizatePage()
    End Sub

    Protected Sub ibtnSPIDBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        BuildSearchCriteriaReview(DrillLevel.PaymentFile)
        MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.PaymentFileListing
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
            Dim lblAmountClaimedRMB As Label = e.Row.FindControl("lblAmountClaimedRMB")
            lblAmountClaimedRMB.Text = udtFormatter.formatMoneyRMB(lblAmountClaimedRMB.Text, False)

        End If
    End Sub

    Protected Sub gvDrillBankAccount_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then

            ' CRE11-004
            Dim objAuditLogInfo As New AuditLogInfo(Left(lblRSPID.Text.Trim, 8), Nothing, Nothing, Nothing, Nothing, Nothing)
            ' End CRE11-004

            Try
                Dim r As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)

                Dim strBankAccount As String = CType(r.Cells(1).FindControl("lbtn_bankAccNo"), LinkButton).Text.Trim
                Dim strBankAccountNoMask As String = CType(r.Cells(1).FindControl("lblOriBank"), Label).Text.Trim
                Dim strPractice As String = r.Cells(3).Text.Trim
                Dim strReimburseID As String = lblRReimburseID.Text.Trim
                Dim strSPID As String = lblRSPID.Text.Trim

                Dim udtSearchCriteria As SearchCriteria = Session(SESS_ReimbursementEnquirySearchCriteria)
                udtSearchCriteria.BankAcctNo = strBankAccountNoMask
                udtSearchCriteria.Practice = strPractice
                udtSearchCriteria.SPPracticeDisplaySeq = udtReimbursementBLL.ExtractPracticeDisplaySeqFromSPIDPracticeDisplaySeq(strSPID)
                Session(SESS_ReimbursementEnquirySearchCriteria) = udtSearchCriteria

                BuildSearchCriteriaReview(DrillLevel.Transaction, Nothing, udtSearchCriteria, strBankAccountNoMask)


                udtAuditLogEntry.AddDescripton("Reimbursement ID", strReimburseID)
                udtAuditLogEntry.AddDescripton("SPID", strSPID)
                udtAuditLogEntry.AddDescripton("Bank Account", strBankAccountNoMask)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00025, "Select Bank Account", objAuditLogInfo)

                Dim dt As DataTable = udtReimbursementBLL.GetAuthorizationSummaryByTxn_PaymentFile(udtSearchCriteria, strReimburseID, udtSearchCriteria.SchemeCode)
                Session(SESS_ReimbursementEnquiryDrillTransactionDataTable) = dt

                gvDrillTransaction.PageIndex = 0
                ' CRE20-015-02 (Special Support Scheme) [Start][Winnie]
                gvDrillTransaction.Columns(9).Visible = udtReimbursementBLL.IsHKDAvailable(udtSearchCriteria.SchemeCode) OrElse udtReimbursementBLL.IsHKDRMBAvailable(udtSearchCriteria.SchemeCode)
                gvDrillTransaction.Columns(10).Visible = udtReimbursementBLL.IsRMBAvailable(udtSearchCriteria.SchemeCode) OrElse udtReimbursementBLL.IsHKDRMBAvailable(udtSearchCriteria.SchemeCode)
                ' CRE20-015-02 (Special Support Scheme) [End][Winnie]

                GridViewDataBind(gvDrillTransaction, dt, "transNum", "ASC", False)

                MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.DrillTransaction

                udtAuditLogEntry.WriteEndLog(LogID.LOG00026, "Select Bank Account successful", objAuditLogInfo)

            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00027, "Select Bank Account fail", objAuditLogInfo)
                Throw ex
            End Try
        End If
    End Sub

    Protected Sub gvDrillBankAccount_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, SESS_ReimbursementEnquiryDrillBankAccountDataTable)
    End Sub

    Protected Sub gvDrillBankAccount_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, SESS_ReimbursementEnquiryDrillBankAccountDataTable)
    End Sub

    Protected Sub gvDrillBankAccount_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_ReimbursementEnquiryDrillBankAccountDataTable)
        Me.pnlSearchCriteriaReview.Visible = True
    End Sub

    Protected Sub ibtnBankAccountBackToAuth_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        BackToAuthorizatePage()
    End Sub

    Protected Sub ibtnBankAccountBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        BuildSearchCriteriaReview(DrillLevel.SPID)
        MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.DrillSPID
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
            Dim lblAmountClaimedRMB As Label = e.Row.FindControl("lblAmountClaimedRMB")
            lblAmountClaimedRMB.Text = udtFormatter.formatMoneyRMB(lblAmountClaimedRMB.Text, False)

        End If
    End Sub

    Protected Sub gvDrillTransaction_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then
            Try
                Dim r As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)

                Dim udtSearchCriteria As SearchCriteria = Session(SESS_ReimbursementEnquirySearchCriteria)
                udtSearchCriteria.TransNum = udtFormatter.formatSystemNumberReverse(CType(r.FindControl("lbtn_transNum"), LinkButton).Text)
                Session(SESS_ReimbursementEnquirySearchCriteria) = udtSearchCriteria
                
                ' CRE11-004
                Dim udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = udtClaimTransBLL.LoadClaimTran(udtSearchCriteria.TransNum, True)
                Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransactionModel)
                ' End CRE11-004

                udtAuditLogEntry.AddDescripton("Transaction No", udtSearchCriteria.TransNum)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Select Transaction", objAuditLogInfo)

                Dim dt As DataTable = Session(SESS_ReimbursementEnquiryDrillTransactionDataTable)
				
				' CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]
                LoadDetail(udtEHSTransactionModel.TransactionID, udtEHSTransactionModel)
                ' CRE11-004
                'ClaimTransDetail1.LoadTranInfo(udtEHSTransactionModel, udtSearchEngineBLL.SearchSuspendHistory(udtSearchCriteria))
                'ClaimTransDetail1.LoadTranInfo(udtClaimTransBLL.LoadClaimTran(udtSearchCriteria.TransNum, True), udtSearchEngineBLL.SearchSuspendHistory(udtSearchCriteria))
                ' End CRE11-004
				' CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]

                lblCurrentRecordNo.Text = gvDrillTransaction.PageIndex * gvDrillTransaction.PageSize + r.RowIndex + 1
                lblMaxRecordNo.Text = dt.Rows.Count

                pnlSearchCriteriaReview.Visible = False
				
				' CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]
                'MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.TransactionDetail
                ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]

                udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Select Transaction successful", objAuditLogInfo)

            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Select Transaction fail")
                Throw
            End Try
        End If
    End Sub

	
	' CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]
    Private Sub LoadDetail(ByVal strTransactionID As String, Optional ByVal udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = Nothing)
		If IsNothing(udtEHSTransactionModel) Then udtEHSTransactionModel = udtClaimTransBLL.LoadClaimTran(strTransactionID, True, True)
	
		Dim udtSearchCriteria As New SearchCriteria
		udtSearchCriteria.TransNum = strTransactionID
	
		Dim dtSuspendHistory As DataTable = udtSearchEngineBLL.SearchSuspendHistory(udtSearchCriteria)
	
		Dim udtSessionHandlerBLL As New BLL.SessionHandlerBLL
		udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransactionModel, FunctionCode)

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        ClaimTransDetail1.ShowHKICSymbol = True
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
		ClaimTransDetail1.LoadTranInfo(udtEHSTransactionModel, dtSuspendHistory)
	
		hfCurrentDetailTransactionNo.Value = strTransactionID
	
		MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.TransactionDetail
    End Sub
	' CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]

    Protected Sub gvDrillTransaction_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS_ReimbursementEnquiryDrillTransactionDataTable)
        pnlSearchCriteriaReview.Visible = True
    End Sub

    Protected Sub gvDrillTransaction_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_ReimbursementEnquiryDrillTransactionDataTable)
    End Sub

    Protected Sub gvDrillTransaction_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_ReimbursementEnquiryDrillTransactionDataTable)
    End Sub

    Protected Sub ibtnTransactionBackToAuth_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        BackToAuthorizatePage()
    End Sub

    Protected Sub ibtnTransactionBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        BuildSearchCriteriaReview(DrillLevel.BankAccount)
        MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.DrillBankAccount
    End Sub

    '

    Protected Sub ibtnDetailBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00029, "Detail Back Click")
        ' CRE11-021 log the missed essential information [End]

        pnlSearchCriteriaReview.Visible = True
        MultiViewReimbursementEnquiry.ActiveViewIndex = ViewIndex.DrillTransaction
    End Sub

    Protected Sub ibtnDetailPrevious_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        If CInt(lblCurrentRecordNo.Text) > 1 Then
            Dim intActualIndex As Integer

            Dim ee As New System.Web.UI.WebControls.GridViewPageEventArgs(udtReimbursementBLL.GetPageIndexInRecordNavigation(gvDrillTransaction.PageSize, CInt(lblCurrentRecordNo.Text) - 1))

            Me.GridViewPageIndexChangingHandler(gvDrillTransaction, ee, SESS_ReimbursementEnquiryDrillTransactionDataTable)
            Dim criteria As New Common.SearchCriteria.SearchCriteria
            Dim dt As New DataTable
            dt = Session(SESS_ReimbursementEnquiryDrillTransactionDataTable)
            intActualIndex = CInt(CType(gvDrillTransaction.Rows(CInt(Me.lblCurrentRecordNo.Text) - (gvDrillTransaction.PageSize * gvDrillTransaction.PageIndex) - 2).Cells(0).FindControl("lblRecordNum"), Label).Text.Trim) - 1
            criteria.TransNum = udtFormatter.formatSystemNumberReverse(dt.Rows(intActualIndex)(2).ToString)

            ' CRE11-004
            Dim udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = udtClaimTransBLL.LoadClaimTran(criteria.TransNum)
            Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransactionModel)
            ' End CRE11-004

            udtAuditLogEntry.AddDescripton("Transaction No", criteria.TransNum)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00013, "Previous record", objAuditLogInfo)
            Try
				' CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]
                LoadDetail(udtEHSTransactionModel.TransactionID, udtEHSTransactionModel)
                ' CRE11-004
                'Me.ClaimTransDetail1.LoadTranInfo(udtEHSTransactionModel, udtSearchEngineBLL.SearchSuspendHistory(criteria))
                'Me.ClaimTransDetail1.LoadTranInfo(udtClaimTransBLL.LoadClaimTran(criteria.TransNum), udtSearchEngineBLL.SearchSuspendHistory(criteria))
                ' End CRE11-004
				' CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]

                lblCurrentRecordNo.Text = CInt(lblCurrentRecordNo.Text) - 1
                udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Previous record successful", objAuditLogInfo)
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Previous record fail")
                Throw
            End Try
        End If
    End Sub

    Protected Sub ibtnDetailNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        If CInt(Me.lblCurrentRecordNo.Text) < CInt(lblMaxRecordNo.Text) Then
            Dim intActualIndex As Integer

            Dim ee As New System.Web.UI.WebControls.GridViewPageEventArgs(udtReimbursementBLL.GetPageIndexInRecordNavigation(gvDrillTransaction.PageSize, CInt(lblCurrentRecordNo.Text) + 1))

            Me.GridViewPageIndexChangingHandler(gvDrillTransaction, ee, SESS_ReimbursementEnquiryDrillTransactionDataTable)
            Dim criteria As New Common.SearchCriteria.SearchCriteria
            Dim dt As New DataTable
            dt = Session(SESS_ReimbursementEnquiryDrillTransactionDataTable)
            intActualIndex = CInt(CType(gvDrillTransaction.Rows(CInt(lblCurrentRecordNo.Text) - gvDrillTransaction.PageSize * gvDrillTransaction.PageIndex).Cells(0).FindControl("lblRecordNum"), Label).Text.Trim) - 1
            criteria.TransNum = udtFormatter.formatSystemNumberReverse(dt.Rows(intActualIndex)(2).ToString)

            ' CRE11-004
            Dim udtEHSTransactionModel As EHSTransaction.EHSTransactionModel = udtClaimTransBLL.LoadClaimTran(criteria.TransNum)
            Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransactionModel)
            ' End CRE11-004

            udtAuditLogEntry.AddDescripton("Transaction No", criteria.TransNum)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "Next record", objAuditLogInfo)
            Try
				' CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]
                LoadDetail(udtEHSTransactionModel.TransactionID, udtEHSTransactionModel)
                ' CRE11-004
                'Me.ClaimTransDetail1.LoadTranInfo(udtEHSTransactionModel, udtSearchEngineBLL.SearchSuspendHistory(criteria))
                'Me.ClaimTransDetail1.LoadTranInfo(udtClaimTransBLL.LoadClaimTran(criteria.TransNum), udtSearchEngineBLL.SearchSuspendHistory(criteria))
                ' End CRE11-004
				' CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]

                lblCurrentRecordNo.Text = CInt(lblCurrentRecordNo.Text) + 1
                udtAuditLogEntry.WriteEndLog(LogID.LOG00017, "Next record successful", objAuditLogInfo)

            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Next record fail")
                Throw
            End Try
        End If
    End Sub


#Region "Report download popup function"
    ' CRE17-004 Generate a new DPAR on EHCP basis [Start][Martin]
    Protected Sub ibtnDownload_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnDownload.Click

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00044, AuditLogDesc.DPAR_Download_Click)

        Try
            If Not udtValidator.IsEmpty(Me.txtNewPassword.Text) Then

                If udtValidator.ValidateFileDownloadPassword(Me.txtNewPassword.Text) Then

                    Session("ReportPassword") = txtNewPassword.Text
                    ScriptManager.RegisterStartupScript(Me, Page.GetType, FunctionCode, "window.open('DPAViewer.aspx','','width=' + (screen.width/1.5) + ',height=' + (screen.width/2) + ',resizable=yes')", True)

                    udtAuditLog.WriteEndLog(LogID.LOG00045, AuditLogDesc.DPAR_Download_Success)

                Else
                    udcDownloadErrorMessage.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00057)
                    Me.udcDownloadErrorMessage.BuildMessageBox("DownloadFail", udtAuditLog, LogID.LOG00046, AuditLogDesc.DPAR_Download_Fail & Session("PathError"))
                    Me.mpeDownload.Show()
                End If
            Else
                udcDownloadErrorMessage.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00057)
                Me.udcDownloadErrorMessage.BuildMessageBox("DownloadFail", udtAuditLog, LogID.LOG00046, AuditLogDesc.DPAR_Download_Fail & Session("PathError"))
                Me.mpeDownload.Show()
            End If

        Catch ex As Exception
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                             Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            udtAuditLog.AddDescripton("DownloadFailException", ex.ToString)
            Me.udcDownloadErrorMessage.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00447)
            Me.udcDownloadErrorMessage.BuildMessageBox("DownloadFail", udtAuditLog, LogID.LOG00046, AuditLogDesc.DPAR_Download_Fail & Session("PathError"))
            Me.mpeDownload.Show()
        End Try
    End Sub
#End Region
    ' CRE17-004 Generate a new DPAR on EHCP basis [End][Martin]


    ''' <summary>
    ''' CRE11-004
    ''' Handle working data on view change, clear working data if no use
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MultiViewReimbursementEnquiry_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MultiViewReimbursementEnquiry.ActiveViewChanged
        Select Case MultiViewReimbursementEnquiry.ActiveViewIndex
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

End Class