Namespace PrintOut.Common
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSSubsidyInfo
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSSubsidyInfo))
            Me.Detail = New DataDynamics.ActiveReports.Detail
            Me.chkSubsidizeItemTemplate = New DataDynamics.ActiveReports.CheckBox
            Me.picTick1 = New DataDynamics.ActiveReports.TextBox
            CType(Me.chkSubsidizeItemTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picTick1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New DataDynamics.ActiveReports.ARControl() {Me.chkSubsidizeItemTemplate, Me.picTick1})
            Me.Detail.Height = 0.219!
            Me.Detail.Name = "Detail"
            '
            'chkSubsidizeItemTemplate
            '
            Me.chkSubsidizeItemTemplate.Border.BottomColor = System.Drawing.Color.Black
            Me.chkSubsidizeItemTemplate.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.chkSubsidizeItemTemplate.Border.LeftColor = System.Drawing.Color.Black
            Me.chkSubsidizeItemTemplate.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.chkSubsidizeItemTemplate.Border.RightColor = System.Drawing.Color.Black
            Me.chkSubsidizeItemTemplate.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.chkSubsidizeItemTemplate.Border.TopColor = System.Drawing.Color.Black
            Me.chkSubsidizeItemTemplate.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.chkSubsidizeItemTemplate.Height = 0.21875!
            Me.chkSubsidizeItemTemplate.Left = 0.28125!
            Me.chkSubsidizeItemTemplate.Name = "chkSubsidizeItemTemplate"
            Me.chkSubsidizeItemTemplate.Style = "ddo-char-set: 0; font-size: 11.25pt; "
            Me.chkSubsidizeItemTemplate.Tag = "Template"
            Me.chkSubsidizeItemTemplate.Text = ""
            Me.chkSubsidizeItemTemplate.Top = 0.0!
            Me.chkSubsidizeItemTemplate.Visible = False
            Me.chkSubsidizeItemTemplate.Width = 6.46875!
            '
            'picTick1
            '
            Me.picTick1.Border.BottomColor = System.Drawing.Color.Black
            Me.picTick1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.picTick1.Border.LeftColor = System.Drawing.Color.Black
            Me.picTick1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.picTick1.Border.RightColor = System.Drawing.Color.Black
            Me.picTick1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.picTick1.Border.TopColor = System.Drawing.Color.Black
            Me.picTick1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None
            Me.picTick1.Height = 0.1875!
            Me.picTick1.Left = 0.0!
            Me.picTick1.Name = "picTick1"
            Me.picTick1.Style = "ddo-char-set: 0; font-size: 14pt; font-family: Wingdings 2; "
            Me.picTick1.Text = "P"
            Me.picTick1.Top = 0.0!
            Me.picTick1.Width = 0.15625!
            '
            'VSSSubsidyInfo
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.0!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.chkSubsidizeItemTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picTick1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents chkSubsidizeItemTemplate As DataDynamics.ActiveReports.CheckBox
        Friend WithEvents picTick1 As DataDynamics.ActiveReports.TextBox
    End Class
End Namespace