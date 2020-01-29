Namespace PrintOut.VoucherConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class ClaimConsentDecaraDeclaration2FullVersionSPNameNA
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ClaimConsentDecaraDeclaration2FullVersionSPNameNA))
            Me.Detail1 = New DataDynamics.ActiveReports.Detail
            Me.txtConsent1c = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent1SPName = New DataDynamics.ActiveReports.TextBox
            Me.txtConsent1a = New DataDynamics.ActiveReports.TextBox
            CType(Me.txtConsent1c, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent1SPName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsent1a, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.txtConsent1c, Me.txtConsent1SPName, Me.txtConsent1a})
            Me.Detail1.Height = 0.6354167!
            Me.Detail1.Name = "Detail1"
            '
            'txtConsent1c
            '
            Me.txtConsent1c.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent1c.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1c.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent1c.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1c.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent1c.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1c.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent1c.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1c.Height = 0.40625!
            Me.txtConsent1c.Left = 0.0!
            Me.txtConsent1c.Name = "txtConsent1c"
            Me.txtConsent1c.Style = "ddo-char-set: 1; text-align: justify; font-size: 11.25pt; "
            Me.txtConsent1c.Text = "and the Government my personal data including Hong Kong Identity Card No., name (" & _
                "in English and Chinese), gender, date of birth and date of issue of Hong Kong Id" & _
                "entity Card."
            Me.txtConsent1c.Top = 0.21875!
            Me.txtConsent1c.Width = 7.15625!
            '
            'txtConsent1SPName
            '
            Me.txtConsent1SPName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent1SPName.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtConsent1SPName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent1SPName.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1SPName.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent1SPName.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1SPName.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent1SPName.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1SPName.Height = 0.1875!
            Me.txtConsent1SPName.Left = 4.40625!
            Me.txtConsent1SPName.Name = "txtConsent1SPName"
            Me.txtConsent1SPName.Style = "ddo-char-set: 1; text-align: center; font-size: 10.25pt; vertical-align: bottom; " & _
                ""
            Me.txtConsent1SPName.Text = Nothing
            Me.txtConsent1SPName.Top = 0.0!
            Me.txtConsent1SPName.Width = 2.75!
            '
            'txtConsent1a
            '
            Me.txtConsent1a.Border.BottomColor = System.Drawing.Color.Black
            Me.txtConsent1a.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1a.Border.LeftColor = System.Drawing.Color.Black
            Me.txtConsent1a.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1a.Border.RightColor = System.Drawing.Color.Black
            Me.txtConsent1a.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1a.Border.TopColor = System.Drawing.Color.Black
            Me.txtConsent1a.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtConsent1a.Height = 0.21875!
            Me.txtConsent1a.Left = 0.0!
            Me.txtConsent1a.Name = "txtConsent1a"
            Me.txtConsent1a.Style = "ddo-char-set: 1; text-align: justify; font-size: 11.25pt; "
            Me.txtConsent1a.Text = "I agree to provide (Name of the Enrolled Health Care Provider)"
            Me.txtConsent1a.Top = 0.03125!
            Me.txtConsent1a.Width = 4.375!
            '
            'ClaimConsentDecaraDeclaration2FullVersionSPNameNA
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.177083!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtConsent1c, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent1SPName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsent1a, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtConsent1c As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent1SPName As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtConsent1a As DataDynamics.ActiveReports.TextBox
    End Class

End Namespace