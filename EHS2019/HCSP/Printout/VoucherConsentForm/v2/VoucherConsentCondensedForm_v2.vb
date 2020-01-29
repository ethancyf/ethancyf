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

Namespace PrintOut.VoucherConsentForm
    Public Class VoucherConsentCondensedForm_v2

        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
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
                txtTransactionTo.Text = udtCFInfo.SPDisplayName
            Else
                txtTransactionTo.Text = "(Name of the Enrolled Health Care Provider) ＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿"
            End If
            ' CRE19-006 (DHC) [End][Winnie]

            Select Case udtCFInfo.ReadSmartID
                Case ConsentFormInformationModel.ReadSmartIDClass.Yes, ConsentFormInformationModel.ReadSmartIDClass.Unknown
                    Me.srSignatureForm.Report = New SignatureFormSmartID_v2(udtCFInfo)
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeCondEngSmartIC", udtCFInfo.FormType)
                    Me.txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.HCVS, Common.PrintoutHelper.PrintoutVersion.CondensedEnglishSmartID)
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

                Case ConsentFormInformationModel.ReadSmartIDClass.No
                    Me.srSignatureForm.Report = New SignatureForm_v2(udtCFInfo)
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeCondEng", udtCFInfo.FormType)
                    Me.txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(ConsentFormInformationModel.FormTypeClass.HCVS, Common.PrintoutHelper.PrintoutVersion.CondensedEnglish)
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            End Select

            Me.sreVoucherNotice.Report = New PrintOut.VoucherConsentForm.VoucherNotice_v2(udtCFInfo)

            'Footer
            txtPrintDetail.Text = String.Format("Print on {0}", DateTime.Now().ToString("yyyy/MM/dd HH:mm"))

        End Sub

    End Class
End Namespace
