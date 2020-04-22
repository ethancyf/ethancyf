Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.ClaimRules
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component

Namespace PrintOut.HSIVSSConsentForm_CHI

    Public Class HSIVSSDeclaration_CHI

        ' Model in use
        Private _blnIsAdult As Boolean
        Private _udtSP As ServiceProviderModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtSchemeClaim As SchemeClaimModel

        'Private _udtClaimRules As ClaimRulesBLL = New ClaimRulesBLL()

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

            If Not String.IsNullOrEmpty(_udtSP.ChineseName) Then
                srDeclaration5.Report = New HSIVSSDeclarationSPName6_CHI(_blnIsAdult, _udtSP, _udtEHSTransaction)
            Else
                Dim strSPName As String = _udtSP.EnglishName
                If strSPName.Length <= 20 Then
                    srDeclaration5.Report = New HSIVSSDeclarationSPName20_CHI(_blnIsAdult, _udtSP, _udtEHSTransaction)
                ElseIf strSPName.Length <= 30 Then
                    srDeclaration5.Report = New HSIVSSDeclarationSPName30_CHI(_blnIsAdult, _udtSP, _udtEHSTransaction)
                Else
                    srDeclaration5.Report = New HSIVSSDeclarationSPName40_CHI(_blnIsAdult, _udtSP, _udtEHSTransaction)
                End If
            End If

            'Adult/Child Adjustment
            AdjustReportContent()

        End Sub

        Private Sub AdjustReportContent()
            ' Update the text according to Adult / Child
            If _blnIsAdult = True Then
                txtDeclaration4Value.Text = "本人在此同意書中所提供的資料是真確。本人同意把同意書中的個人資料及是次到訪向醫生提供的資料供政府用於「收集個人資料目的」所述的用途。本人備悉當局或會與我聯絡，以核實有關資料及本人接種疫苗事宜。"
            Else
                txtDeclaration4Value.Text = "本人在此同意書中所提供的資料是真確。本人同意把同意書中的個人資料及是次到訪向醫生提供的資料供政府用於「收集個人資料目的」所述的用途。本人備悉當局或會與我聯絡，以核實有關資料及本人子女接種疫苗事宜。"
            End If

            '' Check Age < 9
            'If _udtClaimRules.CheckIVSSAge(_udtEHSTransaction.ServiceDate, SchemeClaimModel.HSIVSS, _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode), 9, ClaimRulesBLL.DOBCalUnitClass.ExactYear, "<") Then
            Dim udtHSIVSSHelper as Common.HSIVSSPrintoutHelper = new Common.HSIVSSPrintoutHelper()
            if udtHSIVSSHelper.IsShowDoseInformation(_udtEHSTransaction.ServiceDate, SchemeClaimModel.HSIVSS, _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode)) then
                ' Show 6
                srDeclaration6.Report = New HSIVSSDeclarationDoseInfo_CHI(_udtEHSTransaction, _udtSchemeClaim)
            Else
                ' Hide 6
                txtDeclaration6.Visible = False
                srDeclaration6.Visible = False
                Me.Detail.Height = 1.05
            End If

        End Sub

    End Class


End Namespace