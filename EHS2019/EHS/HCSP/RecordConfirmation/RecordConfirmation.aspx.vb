Imports System.Web.Security.AntiXss
Imports Common.ComObject
Imports Common.Validation
Imports Common.Format
Imports HCSP.BLL
Imports Common.ComFunction
Imports System.Threading
Imports Common.Component.UserAC
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component
Imports Common.Component.VoucherTransaction
'Imports Common.Component.VoucherRecipientAccount
Imports Common.Component.Scheme
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Practice
Imports Common.Component.SortedGridviewHeader
Imports Common.Component.EHSTransaction
Imports Common.Component.RedirectParameter                                      ' CRE11-024-02

Partial Public Class RecordConfirmation
    'Inherits BasePage
    Inherits BasePageWithGridView

#Region "Audit Log Description"
    Public Class AuditLogDescription
        Public Const LoadRecordConfirmation As String = "Record Confirmation Loaded"  '0

        Public Const SearchClaimTransaction As String = "Search Claim Transaction"  '1
        Public Const SearchSuccess As String = "Search Transaction Success"  '2
        Public Const SearchFail As String = "Search Transaction Fail" '3

        'transaction related
        Public Const ViewTransactionDetail As String = "View Transaction Detail" '4
        Public Const VoidTransaction As String = "Void Transaction"  '5
        Public Const VoidTransactionSuccess As String = "Void Transaction Success"  '6
        Public Const VoidTransactionFail As String = "Void Transaction Fail"  '7

        Public Const ConfirmTransaction As String = "Confirm Transaction"   '8
        Public Const ConfirmTransactionSuccess As String = "Confirm Transaction Success"  '9
        Public Const ConfirmTransactionfail As String = "Confirm Transaction Fail" '10

        'Selected record
        Public Const ConfirmSelectedRecord As String = "Confirm Selected Record"  '11
        Public Const ConfirmSelectedRecordComplete As String = "Complete Confirm Selected Record"  '12
        Public Const ConfirmSelectedRecordFail As String = "Confirm Selected Record Fail"   '13

        '-------------------------------------------------------------------------------------------------------------------
        Public Const SearchTempEHSAccount As String = "Search Temp EHS Account"  '14
        Public Const SearchTempAccountSuccess As String = "Search EHS Account Success" '15
        Public Const SearchTempAccountFail As String = "Search EHS Account Fail"  '16

        Public Const ConfirmEHSAccount As String = "Confirm Temp EHS Account"  '17
        Public Const ConfirmEHSAccountComplete As String = "Complete Confirm Temp EHS Account"  '18
        Public Const ConfirmEHSAccountFail As String = "Confirm Temp EHS Account Fail"  '19

        Public Const RejectEHSAccount As String = "Reject EHS Voucher Account"  '20
        Public Const RejectEHSAccountComplete As String = "Complete Reject EHS Voucher Account"  '21
        Public Const RejectEHSAccountFail As String = "Reject Temp EHS Account Fail"  '22

        'Selected record (Confirm)
        Public Const SelectConfirmationAcct As String = "Confirm Selected Confirmation Account"  '23
        Public Const SelectConfirmationAcctComplete As String = "Complete Confirm Selected Confirmation Account"  '24
        Public Const SelectConfirmationAcctFail As String = "Confirm Selected Confirmation Account Fail"  '25

        'Selected record (Reject)
        Public Const SelectRejectAcct As String = "Confirm Selected Reject Account"  '26
        Public Const SelectRejectAcctComplete As String = "Complete Confirm Selected Reject Account"  '27
        Public Const SelectRejectAcctFail As String = "Confirm Selected Reject Account Fail"  '28

        '--------------------------------------------------------------------------------------------------------------------
        Public Const ChkConfirmTransaction As String = "Check the confirmation statement" '29
        Public Const BackToSearchPageFromClaim As String = "Back To Search Page From Claim" '30
        Public Const BackToSearchPageFromTempAccount As String = "Back To Search Page From Temp Account" '31

        Public Const BackToClaimResultFromSelected As String = "Back To Claim Result From Selected" '32
        Public Const BackToClaimResultFromCompletion As String = "Back To Claim Result after transaction comfirmation" '33

        Public Const BackToAccResultFromSelectRejectConfirm As String = "Back To Account Result From Select Reject or Confirm" '34
        Public Const BackToAccResultFromCompletion As String = "Back To Account Result after confirmation or reject" '35
        Public Const BackToClaimResultFromTransDetail As String = "Back To Claim Serach Result From Transaction Detail" '36

        'Reject transaction   (Transaction Details)
        Public Const ClickToRejectTran As String = "Click To Reject Transaction (in transaction detail)"  '37
        Public Const ClickConfirmRejectTranViaTransDetail As String = "Click Confirm to Reject Transaction (in transaction detail)"  '38

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        Public Const ClickCustomTypeChange As String = "Custom Type Change"  '40
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        'Confirm All record
        Public Const ConfirmAllRecord As String = "Confirm All Record"  '41
        Public Const ConfirmAllRecordComplete As String = "Complete Confirm All Record"  '42
        Public Const ConfirmAllRecordFail As String = "Confirm All Record Fail"   '43


    End Class
#End Region

    Public Enum VoidStateType
        INFO = 0
        INPUT = 1
        CONFIRM = 2
    End Enum

    Dim udtAuditLogEntry As AuditLogEntry
    Private udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL
    Private udtTransactionMaintenanceBLL As New TransactionMaintenanceBLL
    Private udtEHSTransactionBLL As New EHSTransactionBLL
    Private udtClaimVoucherBLL As ClaimVoucherBLL = New ClaimVoucherBLL
    Private udtRecordConfirmationBLL As BLL.RecordConfirmationBLL = New RecordConfirmationBLL

    Private udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler
    Private udtSP As ServiceProviderModel
    Private udtDataEntry As DataEntryUserModel
    Private udtUserAC As UserACModel = New UserACModel
    Private udtFormatter As New Formatter
    Private udtSM As Common.ComObject.SystemMessage
    Private udtValidator As Validator = New Validator

    Private udtEHSTransactionModel As EHSTransactionModel


#Region "Constant values"
    Const SESS_Practice As String = "RecordConfirm_Practice"
    Const SESS_Scheme As String = "RecordConfirm_Scheme"
    Const SESS_ALLScheme As String = "RecordConfirm_ALLScheme"
    Const SESS_PracticeSchemeInfo As String = "RecordConfirm_PracticeSchemeInfo"
    Const SESS_DataEntry As String = "RecordConfirm_DataEntry"

    Dim strFuncCode As String = Common.Component.FunctCode.FUNT021001
    Private strValidationFail As String = "ValidationFail"
#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        If Not IsPostBack Then
            ' Init the Active View in Page Load only
            Me.mvRecordConfirmation.ActiveViewIndex = 0
        End If

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        'Get Current USer Account for check Session Expired
        Dim udtUserAC As UserACModel
        udtUserAC = UserACBLL.GetUserAC

        If Not IsPostBack Then
            'Dim udtUserAC As UserACModel
            'udtUserAC = UserACBLL.GetUserAC

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            '-- ---------------------------------------------- --
            FunctionCode = "021001"     ' Record Confirmation
            chkIncludeIncompleteClaim.Checked = True
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If Me.SubPlatform.Equals(EnumHCSPSubPlatform.CN) Then
                chkIncludeIncompleteClaim.Checked = False
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            If udtUserAC.UserType <> SPAcctType.ServiceProvider Then
                Throw New Exception("Access Denied")
            End If

            'Reset Controls
            ResetControls()

            'Log Function Log
            Me.udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, AuditLogDescription.LoadRecordConfirmation)
        Else
            ReRenderPage()

            If Me.mvClaimTrans.ActiveViewIndex = 3 And Me.mvRecordConfirmation.ActiveViewIndex = 1 Then
                'Dim strTransactionID As String = ""
                'strTransactionID = ViewState("TransactionID")

                udtEHSTransactionModel = udtSessionHandler.EHSTransactionGetFromSession(strFuncCode)

                ' go to vTransactionDetail
                udcClaimTranEnquiry.buildClaimObject(udtEHSTransactionModel.TransactionID, udtEHSTransactionModel, False)
                SetClaimInfo(CType(ViewState("VoidStateType"), VoidStateType))
                'SetClaimInfo(0)
            End If
        End If

        'ReRenderPage()



        If Not IsPostBack Then
            If Not Session("fromMain") Is Nothing Then
                If CStr(Session("fromMain")) = "Y" Then
                    If Not IsNothing(Session("ConfirmType")) AndAlso CStr(Session("ConfirmType")) = "C" Then
                        Session("fromMain") = Nothing
                        Session("ConfirmType") = Nothing
                        Me.mvRecordConfirmation.ActiveViewIndex = 1
                        Me.mvClaimTrans.ActiveViewIndex = 0
                        Me.rbConfirmTypeList.SelectedIndex = 0
                        Me.ddlPractice.SelectedIndex = 0
                        Me.ddlDataEntry.SelectedIndex = 0
                        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                        '-- ---------------------------------------------- --
                        Me.rbConfirmTypeList_SelectedIndexChanged(Me.rbConfirmTypeList, New EventArgs())
                        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
                        Me.SearchTranscationRecord(True)

                    ElseIf Not IsNothing(Session("ConfirmType")) AndAlso CStr(Session("ConfirmType")) = "V" Then
                        Session("fromMain") = Nothing
                        Session("ConfirmType") = Nothing
                        Me.mvRecordConfirmation.ActiveViewIndex = 2
                        Me.mvVRAcct.ActiveViewIndex = 0
                        Me.rbConfirmTypeList.SelectedIndex = 1
                        Me.ddlPractice.SelectedIndex = 0
                        Me.ddlDataEntry.SelectedIndex = 0
                        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                        '-- ---------------------------------------------- --
                        Me.rbConfirmTypeList_SelectedIndexChanged(Me.rbConfirmTypeList, New EventArgs())
                        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
                        Me.SearchTempVRAcctRecord(True)
                    End If

                End If
            End If
        End If

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        '-- ---------------------------------------------- --
        ' discard redirect parameter if found any that does not belong to this function
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetFromSession()

        If Not IsNothing(udtRedirectParameter) AndAlso Not IsNothing(udtRedirectParameter.ReturnParameter) AndAlso _
            Not IsNothing(udtRedirectParameter.ReturnParameter.SourceFunctionCode) Then
            If udtRedirectParameter.ReturnParameter.SourceFunctionCode <> CType(Me.Page, BasePage).FunctionCode Then
                udtRedirectParameterBLL.RemoveFromSession()
            End If
        End If

        HandleRedirectAction()

        ' Build Button for redirect to Claim Transaction Management
        If Me.mvRecordConfirmation.ActiveViewIndex = 1 And Me.mvClaimTrans.ActiveViewIndex = 3 Then
            udtEHSTransactionModel = udtSessionHandler.EHSTransactionGetFromSession(strFuncCode)
            If Not udtEHSTransactionModel Is Nothing Then
                Me.BuildRedirectButton(Me.ibtnManagement, ClaimTransactionMaintenance.BuildSearchCriteria(udtEHSTransactionModel.RecordStatus, _
                                                                                                                     udtEHSTransactionModel.TransactionDtm, _
                                                                                                                     udtEHSTransactionModel.TransactionDtm, _
                                                                                                                    udtEHSTransactionModel.TransactionID))
            End If
        End If



        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        AddHandler udcClaimTranEnquiry.VaccineLegendClicked1, AddressOf udcClaimTranEnquiry_VaccineLegendClicked

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        GetCurrentUser(udtSP, udtDataEntry)

        If udtSP.SchemeInfoList.Filter(SchemeClaimModel.COVID19CVC) IsNot Nothing OrElse _
            udtSP.SchemeInfoList.Filter(SchemeClaimModel.COVID19RVP) IsNot Nothing Then
            ibtnConfirmAll.Visible = True
        Else
            ibtnConfirmAll.Visible = False
        End If
        ' CRE20-0022 (Immu record) [End][Chris YIM]

    End Sub

#End Region

