Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports Common.ComFunction
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

'CRE15-003 System-generated Form [Start][Philip Chau]
Imports HCSP.BLL
'CRE15-003 System-generated Form [End][Philip Chau]

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class VoucherConsentCondensedForm_CHI_v2

        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Private _udtPrintoutHelper As New HCSP.PrintOut.Common.PrintoutHelper
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        'CRE15-003 System-generated Form [Start][Philip Chau]
        Private _udtSessionHandler As New SessionHandler
        'CRE15-003 System-generated Form [End][Philip Chau]

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)
            'This call is required by the Windows Form Designer.
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

            'CRE15-003 System-generated Form [Start][Philip Chau]
            Me.txtTransactionNumber.Text = Me._udtSessionHandler.EHSClaimTempTransactionIDGetFromSession()
            'CRE15-003 System-generated Form [End][Philip Chau]

            ' To
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If udtCFInfo.SPDisplayName <> String.Empty Then
                txtTransactionTo.Text = udtCFInfo.SPDisplayName
            Else
                txtTransactionTo.Text = "(已登記醫療服務提供者姓名)＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿"
            End If
            ' CRE19-006 (DHC) [End][Winnie]

            ' Signature
            If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.No Then
                Me.srSignatureForm.Report = New PrintOut.VoucherConsentForm_CHI.SignatureForm_v2(udtCFInfo)
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Me.txtReportInfoText.Text = (New GeneralFunction).getSystemParameter("VersionCodeCondChi", udtCFInfo.FormType)
                Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.HCVS, Common.PrintoutHelper.PrintoutVersion.CondensedChinese)
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
            Else
                Me.srSignatureForm.Report = New PrintOut.VoucherConsentForm_CHI.SignatureFormSmartID_v2(udtCFInfo)
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Me.txtReportInfoText.Text = (New GeneralFunction).getSystemParameter("VersionCodeCondChiSmartIC", udtCFInfo.FormType)
                Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.HCVS, Common.PrintoutHelper.PrintoutVersion.CondensedChineseSmartID)
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
            End If

            ' Voucher Notice
            Me.sreVoucherNotice.Report = New PrintOut.VoucherConsentForm_CHI.VoucherNotice_v2(udtCFInfo)

            ' Footer
            Me.txtPrintDetail.Text = String.Format("列印於 {0}", DateTime.Now().ToString("yyyy年MM月dd日 HH:mm"))

        End Sub

    End Class
End Namespace