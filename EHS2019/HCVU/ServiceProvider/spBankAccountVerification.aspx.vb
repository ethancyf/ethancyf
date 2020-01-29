Imports Common.Component
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Security.AntiXss
Imports Common.ComObject
Imports Common.Component.HCVUUser
Imports Common.Component.Scheme
Imports Common.Component.Token
Imports Common.ComFunction

Partial Public Class spBankAccountVerification
    'Inherits System.Web.UI.Page
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Inherits BasePageWithGridView
    Inherits BasePageWithControl
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    'Dim action As Integer
    Dim df As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
    Dim formater As New Common.Format.Formatter
    Dim validator As New Common.Validation.Validator
    Const FUNCTION_CODE As String = FunctCode.FUNT010106
    Const COMMON_FUNCTION_CODE As String = FunctCode.FUNT990000
    Dim udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
    Dim reimbStatus As New ReimbursementStatus
    Dim udtBkVer As New BankAccVerificationBLL
    Dim udtHCVUUser As HCVUUserModel
    Dim udtHCVUUserBLL As New HCVUUserBLL
    Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
    Dim sm As SystemMessage
    Dim udtcomfunct As New Common.ComFunction.GeneralFunction
    Dim db As New Common.DataAccess.Database
    Dim udtSPAccountUpdateBll As New SPAccountUpdateBLL
    'Dim strServiceProviderModel As String = "BkServiceProviderModel"
    Dim strBankAccList As String = "SearchBankAccountList"
    Dim strBankAccByERN As String = "BankAccByERN"
    Dim strActiveTable As String = "ActiveTable"
    Dim strValidationFail As String = "ValidationFail"
    Dim strUpdateFail As String = "UpdateFail"

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    Dim _udtBLLSearchResult As BaseBLL.BLLSearchResult
    Dim _intSearchResultRowCount As Integer = 0
    Dim _blnOverrideResultLimit As Boolean = False

    Const SESS_FromTaskList As String = "010106_FromTaskList"
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    'CRE17-008 (Remind Delist Practice) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Const VS_POPUP_REMIND_DELIST_PRACTICE As String = "VS_POPUP_REMIND_DELIST_PRACTICE"
    'CRE17-008 (Remind Delist Practice) [End][Chris YIM]

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------    
    Private Enum EnrolmentAction
        Accept
        Defer
        Reject
        ReturnForAmendment
    End Enum
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            FunctionCode = FunctCode.FUNT010106

            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Bank Account Verification Loaded")

            Me.panel_searchCriteria.Visible = False
            Dim intPageSize As Integer
            Dim strvalue As String = String.Empty
            ' CRE11-007
            intPageSize = udtcomfunct.GetPageSizeHCVU()

            Me.gvBankVerificationList.PageSize = intPageSize

            'Shortcut from home page
            If Session("fromMain") = "Y" Then
                Session("fromMain") = Nothing
                'Search Result
                'Me.ddlStatus.SelectedValue = ""
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                Session(SESS_FromTaskList) = "Y"
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
                Me.panel_searchCriteria.Visible = True
                Me.ddlStatus.SelectedIndex = 0
                btn_search_Click(Nothing, Nothing)
            Else
                Me.ddlStatus.SelectedIndex = 1
            End If

            ddlSPHealthProf.DataSource = udtSPProfileBLL.GetHealthProf

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

            ' -----------------------------------------------------------------------------------------

            ddlSPHealthProf.DataValueField = "ServiceCategoryCode"
            If Session("language") = "zh-tw" Then
                ddlSPHealthProf.DataTextField = "ServiceCategoryDescChi"
            Else
                ddlSPHealthProf.DataTextField = "ServiceCategoryDesc"
            End If

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            ddlSPHealthProf.DataBind()

            Dim strEngDesc As String = String.Empty
            Dim strChiDesc As String = String.Empty
            Dim alUserCStatus As New ArrayList
            Dim ctrlDDLItem As ListItem
            Dim i As Integer
            alUserCStatus.Add("U")
            alUserCStatus.Add("D")

            For i = 0 To alUserCStatus.Count - 1
                Common.Component.Status.GetDescriptionFromDBCode(BankAcctVerifyStatus.ClassCode, alUserCStatus(i).ToString.Trim(), strEngDesc, strChiDesc)
                ctrlDDLItem = New ListItem(strEngDesc, alUserCStatus(i).ToString.Trim)
                Me.ddlStatus.Items.Add(ctrlDDLItem)
            Next

            ' Bind scheme information
            ddlScheme.DataSource = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
            ddlScheme.DataValueField = "SchemeCode"
            ddlScheme.DataTextField = "DisplayCode"
            ddlScheme.DataBind()

            ' Handle double post-back
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            'MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnDialogConfirm)

            'CRE17-008 (Remind Delist Practice) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ViewState(VS_POPUP_REMIND_DELIST_PRACTICE) = Nothing
            'CRE17-008 (Remind Delist Practice) [End][Chris YIM]
        End If

        'sm = New Common.ComObject.SystemMessage("990000", "Q", "00001")

        'Dim strRejectMessage As String
        'strRejectMessage = sm.GetMessage
        'Me.lblMsg.Text = strRejectMessage
        'Me.btn_Reject.OnClientClick = "showConfirm(this,'" + strRejectMessage + "'); return false;"        
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case Me.MultiView1.ActiveViewIndex
            Case 0
                'CRE17-008 (Remind Delist Practice) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If Not IsPopupShow() Then
                    ScriptManager1.SetFocus(Me.txt_enrolmentNo)
                    Me.pnlBkVerification.DefaultButton = Me.btn_search.ID
                End If
                'CRE17-008 (Remind Delist Practice) [End][Chris YIM]
            Case 1
                ScriptManager1.SetFocus(Me.btnHidden)

            Case 4
                ScriptManager1.SetFocus(Me.btnHidden)

        End Select
    End Sub

    Private Sub GetAllSelectedRow(ByRef selectedRow As ArrayList)

        Dim cb As CheckBox

        For Each row As GridViewRow In Me.gvBankVerificationList.Rows
            cb = CType(row.Cells(1).FindControl("chk_selected"), CheckBox)
            If cb.Checked = True Then
                selectedRow.Add(row)
            End If
        Next
    End Sub

    Private Sub DisplaySuccessfulMsg(ByRef udtAuditLogEntry As AuditLogEntry, ByVal strFailLogID As String)
        Dim dt, dtSPAccUpd As New DataTable
        Dim strErn As String
        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Dim IsUnsynchronizeRecord As Boolean = False
        Dim dtErrorMessage As DataTable = New DataTable("ErrorMessage")
        Dim IsNotValid As Boolean = False
        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
        dt = Session(strBankAccByERN)
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Try
            strErn = formater.formatSystemNumberReverse(Me.txt_currentERN.Text)

            dtSPAccUpd = udtBkVer.GetSPAccountUpdateByERN(strErn, db)

            '<1> Pass to professional verification
            '<2> Pass to Issue Token
            '<3> Pass to Scheme Enrolment
            '<4> Application Accept

            If dtSPAccUpd.Rows.Count = 1 Then
                If Trim(dtSPAccUpd.Rows(0)("Upd_Professional")).Equals("Y") Then
                    '<1> Pass to professional verification
                    If Not udtBkVer.AcceptBankAccountVerificationAndPassToNextProcess(strErn, SPAccountUpdateProgressStatus.ProfessionalVerification, _
                                                                                      udtHCVUUser.UserID, BankAcctVerifyStatus.Checked, dt) Then
                        'Fail
                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00001")
                    Else
                        'Successful
                        Dim strOld As String() = {"%s"}
                        Dim strNew As String() = {""}

                        If validator.IsEmpty(Me.lblSPID.Text) Then
                            strNew(0) = Me.lblEname.Text.Trim & Me.lblCname.Text.Trim & " [" & Me.txt_currentERN.Text & "]"
                        Else
                            strNew(0) = Me.lblEname.Text.Trim & Me.lblCname.Text.Trim & " [" & Me.lblSPID.Text & "]"
                        End If

                        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                        'Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00001", strOld, strNew)
                        Me.udcInfoMessageBox.AddMessage(COMMON_FUNCTION_CODE, "I", "00006", strOld, strNew)
                    End If

                    ' CRP13-002 - Fix professional verification [Start][Koala]
                    ' -------------------------------------------------------------------------------------
                ElseIf Trim(dtSPAccUpd.Rows(0)("Upd_Professional")).Equals("N") And dtSPAccUpd.Rows(0)("SP_ID") Is DBNull.Value Then
                    '<2> Pass to Issue Token
                    If Not udtBkVer.AcceptBankAccountVerificationAndPassToNextProcess(strErn, SPAccountUpdateProgressStatus.WaitingForIssueToken, _
                                                                                      udtHCVUUser.UserID, BankAcctVerifyStatus.Checked, dt) Then
                        'Fail
                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00001")
                    Else
                        'Successful
                        Dim strOld As String() = {"%s"}
                        Dim strNew As String() = {""}

                        If validator.IsEmpty(Me.lblSPID.Text) Then
                            strNew(0) = Me.lblEname.Text.Trim & Me.lblCname.Text.Trim & " [" & Me.txt_currentERN.Text & "]"
                        Else
                            strNew(0) = Me.lblEname.Text.Trim & Me.lblCname.Text.Trim & " [" & Me.lblSPID.Text & "]"
                        End If

                        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                        Me.udcInfoMessageBox.AddMessage(COMMON_FUNCTION_CODE, "I", "00007", strOld, strNew)
                    End If
                    ' CRP13-002 - Fix professional verification [End][Koala]

                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                ElseIf Trim(dtSPAccUpd.Rows(0)("Upd_Professional")).Equals("N") AndAlso Trim(dtSPAccUpd.Rows(0)("scheme_confirm")).Equals("Y") Then
                    '<3> Pass to Scheme Enrolment
                    If Not udtBkVer.AcceptBankAccountVerificationAndPassToNextProcess(strErn, SPAccountUpdateProgressStatus.WaitingForSchemeEnrolment, _
                                                                                      udtHCVUUser.UserID, BankAcctVerifyStatus.Checked, dt) Then
                        'Fail
                        Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00001")
                    Else
                        'Successful
                        Dim strOld As String() = {"%s"}
                        Dim strNew As String() = {""}

                        If validator.IsEmpty(Me.lblSPID.Text) Then
                            strNew(0) = Me.lblEname.Text.Trim & Me.lblCname.Text.Trim & " [" & Me.txt_currentERN.Text & "]"
                        Else
                            strNew(0) = Me.lblEname.Text.Trim & Me.lblCname.Text.Trim & " [" & Me.lblSPID.Text & "]"
                        End If

                        'Msg: The record of %s is proceeded to scheme enrolment.
                        Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                        Me.udcInfoMessageBox.AddMessage(COMMON_FUNCTION_CODE, SeverityCode.SEVI, MsgCode.MSG00046, strOld, strNew)
                    End If
                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' Remove partial accept flow (Pass to Scheme Enrolment to complete whole enrolment)

                    'ElseIf Trim(dtSPAccUpd.Rows(0)("Upd_Professional")).Equals("N") And Trim(dtSPAccUpd.Rows(0)("scheme_confirm")).Equals("Y") Then
                    '    '1. Get the new enrolled scheme (Master Scheme Code)
                    '    '2. Pass to Token / Scheme Management to complete the new scheme enrolment

                    '    '1 Start
                    '    Dim alEnrolledSchemeCode As New ArrayList
                    '    'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [Start][Karl]
                    '    Dim alEnrolledSuspendedSchemeCode As New ArrayList
                    '    'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [End][Karl]
                    '    Dim alNewSchemeCode As New ArrayList
                    '    Dim udtServiceProviderBLL As New ServiceProvider.ServiceProviderBLL

                    '    Dim udtSPM As ServiceProvider.ServiceProviderModel
                    '    udtSPM = udtServiceProviderBLL.GetServiceProviderStagingByERN(strErn, New Common.DataAccess.Database)

                    '    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                    '    '-----------------------------------------------------------------------------------------
                    '    Dim udtSP As ServiceProvider.ServiceProviderModel
                    '    udtSP = udtServiceProviderBLL.GetSP()
                    '    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '    '-----------------------------------------------------------------------------------------
                    '    'Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
                    '    'If Not udtSPProfileBLL.CheckUnsynchronizeRecord(udtSPM, udtSPPermenant) Then
                    '    If CheckValidationClickAccept(udtSP, dtErrorMessage) Then
                    '        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    '        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

                    '        If Not IsNothing(udtSPM) AndAlso Not IsNothing(udtSPM.SchemeInfoList) Then
                    '            For Each udtSchemeInfoModel As SchemeInformation.SchemeInformationModel In udtSPM.SchemeInfoList.Values
                    '                If udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Existing) Then
                    '                    If Not alEnrolledSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                    '                        alEnrolledSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                    '                    End If
                    '                    'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [Start][Karl]
                    '                    If Not alEnrolledSuspendedSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                    '                        alEnrolledSuspendedSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                    '                    End If
                    '                    'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [End][Karl]
                    '                ElseIf udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Active) Then
                    '                    If Not alNewSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                    '                        alNewSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                    '                    End If
                    '                    'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [Start][Karl]
                    '                ElseIf (udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Suspended) Or _
                    '                    udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.SuspendedPendingDelist) Or _
                    '                    udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.SuspendedPendingReactivate)) Then
                    '                    If Not alEnrolledSuspendedSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                    '                        alEnrolledSuspendedSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                    '                    End If
                    '                    'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [End][Karl]
                    '                Else
                    '                    'Do nothing
                    '                End If
                    '            Next
                    '        End If

                    '        If Not udtBkVer.PartiallyAcceptBankAccountVerificationByScheme(strErn, SPAccountUpdateProgressStatus.CompletionStageWithTokenIssued, _
                    '                                                                                                        udtHCVUUser.UserID, BankAcctVerifyStatus.Checked, dt, alEnrolledSchemeCode) Then
                    '            'Fail
                    '            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00001")
                    '        Else
                    '            'Successful
                    '            Dim strOld As String() = {"%s", "%n"}
                    '            Dim strNew As String() = {"", ""}

                    '            'Dim udtSchemeBLL As New Scheme.SchemeBLL
                    '            Dim udtSchemeBackOfficeBLL As New Scheme.SchemeBackOfficeBLL

                    '            Dim strEnrolledSchemeCode As String = String.Empty
                    '            Dim i As Integer
                    '            'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [Start][Karl]
                    '            'For i = 0 To alEnrolledSchemeCode.Count - 1
                    '            For i = 0 To alEnrolledSuspendedSchemeCode.Count - 1
                    '                'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [End][Karl]
                    '                Dim strTemp As String = String.Empty

                    '                'strTemp = udtSchemeBLL.getExternalSchemeCodeFromMasterSchemeCode(alEnrolledSchemeCode(i).Trim)
                    '                'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [Start][Karl]
                    '                'strTemp =  udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(alEnrolledSchemeCode(i).Trim).DisplayCode
                    '                strTemp = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(alEnrolledSuspendedSchemeCode(i).Trim).DisplayCode
                    '                'CRE14-002 PPIEPR Migration - Fix for enrol new scheme for suspended SP [End][Karl]
                    '                If strEnrolledSchemeCode.Equals(String.Empty) Then
                    '                    strEnrolledSchemeCode = strTemp
                    '                Else
                    '                    strEnrolledSchemeCode = strEnrolledSchemeCode & ", " & strTemp
                    '                End If
                    '            Next

                    '            strNew(0) = strEnrolledSchemeCode

                    '            Dim strNewSchemeCode As String = String.Empty
                    '            For i = 0 To alNewSchemeCode.Count - 1
                    '                Dim strTemp As String = String.Empty

                    '                'strTemp = udtSchemeBLL.getExternalSchemeCodeFromMasterSchemeCode(alNewSchemeCode(i).Trim)
                    '                strTemp = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup.Filter(alNewSchemeCode(i).Trim).DisplayCode
                    '                If strNewSchemeCode.Equals(String.Empty) Then
                    '                    strNewSchemeCode = strTemp
                    '                Else
                    '                    strNewSchemeCode = strNewSchemeCode & ", " & strTemp
                    '                End If
                    '            Next

                    '            strNew(1) = strNewSchemeCode

                    '            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                    '            Me.udcInfoMessageBox.AddMessage("990000", "I", "00016", strOld, strNew)
                    '        End If
                    '        '1 End


                    '        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                    '        '-----------------------------------------------------------------------------------------
                    '    Else
                    '        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '        '-----------------------------------------------------------------------------------------
                    '        'IsUnsynchronizeRecord = True
                    '        IsNotValid = True
                    '        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    '    End If
                    '    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]
                Else
                    '<4> Application Accept
                    Dim alEnrolledSchemeCode As New ArrayList
                    Dim alNewSchemeCode As New ArrayList
                    Dim udtServiceProviderBLL As New ServiceProvider.ServiceProviderBLL

                    Dim udtSPM As ServiceProvider.ServiceProviderModel
                    udtSPM = udtServiceProviderBLL.GetServiceProviderStagingByERN(strErn, New Common.DataAccess.Database)

                    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Dim udtSP As ServiceProvider.ServiceProviderModel
                    udtSP = udtServiceProviderBLL.GetSP()

                    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
                    'If Not udtSPProfileBLL.CheckUnsynchronizeRecord(udtSPM, udtSPPermenant) Then
                    If CheckValidationClickAccept(udtSP, dtErrorMessage) Then
                        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

                        If Not IsNothing(udtSPM) AndAlso Not IsNothing(udtSPM.SchemeInfoList) Then
                            For Each udtSchemeInfoModel As SchemeInformation.SchemeInformationModel In udtSPM.SchemeInfoList.Values
                                If udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Existing) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.ActivePendingDelist) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.ActivePendingSuspend) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Suspended) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.SuspendedPendingDelist) OrElse _
                                   udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.SuspendedPendingReactivate) Then

                                    If Not alEnrolledSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                                        alEnrolledSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                                    End If
                                ElseIf udtSchemeInfoModel.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Active) Then
                                    If Not alNewSchemeCode.Contains(udtSchemeInfoModel.SchemeCode) Then
                                        alNewSchemeCode.Add(udtSchemeInfoModel.SchemeCode)
                                    End If
                                Else
                                    'Do nothing
                                End If
                            Next
                        End If

                        If Not udtBkVer.AcceptBankAccountVerificationAndCompleteApplication(strErn, SPAccountUpdateProgressStatus.CompletionStageWithTokenIssued, _
                                                                                                                        udtHCVUUser.UserID, BankAcctVerifyStatus.Checked, dt, alEnrolledSchemeCode) Then
                            'Fail
                            Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00001")
                        Else
                            'Successful
                            Dim strOld As String() = {"%s"}
                            Dim strNew As String() = {""}

                            If validator.IsEmpty(Me.lblSPID.Text) Then
                                strNew(0) = Me.lblEname.Text.Trim & Me.lblCname.Text.Trim & " [" & Me.txt_currentERN.Text & "]"
                            Else
                                strNew(0) = Me.lblEname.Text.Trim & Me.lblCname.Text.Trim & " [" & Me.lblSPID.Text & "]"
                            End If

                            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                            'Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00001", strOld, strNew)
                            Me.udcInfoMessageBox.AddMessage("990000", "I", "00011", strOld, strNew)
                        End If

                        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                    Else
                        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        'IsUnsynchronizeRecord = True
                        IsNotValid = True
                        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    End If
                    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

                End If

                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If Not IsUnsynchronizeRecord Then
                If Not IsNotValid Then
                    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
                    Me.panel_searchCriteria.Visible = False
                    Me.MultiView1.ActiveViewIndex = 2
                    Me.udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, strFailLogID, "Update Fail")
                    Me.udcInfoMessageBox.BuildMessageBox()
                    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Accept Successful")
                Else
                    'udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Accept Successful")
                    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'udcErrorMessage.AddMessage(FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, "%s", "")
                    For Each drErrorMessage As DataRow In dtErrorMessage.Select()
                        If drErrorMessage.Item("IsReplace") Then
                            udcErrorMessage.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"), drErrorMessage.Item("FindString"), drErrorMessage.Item("ReplaceString"))
                        Else
                            udcErrorMessage.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"))
                        End If
                    Next
                    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    udcErrorMessage.BuildMessageBox(strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00022, "Accept abort")
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
                End If
                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
            Else
                'Throw a concurrent update exception from db
                Dim udtDB As New Common.DataAccess.Database
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, DBNull.Value)}
                udtDB.RunProc("proc_checkTSMP", params)
            End If
        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                udcErrorMessage.AddMessage(sm)
                udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, strFailLogID, "Update Fail")
            Else
                Throw eSQL
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub fillSearchCondition()
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
        Me.lbl_spValue.Text = DisplaySearchCriteria(AntiXssEncoder.HtmlEncode(txt_serviceProvider.Text, True))
        Me.lbl_SPID.Text = DisplaySearchCriteria(AntiXssEncoder.HtmlEncode(txt_SPID.Text, True))
        Me.lbl_serviceType.Text = AntiXssEncoder.HtmlEncode(ddlSPHealthProf.SelectedItem.Text, True)
        Me.lblResultPhone.Text = DisplaySearchCriteria(AntiXssEncoder.HtmlEncode(txt_dayTimeContact.Text, True))
        Me.lblResultStatu.Text = AntiXssEncoder.HtmlEncode(ddlStatus.SelectedItem.Text, True)
        Me.lbl_enrolmentNo.Text = DisplaySearchCriteria(AntiXssEncoder.HtmlEncode(txt_enrolmentNo.Text, True))
        Me.lbl_SPhkic.Text = DisplaySearchCriteria(AntiXssEncoder.HtmlEncode(txt_SPhkid.Text, True))
        Me.lblResultScheme.Text = AntiXssEncoder.HtmlEncode(ddlScheme.SelectedItem.Text, True)
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
    End Sub

    Private Function DisplaySearchCriteria(ByVal sc As String) As String
        Return IIf(validator.IsEmpty(sc), GetGlobalResourceObject("Text", "Any"), sc)
    End Function

    Protected Function GetPracticeTypeName(ByVal strPracticeCode As String) As String
        Dim strPracticeTypeName As String

        If strPracticeCode.Equals(String.Empty) Then
            strPracticeTypeName = String.Empty
        Else
            If Session("language") = "zh-tw" Then
                'strPracticeTypeName = udtEFormBLL.GetPracticeTypeName(strPracticeCode).DataValueChi
                strPracticeTypeName = strPracticeCode
            Else
                'strPracticeTypeName = udtEFormBLL.GetPracticeTypeName(strPracticeCode).DataValue
                strPracticeTypeName = strPracticeCode
            End If
        End If

        Return strPracticeTypeName
    End Function

    Protected Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String, ByVal strArea As String) As String
        Return formater.formatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrict, strArea)
    End Function

    Private Function GetBankAccountIndexInSession(ByVal strERN As String) As String
        Dim dt As New DataTable
        Dim i As Integer
        dt = Session(strBankAccList)
        For i = 0 To dt.Rows.Count - 1
            If Trim(dt.Rows(i)("enrolRefNo").ToString).Equals(strERN) Then
                Return i
            End If
        Next

        Return -1
    End Function

    Private Sub LoadBankAccountGrid(ByVal strEnrolmentRef As String, ByVal strStatus As String)
        Dim udtSp As New Common.Component.ServiceProvider.ServiceProviderModel
        Dim i As Integer
        'Dim db As New Common.DataAccess.Database

        Dim udtSpBll As New Common.Component.ServiceProvider.ServiceProviderBLL
        'udtSp = udtSpBll.GetServiceProviderEnrolmentProfileByERN(strEnrolmentRef, db)
        udtSp = udtSpBll.GetServiceProviderStagingProfileByERNSPIDSPHKID(strEnrolmentRef, db)

        'Session(Me.strServiceProviderModel) = udtSp

        udtSpBll.SaveToSession(udtSp)

        If Not IsNothing(udtSp) Then
            Me.lblEnrolRefNo.Text = formater.formatSystemNumber(udtSp.EnrolRefNo)

            Me.lblSubmissionDate.Text = formater.convertDateTime(udtSp.EnrolDate)
            Me.lblEname.Text = udtSp.EnglishName

            If IsDBNull(udtSp.ChineseName) Then
                Me.lblCname.Text = ""
            Else
                Me.lblCname.Text = formater.formatChineseName(udtSp.ChineseName)
            End If

            Me.lblHKID.Text = formater.formatHKID(udtSp.HKID, False)
            Me.lblAddress.Text = formater.formatAddress(udtSp.SpAddress.Room, udtSp.SpAddress.Floor, udtSp.SpAddress.Floor, _
                                                        udtSp.SpAddress.Building, udtSp.SpAddress.District, udtSp.SpAddress.AreaCode)
            Me.lblEmail.Text = udtSp.Email
            Me.lblContactNo.Text = udtSp.Phone
            Me.lblFax.Text = udtSp.Fax

            'Dim strEng, strChi As String
            'strEng = ""
            'strChi = ""

            'ServiceProviderStagingStatus.GetDescriptionFromDBCode(ServiceProviderStagingStatus.ClassCode, udtSp.RecordStatus, strEng, strChi)
            'Me.lblRecordStatus.Text = udtSp.
            If validator.IsEmpty(udtSp.SPID) Then
                Me.lblSPID.Text = ""
                Me.lblSPID.Visible = False
                Me.lblSPIDLabel.Visible = False
            Else
                Me.lblSPID.Text = udtSp.SPID
                Me.lblSPID.Visible = True
                Me.lblSPIDLabel.Visible = True
            End If

            gvCompletePracticeBank.DataSource = udtSp.PracticeList
            gvCompletePracticeBank.DataBind()
            gvCompletePracticeBank.Visible = True
            Me.panel_searchCriteria.Visible = True
            Me.MultiView1.ActiveViewIndex = 4
        Else
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcInfoMessageBox.AddMessage(COMMON_FUNCTION_CODE, "I", "00001")
            Me.panel_searchCriteria.Visible = False
            Me.MultiView1.ActiveViewIndex = 0
        End If

        AdjustButtonBehaviour(strEnrolmentRef, strStatus)   'udtSp.RecordStatus
    End Sub

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Private Sub LoadTranDetails(Optional ByVal bShowInGridView As Boolean = False)
    Private Sub LoadTranDetails(Optional ByVal bShowInGridView As Boolean = False, Optional ByVal blnOverrideResultLimit As Boolean = False)
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
        Dim dt As DataTable
        Dim dv As DataView
        'Dim i As Integer
        'Dim alERNList As New ArrayList

        Dim criteria As New Common.SearchCriteria.SearchCriteria

        If validator.IsEmpty(Me.txt_enrolmentNo.Text) Then
            criteria.EnrolmentRefNo = Me.txt_enrolmentNo.Text
        Else
            criteria.EnrolmentRefNo = formater.formatSystemNumberReverse(Me.txt_enrolmentNo.Text)
        End If
        criteria.ServiceProviderID = Me.txt_SPID.Text
        criteria.ServiceProviderHKIC = formater.formatHKIDInternal(Me.txt_SPhkid.Text.Trim)
        criteria.ServiceProviderName = Me.txt_serviceProvider.Text.Trim
        criteria.HealthProf = Trim(Me.ddlSPHealthProf.SelectedValue)
        criteria.BankStatus = Me.ddlStatus.SelectedValue

        'criteria.BankAcctSubmissionDate = IIf(Me.txt_submissionDate.Text.Equals(""), Nothing, Me.txt_submissionDate.Text)

        Dim searchEngine As New SearchEngineBLL

        Try
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'dt = searchEngine.SearchBank(criteria, Me.txt_dayTimeContact.Text, Me.ddlScheme.SelectedValue)

            _udtBLLSearchResult = searchEngine.SearchBank(FUNCTION_CODE, criteria, Me.txt_dayTimeContact.Text, Me.ddlScheme.SelectedValue, blnOverrideResultLimit)

            If _udtBLLSearchResult.SqlErrorMessage = BaseBLL.EnumSqlErrorMessage.Normal Then
                dt = CType(_udtBLLSearchResult.Data, DataTable)
                _intSearchResultRowCount = dt.Rows.Count
            Else
                Return
            End If

            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            'dt.Columns.Add(New System.Data.DataColumn("Scheme_Name", GetType(String)))
            'Dim i As Integer
            'Dim udtSPProfileBLL As New SPProfileBLL
            'Dim strSchemeNameInGrid As String = String.Empty

            'For i = 0 To dt.Rows.Count - 1
            '    dt.Rows(i)("Scheme_Name") = udtSPProfileBLL.GetSPSchemeInformationStagingSchemeName(formater.formatSystemNumberReverse(dt.Rows(i)("enrolRefNo").ToString.Trim))
            'Next

            Session(strBankAccList) = dt

            Me.txt_recordCount.Text = dt.Rows.Count

            If dt.Rows.Count = 0 Then
                'If dt.Rows.Count = 0 Then
                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                Me.udcInfoMessageBox.AddMessage(COMMON_FUNCTION_CODE, "I", "00001")
                Me.udcInfoMessageBox.BuildMessageBox()

                Me.panel_searchCriteria.Visible = False
                Me.MultiView1.ActiveViewIndex = 0
                'Me.txt_currentRecordNo.Text = "-1"
                Me.txt_currentERN.Text = ""
            ElseIf dt.Rows.Count = 1 AndAlso (Not Me.txt_SPhkid.Text.Trim.Equals(String.Empty) Or _
                Not Me.txt_enrolmentNo.Text.Trim.Equals(String.Empty) Or Not Me.txt_SPID.Text.Trim.Equals(String.Empty)) Then
                'Handle search case with unique field and with 1 result found
                DisplayRecordDetails(Trim(dt.Rows(0)("enrolRefNo")), Trim(dt.Rows(0)("bankStatus")))
                'Dim strEng, strChi As String
                'strEng = ""
                'strChi = ""
                'BankAcctVerifyStatus.GetDescriptionFromDBCode(BankAcctVerifyStatus.ClassCode, BankAcctVerifyStatus.Defer, strEng, strChi)

                'If Trim(dt.Rows(0)("bankStatus")).Equals(strEng) Then
                '    LoadBankAccountGrid(formater.formatSystemNumberReverse(Trim(dt.Rows(0)("enrolRefNo"))), BankAcctVerifyStatus.Defer)
                '    Me.lblStatus2.Text = strEng
                'Else
                '    LoadBankAccountGrid(formater.formatSystemNumberReverse(Trim(dt.Rows(0)("enrolRefNo"))), BankAcctVerifyStatus.Active)
                '    BankAcctVerifyStatus.GetDescriptionFromDBCode(BankAcctVerifyStatus.ClassCode, BankAcctVerifyStatus.Active, strEng, strChi)
                '    Me.lblStatus2.Text = strEng
                'End If
                'Me.udcInfoMessageBox.BuildMessageBox()
                'Me.txt_currentERN.Text = Trim(dt.Rows(0)("enrolRefNo"))
                'Me.MultiView1.ActiveViewIndex = 4
            Else
                'Multiple records found, show the grid view for selection
                'Me.txt_currentRecordNo.Text = ""
                Me.txt_currentERN.Text = ""

                Me.GridViewDataBind(Me.gvBankVerificationList, dt, "enrolRefNo", "ASC", False)

                'dv = New DataView(dt)
                'Me.gvBankVerificationList.DataSource = dv
                'Me.gvBankVerificationList.DataBind()

                Me.panel_searchCriteria.Visible = True
                Me.MultiView1.ActiveViewIndex = 1
            End If
        Catch eSQL As SqlClient.SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        'Me.gvBankVerificationList.PageIndex = e.NewPageIndex
        'LoadTranDetails()
        Me.GridViewPageIndexChangingHandler(sender, e, strBankAccList)
        'Me.GridView_PageIndexChanging_Handler(Me.gvBankVerificationList, strBankAccList, e.NewPageIndex)
    End Sub

    Protected Sub GridView1_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If Not (e.CommandName.ToUpper.Equals("PAGE") Or e.CommandName.ToUpper.Equals("SORT")) Then

            Dim strERN, strStatus As String
            Dim row As GridViewRow = CType(CType(e.CommandSource, Control).NamingContainer, GridViewRow)
            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            strERN = AntiXssEncoder.HtmlEncode(Trim(CType(row.Cells(1).FindControl("hfERN"), HiddenField).Value), True)
            ' I-CRE16-003 Fix XSS [End][Lawrence]
            strStatus = Trim(row.Cells(7).Text)

            Dim strSPID As String = Trim(CType(row.Cells(2).FindControl("lbtn_spid"), LinkButton).Text)
            strSPID = IIf(strSPID.Equals(GetGlobalResourceObject("Text", "N/A")), "", strSPID)

            Dim strSPHKIC As String = CStr(CType(row.FindControl("lblRSPHKID"), Label).Text)

            strSPHKIC = formater.formatHKIDInternal(strSPHKIC)

            'add audit log
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.AddDescripton("ERN", formater.formatSystemNumberReverse(strERN))
            udtAuditLogEntry.AddDescripton("SPID", strSPID)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Select", New Common.ComObject.AuditLogInfo(strSPID, strSPHKIC, "", "", "", ""))

            Try
                DisplayRecordDetails(strERN, strStatus)
                'Me.udcInfoMessageBox.BuildMessageBox()

                'Dim strERN_Long As String = String.Empty
                'strERN_Long = formater.formatSystemNumberReverse(strERN.Trim)

                ''Handle concurrent update start
                'Dim bUpdatedByOthers As Boolean = False
                'Dim strMessageCode As String = String.Empty
                'Dim strProgress As String = String.Empty
                'strProgress = udtSPProfileBLL.GetEnrolmentProcessStatus(strERN_Long.Trim)

                'If Not strProgress.Equals(String.Empty) Then
                '    If Not strProgress.Equals("00005") Then
                '        bUpdatedByOthers = True
                '        strMessageCode = strProgress
                '    End If
                'Else
                '    bUpdatedByOthers = True
                '    strMessageCode = "00015"
                'End If

                'If bUpdatedByOthers Then
                '    Dim udtServiceProviderBLL As Common.Component.ServiceProvider.ServiceProviderBLL = New Common.Component.ServiceProvider.ServiceProviderBLL
                '    Dim udtSP As Common.Component.ServiceProvider.ServiceProviderModel = Nothing

                '    udtSP = udtServiceProviderBLL.GetServiceProviderStagingByERN_NoReader(strERN_Long.Trim, New Common.DataAccess.Database)

                '    Dim strOld As String() = {"%s"}
                '    Dim strNew As String() = {""}

                '    If udtSP.SPID.Equals(String.Empty) Then
                '        strNew(0) = udtSP.EnglishName + formater.formatChineseName(udtSP.ChineseName) + " [" + _
                '                    formater.formatSystemNumber(udtSP.EnrolRefNo) + "] "
                '    Else
                '        strNew(0) = udtSP.EnglishName + formater.formatChineseName(udtSP.ChineseName) + " [" + _
                '                    udtSP.SPID + "] "
                '    End If

                '    udcInfoMessageBox.AddMessage("990000", "I", strMessageCode, strOld, strNew)
                '    udcInfoMessageBox.BuildMessageBox()
                '    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

                '    Me.MultiView1.ActiveViewIndex = 2
                'Else
                '    strEng = ""
                '    strChi = ""

                '    Dim searchEngine As New SearchEngineBLL
                '    Dim dt As New DataTable
                '    dt = searchEngine.SearchBankTSMP(strERN_Long.Trim)

                '    Session(strBankAccByERN) = dt

                '    BankAcctVerifyStatus.GetDescriptionFromDBCode(BankAcctVerifyStatus.ClassCode, BankAcctVerifyStatus.Defer, strEng, strChi)

                '    If strStatus.Equals(strEng) Then
                '        LoadBankAccountGrid(formater.formatSystemNumberReverse(strERN), BankAcctVerifyStatus.Defer)  ', Trim(Me.txt_SPID.Text), Trim(Me.txt_SPhkid.Text), Trim(Me.txt_serviceProvider.Text)
                '    Else
                '        LoadBankAccountGrid(formater.formatSystemNumberReverse(strERN), BankAcctVerifyStatus.Active)
                '    End If

                '    Me.lblStatus2.Text = strStatus

                '    'AdjustButtonBehaviour(strERN)

                '    'Me.txt_currentRecordNo.Text = Me.GetBankAccountIndexInSession(strERN)
                '    Me.udcInfoMessageBox.BuildMessageBox()
                '    Me.txt_currentERN.Text = strERN
                '    Me.MultiView1.ActiveViewIndex = 4
                'End If

                udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Select successful")
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Select fail")
                Throw ex
            End Try
        End If
    End Sub

    Private Sub DisplayRecordDetails(ByVal strERN As String, ByVal strStatus As String)
        Dim strEng, strChi As String
        Dim strERN_Long As String = String.Empty
        strERN_Long = formater.formatSystemNumberReverse(strERN.Trim)

        udtSPProfileBLL.ClearSession()

        'Handle concurrent update start
        Dim bUpdatedByOthers As Boolean = False
        Dim strMessageCode As String = String.Empty
        Dim strProgress As String = String.Empty
        strProgress = udtSPProfileBLL.GetEnrolmentProcessStatus(strERN_Long.Trim)

        If IsNothing(strProgress) Then
            ' Fix for UAT Only
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            sm = New Common.ComObject.SystemMessage("990000", "E", "00184")
            udcErrorMessage.AddMessage(sm)
            udcErrorMessage.BuildMessageBox(strValidationFail, udtAuditLogEntry, LogID.LOG00003, "Search Fail")

            Me.MultiView1.ActiveViewIndex = 2
            Return

        Else
            If Not strProgress.Equals(String.Empty) Then
                If Not strProgress.Equals("00005") Then
                    bUpdatedByOthers = True
                    strMessageCode = strProgress
                End If
            Else
                bUpdatedByOthers = True
                strMessageCode = "00015"
            End If
        End If



        If bUpdatedByOthers Then
            Dim udtServiceProviderBLL As Common.Component.ServiceProvider.ServiceProviderBLL = New Common.Component.ServiceProvider.ServiceProviderBLL
            Dim udtSP As Common.Component.ServiceProvider.ServiceProviderModel = Nothing

            udtSP = udtServiceProviderBLL.GetServiceProviderStagingByERN(strERN_Long.Trim, New Common.DataAccess.Database)

            Dim strOld As String() = {"%s"}
            Dim strNew As String() = {""}

            If udtSP.SPID.Equals(String.Empty) Then
                strNew(0) = udtSP.EnglishName + formater.formatChineseName(udtSP.ChineseName) + " [" + _
                            formater.formatSystemNumber(udtSP.EnrolRefNo) + "] "
            Else
                strNew(0) = udtSP.EnglishName + formater.formatChineseName(udtSP.ChineseName) + " [" + _
                            udtSP.SPID + "] "
            End If

            udcInfoMessageBox.AddMessage("990000", "I", strMessageCode, strOld, strNew)
            udcInfoMessageBox.BuildMessageBox()
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information

            Me.MultiView1.ActiveViewIndex = 2
        Else
            strEng = ""
            strChi = ""

            Dim searchEngine As New SearchEngineBLL
            Dim dt As New DataTable
            dt = searchEngine.SearchBankTSMP(strERN_Long.Trim)

            Session(strBankAccByERN) = dt

            BankAcctVerifyStatus.GetDescriptionFromDBCode(BankAcctVerifyStatus.ClassCode, BankAcctVerifyStatus.Defer, strEng, strChi)

            If strStatus.Equals(strEng) Then
                LoadBankAccountGrid(formater.formatSystemNumberReverse(strERN), BankAcctVerifyStatus.Defer)
            Else
                LoadBankAccountGrid(formater.formatSystemNumberReverse(strERN), BankAcctVerifyStatus.Active)
            End If

            Me.lblStatus2.Text = strStatus

            'AdjustButtonBehaviour(strERN)

            'Me.txt_currentRecordNo.Text = Me.GetBankAccountIndexInSession(strERN)
            Me.txt_currentERN.Text = strERN
            Me.MultiView1.ActiveViewIndex = 4
            'Me.udcInfoMessageBox.BuildMessageBox()
        End If

        'Me.udcInfoMessageBox.BuildMessageBox()
    End Sub

    Private Sub AdjustButtonBehaviour(ByVal strERN As String, ByVal strStatus As String)
        Dim udtSPVerificationModel As ServiceProviderVerificationModel = New ServiceProviderVerificationModel
        Dim udtSPVerificationBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL
        udtSPVerificationModel = udtSPVerificationBLL.GetSerivceProviderVerificationByERN(strERN, db)
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        'Adjust the buttons based on UserA & UserB
        If udtSPVerificationModel.EnterConfirmBy.ToUpper.Equals(Trim(udtHCVUUser.UserID).ToUpper) Then
            'Disabled all the action buttons + show information
            Me.btnDefer.Visible = False
            Me.btnDefer_Disabled.Visible = True
            Me.btnAccept.Visible = False
            Me.btnAccept_Disabled.Visible = True
            Me.btn_Reject.Visible = False
            Me.btn_Reject_Disabled.Visible = True
            Me.btnReturnForAmendment.Visible = False
            Me.btnReturnForAmendment_Disabled.Visible = True

            'Amended by Kathy 03072008
            'The function code of system message had changed from '010016-I-00004' to '990000-I-00012'
            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcInfoMessageBox.AddMessage(COMMON_FUNCTION_CODE, "I", "00012")
            Me.udcInfoMessageBox.BuildMessageBox()
            'end 03072008
        ElseIf udtSPVerificationModel.VettingBy.ToUpper.Equals(Trim(udtHCVUUser.UserID).ToUpper) Then
            'Disabled all the action buttons + show information
            Me.btnDefer.Visible = False
            Me.btnDefer_Disabled.Visible = True
            Me.btnAccept.Visible = False
            Me.btnAccept_Disabled.Visible = True
            Me.btn_Reject.Visible = False
            Me.btn_Reject_Disabled.Visible = True
            Me.btnReturnForAmendment.Visible = False
            Me.btnReturnForAmendment_Disabled.Visible = True

            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            Me.udcInfoMessageBox.AddMessage(COMMON_FUNCTION_CODE, "I", "00014")
            Me.udcInfoMessageBox.BuildMessageBox()
        Else
            Me.btnDefer.Visible = True
            Me.btnDefer_Disabled.Visible = False
            Me.btnAccept.Visible = True
            Me.btnAccept_Disabled.Visible = False
            Me.btn_Reject.Visible = True
            Me.btn_Reject_Disabled.Visible = False
            Me.btnReturnForAmendment.Visible = True
            Me.btnReturnForAmendment_Disabled.Visible = False

            'Adjust the buttons by status
            If Trim(strStatus).Equals(Common.Component.BankAcctVerifyStatus.Reject) Then
                Me.btnDefer.Visible = False
                Me.btnDefer_Disabled.Visible = True
                Me.btnAccept.Visible = False
                Me.btnAccept_Disabled.Visible = True
                Me.btn_Reject.Visible = False
                Me.btn_Reject_Disabled.Visible = True
                Me.btnReturnForAmendment.Visible = False
                Me.btnReturnForAmendment_Disabled.Visible = True
            Else
                If Trim(strStatus).Equals(Common.Component.BankAcctVerifyStatus.Defer) Then
                    Me.btnDefer.Visible = False
                    Me.btnDefer_Disabled.Visible = True
                Else
                    Me.btnDefer.Visible = True
                    Me.btnDefer_Disabled.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub gvBankVerificationList_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim ctrlSPID As LinkButton
            Dim ctrlHKID As Label
            ctrlSPID = CType(e.Row.Cells(2).FindControl("lbtn_spid"), LinkButton)
            ctrlHKID = CType(e.Row.Cells(3).FindControl("lblRSPHKID"), Label)
            Dim ctrlVettingDtm As Label = CType(e.Row.Cells(4).FindControl("lblVettingProcessingDtm"), Label)
            Dim lblRScheme As Label = CType(e.Row.Cells(8).FindControl("lblRScheme"), Label)



            ctrlHKID.Text = formater.formatHKID(ctrlHKID.Text, False)
            ctrlVettingDtm.Text = formater.formatDateTime(ctrlVettingDtm.Text, "EN")

            ' Enrolment Reference No.
            Dim lnkbtnERN As LinkButton = CType(e.Row.FindControl("lbtn_enrolRef"), LinkButton)


            If ctrlSPID.Text.Trim.Equals(String.Empty) Then
                ctrlSPID.Text = Me.GetGlobalResourceObject("Text", "N/A")
                ctrlSPID.CommandArgument = String.Empty
                ctrlSPID.Enabled = False
            Else
                'lbtn_spid.OnClientClick = "javascript:getERN('" & lbtn_enrolRef.CommandArgument.Trim & "')"
                'lbtn_spid.Attributes.Add("onclick", "return false;")
            End If

        End If
    End Sub

    Private Sub MarkSecondVerify()
        'CRE17-008 (Remind Delist Practice) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim strERN As String = formater.formatSystemNumberReverse(Me.lblEnrolRefNo.Text)
        Dim strSPID As String = Me.lblSPID.Text

        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        If ViewState(VS_POPUP_REMIND_DELIST_PRACTICE) Is Nothing Then
            'Add Audit Log
            udtAuditLogEntry.AddDescripton("ERN", strERN)
            udtAuditLogEntry.AddDescripton("SPID", strSPID)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Accept")

            If CheckToShowReminderForDelistPractice(strSPID, strERN) Then
                Me.ModalPopupExtenderRemindDelistPractice.Show()
                udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "Reminder - Delist Old Practice - Show")
                Return
            End If
        End If

        'CRE17-008 (Remind Delist Practice) [End][Chris YIM]

        Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL
        Dim udtServiceProvider As ServiceProvider.ServiceProviderModel = New ServiceProvider.ServiceProviderModel
        udtServiceProvider = udtSPBLL.GetSP()

        Me.panel_searchCriteria.Visible = False
        Try
            DisplaySuccessfulMsg(udtAuditLogEntry, LogID.LOG00009)

        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Accept Fail")
            Throw ex
        End Try


    End Sub

    Protected Sub btn_nextSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcErrorMessage.Visible = False
        Response.Redirect("spBankAccountVerification.aspx")
    End Sub

    Protected Sub btn_back1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Me.udtSPProfileBLL.ClearSession()


        Me.udcInfoMessageBox.Visible = False
        Me.udcErrorMessage.Visible = False

        If Me.txt_recordCount.Text.Equals("1") Then
            Me.txt_recordCount.Text = "0"
            Me.MultiView1.ActiveViewIndex = 0
        Else
            'CRE17-008 (Remind Delist Practice) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _blnOverrideResultLimit = True
            'CRE17-008 (Remind Delist Practice) [End][Chris YIM]
            Me.btn_search_Click(Nothing, Nothing)
            Me.MultiView1.ActiveViewIndex = 1

        End If

        If MultiView1.ActiveViewIndex = 0 Then
            ResetSearchCriteria()
        End If

        'CRE17-008 (Remind Delist Practice) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ViewState(VS_POPUP_REMIND_DELIST_PRACTICE) = Nothing
        'CRE17-008 (Remind Delist Practice) [End][Chris YIM]
    End Sub

    Private Sub ReturnForAmendment()

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("ERN", formater.formatSystemNumberReverse(Me.lblEnrolRefNo.Text))
        udtAuditLogEntry.AddDescripton("SPID", Me.lblSPID.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "Return for amendment Start")

        'Dim udtSp As New Common.Component.ServiceProvider.ServiceProviderModel
        Dim lstBankTsmp As New List(Of Byte())
        Dim lstBankDisplaySeq As New List(Of Integer)
        Dim lstSPPracticeDisplaySeq As New List(Of Integer)
        Dim dt As New DataTable
        Dim strErn As String
        Dim i As Integer
        Dim tsmpSPAccUpdate As Byte() = Nothing

        'dt = Session(strBankAccList)
        dt = Session(strBankAccByERN)
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        For i = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("enrolRefNo").Equals(Me.txt_currentERN.Text) Then
                lstBankTsmp.Add(dt.Rows(i)("BankAccVerTSMP"))
                lstBankDisplaySeq.Add(dt.Rows(i)("displaySeq"))
                lstSPPracticeDisplaySeq.Add(dt.Rows(i)("sp_practice_display_Seq"))
                tsmpSPAccUpdate = dt.Rows(i)("tsmp")
            End If
        Next

        Try
            'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            strErn = formater.formatSystemNumberReverse(Me.txt_currentERN.Text)

            Dim udtServiceProviderBLL As New ServiceProvider.ServiceProviderBLL

            Dim udtSP As ServiceProvider.ServiceProviderModel
            udtSP = udtServiceProviderBLL.GetSP()

            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Dim udtSPStaging As ServiceProvider.ServiceProviderModel
            'udtSPStaging = udtServiceProviderBLL.GetServiceProviderStagingByERN(strErn, New Common.DataAccess.Database)

            'Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
            'If Not udtSPProfileBLL.CheckUnsynchronizeRecord(udtSPStaging, udtSPPermenant) Then
            Dim dtErrorMessage As DataTable = New DataTable("ErrorMessage")

            If CheckValidationClickReturnForAmendment(udtSP, dtErrorMessage) Then
                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
                udtSPProfileBLL.ReturnForAmendmentFromUserC(strErn, udtHCVUUser.UserID, tsmpSPAccUpdate, lstBankTsmp, lstBankDisplaySeq, lstSPPracticeDisplaySeq)

                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00002")
                Me.panel_searchCriteria.Visible = False
                Me.MultiView1.ActiveViewIndex = 2
                Me.udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00018, "Return for amendment Fail")
                Me.udcInfoMessageBox.BuildMessageBox()
                udtAuditLogEntry.WriteEndLog(LogID.LOG00017, "Return for amendment Successful")
                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
            Else
                'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Me.udcErrorMessage.AddMessage(FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, "%s", "")
                For Each drErrorMessage As DataRow In dtErrorMessage.Select()
                    If drErrorMessage.Item("IsReplace") Then
                        Me.udcErrorMessage.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"), drErrorMessage.Item("FindString"), drErrorMessage.Item("ReplaceString"))
                    Else
                        Me.udcErrorMessage.AddMessage(drErrorMessage.Item("FunctionCode"), drErrorMessage.Item("SeverityCode"), drErrorMessage.Item("MessageCode"))
                    End If
                Next
                'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                Me.udcErrorMessage.BuildMessageBox(strValidationFail, udtAuditLogEntry, Common.Component.LogID.LOG00023, "Return For Amendment abort")
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
            End If
            'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                udcErrorMessage.AddMessage(sm)
                udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00018, "Return for amendment Fail")
            Else
                udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Return for amendment Fail")
                Throw eSQL
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Return for amendment Fail")
            Throw ex
        End Try
    End Sub

    Protected Sub btn_backFromTemp_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00022, "Back Click")
        ' CRE11-021 log the missed essential information [End]

        If Me.txt_recordCount.Text.Equals("1") Then
            Me.MultiView1.ActiveViewIndex = 0
            Me.panel_searchCriteria.Visible = False
        Else
            Me.MultiView1.ActiveViewIndex = 1
            Me.panel_searchCriteria.Visible = True
        End If
        Me.udcErrorMessage.BuildMessageBox()
        Me.udcInfoMessageBox.BuildMessageBox()

    End Sub

    Protected Sub btn_searchSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Open the search SP form
    End Sub

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
#Region "Abstract Method of [HCVU.BasePageWithControl]"
    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)
        Session(strBankAccList) = Nothing
        Me.gvBankVerificationList.PageIndex = 0

        fillSearchCondition()
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As AuditLogEntry) As Boolean
        Return True
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        If blnOverrideResultLimit Then
            LoadTranDetails(False, True)
        Else
            LoadTranDetails()
        End If

        Return _udtBLLSearchResult
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer
        Return _intSearchResultRowCount
    End Function

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_ConfirmSearch_Click()
        If Not IsNothing(Session(SESS_FromTaskList)) Then
            If Session(SESS_FromTaskList) = "Y" Then
                Me.ddlStatus.SelectedIndex = 0
                Session(SESS_FromTaskList) = Nothing
            End If
        End If

        _blnOverrideResultLimit = True
        btn_search_Click(Nothing, Nothing)
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub
#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    Protected Sub btn_search_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        ' CRE12-015 Add the respective practice number in “Practice” in the functions under “Reimbursement” in eHS [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------------------------------------------
        ' Implement Collapsible Search Criteria Review
        udcCollapsibleSearchCriteriaReview.Collapsed = True
        udcCollapsibleSearchCriteriaReview.ClientState = "True"
        ' CRE12-015 Add the respective practice number in “Practice” in the functions under “Reimbursement” in eHS [End][Tommy L]

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Session(strBankAccList) = Nothing
        'Me.gvBankVerificationList.PageIndex = 0
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

        Dim strHKIC As String = String.Empty
        strHKIC = Me.formater.formatHKIDInternal(Me.txt_SPhkid.Text)
        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("ERN", Me.txt_enrolmentNo.Text)
        udtAuditLogEntry.AddDescripton("SPID", Me.txt_SPID.Text)
        udtAuditLogEntry.AddDescripton("SP HKID", Me.txt_SPhkid.Text)
        udtAuditLogEntry.AddDescripton("SP Name", Me.txt_serviceProvider.Text)
        udtAuditLogEntry.AddDescripton("Phone", Me.txt_dayTimeContact.Text)
        udtAuditLogEntry.AddDescripton("Profession", Me.ddlSPHealthProf.SelectedItem.Value)
        udtAuditLogEntry.AddDescripton("Status", Me.ddlStatus.SelectedItem.Value)
        udtAuditLogEntry.AddDescripton("Scheme", Me.ddlScheme.SelectedItem.Value)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Search", New AuditLogInfo(Me.txt_SPID.Text, strHKIC, "", "", "", ""))

        Try
            Dim enumSearchResult As SearchResultEnum

            udcInfoMessageBox.Visible = False
            udcErrorMessage.Visible = False

            If IsNothing(sender) AndAlso _blnOverrideResultLimit = True Then
                enumSearchResult = StartSearchFlow(FUNCTION_CODE, udtAuditLogEntry, udcErrorMessage, Nothing, False, True)
            Else
                enumSearchResult = StartSearchFlow(FUNCTION_CODE, udtAuditLogEntry, udcErrorMessage, Nothing)
            End If

            Select Case enumSearchResult
                Case SearchResultEnum.Success
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Search successful")

                Case SearchResultEnum.ValidationFail
                    ' No Validation
                    Throw New Exception("Error: Class = [HCVU.spBankAccountVerification], Method = [btn_search_Click], Message = The method - [SF_ValidateSearch] should not return [False]")

                Case SearchResultEnum.NoRecordFound
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00019, "Search failed: No Record")

                Case SearchResultEnum.OverResultList1stLimit_PopUp
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search Fail")

                Case SearchResultEnum.OverResultList1stLimit_Alert
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search Fail")

                Case SearchResultEnum.OverResultListOverrideLimit
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search Fail")

                Case Else
                    Throw New Exception("Error: Class = [HCVU.spBankAccountVerification], Method = [btn_search_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

            End Select

        Catch eSQL As SqlClient.SqlException
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search Fail")
            Throw eSQL
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Search Fail")
            Throw ex
        End Try

    End Sub

    Protected Sub btn_cancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00020, "Back Click")
        ' CRE11-021 log the missed essential information [End]

        Session(strBankAccList) = Nothing
        Me.udcErrorMessage.Visible = False
        Me.panel_searchCriteria.Visible = False
        Me.MultiView1.ActiveViewIndex = 0

    End Sub

    Protected Sub gvBankVerificationList_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        'Me.GridView_Sorting_Handler(Me.gvBankVerificationList, strBankAccList, e.SortExpression)
        Me.GridViewSortingHandler(sender, e, strBankAccList)

    End Sub

    Private Sub DeferEnrolment()

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("ERN", formater.formatSystemNumberReverse(Me.lblEnrolRefNo.Text))
        udtAuditLogEntry.AddDescripton("SPID", Me.lblSPID.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Defer")

        Dim dt As New DataTable
        'Dim i As Integer
        'Dim tsmpBkAccUpdate As Byte() = Nothing
        'dt = Session(strBankAccList)
        dt = Session(strBankAccByERN)
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        'For i = 0 To dt.Rows.Count - 1
        '    If dt.Rows(i)("enrolRefNo").Equals(Me.txt_currentERN.Text) Then
        '        tsmpBkAccUpdate = dt.Rows(i)("BankAccVerTSMP")
        '    End If
        'Next

        Try
            If Not udtBkVer.DeferBankAccount(formater.formatSystemNumberReverse(Me.txt_currentERN.Text), udtHCVUUser.UserID, dt) Then
                'Fail
                Me.udcErrorMessage.AddMessage(FUNCTION_CODE, "E", "00003")
            Else
                'Successful
                Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00003")
            End If

            Me.panel_searchCriteria.Visible = False
            Me.MultiView1.ActiveViewIndex = 2
            Me.udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00012, "Defer Fail")
            Me.udcInfoMessageBox.BuildMessageBox()
            udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Defer successful")
        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                sm = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                udcErrorMessage.AddMessage(sm)

                udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00012, "Defer Fail")
            Else
                udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Defer Fail")
                Throw eSQL
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Defer Fail")
            Throw ex
        End Try
    End Sub


    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Protected Sub btn_markSecondVerify_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00024, "Accept Click")

        hfEnrolmentAction.Value = EnrolmentAction.Accept

        ucEnrolmentActionPopup.MessageText = (New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVQ, MsgCode.MSG00005)).GetMessage
        ModalPopupEnrolmentAction.PopupDragHandleControlID = ucEnrolmentActionPopup.Header.ClientID
        ModalPopupEnrolmentAction.Show()
    End Sub

    Protected Sub btnDefer_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00025, "Defer Click")

        hfEnrolmentAction.Value = EnrolmentAction.Defer

        ucEnrolmentActionPopup.MessageText = (New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVQ, MsgCode.MSG00004)).GetMessage
        ModalPopupEnrolmentAction.PopupDragHandleControlID = ucEnrolmentActionPopup.Header.ClientID
        ModalPopupEnrolmentAction.Show()
    End Sub

    Protected Sub btn_Reject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00021, "Reject Click")

        hfEnrolmentAction.Value = EnrolmentAction.Reject

        ucEnrolmentActionPopup.MessageText = (New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVQ, MsgCode.MSG00001)).GetMessage
        ModalPopupEnrolmentAction.PopupDragHandleControlID = ucEnrolmentActionPopup.Header.ClientID
        ModalPopupEnrolmentAction.Show()
    End Sub

    Protected Sub btn_ReturnForAmendment_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00026, "Return For Amendment Click")

        hfEnrolmentAction.Value = EnrolmentAction.ReturnForAmendment

        ucEnrolmentActionPopup.MessageText = (New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVQ, MsgCode.MSG00003)).GetMessage
        ModalPopupEnrolmentAction.PopupDragHandleControlID = ucEnrolmentActionPopup.Header.ClientID
        ModalPopupEnrolmentAction.Show()
    End Sub

    Private Sub ucEnrolmentActionPopup_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucEnrolmentActionPopup.ButtonClick
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                udtAuditLogEntry.WriteLog(LogID.LOG00027, "Confirmation Popup - Confirm Click")

                Select Case hfEnrolmentAction.Value
                    Case EnrolmentAction.Accept
                        MarkSecondVerify()

                    Case EnrolmentAction.Defer
                        DeferEnrolment()

                    Case EnrolmentAction.Reject
                        RejectEnrolment()

                    Case EnrolmentAction.ReturnForAmendment
                        ReturnForAmendment()

                End Select

            Case ucNoticePopUp.enumButtonClick.Cancel
                udtAuditLogEntry.WriteLog(LogID.LOG00028, "Confirmation Popup - Cancel Click")
                ModalPopupEnrolmentAction.Hide()

        End Select
    End Sub
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    Protected Sub gvBankVerificationList_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.GridViewPreRenderHandler(sender, e, strBankAccList)
    End Sub

    Protected Function GetHealthProfName(ByVal strHealthProfCode As String) As String
        Dim strHealthProfName As String

        If IsNothing(strHealthProfCode) Then
            strHealthProfName = String.Empty
        Else
            If strHealthProfCode.Equals(String.Empty) Then
                strHealthProfName = String.Empty
            Else

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

                ' -----------------------------------------------------------------------------------------

                If Session("language") = "zh-tw" Then
                    strHealthProfName = udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDescChi
                Else
                    strHealthProfName = udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDesc
                End If

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            End If
        End If

        Return strHealthProfName
    End Function

    Private Sub RejectEnrolment()

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("ERN", formater.formatSystemNumberReverse(Me.lblEnrolRefNo.Text))
        udtAuditLogEntry.AddDescripton("SPID", Me.lblSPID.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00013, "Reject")

        Dim udtSp As New Common.Component.ServiceProvider.ServiceProviderModel
        Dim lstBankTsmp As New List(Of Byte())
        Dim lstBankDisplaySeq As New List(Of Integer)
        Dim lstSPPracticeDisplaySeq As New List(Of Integer)
        Dim dt As New DataTable
        Dim strErn As String
        Dim i As Integer
        Dim tsmpSPAccUpdate As Byte() = Nothing

        'dt = Session(strBankAccList)
        dt = Session(strBankAccByERN)
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        For i = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("enrolRefNo").Equals(Me.txt_currentERN.Text) Then
                lstBankTsmp.Add(dt.Rows(i)("BankAccVerTSMP"))
                lstBankDisplaySeq.Add(dt.Rows(i)("displaySeq"))
                lstSPPracticeDisplaySeq.Add(dt.Rows(i)("sp_practice_display_Seq"))
                tsmpSPAccUpdate = dt.Rows(i)("tsmp")
            End If
        Next

        Try
            strErn = formater.formatSystemNumberReverse(Me.txt_currentERN.Text)
            udtSPProfileBLL.RejectSPProfileFromUserC(strErn, udtHCVUUser.UserID, tsmpSPAccUpdate, lstBankTsmp, lstBankDisplaySeq, lstSPPracticeDisplaySeq)

            Me.udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            Me.udcInfoMessageBox.AddMessage(FUNCTION_CODE, "I", "00005")
            Me.panel_searchCriteria.Visible = False
            Me.MultiView1.ActiveViewIndex = 2
            Me.udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00015, "Reject fail")
            Me.udcInfoMessageBox.BuildMessageBox()
            udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Reject successful")
        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                sm = New Common.ComObject.SystemMessage("990001", "D", eSQL.Message)
                udcErrorMessage.AddMessage(sm)
                udcErrorMessage.BuildMessageBox(strUpdateFail, udtAuditLogEntry, LogID.LOG00015, "Reject fail")
            Else
                udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Reject fail")
                Throw eSQL
            End If
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Reject fail")
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnDialogRemindDelistPracticeConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("ERN", formater.formatSystemNumberReverse(Me.lblEnrolRefNo.Text))
        udtAuditLogEntry.AddDescripton("SPID", Me.lblSPID.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00025, "Reminder - Delist Old Practice - Confirm Click")

        ViewState(VS_POPUP_REMIND_DELIST_PRACTICE) = YesNo.Yes

        Me.ModalPopupExtenderRemindDelistPractice.Hide()

        MarkSecondVerify()
    End Sub

    Protected Sub ibtnDialogRemindDelistPracticeCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("ERN", formater.formatSystemNumberReverse(Me.lblEnrolRefNo.Text))
        udtAuditLogEntry.AddDescripton("SPID", Me.lblSPID.Text)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00026, "Reminder - Delist Old Practice - Cancel Click")

        Me.ModalPopupExtenderRemindDelistPractice.Hide()
    End Sub

    Protected Function GetMOName(ByVal strMODisplaySeq As String) As String
        Dim strMOName As String
        Dim udtMOBLL As New MedicalOrganization.MedicalOrganizationBLL

        Dim udtMOCollection As MedicalOrganization.MedicalOrganizationModelCollection

        udtMOCollection = udtMOBLL.GetMOListFromStagingByERN(formater.formatSystemNumberReverse(Me.lblEnrolRefNo.Text), New Common.DataAccess.Database)
        udtMOBLL.SaveToSession(udtMOCollection)
        If IsNothing(strMODisplaySeq) Then
            strMOName = String.Empty
        Else
            If strMODisplaySeq.Equals(String.Empty) Then
                strMOName = String.Empty
            Else
                strMOName = udtMOCollection.Item(CInt(strMODisplaySeq)).DisplaySeqMOName 'udtMOBLL.GetMOName(CInt(strMODisplaySeq.Trim))
            End If
        End If

        Return strMOName
    End Function

    Protected Function formatChineseString(ByVal strChineseString) As String
        If IsNothing(strChineseString) Then
            Return ""
        Else
            Return formater.formatChineseName(strChineseString)
        End If
    End Function

    Protected Function formatChiAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String, ByVal strArea As String) As String
        Return formater.formatChineseName(formater.formatAddressChi(strRoom, strFloor, strBlock, strBuilding, strDistrict, strArea))
    End Function

    Protected Sub ibtnErrorBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.udcErrorMessage.Visible = False
        Me.udcInfoMessageBox.Visible = False

        udtSPProfileBLL.ClearSession()
        Me.MultiView1.ActiveViewIndex = 0

    End Sub

    Private Sub ResetSearchCriteria()
        txt_enrolmentNo.Text = String.Empty
        txt_SPID.Text = String.Empty
        txt_SPhkid.Text = String.Empty
        txt_serviceProvider.Text = String.Empty
        txt_dayTimeContact.Text = String.Empty
        ddlSPHealthProf.SelectedValue = String.Empty
        ddlStatus.SelectedValue = String.Empty
        ddlScheme.SelectedValue = String.Empty
    End Sub

    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function CheckValidationClickAccept(ByRef udtSP As ServiceProvider.ServiceProviderModel, ByRef dtErrorMessage As DataTable) As Boolean
        Dim blnRes As Boolean = True

        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtSPProfileBLL As New SPProfileBLL()        
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

        AddDataColumnErrorMessage(dtErrorMessage)

        Dim drErrorMessage As DataRow

        '1. Check token whether is existed in SP when SP's email address is changed.
        If udtSPProfileBLL.CheckChangeEmailWithoutToken(udtSP) Then
            drErrorMessage = dtErrorMessage.NewRow()
            drErrorMessage.ItemArray = New Object() {FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00344, False, "", ""}
            dtErrorMessage.Rows.Add(drErrorMessage)
            blnRes = False
        End If

        '2. Check SP profile whether is synchronized between staging and permanent.
        If udtSPProfileBLL.CheckUnsynchronizeRecord(udtSP) Then
            drErrorMessage = dtErrorMessage.NewRow()
            drErrorMessage.ItemArray = New Object() {FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, True, "%s", ""}
            dtErrorMessage.Rows.Add(drErrorMessage)
            blnRes = False
        End If

        Return blnRes
    End Function

    Private Function CheckValidationClickReturnForAmendment(ByRef udtSP As ServiceProvider.ServiceProviderModel, ByRef dtErrorMessage As DataTable) As Boolean
        Dim blnRes As Boolean = True
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim udtSPProfileBLL As New SPProfileBLL()
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

        AddDataColumnErrorMessage(dtErrorMessage)

        Dim drErrorMessage As DataRow

        '1. Check SP profile whether is synchronized between staging and permanent.
        If udtSPProfileBLL.CheckUnsynchronizeRecord(udtSP) Then
            drErrorMessage = dtErrorMessage.NewRow()
            drErrorMessage.ItemArray = New Object() {FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00010, True, "%s", ""}
            dtErrorMessage.Rows.Add(drErrorMessage)
            blnRes = False
        End If

        Return blnRes
    End Function

    Sub AddDataColumnErrorMessage(ByRef dtErrorMessage As DataTable)

        Dim dcErrorMessage As DataColumn = New DataColumn("FunctionCode", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("SeverityCode", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("MessageCode", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("IsReplace", GetType(System.Boolean))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("FindString", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

        dcErrorMessage = New DataColumn("ReplaceString", GetType(System.String))
        dtErrorMessage.Columns.Add(dcErrorMessage)

    End Sub
    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

    'CRE17-008 (Remind Delist Practice) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Function CheckToShowReminderForDelistPractice(ByVal strSPID As String, ByVal strERN As String) As Boolean
        Dim udtPracticeBLL As New Practice.PracticeBLL

        Dim blnRes As Boolean = False

        If Not String.IsNullOrEmpty(strSPID) Then
            'Get Practice List From Staging
            Dim udtPracticeListStaging As Practice.PracticeModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERNBankStagingStatus(strERN, db)
            'Get Practice List From Permanent
            Dim udtPracticeListPermanent As Practice.PracticeModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(strSPID, db)

            For Each udtPracticeStaging As Practice.PracticeModel In udtPracticeListStaging.Values
                'Only new practice in staging will check the name and address. In "PracticeStagingStatus", "Active" is equal to new practice. 
                If udtPracticeStaging.RecordStatus <> PracticeStagingStatus.Active Then
                    Continue For
                End If

                Dim strAddressStaging As String = formatAddress(udtPracticeStaging.PracticeAddress.Room, _
                                                         udtPracticeStaging.PracticeAddress.Floor, _
                                                         udtPracticeStaging.PracticeAddress.Block, _
                                                         udtPracticeStaging.PracticeAddress.Building, _
                                                         udtPracticeStaging.PracticeAddress.District, _
                                                         udtPracticeStaging.PracticeAddress.AreaCode)

                Dim strChiAddressStaging As String = formatChiAddress(udtPracticeStaging.PracticeAddress.Room, _
                                                         udtPracticeStaging.PracticeAddress.Floor, _
                                                         udtPracticeStaging.PracticeAddress.Block, _
                                                         udtPracticeStaging.PracticeAddress.ChiBuilding, _
                                                         udtPracticeStaging.PracticeAddress.District, _
                                                         udtPracticeStaging.PracticeAddress.AreaCode)

                For Each udtPracticePermanent As Practice.PracticeModel In udtPracticeListPermanent.Values
                    'Only the existing active or suspended practice's name and address will compare the new practice's name and address.
                    If udtPracticePermanent.RecordStatus <> PracticeStatus.Active And _
                        udtPracticePermanent.RecordStatus <> PracticeStatus.Suspended Then
                        Continue For
                    End If

                    Dim strAddressPermanent As String = formatAddress(udtPracticePermanent.PracticeAddress.Room, _
                                                             udtPracticePermanent.PracticeAddress.Floor, _
                                                             udtPracticePermanent.PracticeAddress.Block, _
                                                             udtPracticePermanent.PracticeAddress.Building, _
                                                             udtPracticePermanent.PracticeAddress.District, _
                                                             udtPracticePermanent.PracticeAddress.AreaCode)

                    Dim strChiAddressPermanent As String = formatChiAddress(udtPracticePermanent.PracticeAddress.Room, _
                                                             udtPracticePermanent.PracticeAddress.Floor, _
                                                             udtPracticePermanent.PracticeAddress.Block, _
                                                             udtPracticePermanent.PracticeAddress.ChiBuilding, _
                                                             udtPracticePermanent.PracticeAddress.District, _
                                                             udtPracticePermanent.PracticeAddress.AreaCode)

                    If udtPracticeStaging.PracticeName.ToUpper.Trim = udtPracticePermanent.PracticeName.ToUpper.Trim And _
                        udtPracticeStaging.PracticeNameChi.ToUpper.Trim = udtPracticePermanent.PracticeNameChi.ToUpper.Trim And _
                        strAddressStaging.ToUpper.Trim = strAddressPermanent.ToUpper.Trim And _
                        strChiAddressStaging.ToUpper.Trim = strChiAddressPermanent.ToUpper.Trim Then

                        blnRes = True
                        Exit For
                    End If
                Next

                If blnRes Then
                    Exit For
                End If
            Next

        End If

        Return blnRes
    End Function
    'CRE17-008 (Remind Delist Practice) [End][Chris YIM]

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Dim udtSPBLL As New ServiceProvider.ServiceProviderBLL

        If IsNothing(udtSPBLL.GetSP) Then
            Return Nothing
        Else
            Return udtSPBLL.GetSP
        End If


    End Function

#End Region

End Class