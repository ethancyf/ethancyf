Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 
Namespace PrintOut.VoucherConsentForm

    Public Class ClaimConsentDecaraDeclaration2

        Private _strSPName As String


        Public Sub New(ByVal strSPName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._strSPName = strSPName
        End Sub


        Private Sub ClaimConsedtlntDecaraDeclaration2_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtlClaimConsedtlntDecaraDeclaration2.Format

        End Sub

        Private Sub SetControlPosition()
            Me.FillConsentTransactionSPName()

            If Me._strSPName.Length <= 20 Then
                txtDeclarationSPName2.Visible = False

                txtDeclarationSPName1.Width = 3.031!

                txtDeclaration2.Text = ".  I understand and agree to"
                txtDeclaration2.Width = 1.969!
                txtDeclaration2.Location = New System.Drawing.PointF(5.438!, 0.188!)

                txtDeclaration3.Text = "the contents of the Undertaking and Declaration."
                txtDeclaration3.Width = 7.375!
                txtDeclaration3.Location = New System.Drawing.PointF(0.0!, 0.406!)

                Me.dtlClaimConsedtlntDecaraDeclaration2.Height = 0.625!
            ElseIf Me._strSPName.Length <= 30 Then
                txtDeclarationSPName2.Visible = False

                txtDeclarationSPName1.Width = 4.9!

                txtDeclaration2.Text = "I understand and agree to the contents of the Undertaking and Declaration."
                txtDeclaration2.Width = 7.375!
                txtDeclaration2.Location = New System.Drawing.PointF(0.0!, 0.406!)

                txtDeclaration3.Text = "."
                txtDeclaration3.Width = 0.075!
                txtDeclaration3.Location = New System.Drawing.PointF(7.3!, 0.188!)

                Me.dtlClaimConsedtlntDecaraDeclaration2.Height = 0.625!
            ElseIf Me._strSPName.Length <= 40 Then
                txtDeclarationSPName2.Visible = True
                txtDeclarationSPName2.Location = New System.Drawing.PointF(0.0!, 0.406!)
                txtDeclarationSPName2.Width = 6.031!

                txtDeclarationSPName1.Width = 4.969!

                txtDeclaration2.Text = ".  I understand and"
                txtDeclaration2.Width = 1.344!
                txtDeclaration2.Location = New System.Drawing.PointF(6.031!, 0.406!)

                txtDeclaration3.Text = "agree to the contents of the Undertaking and Declaration."
                txtDeclaration3.Width = 7.375!
                txtDeclaration3.Location = New System.Drawing.PointF(0.0!, 0.594!)
                Me.dtlClaimConsedtlntDecaraDeclaration2.Height = 0.813!
            End If
        End Sub

        Private Sub FillConsentTransactionSPName()
            Dim strSPNames As String() = Me._strSPName.Split(" ")

            txtDeclarationSPName1.Text = String.Empty
            txtDeclarationSPName2.Text = String.Empty

            'Consent Transaction
            If strSPNames.Length > 1 AndAlso strSPNames(0).Length <= 30 Then
                Dim intSPNameLength As Integer = 0

                For Each partaiName As String In strSPNames
                    '+ 1 is the spacing
                    intSPNameLength += partaiName.Length + 1

                    'text will so in txtConsentTransactionSPName1, if inclued characters not larger than 11
                    If 30 >= intSPNameLength Then
                        txtDeclarationSPName1.Text += String.Format("{0} ", partaiName)
                        txtDeclarationSPName1.Alignment = TextAlignment.Right
                    Else

                        txtDeclarationSPName2.Text += String.Format("{0} ", partaiName)
                        txtDeclarationSPName2.Alignment = TextAlignment.Left
                    End If
                Next

                If txtDeclarationSPName1.Text.Equals(String.Empty) Then
                    txtDeclarationSPName2.Alignment = TextAlignment.Center
                ElseIf txtDeclarationSPName2.Text.Equals(String.Empty) Then
                    txtDeclarationSPName1.Alignment = TextAlignment.Center
                End If
            Else
                txtDeclarationSPName1.Text = _strSPName
                txtDeclarationSPName1.Alignment = TextAlignment.Center
            End If
        End Sub

        Private Sub ClaimConsentDecaraDeclaration2_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.SetControlPosition()
        End Sub
    End Class

End Namespace