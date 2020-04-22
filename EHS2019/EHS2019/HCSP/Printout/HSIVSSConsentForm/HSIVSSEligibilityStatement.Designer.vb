Namespace PrintOut.HSIVSSConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HSIVSSEligibility
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HSIVSSEligibility))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtDeclarationTitle = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtDeclaration = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.srEligibilityRole = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.chkRole = New GrapeCity.ActiveReports.SectionReportModel.CheckBox
            Me.txtDeclaration1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.txtDeclarationTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclaration, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.chkRole, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclaration1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtDeclarationTitle, Me.txtDeclaration, Me.srEligibilityRole, Me.chkRole, Me.txtDeclaration1})
            Me.Detail.Height = 0.5625!
            Me.Detail.Name = "Detail"
            '
            'txtDeclarationTitle
            '
            Me.txtDeclarationTitle.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclarationTitle.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationTitle.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclarationTitle.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationTitle.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclarationTitle.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationTitle.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclarationTitle.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeclarationTitle.Height = 0.1875!
            Me.txtDeclarationTitle.Left = 0.0!
            Me.txtDeclarationTitle.Name = "txtDeclarationTitle"
            Me.txtDeclarationTitle.Style = "ddo-char-set: 1; text-align: left; font-weight: bold; font-style: normal; font-si" & _
                "ze: 10pt; "
            Me.txtDeclarationTitle.Text = "Eligibility statement"
            Me.txtDeclarationTitle.Top = 0.0!
            Me.txtDeclarationTitle.Width = 7.375!
            '
            'txtDeclaration
            '
            Me.txtDeclaration.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclaration.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeclaration.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclaration.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeclaration.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclaration.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeclaration.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclaration.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeclaration.Height = 0.15625!
            Me.txtDeclaration.Left = 0.21875!
            Me.txtDeclaration.MultiLine = False
            Me.txtDeclaration.Name = "txtDeclaration"
            Me.txtDeclaration.Style = "ddo-char-set: 1; text-align: justify; font-style: normal; font-size: 10pt; white-" & _
                "space: nowrap; "
            Me.txtDeclaration.Text = "I confirm that *I am/my child is a Hong Kong resident and that:"
            Me.txtDeclaration.Top = 0.21875!
            Me.txtDeclaration.Width = 7.15625!
            '
            'srEligibilityRole
            '
            Me.srEligibilityRole.Border.BottomColor = System.Drawing.Color.Black
            Me.srEligibilityRole.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srEligibilityRole.Border.LeftColor = System.Drawing.Color.Black
            Me.srEligibilityRole.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srEligibilityRole.Border.RightColor = System.Drawing.Color.Black
            Me.srEligibilityRole.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srEligibilityRole.Border.TopColor = System.Drawing.Color.Black
            Me.srEligibilityRole.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srEligibilityRole.CloseBorder = False
            Me.srEligibilityRole.Height = 0.1875!
            Me.srEligibilityRole.Left = 0.6875!
            Me.srEligibilityRole.Name = "srEligibilityRole"
            Me.srEligibilityRole.Report = Nothing
            Me.srEligibilityRole.ReportName = "SubReport1"
            Me.srEligibilityRole.Top = 0.375!
            Me.srEligibilityRole.Width = 6.6875!
            '
            'chkRole
            '
            Me.chkRole.Border.BottomColor = System.Drawing.Color.Black
            Me.chkRole.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.chkRole.Border.LeftColor = System.Drawing.Color.Black
            Me.chkRole.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.chkRole.Border.RightColor = System.Drawing.Color.Black
            Me.chkRole.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.chkRole.Border.TopColor = System.Drawing.Color.Black
            Me.chkRole.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.chkRole.CheckAlignment = System.Drawing.ContentAlignment.MiddleRight
            Me.chkRole.Checked = True
            Me.chkRole.Height = 0.1875!
            Me.chkRole.Left = 0.5!
            Me.chkRole.Name = "chkRole"
            Me.chkRole.Style = ""
            Me.chkRole.Text = ""
            Me.chkRole.Top = 0.375!
            Me.chkRole.Width = 0.1875!
            '
            'txtDeclaration1
            '
            Me.txtDeclaration1.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDeclaration1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeclaration1.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDeclaration1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeclaration1.Border.RightColor = System.Drawing.Color.Black
            Me.txtDeclaration1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeclaration1.Border.TopColor = System.Drawing.Color.Black
            Me.txtDeclaration1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDeclaration1.Height = 0.156!
            Me.txtDeclaration1.Left = 0.0!
            Me.txtDeclaration1.Name = "txtDeclaration1"
            Me.txtDeclaration1.Style = "text-align: left; font-size: 10pt; "
            Me.txtDeclaration1.Text = "1."
            Me.txtDeclaration1.Top = 0.21875!
            Me.txtDeclaration1.Width = 0.21875!
            '
            'HSIVSSEligibility
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.4!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtDeclarationTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclaration, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.chkRole, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclaration1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtDeclarationTitle As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeclaration As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents srEligibilityRole As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents chkRole As GrapeCity.ActiveReports.SectionReportModel.CheckBox
        Friend WithEvents txtDeclaration1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace