Namespace PrintOut.HSIVSSConsentForm_CHI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HSIVSSSignatureAdult_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HSIVSSSignatureAdult_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox14 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox15 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox16 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtContactTelephoneNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSignDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox16, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtContactTelephoneNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSignDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox7, Me.TextBox14, Me.TextBox15, Me.TextBox16, Me.txtContactTelephoneNumber, Me.txtSignDate})
            Me.Detail.Height = 0.36!
            Me.Detail.Name = "Detail"
            '
            'TextBox7
            '
            Me.TextBox7.Height = 0.15625!
            Me.TextBox7.Left = 0.5!
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Style = "font-family: HA_MingLiu; font-size: 10.5pt; font-style: normal; text-align: right" & _
        "; ddo-char-set: 136"
            Me.TextBox7.Text = "服務使用者簽署(如不會讀寫，請印上指模)："
            Me.TextBox7.Top = 0.0!
            Me.TextBox7.Width = 3.53125!
            '
            'TextBox14
            '
            Me.TextBox14.Height = 0.15625!
            Me.TextBox14.Left = 0.5!
            Me.TextBox14.Name = "TextBox14"
            Me.TextBox14.Style = "font-family: HA_MingLiu; font-size: 10.5pt; font-style: normal; text-align: right" & _
        "; ddo-char-set: 136"
            Me.TextBox14.Text = "聯絡電話號碼："
            Me.TextBox14.Top = 0.1875!
            Me.TextBox14.Width = 3.53125!
            '
            'TextBox15
            '
            Me.TextBox15.Height = 0.15625!
            Me.TextBox15.Left = 5.5!
            Me.TextBox15.Name = "TextBox15"
            Me.TextBox15.Style = "font-family: HA_MingLiu; font-size: 10.5pt; font-style: normal; text-align: right" & _
        "; ddo-char-set: 136"
            Me.TextBox15.Text = "日期："
            Me.TextBox15.Top = 0.1875!
            Me.TextBox15.Width = 0.5625!
            '
            'TextBox16
            '
            Me.TextBox16.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.TextBox16.CanGrow = False
            Me.TextBox16.Height = 0.15625!
            Me.TextBox16.Left = 4.0625!
            Me.TextBox16.Name = "TextBox16"
            Me.TextBox16.Style = "font-family: HA_MingLiu; font-size: 10.5pt; text-align: left; ddo-char-set: 136"
            Me.TextBox16.Text = "　"
            Me.TextBox16.Top = 0.0!
            Me.TextBox16.Width = 3.25!
            '
            'txtContactTelephoneNumber
            '
            Me.txtContactTelephoneNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtContactTelephoneNumber.CanGrow = False
            Me.txtContactTelephoneNumber.Height = 0.15625!
            Me.txtContactTelephoneNumber.Left = 4.0625!
            Me.txtContactTelephoneNumber.Name = "txtContactTelephoneNumber"
            Me.txtContactTelephoneNumber.Style = "font-family: HA_MingLiu; font-size: 10.5pt; text-align: left; ddo-char-set: 136"
            Me.txtContactTelephoneNumber.Text = "　"
            Me.txtContactTelephoneNumber.Top = 0.1875!
            Me.txtContactTelephoneNumber.Width = 1.40625!
            '
            'txtSignDate
            '
            Me.txtSignDate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtSignDate.CanGrow = False
            Me.txtSignDate.Height = 0.15625!
            Me.txtSignDate.Left = 6.0625!
            Me.txtSignDate.Name = "txtSignDate"
            Me.txtSignDate.Style = "font-family: HA_MingLiu; font-size: 10.5pt; text-align: left; text-decoration: no" & _
        "ne; ddo-char-set: 136"
            Me.txtSignDate.Text = "　"
            Me.txtSignDate.Top = 0.1875!
            Me.txtSignDate.Width = 1.25!
            '
            'HSIVSSSignatureAdult_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.4!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox16, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtContactTelephoneNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSignDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox14 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox15 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox16 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtContactTelephoneNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSignDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace