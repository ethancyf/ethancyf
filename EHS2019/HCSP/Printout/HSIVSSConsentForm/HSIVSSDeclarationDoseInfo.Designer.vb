Namespace PrintOut.HSIVSSConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HSIVSSDeclarationDoseInfo
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HSIVSSDeclarationDoseInfo))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtDescription = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.chkSubsidizeItem = New GrapeCity.ActiveReports.SectionReportModel.CheckBox
            CType(Me.txtDescription, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.chkSubsidizeItem, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtDescription, Me.chkSubsidizeItem})
            Me.Detail.Height = 0.365!
            Me.Detail.Name = "Detail"
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
            Me.txtDescription.MultiLine = False
            Me.txtDescription.Name = "txtDescription"
            Me.txtDescription.Style = "ddo-char-set: 0; text-align: justify; font-size: 10pt; white-space: nowrap; "
            Me.txtDescription.Text = "(For children under 9 years of age) This is:"
            Me.txtDescription.Top = 0.0!
            Me.txtDescription.Width = 7.15625!
            '
            'chkSubsidizeItem
            '
            Me.chkSubsidizeItem.Border.BottomColor = System.Drawing.Color.Black
            Me.chkSubsidizeItem.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.chkSubsidizeItem.Border.LeftColor = System.Drawing.Color.Black
            Me.chkSubsidizeItem.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.chkSubsidizeItem.Border.RightColor = System.Drawing.Color.Black
            Me.chkSubsidizeItem.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.chkSubsidizeItem.Border.TopColor = System.Drawing.Color.Black
            Me.chkSubsidizeItem.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.chkSubsidizeItem.Checked = True
            Me.chkSubsidizeItem.Height = 0.1875!
            Me.chkSubsidizeItem.Left = 0.28125!
            Me.chkSubsidizeItem.Name = "chkSubsidizeItem"
            Me.chkSubsidizeItem.Style = "ddo-char-set: 0; font-size: 10pt; "
            Me.chkSubsidizeItem.Tag = "Template"
            Me.chkSubsidizeItem.Text = ""
            Me.chkSubsidizeItem.Top = 0.15625!
            Me.chkSubsidizeItem.Width = 6.875!
            '
            'HSIVSSDeclarationDoseInfo
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.2!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtDescription, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.chkSubsidizeItem, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtDescription As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents chkSubsidizeItem As GrapeCity.ActiveReports.SectionReportModel.CheckBox
    End Class

End Namespace