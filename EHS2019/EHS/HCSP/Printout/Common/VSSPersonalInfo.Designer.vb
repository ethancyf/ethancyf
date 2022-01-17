Namespace PrintOut.Common
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSPersonalInfo
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSPersonalInfo))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtDOB = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDOBTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtGender = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtGenderTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNameChi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNameChiTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNameEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNameEngTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtDOB, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDOBTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtGender, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtGenderTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameChiTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameEngTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtDOB, Me.txtDOBTitle, Me.txtGender, Me.txtGenderTitle, Me.txtNameChi, Me.txtNameChiTitle, Me.txtNameEng, Me.txtNameEngTitle})
            Me.Detail.Height = 1.110417!
            Me.Detail.Name = "Detail"
            '
            'txtDOB
            '
            Me.txtDOB.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDOB.Height = 0.188!
            Me.txtDOB.Left = 3.003!
            Me.txtDOB.Name = "txtDOB"
            Me.txtDOB.Style = "font-size: 11.25pt; text-align: left; text-decoration: underline"
            Me.txtDOB.Text = Nothing
            Me.txtDOB.Top = 0.844!
            Me.txtDOB.Width = 3.249001!
            '
            'txtDOBTitle
            '
            Me.txtDOBTitle.Height = 0.1875!
            Me.txtDOBTitle.Left = 0.0!
            Me.txtDOBTitle.Name = "txtDOBTitle"
            Me.txtDOBTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtDOBTitle.Text = "Date of Birth:"
            Me.txtDOBTitle.Top = 0.844!
            Me.txtDOBTitle.Width = 2.94!
            '
            'txtGender
            '
            Me.txtGender.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtGender.Height = 0.1875!
            Me.txtGender.Left = 3.003!
            Me.txtGender.Name = "txtGender"
            Me.txtGender.Style = "font-size: 11.25pt; text-align: left; text-decoration: underline"
            Me.txtGender.Text = Nothing
            Me.txtGender.Top = 0.5720001!
            Me.txtGender.Width = 1.40625!
            '
            'txtGenderTitle
            '
            Me.txtGenderTitle.Height = 0.1875!
            Me.txtGenderTitle.Left = 0.0!
            Me.txtGenderTitle.Name = "txtGenderTitle"
            Me.txtGenderTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtGenderTitle.Text = "Sex:"
            Me.txtGenderTitle.Top = 0.572!
            Me.txtGenderTitle.Width = 2.94!
            '
            'txtNameChi
            '
            Me.txtNameChi.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtNameChi.Height = 0.1875!
            Me.txtNameChi.Left = 3.0025!
            Me.txtNameChi.Name = "txtNameChi"
            Me.txtNameChi.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 11.25pt; text-align: left; text-decoration: u" & _
        "nderline; ddo-char-set: 0"
            Me.txtNameChi.Text = Nothing
            Me.txtNameChi.Top = 0.3020002!
            Me.txtNameChi.Width = 1.181102!
            '
            'txtNameChiTitle
            '
            Me.txtNameChiTitle.Height = 0.1875!
            Me.txtNameChiTitle.Left = 0.0!
            Me.txtNameChiTitle.Name = "txtNameChiTitle"
            Me.txtNameChiTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtNameChiTitle.Text = "(in Chinese):"
            Me.txtNameChiTitle.Top = 0.282!
            Me.txtNameChiTitle.Width = 2.94!
            '
            'txtNameEng
            '
            Me.txtNameEng.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtNameEng.Height = 0.1875!
            Me.txtNameEng.Left = 3.003!
            Me.txtNameEng.Name = "txtNameEng"
            Me.txtNameEng.Style = "font-size: 11.25pt; text-align: left; text-decoration: underline"
            Me.txtNameEng.Text = Nothing
            Me.txtNameEng.Top = 0.0!
            Me.txtNameEng.Width = 3.2495!
            '
            'txtNameEngTitle
            '
            Me.txtNameEngTitle.Height = 0.1875!
            Me.txtNameEngTitle.Left = 0.0!
            Me.txtNameEngTitle.Name = "txtNameEngTitle"
            Me.txtNameEngTitle.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtNameEngTitle.Text = "Name of recipient (in English):"
            Me.txtNameEngTitle.Top = 0.0!
            Me.txtNameEngTitle.Width = 2.94!
            '
            'VSSPersonalInfo
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.364583!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtDOB, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDOBTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtGender, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtGenderTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameChiTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameEngTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtDOB As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtDOBTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtGender As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtGenderTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNameChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNameChiTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNameEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNameEngTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace
