Namespace PrintOut.VSSConsentForm_CHI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSSignature_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSSignature_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox14 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRecipientSignature = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRecipientTelNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRecipientDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox20 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox24 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox25 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox26 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox27 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox29 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox30 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox31 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox15 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.srSignatureGuardian = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.srIdentityDocument = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srPersonalInfo = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientSignature, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientTelNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox20, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox24, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox25, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox26, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox27, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox29, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox30, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox31, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox7, Me.TextBox14, Me.txtRecipientSignature, Me.txtRecipientTelNumber, Me.txtRecipientDate, Me.TextBox20, Me.TextBox24, Me.TextBox25, Me.TextBox26, Me.TextBox27, Me.TextBox29, Me.TextBox30, Me.TextBox31, Me.TextBox15, Me.srSignatureGuardian, Me.TextBox1, Me.TextBox2, Me.TextBox3, Me.TextBox4, Me.TextBox5, Me.srIdentityDocument, Me.srPersonalInfo})
            Me.Detail.Height = 2.729582!
            Me.Detail.Name = "Detail"
            '
            'TextBox7
            '
            Me.TextBox7.Height = 0.1875!
            Me.TextBox7.Left = 0.0!
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: right; " & _
        "ddo-char-set: 0"
            Me.TextBox7.Text = "服務使用者簽署(如不會讀寫，請印上指模)："
            Me.TextBox7.Top = 0.0!
            Me.TextBox7.Width = 3.4!
            '
            'TextBox14
            '
            Me.TextBox14.Height = 0.19!
            Me.TextBox14.Left = 0.0!
            Me.TextBox14.Name = "TextBox14"
            Me.TextBox14.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: right; " & _
        "ddo-char-set: 0"
            Me.TextBox14.Text = "聯絡電話號碼："
            Me.TextBox14.Top = 0.7960001!
            Me.TextBox14.Width = 3.4!
            '
            'txtRecipientSignature
            '
            Me.txtRecipientSignature.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientSignature.CanGrow = False
            Me.txtRecipientSignature.Height = 0.1875!
            Me.txtRecipientSignature.Left = 3.427559!
            Me.txtRecipientSignature.Name = "txtRecipientSignature"
            Me.txtRecipientSignature.Style = "font-family: HA_MingLiu; font-size: 11.25pt; text-align: left; ddo-char-set: 0"
            Me.txtRecipientSignature.Text = "　"
            Me.txtRecipientSignature.Top = 0.0!
            Me.txtRecipientSignature.Width = 3.947441!
            '
            'txtRecipientTelNumber
            '
            Me.txtRecipientTelNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientTelNumber.CanGrow = False
            Me.txtRecipientTelNumber.Height = 0.1875!
            Me.txtRecipientTelNumber.Left = 3.427559!
            Me.txtRecipientTelNumber.Name = "txtRecipientTelNumber"
            Me.txtRecipientTelNumber.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; ddo-char-set: 0"
            Me.txtRecipientTelNumber.Text = "　"
            Me.txtRecipientTelNumber.Top = 0.7960001!
            Me.txtRecipientTelNumber.Width = 1.337008!
            '
            'txtRecipientDate
            '
            Me.txtRecipientDate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtRecipientDate.CanGrow = False
            Me.txtRecipientDate.Height = 0.1875!
            Me.txtRecipientDate.Left = 6.158!
            Me.txtRecipientDate.Name = "txtRecipientDate"
            Me.txtRecipientDate.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; text-decoration: unde" & _
        "rline; ddo-char-set: 0"
            Me.txtRecipientDate.Text = Nothing
            Me.txtRecipientDate.Top = 0.7960001!
            Me.txtRecipientDate.Width = 1.217001!
            '
            'TextBox20
            '
            Me.TextBox20.Height = 0.1875!
            Me.TextBox20.Left = 0.0!
            Me.TextBox20.Name = "TextBox20"
            Me.TextBox20.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: left; t" & _
        "ext-decoration: underline; ddo-char-set: 0"
            Me.TextBox20.Text = "如服務使用者精神上有行為能力但不會讀寫，才須填寫此欄"
            Me.TextBox20.Top = 1.103!
            Me.TextBox20.Width = 7.375!
            '
            'TextBox24
            '
            Me.TextBox24.Height = 0.396!
            Me.TextBox24.Left = 0.0!
            Me.TextBox24.Name = "TextBox24"
            Me.TextBox24.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: left; t" & _
        "ext-decoration: none; ddo-char-set: 0"
            Me.TextBox24.Text = "本人見證此同意書已在服務使用者面前朗讀及解釋，服務使用者完全理解此同意書中服務使用者的義務和責任。"
            Me.TextBox24.Top = 1.2905!
            Me.TextBox24.Width = 7.375!
            '
            'TextBox25
            '
            Me.TextBox25.Height = 0.19!
            Me.TextBox25.Left = 0.0!
            Me.TextBox25.Name = "TextBox25"
            Me.TextBox25.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: right; " & _
        "ddo-char-set: 0"
            Me.TextBox25.Text = "見證人簽署："
            Me.TextBox25.Top = 1.635!
            Me.TextBox25.Width = 1.86425!
            '
            'TextBox26
            '
            Me.TextBox26.Height = 0.19!
            Me.TextBox26.Left = 0.0!
            Me.TextBox26.Name = "TextBox26"
            Me.TextBox26.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: right; " & _
        "ddo-char-set: 0"
            Me.TextBox26.Text = "見證人姓名(英文)："
            Me.TextBox26.Top = 1.917!
            Me.TextBox26.Width = 1.86425!
            '
            'TextBox27
            '
            Me.TextBox27.Height = 0.19!
            Me.TextBox27.Left = 4.031!
            Me.TextBox27.Name = "TextBox27"
            Me.TextBox27.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: right; " & _
        "ddo-char-set: 0"
            Me.TextBox27.Text = "香港身份證號碼："
            Me.TextBox27.Top = 1.632!
            Me.TextBox27.Width = 1.49925!
            '
            'TextBox29
            '
            Me.TextBox29.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.TextBox29.CanGrow = False
            Me.TextBox29.Height = 0.1875!
            Me.TextBox29.Left = 1.8955!
            Me.TextBox29.Name = "TextBox29"
            Me.TextBox29.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: center; ddo-char-set: 0"
            Me.TextBox29.Text = "　"
            Me.TextBox29.Top = 1.635!
            Me.TextBox29.Width = 1.7705!
            '
            'TextBox30
            '
            Me.TextBox30.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.TextBox30.CanGrow = False
            Me.TextBox30.Height = 0.1875!
            Me.TextBox30.Left = 1.8955!
            Me.TextBox30.Name = "TextBox30"
            Me.TextBox30.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: center; ddo-char-set: 0"
            Me.TextBox30.Text = "　"
            Me.TextBox30.Top = 1.917!
            Me.TextBox30.Width = 1.7705!
            '
            'TextBox31
            '
            Me.TextBox31.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.TextBox31.CanGrow = False
            Me.TextBox31.Height = 0.188!
            Me.TextBox31.Left = 5.53!
            Me.TextBox31.Name = "TextBox31"
            Me.TextBox31.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: center; ddo-char-set: 0"
            Me.TextBox31.Text = "　"
            Me.TextBox31.Top = 1.637!
            Me.TextBox31.Width = 1.845!
            '
            'TextBox15
            '
            Me.TextBox15.Height = 0.1875!
            Me.TextBox15.Left = 5.595!
            Me.TextBox15.Name = "TextBox15"
            Me.TextBox15.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: right; " & _
        "ddo-char-set: 0"
            Me.TextBox15.Text = "日期："
            Me.TextBox15.Top = 0.7960001!
            Me.TextBox15.Width = 0.55!
            '
            'srSignatureGuardian
            '
            Me.srSignatureGuardian.CloseBorder = False
            Me.srSignatureGuardian.Height = 0.21875!
            Me.srSignatureGuardian.Left = 0.0!
            Me.srSignatureGuardian.Name = "srSignatureGuardian"
            Me.srSignatureGuardian.Report = Nothing
            Me.srSignatureGuardian.ReportName = "srSignatureGuardian"
            Me.srSignatureGuardian.Top = 2.477!
            Me.srSignatureGuardian.Width = 7.438!
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.19!
            Me.TextBox1.Left = 4.063!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-family: HA_MingLiu; font-size: 11.25pt; font-style: normal; text-align: righ" & _
        "t; ddo-char-set: 0"
            Me.TextBox1.Text = "（只要英文字母及頭3個數字）"
            Me.TextBox1.Top = 1.855!
            Me.TextBox1.Width = 3.312!
            '
            'TextBox2
            '
            Me.TextBox2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.TextBox2.CanGrow = False
            Me.TextBox2.Height = 0.1875!
            Me.TextBox2.Left = 5.53!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; text-decoration: unde" & _
        "rline; ddo-char-set: 0"
            Me.TextBox2.Text = "　"
            Me.TextBox2.Top = 2.197!
            Me.TextBox2.Width = 1.845!
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.1875!
            Me.TextBox3.Left = 4.989!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: right; " & _
        "ddo-char-set: 0"
            Me.TextBox3.Text = "日期："
            Me.TextBox3.Top = 2.197!
            Me.TextBox3.Width = 0.55!
            '
            'TextBox4
            '
            Me.TextBox4.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.TextBox4.CanGrow = False
            Me.TextBox4.Height = 0.1875!
            Me.TextBox4.Left = 1.895!
            Me.TextBox4.Name = "TextBox4"
            Me.TextBox4.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; ddo-char-set: 0"
            Me.TextBox4.Text = "　"
            Me.TextBox4.Top = 2.197!
            Me.TextBox4.Width = 1.771!
            '
            'TextBox5
            '
            Me.TextBox5.Height = 0.19!
            Me.TextBox5.Left = 0.0000003576279!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: right; " & _
        "ddo-char-set: 0"
            Me.TextBox5.Text = "聯絡電話號碼："
            Me.TextBox5.Top = 2.197!
            Me.TextBox5.Width = 1.86375!
            '
            'srIdentityDocument
            '
            Me.srIdentityDocument.CloseBorder = False
            Me.srIdentityDocument.Height = 0.219!
            'Me.srIdentityDocument.Left = 1.775!
            Me.srIdentityDocument.Left = 1.696!
            Me.srIdentityDocument.Name = "srIdentityDocument"
            Me.srIdentityDocument.Report = Nothing
            Me.srIdentityDocument.ReportName = "srIdentityDocument"
            Me.srIdentityDocument.Top = 0.537!
            Me.srIdentityDocument.Width = 5.6!
            '
            'srPersonalInfo
            '
            Me.srPersonalInfo.CloseBorder = False
            Me.srPersonalInfo.Height = 0.219!
            Me.srPersonalInfo.Left = 1.15!
            Me.srPersonalInfo.Name = "srPersonalInfo"
            Me.srPersonalInfo.Report = Nothing
            Me.srPersonalInfo.ReportName = "srPersonalInfo"
            Me.srPersonalInfo.Top = 0.277!
            Me.srPersonalInfo.Width = 5.6!
            '
            'VSSSignature_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.438!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox14, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientSignature, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientTelNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox20, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox24, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox25, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox26, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox27, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox29, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox30, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox31, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox14 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtRecipientSignature As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtRecipientTelNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtRecipientDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox20 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox24 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox25 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox26 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox27 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox29 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox30 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox31 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox15 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents srSignatureGuardian As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents srIdentityDocument As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents srPersonalInfo As GrapeCity.ActiveReports.SectionReportModel.SubReport
    End Class

End Namespace