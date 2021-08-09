Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.Component.VoucherRefund
Imports Common.Validation
Imports Common.Component.HCVUUser
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Component.DocType
Imports Common.Component.RedirectParameter
Imports Common.Component.Scheme
Imports Common.Component.SortedGridviewHeader
Imports Common.Format
Imports HCVU.BLL
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.VoucherInfo
Imports Common.Component.EHSTransaction
Imports Common.Component.StaticData

Partial Public Class eHSAccountEnquiryCallCentre
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
    'Inherits System.Web.UI.Page
    'Inherits BasePageWithGridView
    Inherits BasePageWithControl
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    Dim udtAuditLogEntry As AuditLogEntry
    Dim udtSM As Common.ComObject.SystemMessage
    Dim udtvalidator As Validator = New Validator
    Dim udtformatter As Common.Format.Formatter = New Common.Format.Formatter
    Dim udtCommonFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
    Dim udteHSAccountMaintBLL As eHSAccountMaintBLL = New eHSAccountMaintBLL
    Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL
    Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL

    Dim udtEHSAccount As EHSAccountModel
    Dim udtEHSAccount_Amendment As EHSAccountModel

    Public Const SESSION_REDIRECT_PARAMETER As String = "PageRedirectorBLL.Parameter"
    Public Const SESSION_REDIRECT_SOURCE As String = "PageRedirectorBLL.Source"
    Public Const REDIRECT_NAME As String = "eHSAccountEnquiryCallCentre"

    <Serializable()> _
    Public Class RedirectParamAccountInfo
        Public AccID As String
        Public DocType As String
        Public AccSrc As String
    End Class

#Region "Audit Log Description"
    Public Class AuditLogDesc

        Public Const eHSAccountEnquiryPageLoad_ID = LogID.LOG00000
        Public Const eHSAccountEnquiryPageLoad = "eHealth Account Enquiry (CallCentre) Loaded"

        ' Search
        Public Const SearchByParticulars_ID As String = LogID.LOG00001
        Public Const SearchByParticulars = "Search By Particulars"

        Public Const SearchByParticularsSuccess_ID = LogID.LOG00002
        Public Const SearchByParticularsSuccess = "Search By Particulars Success"

        Public Const SearchByParticularsFail_ID = LogID.LOG00003
        Public Const SearchByParticularsFail = "Search By Particulars Fail"

        Public Const SearchFail_ID = LogID.LOG00004
        Public Const SearchFail = "Search Fail"

        Public Const SelectEHSAccount_ID As String = LogID.LOG00005
        Public Const SelectEHSAccount As String = "Select and view eHealth Account"

        Public Const SelectEHSAccountSuccess_ID As String = LogID.LOG00006
        Public Const SelectEHSAccountSuccess As String = "Select and view eHealth Account Success"

        Public Const SelectEHSAccountFail_ID As String = LogID.LOG00007
        Public Const SelectEHSAccountFail As String = "Select and view eHealth Account Fail"

        'Scheme Info
        Public Const GetSchemeInfo_ID As String = LogID.LOG00008
        Public Const GetSchemeInfo As String = "Get scheme info"

        Public Const GetSchemeInfoSuccess_ID As String = LogID.LOG00009
        Public Const GetSchemeInfoSuccess As String = "Get scheme info Success"

        Public Const GetSchemeInfoFail_ID As String = LogID.LOG00010
        Public Const GetSchemeInfoFail As String = "Get scheme info Fail"

        ' Back
        Public Const BackToSearch_ID As String = LogID.LOG00011
        Public Const BackToSearch As String = "Back To Search"

        Public Const BackToResultList_ID As String = LogID.LOG00012
        Public Const BackToResultList As String = "Back To Search Result List"

        ' Mask Doc No.
        Public Const MaskIdentityDocumentNoClick_ID As String = LogID.LOG00013
        Public Const MaskIdentityDocumentNoClick As String = "Search Result - Mask Identity Document No. click"

        Public Const MaskIdentityDocumentNoSuccess_ID As String = LogID.LOG00014
        Public Const MaskIdentityDocumentNoSuccess As String = "Search Result - Unmask Identity Document No. success"

    End Class

#End Region

#Region "Constant Value"
    Private Const intSearchView As Integer = 0
    Private Const intSearchResult As Integer = 1
    Private Const intAccountDetails As Integer = 2

    Private Const SESS_Language As String = "language"
    Private Const SESS_Result As String = "010309_SearchResult"
    Private Const SESS_ActionMode As String = "010309_ActionMode"
    Private Const SESS_ServiceProvider As String = "010309_ServiceProviderModel"
    Private Const SESS_AccountCreateBy As String = "010309_AccountCreateBy"
    Private Const SESS_VoucherInfo As String = "010309_VoucherInfo"
    Private Const SESS_SchemeClaim As String = "010309_SchemeClaim"
    Private Const SESS_VoucherTransHistory As String = "010309_VoucherTransHistory"

    Private Const FuncCode As String = FunctCode.FUNT010309
    Private Const CommonFunctionCode As String = Common.Component.FunctCode.FUNT990000

    ' CRE11-007
    Private Const EHAccountIDSeparator As String = ","
#End Region

#Region "Private Class"

    Private Class AccountTypeClass
        Public Const Validated As String = "V"
        Public Const Temporary As String = "T"
    End Class

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <remarks></remarks>
    Private Class VS
        Public Const UnmaskPopup As String = "010301_UnmaskPopup"
    End Class

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <remarks></remarks>
    Private Class PopupStatus
        Public Const Active As String = "A"
        Public Const Closed As String = "C"
    End Class
#End Region

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
#Region "SF Search"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
        Dim blnReturn As Boolean = True
        
        Select Case Me.tcSearchRoute.ActiveTabIndex
            Case 0
                Dim strExactDOB As String = String.Empty
                Dim dtDOB As Nullable(Of DateTime) = Nothing
                Dim strDocCode As String = String.Empty
                Dim strAdoptionPrefixNum As String = String.Empty
                Dim strIdentityNum As String = String.Empty
                Dim streHSAccountID As String = String.Empty
                Dim arreHSAccountID() As String = Nothing
                Dim strRefNo As String = String.Empty

                'Start validation

                ' CRE20-001 (Call centre search name) [Start][Winnie]    
                ' ------------------------------------------------------------------------
                Dim blnTextFieldInputted As Boolean = False

                blnTextFieldInputted = Me.txtSearchIdentityNum.Text.Trim <> String.Empty _
                                        OrElse Me.txtSearchEName.Text.Trim <> String.Empty _
                                        OrElse Me.txtSearchCName.Text.Trim <> String.Empty

                If Not blnTextFieldInputted Then
                    udtSM = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00257)
                    Me.udcMsgBox.AddMessage(udtSM)
                    imgSearchIdentityNumError.Visible = True
                    imgENameError.Visible = True
                    imgCNameError.Visible = True

                    udtAuditLogEntry.AddDescripton("IdentityNumber", Me.txtSearchIdentityNum.Text)
                    udtAuditLogEntry.AddDescripton("EngName", Me.txtSearchEName.Text.Trim)
                    udtAuditLogEntry.AddDescripton("ChiName", Me.txtSearchCName.Text.Trim)                    
                End If
                ' CRE20-001 (Call centre search name) [End][Winnie]  

                'Doc Type
                Me.lblAcctListDocType.Text = Me.ddlSearchDocType.SelectedItem.Text.Trim
                strDocCode = Me.ddlSearchDocType.SelectedValue.Trim

                'Identity Num
                If Me.txtSearchIdentityNum.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListIdentityNum.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListIdentityNum.Text = Me.txtSearchIdentityNum.Text.Trim.ToUpper

                    Dim strIdentityNumFullTemp As String
                    strIdentityNumFullTemp = Me.txtSearchIdentityNum.Text.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")

                    Dim strIdentityNumFull() As String
                    strIdentityNumFull = strIdentityNumFullTemp.Trim.Split("/")
                    If strIdentityNumFull.Length > 1 Then
                        strIdentityNum = strIdentityNumFull(1)
                        strAdoptionPrefixNum = strIdentityNumFull(0)
                    Else
                        strIdentityNum = strIdentityNumFullTemp
                    End If
                End If

                'English Name
                If Me.txtSearchEName.Text.Equals(String.Empty) Then
                    Me.lblAcctListEName.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListEName.Text = Me.txtSearchEName.Text.Trim
                End If

                ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                'Chinese Name
                If Me.txtSearchCName.Text.Equals(String.Empty) Then
                    Me.lblAcctListCName.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListCName.Text = Me.txtSearchCName.Text.Trim
                End If
                ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

                'DOB
                If Me.txtSearchDOB.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListDOB.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Dim dtDOBValue As DateTime
                    'DOB passed to "chkDOB" must be of DateTime instead of Nullable(of DateTime) 
                    udtSM = Me.udtvalidator.chkDOB(strDocCode, Me.txtSearchDOB.Text.Trim, dtDOBValue, strExactDOB)
                    If Not IsNothing(udtSM) Then
                        Me.imgDOBError.Visible = True
                        Me.udcMsgBox.AddMessage(udtSM)
                    Else
                        dtDOB = dtDOBValue
                        ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [Start][Koala]
                        ' -----------------------------------------------------------------------------------------------------------------------------
                        Me.lblAcctListDOB.Text = udtformatter.formatDOB(dtDOB, strExactDOB, Session(SESS_Language), Nothing, Nothing)
                        ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [End][Koala]
                    End If
                End If

                If Me.udcMsgBox.GetCodeTable.Rows.Count = 0 Then
                    blnReturn = True
                Else
                    Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, AuditLogDesc.SearchByParticularsFail_ID, AuditLogDesc.SearchByParticularsFail)
                    blnReturn = False
                End If

        End Select

        Return blnReturn
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult
        Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult

        bllSearchResult = Nothing

        Select Case Me.tcSearchRoute.ActiveTabIndex
            Case 0

                Dim strExactDOB As String = String.Empty
                Dim dtDOB As Nullable(Of DateTime) = Nothing
                Dim strDocCode As String = String.Empty
                Dim strAdoptionPrefixNum As String = String.Empty
                Dim strIdentityNum As String = String.Empty
                Dim strGender As String = String.Empty

                ' Field not provided in Enquiry CallCentre function
                Dim streHSAccountID As String = String.Empty
                Dim arreHSAccountID() As String = Nothing
                Dim strRefNo As String = String.Empty
                Dim strAccountType As String = String.Empty
                Dim strAccountStatus As String = String.Empty
                Dim dtmCreationDateFrom As Nullable(Of DateTime) = Nothing
                Dim dtmCreationDateTo As Nullable(Of DateTime) = Nothing

                'Doc Type
                Me.lblAcctListDocType.Text = Me.ddlSearchDocType.SelectedItem.Text.Trim

                If Me.ddlSearchDocType.SelectedValue.Trim = String.Empty Then
                    Dim lstDocType As New List(Of String)

                    For Each lstItem As ListItem In ddlSearchDocType.Items
                        If lstItem.Value <> String.Empty Then
                            lstDocType.Add(lstItem.Value.Trim)
                        End If
                    Next

                    strDocCode = String.Join(",", lstDocType.ToArray)
                Else
                    strDocCode = Me.ddlSearchDocType.SelectedValue.Trim
                End If

                'Identity Num
                If Me.txtSearchIdentityNum.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListIdentityNum.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListIdentityNum.Text = Me.txtSearchIdentityNum.Text.Trim.ToUpper

                    Dim strIdentityNumFullTemp As String
                    strIdentityNumFullTemp = Me.txtSearchIdentityNum.Text.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")

                    Dim strIdentityNumFull() As String
                    strIdentityNumFull = strIdentityNumFullTemp.Trim.Split("/")
                    If strIdentityNumFull.Length > 1 Then
                        strIdentityNum = strIdentityNumFull(1)
                        strAdoptionPrefixNum = strIdentityNumFull(0)
                    Else
                        strIdentityNum = strIdentityNumFullTemp
                    End If
                End If

                'English Name
                If Me.txtSearchEName.Text.Equals(String.Empty) Then
                    Me.lblAcctListEName.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListEName.Text = Me.txtSearchEName.Text.Trim
                End If

                ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                'Chinese Name
                If Me.txtSearchCName.Text.Equals(String.Empty) Then
                    Me.lblAcctListCName.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListCName.Text = Me.txtSearchCName.Text.Trim
                End If
                ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

                'DOB
                If Me.txtSearchDOB.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListDOB.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Dim dtDOBValue As DateTime
                    'DOB passed to "chkDOB" must be of DateTime instead of Nullable(of DateTime) 
                    udtSM = Me.udtvalidator.chkDOB(strDocCode, Me.txtSearchDOB.Text.Trim, dtDOBValue, strExactDOB)
                    If Not IsNothing(udtSM) Then
                        Me.imgDOBError.Visible = True
                        Me.udcMsgBox.AddMessage(udtSM)
                    Else
                        dtDOB = dtDOBValue
                        ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [Start][Koala]
                        ' -----------------------------------------------------------------------------------------------------------------------------
                        Me.lblAcctListDOB.Text = udtformatter.formatDOB(dtDOB, strExactDOB, Session(SESS_Language), Nothing, Nothing)
                        ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [End][Koala]
                    End If
                End If

                ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                ' ------------------------------------------------------------------------
                'Gender
                If rbSearchGender.SelectedValue.Equals(String.Empty) Then
                    Me.lblAcctListGender.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListGender.Text = Me.rbSearchGender.SelectedItem.Text.Trim
                    strGender = rbSearchGender.SelectedValue
                End If

                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListByParticularMultiple(Me.FunctionCode, strDocCode, strIdentityNum, strAdoptionPrefixNum, Me.txtSearchEName.Text.Trim, Me.txtSearchCName.Text.Trim, dtDOB, _
                                                                arreHSAccountID, strRefNo, strGender, _
                                                                strAccountType, strAccountStatus, dtmCreationDateFrom, dtmCreationDateTo, _
                                                                blnOverrideResultLimit, Me.txtSearchIdentityNum.Text.Trim)
                ' CRE19-026 (HCVS hotline service) [End][Winnie]


        End Select


        Return bllSearchResult
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal udtBLLSearchResult As Common.Component.BaseBLL.BLLSearchResult) As Integer
        Dim dt As DataTable
        Dim intRowCount As Integer
        Dim blnShowResultList As Boolean = False

        Try
            dt = CType(udtBLLSearchResult.Data, DataTable)
        Catch ex As Exception
            Throw
        End Try

        intRowCount = dt.Rows.Count

        Select Case dt.Rows.Count
            Case 0
                ' No record found
                blnShowResultList = False

            Case Else
                blnShowResultList = True

        End Select

        If blnShowResultList Then
            Session(SESS_Result) = dt

            Select Case Me.tcSearchRoute.ActiveTabIndex
                Case 0
                    ' Search Route 2
                    Me.GridViewDataBind(Me.gvAcctList, dt, "", "ASC", False)
                    Me.pnlSearchCriteriaRoute2.Visible = True

            End Select

            Me.mveHSAccount.ActiveViewIndex = intSearchResult

        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        Dim udtAuditLogEntry As New AuditLogEntry(FunctionCode, Me)
        Dim enumSearchResult As SearchResultEnum

        Select Case Me.tcSearchRoute.ActiveTabIndex
            Case 0
                'Case 1
                ' Search Route 2
                udtAuditLogEntry.WriteStartLog(AuditLogDesc.SearchByParticulars_ID, AuditLogDesc.SearchByParticulars)

        End Select

        Try
            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox, False, True)

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                udtSM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                udcMsgBox.AddMessage(udtSM)
                If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
                    udcMsgBox.Visible = False
                Else
                    udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, AuditLogDesc.SearchFail_ID, AuditLogDesc.SearchFail)
                End If
            Else
                udtAuditLogEntry.WriteEndLog(AuditLogDesc.SearchFail_ID, AuditLogDesc.SearchFail)
                Throw eSQL
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(AuditLogDesc.SearchFail_ID, AuditLogDesc.SearchFail)
            Throw ex
        End Try

        Select Case Me.tcSearchRoute.ActiveTabIndex
            Case 0
                ' Search Route 2
                Select Case enumSearchResult
                    Case SearchResultEnum.Success
                        udtAuditLogEntry.WriteEndLog(AuditLogDesc.SearchByParticularsSuccess_ID, AuditLogDesc.SearchByParticularsSuccess)

                    Case Else
                        Throw New Exception("Error: Class = [HCVU.eHSAccountMaint], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

                End Select
        End Select

    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub

#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        ' Check session expire
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUserBLL.GetHCVUUser()

        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT010309
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(AuditLogDesc.eHSAccountEnquiryPageLoad_ID, AuditLogDesc.eHSAccountEnquiryPageLoad)

            ResetControls()

        Else
            udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
            udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)
            Me.SetupInputDocControl(udtEHSAccount, udtEHSAccount_Amendment)

            If mveHSAccount.ActiveViewIndex = intAccountDetails Then
                BuildVoucherUtilization()
            End If
        End If

        Me.ibtnAccountDetailsBack.Visible = True

        If Not IsPostBack Then
            Me.hfIsRedirect.Value = False
            If Me.Session(SESSION_REDIRECT_SOURCE) IsNot Nothing AndAlso Me.Session(SESSION_REDIRECT_SOURCE) = REDIRECT_NAME Then
                Me.hfIsRedirect.Value = True
                ShowAccountDetails(CType(Me.Session(SESSION_REDIRECT_PARAMETER), RedirectParamAccountInfo).AccID, _
                                    CType(Me.Session(SESSION_REDIRECT_PARAMETER), RedirectParamAccountInfo).DocType, _
                                    CType(Me.Session(SESSION_REDIRECT_PARAMETER), RedirectParamAccountInfo).AccSrc)
                Me.Session(SESSION_REDIRECT_PARAMETER) = Nothing
                Me.Session(SESSION_REDIRECT_SOURCE) = Nothing
            End If

        End If

        If Me.hfIsRedirect.Value = True Then
            Me.ibtnAccountDetailsBack.Visible = False
        End If


    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' Keep Mask document no. popup show default on status
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case ViewState(VS.UnmaskPopup)
            Case PopupStatus.Active
                popupUnmask.Show()
        End Select
    End Sub

