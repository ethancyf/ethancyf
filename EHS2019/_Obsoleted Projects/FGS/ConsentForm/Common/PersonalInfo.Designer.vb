Namespace PrintOut.Common
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class PersonalInfo
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
        Private WithEvents Detail As DataDynamics.ActiveReports.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PersonalInfo))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.TextBox1 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox4 = New DataDynamics.ActiveReports.TextBox
            Me.txtNameChi = New DataDynamics.ActiveReports.TextBox
            Me.txtDOB = New DataDynamics.ActiveReports.TextBox
            Me.txtGender = New DataDynamics.ActiveReports.TextBox
            Me.txtNameEng = New DataDynamics.ActiveReports.TextBox
            Me.TextBox2 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox3 = New DataDynamics.ActiveReports.TextBox
            Me.txtGender1 = New DataDynamics.ActiveReports.TextBox
            Me.txtGender2 = New DataDynamics.ActiveReports.TextBox
            Me.TextBox5 = New DataDynamics.ActiveReports.TextBox
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDOB, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtGender, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtGender1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtGender2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.TextBox1, Me.TextBox4, Me.txtNameChi, Me.txtDOB, Me.txtGender, Me.txtNameEng, Me.TextBox2, Me.TextBox3, Me.txtGender1, Me.txtGender2, Me.TextBox5})
            Me.Detail.Height = 0.75!
            Me.Detail.Name = "Detail"
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
            Me.TextBox1.Height = 0.21875!
            Me.TextBox1.Left = 0.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "text-align: left; font-size: 11.25pt; "
            Me.TextBox1.Text = "Name:"
            Me.TextBox1.Top = 0.0!
            Me.TextBox1.Width = 0.5!
            '
            'TextBox4
            '
            Me.TextBox4.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox4.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox4.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox4.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox4.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox4.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox4.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox4.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox4.Height = 0.21875!
            Me.TextBox4.Left = 0.0!
            Me.TextBox4.Name = "TextBox4"
            Me.TextBox4.Style = "text-align: left; font-size: 11.25pt; "
            Me.TextBox4.Text = "Date of Birth:"
            Me.TextBox4.Top = 0.5!
            Me.TextBox4.Width = 1.25!
            '
            'txtNameChi
            '
            Me.txtNameChi.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNameChi.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtNameChi.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNameChi.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNameChi.Border.RightColor = System.Drawing.Color.Black
            Me.txtNameChi.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNameChi.Border.TopColor = System.Drawing.Color.Black
            Me.txtNameChi.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNameChi.Height = 0.21875!
            Me.txtNameChi.Left = 1.3125!
            Me.txtNameChi.Name = "txtNameChi"
            Me.txtNameChi.Style = "text-decoration: underline; ddo-char-set: 136; text-align: left; font-size: 12pt;" & _
                " font-family: HA_MingLiu; "
            Me.txtNameChi.Text = "¡@"
            Me.txtNameChi.Top = 0.25!
            Me.txtNameChi.Width = 2.21875!
            '
            'txtDOB
            '
            Me.txtDOB.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDOB.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtDOB.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDOB.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDOB.Border.RightColor = System.Drawing.Color.Black
            Me.txtDOB.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDOB.Border.TopColor = System.Drawing.Color.Black
            Me.txtDOB.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtDOB.Height = 0.21875!
            Me.txtDOB.Left = 1.3125!
            Me.txtDOB.Name = "txtDOB"
            Me.txtDOB.Style = "text-decoration: underline; text-align: left; font-size: 11.25pt; "
            Me.txtDOB.Text = Nothing
            Me.txtDOB.Top = 0.5!
            Me.txtDOB.Width = 2.21875!
            '
            'txtGender
            '
            Me.txtGender.Border.BottomColor = System.Drawing.Color.Black
            Me.txtGender.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtGender.Border.LeftColor = System.Drawing.Color.Black
            Me.txtGender.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender.Border.RightColor = System.Drawing.Color.Black
            Me.txtGender.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender.Border.TopColor = System.Drawing.Color.Black
            Me.txtGender.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender.Height = 0.21875!
            Me.txtGender.Left = 4.125!
            Me.txtGender.Name = "txtGender"
            Me.txtGender.Style = "text-decoration: underline; text-align: justify; font-size: 11.25pt; "
            Me.txtGender.Text = Nothing
            Me.txtGender.Top = 0.5!
            Me.txtGender.Width = 2.21875!
            '
            'txtNameEng
            '
            Me.txtNameEng.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNameEng.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid
            Me.txtNameEng.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNameEng.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNameEng.Border.RightColor = System.Drawing.Color.Black
            Me.txtNameEng.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNameEng.Border.TopColor = System.Drawing.Color.Black
            Me.txtNameEng.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtNameEng.Height = 0.21875!
            Me.txtNameEng.Left = 1.3125!
            Me.txtNameEng.Name = "txtNameEng"
            Me.txtNameEng.Style = "text-decoration: underline; text-align: left; font-size: 11.25pt; "
            Me.txtNameEng.Text = Nothing
            Me.txtNameEng.Top = 0.0!
            Me.txtNameEng.Width = 5.59375!
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
            Me.TextBox2.Height = 0.21875!
            Me.TextBox2.Left = 0.53125!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "text-align: left; font-size: 11.25pt; "
            Me.TextBox2.Text = "(English)"
            Me.TextBox2.Top = 0.0!
            Me.TextBox2.Width = 0.71875!
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
            Me.TextBox3.Height = 0.21875!
            Me.TextBox3.Left = 0.53125!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "text-align: left; font-size: 11.25pt; "
            Me.TextBox3.Text = "(Chinese)"
            Me.TextBox3.Top = 0.25!
            Me.TextBox3.Width = 0.71875!
            '
            'txtGender1
            '
            Me.txtGender1.Border.BottomColor = System.Drawing.Color.Black
            Me.txtGender1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender1.Border.LeftColor = System.Drawing.Color.Black
            Me.txtGender1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender1.Border.RightColor = System.Drawing.Color.Black
            Me.txtGender1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender1.Border.TopColor = System.Drawing.Color.Black
            Me.txtGender1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender1.Height = 0.1875!
            Me.txtGender1.Left = 4.125!
            Me.txtGender1.Name = "txtGender1"
            Me.txtGender1.Style = "ddo-char-set: 0; text-align: left; font-size: 11.25pt; font-family: Arial; "
            Me.txtGender1.Text = "*Male / Female"
            Me.txtGender1.Top = 0.5!
            Me.txtGender1.Width = 1.21875!
            '
            'txtGender2
            '
            Me.txtGender2.Border.BottomColor = System.Drawing.Color.Black
            Me.txtGender2.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender2.Border.LeftColor = System.Drawing.Color.Black
            Me.txtGender2.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender2.Border.RightColor = System.Drawing.Color.Black
            Me.txtGender2.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender2.Border.TopColor = System.Drawing.Color.Black
            Me.txtGender2.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.txtGender2.Height = 0.1875!
            Me.txtGender2.Left = 5.375!
            Me.txtGender2.Name = "txtGender2"
            Me.txtGender2.Style = "ddo-char-set: 0; text-align: left; font-style: italic; font-size: 9.75pt; font-fa" & _
                "mily: Arial; "
            Me.txtGender2.Text = "(* delete as appropriate)"
            Me.txtGender2.Top = 0.5!
            Me.txtGender2.Width = 1.5!
            '
            'TextBox5
            '
            Me.TextBox5.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox5.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox5.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox5.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox5.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Height = 0.21875!
            Me.TextBox5.Left = 3.71875!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "text-align: left; font-size: 11.25pt; "
            Me.TextBox5.Text = "Sex:"
            Me.TextBox5.Top = 0.5!
            Me.TextBox5.Width = 0.34375!
            '
            'PersonalInfo
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.99975!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDOB, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtGender, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtGender1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtGender2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox4 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtNameChi As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtDOB As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox5 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtGender As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox2 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents TextBox3 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtNameEng As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtGender1 As DataDynamics.ActiveReports.TextBox
        Friend WithEvents txtGender2 As DataDynamics.ActiveReports.TextBox
    End Class

End Namespace
