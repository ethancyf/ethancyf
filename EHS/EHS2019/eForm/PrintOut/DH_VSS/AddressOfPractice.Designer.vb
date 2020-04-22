Namespace PrintOut.DH_VSS


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
            Me.txtIndex = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPracticeName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPracticeAddress = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtIndex, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPracticeName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPracticeAddress, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'dtlAddressOfPractice
            '
            Me.dtlAddressOfPractice.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtIndex, Me.TextBox1, Me.TextBox2, Me.txtPracticeName, Me.txtPracticeAddress})
            Me.dtlAddressOfPractice.Height = 0.46875!
            Me.dtlAddressOfPractice.KeepTogether = True
            Me.dtlAddressOfPractice.Name = "dtlAddressOfPractice"
            '
            'txtIndex
            '
            Me.txtIndex.Height = 0.4375!
            Me.txtIndex.Left = 0.0!
            Me.txtIndex.Name = "txtIndex"
            Me.txtIndex.Text = Nothing
            Me.txtIndex.Top = 0.0!
            Me.txtIndex.Width = 0.25!
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.46875!
            Me.TextBox1.Left = 0.25!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-size: 10pt; font-weight: bold; ddo-char-set: 1"
            Me.TextBox1.Text = "Name and Address of Practice:"
            Me.TextBox1.Top = 0.0!
            Me.TextBox1.Width = 2.09375!
            '
            'TextBox2
            '
            Me.TextBox2.Height = 0.46875!
            Me.TextBox2.Left = 2.34375!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "font-size: 7pt; ddo-char-set: 1"
            Me.TextBox2.Text = "(Note B)"
            Me.TextBox2.Top = 0.0!
            Me.TextBox2.Width = 0.40625!
            '
            'txtPracticeName
            '
            Me.txtPracticeName.Height = 0.1875!
            Me.txtPracticeName.Left = 2.75!
            Me.txtPracticeName.Name = "txtPracticeName"
            Me.txtPracticeName.Style = "font-size: 10pt; font-weight: normal; text-decoration: underline; ddo-char-set: 1" & _
        ""
            Me.txtPracticeName.Text = Nothing
            Me.txtPracticeName.Top = 0.0!
            Me.txtPracticeName.Width = 4.34375!
            '
            'txtPracticeAddress
            '
            Me.txtPracticeAddress.Height = 0.25!
            Me.txtPracticeAddress.Left = 2.75!
            Me.txtPracticeAddress.Name = "txtPracticeAddress"
            Me.txtPracticeAddress.Style = "font-size: 10pt; font-weight: normal; text-decoration: underline; ddo-char-set: 1" & _
        ""
            Me.txtPracticeAddress.Text = Nothing
            Me.txtPracticeAddress.Top = 0.31!
            Me.txtPracticeAddress.Width = 4.34375!
            '
            'AddressOfPractice
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.114583!
            Me.Sections.Add(Me.dtlAddressOfPractice)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtIndex, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPracticeName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPracticeAddress, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtIndex As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPracticeName As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPracticeAddress As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace