Namespace PrintOut.DH_VSS

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class PracticeBatch
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
        Private WithEvents dtlPracticeBatch As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PracticeBatch))
            Me.dtlPracticeBatch = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.lblPartition = New GrapeCity.ActiveReports.SectionReportModel.Label
            CType(Me.lblPartition, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'dtlPracticeBatch
            '
            Me.dtlPracticeBatch.ColumnSpacing = 0.0!
            Me.dtlPracticeBatch.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.lblPartition})
            Me.dtlPracticeBatch.Height = 0.1458333!
            Me.dtlPracticeBatch.Name = "dtlPracticeBatch"
            '
            'lblPartition
            '
            Me.lblPartition.Border.BottomColor = System.Drawing.Color.Black
            Me.lblPartition.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblPartition.Border.LeftColor = System.Drawing.Color.Black
            Me.lblPartition.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblPartition.Border.RightColor = System.Drawing.Color.Black
            Me.lblPartition.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblPartition.Border.TopColor = System.Drawing.Color.Black
            Me.lblPartition.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.lblPartition.Height = 0.125!
            Me.lblPartition.HyperLink = Nothing
            Me.lblPartition.Left = 0.0!
            Me.lblPartition.Name = "lblPartition"
            Me.lblPartition.Style = ""
            Me.lblPartition.Text = ""
            Me.lblPartition.Top = 0.0!
            Me.lblPartition.Width = 7.15625!
            '
            'PracticeBatch
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.198!
            Me.Sections.Add(Me.dtlPracticeBatch)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.lblPartition, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents lblPartition As GrapeCity.ActiveReports.SectionReportModel.Label
    End Class

End Namespace