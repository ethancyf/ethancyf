Namespace PrintOut.VSSConsentForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSVaccinationInfo_DA_P
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSVaccinationInfo_DA_P))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox13 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtServiceDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox50 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSPName2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSPName1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtServiceDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox50, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPName2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPName1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox3, Me.TextBox13, Me.txtServiceDate, Me.TextBox50, Me.txtSPName2, Me.txtSPName1, Me.TextBox5})
            Me.Detail.Height = 0.6666667!
            Me.Detail.Name = "Detail"
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.5625!
            Me.TextBox3.Left = 0.0!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-size: 11.25pt; font-style: normal; text-align: justify; text-justify: distri" & _
        "bute; ddo-char-set: 0"
            Me.TextBox3.Text = "I consent to use the following Government subsidy to receive / for the recipient " & _
        "indicated below to receive*"
            Me.TextBox3.Top = 0.0!
            Me.TextBox3.Width = 7.375!
            '
            'TextBox13
            '
            Me.TextBox13.Height = 0.1875!
            Me.TextBox13.Left = 1.9375!
            Me.TextBox13.MultiLine = False
            Me.TextBox13.Name = "TextBox13"
            Me.TextBox13.Style = "font-size: 11.25pt; font-style: normal; text-align: center; white-space: nowrap; " & _
        "ddo-char-set: 0"
            Me.TextBox13.Text = "on"
            Me.TextBox13.Top = 0.405!
            Me.TextBox13.Width = 0.25!
            '
            'txtServiceDate
            '
            Me.txtServiceDate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtServiceDate.Height = 0.1875!
            Me.txtServiceDate.Left = 2.1875!
            Me.txtServiceDate.MultiLine = False
            Me.txtServiceDate.Name = "txtServiceDate"
            Me.txtServiceDate.Style = "font-size: 11.25pt; text-align: center"
            Me.txtServiceDate.Text = Nothing
            Me.txtServiceDate.Top = 0.405!
            Me.txtServiceDate.Width = 1.5625!
            '
            'TextBox50
            '
            Me.TextBox50.Height = 0.1875!
            Me.TextBox50.Left = 3.75!
            Me.TextBox50.MultiLine = False
            Me.TextBox50.Name = "TextBox50"
            Me.TextBox50.Style = "font-size: 11.25pt; font-style: normal; text-align: justify; ddo-char-set: 0"
            Me.TextBox50.Text = ":"
            Me.TextBox50.Top = 0.395!
            Me.TextBox50.Width = 0.125!
            '
            'txtSPName2
            '
            Me.txtSPName2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtSPName2.Height = 0.1875!
            Me.txtSPName2.Left = 0.0!
            Me.txtSPName2.MultiLine = False
            Me.txtSPName2.Name = "txtSPName2"
            Me.txtSPName2.Style = "font-size: 11.25pt; text-align: center"
            Me.txtSPName2.Text = Nothing
            Me.txtSPName2.Top = 0.405!
            Me.txtSPName2.Width = 1.937!
            '
            'txtSPName1
            '
            Me.txtSPName1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtSPName1.Height = 0.1875!
            Me.txtSPName1.Left = 1.875!
            Me.txtSPName1.MultiLine = False
            Me.txtSPName1.Name = "txtSPName1"
            Me.txtSPName1.Style = "font-size: 11.25pt; text-align: center"
            Me.txtSPName1.Top = 0.1875001!
            Me.txtSPName1.Width = 5.5!
            '
            'TextBox5
            '
            Me.TextBox5.Height = 0.1875!
            Me.TextBox5.Left = 0.0!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "font-size: 11.25pt; font-style: normal; text-align: justify; ddo-char-set: 0"
            Me.TextBox5.Text = "vaccination(s) provided by"
            Me.TextBox5.Top = 0.1875001!
            Me.TextBox5.Width = 1.875!
            '
            'VSSVaccinationInfo_DA_P
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.40625!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtServiceDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox50, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPName2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPName1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox13 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtServiceDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPName1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox50 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPName2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace