Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component
Imports Common.Component.ClaimRules
Imports Common.Component.ClaimRules.ClaimRulesBLL

Imports Common.Format
Imports Common.ComFunction

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSSubsidyInfo_CHI

        ' Model in use
        Private _udtSchemeClaim As SchemeClaimModel
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtEHSPersonalInformation As EHSPersonalInformationModel

        ' Helper class
        Private _udtGeneralFunction As GeneralFunction
        Private _udtClaimRulesBLL As ClaimRulesBLL

#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtGeneralFunction = New GeneralFunction
            _udtClaimRulesBLL = New ClaimRulesBLL()

        End Sub

        Public Sub New(ByRef udtSchemeClaim As SchemeClaimModel, ByRef udtEHSTransaction As EHSTransactionModel, ByRef udtEHSPersonalInformation As EHSPersonalInformationModel)
            Me.New()

            _udtSchemeClaim = udtSchemeClaim
            _udtEHSTransaction = udtEHSTransaction
            _udtEHSPersonalInformation = udtEHSPersonalInformation

            LoadReport()

        End Sub
#End Region

        Private Sub LoadReport()

            ' Identify the vaccination is from the First Dose / Second Dose
            FillDoseInfo()

        End Sub


        Private Sub FillDoseInfo()
            ' Show the Available Vaccine according to the model provided
            Dim udtClaimRuleBLL As New ClaimRules.ClaimRulesBLL()
            Dim udtPrintoutHelper As Common.PrintoutHelper = New Common.PrintoutHelper()
            Dim udtServiceSchemeClaimModel As SchemeClaimModel = Nothing

            ' refresh scheme info according to service date
            'udtClaimRuleBLL.CheckSchemeClaimModelByServiceDate(_udtEHSTransaction.ServiceDate, _udtSchemeClaim, udtServiceSchemeClaimModel)
            udtServiceSchemeClaimModel = (New SchemeClaimBLL) _
                .getValidClaimPeriodSchemeClaimWithSubsidizeGroup(_udtSchemeClaim.SchemeCode, _udtEHSTransaction.ServiceDate.AddDays(1).AddMinutes(-1))

            For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtServiceSchemeClaimModel.SubsidizeGroupClaimList
                Dim udtTransactionDetails As TransactionDetailModelCollection = _udtEHSTransaction.TransactionDetails.FilterBySubsidize(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode)
                If udtTransactionDetails.Count > 0 Then
                    ' Contain Information and assume only 1
                    Dim strVaccineCode As String = udtTransactionDetails(0).AvailableItemCode
                    'CRE15-004 TIV & QIV [Start][Philip]
                    Dim strDescription As String = udtPrintoutHelper.GetDoseDescription(udtSubsidizeGroupClaim.SchemeCode, udtSubsidizeGroupClaim.SchemeSeq, udtSubsidizeGroupClaim.SubsidizeCode, strVaccineCode, CultureLanguage.TradChinese)
                    'CRE15-004 TIV & QIV [End][Philip]
                    chkSubsidizeItemTemplate.Text = strDescription

                    Select Case strVaccineCode
                        Case "1STDOSE", "ONLYDOSE"
                            If _udtEHSTransaction.PreSchool = "Y" Then
                                txtSubsidyInformatin.Text = HttpContext.GetGlobalResourceObject("PrintoutText", "CIVSS_PreSchool", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
                            Else
                                txtSubsidyInformatin.Text = HttpContext.GetGlobalResourceObject("PrintoutText", "CIVSS_NonPreSchool", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
                            End If

                        Case Else
                            txtSubsidyInformatin.Visible = False
                            Detail.Height = chkSubsidizeItemTemplate.Height
                    End Select

                End If
            Next
        End Sub

    End Class
End Namespace