#Region "Support Function"
    Private Sub BindDocumentType(ByVal ddlEHealthDocType As DropDownList)
        Dim udtDocTypeModelList As DocTypeModelCollection = udtDocTypeBLL.getAllDocType()
        Dim udtDocTypeModelListFilter As New DocTypeModelCollection

        ' Display doc type for voucher scheme
        Dim udtSchemeCList As SchemeClaimModelCollection = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup()

        For Each udtSchemeClaim As Scheme.SchemeClaimModel In udtSchemeCList

            If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = Common.Component.Scheme.SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher Then

                For Each udtDocType As DocTypeModel In udtDocTypeModelList
                    ' Load Items from Scheme
                    Dim udtSchemeDocTypeList As SchemeDocTypeModelCollection = udtDocTypeBLL.getSchemeDocTypeByScheme(udtSchemeClaim.SchemeCode)
                    If udtSchemeDocTypeList.FilterDocCode(udtDocType.DocCode).Count = 0 Then Continue For
                    If Not udtDocTypeModelListFilter.Contains(udtDocType) Then udtDocTypeModelListFilter.Add(udtDocType)
                Next

            End If
        Next

        ddlEHealthDocType.Items.Clear()
        ddlEHealthDocType.DataSource = udtDocTypeModelListFilter
        ddlEHealthDocType.DataTextField = "DocName"
        ddlEHealthDocType.DataValueField = "DocCode"
        ddlEHealthDocType.DataBind()

        ddlEHealthDocType.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        ddlEHealthDocType.SelectedIndex = 0
    End Sub

#End Region

#Region "View 1 - Search"

    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Me.udcInfoMsgBox.Visible = False
        Me.udcMsgBox.Visible = False
        ClearErrorImage()

        ' Implement Collapsible Search Criteria Review
        udcCollapsibleSearchCriteriaReview2.Collapsed = True
        udcCollapsibleSearchCriteriaReview2.ClientState = "True"

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------
        Dim enumSearchResult As SearchResultEnum
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

        Try
            Select Case Me.tcSearchRoute.ActiveTabIndex

                Case 0
                    ' Search Route 2
                    udtAuditLogEntry.AddDescripton("EngName", Me.txtSearchEName.Text.Trim)
                    ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                    udtAuditLogEntry.AddDescripton("ChiName", Me.txtSearchCName.Text.Trim)
                    ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

                    udtAuditLogEntry.AddDescripton("DocType", Me.ddlSearchDocType.SelectedValue)
                    udtAuditLogEntry.AddDescripton("IdentityNumber", Me.txtSearchIdentityNum.Text)
                    udtAuditLogEntry.AddDescripton("DOB", Me.txtSearchDOB.Text)
                    udtAuditLogEntry.AddDescripton("Gender", Me.rbSearchGender.SelectedValue)

                    udtAuditLogEntry.WriteStartLog(AuditLogDesc.SearchByParticulars_ID, AuditLogDesc.SearchByParticulars)

                    If sender Is Nothing Then
                        enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox, False, True)
                    Else
                        enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox)
                    End If

                    Select Case enumSearchResult
                        Case SearchResultEnum.Success
                            udtAuditLogEntry.WriteEndLog(AuditLogDesc.SearchByParticularsSuccess_ID, AuditLogDesc.SearchByParticularsSuccess)

                        Case SearchResultEnum.ValidationFail
                            ' Audit Log has been handled in [SF_ValidateSearch] method

                        Case SearchResultEnum.NoRecordFound
                            udtAuditLogEntry.WriteEndLog(AuditLogDesc.SearchByParticularsSuccess_ID, AuditLogDesc.SearchByParticularsSuccess)

                        Case SearchResultEnum.OverResultList1stLimit_PopUp
                            udtAuditLogEntry.WriteEndLog(AuditLogDesc.SearchByParticularsFail_ID, AuditLogDesc.SearchByParticularsFail)

                        Case SearchResultEnum.OverResultList1stLimit_Alert
                            udtAuditLogEntry.WriteEndLog(AuditLogDesc.SearchByParticularsFail_ID, AuditLogDesc.SearchByParticularsFail)

                        Case SearchResultEnum.OverResultListOverrideLimit
                            udtAuditLogEntry.WriteEndLog(AuditLogDesc.SearchByParticularsFail_ID, AuditLogDesc.SearchByParticularsFail)

                        Case Else
                            Throw New Exception("Error: Class = [HCVU.eHSAccountEnquiry], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

                    End Select

            End Select

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                udtSM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                udcMsgBox.AddMessage(udtSM)
                If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
                    udcMsgBox.Visible = False
                Else
                    udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, AuditLogDesc.SearchFail_ID, AuditLogDesc.SearchFail)
                End If
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "View 2 - Search Result"

