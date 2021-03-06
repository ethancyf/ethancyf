Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class ClaimConsentDecaraDeclaration2FullVersionSPName40_v2

        Public Sub New(ByVal strSPName As String, ByVal strDocCode As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName, strDocCode)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String, ByVal strDocCode As String)

            Me.txtConsent2SPName.Text = strSPName
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Chi, strSPName, Me.txtConsent2SPName)

            If strDocCode = ConsentFormInformationModel.DocTypeClass.EC Then
                txtConsent2b.Text = "和政府提供本人的個人資料包括中英文姓"
                txtConsent2c.Text = "名、性別、出生日期、豁免登記證明書編號、其檔案編號、豁免登記證明書上所展示的（簽發）日期及香港身份證號碼。"
            Else
                txtConsent2b.Text = "和政府提供本人的個人資料包括香港身份"
                txtConsent2c.Text = "證號碼、中英文姓名、性別、出生日期、香港身份證上符號標記和簽發日期、以及居留身份。"
            End If

        End Sub

    End Class
End Namespace