Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm
    Public Class SignatureFormFullVersionSmartID_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)


            If udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.EC Then
                Me.txtRecipientHKIDText.Text = "Serial No. of the Certificate of Exemption:"
                Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, txtRecipientHKID)
            Else
                Me.txtRecipientHKIDText.Text = "Hong Kong Identity Card No.:"
                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtRecipientHKID)
            End If

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me.sreDeclaration1.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration1FullVersionSPName40_v2(udtCFInfo.SPDisplayName)
            Me.sreDeclaration2.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration2FullVersionSPName40_v2(udtCFInfo.SPDisplayName, udtCFInfo.DocType)
            ' CRE19-006 (DHC) [End][Winnie]

            If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Yes Then
                Me.sreDeclaration3.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration3FullVersion_v2(udtCFInfo)
            ElseIf udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Unknown Then
                Me.sreDeclaration3.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration3FullVersionUnknownSmartID_v2(udtCFInfo)
            End If

            'Recipient
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, Me.txtRecipientName)
            Formatter.FormatUnderLineTextBox(udtCFInfo.SignDate, Me.txtRecipientDate)

        End Sub

    End Class
End Namespace
