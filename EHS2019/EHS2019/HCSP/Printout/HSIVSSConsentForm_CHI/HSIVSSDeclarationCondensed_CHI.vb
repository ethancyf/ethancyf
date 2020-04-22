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
    Public Class HSIVSSDeclarationCondensed_CHI

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

            ' Document Explained By
            Dim strSPChineseName As String = _udtSP.ChineseName
            Dim strSPEnglishName As String = _udtSP.EnglishName

            If String.IsNullOrEmpty(strSPChineseName) Then
                ' Show English Name
                If strSPEnglishName.Length <= 20 Then
                    srDeclaration4.Report = New HSIVSSDeclarationSPName20_CHI(_blnIsAdult, _udtSP, _udtEHSTransaction)
                ElseIf strSPEnglishName.Length <= 30 Then
                    srDeclaration4.Report = New HSIVSSDeclarationSPName30_CHI(_blnIsAdult, _udtSP, _udtEHSTransaction)
                Else
                    srDeclaration4.Report = New HSIVSSDeclarationSPName40_CHI(_blnIsAdult, _udtSP, _udtEHSTransaction)
                End If
            Else
                ' Show Chinese Name
                srDeclaration4.Report = New HSIVSSDeclarationSPName6_CHI(_blnIsAdult, _udtSP, _udtEHSTransaction)
            End If

            ' > 9 Years Old Adjustment
            AdjustReportContent()

        End Sub

        Private Sub AdjustReportContent()

            '' Check Age < 9
            'If _udtClaimRules.CheckIVSSAge(_udtEHSTransaction.ServiceDate, SchemeClaimModel.HSIVSS, _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode), 9, ClaimRulesBLL.DOBCalUnitClass.ExactYear, "<") Then
            Dim udtHSIVSSHelper As Common.HSIVSSPrintoutHelper = New Common.HSIVSSPrintoutHelper()
            If udtHSIVSSHelper.IsShowDoseInformation(_udtEHSTransaction.ServiceDate, SchemeClaimModel.HSIVSS, _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode)) Then
                ' Show 5
                srDeclaration5.Report = New HSIVSSDeclarationDoseInfo_CHI(_udtEHSTransaction, _udtSchemeClaim)
            Else
                ' Hide 5
                txtDeclaration5.Visible = False
                srDeclaration5.Visible = False
                Me.Detail.Height = 0.98
            End If

        End Sub

    End Class
End Namespace
