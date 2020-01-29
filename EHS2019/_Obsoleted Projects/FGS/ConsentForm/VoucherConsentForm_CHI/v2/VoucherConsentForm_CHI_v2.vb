Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class VoucherConsentForm_CHI_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            FillData(udtCFInfo)

        End Sub

        Private Sub FillData(ByVal udtCFInfo As ConsentFormInformationModel)
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

            ' Consent 1
            If udtCFInfo.SPName <> String.Empty Then
                sreSPConsent1.Report = New ClaimConsent1SPName40_v2(udtCFInfo)
            Else
                sreSPConsent1.Report = New ClaimConsent1SPNameNA_v2(udtCFInfo)
            End If

            ' Signature
            If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.No Then
                sreDeclaration.Report = New SignatureFormFullVersion_v2(udtCFInfo)
                Me.txtReportInfoText.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullChi", udtCFInfo.FormType)

            Else
                sreDeclaration.Report = New SignatureFormFullVersionSmartID_v2(udtCFInfo)
                Me.txtReportInfoText.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullChiSmartIC", udtCFInfo.FormType)

            End If

            ' INT12-0009 Fix FGS Voucher address [Start][Tommy Tse]
            ' -----------------------------------------------------------------------------------------

            ' Contact Info
            txtHCVUInfo.Text = (New GeneralFunction).GetSystemParameter("ConsentOfficeContactInfo_CHI", ConsentFormInformationModel.FormTypeClass.HCVS).Replace("  ", Environment.NewLine)

            ' INT12-0009 Fix FGS Voucher address [End][Tommy Tse]

            ' Voucher notice
            sreVoucherNotice.Report = New PrintOut.VoucherConsentForm_CHI.VoucherNotice_v2(udtCFInfo)

            ' Date time
            Me.txtPrintDetail.Text = String.Format("列印於 {0}", DateTime.Now().ToString("yyyy年MM月dd日 HH:mm"))

        End Sub

    End Class
End Namespace