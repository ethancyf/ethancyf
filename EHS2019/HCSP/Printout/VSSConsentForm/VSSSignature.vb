Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component

Imports Common.ComFunction
Imports Common.Format

Namespace PrintOut.VSSConsentForm

    Public Class VSSSignature

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
            srPersonalInfo.Report = New Common.VSSPersonalInfo(_udtEHSPersonalInformation)

            ' Fill Identity Document
            srIdentityDocument.Report = New Common.VSSDocType.IdentityDocument(_udtEHSPersonalInformation)

            ' Fill in Date
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDisplayDate(DateTime.Today, CultureLanguage.English), txtRecipientDate)

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            ' Signature of Guardian

            'If _udtEHSTransaction.CategoryCode.Equals(CategoryCode.VSS_ELDER) Then
            '    srSignatureGuardian.Report = New VSSSignature_E()
            'Else
            '    srSignatureGuardian.Report = New VSSSignature_DA_P_PW()
            'End If

            Select Case _udtEHSTransaction.CategoryCode
                Case CategoryCode.VSS_ELDER, CategoryCode.VSS_ADULT
                    srSignatureGuardian.Report = New VSSSignature_E()

                Case Else
                    srSignatureGuardian.Report = New VSSSignature_DA_P_PW()

            End Select
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]


        End Sub

    End Class


End Namespace