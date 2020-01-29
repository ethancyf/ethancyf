Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.ComObject
Imports Common.Component
Imports Common.Component.Scheme
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.ClaimRules

Namespace PrintOut.HSIVSSConsentForm

    Public Class HSIVSSEligibilityRole

        Private _udtSystemResource As SystemResource

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
        End Sub

        Public Sub New(ByVal udtSystemResource As SystemResource)
            Me.New()

            _udtSystemResource = udtSystemResource

            LoadReport()
        End Sub

        Private Sub LoadReport()

            If _udtSystemResource Is Nothing Then
                Throw New ArgumentNullException("SystemResource")
            Else
                txtDescription.Text = HttpContext.GetGlobalResourceObject(_udtSystemResource.ObjectType, _udtSystemResource.ObjectName, New System.Globalization.CultureInfo(CultureLanguage.English))
            End If

        End Sub

    End Class

End Namespace