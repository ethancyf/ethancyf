Imports System.Web.Security.AntiXss
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSTransaction
Imports Common.Component.HCVUUser
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Format
Imports Common.SearchCriteria
Imports Common.Validation
Imports Common.Component.ClaimRules

Partial Public Class claimCreationApproval
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Inherits BasePageWithGridView
    Inherits BasePageWithControl
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

#Region "Private Classes"

    Private Class ViewIndex
        'Public Const InputCriteria As Integer = 0
        Public Const Transaction As Integer = 0
        Public Const Detail As Integer = 1
        Public Const Finish As Integer = 2

    End Class

    Private Class ViewIndexDetailAction
        Public Const ApproveReview As Integer = 0
        Public Const InputReason As Integer = 1
        Public Const ConfirmReason As Integer = 2
    End Class
#End Region

#Region "Fields"

    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtReimbursementBLL As New ReimbursementBLL
    Private udtFormatter As New Formatter


    Private udtDocTypeBLL As New DocTypeBLL
    Private udtEHSTransactionBLL As New EHSTransactionBLL
    Private udtGeneralFunction As New GeneralFunction
    Private udtSchemeClaimBLL As New SchemeClaimBLL
    Private udtSearchEngineBLL As New SearchEngineBLL
    Private udtSPProfileBLL As New SPProfileBLL
    Private udtUserRoleBLL As New UserRoleBLL
    Private udtValidator As New Validator

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Remove non reference object
    'Private udtOutsideClaimValidationBLL As New OutsideClaimValidationBLL
    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

#End Region

#Region "Session Constants"

    Private Const SESS_SearchCriteria As String = "010409_SearchCriteria"
    Private Const SESS_TransactionDataTable As String = "010409_TransactionDataTable"
    Private Const SESS_TransactionDetailTSMP As String = "010409_TransactionDetailTSMP"


#End Region

    Dim dtTransaction As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FunctionCode = FunctCode.FUNT010409
        If Not IsPostBack Then

            'Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT990000)
            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Claim Creation Approval Loaded")

            FunctionCode = FunctCode.FUNT010409
            RefreshGrid()
            MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.Transaction
   ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]
        Else
            If MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.Detail Then
                LoadDetail(hfCurrentDetailTransactionNo.Value)
            End If
   ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]
        End If

    End Sub

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
#Region "Abstract Method of [HCVU.BasePageWithControl]"
    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As AuditLogEntry) As Boolean
        Return True
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        If blnOverrideResultLimit Then
            Return GetTransaction(Nothing, True)
        Else
            Return GetTransaction()
        End If
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer
        Dim intRowCount As Integer

        Try
            dtTransaction = CType(udtBLLSearchResult.Data, DataTable)

        Catch ex As Exception
            Throw

        End Try

        intRowCount = dtTransaction.Rows.Count

        If dtTransaction.Columns.Count > 0 Then
            LoadTransactionGrid(dtTransaction)
        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        RefreshGrid(True)
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
        udcErrorMessage.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, "00016"))
        udcErrorMessage.BuildMessageBox("SearchFail")
    End Sub
