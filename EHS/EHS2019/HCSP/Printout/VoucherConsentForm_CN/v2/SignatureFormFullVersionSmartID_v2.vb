Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm_CN
    Public Class SignatureFormFullVersionSmartID_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)


            Me.sreDecaration1.Report = New ClaimConsentDecaraDeclaration1FullVersion_v2(udtCFInfo.MOName)
            Me.sreDecaration2.Report = New ClaimConsentDecaraDeclaration2FullVersion_v2(udtCFInfo.MOName, udtCFInfo.DocType)
            Me.sreDecaration3.Report = New ClaimConsentDecaraDeclaration3FullVersion_v2(udtCFInfo.MOName)
            Me.sreDecaration4.Report = New ClaimConsentDecaraDeclaration4FullVersion_v2(udtCFInfo)

            If udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.EC Then
                'Confirmat consent
                Me.txtRecipientHKIDText.Text = "豁免登记证明书编号："
                Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, txtRecipientHKID)

            Else
                'Confirmat consent
                Me.txtRecipientHKIDText.Text = "香港身份证号码："
                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtRecipientHKID)

            End If

            'Recipient
            If Not udtCFInfo.RecipientCName = String.Empty Then
                Me.txtRecipientNameText.Text = "医疗券使用者姓名 （中文）："
                Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientCName, txtRecipientName)
            Else
                Me.txtRecipientNameText.Text = "医疗券使用者姓名："
                Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, txtRecipientName)
            End If


            Formatter.FormatUnderLineTextBox(udtCFInfo.SignDate, txtRecipientDate)

        End Sub

    End Class
End Namespace