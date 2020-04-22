
Namespace PrintOut.DH_VSS_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class AddressOfPracticeBatch
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(AddressOfPracticeBatch))
            Me.dtlAddressOfPractice = New GrapeCity.ActiveReports.SectionReportModel.Detail
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'dtlAddressOfPractice
            '
            Me.dtlAddressOfPractice.ColumnSpacing = 0.0!
            Me.dtlAddressOfPractice.Height = 0.0!
            Me.dtlAddressOfPractice.Name = "dtlAddressOfPractice"
            '
            'AddressOfPracticeBatch
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.146!
            Me.Sections.Add(Me.dtlAddressOfPractice)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
    End Class

End Namespace