#Region "View 1 - Search"

    Private Sub ibtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearch.Click

        If rbConfirmTypeList.SelectedIndex = 0 Then

            'Claim is selected
            SearchTranscationRecord(True)
        Else
            'EHS Account is selected
            SearchTempVRAcctRecord(True)
        End If

    End Sub

    Private Sub rbConfirmTypeList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbConfirmTypeList.SelectedIndexChanged
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)

        If rbConfirmTypeList.SelectedIndex = 0 Then
            Me.udtAuditLogEntry.AddDescripton("Type", "Claim Transaction")
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If Me.SubPlatform <> EnumHCSPSubPlatform.CN Then
                chkIncludeIncompleteClaim.Visible = True
                trIncludeIncompleteClaim.Style("display") = String.Empty
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Else
            Me.udtAuditLogEntry.AddDescripton("Type", "eHealth Account")
            chkIncludeIncompleteClaim.Visible = False
            trIncludeIncompleteClaim.Style("display") = "none"
        End If

        Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00040, AuditLogDescription.ClickCustomTypeChange)
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Koala]
    End Sub

    Private Sub SearchTranscationRecord(ByVal blnDefaultSortOrder As Boolean)

        Me.udcInfoMessageBox.Visible = False
        Me.udcMessageBox.Visible = False
        Me.imgCutOffDateError.Visible = False

        Dim dtTransaction As DataTable
        Dim blnerr As Boolean = False

        Dim dtmCutOffDate As DateTime
        Dim messageCode As String

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'Dim strCutOffDate As String = Me.txtCutOffDate.Text.Trim
        Dim strCutOffDate As String = udtFormatter.formatInputDate(Me.txtCutOffDate.Text.Trim, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        GetCurrentUser(udtSP, udtDataEntry)

        Dim intPracticeDisplaySeq As Nullable(Of Integer)
        If ddlPractice.SelectedValue.Trim.Equals(String.Empty) Then
            intPracticeDisplaySeq = Nothing
        Else
            intPracticeDisplaySeq = CInt(Me.ddlPractice.SelectedValue)
        End If

        Dim strDataEntryBy As String
        strDataEntryBy = CStr(Me.ddlDataEntry.SelectedValue)

        Dim strSchemeCode As String
        strSchemeCode = CStr(Me.ddlScheme.SelectedValue)


        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
        '-- ---------------------------------------------- --
        Dim strIncludeIncompleteClaim As String
        strIncludeIncompleteClaim = IIf(Me.chkIncludeIncompleteClaim.Checked, YesNo.Yes, YesNo.No)
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
            strIncludeIncompleteClaim = YesNo.No
        End If
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'Log start search 
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("PracticeID", ddlPractice.SelectedValue.Trim)
        Me.udtAuditLogEntry.AddDescripton("Scheme", strSchemeCode)
        Me.udtAuditLogEntry.AddDescripton("DataEntry", strDataEntryBy)
        Me.udtAuditLogEntry.AddDescripton("CutOffDate", strCutOffDate)
        Me.udtAuditLogEntry.AddDescripton("IncludeIncompleteClaim", strIncludeIncompleteClaim)            ' CRE11-024-02
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00001, AuditLogDescription.SearchClaimTransaction)

        messageCode = udtValidator.chkValidSearchDate(strCutOffDate)
        If Not messageCode.Equals(String.Empty) Then
            blnerr = True
            Me.imgCutOffDateError.Visible = True
            Me.udcMessageBox.AddMessage(Common.Component.FunctCode.FUNT990000, "E", messageCode)
        Else
            strCutOffDate = udtFormatter.formatSearchDate(strCutOffDate)
            dtmCutOffDate = DateTime.ParseExact(strCutOffDate, udtFormatter.EnterDateFormat, Nothing)
            ViewState("CutOffDate") = dtmCutOffDate

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me.txtCutOffDate.Text = udtFormatter.formatInputTextDate(strCutOffDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        End If

        If Not blnerr Then
            'dtTransaction = udtVoucherTransactionBLL.GetClaimRecord(strSPID, intPracticeDisplaySeq, strDataEntryBy, dtmCutOffDate, strSchemeCode)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
            '-- ---------------------------------------------- --
            'dtTransaction = Me.udtRecordConfirmationBLL.GetTransactionConfirmation(udtSP.SPID, intPracticeDisplaySeq, strDataEntryBy, dtmCutOffDate, strSchemeCode)

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'dtTransaction = Me.udtRecordConfirmationBLL.GetTransactionConfirmation(udtSP.SPID, intPracticeDisplaySeq, strDataEntryBy, dtmCutOffDate, strSchemeCode, strIncludeIncompleteClaim)
            dtTransaction = Me.udtRecordConfirmationBLL.GetTransactionConfirmation(udtSP.SPID, intPracticeDisplaySeq, strDataEntryBy, dtmCutOffDate, strSchemeCode, Me.SubPlatform, strIncludeIncompleteClaim)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            '-- ---------------------------------------------- --
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

            ''CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            ''-----------------------------------------------------------------------------------------
            'Dim udtSchemeClaim As SchemeClaimModel
            'Dim udtSchemeClaimBLL As New SchemeClaimBLL
            'Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection
            'Dim drTransaction() As DataRow
            'Dim dtResTransaction As New DataTable
            'Dim enumSubPlatform As [Enum] = Me.SubPlatform()

            'Dim strFilterCriteria As String = String.Empty

            'udtSchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim.FilterByHCSPSubPlatform(enumSubPlatform)

            'Dim strArrSchemeCode(udtSchemeClaimModelCollection.Count - 1) As String
            'Dim intCnt As String = 0

            'strFilterCriteria = "Scheme in ('"
            'For Each udtSchemeClaim In udtSchemeClaimModelCollection
            '    strArrSchemeCode(intCnt) = udtSchemeClaim.SchemeCode.Trim
            '    intCnt += 1
            'Next
            'strFilterCriteria = strFilterCriteria & String.Join("','", strArrSchemeCode)
            'strFilterCriteria = strFilterCriteria & "')"

            'drTransaction = dtTransaction.Select(strFilterCriteria)

            'dtResTransaction = dtTransaction.Clone

            'For Each dr As DataRow In drTransaction
            '    dtResTransaction.ImportRow(dr)
            'Next
            ''CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If dtTransaction.Rows.Count > 0 Then
                Me.mvRecordConfirmation.ActiveViewIndex = 1
                Me.mvClaimTrans.ActiveViewIndex = 0

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me.lblCutoffDateResult.Text = udtFormatter.formatDate(dtmCutOffDate)
                Me.lblCutoffDateResult.Text = udtFormatter.formatDisplayDate(dtmCutOffDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                Me.lblPracticeResult.Text = Me.ddlPractice.SelectedItem.Text.Trim
                Me.lblDataEntryResult.Text = Me.ddlDataEntry.SelectedItem.Text.Trim

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
                '-- ---------------------------------------------- --
                Me.lblSchemeResult.Text = Me.ddlScheme.SelectedItem.Text.Trim
                '-- ---------------------------------------------- --
                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]

                If Me.SubPlatform.Equals(EnumHCSPSubPlatform.CN) Then
                    trIncludeIncompleteClaim_vr.Style.Add("display", "none")
                End If
                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start][Tony]
                chkIncludeIncompleteClaim_vr.Checked = chkIncludeIncompleteClaim.Checked
                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End][Tony]


                Session("ClaimRecord") = dtTransaction

                If blnDefaultSortOrder = True Then
                    Me.GridViewDataBind(Me.gvClaimRecord, dtTransaction, "Transaction_Dtm", "ASC", False)
                Else
                    Me.GridViewDataBind(Me.gvClaimRecord, dtTransaction)
                End If
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                ShowClaimRecordGridView(True)

                'Log Success search 
                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtTransaction.Rows.Count)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AuditLogDescription.SearchSuccess)


            Else
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Me.mvRecordConfirmation.ActiveViewIndex = 0

                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, LogID.LOG00001)
                ShowClaimRecordGridView(False)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                'Log No Reocrd Found
                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", 0)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AuditLogDescription.SearchSuccess)
            End If
            udcInfoMessageBox.BuildMessageBox()
        Else
            Me.udcMessageBox.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, Common.Component.LogID.LOG00003, AuditLogDescription.SearchFail)
        End If

    End Sub

    Private Sub SearchTempVRAcctRecord(ByVal blnDefaultSortOrder As Boolean)
        Me.udcInfoMessageBox.Visible = False
        Me.udcMessageBox.Visible = False
        Me.imgCutOffDateError.Visible = False

        Dim dtTempEHSAccount As DataTable
        Dim blnECfound As Boolean = False
        Dim blnVISAfound As Boolean = False
        Dim blnPermitRemainfound As Boolean = False
        Dim blnerr As Boolean = False
        Dim dtmCutOffDate As DateTime
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'Dim strCutOffDate As String = Me.txtCutOffDate.Text.Trim
        Dim strCutOffDate As String = udtFormatter.formatInputDate(Me.txtCutOffDate.Text.Trim, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Dim messageCode As String

        GetCurrentUser(udtSP, udtDataEntry)

        ShowTempVRAcctGridView(False)

        Dim intPracticeDisplaySeq As Nullable(Of Integer)
        If ddlPractice.SelectedValue.Trim.Equals(String.Empty) Then
            intPracticeDisplaySeq = Nothing
        Else
            intPracticeDisplaySeq = CInt(Me.ddlPractice.SelectedValue)
        End If


        Dim strDataEntryBy As String
        strDataEntryBy = CStr(Me.ddlDataEntry.SelectedValue)

        Dim strSchemeCode As String
        strSchemeCode = CStr(Me.ddlScheme.SelectedValue)


        'Log start search 
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("PracticeID", udtSP.SPID)
        Me.udtAuditLogEntry.AddDescripton("Scheme", strSchemeCode)
        Me.udtAuditLogEntry.AddDescripton("DataEntry", strDataEntryBy)
        Me.udtAuditLogEntry.AddDescripton("CutOffDate", dtmCutOffDate)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00014, AuditLogDescription.SearchTempEHSAccount)


        messageCode = udtValidator.chkValidSearchDate(strCutOffDate)
        If Not messageCode.Equals(String.Empty) Then
            blnerr = True
            Me.imgCutOffDateError.Visible = True
            Me.udcMessageBox.AddMessage(Common.Component.FunctCode.FUNT990000, "E", messageCode)
        Else
            strCutOffDate = udtFormatter.formatSearchDate(strCutOffDate)
            dtmCutOffDate = DateTime.ParseExact(strCutOffDate, udtFormatter.EnterDateFormat, Nothing)
            ViewState("CutOffDate") = dtmCutOffDate

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me.txtCutOffDate.Text = udtFormatter.formatInputTextDate(strCutOffDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        End If

        If Not blnerr Then
            'dtTempVRAcctRecord = udtVRAcctBLL.GetTempVRAcctWithoutTrans(strSPID, intPracticeDisplaySeq, strDataEntryBy, dtmCutOffDate, strSchemeCode)
            dtTempEHSAccount = Me.udtRecordConfirmationBLL.GetEHSAccountConfirmation(udtSP.SPID, intPracticeDisplaySeq, strDataEntryBy, dtmCutOffDate, strSchemeCode, Me.SubPlatform)

            ' INT12-0012 HCSP Record Confirmation, CTM error on grid and RCH code input problem [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            dtTempEHSAccount.Columns.Add("doc_for_sort", GetType(String))
            ' INT12-0012 HCSP Record Confirmation, CTM error on grid and RCH code input problem [End][Koala]

            For Each dr As DataRow In dtTempEHSAccount.Rows
                'EC checking
                If dr.Item("doc_display_code").ToString.Trim = Common.Component.DocType.DocTypeModel.DocTypeCode.EC Then
                    blnECfound = True
                    Exit For
                End If
                'Visa Checking
                If dr.Item("doc_display_code").ToString.Trim = Common.Component.DocType.DocTypeModel.DocTypeCode.VISA Then
                    blnVISAfound = True
                    Exit For
                End If
                'Permit To Remain checking
                If dr.Item("doc_display_code").ToString.Trim = Common.Component.DocType.DocTypeModel.DocTypeCode.ID235B Then
                    blnPermitRemainfound = True
                    Exit For
                End If


                ' INT12-0012 HCSP Record Confirmation, CTM error on grid and RCH code input problem [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                dr.Item("doc_for_sort") = dr.Item("doc_display_code") + "|" + dr.Item("IdentityNum")
                ' INT12-0012 HCSP Record Confirmation, CTM error on grid and RCH code input problem [End][Koala]
            Next

            Session("TempVRAcctRecord") = dtTempEHSAccount

            If dtTempEHSAccount.Rows.Count > 0 Then
                Me.mvRecordConfirmation.ActiveViewIndex = 2
                Me.mvVRAcct.ActiveViewIndex = 0

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me.lblCutoffDateResult_vr.Text = udtFormatter.formatDate(dtmCutOffDate)
                Me.lblCutoffDateResult_vr.Text = udtFormatter.formatDisplayDate(dtmCutOffDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                Me.lblPracticeResult_vr.Text = Me.ddlPractice.SelectedItem.Text.Trim
                Me.lblDataEntryResult_vr.Text = Me.ddlDataEntry.SelectedItem.Text.Trim
                Me.lblSchemeResult_vr.Text = Me.ddlScheme.SelectedItem.Text.Trim            ' CRE11-024-02

                If blnDefaultSortOrder = True Then
                    GridViewDataBind(gvTempVRAcctRecord, dtTempEHSAccount, "Transaction_Dtm", "ASC", False)
                Else
                    GridViewDataBind(gvTempVRAcctRecord, dtTempEHSAccount)
                End If

                ShowTempVRAcctGridView(True)

                'Control Grid View Width due to existence of certain document types
                If blnVISAfound Or blnPermitRemainfound Or blnECfound Then
                    Me.gvTempVRAcctRecord.Columns.Item(8).Visible = True
                    Me.gvTempVRAcctRecord.Width = 1325
                Else
                    Me.gvTempVRAcctRecord.Columns.Item(8).Visible = False
                    Me.gvTempVRAcctRecord.Width = 1125
                End If
                '-----------------------------------------------------------------



                'Log Success search 
                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtTempEHSAccount.Rows.Count)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00015, AuditLogDescription.SearchTempAccountSuccess)
            Else

                Me.mvRecordConfirmation.ActiveViewIndex = 0

                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMessageBox.AddMessage("990000", "I", "00001")

                ShowTempVRAcctGridView(False)

                'Log  search record = 0
                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", 0)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00015, AuditLogDescription.SearchTempAccountSuccess)
            End If

            udcInfoMessageBox.BuildMessageBox()
        Else
            Me.udcMessageBox.BuildMessageBox(strValidationFail, Me.udtAuditLogEntry, Common.Component.LogID.LOG00016, AuditLogDescription.SearchTempAccountFail)
        End If
    End Sub

    Private Sub ddlPractice_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPractice.SelectedIndexChanged

        GetCurrentUser(udtSP, udtDataEntry)

        Dim udtDataEntryAccBLL As DataEntryAcctBLL = New DataEntryAcctBLL

        Dim dtDataEntryList As New DataTable()
        If ddlPractice.SelectedValue.Trim.Equals(String.Empty) Then
            dtDataEntryList = udtDataEntryAccBLL.getDataEntryAcctBySPPracticeID(udtSP.SPID, Nothing)
        Else
            dtDataEntryList = udtDataEntryAccBLL.getDataEntryAcctBySPPracticeID(udtSP.SPID, CInt(ddlPractice.SelectedValue.Trim))
        End If

        Me.ddlDataEntry.Items.Clear()
        Me.ddlDataEntry.DataSource = dtDataEntryList
        Session(SESS_DataEntry) = dtDataEntryList
        Me.ddlDataEntry.DataTextField = "Data_Entry_Account"
        Me.ddlDataEntry.DataValueField = "Data_Entry_Account"
        Me.ddlDataEntry.SelectedValue = Nothing
        Me.ddlDataEntry.DataBind()
        Me.ddlDataEntry.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        Me.AddExternalAccountToDataEntryList()

        'Update Scheme Drop Down list
        Dim udtPracticeSchemeInfoCollection As PracticeSchemeInfoModelCollection = CType(Session(SESS_PracticeSchemeInfo), PracticeSchemeInfoModelCollection)
        If Not Me.ddlPractice.SelectedValue.Trim.Equals(String.Empty) Then
            Dim udtSelectedPracticeSchemeInfoCollection As PracticeSchemeInfoModelCollection = New PracticeSchemeInfoModelCollection
            For Each udtPracticeSchemeInfoModel As PracticeSchemeInfoModel In udtPracticeSchemeInfoCollection.Values
                If udtPracticeSchemeInfoModel.PracticeDisplaySeq = Me.ddlPractice.SelectedValue Then
                    udtSelectedPracticeSchemeInfoCollection.Add(udtPracticeSchemeInfoModel)
                End If
            Next
            Session(SESS_Scheme) = udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtSelectedPracticeSchemeInfoCollection)
        Else
            Session(SESS_Scheme) = Session(SESS_ALLScheme)
        End If

        Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = CType(Session(SESS_Scheme), SchemeClaimModelCollection)
        Me.ddlScheme.Items.Clear()
        Me.ddlScheme.DataSource = udtSchemeClaimModelCollection

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'If LCase(Session("language")) = TradChinese Then
        '    Me.ddlScheme.DataTextField = "SchemeDescChi"
        'Else
        '    Me.ddlScheme.DataTextField = "SchemeDesc"
        'End If

        Select Case LCase(Session("language"))
            Case English
                Me.ddlScheme.DataTextField = "SchemeDesc"
            Case TradChinese
                Me.ddlScheme.DataTextField = "SchemeDescChi"
            Case SimpChinese
                Me.ddlScheme.DataTextField = "SchemeDescCN"
            Case Else
                Me.ddlScheme.DataTextField = "SchemeDesc"
        End Select
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Me.ddlScheme.DataBind()
        Me.ddlScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Winnie]
        Me.lblSchemeResult.Text = AntiXssEncoder.HtmlEncode(Me.ddlScheme.SelectedItem.Text.Trim(), True)
        Me.lblSchemeResult_vr.Text = AntiXssEncoder.HtmlEncode(Me.ddlScheme.SelectedItem.Text.Trim(), True)
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Winnie]

        'Dim udtDataEntryUserACBankAccBLL As New DataEntryUserACBankAcctBLL
        'Dim udtServiceProvider As ServiceProviderModel
        'udtServiceProvider = CType(UserACBLL.GetUserAC, ServiceProviderModel)
        'Me.ddlDataEntry.DataSource = udtDataEntryUserACBankAccBLL.GetAllDataEntryUserACByPractice(udtServiceProvider.SPID, Me.ddlPractice.SelectedValue)
        'Me.ddlDataEntry.DataBind()



    End Sub

#End Region

#Region "View 2 - Search Result"

#Region "Search Transaction Record"

