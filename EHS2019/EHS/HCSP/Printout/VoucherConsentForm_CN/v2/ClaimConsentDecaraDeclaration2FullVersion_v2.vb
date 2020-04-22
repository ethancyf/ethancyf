Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm_CN
    Public Class ClaimConsentDecaraDeclaration2FullVersion_v2

        Public Sub New(ByVal strMOName As String, ByVal strDocCode As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strMOName, strDocCode)

        End Sub

        Private Sub LoadReport(ByVal strMOName As String, ByVal strDocCode As String)

            Me.txtConsent2MOName.Text = strMOName

            Formatter.SetSPNameFontSize(Formatter.EnumLang.SimpChi, strMOName, Me.txtConsent2MOName)

            'TEST [Start][Winnie]
            'If strDocCode = ConsentFormInformationModel.DocTypeClass.EC Then
            '    txtConsent2b.Text = "和政府提供本人的个人资料包括中英"
            '    txtConsent2c.Text = "文姓名、性别、出生日期、豁免登记证明书编号、其档案编号、豁免登记证明书上所展示的（签发）日期及香港身份证号码。"
            'Else
            '    txtConsent2b.Text = "和政府提供本人的个人资料包括香港"
            '    txtConsent2c.Text = "身份证号码、中英文姓名、性别、出生日期和香港身份证签发日期。"
            'End If
            'TEST [End][Winnie]
        End Sub

        Private Sub dtlClaimConsedtlntDecaraDeclaration3SPName20_Format(sender As Object, e As EventArgs) Handles dtlClaimConsedtlntDecaraDeclaration3SPName20.Format

        End Sub

        Private Sub ClaimConsentDecaraDeclaration2FullVersion_v2_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class
End Namespace