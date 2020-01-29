Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.EVSSConsentForm
    Public Class EVSSConsentCondensedForm

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Vaccination Info
            srVaccinationInfo.Report = New EVSSVaccinationInfo(udtCFInfo)

            ' Document Explained By
            Select Case udtCFInfo.ReadSmartID
                Case ConsentFormInformationModel.ReadSmartIDClass.Yes
                    srDeclaration.Report = New EVSSDeclarationCondensedSmartID(udtCFInfo.SPName)
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeCondEngSmartIC", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.No
                    srDeclaration.Report = New EVSSDeclarationCondensed(udtCFInfo.SPName)
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeCondEng", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.Unknown
                    srDeclaration.Report = New EVSSDeclarationCondensedSmartIDUnknown(udtCFInfo.SPName)
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeCondEngSmartIC", udtCFInfo.FormType)

            End Select
          
            ' Signature
            srSignature.Report = New EVSSSignature(udtCFInfo)

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
