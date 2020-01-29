Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component
Imports Common.ComFunction

Namespace PrintOut.VSSConsentForm
    Public Class VSSDeclarationCondensedSmartID

        ' Model in use
        Private _udtSP As ServiceProviderModel
        Private _udtEHSTransaction As EHSTransactionModel

        ' Helper Class
        Private _udtReportFunction As ReportFunction

#Region "Constructor"
        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtReportFunction = New ReportFunction

        End Sub

        Public Sub New(ByRef udtSP As ServiceProviderModel, ByRef udtEHSTransaction As EHSTransactionModel)
            Me.New()

            ' Init variable
            _udtSP = udtSP
            _udtEHSTransaction = udtEHSTransaction

            LoadReport()

        End Sub
#End Region

        Private Sub LoadReport()

            ' Document Explained By
            _udtReportFunction.FillSPName(_udtSP.EnglishName, txtDocumentExplainedBy1, txtDocumentExplainedBy2, 23)

            Dim strDeclarationSmartID As String = HttpContext.GetGlobalResourceObject("PrintoutText", "VSS_Declaration_SmartID", New System.Globalization.CultureInfo(CultureLanguage.English))

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Select Case _udtEHSTransaction.CategoryCode
                Case CategoryCode.VSS_CHILD, CategoryCode.EVSSO_CHILD
                    txtDeclaration2Value.Text = Replace(strDeclarationSmartID, "%s", "my child¡¦s/ward¡¦s* ")

                Case Else
                    txtDeclaration2Value.Text = Replace(strDeclarationSmartID, "%s", "my / the recipient¡¦s* ")

            End Select
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End Sub

    End Class
End Namespace

