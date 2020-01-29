Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount
Imports Common.Component.eHealthAccountDeathRecord
Imports Common.Component.HCVUUser
Imports Common.Component.RedirectParameter
Imports Common.Component.StaticData
Imports Common.Format
Imports CustomControls
Imports HCVU
Imports HCVU.Component.FunctionInformation
Imports HCVU.Component.Menu
Imports System.Data.SqlClient

Partial Public Class eHealthAccountDeathRecordMatchingResult
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
    'Inherits BasePageWithGridView
    Inherits BasePageWithControl
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    ' FunctionCode = FunctCode.FUNT010306

#Region "Private Class"

    Private Class SESS
        Public Const DeathRecordMatchResult As String = "010306_DeathRecordMatchResult"
        Public Const DeathRecordMatchResultDetail As String = "010306_DeathRecordMatchResultDetail"
    End Class

    Private Class VS
        Public Const UnmaskPopup As String = "010306_UnmaskPopup"
    End Class

    Private Class PopupStatus
        Public Const Active As String = "A"
        Public Const Closed As String = "C"
    End Class

    Private Class ViewIndexCore
        Public Const SearchCriteria As Integer = 0
        Public Const SearchResult As Integer = 1
        Public Const Detail As Integer = 2
        Public Const Finish As Integer = 3
    End Class

    Private Class AccountTypeClass
        Public Const Validated As String = "V"
        Public Const Temporary As String = "T"
    End Class

    Private Class AccountStatusClass
        Public Const Active As String = "A"
        Public Const Suspended As String = "S"
        Public Const Terminiated As String = "D"
    End Class

    Private Class YesNoClass
        Public Const Yes As String = "Y"
        Public Const No As String = "N"
    End Class

    Private Class AccountActionTypeClass
        Public Const Suspend As String = "S"
        Public Const Terminate As String = "T"
    End Class

    Private Class ExactDODClass
        Public Const Y As String = "Y"
        Public Const M As String = "M"
        Public Const D As String = "D"
    End Class

    Private Class MessageBoxHeaderKey
        Public Const ValidationFail As String = "ValidationFail"
    End Class

    Private Class AuditLogDescription
        Public Const LOG00000 As String = "eHealth Account Death Record Matching Result Page Load"
        Public Const LOG00001 As String = "Criteria - Search click"
        Public Const LOG00002 As String = "Criteria - Search success"
        Public Const LOG00003 As String = "Criteria - Search fail"
        Public Const LOG00004 As String = "Result - Back click"
        Public Const LOG00005 As String = "Result - Manage Selected Accounts click"
        Public Const LOG00006 As String = "Result - Manage Selected Accounts success"
        Public Const LOG00007 As String = "Result - Manage Selected Accounts fail"
        Public Const LOG00008 As String = "Result - Mask Identity Document No. click"
        Public Const LOG00009 As String = "Result - Row click"
        Public Const LOG00010 As String = "Detail - Back click"
        Public Const LOG00011 As String = "Detail - Recheck And Update Matching Information click"
        Public Const LOG00012 As String = "Detail - Mask Identity Document No. click"
        Public Const LOG00013 As String = "Detail - Unmask Identity Document No. success"
        'Public Const LOG00014 As String = "Detail - eHealth Account ID Hyperlink click"  -- In user control, cannot be captured here
        Public Const LOG00015 As String = "Detail - Transaction No. Hyperlink click"
        Public Const LOG00016 As String = "Finish - Return click"
    End Class

