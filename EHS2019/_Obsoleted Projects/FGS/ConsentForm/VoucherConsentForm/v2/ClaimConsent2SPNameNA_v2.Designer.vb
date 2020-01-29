
Namespace PrintOut.VoucherConsentForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class ClaimConsent2SPNameNA_v2
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ClaimConsent2SPNameNA_v2))
            Me.Detail1 = New DataDynamics.ActiveReports.Detail
            Me.txtConsentTransaction1 = New DataDynamics.ActiveReports.TextBox
            Me.txtConsentTransactionSPName2 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtConsentTransaction1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsentTransactionSPName2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtConsentTransaction1, Me.txtConsentTransactionSPName2, Me.TextBox1})
            Me.Detail1.Height = 0.65625!
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
            Me.txtConsentTransaction1.Text = "(Name of the Enrolled Health Care Provider)"
            Me.txtConsentTransaction1.Top = 0.0!
            Me.txtConsentTransaction1.Width = 3.125!
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
            Me.txtConsentTransactionSPName2.Left = 3.125!
            Me.txtConsentTransactionSPName2.Name = "txtConsentTransactionSPName2"
            Me.txtConsentTransactionSPName2.Style = "ddo-char-set: 1; text-align: center; font-size: 11.25pt; vertical-align: bottom; " & _
                ""
            Me.txtConsentTransactionSPName2.Text = Nothing
            Me.txtConsentTransactionSPName2.Top = 0.0!
            Me.txtConsentTransactionSPName2.Width = 4.25!
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
            Me.TextBox1.Height = 0.375!
            Me.TextBox1.Left = 0.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "text-align: justify; font-size: 11.25pt; "
            Me.TextBox1.Text = "has read and explained to me the Undertaking and Declaration in the ""Consent of V" & _
                "oucher Recipient to Transfer Personal Data"".  I understand and agree to the cont" & _
                "ents contained therein."
            Me.TextBox1.Top = 0.1875!
            Me.TextBox1.Width = 7.375!
            '
            'ClaimConsent2SPNameNA_v2
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.4375!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            Me.UserData = ""
            CType(Me.txtConsentTransaction1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsentTransactionSPName2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtConsentTransaction1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsentTransactionSPName2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
    End Class

End Namespace