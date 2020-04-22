Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSConsentCondensedForm_CHI

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            Dim udtGeneralFunction As New GeneralFunction

            ' Vaccination Info
            srVaccinationInfo.Report = New CIVSSVaccinationInfo_CHI(udtCFInfo)

            ' Personal Info
            srPersonalInfo.Report = New Common.PersonalInfo_CHI(udtCFInfo)

            ' Identity Document
            txtDocType.Text = udtGeneralFunction.GetSystemResource("PrintoutText", udtCFInfo.DocType, udtCFInfo.Language)
            srIdentityDocument.Report = New Common.DocType.IdentityDocument_CHI(udtCFInfo)

            ' Document Explained By
            Select Case udtCFInfo.ReadSmartID
                Case ConsentFormInformationModel.ReadSmartIDClass.No
                    srDeclaration.Report = New CIVSSDeclarationCondensed_CHI(udtCFInfo)
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeCondChi", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.Yes
                    srDeclaration.Report = New CIVSSDeclarationCondensedSmartID_CHI(udtCFInfo)
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeCondChiSmartIC", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.Unknown
                    srDeclaration.Report = New CIVSSDeclarationCondensedSmartIDUnknown_CHI(udtCFInfo)
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeCondChiSmartIC", udtCFInfo.FormType)

            End Select

            ' Signature
            srSignature.Report = New CIVSSSignature_CHI(udtCFInfo.SignDate)

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