#Region "Gridview Function - gvAcctList"

    Private Sub gvAcctList_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAcctList.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_Result)
    End Sub

    Private Sub gvAcctList_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAcctList.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_Result)
    End Sub

    Private Sub gvAcctList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAcctList.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            txtDocCode.Text = String.Empty

            Dim strDocCode As String = String.Empty
            Dim strAccountID As String = String.Empty
            Dim strAccountSource As String = String.Empty
            Dim strPersonalInformationStatus As String = String.Empty
            Dim strIdentityNum As String = String.Empty
            Dim strSPID As String = String.Empty

            Dim blnShowAmendmentRecord As Boolean = False


            Dim strCommandArgument As String

            strCommandArgument = e.CommandArgument.ToString.Trim
            strAccountID = strCommandArgument.Split("|")(0).Trim
            strDocCode = strCommandArgument.Split("|")(1).Trim
            strAccountSource = strCommandArgument.Split("|")(2).Trim
            strPersonalInformationStatus = strCommandArgument.Split("|")(3).Trim
            strIdentityNum = strCommandArgument.Split("|")(4).Trim
            strSPID = strCommandArgument.Split("|")(5).Trim()

            'Audit Log
            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
            Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
            Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
            Me.udtAuditLogEntry.AddDescripton("PersonalInformationStatus", strPersonalInformationStatus)
            Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
            Dim udtAuditLogInfo As AuditLogInfo
            udtAuditLogInfo = New AuditLogInfo(strSPID, Nothing, strAccountSource, _
                                            strAccountID, strDocCode, Me.udtformatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum))
            Me.udtAuditLogEntry.WriteStartLog(AuditLogDesc.SelectEHSAccount_ID, AuditLogDesc.SelectEHSAccount, udtAuditLogInfo)

            txtDocCode.Text = strDocCode

            If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.ValidateAccount) Then
                If strPersonalInformationStatus.Trim.Equals(EHSAccountModel.PersonalInformationRecordStatusClass.UnderAmendment) Then
                    blnShowAmendmentRecord = True
                End If
            End If
            If Me.GetEHSAccount(strAccountID, strAccountSource, blnShowAmendmentRecord) Then
                udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
                Me.udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

                If blnShowAmendmentRecord Then
                    Session(SESS_ActionMode) = ActionModel.ReadOnly_N_Amending
                Else
                    Session(SESS_ActionMode) = ActionModel.ReadOnly
                End If

                Me.mveHSAccount.ActiveViewIndex = intAccountDetails

                ibtnAccountDetailsBack.Visible = True

                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
                Me.udtAuditLogEntry.WriteEndLog(AuditLogDesc.SelectEHSAccountSuccess_ID, AuditLogDesc.SelectEHSAccountSuccess)
            Else
                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
                udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, AuditLogDesc.SelectEHSAccountFail_ID, AuditLogDesc.SelectEHSAccountFail)
            End If
        End If
    End Sub

    Private Sub gvAcctList_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctList.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(3, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(4, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvAcctList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
            Dim lbtnAccountID As LinkButton
            Dim lblIdentityNum As Label
            Dim lblIdentityNumUnmask As Label
            Dim lblCreateDtm As Label
            Dim lblName As Label
            Dim lblCName As Label
            Dim lblDOB As Label
            Dim lblSex As Label
            Dim lblAccountType As Label
            Dim lblAccountStatus As Label
            Dim lblEnquiryStatus As Label
            'Dim lblDateOfIssue As Label
            Dim lblAmendmentStatus As Label
            Dim lblCreate_By As Label
            Dim lblCreate_By_DH As Label

            lbtnAccountID = CType(e.Row.FindControl("lbtnAccountID"), LinkButton) ' CRE11-007
            lblIdentityNum = CType(e.Row.FindControl("lblIdentityNum"), Label) ' CRE11-007
            lblIdentityNumUnmask = CType(e.Row.FindControl("lblIdentityNumUnmask"), Label) ' CRE11-007
            lblCreateDtm = CType(e.Row.FindControl("lblCreateDtm"), Label)
            lblName = CType(e.Row.FindControl("lblName"), Label)
            lblCName = CType(e.Row.FindControl("lblCName"), Label)
            lblDOB = CType(e.Row.FindControl("lblDOB"), Label)
            lblSex = CType(e.Row.FindControl("lblSex"), Label)
            'lblDateOfIssue = CType(e.Row.FindControl("lblDateOfIssue"), Label)
            lblAccountType = CType(e.Row.FindControl("lblAccountType"), Label)
            lblAccountStatus = CType(e.Row.FindControl("lblAccountStatus"), Label)
            lblEnquiryStatus = CType(e.Row.FindControl("lblEnquiryStatus"), Label)
            lblAmendmentStatus = CType(e.Row.FindControl("lblAmendmentStatus"), Label)
            lblCreate_By = CType(e.Row.FindControl("lblCreate_By"), Label)
            lblCreate_By_DH = CType(e.Row.FindControl("lblCreate_By_DH"), Label)

            Dim dtmCreateDtm As DateTime = CType(dr.Item("Create_Dtm"), DateTime)
            Dim strEngName As String = CStr(dr.Item("Eng_Name"))
            Dim strChiName As String = CStr(dr.Item("Chi_Name"))
            Dim strIdentityNum As String = CStr(dr.Item("IdentityNum"))
            Dim strVoucherAcctID As String = CStr(dr.Item("Voucher_Acc_ID"))
            Dim strSchemeCode As String = CStr(dr.Item("Scheme_Code"))
            Dim dtmDOB As DateTime = CType(dr.Item("DOB"), DateTime)
            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB"))
            Dim strSex As String = CStr(dr.Item("Sex"))
            'Dim dtmDateOfIssue As DateTime
            Dim strAccountSource As String = CStr(dr.Item("Source"))
            Dim strAccountStatus As String = CStr(dr.Item("Account_Status"))
            Dim strEnquiryStatus As String = CStr(dr.Item("Public_Enquiry_Status"))
            Dim strAmendmentStatus As String = CStr(dr.Item("PersonalInformation_Status"))
            Dim strSPID As String = CStr(dr.Item("SP_ID"))
            Dim strCreate_by As String = CStr(dr.Item("Create_By"))
            Dim strSPPracticeDisplaySeq As Integer = CInt(dr.Item("SP_Practice_Display_Seq"))
            Dim intAge As Nullable(Of Integer)
            Dim dtDOR As Nullable(Of Date)
            Dim strAcctTypeCode As String = String.Empty
            Dim strAcctStatusCode As String = String.Empty
            Dim strAdoptionPrefixNum As String = CStr(dr.Item("Adoption_Prefix_Num")).Trim
            Dim strDocCode As String = CStr(dr.Item("Doc_Code")).Trim
            Dim strAccountPurpose As String = CStr(dr.Item("Account_Purpose")).Trim
            Dim strOtherInfo As String

            'If IsDBNull(dr.Item("Date_of_Issue")) Then
            '    dtmDateOfIssue = Nothing
            'Else
            '    dtmDateOfIssue = CType(dr.Item("Date_of_Issue"), DateTime)
            'End If

            If IsDBNull(dr.Item("EC_Age")) Then
                intAge = Nothing
            Else
                intAge = CInt(dr.Item("EC_Age"))
            End If

            If IsDBNull(dr.Item("EC_Date_of_Registration")) Then
                dtDOR = Nothing
            Else
                dtDOR = CType(dr.Item("EC_Date_of_Registration"), Date)
            End If

            'If IsDBNull(dr.Item("Create_By_BO")) Then
            '    If strSPID.Trim.Equals(String.Empty) Then
            '        lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
            '        lblCreate_By_DH.Text = "<br>(Created by DH)"
            '    Else
            '        lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
            '    End If
            'Else
            '    If CStr(dr.Item("Create_By_BO")).Trim = "Y" Then
            '        If strSPID.Trim.Equals(String.Empty) Then
            '            lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
            '            lblCreate_By_DH.Text = "<br>(Created by DH)"
            '        Else
            '            lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
            '            lblCreate_By_DH.Text = "<br>(Created by " + CStr(dr.Item("Create_By")).Trim + ")"
            '        End If
            '    Else
            '        lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
            '    End If
            'End If

            If Not IsDBNull(dr.Item("Create_By_BO")) Then
                'has value
                If CStr(dr.Item("Create_By_BO")).Trim = "Y" Then
                    lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
                Else
                    lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
                End If
            Else
                lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
            End If

            If IsDBNull(dr.Item("other_info")) Then
                strOtherInfo = String.Empty
            Else
                strOtherInfo = CStr(dr.Item("other_info"))
            End If

            lblIdentityNum.Text = udtformatter.FormatDocIdentityNoForDisplay(strDocCode, strIdentityNum, True, strAdoptionPrefixNum)
            lblIdentityNumUnmask.Text = udtformatter.FormatDocIdentityNoForDisplay(strDocCode, strIdentityNum, False, strAdoptionPrefixNum)
            lbtnAccountID.CommandArgument = strVoucherAcctID & "|" & strDocCode & "|" & strAccountSource & "|" & strAmendmentStatus & "|" & strIdentityNum & "|" & strSPID

            ' CRE11-007 :
            If strAccountSource.Trim = AccountTypeClass.Validated Then
                lbtnAccountID.Text = udtformatter.formatValidatedEHSAccountNumber(strVoucherAcctID.Trim)
            Else
                lbtnAccountID.Text = udtformatter.formatSystemNumber(strVoucherAcctID.Trim)
            End If


            lblCreateDtm.Text = udtformatter.formatDateTime(dtmCreateDtm)
            lblName.Text = strEngName
            lblCName.Text = udtformatter.formatChineseName(strChiName.Trim)
            lblDOB.Text = udtformatter.formatDOB(strDocCode, dtmDOB, strExactDOB, Session(SESS_Language), intAge, dtDOR, strOtherInfo)
            'lblDateOfIssue.Text = udtformatter.formatDOI(strDocCode, dtmDateOfIssue)
            'lblDateOfIssue.Text = udtformatter.formatDOI_GV(dtmDateOfIssue)
            'If lblDateOfIssue.Text.Trim.Equals(String.Empty) Then
            '    lblDateOfIssue.Text = Me.GetGlobalResourceObject("Text", "N/A")
            'End If
            lblSex.Text = Me.GetGlobalResourceObject("Text", udtformatter.formatGender(strSex))

            If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.ValidateAccount) Then
                If strAmendmentStatus.Trim.Equals(EHSAccountModel.PersonalInformationRecordStatusClass.UnderAmendment) Then
                    lblAmendmentStatus.Text = "Under Modification"
                Else
                    lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If

                Status.GetDescriptionFromDBCode(EHSAccountModel.EnquiryStatusClass.ClassCode, strEnquiryStatus, lblEnquiryStatus.Text, String.Empty)
            Else
                lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblEnquiryStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            Status.GetDescriptionFromDBCode(EHSAccountModel.SysAccountSourceClass.ClassCode, strAccountSource, lblAccountType.Text, String.Empty)

            If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.TemporaryAccount) Then
                If strAccountPurpose.Trim.Equals(EHSAccountModel.AccountPurposeClass.ForAmendmentOld) Then
                    'CRE13-006 HCVS Ceiling [Start][Karl]
                    'lblAccountType.Text = "Erased"
                    lblAccountType.Text = eHealthAccountStatus.Erased_Desc
                    'CRE13-006 HCVS Ceiling [End][Karl]
                End If
            End If

            lblAccountStatus.Text = Me.udteHSAccountMaintBLL.getAcctStatus(strAccountStatus, strAccountSource)
        End If
    End Sub

    Private Sub gvAcctList_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAcctList.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_Result)
    End Sub

#End Region

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.hfIsRedirect.Value = False

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(AuditLogDesc.BackToSearch_ID, AuditLogDesc.BackToSearch)

        'Me.ResetControls()

        Me.mveHSAccount.ActiveViewIndex = intSearchView
    End Sub

#End Region

#Region "View 3 - Account Details"

    Protected Sub ibtnAccountDetailsBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(AuditLogDesc.BackToResultList_ID, AuditLogDesc.BackToResultList)

        Me.mveHSAccount.ActiveViewIndex = intSearchResult
        Me.ucInputDocumentType.Clear()
        udteHSAccountMaintBLL.EHSAccountRemoveFromSession(FunctionCode)
    End Sub

#End Region

#Region "Gridview Function - gvVoucherTransHistory"

    Private Sub gvVoucherTransHistory_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvVoucherTransHistory.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_VoucherTransHistory)
    End Sub

    Private Sub gvVoucherTransHistory_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvVoucherTransHistory.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_VoucherTransHistory)
    End Sub


    Private Sub gvVoucherTransHistory_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvVoucherTransHistory.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_VoucherTransHistory)
    End Sub

    Private Sub gvVoucherTransHistory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvVoucherTransHistory.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
            Dim udtStaticDataBLL As New StaticDataBLL

            ' Service Date
            Dim lblServiceDate As Label = CType(e.Row.FindControl("lblGServiceDate"), Label)
            lblServiceDate.Text = udtformatter.formatDisplayDate(CDate(dr.Item("Service_Receive_Dtm")))

            ' Transaction No.
            Dim lblTransactionNo As Label = CType(e.Row.FindControl("lblGTransactionNo"), Label)
            lblTransactionNo.Text = udtformatter.formatSystemNumber(lblTransactionNo.Text)

            ' Claim Amt
            Dim lblAmount As Label = CType(e.Row.FindControl("lblGAmount"), Label)
            lblAmount.Text = udtformatter.formatMoney(dr.Item("Total_Amount").ToString, False)

            ' Conversion Rate
            Dim lblConversionRate As Label = CType(e.Row.FindControl("lblGConversionRate"), Label)
            If Not IsDBNull(dr.Item("ExchangeRate_Value")) Then
                Dim decExchangeRate As Decimal = 1.0
                decExchangeRate = CDec(dr.Item("ExchangeRate_Value"))
                lblConversionRate.Text = decExchangeRate.ToString("N3")
            Else
                lblConversionRate.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            ' Cancelled            
            Dim lblCancelled As Label = CType(e.Row.FindControl("lblGCancelled"), Label)
            lblCancelled.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("YesNo", dr.Item("Cancelled").ToString.Trim).DataValue

            ' SP Name
            Dim lblSPName As Label = CType(e.Row.FindControl("lblGSPName"), Label)
            If Not IsDBNull(dr.Item("SP_Name_Chi")) Then
                lblSPName.Text += String.Format("<br/>({0})", CStr(dr.Item("SP_Name_Chi")).Trim)
            End If

            ' Practice Name
            Dim lblPracticeName As Label = CType(e.Row.FindControl("lblGPracticeName"), Label)
            If Not IsDBNull(dr.Item("Practice_Name_Chi")) Then
                lblPracticeName.Text += String.Format("<br/>({0})", CStr(dr.Item("Practice_Name_Chi")).Trim)
            End If

            ' Practice Tel No.
            Dim lblPracticeAddress As Label = CType(e.Row.FindControl("lblGPracticeAddress"), Label)
            If Not IsDBNull(dr.Item("Practice_Address_Chi")) Then
                lblPracticeAddress.Text += String.Format("<br/>({0})", CStr(dr.Item("Practice_Address_Chi")).Trim)
            End If

        End If

    End Sub

#End Region

#Region "Muilt View Function"

    Private Sub mveHSAccount_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mveHSAccount.ActiveViewChanged
        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)


        Select Case Me.mveHSAccount.ActiveViewIndex
            Case intSearchView
                Me.udcMsgBox.Clear()
                Me.udcInfoMsgBox.Clear()

                'ResetControls()

                ClearSession(True)

                ClearErrorImage()

            Case intSearchResult
                Me.udcMsgBox.Clear()
                Me.udcInfoMsgBox.Clear()

                ClearSession(False)

                ' CRE11-007
                ' Unmask HKID checkbox
                chkMaskDocumentNo.Checked = True
                chkMaskDocumentNo.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes


                Me.gvAcctList.Columns(3).Visible = True
                Me.gvAcctList.Columns(4).Visible = False

            Case intAccountDetails
                Me.udcInfoMsgBox.Clear()
                SetAccountBtn()
                Me.SetupInputDocControl(udtEHSAccount, udtEHSAccount_Amendment)
                Me.SetupSchemeInformation()

        End Select
    End Sub

#End Region

