Imports Microsoft.VisualBasic
Imports System.Threading
Imports System.Globalization
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports CustomControls
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.HCVUUser
Imports Common.Component.AccessRight
Imports Common.Component.RedirectParameter
Imports HCVU.Component.FunctionInformation
Imports HCVU.BLL
Imports HCVU

Public MustInherit Class BasePage
    Inherits Common.ComObject.MasterPage
    '<summary>
    'Default constructor
    '</summary>
    Public Sub BasePage()
    End Sub


#Region "Private Members"
    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Public _PostBackPageKey As String = String.Empty
    Public _FunctCodeCommon As String = FunctCode.FUNT029901
    Public Const SESS_PageKey As String = "HCVU_PageKey"

    ' CRE19-026 (HCVS hotline service) [End][Winnie]

    Protected WithEvents _ScriptManager As ScriptManager
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    Protected Const BASE_PAGE_FUNCT_CODE As String = FunctCode.FUNT990002
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

#End Region

#Region "Audit Log Description"
    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Public Class AuditLogDescription
        Public Const Redirect_InvalidAccessPage_ID As String = LogID.LOG00012
        Public Const Redirect_InvalidAccessPage As String = "Redirect to invalid access error page"
    End Class
    ' CRE19-026 (HCVS hotline service) [End][Winnie]
#End Region

#Region "Properties"
    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Public ReadOnly Property SubPlatform() As EnumHCVUSubPlatform
        Get
            Dim strSubPlatform As String = ConfigurationManager.AppSettings("SubPlatform")

            If Not IsNothing(strSubPlatform) Then
                Return [Enum].Parse(GetType(EnumHCVUSubPlatform), strSubPlatform)
            End If

            Return EnumHCVUSubPlatform.BO
        End Get
    End Property
    ' CRE19-026 (HCVS hotline service) [End][Winnie]
