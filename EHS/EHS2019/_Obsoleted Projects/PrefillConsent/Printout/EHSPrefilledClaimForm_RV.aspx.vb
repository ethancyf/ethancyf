Imports Common.Component.Scheme
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports PrefillConsent.PrintOut
Imports Common.ComObject
Imports Common.Component

Partial Public Class EHSPrefilledClaimForm_RV
    Inherits BasePrintoutForm

    Public Const FunctCode As String = Common.Component.FunctCode.FUNT050101

    Dim udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LoadReport()

    End Sub

    Overrides Function GetReport() As GrapeCity.ActiveReports.SectionReport
        Dim objReport As GrapeCity.ActiveReports.SectionReport = Nothing

        ' Get required object from session
        Dim strPrefilledNumber As String = udtSessionHandler.PreFillConsentIDGetFormSession(FunctCode)
        Dim udtEHSAccount As EHSAccountModel = udtSessionHandler.EHSAccountGetFromSession(FunctCode)


        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode, Me)
        'Dim objAuditLogInfo As AuditLogInfo = New AuditLogInfo(Nothing, Nothing, udtEHSAccount.AccountSourceString, udtEHSAccount.VoucherAccID, Nothing, Nothing)
        udtAuditLogEntry.WriteLog(LogID.LOG00022, "Print Consent Form English")


        ' Create the report instance

        ' Only have CIVSS Prefilled Form
        If Not String.IsNullOrEmpty(strPrefilledNumber) AndAlso _
           Not udtEHSAccount Is Nothing Then
            objReport = New CIVSSConsentForm.CIVSSConsentFormPrefilled(strPrefilledNumber, udtEHSAccount)
        End If

        Return objReport
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return udtSessionHandler.EHSAccountGetFromSession(FunctCode)
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
        Dim udtEHSAccountModel As EHSAccountModel = GetEHSAccount()
        Dim udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccountModel.EHSPersonalInformationList(0)

        If Not IsNothing(udtPersonalInformation) Then
            Return udtPersonalInformation.DocCode
        Else
            Return Nothing
        End If
    End Function
    ''' <summary>
    ''' CRE11-004
    '''  Clear all working data
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub ClearWorkingData()
        MyBase.ClearWorkingData()
    End Sub
End Class