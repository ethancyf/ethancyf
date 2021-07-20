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
Imports Common.Component.PassportIssueRegion

Partial Public Class eHSAccountEnquiry
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
    Dim udtPassportIssueRegionBLL As PassportIssueRegionBLL = New PassportIssueRegionBLL


    Dim udtEHSAccount As EHSAccountModel
    Dim udtEHSAccount_Amendment As EHSAccountModel

    Public Const SESSION_REDIRECT_PARAMETER As String = "PageRedirectorBLL.Parameter"
    Public Const SESSION_REDIRECT_SOURCE As String = "PageRedirectorBLL.Source"
    Public Const REDIRECT_NAME As String = "eHSAccountEnquiry"

    <Serializable()> _
    Public Class RedirectParamAccountInfo
        Public AccID As String
        Public DocType As String
        Public AccSrc As String
    End Class

#Region "Audit Log Description"
    Public Class AuditLogDesc

        Public Const eHSAccountEnquiryPageLoad = "eHealth Account Enquiry Loaded"  '0
        'Public Const SearchRoute1 = "Search By Route 1"   '1
        'Public Const SearchRoute1Success = "Search By Route1 Success"   '2
        'Public Const SearchRoute1Fail = "Search By Route 1 Fail"   '3
        'Public Const SearchRoute2 = "Search By Route 2"   '4
        'Public Const SearchRoute2Success = "Search By Route 2 Success"  '5
        'Public Const SearchRoute2Fail = "Search By Route 2 Fail"  '6
        Public Const SearchFail = "Search Fail" '7
        Public Const SelectEHSAccount As String = "Select and view eHealth Account"   '8
        Public Const SelectEHSAccountSuccess As String = "Select and view eHealth Account Success"    '9
        Public Const SelectEHSAccountFail As String = "Select and view eHealth Account Fail"    '10
        'Amendment History
        Public Const GetAmendmentHistory As String = "Get Amendment History" '11
        Public Const GetAmendmentHistorySuccess As String = "Get Amendment History Success" '12
        Public Const GetAmendmentHistorySuccessNoRecordFound As String = "Get Amendment History Success No record Found" '13
        Public Const GetAmendmentHistoryFail As String = "Get Amendment History Fail" '14
        'Scheme Info
        Public Const GetSchemeInfo As String = "Get scheme info" '15
        Public Const GetSchemeInfoSuccess As String = "Get scheme info Success" '16
        Public Const GetSchemeInfoSuccessNoRecordFound As String = "Get scheme info Success No Record Found" '17
        Public Const GetSchemeInfoSuccessNotEligible As String = "Get scheme info Success Not Eligible" '18
        Public Const GetSchemeInfoFail As String = "Get scheme info Fail" '19

        Public Const BackToSearch As String = "Back To Search" '20
        Public Const BackToResultList As String = "Back To Search Result List" '21
        Public Const BackToAccDetailFromAmendmentHist As String = "Back to Account Detail From Amendment History" '22
        Public Const BackToAccDetailFromSchemeInfo As String = "Back to Account Detail From Scheme Info" '23
        Public Const VaccinationRecordClick As String = "Vaccination record button click" '24
        Public Const BackToAccDetailFromVaccinationRecord As String = "Back to Account Detail From Vaccination Record" '25

        ' CRE11-007
        Public Const MaskIdentityDocumentNoClick As String = "Search Result - Mask Identity Document No. click"
        Public Const MaskIdentityDocumentNoClick_ID As String = LogID.LOG00026
        Public Const MaskIdentityDocumentNoSuccess As String = "Search Result - Unmask Identity Document No. success"
        Public Const MaskIdentityDocumentNoSuccess_ID As String = LogID.LOG00027

        ''CRE14-016 (To introduce "Deceased" status into eHS)
        Public Const SearchByParticulars = "Search By Particulars"   '124
        Public Const SearchByParticularsSuccess = "Search By Particulars Success"   '125
        Public Const SearchByParticularsFail = "Search By Particulars Fail"   '126
        Public Const SearchbyManualValidation = "Search By Manual Validation"   '127
        Public Const SearchbyManualValidationSuccess = "Search By Manual Validation Success"  '128
        Public Const SearchbyManualValidationFail = "Search By Manual Validation Fail"  '129
    End Class

#End Region

#Region "Constant Value"
    Private Const intSearchView As Integer = 0
    Private Const intSearchResult As Integer = 1
    Private Const intAccountDetails As Integer = 2
    Private Const intAmendmentHistroy As Integer = 3
    Private Const intSchemeInfo As Integer = 4
    Private Const intVaccinationRecord As Integer = 5

    Private Const SESS_Language As String = "language"
    Private Const SESS_Result As String = "eHSAccount_Enquiry_SearchResult"
    Private Const SESS_ActionMode As String = "eHSAccount_Enquiry_ActionMode"
    Private Const SESS_AmendHistory As String = "eHSAccount_Enquiry_AmendHistory"
    Private Const SESS_ServiceProvider As String = "eHSAccount_Enquiry_ServiceProviderModel"
    Private Const SESS_AccountCreateBy As String = "eHSAccount_Enquiry_AccountCreateBy"
    Private Const FuncCode As String = FunctCode.FUNT010302
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

