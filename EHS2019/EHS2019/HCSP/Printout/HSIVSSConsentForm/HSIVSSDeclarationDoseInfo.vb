Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails

Namespace PrintOut.HSIVSSConsentForm
    Public Class HSIVSSDeclarationDoseInfo

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtSchemeClaim As SchemeClaimModel

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSchemeClaim As SchemeClaimModel)
            Me.New()

            ' Init variable
            _udtEHSTransaction = udtEHSTransaction
            _udtSchemeClaim = udtSchemeClaim

            LoadReport()

        End Sub

        Private Sub LoadReport()

            'Fill in Dose Header Text
            Dim strPrefix As String = HttpContext.GetGlobalResourceObject("PrintoutText", "HSIVSS_DoseHeaderPrefix", New System.Globalization.CultureInfo(CultureLanguage.English))
            Dim strDescription As String
            If Not String.IsNullOrEmpty(strPrefix) Then
                strDescription = String.Format("{0} This is:", strPrefix)
            Else
                strDescription = "This is:"
            End If
            txtDescription.Text = strDescription

            'Fill in Dose Description
            chkSubsidizeItem.Text = GetDoseDescription(_udtEHSTransaction, _udtSchemeClaim)

        End Sub

        Private Function GetDoseDescription(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSchemeClaim As SchemeClaimModel) As String
            Dim udtPrintoutHelper As Common.PrintoutHelper = New Common.PrintoutHelper()
            Dim udtClaimRuleBLL As New ClaimRules.ClaimRulesBLL()
            Dim udtServiceSchemeClaimModel As SchemeClaimModel = Nothing
            udtClaimRuleBLL.CheckSchemeClaimModelByServiceDate(udtEHSTransaction.ServiceDate, udtSchemeClaim, udtServiceSchemeClaimModel)

            For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtServiceSchemeClaimModel.SubsidizeGroupClaimList
                Dim udtTransactionDetail As TransactionDetailModelCollection = udtEHSTransaction.TransactionDetails.FilterBySubsidize(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode)
                If udtTransactionDetail.Count > 0 Then
                    ' Contain Information and assume only 1 for each subsidize
                    Dim strVaccineCode As String = udtTransactionDetail(0).AvailableItemCode
                    Return udtPrintoutHelper.GetDoseDescription(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeItemCode, strVaccineCode, CultureLanguage.English)
                End If
            Next

            Return String.Empty
        End Function

    End Class


End Namespace