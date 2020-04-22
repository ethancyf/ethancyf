Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.EVSSConsentForm_CHI
    Public Class EVSSConsentForm_CHI

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)
        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Vaccination Info
            srVaccinationInfo.Report = New EVSSVaccinationInfo_CHI(udtCFInfo)

            ' Signature
            srSignature.Report = New EVSSSignature_CHI(udtCFInfo)

            ' Declaration
            Select Case udtCFInfo.ReadSmartID
                Case ConsentFormInformationModel.ReadSmartIDClass.No
                    srDeclaration.Report = New EVSSDeclaration_CHI()
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullChi", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.Yes
                    srDeclaration.Report = New EVSSDeclarationSmartID_CHI()
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullChiSmartIC", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.Unknown
                    srDeclaration.Report = New EVSSDeclarationSmartIDUnknown_CHI()
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullChiSmartIC", udtCFInfo.FormType)

            End Select

            ' Statement Of Purpose
            srStatementOfPurpose.Report = New EVSSStatementOfPurpose_CHI()

            'Footer
            txtPrintDetail.Text = String.Format("列印於 {0}", DateTime.Now().ToString("yyyy年MM月dd日 HH:mm"))
        End Sub

#Region "Report Event"
        Private Sub CIVSSConsentForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.Document.Printer.PrinterName = ""
        End Sub
#End Region

    End Class
End Namespace
