Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Imports Common.Component.EHSAccount
Imports Common.Component.ServiceProvider

Namespace PrintOut.VoucherAccountChnageForm

    Public Class SmartIDChangePersonalPaticularForm

        Public Sub New(ByVal udtDiffPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal udtSP As ServiceProviderModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me.SubReport2.Report = New PrintOut.VoucherAccountChangeForm.DiffPersonalInformation(udtDiffPersonalInfo)

        End Sub


        Private Sub SmartIDChangePersonalPaticularForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        End Sub

        Private Sub Detail1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Detail1.Format

        End Sub

    End Class

End Namespace