#Region "Page Event"

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
#Region "SF Search"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
        Dim blnReturn As Boolean = True

        Select Case Me.tcSearchRoute.ActiveTabIndex
            Case 0
                '    'English Name
                '    If Me.txtSearchENameR1.Text.Trim.Equals(String.Empty) Then
                '        Me.lblAcctListENameR1.Text = Me.GetGlobalResourceObject("Text", "Any")
                '    Else
                '        Me.lblAcctListENameR1.Text = Me.txtSearchENameR1.Text.Trim
                '    End If

                '    'Account Type
                '    If Me.ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary OrElse _
                '           Me.ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Special Then
                '        Me.lblAcctListAcctTypeR1.Text = Me.ddlSearchAcctTypeR1.SelectedItem.Text + "(" + Me.ddlSearchTempAcct.SelectedItem.Text + ")"
                '    Else
                '        Me.lblAcctListAcctTypeR1.Text = Me.ddlSearchAcctTypeR1.SelectedItem.Text
                '    End If

                '    'Creation Date
                '    Dim blnIsValid As Boolean = True

                '    If Me.txtSearchCreationDateFromR1.Text.Trim.Equals(String.Empty) Then
                '        Me.lblAcctListCreateDateFromR1.Text = Me.GetGlobalResourceObject("Text", "Any")
                '    Else
                '        'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                '        '-----------------------------------------------------------------------------------------
                '        'udtSM = udtvalidator.chkInputDate(Me.txtSearchCreationDateFromR1.Text, False)
                '        udtSM = udtvalidator.chkInputDate(Me.txtSearchCreationDateFromR1.Text, True, True)

                '        If Not udtSM Is Nothing Then
                '            Me.imgFromDateError.Visible = True
                '            'If udtSM.MessageCode = MsgCode.MSG00022 Then
                '            '    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00139)
                '            'Else
                '            '    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00137)
                '            'End If
                '            Select Case udtSM.MessageCode
                '                Case MsgCode.MSG00022
                '                    '"Creation Date From" should not be future date.
                '                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00139)
                '                Case MsgCode.MSG00028
                '                    'Please input the "Creation Date From".
                '                    Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "CreationDateFrom"))
                '                Case MsgCode.MSG00029
                '                    '"Creation Date From" is invalid.
                '                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00137)
                '                Case Else
                '                    'Please input the "Creation Date From".
                '                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CreationDateFrom"))
                '            End Select
                '            blnIsValid = False
                '        Else
                '            Me.lblAcctListCreateDateFromR1.Text = udtformatter.formatSearchDate(Me.txtSearchCreationDateFromR1.Text.Trim())
                '            Me.lblAcctListCreateDateFromR1.Text = udtformatter.convertDate(Me.lblAcctListCreateDateFromR1.Text.Trim(), "en")
                '        End If
                '        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]
                '    End If

                '    If Me.txtSearchCreationDateFromR1.Text.Trim.Equals(String.Empty) Then
                '        Me.lblAcctListCreateDateToR1.Text = Me.GetGlobalResourceObject("Text", "Any")
                '    Else
                '        'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                '        '-----------------------------------------------------------------------------------------
                '        'udtSM = udtvalidator.chkInputDate(Me.txtSearchCreationDateToR1.Text, False)
                '        udtSM = udtvalidator.chkInputDate(Me.txtSearchCreationDateToR1.Text, True, True)

                '        If Not udtSM Is Nothing Then
                '            Me.imgToDateError.Visible = True
                '            'If udtSM.MessageCode = MsgCode.MSG00022 Then
                '            '    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00140)
                '            'Else
                '            '    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00138)
                '            'End If
                '            Select Case udtSM.MessageCode
                '                Case MsgCode.MSG00022
                '                    '"Creation Date To" should not be future date.
                '                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00140)
                '                Case MsgCode.MSG00028
                '                    'Please input the "Creation Date To".
                '                    Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "CreationDateTo"))
                '                Case MsgCode.MSG00029
                '                    '"Creation Date To" is invalid.
                '                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00138)
                '                Case Else
                '                    'Please input the "Creation Date To".
                '                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CreationDateTo"))
                '            End Select
                '            blnIsValid = False
                '        Else
                '            Me.lblAcctListCreateDateToR1.Text = udtformatter.formatSearchDate(Me.txtSearchCreationDateToR1.Text.Trim())
                '            Me.lblAcctListCreateDateToR1.Text = udtformatter.convertDate(Me.lblAcctListCreateDateToR1.Text.Trim(), "en")
                '        End If
                '        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]
                '    End If

                '    If blnIsValid AndAlso Not Me.lblAcctListCreateDateFromR1.Text.Equals(Me.GetGlobalResourceObject("Text", "Any")) AndAlso _
                '        Not Me.lblAcctListCreateDateToR1.Text.Equals(Me.GetGlobalResourceObject("Text", "Any")) Then

                '        udtSM = udtvalidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00136, Me.lblAcctListCreateDateFromR1.Text, Me.lblAcctListCreateDateToR1.Text)
                '        If Not udtSM Is Nothing Then
                '            Me.imgFromDateError.Visible = True
                '            Me.imgToDateError.Visible = True
                '            blnIsValid = False
                '            Me.udcMsgBox.AddMessage(udtSM)
                '        End If
                '    End If

                '    Dim dt As DataTable = Nothing

                '    If udcMsgBox.GetCodeTable.Rows.Count = 0 Then
                '        blnReturn = True
                '    Else
                '        Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00003, AuditLogDesc.SearchRoute1Fail)
                '        blnReturn = False
                '    End If

                'Case 1
                Dim strExactDOB As String = String.Empty
                Dim dtDOB As Nullable(Of DateTime) = Nothing
                Dim strDocCode As String = String.Empty
                Dim strAdoptionPrefixNum As String = String.Empty
                Dim strIdentityNum As String = String.Empty
                Dim streHSAccountID As String = String.Empty
                Dim arreHSAccountID() As String = Nothing
                Dim strRefNo As String = String.Empty
                Dim strAcctType As String = String.Empty
                Dim dtCreationDate As Nullable(Of DateTime) = Nothing

                'If Me.txtSearchENameR2.Text.Trim.Equals(String.Empty) AndAlso _
                '    Me.ddlSearchDocTypeR2.SelectedValue.Trim.Equals(String.Empty) AndAlso _
                '    Me.txtSearchIdentityNumR2.Text.Trim.Equals(String.Empty) AndAlso _
                '    Me.txtSearchDOBR2.Text.Trim.Equals(String.Empty) AndAlso _
                '    Me.txtSearchAccountIDR2.Text.Trim.Equals(String.Empty) AndAlso _
                '    Me.txtSearchRefNo.Text.Trim.Equals(String.Empty) Then

                '    udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
                '    Me.udcMsgBox.AddMessage(udtSM)
                '    Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00006, AuditLogDesc.SearchRoute2Fail)
                '    'CRE13-006 HCVS Ceiling [Start][Karl]
                '    blnReturn = False
                '    'CRE13-006 HCVS Ceiling [End][Karl]
                'Else
                If Not Me.txtSearchAccountIDR2.Text.Trim() = String.Empty AndAlso Not IsValidEHSAccountNumber(Me.txtSearchAccountIDR2.Text.Trim()) Then
                    ' Invalid EHS Account ID

                    ' Replace the system message to common function_code
                    'udtSM = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
                    udtSM = New SystemMessage(CommonFunctionCode, SeverityCode.SEVE, MsgCode.MSG00362)

                    Me.udcMsgBox.AddMessage(udtSM)
                    'Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00006, AuditLogDesc.SearchRoute2Fail)
                    Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00126, AuditLogDesc.SearchByParticularsFail)
                    'CRE13-006 HCVS Ceiling [Start][Karl]
                    blnReturn = False
                    'CRE13-006 HCVS Ceiling [End][Karl]

                    Me.imgSearchAccountIDR2Error.Visible = True

                Else
                    'Doc Type
                    Me.lblAcctListDocTypeR2.Text = Me.ddlSearchDocTypeR2.SelectedItem.Text.Trim
                    strDocCode = Me.ddlSearchDocTypeR2.SelectedValue.Trim

                    'Identity Num
                    If Me.txtSearchIdentityNumR2.Text.Trim.Equals(String.Empty) Then
                        Me.lblAcctListIdentityNumR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Me.lblAcctListIdentityNumR2.Text = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper

                        Dim strIdentityNumFullTemp As String
                        strIdentityNumFullTemp = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")

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
                    If Me.txtSearchENameR2.Text.Equals(String.Empty) Then
                        Me.lblAcctListENameR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Me.lblAcctListENameR2.Text = Me.txtSearchENameR2.Text.Trim
                    End If

                    ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                    'Chinese Name
                    If Me.txtSearchCNameR2.Text.Equals(String.Empty) Then
                        Me.lblAcctListCNameR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Me.lblAcctListCNameR2.Text = Me.txtSearchCNameR2.Text.Trim
                    End If
                    ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

                    'DOB
                    If Me.txtSearchDOBR2.Text.Trim.Equals(String.Empty) Then
                        Me.lblAcctListDOBR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Dim dtDOBValue As DateTime
                        'DOB passed to "chkDOB" must be of DateTime instead of Nullable(of DateTime) 
                        udtSM = Me.udtvalidator.chkDOB(strDocCode, Me.txtSearchDOBR2.Text.Trim, dtDOBValue, strExactDOB)
                        If Not IsNothing(udtSM) Then
                            Me.imgDOBError.Visible = True
                            Me.udcMsgBox.AddMessage(udtSM)
                        Else
                            dtDOB = dtDOBValue
                            ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [Start][Koala]
                            ' -----------------------------------------------------------------------------------------------------------------------------
                            Me.lblAcctListDOBR2.Text = udtformatter.formatDOB(dtDOB, strExactDOB, Session(SESS_Language), Nothing, Nothing)
                            ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [End][Koala]
                        End If
                    End If

                    'eHealth Account ID (Vaildated Account ID)
                    Me.CollapsiblePanelExtenderAccountIDR2.Enabled = False
                    Me.CollapsiblePanelExtenderAccountIDR2.Collapsed = True
                    Me.ibtnAcctListAccountIDR2Multiple.Visible = False
                    Me.txtAcctListAccountIDR2Multiple.Text = String.Empty

                    If Me.txtSearchAccountIDR2.Text.Trim.Equals(String.Empty) Then
                        Me.lblAcctListAccountIDR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else

                        'If udtvalidator.chkSystemNumber(Me.txtSearchAccountIDR2.Text.Trim) Then
                        '    streHSAccountID = Common.Format.Formatter.ReverseSystemNumber(Me.txtSearchAccountIDR2.Text.Trim)
                        'Else
                        '    streHSAccountID = Me.txtSearchAccountIDR2.Text.Trim
                        'End If                            

                        ' Remove Check Digit Before Search 
                        ' ToDo: Check is valid check digit
                        streHSAccountID = Me.txtSearchAccountIDR2.Text.Trim
                        If Not String.IsNullOrEmpty(streHSAccountID) Then
                            arreHSAccountID = streHSAccountID.Split(New Char() {EHAccountIDSeparator}, StringSplitOptions.RemoveEmptyEntries)
                            For i As Integer = 0 To arreHSAccountID.Length - 1
                                arreHSAccountID(i) = arreHSAccountID(i).Substring(0, arreHSAccountID(i).Length - 1)
                            Next
                        End If

                        If arreHSAccountID.Length = 1 Then
                            Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 0)
                        ElseIf arreHSAccountID.Length = 2 Then
                            Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 1)
                        ElseIf arreHSAccountID.Length > 2 Then
                            Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 1)
                            Me.CollapsiblePanelExtenderAccountIDR2.Enabled = True
                            Me.ibtnAcctListAccountIDR2Multiple.Visible = True
                            Me.txtAcctListAccountIDR2Multiple.Text = GetMultipleAccountIDList(arreHSAccountID, 2, arreHSAccountID.Length - 1)
                        End If

                    End If

                    'Ref No. (Temporary Account ID, Special Account ID, Ivalid Account ID)
                    If Me.txtSearchRefNo.Text.Trim.Equals(String.Empty) Then
                        Me.lblAcctListRefNoR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        Me.lblAcctListRefNoR2.Text = Me.txtSearchRefNo.Text.Trim
                        If udtvalidator.chkSystemNumber(Me.txtSearchRefNo.Text.Trim) Then
                            strRefNo = Common.Format.Formatter.ReverseSystemNumber(Me.txtSearchRefNo.Text.Trim)
                        Else
                            strRefNo = Me.txtSearchRefNo.Text.Trim
                        End If
                    End If

                    'Account Type
                    Me.lblAcctListAcctTypeR2.Text = Me.ddlSearchAcctTypeR2.SelectedItem.Text

                    If Me.ddlSearchAcctTypeR2.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary Then
                        If Not String.IsNullOrEmpty(ddlSearchTempAcct.SelectedValue) Then
                            Me.lblAcctListAcctTypeR2.Text = String.Format("{0} ({1})", Me.ddlSearchAcctTypeR2.SelectedItem.Text, Me.ddlSearchTempAcct.SelectedItem.Text)
                        End If
                    End If

                    'Creation Date
                    Dim blnIsValid As Boolean = True
                    Dim strCreationDateFrom As String = String.Empty
                    Dim strCreationDateTo As String = String.Empty

                    If Me.txtSearchCreationDateFromR2.Text.Trim.Equals(String.Empty) And _
                        Me.txtSearchCreationDateToR2.Text.Trim.Equals(String.Empty) Then
                        Me.lblAcctListCreateDateR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        'Creation Date From
                        udtSM = udtvalidator.chkInputDate(Me.txtSearchCreationDateFromR2.Text, True, True)

                        If Not udtSM Is Nothing Then
                            Me.imgDateError.Visible = True
                            Select Case udtSM.MessageCode
                                Case MsgCode.MSG00022
                                    '"Creation Date From" should not be future date.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00139)
                                Case MsgCode.MSG00028
                                    'Please input the "Creation Date From".
                                    Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "CreationDateFrom"))
                                Case MsgCode.MSG00029
                                    '"Creation Date From" is invalid.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00137)
                                Case Else
                                    'Please input the "Creation Date From".
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CreationDateFrom"))
                            End Select

                            blnIsValid = False
                        Else
                            strCreationDateFrom = udtformatter.formatSearchDate(Me.txtSearchCreationDateFromR2.Text.Trim())
                            strCreationDateFrom = udtformatter.convertDate(strCreationDateFrom.Trim(), "en")
                        End If

                        'Creation Date To
                        udtSM = udtvalidator.chkInputDate(Me.txtSearchCreationDateToR2.Text, True, True)

                        If Not udtSM Is Nothing Then
                            Me.imgDateError.Visible = True
                            Select Case udtSM.MessageCode
                                Case MsgCode.MSG00022
                                    '"Creation Date To" should not be future date.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00140)
                                Case MsgCode.MSG00028
                                    'Please input the "Creation Date To".
                                    Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "CreationDateTo"))
                                Case MsgCode.MSG00029
                                    '"Creation Date To" is invalid.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00138)
                                Case Else
                                    'Please input the "Creation Date To".
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CreationDateTo"))
                            End Select

                            blnIsValid = False
                        Else
                            strCreationDateTo = udtformatter.formatSearchDate(Me.txtSearchCreationDateToR2.Text.Trim())
                            strCreationDateTo = udtformatter.convertDate(strCreationDateTo.Trim(), "en")
                        End If
                    End If

                    'If blnIsValid AndAlso Not Me.lblAcctListCreateDateR2.Text.Equals(Me.GetGlobalResourceObject("Text", "Any")) Then
                    If blnIsValid AndAlso Not String.IsNullOrEmpty(strCreationDateFrom) AndAlso Not String.IsNullOrEmpty(strCreationDateTo) Then
                        udtSM = udtvalidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00136, strCreationDateFrom, strCreationDateTo)
                        If Not udtSM Is Nothing Then
                            Me.imgDateError.Visible = True
                            blnIsValid = False
                            Me.udcMsgBox.AddMessage(udtSM)
                        Else
                            lblAcctListCreateDateR2.Text = String.Format("{0} {1} {2}", strCreationDateFrom, Me.GetGlobalResourceObject("Text", "To_S"), strCreationDateTo)
                        End If
                    End If

                    If Me.udcMsgBox.GetCodeTable.Rows.Count = 0 Then
                        blnReturn = True
                    Else
                        'Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00006, AuditLogDesc.SearchRoute2Fail)
                        Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00126, AuditLogDesc.SearchByParticularsFail)
                        blnReturn = False
                    End If
                End If

            Case 1
                'Search Route 3
                Dim strSPID As String = String.Empty
                Dim strManualValidationStatus As String = String.Empty
                Dim dtCreationDate As Nullable(Of DateTime) = Nothing
                Dim strWithClaims As String = String.Empty
                Dim strDeceased As String = String.Empty
                Dim dtDateofDeath As Nullable(Of DateTime) = Nothing
                Dim strAcctType As String = String.Empty

                'Service Provider ID
                If Me.txtSPIDR3.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListSPIDR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListSPIDR3.Text = Me.lblAcctListSPIDR3.Text.Trim
                End If

                'Manual Validation Status
                Me.lblAcctListManualValidStatusR3.Text = Me.ddlManualValidStatusR3.SelectedItem.Text.Trim

                'Creation Date
                Dim blnCDIsValid As Boolean = True
                Dim strCreationDateFrom As String = String.Empty
                Dim strCreationDateTo As String = String.Empty

                If Me.txtCreationDateFromR3.Text.Trim.Equals(String.Empty) And _
                    Me.txtCreationDateToR3.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListCreationDateR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    'Creation Date From
                    udtSM = udtvalidator.chkInputDate(Me.txtCreationDateFromR3.Text, True, True)

                    If Not udtSM Is Nothing Then
                        Me.imgCreationDateErrorR3.Visible = True
                        Select Case udtSM.MessageCode
                            Case MsgCode.MSG00022
                                '"Creation Date From" should not be future date.
                                Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00139)
                            Case MsgCode.MSG00028
                                'Please input the "Creation Date From".
                                Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "CreationDateFrom"))
                            Case MsgCode.MSG00029
                                '"Creation Date From" is invalid.
                                Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00137)
                            Case Else
                                'Please input the "Creation Date From".
                                Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CreationDateFrom"))
                        End Select

                        blnCDIsValid = False
                    Else
                        strCreationDateFrom = udtformatter.formatSearchDate(Me.txtCreationDateFromR3.Text.Trim())
                        strCreationDateFrom = udtformatter.convertDate(strCreationDateFrom.Trim(), "en")
                    End If

                    'Creation Date To
                    udtSM = udtvalidator.chkInputDate(Me.txtCreationDateToR3.Text, True, True)

                    If Not udtSM Is Nothing Then
                        Me.imgCreationDateErrorR3.Visible = True
                        Select Case udtSM.MessageCode
                            Case MsgCode.MSG00022
                                '"Creation Date To" should not be future date.
                                Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00140)
                            Case MsgCode.MSG00028
                                'Please input the "Creation Date To".
                                Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "CreationDateTo"))
                            Case MsgCode.MSG00029
                                '"Creation Date To" is invalid.
                                Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00138)
                            Case Else
                                'Please input the "Creation Date To".
                                Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "CreationDateTo"))
                        End Select

                        blnCDIsValid = False
                    Else
                        strCreationDateTo = udtformatter.formatSearchDate(Me.txtCreationDateToR3.Text.Trim())
                        strCreationDateTo = udtformatter.convertDate(strCreationDateTo.Trim(), "en")
                    End If
                End If

                'If blnCDIsValid AndAlso Not Me.lblAcctListCreationDateR3.Text.Equals(Me.GetGlobalResourceObject("Text", "Any")) Then
                If blnCDIsValid AndAlso Not String.IsNullOrEmpty(strCreationDateFrom) AndAlso Not String.IsNullOrEmpty(strCreationDateTo) Then
                    udtSM = udtvalidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00136, strCreationDateFrom, strCreationDateTo)
                    If Not udtSM Is Nothing Then
                        Me.imgCreationDateErrorR3.Visible = True
                        blnCDIsValid = False
                        Me.udcMsgBox.AddMessage(udtSM)
                    Else
                        lblAcctListCreationDateR3.Text = String.Format("{0} {1} {2}", strCreationDateFrom, Me.GetGlobalResourceObject("Text", "To_S"), strCreationDateTo)
                    End If
                End If

                'With Claims
                If Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(String.Empty) Then
                    'Any
                    lblAcctListWithClaimsR3.Text = ddlWithClaimsR3.SelectedItem.Text.Trim
                ElseIf Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                    'Yes
                    If String.IsNullOrEmpty(ddlSchemeR3.SelectedValue) Then
                        lblAcctListWithClaimsR3.Text = ddlWithClaimsR3.SelectedItem.Text.Trim
                    Else
                        lblAcctListWithClaimsR3.Text = String.Format("{0} ({1})", ddlWithClaimsR3.SelectedItem.Text.Trim, ddlSchemeR3.SelectedValue)
                    End If
                ElseIf Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(YesNo.No) Then
                    'No
                    lblAcctListWithClaimsR3.Text = Me.GetGlobalResourceObject("Text", "No")
                End If

                'Deceased
                If Me.ddlDeceasedR3.SelectedValue.Trim.Equals(String.Empty) Then
                    lblAcctListDeceasedR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                    'lblAcctListDateofDeathR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                ElseIf Me.ddlDeceasedR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                    lblAcctListDeceasedR3.Text = Me.GetGlobalResourceObject("Text", "Yes")

                    'Date of Death
                    Dim blnDateofDeathIsValid As Boolean = True
                    Dim strDateofDeathFrom As String = String.Empty
                    Dim strDateofDeathTo As String = String.Empty

                    If Me.txtDateofDeathFromR3.Text.Trim.Equals(String.Empty) And Me.txtDateofDeathToR3.Text.Trim.Equals(String.Empty) Then
                        Me.lblAcctListDateofDeathR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        'Date of Death From
                        udtSM = udtvalidator.chkInputDate(Me.txtDateofDeathFromR3.Text, True, True)

                        If Not udtSM Is Nothing Then
                            Me.imgDateofDeathErrorR3.Visible = True
                            Select Case udtSM.MessageCode
                                Case MsgCode.MSG00022
                                    '"Date of Death From" should not be future date.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00405)
                                Case MsgCode.MSG00028
                                    'Please input the "Date of Death From.
                                    Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "DateofDeathFrom"))
                                Case MsgCode.MSG00029
                                    '"Date of Death From" is invalid.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00403)
                                Case Else
                                    'Please input the "Date of Death From".
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "DateofDeathFrom"))
                            End Select

                            blnDateofDeathIsValid = False
                        Else
                            strDateofDeathFrom = udtformatter.formatSearchDate(Me.txtDateofDeathFromR3.Text.Trim())
                            strDateofDeathFrom = udtformatter.convertDate(strDateofDeathFrom.Trim(), "en")
                        End If

                        'Date of Death To
                        udtSM = udtvalidator.chkInputDate(Me.txtDateofDeathToR3.Text, True, True)

                        If Not udtSM Is Nothing Then
                            Me.imgDateofDeathErrorR3.Visible = True
                            Select Case udtSM.MessageCode
                                Case MsgCode.MSG00022
                                    '"Date of Death To" should not be future date.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00406)
                                Case MsgCode.MSG00028
                                    'Please input the "Date of Death To".
                                    Me.udcMsgBox.AddMessage(udtSM, "%s", Me.GetGlobalResourceObject("Text", "DateofDeathTo"))
                                Case MsgCode.MSG00029
                                    '"Date of Death To" is invalid.
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00404)
                                Case Else
                                    'Please input the "Date of Death To".
                                    Me.udcMsgBox.AddMessage(FunctCode.FUNT990000, "E", MsgCode.MSG00028, "%s", Me.GetGlobalResourceObject("Text", "DateofDeathTo"))
                            End Select

                            blnDateofDeathIsValid = False
                        Else
                            strDateofDeathTo = udtformatter.formatSearchDate(Me.txtDateofDeathToR3.Text.Trim())
                            strDateofDeathTo = udtformatter.convertDate(strDateofDeathTo.Trim(), "en")
                        End If
                    End If

                    If blnDateofDeathIsValid AndAlso Not String.IsNullOrEmpty(strDateofDeathFrom) AndAlso Not String.IsNullOrEmpty(strDateofDeathTo) Then
                        udtSM = udtvalidator.chkInputValidFromDateCutoffDate(FunctCode.FUNT990000, MsgCode.MSG00402, strDateofDeathFrom, strDateofDeathTo)
                        If Not udtSM Is Nothing Then
                            Me.imgDateofDeathErrorR3.Visible = True
                            blnDateofDeathIsValid = False
                            Me.udcMsgBox.AddMessage(udtSM)
                        Else
                            lblAcctListDateofDeathR3.Text = String.Format("{0} {1} {2}", strDateofDeathFrom, Me.GetGlobalResourceObject("Text", "To_S"), strDateofDeathTo)
                        End If
                    End If
                ElseIf Me.ddlDeceasedR3.SelectedValue.Trim.Equals(YesNo.No) Then
                    lblAcctListDeceasedR3.Text = Me.GetGlobalResourceObject("Text", "No")
                    lblAcctListDateofDeathR3.Text = Me.GetGlobalResourceObject("Text", "Any")

                End If

                'Account Type
                Me.lblAcctListAcctTypeR3.Text = Me.ddlAcctTypeR3.SelectedItem.Text

                If Me.udcMsgBox.GetCodeTable.Rows.Count = 0 Then
                    blnReturn = True
                Else
                    Me.udcMsgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00129, AuditLogDesc.SearchbyManualValidationFail)
                    blnReturn = False
                End If
        End Select

        Return blnReturn
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult
        Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult

        bllSearchResult = Nothing

        'If blnOverrideResultLimit Then
        Select Case Me.tcSearchRoute.ActiveTabIndex
            Case 0
                'Dim strAccountType As String = String.Empty

                '    strAccountType = Me.ddlSearchAcctTypeR1.SelectedValue.Trim

                '    If strAccountType = VRAcctMaintenanceStatus.Temporary Then
                '        strAccountType = EHSAccountModel.SysAccountSourceClass.TemporaryAccount

                '    ElseIf strAccountType = VRAcctMaintenanceStatus.Special Then
                '        strAccountType = EHSAccountModel.SysAccountSourceClass.SpecialAccount
                '    End If

                '    If Me.ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary OrElse _
                '        Me.ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Special Then

                '        Select Case Me.ddlSearchTempAcct.SelectedValue.Trim
                '            Case TempAcctMaintenanceStatus.PendingRemove
                '                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctOustandingListFor29Days(Me.FunctionCode, Me.txtSearchENameR1.Text.Trim, _
                '                                                                            Me.txtSearchCreationDateFromR1.Text.Trim, _
                '                                                                            Me.txtSearchCreationDateToR1.Text.Trim, True)

                '            Case TempAcctMaintenanceStatus.OutstandingValidation
                '                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctOutstandingValidationList(Me.FunctionCode, Me.txtSearchENameR1.Text.Trim, _
                '                                                                            Me.txtSearchCreationDateFromR1.Text.Trim, _
                '                                                                            Me.txtSearchCreationDateToR1.Text.Trim, strAccountType, True)

                '            Case TempAcctMaintenanceStatus.PendingImmdValidation
                '                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctPendingImmdValidationList(Me.FunctionCode, Me.txtSearchENameR1.Text.Trim, _
                '                                                                            Me.txtSearchCreationDateFromR1.Text.Trim, _
                '                                                                            Me.txtSearchCreationDateToR1.Text.Trim, strAccountType, True)

                '            Case Else
                '                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListInRouteOne(Me.FunctionCode, Me.txtSearchENameR1.Text.Trim, _
                '                                                                                               Me.txtSearchCreationDateFromR1.Text.Trim, _
                '                                                                                               Me.txtSearchCreationDateToR1.Text.Trim, _
                '                                                                                               Me.ddlSearchAcctTypeR1.SelectedValue, True)

                '        End Select
                '    Else
                '        bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListInRouteOne(Me.FunctionCode, Me.txtSearchENameR1.Text.Trim, _
                '                                                           Me.txtSearchCreationDateFromR1.Text.Trim, _
                '                                                           Me.txtSearchCreationDateToR1.Text.Trim, _
                '                                                           Me.ddlSearchAcctTypeR1.SelectedValue, True)
                '    End If

                'Case 1
                Dim strExactDOB As String = String.Empty
                Dim dtDOB As Nullable(Of DateTime) = Nothing
                Dim strDocCode As String = String.Empty
                Dim strAdoptionPrefixNum As String = String.Empty
                Dim strIdentityNum As String = String.Empty
                Dim streHSAccountID As String = String.Empty
                Dim arreHSAccountID() As String = Nothing
                Dim strRefNo As String = String.Empty
                Dim strAccountType As String = String.Empty
                Dim strAccountStatus As String = String.Empty
                Dim dtmCreationDateFrom As Nullable(Of DateTime) = Nothing
                Dim dtmCreationDateTo As Nullable(Of DateTime) = Nothing
                Dim strGender As String = String.Empty

                'Doc Type
                Me.lblAcctListDocTypeR2.Text = Me.ddlSearchDocTypeR2.SelectedItem.Text.Trim
                strDocCode = Me.ddlSearchDocTypeR2.SelectedValue.Trim

                'Identity Num
                If Me.txtSearchIdentityNumR2.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListIdentityNumR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListIdentityNumR2.Text = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper

                    Dim strIdentityNumFullTemp As String

                    If strDocCode = DocTypeModel.DocTypeCode.PASS Or strDocCode = DocTypeModel.DocTypeCode.OW Then
                        strIdentityNumFullTemp = Me.txtSearchIdentityNumR2.Text.Trim()
                    ElseIf strDocCode = DocTypeModel.DocTypeCode.DS Then
                        strIdentityNumFullTemp = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper.Replace("(", "").Replace(")", "").Replace("/", "")
                    Else
                        strIdentityNumFullTemp = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")
                    End If

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
                If Me.txtSearchENameR2.Text.Equals(String.Empty) Then
                    Me.lblAcctListENameR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListENameR2.Text = Me.txtSearchENameR2.Text.Trim
                End If

                ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                'Chinese Name
                If Me.txtSearchCNameR2.Text.Equals(String.Empty) Then
                    Me.lblAcctListCNameR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListCNameR2.Text = Me.txtSearchCNameR2.Text.Trim
                End If
                ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

                'DOB
                If Me.txtSearchDOBR2.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListDOBR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Dim dtDOBValue As DateTime
                    'DOB passed to "chkDOB" must be of DateTime instead of Nullable(of DateTime) 
                    udtSM = Me.udtvalidator.chkDOB(strDocCode, Me.txtSearchDOBR2.Text.Trim, dtDOBValue, strExactDOB)
                    If Not IsNothing(udtSM) Then
                        Me.imgDOBError.Visible = True
                        Me.udcMsgBox.AddMessage(udtSM)
                    Else
                        dtDOB = dtDOBValue
                        ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [Start][Koala]
                        ' -----------------------------------------------------------------------------------------------------------------------------
                        Me.lblAcctListDOBR2.Text = udtformatter.formatDOB(dtDOB, strExactDOB, Session(SESS_Language), Nothing, Nothing)
                        ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [End][Koala]
                    End If
                End If

                'eHealth Account ID (Vaildated Account ID)
                Me.CollapsiblePanelExtenderAccountIDR2.Enabled = False
                Me.CollapsiblePanelExtenderAccountIDR2.Collapsed = True
                Me.ibtnAcctListAccountIDR2Multiple.Visible = False
                Me.txtAcctListAccountIDR2Multiple.Text = String.Empty

                If Me.txtSearchAccountIDR2.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListAccountIDR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else

                    'If udtvalidator.chkSystemNumber(Me.txtSearchAccountIDR2.Text.Trim) Then
                    '    streHSAccountID = Common.Format.Formatter.ReverseSystemNumber(Me.txtSearchAccountIDR2.Text.Trim)
                    'Else
                    '    streHSAccountID = Me.txtSearchAccountIDR2.Text.Trim
                    'End If                            

                    ' Remove Check Digit Before Search 
                    ' ToDo: Check is valid check digit
                    streHSAccountID = Me.txtSearchAccountIDR2.Text.Trim
                    If Not String.IsNullOrEmpty(streHSAccountID) Then
                        arreHSAccountID = streHSAccountID.Split(New Char() {EHAccountIDSeparator}, StringSplitOptions.RemoveEmptyEntries)
                        For i As Integer = 0 To arreHSAccountID.Length - 1
                            arreHSAccountID(i) = arreHSAccountID(i).Substring(0, arreHSAccountID(i).Length - 1)
                        Next
                    End If

                    If arreHSAccountID.Length = 1 Then
                        Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 0)
                    ElseIf arreHSAccountID.Length = 2 Then
                        Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 1)
                    ElseIf arreHSAccountID.Length > 2 Then
                        Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 1)
                        Me.CollapsiblePanelExtenderAccountIDR2.Enabled = True
                        Me.ibtnAcctListAccountIDR2Multiple.Visible = True
                        Me.txtAcctListAccountIDR2Multiple.Text = GetMultipleAccountIDList(arreHSAccountID, 2, arreHSAccountID.Length - 1)
                    End If

                End If

                'Ref No. (Temporary Account ID, Special Account ID, Ivalid Account ID)
                If Me.txtSearchRefNo.Text.Trim.Equals(String.Empty) Then
                    Me.lblAcctListRefNoR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    Me.lblAcctListRefNoR2.Text = Me.txtSearchRefNo.Text.Trim
                    If udtvalidator.chkSystemNumber(Me.txtSearchRefNo.Text.Trim) Then
                        strRefNo = Common.Format.Formatter.ReverseSystemNumber(Me.txtSearchRefNo.Text.Trim)
                    Else
                        strRefNo = Me.txtSearchRefNo.Text.Trim
                    End If
                End If

                'Account Type (+ Account Status)
                strAccountType = Me.ddlSearchAcctTypeR2.SelectedValue.Trim
                Me.lblAcctListAcctTypeR2.Text = Me.ddlSearchAcctTypeR2.SelectedItem.Text

                If Me.ddlSearchAcctTypeR2.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary Then
                    If Not String.IsNullOrEmpty(ddlSearchTempAcct.SelectedValue) Then
                        strAccountStatus = Me.ddlSearchTempAcct.SelectedValue.Trim
                        Me.lblAcctListAcctTypeR2.Text = String.Format("{0} ({1})", Me.ddlSearchAcctTypeR2.SelectedItem.Text, Me.ddlSearchTempAcct.SelectedItem.Text)
                    End If
                End If

                'Creation Date
                If Me.txtSearchCreationDateFromR2.Text.Trim.Equals(String.Empty) AndAlso _
                    Me.txtSearchCreationDateToR2.Text.Trim.Equals(String.Empty) Then
                    lblAcctListCreateDateR2.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    If IsDate(udtformatter.convertDate(Me.txtSearchCreationDateFromR2.Text.Trim, "E")) Then
                        dtmCreationDateFrom = udtformatter.convertDate(Me.txtSearchCreationDateFromR2.Text.Trim, "E")
                    End If

                    If IsDate(udtformatter.convertDate(Me.txtSearchCreationDateToR2.Text.Trim, "E")) Then
                        dtmCreationDateTo = udtformatter.convertDate(Me.txtSearchCreationDateToR2.Text.Trim, "E")
                    End If
                    lblAcctListCreateDateR2.Text = String.Format("{0} {1} {2}", _
                                                    udtformatter.convertDate(Me.txtSearchCreationDateFromR2.Text.Trim, String.Empty), _
                                                    Me.GetGlobalResourceObject("Text", "To_S"), _
                                                    udtformatter.convertDate(Me.txtSearchCreationDateToR2.Text.Trim, String.Empty))
                End If

                ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListByParticularMultiple(Me.FunctionCode, strDocCode, strIdentityNum, strAdoptionPrefixNum, Me.txtSearchENameR2.Text.Trim, Me.txtSearchCNameR2.Text.Trim, dtDOB, _
                                                                arreHSAccountID, strRefNo, strGender, _
                                                                strAccountType, strAccountStatus, dtmCreationDateFrom, dtmCreationDateTo, _
                                                                blnOverrideResultLimit)
                ' CRE19-026 (HCVS hotline service) [End][Winnie]

            Case 1  'Manual Validaion Route
                Dim strServiceProviderID As String = String.Empty
                Dim strManualValidaitonStatus As String = String.Empty
                Dim dtmCreationDateFrom As Nullable(Of DateTime) = Nothing
                Dim dtmCreationDateTo As Nullable(Of DateTime) = Nothing
                Dim strWithClaims As String = String.Empty
                Dim strScheme As String = String.Empty
                Dim strDeceased As String = String.Empty
                Dim dtmDateofDeathFrom As Nullable(Of DateTime) = Nothing
                Dim dtmDateofDeathTo As Nullable(Of DateTime) = Nothing
                Dim strAccountType As String = String.Empty
                Dim strUserID As String = String.Empty

                'UserID
                Dim udtHCVUUserBLL As New HCVUUserBLL
                Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser
                If Not IsNothing(udtHCVUUser) Then strUserID = udtHCVUUser.UserID

                'SPID
                strServiceProviderID = Me.txtSPIDR3.Text.Trim
                If Me.txtSPIDR3.Text.Trim.Equals(String.Empty) Then
                    lblAcctListSPIDR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    lblAcctListSPIDR3.Text = strServiceProviderID
                End If

                'Manual Validation
                strManualValidaitonStatus = Me.ddlManualValidStatusR3.SelectedValue.Trim
                Me.lblAcctListManualValidStatusR3.Text = Me.ddlManualValidStatusR3.SelectedItem.Text.Trim 'Dickson

                'Creation Date
                If Me.txtCreationDateFromR3.Text.Trim.Equals(String.Empty) AndAlso _
                    Me.txtCreationDateToR3.Text.Trim.Equals(String.Empty) Then
                    lblAcctListCreationDateR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                Else
                    If IsDate(udtformatter.convertDate(Me.txtCreationDateFromR3.Text.Trim, "E")) Then
                        dtmCreationDateFrom = udtformatter.convertDate(Me.txtCreationDateFromR3.Text.Trim, "E")
                    End If

                    If IsDate(udtformatter.convertDate(Me.txtCreationDateToR3.Text.Trim, "E")) Then
                        dtmCreationDateTo = udtformatter.convertDate(Me.txtCreationDateToR3.Text.Trim, "E")
                    End If
                    lblAcctListCreationDateR3.Text = String.Format("{0} {1} {2}", _
                                                    udtformatter.convertDate(Me.txtCreationDateFromR3.Text.Trim, String.Empty), _
                                                    Me.GetGlobalResourceObject("Text", "To_S"), _
                                                    udtformatter.convertDate(Me.txtCreationDateToR3.Text.Trim, String.Empty))
                End If

                'With Claims (Scheme)
                strWithClaims = Me.ddlWithClaimsR3.SelectedValue.Trim

                If Not Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(String.Empty) Then
                    strScheme = ddlSchemeR3.SelectedValue.Trim
                End If

                If Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(String.Empty) Then
                    'Any
                    lblAcctListWithClaimsR3.Text = ddlWithClaimsR3.SelectedItem.Text.Trim
                ElseIf Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                    'Yes
                    If String.IsNullOrEmpty(ddlSchemeR3.SelectedValue) Then
                        lblAcctListWithClaimsR3.Text = ddlWithClaimsR3.SelectedItem.Text.Trim
                    Else
                        lblAcctListWithClaimsR3.Text = String.Format("{0} ({1})", ddlWithClaimsR3.SelectedItem.Text.Trim, ddlSchemeR3.SelectedValue)
                    End If
                ElseIf Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                    'No
                    lblAcctListWithClaimsR3.Text = Me.GetGlobalResourceObject("Text", "Nil")
                End If

                'Deceased + Date of Death
                strDeceased = Me.ddlDeceasedR3.SelectedValue.Trim

                If Me.ddlDeceasedR3.SelectedValue.Trim.Equals(String.Empty) Then
                    lblAcctListDeceasedR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                    lblAcctListDateofDeathR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                ElseIf Me.ddlDeceasedR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                    lblAcctListDeceasedR3.Text = Me.GetGlobalResourceObject("Text", "Yes")
                ElseIf Me.ddlDeceasedR3.SelectedValue.Trim.Equals(YesNo.No) Then
                    lblAcctListDeceasedR3.Text = Me.GetGlobalResourceObject("Text", "No")
                    lblAcctListDateofDeathR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                End If

                If Me.ddlDeceasedR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                    'No input
                    If Me.txtDateofDeathFromR3.Text.Trim.Equals(String.Empty) AndAlso _
                        Me.txtDateofDeathToR3.Text.Trim.Equals(String.Empty) Then
                        lblAcctListDateofDeathR3.Text = Me.GetGlobalResourceObject("Text", "Any")
                    Else
                        'With value
                        If IsDate(udtformatter.convertDate(Me.txtDateofDeathFromR3.Text.Trim, "E")) Then
                            dtmDateofDeathFrom = udtformatter.convertDate(Me.txtDateofDeathFromR3.Text.Trim, "E")
                        End If

                        If IsDate(udtformatter.convertDate(Me.txtDateofDeathToR3.Text.Trim, "E")) Then
                            dtmDateofDeathTo = udtformatter.convertDate(Me.txtDateofDeathToR3.Text.Trim, "E")
                        End If
                        lblAcctListDateofDeathR3.Text = String.Format("{0} {1} {2}", _
                                                        udtformatter.convertDate(Me.txtDateofDeathFromR3.Text.Trim, String.Empty), _
                                                        Me.GetGlobalResourceObject("Text", "To_S"), _
                                                        udtformatter.convertDate(Me.txtDateofDeathToR3.Text.Trim, String.Empty))
                    End If
                End If

                'Account Type
                strAccountType = ddlAcctTypeR3.SelectedValue.Trim
                Me.lblAcctListAcctTypeR3.Text = Me.ddlAcctTypeR3.SelectedItem.Text

                'bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctManualValidation(Me.FunctionCode, strServiceProviderID, strManualValidaitonStatus, dtmCreationDateFrom, dtmCreationDateTo, strWithClaims, strScheme, strDeceased, dtmDateofDeathFrom, dtmDateofDeathTo, blnOverrideResultLimit)
                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctManualValidation(Me.FunctionCode, _
                                                strServiceProviderID, strManualValidaitonStatus, _
                                                dtmCreationDateFrom, dtmCreationDateTo, _
                                                strWithClaims, strScheme, _
                                                strDeceased, dtmDateofDeathFrom, dtmDateofDeathTo, strAccountType, _
                                                strUserID, _
                                                blnOverrideResultLimit)
        End Select

        'Else
        '    ' blnOverrideResultLimit = False
        '    Select Case Me.tcSearchRoute.ActiveTabIndex
        '        Case 0
        '            Dim strAccountType As String = String.Empty

        '            strAccountType = Me.ddlSearchAcctTypeR1.SelectedValue.Trim

        '            If strAccountType = VRAcctMaintenanceStatus.Temporary Then
        '                strAccountType = EHSAccountModel.SysAccountSourceClass.TemporaryAccount

        '            ElseIf strAccountType = VRAcctMaintenanceStatus.Special Then
        '                strAccountType = EHSAccountModel.SysAccountSourceClass.SpecialAccount
        '            End If

        '            If Me.ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary OrElse _
        '                Me.ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Special Then

        '                Select Case Me.ddlSearchTempAcct.SelectedValue.Trim
        '                    Case TempAcctMaintenanceStatus.PendingRemove
        '                        bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctOustandingListFor29Days(Me.FunctionCode, Me.txtSearchENameR1.Text.Trim, _
        '                                                                                    Me.txtSearchCreationDateFromR1.Text.Trim, _
        '                                                                                    Me.txtSearchCreationDateToR1.Text.Trim)

        '                    Case TempAcctMaintenanceStatus.OutstandingValidation
        '                        bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctOutstandingValidationList(Me.FunctionCode, Me.txtSearchENameR1.Text.Trim, _
        '                                                                                    Me.txtSearchCreationDateFromR1.Text.Trim, _
        '                                                                                    Me.txtSearchCreationDateToR1.Text.Trim, strAccountType)

        '                    Case TempAcctMaintenanceStatus.PendingImmdValidation
        '                        bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctPendingImmdValidationList(Me.FunctionCode, Me.txtSearchENameR1.Text.Trim, _
        '                                                                                    Me.txtSearchCreationDateFromR1.Text.Trim, _
        '                                                                                    Me.txtSearchCreationDateToR1.Text.Trim, strAccountType)

        '                    Case Else
        '                        bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListInRouteOne(Me.FunctionCode, Me.txtSearchENameR1.Text.Trim, _
        '                                                                                                       Me.txtSearchCreationDateFromR1.Text.Trim, _
        '                                                                                                       Me.txtSearchCreationDateToR1.Text.Trim, _
        '                                                                                                       Me.ddlSearchAcctTypeR1.SelectedValue)

        '                End Select
        '            Else
        '                bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListInRouteOne(Me.FunctionCode, Me.txtSearchENameR1.Text.Trim, _
        '                                                                   Me.txtSearchCreationDateFromR1.Text.Trim, _
        '                                                                   Me.txtSearchCreationDateToR1.Text.Trim, _
        '                                                                   Me.ddlSearchAcctTypeR1.SelectedValue)
        '            End If

        '        Case 1
        '            Dim strExactDOB As String = String.Empty
        '            Dim dtDOB As Nullable(Of DateTime) = Nothing
        '            Dim strDocCode As String = String.Empty
        '            Dim strAdoptionPrefixNum As String = String.Empty
        '            Dim strIdentityNum As String = String.Empty
        '            Dim streHSAccountID As String = String.Empty
        '            Dim arreHSAccountID() As String = Nothing
        '            Dim strRefNo As String = String.Empty

        '            'Doc Type
        '            Me.lblAcctListDocTypeR2.Text = Me.ddlSearchDocTypeR2.SelectedItem.Text.Trim
        '            strDocCode = Me.ddlSearchDocTypeR2.SelectedValue.Trim

        '            'Identity Num
        '            If Me.txtSearchIdentityNumR2.Text.Trim.Equals(String.Empty) Then
        '                Me.lblAcctListIdentityNumR2.Text = Me.GetGlobalResourceObject("Text", "Any")
        '            Else
        '                Me.lblAcctListIdentityNumR2.Text = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper

        '                Dim strIdentityNumFullTemp As String
        '                strIdentityNumFullTemp = Me.txtSearchIdentityNumR2.Text.Trim.ToUpper.Replace("-", "").Replace("(", "").Replace(")", "")

        '                Dim strIdentityNumFull() As String
        '                strIdentityNumFull = strIdentityNumFullTemp.Trim.Split("/")
        '                If strIdentityNumFull.Length > 1 Then
        '                    strIdentityNum = strIdentityNumFull(1)
        '                    strAdoptionPrefixNum = strIdentityNumFull(0)
        '                Else
        '                    strIdentityNum = strIdentityNumFullTemp
        '                End If
        '            End If

        '            'English Name
        '            If Me.txtSearchENameR2.Text.Equals(String.Empty) Then
        '                Me.lblAcctListENameR2.Text = Me.GetGlobalResourceObject("Text", "Any")
        '            Else
        '                Me.lblAcctListENameR2.Text = Me.txtSearchENameR2.Text.Trim
        '            End If

        '            'DOB
        '            If Me.txtSearchDOBR2.Text.Trim.Equals(String.Empty) Then
        '                Me.lblAcctListDOBR2.Text = Me.GetGlobalResourceObject("Text", "Any")
        '            Else
        '                Dim dtDOBValue As DateTime
        '                'DOB passed to "chkDOB" must be of DateTime instead of Nullable(of DateTime) 
        '                udtSM = Me.udtvalidator.chkDOB(strDocCode, Me.txtSearchDOBR2.Text.Trim, dtDOBValue, strExactDOB)
        '                If Not IsNothing(udtSM) Then
        '                    Me.imgDOBError.Visible = True
        '                    Me.udcMsgBox.AddMessage(udtSM)
        '                Else
        '                    dtDOB = dtDOBValue
        '                    ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [Start][Koala]
        '                    ' -----------------------------------------------------------------------------------------------------------------------------
        '                    Me.lblAcctListDOBR2.Text = udtformatter.formatDOB(dtDOB, strExactDOB, Session(SESS_Language), Nothing, Nothing)
        '                    ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [End][Koala]
        '                End If
        '            End If

        '            'eHealth Account ID (Vaildated Account ID)
        '            Me.CollapsiblePanelExtenderAccountIDR2.Enabled = False
        '            Me.CollapsiblePanelExtenderAccountIDR2.Collapsed = True
        '            Me.ibtnAcctListAccountIDR2Multiple.Visible = False
        '            Me.txtAcctListAccountIDR2Multiple.Text = String.Empty

        '            If Me.txtSearchAccountIDR2.Text.Trim.Equals(String.Empty) Then
        '                Me.lblAcctListAccountIDR2.Text = Me.GetGlobalResourceObject("Text", "Any")
        '            Else

        '                'If udtvalidator.chkSystemNumber(Me.txtSearchAccountIDR2.Text.Trim) Then
        '                '    streHSAccountID = Common.Format.Formatter.ReverseSystemNumber(Me.txtSearchAccountIDR2.Text.Trim)
        '                'Else
        '                '    streHSAccountID = Me.txtSearchAccountIDR2.Text.Trim
        '                'End If                            

        '                ' Remove Check Digit Before Search 
        '                ' ToDo: Check is valid check digit
        '                streHSAccountID = Me.txtSearchAccountIDR2.Text.Trim
        '                If Not String.IsNullOrEmpty(streHSAccountID) Then
        '                    arreHSAccountID = streHSAccountID.Split(New Char() {EHAccountIDSeparator}, StringSplitOptions.RemoveEmptyEntries)
        '                    For i As Integer = 0 To arreHSAccountID.Length - 1
        '                        arreHSAccountID(i) = arreHSAccountID(i).Substring(0, arreHSAccountID(i).Length - 1)
        '                    Next
        '                End If

        '                If arreHSAccountID.Length = 1 Then
        '                    Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 0)
        '                ElseIf arreHSAccountID.Length = 2 Then
        '                    Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 1)
        '                ElseIf arreHSAccountID.Length > 2 Then
        '                    Me.lblAcctListAccountIDR2.Text = GetMultipleAccountIDList(arreHSAccountID, 0, 1)
        '                    Me.CollapsiblePanelExtenderAccountIDR2.Enabled = True
        '                    Me.ibtnAcctListAccountIDR2Multiple.Visible = True
        '                    Me.txtAcctListAccountIDR2Multiple.Text = GetMultipleAccountIDList(arreHSAccountID, 2, arreHSAccountID.Length - 1)
        '                End If

        '            End If

        '            'Ref No. (Temporary Account ID, Special Account ID, Ivalid Account ID)
        '            If Me.txtSearchRefNo.Text.Trim.Equals(String.Empty) Then
        '                Me.lblAcctListRefNoR2.Text = Me.GetGlobalResourceObject("Text", "Any")
        '            Else
        '                Me.lblAcctListRefNoR2.Text = Me.txtSearchRefNo.Text.Trim
        '                If udtvalidator.chkSystemNumber(Me.txtSearchRefNo.Text.Trim) Then
        '                    strRefNo = Common.Format.Formatter.ReverseSystemNumber(Me.txtSearchRefNo.Text.Trim)
        '                Else
        '                    strRefNo = Me.txtSearchRefNo.Text.Trim
        '                End If
        '            End If

        '            bllSearchResult = udteHSAccountMaintBLL.GeteHSAcctListInRouteTwoMultiple(Me.FunctionCode, strDocCode, strIdentityNum, strAdoptionPrefixNum, Me.txtSearchENameR2.Text.Trim, dtDOB, _

        '                                                  arreHSAccountID, strRefNo)
        '    End Select

        'End If

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
                'Case 0
                '    ' Search Route 1
                '    Me.GridViewDataBind(Me.gvAcctListR1, dt, "IdentityNum", "ASC", False)
                '    Me.pnlSearchCriteriaRoute1.Visible = True
                '    Me.pnlSearchCriteriaRoute2.Visible = False

                'Case 1
                '    ' Search Route 2
                '    Me.GridViewDataBind(Me.gvAcctListR2, dt, "Voucher_Acc_ID", "ASC", False)
                '    Me.pnlSearchCriteriaRoute1.Visible = False
                '    Me.pnlSearchCriteriaRoute2.Visible = True
                Case 0
                    ' Search Route 2
                    'Me.GridViewDataBind(Me.gvAcctListR2, dt, "Voucher_Acc_ID", "ASC", False)
                    Me.GridViewDataBind(Me.gvAcctListR2, dt, "", "ASC", False)
                    Me.pnlSearchCriteriaRoute2.Visible = True
                    Me.pnlSearchCriteriaRoute3.Visible = False

                Case 1
                    ' Search Route 3
                    'Me.GridViewDataBind(Me.gvAcctListR3, dt, "Voucher_Acc_ID", "ASC", False)
                    Me.GridViewDataBind(Me.gvAcctListR3, dt, "", "ASC", False)
                    Me.pnlSearchCriteriaRoute2.Visible = False
                    Me.pnlSearchCriteriaRoute3.Visible = True

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
            'Case 0
            '    ' Search Route 1
            '    udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00001, AuditLogDesc.SearchRoute1)

            'Case 1
            '    udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00004, AuditLogDesc.SearchRoute2)

            Case 0
                'Case 1
                ' Search Route 2
                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00124, AuditLogDesc.SearchByParticulars)

            Case 1
                ' Search Route 3
                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00127, AuditLogDesc.SearchbyManualValidation)

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
                    udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, Common.Component.LogID.LOG00007, AuditLogDesc.SearchFail)
                End If
            Else
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00007, AuditLogDesc.SearchFail)
                Throw eSQL
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00007, AuditLogDesc.SearchFail)
            Throw ex
        End Try

        Select Case Me.tcSearchRoute.ActiveTabIndex
            'Case 0
            '    ' Search Route 1
            '    Select Case enumSearchResult
            '        Case SearchResultEnum.Success
            '            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDesc.SearchRoute1Success)

            '        Case Else
            '            Throw New Exception("Error: Class = [HCVU.eHSAccountEnquiry], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

            '    End Select

            'Case 1
            '    ' Search Route 2
            '    Select Case enumSearchResult
            '        Case SearchResultEnum.Success
            '            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, AuditLogDesc.SearchRoute2Success)

            '        Case Else
            '            Throw New Exception("Error: Class = [HCVU.eHSAccountEnquiry], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

            '    End Select

            Case 0
                ' Search Route 2
                Select Case enumSearchResult
                    Case SearchResultEnum.Success
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00125, AuditLogDesc.SearchByParticularsSuccess)

                    Case Else
                        Throw New Exception("Error: Class = [HCVU.eHSAccountMaint], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

                End Select


            Case 1
                ' Search Route 3
                Select Case enumSearchResult
                    Case SearchResultEnum.Success
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00128, AuditLogDesc.SearchbyManualValidationSuccess)

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
            FunctionCode = FunctCode.FUNT010302
            udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, AuditLogDesc.eHSAccountEnquiryPageLoad)

            ResetControls()

        Else
            udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
            udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)
            Me.SetupInputDocControl(udtEHSAccount, udtEHSAccount_Amendment)
        End If

        'If ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary OrElse ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Special Then
        '    Me.pnlAdvTempSearch.Visible = True
        'Else
        '    Me.pnlAdvTempSearch.Visible = False
        'End If

        If ddlSearchAcctTypeR2.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary Then
            Me.pnlAdvTempSearchR2.Visible = True
        Else
            Me.pnlAdvTempSearchR2.Visible = False
        End If

        If ddlDeceasedR3.SelectedValue = YesNo.Yes Then
            lblDateofDeathFromR3Text.ForeColor = Drawing.Color.Black
            lblDateofDeathToR3Text.ForeColor = Drawing.Color.Black
            txtDateofDeathFromR3.Enabled = True
            txtDateofDeathToR3.Enabled = True
        Else
            lblDateofDeathFromR3Text.ForeColor = Drawing.Color.Gray
            lblDateofDeathToR3Text.ForeColor = Drawing.Color.Gray
            txtDateofDeathFromR3.Enabled = False
            txtDateofDeathToR3.Enabled = False
        End If

        If ddlWithClaimsR3.SelectedValue = YesNo.Yes Then
            ddlSchemeR3.Enabled = True
        Else
            ddlSchemeR3.SelectedIndex = 0
            ddlSchemeR3.Enabled = False
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

        If Not IsPostBack Then
            HandleRedirectAction()
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

    Private Sub HandleRedirectAction()
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As RedirectParameterModel = udtRedirectParameterBLL.GetFromSession()
        If IsNothing(udtRedirectParameter) Then Return

        udtRedirectParameterBLL.RemoveFromSession()
        udtRedirectParameterBLL.WriteAuditLog(FunctionCode, Me, udtRedirectParameter)

        If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.Search) Then
            ' --- Auto-perform Search action ---

            If udtRedirectParameter.EHealthAccountID <> String.Empty Then
                ' Validated account
                Dim udtGeneralFunction As New GeneralFunction
                Dim strEHealthAccountPrefix As String = String.Empty
                udtGeneralFunction.getSytemParameterByParameterName("eHealthAccountPrefix", strEHealthAccountPrefix, String.Empty)
                strEHealthAccountPrefix = strEHealthAccountPrefix.Trim

                'tcSearchRoute.ActiveTabIndex = 1
                tcSearchRoute.ActiveTabIndex = 0
                ddlSearchDocTypeR2.SelectedValue = udtRedirectParameter.EHealthAccountDocCode

                ' Handle multiple account ID
                If udtRedirectParameter.EHealthAccountID.Contains(EHAccountIDSeparator) Then
                    Dim arrAccID() As String = udtRedirectParameter.EHealthAccountID.Split(EHAccountIDSeparator)
                    For i As Integer = 0 To arrAccID.Length - 1
                        arrAccID(i) = String.Format("{0}{1}", arrAccID(i), udtGeneralFunction.generateChkDgt(String.Format("{0}{1}", strEHealthAccountPrefix, arrAccID(i))))
                    Next
                    udtRedirectParameter.EHealthAccountID = Join(arrAccID, EHAccountIDSeparator)
                ElseIf udtRedirectParameter.EHealthAccountID.Trim.Length = 8 Then
                    udtRedirectParameter.EHealthAccountID = String.Format("{0}{1}", udtRedirectParameter.EHealthAccountID, udtGeneralFunction.generateChkDgt(String.Format("{0}{1}", strEHealthAccountPrefix, udtRedirectParameter.EHealthAccountID)))
                End If

                txtSearchAccountIDR2.Text = udtRedirectParameter.EHealthAccountID
                ibtnSearch_Click(Nothing, Nothing)

            ElseIf udtRedirectParameter.EHealthAccountReferenceNo <> String.Empty Then
                ' Temporary account
                Dim udtFormatter As New Formatter

                'tcSearchRoute.ActiveTabIndex = 1
                tcSearchRoute.ActiveTabIndex = 0
                txtSearchRefNo.Text = udtFormatter.formatSystemNumber(udtRedirectParameter.EHealthAccountReferenceNo)
                ibtnSearch_Click(Nothing, Nothing)

            End If

        End If

        If udtRedirectParameter.ActionList.Contains(RedirectParameterModel.EnumRedirectAction.ViewDetail) Then
            ' --- Auto-perform Row Command (click) action ---

            ' Locate the link button for the row command action
            If gvAcctListR2.Rows.Count = 1 Then 'Throw New Exception(String.Format("eHSAccountEnquiry.HandleRedirectAction: Unexpected no. of rows {0}", gvAcctListR2.Rows.Count))

                Dim lbtnAccountID As LinkButton = gvAcctListR2.Rows(0).FindControl("lbtnAccountID")

                Dim arg As New CommandEventArgs(lbtnAccountID.CommandName, lbtnAccountID.CommandArgument)
                Dim e As New GridViewCommandEventArgs(gvAcctListR2.Rows(0), lbtnAccountID, arg)

                gvAcctListR2_RowCommand(gvAcctListR2, e)

                ibtnAccountDetailsBack.Visible = False

            ElseIf gvAcctListR3.Rows.Count = 1 Then
                Dim lbtnAccountID As LinkButton = gvAcctListR3.Rows(0).FindControl("lbtnAccountID")

                Dim arg As New CommandEventArgs(lbtnAccountID.CommandName, lbtnAccountID.CommandArgument)
                Dim e As New GridViewCommandEventArgs(gvAcctListR3.Rows(0), lbtnAccountID, arg)

                gvAcctListR3_RowCommand(gvAcctListR3, e)

                ibtnAccountDetailsBack.Visible = False

            Else
                ibtnSearchResultBack.Visible = False
            End If
        End If

    End Sub

