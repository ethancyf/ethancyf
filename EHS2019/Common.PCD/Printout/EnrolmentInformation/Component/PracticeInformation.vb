Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.ServiceProvider

Namespace Printout.EnrolmentInformation.Component

    Public Class PracticeInformation

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal udtProvider As ServiceProviderModel, ByVal strLanguage As String)
            Me.New()

            DataBind(udtProvider, strLanguage)
        End Sub

        Private Sub DataBind(ByVal udtProvider As ServiceProviderModel, ByVal strLanguage As String)
            'If IsNothing(udtPracticeList) OrElse udtPracticeList.Count = 0 Then
            '    Detail1.Controls.Clear()
            '    Detail1.Height = 0.0!

            '    Return
            'End If

            Dim udtPrintoutHelper As New PrintoutHelper(strLanguage)

            udtPrintoutHelper.RenderLabel(lblPracticeInformationTitle, "PCDPracticeInfo", intFontSize:=Core.intHeaderFontSize, blnBold:=True)

            srptPracticeList.Report = New PracticeList(udtProvider, strLanguage)

        End Sub

    End Class

End Namespace