#Region "Set Controls"

    Private Sub ResetControls()
        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        BindDocumentType(ddlSearchDocType)
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        txtSearchEName.Text = String.Empty
        txtSearchCName.Text = String.Empty

        txtSearchIdentityNum.Text = String.Empty
        txtSearchDOB.Text = String.Empty
        ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        rbSearchGender.SelectedIndex = 0
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

    End Sub

    Private Sub SetAccountBtn()

        Me.pnlAmendingSmartID.Visible = False

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Dim udteHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
        udteHSAccountPersonalInfo = udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

        Select Case udtEHSAccount.AccountSource
            Case EHSAccountModel.SysAccountSource.ValidateAccount

                '==================================================================== Code for SmartID ============================================================================
                If Me.udteHSAccountMaintBLL.IsAmendingBySmartID(udtEHSAccount.VoucherAccID, udteHSAccountPersonalInfo.DocCode) Then
                    Me.pnlAmendingSmartID.Visible = False
                Else
                    Me.pnlAmendingSmartID.Visible = False
                End If
                '==================================================================================================================================================================
            Case Else
                ' Do Nth
        End Select

    End Sub

    Private Sub ClearSession(ByVal blnClearResultListSession As Boolean)

        If blnClearResultListSession Then
            Session(SESS_Result) = Nothing
            Session.Remove(SESS_Result)
        End If

        Session(SESS_ActionMode) = Nothing
        Session.Remove(SESS_ActionMode)

        udteHSAccountMaintBLL.EHSAccountRemoveFromSession(FunctionCode)
        udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)

        txtDocCode.Text = String.Empty

        ' Clear Scheme Information session
        Session(SESS_VoucherInfo) = Nothing
        Session(SESS_SchemeClaim) = Nothing
        Session(SESS_VoucherTransHistory) = Nothing

    End Sub

    Private Sub ClearErrorImage()
        Me.udcMsgBox.Clear()
        Me.udcInfoMsgBox.Clear()

        'Route 2
        Me.imgSearchIdentityNumError.Visible = False
        Me.imgENameError.Visible = False
        Me.imgCNameError.Visible = False
        Me.imgDOBError.Visible = False
        Me.imgSearchGenderError.Visible = False

    End Sub
    ' CRE13-001 EHAPP [Start][Karl]
    ' -----------------------------------------------------------------------------------------
    'Private Function GetGridviewColumn(ByVal dv As DataView) As GridView
    Private Function GetGridviewColumn(ByVal dv As DataView, Optional ByVal blnIncludeDoseColumn As Boolean = True) As GridView
        ' CRE13-001 EHAPP [End][Karl]
        Dim gv As New GridView()

        gv.AutoGenerateColumns = False

        gv.Width = 380

        'Dim bField1 As New BoundField
        'bField1.DataField = "Scheme_Code"
        'bField1.HeaderText = Me.GetGlobalResourceObject("Text", "Scheme")
        'gv.Columns.Add(bField1)

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Use new Display_Code_For_Claim (Sync with new code in Claim function
        Dim bField2 As New BoundField
        bField2.DataField = "Display_Code_For_Claim"
        'bField2.DataField = "Subsidize_Item_Code"
        bField2.HeaderText = Me.GetGlobalResourceObject("Text", "Vaccine")
        gv.Columns.Add(bField2)
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        Dim bField3 As New BoundField
        bField3.DataField = "Service_Receive_Dtm"
        bField3.HeaderText = Me.GetGlobalResourceObject("Text", "ServiceDate")
        gv.Columns.Add(bField3)

        ' CRE13-001 EHAPP [Start][Karl]
        ' -----------------------------------------------------------------------------------------
        If blnIncludeDoseColumn = True Then
            ' CRE13-001 EHAPP [End][Karl]

            Dim bField4 As New BoundField
            'bField4.DataField = "Available_Item_Code"
            bField4.DataField = "Available_Item_Desc"
            bField4.HeaderText = Me.GetGlobalResourceObject("Text", "Dose")
            gv.Columns.Add(bField4)

            ' CRE13-001 EHAPP [Start][Karl]
            ' -----------------------------------------------------------------------------------------
        End If
        ' CRE13-001 EHAPP [End][Karl]

        'Dim bField5 As New BoundField
        'bField5.DataField = "Source"
        'bField5.HeaderText = "Source"
        'gv.Columns.Add(bField5)

        gv.DataSource = dv
        gv.DataBind()

        Return gv

    End Function


    Private Function GetGridView_VoucherUsage(ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtVoucherInfo As VoucherInfoModel, ByRef strAlertHtmlImg_Tag_Result As String, ByRef strDeadPeriodVoucherAmount As Integer, ByRef udtDeadPeriodVoucherQuota As VoucherQuotaModel,
                                              ByVal blnShowDeceased As Boolean) As GridView

        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL
        Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL
        Dim udtVoucherRefundBLL As New VoucherRefundBLL
        Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL

        Dim strAlertHtmlImg_ImageUrl As String = CStr(Me.GetGlobalResourceObject("ImageUrl", "Override")).Replace("~", "..")
        Dim strAlertHtmlImg_Tag As String = "<img alt='" + Me.GetGlobalResourceObject("AlternateText", "Override") + "' src='" + strAlertHtmlImg_ImageUrl + "' height='12' width='12' style='padding-left: 5px; padding-right: 5px' />"
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
        Dim strDeathHtmlImg_ImageUrl As String = CStr(Me.GetGlobalResourceObject("ImageUrl", "DeathRecordBtn")).Replace("~", "..")
        Dim strDeathHtmlImg_Tag As String = "<img alt='" + Me.GetGlobalResourceObject("AlternateText", "DeathRecord") + "' src='" + strDeathHtmlImg_ImageUrl + "' style='position: relative; top: 2px' />"
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
        ' Construct DataTable
        Dim dtb As New DataTable

        'CRE16-025 (Lowering voucher eligibility age) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim strColName_Period As String = "Period"
        Dim strColName_Age As String = "Age"
        Dim strColName_Given As String = "Given"
        Dim strColName_MaxAccum As String = "MaxAccum"

        Dim strColName_WriteOff As String = "WriteOff"
        Dim strColName_Refund As String = "Refund"
        Dim strColName_UsedHCVS As String = "UsedHCVS"
        Dim strColName_UsedHCVSC As String = "UsedHCVSC"
        Dim strColName_UsedHCVSDHC As String = "UsedHCVSDHC"
        Dim strColName_PeriodEndBal As String = "Period End Balance"
        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim strColName_ROPQouta As String = "ROPQuotaBalance"
        ' CRE19-003 (Opt voucher capping) [End][Winnie]

        Dim dro As DataRow

        Dim intTotalGiven As Integer = 0
        Dim intTotalUsedHCVS As Integer = 0
        Dim intTotalUsedHCVSC As Integer = 0
        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim intTotalUsedHCVSDHC As Integer = 0
        ' CRE19-006 (DHC) [End][Winnie]
        Dim intTotalWriteOff As Integer = 0
        Dim intTotalRefunded As Integer = 0
        Dim intTotalGivenTemp As Integer
        Dim intTotalUsedTempHCVS As Integer
        Dim intTotalUsedTempHCVSC As Integer
        Dim intTotalUsedTempHCVSDHC As Integer
        Dim intTotalWriteOffTemp As Integer
        Dim intTotalRefundedtemp As Integer
        Dim blnIsEligible As Boolean
        Dim blnHasUsedVoucherAfterDead As Boolean
        Dim intLastUsedVoucherAfterDead As Integer
        Dim intSumPeriodEndBal As Integer

        Dim blnIsDead As Boolean
        Dim blnThisPeriodIsDead As Boolean
        Dim intDeadAge As Integer

        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------        
        Dim dtmAvailableQuotaPeriodStart As New DateTime
        ' CRE19-003 (Opt voucher capping) [End][Winnie]

        dtb.Columns.Add(New DataColumn(strColName_Period, GetType(String)))
        dtb.Columns.Add(New DataColumn(strColName_Age, GetType(String)))
        dtb.Columns.Add(New DataColumn(strColName_Given, GetType(String)))
        dtb.Columns.Add(New DataColumn(strColName_MaxAccum, GetType(String)))
        dtb.Columns.Add(New DataColumn(strColName_WriteOff, GetType(String)))
        dtb.Columns.Add(New DataColumn(strColName_Refund, GetType(String)))
        dtb.Columns.Add(New DataColumn(strColName_UsedHCVS, GetType(String)))
        dtb.Columns.Add(New DataColumn(strColName_UsedHCVSC, GetType(String)))
        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        dtb.Columns.Add(New DataColumn(strColName_UsedHCVSDHC, GetType(String)))
        ' CRE19-006 (DHC) [End][Winnie]
        dtb.Columns.Add(New DataColumn(strColName_PeriodEndBal, GetType(String)))
        dtb.Columns.Add(New DataColumn(strColName_ROPQouta, GetType(String)))

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Dim strHCVSCMinClaimDate As String = String.Empty
        Call (New GeneralFunction).getSystemParameter("DateBackClaimMinDate", strHCVSCMinClaimDate, Nothing, SchemeClaimModel.HCVSCHN)
        Dim dtmHCVSCMinClaimDate As DateTime = Convert.ToDateTime(strHCVSCMinClaimDate)

        Dim blnHCVSCHNExist As Boolean = False
        If Not IsNothing((New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(SchemeClaimModel.HCVSCHN)) Then
            blnHCVSCHNExist = True
        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim strHCVSDHCMinClaimDate As String = String.Empty
        Dim dtmHCVSDHCMinClaimDate As DateTime

        Call (New GeneralFunction).getSystemParameter("DateBackClaimMinDate", strHCVSDHCMinClaimDate, Nothing, SchemeClaimModel.HCVSDHC)
        dtmHCVSDHCMinClaimDate = Convert.ToDateTime(strHCVSDHCMinClaimDate)

        Dim blnHCVSDHCExist As Boolean = False
        If Not IsNothing((New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(SchemeClaimModel.HCVSDHC)) Then
            blnHCVSDHCExist = True
        End If
        ' CRE19-006 (DHC) [End][Winnie]

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
        Dim blnRefundExist As Boolean = False

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        'If udtVoucherRefundList_Full.Count > 0 Then
        If udtVoucherInfo.GetTotalRefund > 0 Then
            ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]
            blnRefundExist = True
        End If

        For Each udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList.FilterbyRange(udtSchemeClaim.ClaimPeriodFrom, Now()).OrderBySchemeSeqASC()
            blnIsDead = udtEHSPersonalInfo.IsDeceasedAsAt(EHSAccountModel.EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR, udtSubsidizeGroupClaim.ClaimPeriodFrom)
            If blnIsDead Then
                ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                'If dicTotalUsedVoucherHCVS.ContainsKey(udtSubsidizeGroupClaim.SchemeSeq) OrElse dicTotalUsedVoucherHCVSC.ContainsKey(udtSubsidizeGroupClaim.SchemeSeq) Then
                If udtVoucherInfo.VoucherDetail(udtSubsidizeGroupClaim.SchemeSeq).Used > 0 Then
                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]
                    blnHasUsedVoucherAfterDead = True
                    intLastUsedVoucherAfterDead = udtSubsidizeGroupClaim.SchemeSeq
                End If
            End If
        Next
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

        For Each udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList.FilterbyRange(udtSchemeClaim.ClaimPeriodFrom, Now()).OrderBySchemeSeqASC()
            dro = dtb.NewRow()

            dro(strColName_Period) = udtSubsidizeGroupClaim.ClaimPeriodFrom.ToString("dd MMM yyyy") & " - " & DateAdd(DateInterval.Day, -1, udtSubsidizeGroupClaim.ClaimPeriodTo).ToString("dd MMM yyyy")
            'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim dtmCheckEligible As DateTime = udtSubsidizeGroupClaim.ClaimPeriodTo.AddDays(-1)
            Dim dtmCurrentDate As DateTime = DateTime.Now

            If udtSubsidizeGroupClaim.ClaimPeriodFrom <= dtmCurrentDate And udtSubsidizeGroupClaim.ClaimPeriodTo > dtmCurrentDate Then
                dtmCheckEligible = dtmCurrentDate
            End If

            'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]


            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            blnIsEligible = udtClaimRulesBLL.CheckEligibilityPerSubsidize(udtSubsidizeGroupClaim, udtEHSPersonalInfo, dtmCheckEligible, Nothing, True).IsEligible
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            'Display ¡¥N/A¡¦ for ¡¥Entitled¡¦ and ¡¥Write off¡¦ for period after death

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            'blnIsDead = udtEHSPersonalInfo.DeathRecord.IsDead(udtSubsidizeGroupClaim.ClaimPeriodFrom)
            blnIsDead = udtEHSPersonalInfo.IsDeceasedAsAt(EHSAccountModel.EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR, udtSubsidizeGroupClaim.ClaimPeriodFrom)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
            blnThisPeriodIsDead = udtEHSPersonalInfo.IsDeceasedAsAt(EHSAccountModel.EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR, udtSubsidizeGroupClaim.ClaimPeriodTo)

            ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            Dim udtVoucherDetail As VoucherDetailModel = udtVoucherInfo.VoucherDetail(udtSubsidizeGroupClaim.SchemeSeq)
            ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]

            If Not blnIsDead Then
                ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                ' ------------------------------------------------------------------------
                Dim intAge As Integer = udtSubsidizeGroupClaim.ClaimPeriodFrom.Year - udtEHSPersonalInfo.DOB.Year
                If blnThisPeriodIsDead Then

                    If blnShowDeceased Then
                        dro(strColName_Age) = strDeathHtmlImg_Tag & " " & IIf(intAge >= 0, intAge, Me.GetGlobalResourceObject("Text", "NA"))
                    Else
                        dro(strColName_Age) = IIf(intAge >= 0, intAge, Me.GetGlobalResourceObject("Text", "NA"))
                    End If
                    intDeadAge = intAge

                Else
                    dro(strColName_Age) = IIf(intAge >= 0, intAge, Me.GetGlobalResourceObject("Text", "NA"))
                End If
            Else
                dro(strColName_Age) = IIf(intDeadAge >= 0, intDeadAge, Me.GetGlobalResourceObject("Text", "NA"))
            End If
            ' CRE19-026 (HCVS hotline service) [End][Winnie]

            If Not blnIsDead AndAlso blnIsEligible Then
                ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                'intTotalGivenTemp = CInt(udtSubsidizeGroupClaim.NumSubsidize * dblSubsidizeFee)
                intTotalGivenTemp = udtVoucherDetail.Entitlement
                ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]
                dro(strColName_Given) = udtformatter.formatMoney(intTotalGivenTemp.ToString(), False)
            Else
                intTotalGivenTemp = 0
                dro(strColName_Given) = Me.GetGlobalResourceObject("Text", "NA")
            End If


            ' --- Max. Accumulative ($) ---
            If Not blnIsDead AndAlso blnIsEligible Then

                ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                If udtVoucherDetail.Ceiling.HasValue Then
                    dro(strColName_MaxAccum) = udtformatter.formatMoney(udtVoucherDetail.Ceiling.ToString(), False)
                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]
                Else
                    dro(strColName_MaxAccum) = Me.GetGlobalResourceObject("Text", "NA")
                End If
            Else
                dro(strColName_MaxAccum) = Me.GetGlobalResourceObject("Text", "NA")
            End If


            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
            ' --- Write Off ($) ---

            ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            If Not blnIsDead AndAlso udtVoucherDetail.Ceiling.HasValue AndAlso blnIsEligible Then
                intTotalWriteOffTemp = udtVoucherDetail.WriteOff
                ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]

                dro(strColName_WriteOff) = udtformatter.formatMoney(intTotalWriteOffTemp.ToString(), False)
            Else
                intTotalWriteOffTemp = 0
                dro(strColName_WriteOff) = Me.GetGlobalResourceObject("Text", "NA")
            End If


            ' --- Refund ($) ---
            If blnRefundExist Then

                ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                intTotalRefundedtemp = udtVoucherDetail.Refund
                ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]

                If intTotalRefundedtemp <> 0 Then
                    dro(strColName_Refund) = udtformatter.formatMoney(intTotalRefundedtemp.ToString(), False)
                Else
                    dro(strColName_Refund) = udtformatter.formatMoney(intTotalRefundedtemp.ToString(), False)
                End If
            End If

            ' --- Used ($) (HCVS) ---
            intTotalUsedTempHCVS = udtVoucherDetail.Used(SchemeClaimModel.HCVS)

            If blnIsEligible OrElse intTotalUsedTempHCVS > 0 Then
                If Not blnIsEligible Then
                    strAlertHtmlImg_Tag_Result = strAlertHtmlImg_Tag
                    dro(strColName_UsedHCVS) += strAlertHtmlImg_Tag
                End If

                dro(strColName_UsedHCVS) += udtformatter.formatMoney(intTotalUsedTempHCVS.ToString(), False)
            Else
                intTotalUsedTempHCVS = 0
                If blnIsDead Then
                    dro(strColName_UsedHCVS) = 0
                Else
                    dro(strColName_UsedHCVS) = Me.GetGlobalResourceObject("Text", "NA")
                End If
            End If


            ' --- Used ($) (HCVSC) ---
            If udtSubsidizeGroupClaim.ClaimPeriodTo.AddSeconds(-1) < dtmHCVSCMinClaimDate Then
                intTotalUsedTempHCVSC = 0
                dro(strColName_UsedHCVSC) = Me.GetGlobalResourceObject("Text", "NA")

            Else
                ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                intTotalUsedTempHCVSC = udtVoucherDetail.Used(SchemeClaimModel.HCVSCHN)

                If blnIsEligible OrElse intTotalUsedTempHCVSC > 0 Then
                    If Not blnIsEligible Then
                        strAlertHtmlImg_Tag_Result = strAlertHtmlImg_Tag
                        dro(strColName_UsedHCVSC) += strAlertHtmlImg_Tag
                    End If

                    dro(strColName_UsedHCVSC) += udtformatter.formatMoney(intTotalUsedTempHCVSC.ToString(), False)
                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]
                Else
                    intTotalUsedTempHCVSC = 0
                    If blnIsDead Then
                        dro(strColName_UsedHCVSC) = 0
                    Else
                        dro(strColName_UsedHCVSC) = Me.GetGlobalResourceObject("Text", "NA")
                    End If

                End If

            End If

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' --- Used ($) (HCVSDHC) ---
            If udtSubsidizeGroupClaim.ClaimPeriodTo.AddSeconds(-1) < dtmHCVSDHCMinClaimDate Then
                intTotalUsedTempHCVSDHC = 0
                dro(strColName_UsedHCVSDHC) = Me.GetGlobalResourceObject("Text", "NA")

            Else
                intTotalUsedTempHCVSDHC = udtVoucherDetail.Used(SchemeClaimModel.HCVSDHC)

                If blnIsEligible OrElse intTotalUsedTempHCVSDHC > 0 Then
                    If Not blnIsEligible Then
                        strAlertHtmlImg_Tag_Result = strAlertHtmlImg_Tag
                        dro(strColName_UsedHCVSDHC) += strAlertHtmlImg_Tag
                    End If

                    dro(strColName_UsedHCVSDHC) += udtformatter.formatMoney(intTotalUsedTempHCVSDHC.ToString(), False)

                Else
                    intTotalUsedTempHCVSDHC = 0
                    If blnIsDead Then
                        dro(strColName_UsedHCVSDHC) = 0
                    Else
                        dro(strColName_UsedHCVSDHC) = Me.GetGlobalResourceObject("Text", "NA")
                    End If

                End If

            End If
            ' CRE19-006 (DHC) [End][Winnie]


            ' --- Period End Balance ($) ---

            ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            intSumPeriodEndBal = udtVoucherInfo.GetPeriodEndBalance(udtSubsidizeGroupClaim.SchemeSeq)
            ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]

            If Not blnIsDead AndAlso blnIsEligible Then

                ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If intSumPeriodEndBal >= 0 Then
                    dro(strColName_PeriodEndBal) = udtformatter.formatMoney(intSumPeriodEndBal.ToString(), False)
                Else
                    dro(strColName_PeriodEndBal) = String.Format("{0}" + "<br>" + "<font color=""red"">[{1}]</font>", _
                                                      udtformatter.formatMoney("0", False), _
                                                      udtformatter.formatMoney(intSumPeriodEndBal.ToString(), False))

                    intSumPeriodEndBal = 0
                End If
                ' CRE19-003 (Opt voucher capping) [End][Winnie]

                If blnThisPeriodIsDead Then
                    strDeadPeriodVoucherAmount = intSumPeriodEndBal
                End If
            Else
                dro(strColName_PeriodEndBal) = Me.GetGlobalResourceObject("Text", "NA")
            End If

            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' --- Optometrist Quota Balance ($) ---
            If blnIsEligible AndAlso Not blnIsDead Then

                Dim udtVoucherQuota As VoucherQuotaModel = Nothing
                Dim strProfessionCode_ROP As String = "ROP" ' Show Optom quota only

                If blnThisPeriodIsDead Then
                    ' Find quota up to the claim period end of dead
                    udtVoucherQuota = udtVoucherInfo.GetVoucherQuota(dtmCheckEligible, udtSchemeClaim, udtEHSPersonalInfo, strProfessionCode_ROP, udtSubsidizeGroupClaim.ClaimPeriodTo)
                    udtDeadPeriodVoucherQuota = udtVoucherQuota
                Else
                    udtVoucherQuota = udtVoucherInfo.GetVoucherQuota(dtmCheckEligible, udtSchemeClaim, udtEHSPersonalInfo, strProfessionCode_ROP)
                End If

                If udtVoucherQuota Is Nothing Then
                    dro(strColName_ROPQouta) = Me.GetGlobalResourceObject("Text", "NA")
                Else

                    If dtmAvailableQuotaPeriodStart <> udtVoucherQuota.PeriodStartDtm Then

                        Dim intAvailableQuota As Integer = 0

                        If udtVoucherQuota.AvailableQuota >= 0 Then
                            intAvailableQuota = udtVoucherQuota.AvailableQuota
                            dro(strColName_ROPQouta) = udtformatter.formatMoney(intAvailableQuota.ToString, False)
                        Else
                            dro(strColName_ROPQouta) = String.Format("{0}" + "<br>" + "<font color=""red"">[{1}]</font>", _
                                                              udtformatter.formatMoney(intAvailableQuota.ToString, False), _
                                                              udtformatter.formatMoney(udtVoucherQuota.AvailableQuota.ToString, False))
                        End If


                        dtmAvailableQuotaPeriodStart = udtVoucherQuota.PeriodStartDtm
                    Else
                        ' Same period will be merged
                        dro(strColName_ROPQouta) = String.Empty
                    End If

                End If
            Else
                dro(strColName_ROPQouta) = Me.GetGlobalResourceObject("Text", "NA")
            End If
            ' CRE19-003 (Opt voucher capping) [End][Winnie]

            ' For dead, show period up to dead season or last voucher used season
            If Not blnIsDead Then
                dtb.Rows.Add(dro)
            Else
                If blnHasUsedVoucherAfterDead Then
                    If intLastUsedVoucherAfterDead >= udtSubsidizeGroupClaim.SchemeSeq Then
                        dtb.Rows.Add(dro)
                    End If
                End If
            End If
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]


            'dtb.Rows.Add(dro)
            intTotalGiven += intTotalGivenTemp
            intTotalUsedHCVS += intTotalUsedTempHCVS
            intTotalUsedHCVSC += intTotalUsedTempHCVSC
            intTotalWriteOff += intTotalWriteOffTemp
            intTotalRefunded += intTotalRefundedtemp

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            intTotalUsedHCVSDHC += intTotalUsedTempHCVSDHC
            ' CRE19-006 (DHC) [End][Winnie]
        Next

        ' Construct GridView
        Dim gvw As New GridView
        Dim bfd As BoundField

        gvw.AutoGenerateColumns = False

        bfd = New BoundField()
        bfd.HeaderStyle.Width = 190
        bfd.DataField = strColName_Period
        gvw.Columns.Add(bfd)

        bfd = New BoundField()
        bfd.HtmlEncode = False
        bfd.HeaderStyle.Width = 80
        bfd.ItemStyle.HorizontalAlign = HorizontalAlign.Right
        bfd.DataField = strColName_Age
        gvw.Columns.Add(bfd)

        bfd = New BoundField()
        bfd.HeaderStyle.Width = 75
        bfd.ItemStyle.HorizontalAlign = HorizontalAlign.Right
        bfd.DataField = strColName_Given
        gvw.Columns.Add(bfd)

        bfd = New BoundField()
        bfd.HeaderStyle.Width = 100
        bfd.ItemStyle.HorizontalAlign = HorizontalAlign.Right
        bfd.DataField = strColName_MaxAccum
        gvw.Columns.Add(bfd)

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
        bfd = New BoundField()
        bfd.HeaderStyle.Width = 80
        bfd.ItemStyle.HorizontalAlign = HorizontalAlign.Right
        bfd.DataField = strColName_WriteOff
        gvw.Columns.Add(bfd)

        If blnRefundExist Then
            bfd = New BoundField()
            bfd.HeaderStyle.Width = 75
            bfd.ItemStyle.HorizontalAlign = HorizontalAlign.Right
            bfd.DataField = strColName_Refund
            gvw.Columns.Add(bfd)
        End If

        bfd = New BoundField()
        bfd.HtmlEncode = False
        bfd.HeaderStyle.Width = 80
        bfd.ItemStyle.HorizontalAlign = HorizontalAlign.Right
        bfd.DataField = strColName_UsedHCVS
        gvw.Columns.Add(bfd)

        If blnHCVSCHNExist Then
            bfd = New BoundField()
            bfd.HtmlEncode = False
            bfd.HeaderStyle.Width = 80
            bfd.ItemStyle.HorizontalAlign = HorizontalAlign.Right
            bfd.DataField = strColName_UsedHCVSC
            gvw.Columns.Add(bfd)
        End If

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If blnHCVSDHCExist Then
            bfd = New BoundField()
            bfd.HtmlEncode = False
            bfd.HeaderStyle.Width = 80
            bfd.ItemStyle.HorizontalAlign = HorizontalAlign.Right
            bfd.DataField = strColName_UsedHCVSDHC
            gvw.Columns.Add(bfd)
        End If
        ' CRE19-006 (DHC) [End][Winnie]

        bfd = New BoundField()
        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        bfd.HtmlEncode = False
        ' CRE19-003 (Opt voucher capping) [End][Winnie]
        bfd.HeaderStyle.Width = 80
        bfd.ItemStyle.HorizontalAlign = HorizontalAlign.Right
        bfd.DataField = strColName_PeriodEndBal
        gvw.Columns.Add(bfd)

        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        bfd = New BoundField()
        bfd.HtmlEncode = False
        bfd.HeaderStyle.Width = 120
        bfd.ItemStyle.HorizontalAlign = HorizontalAlign.Right
        bfd.DataField = strColName_ROPQouta
        gvw.Columns.Add(bfd)

        Dim intQuotaColumn As Integer = gvw.Columns.Count
        ' CRE19-003 (Opt voucher capping) [End][Winnie]

        gvw.DataSource = dtb
        gvw.DataBind()
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------        
        GridView_MergeRow(gvw, intQuotaColumn)
        ' CRE19-003 (Opt voucher capping) [End][Winnie]

        '==========================================
        ' Decorate header
        Dim udtSchemeClaimList As SchemeClaimModelCollection = (New SchemeClaimBLL).getAllDistinctSchemeClaim

        ' Hide the original header and add own header
        gvw.HeaderRow.Visible = False

        ' Header row 1
        Dim gvHeaderRow1 As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
        Dim tc1 As TableCell = Nothing

        tc1 = New TableCell
        tc1.Text = Me.GetGlobalResourceObject("Text", "Period")
        tc1.RowSpan = 2
        tc1.Width = 190
        tc1.Style.Add("vertical-align", "middle")
        tc1.Style.Add("border", "1px solid white")
        gvHeaderRow1.Cells.Add(tc1)

        tc1 = New TableCell
        tc1.Text = Me.GetGlobalResourceObject("Text", "AgeInPeriod")
        tc1.RowSpan = 2
        tc1.Width = 80
        tc1.Style.Add("vertical-align", "middle")
        tc1.Style.Add("border", "1px solid white")
        gvHeaderRow1.Cells.Add(tc1)

        tc1 = New TableCell
        tc1.Text = Me.GetGlobalResourceObject("Text", "EntitledSign")
        tc1.RowSpan = 2
        tc1.Width = 75
        tc1.Style.Add("vertical-align", "middle")
        tc1.Style.Add("border", "1px solid white")
        gvHeaderRow1.Cells.Add(tc1)

        tc1 = New TableCell
        tc1.Text = Me.GetGlobalResourceObject("Text", "MaxAccumulativeSign")
        tc1.RowSpan = 2
        tc1.Width = 100
        tc1.Style.Add("vertical-align", "middle")
        tc1.Style.Add("border", "1px solid white")
        gvHeaderRow1.Cells.Add(tc1)

        tc1 = New TableCell
        tc1.Text = Me.GetGlobalResourceObject("Text", "WriteOffSign")
        tc1.RowSpan = 2
        tc1.Width = 80
        tc1.Style.Add("vertical-align", "middle")
        tc1.Style.Add("border", "1px solid white")
        gvHeaderRow1.Cells.Add(tc1)

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
        If blnRefundExist Then
            tc1 = New TableCell
            tc1.Text = Me.GetGlobalResourceObject("Text", "RefundSign")
            tc1.RowSpan = 2
            tc1.Width = 75
            tc1.Style.Add("vertical-align", "middle")
            tc1.Style.Add("border", "1px solid white")
            gvHeaderRow1.Cells.Add(tc1)
        End If

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim intUsedSpan As Integer = 1
        If blnHCVSCHNExist Then intUsedSpan += 1
        If blnHCVSDHCExist Then intUsedSpan += 1

        tc1 = New TableCell
        tc1.Text = Me.GetGlobalResourceObject("Text", "UsedSign")
        tc1.ColumnSpan = intUsedSpan
        tc1.Width = Unit.Pixel(80 * intUsedSpan)
        tc1.Style.Add("vertical-align", "middle")
        tc1.Style.Add("border", "1px solid white")
        gvHeaderRow1.Cells.Add(tc1)
        ' CRE19-006 (DHC) [End][Winnie]

        tc1 = New TableCell
        tc1.Text = Me.GetGlobalResourceObject("Text", "PeriodEndBalanceSign")
        tc1.RowSpan = 2
        tc1.Width = 80
        tc1.Style.Add("vertical-align", "middle")
        tc1.Style.Add("border", "1px solid white")
        gvHeaderRow1.Cells.Add(tc1)
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Show Optom Quota only
        tc1 = New TableCell
        tc1.Text = Me.GetGlobalResourceObject("Text", "ROP") + " " + Me.GetGlobalResourceObject("Text", "QuotaBalanceSign")
        tc1.RowSpan = 2
        tc1.Width = 120
        tc1.Style.Add("vertical-align", "middle")
        tc1.Style.Add("border", "1px solid white")
        gvHeaderRow1.Cells.Add(tc1)
        ' CRE19-003 (Opt voucher capping) [End][Winnie]

        gvHeaderRow1.Style.Add("font-weight", "bold")

        ' Header row 2
        Dim gvHeaderRow2 As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
        Dim tc2 As TableCell = Nothing

        tc2 = New TableCell
        tc2.Text = udtSchemeClaimList.Filter(SchemeClaimModel.HCVS).DisplayCode
        tc2.Width = 80
        tc2.Style.Add("vertical-align", "middle")
        tc2.Style.Add("border", "1px solid white")
        gvHeaderRow2.Cells.Add(tc2)

        If blnHCVSCHNExist Then
            tc2 = New TableCell
            tc2.Text = udtSchemeClaimList.Filter(SchemeClaimModel.HCVSCHN).DisplayCode
            tc2.Width = 80
            tc2.Style.Add("vertical-align", "middle")
            tc2.Style.Add("border", "1px solid white")
            gvHeaderRow2.Cells.Add(tc2)
        End If

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If blnHCVSDHCExist Then
            tc2 = New TableCell
            tc2.Text = udtSchemeClaimList.Filter(SchemeClaimModel.HCVSDHC).DisplayCode
            tc2.Width = 80
            tc2.Style.Add("vertical-align", "middle")
            tc2.Style.Add("border", "1px solid white")
            gvHeaderRow2.Cells.Add(tc2)
        End If
        ' CRE19-006 (DHC) [End][Winnie]

        gvHeaderRow2.Style.Add("font-weight", "bold")

        ' Add the headers
        gvw.Controls(0).Controls.AddAt(0, gvHeaderRow2)
        gvw.Controls(0).Controls.AddAt(0, gvHeaderRow1)

        '==========================================
        'Bottom row: Total 
        blnIsDead = udtEHSPersonalInfo.Deceased

        ' If Not blnIsDead Then

        Dim gvBottomRowTotal As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)
        Dim tcTotal As TableCell = Nothing

        tcTotal = New TableCell
        tcTotal.Text = Me.GetGlobalResourceObject("Text", "Total")
        tcTotal.ColumnSpan = 2
        tcTotal.Width = 270
        tcTotal.Style.Add("text-align", "right")
        gvBottomRowTotal.Cells.Add(tcTotal)

        tcTotal = New TableCell
        tcTotal.Text = udtformatter.formatMoney(intTotalGiven.ToString(), False)
        tcTotal.Width = 75
        tcTotal.Style.Add("text-align", "right")
        gvBottomRowTotal.Cells.Add(tcTotal)

        tcTotal = New TableCell
        tcTotal.Text = ""
        tcTotal.BackColor = Drawing.Color.DarkGray
        tcTotal.Width = 100
        tcTotal.Style.Add("text-align", "right")
        gvBottomRowTotal.Cells.Add(tcTotal)

        tcTotal = New TableCell
        tcTotal.Text = udtformatter.formatMoney(intTotalWriteOff.ToString(), False)
        tcTotal.Width = 80
        tcTotal.Style.Add("text-align", "right")
        gvBottomRowTotal.Cells.Add(tcTotal)

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
        If blnRefundExist Then
            tcTotal = New TableCell
            tcTotal.Text = udtformatter.formatMoney(intTotalRefunded.ToString(), False)
            tcTotal.Width = 75
            tcTotal.Style.Add("text-align", "right")
            gvBottomRowTotal.Cells.Add(tcTotal)
        End If

        tcTotal = New TableCell
        tcTotal.Text = udtformatter.formatMoney(intTotalUsedHCVS.ToString(), False)
        'tcTotal.ColumnSpan = 2
        tcTotal.Style.Add("text-align", "right")
        gvBottomRowTotal.Cells.Add(tcTotal)

        If blnHCVSCHNExist Then
            tcTotal = New TableCell
            tcTotal.Text = udtformatter.formatMoney(intTotalUsedHCVSC.ToString(), False)
            tcTotal.Style.Add("text-align", "right")
            gvBottomRowTotal.Cells.Add(tcTotal)
        End If

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If blnHCVSDHCExist Then
            tcTotal = New TableCell
            tcTotal.Text = udtformatter.formatMoney(intTotalUsedHCVSDHC.ToString(), False)
            tcTotal.Style.Add("text-align", "right")
            gvBottomRowTotal.Cells.Add(tcTotal)
        End If
        ' CRE19-006 (DHC) [End][Winnie]

        tcTotal = New TableCell
        tcTotal.Text = ""
        tcTotal.BackColor = Drawing.Color.DarkGray
        tcTotal.Width = 80
        tcTotal.Style.Add("text-align", "right")
        gvBottomRowTotal.Cells.Add(tcTotal)

        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        tcTotal = New TableCell
        tcTotal.Text = ""
        tcTotal.BackColor = Drawing.Color.DarkGray
        tcTotal.Width = 80
        tcTotal.Style.Add("text-align", "right")
        gvBottomRowTotal.Cells.Add(tcTotal)
        ' CRE19-003 (Opt voucher capping) [End][Winnie]

        gvw.Style.Add("position", "relative")
        gvw.Style.Add("left", "-3px")
        gvw.Style.Add("table-layout", "fixed")

        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim intBasicWidth As Integer = 780
        ' CRE19-003 (Opt voucher capping) [End][Winnie]

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        intBasicWidth = intBasicWidth + 80 * (intUsedSpan - 1)
        ' CRE19-006 (DHC) [End][Winnie]

        If blnRefundExist Then intBasicWidth = intBasicWidth + 75
        gvw.Width = Unit.Pixel(intBasicWidth)
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

        'Add the bottom row: Total
        gvw.Controls(0).Controls.Add(gvBottomRowTotal)


        'End If
        'CRE16-025 (Lowering voucher eligibility age) [End][Chris YIM]

        '==========================================
        ' Next Year forfeited table

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
        ' ---------------------------------------------------------------------------------------------------------
        ' Show to be forfeited voucher when the person is still alive and eligible in current year
        If blnIsEligible AndAlso Not blnIsDead Then

            ' Separator Row
            Dim gvSeparatorRow As New GridViewRow(0, 0, DataControlRowType.Separator, DataControlRowState.Insert)
            gvSeparatorRow.Style.Add("border", "1px solid #dedfde")
            gvSeparatorRow.Height = 15

            gvw.Controls(0).Controls.Add(gvSeparatorRow)

            ' Header row
            Dim gvHeaderRowNextYear As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
            Dim tcNextYear As TableCell = Nothing

            tcNextYear = New TableCell
            tcNextYear.Text = Me.GetGlobalResourceObject("Text", "VoucherUsageNextPositionHeading")
            tcNextYear.Style.Add("border", "1px solid white")
            tcNextYear.Style.Add("text-align", "left")
            tcNextYear.ColumnSpan = gvw.Columns.Count

            gvHeaderRowNextYear.Cells.Add(tcNextYear)
            gvHeaderRowNextYear.Style.Add("font-weight", "bold")
            gvHeaderRowNextYear.Style.Add("background-color", "#31859C")

            gvw.Controls(0).Controls.Add(gvHeaderRowNextYear)

            ' Forfeit Data
            Dim gvDataRowNextYear As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

            Dim intNextYearAge As Integer = 0
            Dim intNextYearGiven As Integer = 0
            Dim intNextYearMaxAccum As Integer = 0
            Dim intNextYearWriteOff As Integer = 0
            Dim intNextYearPeriodEndBal As Integer = 0


            intNextYearAge = udtVoucherInfo.GetNextForfeitDate.Year - udtEHSPersonalInfo.DOB.Year
            intNextYearGiven = udtVoucherInfo.GetNextDepositAmount
            intNextYearMaxAccum = udtVoucherInfo.GetNextCappingAmount
            intNextYearWriteOff = udtVoucherInfo.GetNextForfeitAmount
            intNextYearPeriodEndBal = udtVoucherInfo.GetAvailableVoucher + udtVoucherInfo.GetNextDepositAmount - udtVoucherInfo.GetNextForfeitAmount


            tcNextYear = New TableCell
            tcNextYear.Text = "As at " + udtVoucherInfo.GetNextForfeitDate.ToString("d MMM yyyy")
            gvDataRowNextYear.Cells.Add(tcNextYear)

            tcNextYear = New TableCell
            tcNextYear.Text = intNextYearAge.ToString()
            tcNextYear.Style.Add("text-align", "right")
            gvDataRowNextYear.Cells.Add(tcNextYear)

            tcNextYear = New TableCell
            tcNextYear.Text = udtformatter.formatMoney(intNextYearGiven.ToString(), False)
            tcNextYear.Style.Add("text-align", "right")
            gvDataRowNextYear.Cells.Add(tcNextYear)

            tcNextYear = New TableCell
            tcNextYear.Text = udtformatter.formatMoney(intNextYearMaxAccum.ToString(), False)
            tcNextYear.Style.Add("text-align", "right")
            gvDataRowNextYear.Cells.Add(tcNextYear)

            tcNextYear = New TableCell
            tcNextYear.Text = udtformatter.formatMoney(intNextYearWriteOff.ToString(), False)
            tcNextYear.Style.Add("text-align", "right")
            gvDataRowNextYear.Cells.Add(tcNextYear)

            If blnRefundExist Then
                tcNextYear = New TableCell
                tcNextYear.Text = Me.GetGlobalResourceObject("Text", "NA")
                tcNextYear.Style.Add("text-align", "right")
                gvDataRowNextYear.Cells.Add(tcNextYear)
            End If

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            tcNextYear = New TableCell
            tcNextYear.Text = Me.GetGlobalResourceObject("Text", "NA")
            'tcNextYear.ColumnSpan = 2
            tcNextYear.Style.Add("text-align", "right")
            gvDataRowNextYear.Cells.Add(tcNextYear)
            ' CRE19-006 (DHC) [End][Winnie]

            If blnHCVSCHNExist Then
                tcNextYear = New TableCell
                tcNextYear.Text = Me.GetGlobalResourceObject("Text", "NA")
                tcNextYear.Style.Add("text-align", "right")
                gvDataRowNextYear.Cells.Add(tcNextYear)
            End If

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If blnHCVSDHCExist Then
                tcNextYear = New TableCell
                tcNextYear.Text = Me.GetGlobalResourceObject("Text", "NA")
                tcNextYear.Style.Add("text-align", "right")
                gvDataRowNextYear.Cells.Add(tcNextYear)
            End If
            ' CRE19-006 (DHC) [End][Winnie]

            tcNextYear = New TableCell

            If intNextYearPeriodEndBal >= 0 Then
                tcNextYear.Text = udtformatter.formatMoney(intNextYearPeriodEndBal.ToString(), False)
            Else
                tcNextYear.Text = String.Format("{0}" + "<br>" + "<font color=""red"">[{1}]</font>", _
                                                  udtformatter.formatMoney("0", False), _
                                                  udtformatter.formatMoney(intNextYearPeriodEndBal.ToString(), False))
            End If

            tcNextYear.Style.Add("text-align", "right")
            gvDataRowNextYear.Cells.Add(tcNextYear)

            tcNextYear = New TableCell
            tcNextYear.Text = Me.GetGlobalResourceObject("Text", "NA")
            tcNextYear.Style.Add("text-align", "right")
            gvDataRowNextYear.Cells.Add(tcNextYear)

            gvDataRowNextYear.Style.Add("background-color", "#FFDDCC")

            gvw.Controls(0).Controls.Add(gvDataRowNextYear)

        End If
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]

        Return gvw
    End Function
    ' CRE13-006 - HCVS Ceiling [End][Tommy L]

    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Private Sub GetTransactionHistoryPeriod(ByRef dtmPeriodFrom As DateTime, ByRef dtmPeriodTo As DateTime)

        Dim udtGF As New Common.ComFunction.GeneralFunction
        Dim dtmCurrentDate As DateTime = udtGF.GetSystemDateTime.Date

        Dim intLatestYear As Integer
        Dim strLatestYear As String = String.Empty

        udtGF.getSystemParameter("VoucherTransactionHistory_LatestYear", strLatestYear, String.Empty)

        If strLatestYear.Equals(String.Empty) OrElse Not Integer.TryParse(strLatestYear, intLatestYear) OrElse intLatestYear < 0 Then
            Throw New Exception(String.Format("The value of system parameter [VoucherTransactionHistory_LatestYear] is not valid. Value: {0}.", IIf(strLatestYear.Equals(String.Empty), "Empty", strLatestYear)))
        End If

        ' e.g.  Today = 2020-01-15   History = 3 yrs  => PeriodFrom = 2018/01/01
        Dim intStartYear As Integer = dtmCurrentDate.Year - (intLatestYear) + 1

        dtmPeriodFrom = New Date(intStartYear, 1, 1)
        dtmPeriodTo = dtmCurrentDate

    End Sub

    Private Function GetVoucherTransactionHistory(ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, _
                                                  ByVal dtmPeriodFrom As DateTime, ByVal dtmPeriodTo As DateTime) As DataTable
        Dim dtTransaction As DataTable
        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL

        Dim udtGF As New Common.ComFunction.GeneralFunction

        dtTransaction = udtEHSTransactionBLL.getVoucherTransactionHistory(udtEHSPersonalInfo.DocCode,
                                                                              udtEHSPersonalInfo.IdentityNum,
                                                                              String.Empty,
                                                                              udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode(),
                                                                              String.Empty,
                                                                              dtmPeriodFrom, dtmPeriodTo)

        Dim dvTransactionHistory As New DataView(dtTransaction)

        Return dvTransactionHistory.ToTable()
    End Function
    ' CRE19-026 (HCVS hotline service) [End][Winnie]

    Private Sub GridView_MergeRow(ByVal gv As GridView, ByVal intColumn As Integer)
        Dim strBgColor As String = String.Empty
        Dim strBgColor1 As String = "#f0eef7"
        Dim strBgColo As String = "White"

        Dim firstRowCell As New TableCell

        strBgColor = strBgColor1

        For rowIndex As Integer = 0 To gv.Rows.Count - 1
            Dim currentRowCell As TableCell = gv.Rows(rowIndex).Cells(intColumn - 1)

            Dim strValue As String = currentRowCell.Text

            If strValue = "&nbsp;" Then
                ' value is empty, merge with first row
                If firstRowCell.RowSpan < 2 Then
                    firstRowCell.RowSpan = 2
                Else
                    firstRowCell.RowSpan += 1
                End If

                currentRowCell.Visible = False

            Else
                firstRowCell = currentRowCell
                firstRowCell.Attributes.Add("bgcolor", strBgColor)
                firstRowCell.VerticalAlign = VerticalAlign.Top

                If strBgColor = strBgColor1 Then
                    strBgColor = strBgColo
                Else
                    strBgColor = strBgColor1
                End If
            End If

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim currentRowPrevCell As TableCell = gv.Rows(rowIndex).Cells(intColumn - 2)
            currentRowPrevCell.BorderWidth = 1
            currentRowPrevCell.BorderColor = Drawing.Color.Gray
            ' CRE19-006 (DHC) [End][Winnie]
        Next
    End Sub
    ' CRE19-003 (Opt voucher capping) [End][Winnie]

#End Region

#Region "Get eHSAccount Function"

    Private Overloads Function GeteHSAccount(ByVal strAccountID As String, ByVal strAccountSource As String, ByVal blnGetAmendmentRecord As Boolean) As Boolean
        Dim blnRes As Boolean = False
        udteHSAccountMaintBLL.EHSAccountRemoveFromSession(FunctionCode)
        udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)

        udtEHSAccount = GeteHSAccount(strAccountID, strAccountSource)

        If blnGetAmendmentRecord Then
            udtEHSAccount_Amendment = udtEHSAccountBLL.LoadAmendingEHSAccountByVRID(strAccountID, Me.txtDocCode.Text.Trim)
        Else
            udtEHSAccount_Amendment = Nothing
        End If


        If Not IsNothing(udtEHSAccount) Then
            Me.udteHSAccountMaintBLL.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
            Me.udteHSAccountMaintBLL.EHSAccount_Amend_SaveToSession(udtEHSAccount_Amendment, FunctionCode)
            blnRes = True
        End If
        Return blnRes
    End Function

    Private Overloads Function GeteHSAccount(ByVal strAccountID As String, ByVal strAccountSource As String)
        Select Case strAccountSource
            Case EHealthAccountType.Temporary
                udtEHSAccount = udtEHSAccountBLL.LoadTempEHSAccountByVRID(strAccountID)

            Case EHealthAccountType.Special
                udtEHSAccount = udtEHSAccountBLL.LoadSpecialEHSAccountByVRID(strAccountID)

            Case EHealthAccountType.Validated
                udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByVRID(strAccountID)

            Case EHealthAccountType.Invalid
                udtEHSAccount = udtEHSAccountBLL.LoadInvalidEHSAccountByVRID(strAccountID)

        End Select

        Return udtEHSAccount
    End Function

    Private Sub BindPersonalInfo(ByVal _udtEHSAccount As EHSAccountModel, ByVal _udtEHSAccount_Amendment As EHSAccountModel, ByVal strDocCode As String)
        Dim blnRes As Boolean = False
        If Not IsNothing(_udtEHSAccount) AndAlso Not strDocCode.Equals(String.Empty) Then
            Dim mode As ActionModel


            If Not IsNothing(Session(SESS_ActionMode)) Then
                mode = CType(Session(SESS_ActionMode), ActionModel)

                Me.ucReadOnlyDocumnetType.Visible = False
                Me.ucInputDocumentType.Visible = False

                Select Case mode
                    Case ActionModel.ReadOnly
                        'ReadOnly Control

                        Me.ucReadOnlyDocumnetType.DocumentType = strDocCode
                        Me.ucReadOnlyDocumnetType.EHSPersonalInformation = _udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)
                        Me.ucReadOnlyDocumnetType.Vertical = True
                        Me.ucReadOnlyDocumnetType.Width = 220

                        ' CRE19-026 (HCVS hotline service) [Start][Winnie]                        
                        Me.ucReadOnlyDocumnetType.ShowDateOfDeath = False
                        Me.ucReadOnlyDocumnetType.ShowCreationMethod = False
                        ' CRE19-026 (HCVS hotline service) [End][Winnie]

                        Me.ucReadOnlyDocumnetType.Build()

                        ucReadOnlyDocumnetType.Visible = True

                        Me.phReadOnlyAccountInfo.Controls.Clear()
                        Dim ucReadOnlyAccountInfo As ucReadOnlyAccountInfo = Me.LoadControl("~/UIControl/ucReadOnlyAccountInfo.ascx")
                        ucReadOnlyAccountInfo.EHSAccount = _udtEHSAccount
                        ucReadOnlyAccountInfo.Width = 220
                        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                        ucReadOnlyAccountInfo.ShowAccountStatusRemarkWhenDeceased = False
                        ucReadOnlyAccountInfo.ShowDeceased = False
                        ' CRE19-026 (HCVS hotline service) [End][Winnie]
                        Me.phReadOnlyAccountInfo.Controls.Add(ucReadOnlyAccountInfo)

                    Case ActionModel.ReadOnly_N_Amending

                        Me.ucInputDocumentType.DocType = strDocCode
                        Me.ucInputDocumentType.EHSAccountOriginal = _udtEHSAccount
                        Me.ucInputDocumentType.EHSAccountAmend = _udtEHSAccount_Amendment
                        Me.ucInputDocumentType.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly
                        Me.ucInputDocumentType.FillValue = True
                        ' CRE19-026 (HCVS hotline service) [Start][Winnie]                        
                        Me.ucInputDocumentType.ShowCreationMethod = False
                        ' CRE19-026 (HCVS hotline service) [End][Winnie]

                        Me.ucInputDocumentType.Built()

                        Me.ucInputDocumentType.Visible = True

                        Me.phReadOnlyAccountInfo.Controls.Clear()
                        Dim ucReadOnlyAccountInfo As ucReadOnlyAccountInfo = Me.LoadControl("~/UIControl/ucReadOnlyAccountInfo.ascx")
                        ucReadOnlyAccountInfo.EHSAccount = _udtEHSAccount
                        ucReadOnlyAccountInfo.Width = 220
                        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                        ucReadOnlyAccountInfo.ShowAccountStatusRemarkWhenDeceased = False
                        ucReadOnlyAccountInfo.ShowDeceased = False
                        ' CRE19-026 (HCVS hotline service) [End][Winnie]
                        Me.phReadOnlyAccountInfo.Controls.Add(ucReadOnlyAccountInfo)
                        'blnRes = True
                End Select
            End If

        End If
    End Sub

    Private Sub SetupInputDocControl(ByVal _udtEHSAccount As EHSAccountModel, ByVal _udtEHSAccount_Amendment As EHSAccountModel)
        Select Case Me.mveHSAccount.ActiveViewIndex
            Case intSearchView
            Case intSearchResult
            Case intAccountDetails
                BindPersonalInfo(_udtEHSAccount, _udtEHSAccount_Amendment, Me.txtDocCode.Text.Trim)

        End Select
    End Sub

