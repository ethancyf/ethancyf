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

Namespace PrintOut.VoucherConsentForm
    Public Class VoucherConsentForm_v2

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

            'Transaction
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
                txtTransactionTo.Text = "(Name of the Enrolled Health Care Provider) ＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿"
            End If
            ' CRE19-006 (DHC) [End][Winnie]

            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'If udtCFInfo.SPName <> String.Empty Then
            Me.sreSPConsent1.Report = New PrintOut.VoucherConsentForm.ClaimConsent1SPName40_v2(udtCFInfo)
            'Else
            '   Me.sreSPConsent1.Report = New PrintOut.VoucherConsentForm.ClaimConsent1SPNameNA_v2(udtCFInfo)
            'End If
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            Select Case udtCFInfo.ReadSmartID
                Case ConsentFormInformationModel.ReadSmartIDClass.Yes, ConsentFormInformationModel.ReadSmartIDClass.Unknown
                    Me.sreDeclaration.Report = New PrintOut.VoucherConsentForm.SignatureFormFullVersionSmartID_v2(udtCFInfo)
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'Me.txtPageName.Text = (New GeneralFunction).getSystemParameter("VersionCodeFullEngSmartIC", udtCFInfo.FormType)
                    Me.txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.HCVS, Common.PrintoutHelper.PrintoutVersion.FullEnglishSmartID)
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

                Case ConsentFormInformationModel.ReadSmartIDClass.No
                    Me.sreDeclaration.Report = New PrintOut.VoucherConsentForm.SignatureFormFullVersion_v2(udtCFInfo)
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'Me.txtPageName.Text = (New GeneralFunction).getSystemParameter("VersionCodeFullEng", udtCFInfo.FormType)
                    Me.txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.HCVS, Common.PrintoutHelper.PrintoutVersion.FullEnglish)
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            End Select

            Me.sreVoucherNotice.Report = New PrintOut.VoucherConsentForm.VoucherNotice_v2(udtCFInfo)

            ' INT12-0009 Fix FGS Voucher address [Start][Tommy Tse]
            ' -----------------------------------------------------------------------------------------

            ' Contact Info
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'txtHCVUInfo.Text = (New GeneralFunction).getSystemParameter("ConsentOfficeContactInfo", ConsentFormInformationModel.FormTypeClass.HCVS).Replace("  ", Environment.NewLine)

            'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("ConsentFormContactInfo_Voucher_EN", txtHCVUInfo.Text, String.Empty, ConsentFormInformationModel.FormTypeClass.HCVS)
            'txtHCVUInfo.Text = txtHCVUInfo.Text.Replace("  ", Environment.NewLine)
            Dim udtFormatter As New Formatter
            txtHCVUInfo.Text = udtFormatter.formatLineBreak(txtHCVUInfo.Text)
            'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]

            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            ' INT12-0009 Fix FGS Voucher address [End][Tommy Tse]

            'Footer
            txtPrintDetail.Text = String.Format("Print on {0}", DateTime.Now().ToString("yyyy/MM/dd HH:mm"))

        End Sub

    End Class
End Namespace
