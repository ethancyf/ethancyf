
Namespace PrintOut.VoucherConsentForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class ClaimConsent1SPNameNA_v2
        Inherits DataDynamics.ActiveReports.ActiveReport3

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
            End If
            MyBase.Dispose(disposing)
        End Sub

        'NOTE: The following procedure is required by the ActiveReports Designer
        'It can be modified using the ActiveReports Designer.
        'Do not modify it using the code editor.
        Private WithEvents Detail1 As DataDynamics.ActiveReports.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ClaimConsent1SPNameNA_v2))
            Me.Detail1 = New DataDynamics.ActiveReports.Detail
            Me.txtConsentTransaction1 = New DataDynamics.ActiveReports.TextBox
            Me.txtConsentTransaction2 = New DataDynamics.ActiveReports.TextBox
            Me.txtConsentTransaction3 = New DataDynamics.ActiveReports.TextBox
            Me.txtConsentTransaction5 = New DataDynamics.ActiveReports.TextBox
            Me.txtCoPaymentFee = New DataDynamics.ActiveReports.TextBox
            Me.txtConsentTransactionSPName2 = New DataDynamics.ActiveReports.TextBox
            Me.txtConsentTransactionUsedVoucher = New DataDynamics.ActiveReports.TextBox
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox2 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox3 = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtConsentTransaction1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsentTransaction2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsentTransaction3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsentTransaction5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtCoPaymentFee, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsentTransactionSPName2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsentTransactionUsedVoucher, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtConsentTransaction1, Me.txtConsentTransaction2, Me.txtConsentTransaction3, Me.txtConsentTransaction5, Me.txtCoPaymentFee, Me.txtConsentTransactionSPName2, Me.txtConsentTransactionUsedVoucher, Me.TextBox1, Me.TextBox2, Me.TextBox3})
            Me.Detail1.Height = 0.6770833!
            Me.Detail1.Name = "Detail1"
            '
            'txtConsentTransaction1
            '
            Me.txtConsentTransaction1.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsentTransaction1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction1.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsentTransaction1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction1.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsentTransaction1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction1.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsentTransaction1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction1.Height = 0.1875!
            Me.txtConsentTransaction1.Left = 0.0!
            Me.txtConsentTransaction1.Name = "txtConsentTransaction1"
            Me.txtConsentTransaction1.Style = "text-align: justify; font-size: 11.25pt; "
            Me.txtConsentTransaction1.Text = "I consent to use (whole number of)"
            Me.txtConsentTransaction1.Top = 0.0!
            Me.txtConsentTransaction1.Width = 2.4375!
            '
            'txtConsentTransaction2
            '
            Me.txtConsentTransaction2.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsentTransaction2.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction2.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsentTransaction2.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction2.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsentTransaction2.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction2.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsentTransaction2.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction2.Height = 0.1875!
            Me.txtConsentTransaction2.Left = 2.875!
            Me.txtConsentTransaction2.Name = "txtConsentTransaction2"
            Me.txtConsentTransaction2.Style = "text-align: left; font-size: 11.25pt; "
            Me.txtConsentTransaction2.Text = "voucher(s) for the healthcare service provided by (Name of the"
            Me.txtConsentTransaction2.Top = 0.0!
            Me.txtConsentTransaction2.Width = 4.5!
            '
            'txtConsentTransaction3
            '
            Me.txtConsentTransaction3.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsentTransaction3.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction3.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsentTransaction3.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction3.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsentTransaction3.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction3.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsentTransaction3.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction3.Height = 0.1875!
            Me.txtConsentTransaction3.Left = 0.0!
            Me.txtConsentTransaction3.Name = "txtConsentTransaction3"
            Me.txtConsentTransaction3.Style = "text-align: justify; font-size: 11.25pt; "
            Me.txtConsentTransaction3.Text = "an extra service fee"
            Me.txtConsentTransaction3.Top = 0.375!
            Me.txtConsentTransaction3.Width = 1.4375!
            '
            'txtConsentTransaction5
            '
            Me.txtConsentTransaction5.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsentTransaction5.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction5.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsentTransaction5.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction5.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsentTransaction5.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction5.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsentTransaction5.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransaction5.Height = 0.1875!
            Me.txtConsentTransaction5.Left = 2.25!
            Me.txtConsentTransaction5.Name = "txtConsentTransaction5"
            Me.txtConsentTransaction5.Style = "text-align: justify; font-size: 11.25pt; "
            Me.txtConsentTransaction5.Text = "paid."
            Me.txtConsentTransaction5.Top = 0.375!
            Me.txtConsentTransaction5.Width = 0.4375!
            '
            'txtCoPaymentFee
            '
            Me.txtCoPaymentFee.Border.BottomColor = System.Drawing.Color.Black
            Me.txtCoPaymentFee.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtCoPaymentFee.Border.LeftColor = System.Drawing.Color.Black
            Me.txtCoPaymentFee.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtCoPaymentFee.Border.RightColor = System.Drawing.Color.Black
            Me.txtCoPaymentFee.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtCoPaymentFee.Border.TopColor = System.Drawing.Color.Black
            Me.txtCoPaymentFee.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtCoPaymentFee.Height = 0.1875!
            Me.txtCoPaymentFee.Left = 1.625!
            Me.txtCoPaymentFee.Name = "txtCoPaymentFee"
            Me.txtCoPaymentFee.Style = "text-align: center; font-size: 11.25pt; vertical-align: bottom; "
            Me.txtCoPaymentFee.Text = Nothing
            Me.txtCoPaymentFee.Top = 0.375!
            Me.txtCoPaymentFee.Width = 0.5625!
            '
            'txtConsentTransactionSPName2
            '
            Me.txtConsentTransactionSPName2.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsentTransactionSPName2.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtConsentTransactionSPName2.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsentTransactionSPName2.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransactionSPName2.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsentTransactionSPName2.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransactionSPName2.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsentTransactionSPName2.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransactionSPName2.Height = 0.1875!
            Me.txtConsentTransactionSPName2.Left = 2.1875!
            Me.txtConsentTransactionSPName2.Name = "txtConsentTransactionSPName2"
            Me.txtConsentTransactionSPName2.Style = "ddo-char-set: 1; text-align: center; font-size: 11.25pt; vertical-align: bottom; " & _
                ""
            Me.txtConsentTransactionSPName2.Text = Nothing
            Me.txtConsentTransactionSPName2.Top = 0.1875!
            Me.txtConsentTransactionSPName2.Width = 4.6!
            '
            'txtConsentTransactionUsedVoucher
            '
            Me.txtConsentTransactionUsedVoucher.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsentTransactionUsedVoucher.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtConsentTransactionUsedVoucher.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsentTransactionUsedVoucher.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransactionUsedVoucher.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsentTransactionUsedVoucher.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransactionUsedVoucher.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsentTransactionUsedVoucher.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsentTransactionUsedVoucher.Height = 0.1875!
            Me.txtConsentTransactionUsedVoucher.Left = 2.5!
            Me.txtConsentTransactionUsedVoucher.Name = "txtConsentTransactionUsedVoucher"
            Me.txtConsentTransactionUsedVoucher.Style = "text-align: center; font-size: 11.25pt; vertical-align: bottom; "
            Me.txtConsentTransactionUsedVoucher.Text = Nothing
            Me.txtConsentTransactionUsedVoucher.Top = 0.0!
            Me.txtConsentTransactionUsedVoucher.Width = 0.3125!
            '
            'TextBox1
            '
            Me.TextBox1.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Height = 0.1875!
            Me.TextBox1.Left = 1.4375!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "text-align: justify; font-size: 11.25pt; "
            Me.TextBox1.Text = "$"
            Me.TextBox1.Top = 0.375!
            Me.TextBox1.Width = 0.1875!
            '
            'TextBox2
            '
            Me.TextBox2.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox2.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox2.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox2.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox2.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Height = 0.1875!
            Me.TextBox2.Left = 0.0!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "text-align: left; font-size: 11.25pt; "
            Me.TextBox2.Text = "Enrolled Health Care Provider)"
            Me.TextBox2.Top = 0.1875!
            Me.TextBox2.Width = 2.1875!
            '
            'TextBox3
            '
            Me.TextBox3.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox3.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox3.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox3.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox3.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Height = 0.1875!
            Me.TextBox3.Left = 6.8125!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "text-align: justify; font-size: 11.25pt; "
            Me.TextBox3.Text = "with "
            Me.TextBox3.Top = 0.1875!
            Me.TextBox3.Width = 0.5625!
            '
            'ClaimConsent1SPNameNA_v2
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.417!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            Me.UserData = ""
            CType(Me.txtConsentTransaction1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsentTransaction2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsentTransaction3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsentTransaction5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtCoPaymentFee, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsentTransactionSPName2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsentTransactionUsedVoucher, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtConsentTransaction1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsentTransaction2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsentTransaction3 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsentTransaction5 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtCoPaymentFee As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsentTransactionSPName2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsentTransactionUsedVoucher As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox3 As DataDynamics.ActiveReports.TextBox
    End Class

End Namespace