#End Region

#Region "Get Scheme Info Function"
    Private Sub BuildVoucherUtilization()

        Dim udtSchemeClaim As SchemeClaimModel = Session(SESS_SchemeClaim)
        Dim udtVoucherInfo As VoucherInfoModel = Session(SESS_VoucherInfo)
        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        If udtSchemeClaim Is Nothing OrElse udtVoucherInfo Is Nothing OrElse udtEHSAccount Is Nothing Then Return

        Dim udtGF As New Common.ComFunction.GeneralFunction
        Dim dtmCurrent As DateTime = udtGF.GetSystemDateTime

        Dim dtmCurrentDate As DateTime = dtmCurrent.Date
        Dim intColWidth_1 As Integer = 220

        Dim intAvailableVoucher As Integer

        Dim blnIsDead As Boolean = False

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        Dim blnShowDeceased As Boolean = False
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

        Try

            Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel
            udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

            pnlVoucher.Visible = True

            intAvailableVoucher = udtVoucherInfo.GetAvailableVoucher()

            If intAvailableVoucher < 0 Then
                intAvailableVoucher = 0
            End If

            Dim tbl As New Table
            Dim rw As TableRow
            Dim cll As TableCell

            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Dim strAlertHtmlImg_Tag_Result As String = String.Empty
            Dim strDeadPeriodVoucherAmount As Integer = 0 ' CRE14-016 

            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim udtDeadPeriodVoucherQuota As VoucherQuotaModel = Nothing
            ' CRE19-003 (Opt voucher capping) [End][Winnie]


            Dim gvw As GridView

            rw = New TableRow

            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Prepare the Voucher Usage grid view
            gvw = GetGridView_VoucherUsage(udtSchemeClaim, udtEHSPersonalInfo, udtVoucherInfo, strAlertHtmlImg_Tag_Result, strDeadPeriodVoucherAmount, udtDeadPeriodVoucherQuota, blnShowDeceased)
            ' CRE19-003 (Opt voucher capping) [End][Winnie]


            'Display N/A for deceased account
            blnIsDead = udtEHSPersonalInfo.Deceased

            If Not blnIsDead Then
                lblAvailableVoucher.Text = udtformatter.formatMoney(CStr(intAvailableVoucher), True)
            Else

                If intAvailableVoucher <= 0 Then
                    lblAvailableVoucher.Text = Me.GetGlobalResourceObject("Text", "N/A")
                Else
                    lblAvailableVoucher.Text = udtformatter.formatMoney(CStr(strDeadPeriodVoucherAmount), True)
                    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                    If blnShowDeceased Then
                        lblAvailableVoucher.Text &= " as of Date of Death"
                    End If
                    ' CRE19-026 (HCVS hotline service) [End][Winnie]
                End If
            End If

            ' CRE19-003 (Opt voucher capping) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            tbl.Controls.Add(rw)

            ' Display Quota for ROP                                     
            Dim udtVoucherQuota As VoucherQuotaModel

            If Not blnIsDead Then
                Dim strProfessionCode_ROP As String = "ROP"
                udtVoucherQuota = udtVoucherInfo.VoucherQuotalist.FilterByProfCodeEffectiveDtm(strProfessionCode_ROP, dtmCurrentDate)

            Else
                ' Get From Grid View
                udtVoucherQuota = udtDeadPeriodVoucherQuota
            End If

            If Not udtVoucherQuota Is Nothing Then
                trAvailableQuota.Visible = True

                lblAvailableQuotaText.Text = String.Format(Me.GetGlobalResourceObject("Text", "ProfessionQuota"), Me.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode))

                lblAvailableQuota.Text = (New Common.Format.Formatter).formatMoney(IIf(udtVoucherQuota.AvailableQuota > 0, udtVoucherQuota.AvailableQuota, 0), True)
                lblAvailableQuota.Text += " " + String.Format(Me.GetGlobalResourceObject("Text", "Upto") _
                                          , udtVoucherQuota.PeriodEndDtm.ToString("dd-MM-yyyy"))
            Else
                trAvailableQuota.Visible = False
            End If


            ' Voucher Usage Panel
            tbl = New Table
            rw = New TableRow

            cll = New TableCell
            cll.Width = intColWidth_1
            cll.VerticalAlign = VerticalAlign.Top

            rw.Controls.Add(cll)

            cll = New TableCell
            cll.VerticalAlign = VerticalAlign.Top


            Dim tblVU As Table = New Table
            Dim rwVU As TableRow = New TableRow
            Dim cllVU As TableCell = New TableCell

            cllVU.Controls.Add(gvw)

            rwVU.Controls.Add(cllVU)

            tblVU.Controls.Add(rwVU)

            If Not strAlertHtmlImg_Tag_Result.Equals(String.Empty) Then
                cllVU = New TableCell
                rwVU = New TableRow

                Dim tblRemark = New Table
                Dim rwRemark = New TableRow
                Dim cllRemark = New TableCell

                cllRemark.VerticalAlign = VerticalAlign.Top
                cllRemark.Text = strAlertHtmlImg_Tag_Result

                rwRemark.Controls.Add(cllRemark)
                tblRemark.Controls.Add(rwRemark)

                cllRemark = New TableCell
                cllRemark.VerticalAlign = VerticalAlign.Top
                cllRemark.Text = Me.GetGlobalResourceObject("Text", "ClaimBySameId_DiffDOB")

                rwRemark.Controls.Add(cllRemark)
                tblRemark.Controls.Add(rwRemark)
                tblRemark.Style.Add("position", "relative")
                tblRemark.Style.Add("left", "-5px")

                cllVU.Controls.Add(tblRemark)
                rwVU.Controls.Add(cllVU)
                tblVU.Controls.Add(rwVU)

                tblVU.Width = gvw.Width.Value
            End If

            cll.Controls.Add(tblVU)

            rw.Controls.Add(cll)

            tbl.Controls.Add(rw)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

            tbl.Width = Unit.Pixel(1200)

            pnlVoucherUsage.Controls.Add(tbl)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub SetupSchemeInformation()

        Session(SESS_VoucherInfo) = Nothing
        Session(SESS_SchemeClaim) = Nothing
        Session(SESS_VoucherTransHistory) = Nothing

        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountType", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteStartLog(AuditLogDesc.GetSchemeInfo_ID, AuditLogDesc.GetSchemeInfo)

        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel
        udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

        Dim udtSchemeClaimBLL As Scheme.SchemeClaimBLL = New Scheme.SchemeClaimBLL
        Dim udtSchemeClaimList As Scheme.SchemeClaimModelCollection

        udtSchemeClaimList = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithSubsidizeGroup

        Dim udtGF As New Common.ComFunction.GeneralFunction
        Dim dtmCurrent As DateTime = udtGF.GetSystemDateTime

        Dim dtmCurrentDate As DateTime = dtmCurrent.Date

        Dim blnVoucherIncluded As Boolean = False

        Try
            For Each udtSchemeClaim As Scheme.SchemeClaimModel In udtSchemeClaimList

                ' ======== Voucher ========
                If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = Common.Component.Scheme.SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher Then

                    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                    If blnVoucherIncluded Then Continue For

                    blnVoucherIncluded = True
                    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    Dim dtmStartDate As DateTime = udtSchemeClaim.SubsidizeGroupClaimList(0).ClaimPeriodFrom
                    Dim dtmEndDate As DateTime = Date.Now

                    Dim udtCloneSchemeClaim As New SchemeClaimModel(udtSchemeClaim)

                    udtCloneSchemeClaim.SubsidizeGroupClaimList = udtSchemeClaim.SubsidizeGroupClaimList.FilterbyRange(dtmStartDate, dtmEndDate)

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]


                    Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, VoucherInfoModel.AvailableQuota.Include)

                    udtVoucherInfo.GetInfo(dtmCurrentDate, udtCloneSchemeClaim, udtEHSPersonalInfo)

                    udtVoucherInfo.FunctionCode = FunctionCode

                    Session(SESS_VoucherInfo) = udtVoucherInfo
                    Session(SESS_SchemeClaim) = udtCloneSchemeClaim

                    BuildVoucherUtilization()

                    ' Collapse Voucher Usage
                    collapsiblePanelExtender_v4_HCVS.ClientState = True


                    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                    ' ------------------------------------------------------------------------
                    ' -- Voucher Transaction History --

                    Dim dtmPeriodFrom As DateTime = Nothing
                    Dim dtmPeriodTo As DateTime = Nothing

                    GetTransactionHistoryPeriod(dtmPeriodFrom, dtmPeriodTo)

                    Dim dtVoucherTransHistory As DataTable = GetVoucherTransactionHistory(udtCloneSchemeClaim, udtEHSPersonalInfo, dtmPeriodFrom, dtmPeriodTo)

                    lblVoucherTransHistory.Text = String.Format("{0} (From {1} to {2})", HttpContext.GetGlobalResourceObject("Text", "VoucherTransHistory"), _
                                                                udtformatter.formatDisplayDate(dtmPeriodFrom), udtformatter.formatDisplayDate(dtmPeriodTo))

                    If dtVoucherTransHistory.Rows.Count > 0 Then
                        trVoucherTransHistoryNoRecord.Visible = False
                    Else
                        trVoucherTransHistoryNoRecord.Visible = True
                    End If

                    Session(SESS_VoucherTransHistory) = dtVoucherTransHistory

                    Me.GridViewDataBind(Me.gvVoucherTransHistory, dtVoucherTransHistory, "Service_Receive_Dtm", "DESC", False)

                    'auidt log
                    Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                    Me.udtAuditLogEntry.AddDescripton("SchemeCode", udtCloneSchemeClaim.SchemeCode.Trim)
                    Me.udtAuditLogEntry.AddDescripton("Trans History Period From", dtmPeriodFrom.ToString(udtformatter.DisplayDateFormat))
                    Me.udtAuditLogEntry.AddDescripton("Trans History Period To", dtmPeriodTo.ToString(udtformatter.DisplayDateFormat))
                    Me.udtAuditLogEntry.AddDescripton("No Of Trans History Record", dtVoucherTransHistory.Rows.Count)
                    Me.udtAuditLogEntry.WriteEndLog(AuditLogDesc.GetSchemeInfoSuccess_ID, AuditLogDesc.GetSchemeInfoSuccess)
                    ' CRE19-026 (HCVS hotline service) [End][Winnie]

                Else
                    ' ======== Skip Vaccine ========

                End If
            Next


        Catch ex As Exception
            If Not IsNothing(udtEHSAccount) Then
                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                Me.udtAuditLogEntry.AddDescripton("AccountType", udtEHSAccount.AccountSourceString.Trim)
            End If
            Me.udtAuditLogEntry.WriteEndLog(AuditLogDesc.GetSchemeInfoFail_ID, AuditLogDesc.GetSchemeInfoFail)
            Throw
        End Try

    End Sub