#Region "Selection"

    Private Sub ibtnSearchedResultBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearchedResultBack.Click
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00030, AuditLogDescription.BackToSearchPageFromClaim)

        ' go back to search record
        Me.mvRecordConfirmation.ActiveViewIndex = 0
    End Sub

    Private Sub ibtnConfirmAll_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirmAll.Click
        Dim udtSystemMessage As SystemMessage
        Dim udtValidator As New Validator
        Dim dtClaimRecord As DataTable
        Dim dtRecordSelected As New DataTable
        Dim noPendingRecord As Boolean = True
        dtClaimRecord = CType(Session("ClaimRecord"), DataTable)
        dtRecordSelected = dtClaimRecord.Clone
        dtRecordSelected.Clear()

        'Log start search 
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00041, AuditLogDescription.ConfirmAllRecord)

        For Each row As DataRow In dtClaimRecord.Rows
            If row.Item("Record_Status") = ClaimTransStatus.Pending Then
                dtRecordSelected.Rows.Add(row.ItemArray)
                noPendingRecord = False
            End If
        Next row

        If noPendingRecord Then
            udtSystemMessage = New Common.ComObject.SystemMessage("990000", "E", "00465")
            Me.udcMessageBox.AddMessage(udtSystemMessage)
        End If



        Dim i As Integer
        If Me.udcMessageBox.GetCodeTable.Rows.Count = 0 Then

            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                gvSelectedRecord.Columns(5).Visible = True    'lblTotalAmountRMB
            Else
                gvSelectedRecord.Columns(5).Visible = False    'lblTotalAmountRMB
            End If

            ' go to vSelectedRecord
            Me.mvRecordConfirmation.ActiveViewIndex = 1
            Me.mvClaimTrans.ActiveViewIndex = 1

            If dtRecordSelected.Rows.Count > 0 Then

                Dim strSortExpression As String
                Dim strSortDirection As String

                strSortExpression = Me.GetGridViewSortExpression(Me.gvClaimRecord)
                strSortDirection = Me.GetGridViewSortDirection(Me.gvClaimRecord)

                Session("RecordSelected") = dtRecordSelected
                Me.GridViewDataBind(Me.gvSelectedRecord, Session("RecordSelected"), strSortExpression, strSortDirection, False)

                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
                'log each transaction ID
                Dim strTransactionIDList As String = String.Empty
                Dim k As Integer
                If dtRecordSelected.Rows.Count > 0 Then
                    For i = 0 To dtRecordSelected.Rows.Count - 1
                        If strTransactionIDList.Trim.Equals(String.Empty) Then
                            strTransactionIDList = CStr(dtRecordSelected.Rows(i).Item("Transaction_ID")).Trim
                        Else
                            strTransactionIDList += " ," + CStr(dtRecordSelected.Rows(i).Item("Transaction_ID")).Trim
                        End If
                    Next
                End If
                Me.udtAuditLogEntry.AddDescripton("TransactionIDList", strTransactionIDList)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00042, AuditLogDescription.ConfirmAllRecordComplete)

            End If

            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.UpdateImageURL(Me.ibtnConfirmTransaction, False)
            Me.chkConfirmTransaction.Checked = False

        End If
        Me.udcMessageBox.BuildMessageBox("ValidationFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00043, AuditLogDescription.ConfirmAllRecordFail)
    End Sub

    Private Sub ibtnConfirmSelection_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirmSelection.Click
        Dim udtSystemMessage As SystemMessage
        Dim udtValidator As New Validator

        'Log start search 
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00011, AuditLogDescription.ConfirmSelectedRecord)


        udtSystemMessage = udtValidator.chkGridSelectedNothing(Me.gvClaimRecord, "chkSelect", 1)
        If Not udtSystemMessage Is Nothing Then
            Me.udcMessageBox.AddMessage(udtSystemMessage)
        End If
        If Me.udcMessageBox.GetCodeTable.Rows.Count = 0 Then

            ' go to vSelectedRecord
            Me.mvRecordConfirmation.ActiveViewIndex = 1
            Me.mvClaimTrans.ActiveViewIndex = 1

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                gvSelectedRecord.Columns(5).Visible = True    'lblTotalAmountRMB
            Else
                gvSelectedRecord.Columns(5).Visible = False    'lblTotalAmountRMB
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            Dim dtRecordSelected As New DataTable
            Dim drRecordSelected As DataRow
            Dim dtClaimRecord As DataTable
            Dim drClaimRecord() As DataRow

            dtClaimRecord = CType(Session("ClaimRecord"), DataTable)
            dtRecordSelected = dtClaimRecord.Clone
            Dim gvr As GridViewRow
            Dim i, j As Integer
            Dim strTransactionID As String

            For Each gvr In Me.gvClaimRecord.Rows
                Dim chkSelect As CheckBox
                Dim lbtnTransactionID As LinkButton
                chkSelect = CType(gvr.FindControl("chkSelect"), CheckBox)
                If chkSelect.Checked Then
                    lbtnTransactionID = CType(gvr.FindControl("lbtnTransactionID"), LinkButton)
                    strTransactionID = lbtnTransactionID.CommandArgument.Trim
                    drClaimRecord = dtClaimRecord.Select("Transaction_ID = '" & strTransactionID & "'")
                    For i = 0 To drClaimRecord.Length - 1
                        drRecordSelected = dtRecordSelected.NewRow
                        For j = 0 To dtRecordSelected.Columns.Count - 1
                            drRecordSelected.Item(j) = drClaimRecord(i).Item(j)
                        Next
                        dtRecordSelected.Rows.Add(drRecordSelected)
                    Next
                End If
            Next

            If dtRecordSelected.Rows.Count > 0 Then

                Dim strSortExpression As String
                Dim strSortDirection As String

                strSortExpression = Me.GetGridViewSortExpression(Me.gvClaimRecord)
                strSortDirection = Me.GetGridViewSortDirection(Me.gvClaimRecord)

                Session("RecordSelected") = dtRecordSelected
                Me.GridViewDataBind(Me.gvSelectedRecord, Session("RecordSelected"), strSortExpression, strSortDirection, False)

                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
                'log each transaction ID
                Dim strTransactionIDList As String = String.Empty
                Dim k As Integer
                If dtRecordSelected.Rows.Count > 0 Then
                    For i = 0 To dtRecordSelected.Rows.Count - 1
                        If strTransactionIDList.Trim.Equals(String.Empty) Then
                            strTransactionIDList = CStr(dtRecordSelected.Rows(i).Item("Transaction_ID")).Trim
                        Else
                            strTransactionIDList += " ," + CStr(dtRecordSelected.Rows(i).Item("Transaction_ID")).Trim
                        End If
                    Next
                End If
                Me.udtAuditLogEntry.AddDescripton("TransactionIDList", strTransactionIDList)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00012, AuditLogDescription.ConfirmSelectedRecordComplete)

            End If

            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.UpdateImageURL(Me.ibtnConfirmTransaction, False)
            Me.chkConfirmTransaction.Checked = False

        End If
        Me.udcMessageBox.BuildMessageBox("ValidationFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00013, AuditLogDescription.ConfirmSelectedRecordFail)
    End Sub

#End Region

#Region "Confirm Selection"

    Private Sub ibtnSelectedRecordBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSelectedRecordBack.Click
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00032, AuditLogDescription.BackToClaimResultFromSelected)

        Me.mvRecordConfirmation.ActiveViewIndex = 1
        Me.mvClaimTrans.ActiveViewIndex = 0
        ResetGridViewSelection()
    End Sub

    Private Sub ibtnConfirmTransaction_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirmTransaction.Click
        Dim blnIsValid As Boolean = True
        Dim udtSystemMessage As SystemMessage
        ' go to vConfirmatedResult
        Me.mvRecordConfirmation.ActiveViewIndex = 1
        Me.mvClaimTrans.ActiveViewIndex = 2

        Dim dtRecordSelected As DataTable
        dtRecordSelected = CType(Session("RecordSelected"), DataTable)

        'Create Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
        'log each transaction ID
        Dim strTransactionIDList As String = String.Empty
        Dim i As Integer
        If dtRecordSelected.Rows.Count > 0 Then
            For i = 0 To dtRecordSelected.Rows.Count - 1
                If strTransactionIDList.Trim.Equals(String.Empty) Then
                    strTransactionIDList = CStr(dtRecordSelected.Rows(i).Item("Transaction_ID")).Trim
                Else
                    strTransactionIDList += " ," + CStr(dtRecordSelected.Rows(i).Item("Transaction_ID")).Trim
                End If
            Next
        End If
        Me.udtAuditLogEntry.AddDescripton("TransactionIDList", strTransactionIDList)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00008, AuditLogDescription.ConfirmTransaction)

        Dim dtmClaimConfirmationDate As DateTime

        Me.GetCurrentUser(udtSP, udtDataEntry)

        'Log Confirm Claim Transaction----------------------------------
        Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
        'log each transaction ID
        strTransactionIDList = String.Empty
        If dtRecordSelected.Rows.Count > 0 Then
            For i = 0 To dtRecordSelected.Rows.Count - 1
                If strTransactionIDList.Trim.Equals(String.Empty) Then
                    strTransactionIDList = CStr(dtRecordSelected.Rows(i).Item("Transaction_ID")).Trim
                Else
                    strTransactionIDList += " ," + CStr(dtRecordSelected.Rows(i).Item("Transaction_ID")).Trim
                End If
            Next
        End If

        Me.udtAuditLogEntry.AddDescripton("TransactionIDList", strTransactionIDList)
        '--------------------------------------------------------------
        Try
            dtmClaimConfirmationDate = Me.udtRecordConfirmationBLL.ConfirmTransaction(dtRecordSelected, udtSP.SPID)

            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            Me.udcInfoMessageBox.AddMessage(Me.strFuncCode, "I", "00001")
            Me.udcInfoMessageBox.BuildMessageBox()

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                'Log Fail and Build Message Box
                udtSystemMessage = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                Me.udcMessageBox.AddMessage(udtSystemMessage)
                Me.udcMessageBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00010, AuditLogDescription.ConfirmTransactionfail)
                blnIsValid = False
            Else
                'log Fail
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00010, AuditLogDescription.ConfirmTransactionfail)
                Throw eSQL
            End If
        Catch ex As Exception
            Throw ex
        End Try


        If blnIsValid Then
            Me.lblClaimConfirmationDateText.Visible = True
            Me.lblClaimConfirmationDate.Visible = True

            Me.lblNoOfTransactionConfirmedText.Visible = True
            Me.lblNoOfTransactionConfirmed.Visible = True

            Dim udtFormatter As New Formatter

            ViewState("ClaimConfirmationDate") = dtmClaimConfirmationDate
            Me.lblClaimConfirmationDate.Text = udtFormatter.formatDateTime(dtmClaimConfirmationDate)
            Me.lblNoOfTransactionConfirmed.Text = dtRecordSelected.Rows.Count

            'log success
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00009, AuditLogDescription.ConfirmTransactionSuccess)
        Else
            Me.lblClaimConfirmationDateText.Visible = False
            Me.lblClaimConfirmationDate.Visible = False

            Me.lblNoOfTransactionConfirmedText.Visible = False
            Me.lblNoOfTransactionConfirmed.Visible = False
        End If
    End Sub

    Private Sub chkConfirmTransaction_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkConfirmTransaction.CheckedChanged
        'Create Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Dim dtRecordSelected As DataTable
        dtRecordSelected = CType(Session("RecordSelected"), DataTable)
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
        'log each transaction ID
        Dim strTransactionIDList As String = String.Empty
        Dim i As Integer
        If dtRecordSelected.Rows.Count > 0 Then
            For i = 0 To dtRecordSelected.Rows.Count - 1
                If strTransactionIDList.Trim.Equals(String.Empty) Then
                    strTransactionIDList = CStr(dtRecordSelected.Rows(i).Item("Transaction_ID")).Trim
                Else
                    strTransactionIDList += " ," + CStr(dtRecordSelected.Rows(i).Item("Transaction_ID")).Trim
                End If
            Next
        End If
        Me.udtAuditLogEntry.AddDescripton("TransactionIDList", strTransactionIDList)
        Me.udtAuditLogEntry.AddDescripton("Checked", chkConfirmTransaction.Checked)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00029, AuditLogDescription.ChkConfirmTransaction)

        Dim udtGeneralFunction As New GeneralFunction
        If chkConfirmTransaction.Checked Then
            udtGeneralFunction.UpdateImageURL(Me.ibtnConfirmTransaction, True)
        Else
            udtGeneralFunction.UpdateImageURL(Me.ibtnConfirmTransaction, False)
        End If
    End Sub

#End Region

#Region "Confirmed"

    Private Sub ibtnConfirmedResultNextRetrieve_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirmedResultNextRetrieve.Click
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00033, AuditLogDescription.BackToClaimResultFromCompletion)

        ' search transaction record again and go to vSearchedResult
        SearchTranscationRecord(False)
    End Sub

#End Region

#Region "Rejected"

    Private Sub ibtnRejectResultNextRetrieve_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRejectResultNextRetrieve.Click
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00033, AuditLogDescription.BackToClaimResultFromCompletion)

        ' search transaction record again and go to vSearchedResult
        SearchTranscationRecord(False)
    End Sub

#End Region

#Region "Transaction Details"

    Private Sub ibtnTransactionDetailBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnTransactionDetailBack.Click, ibtnCancelVoidTransaction.Click
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00036, AuditLogDescription.BackToClaimResultFromTransDetail)

        Me.udcMessageBox.Clear()

        Dim state As VoidStateType
        state = ViewState("VoidStateType")
        Me.txtRejectReason.Text = ""
        Me.imgRejectReasonAlert.Visible = False
        If state = VoidStateType.INFO Then
            SetClaimInfo(0)
            Me.mvRecordConfirmation.ActiveViewIndex = 1
            Me.mvClaimTrans.ActiveViewIndex = 0
            udcClaimTranEnquiry.Clear()
            ResetGridViewSelection()
        ElseIf state = VoidStateType.INPUT Then
            SetClaimInfo(0)
        ElseIf state = VoidStateType.CONFIRM Then
            SetClaimInfo(1)
        End If

        'udcClaimTranEnquiry.Clear()
    End Sub

    Private Sub ibtnRejectTransaction_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRejectTransaction.Click
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)

        Me.udtEHSTransactionModel = Me.udtSessionHandler.EHSTransactionGetFromSession(Me.strFuncCode)
        Dim strTransactionID As String = udtEHSTransactionModel.TransactionID.Trim

        Me.udtAuditLogEntry.AddDescripton("TransactionID", strTransactionID)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00037, AuditLogDescription.ClickToRejectTran)

        SetClaimInfo(1)
        Me.txtRejectReason.Text = ""
    End Sub

    Private Sub ibtnSaveRejectTransaction_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSaveRejectTransaction.Click
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)

        Me.udtEHSTransactionModel = Me.udtSessionHandler.EHSTransactionGetFromSession(Me.strFuncCode)
        Dim strTransactionID As String = udtEHSTransactionModel.TransactionID.Trim

        Me.udtAuditLogEntry.AddDescripton("TransactionID", strTransactionID)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00038, AuditLogDescription.ClickConfirmRejectTranViaTransDetail)

        Dim udtValidator As New Validator
        Me.imgRejectReasonAlert.Visible = False

        If udtValidator.IsEmpty(Me.txtRejectReason.Text.Trim) Then
            Me.udcMessageBox.AddMessage(Me.strFuncCode, "E", "00001")
            Me.imgRejectReasonAlert.Visible = True
        End If

        If udcMessageBox.GetCodeTable.Rows.Count = 0 Then
            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            Me.lblRejectReason.Text = AntiXssEncoder.HtmlEncode(txtRejectReason.Text, True)
            ' I-CRE16-003 Fix XSS [End][Lawrence]
            SetClaimInfo(2)
            ibtnConfirmRejectTransaction_Click(Nothing, Nothing)
        Else
            SetClaimInfo(1)
            Me.udcMessageBox.BuildMessageBox("ValidationFail")
        End If

    End Sub

    Private Sub ibtnConfirmRejectTransaction_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirmRejectTransaction.Click
        Me.mvRecordConfirmation.ActiveViewIndex = 1
        Me.mvClaimTrans.ActiveViewIndex = 4

        Dim blnIsValid As Boolean = True
        Dim udtSystemMessage As SystemMessage
        Dim udtGeneralFunction As New GeneralFunction

        Me.udtEHSTransactionModel = Me.udtSessionHandler.EHSTransactionGetFromSession(Me.strFuncCode)
        Dim strTransactionID As String = udtEHSTransactionModel.TransactionID.Trim

        'Dim dtClaimRecord As DataTable
        'dtClaimRecord = CType(Session("ClaimRecord"), DataTable)

        'Dim drClaimRecordList() As DataRow
        'drClaimRecordList = dtClaimRecord.Select("Transaction_ID = '" & strTransactionID & "'")

        'Dim drClaimRecord As DataRow
        'drClaimRecord = drClaimRecordList(0)

        'Dim tsmp As Byte()
        'tsmp = drClaimRecord.Item("tsmp")

        Dim udtServiceProvider As ServiceProviderModel
        udtServiceProvider = CType(UserACBLL.GetUserAC, ServiceProviderModel)

        Me.GetCurrentUser(udtSP, udtDataEntry)

        Dim strRejectReason As String
        strRejectReason = Me.txtRejectReason.Text

        'Create Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("TransactionNo", strTransactionID)
        Me.udtAuditLogEntry.AddDescripton("Void Reason", strRejectReason)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00005, AuditLogDescription.VoidTransaction)

        Try
            'Dim udtEHSTransaction As EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(strTransactionID)

            udtEHSTransactionModel.VoidReason = strRejectReason
            udtEHSTransactionModel.VoidUser = udtSP.SPID
            udtEHSTransactionModel.VoidByDataEntry = String.Empty

            If udtTransactionMaintenanceBLL.OnVoid(udtEHSTransactionModel) Then
                Me.udtSessionHandler.EHSTransactionRemoveFromSession(strFuncCode)
                blnIsValid = True
            Else
                udtSystemMessage = New Common.ComObject.SystemMessage("990000", "E", "00184")
                Me.udcMessageBox.AddMessage(udtSystemMessage)
                Me.udcMessageBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00007, AuditLogDescription.VoidTransactionFail)
                blnIsValid = False
            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                'Log Void Transaction Fail and Build Message Box
                udtSystemMessage = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                Me.udcMessageBox.AddMessage(udtSystemMessage)
                Me.udcMessageBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00007, AuditLogDescription.VoidTransactionFail)
                blnIsValid = False
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            Throw ex
        End Try


        If blnIsValid Then
            Me.lblRejectDateText.Visible = True
            Me.lblRejectDate.Visible = True

            Me.lblRejectReferenceNoText.Visible = True
            Me.lblRejectReferenceNo.Visible = True

            Dim udtFormatter As New Formatter

            Dim udtEHSUpdatedTransaction As EHSTransactionModel
            udtEHSUpdatedTransaction = udtEHSTransactionBLL.LoadClaimTran(strTransactionID)

            ' CRE11-004
            Me.udtSessionHandler.EHSTransactionSaveToSession(udtEHSUpdatedTransaction, strFuncCode)

            'Show Complete Message
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            If udtEHSUpdatedTransaction.EHSAcct.RecordStatus = "D" Then
                If Not udtEHSUpdatedTransaction.EHSAcct.OriginalAccID Is Nothing AndAlso Not udtEHSUpdatedTransaction.EHSAcct.OriginalAccID.Equals(String.Empty) Then
                    Me.udcInfoMessageBox.AddMessage(Me.strFuncCode, "I", "00002", New String() {"%s"}, New Object() {udtFormatter.formatSystemNumber(udtEHSUpdatedTransaction.VoidTranNo)})
                Else
                    Me.udcInfoMessageBox.AddMessage(Me.strFuncCode, "I", "00003", New String() {"%r", "%s"}, New Object() {udtFormatter.formatSystemNumber(udtEHSUpdatedTransaction.EHSAcct.VoucherAccID), udtFormatter.formatSystemNumber(udtEHSUpdatedTransaction.VoidTranNo)})
                End If
            Else
                Me.udcInfoMessageBox.AddMessage(Me.strFuncCode, "I", "00002", New String() {"%s"}, New Object() {udtFormatter.formatSystemNumber(udtEHSUpdatedTransaction.VoidTranNo)})
            End If
            Me.udcInfoMessageBox.BuildMessageBox()


            ViewState("RejectDatetime") = udtEHSUpdatedTransaction.VoidDate 'dtmReject
            Me.lblRejectDate.Text = udtFormatter.formatDateTime(udtEHSUpdatedTransaction.VoidDate)
            Me.lblRejectReferenceNo.Text = udtFormatter.formatSystemNumber(udtEHSUpdatedTransaction.VoidTranNo)

            'Log Void Transaction Success
            Me.udtAuditLogEntry.AddDescripton("TransactionNo", strTransactionID)
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00006, AuditLogDescription.VoidTransactionSuccess)

            ' CRE11-004
            Me.udtSessionHandler.EHSTransactionRemoveFromSession(strFuncCode)
        Else
            Me.lblRejectDateText.Visible = False
            Me.lblRejectDate.Visible = False

            Me.lblRejectReferenceNoText.Visible = False
            Me.lblRejectReferenceNo.Visible = False

        End If
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    '-- ---------------------------------------------- --
    Private Sub BuildRedirectButton(ByVal btn As CustomControls.CustomImageButton, ByVal objSearchCriteria As RedirectParameter.SearchCriteriaCollection)
        btn.SourceFunctionCode = CType(Me.Page, BasePage).FunctionCode
        btn.TargetFunctionCode = FunctCode.FUNT020301
        'btn.TargetUrl = GetURLByFunctionCode(FunctCode.FUNT020301)
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL((New BLL.MenuBLL).GetURL(FunctCode.FUNT020301))

        btn.Build()

        btn.ConstructNewRedirectParameter()

        btn.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
        btn.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)
        btn.RedirectParameter.SearchCriteria = objSearchCriteria
        btn.RedirectParameter.ReturnParameter = New RedirectParameterModel
        btn.RedirectParameter.ReturnParameter.SourceFunctionCode = btn.TargetFunctionCode
        btn.RedirectParameter.ReturnParameter.TargetFunctionCode = btn.SourceFunctionCode
        btn.RedirectParameter.ReturnParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)

        btn.RedirectParameter.ReturnParameter.SearchCriteria = RecordConfirmation.BuildSearchCriteria(Me.chkIncludeIncompleteClaim.Checked, _
                                Me.txtCutOffDate.Text.Trim(), _
                                Me.ddlPractice.SelectedValue, _
                                Me.ddlDataEntry.SelectedValue, _
                                Me.ddlScheme.SelectedValue)



    End Sub

    Private Function GetURLByFunctionCode(ByVal strFunctionCode As String) As String
        Dim dr() As DataRow = (New MenuBLL).GetMenuItem.Select(String.Format("Function_Code='{0}'", strFunctionCode))
        If dr.Length <> 1 Then Throw New Exception("RecordConfirmation.GetURLByFunctionCode: Unexpected no. of rows")
        Return dr(0)("URL")
    End Function

    Private Sub ibtnManagement_Click(ByVal sender As System.Web.UI.WebControls.ImageButton, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnManagement.Click
        '_udtAuditLogEntry.WriteLog(AuditLogDesc.ManagementClick_ID, AuditLogDesc.ManagementClick)

        Dim btn As CustomControls.CustomImageButton = sender.Parent
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL((New BLL.MenuBLL).GetURL(FunctCode.FUNT020301))
        btn.Redirect()
    End Sub

    Private Sub HandleRedirectAction()
        ' remove the session variables
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetFromSession()
        If IsNothing(udtRedirectParameter) Then Return

        udtRedirectParameterBLL.RemoveFromSession()
        udtRedirectParameterBLL.WriteAuditLog(FunctionCode, Me.Page, udtRedirectParameter)

        If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.Search) Then
            udtRedirectParameter.SearchCriteria.TryGetValue(RecordConfirmation.SEARCH_PARAM_INCLUDE_INCOMPLETE_CLAIMS, Me.chkIncludeIncompleteClaim.Checked)
            udtRedirectParameter.SearchCriteria.TryGetValue(RecordConfirmation.SEARCH_PARAM_INCLUDE_INCOMPLETE_CLAIMS, Me.chkIncludeIncompleteClaim_vr.Checked)
            udtRedirectParameter.SearchCriteria.TryGetValue(RecordConfirmation.SEARCH_PARAM_CUT_OFF_DATE, Me.txtCutOffDate.Text)
            udtRedirectParameter.SearchCriteria.TryGetValue(RecordConfirmation.SEARCH_PARAM_PRACTICE, Me.ddlPractice.SelectedValue)
            udtRedirectParameter.SearchCriteria.TryGetValue(RecordConfirmation.SEARCH_PARAM_DATA_ENTRY_ACCOUNT, Me.ddlDataEntry.SelectedValue)
            udtRedirectParameter.SearchCriteria.TryGetValue(RecordConfirmation.SEARCH_PARAM_SCHEME, Me.ddlScheme.SelectedValue)
            SearchTranscationRecord(True)
        End If
        ' if has view detail, then display view detail.
        If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.ViewDetail) Then
            Me.mvClaimTrans.ActiveViewIndex = 3
            Me.mvRecordConfirmation.ActiveViewIndex = 1

            If Not Session("ReturnTransactionNo") Is Nothing Then
                udtEHSTransactionModel = Me.udtEHSTransactionBLL.LoadClaimTran(Session("ReturnTransactionNo"))
            End If

            ' go to vTransactionDetail
            udcClaimTranEnquiry.buildClaimObject(udtEHSTransactionModel.TransactionID, udtEHSTransactionModel, False)
            If udtSessionHandler.EHSTransactionGetFromSession(strFuncCode) Is Nothing Then
                udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransactionModel, strFuncCode)
            End If

            ' display the appropriate languages
            Me.udcClaimTranEnquiry.chgLanguage()
            Me.ibtnRejectTransaction.Visible = False
            Me.ibtnSaveRejectTransaction.Visible = False
            Me.ibtnCancelVoidTransaction.Visible = False
        End If
    End Sub




    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

