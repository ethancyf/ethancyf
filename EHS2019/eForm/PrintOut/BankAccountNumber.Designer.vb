<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class BankAccountNumber 
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
    Private WithEvents detBankAccountNumber As GrapeCity.ActiveReports.SectionReportModel.Detail
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(BankAccountNumber))
        Me.detBankAccountNumber = New GrapeCity.ActiveReports.SectionReportModel.Detail
        Me.txtBankCodeText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
        Me.txtBarchCodeText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
        Me.txtAccountNoText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
        CType(Me.txtBankCodeText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtBarchCodeText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtAccountNoText, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'detBankAccountNumber
        '
        Me.detBankAccountNumber.ColumnSpacing = 0.0!
        Me.detBankAccountNumber.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtBankCodeText, Me.txtBarchCodeText, Me.txtAccountNoText})
        Me.detBankAccountNumber.Height = 0.2708333!
        Me.detBankAccountNumber.Name = "detBankAccountNumber"
        '
        'txtBankCodeText
        '
        Me.txtBankCodeText.Border.BottomColor = System.Drawing.Color.Black
        Me.txtBankCodeText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
        Me.txtBankCodeText.Border.LeftColor = System.Drawing.Color.Black
        Me.txtBankCodeText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
        Me.txtBankCodeText.Border.RightColor = System.Drawing.Color.Black
        Me.txtBankCodeText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
        Me.txtBankCodeText.Border.TopColor = System.Drawing.Color.Black
        Me.txtBankCodeText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
        Me.txtBankCodeText.Height = 0.25!
        Me.txtBankCodeText.HyperLink = Nothing
        Me.txtBankCodeText.Left = 0.0!
        Me.txtBankCodeText.Name = "txtBankCodeText"
        Me.txtBankCodeText.Style = "ddo-char-set: 1; text-decoration: none; text-align: left; font-weight: normal; fo" & _
            "nt-size: 12pt; font-family: Arial; "
        Me.txtBankCodeText.Text = "Bank Code"
        Me.txtBankCodeText.Top = 0.0!
        Me.txtBankCodeText.Width = 0.875!
        '
        'txtBarchCodeText
        '
        Me.txtBarchCodeText.Border.BottomColor = System.Drawing.Color.Black
        Me.txtBarchCodeText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
        Me.txtBarchCodeText.Border.LeftColor = System.Drawing.Color.Black
        Me.txtBarchCodeText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
        Me.txtBarchCodeText.Border.RightColor = System.Drawing.Color.Black
        Me.txtBarchCodeText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
        Me.txtBarchCodeText.Border.TopColor = System.Drawing.Color.Black
        Me.txtBarchCodeText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
        Me.txtBarchCodeText.Height = 0.25!
        Me.txtBarchCodeText.HyperLink = Nothing
        Me.txtBarchCodeText.Left = 1.3125!
        Me.txtBarchCodeText.Name = "txtBarchCodeText"
        Me.txtBarchCodeText.Style = "text-decoration: none; ddo-char-set: 1; text-align: left; font-weight: normal; fo" & _
            "nt-size: 12pt; font-family: Arial; "
        Me.txtBarchCodeText.Text = "Branch Code "
        Me.txtBarchCodeText.Top = 0.0!
        Me.txtBarchCodeText.Width = 1.0625!
        '
        'txtAccountNoText
        '
        Me.txtAccountNoText.Border.BottomColor = System.Drawing.Color.Black
        Me.txtAccountNoText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
        Me.txtAccountNoText.Border.LeftColor = System.Drawing.Color.Black
        Me.txtAccountNoText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
        Me.txtAccountNoText.Border.RightColor = System.Drawing.Color.Black
        Me.txtAccountNoText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
        Me.txtAccountNoText.Border.TopColor = System.Drawing.Color.Black
        Me.txtAccountNoText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
        Me.txtAccountNoText.Height = 0.25!
        Me.txtAccountNoText.HyperLink = Nothing
        Me.txtAccountNoText.Left = 3.4375!
        Me.txtAccountNoText.Name = "txtAccountNoText"
        Me.txtAccountNoText.Style = "ddo-char-set: 1; text-decoration: none; text-align: left; font-weight: normal; fo" & _
            "nt-size: 12pt; font-family: Arial; "
        Me.txtAccountNoText.Text = "Account No."
        Me.txtAccountNoText.Top = 0.0!
        Me.txtAccountNoText.Width = 0.9375!
        '
        'BankAccountNumber
        '
        Me.MasterReport = False
        Me.PageSettings.PaperHeight = 11.69!
        Me.PageSettings.PaperWidth = 8.27!
        Me.Sections.Add(Me.detBankAccountNumber)
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                    "l; font-size: 10pt; color: Black; ", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                    "lic; ", "Heading2", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
        CType(Me.txtBankCodeText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtBarchCodeText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtAccountNoText, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents txtBankCodeText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents txtBarchCodeText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    Friend WithEvents txtAccountNoText As GrapeCity.ActiveReports.SectionReportModel.TextBox
End Class 
