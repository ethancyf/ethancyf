Namespace PrintOut.Common

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSPersonalInfo_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSPersonalInfo_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtNameEngTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNameEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNameChiTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNameChi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtGenderTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtGender = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDOBTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDOB = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtNameEngTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameChiTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtGenderTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtGender, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDOBTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDOB, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtNameEngTitle, Me.txtNameEng, Me.txtNameChiTitle, Me.txtNameChi, Me.txtGenderTitle, Me.txtGender, Me.txtDOBTitle, Me.txtDOB})
            Me.Detail.Height = 1.079167!
            Me.Detail.Name = "Detail"
            '
            'txtNameEngTitle
            '
            Me.txtNameEngTitle.Height = 0.188!
            Me.txtNameEngTitle.Left = 0.0!
            Me.txtNameEngTitle.Name = "txtNameEngTitle"
            Me.txtNameEngTitle.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: right; " & _
        "ddo-char-set: 0"
            Me.txtNameEngTitle.Text = "服務使用者姓名(英文)："
            Me.txtNameEngTitle.Top = 0.0!
            Me.txtNameEngTitle.Width = 2.251!
            '
            'txtNameEng
            '
            Me.txtNameEng.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtNameEng.CanGrow = False
            Me.txtNameEng.Height = 0.1875!
            Me.txtNameEng.Left = 2.275!
            Me.txtNameEng.Name = "txtNameEng"
            Me.txtNameEng.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; text-decoration: unde" & _
        "rline; ddo-char-set: 0"
            Me.txtNameEng.Text = "　"
            Me.txtNameEng.Top = 0.0!
            Me.txtNameEng.Width = 3.3125!
            '
            'txtNameChiTitle
            '
            Me.txtNameChiTitle.Height = 0.1875!
            Me.txtNameChiTitle.Left = 0.0!
            Me.txtNameChiTitle.Name = "txtNameChiTitle"
            Me.txtNameChiTitle.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: right; " & _
        "ddo-char-set: 0"
            Me.txtNameChiTitle.Text = "(中文)："
            Me.txtNameChiTitle.Top = 0.281!
            Me.txtNameChiTitle.Width = 2.251!
            '
            'txtNameChi
            '
            Me.txtNameChi.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtNameChi.CanGrow = False
            Me.txtNameChi.Height = 0.188!
            Me.txtNameChi.Left = 2.275!
            Me.txtNameChi.Name = "txtNameChi"
            Me.txtNameChi.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: left; text-decoration: u" & _
        "nderline; ddo-char-set: 0"
            Me.txtNameChi.Text = "　"
            Me.txtNameChi.Top = 0.281!
            Me.txtNameChi.Width = 1.406!
            '
            'txtGenderTitle
            '
            Me.txtGenderTitle.Height = 0.1875!
            Me.txtGenderTitle.Left = 0.0!
            Me.txtGenderTitle.Name = "txtGenderTitle"
            Me.txtGenderTitle.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: right; " & _
        "ddo-char-set: 0"
            Me.txtGenderTitle.Text = "性別："
            Me.txtGenderTitle.Top = 0.562!
            Me.txtGenderTitle.Width = 2.251!
            '
            'txtGender
            '
            Me.txtGender.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtGender.CanGrow = False
            Me.txtGender.Height = 0.188!
            Me.txtGender.Left = 2.275!
            Me.txtGender.Name = "txtGender"
            Me.txtGender.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; text-decoration: unde" & _
        "rline; ddo-char-set: 0"
            Me.txtGender.Text = "　"
            Me.txtGender.Top = 0.562!
            Me.txtGender.Width = 1.406!
            '
            'txtDOBTitle
            '
            Me.txtDOBTitle.Height = 0.1875!
            Me.txtDOBTitle.Left = 0.0!
            Me.txtDOBTitle.Name = "txtDOBTitle"
            Me.txtDOBTitle.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: right; " & _
        "ddo-char-set: 0"
            Me.txtDOBTitle.Text = "出生日期："
            Me.txtDOBTitle.Top = 0.843!
            Me.txtDOBTitle.Width = 2.251!
            '
            'txtDOB
            '
            Me.txtDOB.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDOB.CanGrow = False
            Me.txtDOB.Height = 0.1875!
            Me.txtDOB.Left = 2.275!
            Me.txtDOB.Name = "txtDOB"
            Me.txtDOB.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; text-decoration: unde" & _
        "rline; ddo-char-set: 0"
            Me.txtDOB.Text = "　"
            Me.txtDOB.Top = 0.8430001!
            Me.txtDOB.Width = 3.3125!
            '
            'VSSPersonalInfo_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 5.760167!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtNameEngTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameChiTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtGenderTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtGender, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDOBTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDOB, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtNameEngTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNameEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNameChiTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtNameChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtGenderTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtGender As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtDOBTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtDOB As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class


End Namespace