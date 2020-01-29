Namespace PrintOut.DH_VSS_CHI


    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class AddressOfPractice
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
        Private WithEvents dtlAddressOfPractice As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(AddressOfPractice))
            Me.dtlAddressOfPractice = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtPracticeAddress = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtIndex = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPraticeNameAddressText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPracticeName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtPracticeAddress, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtIndex, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPraticeNameAddressText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPracticeName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'dtlAddressOfPractice
            '
            Me.dtlAddressOfPractice.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtPracticeAddress, Me.txtIndex, Me.txtPraticeNameAddressText, Me.TextBox2, Me.txtPracticeName})
            Me.dtlAddressOfPractice.Height = 0.5208333!
            Me.dtlAddressOfPractice.Name = "dtlAddressOfPractice"
            '
            'txtPracticeAddress
            '
            Me.txtPracticeAddress.Height = 0.25!
            Me.txtPracticeAddress.Left = 2.926!
            Me.txtPracticeAddress.Name = "txtPracticeAddress"
            Me.txtPracticeAddress.Style = "font-family: HA_MingLiu; font-size: 12.75pt; font-weight: normal; text-decoration" & _
        ": underline; text-justify: auto; ddo-char-set: 136"
            Me.txtPracticeAddress.Text = Nothing
            Me.txtPracticeAddress.Top = 0.271!
            Me.txtPracticeAddress.Width = 4.1875!
            '
            'txtIndex
            '
            Me.txtIndex.Height = 0.5!
            Me.txtIndex.Left = 0.0!
            Me.txtIndex.Name = "txtIndex"
            Me.txtIndex.Style = "font-family: 新細明體; font-size: 12.75pt; ddo-char-set: 136"
            Me.txtIndex.Text = Nothing
            Me.txtIndex.Top = 0.0!
            Me.txtIndex.Width = 0.25!
            '
            'txtPraticeNameAddressText
            '
            Me.txtPraticeNameAddressText.Height = 0.25!
            Me.txtPraticeNameAddressText.Left = 0.25!
            Me.txtPraticeNameAddressText.Name = "txtPraticeNameAddressText"
            Me.txtPraticeNameAddressText.Style = "font-family: 新細明體; font-size: 12.75pt; font-weight: normal; ddo-char-set: 136"
            Me.txtPraticeNameAddressText.Text = "執業地點（1）名稱及地址："
            Me.txtPraticeNameAddressText.Top = 0.0!
            Me.txtPraticeNameAddressText.Width = 2.241!
            '
            'TextBox2
            '
            Me.TextBox2.Height = 0.5!
            Me.TextBox2.Left = 2.46875!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "font-family: 新細明體; font-size: 8pt; ddo-char-set: 1"
            Me.TextBox2.Text = "（註B）"
            Me.TextBox2.Top = 0.0!
            Me.TextBox2.Width = 0.434!
            '
            'txtPracticeName
            '
            Me.txtPracticeName.Height = 0.21875!
            Me.txtPracticeName.Left = 2.926!
            Me.txtPracticeName.Name = "txtPracticeName"
            Me.txtPracticeName.Style = "font-family: HA_MingLiu; font-size: 12.75pt; font-weight: normal; text-decoration" & _
        ": underline; text-justify: auto; ddo-char-set: 136"
            Me.txtPracticeName.Text = Nothing
            Me.txtPracticeName.Top = 0.0!
            Me.txtPracticeName.Width = 4.1875!
            '
            'AddressOfPractice
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.125!
            Me.Sections.Add(Me.dtlAddressOfPractice)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtPracticeAddress, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtIndex, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPraticeNameAddressText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPracticeName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtPracticeAddress As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtIndex As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPraticeNameAddressText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPracticeName As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace