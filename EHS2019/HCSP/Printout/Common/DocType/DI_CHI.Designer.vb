
Namespace PrintOut.Common.DocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class DI_CHI
        Inherits GrapeCity.ActiveReports.SectionReport

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
            End If
            MyBase.Dispose(disposing)
        End Sub

        'NOTE: The following procedure is required by the ActiveReports Designer
        'It can be modified using the ActiveReports Designer.
        'Do not modify it using the code editor.
        Private WithEvents Detail As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(DI_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtDINo = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtDateOfIssue = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.txtDINo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDateOfIssue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox1, Me.txtDINo, Me.txtDateOfIssue, Me.TextBox2, Me.TextBox3})
            Me.Detail.Height = 0.667!
            Me.Detail.Name = "Detail"
            '
            'txtDINo
            '
            Me.txtDINo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDINo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDINo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDINo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDINo.Border.RightColor = System.Drawing.Color.Black
            Me.txtDINo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDINo.Border.TopColor = System.Drawing.Color.Black
            Me.txtDINo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDINo.Height = 0.1875!
            Me.txtDINo.Left = 1.0625!
            Me.txtDINo.Name = "txtDINo"
            Me.txtDINo.Style = "ddo-char-set: 0; font-size: 12pt; font-family: HA_MingLiu; "
            Me.txtDINo.Text = Nothing
            Me.txtDINo.Top = 0.21875!
            Me.txtDINo.Width = 1.875!
            '
            'txtDateOfIssue
            '
            Me.txtDateOfIssue.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDateOfIssue.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDateOfIssue.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDateOfIssue.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDateOfIssue.Border.RightColor = System.Drawing.Color.Black
            Me.txtDateOfIssue.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDateOfIssue.Border.TopColor = System.Drawing.Color.Black
            Me.txtDateOfIssue.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDateOfIssue.Height = 0.1875!
            Me.txtDateOfIssue.Left = 1.0625!
            Me.txtDateOfIssue.Name = "txtDateOfIssue"
            Me.txtDateOfIssue.Style = "ddo-char-set: 0; font-size: 12pt; font-family: HA_MingLiu; "
            Me.txtDateOfIssue.Text = Nothing
            Me.txtDateOfIssue.Top = 0.4375!
            Me.txtDateOfIssue.Width = 1.59375!
            '
            'TextBox2
            '
            Me.TextBox2.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Height = 0.1875!
            Me.TextBox2.Left = 0.0!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "ddo-char-set: 0; font-size: 12pt; font-family: HA_MingLiu; "
            Me.TextBox2.Text = "- 證件號碼："
            Me.TextBox2.Top = 0.21875!
            Me.TextBox2.Width = 1.03125!
            '
            'TextBox3
            '
            Me.TextBox3.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox3.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox3.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox3.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox3.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Height = 0.1875!
            Me.TextBox3.Left = 0.0!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "ddo-char-set: 0; font-size: 12pt; font-family: HA_MingLiu; "
            Me.TextBox3.Text = "- 簽發日期："
            Me.TextBox3.Top = 0.4375!
            Me.TextBox3.Width = 1.03125!
            '
            'TextBox1
            '
            Me.TextBox1.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Height = 0.1875!
            Me.TextBox1.Left = 0.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "ddo-char-set: 0; font-size: 12pt; font-family: HA_MingLiu; "
            Me.TextBox1.Text = "香港特別行政區簽證身份書"
            Me.TextBox1.Top = 0.0!
            Me.TextBox1.Width = 6.375!
            '
            'DI_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtDINo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDateOfIssue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDINo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDateOfIssue As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