#End Region

#Region "View 1 - Search"

    'Protected Sub ddlSearchAcctTypeR1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary OrElse _
    '        ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Special Then
    '        Me.pnlAdvTempSearch.Visible = True

    '        Me.ddlSearchTempAcct.Items.Clear()

    '        If ddlSearchAcctTypeR1.SelectedValue = VRAcctMaintenanceStatus.Special Then
    '            Dim dv As New DataView(Common.Component.Status.GetDescriptionListFromDBEnumCode(TempAcctMaintenanceStatus.ClassCode, True))
    '            dv.RowFilter = "Status_Value <>'" + TempAcctMaintenanceStatus.PendingRemove.Trim + "' and Status_Value <>'" + TempAcctMaintenanceStatus.OutstandingValidation.Trim + "'"
    '            Me.ddlSearchTempAcct.DataSource = dv
    '        Else
    '            Me.ddlSearchTempAcct.DataSource = Common.Component.Status.GetDescriptionListFromDBEnumCode(TempAcctMaintenanceStatus.ClassCode, True)
    '        End If

    '        Me.ddlSearchTempAcct.DataTextField = "Status_Description"
    '        Me.ddlSearchTempAcct.DataValueField = "Status_Value"
    '        Me.ddlSearchTempAcct.DataBind()

    '        Me.ddlSearchTempAcct.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

    '    Else
    '        Me.pnlAdvTempSearch.Visible = False
    '    End If

    'End Sub

    Protected Sub ddlSearchAcctTypeR2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If ddlSearchAcctTypeR2.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary Then
            Me.pnlAdvTempSearchR2.Visible = True

            Me.ddlSearchTempAcct.Items.Clear()

            Dim dv As New DataView(Common.Component.Status.GetDescriptionListFromDBEnumCode(TempAcctMaintenanceStatusByParticular.ClassCode, True))
            Me.ddlSearchTempAcct.DataSource = dv

            Me.ddlSearchTempAcct.DataTextField = "Status_Description"
            Me.ddlSearchTempAcct.DataValueField = "Status_Value"
            Me.ddlSearchTempAcct.DataBind()

            Me.ddlSearchTempAcct.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))

        Else
            Me.pnlAdvTempSearchR2.Visible = False
        End If

    End Sub

    Protected Sub ddlWithClaimsR3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If ddlWithClaimsR3.SelectedValue.Trim = YesNo.Yes Then
            ddlSchemeR3.Enabled = True
        Else
            ddlSchemeR3.SelectedIndex = 0
            ddlSchemeR3.Enabled = False
        End If
    End Sub

    Protected Sub ddlDeceasedR3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If ddlDeceasedR3.SelectedValue.Trim = YesNo.Yes Then
            lblDateofDeathFromR3Text.ForeColor = Drawing.Color.Black
            txtDateofDeathFromR3.Enabled = True
            lblDateofDeathToR3Text.ForeColor = Drawing.Color.Black
            txtDateofDeathToR3.Enabled = True
            btnDateofDeathFromR3.Enabled = True
            btnDateofDeathToR3.Enabled = True
        Else
            lblDateofDeathFromR3Text.ForeColor = Drawing.Color.Gray
            txtDateofDeathFromR3.Text = String.Empty
            txtDateofDeathFromR3.Enabled = False
            lblDateofDeathToR3Text.ForeColor = Drawing.Color.Gray
            txtDateofDeathToR3.Text = String.Empty
            txtDateofDeathToR3.Enabled = False
            btnDateofDeathFromR3.Enabled = False
            btnDateofDeathToR3.Enabled = False
            imgDateofDeathErrorR3.Visible = False
        End If
    End Sub

    Protected Sub ibtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Me.udcInfoMsgBox.Visible = False
        Me.udcMsgBox.Visible = False
        ClearErrorImage()

        ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------------------------------------------
        ' Implement Collapsible Search Criteria Review
        'udcCollapsibleSearchCriteriaReview1.Collapsed = True
        'udcCollapsibleSearchCriteriaReview1.ClientState = "True"
        udcCollapsibleSearchCriteriaReview2.Collapsed = True
        udcCollapsibleSearchCriteriaReview2.ClientState = "True"
        udcCollapsibleSearchCriteriaReview3.Collapsed = True
        udcCollapsibleSearchCriteriaReview3.ClientState = "True"
        ' CRE12-015 Add the respective practice number in ¡§Practice¡¨ in the functions under ¡§Reimbursement¡¨ in eHS [End][Tommy L]

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------
        Dim enumSearchResult As SearchResultEnum
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

        Try
            Select Case Me.tcSearchRoute.ActiveTabIndex

                Case 0
                    '    ' Search Route 1

                    '    udtAuditLogEntry.AddDescripton("EngName", Me.txtSearchENameR1.Text)
                    '    udtAuditLogEntry.AddDescripton("AccountType", ddlSearchAcctTypeR1.SelectedItem.Text)
                    '    udtAuditLogEntry.AddDescripton("TempAccountType", ddlSearchTempAcct.SelectedItem.Text)
                    '    udtAuditLogEntry.AddDescripton("CreationDateFrom", Me.txtSearchCreationDateFromR1.Text)
                    '    udtAuditLogEntry.AddDescripton("CreationDateTo", Me.txtSearchCreationDateToR1.Text)
                    '    udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00001, AuditLogDesc.SearchRoute1)

                    '    If sender Is Nothing Then
                    '        enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox, False, True)
                    '    Else
                    '        enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox)
                    '    End If

                    '    Select Case enumSearchResult
                    '        Case SearchResultEnum.Success
                    '            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDesc.SearchRoute1Success)

                    '        Case SearchResultEnum.ValidationFail
                    '            ' Audit Log has been handled in [SF_ValidateSearch] method

                    '        Case SearchResultEnum.NoRecordFound
                    '            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDesc.SearchRoute1Success)

                    '        Case SearchResultEnum.OverResultList1stLimit_PopUp
                    '            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDesc.SearchRoute1Fail)

                    '        Case SearchResultEnum.OverResultList1stLimit_Alert
                    '            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDesc.SearchRoute1Fail)

                    '        Case SearchResultEnum.OverResultListOverrideLimit
                    '            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, AuditLogDesc.SearchRoute1Fail)

                    '        Case Else
                    '            Throw New Exception("Error: Class = [HCVU.eHSAccountEnquiry], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

                    '    End Select

                    'Case 1
                    ' Search Route 2
                    udtAuditLogEntry.AddDescripton("EngName", Me.txtSearchENameR2.Text.Trim)
                    ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
                    udtAuditLogEntry.AddDescripton("ChiName", Me.txtSearchCNameR2.Text.Trim)
                    ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

                    udtAuditLogEntry.AddDescripton("DocType", Me.ddlSearchDocTypeR2.SelectedValue)
                    udtAuditLogEntry.AddDescripton("IdentityNumber", Me.txtSearchIdentityNumR2.Text)
                    udtAuditLogEntry.AddDescripton("DOB", Me.txtSearchDOBR2.Text)
                    udtAuditLogEntry.AddDescripton("AccountID", Me.txtSearchAccountIDR2.Text)
                    udtAuditLogEntry.AddDescripton("RefNo", Me.txtSearchRefNo.Text)
                    'udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00004, AuditLogDesc.SearchRoute2)
                    udtAuditLogEntry.AddDescripton("AccountType", ddlSearchAcctTypeR2.SelectedItem.Text)
                    If Me.ddlSearchAcctTypeR2.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary Then
                        udtAuditLogEntry.AddDescripton("Temp Account Type", ddlSearchTempAcct.SelectedItem.Text)
                    Else
                        udtAuditLogEntry.AddDescripton("Temp Account Type", String.Empty)
                    End If
                    udtAuditLogEntry.AddDescripton("CreationDateFrom", Me.txtSearchCreationDateFromR2.Text)
                    udtAuditLogEntry.AddDescripton("CreationDateTo", Me.txtSearchCreationDateToR2.Text)
                    udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00124, AuditLogDesc.SearchByParticulars)

                    If sender Is Nothing Then
                        enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox, False, True)
                    Else
                        enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox)
                    End If

                    Select Case enumSearchResult
                        Case SearchResultEnum.Success
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00125, AuditLogDesc.SearchByParticularsSuccess)

                        Case SearchResultEnum.ValidationFail
                            ' Audit Log has been handled in [SF_ValidateSearch] method

                        Case SearchResultEnum.NoRecordFound
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00125, AuditLogDesc.SearchByParticularsSuccess)

                        Case SearchResultEnum.OverResultList1stLimit_PopUp
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00126, AuditLogDesc.SearchByParticularsFail)

                        Case SearchResultEnum.OverResultList1stLimit_Alert
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00126, AuditLogDesc.SearchByParticularsFail)

                        Case SearchResultEnum.OverResultListOverrideLimit
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00126, AuditLogDesc.SearchByParticularsFail)

                        Case Else
                            Throw New Exception("Error: Class = [HCVU.eHSAccountEnquiry], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

                    End Select
                Case 1
                    ' Search Route 3
                    udtAuditLogEntry.AddDescripton("SPID", Me.txtSPIDR3.Text.Trim)
                    udtAuditLogEntry.AddDescripton("ManualValidationStatus", Me.ddlManualValidStatusR3.SelectedItem.Text)
                    udtAuditLogEntry.AddDescripton("CreationDateFrom", Me.txtCreationDateFromR3.Text)
                    udtAuditLogEntry.AddDescripton("CreationDateTo", Me.txtCreationDateToR3.Text)
                    udtAuditLogEntry.AddDescripton("WithClaims", Me.ddlWithClaimsR3.SelectedItem.Text)
                    If Me.ddlWithClaimsR3.SelectedValue.Trim.Equals(YesNo.Yes) Then
                        udtAuditLogEntry.AddDescripton("Scheme", Me.ddlSchemeR3.SelectedItem.Text)
                    Else
                        udtAuditLogEntry.AddDescripton("Scheme", String.Empty)
                    End If
                    udtAuditLogEntry.AddDescripton("Deceased", Me.ddlDeceasedR3.SelectedItem.Text)
                    udtAuditLogEntry.AddDescripton("DateofDeathFrom", Me.txtDateofDeathFromR3.Text)
                    udtAuditLogEntry.AddDescripton("DateofDeathTo", Me.txtDateofDeathToR3.Text)
                    udtAuditLogEntry.AddDescripton("AccountType", ddlAcctTypeR3.SelectedItem.Text)
                    udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00127, AuditLogDesc.SearchbyManualValidation)

                    If sender Is Nothing Then
                        enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox, False, True)
                    Else
                        enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLogEntry, udcMsgBox, udcInfoMsgBox)
                    End If

                    Select Case enumSearchResult
                        Case SearchResultEnum.Success
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00128, AuditLogDesc.SearchbyManualValidationSuccess)

                        Case SearchResultEnum.ValidationFail
                            ' Audit Log has been handled in [SF_ValidateSearch] method

                        Case SearchResultEnum.NoRecordFound
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00128, AuditLogDesc.SearchbyManualValidationSuccess)

                        Case SearchResultEnum.OverResultList1stLimit_PopUp
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00129, AuditLogDesc.SearchbyManualValidationFail)

                        Case SearchResultEnum.OverResultList1stLimit_Alert
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00129, AuditLogDesc.SearchbyManualValidationFail)

                        Case SearchResultEnum.OverResultListOverrideLimit
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00129, AuditLogDesc.SearchbyManualValidationFail)

                        Case Else
                            Throw New Exception("Error: Class = [HCVU.eHSAccountMaint], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

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
                    udcMsgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, Common.Component.LogID.LOG00007, AuditLogDesc.SearchFail)
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

    '#Region "Gridview Function - gvAcctListR1"

    '    Private Sub gvAcctListR1_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAcctListR1.PageIndexChanging
    '        Me.GridViewPageIndexChangingHandler(sender, e, SESS_Result)
    '    End Sub

    '    Private Sub gvAcctListR1_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAcctListR1.PreRender
    '        Me.GridViewPreRenderHandler(sender, e, SESS_Result)
    '    End Sub

    '    Private Sub gvAcctListR1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAcctListR1.RowCommand
    '        If TypeOf e.CommandSource Is LinkButton Then

    '            txtDocCode.Text = String.Empty

    '            Dim strDocCode As String = String.Empty
    '            Dim strAccountID As String = String.Empty
    '            Dim strAccountSource As String = String.Empty
    '            Dim strPersonalInformationStatus As String = String.Empty
    '            Dim strIdentityNum As String = String.Empty
    '            Dim strSPID As String = String.Empty

    '            Dim blnShowAmendmentRecord As Boolean = False

    '            Dim strCommandArgument As String

    '            strCommandArgument = e.CommandArgument.ToString.Trim
    '            strAccountID = strCommandArgument.Split("|")(0).Trim
    '            strDocCode = strCommandArgument.Split("|")(1).Trim
    '            strAccountSource = strCommandArgument.Split("|")(2).Trim
    '            strPersonalInformationStatus = strCommandArgument.Split("|")(3).Trim
    '            strIdentityNum = strCommandArgument.Split("|")(4).Trim
    '            strSPID = strCommandArgument.Split("|")(5).Trim()

    '            'Audit Log
    '            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
    '            Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
    '            Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
    '            Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
    '            Me.udtAuditLogEntry.AddDescripton("PersonalInformationStatus", strPersonalInformationStatus)
    '            Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
    '            Dim udtAuditLogInfo As AuditLogInfo
    '            udtAuditLogInfo = New AuditLogInfo(strSPID, Nothing, strAccountSource, _
    '                                            strAccountID, strDocCode, Me.udtformatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum))
    '            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00008, AuditLogDesc.SelectEHSAccount, udtAuditLogInfo)

    '            If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.ValidateAccount) Then
    '                If strPersonalInformationStatus.Trim.Equals(EHSAccountModel.PersonalInformationRecordStatusClass.UnderAmendment) Then
    '                    blnShowAmendmentRecord = True
    '                End If
    '            End If

    '            txtDocCode.Text = strDocCode

    '            If Me.GetEHSAccount(strAccountID, strAccountSource, blnShowAmendmentRecord) Then
    '                udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
    '                udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

    '                If blnShowAmendmentRecord Then
    '                    Session(SESS_ActionMode) = ActionModel.ReadOnly_N_Amending
    '                Else
    '                    Session(SESS_ActionMode) = ActionModel.ReadOnly
    '                End If

    '                Me.mveHSAccount.ActiveViewIndex = intAccountDetails

    '                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
    '                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
    '                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
    '                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
    '                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00009, AuditLogDesc.SelectEHSAccountSuccess)
    '            Else
    '                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
    '                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
    '                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
    '                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
    '                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
    '                udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00010, AuditLogDesc.SelectEHSAccountFail)
    '            End If
    '        End If
    '    End Sub

    '    Private Sub gvAcctListR1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctListR1.RowCreated
    '        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
    '        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

    '        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
    '        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(3, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

    '        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    '    End Sub

    '    Private Sub gvAcctListR1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctListR1.RowDataBound

    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
    '            Dim lblCName As Label = CType(e.Row.FindControl("lblCName"), Label)
    '            Dim lbtnIdentityNum As LinkButton = CType(e.Row.FindControl("lbtnIdentityNum"), LinkButton)
    '            Dim lbtnIdentityNumUnmask As LinkButton = CType(e.Row.FindControl("lbtnIdentityNumUnmask"), LinkButton)
    '            Dim lblDOB As Label = CType(e.Row.FindControl("lblDOB"), Label)
    '            Dim lblSex As Label = CType(e.Row.FindControl("lblSex"), Label)
    '            Dim lblCreate_By As Label = CType(e.Row.FindControl("lblCreate_By"), Label)
    '            Dim lblCreateDtm As Label = CType(e.Row.FindControl("lblCreateDtm"), Label)
    '            Dim lblCreate_By_DH As Label = CType(e.Row.FindControl("lblCreate_By_DH"), Label)

    '            Dim strIdentityNum As String = CStr(dr.Item("IdentityNum")).Trim
    '            Dim strVoucherAcctID As String = CStr(dr.Item("Voucher_Acc_ID")).Trim
    '            Dim strSchemeCode As String = CStr(dr.Item("Scheme_Code")).Trim
    '            Dim strChiName As String = CStr(dr.Item("CName")).Trim
    '            Dim dtmDOB As DateTime = CType(dr.Item("DOB"), DateTime)
    '            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB")).Trim
    '            Dim strSex As String = CStr(dr.Item("Sex")).Trim
    '            Dim strSPID As String = CStr(dr.Item("SP_ID")).Trim
    '            Dim strCreate_by As String = CStr(dr.Item("Create_By"))
    '            Dim strSPPracticeDisplaySeq As Integer = CInt(dr.Item("SP_Practice_Display_Seq"))
    '            Dim dtmCreateDtm As DateTime = CType(dr.Item("Create_Dtm"), DateTime)
    '            Dim intAge As Nullable(Of Integer)
    '            Dim dtDOR As Nullable(Of Date)
    '            Dim strDocType As String = CStr(dr.Item("doc_code")).Trim
    '            Dim strAccountSource As String = CStr(dr.Item("Source")).Trim
    '            Dim strAdoptionPrefixNum As String = CStr(dr.Item("Adoption_Prefix_Num")).Trim
    '            Dim strOtherInfo As String

    '            Dim strAccountStatus As String = CStr(dr.Item("Account_Status")).Trim
    '            Dim strPersonalInformationStatus As String = CStr(dr.Item("PersonalInformation_Status")).Trim

    '            If IsDBNull(dr.Item("EC_Age")) Then
    '                intAge = Nothing
    '            Else
    '                intAge = CInt(dr.Item("EC_Age"))
    '            End If

    '            If IsDBNull(dr.Item("EC_Date_of_Registration")) Then
    '                dtDOR = Nothing
    '            Else
    '                dtDOR = CType(dr.Item("EC_Date_of_Registration"), Date)
    '            End If

    '            If IsDBNull(dr.Item("other_info")) Then
    '                strOtherInfo = String.Empty
    '            Else
    '                strOtherInfo = CStr(dr.Item("other_info"))
    '            End If

    '            lbtnIdentityNum.Text = udtformatter.FormatDocIdentityNoForDisplay(strDocType, strIdentityNum, True, strAdoptionPrefixNum)
    '            lbtnIdentityNumUnmask.Text = udtformatter.FormatDocIdentityNoForDisplay(strDocType, strIdentityNum, False, strAdoptionPrefixNum)
    '            lbtnIdentityNum.CommandArgument = strVoucherAcctID & "|" & strDocType & "|" & strAccountSource & "|" & strPersonalInformationStatus & "|" & strIdentityNum & "|" & strSPID
    '            lbtnIdentityNumUnmask.CommandArgument = lbtnIdentityNum.CommandArgument

    '            lblCName.Text = udtformatter.formatChineseName(strChiName.Trim)

    '            lblDOB.Text = udtformatter.formatDOB(strDocType, dtmDOB, strExactDOB, Session(SESS_Language), intAge, dtDOR, strOtherInfo)

    '            lblSex.Text = Me.GetGlobalResourceObject("Text", udtformatter.formatGender(strSex))

    '            lblCreateDtm.Text = udtformatter.formatDateTime(dtmCreateDtm)

    '            'If IsDBNull(dr.Item("Create_By_BO")) Then
    '            '    If strSPID.Trim.Equals(String.Empty) Then
    '            '        lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
    '            '        lblCreate_By_DH.Text = "<br>(Created by DH)"
    '            '    Else
    '            '        lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
    '            '    End If
    '            'Else
    '            '    If CStr(dr.Item("Create_By_BO")).Trim = "Y" Then
    '            '        If strSPID.Trim.Equals(String.Empty) Then
    '            '            lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
    '            '            lblCreate_By_DH.Text = "<br>(Created by DH)"
    '            '        Else
    '            '            lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
    '            '            lblCreate_By_DH.Text = "<br>(Created by " + CStr(dr.Item("Create_By")).Trim + ")"
    '            '        End If
    '            '    Else
    '            '        lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
    '            '    End If
    '            'End If

    '            If Not IsDBNull(dr.Item("Create_By_BO")) Then
    '                'has value
    '                If CStr(dr.Item("Create_By_BO")).Trim = "Y" Then
    '                    lblCreate_By.Text = CStr(dr.Item("Create_By")).Trim
    '                Else
    '                    lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
    '                End If
    '            Else
    '                lblCreate_By.Text = strSPID + "(" + strSPPracticeDisplaySeq.ToString().Trim + ")"
    '            End If

    '            dr.Item("Create_By") = lblCreate_By.Text


    '        End If

    '    End Sub

    '    Private Sub gvAcctListR1_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAcctListR1.Sorting
    '        Me.GridViewSortingHandler(sender, e, SESS_Result)
    '    End Sub

    '#End Region

