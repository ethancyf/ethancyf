Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.Component.InternetMail
Imports Common.Component.ServiceProvider
Imports Common.Component.Scheme
Imports Common.Component.SchemeInformation
Imports Common.Component.Token
Imports Common.Component.UserAC
Imports Common.DataAccess
Imports Common.Encryption.Encrypt
Imports Common.ComObject
Imports HCVU.spEnrolmentPrintOutViewer
Imports Common.ComFunction.AccountSecurity

Partial Public Class spPrintFunction
    Inherits System.Web.UI.UserControl

#Region "Fields"

    Private udtGeneralFunction As New GeneralFunction
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtInternetMailBLL As New InternetMailBLL
    Private udtTokenBLL As New TokenBLL
    Private udtUserACBLL As New UserACBLL
    Private udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
#End Region

#Region "Constants"

    Public Const ActionPrintNewEnrolmentLetter As Integer = 1
    Public Const ActionPrintSchemeEnrolmentLetter As Integer = 2
    ' CRE13-003 - Token Replacement [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Public Const ActionPrintTokenReplacementLetter As Integer = 3
    ' CRE13-003 - Token Replacement [End][Tommy L]
    Public Const ActionSendActivationEmail As Integer = 11
    Public Const ActionSendSchemeEnrolmentEmail As Integer = 12
    Public Const ActionSendDelistEmail As Integer = 13
    Public Const ActionPrintAndSendSchemeEnrolment As Integer = 22

    Private Const PrintAction As String = "Print"
    Private Const SendAction As String = "Send"

    Private Const PrintBtn As String = "PrintBtn"
    Private Const PrintDisableBtn As String = "PrintDisableBtn"
    Private Const SendBtn As String = "SendBtn"
    Private Const SendDisableBtn As String = "SendDisableBtn"
    Private Const CssIbtnEnabled As String = "ibtnEnabled"
    Private Const CssIbtnDisabled As String = "ibtnDisabled"

    Private Const strExceptionTokenNotFound As String = "Token not found for User ID %s."
    ' CRE13-003 - Token Replacement [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Private Const strExceptionTokenReplacementStatus As String = "Token Replacement Status has been changed."
    ' CRE13-003 - Token Replacement [End][Tommy L]
    Public Const SESS_PrintOutAuditLogEntry As String = "Print_Out_Audit_Log_Entry"
#End Region

#Region "Session Constants"

    Private Const SESS_CallerFunctionCode As String = "CallerFunctionCode"
    Private Const SESS_ServiceProvider As String = "ServiceProvider"
    Private Const SESS_Applicant_Type As String = "ApplicantType"
    Private Const SESS_SPPrintFunctionSP As String = "SPPrintFunctionSP"
    Private Const SESS_SPPrintFunctionSPIsPermanent As String = "SPPrintFunctionSPIsPermanent"
    Private Const SESS_SPPrintFunctionVisibleScheme As String = "SPPrintFunctionVisibleScheme"

    Public Const SESS_PrintFunctionStatus As String = "PrintFunctionStatus"
    ' CRE13-003 - Token Replacement [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Public Const SESS_SPPrintFunctionToken As String = "SPPrintFunctionToken"
    ' CRE13-003 - Token Replacement [End][Tommy L]

#End Region

#Region "Properties"

    Public Property SPPermanent() As Boolean
        Get
            Return Session(SESS_SPPrintFunctionSPIsPermanent)
        End Get
        Set(ByVal value As Boolean)
            Session(SESS_SPPrintFunctionSPIsPermanent) = value
        End Set
    End Property

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            cblScheme.Items.Clear()

            Dim udtGeneralFunction As GeneralFunction = New GeneralFunction
            For Each udtSchemeBackOfficeModel As SchemeBackOfficeModel In udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().FilterByEffectiveExpiryDate(udtGeneralFunction.GetSystemDateTime)
                cblScheme.Items.Add(New ListItem(udtSchemeBackOfficeModel.SchemeDesc.Trim, udtSchemeBackOfficeModel.SchemeCode.Trim))
            Next

            ' Reset the hidden fields value
            hfActionCode.Value = String.Empty
            hfActionType.Value = String.Empty
            Session(SESS_SPPrintFunctionVisibleScheme) = New ArrayList

            ' 2009-07-29 avoid double post back in firefox

            ' Browser: Firefox
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            DirectCast(Me.Page, BasePage).preventMultiImgClick(Me.Page.ClientScript, Me.ibtnAction)

        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim aryVisibleScheme As ArrayList = Session(SESS_SPPrintFunctionVisibleScheme)

        For Each cboScheme As ListItem In cblScheme.Items
            If IsNothing(aryVisibleScheme) OrElse Not aryVisibleScheme.Contains(cboScheme.Value) Then
                cboScheme.Attributes.Add("style", "display:none")
            End If
        Next

        If aryVisibleScheme.Count = 1 Then
            For Each cboScheme As ListItem In cblScheme.Items
                If cboScheme.Value = aryVisibleScheme(0) Then
                    cboScheme.Selected = True
                    cboScheme.Attributes.Add("onclick", "return false")
                    RenewButtonImage(True)
                    Exit For
                End If
            Next
        End If

    End Sub