#End Region

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        _ScriptManager = ScriptManager.GetCurrent(Page)
        If _ScriptManager Is Nothing Then
            Throw New Exception("Page must has a ScriptManager")
        End If

        If Request.IsSecureConnection Then
            ' Setting the secure flag in the ASP.NET Session id cookie
            Request.Cookies("ASP.NET_SessionId").Secure = True
        End If

        Dim udtHCVUUser As HCVUUserModel = Nothing
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim objLang As Object
        Dim objLastTimeCheck As Object
        Dim objLastCheckCount As Object
        Dim objBackToDataEntryPage As Object
        Dim objDataEntryResult As Object
        Dim objDataEntrySearchCriteria As Object
        Dim objFromMain As Object
        Dim objSearchPage As Object
        Dim strFirstChangePassword As String = Nothing
        Dim objHCVUUser As Object = Nothing
        Dim objDataEntryAction As Object = Nothing
        Dim objDataEntryERN As Object = Nothing
        Dim objDataEntryTableLocation As Object = Nothing
        Dim objSPMigrationSearchCriteria As Object = Nothing
        Dim objPageRedirectorParam As Object = Nothing
        Dim objPageRedirectorSource As Object = Nothing
        Dim objIsCheckedForNotice As Object = Nothing
        Dim udtRedirectParameter As RedirectParameterModel = Nothing

        Dim objPageKey As Object = Nothing

        If Not IsPostBack() Then
            If HCVUUserBLL.Exist() Then
                udtHCVUUser = udtHCVUUserBLL.GetHCVUUser
            End If
            objLang = Session("language")
            objLastTimeCheck = Session(home.SESS_LastTimeCheck)
            objLastCheckCount = Session(home.SESS_LastCheckCount)
            objBackToDataEntryPage = Session("BackToDataEntryPage")
            objDataEntryResult = Session("DataEntryResult")
            objDataEntrySearchCriteria = Session("DataEntrySearchCriteria")
            objDataEntryAction = Session("EnrolAction")
            objDataEntryERN = Session("Enrolment_Ref_No")
            objDataEntryTableLocation = Session("TableLocation")

            objSPMigrationSearchCriteria = Session("SPMigrationSearchCriteria")
            objPageRedirectorParam = Session(eHSAccountEnquiry.SESSION_REDIRECT_PARAMETER) ' Same as spEnquiry.SESSION_REDIRECT_PARAMETER
            objPageRedirectorSource = Session(eHSAccountEnquiry.SESSION_REDIRECT_SOURCE) ' Same as spEnquiry.SESSION_REDIRECT_SOURCE

            objFromMain = Session(home.SESS_FromMain)
            objSearchPage = Session(home.SESS_SearchPage)
            If Not Session("FirstChangePassword") Is Nothing Then
                strFirstChangePassword = Session("FirstChangePassword")
            End If
            If Not Session("ChangePasswordHCVUUser") Is Nothing Then
                objHCVUUser = Session("ChangePasswordHCVUUser")
            End If

            objIsCheckedForNotice = Session(home.SESS_IsCheckedForNotice)

            Dim udtRedirectParameterBLL As New RedirectParameterBLL
            udtRedirectParameter = udtRedirectParameterBLL.GetFromSession()

            'I-CRE16-006 (Capture detail client browser and OS information) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim blnSESSUUA As Boolean = CommonSessionHandler.AddedUndefinedUserAgent
            'I-CRE16-006 (Capture detail client browser and OS information) [End][Chris YIM]

            Dim strFileGenerationID As String = Session("FileGenerateID")

            ' CRE19-026 (HCVS hotline service) [Start][Winnie]
            objPageKey = Session(SESS_PageKey)
            ' CRE19-026 (HCVS hotline service) [End][Winnie]

            Session.RemoveAll()

            Session("language") = objLang
            Session(home.SESS_LastTimeCheck) = objLastTimeCheck
            Session(home.SESS_LastCheckCount) = objLastCheckCount
            Session("BackToDataEntryPage") = objBackToDataEntryPage
            Session("DataEntryResult") = objDataEntryResult
            Session("DataEntrySearchCriteria") = objDataEntrySearchCriteria
            Session("EnrolAction") = objDataEntryAction
            Session("Enrolment_Ref_No") = objDataEntryERN
            Session("TableLocation") = objDataEntryTableLocation
            Session(home.SESS_FromMain) = objFromMain
            Session(home.SESS_SearchPage) = objSearchPage
            Session("SPMigrationSearchCriteria") = objSPMigrationSearchCriteria
            Session(eHSAccountEnquiry.SESSION_REDIRECT_PARAMETER) = objPageRedirectorParam ' Same as spEnquiry.SESSION_REDIRECT_PARAMETER
            Session(eHSAccountEnquiry.SESSION_REDIRECT_SOURCE) = objPageRedirectorSource ' Same as spEnquiry.SESSION_REDIRECT_SOURCE
            Session("FileGenerateID") = strFileGenerationID

            Session("IsCheckedForNotice") = objIsCheckedForNotice

            If Not strFirstChangePassword Is Nothing Then
                Session("FirstChangePassword") = strFirstChangePassword
            End If
            If Not objHCVUUser Is Nothing Then
                Session("ChangePasswordHCVUUser") = objHCVUUser
            End If

            If Not udtHCVUUser Is Nothing Then
                udtHCVUUserBLL.SaveToSession(udtHCVUUser)
            End If

            udtRedirectParameterBLL.SaveToSession(udtRedirectParameter)

            'I-CRE16-006 (Capture detail client browser and OS information) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            CommonSessionHandler.AddedUndefinedUserAgent = blnSESSUUA
            'I-CRE16-006 (Capture detail client browser and OS information) [End][Chris YIM]

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------

            ' Check opening hour
            If Me.FunctionCode <> String.Empty Then
                CheckOpenHour()
            End If

            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

            ' CRE19-026 (HCVS hotline service) [Start][Winnie]
            ' ------------------------------------------------------------------------
            Session(SESS_PageKey) = objPageKey
            ' CRE19-026 (HCVS hotline service) [End][Winnie]
        End If
    End Sub

    Public Property FunctionCode() As String
        Get
            Return ViewState("FunctionCode")
        End Get
        Set(ByVal value As String)
            ViewState("FunctionCode") = value
        End Set
    End Property

    Private Sub ScriptManager_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs) Handles _ScriptManager.AsyncPostBackError
        Dim udtFunctionInformationBLL As New FunctionInformationBLL
        Dim strFunctionCode As String = udtFunctionInformationBLL.GetFunctionCode
        ErrorHandler.HandleScriptManagerAsyncPostBackError(e, FunctionCode)
    End Sub

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
    ' -------------------------------------------------------------------------

    Public Overridable ReadOnly Property InfoMessageBox() As CustomControls.InfoMessageBox
        Get
            Return Nothing
        End Get
    End Property

    Public Overridable ReadOnly Property MessageBox() As CustomControls.MessageBox
        Get
            Return Nothing
        End Get
    End Property

    Public Sub CheckOpenHour()
        Dim udtFunctionInformationBLL As New Common.Component.FunctionInformation.FunctionInformationBLL
        Dim udtFunctionFeatureModel As Common.Component.FunctionInformation.FunctionFeatureModel

        Dim udtAuditLogEntry As AuditLogEntry
        udtAuditLogEntry = New AuditLogEntry(FunctionCode)

        udtFunctionFeatureModel = FunctionInformation.FunctionInformationBLL.GetFunctionFeatureModel(Me.FunctionCode, FunctionInformation.FunctionFeatureModel.EnumFeatureCode.SPECIFIC_OPENHOUR)

        If Not udtFunctionFeatureModel Is Nothing Then

            Select Case udtFunctionFeatureModel.IsOpeningHour
                Case True
                    ' Opening hour
                Case False
                    ' Closing hour
                    InfoMessageBox.AddMessageWithExtraInfo("010703", "I", "00001", GetOpenHourToDisplay)
                    InfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                    udtAuditLogEntry.WriteLog(Statistics.AuditLogDesc.StatEnq_NotAvailable_ID, Statistics.AuditLogDesc.StatEnq_NotAvailable)
                    InfoMessageBox.BuildMessageBox()
            End Select

        End If

    End Sub

    Public Function CheckActionAccessibility() As Boolean
        Dim udtFunctionInformationBLL As New Common.Component.FunctionInformation.FunctionInformationBLL
        Dim udtFunctionFeatureModel As Common.Component.FunctionInformation.FunctionFeatureModel
        Dim blnRes As Boolean = True

        Dim udtAuditLogEntry As AuditLogEntry
        udtAuditLogEntry = New AuditLogEntry(FunctionCode)

        udtFunctionFeatureModel = FunctionInformation.FunctionInformationBLL.GetFunctionFeatureModel(Me.FunctionCode, FunctionInformation.FunctionFeatureModel.EnumFeatureCode.SPECIFIC_OPENHOUR)

        If Not udtFunctionFeatureModel Is Nothing Then

            Select Case udtFunctionFeatureModel.IsOpeningHour
                Case True
                    ' Opening hour
                Case False
                    ' Closing hour
                    MessageBox.AddMessageWithExtraInfo("010703", "E", "00001", GetOpenHourToDisplay)
                    MessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Statistics.AuditLogDesc.StatEnq_NotAvailable_ID, Statistics.AuditLogDesc.StatEnq_NotAvailable)
                    blnRes = False

            End Select

        End If

        Return blnRes
    End Function

    Public Function GetOpenHourToDisplay() As String
        Dim strRes As String = String.Empty
        Dim udtFunctionInformationBLL As New Common.Component.FunctionInformation.FunctionInformationBLL
        Dim udtFunctionFeatureModel As Common.Component.FunctionInformation.FunctionFeatureModel

        udtFunctionFeatureModel = FunctionInformation.FunctionInformationBLL.GetFunctionFeatureModel(Me.FunctionCode, FunctionInformation.FunctionFeatureModel.EnumFeatureCode.SPECIFIC_OPENHOUR)

        If Not udtFunctionFeatureModel Is Nothing Then

            strRes += "<ul style='padding-left: 20px; margin: 0px'>"
            For Each udtFeatureOpenHourModel As Common.Component.FunctionInformation.FeatureOpenHourModel In udtFunctionFeatureModel.FeatureOpenHours
                strRes += "<li>" + udtFeatureOpenHourModel.FromTime + " - " + udtFeatureOpenHourModel.ToTime + "</li>"
            Next
            strRes += "</ul>"

        End If

        Return strRes
    End Function

    ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

    Public Sub preventMultiImgClick(cs As ClientScriptManager, ibtn As ImageButton)
        Dim strScript As String = "if (this.style.cursor != 'wait') { this.style.cursor = 'wait'; return true; } else { this.disabled = true; return false; }"

        ibtn.Attributes.Add("onclick", strScript)

    End Sub

    Protected Overrides Sub InitializeCulture()

        Dim strLanguage As String = "en-us"

        'Set the Culture.
        Thread.CurrentThread.CurrentCulture = New CultureInfo(strLanguage)
        Thread.CurrentThread.CurrentUICulture = New CultureInfo(strLanguage)

    End Sub


    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Protected Overrides Sub OnPreLoad(ByVal e As System.EventArgs)
        If Me.IsTurnOnConcurrentBrowserHandling Then
            MyBase.OnPreLoad(e)
            If Me.IsPostBack Then
                If Not Me.Master Is Nothing AndAlso Not Me.Master.FindControl(SESS_PageKey) Is Nothing Then
                    Me._PostBackPageKey = CType(Me.Master.FindControl(SESS_PageKey), TextBox).Text
                    PreCheckConcurrentAccessForHttpPost()
                End If
            End If
        End If
    End Sub

    Public Sub PreCheckConcurrentAccessForHttpPost()
        If Me.IsTurnOnConcurrentBrowserHandling Then
            If Me.IsPostBack Then
                If Me._PostBackPageKey = String.Empty Then
                    RedirectToInvalidAccessErrorPage()
                Else
                    If Not Me._PostBackPageKey = String.Empty AndAlso Session(SESS_PageKey) Is Nothing Then
                        ' Session Timout
                        Throw New Exception("Session Expired!")
                    Else
                        If Not Me._PostBackPageKey = String.Empty AndAlso Session(SESS_PageKey) IsNot Nothing AndAlso Me._PostBackPageKey = Session(SESS_PageKey).ToString() Then
                            ' Don't RenewPageKey
                        Else
                            RedirectToInvalidAccessErrorPage()
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Function IsTurnOnConcurrentBrowserHandling() As Boolean

        Dim isTurnOn As Boolean = False

        Dim strConcurrentBrowserHandling As String = String.Empty

        strConcurrentBrowserHandling = ConfigurationManager.AppSettings("ConcurrentBrowserHandling")

        If strConcurrentBrowserHandling.Trim.Equals("Y") Then
            isTurnOn = True
        End If

        Return isTurnOn

    End Function

    Private Sub RedirectToInvalidAccessErrorPage()

        Dim udtAuditLogEntry As New AuditLogEntry(_FunctCodeCommon, Me)

        udtAuditLogEntry.AddDescripton(SESS_PageKey, Me._PostBackPageKey)
        udtAuditLogEntry.WriteLog(AuditLogDescription.Redirect_InvalidAccessPage_ID, AuditLogDescription.Redirect_InvalidAccessPage)

        Response.Redirect("~/ImproperAccess.aspx")
    End Sub
    ' CRE19-026 (HCVS hotline service) [End][Winnie]

End Class
