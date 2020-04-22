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

Namespace PrintOut.VoucherConsentForm_CN
    Public Class VoucherConsentForm_CN_v2

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
            sreSPConsent1.Report = New ClaimConsent1_v2(udtCFInfo)
            'Else
            '   sreSPConsent1.Report = New ClaimConsent1SPNameNA_v2(udtCFInfo)
            'End If
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            ' Signature
            If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.No Then
                sreDeclaration.Report = New SignatureFormFullVersion_v2(udtCFInfo)
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Me.txtReportInfoText.Text = (New GeneralFunction).getSystemParameter("VersionCodeFullChi", udtCFInfo.FormType)
                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                'Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.HCVS, Common.PrintoutHelper.PrintoutVersion.FullChinese)
                Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.HCVSC, Common.PrintoutHelper.PrintoutVersion.FullSimpChinese)
                'CRE13-019-02 Extend HCVS to China [End][Winnie]
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
            Else
                sreDeclaration.Report = New SignatureFormFullVersionSmartID_v2(udtCFInfo)
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Me.txtReportInfoText.Text = (New GeneralFunction).getSystemParameter("VersionCodeFullChiSmartIC", udtCFInfo.FormType)
                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                'Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.HCVS, Common.PrintoutHelper.PrintoutVersion.FullChineseSmartID)
                Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.HCVSC, Common.PrintoutHelper.PrintoutVersion.FullSimpChineseSmartID)
                'CRE13-019-02 Extend HCVS to China [End][Winnie]
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
            End If

            ' INT12-0009 Fix FGS Voucher address [Start][Tommy Tse]
            ' -----------------------------------------------------------------------------------------

            ' Contact Info
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'txtHCVUInfo.Text = (New GeneralFunction).getSystemParameter("ConsentOfficeContactInfo_CHI", ConsentFormInformationModel.FormTypeClass.HCVS).Replace("  ", Environment.NewLine)

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            _udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("ConsentFormContactInfo_Voucher_CN", txtHCVUInfo.Text, String.Empty, ConsentFormInformationModel.FormTypeClass.HCVSC)
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'txtHCVUInfo.Text = txtHCVUInfo.Text.Replace("  ", Environment.NewLine)
            Dim udtFormatter As New Formatter
            txtHCVUInfo.Text = udtFormatter.formatLineBreak(txtHCVUInfo.Text)
            'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]

            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            ' INT12-0009 Fix FGS Voucher address [End][Tommy Tse]

            ' Voucher notice
            sreVoucherNotice.Report = New PrintOut.VoucherConsentForm_CN.VoucherNotice_v2(udtCFInfo)

            ' Date time
            Me.txtPrintDetail.Text = String.Format("列印于 {0}", DateTime.Now().ToString("yyyy年MM月dd日 HH:mm"))

        End Sub

    End Class
End Namespace