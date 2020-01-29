Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document
Imports Common.ComFunction

Namespace PrintOut.VoucherConsentForm

    Public Class ClaimConsentDecaraDeclaration3FullVersionSPName40

        Private _strSPName As String
        Private _udtReportFunction As ReportFunction


        Public Sub New(ByVal strSPName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            _udtReportFunction = New ReportFunction

            Me._strSPName = strSPName
        End Sub


        Private Sub ClaimConsedtlntDecaraDeclaration2_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtlClaimConsedtlntDecaraDeclaration3SPName20.Format

        End Sub

        Private Sub SetControlPosition()
            ' Document Explained By
            _udtReportFunction.FillSPName(Me._strSPName, Me.txtDeclarationSPName1, Me.txtDeclarationSPName2, 36)
        End Sub

        Private Sub ClaimConsentDecaraDeclaration2_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.SetControlPosition()
        End Sub
    End Class

End Namespace