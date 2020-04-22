Namespace PrintOut.DH_VSS

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class ServiceFeeExplanation
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ServiceFeeExplanation))
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.TextBox133 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.TextBox133, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox133})
            Me.Detail1.Height = 0.4166667!
            Me.Detail1.Name = "Detail1"
            '
            'TextBox133
            '
            Me.TextBox133.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox133.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox133.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox133.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox133.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox133.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox133.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox133.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox133.Height = 0.375!
            Me.TextBox133.HyperLink = Nothing
            Me.TextBox133.Left = 0.0!
            Me.TextBox133.Name = "TextBox133"
            Me.TextBox133.Style = "text-decoration: none; ddo-char-set: 1; text-align: justify; font-weight: normal;" & _
                " font-size: 10pt; font-family: Arial; "
            Me.TextBox133.Text = "* The service fee is the fee (exclusive of Government  subsidy) for one vaccinati" & _
                "on  charged by the Applicant or the Medical Organization. It shall be a specific" & _
                " fee."
            Me.TextBox133.Top = 0.0!
            Me.TextBox133.Width = 7.125!
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
            CType(Me.TextBox133, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox133 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace
