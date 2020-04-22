Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DataEntryUser
Imports Common.Component.Scheme
Imports Common.Component.SchemeInformation
Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.UserAC

Partial Public Class ProperPractice
    Inherits BasePage

#Region "Properties"

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    Protected ReadOnly Property PageLanguage() As String
        Get
            If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                Return "lang=""zh"""
            Else
                Return String.Empty
            End If
        End Get
    End Property
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        basetag.Attributes("href") = (New GeneralFunction).getPageBasePath

        InitControlAlways()

        ' Check login session
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC()

        ' Check which section is available to this SP
        Dim udtSP As ServiceProviderModel = Nothing
        Dim aryDataEntryPracticeList As ArrayList = Nothing

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            udtSP = udtUserAC
        Else
            Dim udtDataEntry As DataEntryUserModel = udtUserAC
            udtSP = udtDataEntry.ServiceProvider
            aryDataEntryPracticeList = udtDataEntry.PracticeList
        End If

        ' Convert scheme enrol to scheme claim list

        ' Get all SP Scheme
        Dim udtSchemeList As SchemeInformationModelCollection = udtSP.SchemeInfoList

        ' Get all Practice Scheme
        Dim udtFilterPracticeSchemeList As New PracticeSchemeInfoModelCollection

        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            If Not IsNothing(aryDataEntryPracticeList) Then
                If Not aryDataEntryPracticeList.Contains(udtPractice.DisplaySeq) Then Continue For
            End If

            For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                Dim strPracticeSchemeStatus As String = udtPracticeScheme.RecordStatus
                Dim strSchemeStatus As String = udtSchemeList.Filter(udtPracticeScheme.SchemeCode).RecordStatus

                If (strPracticeSchemeStatus = PracticeSchemeInfoMaintenanceDisplayStatus.Active _
                            OrElse strPracticeSchemeStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend _
                            OrElse strPracticeSchemeStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist) _
                        AndAlso (strSchemeStatus = SchemeInformationMaintenanceDisplayStatus.Active _
                            OrElse strSchemeStatus = SchemeInformationMaintenanceDisplayStatus.ActivePendingSuspend _
                            OrElse strSchemeStatus = SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist) Then
                    udtFilterPracticeSchemeList.Add(udtPracticeScheme)
                End If

            Next

        Next

        udtFilterPracticeSchemeList = udtFilterPracticeSchemeList.FilterByHCSPSubPlatform(Me.SubPlatform)

        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtFilterPracticeSchemeList)

        ' HCVS
        trPP01.Visible = CheckSectionVisible("PP01", udtSchemeClaimList)
        trPP02.Visible = CheckSectionVisible("PP02", udtSchemeClaimList)

        ' Audit log
        Dim udtAuditLogEntry As New AuditLogEntry("000015", Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00000, "Proper Practice Page Loaded")

    End Sub

    Private Sub InitControlAlways()
        tdHeader.Style("background-image") = String.Format("url({0})", Me.GetGlobalResourceObject("ImageUrl", "Banner"))
        lbtnPP01.OnClientClick = String.Format("javascript:openNewHTML('{0}');return false;", Me.GetGlobalResourceObject("Text", "ProperPractice_PP01_Link"))
        lbtnPP02.OnClientClick = String.Format("javascript:openNewHTML('{0}');return false;", Me.GetGlobalResourceObject("Text", "ProperPractice_PP02_Link"))

        Dim udtGeneralFunction As New GeneralFunction

        ' Link Javascript
        Dim strPrivacyPolicyLink As String = String.Empty

        Select Case Session("language")
            Case CultureLanguage.English
                udtGeneralFunction.getSystemParameter("PrivacyPolicyLink", strPrivacyPolicyLink, String.Empty)
            Case CultureLanguage.TradChinese
                udtGeneralFunction.getSystemParameter("PrivacyPolicyLink_CHI", strPrivacyPolicyLink, String.Empty)
            Case CultureLanguage.SimpChinese
                udtGeneralFunction.getSystemParameter("PrivacyPolicyLink_CN", strPrivacyPolicyLink, String.Empty)
            Case Else
                Throw New Exception(String.Format("Unexpected value (Session(language)={0})", Session("language")))
        End Select

        lnkBtnPrivacyPolicy.OnClientClick = "javascript:openNewHTML('" + strPrivacyPolicyLink + "');return false;"

        Dim strDisclaimerPolicyLink As String = String.Empty

        Select Case Session("language")
            Case CultureLanguage.English
                udtGeneralFunction.getSystemParameter("DisclaimerLink", strDisclaimerPolicyLink, String.Empty)
            Case CultureLanguage.TradChinese
                udtGeneralFunction.getSystemParameter("DisclaimerLink_CHI", strDisclaimerPolicyLink, String.Empty)
            Case CultureLanguage.SimpChinese
                udtGeneralFunction.getSystemParameter("DisclaimerLink_CN", strDisclaimerPolicyLink, String.Empty)
            Case Else
                Throw New Exception(String.Format("Unexpected value (Session(language)={0})", Session("language")))
        End Select

        lnkBtnDisclaimer.OnClientClick = "javascript:openNewHTML('" + strDisclaimerPolicyLink + "');return false;"

        Dim strSysMaintLink As String = String.Empty

        Select Case Session("language")
            Case CultureLanguage.English
                udtGeneralFunction.getSystemParameter("SysMaintLink", strSysMaintLink, String.Empty)
            Case CultureLanguage.TradChinese
                udtGeneralFunction.getSystemParameter("SysMaintLink_CHI", strSysMaintLink, String.Empty)
            Case CultureLanguage.SimpChinese
                udtGeneralFunction.getSystemParameter("SysMaintLink_CN", strSysMaintLink, String.Empty)
            Case Else
                Throw New Exception(String.Format("Unexpected value (Session(language)={0})", Session("language")))
        End Select

        lnkBtnSysMaint.OnClientClick = "javascript:openNewHTML('" + strSysMaintLink + "');return false;"

        ' CRE15-006 Rename of eHS [Start][Lawrence]
        lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

        If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty

        Select Case Session("language")
            Case CultureLanguage.TradChinese, CultureLanguage.SimpChinese
                tdHeader.Attributes("class") = "AppEnvironmentZH"
            Case Else
                tdHeader.Attributes("class") = "AppEnvironment"
        End Select

        ' CRE15-006 Rename of eHS [End][Lawrence]

    End Sub

    Private Function CheckSectionVisible(ByVal strSectionID As String, ByVal udtSchemeClaimList As SchemeClaimModelCollection) As Boolean
        For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList
            If udtSchemeClaim.ProperPracticeAvail AndAlso udtSchemeClaim.ProperPracticeSectionID = strSectionID Then
                Return True
            End If
        Next

        Return False

    End Function

#Region "Implement IWorkingData (CRE11-004)"

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

#End Region

End Class
