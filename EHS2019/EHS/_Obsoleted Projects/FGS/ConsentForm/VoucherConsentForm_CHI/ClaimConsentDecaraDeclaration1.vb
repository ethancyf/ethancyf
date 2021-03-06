Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 
Namespace PrintOut.VoucherConsentForm_CHI

    Public Class ClaimConsentDecaraDeclaration1

        Private _strSPName As String
        Private _blnIsChiName As Boolean
        Private _strVoucherRedem As String
        Private _strVoucherUnuse As String

        Public Sub New(ByVal strSPName As String, ByVal blnIsChiName As Boolean, ByVal strVoucherRedem As String, ByVal strVoucherUnuse As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            Me._strSPName = strSPName
            Me._strVoucherRedem = strVoucherRedem
            Me._strVoucherUnuse = strVoucherUnuse
            Me._blnIsChiName = blnIsChiName
        End Sub

        Private Sub dtlClaimConsentDecaraDeclaration1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtlClaimConsentDecaraDeclaration1.Format

        End Sub

        Private Sub ClaimConsentDecaraDeclaration1_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.SetControlPosition(Me._blnIsChiName)
        End Sub

        Private Sub SetControlPosition(ByVal blnIsChiName As Boolean)
            txtConsentTransactionSPName.Text = String.Empty
            txtConsentTransactionSPName.Text = Me._strSPName

            If blnIsChiName Then
                txtConsentTransactionSPName.Width = 1.469!

                txtConsentTransaction2.Text = "處所求診時，使用"
                txtConsentTransaction2.Width = 1.375!
                txtConsentTransaction2.Location = New System.Drawing.PointF(2.688!, 0.0!)

                txtConsentTransactionUsedVoucher.Text = Me._strVoucherRedem
                txtConsentTransactionUsedVoucher.Location = New System.Drawing.PointF(4.063!, 0.0!)

                txtConsentTransaction3.Text = "張醫療券。本人備悉，在是次診症後本人"
                txtConsentTransaction3.Width = 3.063!
                txtConsentTransaction3.Location = New System.Drawing.PointF(4.344!, 0.0!)


                txtConsentTransaction4.Visible = True
                txtConsentTransaction4.Text = "剩餘"
                txtConsentTransaction4.Width = 0.406!
                txtConsentTransaction4.Location = New System.Drawing.PointF(0.0!, 0.219!)


                txtConsentTransactionUnusedVoucher.Text = Me._strVoucherUnuse
                txtConsentTransactionUnusedVoucher.Location = New System.Drawing.PointF(0.406!, 0.219!)

                txtConsentTransaction5.Text = "張醫療券。"
                txtConsentTransaction5.Location = New System.Drawing.PointF(0.719!, 0.219!)

            Else
                If Me._strSPName.Length <= 20 Then
                    txtConsentTransactionSPName.Width = 3.126!

                    txtConsentTransaction2.Text = "處所求診時，使用"
                    txtConsentTransaction2.Width = 1.438!
                    txtConsentTransaction2.Location = New System.Drawing.PointF(4.384!, 0.0!)

                    txtConsentTransactionUsedVoucher.Text = Me._strVoucherRedem
                    txtConsentTransactionUsedVoucher.Width = 0.344!
                    txtConsentTransactionUsedVoucher.Location = New System.Drawing.PointF(5.783!, 0.0!)

                    txtConsentTransaction3.Text = "張醫療券。本人"
                    txtConsentTransaction3.Width = 1.219!
                    txtConsentTransaction3.Location = New System.Drawing.PointF(6.157!, 0.0!)

                    txtConsentTransaction4.Visible = True
                    txtConsentTransaction4.Text = "備悉，在是次診症後本人剩餘"
                    txtConsentTransaction4.Width = 2.719!
                    txtConsentTransaction4.Location = New System.Drawing.PointF(0.0!, 0.219!)

                    txtConsentTransactionUnusedVoucher.Text = Me._strVoucherUnuse
                    txtConsentTransactionUnusedVoucher.Width = 0.344!
                    txtConsentTransactionUnusedVoucher.Location = New System.Drawing.PointF(2.25!, 0.219!)

                    txtConsentTransaction5.Text = "張醫療券。"
                    txtConsentTransaction5.Location = New System.Drawing.PointF(2.594!, 0.219!)

                ElseIf Me._strSPName.Length <= 30 Then
                    txtConsentTransactionSPName.Width = 4.75!

                    txtConsentTransaction2.Text = "處所求診時，使用"
                    txtConsentTransaction2.Width = 1.406!
                    txtConsentTransaction2.Location = New System.Drawing.PointF(5.969!, 0.0!)

                    txtConsentTransactionUsedVoucher.Text = Me._strVoucherRedem
                    txtConsentTransactionUsedVoucher.Location = New System.Drawing.PointF(0.0!, 0.219!)

                    txtConsentTransaction3.Text = "張醫療券。本人備悉，在是次診症後本人剩餘"
                    txtConsentTransaction3.Width = 3.375!
                    txtConsentTransaction3.Location = New System.Drawing.PointF(0.313!, 0.219!)

                    txtConsentTransaction4.Visible = False

                    txtConsentTransactionUnusedVoucher.Text = Me._strVoucherUnuse
                    txtConsentTransactionUnusedVoucher.Location = New System.Drawing.PointF(3.688!, 0.219!)

                    txtConsentTransaction5.Text = "張醫療券。"
                    txtConsentTransaction5.Location = New System.Drawing.PointF(3.991!, 0.219!)
                ElseIf Me._strSPName.Length <= 40 Then
                    txtConsentTransactionSPName.Width = 5.781!

                    txtConsentTransaction2.Text = "處所"
                    txtConsentTransaction2.Width = 1.375!
                    txtConsentTransaction2.Location = New System.Drawing.PointF(7.0!, 0.0!)

                    txtConsentTransaction3.Text = "求診時，使用"
                    txtConsentTransaction3.Width = 1.031!
                    txtConsentTransaction3.Location = New System.Drawing.PointF(0.0!, 0.219!)

                    txtConsentTransactionUsedVoucher.Text = Me._strVoucherRedem
                    txtConsentTransactionUsedVoucher.Location = New System.Drawing.PointF(1.031!, 0.219!)

                    txtConsentTransaction4.Visible = True
                    txtConsentTransaction4.Text = "張醫療券。本人備悉，在是次診症後本人剩餘"
                    txtConsentTransaction4.Width = 3.406!
                    txtConsentTransaction4.Location = New System.Drawing.PointF(1.313!, 0.219!)

                    txtConsentTransactionUnusedVoucher.Text = Me._strVoucherUnuse
                    txtConsentTransactionUnusedVoucher.Location = New System.Drawing.PointF(4.719!, 0.219!)

                    txtConsentTransaction5.Text = "張醫療券。"
                    txtConsentTransaction5.Location = New System.Drawing.PointF(5.031!, 0.219!)
                End If
            End If
        End Sub

    End Class

End Namespace
