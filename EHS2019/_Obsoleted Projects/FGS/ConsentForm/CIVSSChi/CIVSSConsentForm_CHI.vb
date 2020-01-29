Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSConsentForm_CHI

#Region "Constructor"

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Initialize helper object
            LoadReport(udtCFInfo)

        End Sub

#End Region

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            Dim udtGeneralFunction As New GeneralFunction

            ' Vaccination Info
            srVaccinationInfo.Report = New CIVSSVaccinationInfo_CHI(udtCFInfo)

            ' Personal Info
            srPersonalInfo.Report = New Common.PersonalInfo_CHI(udtCFInfo)

            ' Identity Document
            txtDocType.Text = udtGeneralFunction.GetSystemResource("PrintoutText", udtCFInfo.DocType, udtCFInfo.Language)
            srIdentityDocument.Report = New Common.DocType.IdentityDocument_CHI(udtCFInfo)

            ' Declaration
            Select Case udtCFInfo.ReadSmartID
                Case ConsentFormInformationModel.ReadSmartIDClass.No
                    srDeclaration.Report = New CIVSSDeclaration_CHI()
                    txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullChi", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.Yes
                    srDeclaration.Report = New CIVSSDeclarationSmartID_CHI()
                    txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullChiSmartIC", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.Unknown
                    srDeclaration.Report = New CIVSSDeclarationSmartIDUnknown_CHI()
                    txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullChiSmartIC", udtCFInfo.FormType)

            End Select

            ' Signature
            srSignature.Report = New CIVSSSignature_CHI(udtCFInfo.SignDate)

            ' Statement Of Purpose
            srStatementOfPurpose.Report = New CIVSSStatementOfPurpose_CHI()

            ' Footer
            txtPrintDetail.Text = String.Format("列印於 {0}", DateTime.Now().ToString("yyyy年MM月dd日 HH:mm"))
        End Sub

#Region "Report Event"
        Private Sub CIVSSConsentForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.Document.Printer.PrinterName = ""
        End Sub
#End Region

    End Class
End Namespace