#Region "Gridview Function - gvAcctListR2"

    Private Sub gvAcctListR2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAcctListR2.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_Result)
    End Sub

    Private Sub gvAcctListR2_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAcctListR2.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_Result)
    End Sub

    Private Sub gvAcctListR2_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAcctListR2.RowCommand
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
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00008, AuditLogDesc.SelectEHSAccount, udtAuditLogInfo)

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
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00009, AuditLogDesc.SelectEHSAccountSuccess)
            Else
                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
                udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00010, AuditLogDesc.SelectEHSAccountFail)
            End If
        End If
    End Sub

    Private Sub gvAcctListR2_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctListR2.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(3, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(4, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvAcctListR2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctListR2.RowDataBound
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

    Private Sub gvAcctListR2_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAcctListR2.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_Result)
    End Sub

#End Region

#Region "Gridview Function - gvAcctListR3"
    Private Sub gvAcctListR3_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAcctListR3.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_Result)
    End Sub

    Private Sub gvAcctListR3_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAcctListR3.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_Result)
    End Sub

    Private Sub gvAcctListR3_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvAcctListR3.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            txtDocCode.Text = String.Empty

            Dim strDocCode As String = String.Empty
            Dim strAccountID As String = String.Empty
            Dim strAccountSource As String = String.Empty
            'Dim strPersonalInformationStatus As String = String.Empty
            Dim strIdentityNum As String = String.Empty
            Dim strSPID As String = String.Empty

            Dim blnShowAmendmentRecord As Boolean = False


            Dim strCommandArgument As String

            strCommandArgument = e.CommandArgument.ToString.Trim
            strAccountID = strCommandArgument.Split("|")(0).Trim
            strDocCode = strCommandArgument.Split("|")(1).Trim
            strAccountSource = strCommandArgument.Split("|")(2).Trim
            'strPersonalInformationStatus = strCommandArgument.Split("|")(3).Trim
            strIdentityNum = strCommandArgument.Split("|")(3).Trim
            strSPID = strCommandArgument.Split("|")(4).Trim()

            'Audit Log
            Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
            Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
            Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
            'Me.udtAuditLogEntry.AddDescripton("PersonalInformationStatus", strPersonalInformationStatus)
            Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
            Dim udtAuditLogInfo As AuditLogInfo
            udtAuditLogInfo = New AuditLogInfo(strSPID, Nothing, strAccountSource, _
                                            strAccountID, strDocCode, Me.udtformatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum))
            Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00008, AuditLogDesc.SelectEHSAccount, udtAuditLogInfo)

            txtDocCode.Text = strDocCode

            'If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.ValidateAccount) Then
            '    If strPersonalInformationStatus.Trim.Equals(EHSAccountModel.PersonalInformationRecordStatusClass.UnderAmendment) Then
            '        blnShowAmendmentRecord = True
            '    End If
            'End If

            If Me.GetEHSAccount(strAccountID, strAccountSource, blnShowAmendmentRecord) Then
                udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
                Me.udtEHSAccount_Amendment = Me.udteHSAccountMaintBLL.EHSAccount_Amend_GetFromSession(FunctionCode)

                If blnShowAmendmentRecord Then
                    Session(SESS_ActionMode) = ActionModel.ReadOnly_N_Amending
                Else
                    Session(SESS_ActionMode) = ActionModel.ReadOnly
                End If

                'Session(SESS_DefaultSetCCCode) = True

                Me.mveHSAccount.ActiveViewIndex = intAccountDetails

                ibtnAccountDetailsBack.Visible = True

                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00009, AuditLogDesc.SelectEHSAccountSuccess)
                udcMsgBox.Clear()
            Else
                udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                Me.udtAuditLogEntry.AddDescripton("AccountID", strAccountID)
                Me.udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
                Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccountSource)
                Me.udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
                udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00010, AuditLogDesc.SelectEHSAccountFail)
            End If
        End If
    End Sub

    Private Sub gvAcctListR3_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctListR3.RowCreated
        Dim udtSortedGVHeaderList As SortedGridviewHeaderModelCollection = New SortedGridviewHeaderModelCollection
        Dim strImageURL As String = Me.GetGlobalResourceObject("ImageURL", "infoicon")

        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(2, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(3, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))
        udtSortedGVHeaderList.Add(New SortedGridviewHeaderModel(4, strImageURL, Me.GetGlobalResourceObject("Text", "Legend")))

        Me.GirdViewRowCreated(sender, e, udtSortedGVHeaderList)
    End Sub

    Private Sub gvAcctListR3_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAcctListR3.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & _
            CType(sender, GridView).ClientID & "','" & _
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).ClientID & "')")
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row
            Dim chkSelect As CheckBox
            Dim lbtnAccountID As LinkButton
            Dim lblIdentityNum As Label
            Dim lblIdentityNumUnmask As Label
            Dim lblCreateDtm As Label
            Dim lblName As Label
            Dim lblCName As Label
            Dim lblDOB As Label
            Dim lblSex As Label
            'Dim lblAccountType As Label
            'Dim lblAccountStatus As Label
            'Dim lblEnquiryStatus As Label
            'Dim lblDateOfIssue As Label
            'Dim lblAmendmentStatus As Label
            Dim lblCreate_By As Label
            Dim lblCreate_By_DH As Label
            Dim lblManualValidationStatus As Label
            'Dim lblScheme As Label
            'Dim lblDeceased As Label
            'Dim lblDateofDeath As Label

            chkSelect = e.Row.FindControl("chkSelect")
            lbtnAccountID = CType(e.Row.FindControl("lbtnAccountID"), LinkButton) ' CRE11-007
            lblIdentityNum = CType(e.Row.FindControl("lblIdentityNum"), Label) ' CRE11-007
            lblIdentityNumUnmask = CType(e.Row.FindControl("lblIdentityNumUnmask"), Label) ' CRE11-007
            lblCreateDtm = CType(e.Row.FindControl("lblCreateDtm"), Label)
            lblName = CType(e.Row.FindControl("lblName"), Label)
            lblCName = CType(e.Row.FindControl("lblCName"), Label)
            lblDOB = CType(e.Row.FindControl("lblDOB"), Label)
            lblSex = CType(e.Row.FindControl("lblSex"), Label)
            'lblDateOfIssue = CType(e.Row.FindControl("lblDateOfIssue"), Label)
            'lblAccountType = CType(e.Row.FindControl("lblAccountType"), Label)
            'lblAccountStatus = CType(e.Row.FindControl("lblAccountStatus"), Label)
            'lblEnquiryStatus = CType(e.Row.FindControl("lblEnquiryStatus"), Label)
            'lblAmendmentStatus = CType(e.Row.FindControl("lblAmendmentStatus"), Label)
            lblCreate_By = CType(e.Row.FindControl("lblCreate_By"), Label)
            lblCreate_By_DH = CType(e.Row.FindControl("lblCreate_By_DH"), Label)
            lblManualValidationStatus = CType(e.Row.FindControl("lblManualValidationStatus"), Label)
            'lblScheme = CType(e.Row.FindControl("lblScheme"), Label)
            'lblDeceased = CType(e.Row.FindControl("lblDeceased"), Label)
            'lblDateofDeath = CType(e.Row.FindControl("lblDateOfDeath"), Label)

            Dim dtmCreateDtm As DateTime = CType(dr.Item("Create_Dtm"), DateTime)
            Dim strEngName As String = CStr(dr.Item("Eng_Name"))
            Dim strChiName As String = CStr(dr.Item("Chi_Name"))
            Dim strIdentityNum As String = CStr(dr.Item("IdentityNum"))
            Dim strVoucherAcctID As String = CStr(dr.Item("Voucher_Acc_ID"))
            Dim strSchemeCode As String = CStr(dr.Item("Scheme_Code"))
            Dim dtmDOB As DateTime = CType(dr.Item("DOB"), DateTime)
            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB"))
            Dim strSex As String = CStr(dr.Item("Sex"))
            'Dim dtmDateOfIssue As Nullable(Of Date)
            Dim strAccountSource As String = CStr(dr.Item("Source"))
            'Dim strAccountStatus As String = CStr(dr.Item("Account_Status"))
            'Dim strEnquiryStatus As String = CStr(dr.Item("Public_Enquiry_Status"))
            'Dim strAmendmentStatus As String = CStr(dr.Item("PersonalInformation_Status"))
            Dim strSPID As String = CStr(dr.Item("SP_ID"))
            Dim strCreate_by As String = CStr(dr.Item("Create_By"))
            Dim strSPPracticeDisplaySeq As Integer = CInt(dr.Item("SP_Practice_Display_Seq"))
            Dim intAge As Nullable(Of Integer)
            Dim dtDOR As Nullable(Of Date)
            'Dim strAcctTypeCode As String = String.Empty
            'Dim strAcctStatusCode As String = String.Empty
            Dim strAdoptionPrefixNum As String = CStr(dr.Item("Adoption_Prefix_Num")).Trim
            Dim strDocCode As String = CStr(dr.Item("Doc_Code")).Trim
            Dim strAccountPurpose As String = CStr(dr.Item("Account_Purpose")).Trim
            Dim strOtherInfo As String
            Dim strManualValidationStatus As String = CStr(dr.Item("ManualValidationStatus"))
            'Dim strScheme As String
            'Dim strDeceased As String = CStr(dr.Item("Deceased"))
            'Dim dtmDOD As DateTime = CType(dr.Item("DOD"), DateTime)

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
            'lbtnAccountID.CommandArgument = strVoucherAcctID & "|" & strDocCode & "|" & strAccountSource & "|" & strAmendmentStatus & "|" & strIdentityNum & "|" & strSPID
            lbtnAccountID.CommandArgument = strVoucherAcctID & "|" & strDocCode & "|" & strAccountSource & "|" & strIdentityNum & "|" & strSPID

            If strAccountSource.Trim = AccountTypeClass.Validated Then
                lbtnAccountID.Text = udtformatter.formatValidatedEHSAccountNumber(strVoucherAcctID.Trim)
                ' CRE11-007 : Checkbox
                chkSelect.Enabled = True
            Else
                lbtnAccountID.Text = udtformatter.formatSystemNumber(strVoucherAcctID.Trim)
                ' CRE11-007 : Checkbox
                chkSelect.Enabled = False
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

            'If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.ValidateAccount) Then
            '    If strAmendmentStatus.Trim.Equals(EHSAccountModel.PersonalInformationRecordStatusClass.UnderAmendment) Then
            '        lblAmendmentStatus.Text = "Under Modification"
            '    Else
            '        lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
            '    End If
            '    Status.GetDescriptionFromDBCode(EHSAccountModel.EnquiryStatusClass.ClassCode, strEnquiryStatus, lblEnquiryStatus.Text, String.Empty)
            'Else
            '    lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
            '    lblEnquiryStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
            'End If

            'Status.GetDescriptionFromDBCode(EHSAccountModel.SysAccountSourceClass.ClassCode, strAccountSource, lblAccountType.Text, String.Empty)

            'If strAccountSource.Trim.Equals(EHSAccountModel.SysAccountSourceClass.TemporaryAccount) Then
            '    If strAccountPurpose.Trim.Equals(EHSAccountModel.AccountPurposeClass.ForAmendmentOld) Then
            '        'CRE13-006 HCVS Ceiling [Start][Karl]
            '        'lblAccountType.Text = "Erased"
            '        lblAccountType.Text = eHealthAccountStatus.Erased_Desc
            '        'CRE13-006 HCVS Ceiling [End][Karl]
            '    End If
            'End If

            'lblAccountStatus.Text = Me.udteHSAccountMaintBLL.getAcctStatus(strAccountStatus, strAccountSource)

            'Manual Validation
            Status.GetDescriptionFromDBCode(TempAcctMaintenanceStatusByManualValidation.ClassCode, strManualValidationStatus, lblManualValidationStatus.Text, String.Empty)

            ''Scheme_claim
            'If IsDBNull(dr.Item("Scheme_Claim")) Then
            '    lblScheme.Text = String.Empty
            'Else
            '    lblScheme.Text = CStr(dr.Item("Scheme_Claim")).Trim
            'End If

            ''Deceased
            'If strDeceased = YesNo.Yes Then
            '    lblDeceased.Text = Me.GetGlobalResourceObject("Text", "Yes")
            'ElseIf strDeceased = YesNo.No Then
            '    lblDeceased.Text = Me.GetGlobalResourceObject("Text", "No")
            'End If

            ''Date of Death
            'If IsDBNull(dr.Item("DOD")) Then
            '    lblDateofDeath.Text = String.Empty
            'Else
            '    lblDateofDeath.Text = udtformatter.formatDateTime(CType(dr.Item("DOD"), DateTime))
            'End If

        End If
    End Sub

    Private Sub gvAcctListR3_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAcctListR3.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_Result)
    End Sub
