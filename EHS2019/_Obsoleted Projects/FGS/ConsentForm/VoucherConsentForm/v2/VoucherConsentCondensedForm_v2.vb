Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm
    Public Class VoucherConsentCondensedForm_v2

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

            Select Case udtCFInfo.ReadSmartID
                Case ConsentFormInformationModel.ReadSmartIDClass.Yes, ConsentFormInformationModel.ReadSmartIDClass.Unknown
                    Me.srSignatureForm.Report = New SignatureFormSmartID_v2(udtCFInfo)
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeCondEngSmartIC", udtCFInfo.FormType)

                Case ConsentFormInformationModel.ReadSmartIDClass.No
                    Me.srSignatureForm.Report = New SignatureForm_v2(udtCFInfo)
                    Me.txtPageName.Text = (New GeneralFunction).GetSystemParameter("VersionCodeCondEng", udtCFInfo.FormType)

            End Select

            Me.sreVoucherNotice.Report = New PrintOut.VoucherConsentForm.VoucherNotice_v2(udtCFInfo)

            'Footer
            txtPrintDetail.Text = String.Format("Print on {0}", DateTime.Now().ToString("yyyy/MM/dd HH:mm"))

        End Sub

    End Class
End Namespace
