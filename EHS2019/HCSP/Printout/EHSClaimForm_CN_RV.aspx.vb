Imports Common.Component.Printout
Imports Common.Component.Scheme
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.ClaimRules
Imports Common.Component.ClaimRules.ClaimRulesBLL
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
'Imports ConsentFormEHS
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Imports HCSP.PrintOut
Imports HCSP.PrintOut.Common

Partial Public Class EHSClaimForm_CN_RV
    Inherits BasePrintoutForm

    Public Const FunctCode As String = Common.Component.FunctCode.FUNT020201

    Dim _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
    Dim _udtClaimRules As ClaimRulesBLL = New ClaimRulesBLL()
    Dim _udtPrintoutBLL As PrintoutBLL = New PrintoutBLL
    Dim _udtPrintoutHelper As PrintoutHelper = New PrintoutHelper()

    Public Overrides ReadOnly Property Language() As String
        Get
            Return ConsentFormInformationModel.LanguageClassInternal.SimpChinese
        End Get
    End Property

    Public Overrides ReadOnly Property FormStyle() As String
        Get
            Return ConsentFormInformationModel.FormStyleClass.Full
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LoadReport()

    End Sub


    Overrides Function GetReport() As GrapeCity.ActiveReports.SectionReport
        Dim objReport As GrapeCity.ActiveReports.SectionReport = Nothing
        Dim strFunctCode As String = FunctCode
        Dim strSessionFunctCode As String = _udtSessionHandler.EHSClaimPrintoutFunctionCodeGetFromSession()
        If Not String.IsNullOrEmpty(strSessionFunctCode) Then
            strFunctCode = strSessionFunctCode
        End If

        Throw New Exception("Unknown report")

        Return objReport
    End Function

End Class