#End Region

#Region "Action Model"
    Public Enum ActionModel
        [ReadOnly]
        ReadOnly_N_Amending

    End Enum
#End Region

#Region "Pop-up event"
    Public Overrides Sub GridViewHeaderImage_Click(ByVal sender As Object, ByVal e As Common.Component.SortedGridviewHeader.SortedGridviewHeaderModel.GridViewHeaderImageEventArgs)
        'Dim intColumn As Integer
        'intColumn = e.intColumn

        popupDocTypeHelp.Show()
        udcDocTypeLegend.BindDocType(Session("language"))
    End Sub

    Protected Sub ibtnCloseDocTypeHelp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseDocTypeHelp.Click
        popupDocTypeHelp.Hide()
    End Sub

#End Region

    Private Sub ShowAccountDetails(ByVal strAccID As String, ByVal strDocType As String, ByVal strAccSrc As String)
        txtDocCode.Text = String.Empty

        Dim blnShowAmendmentRecord As Boolean = False


        Dim udtEHSAccountModel As EHSAccount.EHSAccountModel = GeteHSAccount(strAccID, strAccSrc)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", strAccID)
        Me.udtAuditLogEntry.AddDescripton("DocCode", udtEHSAccountModel.getPersonalInformation(strDocType).DocCode)
        Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccSrc)
        Me.udtAuditLogEntry.AddDescripton("PersonalInformationStatus", udtEHSAccountModel.getPersonalInformation(strDocType).RecordStatus)
        Me.udtAuditLogEntry.AddDescripton("IdentityNumber", udtEHSAccountModel.getPersonalInformation(strDocType).IdentityNum)
        Dim udtAuditLogInfo As AuditLogInfo
        udtAuditLogInfo = New AuditLogInfo(udtEHSAccountModel.CreateBy, Nothing, strAccSrc, _
                                        strAccID, udtEHSAccountModel.getPersonalInformation(strDocType).DocCode, udtEHSAccountModel.getPersonalInformation(strDocType).IdentityNum)
        Me.udtAuditLogEntry.WriteStartLog(AuditLogDesc.SelectEHSAccount_ID, AuditLogDesc.SelectEHSAccount, udtAuditLogInfo)

        If strAccSrc.Trim.Equals(EHSAccountModel.SysAccountSourceClass.ValidateAccount) Then
            If udtEHSAccountModel.getPersonalInformation(strDocType).RecordStatus.Trim.Equals(EHSAccountModel.PersonalInformationRecordStatusClass.UnderAmendment) Then
                blnShowAmendmentRecord = True
            End If
        End If

        txtDocCode.Text = udtEHSAccountModel.getPersonalInformation(strDocType).DocCode

        If Me.GetEHSAccount(strAccID, strAccSrc, blnShowAmendmentRecord) Then
            udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
            udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

            If blnShowAmendmentRecord Then
                Session(SESS_ActionMode) = ActionModel.ReadOnly_N_Amending
            Else
                Session(SESS_ActionMode) = ActionModel.ReadOnly
            End If

            Me.mveHSAccount.ActiveViewIndex = intAccountDetails

            Me.udtAuditLogEntry.AddDescripton("AccountID", strAccID)
            Me.udtAuditLogEntry.AddDescripton("DocCode", udtEHSAccountModel.getPersonalInformation(strDocType).DocCode)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccSrc)
            Me.udtAuditLogEntry.AddDescripton("IdentityNumber", udtEHSAccountModel.getPersonalInformation(strDocType).IdentityNum)
            Me.udtAuditLogEntry.WriteEndLog(AuditLogDesc.SelectEHSAccountSuccess_ID, AuditLogDesc.SelectEHSAccountSuccess)

            Me.ibtnAccountDetailsBack.Visible = False
        Else
            udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
            Me.udtAuditLogEntry.AddDescripton("AccountID", strAccID)
            Me.udtAuditLogEntry.AddDescripton("DocCode", udtEHSAccountModel.getPersonalInformation(strDocType).DocCode)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccSrc)
            Me.udtAuditLogEntry.AddDescripton("IdentityNumber", udtEHSAccountModel.getPersonalInformation(strDocType).IdentityNum)
            udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, AuditLogDesc.SelectEHSAccountFail_ID, AuditLogDesc.SelectEHSAccountFail)
        End If
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="strSearchAccountID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsValidEHSAccountNumber(ByVal strSearchAccountID As String) As Boolean
        Dim arrAccountID() As String = strSearchAccountID.Split(EHAccountIDSeparator)
        For i As Integer = 0 To arrAccountID.Length - 1
            'CRE13-006 HCVS Ceiling [Start][Karl]
            If arrAccountID(i).Trim() <> String.Empty Then
                If Not udtvalidator.chkValidatedEHSAccountNumber(arrAccountID(i).Trim()) Then
                    'If arrAccountID(0).Trim() <> String.Empty Then
                    '    If Not udtvalidator.chkValidatedEHSAccountNumber(arrAccountID(0).Trim()) Then
                    'CRE13-006 HCVS Ceiling [End][Karl]
                    Return False
                End If
            End If
        Next

        Return True
    End Function

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="arreHSAccountID"></param>
    ''' <param name="iStartIndex"></param>
    ''' <param name="iEndIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetMultipleAccountIDList(ByVal arreHSAccountID() As String, ByVal iStartIndex As Integer, ByVal iEndIndex As Integer) As String
        Dim udtFormatter As New Formatter
        Dim udtGeneralFunction = New GeneralFunction
        Dim sbResult As New StringBuilder()
        For i As Integer = iStartIndex To iEndIndex
            If sbResult.Length > 0 Then sbResult.Append(", ")
            sbResult.Append(udtFormatter.formatValidatedEHSAccountNumber(arreHSAccountID(i)))
        Next

        Return sbResult.ToString
    End Function

