Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.ComObject
Imports Common.Component
Imports Common.Component.Scheme
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimRules

Namespace PrintOut.HSIVSSConsentForm_CHI

    Public Class HSIVSSEligibilityRole_CHI

        Private _udtSchemeCategoryDescriptionSystemResource As SystemResource

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
        End Sub

        Public Sub New(ByVal udtSchemeCategoryDescriptionSystemResource As SystemResource)
            Me.New()

            _udtSchemeCategoryDescriptionSystemResource = udtSchemeCategoryDescriptionSystemResource

            LoadReport()
        End Sub

        Private Sub LoadReport()

            If _udtSchemeCategoryDescriptionSystemResource Is Nothing Then
                Throw New ArgumentNullException("SystemResource")
            Else
                txtDescription.Text = HttpContext.GetGlobalResourceObject(_udtSchemeCategoryDescriptionSystemResource.ObjectType, _udtSchemeCategoryDescriptionSystemResource.ObjectName, New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
            End If

        End Sub

    End Class

End Namespace