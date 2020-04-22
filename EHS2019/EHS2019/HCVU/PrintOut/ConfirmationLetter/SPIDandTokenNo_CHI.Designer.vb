
Namespace PrintOut.ConfirmationLetter

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class SPIDandTokenNo_CHI
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
        Private WithEvents dtlSPIDandTokenNo As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SPIDandTokenNo_CHI))
            Me.dtlSPIDandTokenNo = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtTokenNoChi = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtStar = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtTokenNoChiText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtSPNoChiText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtSPNoChi = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.txtTokenNoChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtStar, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTokenNoChiText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPNoChiText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPNoChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'dtlSPIDandTokenNo
            '
            Me.dtlSPIDandTokenNo.ColumnSpacing = 0.0!
            Me.dtlSPIDandTokenNo.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTokenNoChi, Me.txtStar, Me.txtTokenNoChiText, Me.txtSPNoChiText, Me.txtSPNoChi})
            Me.dtlSPIDandTokenNo.Height = 0.4791667!
            Me.dtlSPIDandTokenNo.Name = "dtlSPIDandTokenNo"
            '
            'txtTokenNoChi
            '
            Me.txtTokenNoChi.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTokenNoChi.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoChi.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTokenNoChi.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoChi.Border.RightColor = System.Drawing.Color.Black
            Me.txtTokenNoChi.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoChi.Border.TopColor = System.Drawing.Color.Black
            Me.txtTokenNoChi.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoChi.DataField = "Token_Serial_No"
            Me.txtTokenNoChi.Height = 0.21875!
            Me.txtTokenNoChi.Left = 1.59375!
            Me.txtTokenNoChi.LineSpacing = 1.0!
            Me.txtTokenNoChi.Name = "txtTokenNoChi"
            Me.txtTokenNoChi.Style = "ddo-char-set: 1; font-weight: normal; font-size: 14.25pt; font-family: 新細明體; " & _
                "vertical-align: top; "
            Me.txtTokenNoChi.Text = "不適用"
            Me.txtTokenNoChi.Top = 0.03125!
            Me.txtTokenNoChi.Width = 4.65625!
            '
            'txtStar
            '
            Me.txtStar.Border.BottomColor = System.Drawing.Color.Black
            Me.txtStar.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtStar.Border.LeftColor = System.Drawing.Color.Black
            Me.txtStar.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtStar.Border.RightColor = System.Drawing.Color.Black
            Me.txtStar.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtStar.Border.TopColor = System.Drawing.Color.Black
            Me.txtStar.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtStar.Height = 0.1875!
            Me.txtStar.Left = 2.1875!
            Me.txtStar.Name = "txtStar"
            Me.txtStar.Style = "ddo-char-set: 1; font-weight: normal; font-size: 14.25pt; font-family: 新細明體; " & _
                ""
            Me.txtStar.Text = "*"
            Me.txtStar.Top = 0.03125!
            Me.txtStar.Width = 0.8125!
            '
            'txtTokenNoChiText
            '
            Me.txtTokenNoChiText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTokenNoChiText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoChiText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTokenNoChiText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoChiText.Border.RightColor = System.Drawing.Color.Black
            Me.txtTokenNoChiText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoChiText.Border.TopColor = System.Drawing.Color.Black
            Me.txtTokenNoChiText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoChiText.Height = 0.21875!
            Me.txtTokenNoChiText.Left = 0.0!
            Me.txtTokenNoChiText.LineSpacing = 1.0!
            Me.txtTokenNoChiText.Name = "txtTokenNoChiText"
            Me.txtTokenNoChiText.Style = "ddo-char-set: 1; font-weight: normal; font-size: 14.25pt; font-family: 新細明體; " & _
                "vertical-align: top; "
            Me.txtTokenNoChiText.Text = "保安編碼器編號："
            Me.txtTokenNoChiText.Top = 0.03125!
            Me.txtTokenNoChiText.Width = 1.625!
            '
            'txtSPNoChiText
            '
            Me.txtSPNoChiText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtSPNoChiText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoChiText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtSPNoChiText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoChiText.Border.RightColor = System.Drawing.Color.Black
            Me.txtSPNoChiText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoChiText.Border.TopColor = System.Drawing.Color.Black
            Me.txtSPNoChiText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoChiText.Height = 0.21875!
            Me.txtSPNoChiText.Left = 0.0!
            Me.txtSPNoChiText.LineSpacing = 1.0!
            Me.txtSPNoChiText.Name = "txtSPNoChiText"
            Me.txtSPNoChiText.Style = "ddo-char-set: 1; font-weight: normal; font-size: 14.25pt; font-family: 新細明體; " & _
                "vertical-align: top; "
            Me.txtSPNoChiText.Text = "獲編配的服務提供者編號："
            Me.txtSPNoChiText.Top = 0.25!
            Me.txtSPNoChiText.Width = 2.40625!
            '
            'txtSPNoChi
            '
            Me.txtSPNoChi.Border.BottomColor = System.Drawing.Color.Black
            Me.txtSPNoChi.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoChi.Border.LeftColor = System.Drawing.Color.Black
            Me.txtSPNoChi.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoChi.Border.RightColor = System.Drawing.Color.Black
            Me.txtSPNoChi.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoChi.Border.TopColor = System.Drawing.Color.Black
            Me.txtSPNoChi.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoChi.DataField = "SP_ID"
            Me.txtSPNoChi.Height = 0.21875!
            Me.txtSPNoChi.Left = 2.375!
            Me.txtSPNoChi.Name = "txtSPNoChi"
            Me.txtSPNoChi.Style = "text-decoration: underline; ddo-char-set: 1; text-align: left; font-weight: norma" & _
                "l; font-size: 14.25pt; font-family: 新細明體; vertical-align: top; "
            Me.txtSPNoChi.Text = " <SP_ID>"
            Me.txtSPNoChi.Top = 0.25!
            Me.txtSPNoChi.Width = 3.875!
            '
            'SPIDandTokenNo_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.604167!
            Me.Sections.Add(Me.dtlSPIDandTokenNo)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtTokenNoChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtStar, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTokenNoChiText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPNoChiText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPNoChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtStar As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTokenNoChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTokenNoChiText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPNoChiText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPNoChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace