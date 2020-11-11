Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.SSSCMCConsentForm_CN
    Public Class SignatureFormFullVersionSmartID

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)


            Me.sreDecaration1.Report = New ClaimConsentDecaraDeclaration1FullVersion(udtCFInfo.MOName)
            Me.sreDecaration2.Report = New ClaimConsentDecaraDeclaration2FullVersion(udtCFInfo.MOName, udtCFInfo.DocType)
            Me.sreDecaration3.Report = New ClaimConsentDecaraDeclaration3FullVersion(udtCFInfo.MOName)
            Me.sreDecaration4.Report = New ClaimConsentDecaraDeclaration4FullVersion(udtCFInfo)

            'Confirmat consent
            Select Case udtCFInfo.DocType
                Case ConsentFormInformationModel.DocTypeClass.EC
                    Me.txtRecipientHKIDText.Text = "豁免登记证明书编号："
                    Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, txtRecipientHKID)

                Case ConsentFormInformationModel.DocTypeClass.HKBC
                    Me.txtRecipientHKIDText.Text = "香港出生证明书登记编号："
                    Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtRecipientHKID)

                Case Else
                    Me.txtRecipientHKIDText.Text = "香港身份证号码："
                    Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtRecipientHKID)
            End Select


            'Recipient
            If Not udtCFInfo.RecipientCName = String.Empty Then
                Me.txtRecipientNameText.Text = "服务使用者姓名 （中文）："
                Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientCName, txtRecipientName)
            Else
                Me.txtRecipientNameText.Text = "服务使用者姓名："
                Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, txtRecipientName)
            End If


            Formatter.FormatUnderLineTextBox(udtCFInfo.SignDate, txtRecipientDate)

        End Sub

    End Class
End Namespace