#End Region


#End Region

#Region "Search EHS Account Record"

#Region "Selection"

    Private Sub ibtnSearchedResultBack_vr_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSearchedResultBack_vr.Click
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00031, AuditLogDescription.BackToSearchPageFromTempAccount)

        ' go back to search record
        Me.mvRecordConfirmation.ActiveViewIndex = 0
    End Sub

    Private Sub ibtnConfirmSelected_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirmSelected.Click
        Dim udtSystemMessage As SystemMessage
        Dim udtValidator As New Validator

        'Create Audit Log and Log Selected Account for confirmation 
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00023, AuditLogDescription.SelectConfirmationAcct)

        udtSystemMessage = udtValidator.chkGridSelectedNothing(Me.gvTempVRAcctRecord, "chkSelect", 1)
        If Not udtSystemMessage Is Nothing Then
            Me.udcMessageBox.AddMessage(udtSystemMessage)
        End If
        If Me.udcMessageBox.GetCodeTable.Rows.Count = 0 Then
            Me.mvRecordConfirmation.ActiveViewIndex = 2
            Me.mvVRAcct.ActiveViewIndex = 1

            Me.lblConfirmRecord_vr.Text = Me.GetGlobalResourceObject("Text", "ConfirmRecord")
            ViewState("lblConfirmRecord") = "ConfirmRecord"

            Me.ibtnConfirmTempVRAcct.Visible = True
            Me.ibtnRejectTempVRAcct.Visible = False

            Dim dtRecordSelected As New DataTable
            Dim drRecordSelected As DataRow
            Dim dtTempVRAcctRecord As DataTable
            Dim drTempVRAcctRecord() As DataRow

            dtTempVRAcctRecord = CType(Session("TempVRAcctRecord"), DataTable)
            dtRecordSelected = dtTempVRAcctRecord.Clone
            Dim gvr As GridViewRow
            Dim i, j As Integer
            Dim strVRAcctID As String

            For Each gvr In Me.gvTempVRAcctRecord.Rows
                Dim chkSelect As CheckBox
                Dim lbtnHKID As LinkButton
                chkSelect = CType(gvr.FindControl("chkSelect"), CheckBox)
                If chkSelect.Checked Then
                    lbtnHKID = CType(gvr.FindControl("lbtnHKID"), LinkButton)
                    strVRAcctID = lbtnHKID.CommandArgument.Split("|")(0)
                    drTempVRAcctRecord = dtTempVRAcctRecord.Select("Voucher_Acc_ID = '" & strVRAcctID & "'")
                    For i = 0 To drTempVRAcctRecord.Length - 1
                        drRecordSelected = dtRecordSelected.NewRow
                        For j = 0 To dtRecordSelected.Columns.Count - 1
                            drRecordSelected.Item(j) = drTempVRAcctRecord(i).Item(j)
                        Next
                        dtRecordSelected.Rows.Add(drRecordSelected)
                    Next
                End If
            Next

            If dtRecordSelected.Rows.Count > 0 Then
                Dim strSortExpression As String
                Dim strSortDirection As String

                strSortExpression = Me.GetGridViewSortExpression(Me.gvTempVRAcctRecord)
                strSortDirection = Me.GetGridViewSortDirection(Me.gvTempVRAcctRecord)

                Session("RecordSelected") = dtRecordSelected
                Me.GridViewDataBind(Me.gvSelectedRecord_vr, Session("RecordSelected"), strSortExpression, strSortDirection, False)

                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
                'log each transaction ID
                Dim strAccountIDList As String = String.Empty
                Dim k As Integer
                If dtRecordSelected.Rows.Count > 0 Then
                    For i = 0 To dtRecordSelected.Rows.Count - 1
                        If strAccountIDList.Trim.Equals(String.Empty) Then
                            strAccountIDList = CStr(dtRecordSelected.Rows(k).Item("Voucher_Acc_ID")).Trim
                        Else
                            strAccountIDList += " ," + CStr(dtRecordSelected.Rows(k).Item("Voucher_Acc_ID")).Trim
                        End If
                    Next
                End If
                Me.udtAuditLogEntry.AddDescripton("AccountIDList", strAccountIDList)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00024, AuditLogDescription.SelectConfirmationAcctComplete)
            End If
        End If
        Me.udcMessageBox.BuildMessageBox("ValidationFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00025, AuditLogDescription.SelectConfirmationAcctFail)

    End Sub

    Private Sub ibtnRejectSelected_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRejectSelected.Click
        Dim udtSystemMessage As SystemMessage
        Dim udtValidator As New Validator

        'Create Audit Log and Log Selected Account for confirmation 
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00026, AuditLogDescription.SelectRejectAcct)

        udtSystemMessage = udtValidator.chkGridSelectedNothing(Me.gvTempVRAcctRecord, "chkSelect", 1)
        If Not udtSystemMessage Is Nothing Then
            Me.udcMessageBox.AddMessage(udtSystemMessage)
        End If

        If Me.udcMessageBox.GetCodeTable.Rows.Count = 0 Then
            Me.mvRecordConfirmation.ActiveViewIndex = 2
            Me.mvVRAcct.ActiveViewIndex = 1

            Me.lblConfirmRecord_vr.Text = Me.GetGlobalResourceObject("Text", "RejectRecord")
            ViewState("lblConfirmRecord") = "RejectRecord"

            Me.ibtnConfirmTempVRAcct.Visible = False
            Me.ibtnRejectTempVRAcct.Visible = True


            Dim dtRecordSelected As New DataTable
            Dim drRecordSelected As DataRow
            Dim dtTempVRAcctRecord As DataTable
            Dim drTempVRAcctRecord() As DataRow

            dtTempVRAcctRecord = CType(Session("TempVRAcctRecord"), DataTable)
            dtRecordSelected = dtTempVRAcctRecord.Clone
            Dim gvr As GridViewRow
            Dim i, j As Integer
            Dim strVRAcctID As String

            For Each gvr In Me.gvTempVRAcctRecord.Rows
                Dim chkSelect As CheckBox
                Dim lbtnHKID As LinkButton
                chkSelect = CType(gvr.FindControl("chkSelect"), CheckBox)
                If chkSelect.Checked Then
                    lbtnHKID = CType(gvr.FindControl("lbtnHKID"), LinkButton)
                    strVRAcctID = lbtnHKID.CommandArgument.Split("|")(0)
                    drTempVRAcctRecord = dtTempVRAcctRecord.Select("Voucher_Acc_ID = '" & strVRAcctID & "'")
                    For i = 0 To drTempVRAcctRecord.Length - 1
                        drRecordSelected = dtRecordSelected.NewRow
                        For j = 0 To dtRecordSelected.Columns.Count - 1
                            drRecordSelected.Item(j) = drTempVRAcctRecord(i).Item(j)
                        Next
                        dtRecordSelected.Rows.Add(drRecordSelected)
                    Next
                End If
            Next

            If dtRecordSelected.Rows.Count > 0 Then

                Dim strSortExpression As String
                Dim strSortDirection As String

                strSortExpression = Me.GetGridViewSortExpression(Me.gvTempVRAcctRecord)
                strSortDirection = Me.GetGridViewSortDirection(Me.gvTempVRAcctRecord)

                Session("RecordSelected") = dtRecordSelected
                Me.GridViewDataBind(Me.gvSelectedRecord_vr, Session("RecordSelected"), strSortExpression, strSortDirection, False)

                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
                'log each transaction ID
                Dim strAccountIDList As String = String.Empty
                Dim k As Integer
                If dtRecordSelected.Rows.Count > 0 Then
                    For i = 0 To dtRecordSelected.Rows.Count - 1
                        If strAccountIDList.Trim.Equals(String.Empty) Then
                            strAccountIDList = CStr(dtRecordSelected.Rows(k).Item("Voucher_Acc_ID")).Trim
                        Else
                            strAccountIDList += " ," + CStr(dtRecordSelected.Rows(k).Item("Voucher_Acc_ID")).Trim
                        End If
                    Next
                End If
                Me.udtAuditLogEntry.AddDescripton("AccountIDList", strAccountIDList)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00027, AuditLogDescription.SelectRejectAcctComplete)
            End If

        End If

        Me.udcMessageBox.BuildMessageBox("ValidationFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00028, AuditLogDescription.SelectRejectAcctFail)

    End Sub

#End Region

#Region "Confirm Selection"

    Private Sub ibtnSelectedRecordBack_vr_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnSelectedRecordBack_vr.Click
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00034, AuditLogDescription.BackToAccResultFromSelectRejectConfirm)

        Me.mvRecordConfirmation.ActiveViewIndex = 2
        Me.mvVRAcct.ActiveViewIndex = 0
        ResetGridViewSelection()
    End Sub

    Private Sub ibtnConfirmTempVRAcct_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirmTempVRAcct.Click
        Dim blnIsValid As Boolean = True
        Dim udtSystemMessage As SystemMessage

        Me.mvRecordConfirmation.ActiveViewIndex = 2
        Me.mvVRAcct.ActiveViewIndex = 2

        Dim dtRecordSelected As DataTable
        dtRecordSelected = CType(Session("RecordSelected"), DataTable)

        'Create Audit Log and Log Confirm
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
        'log each transaction ID
        Dim strAccountIDList As String = String.Empty
        Dim k As Integer
        If dtRecordSelected.Rows.Count > 0 Then
            For k = 0 To dtRecordSelected.Rows.Count - 1
                If strAccountIDList.Trim.Equals(String.Empty) Then
                    strAccountIDList = CStr(dtRecordSelected.Rows(k).Item("Voucher_Acc_ID")).Trim
                Else
                    strAccountIDList += " ," + CStr(dtRecordSelected.Rows(k).Item("Voucher_Acc_ID")).Trim
                End If
            Next
        End If
        Me.udtAuditLogEntry.AddDescripton("AccountIDList", strAccountIDList)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00017, AuditLogDescription.ConfirmEHSAccount)


        Dim dtmConfirm As DateTime

        Me.GetCurrentUser(udtSP, udtDataEntry)

        Try
            dtmConfirm = Me.udtRecordConfirmationBLL.ConfirmTempEHSAccount(dtRecordSelected, udtSP.SPID)

            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            Me.udcInfoMessageBox.AddMessage(Me.strFuncCode, "I", "00004")
            Me.udcInfoMessageBox.BuildMessageBox()

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                'Build Message Box and Log Fail 
                udtSystemMessage = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                Me.udcMessageBox.AddMessage(udtSystemMessage)
                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
                Me.udtAuditLogEntry.AddDescripton("AccountIDList", strAccountIDList)
                Me.udcMessageBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00019, AuditLogDescription.ConfirmEHSAccountFail)
                blnIsValid = False
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            Throw ex
        End Try

        If blnIsValid Then
            lblVRAcctConfirmationDateText.Visible = True
            lblVRAcctConfirmationDate.Visible = True

            lblNoOfTempAcctConfirmedText.Visible = True
            lblNoOfVRAcctConfirmed.Visible = True

            Dim udtFormatter As New Formatter

            ViewState("ConfirmDatetime") = dtmConfirm
            Me.lblVRAcctConfirmationDate.Text = udtFormatter.formatDateTime(dtmConfirm)
            Me.lblNoOfVRAcctConfirmed.Text = CStr(dtRecordSelected.Rows.Count)

            'Log confirm Account Success
            Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
            'log each transaction ID
            strAccountIDList = String.Empty
            If dtRecordSelected.Rows.Count > 0 Then
                For k = 0 To dtRecordSelected.Rows.Count - 1
                    If strAccountIDList.Trim.Equals(String.Empty) Then
                        strAccountIDList = CStr(dtRecordSelected.Rows(k).Item("Voucher_Acc_ID")).Trim
                    Else
                        strAccountIDList += " ," + CStr(dtRecordSelected.Rows(k).Item("Voucher_Acc_ID")).Trim
                    End If
                Next
            End If
            Me.udtAuditLogEntry.AddDescripton("AccountIDList", strAccountIDList)
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00018, AuditLogDescription.ConfirmEHSAccountComplete)
        Else
            lblVRAcctConfirmationDateText.Visible = False
            lblVRAcctConfirmationDate.Visible = False

            Me.lblNoOfTempAcctConfirmedText.Visible = False
            lblNoOfVRAcctConfirmed.Visible = False

        End If
    End Sub

    Private Sub ibtnRejectTempVRAcct_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRejectTempVRAcct.Click
        Dim blnIsValid As Boolean = True
        Dim udtSystemMessage As SystemMessage

        Me.mvRecordConfirmation.ActiveViewIndex = 2
        Me.mvVRAcct.ActiveViewIndex = 4

        Dim dtRecordSelected As DataTable
        dtRecordSelected = CType(Session("RecordSelected"), DataTable)

        'Create Audit Log and Log Reject 
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
        'log each transaction ID
        Dim strAccountIDList As String = String.Empty
        Dim k As Integer
        If dtRecordSelected.Rows.Count > 0 Then
            For k = 0 To dtRecordSelected.Rows.Count - 1
                If strAccountIDList.Trim.Equals(String.Empty) Then
                    strAccountIDList = CStr(dtRecordSelected.Rows(k).Item("Voucher_Acc_ID")).Trim
                Else
                    strAccountIDList += " ," + CStr(dtRecordSelected.Rows(k).Item("Voucher_Acc_ID")).Trim
                End If
            Next
        End If
        Me.udtAuditLogEntry.AddDescripton("AccountIDList", strAccountIDList)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00020, AuditLogDescription.RejectEHSAccount)


        Dim dtmReject As DateTime

        Me.GetCurrentUser(udtSP, udtDataEntry)

        Try
            dtmReject = Me.udtRecordConfirmationBLL.RejectTempEHSAccount(dtRecordSelected, udtSP.SPID)

            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            Me.udcInfoMessageBox.AddMessage(Me.strFuncCode, "I", "00005")
            Me.udcInfoMessageBox.BuildMessageBox()

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                'Log reject Account Fail and Build Message
                udtSystemMessage = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                Me.udcMessageBox.AddMessage(udtSystemMessage)
                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
                Me.udtAuditLogEntry.AddDescripton("AccountIDList", strAccountIDList)
                Me.udcMessageBox.BuildMessageBox("UpdateFail", Me.udtAuditLogEntry, Common.Component.LogID.LOG00022, AuditLogDescription.RejectEHSAccountFail)
                blnIsValid = False
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            Throw ex
        End Try

        If blnIsValid Then
            Me.lblRejectDateText_vr.Visible = True
            Me.lblRejectDate_vr.Visible = True
            Me.lblNoOfVRAcctRejectedText.Visible = True
            Me.lblNoOfVRAcctRejected.Visible = True

            ViewState("dtmReject") = dtmReject

            Dim udtFormatter As New Formatter

            Me.lblRejectDate_vr.Text = udtFormatter.formatDateTime(dtmReject)
            Me.lblNoOfVRAcctRejected.Text = CStr(dtRecordSelected.Rows.Count)

            'Log reject Account Success
            Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtRecordSelected.Rows.Count)
            'log each transaction ID
            strAccountIDList = String.Empty
            If dtRecordSelected.Rows.Count > 0 Then
                For k = 0 To dtRecordSelected.Rows.Count - 1
                    If strAccountIDList.Trim.Equals(String.Empty) Then
                        strAccountIDList = CStr(dtRecordSelected.Rows(k).Item("Voucher_Acc_ID")).Trim
                    Else
                        strAccountIDList += " ," + CStr(dtRecordSelected.Rows(k).Item("Voucher_Acc_ID")).Trim
                    End If
                Next
            End If
            Me.udtAuditLogEntry.AddDescripton("AccountIDList", strAccountIDList)
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00021, AuditLogDescription.RejectEHSAccountComplete)
        Else

            Me.lblRejectDateText_vr.Visible = False
            Me.lblRejectDate_vr.Visible = False
            Me.lblNoOfVRAcctRejectedText.Visible = False
            Me.lblNoOfVRAcctRejected.Visible = False

        End If

    End Sub

