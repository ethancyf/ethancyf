Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm
    Public Class VoucherConsentForm

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

            'Transaction
            If udtCFInfo.SPName <> String.Empty Then
                txtTransactionTo.Text = udtCFInfo.SPName
            Else
                txtTransactionTo.Text = "(Name of the Enrolled Health Care Provider) ＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿"
            End If

            If udtCFInfo.SPName <> String.Empty Then
                Me.sreSPConsent1.Report = New PrintOut.VoucherConsentForm.ClaimConsent1SPName30(udtCFInfo)
            Else
                Me.sreSPConsent1.Report = New PrintOut.VoucherConsentForm.ClaimConsent1SPNameNA(udtCFInfo)
            End If

            Select Case udtCFInfo.ReadSmartID
                Case ConsentFormInformationModel.ReadSmartIDClass.Yes, ConsentFormInformationModel.ReadSmartIDClass.Unknown
                    Me.sreDeclaration.Report = New PrintOut.VoucherConsentForm.SignatureFormFullVersionSmartID(udtCFInfo)
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullEngSmartIC", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.No
                    Me.sreDeclaration.Report = New PrintOut.VoucherConsentForm.SignatureFormFullVersion(udtCFInfo)
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeFullEng", udtCFInfo.FormType)

            End Select

            Me.sreVoucherNotice.Report = New PrintOut.VoucherConsentForm.VoucherNotice(udtCFInfo)

            'Appendix
            Me.txtHCVUInfo.Text = "Health Care Voucher Unit  Department of Health  1/F Tang Chi Ngong Specialist Clinic Block  284, Queen's Road East  Wan Chai, Hong Kong".Replace("  ", Environment.NewLine)
            Me.txtTelNo.Text = String.Format("Telephone No.: {0}", "3582 4102")

            'Footer
            txtPrintDetail.Text = String.Format("Print on {0}", DateTime.Now().ToString("yyyy/MM/dd HH:mm"))

        End Sub

    End Class
End Namespace