#End Region

    Protected Sub ibtnSearchResultBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.hfIsRedirect.Value = False

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00020, AuditLogDesc.BackToSearch)

        'Me.ResetControls()

        Me.mveHSAccount.ActiveViewIndex = intSearchView
    End Sub

#End Region

#Region "View 3 - Account Details"

    Protected Sub ibtnAccountDetailsBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00021, AuditLogDesc.BackToResultList)

        Me.mveHSAccount.ActiveViewIndex = intSearchResult
        Me.ucInputDocumentType.Clear()
        udteHSAccountMaintBLL.EHSAccountRemoveFromSession(FunctionCode)
    End Sub

    Protected Sub ibtnAmendHistory_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcInfoMsgBox.Clear()

        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountType", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00011, AuditLogDesc.GetAmendmentHistory)

        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel
        udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

        Dim dt As DataTable

        Try
            dt = Me.udteHSAccountMaintBLL.GetAmendmentHistory(udtEHSAccount.VoucherAccID, Me.txtDocCode.Text.Trim)

            If dt.Rows.Count = 0 Then
                udtSM = New Common.ComObject.SystemMessage(CommonFunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
                Me.udcInfoMsgBox.AddMessage(udtSM)
                Me.udcInfoMsgBox.BuildMessageBox()
                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information

                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                Me.udtAuditLogEntry.AddDescripton("AccountType", udtEHSAccount.AccountSourceString)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00013, AuditLogDesc.GetAmendmentHistorySuccessNoRecordFound)
            Else
                Me.gvAmendHistory.DataSource = dt
                Me.gvAmendHistory.DataBind()

                Session(SESS_AmendHistory) = dt

                Me.GridViewDataBind(Me.gvAmendHistory, dt, "Amend_Dtm", "DESC", False)

                Me.gvAmendHistory.Columns(6).Visible = False 'EC_Serial_No
                Me.gvAmendHistory.Columns(7).Visible = False 'EC_Reference_No
                Me.gvAmendHistory.Columns(8).Visible = False 'Foreign_Passport_No
                Me.gvAmendHistory.Columns(9).Visible = False 'PASS_Issue_Region      
                Me.gvAmendHistory.Columns(10).Visible = False 'Permit_To_Remain_Until
                Me.gvAmendHistory.Columns(11).Visible = False 'HKIC - Create By SmartID

                Select Case txtDocCode.Text.Trim
                    Case DocType.DocTypeModel.DocTypeCode.EC
                        Me.gvAmendHistory.Columns(6).Visible = True 'EC_Serial_No
                        Me.gvAmendHistory.Columns(7).Visible = True 'EC_Reference_No
                    Case DocType.DocTypeModel.DocTypeCode.ID235B
                        Me.gvAmendHistory.Columns(10).Visible = True 'Permit_To_Remain_Until
                    Case DocType.DocTypeModel.DocTypeCode.VISA
                        Me.gvAmendHistory.Columns(8).Visible = True 'Foreign_Passport_No
                    Case DocType.DocTypeModel.DocTypeCode.HKIC
                        Me.gvAmendHistory.Columns(11).Visible = True 'HKIC - Create By SmartID
                    Case DocType.DocTypeModel.DocTypeCode.PASS
                        Me.gvAmendHistory.Columns(9).Visible = True 'PASS_Issue_Region
                End Select


                Me.mveHSAccount.ActiveViewIndex = intAmendmentHistroy

                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                Me.udtAuditLogEntry.AddDescripton("AccountType", udtEHSAccount.AccountSourceString)
                Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dt.Rows.Count)
                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00012, AuditLogDesc.GetAmendmentHistorySuccess)
            End If
        Catch ex As Exception
            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
            Me.udtAuditLogEntry.AddDescripton("AccountType", udtEHSAccount.AccountSourceString)
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00014, AuditLogDesc.GetAmendmentHistoryFail)
            Throw
        End Try

    End Sub

    Protected Sub ibtnSchemeInfo_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcInfoMsgBox.Clear()

        udtEHSAccount = Me.udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        Me.udtAuditLogEntry.AddDescripton("AccountType", udtEHSAccount.AccountSourceString)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00015, AuditLogDesc.GetSchemeInfo)

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        'Dim udtSubsidizeFeeModel As Scheme.SubsidizeFeeModel
        'Dim dblSubsidizeFee As Double
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]

        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel
        udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

        Dim udtSchemeClaimBLL As Scheme.SchemeClaimBLL = New Scheme.SchemeClaimBLL

        Dim udtSchemeClaimList As Scheme.SchemeClaimModelCollection

        udtSchemeClaimList = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithSubsidizeGroup

        Dim dtTransaction As DataTable

        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL

        dtTransaction = udtEHSTransactionBLL.getTransactionDetailBenefitDataTable(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum)

        ' CRE13-021-02 Add-on Voucher (2014 Jan) [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Dim dvTransaction As New DataView(dtTransaction)
        ' CRE13-021-02 Add-on Voucher (2014 Jan) [End][Tommy L]

        Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL

        Dim udtGF As New Common.ComFunction.GeneralFunction
        Dim dtmCurrent As DateTime = udtGF.GetSystemDateTime

        ' ---------------------------
        ' INT10-001: Cross Year Fix
        ' ---------------------------
        Dim dtmCurrentDate As DateTime = dtmCurrent.Date
        ' ---------------------------
        ' End of INT10-001
        ' ---------------------------

        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Dim intColWidth_1 As Integer = 200
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        Dim blnEligible As Boolean = False
        'Dim intVoucher As Integer
        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Dim intTotalGrantVoucher As Integer
        'Dim intUsedVoucher As Integer
        Dim intAvailableVoucher As Integer
        ' CRE13-006 - HCVS Ceiling [End][Tommy L]

        Dim dblPerValue As Double = Nothing

        'CRE14-018 Suspend annual allotment for deceased eHealth Account [Start][Winnie]
        Dim blnIsDead As Boolean = False
        'CRE14-018 Suspend annual allotment for deceased eHealth Account [End][Winnie]

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Dim blnVoucherIncluded As Boolean = False
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        Try
            For Each udtSchemeClaim As Scheme.SchemeClaimModel In udtSchemeClaimList
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

                    blnEligible = (New ClaimRules.ClaimRulesBLL).CheckEligibleForClaimVoucherPerSeason(udtCloneSchemeClaim.SchemeCode, udtEHSPersonalInfo, dtmEndDate, True, Nothing)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    dvTransaction.RowFilter = "Scheme_Code='" + udtCloneSchemeClaim.DisplayCode.Trim + "'"
                    'dvTransaction.RowFilter = "Scheme_Code='" + udtCloneSchemeClaim.SchemeCode.Trim + "'"
                    ' CRE19-026 (HCVS hotline service) [End][Winnie]

                    'If blnEligible Then
                    If blnEligible OrElse dvTransaction.Count > 0 Then
                        ' CRE13-021-02 Add-on Voucher (2014 Jan) [End][Tommy L]

                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, VoucherInfoModel.AvailableQuota.Include)

                        udtVoucherInfo.GetInfo(dtmCurrentDate, udtCloneSchemeClaim, udtEHSPersonalInfo)

                        udtVoucherInfo.FunctionCode = FunctionCode

                        intAvailableVoucher = udtVoucherInfo.GetAvailableVoucher()
                        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

                        If intAvailableVoucher < 0 Then
                            intAvailableVoucher = 0
                        End If

                        Dim hr As New HtmlGenericControl
                        hr.TagName = "hr"
                        hr.Attributes.Add("style", "width: 99%; color: #ff8080; border-top-style: none; border-right-style: none;border-left-style: none; height: 1px;")
                        pnlScheme.Controls.Add(hr)

                        Dim br As New HtmlGenericControl
                        br.TagName = "br"
                        pnlScheme.Controls.Add(br)

                        Dim lbl As New Label
                        lbl.Text = Me.GetGlobalResourceObject("Text", "VoucherUtilization") ' CRE13-019-02 Extend HCVS to China [Lawrence]
                        lbl.Font.Bold = True
                        pnlScheme.Controls.Add(lbl)

                        pnlScheme.Controls.Add(br)
                        pnlScheme.Controls.Add(br)

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

                        Dim collapsiblePanelExtender As New AjaxControlToolkit.CollapsiblePanelExtender()

                        Dim img As Image
                        Dim pnl As Panel
                        Dim gvw As GridView

                        rw = New TableRow

                        cll = New TableCell
                        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                        ' -----------------------------------------------------------------------------------------
                        'cll.Width = 150
                        cll.Width = intColWidth_1
                        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
                        cll.Text = Me.GetGlobalResourceObject("Text", "AvailableVoucher")
                        cll.VerticalAlign = VerticalAlign.Middle
                        rw.Controls.Add(cll)

                        cll = New TableCell
                        cll.VerticalAlign = VerticalAlign.Middle
                        lbl = New Label

                        ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                        ' ----------------------------------------------------------------------------------------
                        'gvw = GetGridView_VoucherUsage(udtCloneSchemeClaim, udtEHSPersonalInfo, dblSubsidizeFee, strAlertHtmlImg_Tag_Result, strDeadPeriodVoucherAmount)
                        gvw = GetGridView_VoucherUsage(udtCloneSchemeClaim, udtEHSPersonalInfo, udtVoucherInfo, strAlertHtmlImg_Tag_Result, strDeadPeriodVoucherAmount, udtDeadPeriodVoucherQuota)
                        ' CRE19-003 (Opt voucher capping) [End][Winnie]


                        'Display N/A for deceased account
                        blnIsDead = udtEHSPersonalInfo.Deceased

                        If Not blnIsDead Then
                            ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
                            'lbl.Text = udtformatter.formatMoney(CStr(CInt(intAvailableVoucher * dblSubsidizeFee)), True)
                            lbl.Text = udtformatter.formatMoney(CStr(intAvailableVoucher), True)
                            ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]
                        Else

                            If intAvailableVoucher <= 0 Then
                                lbl.Text = Me.GetGlobalResourceObject("Text", "N/A")
                            Else
                                ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
                                'lbl.Text = udtformatter.formatMoney(CStr(CInt(strDeadPeriodVoucherAmount * dblSubsidizeFee)), True) & " as of Date of Death"
                                lbl.Text = udtformatter.formatMoney(CStr(strDeadPeriodVoucherAmount), True) & " as of Date of Death"
                                ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]
                            End If

                        End If

                        cll.Controls.Add(lbl)
                        rw.Controls.Add(cll)

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
                            rw = New TableRow

                            cll = New TableCell
                            cll.Width = intColWidth_1
                            cll.Text = String.Format(Me.GetGlobalResourceObject("Text", "ProfessionQuota"), Me.GetGlobalResourceObject("Text", udtVoucherQuota.ProfCode))
                            cll.VerticalAlign = VerticalAlign.Middle
                            rw.Controls.Add(cll)

                            cll = New TableCell
                            cll.VerticalAlign = VerticalAlign.Middle
                            lbl = New Label

                            lbl.Text = (New Common.Format.Formatter).formatMoney(IIf(udtVoucherQuota.AvailableQuota > 0, udtVoucherQuota.AvailableQuota, 0), True)
                            lbl.Text += " " + String.Format(Me.GetGlobalResourceObject("Text", "Upto") _
                                                      , udtVoucherQuota.PeriodEndDtm.ToString("dd-MM-yyyy"))

                            cll.Controls.Add(lbl)
                            rw.Controls.Add(cll)

                            tbl.Controls.Add(rw)
                        End If

                        ' Voucher Usage
                        rw = New TableRow

                        cll = New TableCell
                        cll.Width = intColWidth_1
                        cll.Text = Me.GetGlobalResourceObject("Text", "VoucherUsage")
                        cll.VerticalAlign = VerticalAlign.Top
                        rw.Controls.Add(cll)

                        ' Voucher Usage Button
                        cll = New TableCell
                        img = New Image
                        img.ID = "imgCollapsiblePanelController_v4_HCVS"
                        img.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ShowVoucherUsageSBtn")
                        img.ImageAlign = ImageAlign.AbsMiddle
                        img.Attributes.Add("onmouseover", "this.style.cursor='pointer';")
                        cll.Controls.Add(img)
                        rw.Controls.Add(cll)

                        tbl.Controls.Add(rw)
                        Me.pnlScheme.Controls.Add(tbl)

                        ' Voucher Usage Panel
                        pnl = New Panel
                        pnl.ID = "pnlVoucherUsage"

                        tbl = New Table
                        rw = New TableRow

                        cll = New TableCell
                        cll.Width = intColWidth_1
                        cll.VerticalAlign = VerticalAlign.Top

                        rw.Controls.Add(cll)
                        ' CRE19-003 (Opt voucher capping) [End][Winnie]

                        cll = New TableCell
                        cll.VerticalAlign = VerticalAlign.Top

                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]

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

                        pnl.Controls.Add(tbl)

                        Me.pnlScheme.Controls.Add(pnl)

                        collapsiblePanelExtender.ID = "collapsiblePanelExtender_v4"
                        collapsiblePanelExtender.CollapseControlID = "imgCollapsiblePanelController_v4_HCVS"
                        collapsiblePanelExtender.ExpandControlID = "imgCollapsiblePanelController_v4_HCVS"
                        collapsiblePanelExtender.ImageControlID = "imgCollapsiblePanelController_v4_HCVS"
                        collapsiblePanelExtender.TargetControlID = "pnlVoucherUsage"
                        collapsiblePanelExtender.AutoCollapse = False
                        collapsiblePanelExtender.AutoExpand = False
                        collapsiblePanelExtender.Collapsed = True
                        collapsiblePanelExtender.CollapsedSize = 0
                        collapsiblePanelExtender.ExpandDirection = AjaxControlToolkit.CollapsiblePanelExpandDirection.Vertical
                        collapsiblePanelExtender.ScrollContents = False
                        collapsiblePanelExtender.CollapsedImage = Me.GetGlobalResourceObject("ImageUrl", "ShowVoucherUsageSBtn")
                        collapsiblePanelExtender.ExpandedImage = Me.GetGlobalResourceObject("ImageUrl", "HideVoucherUsageSBtn")
                        collapsiblePanelExtender.ClientState = "True"

                        Me.pnlScheme.Controls.Add(collapsiblePanelExtender)

                        'rw = New TableRow

                        'cll = New TableCell
                        'cll.Width = 150
                        'cll.Text = Me.GetGlobalResourceObject("Text", "NoOfVouchers")
                        'rw.Controls.Add(cll)

                        'cll = New TableCell
                        'cll.Width = 150
                        'cll.Text = "(" + Me.GetGlobalResourceObject("Text", "Entitle") + ")"
                        'rw.Controls.Add(cll)

                        'cll = New TableCell
                        'cll.Text = intTotalGrantVoucher.ToString
                        'cll.CssClass = "tableText"
                        'rw.Controls.Add(cll)
                        'tbl.Controls.Add(rw)

                        'rw = New TableRow

                        'cll = New TableCell
                        'cll.Width = 150
                        'cll.Text = Me.GetGlobalResourceObject("Text", "NoOfVouchers")
                        'rw.Controls.Add(cll)

                        'cll = New TableCell
                        'cll.Width = 150
                        'cll.Text = "(" + Me.GetGlobalResourceObject("Text", "Used") + ")"
                        'rw.Controls.Add(cll)

                        'cll = New TableCell
                        'cll.Text = intUsedVoucher.ToString
                        'cll.CssClass = "tableText"
                        'rw.Controls.Add(cll)
                        'tbl.Controls.Add(rw)

                        'Me.pnlScheme.Controls.Add(tbl)
                        ' CRE13-006 - HCVS Ceiling [End][Tommy L]

                        'auidt log
                        If dtTransaction.Rows.Count = 0 Then
                            'auidt log
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("SchemeCode", udtCloneSchemeClaim.SchemeCode.Trim)
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00017, AuditLogDesc.GetSchemeInfoSuccessNoRecordFound)
                        Else
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("SchemeCode", udtCloneSchemeClaim.SchemeCode.Trim)
                            Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtTransaction.Rows.Count)
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00016, AuditLogDesc.GetSchemeInfoSuccess)
                        End If
                    Else
                        If dtTransaction.Rows.Count = 0 Then
                            udtSM = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVI, MsgCode.MSG00001)

                            'auidt log
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("SchemeCode", udtCloneSchemeClaim.SchemeCode.Trim)
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00017, AuditLogDesc.GetSchemeInfoSuccessNoRecordFound)
                            Exit For
                        Else
                            'auidt log
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("SchemeCode", udtCloneSchemeClaim.SchemeCode.Trim)
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00018, AuditLogDesc.GetSchemeInfoSuccessNotEligible)
                        End If
                    End If
                Else
                    If dtTransaction.Rows.Count > 0 Then
                        Dim dv As New DataView(dtTransaction)

                        ' CRE20-0XX (HA Scheme) [Start][Winnie]
                        ' Hide HA Scheme 
                        If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = Common.Component.Scheme.SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeType_HAService Then
                            Continue For
                        End If
                        ' CRE20-0XX (HA Scheme) [End][Winnie]

                        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                        ' -----------------------------------------------------------------------------------------
                        'dv.RowFilter = "Scheme_Code='" + udtSchemeClaim.SchemeCode.Trim + "'"
                        dv.RowFilter = "Scheme_Code='" + udtSchemeClaim.DisplayCode.Trim + "'"
                        ' CRE19-026 (HCVS hotline service) [End][Winnie]

                        If dv.Count > 0 Then
                            ' CRE13-021-02 Add-on Voucher (2014 Jan) [Start][Tommy L]
                            ' -----------------------------------------------------------------------------------------
                            ' Add a dummy image to fix the display problem of collapsible panel
                            Dim imgDummy As New Image
                            imgDummy.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "Transparent")
                            pnlScheme.Controls.Add(imgDummy)
                            ' CRE13-021-02 Add-on Voucher (2014 Jan) [End][Tommy L]

                            Dim hr As New HtmlGenericControl
                            hr.TagName = "hr"
                            hr.Attributes.Add("style", "width: 99%; color: #ff8080; border-top-style: none; border-right-style: none;border-left-style: none; height: 1px;")
                            pnlScheme.Controls.Add(hr)

                            Dim br As New HtmlGenericControl
                            br.TagName = "br"
                            pnlScheme.Controls.Add(br)

                            Dim lbl As New Label
                            lbl.Text = udtSchemeClaim.DisplayCode.Trim
                            lbl.Font.Bold = True
                            pnlScheme.Controls.Add(lbl)

                            pnlScheme.Controls.Add(br)
                            pnlScheme.Controls.Add(br)

                            Dim tbl As New Table
                            Dim rw As TableRow
                            Dim cll As TableCell

                            rw = New TableRow

                            cll = New TableCell
                            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                            ' -----------------------------------------------------------------------------------------
                            'cll.Width = 150
                            cll.Width = intColWidth_1
                            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
                            cll.Text = "Received Subsidy"
                            cll.VerticalAlign = VerticalAlign.Top
                            rw.Controls.Add(cll)

                            cll = New TableCell

                            Dim gv As GridView

                            'CRE13-001 EHAPP [Start][Karl]
                            '-------------------------------------------------------------------------------------
                            Dim blnShowDoseColumn As Boolean
                            blnShowDoseColumn = True
                            ' CRE20-0XX (HA Scheme) [Start][Winnie]
                            If udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = Common.Component.Scheme.SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeRegistration OrElse _
                                udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeType = Common.Component.Scheme.SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeType_HAService Then
                                ' CRE20-0XX (HA Scheme) [End][Winnie]
                                blnShowDoseColumn = False
                            End If

                            'gv = GetGridviewColumn(dv)
                            gv = GetGridviewColumn(dv, blnShowDoseColumn)
                            'CRE13-001 EHAPP [End][Karl]

                            cll.Controls.Add(gv)
                            rw.Controls.Add(cll)
                            tbl.Controls.Add(rw)
                            Me.pnlScheme.Controls.Add(tbl)

                            'audit log
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("SchemeCode", udtSchemeClaim.SchemeCode.Trim)
                            Me.udtAuditLogEntry.AddDescripton("NoOfRecord", dtTransaction.Rows.Count)
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00016, AuditLogDesc.GetSchemeInfoSuccess)
                        End If
                    Else
                        If dtTransaction.Rows.Count = 0 Then
                            udtSM = New Common.ComObject.SystemMessage(FuncCode, SeverityCode.SEVI, MsgCode.MSG00001)

                            'audit log
                            Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                            Me.udtAuditLogEntry.AddDescripton("SchemeCode", udtSchemeClaim.SchemeCode.Trim)
                            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00017, AuditLogDesc.GetSchemeInfoSuccessNoRecordFound)
                            Exit For
                        End If
                    End If
                End If

            Next

            If IsNothing(udtSM) Then
                Me.mveHSAccount.ActiveViewIndex = intSchemeInfo
            Else
                Me.udcInfoMsgBox.AddMessage(udtSM)
                Me.udcInfoMsgBox.BuildMessageBox()
                Me.udcInfoMsgBox.Type = CustomControls.InfoMessageBoxType.Information
            End If

            ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]
            Dim FootBr As New LiteralControl("<br/>")
            pnlScheme.Controls.Add(FootBr)

            Dim Foottbl As New Table
            Dim Footrw As TableRow
            Dim Footcll As TableCell

            Footrw = New TableRow
            Footcll = New TableCell

            Footcll.Text = Me.GetGlobalResourceObject("Text", "SchemeInfoBaseOnIDDoc")
            Footcll.VerticalAlign = VerticalAlign.Top
            Footrw.Controls.Add(Footcll)
            Foottbl.Controls.Add(Footrw)
            Me.pnlScheme.Controls.Add(Foottbl)
            ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]



        Catch ex As Exception
            If Not IsNothing(udtEHSAccount) Then
                Me.udtAuditLogEntry.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                Me.udtAuditLogEntry.AddDescripton("AccountType", udtEHSAccount.AccountSourceString.Trim)
            End If
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00019, AuditLogDesc.GetSchemeInfoFail)
            Throw
        End Try

    End Sub

    Protected Sub ibtnVaccinationRecord_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00024, AuditLogDesc.VaccinationRecordClick)

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        ' INT18-0033 (Fix selection of document for vaccination record enquiry in back-office platform) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim udtEHSAccountClone As EHSAccount.EHSAccountModel = Nothing
        Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = Nothing

        'Clone EHSAccount
        udtEHSAccountClone = New EHSAccountModel(udtEHSAccount)
        udtEHSPersonalInfo = udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

        udtEHSAccountClone.SetPersonalInformation(udtEHSPersonalInfo)
        udtEHSAccountClone.SetSearchDocCode(Me.txtDocCode.Text.Trim)

        'udcVaccinationRecord.Build(udtEHSAccount, New AuditLogEntry(FuncCode, Me), True)
        udcVaccinationRecord.Build(udtEHSAccountClone, New AuditLogEntry(FuncCode, Me), True)

        ' INT18-0033 (Fix selection of document for vaccination record enquiry in back-office platform) [End][Chris YIM]

        mveHSAccount.ActiveViewIndex = intVaccinationRecord

    End Sub

    Protected Sub ibtnManagement_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtEHSAccount As EHSAccountModel = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        Dim udtRedirectParameter As New RedirectParameterModel

        If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
            udtRedirectParameter.EHealthAccountID = udtEHSAccount.VoucherAccID
            udtRedirectParameter.EHealthAccountDocCode = udtEHSAccount.EHSPersonalInformationList(0).DocCode
        Else
            udtRedirectParameter.EHealthAccountReferenceNo = udtEHSAccount.VoucherAccID
        End If

        udtRedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
        udtRedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)

        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        udtRedirectParameterBLL.SaveToSession(udtRedirectParameter)

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        RedirectHandler.ToURL((New Component.Menu.MenuBLL).GetURLByFunctionCode(FunctCode.FUNT010301))
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

    End Sub

#End Region

#Region "View 4 - Amendment History"

    Protected Sub ibtnAmendmentHistoryBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Audit Log
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00022, AuditLogDesc.BackToAccDetailFromAmendmentHist)

        Me.mveHSAccount.ActiveViewIndex = intAccountDetails
    End Sub

#Region "Gridview Function - gvAmendHistory"
    Private Sub gvAmendHistory_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvAmendHistory.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, SESS_AmendHistory)
    End Sub

    Private Sub gvAmendHistory_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAmendHistory.PreRender
        Me.GridViewPreRenderHandler(sender, e, SESS_AmendHistory)
    End Sub

    Private Sub gvAmendHistory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAmendHistory.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            Dim lblAmendDtm As Label
            Dim lblName As Label
            Dim lblCName As Label
            Dim lblDOB As Label
            Dim lblSex As Label
            Dim lblDateOfIssue As Label
            Dim lblUpdate_By As Label
            Dim lblAmendmentStatus As Label
            Dim lblECSN As Label
            Dim lblECRef As Label
            Dim lblForeignPassportNo As Label
            Dim lblPermitToRemainUntil As Label
            Dim lblCreationMethod As Label
            Dim lblPASS_Issue_Region As Label


            lblAmendDtm = CType(e.Row.FindControl("lblAmendDtm"), Label)
            lblName = CType(e.Row.FindControl("lblName"), Label)
            lblCName = CType(e.Row.FindControl("lblCName"), Label)
            lblDOB = CType(e.Row.FindControl("lblDOB"), Label)
            lblSex = CType(e.Row.FindControl("lblSex"), Label)
            lblDateOfIssue = CType(e.Row.FindControl("lblDateOfIssue"), Label)
            lblUpdate_By = CType(e.Row.FindControl("lblUpdate_By"), Label)
            lblAmendmentStatus = CType(e.Row.FindControl("lblAmendmentStatus"), Label)

            lblECSN = CType(e.Row.FindControl("lblECSN"), Label)
            lblECRef = CType(e.Row.FindControl("lblECRef"), Label)

            lblForeignPassportNo = CType(e.Row.FindControl("lblForeignPassportNo"), Label)
            lblPermitToRemainUntil = CType(e.Row.FindControl("lblPermitToRemainUntil"), Label)

            lblCreationMethod = CType(e.Row.FindControl("lblCreationMethod"), Label)

            lblPASS_Issue_Region = CType(e.Row.FindControl("lblPASS_Issue_Region"), Label)

            Dim dtmAmendDtm As DateTime = CType(dr.Item("Amend_Dtm"), DateTime)
            Dim strEngName As String = CStr(dr.Item("Eng_Name"))
            Dim strChiName As String = CStr(dr.Item("Chi_Name"))

            Dim dtmDOB As DateTime = CType(dr.Item("DOB"), DateTime)
            Dim strExactDOB As String = CStr(dr.Item("Exact_DOB"))
            Dim strSex As String = CStr(dr.Item("Sex"))
            Dim dtmDateOfIssue As DateTime '= CType(dr.Item("Date_of_Issue"), DateTime)
            Dim strUpdate_By As String = CStr(dr.Item("Update_By"))
            Dim strAmendmentStatus As String = CStr(dr.Item("SubmitToVerify"))
            Dim strRecord_status As String = dr.Item("Record_Status").ToString.Trim
            Dim strDocCode As String = dr.Item("doc_code").ToString.Trim

            Dim intAge As Nullable(Of Integer)
            Dim dtDOR As Nullable(Of Date)
            Dim dtmPermitToRemainUntil As Nullable(Of Date)
            Dim strOtherInfo As String

            Dim strCreateBySmartID As String

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

            If IsDBNull(dr.Item("Date_of_Issue")) Then
                dtmDateOfIssue = Nothing
            Else
                dtmDateOfIssue = CType(dr.Item("Date_of_Issue"), Date)
            End If

            Dim strSN As String
            If IsDBNull(dr.Item("EC_Serial_No")) Then
                strSN = String.Empty
            Else
                strSN = CStr(dr.Item("EC_Serial_No")).Trim
            End If

            Dim strRef As String
            If IsDBNull(dr.Item("EC_Reference_No")) Then
                strRef = String.Empty
            Else
                strRef = CStr(dr.Item("EC_Reference_No")).Trim
            End If

            Dim strForeignPassportNo
            If IsDBNull(dr.Item("Foreign_Passport_No")) Then
                strForeignPassportNo = String.Empty
            Else
                strForeignPassportNo = CStr(dr.Item("Foreign_Passport_No")).Trim
            End If

            Dim strPASSIssueRegion
            If IsDBNull(dr.Item("PASS_Issue_Region")) Then
                strPASSIssueRegion = String.Empty
            Else
                strPASSIssueRegion = CStr(dr.Item("PASS_Issue_Region")).Trim
            End If

            If IsDBNull(dr.Item("Permit_To_Remain_Until")) Then
                dtmPermitToRemainUntil = Nothing
            Else
                dtmPermitToRemainUntil = CType(dr.Item("Permit_To_Remain_Until"), Date)
            End If

            If IsDBNull(dr.Item("other_info")) Then
                strOtherInfo = String.Empty
            Else
                strOtherInfo = CStr(dr.Item("other_info"))
            End If

            If IsDBNull(dr.Item("Create_By_SmartID")) Then
                strCreateBySmartID = String.Empty
            Else
                strCreateBySmartID = CStr(dr.Item("Create_By_SmartID"))

                If strCreateBySmartID.Equals("Y") Then
                    strCreateBySmartID = Me.GetGlobalResourceObject("Text", "SmartIC")
                Else
                    strCreateBySmartID = Me.GetGlobalResourceObject("Text", "ManualInput")
                End If
            End If

            lblAmendDtm.Text = udtformatter.formatDateTime(dtmAmendDtm)
            lblName.Text = strEngName
            lblCName.Text = udtformatter.formatChineseName(strChiName.Trim)

            lblDOB.Text = udtformatter.formatDOB(strDocCode, dtmDOB, strExactDOB, Session(SESS_Language), intAge, dtDOR, strOtherInfo)

            lblSex.Text = Me.GetGlobalResourceObject("Text", udtformatter.formatGender(strSex))
            lblUpdate_By.Text = strUpdate_By

            lblDateOfIssue.Text = udtformatter.formatDOI(strDocCode, dtmDateOfIssue)
            If lblDateOfIssue.Text.Trim.Equals(String.Empty) Then
                lblDateOfIssue.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            lblECSN.Text = strSN
            lblECRef.Text = udtformatter.formatReferenceNo(strRef, False)

            lblForeignPassportNo.Text = strForeignPassportNo


            If (IsNothing(strPASSIssueRegion.Trim) Or strPASSIssueRegion.Trim.Equals(String.Empty)) Then
                lblPASS_Issue_Region.Text = Me.GetGlobalResourceObject("Text", "NotProvided")
            Else
                lblPASS_Issue_Region.Text = udtPassportIssueRegionBLL.GetPassportIssueRegion.Filter(strPASSIssueRegion.Trim).NationalDesc
            End If

            If Not IsNothing(dtmPermitToRemainUntil) Then
                lblPermitToRemainUntil.Text = udtformatter.formatID235BPermittedToRemainUntil(dtmPermitToRemainUntil)
            End If

            lblCreationMethod.Text = strCreateBySmartID

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Get the Record_status wording 
            If strRecord_status = "V" Then
                lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "PendingValidation")
            ElseIf strRecord_status = "A" Then
                lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
            ElseIf strRecord_status = "C" Then
                'lblAmendmentStatus.Text = "Current Record"
                lblAmendmentStatus.Text = Me.GetGlobalResourceObject("Text", "LatestMergedRecord")
            End If
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        End If
    End Sub

    Private Sub gvAmendHistory_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvAmendHistory.Sorting
        Me.GridViewSortingHandler(sender, e, SESS_AmendHistory)
    End Sub
