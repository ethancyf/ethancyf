Namespace PrintOut.HSIVSSConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HSIVSSEligibilityRole
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HSIVSSEligibilityRole))
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtDescription = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.txtDescription, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtDescription})
            Me.Detail1.Height = 0.16!
            Me.Detail1.Name = "Detail1"
            '
            'txtDescription
            '
            Me.txtDescription.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDescription.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDescription.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDescription.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDescription.Border.RightColor = System.Drawing.Color.Black
            Me.txtDescription.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDescription.Border.TopColor = System.Drawing.Color.Black
            Me.txtDescription.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDescription.Height = 0.15625!
            Me.txtDescription.Left = 0.0!
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Style = "ddo-char-set: 1; text-align: justify; font-style: normal; font-size: 10pt; white-" & _
                "space: inherit; "
            Me.txtDescription.Text = Nothing
            Me.txtDescription.Top = 0.0!
            Me.txtDescription.Width = 6.688!
            '
            'HSIVSSEligibilityRoleStudent
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.688!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtDescription, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtDescription As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class


End Namespace