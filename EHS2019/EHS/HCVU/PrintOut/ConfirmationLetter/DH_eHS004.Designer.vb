Namespace PrintOut.ConfirmationLetter
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class DH_eHS004
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
        Private WithEvents PageHeader1 As GrapeCity.ActiveReports.SectionReportModel.PageHeader
        Private WithEvents Detail1 As GrapeCity.ActiveReports.SectionReportModel.Detail
        Private WithEvents PageFooter1 As GrapeCity.ActiveReports.SectionReportModel.PageFooter
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DH_eHS004))
            Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
            Me.TextBox48 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox56 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox57 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox58 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.Picture1 = New GrapeCity.ActiveReports.SectionReportModel.Picture()
            Me.TextBox59 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox61 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox69 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox65 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox21 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.sreEnrolmentSchemeEng = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.txtPrintDateEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDearText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtHeaderEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionEng1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionEng2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDearSPNameChi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtHeaderChi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionChi1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionChi2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionEng3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionChi3c = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.sreLetterHeaderEng = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.sreSPIDandTokenNoEng = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.PageBreak1 = New GrapeCity.ActiveReports.SectionReportModel.PageBreak()
            Me.sreLetterHeaderChi = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.sreEnrolmentSchemeChi = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.sreSPIDandTokenNoChi = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.sreNameOfMOEng = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.sreNameOfMOChi = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.txtDescriptionChi3a = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionEng5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtFooterEng1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionEng4b = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionEng4ActivationPeriod = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionChi4a = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionChi4ActivationPeriod = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionChi4c = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionChi4b = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionChi5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtFooterChi1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNameOfServiceProviderEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNameOfServiceProviderChi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtStarEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionEng3b = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtStarChi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionChi3b = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionEng3a = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionEng4a = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.sreLetterEndingEng = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.sreLetterEndingChi = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.txtDescriptionEng4c = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionEng4d = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionEng4e = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDescriptionChi4e = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.richboxEmailChi = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
            Me.richboxEmailEng = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
            Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
            Me.txtFormCode = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.ReportHeader1 = New GrapeCity.ActiveReports.SectionReportModel.ReportHeader()
            Me.ReportFooter1 = New GrapeCity.ActiveReports.SectionReportModel.ReportFooter()
            CType(Me.TextBox48, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox56, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox57, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox58, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Picture1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox59, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox61, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox69, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox65, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox21, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDateEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDearText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtHeaderEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionEng1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionEng2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDearSPNameChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtHeaderChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionChi1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionChi2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionEng3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionChi3c, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionChi3a, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionEng5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtFooterEng1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionEng4b, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionEng4ActivationPeriod, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionChi4a, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionChi4ActivationPeriod, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionChi4c, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionChi4b, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionChi5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtFooterChi1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameOfServiceProviderEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameOfServiceProviderChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtStarEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionEng3b, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtStarChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionChi3b, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionEng3a, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionEng4a, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionEng4c, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionEng4d, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionEng4e, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDescriptionChi4e, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtFormCode, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox48, Me.TextBox56, Me.TextBox57, Me.TextBox58, Me.Picture1, Me.TextBox59, Me.TextBox61, Me.TextBox69, Me.TextBox65, Me.TextBox21})
            Me.PageHeader1.Height = 1.25!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'TextBox48
            '
            Me.TextBox48.Height = 0.375!
            Me.TextBox48.Left = 0.0625!
            Me.TextBox48.Name = "TextBox48"
            Me.TextBox48.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: center; vertic" & _
        "al-align: middle; ddo-char-set: 136"
            Me.TextBox48.Text = "香港特別行政區政府"
            Me.TextBox48.Top = 0.0!
            Me.TextBox48.Visible = False
            Me.TextBox48.Width = 2.5625!
            '
            'TextBox56
            '
            Me.TextBox56.Height = 0.375!
            Me.TextBox56.Left = 0.0625!
            Me.TextBox56.Name = "TextBox56"
            Me.TextBox56.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: center; ddo-ch" & _
        "ar-set: 136"
            Me.TextBox56.Text = "衞 生 署"
            Me.TextBox56.Top = 0.375!
            Me.TextBox56.Visible = False
            Me.TextBox56.Width = 2.5625!
            '
            'TextBox57
            '
            Me.TextBox57.Height = 0.1875!
            Me.TextBox57.Left = 0.0625!
            Me.TextBox57.Name = "TextBox57"
            Me.TextBox57.Style = "font-family: 新細明體; font-size: 8.25pt; text-align: center; ddo-char-set: 136"
            Me.TextBox57.Text = "香港灣仔皇后大道東二八四號"
            Me.TextBox57.Top = 0.75!
            Me.TextBox57.Visible = False
            Me.TextBox57.Width = 2.5625!
            '
            'TextBox58
            '
            Me.TextBox58.Height = 0.3125!
            Me.TextBox58.Left = 0.0625!
            Me.TextBox58.Name = "TextBox58"
            Me.TextBox58.Style = "font-family: 新細明體; font-size: 8.25pt; text-align: center; ddo-char-set: 136"
            Me.TextBox58.Text = "鄧志昂專科診所一樓"
            Me.TextBox58.Top = 0.9375!
            Me.TextBox58.Visible = False
            Me.TextBox58.Width = 2.5625!
            '
            'Picture1
            '
            Me.Picture1.Height = 1.25!
            Me.Picture1.ImageData = CType(resources.GetObject("Picture1.ImageData"), System.IO.Stream)
            Me.Picture1.Left = 2.625!
            Me.Picture1.Name = "Picture1"
            Me.Picture1.Top = 0.0!
            Me.Picture1.Visible = False
            Me.Picture1.Width = 1.0!
            '
            'TextBox59
            '
            Me.TextBox59.Height = 0.3125!
            Me.TextBox59.Left = 3.625!
            Me.TextBox59.Name = "TextBox59"
            Me.TextBox59.Style = "font-family: Arial; font-size: 9pt; font-weight: bold; text-align: center; vertic" & _
        "al-align: bottom; ddo-char-set: 0"
            Me.TextBox59.Text = "THE GOVERNMENT OF THE HONG KONG "
            Me.TextBox59.Top = 0.0!
            Me.TextBox59.Visible = False
            Me.TextBox59.Width = 2.75!
            '
            'TextBox61
            '
            Me.TextBox61.Height = 0.1875!
            Me.TextBox61.Left = 3.625!
            Me.TextBox61.Name = "TextBox61"
            Me.TextBox61.Style = "font-family: Arial; font-size: 9pt; font-weight: bold; text-align: center; vertic" & _
        "al-align: top; ddo-char-set: 0"
            Me.TextBox61.Text = "SPECIAL ADMINISTRATIVE REGION"
            Me.TextBox61.Top = 0.3125!
            Me.TextBox61.Visible = False
            Me.TextBox61.Width = 2.75!
            '
            'TextBox69
            '
            Me.TextBox69.Height = 0.1875!
            Me.TextBox69.Left = 3.625!
            Me.TextBox69.Name = "TextBox69"
            Me.TextBox69.Style = "font-family: Arial; font-size: 9pt; font-weight: bold; text-align: center; vertic" & _
        "al-align: top; ddo-char-set: 0"
            Me.TextBox69.Text = "DEPARTMENT OF HEALTH"
            Me.TextBox69.Top = 0.5!
            Me.TextBox69.Visible = False
            Me.TextBox69.Width = 2.75!
            '
            'TextBox65
            '
            Me.TextBox65.Height = 0.1875!
            Me.TextBox65.Left = 3.625!
            Me.TextBox65.Name = "TextBox65"
            Me.TextBox65.Style = "font-family: Arial; font-size: 8.25pt; text-align: center; ddo-char-set: 0"
            Me.TextBox65.Text = "1/F, TANG CHI NGONG SPECIALIST CLINIC BLOCK"
            Me.TextBox65.Top = 0.6875!
            Me.TextBox65.Visible = False
            Me.TextBox65.Width = 2.75!
            '
            'TextBox21
            '
            Me.TextBox21.Height = 0.375!
            Me.TextBox21.Left = 3.625!
            Me.TextBox21.Name = "TextBox21"
            Me.TextBox21.Style = "font-family: Arial; font-size: 8.25pt; text-align: center; ddo-char-set: 0"
            Me.TextBox21.Text = "284 QUEEN’S ROAD EAST, WAN CHAI, HONG KONG"
            Me.TextBox21.Top = 0.875!
            Me.TextBox21.Visible = False
            Me.TextBox21.Width = 2.75!
            '
            'Detail1
            '
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.sreEnrolmentSchemeEng, Me.txtPrintDateEng, Me.txtDearText, Me.txtHeaderEng, Me.txtDescriptionEng1, Me.txtDescriptionEng2, Me.txtDearSPNameChi, Me.txtHeaderChi, Me.txtDescriptionChi1, Me.txtDescriptionChi2, Me.txtDescriptionEng3, Me.txtDescriptionChi3c, Me.sreLetterHeaderEng, Me.sreSPIDandTokenNoEng, Me.PageBreak1, Me.sreLetterHeaderChi, Me.sreEnrolmentSchemeChi, Me.sreSPIDandTokenNoChi, Me.sreNameOfMOEng, Me.sreNameOfMOChi, Me.txtDescriptionChi3a, Me.txtDescriptionEng5, Me.txtFooterEng1, Me.txtDescriptionEng4b, Me.txtDescriptionEng4ActivationPeriod, Me.txtDescriptionChi4a, Me.txtDescriptionChi4ActivationPeriod, Me.txtDescriptionChi4c, Me.txtDescriptionChi4b, Me.txtDescriptionChi5, Me.txtFooterChi1, Me.txtNameOfServiceProviderEng, Me.txtNameOfServiceProviderChi, Me.txtStarEng, Me.txtDescriptionEng3b, Me.txtStarChi, Me.txtDescriptionChi3b, Me.txtDescriptionEng3a, Me.txtDescriptionEng4a, Me.sreLetterEndingEng, Me.sreLetterEndingChi, Me.txtDescriptionEng4c, Me.txtDescriptionEng4d, Me.txtDescriptionEng4e, Me.txtDescriptionChi4e, Me.richboxEmailChi, Me.richboxEmailEng})
            Me.Detail1.Height = 12.13833!
            Me.Detail1.KeepTogether = True
            Me.Detail1.Name = "Detail1"
            '
            'sreEnrolmentSchemeEng
            '
            Me.sreEnrolmentSchemeEng.CloseBorder = False
            Me.sreEnrolmentSchemeEng.Height = 0.21875!
            Me.sreEnrolmentSchemeEng.Left = 0.0!
            Me.sreEnrolmentSchemeEng.Name = "sreEnrolmentSchemeEng"
            Me.sreEnrolmentSchemeEng.Report = Nothing
            Me.sreEnrolmentSchemeEng.ReportName = "SubReport1"
            Me.sreEnrolmentSchemeEng.Top = 1.78125!
            Me.sreEnrolmentSchemeEng.Width = 6.5625!
            '
            'txtPrintDateEng
            '
            Me.txtPrintDateEng.Height = 0.1875!
            Me.txtPrintDateEng.Left = 0.0!
            Me.txtPrintDateEng.LineSpacing = 1.0!
            Me.txtPrintDateEng.Name = "txtPrintDateEng"
            Me.txtPrintDateEng.Style = "font-family: Arial; font-size: 10pt; text-align: right; vertical-align: top; ddo-" & _
        "char-set: 1"
            Me.txtPrintDateEng.Text = "<DATE>"
            Me.txtPrintDateEng.Top = 0.34375!
            Me.txtPrintDateEng.Width = 6.5625!
            '
            'txtDearText
            '
            Me.txtDearText.Height = 0.1875!
            Me.txtDearText.Left = 0.0!
            Me.txtDearText.Name = "txtDearText"
            Me.txtDearText.Style = "font-family: Arial; font-size: 10pt; vertical-align: top; ddo-char-set: 1"
            Me.txtDearText.Text = "Dear Sir / Madam,"
            Me.txtDearText.Top = 0.53125!
            Me.txtDearText.Width = 1.46875!
            '
            'txtHeaderEng
            '
            Me.txtHeaderEng.Height = 0.1875!
            Me.txtHeaderEng.Left = 0.0!
            Me.txtHeaderEng.Name = "txtHeaderEng"
            Me.txtHeaderEng.Style = "font-family: Arial; font-size: 10pt; font-weight: bold; text-align: center; text-" & _
        "decoration: underline; vertical-align: top; ddo-char-set: 1"
            Me.txtHeaderEng.Text = "Enrolment Confirmation Letter"
            Me.txtHeaderEng.Top = 0.71875!
            Me.txtHeaderEng.Width = 6.5625!
            '
            'txtDescriptionEng1
            '
            Me.txtDescriptionEng1.Height = 0.34375!
            Me.txtDescriptionEng1.Left = 0.0!
            Me.txtDescriptionEng1.Name = "txtDescriptionEng1"
            Me.txtDescriptionEng1.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.txtDescriptionEng1.Text = "We are pleased to inform you that your application for enrolment in the scheme(s)" & _
        " listed below has been successful."
            Me.txtDescriptionEng1.Top = 1.34375!
            Me.txtDescriptionEng1.Width = 6.5625!
            '
            'txtDescriptionEng2
            '
            Me.txtDescriptionEng2.Height = 0.34375!
            Me.txtDescriptionEng2.Left = 0.0!
            Me.txtDescriptionEng2.Name = "txtDescriptionEng2"
            Me.txtDescriptionEng2.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.txtDescriptionEng2.Text = "We are now sending you the scheme logo(s). Please display the logo(s) at conspicu" & _
        "ous location(s) of your practice."
            Me.txtDescriptionEng2.Top = 2.09375!
            Me.txtDescriptionEng2.Width = 6.5625!
            '
            'txtDearSPNameChi
            '
            Me.txtDearSPNameChi.Height = 0.1875!
            Me.txtDearSPNameChi.Left = 0.0!
            Me.txtDearSPNameChi.Name = "txtDearSPNameChi"
            Me.txtDearSPNameChi.Style = "font-family: 新細明體; font-size: 11.25pt; vertical-align: top; ddo-char-set: 1"
            Me.txtDearSPNameChi.Text = "先生 / 女士："
            Me.txtDearSPNameChi.Top = 6.832!
            Me.txtDearSPNameChi.Width = 6.5625!
            '
            'txtHeaderChi
            '
            Me.txtHeaderChi.Height = 0.21875!
            Me.txtHeaderChi.Left = 0.0!
            Me.txtHeaderChi.Name = "txtHeaderChi"
            Me.txtHeaderChi.Style = "font-family: 新細明體; font-size: 14.25pt; font-weight: normal; text-align: center; t" & _
        "ext-decoration: underline; vertical-align: top; ddo-char-set: 136"
            Me.txtHeaderChi.Text = "登記確認書"
            Me.txtHeaderChi.Top = 7.05075!
            Me.txtHeaderChi.Width = 6.5625!
            '
            'txtDescriptionChi1
            '
            Me.txtDescriptionChi1.Height = 0.1875!
            Me.txtDescriptionChi1.Left = 0.0!
            Me.txtDescriptionChi1.LineSpacing = 1.0!
            Me.txtDescriptionChi1.Name = "txtDescriptionChi1"
            Me.txtDescriptionChi1.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; " & _
        "ddo-char-set: 1"
            Me.txtDescriptionChi1.Text = "謹此通知，你申請登記參加下列計劃一事，已經完成辦埋。"
            Me.txtDescriptionChi1.Top = 7.832!
            Me.txtDescriptionChi1.Width = 6.5625!
            '
            'txtDescriptionChi2
            '
            Me.txtDescriptionChi2.Height = 0.1875!
            Me.txtDescriptionChi2.Left = 0.0!
            Me.txtDescriptionChi2.LineSpacing = 1.0!
            Me.txtDescriptionChi2.Name = "txtDescriptionChi2"
            Me.txtDescriptionChi2.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; " & _
        "ddo-char-set: 1"
            Me.txtDescriptionChi2.Text = "現隨信寄上上述計劃的標誌。請將標誌展示於執業處所內的顯眼位置。"
            Me.txtDescriptionChi2.Top = 8.45701!
            Me.txtDescriptionChi2.Width = 6.5625!
            '
            'txtDescriptionEng3
            '
            Me.txtDescriptionEng3.Height = 0.46875!
            Me.txtDescriptionEng3.Left = 0.0!
            Me.txtDescriptionEng3.Name = "txtDescriptionEng3"
            Me.txtDescriptionEng3.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.txtDescriptionEng3.Text = resources.GetString("txtDescriptionEng3.Text")
            Me.txtDescriptionEng3.Top = 3.434353!
            Me.txtDescriptionEng3.Width = 6.5625!
            '
            'txtDescriptionChi3c
            '
            Me.txtDescriptionChi3c.Height = 0.375!
            Me.txtDescriptionChi3c.Left = 0.0!
            Me.txtDescriptionChi3c.LineSpacing = 1.0!
            Me.txtDescriptionChi3c.Name = "txtDescriptionChi3c"
            Me.txtDescriptionChi3c.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; " & _
        "ddo-char-set: 1"
            Me.txtDescriptionChi3c.Text = "我們已發出一封電子郵件到你的登記電郵地址，電子郵件附有啟動上述戶口的超連結。請點擊上述電子郵件的啟動超連結，使用你的服務提供者編號及保安編碼器進行戶口啟動程序。 " & _
        ""
            Me.txtDescriptionChi3c.Top = 9.58601!
            Me.txtDescriptionChi3c.Width = 6.5625!
            '
            'sreLetterHeaderEng
            '
            Me.sreLetterHeaderEng.CloseBorder = False
            Me.sreLetterHeaderEng.Height = 0.1875!
            Me.sreLetterHeaderEng.Left = 0.0!
            Me.sreLetterHeaderEng.Name = "sreLetterHeaderEng"
            Me.sreLetterHeaderEng.Report = Nothing
            Me.sreLetterHeaderEng.ReportName = "SubReport1"
            Me.sreLetterHeaderEng.Top = 0.0!
            Me.sreLetterHeaderEng.Width = 6.5625!
            '
            'sreSPIDandTokenNoEng
            '
            Me.sreSPIDandTokenNoEng.CloseBorder = False
            Me.sreSPIDandTokenNoEng.Height = 0.21875!
            Me.sreSPIDandTokenNoEng.Left = 0.0!
            Me.sreSPIDandTokenNoEng.Name = "sreSPIDandTokenNoEng"
            Me.sreSPIDandTokenNoEng.Report = Nothing
            Me.sreSPIDandTokenNoEng.ReportName = "SubReport1"
            Me.sreSPIDandTokenNoEng.Top = 2.53125!
            Me.sreSPIDandTokenNoEng.Width = 6.5625!
            '
            'PageBreak1
            '
            Me.PageBreak1.Height = 0.01!
            Me.PageBreak1.Left = 0.0!
            Me.PageBreak1.Name = "PageBreak1"
            Me.PageBreak1.Size = New System.Drawing.SizeF(6.5!, 0.01!)
            Me.PageBreak1.Top = 6.42575!
            Me.PageBreak1.Width = 6.5!
            '
            'sreLetterHeaderChi
            '
            Me.sreLetterHeaderChi.CloseBorder = False
            Me.sreLetterHeaderChi.Height = 0.1875!
            Me.sreLetterHeaderChi.Left = 0.0!
            Me.sreLetterHeaderChi.Name = "sreLetterHeaderChi"
            Me.sreLetterHeaderChi.Report = Nothing
            Me.sreLetterHeaderChi.ReportName = "SubReport1"
            Me.sreLetterHeaderChi.Top = 6.457!
            Me.sreLetterHeaderChi.Width = 6.5625!
            '
            'sreEnrolmentSchemeChi
            '
            Me.sreEnrolmentSchemeChi.CloseBorder = False
            Me.sreEnrolmentSchemeChi.Height = 0.1875!
            Me.sreEnrolmentSchemeChi.Left = 0.0!
            Me.sreEnrolmentSchemeChi.Name = "sreEnrolmentSchemeChi"
            Me.sreEnrolmentSchemeChi.Report = Nothing
            Me.sreEnrolmentSchemeChi.ReportName = "SubReport1"
            Me.sreEnrolmentSchemeChi.Top = 8.144503!
            Me.sreEnrolmentSchemeChi.Width = 6.5625!
            '
            'sreSPIDandTokenNoChi
            '
            Me.sreSPIDandTokenNoChi.CloseBorder = False
            Me.sreSPIDandTokenNoChi.Height = 0.1875!
            Me.sreSPIDandTokenNoChi.Left = 0.0!
            Me.sreSPIDandTokenNoChi.Name = "sreSPIDandTokenNoChi"
            Me.sreSPIDandTokenNoChi.Report = Nothing
            Me.sreSPIDandTokenNoChi.ReportName = "SubReport1"
            Me.sreSPIDandTokenNoChi.Top = 8.769512!
            Me.sreSPIDandTokenNoChi.Width = 6.5625!
            '
            'sreNameOfMOEng
            '
            Me.sreNameOfMOEng.CloseBorder = False
            Me.sreNameOfMOEng.Height = 0.1875!
            Me.sreNameOfMOEng.Left = 0.0!
            Me.sreNameOfMOEng.Name = "sreNameOfMOEng"
            Me.sreNameOfMOEng.Report = Nothing
            Me.sreNameOfMOEng.ReportName = "SubReport1"
            Me.sreNameOfMOEng.Top = 1.0625!
            Me.sreNameOfMOEng.Width = 6.5625!
            '
            'sreNameOfMOChi
            '
            Me.sreNameOfMOChi.CloseBorder = False
            Me.sreNameOfMOChi.Height = 0.1875!
            Me.sreNameOfMOChi.Left = 0.0!
            Me.sreNameOfMOChi.Name = "sreNameOfMOChi"
            Me.sreNameOfMOChi.Report = Nothing
            Me.sreNameOfMOChi.ReportName = "SubReport1"
            Me.sreNameOfMOChi.Top = 7.457!
            Me.sreNameOfMOChi.Width = 6.5625!
            '
            'txtDescriptionChi3a
            '
            Me.txtDescriptionChi3a.Height = 0.1875!
            Me.txtDescriptionChi3a.Left = 0.0625!
            Me.txtDescriptionChi3a.LineSpacing = 1.0!
            Me.txtDescriptionChi3a.Name = "txtDescriptionChi3a"
            Me.txtDescriptionChi3a.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: left; text-decoration: underli" & _
        "ne; ddo-char-set: 136"
            Me.txtDescriptionChi3a.Text = "由於你現為醫院管理局的公私營醫療合作－醫療病歷互聯試驗計劃 / 電子健康紀錄互通系統的"
            Me.txtDescriptionChi3a.Top = 9.082012!
            Me.txtDescriptionChi3a.Width = 6.5!
            '
            'txtDescriptionEng5
            '
            Me.txtDescriptionEng5.Height = 0.1875!
            Me.txtDescriptionEng5.Left = 0.0!
            Me.txtDescriptionEng5.LineSpacing = 1.0!
            Me.txtDescriptionEng5.Name = "txtDescriptionEng5"
            Me.txtDescriptionEng5.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.txtDescriptionEng5.Text = "Please feel free to contact us if you have any enquires. Our e-mail address is:"
            Me.txtDescriptionEng5.Top = 5.081853!
            Me.txtDescriptionEng5.Width = 4.8125!
            '
            'txtFooterEng1
            '
            Me.txtFooterEng1.Height = 0.21875!
            Me.txtFooterEng1.Left = 0.0!
            Me.txtFooterEng1.LineSpacing = 1.0!
            Me.txtFooterEng1.Name = "txtFooterEng1"
            Me.txtFooterEng1.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.txtFooterEng1.Text = "Thank you once again for your support!"
            Me.txtFooterEng1.Top = 5.581853!
            Me.txtFooterEng1.Width = 6.5625!
            '
            'txtDescriptionEng4b
            '
            Me.txtDescriptionEng4b.CanGrow = False
            Me.txtDescriptionEng4b.Height = 0.25!
            Me.txtDescriptionEng4b.Left = 5.562599!
            Me.txtDescriptionEng4b.LineSpacing = 1.0!
            Me.txtDescriptionEng4b.Name = "txtDescriptionEng4b"
            Me.txtDescriptionEng4b.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.txtDescriptionEng4b.Text = "days of the date"
            Me.txtDescriptionEng4b.Top = 3.996951!
            Me.txtDescriptionEng4b.Width = 1.0625!
            '
            'txtDescriptionEng4ActivationPeriod
            '
            Me.txtDescriptionEng4ActivationPeriod.Height = 0.1875!
            Me.txtDescriptionEng4ActivationPeriod.Left = 5.343701!
            Me.txtDescriptionEng4ActivationPeriod.Name = "txtDescriptionEng4ActivationPeriod"
            Me.txtDescriptionEng4ActivationPeriod.Style = "font-size: 10pt; font-weight: bold; text-align: center; vertical-align: top; ddo-" & _
        "char-set: 1"
            Me.txtDescriptionEng4ActivationPeriod.Text = "99"
            Me.txtDescriptionEng4ActivationPeriod.Top = 3.996951!
            Me.txtDescriptionEng4ActivationPeriod.Width = 0.21875!
            '
            'txtDescriptionChi4a
            '
            Me.txtDescriptionChi4a.Height = 0.1875!
            Me.txtDescriptionChi4a.Left = 0.0!
            Me.txtDescriptionChi4a.LineSpacing = 1.0!
            Me.txtDescriptionChi4a.Name = "txtDescriptionChi4a"
            Me.txtDescriptionChi4a.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; " & _
        "ddo-char-set: 1"
            Me.txtDescriptionChi4a.Text = "請儘快啟動戶口。倘若在本信發出日期"
            Me.txtDescriptionChi4a.Top = 10.09601!
            Me.txtDescriptionChi4a.Width = 2.6875!
            '
            'txtDescriptionChi4ActivationPeriod
            '
            Me.txtDescriptionChi4ActivationPeriod.CanShrink = True
            Me.txtDescriptionChi4ActivationPeriod.Height = 0.1875!
            Me.txtDescriptionChi4ActivationPeriod.Left = 2.65625!
            Me.txtDescriptionChi4ActivationPeriod.Name = "txtDescriptionChi4ActivationPeriod"
            Me.txtDescriptionChi4ActivationPeriod.Style = "font-family: 新細明體; font-size: 12pt; font-weight: normal; text-align: center; vert" & _
        "ical-align: top; ddo-char-set: 1"
            Me.txtDescriptionChi4ActivationPeriod.Text = "99"
            Me.txtDescriptionChi4ActivationPeriod.Top = 10.09601!
            Me.txtDescriptionChi4ActivationPeriod.Width = 0.25!
            '
            'txtDescriptionChi4c
            '
            Me.txtDescriptionChi4c.Height = 0.1875!
            Me.txtDescriptionChi4c.Left = 0.0!
            Me.txtDescriptionChi4c.LineSpacing = 1.0!
            Me.txtDescriptionChi4c.Name = "txtDescriptionChi4c"
            Me.txtDescriptionChi4c.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; " & _
        "ddo-char-set: 1"
            Me.txtDescriptionChi4c.Text = "銷，而需重新申請登記。"
            Me.txtDescriptionChi4c.Top = 10.27351!
            Me.txtDescriptionChi4c.Width = 6.5625!
            '
            'txtDescriptionChi4b
            '
            Me.txtDescriptionChi4b.Height = 0.1875!
            Me.txtDescriptionChi4b.Left = 2.8125!
            Me.txtDescriptionChi4b.LineSpacing = 1.0!
            Me.txtDescriptionChi4b.Name = "txtDescriptionChi4b"
            Me.txtDescriptionChi4b.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; " & _
        "ddo-char-set: 136"
            Me.txtDescriptionChi4b.Text = " 日內未能啟動戶口，你的登記申請可能會被視為已撤"
            Me.txtDescriptionChi4b.Top = 10.08601!
            Me.txtDescriptionChi4b.Width = 3.75!
            '
            'txtDescriptionChi5
            '
            Me.txtDescriptionChi5.Height = 0.1875!
            Me.txtDescriptionChi5.Left = 0.0!
            Me.txtDescriptionChi5.LineSpacing = 1.0!
            Me.txtDescriptionChi5.Name = "txtDescriptionChi5"
            Me.txtDescriptionChi5.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; " & _
        "ddo-char-set: 1"
            Me.txtDescriptionChi5.Text = "如有任何查詢，請隨時與本署聯絡，電郵地址是："
            Me.txtDescriptionChi5.Top = 10.855!
            Me.txtDescriptionChi5.Width = 3.5!
            '
            'txtFooterChi1
            '
            Me.txtFooterChi1.Height = 0.1875!
            Me.txtFooterChi1.Left = 0.0!
            Me.txtFooterChi1.LineSpacing = 1.0!
            Me.txtFooterChi1.Name = "txtFooterChi1"
            Me.txtFooterChi1.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; " & _
        "ddo-char-set: 1"
            Me.txtFooterChi1.Text = "再次多謝你的支持。"
            Me.txtFooterChi1.Top = 11.48!
            Me.txtFooterChi1.Width = 6.5625!
            '
            'txtNameOfServiceProviderEng
            '
            Me.txtNameOfServiceProviderEng.Height = 0.15625!
            Me.txtNameOfServiceProviderEng.Left = 0.0!
            Me.txtNameOfServiceProviderEng.LineSpacing = 1.0!
            Me.txtNameOfServiceProviderEng.Name = "txtNameOfServiceProviderEng"
            Me.txtNameOfServiceProviderEng.Style = "font-family: Arial; font-size: 10pt; font-weight: bold; text-align: center; text-" & _
        "decoration: none; vertical-align: top; ddo-char-set: 1"
            Me.txtNameOfServiceProviderEng.Text = "<ServiceProviderName>"
            Me.txtNameOfServiceProviderEng.Top = 0.90625!
            Me.txtNameOfServiceProviderEng.Width = 6.5625!
            '
            'txtNameOfServiceProviderChi
            '
            Me.txtNameOfServiceProviderChi.Height = 0.1875!
            Me.txtNameOfServiceProviderChi.Left = 0.0!
            Me.txtNameOfServiceProviderChi.Name = "txtNameOfServiceProviderChi"
            Me.txtNameOfServiceProviderChi.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 14.25pt; font-weight: normal; text-align: cen" & _
        "ter; text-decoration: none; vertical-align: top; ddo-char-set: 136"
            Me.txtNameOfServiceProviderChi.Text = "<ServiceProviderName>"
            Me.txtNameOfServiceProviderChi.Top = 7.2695!
            Me.txtNameOfServiceProviderChi.Width = 6.5625!
            '
            'txtStarEng
            '
            Me.txtStarEng.Height = 0.1875!
            Me.txtStarEng.Left = 0.0!
            Me.txtStarEng.Name = "txtStarEng"
            Me.txtStarEng.Text = "*"
            Me.txtStarEng.Top = 2.84375!
            Me.txtStarEng.Width = 0.09375!
            '
            'txtDescriptionEng3b
            '
            Me.txtDescriptionEng3b.Height = 0.344!
            Me.txtDescriptionEng3b.Left = 0.0!
            Me.txtDescriptionEng3b.Name = "txtDescriptionEng3b"
            Me.txtDescriptionEng3b.Style = "font-family: Arial; font-size: 10pt; text-align: left; text-decoration: underline" & _
        "; ddo-char-set: 1"
            Me.txtDescriptionEng3b.Text = "Record Sharing Pilot Project (PPI-ePR) / Electronic Health Record Sharing System " & _
        "(eHRSS), please use the same token to access the eHealth System (Subsidies) for " & _
        "account activation and operation."
            Me.txtDescriptionEng3b.Top = 3.0!
            Me.txtDescriptionEng3b.Width = 6.594!
            '
            'txtStarChi
            '
            Me.txtStarChi.Height = 0.1875!
            Me.txtStarChi.Left = 0.0!
            Me.txtStarChi.Name = "txtStarChi"
            Me.txtStarChi.Style = "font-size: 11.25pt"
            Me.txtStarChi.Text = "*"
            Me.txtStarChi.Top = 9.08201!
            Me.txtStarChi.Width = 0.09375!
            '
            'txtDescriptionChi3b
            '
            Me.txtDescriptionChi3b.Height = 0.1875!
            Me.txtDescriptionChi3b.Left = 0.0!
            Me.txtDescriptionChi3b.LineSpacing = 1.0!
            Me.txtDescriptionChi3b.Name = "txtDescriptionChi3b"
            Me.txtDescriptionChi3b.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; text-decoration: unde" & _
        "rline; vertical-align: top; ddo-char-set: 1"
            Me.txtDescriptionChi3b.Text = "登記用戶，請用該系統的編碼器，登入醫健通(資助)系統，以啓動戶口並進行操作。"
            Me.txtDescriptionChi3b.Top = 9.238262!
            Me.txtDescriptionChi3b.Width = 6.5625!
            '
            'txtDescriptionEng3a
            '
            Me.txtDescriptionEng3a.Height = 0.15625!
            Me.txtDescriptionEng3a.Left = 0.061!
            Me.txtDescriptionEng3a.Name = "txtDescriptionEng3a"
            Me.txtDescriptionEng3a.Style = "font-family: Arial; font-size: 10pt; text-align: left; text-decoration: underline" & _
        "; ddo-char-set: 1"
            Me.txtDescriptionEng3a.Text = "Since you are a current user under the Hospital Authority (HA) Public-Private Int" & _
        "erface - Electronic Patient"
            Me.txtDescriptionEng3a.Top = 2.844!
            Me.txtDescriptionEng3a.Width = 6.5625!
            '
            'txtDescriptionEng4a
            '
            Me.txtDescriptionEng4a.CanGrow = False
            Me.txtDescriptionEng4a.Height = 0.21875!
            Me.txtDescriptionEng4a.Left = 0.0!
            Me.txtDescriptionEng4a.LineSpacing = 1.0!
            Me.txtDescriptionEng4a.Name = "txtDescriptionEng4a"
            Me.txtDescriptionEng4a.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.txtDescriptionEng4a.Text = "Please activate your account as soon as possible. If the account is not activated" & _
        " within "
            Me.txtDescriptionEng4a.Top = 3.996853!
            Me.txtDescriptionEng4a.Width = 5.34375!
            '
            'sreLetterEndingEng
            '
            Me.sreLetterEndingEng.CloseBorder = False
            Me.sreLetterEndingEng.Height = 0.1875!
            Me.sreLetterEndingEng.Left = 0.0!
            Me.sreLetterEndingEng.Name = "sreLetterEndingEng"
            Me.sreLetterEndingEng.Report = Nothing
            Me.sreLetterEndingEng.ReportName = "SubReport1"
            Me.sreLetterEndingEng.Top = 6.019353!
            Me.sreLetterEndingEng.Width = 6.5625!
            '
            'sreLetterEndingChi
            '
            Me.sreLetterEndingChi.CloseBorder = False
            Me.sreLetterEndingChi.Height = 0.1875!
            Me.sreLetterEndingChi.Left = 0.0!
            Me.sreLetterEndingChi.Name = "sreLetterEndingChi"
            Me.sreLetterEndingChi.Report = Nothing
            Me.sreLetterEndingChi.ReportName = "SubReport1"
            Me.sreLetterEndingChi.Top = 11.94875!
            Me.sreLetterEndingChi.Width = 6.5625!
            '
            'txtDescriptionEng4c
            '
            Me.txtDescriptionEng4c.Height = 0.34375!
            Me.txtDescriptionEng4c.Left = 0.0!
            Me.txtDescriptionEng4c.LineSpacing = 1.0!
            Me.txtDescriptionEng4c.Name = "txtDescriptionEng4c"
            Me.txtDescriptionEng4c.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.txtDescriptionEng4c.Text = "of issue of this letter, your enrolment may be considered  as withdrawn and you m" & _
        "ay have to re-apply for enrolment."
            Me.txtDescriptionEng4c.Top = 4.184353!
            Me.txtDescriptionEng4c.Width = 6.5625!
            '
            'txtDescriptionEng4d
            '
            Me.txtDescriptionEng4d.Height = 0.34375!
            Me.txtDescriptionEng4d.Left = 0.01574993!
            Me.txtDescriptionEng4d.Name = "txtDescriptionEng4d"
            Me.txtDescriptionEng4d.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.txtDescriptionEng4d.Text = "The opportunity is taken to remind you to observe and comply with the terms and c" & _
        "onditions of the Agreement of respective scheme(s), with link(s) provided above." & _
        ""
            Me.txtDescriptionEng4d.Top = 6.130207!
            Me.txtDescriptionEng4d.Width = 6.5625!
            '
            'txtDescriptionEng4e
            '
            Me.txtDescriptionEng4e.Height = 0.34375!
            Me.txtDescriptionEng4e.Left = 0.0!
            Me.txtDescriptionEng4e.Name = "txtDescriptionEng4e"
            Me.txtDescriptionEng4e.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.txtDescriptionEng4e.Text = "The opportunity is taken to remind you to observe and comply with the terms and c" & _
        "onditions of the Agreement of respective scheme(s), with link(s) provided above." & _
        ""
            Me.txtDescriptionEng4e.Top = 4.631!
            Me.txtDescriptionEng4e.Width = 6.5625!
            '
            'txtDescriptionChi4e
            '
            Me.txtDescriptionChi4e.Height = 0.2079993!
            Me.txtDescriptionChi4e.Left = -0.00000001490116!
            Me.txtDescriptionChi4e.LineSpacing = 1.0!
            Me.txtDescriptionChi4e.Name = "txtDescriptionChi4e"
            Me.txtDescriptionChi4e.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; " & _
        "ddo-char-set: 1"
            Me.txtDescriptionChi4e.Text = "現藉此機會提示你必須留意及遵守有關計劃協議的條款和條件(上文已提供相關協議的連結)。"
            Me.txtDescriptionChi4e.Top = 10.54!
            Me.txtDescriptionChi4e.Width = 6.5625!
            '
            'richboxEmailChi
            '
            Me.richboxEmailChi.AutoReplaceFields = True
            Me.richboxEmailChi.Font = New System.Drawing.Font("Arial", 10.0!)
            Me.richboxEmailChi.Height = 0.1880002!
            Me.richboxEmailChi.Left = 0.0!
            Me.richboxEmailChi.Name = "richboxEmailChi"
            Me.richboxEmailChi.RTF = resources.GetString("richboxEmailChi.RTF")
            Me.richboxEmailChi.Top = 11.042!
            Me.richboxEmailChi.Width = 4.812!
            '
            'richboxEmailEng
            '
            Me.richboxEmailEng.AutoReplaceFields = True
            Me.richboxEmailEng.Font = New System.Drawing.Font("Arial", 10.0!)
            Me.richboxEmailEng.Height = 0.1880002!
            Me.richboxEmailEng.Left = 0.0!
            Me.richboxEmailEng.Name = "richboxEmailEng"
            Me.richboxEmailEng.RTF = resources.GetString("richboxEmailEng.RTF")
            Me.richboxEmailEng.Top = 5.269!
            Me.richboxEmailEng.Width = 4.812!
            '
            'PageFooter1
            '
            Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtFormCode})
            Me.PageFooter1.Height = 0.3!
            Me.PageFooter1.Name = "PageFooter1"
            '
            'txtFormCode
            '
            Me.txtFormCode.Height = 0.1875!
            Me.txtFormCode.Left = 0.0!
            Me.txtFormCode.Name = "txtFormCode"
            Me.txtFormCode.Text = "DH_XXXXX(X/XX)"
            Me.txtFormCode.Top = 0.0!
            Me.txtFormCode.Width = 6.5625!
            '
            'ReportHeader1
            '
            Me.ReportHeader1.Height = 0.0!
            Me.ReportHeader1.Name = "ReportHeader1"
            '
            'ReportFooter1
            '
            Me.ReportFooter1.Height = 0.0!
            Me.ReportFooter1.Name = "ReportFooter1"
            '
            'DH_eHS004
            '
            Me.MasterReport = False
            Me.PageSettings.Margins.Bottom = 0.0!
            Me.PageSettings.Margins.Top = 0.0!
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.7135!
            Me.Sections.Add(Me.ReportHeader1)
            Me.Sections.Add(Me.PageHeader1)
            Me.Sections.Add(Me.Detail1)
            Me.Sections.Add(Me.PageFooter1)
            Me.Sections.Add(Me.ReportFooter1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox48, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox56, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox57, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox58, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Picture1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox59, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox61, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox69, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox65, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox21, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDateEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDearText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtHeaderEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionEng1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionEng2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDearSPNameChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtHeaderChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionChi1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionChi2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionEng3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionChi3c, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionChi3a, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionEng5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtFooterEng1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionEng4b, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionEng4ActivationPeriod, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionChi4a, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionChi4ActivationPeriod, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionChi4c, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionChi4b, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionChi5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtFooterChi1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameOfServiceProviderEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameOfServiceProviderChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtStarEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionEng3b, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtStarChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionChi3b, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionEng3a, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionEng4a, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionEng4c, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionEng4d, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionEng4e, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDescriptionChi4e, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtFormCode, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtFormCode As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox48 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox56 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox57 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox58 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents Picture1 As GrapeCity.ActiveReports.SectionReportModel.Picture
        Friend WithEvents TextBox59 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox61 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox69 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox65 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox21 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents sreEnrolmentSchemeEng As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents txtPrintDateEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDearText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtHeaderEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionEng1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionEng2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDearSPNameChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtHeaderChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionEng3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi3c As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents sreLetterHeaderEng As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents sreSPIDandTokenNoEng As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents PageBreak1 As GrapeCity.ActiveReports.SectionReportModel.PageBreak
        Friend WithEvents sreLetterHeaderChi As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents sreEnrolmentSchemeChi As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents sreSPIDandTokenNoChi As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents sreNameOfMOEng As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents sreNameOfMOChi As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents txtDescriptionEng3a As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi3a As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionEng5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtFooterEng1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionEng4b As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionEng4c As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionEng4ActivationPeriod As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi4a As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi4ActivationPeriod As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi4c As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi4b As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtFooterChi1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtNameOfServiceProviderEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtNameOfServiceProviderChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtStarEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionEng3b As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtStarChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi3b As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionEng4a As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents ReportHeader1 As GrapeCity.ActiveReports.SectionReportModel.ReportHeader
        Friend WithEvents ReportFooter1 As GrapeCity.ActiveReports.SectionReportModel.ReportFooter
        Friend WithEvents sreLetterEndingEng As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents sreLetterEndingChi As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents txtDescriptionEng4d As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents richboxEmailChi As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
        Private WithEvents richboxEmailEng As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
        Friend WithEvents txtDescriptionEng4e As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi4e As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace