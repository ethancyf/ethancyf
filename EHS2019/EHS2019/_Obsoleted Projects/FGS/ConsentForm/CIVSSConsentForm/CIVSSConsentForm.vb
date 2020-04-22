Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.CIVSSConsentForm
    Public Class CIVSSConsentForm

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Initialize helper object
            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            Dim udtGeneralFunction As New GeneralFunction

            ' Vaccination Info
            srVaccinationInfo.Report = New CIVSSVaccinationInfo(udtCFInfo)

            ' Personal Info
            srPersonalInfo.Report = New Common.PersonalInfo(udtCFInfo)

            ' Identity Document
            txtDocType.Text = udtGeneralFunction.GetSystemResource("PrintoutText", udtCFInfo.DocType, udtCFInfo.Language)
            srIdentityDocument.Report = New Common.DocType.IdentityDocument(udtCFInfo)

            ' Declaration

            Select Case udtCFInfo.ReadSmartID
                Case ConsentFormInformationModel.ReadSmartIDClass.Yes
                    srDeclaration.Report = New CIVSSDeclarationSmartID()
                    txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullEngSmartIC", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.No
                    srDeclaration.Report = New CIVSSDeclaration()
                    txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullEng", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.Unknown
                    srDeclaration.Report = New CIVSSDeclarationSmartIDUnknown()
                    txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullEngSmartIC", udtCFInfo.FormType)

            End Select

            ' Signature
            srSignature.Report = New CIVSSSignature(udtCFInfo.SignDate)

            ' Statement Of Purpose
            srStatementOfPurpose.Report = New CIVSSStatementOfPurpose()

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
