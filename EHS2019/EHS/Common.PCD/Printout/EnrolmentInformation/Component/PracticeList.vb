Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.Component.ThirdParty

Namespace Printout.EnrolmentInformation.Component

    Public Class PracticeList

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal udtProvider As ServiceProviderModel, ByVal strLanguage As String)
            Me.New()

            DataBind(udtProvider, strLanguage)
        End Sub

        Private Sub DataBind(ByVal udtProvider As ServiceProviderModel, ByVal strLanguage As String)
            Dim startTop As Single = 0.0!

            For Each udtPractice As PracticeModel In udtProvider.PracticeList.Values

                Dim udtThirdPartyList As ThirdPartyAdditionalFieldEnrolmentCollection = udtProvider.ThirdPartyAdditionalFieldEnrolmentList
                If Not udtThirdPartyList Is Nothing AndAlso udtThirdPartyList.GetListBySysCode(ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD).Count > 0 Then
                    Dim udtThirdPartyModel As ThirdPartyAdditionalFieldEnrolmentModel = udtThirdPartyList.GetByValueCode(ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD, _
                                                                                                     udtPractice.DisplaySeq, _
                                                                                                     EnumConstant.EnumAdditionalFieldID.TYPE_OF_PRACTICE.ToString())
                    If Not udtThirdPartyModel Is Nothing Then

                        Dim subReport As New SubReport

                        subReport.Report = New Practice(udtProvider, udtPractice, strLanguage)
                        subReport.Top = startTop
                        subReport.Height = 0.2!
                        subReport.Width = PrintWidth

                        Detail1.Controls.Add(subReport)

                        startTop += subReport.Height + 0.2!

                    End If

                End If

            Next

            Detail1.Height = startTop

        End Sub

    End Class

End Namespace