#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Private Sub RefreshGrid()
    Private Sub RefreshGrid(Optional ByVal blnOverrideResultLimit As Boolean = False)
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
        'Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT990000)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00012, "Refresh Grid")

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Dim enumSearchResult As SearchResultEnum

        Try
            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, Nothing, udcInfoMessageBox, False, blnOverrideResultLimit)

        Catch eSQL As SqlClient.SqlException
            Throw eSQL

        Catch ex As Exception
            udcErrorMessage.AddMessage(FunctCode.FUNT010409, SeverityCode.SEVE, MsgCode.MSG00006)
            udcErrorMessage.BuildMessageBox("Warning")
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00013, "Load Claim Creation Approval Failed")

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                udtAuditLogEntry.AddDescripton("No of record", dtTransaction.Rows.Count)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Refresh Grid Successful")

            Case SearchResultEnum.ValidationFail
                ' No Validation
                Throw New Exception("Error: Class = [HCVU.claimCreationApproval], Method = [RefreshGrid], Message = The method - [SF_ValidateSearch] should not return [False]")

            Case SearchResultEnum.NoRecordFound
                udtAuditLogEntry.AddDescripton("No of record", dtTransaction.Rows.Count)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Refresh Grid Successful")

            Case SearchResultEnum.OverResultList1stLimit_PopUp
                udtAuditLogEntry.WriteEndLog(LogID.LOG00013, "Load Claim Creation Approval Failed")

            Case SearchResultEnum.OverResultList1stLimit_Alert
                udcErrorMessage.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, "00016"))
                udcErrorMessage.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00013, "Load Claim Creation Approval Failed")

            Case SearchResultEnum.OverResultListOverrideLimit
                udcErrorMessage.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, "00016"))
                udcErrorMessage.BuildMessageBox("SearchFail", udtAuditLogEntry, LogID.LOG00013, "Load Claim Creation Approval Failed")

            Case Else
                Throw New Exception("Error: Class = [HCVU.claimCreationApproval], Method = [RefreshGrid], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

        End Select

        'dtTransaction = GetTransaction()

        'If Not dtTransaction Is Nothing AndAlso dtTransaction.Columns.Count > 0 Then
        'LoadTransactionGrid(dtTransaction)

        'udtAuditLogEntry.AddDescripton("No of record", dtTransaction.Rows.Count)
        'udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Refresh Grid Successful")

        'If dtTransaction.Rows.Count = 0 Then
        'udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
        'udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
        'udcInfoMessageBox.BuildMessageBox()
        'End If
        'End If
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    End Sub

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Private Function GetTransaction(Optional ByVal udtSearchCriteria As SearchCriteria = Nothing) As DataTable
    Private Function GetTransaction(Optional ByVal udtSearchCriteria As SearchCriteria = Nothing, Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)

        If IsNothing(udtSearchCriteria) Then
            udtSearchCriteria = New SearchCriteria()

            udtSearchCriteria.TransStatus = "B"
            'udtSearchCriteria.TransStatus = "A"

            'udtSearchCriteria.ServiceProviderHKIC = udtFormatter.formatHKIDInternal(txtSPHKID.Text)
            'udtSearchCriteria.AuthorizedStatus = ddlAuthorizedStatus.SelectedValue

            'Dim aryDocumentNo As String() = txtEHealthDocNo.Text.Replace("(", "").Replace(")", "").Replace("-", "").Split("/")
            'If aryDocumentNo.Length > 1 Then
            '            udtSearchCriteria.DocumentNo1 = aryDocumentNo(0)
            'udtSearchCriteria.DocumentNo2 = aryDocumentNo(1)
            'Else
            'udtSearchCriteria.DocumentNo1 = aryDocumentNo(0)
            'udtSearchCriteria.DocumentNo2 = String.Empty
            'End If

            Session(SESS_SearchCriteria) = udtSearchCriteria

        End If
        'udtEHSTransactionBLL.LoadEHSTransaction(udtEHSAccount.TransactionID.Trim)
        'Dim dtTransaction As DataTable = udtReimbursementBLL.GetTransactionManualReimbursedByStatus(udtSearchCriteria, udtHCVUUserBLL.GetHCVUUser.UserID.Trim)

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Dim dtTransaction As New DataTable()

        'Try
        'dtTransaction = udtReimbursementBLL.GetTransactionManualReimbursedByStatus(udtSearchCriteria, udtHCVUUserBLL.GetHCVUUser.UserID.Trim)
        Return udtReimbursementBLL.GetTransactionManualReimbursedByStatus(FunctionCode, udtSearchCriteria, udtHCVUUserBLL.GetHCVUUser.UserID.Trim, blnOverrideResultLimit)

        'Catch eSQL As SqlClient.SqlException
        'If eSQL.Number = 50000 AndAlso eSQL.Message = "00009" Then
        'Me.udcErrorMessage.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, "00016"))
        'Me.udcErrorMessage.BuildMessageBox("SearchFail", udtAuditLogEntry, Common.Component.LogID.LOG00013, "Load Claim Creation Approval Failed")
        'Else
        'Throw eSQL
        'End If
        'Catch ex As Exception
        'udcErrorMessage.AddMessage(FunctCode.FUNT010409, SeverityCode.SEVE, MsgCode.MSG00006)
        'udcErrorMessage.BuildMessageBox("Warning")
        'udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)
        'End Try

        'Return dtTransaction
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
    End Function

    Private Sub LoadDetail(ByVal strTransactionNo As String)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Load Detail")

        ' I-CRE16-003 Fix XSS [Start][Lawrence]
        strTransactionNo = AntiXssEncoder.HtmlEncode(strTransactionNo, True)
        ' I-CRE16-003 Fix XSS [End][Lawrence]

        Try
            BuildClaimTransDetail(strTransactionNo)

            Dim udtSessionHandlerBLL As New BLL.SessionHandlerBLL

            ' CRE11-004      
            Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode))

            Dim dt As DataTable = Session(SESS_TransactionDataTable)

            MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.Detail

            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Load Detail end", objAuditLogInfo)
        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Load Detail fail")
            Throw
        End Try
    End Sub

    Private Sub LoadTransactionGrid(ByVal dt As DataTable)
        'GridViewDataBind(gvTransaction, dt, "transDate", "ASC", False)
        'dt.DefaultView.Sort = String.Format("{0} {1}", "Service_Receive_Dtm", "DESC")
        dt.DefaultView.Sort = "Service_Receive_Dtm DESC"
        GridViewDataBind(gvTransaction, dt, "Service_Receive_Dtm", "DESC", False)
        Session(SESS_TransactionDataTable) = dt
    End Sub

    Protected Sub gvTransaction_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS_TransactionDataTable)
    End Sub

    Protected Sub gvTransaction_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS_TransactionDataTable)
    End Sub

    Protected Sub gvTransaction_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS_TransactionDataTable)
    End Sub


    Protected Sub gvTransaction_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
            ' Transaction No.
            Dim lbtnTransactionNo As LinkButton = e.Row.FindControl("lbtn_transNum")
            lbtnTransactionNo.Text = udtFormatter.formatSystemNumber(lbtnTransactionNo.Text)

            ' Transaction Time
            'Dim lblGTransactionTime As Label = e.Row.FindControl("lblGTransactionTime")
            'lblGTransactionTime.Text = udtFormatter.formatDateTime(lblGTransactionTime.Text, String.Empty)

            ' Service Date
            Dim lblServiceReceiveDtm As Label = e.Row.FindControl("lblServiceReceiveDtm")
            'lblServiceReceiveDtm.Text = udtFormatter.formatDate(lblServiceReceiveDtm.Text)
            'lblServiceReceiveDtm.Text = udtFormatter.formatDateTime(lblServiceReceiveDtm.Text).Trim().Substring(0, lblServiceReceiveDtm.Text.Length - 9)
            lblServiceReceiveDtm.Text = udtFormatter.formatDateTime(lblServiceReceiveDtm.Text).Trim().Replace("00:00", "")

            ' Created Time
            Dim lblCreateDtm As Label = e.Row.FindControl("lblCreateDtm")
            lblCreateDtm.Text = udtFormatter.formatDateTime(lblCreateDtm.Text, String.Empty)

            '' Bank Account No.
            'Dim lblMaskedBankAccountNo As Label = e.Row.FindControl("lblMaskedBank")
            'Dim lblOriBankAccountNo As Label = e.Row.FindControl("lblOriBank")
            'lblMaskedBankAccountNo.Text = udtFormatter.maskBankAccount(lblOriBankAccountNo.Text)

            ' Creation Reason
            Dim lblCreationReason As Label = e.Row.FindControl("lblCreationReason")
            Dim udtStaticDataBLL As New StaticData.StaticDataBLL()
            Dim udtStaticDataModel As StaticData.StaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("ClaimCreationReason", lblCreationReason.Text.Trim)
            lblCreationReason.Text = udtStaticDataModel.DataValue

            ' CRE13-001 EHAPP [Start][Karl]
            ' -----------------------------------------------------------------------------------------
            'Total Amount
            Dim strTotalAmount As String
            Dim lblTotalAmount As Label            

            lblTotalAmount = CType(e.Row.FindControl("lblTotalAmount"), Label)

            If IsDBNull(dr.Item("totalAmount")) = True Then
                strTotalAmount = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
            Else
                strTotalAmount = CDbl(dr.Item("totalAmount")).ToString("#,##0")
            End If

            lblTotalAmount.Text = strTotalAmount
            ' CRE13-001 EHAPP [End][Karl]


            ' Override Reason
            Dim objOverrideReason As Object = dr.Item("Override_Reason")
            Dim strOverrideResaons As String

            If IsDBNull(objOverrideReason) Then
                strOverrideResaons = String.Empty
            Else
                strOverrideResaons = CStr(dr.Item("Override_Reason")).Trim
            End If

            Dim imgOverride As Image = e.Row.FindControl("imgOverride")
            If strOverrideResaons.Trim().Equals(String.Empty) Then
                imgOverride.Visible = False
            Else
                imgOverride.Visible = True
            End If
        End If

    End Sub

    Protected Sub gvTransaction_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        udcInfoMessageBox.Visible = False


        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.Equals("Sort")) Then
            Dim r As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            Dim strTransactionNo As String = CType(r.FindControl("hfTransactionNo"), HiddenField).Value.Trim

            Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Select Transaction")

            Try
                MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.ApproveReview
                LoadDetail(strTransactionNo)
                ' CRE11-004   
                Dim udtSessionHandlerBLL As New BLL.SessionHandlerBLL
                Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode))

                'ValidateRecord(False)
                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Select Transaction end", objAuditLogInfo)
            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
                udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Select Transaction fail")
                Throw
            End Try



        End If
    End Sub


    Private Sub BuildClaimTransDetail(ByVal strTransactionNo As String)
        Dim udtEHSTransaction As EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(strTransactionNo, True, True)
        'Dim udtEHSTransaction As EHSTransactionModel = udtEHSTransactionBLL.LoadEHSTransaction(strTransactionNo, True)

        Dim udtSearchCriteria As New SearchCriteria
        udtSearchCriteria.TransNum = strTransactionNo

        Dim dtSuspendHistory As DataTable = udtSearchEngineBLL.SearchSuspendHistory(udtSearchCriteria)

        Dim udtSessionHandlerBLL As New BLL.SessionHandlerBLL
        udtSessionHandlerBLL.EHSTransactionSaveToSession(udtEHSTransaction, FunctionCode)

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        udcClaimTransDetail.ShowHKICSymbol = True
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        udcClaimTransDetail.LoadTranInfo(udtEHSTransaction, dtSuspendHistory, True, True, True)

        ' CRE11-004      
        Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtEHSTransaction)
        ' End CRE11-004

        ' Save the current Transaction No to hidden field for the rebind in clicking action button (Suspend History)

        Session(SESS_TransactionDetailTSMP) = udtEHSTransaction.TSMP()
        hfCurrentDetailTransactionNo.Value = strTransactionNo

        ' display warning message'  
        'Dim udtOutsideClaimValidationBLL As New OutsideClaimValidationBLL
        'Dim udtEHSClaimVaccine As EHSClaimVaccine.EHSClaimVaccineModel

        ' Validate Transaction
        'Dim udtOutsideClaimValidationModel As OutsideClaimValidationModel = udtOutsideClaimValidationBLL.ValidateTransaction(udtEHSTransaction, udtEHSClaimVaccine)
        'Dim udtOutsideClaimBlockMessageCollection As OutsideClaimBlockMessageCollection = udtOutsideClaimValidationModel.BlockSystemMessage
        'Dim udtOutsideClaimWarningMessageCollection As OutsideClaimWarningMessageCollection = udtOutsideClaimValidationModel.WarningSystemMessage

        ' Add Error Message to display panel
        'For Each udtOutsideClaimBlockMessage As SystemMessage In udtOutsideClaimBlockMessageCollection
        'udcErrorMessage.AddMessage(udtOutsideClaimBlockMessage)
        'Next
        'For Each udtOutsideClaimWarningMessage As SystemMessage In udtOutsideClaimWarningMessageCollection
        'udcErrorMessage.AddMessage(udtOutsideClaimWarningMessage)
        'Next

        ' Add Warning Message if not exist
        'Dim udtOutsideClaimWarningMessageCollectionRead As OutsideClaimWarningMessageCollection
        ' udtOutsideClaimWarningMessageCollectionRead = udtOutsideClaimValidationModel.WarningSystemMessage
        'udtOutsideClaimWarningMessageCollectionRead = udtOutsideClaimValidationBLL.GetWarningMessageFromDBByTransactionID(udtEHSTransaction)
        'If Not udtOutsideClaimWarningMessageCollectionRead.Count > 0 Then
        '    udtOutsideClaimValidationBLL.AddWarningMessageToDBByCollection(udtEHSTransaction, udtOutsideClaimWarningMessageCollection)
        'End If

        'udtOutsideClaimWarningMessageCollectionRead = udtOutsideClaimValidationBLL.GetWarningMessageFromDBByTransactionID(udtEHSTransaction)
        'For Each udtOutsideClaimWarningMessage As SystemMessage In udtOutsideClaimWarningMessageCollectionRead
        '    udcErrorMessage.AddMessage(udtOutsideClaimWarningMessage)
        'Next

        'udcErrorMessage.BuildMessageBox("Warning")
        'end  display warning message'

    End Sub

    Protected Sub ibtnApproveReviewBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
        udtAuditLogEntry.WriteLog(LogID.LOG00018, "Detail Back Click")
        ' CRE11-021 log the missed essential information [End]


        ' return to transaction view from detail view    
        udcClaimTransDetail.ClearDocumentType()
        LoadDetail(hfCurrentDetailTransactionNo.Value)
        MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.Transaction
        udcInfoMessageBox.Clear()
        udcErrorMessage.Clear()
        'MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.ApproveReview
    End Sub

    Protected Sub ibtnInputReasonBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' return to transaction view from detail view
        udcInfoMessageBox.Clear()
        udcErrorMessage.Clear()
        udcClaimTransDetail.ClearDocumentType()
        LoadDetail(hfCurrentDetailTransactionNo.Value)
        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.ApproveReview
        ValidateRecord(False)
    End Sub


    Protected Sub ibtnReturn_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' return button on Finish view
        udcErrorMessage.Clear()
        udcInfoMessageBox.Clear()
        udcInfoMessageBox.Visible = False
        udcClaimTransDetail.ClearDocumentType()
        MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.Transaction
        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'RefreshGrid()
        RefreshGrid(True)
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
    End Sub

    Protected Sub ibtnApprove_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' confirm at step1
        udcErrorMessage.Clear()
        LoadDetail(hfCurrentDetailTransactionNo.Value)
        ValidateRecord(True)

    End Sub
    Private Function IsValidRecord()
        ' include the logic to valid record here
        'If hfCurrentDetailTransactionNo.Value.Trim() = "T09108000000102" Or hfCurrentDetailTransactionNo.Value.Trim() = "T09108000000113" Then
        'If hfCurrentDetailTransactionNo.Value.Trim() = "TH10707000000797" Then
        '    'TH10707000000797
        '    'TE10707000000236
        '    'TE10707000000224
        '    Return False
        'Else
        '    Return True
        'End If
        Return True
    End Function
    Private Sub ValidateRecord(ByVal blnEnterOverrideReasonShow As Boolean)
        ' if blnEnterOverrideReasonShow is false, will only show warning msg
        ' if blnEnterOverrideReasonShow is true, will show override reason textbox or popup

        If Not IsValidRecord() Then
            'invalid record
            ShowWarning(blnEnterOverrideReasonShow)
        Else
            'valid record
            If blnEnterOverrideReasonShow Then
                popupApprove.Show()
            End If
        End If
    End Sub


    Private Sub ShowWarning(ByVal blnEnterOverrideReasonShow As Boolean)
        lblReasonText.Text = Me.GetGlobalResourceObject("Text", "OverrideReason")
        lblConfirmReasonText.Text = Me.GetGlobalResourceObject("Text", "OverrideReason")
        udcErrorMessage.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00195)
        udcErrorMessage.BuildMessageBox("Warning")

        If blnEnterOverrideReasonShow Then
            ShowEnterOverrideReason()
        End If

    End Sub
    Private Sub ShowEnterOverrideReason()

        ' Switch to enter override reason view
        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.InputReason
        ' Override reason message
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
        udcInfoMessageBox.AddMessage(FunctCode.FUNT010409, SeverityCode.SEVE, MsgCode.MSG00003)
        udcInfoMessageBox.BuildMessageBox()
    End Sub


    Protected Sub ibtnInputReasonSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' confirm at step1 with reason
        If Not txtReason.Text.Trim().Length = 0 Then
            ' Override Reason is valid
            udcErrorMessage.Clear()
            imgAlertReason.Visible = False

            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            lblConfirmReason.Text = AntiXssEncoder.HtmlEncode(txtReason.Text, True)
            ' I-CRE16-003 Fix XSS [End][Lawrence]
            MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.ConfirmReason

            LoadDetail(hfCurrentDetailTransactionNo.Value)

            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.AddMessage(FunctCode.FUNT010409, SeverityCode.SEVI, MsgCode.MSG00002)
            udcInfoMessageBox.BuildMessageBox()

        Else
            LoadDetail(hfCurrentDetailTransactionNo.Value)
            imgAlertReason.Visible = True

        End If
    End Sub


    Protected Sub ibtnApproveConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' Press confirm on approve popup
        ConfirmStep2()
    End Sub

    Protected Sub ibtnConfirmReasonConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' Press confirm on review reason screen
        ConfirmStep2()
    End Sub

    Private Sub ConfirmStep2()
        udcErrorMessage.Visible = False
        Dim udtSessionHandlerBLL As New BLL.SessionHandlerBLL

        ' CRE11-004      
        Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode))
        ' End CRE11-004

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
        udtAuditLogEntry.AddDescripton("User ID", udtHCVUUserBLL.GetHCVUUser.UserID.Trim)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Confirm Transaction", objAuditLogInfo)

        Dim udtEHSTransactionBLL As New EHSTransactionBLL

        Dim udtEHSTransaction As EHSTransactionModel
        udtEHSTransaction = udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)

        Dim udtCurrentDtm As DateTime = Me.udtGeneralFunction.GetSystemDateTime

        Dim udtDB As New Common.DataAccess.Database

        ' CRE13-001 EHAPP [Start][Karl]
        ' -----------------------------------------------------------------------------------------
        Dim udtSchemeClaimModel As SchemeClaimModel
        Dim udtSchemeClaimBLL As New SchemeClaimBLL

        udtSchemeClaimModel = udtSchemeClaimBLL.getEffectiveSchemeClaim(udtEHSTransaction.SchemeCode)

        ' CRE13-001 EHAPP [End][Karl]


        Try
            Dim byteTSMP As Byte() = Session(SESS_TransactionDetailTSMP)
            'udtReimbursementBLL.UpdateClaimCreationApprovalStatus(hfCurrentDetailTransactionNo.Value, EHSTransactionModel.TransRecordStatusClass.Active, udtHCVUUserBLL.GetHCVUUser.UserID.Trim, byteTSMP)

            udtDB.BeginTransaction()

            'Update Record in Manual Reimbursement Table (Record Status => R)
            ' CRE13-001 EHAPP [Start][Karl]
            ' -----------------------------------------------------------------------------------------
            udtEHSTransactionBLL.UpdateManualReimburseApprove(udtEHSTransaction, udtHCVUUserBLL.GetHCVUUser.UserID.Trim, udtCurrentDtm, udtSchemeClaimModel, udtDB)
            'udtEHSTransactionBLL.UpdateManualReimburseApprove(udtEHSTransaction, udtHCVUUserBLL.GetHCVUUser.UserID.Trim, udtCurrentDtm, udtDB)
            ' CRE13-001 EHAPP [End][Karl]

            udtDB.CommitTransaction()


            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.AddMessage(FunctCode.FUNT010409, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMessageBox.BuildMessageBox()

            udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)

            udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Confirm Transaction end", objAuditLogInfo)

        Catch eSQL As SqlClient.SqlException
            udtDB.RollBackTranscation()
            If eSQL.Number = 50000 Then
                udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
                Me.udcErrorMessage.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))
                Me.udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00006, "Confirm Transaction fail", objAuditLogInfo)
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            udtDB.RollBackTranscation()
            udcErrorMessage.AddMessage(FunctCode.FUNT010409, SeverityCode.SEVE, MsgCode.MSG00006)
            udcErrorMessage.BuildMessageBox("Warning")
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)
            udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Confirm Transaction fail", objAuditLogInfo)
        End Try

        'RefreshGrid()

        'LoadDetail(hfCurrentDetailTransactionNo.Value)


        txtReason.Text = ""
        lblConfirmReason.Text = ""
        ' udcErrorMessage.Clear()
        MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.Finish
    End Sub


    Protected Sub ibtnConfirmReasonBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' Press Back on the review reason screen
        LoadDetail(hfCurrentDetailTransactionNo.Value)
        txtReason.Text = ""
        MultiViewDetailAction.ActiveViewIndex = ViewIndexDetailAction.InputReason
        ValidateRecord(True)

    End Sub


    Protected Sub ibtnApproveCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
        udtAuditLogEntry.WriteLog(LogID.LOG00016, "Approve Cancel Click")
        ' CRE11-021 log the missed essential information [End]

        ' Press cancel on approval popup
        LoadDetail(hfCurrentDetailTransactionNo.Value)
    End Sub

    Protected Sub ibtnRejectCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
        udtAuditLogEntry.WriteLog(LogID.LOG00017, "Reject Cancel Click")
        ' CRE11-021 log the missed essential information [End]

        LoadDetail(hfCurrentDetailTransactionNo.Value)
    End Sub

    Protected Sub ibtnReject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        LoadDetail(hfCurrentDetailTransactionNo.Value)
        popupReject.Show()
    End Sub

    Protected Sub ibtnRejectConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtSessionHandlerBLL As New BLL.SessionHandlerBLL

        ' CRE11-004      
        Dim objAuditLogInfo As AuditLogInfo = BuildAuditLogInfoWithTransaction(udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode))
        ' End CRE11-004

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
        udtAuditLogEntry.AddDescripton("User ID", udtHCVUUserBLL.GetHCVUUser.UserID.Trim())
        udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Reject Transaction", objAuditLogInfo)

        Dim udtEHSTransactionBLL As New EHSTransactionBLL

        Dim udtEHSTransaction As EHSTransactionModel
        udtEHSTransaction = udtSessionHandlerBLL.EHSTransactionGetFromSession(FunctionCode)

        Dim udtCurrentDtm As DateTime = Me.udtGeneralFunction.GetSystemDateTime

        Dim udtDB As New Common.DataAccess.Database



        Try
            Dim byteTSMP As Byte() = Session(SESS_TransactionDetailTSMP)
            'udtReimbursementBLL.UpdateClaimCreationApprovalStatus(hfCurrentDetailTransactionNo.Value, "D", udtHCVUUserBLL.GetHCVUUser.UserID.Trim, byteTSMP)

            udtDB.BeginTransaction()

            'Update Record in Manual Reimbursement Table (Record Status => R)
            udtEHSTransactionBLL.UpdateManualReimburseReject(udtEHSTransaction, udtHCVUUserBLL.GetHCVUUser.UserID.Trim, udtCurrentDtm, udtDB)

            udtDB.CommitTransaction()

            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.AddMessage(FunctCode.FUNT010409, SeverityCode.SEVI, MsgCode.MSG00005)
            udcInfoMessageBox.BuildMessageBox()
            udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Reject Transaction end", objAuditLogInfo)

            udtSessionHandlerBLL.EHSTransactionRemoveFromSession(FunctionCode)

        Catch eSQL As SqlClient.SqlException
            udtDB.RollBackTranscation()
            If eSQL.Number = 50000 Then
                Me.udcErrorMessage.AddMessage(New SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, eSQL.Message))
                udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
                Me.udcErrorMessage.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00009, "Reject Transaction fail", objAuditLogInfo)
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            udtDB.RollBackTranscation()
            udcErrorMessage.AddMessage(FunctCode.FUNT010409, SeverityCode.SEVE, MsgCode.MSG00006)
            udcErrorMessage.BuildMessageBox("Warning")
            udtAuditLogEntry.AddDescripton("Transaction No", hfCurrentDetailTransactionNo.Value)
            udtAuditLogEntry.AddDescripton("StackTrace", "Unknown Exception: " + ex.Message)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Reject Transaction fail", objAuditLogInfo)
        End Try
        'RefreshGrid()


        'udcErrorMessage.Clear()
        MultiViewClaimTransEnquiry.ActiveViewIndex = ViewIndex.Finish

    End Sub

    'Protected Sub ibtnRejectBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    LoadDetail(hfCurrentDetailTransactionNo.Value)
    '    udcInfoMessageBox.Clear()
    'End Sub

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
