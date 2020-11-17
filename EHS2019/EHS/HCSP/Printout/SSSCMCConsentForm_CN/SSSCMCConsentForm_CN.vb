Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
'-----------------------------------------------------------------------------------------
Imports Common.Format
'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]

' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports Common.ComFunction
Imports HCSP.PrintOut.ConsentFormInformation
Imports HCSP.BLL
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.SSSCMCConsentForm_CN
    Public Class SSSCMCConsentForm_CN

        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Private _udtGeneralFunction As New GeneralFunction
        Private _udtPrintoutHelper As New HCSP.PrintOut.Common.PrintoutHelper
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        'CRE15-003 System-generated Form [Start][Philip Chau]
        Private _udtSessionHandler As New SessionHandler
        'CRE15-003 System-generated Form [End][Philip Chau]

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
            If udtCFInfo.MOName <> String.Empty Then
                txtTransactionTo.Text = udtCFInfo.MOName
            Else
                txtTransactionTo.Text = "(已登记医疗服务提供者姓名)＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿"
            End If

            'CRE15-003 System-generated Form [Start][Philip Chau]
            Me.txtTransactionNumber.Text = Me._udtSessionHandler.EHSClaimTempTransactionIDGetFromSession()
            'CRE15-003 System-generated Form [End][Philip Chau]

            ' Consent 1
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'If udtCFInfo.SPName <> String.Empty Then
            sreSPConsent1.Report = New ClaimConsent1(udtCFInfo)
            'Else
            '   sreSPConsent1.Report = New ClaimConsent1SPNameNA(udtCFInfo)
            'End If
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            ' Signature
            If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.No Then
                sreDeclaration.Report = New SignatureFormFullVersion(udtCFInfo)
                Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.SSSCMC, Common.PrintoutHelper.PrintoutVersion.FullSimpChinese)

            Else
                sreDeclaration.Report = New SignatureFormFullVersionSmartID(udtCFInfo)
                Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.SSSCMC, Common.PrintoutHelper.PrintoutVersion.FullSimpChineseSmartID)

            End If

            ' Voucher notice
            sreVoucherNotice.Report = New PrintOut.SSSCMCConsentForm_CN.VoucherNotice(udtCFInfo)

            ' Date time
            Me.txtPrintDetail.Text = String.Format("列印于 {0}", DateTime.Now().ToString("yyyy年MM月dd日 HH:mm"))

        End Sub

    End Class
End Namespace