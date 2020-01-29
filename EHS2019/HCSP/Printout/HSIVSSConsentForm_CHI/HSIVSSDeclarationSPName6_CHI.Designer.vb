Namespace PrintOut.HSIVSSConsentForm_CHI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HSIVSSDeclarationSPName6_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HSIVSSDeclarationSPName6_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSPName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtServiceDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox13 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.chkDescription = New GrapeCity.ActiveReports.SectionReportModel.CheckBox()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtServiceDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.chkDescription, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox3, Me.txtSPName, Me.txtServiceDate, Me.TextBox13, Me.chkDescription, Me.TextBox1})
            Me.Detail.Height = 0.390625!
            Me.Detail.Name = "Detail"
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.15625!
            Me.TextBox3.Left = 0.0!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-family: HA_MingLiu; font-size: 10.5pt; font-style: normal; text-align: justi" & _
        "fy; ddo-char-set: 0"
            Me.TextBox3.Text = "本人同意由"
            Me.TextBox3.Top = 0.0!
            Me.TextBox3.Width = 0.78125!
            '
            'txtSPName
            '
            Me.txtSPName.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtSPName.CanGrow = False
            Me.txtSPName.Height = 0.15625!
            Me.txtSPName.Left = 0.78125!
            Me.txtSPName.Name = "txtSPName"
            Me.txtSPName.Style = "font-family: HA_MingLiu; font-size: 10.5pt; text-align: center; ddo-char-set: 0"
            Me.txtSPName.Text = Nothing
            Me.txtSPName.Top = 0.0!
            Me.txtSPName.Width = 1.219!
            '
            'txtServiceDate
            '
            Me.txtServiceDate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtServiceDate.CanGrow = False
            Me.txtServiceDate.Height = 0.15625!
            Me.txtServiceDate.Left = 2.1875!
            Me.txtServiceDate.Name = "txtServiceDate"
            Me.txtServiceDate.Style = "font-family: HA_MingLiu; font-size: 10.5pt; text-align: center; ddo-char-set: 0"
            Me.txtServiceDate.Text = Nothing
            Me.txtServiceDate.Top = 0.0!
            Me.txtServiceDate.Width = 1.25!
            '
            'TextBox13
            '
            Me.TextBox13.Height = 0.15625!
            Me.TextBox13.Left = 2.0!
            Me.TextBox13.Name = "TextBox13"
            Me.TextBox13.Style = "font-family: HA_MingLiu; font-size: 10.5pt; font-style: normal; text-align: left;" & _
        " ddo-char-set: 0"
            Me.TextBox13.Text = "於"
            Me.TextBox13.Top = 0.0!
            Me.TextBox13.Width = 0.1875!
            '
            'chkDescription
            '
            Me.chkDescription.Checked = True
            Me.chkDescription.Height = 0.188!
            Me.chkDescription.Left = 0.28125!
            Me.chkDescription.Name = "chkDescription"
            Me.chkDescription.Style = "font-family: HA_MingLiu; font-size: 10.5pt; ddo-char-set: 0"
            Me.chkDescription.Tag = "Template"
            Me.chkDescription.Text = ""
            Me.chkDescription.Top = 0.1875!
            Me.chkDescription.Width = 6.75!
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.15625!
            Me.TextBox1.Left = 3.4375!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-family: HA_MingLiu; font-size: 10.5pt; font-style: normal; text-align: left;" & _
        " ddo-char-set: 0"
            Me.TextBox1.Text = "："
            Me.TextBox1.Top = 0.0!
            Me.TextBox1.Width = 0.15625!
            '
            'HSIVSSDeclarationSPName6_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.125!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtServiceDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.chkDescription, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPName As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtServiceDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox13 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents chkDescription As GrapeCity.ActiveReports.SectionReportModel.CheckBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace