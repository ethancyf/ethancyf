Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component

Imports Common.ComFunction
Imports Common.Format

Namespace PrintOut.VSSConsentForm_CHI

    Public Class VSSSignature_CHI

        ' Model in use
        Private _udtEHSPersonalInformation As EHSPersonalInformationModel
        Private _udtEHSTransaction As EHSTransactionModel

        ' Helper class
        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction

        End Sub

        Public Sub New(ByRef udtEHSPersonalInformation As EHSPersonalInformationModel, ByRef udtEHSTransaction As EHSTransactionModel)
            ' Invoke default constructor
            Me.New()

            _udtEHSPersonalInformation = udtEHSPersonalInformation
            _udtEHSTransaction = udtEHSTransaction

            LoadReport()

        End Sub

        Private Sub LoadReport()

            ' Fill Personal Info
            srPersonalInfo.Report = New Common.VSSPersonalInfo_CHI(_udtEHSPersonalInformation)

            ' Fill Identity Document
            srIdentityDocument.Report = New Common.VSSDocType.IdentityDocument_CHI(_udtEHSPersonalInformation)

            ' Fill in Date
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDisplayDate(DateTime.Today, CultureLanguage.TradChinese), txtRecipientDate)

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            ' Signature of Guardian

            'If _udtEHSTransaction.CategoryCode.Equals(CategoryCode.VSS_ELDER) Then
            '    srSignatureGuardian.Report = New VSSSignature_E_CHI()
            'Else
            '    srSignatureGuardian.Report = New VSSSignature_DA_P_PW_CHI()
            'End If

            Select _udtEHSTransaction.CategoryCode
                Case CategoryCode.VSS_ELDER, CategoryCode.VSS_ADULT
                    srSignatureGuardian.Report = New VSSSignature_E_CHI()

                Case Else
                    srSignatureGuardian.Report = New VSSSignature_DA_P_PW_CHI()

            End Select
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End Sub

        Private Sub Detail_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Detail.Format

        End Sub
    End Class


End Namespace