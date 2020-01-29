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
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

'CRE15-003 System-generated Form [Start][Philip Chau]
Imports HCSP.BLL
'CRE15-003 System-generated Form [End][Philip Chau]

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class VoucherConsentForm_CHI_v2

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

            'CRE15-003 System-generated Form [Start][Philip Chau]
            Me.txtTransactionNumber.Text = Me._udtSessionHandler.EHSClaimTempTransactionIDGetFromSession()
            'CRE15-003 System-generated Form [End][Philip Chau]

            ' To
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If udtCFInfo.SPDisplayName <> String.Empty Then
                If udtCFInfo.DisplayPracticeName Then
                    txtTransactionTo.Text = String.Format(HttpContext.GetGlobalResourceObject("Text", "Operator", New System.Globalization.CultureInfo(udtCFInfo.Language)), _
                                                          udtCFInfo.SPDisplayName, udtCFInfo.ProfessionDesc)
                Else
                    txtTransactionTo.Text = udtCFInfo.SPDisplayName
                End If
            Else
                txtTransactionTo.Text = "(已登記醫療服務提供者姓名)＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿"
            End If
            ' CRE19-006 (DHC) [End][Winnie]

            ' Consent 1
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'If udtCFInfo.SPName <> String.Empty Then
            sreSPConsent1.Report = New ClaimConsent1SPName40_v2(udtCFInfo)
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
                Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.HCVS, Common.PrintoutHelper.PrintoutVersion.FullChinese)
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
            Else
                sreDeclaration.Report = New SignatureFormFullVersionSmartID_v2(udtCFInfo)
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Me.txtReportInfoText.Text = (New GeneralFunction).getSystemParameter("VersionCodeFullChiSmartIC", udtCFInfo.FormType)
                Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.HCVS, Common.PrintoutHelper.PrintoutVersion.FullChineseSmartID)
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
            End If

            ' INT12-0009 Fix FGS Voucher address [Start][Tommy Tse]
            ' -----------------------------------------------------------------------------------------

            ' Contact Info
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'txtHCVUInfo.Text = (New GeneralFunction).getSystemParameter("ConsentOfficeContactInfo_CHI", ConsentFormInformationModel.FormTypeClass.HCVS).Replace("  ", Environment.NewLine)

            'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("ConsentFormContactInfo_Voucher_CHI", txtHCVUInfo.Text, String.Empty, ConsentFormInformationModel.FormTypeClass.HCVS)
            'txtHCVUInfo.Text = txtHCVUInfo.Text.Replace("  ", Environment.NewLine)
            Dim udtFormatter As New Formatter
            txtHCVUInfo.Text = udtFormatter.formatLineBreak(txtHCVUInfo.Text)
            'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]

            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            ' INT12-0009 Fix FGS Voucher address [End][Tommy Tse]

            ' Voucher notice
            sreVoucherNotice.Report = New PrintOut.VoucherConsentForm_CHI.VoucherNotice_v2(udtCFInfo)

            ' Date time
            Me.txtPrintDetail.Text = String.Format("列印於 {0}", DateTime.Now().ToString("yyyy年MM月dd日 HH:mm"))

        End Sub

    End Class
End Namespace