#End Region

    ' To be called from outside

    Public Sub PrepareThePanel(ByVal strHeadingResource As String, ByVal strActionCode As String, ByVal udtSP As ServiceProviderModel, ByVal blnSPIsPermanent As Boolean, ByVal strCallerFunctionCode As String)
        ' Heading text
        If strHeadingResource = String.Empty Then
            lblHeader.Text = String.Empty
        Else
            lblHeader.Text = Me.GetGlobalResourceObject("Text", strHeadingResource)
        End If

        ' Action code
        hfActionCode.Value = strActionCode

        ' Caller function code
        Session(SESS_CallerFunctionCode) = strCallerFunctionCode

        ' Action type - integrate the action code to Print or Send
        Select Case strActionCode
            ' CRE13-003 - Token Replacement [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            'Case ActionPrintNewEnrolmentLetter, ActionPrintSchemeEnrolmentLetter, ActionPrintAndSendSchemeEnrolment
            Case ActionPrintNewEnrolmentLetter, ActionPrintSchemeEnrolmentLetter, ActionPrintTokenReplacementLetter, ActionPrintAndSendSchemeEnrolment
                ' CRE13-003 - Token Replacement [End][Tommy L]
                hfActionType.Value = PrintAction
            Case ActionSendActivationEmail, ActionSendSchemeEnrolmentEmail, ActionSendDelistEmail
                hfActionType.Value = SendAction
        End Select

        RenewButtonImage(False)

        ' Save the SP to session
        Session(SESS_SPPrintFunctionSP) = udtSP

        ' SP is permanent or not
        Session(SESS_SPPrintFunctionSPIsPermanent) = blnSPIsPermanent

        ResetSchemeCheckBoxList()

        Select Case strActionCode
            Case ActionPrintNewEnrolmentLetter, ActionPrintSchemeEnrolmentLetter, ActionSendActivationEmail, ActionSendSchemeEnrolmentEmail, ActionPrintAndSendSchemeEnrolment
                ShowEnrolledScheme(udtSP.SchemeInfoList)
            Case ActionSendDelistEmail
                ShowDelistedScheme(udtSP.SchemeInfoList)
                ' CRE13-003 - Token Replacement [Start][Tommy L]
                ' -------------------------------------------------------------------------------------
            Case ActionPrintTokenReplacementLetter
                ' Do Nothing
                ' CRE13-003 - Token Replacement [End][Tommy L]
        End Select

    End Sub

    Private Sub ShowEnrolledScheme(ByVal udtSchemeList As SchemeInformationModelCollection)

        For Each udtSchemeInformationModel As SchemeInformationModel In udtSchemeList.Values

            If Session(SESS_SPPrintFunctionSPIsPermanent) = True Then
                If udtSchemeInformationModel.RecordStatus = SchemeInformationStatus.Delisted _
                        OrElse udtSchemeInformationModel.RecordStatus = SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary _
                        OrElse udtSchemeInformationModel.RecordStatus = SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary Then
                Else
                    CType(Session(SESS_SPPrintFunctionVisibleScheme), ArrayList).Add(udtSchemeInformationModel.SchemeCode.Trim)
                End If
            Else
                CType(Session(SESS_SPPrintFunctionVisibleScheme), ArrayList).Add(udtSchemeInformationModel.SchemeCode.Trim)
            End If
        Next
    End Sub

    Private Sub ShowDelistedScheme(ByVal udtSchemeList As SchemeInformationModelCollection)

        For Each udtSchemeInformationModel As SchemeInformationModel In udtSchemeList.Values

            If udtSchemeInformationModel.RecordStatus = SchemeInformationStatus.Delisted _
                        OrElse udtSchemeInformationModel.RecordStatus = SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary _
                        OrElse udtSchemeInformationModel.RecordStatus = SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary Then
                CType(Session(SESS_SPPrintFunctionVisibleScheme), ArrayList).Add(udtSchemeInformationModel.SchemeCode.Trim)
            End If
        Next

    End Sub

    ' Used in Token Management print function (new enrolment)

    Public Sub PrintLetterWithoutPopup()
        ResetSchemeCheckBoxList()

        ' CRE13-003 - Token Replacement [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        'SetDefaultSelectedScheme(Session(SESS_SPPrintFunctionSP))
        'GetSelectedScheme()
        ' CRE13-003 - Token Replacement [End][Tommy L]

        Select Case hfActionCode.Value
            Case ActionPrintNewEnrolmentLetter
                ' CRE13-003 - Token Replacement [Start][Tommy L]
                ' -------------------------------------------------------------------------------------
                SetDefaultSelectedScheme(Session(SESS_SPPrintFunctionSP))
                GetSelectedScheme()
                ' CRE13-003 - Token Replacement [End][Tommy L]
                ExecutePrintNewEnrolmentLetter(Session(SESS_SPPrintFunctionSP))

            Case ActionPrintSchemeEnrolmentLetter
                ' CRE13-003 - Token Replacement [Start][Tommy L]
                ' -------------------------------------------------------------------------------------
                SetDefaultSelectedScheme(Session(SESS_SPPrintFunctionSP))
                GetSelectedScheme()
                ' CRE13-003 - Token Replacement [End][Tommy L]
                ExecutePrintSchemeEnrolmentLetter(Session(SESS_SPPrintFunctionSP))

                ' CRE13-003 - Token Replacement [Start][Tommy L]
                ' -------------------------------------------------------------------------------------
            Case ActionPrintTokenReplacementLetter
                ExecutePrintTokenReplacementLetter(Session(SESS_SPPrintFunctionSP))

                ' CRE13-003 - Token Replacement [End][Tommy L]
        End Select
    End Sub

    Public Sub SendActivationEmailWithoutPopup(ByVal strActivationCode As String)
        Dim udtSP As ServiceProviderModel = Session(SESS_SPPrintFunctionSP)

        ResetSchemeCheckBoxList()
        SetDefaultSelectedScheme(udtSP)

        If Session(SESS_SPPrintFunctionSPIsPermanent) Then
            ExecuteSendActivationEmail(udtSP, GetSelectedScheme())
        Else
            ExecuteSendSchemeEnrolmentEmail(udtSP, GetSelectedScheme())
        End If

    End Sub

    Public Sub SetDefaultSelectedScheme(ByVal udtSPStaging As ServiceProviderModel)
        For Each udtSchemeStaging As SchemeInformationModel In udtSPStaging.SchemeInfoList.Values
            Select Case udtSchemeStaging.RecordStatus
                Case SchemeInformationStagingStatus.Active
                    cblScheme.Items(GetSeqNoFromSchemeCode(udtSchemeStaging.SchemeCode) - 1).Enabled = True
                    cblScheme.Items(GetSeqNoFromSchemeCode(udtSchemeStaging.SchemeCode) - 1).Selected = True

                Case SchemeInformationStagingStatus.Existing
                    cblScheme.Items(GetSeqNoFromSchemeCode(udtSchemeStaging.SchemeCode) - 1).Enabled = True

                Case Else
                    ' What should be put here?
            End Select

        Next

        Session(SESS_SPPrintFunctionSP) = udtSPStaging

        Dim blnChecked As Boolean = False

        For Each cboScheme As ListItem In cblScheme.Items
            If cboScheme.Selected Then
                blnChecked = True
                Exit For
            End If
        Next

        RenewButtonImage(blnChecked)

    End Sub

    '

    Public Sub Activate()
        Session(SESS_PrintFunctionStatus) = PrintFunctionStatus.ActivePrintFunction
    End Sub

    Protected Sub ibtnAction_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtSP As ServiceProviderModel = Session(SESS_SPPrintFunctionSP)

        'AuditLog
        Dim udtAuditLogEntry As New AuditLogEntry(Session(SESS_CallerFunctionCode))
        udtAuditLogEntry.AddDescripton("Action Code", hfActionCode.Value)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00107, "Print Function Click Action Start", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))


        Try
            ' Get the selected schemes and save to session
            Dim arySelectedScheme As ArrayList = GetSelectedScheme()

            Select Case hfActionCode.Value
                Case ActionPrintNewEnrolmentLetter
                    ExecutePrintNewEnrolmentLetter(Session(SESS_SPPrintFunctionSP))
                    Session(SESS_PrintFunctionStatus) = PrintFunctionStatus.FinishPrintFunction

                Case ActionPrintSchemeEnrolmentLetter
                    ExecutePrintSchemeEnrolmentLetter(Session(SESS_SPPrintFunctionSP))
                    Session(SESS_PrintFunctionStatus) = PrintFunctionStatus.FinishPrintFunction

                Case ActionSendActivationEmail
                    ' CRE16-018 Display SP tentative email in HCVU [Start][Winnie]
                    Try
                        ExecuteSendActivationEmail(Session(SESS_SPPrintFunctionSP), arySelectedScheme)
                        Session(SESS_PrintFunctionStatus) = PrintFunctionStatus.FinishPrintFunction
                    Catch ex As Exception
                        If ex.Message.Equals("SPActivated") Then
                            ' Show Error Msg 
                            Session(SESS_PrintFunctionStatus) = PrintFunctionStatus.ErrorPrintFunction
                            udtAuditLogEntry.WriteEndLog(LogID.LOG00109, "Print Function Click Action Failed", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
                        Else
                            Throw
                        End If
                    End Try
                    ' CRE16-018 Display SP tentative email in HCVU [End][Winnie]
                Case ActionSendSchemeEnrolmentEmail
                    ExecuteSendSchemeEnrolmentEmail(Session(SESS_SPPrintFunctionSP), arySelectedScheme)
                    Session(SESS_PrintFunctionStatus) = PrintFunctionStatus.FinishPrintFunction

                Case ActionSendDelistEmail
                    ExecuteSendDelistEmail(Session(SESS_SPPrintFunctionSP), arySelectedScheme)
                    Session(SESS_PrintFunctionStatus) = PrintFunctionStatus.FinishPrintFunction

                Case ActionPrintAndSendSchemeEnrolment
                    ExecutePrintSchemeEnrolmentLetter(Session(SESS_SPPrintFunctionSP))
                    ExecuteSendSchemeEnrolmentEmailForTokenMgtOnly(Session(SESS_SPPrintFunctionSP), arySelectedScheme)
                    Session(SESS_PrintFunctionStatus) = PrintFunctionStatus.FinishPrintFunction

                    ' CRE13-003 - Token Replacement [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------
                Case ActionPrintTokenReplacementLetter
                    ' No Pop-up for Scheme Selection & Nothing here

                    ' CRE13-003 - Token Replacement [End][Tommy L]
            End Select
            udtAuditLogEntry.WriteEndLog(LogID.LOG00108, "Print Function Click Action Completed", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00109, "Print Function Click Action Failed", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            Throw
        End Try
    End Sub

    Protected Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        Session(SESS_PrintFunctionStatus) = PrintFunctionStatus.ClosePrintFunction
    End Sub

    Private Function GetSelectedScheme() As ArrayList
        Dim arySelectedScheme As New ArrayList

        For Each cboScheme As ListItem In cblScheme.Items
            If cboScheme.Selected Then arySelectedScheme.Add(cboScheme.Value)
        Next

        Session(SESS_PrintSchemeCodeArrayList) = arySelectedScheme

        Return arySelectedScheme

    End Function

    '

    Private Sub ExecutePrintNewEnrolmentLetter(ByVal udtSP As ServiceProviderModel)
        'AuditLog
        Dim udtAuditLogEntry As New AuditLogEntry(Session(SESS_CallerFunctionCode))
        udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
        udtAuditLogEntry.AddDescripton("Letter Type", "New Enrolment")
        'Audit Log (Scheme Code List)
        Dim strSchemeCodeForAduit As String = String.Empty
        For Each strCode As String In CType(Session(SESS_PrintSchemeCodeArrayList), ArrayList)
            If strSchemeCodeForAduit.Equals(String.Empty) Then
                strSchemeCodeForAduit = strCode.Trim
            Else
                strSchemeCodeForAduit = strSchemeCodeForAduit + ", " + strCode.Trim
            End If
        Next
        udtAuditLogEntry.AddDescripton("Scheme Code List", strSchemeCodeForAduit)

        ' Select Print Letter
        If Not IsNothing(Session(SESS_Applicant_Type)) Then
            Session(SESS_Applicant_Type) = Nothing
        End If

        ' New Enrolment
        Dim udtTokenModel As TokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtSP.SPID, New Database)

        If IsNothing(udtTokenModel) Then
            ' Token is deactivated, get from [TokenDeactivated] table
            Dim dt As DataTable = udtTokenBLL.GetTokenDeactivatedByUserID(udtSP.SPID, New Database)

            udtAuditLogEntry.AddDescripton("Token deactivated", "True")
            If dt.Rows.Count > 0 Then
                'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                '' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                '' -----------------------------------------------------------------------------------------
                ''If dt.Rows(0)("Token_Serial_No") = "******" Then
                'If CStr(dt.Rows(0)("Project")).Trim() = TokenProjectType.PPIEPR Then
                '    udtSP.TokenSerialNo = dt.Rows(0)("Token_Serial_No")
                '    ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                '    Session(SESS_Applicant_Type) = ApplicantType.Applicant_of_PPiePR
                '    udtAuditLogEntry.AddDescripton("use PPI-ePR token", "True")
                '    ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                '    ' -----------------------------------------------------------------------------------------
                '    'udtAuditLogEntry.AddDescripton("Token Serial No", "******")
                '    udtAuditLogEntry.AddDescripton("Token Serial No", udtSP.TokenSerialNo)
                '    ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                'Else
                '    Session(SESS_Applicant_Type) = ApplicantType.Applicant_of_fresh
                '    udtSP.TokenSerialNo = dt.Rows(0)("Token_Serial_No")
                '    udtAuditLogEntry.AddDescripton("use PPI-ePR token", "False")
                '    udtAuditLogEntry.AddDescripton("Token Serial No", udtSP.TokenSerialNo)
                'End If

                If CStr(dt.Rows(0)("Project")).Trim() = TokenProjectType.EHCVS Then
                    Session(SESS_Applicant_Type) = ApplicantType.NewEnrolment_EHSS_Token
                    udtSP.TokenSerialNo = dt.Rows(0)("Token_Serial_No")
                    udtAuditLogEntry.AddDescripton("use EHRSS token", "False")
                    udtAuditLogEntry.AddDescripton("Token Serial No", udtSP.TokenSerialNo)
                Else
                    Session(SESS_Applicant_Type) = ApplicantType.NewEnrolment_EHRSS_Token
                    udtSP.TokenSerialNo = dt.Rows(0)("Token_Serial_No")
                    udtAuditLogEntry.AddDescripton("use EHRSS token", "True")
                    udtAuditLogEntry.AddDescripton("Token Serial No", udtSP.TokenSerialNo)
                End If
                'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]
            Else
                udtAuditLogEntry.WriteLog(LogID.LOG00103, "Print failed : " + strExceptionTokenNotFound.Replace("%s", udtSP.SPID.Trim), New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
                Throw New Exception(strExceptionTokenNotFound.Replace("%s", udtSP.SPID.Trim))
            End If

        Else
            'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'If udtTokenModel.Project = TokenProjectType.PPIEPR Then
            '    Session(SESS_Applicant_Type) = ApplicantType.Applicant_of_PPiePR
            'Else
            '    Session(SESS_Applicant_Type) = ApplicantType.Applicant_of_fresh
            'End If

            If udtTokenModel.Project = TokenProjectType.EHCVS Then
                Session(SESS_Applicant_Type) = ApplicantType.NewEnrolment_EHSS_Token
            Else
                Session(SESS_Applicant_Type) = ApplicantType.NewEnrolment_EHRSS_Token
            End If


            ' Take Token Serial No. here instead of that from SP model
            udtSP.TokenSerialNo = udtTokenModel.TokenSerialNo

            udtAuditLogEntry.AddDescripton("Token deactivated", "False")
            'udtAuditLogEntry.AddDescripton("use PPI-ePR token", (udtTokenModel.Project = TokenProjectType.PPIEPR).ToString())
            udtAuditLogEntry.AddDescripton("use EHRSS token", (udtTokenModel.Project = TokenProjectType.EHR).ToString())
            udtAuditLogEntry.AddDescripton("Token Serial No", udtSP.TokenSerialNo)

            'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]
        End If

        Session(SESS_ServiceProvider) = udtSP
        'AuditLog
        Session(SESS_PrintOutAuditLogEntry) = udtAuditLogEntry

        ScriptManager.RegisterStartupScript(Me, Page.GetType, String.Empty, "javascript:openNewWin('spEnrolmentPrintOutViewer.aspx')", True)

    End Sub

    Private Sub ExecutePrintSchemeEnrolmentLetter(ByVal udtSP As ServiceProviderModel)
        'AuditLog
        Dim udtAuditLogEntry As New AuditLogEntry(Session(SESS_CallerFunctionCode))
        udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
        udtAuditLogEntry.AddDescripton("Letter Type", "Scheme Enrolment")
        'Audit Log (Scheme Code List)
        Dim strSchemeCodeForAduit As String = String.Empty
        For Each strCode As String In CType(Session(SESS_PrintSchemeCodeArrayList), ArrayList)
            If strSchemeCodeForAduit.Equals(String.Empty) Then
                strSchemeCodeForAduit = strCode.Trim
            Else
                strSchemeCodeForAduit = strSchemeCodeForAduit + ", " + strCode.Trim
            End If
        Next
        udtAuditLogEntry.AddDescripton("Scheme Code List", strSchemeCodeForAduit)

        If IsNothing(udtSP) Then
            udtSP = Session(SESS_SPPrintFunctionSP)
        End If

        ' Scheme Enrolment
        Dim udtTokenModel As TokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtSP.SPID, New Database)

        If IsNothing(udtTokenModel) Then
            ' Token is deactivated, get from [TokenDeactivated] table
            Dim dt As DataTable = udtTokenBLL.GetTokenDeactivatedByUserID(udtSP.SPID, New Database)

            If dt.Rows.Count > 0 Then
                'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                '' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                '' -----------------------------------------------------------------------------------------
                ''If dt.Rows(0)("Token_Serial_No") = "******" Then
                'If CStr(dt.Rows(0)("Project")).Trim() = TokenProjectType.PPIEPR Then
                '    ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                '    Session(SESS_Applicant_Type) = ApplicantType.Applicant_of_eHS_PPiePR
                '    udtAuditLogEntry.AddDescripton("use PPI-ePR token", "True")
                'Else
                '    Session(SESS_Applicant_Type) = ApplicantType.Applicant_of_eHS
                '    udtAuditLogEntry.AddDescripton("use PPI-ePR token", "False")
                'End If

                If CStr(dt.Rows(0)("Project")).Trim() = TokenProjectType.EHCVS Then
                    Session(SESS_Applicant_Type) = ApplicantType.SchemeEnrolment_EHSS_Token
                    udtAuditLogEntry.AddDescripton("use EHRSS token", "False")
                Else
                    Session(SESS_Applicant_Type) = ApplicantType.SchemeEnrolment_EHRSS_Token
                    udtAuditLogEntry.AddDescripton("use EHRSS token", "True")
                End If
                'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]
            Else
                udtAuditLogEntry.WriteLog(LogID.LOG00103, "Print failed : " + strExceptionTokenNotFound.Replace("%s", udtSP.SPID.Trim))
                Throw New Exception(strExceptionTokenNotFound.Replace("%s", udtSP.SPID.Trim))
            End If

        Else
            'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'If udtTokenModel.Project = TokenProjectType.PPIEPR Then
            '    Session(SESS_Applicant_Type) = ApplicantType.Applicant_of_eHS_PPiePR
            'Else
            '    Session(SESS_Applicant_Type) = ApplicantType.Applicant_of_eHS
            'End If

            If udtTokenModel.Project = TokenProjectType.EHCVS Then
                Session(SESS_Applicant_Type) = ApplicantType.SchemeEnrolment_EHSS_Token
            Else
                Session(SESS_Applicant_Type) = ApplicantType.SchemeEnrolment_EHRSS_Token
            End If

            'udtAuditLogEntry.AddDescripton("use PPI-ePR token", (udtTokenModel.Project = TokenProjectType.PPIEPR).ToString())
            udtAuditLogEntry.AddDescripton("use EHRSS token", (udtTokenModel.Project = TokenProjectType.EHR).ToString())
            'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]
        End If

        Session(SESS_ServiceProvider) = udtSP
        'AuditLog
        Session(SESS_PrintOutAuditLogEntry) = udtAuditLogEntry
        ScriptManager.RegisterStartupScript(Me, Page.GetType, String.Empty, "javascript:openNewWin('spEnrolmentPrintOutViewer.aspx')", True)

    End Sub

    ' CRE13-003 - Token Replacement [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Private Sub ExecutePrintTokenReplacementLetter(ByVal udtSP As ServiceProviderModel)
        'AuditLog
        Dim udtAuditLogEntry As New AuditLogEntry(Session(SESS_CallerFunctionCode))

        udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
        udtAuditLogEntry.AddDescripton("Letter Type", "Token Replacement")

        If Not IsNothing(Session(SESS_Applicant_Type)) Then
            Session(SESS_Applicant_Type) = Nothing
        End If

        Dim udtTokenModel As TokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtSP.SPID, New Database)

        If Not IsNothing(udtTokenModel) Then
            If udtTokenBLL.IsRequiredActivateAfterTokenReplaced(udtSP.SPID) Then
                ' Take Token Serial No. here instead of that from SP model
                udtSP.TokenSerialNo = udtTokenModel.TokenSerialNo

                'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                '' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                '' -----------------------------------------------------------------------------------------
                ''udtAuditLogEntry.AddDescripton("Use PPI-ePR Token", (udtTokenModel.Project = TokenProjectType.PPIEPR).ToString())
                ''udtAuditLogEntry.AddDescripton("Old Token Serial No", udtTokenModel.TokenSerialNo)
                ''udtAuditLogEntry.AddDescripton("New Token Serial No", IIf(IsNothing(udtTokenModel.TokenSerialNoReplacement), String.Empty, udtTokenModel.TokenSerialNoReplacement))
                'udtAuditLogEntry.AddDescripton("Use PPI-ePR Token (Existing)", (udtTokenModel.Project = TokenProjectType.PPIEPR).ToString())
                'udtAuditLogEntry.AddDescripton("Token Serial No (Existing)", udtTokenModel.TokenSerialNo)
                'udtAuditLogEntry.AddDescripton("Is Share Token (Existing)", udtTokenModel.IsShareToken.ToString())
                'udtAuditLogEntry.AddDescripton("Use PPI-ePR Token (New)", (udtTokenModel.ProjectReplacement = TokenProjectType.PPIEPR).ToString())
                'udtAuditLogEntry.AddDescripton("Token Serial No (New)", udtTokenModel.TokenSerialNoReplacement)
                'udtAuditLogEntry.AddDescripton("Is Share Token (New)", udtTokenModel.IsShareTokenReplacement.Value.ToString())
                '' CRE14-002 - PPI-ePR Migration [End][Tommy L]

                udtAuditLogEntry.AddDescripton("Use EHRSS Token (Existing)", (udtTokenModel.Project = TokenProjectType.EHR).ToString())
                udtAuditLogEntry.AddDescripton("Token Serial No (Existing)", udtTokenModel.TokenSerialNo)
                udtAuditLogEntry.AddDescripton("Is Share Token (Existing)", udtTokenModel.IsShareToken.ToString())
                udtAuditLogEntry.AddDescripton("Use EHRSS Token (New)", (udtTokenModel.ProjectReplacement = TokenProjectType.EHR).ToString())
                udtAuditLogEntry.AddDescripton("Token Serial No (New)", udtTokenModel.TokenSerialNoReplacement)
                udtAuditLogEntry.AddDescripton("Is Share Token (New)", udtTokenModel.IsShareTokenReplacement.Value.ToString())
                'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]

            Else
                udtAuditLogEntry.WriteLog(LogID.LOG00103, "Print failed : " + strExceptionTokenReplacementStatus)
                Throw New Exception(strExceptionTokenReplacementStatus)
            End If
        Else
            udtAuditLogEntry.WriteLog(LogID.LOG00103, "Print failed : " + strExceptionTokenNotFound.Replace("%s", udtSP.SPID.Trim))
            Throw New Exception(strExceptionTokenNotFound.Replace("%s", udtSP.SPID.Trim))
        End If

        Session(SESS_ServiceProvider) = udtSP
        Session(SESS_SPPrintFunctionToken) = udtTokenModel

        'AuditLog
        Session(SESS_PrintOutAuditLogEntry) = udtAuditLogEntry

        ScriptManager.RegisterStartupScript(Me, Page.GetType, String.Empty, "javascript:openNewWin('spTokenReplacementPrintOutViewer.aspx')", True)
    End Sub
    ' CRE13-003 - Token Replacement [End][Tommy L]

    Private Sub ExecuteSendActivationEmail(ByVal udtSP As ServiceProviderModel, ByVal arySelectedScheme As ArrayList)
        Dim strNewActivationCode As String = udtGeneralFunction.generateAccountActivationCode()
        Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
        Dim udtDB As New Database
        Dim udtAuditLogEntry As New AuditLogEntry(Session(SESS_CallerFunctionCode))

        Try
            'AuditLog
            udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
            udtAuditLogEntry.AddDescripton("Email Type", "Account Activation")
            'Audit Log (Scheme Code List)
            Dim strSchemeCodeForAduit As String = String.Empty
            For Each strCode As String In arySelectedScheme
                If strSchemeCodeForAduit.Equals(String.Empty) Then
                    strSchemeCodeForAduit = strCode.Trim
                Else
                    strSchemeCodeForAduit = strSchemeCodeForAduit + ", " + strCode.Trim
                End If
            Next
            udtAuditLogEntry.AddDescripton("Scheme Code List", strSchemeCodeForAduit)

            udtDB.BeginTransaction()

            ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
            udtUserACBLL.UpdateUserACActivationCode(udtSP.SPID, Hash(strNewActivationCode), strUserID, udtDB)
            ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

            Dim blnIsJoinEHRSS As Boolean
            Dim udtTokenModel As TokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtSP.SPID, udtDB)

            If IsNothing(udtTokenModel) Then
                ' Token is deactivated, get from [TokenDeactivated] table
                Dim dt As DataTable = udtTokenBLL.GetTokenDeactivatedByUserID(udtSP.SPID, udtDB)

                If dt.Rows.Count > 0 Then
                    'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    '' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                    '' -----------------------------------------------------------------------------------------
                    ''If dt.Rows(0)("Token_Serial_No") = "******" Then
                    'If CStr(dt.Rows(0)("Project")).Trim() = TokenProjectType.PPIEPR Then
                    '    ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                    '    blnIsJoinPPIePR = True
                    'Else
                    '    blnIsJoinPPIePR = False
                    'End If

                    If CStr(dt.Rows(0)("Project")).Trim() = TokenProjectType.EHCVS Then
                        blnIsJoinEHRSS = False
                    Else
                        blnIsJoinEHRSS = True
                    End If
                    'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]
                Else
                    Throw New Exception(strExceptionTokenNotFound.Replace("%s", udtSP.SPID.Trim))
                End If
            Else
                'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If udtTokenModel.Project = TokenProjectType.PPIEPR Then
                '    blnIsJoinPPIePR = True
                'Else
                '    blnIsJoinPPIePR = False
                'End If

                If udtTokenModel.Project = TokenProjectType.EHCVS Then
                    blnIsJoinEHRSS = False
                Else
                    blnIsJoinEHRSS = True
                End If
                'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]
            End If

            udtAuditLogEntry.AddDescripton("use EHRSS token", blnIsJoinEHRSS.ToString())
            udtAuditLogEntry.AddDescripton("Activation Code", strNewActivationCode)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00104, "Send Email for Enrolment", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            udtInternetMailBLL.SubmitAccountActivationEmail(udtDB, udtSP.SPID, strNewActivationCode, blnIsJoinEHRSS, True, arySelectedScheme)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00105, "Send Email for Enrolment successful", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()
            udtAuditLogEntry.WriteEndLog(LogID.LOG00106, "Send Email for Enrolment fail", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            Throw ex

        End Try

    End Sub

    Private Sub ExecuteSendSchemeEnrolmentEmail(ByVal udtSP As ServiceProviderModel, ByVal arySelectedScheme As ArrayList)
        Dim blnIsJoinEHRSS As Boolean
        Dim udtDB As New Database
        Dim udtTokenModel As TokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtSP.SPID, udtDB)
        Dim udtAuditLogEntry As New AuditLogEntry(Session(SESS_CallerFunctionCode))

        Try
            'AuditLog
            udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
            udtAuditLogEntry.AddDescripton("Email Type", "Scheme Enrolment Confirmation")
            'Audit Log (Scheme Code List)
            Dim strSchemeCodeForAduit As String = String.Empty
            For Each strCode As String In arySelectedScheme
                If strSchemeCodeForAduit.Equals(String.Empty) Then
                    strSchemeCodeForAduit = strCode.Trim
                Else
                    strSchemeCodeForAduit = strSchemeCodeForAduit + ", " + strCode.Trim
                End If
            Next
            udtAuditLogEntry.AddDescripton("Scheme Code List", strSchemeCodeForAduit)

            udtDB.BeginTransaction()

            If IsNothing(udtTokenModel) Then
                ' Token is deactivated, get from [TokenDeactivated] table
                Dim dt As DataTable = udtTokenBLL.GetTokenDeactivatedByUserID(udtSP.SPID, udtDB)

                If dt.Rows.Count > 0 Then
                    'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    '' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
                    '' -----------------------------------------------------------------------------------------
                    ''If dt.Rows(0)("Token_Serial_No") = "******" Then
                    'If CStr(dt.Rows(0)("Project")).Trim() = TokenProjectType.PPIEPR Then
                    '    ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
                    '    blnIsJoinPPIePR = True
                    'Else
                    '    blnIsJoinPPIePR = False
                    'End If

                    If CStr(dt.Rows(0)("Project")).Trim() = TokenProjectType.EHCVS Then
                        blnIsJoinEHRSS = False
                    Else
                        blnIsJoinEHRSS = True
                    End If
                    'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]

                Else
                    Throw New Exception(strExceptionTokenNotFound.Replace("%s", udtSP.SPID.Trim))
                End If
            Else
                'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If udtTokenModel.Project = TokenProjectType.PPIEPR Then
                '    blnIsJoinPPIePR = True
                'Else
                '    blnIsJoinPPIePR = False
                'End If

                If udtTokenModel.Project = TokenProjectType.EHCVS Then
                    blnIsJoinEHRSS = False
                Else
                    blnIsJoinEHRSS = True
                End If
                'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]
            End If

            udtAuditLogEntry.AddDescripton("use EHRSS token", blnIsJoinEHRSS.ToString())
            udtAuditLogEntry.WriteStartLog(LogID.LOG00104, "Send Email for Enrolment", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            udtInternetMailBLL.SubmitAccountActivationEmail(udtDB, udtSP.SPID, String.Empty, blnIsJoinEHRSS, False, arySelectedScheme)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00105, "Send Email for Enrolment successful", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()
            udtAuditLogEntry.WriteEndLog(LogID.LOG00106, "Send Email for Enrolment fail", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            Throw ex

        End Try

    End Sub

    Private Sub ExecuteSendSchemeEnrolmentEmailForTokenMgtOnly(ByVal udtSP As ServiceProviderModel, ByVal arySelectedScheme As ArrayList)
        Dim blnIsJoinEHRSS As Boolean
        Dim udtDB As New Database
        Dim udtTokenModel As TokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtSP.SPID, udtDB)
        Dim udtAuditLogEntry As New AuditLogEntry(Session(SESS_CallerFunctionCode))

        Try
            If Not IsNothing(Session("Printed")) AndAlso Session("Printed") <> True Then
                'AuditLog
                udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
                udtAuditLogEntry.AddDescripton("Email Type", "Scheme Enrolment Confirmation")
                'Audit Log (Scheme Code List)
                Dim strSchemeCodeForAduit As String = String.Empty
                For Each strCode As String In arySelectedScheme
                    If strSchemeCodeForAduit.Equals(String.Empty) Then
                        strSchemeCodeForAduit = strCode.Trim
                    Else
                        strSchemeCodeForAduit = strSchemeCodeForAduit + ", " + strCode.Trim
                    End If
                Next
                udtAuditLogEntry.AddDescripton("Scheme Code List", strSchemeCodeForAduit)

                udtDB.BeginTransaction()

                If IsNothing(udtTokenModel) Then
                    ' Token is deactivated, get from [TokenDeactivated] table
                    Dim dt As DataTable = udtTokenBLL.GetTokenDeactivatedByUserID(udtSP.SPID, udtDB)

                    If dt.Rows.Count > 0 Then
                        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        'If dt.Rows(0)("Token_Serial_No") = "******" Then
                        '    blnIsJoinPPIePR = True
                        'Else
                        '    blnIsJoinPPIePR = False
                        'End If
                        If CStr(dt.Rows(0)("Project")).Trim = TokenProjectType.EHCVS Then
                            blnIsJoinEHRSS = False
                        Else
                            blnIsJoinEHRSS = True
                        End If

                        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
                    Else
                        Throw New Exception(strExceptionTokenNotFound.Replace("%s", udtSP.SPID.Trim))
                    End If
                Else
                    'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'If udtTokenModel.Project = TokenProjectType.PPIEPR Then
                    '    blnIsJoinPPIePR = True
                    'Else
                    '    blnIsJoinPPIePR = False
                    'End If

                    If udtTokenModel.Project = TokenProjectType.EHCVS Then
                        blnIsJoinEHRSS = False
                    Else
                        blnIsJoinEHRSS = True
                    End If
                    'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]
                End If

                udtAuditLogEntry.AddDescripton("use EHRSS token", blnIsJoinEHRSS.ToString())
                udtAuditLogEntry.WriteStartLog(LogID.LOG00104, "Send Email for Enrolment", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
                udtInternetMailBLL.SubmitAccountActivationEmail(udtDB, udtSP.SPID, String.Empty, blnIsJoinEHRSS, False, arySelectedScheme)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00105, "Send Email for Enrolment successful", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
                udtDB.CommitTransaction()

                Session("Printed") = True
            End If
        Catch ex As Exception
            udtDB.RollBackTranscation()
            udtAuditLogEntry.WriteEndLog(LogID.LOG00106, "Send Email for Enrolment fail", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
            Throw ex

        End Try

    End Sub

    Private Sub ExecuteSendDelistEmail(ByVal udtSP As ServiceProviderModel, ByVal arySelectedScheme As ArrayList)
        For Each strSelectedScheme As String In arySelectedScheme
            Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(Session(SESS_CallerFunctionCode))
            udtAuditLogEntry.AddDescripton("SPID", udtSP.SPID)
            udtAuditLogEntry.AddDescripton("Scheme", strSelectedScheme)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00110, "Resend De-listing Scheme Notification Email", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

            udtInternetMailBLL.SubmitDelistNotificationEmail(New Database, udtSP.SPID, strSelectedScheme)

            udtAuditLogEntry.WriteEndLog(LogID.LOG00111, "Resend De-listing Scheme Notification Email successful", New AuditLogInfo(udtSP.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

        Next

    End Sub

    '

    Protected Sub cblScheme_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim blnOneChecked As Boolean = False

        For Each cboScheme As ListItem In cblScheme.Items
            If cboScheme.Selected Then
                blnOneChecked = True
                Exit For
            End If
        Next

        RenewButtonImage(blnOneChecked)

    End Sub

    '

    Private Sub RenewButtonImage(ByVal blnEnable As Boolean)
        If ibtnAction.Enabled = blnEnable Then Return

        ibtnAction.Enabled = blnEnable

        Select Case hfActionType.Value
            Case PrintAction
                ibtnAction.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnAction.Enabled, PrintBtn, PrintDisableBtn))
                ibtnAction.AlternateText = Me.GetGlobalResourceObject("AlternateText", IIf(ibtnAction.Enabled, PrintBtn, PrintDisableBtn))
            Case SendAction
                ibtnAction.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", IIf(ibtnAction.Enabled, SendBtn, SendDisableBtn))
                ibtnAction.AlternateText = Me.GetGlobalResourceObject("AlternateText", IIf(ibtnAction.Enabled, SendBtn, SendDisableBtn))
        End Select

        ibtnAction.CssClass = IIf(ibtnAction.Enabled, CssIbtnEnabled, CssIbtnDisabled)

    End Sub

    Private Sub ResetSchemeCheckBoxList()
        For Each cboScheme As ListItem In cblScheme.Items
            Session(SESS_SPPrintFunctionVisibleScheme) = New ArrayList
            cboScheme.Selected = False
        Next

        RenewButtonImage(False)

    End Sub

    Private Function GetSeqNoFromSchemeCode(ByVal strScheme As String) As Integer
        For Each udtSchemeBackOfficeModel As SchemeBackOfficeModel In udtSchemeBackOfficeBLL.getAllSchemeBackOfficeWithSubsidizeGroup()
            If udtSchemeBackOfficeModel.SchemeCode.Trim = strScheme.Trim Then
                Return udtSchemeBackOfficeModel.DisplaySeq
            End If
        Next

        Return Nothing

    End Function

End Class