#End Region

#Region "Rejected / Confirmed"

    Private Sub ibtnRejectResultBackToRecordList_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnRejectResultBackToRecordList.Click, ibtnConfirmedResultBackToRecordList.Click
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00035, AuditLogDescription.BackToAccResultFromCompletion)

        Me.mvRecordConfirmation.ActiveViewIndex = 2
        Me.mvVRAcct.ActiveViewIndex = 0
        SearchTempVRAcctRecord(False)
    End Sub

#End Region

    Private Sub ibtnVRAcctInfoBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnVRAcctInfoBack.Click
        Me.mvRecordConfirmation.ActiveViewIndex = 2
        Me.mvVRAcct.ActiveViewIndex = 0
    End Sub

    Private Sub ibtnConfirmRejectTempVRAcct_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnConfirmRejectTempVRAcct.Click

    End Sub



#End Region

#End Region

#Region "Gridview Function - EHS Account"

    Private Sub gvTempVRAcctRecord_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTempVRAcctRecord.PageIndexChanging
        GridViewPageIndexChangingHandler(sender, e, "TempVRAcctRecord")
    End Sub

    Private Sub gvTempVRAcctRecord_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTempVRAcctRecord.PreRender
        GridViewPreRenderHandler(sender, e, "TempVRAcctRecord")
    End Sub

    Private Sub gvTempVRAcctRecord_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTempVRAcctRecord.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then
            Dim strCommandArgument As String
            Dim strTempVRAcctID As String
            Dim strSchemeCode As String

            strCommandArgument = e.CommandArgument.ToString.Trim
            strTempVRAcctID = strCommandArgument.Split("|")(0).Trim
            strSchemeCode = strCommandArgument.Split("|")(1).Trim

            ViewState("TempVRAcctID") = strTempVRAcctID

            Me.mvRecordConfirmation.ActiveViewIndex = 2
            Me.mvVRAcct.ActiveViewIndex = 3

            SetVRAcctInfo(True)

            Me.lblVRAcctIDTemp.Text = strTempVRAcctID
            Me.lblSchemeCodeTemp.Text = strSchemeCode

        End If
    End Sub

    Private Sub gvTempVRAcctRecord_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTempVRAcctRecord.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(3, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        'udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(4, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvTempVRAcctRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTempVRAcctRecord.RowDataBound
        gv_RowDataBound_vr(e)
    End Sub

    Private Sub gv_RowDataBound_vr(ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
            Dim lbtnHKID As LinkButton
            Dim lblIdentityNum As Label
            Dim lblTransactionDtm As Label
            Dim lblEngName As Label
            Dim lblChiName As Label
            Dim lblDOB As Label
            Dim lblSex As Label
            Dim lblDateOfIssue As Label
            Dim imgEC As Image
            Dim intAge As Nullable(Of Integer)
            Dim dtDOR As Nullable(Of Date)
            Dim dtECDate As Nullable(Of Date)
            Dim lblECSerialNo As Label
            Dim lblECRefNo As Label
            Dim hfDocType As HiddenField
            Dim lblForeignPassportNo As Label
            Dim lblPermitToRemain As Label

            Dim udtFormatter As New Formatter

            lbtnHKID = CType(e.Row.FindControl("lbtnHKID"), LinkButton)
            lblIdentityNum = CType(e.Row.FindControl("lblIdentityNum"), Label)
            lblTransactionDtm = CType(e.Row.FindControl("lblTransactionDtm"), Label)
            lblEngName = CType(e.Row.FindControl("lblEngName"), Label)
            lblChiName = CType(e.Row.FindControl("lblChiName"), Label)
            lblDOB = CType(e.Row.FindControl("lblDOB"), Label)
            lblSex = CType(e.Row.FindControl("lblSex"), Label)
            lblDateOfIssue = CType(e.Row.FindControl("lblDateOfIssue"), Label)
            imgEC = CType(e.Row.FindControl("imgEC"), Image)
            lblECSerialNo = CType(e.Row.FindControl("lblECSerialNo"), Label)
            lblECRefNo = CType(e.Row.FindControl("lblECRefNo"), Label)
            hfDocType = CType(e.Row.FindControl("hfDocType"), HiddenField)
            lblForeignPassportNo = CType(e.Row.FindControl("lblForeignPassportNo"), Label)
            lblPermitToRemain = CType(e.Row.FindControl("lblPermitToRemain"), Label)

            Dim lblPracticeName As Label = CType(e.Row.FindControl("lblPractice"), Label)

            Dim dtmTransaction As DateTime = CType(dr.Item("Transaction_Dtm"), DateTime)
            Dim strEngName As String = CStr(dr.Item("Eng_Name"))
            Dim strChiName As String = CStr(dr.Item("Chi_Name"))
            Dim strIdentityNum As String = CStr(dr.Item("IdentityNum"))
            Dim strVoucherAcctID As String = CStr(dr.Item("Voucher_Acc_ID"))
            Dim strSchemeCode As String = CStr(dr.Item("Scheme_Display_Code"))
            Dim dtmDOB As DateTime = CType(dr.Item("DOB"), DateTime)
            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB"))
            Dim strSex As String = CStr(dr.Item("Sex"))
            Dim dtmDateOfIssue As Nullable(Of DateTime)
            Dim strECSerialNo As String
            Dim strECRefNo As String
            Dim strPassportNo As String
            Dim dtmPermit As Nullable(Of Date)
            Dim strAdoptionPrefixNum As String
            Dim strOtherInfo As String
            Dim strECRefNoOtherFormat As String

            If IsDBNull(dr.Item("EC_Date_of_Registration")) Then
                dtDOR = Nothing
            Else
                dtDOR = CType(dr.Item("EC_Date_of_Registration"), Date)
            End If

            If IsDBNull(dr.Item("EC_Age")) Then
                intAge = Nothing
            Else
                intAge = CType(dr.Item("EC_Age"), Integer)
            End If

            If IsDBNull(dr.Item("Date_of_Issue")) Then
                dtmDateOfIssue = Nothing
            Else
                dtmDateOfIssue = CType(dr.Item("Date_of_Issue"), DateTime)
            End If

            If IsDBNull(dr.Item("EC_Serial_No")) Then
                strECSerialNo = String.Empty
            Else
                strECSerialNo = dr.Item("EC_Serial_No").ToString
            End If

            If IsDBNull(dr.Item("EC_Reference_No")) Then
                strECRefNo = String.Empty
            Else
                strECRefNo = dr.Item("EC_Reference_No").ToString
            End If

            If IsDBNull(dr.Item("foreign_passport_no")) Then
                strPassportNo = String.Empty
            Else
                strPassportNo = dr.Item("foreign_passport_no").ToString
            End If

            If IsDBNull(dr.Item("permit_to_remain_until")) Then
                dtmPermit = Nothing
            Else
                dtmPermit = CType(dr.Item("permit_to_remain_until"), Date)
            End If

            If IsDBNull(dr.Item("Adoption_Prefix_Num")) Then
                strAdoptionPrefixNum = String.Empty
            Else
                strAdoptionPrefixNum = CStr(dr.Item("Adoption_Prefix_Num")).Trim
            End If

            If IsDBNull(dr.Item("Other_Info")) Then
                strOtherInfo = String.Empty
            Else
                strOtherInfo = CStr(dr.Item("Other_Info")).Trim
            End If

            If IsDBNull(dr.Item("EC_Reference_No_Other_Format")) Then
                strECRefNoOtherFormat = String.Empty
            Else
                strECRefNoOtherFormat = CStr(dr.Item("EC_Reference_No_Other_Format")).Trim
            End If

            If Not lbtnHKID Is Nothing Then
                lbtnHKID.CommandArgument = strVoucherAcctID & "|" & strSchemeCode
            End If
            lblIdentityNum.Text = "<br>" & udtFormatter.FormatDocIdentityNoForDisplay(CStr(dr.Item("Doc_Code")).Trim, strIdentityNum, False, strAdoptionPrefixNum)


            Dim strPracticeName As String = ""
            Dim strPracticeNameChi As String = ""

            strPracticeName = CStr(dr.Item("Practice_Name")).Trim()
            If Not dr.IsNull("Practice_Name_Chi") Then
                strPracticeNameChi = CStr(dr.Item("Practice_Name_Chi")).Trim()
            End If

            If strPracticeNameChi.Trim() = "" Then
                strPracticeNameChi = strPracticeName
            End If

            ' Handle Mirgration Complete Show Practice Chi
            Dim strHCSPDataMirgrationCompleteTurnOn As String = String.Empty
            Dim strDummy As String = String.Empty

            Dim udtCommFunctBLL As New Common.ComFunction.GeneralFunction()
            udtCommFunctBLL.getSystemParameter("HCSPDataMirgrationCompleteTurnOn", strHCSPDataMirgrationCompleteTurnOn, strDummy)

            ' Handle Mirgration Complete Show Practice Chi
            If strHCSPDataMirgrationCompleteTurnOn.Trim() = "Y" Then
            Else
                strPracticeNameChi = strPracticeName
            End If

            If LCase(Session("language")) = TradChinese OrElse LCase(Session("language")) = SimpChinese Then
                lblPracticeName.Text = strPracticeNameChi
                lblPracticeName.CssClass = "tableTextChi"
            Else
                lblPracticeName.Text = strPracticeName
                lblPracticeName.CssClass = String.Empty
            End If

            lblTransactionDtm.Text = udtFormatter.formatDateTime(dtmTransaction)
            lblEngName.Text = strEngName
            If strChiName.Trim <> "" Then
                lblChiName.Visible = True
                lblChiName.Text = "<br>(" & strChiName.Trim & ")"
            End If

            lblDOB.Text = udtFormatter.formatDOB(CStr(dr.Item("Doc_Code")).Trim, dtmDOB, strExactDOB, Session("language"), intAge, dtDOR, strOtherInfo)
            lblSex.Text = Me.GetGlobalResourceObject("Text", udtFormatter.formatGender(strSex))

            '------------------------------------------Other information-------------------------------------
            If hfDocType.Value.Trim.Equals(Common.Component.DocType.DocTypeModel.DocTypeCode.EC) Then
                ' Serial No.
                If strECSerialNo = String.Empty Then
                    lblECSerialNo.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo") + ": " + Me.GetGlobalResourceObject("Text", "NotProvided") + "<br>"
                Else
                    lblECSerialNo.Text = Me.GetGlobalResourceObject("Text", "ECSerialNo") + ": " + strECSerialNo + "<br>"
                End If

                ' Reference
                If strECRefNoOtherFormat = "Y" Then
                    lblECRefNo.Text = Me.GetGlobalResourceObject("Text", "ECReference") + ": " + strECRefNo
                Else
                    lblECRefNo.Text = Me.GetGlobalResourceObject("Text", "ECReference") + ": " + udtFormatter.formatReferenceNo(strECRefNo, False)
                End If

                If IsDBNull(dr.Item("Date_Of_Issue")) Then
                    dtECDate = Nothing
                Else
                    dtECDate = CType(dr.Item("Date_Of_Issue"), Date)
                End If
            Else
                lblECSerialNo.Text = String.Empty
                lblECRefNo.Text = String.Empty
            End If

            If strPassportNo.Trim.Equals(String.Empty) Then
                lblForeignPassportNo.Text = String.Empty
            Else
                lblForeignPassportNo.Text = Me.GetGlobalResourceObject("Text", "PassportNo") + ": " + strPassportNo
            End If

            If IsNothing(dtmPermit) Then
                lblPermitToRemain.Text = String.Empty
            Else
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'lblPermitToRemain.Text = Me.GetGlobalResourceObject("Text", "PermitToRemain") + ": " + udtFormatter.formatEnterDate(dtmPermit)
                lblPermitToRemain.Text = Me.GetGlobalResourceObject("Text", "PermitToRemain") + ": " + udtFormatter.formatID235BPermittedToRemainUntil(dtmPermit)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If

            'show -- in "Other information" if nothing is shown
            If lblPermitToRemain.Text = String.Empty And lblECSerialNo.Text = String.Empty And lblECRefNo.Text = String.Empty And lblForeignPassportNo.Text = String.Empty Then
                lblECRefNo.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If
            '------------------------------------------------------------------------------------------

            'Date of Issue
            lblDateOfIssue.Text = udtFormatter.formatDOI_GV(dtmDateOfIssue)
            If lblDateOfIssue.Text.Trim.Equals(String.Empty) Then
                lblDateOfIssue.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If
        End If
    End Sub

    Private Sub gvTempVRAcctRecord_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvTempVRAcctRecord.Sorting
        GridViewSortingHandler(sender, e, "TempVRAcctRecord")
    End Sub

    Private Sub gvSelectedRecord_vr_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSelectedRecord_vr.PreRender
        GridViewPreRenderHandler(sender, e, "RecordSelected")

        Dim dtTempVRAcctRecord As DataTable
        Dim blnECfound As Boolean = False
        Dim blnVISAfound As Boolean = False
        Dim blnPermitRemainfound As Boolean = False

        dtTempVRAcctRecord = CType(Session("RecordSelected"), DataTable)
        For Each dr As DataRow In dtTempVRAcctRecord.Rows
            'EC checking
            If dr.Item("doc_display_code").ToString.Trim = Common.Component.DocType.DocTypeModel.DocTypeCode.EC Then
                blnECfound = True
                Exit For
            End If
            'Visa Checking
            If dr.Item("doc_display_code").ToString.Trim = Common.Component.DocType.DocTypeModel.DocTypeCode.VISA Then
                blnVISAfound = True
                Exit For
            End If
            'Permit To Remain checking
            If dr.Item("doc_display_code").ToString.Trim = Common.Component.DocType.DocTypeModel.DocTypeCode.ID235B Then
                blnPermitRemainfound = True
                Exit For
            End If
        Next

        If dtTempVRAcctRecord.Rows.Count > 0 Then
            If blnECfound Or blnVISAfound Or blnPermitRemainfound Then
                Me.gvSelectedRecord_vr.Columns.Item(6).Visible = True
                Me.gvSelectedRecord_vr.Width = 1290
            Else
                Me.gvSelectedRecord_vr.Columns.Item(6).Visible = False
                Me.gvSelectedRecord_vr.Width = 1070
            End If
        End If


    End Sub

    Private Sub gvSelectedRecord_vr_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSelectedRecord_vr.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(0, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(1, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        'udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvSelectedRecord_vr_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSelectedRecord_vr.RowDataBound
        gv_RowDataBound_vr(e)
    End Sub

    Protected Sub gvSelectedRecord_vr_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSelectedRecord_vr.SelectedIndexChanged

    End Sub

    Private Sub gvSelectedRecord_vr_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSelectedRecord_vr.Sorting
        GridViewSortingHandler(sender, e, "RecordSelected")
    End Sub

#End Region

#Region "Gridview Funtion - Transaction"

#Region "Select Record"
    Private Sub gvClaimRecord_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvClaimRecord.DataBinding

    End Sub

    Private Sub gvClaimRecord_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvClaimRecord.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, "ClaimRecord")
    End Sub

    Private Sub gvClaimRecord_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvClaimRecord.Sorting
        Me.GridViewSortingHandler(sender, e, "ClaimRecord")
    End Sub

    Private Sub gvClaimRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvClaimRecord.RowDataBound
        gv_RowDataBound(e)
    End Sub

    Private Sub gvClaimRecord_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvClaimRecord.PreRender
        Me.GridViewPreRenderHandler(sender, e, "ClaimRecord")
    End Sub

    Protected Sub gvClaimRecord_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub gvClaimRecord_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvClaimRecord.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(4, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        ' Add the subsidy legend only if in non-CN platform
        If Me.SubPlatform <> EnumHCSPSubPlatform.CN Then
            udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(9, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        End If

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvClaimRecord_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvClaimRecord.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            Dim strTransactionID As String = ""
            strTransactionID = e.CommandArgument.ToString.Trim

            Me.udtSessionHandler.EHSTransactionRemoveFromSession(Me.strFuncCode)
            Me.udtEHSTransactionModel = Me.udtEHSTransactionBLL.LoadClaimTran(strTransactionID)
            Me.udtSessionHandler.EHSTransactionSaveToSession(udtEHSTransactionModel, Me.strFuncCode)

            'Log view details 
            Me.udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me)
            Me.udtAuditLogEntry.AddDescripton("TransactionNo", strTransactionID)
            Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00004, AuditLogDescription.ViewTransactionDetail)

            'ViewState("TransactionID") = strTransactionID


            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            '-- ---------------------------------------------- --
            If Not udtEHSTransactionModel Is Nothing Then
                Me.BuildRedirectButton(Me.ibtnManagement, ClaimTransactionMaintenance.BuildSearchCriteria(Common.Component.ClaimTransStatus.Incomplete, _
                                                                                                                     udtEHSTransactionModel.TransactionDtm, _
                                                                                                                     udtEHSTransactionModel.TransactionDtm, _
                                                                                                                    udtEHSTransactionModel.TransactionID))
            End If
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            ' go to vTransactionDetail
            Me.mvRecordConfirmation.ActiveViewIndex = 1
            Me.mvClaimTrans.ActiveViewIndex = 3
            udcClaimTranEnquiry.buildClaimObject(udtEHSTransactionModel.TransactionID, udtEHSTransactionModel, False)
            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Me.udcClaimTranEnquiry.chgLanguage()
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
            SetClaimInfo(0)
        End If
    End Sub

#End Region

#Region "Confirm Record"
    Private Sub gvSelectedRecord_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSelectedRecord.PreRender
        Me.GridViewPreRenderHandler(sender, e, "RecordSelected")
    End Sub

    Private Sub gvSelectedRecord_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSelectedRecord.Sorting
        Me.GridViewSortingHandler(sender, e, "RecordSelected")
    End Sub

    Private Sub gvSelectedRecord_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSelectedRecord.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(1, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(3, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        ' Add the subsidy legend only if in non-CN platform
        If Me.SubPlatform <> EnumHCSPSubPlatform.CN Then
            udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(8, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        End If

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvSelectedRecord_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSelectedRecord.RowDataBound
        gv_RowDataBound(e)
    End Sub

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub gvSelectedRecord_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSelectedRecord.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, "RecordSelected")
    End Sub
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    Protected Sub gvSelectedRecord_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSelectedRecord.SelectedIndexChanged

    End Sub

#End Region

#Region "Common"
    Private Sub gv_RowDataBound(ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Chris YIM]
        ' ----------------------------------------------------------------------------------------
        'If e.Row.RowType = DataControlRowType.Header Then
        '    e.Row.Cells.Item(5).Attributes.Add("title", Me.GetGlobalResourceObject("Text", "DocumentType") + Environment.NewLine + Me.GetGlobalResourceObject("Text", "IdentityDocNo"))
        'End If
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Chris YIM]

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
            Dim lbtnTransactionID As LinkButton = Nothing
            Dim lblTransactionID As Label = Nothing
            Dim lblTransactionDtm As Label
            Dim lblTotalAmount As Label
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim lblTotalAmountRMB As Label
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            Dim lblEngName As Label
            Dim lblChiName As Label
            'Dim lblBankAccountNo As Label
            Dim lblIdentityNum As Label
            'Dim imgEC As Image
            Dim udtFormatter As New Formatter
            Dim lblChiTransactionDtm As Label
            Dim lblDataEntryBy As Label
            Dim lblVia As Label
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -- --------------------------------------------- --
            Dim lblTransactionStatus As Label = CType(e.Row.FindControl("lblTranListTranStatus"), Label)
            Dim hfTransactionStatus As HiddenField = CType(e.Row.FindControl("hfTranListTranStatus"), HiddenField)
            Dim chkSelect As CheckBox = CType(e.Row.FindControl("chkSelect"), CheckBox)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            lblTransactionDtm = CType(e.Row.FindControl("lblTransactionDtm"), Label)
            lblTotalAmount = CType(e.Row.FindControl("lblTotalAmount"), Label)
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            lblTotalAmountRMB = CType(e.Row.FindControl("lblTotalAmountRMB"), Label)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            lblEngName = CType(e.Row.FindControl("lblEngName"), Label)
            lblChiName = CType(e.Row.FindControl("lblChiName"), Label)
            'lblBankAccountNo = CType(e.Row.FindControl("lblBankAccountNo"), Label)
            lblIdentityNum = CType(e.Row.FindControl("lblIdentityNum"), Label)
            'imgEC = CType(e.Row.FindControl("imgEC"), Image)
            lblChiTransactionDtm = CType(e.Row.FindControl("lblChiTransactionDtm"), Label)
            lblDataEntryBy = CType(e.Row.FindControl("lblDataEntryBy"), Label)
            lblVia = CType(e.Row.FindControl("lblVia"), Label)

            Dim lblPracticeName As Label = CType(e.Row.FindControl("lblPracticeName"), Label)

            Dim strTransactionID As String = CStr(dr.Item("Transaction_ID"))
            Dim dtmTransaction As DateTime = CType(dr.Item("Transaction_Dtm"), DateTime)

            ' CRE13-001 EHAPP [Start][Karl]
            ' -----------------------------------------------------------------------------------------
            'Dim dblTotalAmount As Double = CDbl(dr.Item("Total_Amount"))
            Dim strTotalAmount As String

            If IsDBNull(dr.Item("Total_Amount")) = True Then
                strTotalAmount = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
            Else
                'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Chris YIM]
                ' ----------------------------------------------------------------------------------------
                'strTotalAmount = CDbl(dr.Item("Total_Amount")).ToString("#,##0")
                strTotalAmount = udtFormatter.formatMoney(dr.Item("Total_Amount").ToString, False)
                'CRE13-018 Change Voucher Amount to 1 Dollar [End][Chris YIM]
            End If

            ' CRE13-001 EHAPP [End][Karl]

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim strTotalAmountRMB As String

            If IsDBNull(dr.Item("Total_Amount_RMB")) = True Then
                strTotalAmountRMB = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
            Else
                'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Chris YIM]
                ' ----------------------------------------------------------------------------------------
                'strTotalAmount = CDbl(dr.Item("Total_Amount")).ToString("#,##0")
                strTotalAmountRMB = udtFormatter.formatMoneyRMB(dr.Item("Total_Amount_RMB").ToString, False)
                'CRE13-018 Change Voucher Amount to 1 Dollar [End][Chris YIM]
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            Dim strEngName As String = CStr(dr.Item("Eng_Name"))
            Dim strChiName As String = CStr(dr.Item("Chi_Name"))
            Dim strBankAccountNo As String = CStr(dr.Item("Bank_Account_No"))
            Dim strIdentityNum As String = CStr(dr.Item("IdentityNum"))
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -- --------------------------------------------- --
            Dim strRecord_Status As String = CStr(dr.Item("Record_Status"))
            If Not chkSelect Is Nothing Then
                If strRecord_Status = ClaimTransStatus.Incomplete Then
                    chkSelect.Enabled = False
                    chkSelect.Checked = False
                Else
                    chkSelect.Enabled = True
                End If
            End If
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            Dim strPracticeName As String = ""
            Dim strPracticeNameChi As String = ""

            Dim strAdoptionPrefixNum As String = String.Empty

            If Not IsDBNull(dr.Item("Adoption_Prefix_Num")) Then
                strAdoptionPrefixNum = CStr(dr.Item("Adoption_Prefix_Num")).Trim
            End If

            strPracticeName = CStr(dr.Item("Practice_Name")).Trim()
            If Not dr.IsNull("Practice_Name_Chi") Then
                strPracticeNameChi = CStr(dr.Item("Practice_Name_Chi")).Trim()
            End If

            If strPracticeNameChi.Trim() = "" Then
                strPracticeNameChi = strPracticeName
            End If

            ' Handle Mirgration Complete Show Practice Chi
            Dim strHCSPDataMirgrationCompleteTurnOn As String = String.Empty
            Dim strDummy As String = String.Empty

            Dim udtCommFunctBLL As New Common.ComFunction.GeneralFunction()
            udtCommFunctBLL.getSystemParameter("HCSPDataMirgrationCompleteTurnOn", strHCSPDataMirgrationCompleteTurnOn, strDummy)

            ' Handle Mirgration Complete Show Practice Chi
            If strHCSPDataMirgrationCompleteTurnOn.Trim() = "Y" Then
            Else
                strPracticeNameChi = strPracticeName
            End If

            If Not e.Row.FindControl("lbtnTransactionID") Is Nothing Then
                lbtnTransactionID = CType(e.Row.FindControl("lbtnTransactionID"), LinkButton)
                lbtnTransactionID.Text = udtFormatter.formatSystemNumber(strTransactionID.Trim)
            End If
            If Not e.Row.FindControl("lblTransactionID") Is Nothing Then
                lblTransactionID = CType(e.Row.FindControl("lblTransactionID"), Label)
                lblTransactionID.Text = udtFormatter.formatSystemNumber(strTransactionID.Trim)
            End If
            lblTransactionDtm.Text = udtFormatter.formatDateTime(dtmTransaction)

            ' CRE13-001 EHAPP [Start][Karl]
            ' -----------------------------------------------------------------------------------------
            'lblTotalAmount.Text = dblTotalAmount.ToString("#,##0")
            lblTotalAmount.Text = strTotalAmount
            ' CRE13-001 EHAPP [End][Karl]

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            lblTotalAmountRMB.Text = strTotalAmountRMB
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]


            lblEngName.Text = strEngName
            If strChiName.Trim <> "" Then
                lblChiName.Visible = True
                lblChiName.Text = "<br>(" & strChiName.Trim & ")"
            End If
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -- --------------------------------------------- --         
            Dim strTmpTxEng As String = String.Empty
            Dim strTmpTxChi As String = String.Empty
            Dim strTmpTxCN As String = String.Empty
            hfTransactionStatus.Value = strRecord_Status
            Status.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, hfTransactionStatus.Value.Trim, _
                        strTmpTxEng, strTmpTxChi, strTmpTxCN)

            If LCase(Session("language")) = TradChinese Then
                lblTransactionStatus.Text = strTmpTxChi
                lblTransactionStatus.Font.Name = "HA_MingLiu"
            ElseIf LCase(Session("language")) = SimpChinese Then
                lblTransactionStatus.Text = strTmpTxCN
            Else
                lblTransactionStatus.Text = strTmpTxEng
            End If
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]


            'lblBankAccountNo.Text = udtFormatter.maskBankAccount(strBankAccountNo)
            lblIdentityNum.Text = "<br>" & udtFormatter.FormatDocIdentityNoForDisplay(CStr(dr.Item("Doc_Code")).Trim, strIdentityNum, True, strAdoptionPrefixNum)


            'Dim lblReasonForVisit As Label
            'lblReasonForVisit = CType(e.Row.FindControl("lblReasonForVisit"), Label)
            Dim lblOtherInformation As Label
            lblOtherInformation = CType(e.Row.FindControl("lblOtherInformation"), Label)


            ' INT16-0032 Long term fix for HCSP Record Confirmation [Start][Winnie]
            If LCase(Session("language")) = TradChinese Then
                lblOtherInformation.ToolTip = CStr(dr.Item("Details_Chi")).Trim
            ElseIf LCase(Session("language")) = SimpChinese Then
                lblOtherInformation.ToolTip = CStr(dr.Item("Details_CN")).Trim
            Else
                lblOtherInformation.ToolTip = CStr(dr.Item("Details")).Trim
            End If
            ' INT16-0032 Long term fix for HCSP Record Confirmation [End][Winnie]

            'Dim strReasonForVisit As String
            'Dim strReasonForVisitChi As String
            'strReasonForVisit = CStr(dr.Item("Details"))
            'strReasonForVisitChi = CStr(dr.Item("Details"))

            'If Thread.CurrentThread.CurrentUICulture.Name.ToLower = "zh-tw" Then
            '    If strReasonForVisitChi.Trim.Equals(String.Empty) Then
            '        lblOtherInformation.Enabled = True
            '        lblOtherInformation.Text = Me.GetGlobalResourceObject("Text", "Details")
            '    Else
            '        lblOtherInformation.Enabled = True
            '        lblOtherInformation.Text = Me.GetGlobalResourceObject("Text", "Details")
            '        lblOtherInformation.ToolTip = strReasonForVisitChi
            '        'lblOtherInformation.Style.Value = "text-decoration: underline"
            '    End If
            '    'lblReasonForVisit.Text = strReasonForVisitChi
            'Else
            '    If strReasonForVisit.Trim.Equals(String.Empty) Then
            '        lblOtherInformation.Enabled = True
            '        lblOtherInformation.Text = Me.GetGlobalResourceObject("Text", "Details")
            '    Else
            '        lblOtherInformation.Enabled = True
            '        lblOtherInformation.Text = Me.GetGlobalResourceObject("Text", "Details")
            '        lblOtherInformation.ToolTip = strReasonForVisit
            '        'lblOtherInformation.Style.Value = "text-decoration: underline"
            '    End If
            '    'lblReasonForVisit.Text = strReasonForVisit
            'End If

            If LCase(Session("language")) = TradChinese OrElse LCase(Session("language")) = SimpChinese Then
                lblPracticeName.Text = strPracticeNameChi
            Else
                lblPracticeName.Text = strPracticeName
            End If

            'If Not imgEC.AlternateText.Equals("Y") Then
            '    imgEC.Visible = True
            '    imgEC.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ExemptionCert")
            'Else
            '    imgEC.Visible = False
            'End If
            If CStr(dr.Item("IsUpload")).Trim = "Y" Then
                'lblDataEntryBy.Text = lblDataEntryBy.Text
                lblDataEntryBy.ForeColor = System.Drawing.Color.Brown
                lblVia.ForeColor = System.Drawing.Color.Brown
                lblVia.Visible = True
            Else
                lblVia.Visible = False
            End If
        End If
    End Sub
#End Region

#End Region

#Region "MuiltView function"
    Private Sub mvClaimTrans_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvClaimTrans.ActiveViewChanged
        Me.udcMessageBox.Visible = False
        Dim intActiveIndex As Integer = Me.mvClaimTrans.ActiveViewIndex

        If intActiveIndex = 2 OrElse intActiveIndex = 4 Then

        Else
            Me.udcInfoMessageBox.Visible = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ConfirmSelection", "setTimeout('window.scrollBy(0, -(document.body.clientHeight));', 1); ", True)
        End If

        If Not intActiveIndex = 3 AndAlso Not Me.mvRecordConfirmation.ActiveViewIndex = 1 Then
            Me.udtSessionHandler.EHSTransactionRemoveFromSession(strFuncCode)
        End If

        ' CRE11-004
        Select Case mvClaimTrans.ActiveViewIndex
            Case 0, 1, 2
                Me.ClearWorkingData()
            Case 3, 4
                ' Keep working data
        End Select

    End Sub

    Private Sub mvVRAcct_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvVRAcct.ActiveViewChanged
        Me.udcMessageBox.Visible = False
        Dim intActiveIndex As Integer = Me.mvVRAcct.ActiveViewIndex

        If intActiveIndex = 3 OrElse intActiveIndex = 4 Then

        Else
            Me.udcInfoMessageBox.Visible = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "ConfirmSelection", "setTimeout('window.scrollBy(0, -(document.body.clientHeight));', 1); ", True)
        End If
    End Sub

    Private Sub mvRecordConfirmation_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvRecordConfirmation.ActiveViewChanged
        Me.udcMessageBox.Visible = False
        Me.udcInfoMessageBox.Visible = False

        If Me.mvClaimTrans.ActiveViewIndex <> 3 Then
            Me.udtSessionHandler.EHSTransactionRemoveFromSession(strFuncCode)
        End If

        ' CRE11-004
        ' Handle working data on view change, clear working data if no use       
        Select Case mvRecordConfirmation.ActiveViewIndex
            Case 0
                Me.ClearWorkingData()
            Case 1
                ' Do Nothing (Keep working data)            
            Case 2
                Me.ClearWorkingData()
        End Select
    End Sub
