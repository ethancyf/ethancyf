Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.EVSSConsentForm
    Public Class EVSSConsentForm

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            ' Vaccination Info
            srVaccinationInfo.Report = New EVSSVaccinationInfo(udtCFInfo)

            ' Signature
            srSignature.Report = New EVSSSignature(udtCFInfo)

            ' Declaration
            Select Case udtCFInfo.ReadSmartID
                Case ConsentFormInformationModel.ReadSmartIDClass.Yes
                    srDeclaration.Report = New EVSSDeclarationSmartID()
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullEngSmartIC", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.No
                    srDeclaration.Report = New EVSSDeclaration()
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullEng", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.Unknown
                    srDeclaration.Report = New EVSSDeclarationSmartIDUnknown()
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullEngSmartIC", udtCFInfo.FormType)

            End Select

            ' Statement Of Purpose
            srStatementOfPurpose.Report = New EVSSStatementOfPurpose()

            ' Footer
            txtPrintDetail.Text = String.Format("Print on {0}", DateTime.Now().ToString("yyyy/MM/dd HH:mm"))

        End Sub

#Region "Report Event"

        Private Sub CIVSSConsentForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.Document.Printer.PrinterName = ""
        End Sub

#End Region

    End Class
End Namespace
