Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.EHSTransaction
Imports Common.Component

Namespace PrintOut.VSSConsentForm_CHI

    Public Class VSSDeclaration_CHI

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel)
            Me.New()

            _udtEHSTransaction = udtEHSTransaction

            LoadReport()
        End Sub

        Private Sub LoadReport()

            ' Document Explained By

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Select Case _udtEHSTransaction.CategoryCode
                Case CategoryCode.VSS_CHILD, CategoryCode.EVSSO_CHILD
                    txtDeclaration2Value.Text = HttpContext.GetGlobalResourceObject("PrintoutText", "VSS_Declaration_2_Child", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

                Case Else
                    txtDeclaration2Value.Text = HttpContext.GetGlobalResourceObject("PrintoutText", "VSS_Declaration_2_Others", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
            End Select
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End Sub

    End Class


End Namespace