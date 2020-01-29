Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document


Namespace PrintOut.VoucherConsentForm_CHI
    Public Class VoucherConsentCondensedForm_CHI

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------
            ' Preprint
            If udtCFInfo.Platform = ConsentFormInformationModel.EnumPlatform.HCSP Then
                txtPreprint.Visible = False
            End If
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            ' To
            If udtCFInfo.SPName <> String.Empty Then
                txtTransactionTo.Text = udtCFInfo.SPName
            Else
                txtTransactionTo.Text = "(已登記醫療服務提供者姓名)＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿"
            End If

            ' Signature
            If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.No Then
                Me.srSignatureForm.Report = New PrintOut.VoucherConsentForm_CHI.SignatureForm(udtCFInfo)
                Me.txtReportInfoText.Text = (New GeneralFunction).GetSystemParameter("VersionCodeCondChi", udtCFInfo.FormType)

            Else
                Me.srSignatureForm.Report = New PrintOut.VoucherConsentForm_CHI.SignatureFormSmartID(udtCFInfo)
                Me.txtReportInfoText.Text = (New GeneralFunction).GetSystemParameter("VersionCodeCondChiSmartIC", udtCFInfo.FormType)

            End If

            ' Voucher Notice
            Me.sreVoucherNotice.Report = New PrintOut.VoucherConsentForm_CHI.VoucherNotice(udtCFInfo)

            ' Footer
            Me.txtPrintDetail.Text = String.Format("列印於 {0}", DateTime.Now().ToString("yyyy年MM月dd日 HH:mm"))

        End Sub

    End Class
End Namespace