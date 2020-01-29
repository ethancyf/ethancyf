Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSAccount
Imports Common.Component.ClaimRules
Imports Common.Component.Scheme
Imports Common.Component

Imports Common.Format


Namespace PrintOut.HSIVSSConsentForm

    Public Class HSIVSSDeclarationSPName30

        ' Model in use
        Private _blnIsAdult As Boolean
        Private _udtSP As ServiceProviderModel
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtEHSAccount As EHSAccountModel

        ' Helper class
        Private _udtClaimRules As ClaimRulesBLL = New ClaimRulesBLL()
        Private _udtFormatter As Formatter = New Formatter()

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByVal blnIsAdult As Boolean, ByVal udtSP As ServiceProviderModel, ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel)
            Me.New()

            ' Init variable
            _blnIsAdult = blnIsAdult
            _udtSP = udtSP
            _udtEHSTransaction = udtEHSTransaction
            _udtEHSAccount = udtEHSAccount

            LoadReport()

        End Sub

        Private Sub LoadReport()

            ' Fill in SPName
            txtSPName.Text = _udtSP.EnglishName

            'Fill in Date of Vaccination
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'txtServiceDate.Text = _udtFormatter.formatDate(_udtEHSTransaction.ServiceDate, CultureLanguage.English)
            txtServiceDate.Text = _udtFormatter.formatDisplayDate(_udtEHSTransaction.ServiceDate, CultureLanguage.English)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            'Fill in Age Role Description
            If (_blnIsAdult) Then
                chkAgeRole.Text = "to use government subsidy for myself to receive human swine influenza vaccination"
            Else
                chkAgeRole.Text = "for my child to use government subsidy to receive human swine influenza vaccination"
            End If

            '' Check Age > 9
            'If _udtClaimRules.CheckIVSSAge(_udtEHSTransaction.ServiceDate, SchemeClaimModel.HSIVSS, _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode), 9, ClaimRulesBLL.DOBCalUnitClass.ExactYear, ">=") Then
            Dim udtHSIVSSHelper As Common.HSIVSSPrintoutHelper = New Common.HSIVSSPrintoutHelper()
            If udtHSIVSSHelper.IsShowDoseInformation(_udtEHSTransaction.ServiceDate, SchemeClaimModel.HSIVSS, _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode)) Then
                txtEndingCharacter.Text = ":"
            Else
                txtEndingCharacter.Text = "."
            End If

        End Sub

    End Class


End Namespace