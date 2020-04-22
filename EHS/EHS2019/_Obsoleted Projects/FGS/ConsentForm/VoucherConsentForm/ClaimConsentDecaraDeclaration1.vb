Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 
Namespace PrintOut.VoucherConsentForm

    Public Class ClaimConsentDecaraDeclaration1

        Private _strSPName As String
        Private _strVoucherRedem As String
        Private _strVoucherUnuse As String

        Public Sub New(ByVal strSPName As String, ByVal strVoucherRedem As String, ByVal strVoucherUnuse As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            Me._strSPName = strSPName
            Me._strVoucherRedem = strVoucherRedem
            Me._strVoucherUnuse = strVoucherUnuse

        End Sub

        Private Sub dtlClaimConsentDecaraDeclaration1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtlClaimConsentDecaraDeclaration1.Format

        End Sub

        Private Sub ClaimConsentDecaraDeclaration1_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.SetControlPosition()
        End Sub

        Private Sub SetControlPosition()
            Me.txtConsentTransactionUsedVoucher.Text = Me._strVoucherRedem
            Me.txtConsentTransactionUnuseVoucher.Text = Me._strVoucherUnuse
            Me.FillConsentTransactionSPName()

            If Me._strSPName.Length <= 20 Then
                txtConsentTransactionSPName2.Width = 3.125!

                txtConsentTransaction3.Text = ". I acknowledge that I have"
                txtConsentTransaction3.Width = 1.906!
                txtConsentTransaction3.Location = New System.Drawing.PointF(3.125!, 0.219!)

                txtConsentTransactionUnuseVoucher.Location = New System.Drawing.PointF(5.031!, 0.219!)

                txtConsentTransaction4.Text = "unused voucher(s) after this"
                txtConsentTransaction4.Width = 2.031!
                txtConsentTransaction4.Location = New System.Drawing.PointF(5.375!, 0.219!)

                txtConsentTransaction5.Text = "consultation."
                txtConsentTransaction5.Width = 7.375!
                txtConsentTransaction5.Location = New System.Drawing.PointF(0.0!, 0.438!)

            ElseIf Me._strSPName.Length <= 30 Then
                txtConsentTransactionSPName2.Width = 4.594!

                txtConsentTransaction3.Text = ". I acknowledge that I have"
                txtConsentTransaction3.Width = 1.906!
                txtConsentTransaction3.Location = New System.Drawing.PointF(4.594!, 0.219!)

                txtConsentTransactionUnuseVoucher.Location = New System.Drawing.PointF(6.5!, 0.219!)

                txtConsentTransaction4.Text = "unused"
                txtConsentTransaction4.Width = 0.563!
                txtConsentTransaction4.Location = New System.Drawing.PointF(6.813!, 0.2199!)

                txtConsentTransaction5.Text = "voucher(s) after this consultation."
                txtConsentTransaction5.Width = 7.375!
                txtConsentTransaction5.Location = New System.Drawing.PointF(0.0!, 0.438!)

            ElseIf Me._strSPName.Length <= 40 Then

                txtConsentTransactionSPName2.Width = 5.875!

                txtConsentTransaction3.Text = ". I acknowledge that I"
                txtConsentTransaction3.Width = 1.906!
                txtConsentTransaction3.Location = New System.Drawing.PointF(5.875!, 0.219!)

                txtConsentTransactionUnuseVoucher.Location = New System.Drawing.PointF(0.406!, 0.438!)

                txtConsentTransaction4.Text = "have"
                txtConsentTransaction4.Width = 0.406!
                txtConsentTransaction4.Location = New System.Drawing.PointF(0.0!, 0.438!)

                txtConsentTransaction5.Text = "unused voucher(s) after this consultation."
                txtConsentTransaction5.Width = 6.688!
                txtConsentTransaction5.Location = New System.Drawing.PointF(0.719!, 0.438!)
            End If
        End Sub

        Private Sub FillConsentTransactionSPName()
            txtConsentTransactionSPName1.Text = String.Empty
            txtConsentTransactionSPName2.Text = String.Empty

            'Consent Transaction

            Dim strSPNames As String() = Me._strSPName.Split(" ")
            If strSPNames.Length > 1 AndAlso strSPNames(0).Length <= 11 Then
                Dim intSPNameLength As Integer = 0

                For Each partaiName As String In strSPNames
                    '+ 1 is the spacing
                    intSPNameLength += partaiName.Length + 1

                    'text will so in txtConsentTransactionSPName1, if inclued characters not larger than 11
                    If 11 >= intSPNameLength Then
                        txtConsentTransactionSPName1.Text += String.Format("{0} ", partaiName)
                        txtConsentTransactionSPName1.Alignment = TextAlignment.Right
                    Else

                        txtConsentTransactionSPName2.Text += String.Format("{0} ", partaiName)
                        txtConsentTransactionSPName2.Alignment = TextAlignment.Left
                    End If
                Next

                If txtConsentTransactionSPName1.Text.Equals(String.Empty) Then
                    txtConsentTransactionSPName2.Alignment = TextAlignment.Center
                ElseIf txtConsentTransactionSPName2.Text.Equals(String.Empty) Then
                    txtConsentTransactionSPName1.Alignment = TextAlignment.Center
                End If
            Else
                txtConsentTransactionSPName1.Text = _strSPName
                txtConsentTransactionSPName1.Alignment = TextAlignment.Center

            End If
        End Sub

    End Class

End Namespace