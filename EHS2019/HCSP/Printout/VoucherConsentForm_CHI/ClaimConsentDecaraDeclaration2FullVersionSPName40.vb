Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document
Imports Common.Component

Namespace PrintOut.VoucherConsentForm_CHI

    Public Class ClaimConsentDecaraDeclaration2FullVersionSPName40
        Private _strSPName As String
        Private _strDocCode As String

        Public Sub New(ByVal strSPName As String, ByVal strDocCode As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._strSPName = strSPName
            Me._strDocCode = strDocCode
        End Sub

        Private Sub ClaimConsentDecaraDeclaration2FullVersionSPName_CHI_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Dim strIDCardInfo As String

            Me.txtConsent2SPName.Text = Me._strSPName

            If Me._strDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                strIDCardInfo = "中英文姓名、性別、出生日期、豁免登記證明書編號、其檔案編號、豁免登記證明書上所展示的（簽發）日期及香港身份證號碼。"
            Else
                strIDCardInfo = "香港身份證號碼、中英文姓名、性別、出生日期和香港身份證簽發日期。"
            End If
            Me.txtConsent2c.Text = String.Format("{0}", strIDCardInfo)
            '和政府提供本人的個人資料

        End Sub
    End Class

End Namespace