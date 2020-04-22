Namespace PrintOut.DH_VSS
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class ProfessionalBatch
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
        Private WithEvents professionalType As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ProfessionalBatch))
            Me.professionalType = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.sreProfessional = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'professionalType
            '
            Me.professionalType.ColumnSpacing = 0.0!
            Me.professionalType.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.sreProfessional})
            Me.professionalType.Height = 0.2916667!
            Me.professionalType.Name = "professionalType"
            '
            'sreProfessional
            '
            Me.sreProfessional.Border.BottomColor = System.Drawing.Color.Black
            Me.sreProfessional.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.sreProfessional.Border.LeftColor = System.Drawing.Color.Black
            Me.sreProfessional.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.sreProfessional.Border.RightColor = System.Drawing.Color.Black
            Me.sreProfessional.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.sreProfessional.Border.TopColor = System.Drawing.Color.Black
            Me.sreProfessional.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.sreProfessional.CloseBorder = False
            Me.sreProfessional.Height = 0.25!
            Me.sreProfessional.Left = 0.0!
            Me.sreProfessional.Name = "sreProfessional"
            Me.sreProfessional.Report = Nothing
            Me.sreProfessional.ReportName = "SubReport1"
            Me.sreProfessional.Top = 0.0!
            Me.sreProfessional.Width = 7.125!
            '
            'ProfessionalBatch
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.15625!
            Me.Sections.Add(Me.professionalType)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents sreProfessional As GrapeCity.ActiveReports.SectionReportModel.SubReport
    End Class
End Namespace
