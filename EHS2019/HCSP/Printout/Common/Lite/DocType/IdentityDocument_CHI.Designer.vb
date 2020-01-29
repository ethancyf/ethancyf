
Namespace PrintOut.Common.Lite.DocType
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class IdentityDocument_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(IdentityDocument_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.srDocType = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.srDocType})
            Me.Detail.Height = 0.188!
            Me.Detail.Name = "Detail"
            '
            'srDocType
            '
            Me.srDocType.Border.BottomColor = System.Drawing.Color.Black
            Me.srDocType.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDocType.Border.LeftColor = System.Drawing.Color.Black
            Me.srDocType.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDocType.Border.RightColor = System.Drawing.Color.Black
            Me.srDocType.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDocType.Border.TopColor = System.Drawing.Color.Black
            Me.srDocType.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDocType.CloseBorder = False
            Me.srDocType.Height = 0.188!
            Me.srDocType.Left = 0.0!
            Me.srDocType.Name = "srDocType"
            Me.srDocType.Report = Nothing
            Me.srDocType.ReportName = "SubReport1"
            Me.srDocType.Top = 0.0!
            Me.srDocType.Width = 7.125!
            '
            'IdentityDocument_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.125!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents srDocType As GrapeCity.ActiveReports.SectionReportModel.SubReport
    End Class
End Namespace