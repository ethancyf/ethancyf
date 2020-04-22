

Namespace PrintOut.VoucherAccountChnageForm


    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class SmartIDChangePersonalPaticularForm
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
        Private WithEvents Detail1 As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SmartIDChangePersonalPaticularForm))
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.Label1 = New GrapeCity.ActiveReports.SectionReportModel.Label
            Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label
            Me.SubReport2 = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.ReportHeader1 = New GrapeCity.ActiveReports.SectionReportModel.ReportHeader
            Me.ReportFooter1 = New GrapeCity.ActiveReports.SectionReportModel.ReportFooter
            CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Label1, Me.Label2, Me.SubReport2})
            Me.Detail1.Height = 2.322917!
            Me.Detail1.Name = "Detail1"
            '
            'Label1
            '
            Me.Label1.Border.BottomColor = System.Drawing.Color.Black
            Me.Label1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Label1.Border.LeftColor = System.Drawing.Color.Black
            Me.Label1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Label1.Border.RightColor = System.Drawing.Color.Black
            Me.Label1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Label1.Border.TopColor = System.Drawing.Color.Black
            Me.Label1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Label1.Height = 0.46875!
            Me.Label1.HyperLink = Nothing
            Me.Label1.Left = 0.34375!
            Me.Label1.Name = "Label1"
            Me.Label1.Style = "text-decoration: none; text-align: center; font-size: 20pt; "
            Me.Label1.Text = "Form on Change of Particulars in eHealth Account"
            Me.Label1.Top = 0.0625!
            Me.Label1.Width = 6.28125!
            '
            'Label2
            '
            Me.Label2.Border.BottomColor = System.Drawing.Color.Black
            Me.Label2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Label2.Border.LeftColor = System.Drawing.Color.Black
            Me.Label2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Label2.Border.RightColor = System.Drawing.Color.Black
            Me.Label2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Label2.Border.TopColor = System.Drawing.Color.Black
            Me.Label2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.Label2.Height = 0.46875!
            Me.Label2.HyperLink = Nothing
            Me.Label2.Left = 0.34375!
            Me.Label2.Name = "Label2"
            Me.Label2.Style = "text-decoration: none; text-align: center; font-size: 20pt; "
            Me.Label2.Text = "[To be provided by DH]"
            Me.Label2.Top = 0.5625!
            Me.Label2.Width = 6.28125!
            '
            'SubReport2
            '
            Me.SubReport2.Border.BottomColor = System.Drawing.Color.Black
            Me.SubReport2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.SubReport2.Border.LeftColor = System.Drawing.Color.Black
            Me.SubReport2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.SubReport2.Border.RightColor = System.Drawing.Color.Black
            Me.SubReport2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.SubReport2.Border.TopColor = System.Drawing.Color.Black
            Me.SubReport2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.SubReport2.CloseBorder = False
            Me.SubReport2.Height = 1.0!
            Me.SubReport2.Left = 0.03125!
            Me.SubReport2.Name = "SubReport2"
            Me.SubReport2.Report = Nothing
            Me.SubReport2.ReportName = "SubReport2"
            Me.SubReport2.Top = 1.09375!
            Me.SubReport2.Width = 6.96875!
            '
            'ReportHeader1
            '
            Me.ReportHeader1.Height = 0.25!
            Me.ReportHeader1.Name = "ReportHeader1"
            '
            'ReportFooter1
            '
            Me.ReportFooter1.Height = 0.25!
            Me.ReportFooter1.Name = "ReportFooter1"
            '
            'SmartIDChangePersonalPaticularForm
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.03125!
            Me.Sections.Add(Me.ReportHeader1)
            Me.Sections.Add(Me.Detail1)
            Me.Sections.Add(Me.ReportFooter1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents ReportHeader1 As GrapeCity.ActiveReports.SectionReportModel.ReportHeader
        Friend WithEvents ReportFooter1 As GrapeCity.ActiveReports.SectionReportModel.ReportFooter
        Friend WithEvents Label1 As GrapeCity.ActiveReports.SectionReportModel.Label
        Friend WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
        Friend WithEvents SubReport1 As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents SubReport2 As GrapeCity.ActiveReports.SectionReportModel.SubReport
    End Class
End Namespace