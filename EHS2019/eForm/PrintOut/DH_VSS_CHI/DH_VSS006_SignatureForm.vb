Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Imports Common.Component.ServiceProvider
Imports Common.ComFunction
Imports Common.Format

Namespace PrintOut.DH_VSS_CHI

    Public Class DH_VSS006_SignatureForm
        Private _udtSP As ServiceProviderModel
        Private _udtReportFunction As Common.ComFunction.ReportFunction
        Private _udtFormatter As Formatter

        Public Sub New(ByVal udtSP As ServiceProviderModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me._udtReportFunction = New ReportFunction()
            Me._udtFormatter = New Formatter
            Me._udtSP = udtSP
        End Sub

        Private Sub dtlDH_VSS006_SignatureForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

            Me._udtReportFunction.formatUnderLineTextBox(Me._udtSP.EnglishName, Me.txtByProviderLettersName)
            Me._udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatHKID(Me._udtSP.HKID, False), Me.txtByProviderHKICNo)
            Me._udtReportFunction.formatUnderLineTextBox(Me._udtSP.Phone, Me.txtByProviderTelNo)

        End Sub
    End Class

End Namespace