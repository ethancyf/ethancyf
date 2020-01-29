Namespace PrintOut.DH_VSS_CHI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class ServiceFeeRemark
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ServiceFeeRemark))
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtRemark = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.txtRemark, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtRemark})
            Me.Detail1.Height = 0.3020833!
            Me.Detail1.Name = "Detail1"
            '
            'txtRemark
            '
            Me.txtRemark.Border.BottomColor = System.Drawing.Color.Black
            Me.txtRemark.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtRemark.Border.LeftColor = System.Drawing.Color.Black
            Me.txtRemark.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtRemark.Border.RightColor = System.Drawing.Color.Black
            Me.txtRemark.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtRemark.Border.TopColor = System.Drawing.Color.Black
            Me.txtRemark.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtRemark.Height = 0.25!
            Me.txtRemark.HyperLink = Nothing
            Me.txtRemark.Left = 0.0!
            Me.txtRemark.Name = "txtRemark"
            Me.txtRemark.Style = "ddo-char-set: 136; text-decoration: none; text-align: justify; font-weight: norma" & _
                "l; font-size: 11.25pt; font-family: 新細明體; "
            Me.txtRemark.Text = "[txtRemark]"
            Me.txtRemark.Top = 0.0!
            Me.txtRemark.Width = 7.125!
            '
            'ServiceFeeExplanation
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.198!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtRemark, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtRemark As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace
