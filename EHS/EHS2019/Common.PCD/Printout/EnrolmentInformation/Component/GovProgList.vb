Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.ServiceProvider
Imports Common.Component.SchemeInformation
Imports Common.Component.Scheme
Imports Common.ComFunction.GeneralFunction
Imports Common.Component
Imports System.Web
Namespace Printout.EnrolmentInformation.Component

    Public Class GovProgList

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal udtProvider As ServiceProviderModel, ByVal udtPractice As PracticeModel, ByVal strLanguage As String)
            Me.New()

            DataBind(udtProvider, udtPractice, strLanguage)
        End Sub

        Private Sub DataBind(ByVal udtProvider As ServiceProviderModel, ByVal udtPractice As PracticeModel, ByVal strLanguage As String)
            Dim startTop As Single = 0.0!

            If Not udtPractice.PracticeSchemeInfoList Is Nothing Then
                For Each udtScheme As SchemeInformationModel In udtProvider.SchemeInfoList.Values

                    ' Check this practice contains this scheme
                    Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = udtPractice.PracticeSchemeInfoList.FilterByPracticeScheme(udtPractice.DisplaySeq, udtScheme.SchemeCode)
                    If Not udtPracticeSchemeInfoList Is Nothing Then

                        Dim subReport As New SubReport

                        subReport.Report = New GovProg(udtPracticeSchemeInfoList, strLanguage)
                        subReport.Top = startTop
                        subReport.Height = 0.2!
                        subReport.Width = PrintWidth

                        Detail1.Controls.Add(subReport)

                        startTop += subReport.Height
                    End If
                Next

                Detail1.Height = startTop
            End If

        End Sub

    End Class

End Namespace
