Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.ServiceProvider

Namespace Printout.EnrolmentInformation.Component

    Public Class TypeOfCMP

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal blnAllowSelectSubProfession As Boolean, ByVal udtProvider As ServiceProviderModel, ByVal strLanguage As String)
            Me.New()
            DataBind(blnAllowSelectSubProfession, udtProvider, strLanguage)
        End Sub

        Private Sub DataBind(ByVal blnAllowSelectSubProfession As Boolean, ByVal udtProvider As ServiceProviderModel, ByVal strLanguage As String)
            If blnAllowSelectSubProfession Then
                Dim udtPrintoutHelper As New PrintoutHelper(strLanguage)

                ' Type of Chinese Medicine Practitioner
                udtPrintoutHelper.RenderResource(txtTypeOfCMPText, "TypeOfCMP", blnColon:=True)
                udtPrintoutHelper.RenderResource(txtTypeOfCMP, "RCMP")

            Else
                Detail1.Controls.Clear()
                Detail1.Height = 0.0!

            End If

        End Sub

    End Class

End Namespace