#End Region

#End Region

#Region "View 5 - Scheme Information"

    Protected Sub ibtnSchemeInfoBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00023, AuditLogDesc.BackToAccDetailFromSchemeInfo)

        Me.mveHSAccount.ActiveViewIndex = intAccountDetails
    End Sub

#End Region

#Region "View 6 - Vaccination Record"

    Protected Sub ibtnVaccinationRecordBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry = New AuditLogEntry(FuncCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00025, AuditLogDesc.BackToAccDetailFromVaccinationRecord)

        mveHSAccount.ActiveViewIndex = intAccountDetails
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
                'chkMaskDocumentNoR1.Checked = True
                chkMaskDocumentNoR2.Checked = True
                chkMaskDocumentNoR3.Checked = True
                'chkMaskDocumentNoR1.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes
                chkMaskDocumentNoR2.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes
                chkMaskDocumentNoR3.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes
                'Me.gvAcctListR1.Columns(2).Visible = True
                'Me.gvAcctListR1.Columns(3).Visible = False

                Me.gvAcctListR2.Columns(3).Visible = True
                Me.gvAcctListR2.Columns(4).Visible = False
                Me.gvAcctListR3.Columns(3).Visible = True
                Me.gvAcctListR3.Columns(4).Visible = False
            Case intAccountDetails
                Me.udcInfoMsgBox.Clear()
                SetAccountBtn()
                Me.SetupInputDocControl(udtEHSAccount, udtEHSAccount_Amendment)

            Case intAmendmentHistroy
                'By Paul, added on 22/9 (Trigger the binding action on readonly control in Amendment History page)
                Me.udcInfoMsgBox.Clear()
                Me.udcInfoMsgBox.Clear()
                Me.SetupInputDocControl(udtEHSAccount, udtEHSAccount_Amendment)

            Case intSchemeInfo
                'By Paul, added on 22/9 (Trigger the binding action on readonly control in Scheme Info page)
                Me.udcInfoMsgBox.Clear()
                Me.udcInfoMsgBox.Clear()
                Me.SetupInputDocControl(udtEHSAccount, udtEHSAccount_Amendment)

            Case intVaccinationRecord
                udcMsgBox.Clear()
                udcInfoMsgBox.Clear()
                SetupInputDocControl(udtEHSAccount, Nothing)

        End Select
    End Sub

#End Region

#Region "Set Controls"

    Private Sub ResetControls()
        'Me.ddlSearchAcctTypeR1.Items.Clear()
        'Me.ddlSearchAcctTypeR1.DataSource = Common.Component.Status.GetDescriptionListFromDBEnumCode(VRAcctMaintenanceStatus.ClassCode, True)
        'Me.ddlSearchAcctTypeR1.DataTextField = "Status_Description"
        'Me.ddlSearchAcctTypeR1.DataValueField = "Status_Value"
        'Me.ddlSearchAcctTypeR1.DataBind()

        'ddlSearchAcctTypeR1.SelectedIndex = 0

        'Route 2
        Me.ddlSearchAcctTypeR2.Items.Clear()
        Me.ddlSearchAcctTypeR2.DataSource = Common.Component.Status.GetDescriptionListFromDBEnumCode(VRAcctMaintenanceStatus.ClassCode, True)
        Me.ddlSearchAcctTypeR2.DataTextField = "Status_Description"
        Me.ddlSearchAcctTypeR2.DataValueField = "Status_Value"
        Me.ddlSearchAcctTypeR2.DataBind()
        Me.ddlSearchAcctTypeR2.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlSearchAcctTypeR2.SelectedIndex = 0

        Me.ddlSearchTempAcct.Items.Clear()

        'If ddlSearchAcctTypeR1.SelectedValue = VRAcctMaintenanceStatus.Special Then
        '    Dim dv As New DataView(Common.Component.Status.GetDescriptionListFromDBEnumCode(TempAcctMaintenanceStatus.ClassCode, True))
        '    dv.RowFilter = "Status_Value <>'" + TempAcctMaintenanceStatus.PendingRemove.Trim + "' and Status_Value <>'" + TempAcctMaintenanceStatus.OutstandingValidation.Trim + "'"
        '    Me.ddlSearchTempAcct.DataSource = dv
        'Else
        '    Me.ddlSearchTempAcct.DataSource = Common.Component.Status.GetDescriptionListFromDBEnumCode(TempAcctMaintenanceStatus.ClassCode, True)
        'End If
        Me.ddlSearchTempAcct.DataSource = Common.Component.Status.GetDescriptionListFromDBEnumCode(TempAcctMaintenanceStatusByParticular.ClassCode, True)

        Me.ddlSearchTempAcct.DataTextField = "Status_Description"
        Me.ddlSearchTempAcct.DataValueField = "Status_Value"
        Me.ddlSearchTempAcct.DataBind()
        Me.ddlSearchTempAcct.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlSearchTempAcct.SelectedIndex = 0

        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Me.ddlSearchDocTypeR2.Items.Clear()
        Me.ddlSearchDocTypeR2.DataSource = udtDocTypeBLL.getAllDocType
        Me.ddlSearchDocTypeR2.DataTextField = "DocName"
        Me.ddlSearchDocTypeR2.DataValueField = "DocCode"
        Me.ddlSearchDocTypeR2.DataBind()
        Me.ddlSearchDocTypeR2.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlSearchDocTypeR2.SelectedIndex = 0

        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        'txtSearchENameR1.Text = String.Empty
        'txtSearchCreationDateFromR1.Text = String.Empty
        'txtSearchCreationDateToR1.Text = String.Empty
        txtSearchENameR2.Text = String.Empty
        txtSearchCNameR2.Text = String.Empty
        txtSearchCreationDateFromR2.Text = String.Empty
        txtSearchCreationDateToR2.Text = String.Empty
        txtSearchIdentityNumR2.Text = String.Empty
        txtSearchDOBR2.Text = String.Empty
        txtSearchAccountIDR2.Text = String.Empty
        txtSearchRefNo.Text = String.Empty
        ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

        'If ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary OrElse ddlSearchAcctTypeR1.SelectedValue.Trim = VRAcctMaintenanceStatus.Special Then
        '    Me.pnlAdvTempSearch.Visible = True
        'Else
        '    Me.pnlAdvTempSearch.Visible = False
        'End If

        If ddlSearchAcctTypeR2.SelectedValue.Trim = VRAcctMaintenanceStatus.Temporary Then
            Me.pnlAdvTempSearchR2.Visible = True
        Else
            Me.pnlAdvTempSearchR2.Visible = False
        End If

        'Route 3(Manual Validation)
        'SPID
        txtSPIDR3.Text = String.Empty

        'Creation Date
        txtCreationDateFromR3.Text = String.Empty
        txtCreationDateToR3.Text = String.Empty

        'Manual Validation
        ddlManualValidStatusR3.Items.Clear()
        ddlManualValidStatusR3.DataSource = Common.Component.Status.GetDescriptionListFromDBEnumCode(TempAcctMaintenanceStatusByManualValidation.ClassCode, True)
        ddlManualValidStatusR3.DataTextField = "Status_Description"
        ddlManualValidStatusR3.DataValueField = "Status_Value"
        ddlManualValidStatusR3.DataBind()
        ddlManualValidStatusR3.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        ddlManualValidStatusR3.SelectedIndex = 0

        Dim udtStaticDataList As StaticData.StaticDataModelCollection = (New StaticData.StaticDataBLL).GetStaticDataListByColumnName("YesNo")

        'WithClaims
        ddlWithClaimsR3.DataSource = udtStaticDataList
        ddlWithClaimsR3.DataValueField = "ItemNo"
        ddlWithClaimsR3.DataTextField = "DataValue"
        ddlWithClaimsR3.DataBind()
        ddlWithClaimsR3.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlWithClaimsR3.SelectedIndex = 0

        'Scheme
        Dim udtSchemeClaimModelListFilter As New SchemeClaimModelCollection
        Dim udtUserRoleCollection As UserRole.UserRoleModelCollection = (New UserRole.UserRoleBLL).GetUserRoleCollection((New HCVUUser.HCVUUserBLL).GetHCVUUser.UserID)
        Dim udtSchemeCList As SchemeClaimModelCollection = (New SchemeClaimBLL).getAllDistinctSchemeClaim()

        For Each udtSchemeC As SchemeClaimModel In udtSchemeCList
            For Each udtUserRoleModel As UserRole.UserRoleModel In udtUserRoleCollection.Values
                If udtUserRoleModel.SchemeCode.Trim = udtSchemeC.SchemeCode Then
                    If Not udtSchemeClaimModelListFilter.Contains(udtSchemeC) Then udtSchemeClaimModelListFilter.Add(udtSchemeC)
                End If
            Next
        Next
        ddlSchemeR3.DataSource = udtSchemeClaimModelListFilter
        ddlSchemeR3.DataValueField = "SchemeCode"
        ddlSchemeR3.DataTextField = "DisplayCode"
        ddlSchemeR3.DataBind()

        txtSPIDR3.Text = String.Empty
        txtCreationDateFromR3.Text = String.Empty
        txtCreationDateToR3.Text = String.Empty


        ' Set the scheme list to disabled if only 1 scheme
        If udtSchemeClaimModelListFilter.Count = 1 Then
            ddlSchemeR3.SelectedIndex = 1
            'ddlSchemeR3.Enabled = False
        Else
            ddlSchemeR3.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
            ddlSchemeR3.SelectedIndex = 0
            'ddlSchemeR3.Enabled = True
        End If

        'Deceased
        ddlDeceasedR3.DataSource = udtStaticDataList
        ddlDeceasedR3.DataValueField = "ItemNo"
        ddlDeceasedR3.DataTextField = "DataValue"
        ddlDeceasedR3.DataBind()
        ddlDeceasedR3.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlDeceasedR3.SelectedIndex = 0

        'Date of Death
        btnDateofDeathFromR3.Enabled = False
        btnDateofDeathToR3.Enabled = False
        txtDateofDeathFromR3.Text = String.Empty
        txtDateofDeathToR3.Text = String.Empty

        'Account Type R3
        Me.ddlAcctTypeR3.Items.Clear()
        Dim dtAcctTypeR3 As DataTable = Common.Component.Status.GetDescriptionListFromDBEnumCode(VRAcctMaintenanceStatus.ClassCode, True)
        Me.ddlAcctTypeR3.DataSource = dtAcctTypeR3.AsEnumerable().Where(Function(r) _
                                    r.Field(Of String)("Status_Value") = VRAcctMaintenanceStatus.Temporary Or _
                                    r.Field(Of String)("Status_Value") = VRAcctMaintenanceStatus.Special).CopyToDataTable
        Me.ddlAcctTypeR3.DataTextField = "Status_Description"
        Me.ddlAcctTypeR3.DataValueField = "Status_Value"
        Me.ddlAcctTypeR3.DataBind()
        Me.ddlAcctTypeR3.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), ""))
        Me.ddlAcctTypeR3.SelectedIndex = 0


        ' eHealth Account Prefix
        Dim strParm1 As String = String.Empty
        Dim strParm2 As String = String.Empty
        If udtCommonFunction.getSystemParameter("eHealthAccountPrefix", strParm1, strParm2) Then
            lblSearchAccountIDR2Prefix.Text = strParm1
        Else
            Throw New ArgumentNullException("Parameter: eHealthAccountPrefix not found")
        End If

        ' CRE11-007
        ' eHealth Account ID length
        Me.txtSearchAccountIDR2.MaxLength = 10 * Me.udtCommonFunction.GetPageSize()
    End Sub

    Private Sub SetAccountBtn()
        Me.ibtnAmendHistory.Visible = False
        Me.ibtnSchemeInfo.Enabled = False
        Me.ibtnSchemeInfo.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SchemeInformationDisbaleBtn")

        Me.pnlTransactionInfo.Visible = False
        Me.pnlAmendingSmartID.Visible = False

        udtEHSAccount = udteHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)
        Dim udteHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
        udteHSAccountPersonalInfo = udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

        Dim udtSchemeClaimBLL As Scheme.SchemeClaimBLL = New Scheme.SchemeClaimBLL

        Dim udtSchemeClaimList As Scheme.SchemeClaimModelCollection

        udtSchemeClaimList = udtSchemeClaimBLL.getAllEffectiveSchemeClaim_WithSubsidizeGroup

        Dim dtTransaction As DataTable

        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL

        dtTransaction = udtEHSTransactionBLL.getTransactionDetailBenefitDataTable(udteHSAccountPersonalInfo.DocCode, udteHSAccountPersonalInfo.IdentityNum)

        If dtTransaction.Rows.Count > 0 Then
            Me.ibtnSchemeInfo.Enabled = True
            Me.ibtnSchemeInfo.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SchemeInformationBtn")

        End If

        Select Case udtEHSAccount.AccountSource
            Case EHSAccountModel.SysAccountSource.ValidateAccount
                ' Amendment History Btn (Only Show when account is validated)
                Me.ibtnAmendHistory.Visible = True

                '==================================================================== Code for SmartID ============================================================================
                If Me.udteHSAccountMaintBLL.IsAmendingBySmartID(udtEHSAccount.VoucherAccID, udteHSAccountPersonalInfo.DocCode) Then
                    Me.pnlAmendingSmartID.Visible = False
                Else
                    Me.pnlAmendingSmartID.Visible = False
                End If
                '==================================================================================================================================================================

            Case EHSAccountModel.SysAccountSource.TemporaryAccount
                'Amendment History Btn
                Me.ibtnAmendHistory.Visible = False


                'If udtEHSAccount.FirstValidateDtm.HasValue Then
                'If DateDiff(DateInterval.Day, udtEHSAccount.FirstValidateDtm.Value, Now, Microsoft.VisualBasic.FirstDayOfWeek.Sunday, FirstWeekOfYear.System) > 28 Then

                'End If

                'End If

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                pnlTransactionInfo.Visible = True
                If udtEHSAccount.TransactionID IsNot Nothing Then
                    If Not udtEHSAccount.TransactionID.Trim.Equals(String.Empty) Then

                        Me.lblTransactionID.Text = udtformatter.formatSystemNumber(udtEHSAccount.TransactionID)
                    Else
                        Me.lblTransactionID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                    End If
                Else
                    Me.lblTransactionID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

            Case EHSAccountModel.SysAccountSource.SpecialAccount

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                pnlTransactionInfo.Visible = True
                If udtEHSAccount.TransactionID IsNot Nothing Then
                    If Not udtEHSAccount.TransactionID.Trim.Equals(String.Empty) Then

                        Me.lblTransactionID.Text = udtformatter.formatSystemNumber(udtEHSAccount.TransactionID)
                    Else
                        Me.lblTransactionID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                    End If
                Else
                    Me.lblTransactionID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

            Case Else
                Me.ibtnAmendHistory.Visible = False
        End Select



        ' Management: Enable when have access right
        If (New HCVUUserBLL).GetHCVUUser().AccessRightCollection.Item(FunctCode.FUNT010301).Allow Then
            ibtnManagement.Enabled = True
            ibtnManagement.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ManagementBtn")
        Else
            ibtnManagement.Enabled = False
            ibtnManagement.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ManagementDisableBtn")
        End If

    End Sub

    Private Sub ClearSession(ByVal blnClearResultListSession As Boolean)

        If blnClearResultListSession Then
            Session(SESS_Result) = Nothing
            Session.Remove(SESS_Result)
        End If

        Session(SESS_ActionMode) = Nothing
        Session.Remove(SESS_ActionMode)

        Session(SESS_AmendHistory) = Nothing
        Session.Remove(SESS_AmendHistory)

        udteHSAccountMaintBLL.EHSAccountRemoveFromSession(FunctionCode)
        udteHSAccountMaintBLL.EHSAccount_Amend_RemoveFromSession(FunctionCode)

        txtDocCode.Text = String.Empty


    End Sub

    Private Sub ClearErrorImage()
        Me.udcMsgBox.Clear()
        Me.udcInfoMsgBox.Clear()

        'Route 2
        Me.imgSearchAccountIDR2Error.Visible = False
        Me.imgDOBError.Visible = False
        Me.imgDateError.Visible = False
        Me.imgSearchAccountIDR2Error.Visible = False

        'Route 3
        Me.imgSPIDErrorR3.Visible = False
        Me.imgCreationDateErrorR3.Visible = False
        Me.imgDateofDeathErrorR3.Visible = False
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

    ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
    ' -----------------------------------------------------------------------------------------
    'Private Function GetGridView_VoucherUsage(ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal dblSubsidizeFee As Double, ByRef strAlertHtmlImg_Tag_Result As String, ByRef strDeadPeriodVoucherAmount As Integer) As GridView
    Private Function GetGridView_VoucherUsage(ByVal udtSchemeClaim As SchemeClaimModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtVoucherInfo As VoucherInfoModel, ByRef strAlertHtmlImg_Tag_Result As String, ByRef strDeadPeriodVoucherAmount As Integer, ByRef udtDeadPeriodVoucherQuota As VoucherQuotaModel) As GridView
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]

        Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL
        Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL
        Dim udtVoucherRefundBLL As New VoucherRefundBLL
        Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ' Use VoucherInfo instead
        'Dim dicTotalUsedVoucherHCVS As Dictionary(Of Integer, Integer) = udtEHSTransactionBLL.getTotalUsedVoucherBySchemeSubsidize(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, SchemeClaimModel.HCVS, udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode)
        'Dim dicTotalUsedVoucherHCVSC As Dictionary(Of Integer, Integer) = udtEHSTransactionBLL.getTotalUsedVoucherBySchemeSubsidize(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, SchemeClaimModel.HCVSCHN, udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode)

        'Dim udtSubsidizeWriteOffList_Full As SubsidizeWriteOffModelCollection = udtSubsidizeWriteOffBLL.GetSubsidizeWriteOffList( _
        '                                                                            udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, _
        '                                                                            udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, _
        '                                                                            udtEHSPersonalInfo.DOD, udtEHSPersonalInfo.ExactDOD, _
        '                                                                            udtSchemeClaim.SchemeCode, udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode, _
        '                                                                            eHASubsidizeWriteOff_CreateReason.TxEnquiry)

        'Dim udtVoucherRefundList_Full As VoucherRefundModelCollection = udtVoucherRefundBLL.GetVoucherRefundByDocID(udtEHSPersonalInfo.IdentityNum)
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]

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
                If blnThisPeriodIsDead Then
                    dro(strColName_Age) = strDeathHtmlImg_Tag & " " & udtSubsidizeGroupClaim.ClaimPeriodFrom.Year - udtEHSPersonalInfo.DOB.Year
                    intDeadAge = udtSubsidizeGroupClaim.ClaimPeriodFrom.Year - udtEHSPersonalInfo.DOB.Year
                Else
                    dro(strColName_Age) = udtSubsidizeGroupClaim.ClaimPeriodFrom.Year - udtEHSPersonalInfo.DOB.Year
                End If
            Else
                dro(strColName_Age) = intDeadAge
            End If
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

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
                'Dim intSumRefundedtemp As Integer = 0
                'For Each udtVoucherRefund As VoucherRefundModel In udtVoucherRefundList_Full
                '    If Not udtVoucherRefund.SchemeSeq Is Nothing Then
                '        If udtVoucherRefund.SchemeSeq = udtSubsidizeGroupClaim.SchemeSeq Then
                '            intSumRefundedtemp = intSumRefundedtemp + udtVoucherRefund.RefundAmt
                '        End If
                '    End If
                'Next

                'intTotalRefundedtemp = intSumRefundedtemp
                intTotalRefundedtemp = udtVoucherDetail.Refund
                ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Winnie]

                If intTotalRefundedtemp <> 0 Then
                    dro(strColName_Refund) = udtformatter.formatMoney(intTotalRefundedtemp.ToString(), False)
                Else
                    dro(strColName_Refund) = udtformatter.formatMoney(intTotalRefundedtemp.ToString(), False)
                End If
            End If

            ' --- Used ($) (HCVS) ---
            'blnHasUsedVoucherHCVS = dicTotalUsedVoucherHCVS.ContainsKey(udtSubsidizeGroupClaim.SchemeSeq)

            'If blnIsEligible OrElse blnHasUsedVoucherHCVS Then
            '    If blnHasUsedVoucherHCVS Then
            '        If Not blnIsEligible Then
            '            strAlertHtmlImg_Tag_Result = strAlertHtmlImg_Tag
            '            dro(strColName_UsedHCVS) += strAlertHtmlImg_Tag
            '        End If

            '        intTotalUsedTempHCVS = CInt(dicTotalUsedVoucherHCVS(udtSubsidizeGroupClaim.SchemeSeq) * dblSubsidizeFee)
            '        dro(strColName_UsedHCVS) += udtformatter.formatMoney(intTotalUsedTempHCVS.ToString(), False)
            '    Else
            '        intTotalUsedTempHCVS = 0
            '        dro(strColName_UsedHCVS) = 0
            '    End If

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

    ' CRE19-003 (Opt voucher capping) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub GridView_MergeRow(ByVal gv As GridView, ByVal intColumn As Integer)
        Dim strBgColor As String = String.Empty
        Dim strBgColor1 As String = "#f0eef7"
        Dim strBgColor2 As String = "White"

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
                    strBgColor = strBgColor2
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
        'Select Case strAccountSource
        '    Case EHealthAccountType.Temporary
        '        udtEHSAccount = udtEHSAccountBLL.LoadTempEHSAccountByVRID(strAccountID)

        '    Case EHealthAccountType.Special
        '        udtEHSAccount = udtEHSAccountBLL.LoadSpecialEHSAccountByVRID(strAccountID)

        '    Case EHealthAccountType.Validated
        '        udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByVRID(strAccountID)

        '    Case EHealthAccountType.Invalid
        '        udtEHSAccount = udtEHSAccountBLL.LoadInvalidEHSAccountByVRID(strAccountID)

        'End Select

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
                        Me.ucReadOnlyDocumnetType.Build()

                        ucReadOnlyDocumnetType.Visible = True

                        Me.phReadOnlyAccountInfo.Controls.Clear()
                        Dim ucReadOnlyAccountInfo As ucReadOnlyAccountInfo = Me.LoadControl("~/UIControl/ucReadOnlyAccountInfo.ascx")
                        ucReadOnlyAccountInfo.EHSAccount = _udtEHSAccount
                        ucReadOnlyAccountInfo.Width = 220
                        Me.phReadOnlyAccountInfo.Controls.Add(ucReadOnlyAccountInfo)

                    Case ActionModel.ReadOnly_N_Amending

                        Me.ucInputDocumentType.DocType = strDocCode
                        Me.ucInputDocumentType.EHSAccountOriginal = _udtEHSAccount
                        Me.ucInputDocumentType.EHSAccountAmend = _udtEHSAccount_Amendment
                        Me.ucInputDocumentType.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly
                        Me.ucInputDocumentType.FillValue = True
                        Me.ucInputDocumentType.Built()

                        Me.ucInputDocumentType.Visible = True

                        Me.phReadOnlyAccountInfo.Controls.Clear()
                        Dim ucReadOnlyAccountInfo As ucReadOnlyAccountInfo = Me.LoadControl("~/UIControl/ucReadOnlyAccountInfo.ascx")
                        ucReadOnlyAccountInfo.EHSAccount = _udtEHSAccount
                        ucReadOnlyAccountInfo.Width = 220
                        Me.phReadOnlyAccountInfo.Controls.Add(ucReadOnlyAccountInfo)
                        'blnRes = True
                End Select
            End If

        End If
    End Sub

    'By Paul, bind reaonly control to show document type related information in Amendment History)
    Private Sub BindPersonalInfoInAmendmentHistory(ByVal _udtEHSAccount As EHSAccountModel, ByVal _udtEHSAccount_Amendment As EHSAccountModel, ByVal strDocCode As String)
        Dim blnRes As Boolean = False
        If Not IsNothing(_udtEHSAccount) AndAlso Not strDocCode.Equals(String.Empty) Then
            Me.ucReadOnlyDocTypeAmendHistory.DocumentType = strDocCode
            Me.ucReadOnlyDocTypeAmendHistory.EHSPersonalInformation = _udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)
            Me.ucReadOnlyDocTypeAmendHistory.Vertical = False
            Me.ucReadOnlyDocTypeAmendHistory.Width = 220
            Me.ucReadOnlyDocTypeAmendHistory.Build()

            Me.ucReadOnlyDocTypeAmendHistory.Visible = True
        End If
    End Sub

    'By Paul, bind reaonly control to show document type related information in Amendment History)
    Private Sub BindPersonalInfoInSchemeInfo(ByVal _udtEHSAccount As EHSAccountModel, ByVal _udtEHSAccount_Amendment As EHSAccountModel, ByVal strDocCode As String)
        Dim blnRes As Boolean = False

        If Not IsNothing(_udtEHSAccount) AndAlso Not strDocCode.Equals(String.Empty) Then
            'CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]

            Me.ucReadOnlyDocTypeSchemeInfo.Clear()
            Me.ucReadOnlyDocTypeSchemeInfo.DocumentType = strDocCode
            Me.ucReadOnlyDocTypeSchemeInfo.EHSPersonalInformation = _udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)
            Me.ucReadOnlyDocTypeSchemeInfo.Vertical = False
            Me.ucReadOnlyDocTypeSchemeInfo.Width = 220
            Me.ucReadOnlyDocTypeSchemeInfo.EHSAccountModel = _udtEHSAccount
            Me.ucReadOnlyDocTypeSchemeInfo.ShowAccountID = True
            Me.ucReadOnlyDocTypeSchemeInfo.ShowAccountIDAsBtn = False
            Me.ucReadOnlyDocTypeSchemeInfo.Build()

            Me.ucReadOnlyDocTypeSchemeInfo.Visible = True

            'CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]
        End If
    End Sub

    Private Sub BindPersonalInfoInVaccinationRecord(ByVal _udtEHSAccount As EHSAccountModel)
        If Not IsNothing(_udtEHSAccount) Then
            udcVaccinationRecord.BuildEHSAccount(_udtEHSAccount, True)

        End If

    End Sub

    Private Sub SetupInputDocControl(ByVal _udtEHSAccount As EHSAccountModel, ByVal _udtEHSAccount_Amendment As EHSAccountModel)
        Select Case Me.mveHSAccount.ActiveViewIndex
            Case intSearchView
            Case intSearchResult
            Case intAccountDetails
                BindPersonalInfo(_udtEHSAccount, _udtEHSAccount_Amendment, Me.txtDocCode.Text.Trim)
            Case intAmendmentHistroy
                'By Paul, added on 22/9 (Use read-only control to show document type related information in Amendment History)
                BindPersonalInfoInAmendmentHistory(_udtEHSAccount, _udtEHSAccount_Amendment, Me.txtDocCode.Text.Trim)
            Case intSchemeInfo
                'By Paul, added on 22/9 (Use read-only control to show document type related information in Scheme Info)
                BindPersonalInfoInSchemeInfo(_udtEHSAccount, _udtEHSAccount_Amendment, Me.txtDocCode.Text.Trim)

            Case intVaccinationRecord
                ' INT18-0033 (Fix selection of document for vaccination record enquiry in back-office platform) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Dim udtEHSAccountClone As EHSAccount.EHSAccountModel = Nothing
                Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = Nothing

                'Clone EHSAccount
                udtEHSPersonalInfo = _udtEHSAccount.getPersonalInformation(Me.txtDocCode.Text.Trim)

                udtEHSAccountClone = New EHSAccountModel(_udtEHSAccount)
                udtEHSAccountClone.SetPersonalInformation(udtEHSPersonalInfo)
                udtEHSAccountClone.SetSearchDocCode(Me.txtDocCode.Text.Trim)

                'BindPersonalInfoInVaccinationRecord(_udtEHSAccount)
                BindPersonalInfoInVaccinationRecord(udtEHSAccountClone)

                ' INT18-0033 (Fix selection of document for vaccination record enquiry in back-office platform) [End][Chris YIM]

        End Select
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
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00008, AuditLogDesc.SelectEHSAccount, udtAuditLogInfo)

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
            Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00009, AuditLogDesc.SelectEHSAccountSuccess)

            Me.ibtnAccountDetailsBack.Visible = False
        Else
            udcMsgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
            Me.udtAuditLogEntry.AddDescripton("AccountID", strAccID)
            Me.udtAuditLogEntry.AddDescripton("DocCode", udtEHSAccountModel.getPersonalInformation(strDocType).DocCode)
            Me.udtAuditLogEntry.AddDescripton("AccountSource", strAccSrc)
            Me.udtAuditLogEntry.AddDescripton("IdentityNumber", udtEHSAccountModel.getPersonalInformation(strDocType).IdentityNum)
            udcMsgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00010, AuditLogDesc.SelectEHSAccountFail)
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
            'gvAcctListR1.Columns(3).Visible = False
            'gvAcctListR1.Columns(2).Visible = True

            gvAcctListR2.Columns(4).Visible = False
            gvAcctListR2.Columns(3).Visible = True

            gvAcctListR3.Columns(4).Visible = False
            gvAcctListR3.Columns(3).Visible = True

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
            'Case 0
            '    gvAcctListR1.Columns(3).Visible = True
            '    gvAcctListR1.Columns(2).Visible = False

            'Case 1
            '    gvAcctListR2.Columns(4).Visible = True
            '    gvAcctListR2.Columns(3).Visible = False
            '    'hfDMaskDocumentNo.Value = YesNo.No
            '    'udcReadOnlyDocumentType.Clear()
            '    'udcReadOnlyDocumentType.MaskIdentityNo = hfDMaskDocumentNo.Value = YesNo.Yes
            '    'udcReadOnlyDocumentType.Build()

            Case 0
                gvAcctListR2.Columns(4).Visible = True
                gvAcctListR2.Columns(3).Visible = False
                'hfDMaskDocumentNo.Value = YesNo.No
                'udcReadOnlyDocumentType.Clear()
                'udcReadOnlyDocumentType.MaskIdentityNo = hfDMaskDocumentNo.Value = YesNo.Yes
                'udcReadOnlyDocumentType.Build()
            Case 1
                gvAcctListR3.Columns(4).Visible = True
                gvAcctListR3.Columns(3).Visible = False

        End Select

    End Sub

    Protected Sub ibtnPopupUnmaskCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Cancel_Click
        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        Select Case tcSearchRoute.ActiveTabIndex
            'Case 0
            '    chkMaskDocumentNoR1.Checked = True
            'Case 1
            '    chkMaskDocumentNoR2.Checked = True

            Case 0
                chkMaskDocumentNoR2.Checked = True
            Case 1
                chkMaskDocumentNoR2.Checked = True

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