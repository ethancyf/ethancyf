Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class ClaimConsentDecaraDeclaration2FullVersionSPNameNA_v2

        Public Sub New(ByVal strDocCode As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strDocCode)
        End Sub

        Private Sub LoadReport(ByVal strDocCode As String)
            Dim strIDCardInfo As String

            If strDocCode = ConsentFormInformationModel.DocTypeClass.EC Then
                strIDCardInfo = "中英文姓名、性別、出生日期、豁免登記證明書編號、其檔案編號、豁免登記證明書上所展示的（簽發）日期及香港身份證號碼。"
            Else
                strIDCardInfo = "香港身份證號碼、中英文姓名、性別、出生日期和香港身份證簽發日期。"
            End If

            Me.txtConsent2c.Text = String.Format("{0}", strIDCardInfo)
            '和政府提供本人的個人資料

        End Sub

    End Class
End Namespace