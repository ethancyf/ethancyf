Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.ClaimRules
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component

Namespace PrintOut.HSIVSSConsentForm
    Public Class HSIVSSDeclarationSmartID

        ' Model in use
        Private _blnIsAdult As Boolean
        Private _udtSP As ServiceProviderModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtSchemeClaim As SchemeClaimModel

        Private _udtClaimRules As ClaimRulesBLL = New ClaimRulesBLL()

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByVal blnIsAdult As Boolean, ByVal udtSP As ServiceProviderModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSchemeClaim As SchemeClaimModel)
            Me.New()

            ' Init variable
            _blnIsAdult = blnIsAdult
            _udtSP = udtSP
            _udtEHSAccount = udtEHSAccount
            _udtEHSTransaction = udtEHSTransaction
            _udtSchemeClaim = udtSchemeClaim

            LoadReport()

        End Sub

        Private Sub LoadReport()

            ' SP Name
            Dim strSPName As String = _udtSP.EnglishName

            If strSPName.Length <= 20 Then
                srDeclaration6.Report = New HSIVSSDeclarationSPName20(_blnIsAdult, _udtSP, _udtEHSTransaction, _udtEHSAccount)
            ElseIf strSPName.Length <= 30 Then
                srDeclaration6.Report = New HSIVSSDeclarationSPName30(_blnIsAdult, _udtSP, _udtEHSTransaction, _udtEHSAccount)
            Else
                srDeclaration6.Report = New HSIVSSDeclarationSPName40(_blnIsAdult, _udtSP, _udtEHSTransaction, _udtEHSAccount)
            End If

            ' Adult / Child Adjustment
            AdjustReportContent()

        End Sub

        Private Sub AdjustReportContent()
            ' Update the text according to Adult / Child
            If _blnIsAdult = True Then
                txtDeclaration4RoleValue.Text = "me."
                Me.srDeclarationSmartID.Report = New HSIVSSDeclarationSmartIDDeclaration_My()
            Else
                txtDeclaration4RoleValue.Text = "my child."
                Me.srDeclarationSmartID.Report = New HSIVSSDeclarationSmartIDDeclaration_MyChild()
            End If


            '' Check Age < 9
            ' If _udtClaimRules.CheckIVSSAge(_udtEHSTransaction.ServiceDate, SchemeClaimModel.HSIVSS, _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode), 9, ClaimRulesBLL.DOBCalUnitClass.ExactYear, "<") Then
            Dim udtHSIVSSHelper As Common.HSIVSSPrintoutHelper = New Common.HSIVSSPrintoutHelper()
            If udtHSIVSSHelper.IsShowDoseInformation(_udtEHSTransaction.ServiceDate, SchemeClaimModel.HSIVSS, _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode)) Then
                ' Show Declaration 6
                srDeclaration7.Report = New HSIVSSDeclarationDoseInfo(_udtEHSTransaction, _udtSchemeClaim)
            Else
                ' Hide Declaration 6
                txtDeclaration7.Visible = False
                srDeclaration7.Visible = False
                Me.Detail.Height = 1.88
            End If

        End Sub

    End Class


End Namespace