#Region "Implement IWorkingData (CRE11-004)"

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtEHSAccount As EHSAccountModel = Nothing
        Dim udtSPBLL As ServiceProviderBLL = Nothing
        Dim strAccountCreateBy As String = String.Empty
        Dim udtDB As Database = Nothing

        If Not IsNothing(Session(SESS_AccountCreateBy)) Then
            strAccountCreateBy = CType(Session(SESS_AccountCreateBy), String)
        End If

        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)
        If Not IsNothing(udtEHSAccount) Then
            If udtEHSAccount.CreateByBO Then
                'Create By BO
                If strAccountCreateBy <> udtEHSAccount.CreateBy Or IsNothing(Session(SESS_ServiceProvider)) Then
                    udtSP = New ServiceProviderModel()
                    'udtSP.SPID = udtEHSAccount.CreateBy
                    udtSP.SPID = String.Empty
                    Session(SESS_ServiceProvider) = udtSP
                    Session(SESS_AccountCreateBy) = udtEHSAccount.CreateBy
                    Return udtSP
                Else
                    udtSP = CType(Session(SESS_ServiceProvider), ServiceProviderModel)
                    Return udtSP
                End If
            Else
                'Create By SP
                If strAccountCreateBy <> udtEHSAccount.CreateBy Or IsNothing(Session(SESS_ServiceProvider)) Then
                    udtSPBLL = New ServiceProviderBLL()
                    udtDB = New Database()
                    udtSP = New ServiceProviderModel()
                    udtSP.SPID = IIf(IsNothing(udtEHSAccount.CreateSPID), String.Empty, udtEHSAccount.CreateSPID)
                    Session(SESS_ServiceProvider) = udtSP
                    Session(SESS_AccountCreateBy) = udtEHSAccount.CreateBy
                    Return udtSP
                Else
                    udtSP = CType(Session(SESS_ServiceProvider), ServiceProviderModel)
                    Return udtSP
                End If
            End If
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve Document Code which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocCode() As String
        If IsNothing(Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FuncCode)) Then
            Return Nothing
        End If

        If IsNothing(txtDocCode.Text) Then
            Return Nothing
        Else
            If txtDocCode.Text.Trim = "" Then
                Return Nothing
            Else
                Return txtDocCode.Text.Trim
            End If
        End If
    End Function

#End Region

#Region "Unmask Document No (CRE11-007)"

    Protected Sub chkMaskDocumentNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim chk As CheckBox = CType(sender, CheckBox)
        udtAuditLog.AddDescripton("Checked change to", IIf(chk.Checked, "T", "F"))
        udtAuditLog.WriteLog(AuditLogDesc.MaskIdentityDocumentNoClick_ID, AuditLogDesc.MaskIdentityDocumentNoClick)

        If chk.Checked Then
            ' Unchecked -> Checked
            gvAcctList.Columns(4).Visible = False
            gvAcctList.Columns(3).Visible = True

        Else
            ' Checked -> Unchecked
            popupUnmask.Show()
            ViewState(VS.UnmaskPopup) = PopupStatus.Active
            InitPopupUnmask()
        End If

    End Sub

    Protected Sub ibtnPopupUnmaskConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Confirm_Click
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(AuditLogDesc.MaskIdentityDocumentNoSuccess_ID, AuditLogDesc.MaskIdentityDocumentNoSuccess)

        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        Select Case tcSearchRoute.ActiveTabIndex
            Case 0
                gvAcctList.Columns(4).Visible = True
                gvAcctList.Columns(3).Visible = False

        End Select

    End Sub

    Protected Sub ibtnPopupUnmaskCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Cancel_Click
        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        Select Case tcSearchRoute.ActiveTabIndex
            Case 0
                chkMaskDocumentNo.Checked = True
        End Select
    End Sub

    Private Sub InitPopupUnmask()
        ' CRE12-014 - Relax 500 row limit in back office platform [Start][Twinsen]
        popupUnmask.PopupDragHandleControlID = udcPUInputToken.Header.ClientID
        udcPUInputToken.HeaderText = Me.GetGlobalResourceObject("Text", "ConfirmUnmaskIdentityDocumentNo")
        ' CRE12-014 - Relax 500 row limit in back office platform [End] [Twinsen]
        udcPUInputToken.Message = Me.GetGlobalResourceObject("Text", "ConfirmUnmaskMessage")
        udcPUInputToken.Build()
    End Sub

#End Region

End Class