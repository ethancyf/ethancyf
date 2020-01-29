Namespace PrintOut.CIVSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class CIVSSDeclarationCondensed_CHI
        Inherits DataDynamics.ActiveReports.ActiveReport3

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
            End If
            MyBase.Dispose(disposing)
        End Sub

        'NOTE: The following procedure is required by the ActiveReports Designer
        'It can be modified using the ActiveReports Designer.
        'Do not modify it using the code editor.
        Private WithEvents Detail As DataDynamics.ActiveReports.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CIVSSDeclarationCondensed_CHI))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.srDeclaration = New DataDynamics.ActiveReports.SubReport
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.srDeclaration})
            Me.Detail.Height = 0.2083333!
            Me.Detail.Name = "Detail"
            '
            'srDeclaration
            '
            Me.srDeclaration.Border.BottomColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.Border.LeftColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.Border.RightColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.Border.TopColor = System.Drawing.Color.Black
            Me.srDeclaration.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.srDeclaration.CloseBorder = False
            Me.srDeclaration.Height = 0.21875!
            Me.srDeclaration.Left = 0.0!
            Me.srDeclaration.Name = "srDeclaration"
            Me.srDeclaration.Report = Nothing
            Me.srDeclaration.ReportName = "SubReport1"
            Me.srDeclaration.Top = 0.0!
            Me.srDeclaration.Width = 7.4!
            '
            'CIVSSDeclarationCondensed_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.375!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents srDeclaration As DataDynamics.ActiveReports.SubReport
    End Class
End Namespace