#End Region

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------
#Region "SF Search"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        ' --- Validation ---

        ' Init
        Session.Remove(SESS.DeathRecordMatchResult)

        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False
        imgSYearOfBirth.Visible = False

        Dim intYearOfBirthFrom As Integer = -1
        Dim intYearOfBirthTo As Integer = -1
        Dim blnYearOfBirthValid As Boolean = True
        Dim blnYearOfBirthBothEntered As Boolean = True

        If (txtSYearOfBirthFrom.Text.Trim = String.Empty AndAlso txtSYearOfBirthTo.Text.Trim <> String.Empty) _
                OrElse (txtSYearOfBirthFrom.Text.Trim <> String.Empty AndAlso txtSYearOfBirthTo.Text.Trim = String.Empty) Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, "%s", Me.GetGlobalResourceObject("Text", "YearOfBirth"))
            imgSYearOfBirth.Visible = True

            blnYearOfBirthValid = False

        End If

        If blnYearOfBirthValid Then
            If txtSYearOfBirthFrom.Text.Trim = String.Empty Then
                intYearOfBirthFrom = -1
                blnYearOfBirthBothEntered = False

            ElseIf Integer.TryParse(txtSYearOfBirthFrom.Text.Trim, intYearOfBirthFrom) = False Then
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "%s", Me.GetGlobalResourceObject("Text", "YearOfBirth"))
                imgSYearOfBirth.Visible = True
                blnYearOfBirthBothEntered = False

                'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'ElseIf intYearOfBirthFrom < 1753 OrElse intYearOfBirthFrom > 9999 Then
            ElseIf intYearOfBirthFrom < DateValidation.YearMinValue OrElse intYearOfBirthFrom > DateValidation.YearMaxValue Then
                'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
                imgSYearOfBirth.Visible = True
                blnYearOfBirthBothEntered = False

            End If

            If txtSYearOfBirthTo.Text.Trim = String.Empty Then
                intYearOfBirthTo = -1
                blnYearOfBirthBothEntered = False

            ElseIf Integer.TryParse(txtSYearOfBirthTo.Text.Trim, intYearOfBirthTo) = False Then
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "%s", Me.GetGlobalResourceObject("Text", "YearOfBirth"))
                imgSYearOfBirth.Visible = True
                blnYearOfBirthBothEntered = False

                'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'ElseIf intYearOfBirthTo < 1753 OrElse intYearOfBirthTo > 9999 Then
            ElseIf intYearOfBirthTo < DateValidation.YearMinValue OrElse intYearOfBirthTo > DateValidation.YearMaxValue Then
                'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
                imgSYearOfBirth.Visible = True
                blnYearOfBirthBothEntered = False

            End If

        End If

        If blnYearOfBirthBothEntered Then
            If intYearOfBirthFrom > intYearOfBirthTo Then
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008)
                imgSYearOfBirth.Visible = True
            End If
        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00003, AuditLogDescription.LOG00003)
            Return False
        Else
            Return True
        End If

        ' --- End of Validation ---
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult
        Dim bllSearchResult As Common.Component.BaseBLL.BLLSearchResult
        Dim udtFormatter As New Formatter

        Dim intYearOfBirthFrom As Integer = -1
        Dim intYearOfBirthTo As Integer = -1
        Dim blnYearOfBirthValid As Boolean = True
        Dim blnYearOfBirthBothEntered As Boolean = True

        If (txtSYearOfBirthFrom.Text.Trim = String.Empty AndAlso txtSYearOfBirthTo.Text.Trim <> String.Empty) _
                OrElse (txtSYearOfBirthFrom.Text.Trim <> String.Empty AndAlso txtSYearOfBirthTo.Text.Trim = String.Empty) Then
            'udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, "%s", Me.GetGlobalResourceObject("Text", "YearOfBirth"))
            'imgSYearOfBirth.Visible = True

            blnYearOfBirthValid = False

        End If

        If blnYearOfBirthValid Then
            If txtSYearOfBirthFrom.Text.Trim = String.Empty Then
                intYearOfBirthFrom = -1
                blnYearOfBirthBothEntered = False

            ElseIf Integer.TryParse(txtSYearOfBirthFrom.Text.Trim, intYearOfBirthFrom) = False Then
                'udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "%s", Me.GetGlobalResourceObject("Text", "YearOfBirth"))
                'imgSYearOfBirth.Visible = True
                blnYearOfBirthBothEntered = False

                'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'ElseIf intYearOfBirthFrom < 1753 OrElse intYearOfBirthFrom > 9999 Then
            ElseIf intYearOfBirthFrom < DateValidation.YearMinValue OrElse intYearOfBirthFrom > DateValidation.YearMaxValue Then
                'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

                'udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
                'imgSYearOfBirth.Visible = True
                blnYearOfBirthBothEntered = False

            End If

            If txtSYearOfBirthTo.Text.Trim = String.Empty Then
                intYearOfBirthTo = -1
                blnYearOfBirthBothEntered = False

            ElseIf Integer.TryParse(txtSYearOfBirthTo.Text.Trim, intYearOfBirthTo) = False Then
                'udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "%s", Me.GetGlobalResourceObject("Text", "YearOfBirth"))
                'imgSYearOfBirth.Visible = True
                blnYearOfBirthBothEntered = False

                'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'ElseIf intYearOfBirthTo < 1753 OrElse intYearOfBirthTo > 9999 Then
            ElseIf intYearOfBirthTo < DateValidation.YearMinValue OrElse intYearOfBirthTo > DateValidation.YearMaxValue Then
                'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]

                'udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
                'imgSYearOfBirth.Visible = True
                blnYearOfBirthBothEntered = False

            End If

        End If

        If blnOverrideResultLimit Then
            bllSearchResult = (New eHealthAccountDeathRecordBLL).GetDeathRecordMatchResult(Me.FunctionCode, ddlSDocumentType.SelectedValue.Trim, _
                            udtFormatter.formatDocumentIdentityNumber(DocTypeCode.HKIC, udtFormatter.formatHKIDInternal(txtSDocumentNo.Text)), _
                            ddlSAccountType.SelectedValue.Trim, ddlSAccountStatus.SelectedValue.Trim, ddlSWithClaim.SelectedValue.Trim, _
                            ddlSWithSuspiciousClaim.SelectedValue.Trim, ddlSNameMatch.SelectedValue.Trim, intYearOfBirthFrom, intYearOfBirthTo, Nothing, False, True)

        Else
            bllSearchResult = (New eHealthAccountDeathRecordBLL).GetDeathRecordMatchResult(Me.FunctionCode, ddlSDocumentType.SelectedValue.Trim, _
                            udtFormatter.formatDocumentIdentityNumber(DocTypeCode.HKIC, udtFormatter.formatHKIDInternal(txtSDocumentNo.Text)), _
                            ddlSAccountType.SelectedValue.Trim, ddlSAccountStatus.SelectedValue.Trim, ddlSWithClaim.SelectedValue.Trim, _
                            ddlSWithSuspiciousClaim.SelectedValue.Trim, ddlSNameMatch.SelectedValue.Trim, intYearOfBirthFrom, intYearOfBirthTo)
        End If

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
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.LOG00002)

            Case Else
                blnShowResultList = True

        End Select

        If blnShowResultList Then
            dt = ProcessMatchResult(dt)

            Session(SESS.DeathRecordMatchResult) = dt

            BuildSearchCriteriaReview()

            GridViewDataBind(gvMatchResult, dt, "Account_ID", "ASC", False)

            ' Unmask Identity Document No.: Default at checked
            cboMaskDocumentNo.Checked = True
            gvMatchResult.Columns(4).Visible = False
            gvMatchResult.Columns(5).Visible = True

            ' Check whether the show the Unmask Identity Document No.
            cboMaskDocumentNo.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes

            mvCore.ActiveViewIndex = ViewIndexCore.SearchResult

            ' Manage Selected Accounts: Enable when user has right to [eHealth Account Maintenance]
            If (New HCVUUserBLL).GetHCVUUser().AccessRightCollection.Item(FunctCode.FUNT010301).Allow Then
                ibtnRManageSelectedAccount.Enabled = True
                ibtnRManageSelectedAccount.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ManageSelectedAccountsBtn")
            Else
                ibtnRManageSelectedAccount.Enabled = False
                ibtnRManageSelectedAccount.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ManageSelectedAccountsDisableBtn")
            End If

            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, AuditLogDescription.LOG00002)

        End If

        Return intRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        Dim enumSearchResult As SearchResultEnum

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00001, AuditLogDescription.LOG00001)

        Try
            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLog, udcMessageBox, udcInfoMessageBox, False, True)

        Catch eSQL As SqlException
            udtAuditLog.AddDescripton("StackTrace", "SqlException")
            udtAuditLog.AddDescripton("Message", eSQL.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00003, AuditLogDescription.LOG00003)
            Throw eSQL

        Catch ex As Exception
            udtAuditLog.AddDescripton("StackTrace", "Exception: Unknown error")
            udtAuditLog.AddDescripton("Message", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00003, AuditLogDescription.LOG00003)
            Throw ex

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success
                udtAuditLog.WriteEndLog(LogID.LOG00002, AuditLogDescription.LOG00002)

            Case Else
                Throw New Exception("Error: Class = [HCVU.eHSAccountDeathRecordMatchingResult], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

        End Select

    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub

#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Get HCVU User to check session expire
        GetCurrentUser()

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT010306

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, AuditLogDescription.LOG00000)

            mvCore.ActiveViewIndex = ViewIndexCore.SearchCriteria
            InitSearchCriteria()

        End If

        ' Re-bind the detail if in the Detail View
        If mvCore.ActiveViewIndex = ViewIndexCore.Detail Then
            BuildDetail()
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case ViewState(VS.UnmaskPopup)
            Case PopupStatus.Active
                popupUnmask.Show()

        End Select
    End Sub

    Private Function GetCurrentUser() As String
        Return (New HCVUUserBLL).GetHCVUUser.UserID
    End Function

    '

    Private Sub mvCore_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvCore.ActiveViewChanged
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False

        Select Case mvCore.ActiveViewIndex
            Case ViewIndexCore.SearchCriteria

            Case ViewIndexCore.SearchResult
                Me.cboMaskDocumentNo.Checked = True

                gvMatchResult.Columns(4).Visible = False
                gvMatchResult.Columns(5).Visible = True

            Case ViewIndexCore.Detail
                cboDMaskDocumentNo.Checked = True
                hfDMaskDocumentNo.Value = YesNo.Yes

                ' Check whether to show the Unmask Identity Document No.
                cboDMaskDocumentNo.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes

        End Select

    End Sub

    Private Sub InitSearchCriteria()
        ' Bind Document Type
        ddlSDocumentType.Items.Clear()

        ddlSDocumentType.DataSource = (New DocTypeBLL).getDocTypeByAvailable(DocTypeBLL.EnumAvailable.DeathRecordAvailable)
        ddlSDocumentType.DataTextField = "DocName"
        ddlSDocumentType.DataValueField = "DocCode"
        ddlSDocumentType.DataBind()

        ddlSDocumentType.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSDocumentType.SelectedIndex = 0

        ' Bind Account Type
        ddlSAccountType.Items.Clear()

        ddlSAccountType.DataSource = Status.GetDescriptionListFromDBEnumCode("DeathRecordAccountType")
        ddlSAccountType.DataValueField = "Status_Value"
        ddlSAccountType.DataTextField = "Status_Description"
        ddlSAccountType.DataBind()

        ddlSAccountType.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSAccountType.SelectedIndex = 1

        ' Bind Account Status
        ddlSAccountStatus.Items.Clear()

        ddlSAccountStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("DeathRecordValidatedAccountRecordStatus")
        ddlSAccountStatus.DataValueField = "Status_Value"
        ddlSAccountStatus.DataTextField = "Status_Description"
        ddlSAccountStatus.DataBind()

        ddlSAccountStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSAccountStatus.SelectedIndex = 0

        ' Bind With Claims
        Dim udtStaticDataList As StaticDataModelCollection = (New StaticDataBLL).GetStaticDataListByColumnName("YesNo")

        ddlSWithClaim.DataSource = udtStaticDataList
        ddlSWithClaim.DataValueField = "ItemNo"
        ddlSWithClaim.DataTextField = "DataValue"
        ddlSWithClaim.DataBind()

        ddlSWithClaim.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSWithClaim.SelectedIndex = 0

        ' Bind With Suspicious Claims
        ddlSWithSuspiciousClaim.DataSource = udtStaticDataList
        ddlSWithSuspiciousClaim.DataValueField = "ItemNo"
        ddlSWithSuspiciousClaim.DataTextField = "DataValue"
        ddlSWithSuspiciousClaim.DataBind()

        ddlSWithSuspiciousClaim.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSWithSuspiciousClaim.SelectedIndex = 0

        ' Bind Name Matched
        ddlSNameMatch.DataSource = udtStaticDataList
        ddlSNameMatch.DataValueField = "ItemNo"
        ddlSNameMatch.DataTextField = "DataValue"
        ddlSNameMatch.DataBind()

        ddlSNameMatch.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSNameMatch.SelectedIndex = 0

        ' Clear text fields
        txtSDocumentNo.Text = String.Empty
        txtSYearOfBirthFrom.Text = String.Empty
        txtSYearOfBirthTo.Text = String.Empty

        InitSearchCriteriaEnable()

        Session.Remove(SESS.DeathRecordMatchResult)

    End Sub

    '

    Protected Sub ddlSAccountType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        InitSearchCriteriaEnable()
    End Sub

    Protected Sub ddlSWithClaim_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        InitSearchCriteriaEnable()
    End Sub

    Private Sub InitSearchCriteriaEnable()
        ' Account Type
        If ddlSAccountType.SelectedValue.Trim = String.Empty OrElse ddlSAccountType.SelectedValue.Trim = AccountTypeClass.Validated Then
            ddlSAccountStatus.Enabled = True
        Else
            ddlSAccountStatus.Enabled = False
            ddlSAccountStatus.SelectedIndex = 0
        End If

        ' With Claims
        If ddlSWithClaim.SelectedValue.Trim = String.Empty OrElse ddlSWithClaim.SelectedValue.Trim = YesNoClass.Yes Then
            ddlSWithSuspiciousClaim.Enabled = True
        Else
            ddlSWithSuspiciousClaim.Enabled = False
            ddlSWithSuspiciousClaim.SelectedIndex = 0
        End If

    End Sub

    Protected Sub ibtnSSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Document Type", ddlSDocumentType.SelectedValue)
        udtAuditLog.AddDescripton("Identity Document No.", txtSDocumentNo.Text)
        udtAuditLog.AddDescripton("Account Type", ddlSAccountType.SelectedValue)
        udtAuditLog.AddDescripton("Account Status", ddlSAccountStatus.SelectedValue)
        udtAuditLog.AddDescripton("With Claims", ddlSWithClaim.SelectedValue)
        udtAuditLog.AddDescripton("With Suspicious Claims", ddlSWithSuspiciousClaim.SelectedValue)
        udtAuditLog.AddDescripton("Name Matched", ddlSNameMatch.SelectedValue)
        udtAuditLog.AddDescripton("Year of Birth From", txtSYearOfBirthFrom.Text)
        udtAuditLog.AddDescripton("Year of Birth To", txtSYearOfBirthTo.Text)
        udtAuditLog.WriteStartLog(LogID.LOG00001, AuditLogDescription.LOG00001)

        ' CRE12-015 Add the respective practice number in “Practice” in the functions under “Reimbursement” in eHS [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------------------------------------------
        ' Implement Collapsible Search Criteria Review
        udcCollapsibleSearchCriteriaReview.Collapsed = True
        udcCollapsibleSearchCriteriaReview.ClientState = "True"
        ' CRE12-015 Add the respective practice number in “Practice” in the functions under “Reimbursement” in eHS [End][Tommy L]

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------

        '' --- Validation ---

        '' Init
        'Session.Remove(SESS.DeathRecordMatchResult)

        'udcMessageBox.Visible = False
        'udcInfoMessageBox.Visible = False
        'imgSYearOfBirth.Visible = False

        'Dim intYearOfBirthFrom As Integer = -1
        'Dim intYearOfBirthTo As Integer = -1
        'Dim blnYearOfBirthValid As Boolean = True
        'Dim blnYearOfBirthBothEntered As Boolean = True

        'If (txtSYearOfBirthFrom.Text.Trim = String.Empty AndAlso txtSYearOfBirthTo.Text.Trim <> String.Empty) _
        '        OrElse (txtSYearOfBirthFrom.Text.Trim <> String.Empty AndAlso txtSYearOfBirthTo.Text.Trim = String.Empty) Then
        '    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, "%s", Me.GetGlobalResourceObject("Text", "YearOfBirth"))
        '    imgSYearOfBirth.Visible = True

        '    blnYearOfBirthValid = False

        'End If

        'If blnYearOfBirthValid Then
        '    If txtSYearOfBirthFrom.Text.Trim = String.Empty Then
        '        intYearOfBirthFrom = -1
        '        blnYearOfBirthBothEntered = False

        '    ElseIf Integer.TryParse(txtSYearOfBirthFrom.Text.Trim, intYearOfBirthFrom) = False Then
        '        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "%s", Me.GetGlobalResourceObject("Text", "YearOfBirth"))
        '        imgSYearOfBirth.Visible = True
        '        blnYearOfBirthBothEntered = False

        '    ElseIf intYearOfBirthFrom < 1753 OrElse intYearOfBirthFrom > 9999 Then
        '        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
        '        imgSYearOfBirth.Visible = True
        '        blnYearOfBirthBothEntered = False

        '    End If

        '    If txtSYearOfBirthTo.Text.Trim = String.Empty Then
        '        intYearOfBirthTo = -1
        '        blnYearOfBirthBothEntered = False

        '    ElseIf Integer.TryParse(txtSYearOfBirthTo.Text.Trim, intYearOfBirthTo) = False Then
        '        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "%s", Me.GetGlobalResourceObject("Text", "YearOfBirth"))
        '        imgSYearOfBirth.Visible = True
        '        blnYearOfBirthBothEntered = False

        '    ElseIf intYearOfBirthTo < 1753 OrElse intYearOfBirthTo > 9999 Then
        '        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
        '        imgSYearOfBirth.Visible = True
        '        blnYearOfBirthBothEntered = False

        '    End If

        'End If

        'If blnYearOfBirthBothEntered Then
        '    If intYearOfBirthFrom > intYearOfBirthTo Then
        '        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008)
        '        imgSYearOfBirth.Visible = True
        '    End If
        'End If

        'If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
        '    udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00003, AuditLogDescription.LOG00003)
        '    Return
        'End If

        '' --- End of Validation ---

        '' Retrieve data from database
        'Dim udtFormatter As New Formatter
        'Dim dt As DataTable = Nothing

        'Try
        '    dt = (New eHealthAccountDeathRecordBLL).GetDeathRecordMatchResult(ddlSDocumentType.SelectedValue.Trim, _
        '        udtFormatter.formatDocumentIdentityNumber(DocTypeCode.HKIC, udtFormatter.formatHKIDInternal(txtSDocumentNo.Text)), _
        '        ddlSAccountType.SelectedValue.Trim, ddlSAccountStatus.SelectedValue.Trim, ddlSWithClaim.SelectedValue.Trim, _
        '        ddlSWithSuspiciousClaim.SelectedValue.Trim, ddlSNameMatch.SelectedValue.Trim, intYearOfBirthFrom, intYearOfBirthTo)

        '    udtAuditLog.AddDescripton("No. of record", dt.Rows.Count)

        '    If dt.Rows.Count > (New GeneralFunction).GetMaxRowRetrieve() Then
        '        udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00002)
        '        udcInfoMessageBox.Type = InfoMessageBoxType.Information
        '        udcInfoMessageBox.BuildMessageBox()

        '        udtAuditLog.AddDescripton("StackTrace", "Too many records")
        '        udtAuditLog.WriteEndLog(LogID.LOG00003, AuditLogDescription.LOG00003)

        '        Return
        '    End If

        'Catch eSQL As SqlException
        '    udtAuditLog.AddDescripton("StackTrace", "SqlException")
        '    udtAuditLog.AddDescripton("Message", eSQL.Message)

        '    Throw eSQL

        'Catch ex As Exception
        '    udtAuditLog.AddDescripton("StackTrace", "Exception: Unknown error")
        '    udtAuditLog.AddDescripton("Message", ex.Message)

        '    Throw ex

        'End Try

        'udtAuditLog.WriteEndLog(LogID.LOG00002, AuditLogDescription.LOG00002)

        'dt = ProcessMatchResult(dt)

        'Session(SESS.DeathRecordMatchResult) = dt

        'If dt.Rows.Count = 0 Then
        '    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
        '    udcInfoMessageBox.Type = InfoMessageBoxType.Information
        '    udcInfoMessageBox.BuildMessageBox()

        'Else
        '    BuildSearchCriteriaReview()

        '    GridViewDataBind(gvMatchResult, dt, "Account_ID", "ASC", False)

        '    ' Unmask Identity Document No.: Default at checked
        '    cboMaskDocumentNo.Checked = True
        '    gvMatchResult.Columns(4).Visible = False
        '    gvMatchResult.Columns(5).Visible = True

        '    ' Check whether the show the Unmask Identity Document No.
        '    cboMaskDocumentNo.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes

        '    mvCore.ActiveViewIndex = ViewIndexCore.SearchResult

        '    ' Manage Selected Accounts: Enable when user has right to [eHealth Account Maintenance]
        '    If (New HCVUUserBLL).GetHCVUUser().AccessRightCollection.Item(FunctCode.FUNT010301).Allow Then
        '        ibtnRManageSelectedAccount.Enabled = True
        '        ibtnRManageSelectedAccount.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ManageSelectedAccountsBtn")
        '    Else
        '        ibtnRManageSelectedAccount.Enabled = False
        '        ibtnRManageSelectedAccount.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ManageSelectedAccountsDisableBtn")
        '    End If

        'End If
        
        Dim enumSearchResult As SearchResultEnum

        Try

            If sender Is Nothing Then
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLog, udcMessageBox, udcInfoMessageBox, False, True)
            Else
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLog, udcMessageBox, udcInfoMessageBox)
            End If

            Select Case enumSearchResult
                Case SearchResultEnum.Success
                    udtAuditLog.WriteEndLog(LogID.LOG00002, AuditLogDescription.LOG00002)

                Case SearchResultEnum.ValidationFail
                    ' Audit Log has been handled in [SF_ValidateSearch] method

                Case SearchResultEnum.NoRecordFound
                    udtAuditLog.WriteEndLog(LogID.LOG00002, AuditLogDescription.LOG00002)

                Case SearchResultEnum.OverResultList1stLimit_PopUp
                    udtAuditLog.WriteEndLog(LogID.LOG00003, AuditLogDescription.LOG00003)

                Case SearchResultEnum.OverResultList1stLimit_Alert
                    udtAuditLog.WriteEndLog(LogID.LOG00003, AuditLogDescription.LOG00003)

                Case SearchResultEnum.OverResultListOverrideLimit
                    udtAuditLog.WriteEndLog(LogID.LOG00003, AuditLogDescription.LOG00003)

                Case Else
                    Throw New Exception("Error: Class = [HCVU.eHealthAccountDeathRecordMatchingResult], Method = [ibtnSSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

            End Select

        Catch ex As Exception
            Throw ex
        End Try

        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    End Sub

    Private Sub BuildSearchCriteriaReview()
        lblRDocumentType.Text = ddlSDocumentType.SelectedItem.Text
        lblRDocumentNo.Text = FillAnyToEmptyString(txtSDocumentNo.Text.Trim)
        lblRAccountType.Text = ddlSAccountType.SelectedItem.Text
        lblRAccountStatus.Text = ddlSAccountStatus.SelectedItem.Text
        lblRWithClaim.Text = ddlSWithClaim.SelectedItem.Text
        lblRWithSuspiciousClaim.Text = ddlSWithSuspiciousClaim.SelectedItem.Text
        lblRNameMatch.Text = ddlSNameMatch.SelectedItem.Text

        If txtSYearOfBirthFrom.Text.Trim = String.Empty Then
            lblRYearOfBirth.Text = FillAnyToEmptyString(String.Empty)
        Else
            lblRYearOfBirth.Text = String.Format("{0} {1} {2}", txtSYearOfBirthFrom.Text.Trim, Me.GetGlobalResourceObject("Text", "To_S"), txtSYearOfBirthTo.Text.Trim)
        End If

    End Sub

    Private Function ProcessMatchResult(ByVal dt As DataTable) As DataTable
        dt.Columns.Add("Account_ID_F", GetType(String))
        dt.Columns.Add("Account_Status_F", GetType(String))
        dt.Columns.Add("Document_No_F", GetType(String))
        dt.Columns.Add("Document_No_FM", GetType(String))

        Dim udtFormatter As New Formatter

        For Each dr As DataRow In dt.Rows
            If dr("Account_Type").ToString.Trim = AccountTypeClass.Validated Then
                dr("Account_ID_F") = udtFormatter.formatValidatedEHSAccountNumber(dr("Account_ID").ToString.Trim)
                dr("Account_Status_F") = String.Empty
                Status.GetDescriptionFromDBCode("ValidatedAccountRecordStatusClass", dr("Account_Status").ToString.Trim, dr("Account_Status_F"), String.Empty)
            Else
                dr("Account_ID_F") = udtFormatter.formatSystemNumber(dr("Account_ID").ToString.Trim)
                dr("Account_Status_F") = String.Empty
                Status.GetDescriptionFromDBCode("TempAccountRecordStatusClass", dr("Account_Status").ToString.Trim, dr("Account_Status_F"), String.Empty)
            End If

            dr("Document_No_F") = udtFormatter.FormatDocIdentityNoForDisplay(dr("Document_Code").ToString.Trim, dr("Document_No").ToString.Trim, False)
            dr("Document_No_FM") = udtFormatter.FormatDocIdentityNoForDisplay(dr("Document_Code").ToString.Trim, dr("Document_No").ToString.Trim, True)

            dr.AcceptChanges()

        Next

        Return dt

    End Function

    Private Function FillAnyToEmptyString(ByVal value As String) As String
        If IsNothing(value) OrElse value.Trim = String.Empty Then
            Return Me.GetGlobalResourceObject("Text", "Any")
        End If

        Return value
    End Function

    '

    Protected Sub cboMaskDocumentNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Checked change to", IIf(cboMaskDocumentNo.Checked, "T", "F"))
        udtAuditLog.WriteLog(LogID.LOG00008, AuditLogDescription.LOG00008)

        If cboMaskDocumentNo.Checked Then
            ' Unchecked -> Checked
            gvMatchResult.Columns(4).Visible = False
            gvMatchResult.Columns(5).Visible = True

        Else
            ' Checked -> Unchecked
            popupUnmask.Show()
            ViewState(VS.UnmaskPopup) = PopupStatus.Active
            InitPopupUnmask()

        End If

    End Sub

    Protected Sub ibtnRBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00004, AuditLogDescription.LOG00004)

        mvCore.ActiveViewIndex = ViewIndexCore.SearchCriteria
    End Sub

    Private Sub InitPopupUnmask()
        ' CRE12-014 - Relax 500 row limit in back office platform [Start][Twinsen]
        popupUnmask.PopupDragHandleControlID = udcPUInputToken.Header.ClientID
        udcPUInputToken.HeaderText = Me.GetGlobalResourceObject("Text", "ConfirmUnmaskIdentityDocumentNo")
        ' CRE12-014 - Relax 500 row limit in back office platform [End] [Twinsen]
        udcPUInputToken.Message = Me.GetGlobalResourceObject("Text", "ConfirmUnmaskMessage")
        udcPUInputToken.Build()

    End Sub

    '

    Protected Sub gvMatchResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Header Then
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & _
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).ClientID & "')")
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem

            Dim udtFormatter As New Formatter

            ' Checkbox
            Dim cboSelect As CheckBox = e.Row.FindControl("cboSelect")
            cboSelect.Enabled = dr("Account_Type").ToString.Trim = AccountTypeClass.Validated

            ' Account Type
            Dim lblGAccountType As Label = e.Row.FindControl("lblGAccountType")
            Status.GetDescriptionFromDBCode("DeathRecordAccountType", lblGAccountType.Text.Trim, lblGAccountType.Text, String.Empty)

            ' Date of Birth
            Dim lblGDOB As Label = e.Row.FindControl("lblGDOB")

            If Not IsDBNull(dr("DOB")) Then
                Dim intECAge As Nullable(Of Integer) = Nothing
                If Not IsDBNull(dr("EC_Age")) Then intECAge = CInt(dr("EC_Age"))

                Dim dtmECDOR As Nullable(Of DateTime) = Nothing
                If Not IsDBNull(dr("EC_Date_of_Registration")) Then dtmECDOR = dr("EC_Date_of_Registration")

                lblGDOB.Text = udtFormatter.formatDOB(dr("DOB"), dr("Exact_DOB"), Nothing, intECAge, dtmECDOR)
            End If

            ' Date of Death
            Dim lblGDOD As Label = e.Row.FindControl("lblGDOD")

            If Not IsDBNull(dr("DOD")) Then
                lblGDOD.Text = udtFormatter.formatDOB(dr("DOD"), dr("Exact_DOD"), Nothing, Nothing)
            End If

            ' With Suspicious Claim
            Dim lblGWithSuspiciousClaim As Label = e.Row.FindControl("lblGWithSuspiciousClaim")
            lblGWithSuspiciousClaim.Text = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("YesNo", lblGWithSuspiciousClaim.Text.Trim).DataValue

        End If

    End Sub

    Protected Sub gvMatchResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If e.CommandName.ToUpper <> "PAGE" AndAlso e.CommandName.ToUpper <> "SORT" Then
            Dim r As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)

            Dim strAccountID As String = CType(r.FindControl("hfGAccountID"), HiddenField).Value.Trim
            Dim strDocCode As String = CType(r.FindControl("lblGDocumentType"), Label).Text.Trim

            mvCore.ActiveViewIndex = ViewIndexCore.Detail

            BuildDetail(strAccountID, strDocCode)

            ScriptManager1.SetFocus(ibtnDBack)

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.AddDescripton("Account ID", strAccountID)
            udtAuditLog.AddDescripton("Doc Code", strDocCode)
            udtAuditLog.WriteLog(LogID.LOG00009, AuditLogDescription.LOG00009)

        End If
    End Sub

    Protected Sub gvMatchResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS.DeathRecordMatchResult)
    End Sub

    Protected Sub gvMatchResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS.DeathRecordMatchResult)
    End Sub

    Protected Sub gvMatchResult_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS.DeathRecordMatchResult)
    End Sub

    '

    Protected Sub ibtnRManageSelectedAccount_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00005, AuditLogDescription.LOG00005)

        Dim udtGeneralFunction As New GeneralFunction
        Dim sbAccID As New StringBuilder
        Dim strAccID As String = String.Empty
        For Each gvr As GridViewRow In gvMatchResult.Rows
            If CType(gvr.FindControl("cboSelect"), CheckBox).Checked Then
                strAccID = CType(gvr.FindControl("hfGAccountID"), HiddenField).Value.Trim
                If sbAccID.Length > 0 Then sbAccID.Append(eHSAccountMaint.EHAccountIDSeparator)
                sbAccID.Append(strAccID)
            End If
        Next

        udtAuditLog.AddDescripton("Account ID", sbAccID.ToString)

        If sbAccID.Length = 0 Then
            udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00023)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00007, AuditLogDescription.LOG00007)

            Return
        End If

        udtAuditLog.WriteEndLog(LogID.LOG00006, AuditLogDescription.LOG00006)

        ' Save to session and redirect to eHealth Account Maintenance
        Dim udtRedirectParameterBLL As New RedirectParameterBLL
        Dim udtRedirectParameter As New RedirectParameterModel
        udtRedirectParameter.EHealthAccountID = sbAccID.ToString()
        udtRedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
        udtRedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)
        udtRedirectParameterBLL.SaveToSession(udtRedirectParameter)
        Response.Redirect(GetURLByFunctionCode(FunctCode.FUNT010301))

    End Sub

    '

    Protected Sub gvDST_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            ' Transaction No.
            Dim clbtnGTransactionNo As CustomLinkButton = e.Row.FindControl("clbtnGTransactionNo")
            clbtnGTransactionNo.Text = udtFormatter.formatSystemNumber(dr("Transaction_ID").ToString.Trim)
            clbtnGTransactionNo.SourceFunctionCode = FunctionCode
            clbtnGTransactionNo.TargetFunctionCode = FunctCode.FUNT010403
            clbtnGTransactionNo.TargetUrl = GetURLByFunctionCode(FunctCode.FUNT010403)
            clbtnGTransactionNo.SchemeCode = dr("Scheme_Code").ToString.Trim

            If clbtnGTransactionNo.Build() Then
                clbtnGTransactionNo.ConstructNewRedirectParameter()
                clbtnGTransactionNo.RedirectParameter.TransactionNo = dr("Transaction_ID").ToString.Trim
                clbtnGTransactionNo.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
                clbtnGTransactionNo.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)

                AddHandler clbtnGTransactionNo.Click, AddressOf clbtnGTransactionNo_Click
            End If

            ' Transaction Status
            Dim lblGTransactionStatus As Label = e.Row.FindControl("lblGTransactionStatus")
            Status.GetDescriptionFromDBCode(ClaimTransStatus.ClassCode, dr("Record_Status").ToString.Trim, lblGTransactionStatus.Text, String.Empty)

        End If
    End Sub

    Protected Sub cboDMaskDocumentNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Checked change to", IIf(cboDMaskDocumentNo.Checked, "T", "F"))
        udtAuditLog.WriteLog(LogID.LOG00012, AuditLogDescription.LOG00012)

        If cboDMaskDocumentNo.Checked Then
            ' Unchecked -> Checked
            hfDMaskDocumentNo.Value = YesNo.Yes
            udcReadOnlyDocumentType.Clear()
            udcReadOnlyDocumentType.MaskIdentityNo = hfDMaskDocumentNo.Value = YesNo.Yes
            udcReadOnlyDocumentType.Build()

        Else
            ' Checked -> Unchecked
            popupUnmask.Show()
            ViewState(VS.UnmaskPopup) = PopupStatus.Active
            InitPopupUnmask()

        End If

    End Sub

    Protected Sub clbtnGTransactionNo_Click(ByVal sender As LinkButton, ByVal e As System.EventArgs)
        Dim clbtn As CustomLinkButton = sender.Parent

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Transaction No.", clbtn.RedirectParameter.TransactionNo)
        udtAuditLog.WriteLog(LogID.LOG00015, AuditLogDescription.LOG00015)

        clbtn.Redirect()

    End Sub

    Protected Sub ibtnDBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00010, AuditLogDescription.LOG00010)

        mvCore.ActiveViewIndex = ViewIndexCore.SearchResult
    End Sub

    Protected Sub ibtnDRecheck_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strAccountID As String = hfDEHealthAccountID.Value
        Dim strDocCode As String = hfDEHealthAccountDocumentType.Value

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Account ID", strAccountID)
        udtAuditLog.AddDescripton("Doc Code", strDocCode)
        udtAuditLog.WriteLog(LogID.LOG00011, AuditLogDescription.LOG00011)

        Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL
        udteHealthAccountDeathRecordBLL.RecheckDeathRecordMatchResult(strAccountID, strDocCode, GetCurrentUser, Nothing, Nothing)

        mvCore.ActiveViewIndex = ViewIndexCore.Finish

        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
        udcInfoMessageBox.Type = InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

    End Sub

    Private Sub BuildDetail(ByVal strAccountID As String, ByVal strDocCode As String)
        Dim ds As DataSet = (New eHealthAccountDeathRecordBLL).GetDeathRecordMatchResultDetail(strAccountID, strDocCode)
        Session(SESS.DeathRecordMatchResultDetail) = ds

        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        udtEHSAccountMaintBLL.EHSAccountRemoveFromSession(FunctionCode)

        BuildDetail()

    End Sub

    Private Sub BuildDetail()
        Dim ds As DataSet = Session(SESS.DeathRecordMatchResultDetail)
        If IsNothing(ds) Then Throw New Exception("Session Expired!")

        Dim dt As DataTable = ds.Tables(0)
        Dim dr As DataRow = dt.Rows(0)

        Dim udtFormatter As New Formatter

        ' Get the eHealth Account
        Dim udtEHSAccountMaintBLL As New eHSAccountMaintBLL
        Dim udtEHSAccount As EHSAccountModel = udtEHSAccountMaintBLL.EHSAccountGetFromSession(FunctionCode)

        ' If cannot retrieve from session, retrieve from database
        If IsNothing(udtEHSAccount) Then
            If dr("EHA_Acc_Type").ToString.Trim = AccountTypeClass.Validated Then
                udtEHSAccount = (New EHSAccountBLL).LoadEHSAccountByVRID(dr("EHA_Acc_ID").ToString.Trim)
            Else
                udtEHSAccount = (New EHSAccountBLL).LoadTempEHSAccountByVRID(dr("EHA_Acc_ID").ToString.Trim)
            End If

            udtEHSAccountMaintBLL.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)

        End If


        ' --- Hidden field ---
        hfDEHealthAccountID.Value = dr("EHA_Acc_ID").ToString.Trim
        hfDEHealthAccountDocumentType.Value = dr("EHA_Doc_Code").ToString.Trim
        hfDAccountType.Value = dr("EHA_Acc_Type").ToString.Trim


        ' --- eHealth Account Information ---

        udcReadOnlyDocumentType.Clear()
        udcReadOnlyDocumentType.EHSAccountModel = udtEHSAccount
        udcReadOnlyDocumentType.DocumentType = dr("EHA_Doc_Code").ToString.Trim
        udcReadOnlyDocumentType.MaskIdentityNo = hfDMaskDocumentNo.Value = YesNo.Yes
        udcReadOnlyDocumentType.Width = 180
        udcReadOnlyDocumentType.Width2 = 250
        udcReadOnlyDocumentType.ShowDateOfDeath = False
        udcReadOnlyDocumentType.ShowAccountID = True
        udcReadOnlyDocumentType.ShowAccountTypeAndStatus = True
        udcReadOnlyDocumentType.Build()


        ' --- Death Record Information ---

        ' Name
        lblDName.Text = dr("Death_English_Name").ToString.Trim

        If dr("Death_English_Name").ToString.Trim <> udtEHSAccount.getPersonalInformation(dr("EHA_Doc_Code").ToString.Trim).EName Then
            imgDName.Visible = True
        Else
            imgDName.Visible = False
        End If

        ' Death of Death
        lblDDateOfDeath.Text = udtFormatter.formatDOB(dr("DOD"), dr("Exact_DOD").ToString.Trim, String.Empty, Nothing, Nothing)
        hfDDateOfDeathType.Value = dr("Exact_DOD").ToString.Trim

        ' Death of Registration
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'lblDDateOfRegistration.Text = udtFormatter.formatDate(dr("DOR"), String.Empty)
        lblDDateOfRegistration.Text = udtFormatter.formatDisplayDate(dr("DOR"))
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        ' --- Matching Information ---

        ' Match Time
        lblDMatchTime.Text = udtFormatter.formatDateTime(dr("Match_Dtm"), String.Empty)

        ' With Claims
        Dim udtStaticDataBLL As New StaticDataBLL
        lblDWithClaim.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("YesNo", dr("With_Claim").ToString.Trim).DataValue

        ' With Suspicious Claims
        lblDWithSuspiciousClaim.Text = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("YesNo", dr("With_Suspicious_Claim").ToString.Trim).DataValue


        ' --- Transaction Information ---

        lblDSuspiciousTransactionText.Visible = False
        lblDTransactionText.Visible = False
        lblDNoTransaction.Visible = False
        gvDST.Visible = False

        If dr("EHA_Acc_Type").ToString.Trim = AccountTypeClass.Validated Then
            lblDSuspiciousTransactionText.Visible = True
        Else
            lblDTransactionText.Visible = True
        End If

        If ds.Tables(1).Rows.Count = 0 Then
            lblDNoTransaction.Visible = True

        Else
            gvDST.Visible = True

            gvDST.DataSource = ds.Tables(1)
            gvDST.DataBind()

        End If

    End Sub

    Private Function GetURLByFunctionCode(ByVal strFunctionCode As String) As String
        Dim dr() As DataRow = (New MenuBLL).GetMenuItemTable.Select(String.Format("Function_Code='{0}'", strFunctionCode))
        If dr.Length <> 1 Then Throw New Exception("eHealthAccountDeathRecordMatching.GetURLByFunctionCode: Unexpected no. of rows")
        Return dr(0)("URL")
    End Function

    '

    Protected Sub ibtnAFReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00016, AuditLogDescription.LOG00016)

        mvCore.ActiveViewIndex = ViewIndexCore.SearchCriteria

        ibtnSSearch_Click(Nothing, Nothing)
    End Sub

    '

    Protected Sub ibtnPopupUnmaskConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Confirm_Click
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00013, AuditLogDescription.LOG00013)

        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        Select Case mvCore.ActiveViewIndex
            Case ViewIndexCore.SearchResult
                gvMatchResult.Columns(4).Visible = True
                gvMatchResult.Columns(5).Visible = False

            Case ViewIndexCore.Detail
                hfDMaskDocumentNo.Value = YesNo.No
                udcReadOnlyDocumentType.Clear()
                udcReadOnlyDocumentType.MaskIdentityNo = hfDMaskDocumentNo.Value = YesNo.Yes
                udcReadOnlyDocumentType.Build()

        End Select

    End Sub

    Protected Sub ibtnPopupUnmaskCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Cancel_Click
        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        Select Case mvCore.ActiveViewIndex
            Case ViewIndexCore.SearchResult
                cboMaskDocumentNo.Checked = True

            Case ViewIndexCore.Detail
                cboDMaskDocumentNo.Checked = True

        End Select

    End Sub

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        If hfDEHealthAccountDocumentType.Value.Trim = String.Empty Then Return Nothing
        Return hfDEHealthAccountDocumentType.Value.Trim
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        If GetDocCode() Is Nothing Then Return Nothing
        Return (New eHSAccountMaintBLL).EHSAccountGetFromSession(FunctionCode)
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

#End Region

End Class