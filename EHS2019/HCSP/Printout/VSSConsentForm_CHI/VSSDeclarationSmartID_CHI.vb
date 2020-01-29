Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.EHSTransaction
Imports Common.Component

Namespace PrintOut.VSSConsentForm_CHI

    Public Class VSSDeclarationSmartID_CHI

        '' Model in use
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
            Dim strDeclarationSmartID As String = HttpContext.GetGlobalResourceObject("PrintoutText", "VSS_Declaration_SmartID", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Select _udtEHSTransaction.CategoryCode
                Case CategoryCode.VSS_CHILD, CategoryCode.EVSSO_CHILD
                    txtDeclaration2Value.Text = HttpContext.GetGlobalResourceObject("PrintoutText", "VSS_Declaration_2_Child", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
                    txtDeclaration3Value.Text = Replace(Replace(strDeclarationSmartID, "%s", "本人子女/受監護者"), "%e", "本人")

                Case Else
                    txtDeclaration2Value.Text = HttpContext.GetGlobalResourceObject("PrintoutText", "VSS_Declaration_2_Others", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
                    txtDeclaration3Value.Text = Replace(Replace(strDeclarationSmartID, "%s", "本人或此同意書中的服務使用者"), "%e", "本人")

            End Select
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]
        End Sub

    End Class


End Namespace