Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 
Imports Common.Component

Namespace PrintOut.VoucherConsentForm

    Public Class ClaimConsentDecaraDeclaration2FullVersionSPName30

        Private _strSPName As String
        Private _strDocType As String

        Public Sub New(ByVal udtSPName As String, ByVal strDocType As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._strDocType = strDocType
            Me._strSPName = udtSPName
        End Sub

        Private Sub ClaimConsentDecaraDeclaration1SPName20_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Dim strIDCardData As String = String.Empty

            Me.txtConsent1SPName.Text = Me._strSPName

            If Me._strDocType.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then
                strIDCardData = "including Hong Kong Identity Card No., name (in English and Chinese), gender, date of birth and date of issue of Hong Kong Identity Card"
            Else
                strIDCardData = "including name (in English and Chinese), gender, date of birth, Serial No., Reference (number), (issue) date and Hong Kong Identity Card No. shown on the Certificate of Exemption"
            End If
            Me.txtConsent1c.Text = String.Format("my personal data {0}.", strIDCardData)


        End Sub

    End Class

End Namespace