#End Region

#Region "Support Function"

    Private Sub GetCurrentUser(ByRef _udtSP As ServiceProviderModel, ByRef _udtDataEntry As DataEntryUserModel)
        Me.udtSessionHandler.CurrentUserGetFromSession(_udtSP, _udtDataEntry)

        If IsNothing(_udtSP) Then
            Dim udtClaimVoucherBLL As New ClaimVoucherBLL

            'Get Current USer Account
            Me.udtUserAC = UserACBLL.GetUserAC

            If Me.udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
                'Get SP from form database
                _udtSP = CType(udtUserAC, ServiceProviderModel)
                _udtSP = Me.udtClaimVoucherBLL.loadSP(_udtSP.SPID, Me.SubPlatform)

                _udtDataEntry = Nothing

            ElseIf Me.udtUserAC.UserType = Common.Component.SPAcctType.DataEntryAcct Then
                _udtDataEntry = CType(udtUserAC, DataEntryUserModel)

                Dim udtDataEntryAcctBLL As BLL.DataEntryAcctBLL = New BLL.DataEntryAcctBLL
                _udtDataEntry = udtDataEntryAcctBLL.LoadDataEntry(_udtDataEntry.SPID, _udtDataEntry.DataEntryAccount)
                _udtSP = Me.udtClaimVoucherBLL.loadSP(_udtDataEntry.SPID, Me.SubPlatform)
            End If

            Me.udtSessionHandler.CurrentUserSaveToSession(_udtSP, _udtDataEntry)

        End If

    End Sub

    Private Sub ResetControls()

        Dim udtFormatter As New Formatter
        Dim udtPracticeBankAcctBLL As New PracticeBankAcctBLL

        Me.GetCurrentUser(udtSP, udtDataEntry)

        If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
            chkIncludeIncompleteClaim.Visible = False
            trIncludeIncompleteClaim.Style("display") = "none"
        End If

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'Me.txtCutOffDate.Text = DateTime.Now.ToString(udtFormatter.EnterDateFormat)
        Me.txtCutOffDate.Text = DateTime.Now.ToString(udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Dim dtPracticeList = udtPracticeBankAcctBLL.getAllPractice(udtSP.SPID, PracticeBankAcctBLL.PracticeDisplayType.Practice)
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        FilterPracticeList(dtPracticeList, udtSP.PracticeList)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        Me.ddlPractice.DataSource = dtPracticeList

        ' Handle Practice display Chinese
        If LCase(Session("language")) = TradChinese OrElse LCase(Session("language")) = SimpChinese Then
            Me.ddlPractice.DataTextField = PracticeBankAcctBLL.PracticeDisplayField.Display_Chi
        Else
            Me.ddlPractice.DataTextField = PracticeBankAcctBLL.PracticeDisplayField.Display_Eng
        End If

        Me.ddlPractice.DataValueField = "PracticeID"
        Me.ddlPractice.DataBind()
        Me.ddlPractice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        Session(SESS_Practice) = dtPracticeList

        Dim udtDataEntryAccBLL As DataEntryAcctBLL = New DataEntryAcctBLL

        Dim dtDataEntryList As New DataTable()
        If ddlPractice.SelectedValue.Trim.Equals(String.Empty) Then
            dtDataEntryList = udtDataEntryAccBLL.getDataEntryAcctBySPPracticeID(udtSP.SPID, Nothing)
        Else
            dtDataEntryList = udtDataEntryAccBLL.getDataEntryAcctBySPPracticeID(udtSP.SPID, CInt(ddlPractice.SelectedValue.Trim))
        End If

        Me.ddlDataEntry.DataSource = dtDataEntryList
        Session(SESS_DataEntry) = dtDataEntryList

        Me.ddlDataEntry.DataTextField = "Data_Entry_Account"
        Me.ddlDataEntry.DataValueField = "Data_Entry_Account"
        Me.ddlDataEntry.DataBind()
        Me.ddlDataEntry.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        Me.AddExternalAccountToDataEntryList()

        ' Bind scheme information
        Dim udtPracticeSchemeInfoModelCollection As PracticeSchemeInfoModelCollection = New PracticeSchemeInfoModelCollection
        For Each udtPracticeModel As PracticeModel In udtSP.PracticeList.Values
            For Each udtPracticeSchemeInfoModel As PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
                udtPracticeSchemeInfoModelCollection.Add(udtPracticeSchemeInfoModel)
            Next
        Next

        Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtPracticeSchemeInfoModelCollection)

        ddlScheme.DataSource = udtSchemeClaimModelCollection
        ddlScheme.DataValueField = "SchemeCode"
        If LCase(Session("language")) = TradChinese Then
            Me.ddlScheme.DataTextField = "SchemeDescChi"
        ElseIf LCase(Session("language")) = SimpChinese Then
            Me.ddlScheme.DataTextField = "SchemeDescCN"
        Else
            Me.ddlScheme.DataTextField = "SchemeDesc"
        End If
        ddlScheme.DataBind()
        Me.ddlScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Winnie]
        Me.lblSchemeResult.Text = AntiXssEncoder.HtmlEncode(Me.ddlScheme.SelectedItem.Text.Trim(), True)
        Me.lblSchemeResult_vr.Text = AntiXssEncoder.HtmlEncode(Me.ddlScheme.SelectedItem.Text.Trim(), True)
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Winnie]

        Session(SESS_Scheme) = udtSchemeClaimModelCollection
        Session(SESS_ALLScheme) = udtSchemeClaimModelCollection
        Session(SESS_PracticeSchemeInfo) = udtPracticeSchemeInfoModelCollection

        ResetGridViewSelection()
    End Sub

    Private Sub ResetGridViewSelection()
        Dim i As Integer
        Dim chkSelect As CheckBox
        Dim chkSelectAll As CheckBox
        If Not Me.gvClaimRecord.HeaderRow Is Nothing Then
            chkSelectAll = CType(Me.gvClaimRecord.HeaderRow.Cells.Item(1).FindControl("chkSelectAll"), CheckBox)
            chkSelectAll.Checked = False
        End If
        For i = 0 To Me.gvClaimRecord.Rows.Count - 1
            chkSelect = CType(Me.gvClaimRecord.Rows.Item(i).FindControl("chkSelect"), CheckBox)
            chkSelect.Checked = False
        Next

        If Not Me.gvTempVRAcctRecord.HeaderRow Is Nothing Then
            chkSelectAll = CType(Me.gvTempVRAcctRecord.HeaderRow.Cells.Item(1).FindControl("chkSelectAll"), CheckBox)
            chkSelectAll.Checked = False
        End If
        For i = 0 To Me.gvTempVRAcctRecord.Rows.Count - 1
            chkSelect = CType(Me.gvTempVRAcctRecord.Rows.Item(i).FindControl("chkSelect"), CheckBox)
            chkSelect.Checked = False
        Next
    End Sub

    Private Sub ReRenderPage()

        Dim udtFormatter As New Formatter

        If Not ViewState("ClaimConfirmationDate") Is Nothing Then
            Dim dtmClaimConfirmationDate As DateTime
            dtmClaimConfirmationDate = CType(ViewState("ClaimConfirmationDate"), DateTime)
            Me.lblClaimConfirmationDate.Text = udtFormatter.formatDateTime(dtmClaimConfirmationDate)
        End If

        If Not ViewState("RejectDatetime") Is Nothing Then
            Dim dtmReject As DateTime
            dtmReject = CType(ViewState("RejectDatetime"), DateTime)
            Me.lblRejectDate.Text = udtFormatter.formatDateTime(dtmReject)
        End If

        If Not ViewState("ConfirmDatetime") Is Nothing Then
            Dim dtmConfirm As DateTime
            dtmConfirm = CType(ViewState("ConfirmDatetime"), DateTime)
            Me.lblVRAcctConfirmationDate.Text = udtFormatter.formatDateTime(dtmConfirm)
        End If

        If Not ViewState("lblClaimInfo") Is Nothing Then
            Dim strVRAcctInfo As String = CStr(ViewState("lblClaimInfo"))
            Me.lblClaimInfo.Text = Me.GetGlobalResourceObject("Text", strVRAcctInfo)
        End If

        'gvClaimRecord.Columns.Item(9).HeaderText = Me.GetGlobalResourceObject("Text", "TotalAmount") & " ($)"
        'gvSelectedRecord.Columns.Item(8).HeaderText = Me.GetGlobalResourceObject("Text", "TotalAmount") & " ($)"

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
            gvClaimRecord.Columns(6).Visible = True    'lblTotalAmountRMB
        Else
            gvClaimRecord.Columns(6).Visible = False   'lblTotalAmountRMB
        End If
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If Me.ddlPractice.SelectedIndex = 0 Then
            Me.lblPracticeResult.Text = Me.GetGlobalResourceObject("Text", "Any")
        End If
        If Me.ddlDataEntry.SelectedIndex = 0 Then
            Me.lblDataEntryResult.Text = Me.GetGlobalResourceObject("Text", "Any")
        End If

        If Not ViewState("CutOffDate") Is Nothing Then
            Dim dtmCutOffDate As DateTime
            dtmCutOffDate = CType(ViewState("CutOffDate"), DateTime)
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL
            udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)
            'Me.lblCutoffDateResult.Text = udtFormatter.formatDate(dtmCutOffDate)
            'Me.lblCutoffDateResult_vr.Text = udtFormatter.formatDate(dtmCutOffDate)
            Me.lblCutoffDateResult.Text = udtFormatter.formatDisplayDate(dtmCutOffDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            Me.lblCutoffDateResult_vr.Text = udtFormatter.formatDisplayDate(dtmCutOffDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        End If

        Me.ibtnConfirmTransaction.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")

        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.UpdateImageURL(Me.ibtnConfirmTransaction)

        Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
        If Not controlID Is Nothing AndAlso (controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) OrElse controlID.Equals(SelectSimpChinese)) Then

            Me.ddlPractice.Items(0).Text = Me.GetGlobalResourceObject("Text", "Any")
            Me.ddlDataEntry.Items(0).Text = Me.GetGlobalResourceObject("Text", "Any")

            'If Me.mvRecordConfirmation.ActiveViewIndex = 1 And Me.mvClaimTrans.ActiveViewIndex = 0 Then
            If gvClaimRecord.Rows.Count > 0 Then
                Me.GridViewDataBind(Me.gvClaimRecord, Session("ClaimRecord"))
            End If
            'ElseIf Me.mvRecordConfirmation.ActiveViewIndex = 1 And Me.mvClaimTrans.ActiveViewIndex = 1 Then

            If Me.mvRecordConfirmation.ActiveViewIndex = 1 And Me.mvClaimTrans.ActiveViewIndex = 1 Then
                If Me.gvSelectedRecord.Rows.Count > 0 Then
                    Me.GridViewDataBind(Me.gvSelectedRecord, Session("RecordSelected"))
                End If
            End If
        End If

        ' Handle Practice Bank Change Language

        Dim strPracticeSelected As String = Me.ddlPractice.SelectedValue.Trim
        Dim dtPracticeList As DataTable = CType(Session(SESS_Practice), DataTable)
        Me.ddlPractice.Items.Clear()
        Me.ddlPractice.DataSource = dtPracticeList
        If LCase(Session("language")) = TradChinese OrElse LCase(Session("language")) = SimpChinese Then
            Me.ddlPractice.DataTextField = PracticeBankAcctBLL.PracticeDisplayField.Display_Chi
        Else
            Me.ddlPractice.DataTextField = PracticeBankAcctBLL.PracticeDisplayField.Display_Eng
        End If

        Me.ddlPractice.DataBind()
        Me.ddlPractice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlPractice.SelectedValue = strPracticeSelected

        Me.lblPracticeResult.Text = Me.ddlPractice.SelectedItem.Text.Trim()
        Me.lblPracticeResult_vr.Text = Me.ddlPractice.SelectedItem.Text.Trim()


        Dim strDataEntrySelected As String = Me.ddlDataEntry.SelectedValue
        Dim dtDataEntryList As DataTable = CType(Session(SESS_DataEntry), DataTable)
        Me.ddlDataEntry.Items.Clear()
        Me.ddlDataEntry.DataSource = dtDataEntryList
        Me.ddlDataEntry.DataBind()
        Me.ddlDataEntry.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.AddExternalAccountToDataEntryList()
        If rbConfirmTypeList.SelectedIndex = 0 Or strDataEntrySelected <> "Upload" Then
            Me.ddlDataEntry.SelectedValue = strDataEntrySelected
        Else
            Me.ddlDataEntry.SelectedValue = ""
        End If


        Me.udcClaimTranEnquiry.chgLanguage()

        ' Handle Scheme Change Language
        If IsPostBack Then
            'If the post back is triggered by index change of practice drop down list, skip refresh of scheme list in ReRenderPage
            If controlID.Equals("ctl00$ContentPlaceHolder1$ddlPractice") Then
            Else
                Dim strSchemeSelected As String = Me.ddlScheme.SelectedValue
                Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = CType(Session(SESS_Scheme), SchemeClaimModelCollection)
                Me.ddlScheme.Items.Clear()
                Me.ddlScheme.DataSource = udtSchemeClaimModelCollection
                If LCase(Session("language")) = TradChinese Then
                    Me.ddlScheme.DataTextField = "SchemeDescChi"
                ElseIf LCase(Session("language")) = SimpChinese Then
                    Me.ddlScheme.DataTextField = "SchemeDescCN"
                Else
                    Me.ddlScheme.DataTextField = "SchemeDesc"
                End If
                Me.ddlScheme.DataBind()
                Me.ddlScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
                Me.ddlScheme.SelectedValue = strSchemeSelected

                Me.lblSchemeResult.Text = Me.ddlScheme.SelectedItem.Text.Trim()
                Me.lblSchemeResult_vr.Text = Me.ddlScheme.SelectedItem.Text.Trim()
            End If
        End If

        '''''''''''''''''''''''''''''''''''''''''''''''''

        If Not controlID Is Nothing AndAlso (controlID.Equals(SelectTradChinese) OrElse controlID.Equals(SelectEnglish) OrElse controlID.Equals(SelectSimpChinese)) Then

            Me.ddlPractice.Items(0).Text = Me.GetGlobalResourceObject("Text", "Any")
            Me.ddlDataEntry.Items(0).Text = Me.GetGlobalResourceObject("Text", "Any")

            If Me.ddlPractice.SelectedIndex = 0 Then
                Me.lblPracticeResult_vr.Text = Me.GetGlobalResourceObject("Text", "Any")
            End If
            If Me.ddlDataEntry.SelectedIndex = 0 Then
                Me.lblDataEntryResult_vr.Text = Me.GetGlobalResourceObject("Text", "Any")
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            If Me.ddlDataEntry.SelectedIndex = 0 Then
                Me.lblSchemeResult_vr.Text = Me.GetGlobalResourceObject("Text", "Any")
            End If
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            If Not ViewState("CuttOffDate") Is Nothing Then
                Dim dtmCutOffDate As DateTime
                dtmCutOffDate = CType(ViewState("CuttOffDate"), DateTime)
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim udtSubPlatformBLL As New SubPlatformBLL
                'Me.lblCutoffDateResult_vr.Text = udtFormatter.formatDate(dtmCutOffDate)
                Me.lblCutoffDateResult_vr.Text = udtFormatter.formatDisplayDate(dtmCutOffDate, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            End If

            If Not ViewState("lblVRAcctInfo") Is Nothing Then
                Dim strVRAcctInfo As String = CStr(ViewState("lblVRAcctInfo"))
                Me.lblVRAcctInfo.Text = Me.GetGlobalResourceObject("Text", strVRAcctInfo)
            End If

            If Not ViewState("dtmReject") Is Nothing Then
                Dim dtmReject As DateTime = CType(ViewState("dtmReject"), DateTime)
                Me.lblRejectDate_vr.Text = udtFormatter.formatDateTime(dtmReject)
            End If

            If Not ViewState("lblConfirmRecord") Is Nothing Then
                Dim strConfirmRecord As String = CStr(ViewState("lblConfirmRecord"))
                Me.lblConfirmRecord_vr.Text = Me.GetGlobalResourceObject("Text", strConfirmRecord)
            End If

            'If Me.mvRecordConfirmation.ActiveViewIndex = 2 And Me.mvVRAcct.ActiveViewIndex = 0 Then
            If Me.gvTempVRAcctRecord.Rows.Count > 0 Then
                GridViewDataBind(gvTempVRAcctRecord, Session("TempVRAcctRecord"))
            End If
            'ElseIf Me.mvRecordConfirmation.ActiveViewIndex = 2 And Me.mvVRAcct.ActiveViewIndex = 1 Then
            If Me.mvRecordConfirmation.ActiveViewIndex = 2 And Me.mvVRAcct.ActiveViewIndex = 1 Then
                If Me.gvSelectedRecord_vr.Rows.Count > 0 Then
                    GridViewDataBind(gvSelectedRecord_vr, Session("RecordSelected"))
                End If
            End If
        End If
    End Sub

    Private Sub ShowClaimRecordGridView(ByVal blnShow As Boolean)
        Me.ibtnConfirmSelection.Visible = blnShow
    End Sub

    Private Sub ShowTempVRAcctGridView(ByVal blnShow As Boolean)
        Me.pnlTempVRAcctRecord.Visible = blnShow
    End Sub

    Private Sub SetVRAcctInfo(ByVal blnReset As Boolean)
        If blnReset Then
            ViewState("lblVRAcctInfo") = "VRAInfo"
            Me.lblVRAcctInfo.Text = Me.GetGlobalResourceObject("Text", "VRAInfo")
            Me.ibtnConfirmRejectTempVRAcct.Visible = False
        Else
            ViewState("lblVRAcctInfo") = "ConfirmRejectVRA"
            Me.lblVRAcctInfo.Text = Me.GetGlobalResourceObject("Text", "ConfirmRejectVRA")
            Me.ibtnConfirmRejectTempVRAcct.Visible = True
        End If
    End Sub

    Private Sub SetClaimInfo(ByVal state As VoidStateType)
        ViewState("VoidStateType") = state
        If state = VoidStateType.INFO Then
            ViewState("lblClaimInfo") = "ClaimInfo"
            Me.lblClaimInfo.Text = Me.GetGlobalResourceObject("Text", "ClaimInfo")
            Me.pnlRejectReason.Visible = False
            Me.ibtnRejectTransaction.Visible = True

            Me.ibtnSaveRejectTransaction.Visible = False
            Me.ibtnConfirmRejectTransaction.Visible = False
            Me.ibtnCancelVoidTransaction.Visible = False
            Me.ibtnTransactionDetailBack.Visible = True

            Me.ibtnManagement.Visible = True               ' CRE11-024-02
            'Me.ibtnTransactionDetailBack.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "BackBtn")
            'Me.ibtnTransactionDetailBack.AlternateText = Me.GetGlobalResourceObject("AlternateText", "BackBtn")
        ElseIf state = VoidStateType.INPUT Then
            ViewState("lblClaimInfo") = "InputVoidClaimReason"
            Me.lblClaimInfo.Text = Me.GetGlobalResourceObject("Text", "InputVoidClaimReason")
            Me.pnlRejectReason.Visible = True
            Me.ibtnRejectTransaction.Visible = False
            Me.ibtnSaveRejectTransaction.Visible = True
            Me.ibtnConfirmRejectTransaction.Visible = False
            Me.txtRejectReason.Visible = True
            Me.lblRejectReason.Visible = False
            Me.ibtnCancelVoidTransaction.Visible = True
            Me.ibtnTransactionDetailBack.Visible = False
            Me.ibtnManagement.Visible = False               ' CRE11-024-02
            'Me.ibtnTransactionDetailBack.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CancelBtn")
            'Me.ibtnTransactionDetailBack.AlternateText = Me.GetGlobalResourceObject("AlternateText", "CancelBtn")
        ElseIf state = VoidStateType.CONFIRM Then
            ViewState("lblClaimInfo") = "ConfirmVoidClaim"
            Me.lblClaimInfo.Text = Me.GetGlobalResourceObject("Text", "ConfirmVoidClaim")
            Me.pnlRejectReason.Visible = True
            Me.ibtnRejectTransaction.Visible = False
            Me.ibtnSaveRejectTransaction.Visible = False
            Me.ibtnConfirmRejectTransaction.Visible = True
            Me.txtRejectReason.Visible = False
            Me.lblRejectReason.Visible = True
            Me.ibtnCancelVoidTransaction.Visible = True
            Me.ibtnTransactionDetailBack.Visible = False
            Me.ibtnManagement.Visible = False               ' CRE11-024-02
            'Me.ibtnTransactionDetailBack.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CancelBtn")
            'Me.ibtnTransactionDetailBack.AlternateText = Me.GetGlobalResourceObject("AlternateText", "CancelBtn")
        End If

    End Sub

    Private Sub AddExternalAccountToDataEntryList()
        'Dim udtDataEntryAccBLL As DataEntryAcctBLL = New DataEntryAcctBLL
        'Dim dtExternalAC As New DataTable()
        '' Add External Upload Account

        'If ddlPractice.SelectedValue.Trim.Equals(String.Empty) Then
        '    dtExternalAC = udtDataEntryAccBLL.getExternalUploadACFromTransaction(udtSP.SPID, Nothing)
        'Else
        '    dtExternalAC = udtDataEntryAccBLL.getExternalUploadACFromTransaction(udtSP.SPID, CInt(ddlPractice.SelectedValue.Trim))
        'End If


        'For Each dtRow As DataRow In dtExternalAC.Rows
        '    'Me.ddlDataEntry.Items.Insert(ddlDataEntry.Items.Count, New ListItem(dtRow("dataentry_by"), dtRow("dataentry_by")))
        '    Me.ddlDataEntry.Items.Add(New ListItem("Via " + dtRow("dataentry_by"), dtRow("dataentry_by")))
        'Next

        'Me.ddlDataEntry.Items.Insert(ddlDataEntry.Items.Count, New ListItem(Me.GetGlobalResourceObject("Text", "ViaExternalSystem"), "Upload"))

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If Not Me.SubPlatform.Equals(EnumHCSPSubPlatform.CN) Then
            If rbConfirmTypeList.SelectedIndex = 0 Then
                Me.ddlDataEntry.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ViaExternalSystem"), "Upload"))
            End If
        End If
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

    End Sub

#End Region

#Region "Pop-up event"

    Public Overrides Sub GridViewHeaderImage_Click(ByVal sender As Object, ByVal e As Common.Component.SortedGridviewHeader.SortedGridviewHeaderModel.GridViewHeaderImageEventArgs)
        Dim intColumn As Integer
        intColumn = e.intColumn

        Dim intPopUpSelection As Integer = 1
        '   1: Scheme Legend
        '   2: Subsidy Legend
        '   3: Document Type Legend 

        If Me.mvRecordConfirmation.ActiveViewIndex = 1 Then
            Select Case Me.mvClaimTrans.ActiveViewIndex
                Case 0
                    If intColumn = 2 Then
                        intPopUpSelection = 1
                    ElseIf intColumn = 4 Then
                        intPopUpSelection = 3
                    ElseIf intColumn = 9 Then
                        intPopUpSelection = 2
                    End If
                Case 1
                    If intColumn = 1 Then
                        intPopUpSelection = 1
                    ElseIf intColumn = 3 Then
                        intPopUpSelection = 3
                    ElseIf intColumn = 8 Then
                        intPopUpSelection = 2
                    End If
                Case Else
                    If intColumn = 3 Then
                        intPopUpSelection = 1
                    ElseIf intColumn = 4 Then
                        intPopUpSelection = 3
                    End If
            End Select

        Else
            Select Case Me.mvVRAcct.ActiveViewIndex
                Case 0
                    If intColumn = 2 Then
                        intPopUpSelection = 1
                    ElseIf intColumn = 3 Or intColumn = 4 Then
                        intPopUpSelection = 3
                    End If
                Case 1
                    If intColumn = 0 Then
                        intPopUpSelection = 1
                    ElseIf intColumn = 1 Or intColumn = 2 Then
                        intPopUpSelection = 3
                    End If
                Case Else
                    If intColumn = 0 Then
                        intPopUpSelection = 1
                    ElseIf intColumn = 1 Or intColumn = 2 Then
                        intPopUpSelection = 3
                    End If
            End Select
        End If

        Select Case intPopUpSelection
            Case 1
                popupSchemeNameHelp.Show()
                udcSchemeLegend.ShowFilteredSubsidy = True
                udcSchemeLegend.ShowSubsidy = False
                udcSchemeLegend.SchemeLegendSubPlatform = Me.SubPlatform
                udcSchemeLegend.BindSchemeClaim(Session("language"))

            Case 2
                popupSchemeNameHelp.Show()
                udcSchemeLegend.ShowFilteredSubsidy = True
                udcSchemeLegend.ShowScheme = False
                udcSchemeLegend.SchemeLegendSubPlatform = Me.SubPlatform
                udcSchemeLegend.BindSchemeClaim(Session("language"))

            Case 3
                popupDocTypeHelp.Show()
                udcDocTypeLegend.DocTypeLegendSubPlatform = Me.SubPlatform
                udcDocTypeLegend.BindDocType(Session("language"))

        End Select

    End Sub

    Protected Sub ibtnCloseDocTypeHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseDocTypeHelp.Click
        popupDocTypeHelp.Hide()
    End Sub

    Protected Sub ibtnCloseSchemeNameHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseSchemeNameHelp.Click
        popupSchemeNameHelp.Hide()
    End Sub

    Public Sub udcClaimTranEnquiry_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcSchemeLegend.ShowScheme = False
        udcSchemeLegend.BindSchemeClaim(Session("language"), Me.SubPlatform)
        popupSchemeNameHelp.Show()
    End Sub

#End Region

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Dim udtEHSTransaction As EHSTransactionModel = udtSessionHandler.EHSTransactionGetFromSession(strFuncCode)
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
        Return udtSessionHandler.EHSTransactionGetFromSession(strFuncCode)
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

        udtSessionHandler.EHSTransactionRemoveFromSession(strFuncCode)
    End Sub

    Public Const SEARCH_PARAM_INCLUDE_INCOMPLETE_CLAIMS As String = "INCLUDE_INCOMPLETE_CLAIMS"
    Public Const SEARCH_PARAM_CUT_OFF_DATE As String = "CUT_OFF_DATE"
    Public Const SEARCH_PARAM_PRACTICE As String = "PRACTICE"
    Public Const SEARCH_PARAM_DATA_ENTRY_ACCOUNT As String = "DATA_ENTRY_ACCOUNT"
    Public Const SEARCH_PARAM_SCHEME As String = "SCHEME"

    Public Shared Function BuildSearchCriteria(ByVal strIncludeIncompleteClaims As Boolean, ByVal strCutOffDate As String, ByVal strPractice As String, ByVal strDataEntryAccount As String, ByVal strScheme_Code As String) As RedirectParameter.SearchCriteriaCollection
        Dim clln As New RedirectParameter.SearchCriteriaCollection
        clln.Add(SEARCH_PARAM_INCLUDE_INCOMPLETE_CLAIMS, strIncludeIncompleteClaims)
        clln.Add(SEARCH_PARAM_CUT_OFF_DATE, strCutOffDate)
        clln.Add(SEARCH_PARAM_PRACTICE, strPractice)
        clln.Add(SEARCH_PARAM_DATA_ENTRY_ACCOUNT, strDataEntryAccount)
        clln.Add(SEARCH_PARAM_SCHEME, strScheme_Code)

        Return clln
    End Function

    Private Sub chkIncludeIncompleteClaim_vr_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIncludeIncompleteClaim_vr.CheckedChanged
        Me.chkIncludeIncompleteClaim.Checked = Me.chkIncludeIncompleteClaim_vr.Checked
        Me.SearchTranscationRecord(True)
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Popup legend again if legend data changed
    Private Sub udcSchemeLegend_DataChanged() Handles udcSchemeLegend.DataChanged
        popupSchemeNameHelp.Show()
    End Sub

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

    Private Sub calExtCutOffDate_Load(sender As Object, e As EventArgs) Handles calExtCutOffDate.Load
        Dim selectedLang As String
        Dim chineseTodayDateFormat As String
        Dim udtSubPlatformBLL As New SubPlatformBLL

        selectedLang = LCase(Session("language"))
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Select Case selectedLang
        '    Case English
        '        Me.calExtCutOffDate.TodaysDateFormat = "d MMMM, yyyy"
        '        Me.calExtCutOffDate.DaysModeTitleFormat = "MMMM, yyyy"
        '    Case TradChinese, SimpChinese
        '        chineseTodayDateFormat = CStr(Today.Year) + "年" + CStr(Today.Month) + "月" + CStr(Today.Day) & "日"
        '        Me.calExtCutOffDate.TodaysDateFormat = chineseTodayDateFormat
        '        Me.calExtCutOffDate.DaysModeTitleFormat = "MMMM, yyyy"
        '    Case Else
        '        Me.calExtCutOffDate.TodaysDateFormat = "dd-MM-yyyy"
        '        Me.calExtCutOffDate.DaysModeTitleFormat = "MMMM, yyyy"
        'End Select
        Select Case selectedLang
            Case English
                Me.calExtCutOffDate.TodaysDateFormat = "d MMMM, yyyy"
                Me.calExtCutOffDate.DaysModeTitleFormat = "MMMM, yyyy"
                Me.calExtCutOffDate.Format = udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            Case TradChinese
                chineseTodayDateFormat = CStr(Today.Year) + "年" + CStr(Today.Month) + "月" + CStr(Today.Day) & "日"
                Me.calExtCutOffDate.TodaysDateFormat = chineseTodayDateFormat
                Me.calExtCutOffDate.DaysModeTitleFormat = "MMMM, yyyy"
                Me.calExtCutOffDate.Format = udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            Case SimpChinese
                chineseTodayDateFormat = CStr(Today.Year) + "年" + CStr(Today.Month) + "月" + CStr(Today.Day) & "日"
                Me.calExtCutOffDate.TodaysDateFormat = chineseTodayDateFormat
                Me.calExtCutOffDate.DaysModeTitleFormat = "MMMM, yyyy"
                Me.calExtCutOffDate.Format = udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            Case Else
                Me.calExtCutOffDate.TodaysDateFormat = "dd-MM-yyyy"
                Me.calExtCutOffDate.DaysModeTitleFormat = "MMMM, yyyy"
                Me.calExtCutOffDate.Format = udtFormatter.EnterDateFormat(udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
        End Select
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

    End Sub

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

