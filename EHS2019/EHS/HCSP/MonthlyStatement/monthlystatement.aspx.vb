Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DataEntryUser
Imports Common.Component.Practice
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.SortedGridviewHeader
Imports Common.Component.UserAC
Imports Common.Component.ClaimCategory
Imports Common.Format
Imports HCSP.BLL

Partial Public Class monthlystatement
    Inherits BasePageWithGridView

    ' FunctionCode = FunctCode.FUNT020701

#Region "Private Classes"

    Private Class AuditLogDescription
        Public Const Load As String = "Monthly Statement load" '00000
        Public Const SearchClick As String = "Search click"
        Public Const SearchSuccessful As String = "Search successful"
        Public Const SearchCompleteNoRecordFound As String = "Search complete. No record found"
        Public Const SearchFail As String = "Search Fail" ' Not used actually
        Public Const ViewDetailClick As String = "View Detail click" '00005
        Public Const ViewScheme As String = "View Scheme"
        Public Const ViewTransaction As String = "View Transaction"
        Public Const ViewTransactionDetail As String = "View Transaction Detail"
        Public Const SchemeBackClick As String = "Scheme Back click"
        Public Const TransactionBackClick As String = "Transaction Back click" '00010
        Public Const TransactionDetailBackClick As String = "Transaction Detail Back click"
    End Class

    Private Class ViewIndex
        Public Const Summary As Integer = 0
        Public Const Scheme As Integer = 1
        Public Const Transaction As Integer = 2
        Public Const Detail As Integer = 3
    End Class

    <Serializable()> Private Class SubsidizeSummary
        Public SubsidizeCode As String
        Public TotalTransaction As Integer
        Public TotalUnit As Integer
        Public TotalAmount As Double = 0
        Public MthStatementDesc As String
        Public MthStatementDescChi As String
        Public MthStatementDescCN As String
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Public MthStatementUnitVisible As Boolean
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
    End Class

#End Region

#Region "Fields"

    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtPracticeBankAcctBLL As New PracticeBankAcctBLL
    Private udtReimbursementBLL As New ReimbursementBLL
    Private udtSchemeClaimBLL As New SchemeClaimBLL

#End Region

#Region "Session Constants"

    Private Const SESS_PracticeDropDownList As String = "020701_PracticeDropDownList"
    Private Const SESS_StatementDropDownList As String = "020701_StatementDropDownList"
    Private Const SESS_SummaryDataTable As String = "020701_SummaryDataTable"
    Private Const SESS_StatementDataSet As String = "020701_StatementDataSet"
    Private Const SESS_SchemeDataTable As String = "020701_SchemeDataTable"
    Private Const SESS_TransactionDataTable As String = "020701_TransactionDataTable"
    Private Const SESS_TransactionNo As String = "020701_TransactionNo"
    Private Const SESS_EHSTransaction As String = "020701_EHSTransaction"

    Private Const AvailableItemDescInjection As String = "Injection"

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        Me.PreCheckConcurrentAccessForHttpPost()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        ' Get Current User Account for check Session Expired
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC

        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT020701

            ResetControls()

            Dim udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, AuditLogDescription.Load)
        End If

        ReRenderPage()

        Dim udtSP As ServiceProviderModel = UserACBLL.GetUserAC

        If ddlStatement.SelectedValue = "-1" Then
            ibtnSearch.Enabled = False
            ibtnSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtnDisabled")

            ddlStatement.Items.Clear()
            ddlStatement.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "N/A"), "-1"))
            ddlStatement.SelectedIndex = 0

        Else
            ibtnSearch.Enabled = True
            ibtnSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtn")

            Dim dtBankInDateList As DataTable = udtReimbursementBLL.GetBankInDateList(udtSP.SPID, CInt(Me.ddlBankAccount.SelectedValue))
            Session(SESS_StatementDropDownList) = dtBankInDateList

            ddlStatement.DataSource = dtBankInDateList

        End If

        If Session("language") = TradChinese OrElse Session("language") = SimpChinese Then
            If udtSP.ChineseName.Trim = String.Empty Then
                lblSPEngName.Text = udtSP.EnglishName
                lblSPEngName.CssClass = "boldText"
            Else
                lblSPEngName.Text = udtSP.ChineseName
                lblSPEngName.CssClass = "boldTextChi"

            End If

            lblStatementHeader2.Text = ddlStatement.SelectedItem.Text.Trim + Me.GetGlobalResourceObject("Text", "MonthlyStatementAsOf")

        Else
            lblSPEngName.Text = udtSP.EnglishName
            lblSPEngName.CssClass = "boldText"
            lblStatementHeader2.Text = Me.GetGlobalResourceObject("Text", "MonthlyStatementAsOf") + " " + ddlStatement.SelectedItem.Text.Trim
        End If

        Select Case MultiViewMonthlyStatement.ActiveViewIndex
            Case ViewIndex.Summary
                If Not IsNothing(Session(SESS_StatementDataSet)) Then BuildDynamicStatement(Session(SESS_StatementDataSet))

            Case ViewIndex.Detail
                BuildDetail()

        End Select

    End Sub

    Private Sub ResetControls()
        Dim udtSP As ServiceProviderModel = CType(UserACBLL.GetUserAC, ServiceProviderModel)

        Dim dtPracticeList As DataTable = udtPracticeBankAcctBLL.getAllPractice(udtSP.SPID, PracticeBankAcctBLL.PracticeDisplayType.Practice)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        FilterPracticeList(dtPracticeList, udtSP.PracticeList)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Session(SESS_PracticeDropDownList) = dtPracticeList

        ddlBankAccount.DataSource = dtPracticeList

        If Session("language") = TradChinese OrElse Session("language") = SimpChinese Then
            ddlBankAccount.DataTextField = PracticeBankAcctBLL.PracticeDisplayField.Display_Chi
            ddlBankAccount.CssClass = "textChi"
        Else
            ddlBankAccount.DataTextField = PracticeBankAcctBLL.PracticeDisplayField.Display_Eng
            ddlBankAccount.CssClass = String.Empty
        End If

        ddlBankAccount.DataBind()

        LoadStatementByPractice(CInt(ddlBankAccount.SelectedValue))

    End Sub

    Private Sub ReRenderPage()
        ' Handle Practice Bank Change Language
        Dim strPracticeBankSelected As String = ddlBankAccount.SelectedValue
        Dim dtPracticeList As DataTable = Session(SESS_PracticeDropDownList)

        ddlBankAccount.Items.Clear()
        ddlBankAccount.DataSource = dtPracticeList

        If Session("language") = TradChinese OrElse Session("language") = SimpChinese Then
            ddlBankAccount.DataTextField = BLL.PracticeBankAcctBLL.PracticeDisplayField.Display_Chi
            ddlBankAccount.CssClass = "textChi"
        Else
            ddlBankAccount.DataTextField = BLL.PracticeBankAcctBLL.PracticeDisplayField.Display_Eng
            ddlBankAccount.CssClass = String.Empty
        End If

        ddlBankAccount.DataBind()
        ddlBankAccount.SelectedValue = strPracticeBankSelected

        Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
        If Not controlID Is Nothing AndAlso (controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) OrElse controlID.Equals(SelectSimpChinese)) Then
            Dim dtBankInDateList As DataTable
            Dim intSelectedIndex As Integer
            dtBankInDateList = Session(SESS_StatementDropDownList)

            If dtBankInDateList.Rows.Count > 0 Then
                intSelectedIndex = Me.ddlStatement.SelectedIndex
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubPlatformBLL As New SubPlatformBLL
                'Me.ddlStatement.DataSource = udtReimbursementBLL.FormatStatementList(dtBankInDateList)
                Me.ddlStatement.DataSource = udtReimbursementBLL.FormatStatementList(dtBankInDateList, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                Me.ddlStatement.DataBind()

                Me.ddlStatement.SelectedIndex = intSelectedIndex
            End If

            If Session("language") = TradChinese OrElse Session("language") = SimpChinese Then
                ddlBankAccount.CssClass = "textChi"
            Else
                ddlBankAccount.CssClass = String.Empty
            End If

            ' ViewIndex.Summary
            If Not ViewState("dtmReimbursementDate") Is Nothing Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubPlatformBLL As New SubPlatformBLL
                'lblStatementIssueDate.Text = udtFormatter.formatDate(CType(ViewState("dtmReimbursementDate"), DateTime))
                lblStatementIssueDate.Text = udtFormatter.formatDisplayDate(CType(ViewState("dtmReimbursementDate"), DateTime), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If

            If (Session("language") = TradChinese OrElse Session("language") = SimpChinese) AndAlso lblPracticeName.Text <> lblPracticeName_Chi.Text Then
                lblPracticeName_Chi.Visible = True
                lblPracticeName.Visible = False
            Else
                lblPracticeName.Visible = True
                lblPracticeName_Chi.Visible = False
            End If

            Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithSubsidizeGroup()

            If Not IsNothing(Session(SESS_StatementDataSet)) Then BuildDynamicStatement(Session(SESS_StatementDataSet))

            ' ViewIndex.Scheme
            If (Session("language") = TradChinese OrElse Session("language") = SimpChinese) AndAlso lblSPractice.Text <> lblSPractice_Chi.Text Then
                lblSPractice_Chi.Visible = True
                lblSPractice.Visible = False
            Else
                lblSPractice.Visible = True
                lblSPractice_Chi.Visible = False
            End If

            lblSStatement.Text = GetSelectedValueInDropDownList(ddlStatement, CStr(ViewState("ReimburseID")))

            ' ViewIndex.Transaction
            If gvTransaction.Rows.Count > 0 Then
                GridViewDataBind(gvTransaction, Session(SESS_TransactionDataTable))
            End If

            If (Session("language") = TradChinese OrElse Session("language") = SimpChinese) AndAlso lblTPractice.Text <> lblTPractice_Chi.Text Then
                lblTPractice_Chi.Visible = True
                lblTPractice.Visible = False
            Else
                lblTPractice.Visible = True
                lblTPractice_Chi.Visible = False
            End If

            lblTStatement.Text = GetSelectedValueInDropDownList(ddlStatement, CStr(ViewState("ReimburseID")))

        End If

        udcClaimTranEnquiry.chgLanguage()

    End Sub

#End Region

    Private Sub BuildDetail()
        udcClaimTranEnquiry.buildClaimObject(Session(SESS_TransactionNo), Nothing, True)
        udcClaimTranEnquiry.chgLanguage()
    End Sub

    '

    Protected Sub ddlBankAccount_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        panSummary.Visible = False
        ibtnViewDetails.Visible = False
        LoadStatementByPractice(CInt(ddlBankAccount.SelectedValue))
    End Sub

    Protected Sub ddlStatement_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        panSummary.Visible = False
        ibtnViewDetails.Visible = False
    End Sub

    Private Sub LoadStatementByPractice(ByVal intPracticeDisplaySeq As Integer)
        Dim udtSP As ServiceProviderModel = UserACBLL.GetUserAC

        Dim dtBankInDateList As DataTable = udtReimbursementBLL.GetBankInDateList(udtSP.SPID, intPracticeDisplaySeq)
        Session(SESS_StatementDropDownList) = dtBankInDateList

        If dtBankInDateList.Rows.Count > 0 Then
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL
            'ddlStatement.DataSource = udtReimbursementBLL.FormatStatementList(dtBankInDateList)
            ddlStatement.DataSource = udtReimbursementBLL.FormatStatementList(dtBankInDateList, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ddlStatement.DataBind()

            ddlStatement.SelectedIndex = 0

            ibtnSearch.Enabled = True
            ibtnSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtn")

        Else
            ddlStatement.Items.Clear()
            ddlStatement.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "N/A"), "-1"))
            ddlStatement.SelectedIndex = 0

            ibtnSearch.Enabled = False
            ibtnSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtnDisabled")
        End If

    End Sub

    '

    Protected Sub ibtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearch.Click
        Dim udtSP As ServiceProviderModel = UserACBLL.GetUserAC

        Dim strSPID As String = udtSP.SPID
        Dim intPracticeDisplaySeq As Integer = ddlBankAccount.SelectedValue.Trim
        Dim strReimburseID As String = ddlStatement.SelectedValue.Trim

        Dim udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("SPID", strSPID)
        udtAuditLogEntry.AddDescripton("Practice Seq", intPracticeDisplaySeq)
        udtAuditLogEntry.AddDescripton("Statement Cutoff Date", ddlStatement.SelectedItem.Text.Trim)
        udtAuditLogEntry.AddDescripton("Reimbursement ID", strReimburseID)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, AuditLogDescription.SearchClick)

        ' Clear the session of SchemeDictionary
        Session(SESS_StatementDataSet) = Nothing

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim enumSubPlatform As [Enum] = Me.SubPlatform()
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        ' Search monthly statement
        Dim ds As DataSet = udtReimbursementBLL.GetMonthlyReimbursementStatement(strSPID, intPracticeDisplaySeq, strReimburseID, enumSubPlatform)
        Session(SESS_SummaryDataTable) = ds.Tables(1)

        If ds.Tables(0).Rows.Count = 0 Then
            panSummary.Visible = False
            ibtnViewDetails.Visible = False

            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, AuditLogDescription.SearchCompleteNoRecordFound)

        Else
            ViewState("PracticeDisplaySeq") = intPracticeDisplaySeq
            ViewState("ReimburseID") = strReimburseID

            panSummary.Visible = True
            ibtnViewDetails.Visible = True

            If Session("language") = TradChinese OrElse Session("language") = SimpChinese Then
                ' Heading2
                lblStatementHeader2.Text = ddlStatement.SelectedItem.Text.Trim + Me.GetGlobalResourceObject("Text", "MonthlyStatementAsOf")

                ' Service Provider Name
                If udtSP.ChineseName.Trim = String.Empty Then
                    lblSPEngName.Text = udtSP.EnglishName
                    lblSPEngName.CssClass = "boldText"
                Else
                    lblSPEngName.Text = udtSP.ChineseName
                    lblSPEngName.CssClass = "boldTextChi"
                End If

            Else
                ' Heading2
                lblStatementHeader2.Text = Me.GetGlobalResourceObject("Text", "MonthlyStatementAsOf") + " " + ddlStatement.SelectedItem.Text.Trim

                ' Service Provider Name
                lblSPEngName.Text = udtSP.EnglishName
                lblSPEngName.CssClass = "boldText"
            End If

            ' Service Provider ID
            lblSPID.Text = udtSP.SPID

            ' Practice No.
            lblPracticeNo.Text = intPracticeDisplaySeq

            Dim drStatementHeader As DataRow = ds.Tables(0).Rows(0)

            ' Statement Issue Date
            Dim dtmReimbursementDate As DateTime = drStatementHeader("Statement_Issue_Date")
            ViewState("dtmReimbursementDate") = dtmReimbursementDate

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL

            'lblStatementIssueDate.Text = udtFormatter.formatDate(dtmReimbursementDate)
            lblStatementIssueDate.Text = udtFormatter.formatDisplayDate(dtmReimbursementDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ' Name of Practice
            lblPracticeName.Text = drStatementHeader("Practice_Name").ToString.Trim
            lblPracticeName_Chi.Text = lblPracticeName.Text

            Dim strParmValue As String = String.Empty
            udtGeneralFunction.getSystemParameter("HCSPDataMirgrationCompleteTurnOn", strParmValue, Nothing)

            If strParmValue.Trim = "Y" Then
                If Not IsDBNull(drStatementHeader("Practice_Name_Chi")) AndAlso CStr(drStatementHeader("Practice_Name_Chi")).Trim <> String.Empty Then
                    lblPracticeName_Chi.Text = CStr(drStatementHeader("Practice_Name_Chi")).Trim
                End If
            End If

            If (Session("language") = TradChinese OrElse Session("language") = SimpChinese) AndAlso lblPracticeName.Text <> lblPracticeName_Chi.Text Then
                lblPracticeName_Chi.Visible = True
                lblPracticeName.Visible = False
            Else
                lblPracticeName.Visible = True
                lblPracticeName_Chi.Visible = False
            End If

            ' Bank Account No.
            lblBankAccount.Text = udtFormatter.maskBankAccount(drStatementHeader("Bank_Account_No").ToString.Trim)

            ' Bank Account Name
            lblBankAccountName.Text = drStatementHeader("Bank_Acc_Holder").ToString.Trim

            lblBankAccountName.Visible = True

            If Session("language") = TradChinese OrElse Session("language") = SimpChinese Then
                Me.lblSPEngName.CssClass = "boldTextChi"

            Else
                Me.lblSPEngName.CssClass = "boldText"

            End If

            ' Scheme summary
            Session(SESS_StatementDataSet) = ds
            BuildDynamicStatement(ds)

            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AuditLogDescription.SearchSuccessful)

            ' Set focus to the View Details button
            Me._ScriptManager.SetFocus(ibtnViewDetails)

        End If

    End Sub

    '

    Protected Sub ibtnViewDetails_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnViewDetails.Click
        Dim udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00003, AuditLogDescription.ViewDetailClick)

        MultiViewMonthlyStatement.ActiveViewIndex = ViewIndex.Scheme

        Dim intPracticeDisplaySeq As Integer = CInt(ViewState("PracticeDisplaySeq"))
        Dim strReimburseID As String = CStr(ViewState("ReimburseID"))
        Dim udtSP As ServiceProviderModel = UserACBLL.GetUserAC

        Dim dtScheme As DataTable = BuildSchemeDataTable(Session(SESS_SummaryDataTable))
        Session(SESS_SchemeDataTable) = dtScheme

        ' Practice
        lblSPractice.Text = lblPracticeName.Text
        lblSPractice_Chi.Text = lblPracticeName_Chi.Text

        If (Session("language") = TradChinese OrElse Session("language") = SimpChinese) AndAlso lblSPractice.Text <> lblSPractice_Chi.Text Then
            lblSPractice_Chi.Visible = True
            lblSPractice.Visible = False
        Else
            lblSPractice.Visible = True
            lblSPractice_Chi.Visible = False
        End If

        ' Statement
        lblSStatement.Text = GetSelectedValueInDropDownList(ddlStatement, strReimburseID)

        If dtScheme.Rows.Count = 1 Then
            Dim drScheme As DataRow = dtScheme.Rows(0)
            BuildGvTransaction(CStr(drScheme("Scheme_Code")).Trim, CStr(drScheme("Display_Code")).Trim)

        Else
            GridViewDataBind(gvScheme, dtScheme, "Display_Seq", "ASC", False)

            udtAuditLogEntry.WriteLog(LogID.LOG00006, AuditLogDescription.ViewScheme)

        End If

    End Sub

    Private Function BuildSchemeDataTable(ByVal dtSummary As DataTable) As DataTable
        Dim dtScheme As New DataTable
        dtScheme.Columns.Add("Scheme_Code", GetType(String))
        dtScheme.Columns.Add("Display_Code", GetType(String))
        dtScheme.Columns.Add("Display_Seq", GetType(Integer))
        dtScheme.Columns.Add("No_of_Transaction", GetType(Integer))
        dtScheme.Columns.Add("Total_Amount", GetType(Double))

        Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithSubsidizeGroup()

        For Each drSummary As DataRow In dtSummary.Select(String.Empty, "Scheme_Display_Seq")
            Dim drScheme As DataRow = dtScheme.NewRow()
            drScheme("Scheme_Code") = CStr(drSummary("Scheme_Code")).Trim
            drScheme("Display_Code") = udtSchemeCList.Filter(CStr(drSummary("Scheme_Code")).Trim).DisplayCode
            drScheme("Display_Seq") = CInt(drSummary("Scheme_Display_Seq"))
            drScheme("No_of_Transaction") = CInt(drSummary("Transaction_Count"))
            drScheme("Total_Amount") = CDbl(drSummary("Transaction_Amount"))
            dtScheme.Rows.Add(drScheme)

        Next

        Return dtScheme

    End Function

    Private Function SearchRowInDtScheme(ByVal dtScheme As DataTable, ByVal strSchemeCode As String) As DataRow
        For Each drScheme As DataRow In dtScheme.Rows
            If CStr(drScheme("Scheme_Code")).Trim = strSchemeCode Then Return drScheme
        Next

        Return Nothing
    End Function

    Private Function GetSelectedValueInDropDownList(ByVal ddlMonthlyStatement As DropDownList, ByVal strValue As String) As String
        For i As Integer = 0 To ddlMonthlyStatement.Items.Count - 1
            If ddlMonthlyStatement.Items(i).Value.Trim = strValue Then
                Return ddlMonthlyStatement.Items(i).Text
            End If
        Next

        Return String.Empty

    End Function

    '

    Protected Sub gvScheme_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvScheme.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then
            Dim strSchemeCode As String = e.CommandArgument.ToString.Trim
            Dim strDisplayCode As String = CType(e.CommandSource, LinkButton).Text.Trim

            BuildGvTransaction(strSchemeCode, strDisplayCode)

        End If
    End Sub

    Protected Sub gvScheme_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvScheme.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(1, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvScheme_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvScheme.RowDataBound
        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Chris YIM]
        ' ----------------------------------------------------------------------------------------
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblTotalAmount As Label
            lblTotalAmount = CType(e.Row.FindControl("lblTotalAmount"), Label)

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

                lblTotalAmount.Text = udtFormatter.formatMoney(dr.Item("Total_Amount").ToString, False)
            End If
        End If
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Chris YIM]
    End Sub

    Protected Sub gvScheme_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvScheme.PageIndexChanging
        GridViewPageIndexChangingHandler(sender, e, SESS_SchemeDataTable)
    End Sub

    Protected Sub gvScheme_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvScheme.PreRender
        GridViewPreRenderHandler(sender, e, SESS_SchemeDataTable)
    End Sub

    Protected Sub gvScheme_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvScheme.Sorting
        GridViewSortingHandler(sender, e, SESS_SchemeDataTable)
    End Sub

    Private Sub BuildGvTransaction(ByVal strSchemeCode As String, ByVal strDisplayCode As String)
        MultiViewMonthlyStatement.ActiveViewIndex = ViewIndex.Transaction

        Dim intPracticeDisplaySeq As Integer = CInt(ViewState("PracticeDisplaySeq"))
        Dim strReimburseID As String = CStr(ViewState("ReimburseID"))
        Dim udtSP As ServiceProviderModel = UserACBLL.GetUserAC

        Dim dtTransaction As DataTable = udtReimbursementBLL.GetMonthlyReimbursementStatementDetails(udtSP.SPID, intPracticeDisplaySeq, strReimburseID, strSchemeCode)
        Session(SESS_TransactionDataTable) = dtTransaction

        GridViewDataBind(gvTransaction, dtTransaction, "Transaction_Dtm", "ASC", False)

        ' Practice
        lblTPractice.Text = lblSPractice.Text
        lblTPractice_Chi.Text = lblSPractice_Chi.Text

        If (Session("language") = TradChinese OrElse Session("language") = SimpChinese) AndAlso lblTPractice.Text <> lblTPractice_Chi.Text Then
            lblTPractice_Chi.Visible = True
            lblTPractice.Visible = False
        Else
            lblTPractice.Visible = True
            lblTPractice_Chi.Visible = False
        End If

        ' Statement
        lblTStatement.Text = lblSStatement.Text

        ' Scheme
        lblTScheme.Text = strDisplayCode

        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Scheme", strSchemeCode)
        udtAuditLogEntry.WriteLog(LogID.LOG00007, AuditLogDescription.ViewTransaction)

    End Sub

    '

    Protected Sub gvTransaction_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTransaction.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            ' Transaction No.
            Dim lbtnTransactionID As LinkButton = e.Row.FindControl("lbtnTransactionID")
            lbtnTransactionID.Text = udtFormatter.formatSystemNumber(CStr(dr.Item("Transaction_ID")).Trim)

            ' Transaction Time
            Dim lblTransactionDtm As Label = e.Row.FindControl("lblTransactionDtm")
            lblTransactionDtm.Text = udtFormatter.formatDateTime(CType(dr.Item("Transaction_Dtm"), DateTime))

            ' Service Date
            Dim lblServiceDate As Label = e.Row.FindControl("lblServiceDate")
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL
            'lblServiceDate.Text = udtFormatter.formatDate(CType(dr.Item("Service_Receive_Dtm"), DateTime))
            lblServiceDate.Text = udtFormatter.formatDisplayDate(CType(dr.Item("Service_Receive_Dtm"), DateTime), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ' Document Type, Identity Document No., Name
            Dim hfInvalidation As HiddenField = e.Row.FindControl("hfInvalidation")
            Dim lblDocCode As Label = e.Row.FindControl("lblDocCode")
            Dim lblIDNo As Label = e.Row.FindControl("lblIDNo")
            Dim lblName As Label = e.Row.FindControl("lblName")
            Dim lblNameChi As Label = e.Row.FindControl("lblNameChi")

            If hfInvalidation.Value.Trim = Common.Component.EHSTransaction.EHSTransactionModel.InvalidationStatusClass.Invalidated Then
                If LCase(Session("language")) = "zh-tw" Then
                    lblDocCode.Text = Me.GetGlobalResourceObject("Text", "ChiN/A")
                    lblIDNo.Text = Me.GetGlobalResourceObject("Text", "ChiN/A")
                    lblName.Text = Me.GetGlobalResourceObject("Text", "ChiN/A")
                Else
                    lblDocCode.Text = Me.GetGlobalResourceObject("Text", "EngN/A")
                    lblIDNo.Text = Me.GetGlobalResourceObject("Text", "EngN/A")
                    lblName.Text = Me.GetGlobalResourceObject("Text", "EngN/A")
                End If

                lblNameChi.Visible = False

            Else
                ' Identity Document No.
                Dim hfDocCode As HiddenField = e.Row.FindControl("hfDocCode")
                Dim hfIDNo As HiddenField = e.Row.FindControl("hfIDNo")
                Dim hfIDNo2 As HiddenField = e.Row.FindControl("hfIDNo2")

                lblIDNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(hfDocCode.Value.Trim, hfIDNo.Value.Trim, True, hfIDNo2.Value.Trim)

                ' Name
                lblName.Text = CStr(dr.Item("Eng_Name")).Trim
                Dim strChiName As String = CStr(IIf(IsDBNull(dr("Chi_Name")), String.Empty, dr("Chi_Name"))).Trim

                If strChiName.Trim <> String.Empty Then
                    lblNameChi.Text = String.Format("({0})", strChiName)
                Else
                    lblNameChi.Visible = False
                End If

            End If

            ' Total Amount ($)
            Dim lblTotalAmount As Label = e.Row.FindControl("lblTotalAmount")
            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Chris YIM]
            ' ----------------------------------------------------------------------------------------
            'lblTotalAmount.Text = CDbl(dr.Item("Total_Amount")).ToString("#,##0")
            lblTotalAmount.Text = udtFormatter.formatMoney(dr.Item("Total_Amount").ToString, False)
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Chris YIM]

        End If

    End Sub

    Protected Sub gvTransaction_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTransaction.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then
            Dim strTransactionNo As String = e.CommandArgument.ToString.Trim
            Session(SESS_TransactionNo) = strTransactionNo
            BuildDetail()

            ' CRE11-004
            ' Build Transaction Model For Audit Log
            Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = Nothing
            udtEHSTransaction = (New EHSTransaction.EHSTransactionBLL).LoadClaimTran(strTransactionNo)
            If Not IsNothing(strTransactionNo) Then Session(SESS_EHSTransaction) = udtEHSTransaction

            MultiViewMonthlyStatement.ActiveViewIndex = ViewIndex.Detail

            Dim udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.AddDescripton("Transaction No", strTransactionNo)
            udtAuditLogEntry.WriteLog(LogID.LOG00008, AuditLogDescription.ViewTransactionDetail)

        End If
    End Sub

    Protected Sub gvTransaction_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTransaction.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(4, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(5, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(6, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
        ' -- --------------------------------------------- -- 
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(1, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

        GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Protected Sub gvTransaction_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTransaction.PageIndexChanging
        GridViewPageIndexChangingHandler(sender, e, SESS_TransactionDataTable)
    End Sub

    Protected Sub gvTransaction_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTransaction.PreRender
        GridViewPreRenderHandler(sender, e, SESS_TransactionDataTable)
    End Sub

    Protected Sub gvTransaction_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvTransaction.Sorting
        GridViewSortingHandler(sender, e, SESS_TransactionDataTable)
    End Sub

    '

    Public Overrides Sub GridViewHeaderImage_Click(ByVal sender As Object, ByVal e As SortedGridviewHeaderModel.GridViewHeaderImageEventArgs)
        Select Case e.intColumn
            Case 1, 4
                popupSchemeNameHelp.Show()
                udcSchemeLegend.ShowSubsidy = False
                udcSchemeLegend.BindSchemeClaim(Session("language"), Me.SubPlatform)

            Case 5, 6
                popupDocTypeHelp.Show()
                udcDocTypeLegend.DocTypeLegendSubPlatform = Me.SubPlatform
                udcDocTypeLegend.BindDocType(Session("language"))

        End Select

    End Sub

    '

    Protected Sub ibtnSchemeBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00009, AuditLogDescription.SchemeBackClick)

        MultiViewMonthlyStatement.ActiveViewIndex = ViewIndex.Summary

        SetDropDownListSelectedIndex(ddlBankAccount, CStr(CInt(ViewState("PracticeDisplaySeq"))))
        SetDropDownListSelectedIndex(ddlStatement, CStr(ViewState("ReimburseID")))

        If Not IsNothing(Session(SESS_StatementDataSet)) Then BuildDynamicStatement(Session(SESS_StatementDataSet))

    End Sub

    Protected Sub ibtnTransactionBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00010, AuditLogDescription.TransactionBackClick)

        Dim dtScheme As DataTable = Session(SESS_SchemeDataTable)

        If dtScheme.Rows.Count = 1 Then
            MultiViewMonthlyStatement.ActiveViewIndex = ViewIndex.Summary

            If Not IsNothing(Session(SESS_StatementDataSet)) Then BuildDynamicStatement(Session(SESS_StatementDataSet))

        Else
            MultiViewMonthlyStatement.ActiveViewIndex = ViewIndex.Scheme
        End If

    End Sub

    Protected Sub ibtnDetailBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00011, AuditLogDescription.TransactionDetailBackClick)

        udcClaimTranEnquiry.Clear()

        MultiViewMonthlyStatement.ActiveViewIndex = ViewIndex.Transaction
    End Sub

    Private Sub SetDropDownListSelectedIndex(ByRef ddlMonthlyStatement As DropDownList, ByVal strValue As String)
        Dim i As Integer
        For i = 0 To ddlMonthlyStatement.Items.Count - 1
            If ddlMonthlyStatement.Items(i).Value = strValue Then
                ddlMonthlyStatement.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub

    '

    Protected Sub ibtnCloseDocTypeHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    End Sub

    Protected Sub ibtnCloseSchemeNameHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    End Sub

    '

    Public Sub udcClaimTranEnquiry_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcClaimTranEnquiry.VaccineLegendClicked1
        udcSchemeLegend.ShowScheme = False
        udcSchemeLegend.BindSchemeClaim(Session("language"), Me.SubPlatform)

        popupSchemeNameHelp.Show()
    End Sub

    Public Sub BuildDynamicStatement(ByVal ds As DataSet)
        panDynamicStatement.Controls.Clear()

        Dim table As New Table()

        table.Width = 590
        table.CellPadding = 2
        table.CellSpacing = 0

        Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithSubsidizeGroup()

        Dim dtStatementHeader As DataTable = ds.Tables(0)
        Dim dtScheme As DataTable = ds.Tables(1)
        Dim dtCategory As DataTable = ds.Tables(2)
        Dim dtSubsidy As DataTable = ds.Tables(3)

        For Each drScheme As DataRow In dtScheme.Select(String.Empty, "Scheme_Display_Seq")
            ' Scheme Header [e.g. Health Care Voucher Scheme (HCVS)]
            Dim strSchemeCode As String = drScheme("Scheme_Code").ToString.Trim
            Dim udtSchemeC As SchemeClaimModel = udtSchemeCList.Filter(strSchemeCode)
            Dim strSchemeName As String = String.Empty
            Dim strSchemeShortForm As String = String.Empty

            Select Case Session("language")
                Case TradChinese
                    strSchemeName = udtSchemeC.SchemeDescChi
                    strSchemeShortForm = udtSchemeC.SchemeDescChi
                Case SimpChinese
                    strSchemeName = udtSchemeC.SchemeDescCN
                    strSchemeShortForm = udtSchemeC.SchemeDescCN
                Case Else
                    strSchemeName = String.Format("{0} ({1})", udtSchemeC.SchemeDesc, udtSchemeC.DisplayCode)
                    strSchemeShortForm = udtSchemeC.DisplayCode
            End Select

            table.Rows.Add(GenerateStatementRow(strText:=strSchemeName, _
                                                blnUseWholeRow:=True, _
                                                blnTextBold:=True, _
                                                blnTextUnderline:=True))

            ' Non-clinic
            If drScheme("Clinic_Type") = "N" Then
                table.Rows.Add(GenerateStatementRow(strText:=String.Format("({0})", Me.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting")), _
                                                    blnUseWholeRow:=True))
            End If

            ' ------ Subsidize Section ------
            If drScheme("Mth_Statement_Unit_Visible").ToString.Trim = "Y" Then
                If dtCategory.Select(String.Format("Scheme_Code = '{0}'", strSchemeCode)).Length > 0 Then
                    ' Contains category
                    For Each drCategory As DataRow In dtCategory.Select(String.Format("Scheme_Code = '{0}'", strSchemeCode), "Category_Display_Seq")
                        Dim strCategoryCode As String = drCategory("Category_Code").ToString.Trim

                        table.Rows.Add(GenerateStatementRow(strText:=String.Format("{0}:", (New ClaimCategoryBLL).GetClaimCategoryCache.Filter(strCategoryCode).GetCategoryName(Session("language"))), _
                                                            blnTextBold:=True, _
                                                            intTextPaddingLevel:=1))

                        For Each drSubsidy As DataRow In dtSubsidy.Select(String.Format("Scheme_Code = '{0}' AND Category_Code = '{1}'", strSchemeCode, strCategoryCode), "Subsidize_Display_Seq")
                            Dim strSubsidyName As String = String.Empty

                            Select Case Session("language")
                                Case CultureLanguage.TradChinese
                                    strSubsidyName = String.Format("{0} (${1}):", drSubsidy("Mth_Statement_Desc_Chi").ToString.Trim, CDbl(drSubsidy("Per_Unit_Value")).ToString("0").Trim)
                                Case CultureLanguage.SimpChinese
                                    strSubsidyName = String.Format("{0} (${1}):", drSubsidy("Mth_Statement_Desc_CN").ToString.Trim, CDbl(drSubsidy("Per_Unit_Value")).ToString("0").Trim)
                                Case Else
                                    strSubsidyName = String.Format("{0} (${1}):", drSubsidy("Mth_Statement_Desc").ToString.Trim, CDbl(drSubsidy("Per_Unit_Value")).ToString("0").Trim)
                            End Select

                            table.Rows.Add(GenerateStatementRow(strText:=strSubsidyName, _
                                                                strTranCount:=drSubsidy("Transaction_Count").ToString.Trim, _
                                                                intTextPaddingLevel:=1))

                        Next

                        table.Rows.Add(GenerateStatementRow(strText:=String.Format("{0}:", Me.GetGlobalResourceObject("Text", "SubTotalSign")), _
                                                            dblCatTotal:=CDbl(drCategory("Transaction_Amount")), _
                                                            intTextPaddingLevel:=2))

                        table.Rows.Add(GenerateEmptyRow)

                    Next

                Else
                    ' No category
                    For Each drSubsidy As DataRow In dtSubsidy.Select(String.Format("Scheme_Code = '{0}'", strSchemeCode), "Subsidize_Display_Seq")
                        Dim strSubsidyName As String = String.Empty

                        Select Case Session("language")
                            Case CultureLanguage.TradChinese
                                strSubsidyName = String.Format("{0} (${1}):", drSubsidy("Mth_Statement_Desc_Chi").ToString.Trim, CDbl(drSubsidy("Per_Unit_Value")).ToString("0").Trim)
                            Case CultureLanguage.SimpChinese
                                strSubsidyName = String.Format("{0} (${1}):", drSubsidy("Mth_Statement_Desc_CN").ToString.Trim, CDbl(drSubsidy("Per_Unit_Value")).ToString("0").Trim)
                            Case Else
                                strSubsidyName = String.Format("{0} (${1}):", drSubsidy("Mth_Statement_Desc").ToString.Trim, CDbl(drSubsidy("Per_Unit_Value")).ToString("0").Trim)
                        End Select

                        table.Rows.Add(GenerateStatementRow(strText:=strSubsidyName, _
                                                            strTranCount:=drSubsidy("Transaction_Count").ToString.Trim))

                    Next

                    table.Rows.Add(GenerateEmptyRow)

                End If

            End If

            ' -- No. of transaction(s), XXXX: --
            table.Rows.Add(GenerateStatementRow(strText:=String.Format("{0}, {1}:", HttpContext.GetGlobalResourceObject("Text", "NoOfTran"), strSchemeShortForm), _
                                                strTranCount:=drScheme("Transaction_Count").ToString.Trim))

            ' -- Sub-total ($), XXXX --
            table.Rows.Add(GenerateStatementRow(strText:=String.Format("{0}, {1}:", Me.GetGlobalResourceObject("Text", "SubTotalSign"), strSchemeShortForm), _
                                                dblSchemeTotal:=CDbl(drScheme("Transaction_Amount").ToString.Trim)))

            table.Rows.Add(GenerateEmptyRow)

        Next

        ' Horizontal Line <hr>
        table.Rows.Add(GenerateHrRow)

        ' Total Amount ($):
        table.Rows.Add(GenerateStatementRow(strText:=String.Format("{0}:", HttpContext.GetGlobalResourceObject("Text", "TotalAmountSign")), _
                                            dblSchemeTotal:=CDbl(dtStatementHeader.Rows(0)("Transaction_Amount").ToString.Trim), _
                                            blnIsGrandTotal:=True))

        panDynamicStatement.Controls.Add(table)

    End Sub

    Private Function GenerateStatementRow(strText As String, _
                                          Optional strTranCount As String = "", _
                                          Optional dblCatTotal As Nullable(Of Double) = Nothing, _
                                          Optional dblSchemeTotal As Nullable(Of Double) = Nothing, _
                                          Optional blnUseWholeRow As Boolean = False, _
                                          Optional intTextPaddingLevel As Integer = 0, _
                                          Optional blnTextBold As Boolean = False, _
                                          Optional blnTextUnderline As Boolean = False, _
                                          Optional blnIsGrandTotal As Boolean = False) As TableRow
        ' Structure

        ' +-----------------------------------------------------------------------+
        ' | [Text]                       [TranCount]   [CatTotal]   [SchemeTotal] |
        ' +-----------------------------------------------------------------------+

        Dim tr As New TableRow()
        tr.Height = 16
        tr.VerticalAlign = VerticalAlign.Top

        ' Text
        Dim td1 As New TableCell()
        td1.Text = strText
        td1.HorizontalAlign = HorizontalAlign.Left
        td1.BorderStyle = BorderStyle.None
        If intTextPaddingLevel <> 0 Then td1.Style.Add("padding-left", String.Format("{0}px", 25 * intTextPaddingLevel))
        td1.Font.Bold = blnTextBold
        td1.Font.Underline = blnTextUnderline
        tr.Cells.Add(td1)

        ' TranCount
        Dim td2 As New TableCell()
        td2.Text = strTranCount
        td2.HorizontalAlign = HorizontalAlign.Right
        td2.BorderStyle = BorderStyle.None
        td2.Font.Bold = True
        td2.Width = 50
        tr.Cells.Add(td2)

        ' CatTotal
        Dim td3 As New TableCell()
        If dblCatTotal.HasValue Then
            td3.Text = dblCatTotal.Value.ToString("#,##0")
        Else
            td3.Text = String.Empty
        End If
        td3.HorizontalAlign = HorizontalAlign.Right
        td3.BorderStyle = BorderStyle.None
        td3.Font.Bold = True
        td3.Width = 70
        tr.Cells.Add(td3)

        ' SchemeTotal
        Dim td4 As New TableCell()
        If dblSchemeTotal.HasValue Then
            td4.Text = dblSchemeTotal.Value.ToString("#,##0")
        Else
            td4.Text = String.Empty
        End If
        td4.HorizontalAlign = HorizontalAlign.Right
        td4.BorderStyle = BorderStyle.None
        td4.Font.Bold = True
        td4.Font.Underline = blnIsGrandTotal
        td4.Width = 70
        tr.Cells.Add(td4)

        ' Use whole row
        If blnUseWholeRow Then
            tr.Cells(0).ColumnSpan = 4
            tr.Cells(1).Visible = False
            tr.Cells(2).Visible = False
            tr.Cells(3).Visible = False
        End If

        Return tr

    End Function

    Private Function GenerateHrRow(Optional intTrHeight As Integer = 16)
        Dim tr As New TableRow()
        tr.Height = intTrHeight

        Dim td As New TableCell()
        td.ColumnSpan = 4
        td.Text = "<hr style='color:#888888'>"
        td.BorderStyle = BorderStyle.None
        tr.Cells.Add(td)

        Return tr

    End Function

    Private Function GenerateEmptyRow(Optional intTrHeight As Integer = 16)
        Dim tr As New TableRow()
        tr.Height = intTrHeight

        Dim td As New TableCell()
        td.ColumnSpan = 4
        td.Text = String.Empty
        td.BorderStyle = BorderStyle.None
        tr.Cells.Add(td)

        Return tr

    End Function

#Region "Implement Working Data"

    ''' <summary>
    ''' CRE11-004
    ''' Handle working data on view change, clear working data if no use
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub MultiViewMonthlyStatement_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MultiViewMonthlyStatement.ActiveViewChanged
        Select Case MultiViewMonthlyStatement.ActiveViewIndex

            Case ViewIndex.Transaction
                Me.ClearWorkingData()
                'Do Nothing (Keep working data)
            Case ViewIndex.Detail
                'Do Nothing (Keep working data)
            Case ViewIndex.Scheme
                Me.ClearWorkingData()
            Case ViewIndex.Summary
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
        Dim udtEHSTransaction As EHSTransaction.EHSTransactionModel = Session(SESS_EHSTransaction)
        If Not IsNothing(udtEHSTransaction) Then
            Return udtEHSTransaction.EHSAcct
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Session(SESS_EHSTransaction)
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

        If GetEHSTransaction() Is Nothing Then Return Nothing
        If GetEHSTransaction.DocCode = String.Empty Then Return Nothing
        Return GetEHSTransaction.DocCode
    End Function
    ''' <summary>
    ''' CRE11-004
    '''  Clear all working data
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub ClearWorkingData()
        MyBase.ClearWorkingData()

        Session(SESS_EHSTransaction) = Nothing
    End Sub

#End Region

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Popup legend again if legend data changed
    Private Sub udcSchemeLegend_DataChanged() Handles udcSchemeLegend.DataChanged
        popupSchemeNameHelp.Show()
    End Sub

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub FilterPracticeList(ByRef dtPracticeList As DataTable, ByVal PracticeList As PracticeModelCollection)
        Dim IsPracticeSchemeCoexist As Boolean
        Dim IsMatchPractice As Boolean
        Dim udtPracticeModel As PracticeModel

        Dim drPracticeList As DataRow
        Dim drRemoveRow As DataRow = dtPracticeList.NewRow()

        For intPracticeList As Integer = 1 To PracticeList.Count
            IsPracticeSchemeCoexist = False
            If intPracticeList <= PracticeList.Count Then
                udtPracticeModel = PracticeList.GetValueList(intPracticeList - 1)
                If udtPracticeModel.PracticeSchemeInfoList.Count > 0 Then
                    IsPracticeSchemeCoexist = True
                End If

                If Not IsPracticeSchemeCoexist Then
                    PracticeList.Remove(udtPracticeModel)
                    intPracticeList -= 1
                End If
            End If
        Next

        For intDtPracticeList As Integer = 1 To dtPracticeList.Rows.Count
            IsMatchPractice = False
            If intDtPracticeList <= dtPracticeList.Rows.Count Then
                drPracticeList = dtPracticeList.Rows(intDtPracticeList - 1)
                For intPracticeList As Integer = 1 To PracticeList.Count
                    udtPracticeModel = PracticeList.GetValueList(intPracticeList - 1)
                    If drPracticeList.Item("PracticeID") = udtPracticeModel.DisplaySeq Then
                        IsMatchPractice = True
                    End If
                Next

                If Not IsMatchPractice Then
                    dtPracticeList.Rows.Remove(drPracticeList)
                    intDtPracticeList -= 1
                End If
            End If
        Next

    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
End Class
