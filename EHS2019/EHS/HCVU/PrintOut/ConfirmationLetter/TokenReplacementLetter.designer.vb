Namespace PrintOut.ConfirmationLetter

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class TokenReplacementLetter
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
        Private WithEvents Detail1 As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TokenReplacementLetter))
        Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
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
        Me.txtDescriptionChi3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.sreLetterHeaderEng = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
        Me.PageBreak1 = New GrapeCity.ActiveReports.SectionReportModel.PageBreak()
        Me.sreLetterHeaderChi = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
        Me.sreLetterEndingEng = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
        Me.sreLetterEndingChi = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
        Me.txtDescriptionEng4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtDescriptionChi4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtReplaceReasonEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtDescriptionEng5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSpecialFont01Eng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSpecialFont02Eng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtReplaceReason_Chi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtDescriptionChi5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSpecialFont01Chi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
        Me.txtSpecialFont02Chi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
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
        Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
        Me.ReportHeader1 = New GrapeCity.ActiveReports.SectionReportModel.ReportHeader()
        Me.ReportFooter1 = New GrapeCity.ActiveReports.SectionReportModel.ReportFooter()
        CType(Me.txtPrintDateEng,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtDearText,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtHeaderEng,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtDescriptionEng1,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtDescriptionEng2,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtDearSPNameChi,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtHeaderChi,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtDescriptionChi1,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtDescriptionChi2,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtDescriptionEng3,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtDescriptionChi3,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtDescriptionEng4,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtDescriptionChi4,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtReplaceReasonEng,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtDescriptionEng5,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtSpecialFont01Eng,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtSpecialFont02Eng,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtReplaceReason_Chi,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtDescriptionChi5,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtSpecialFont01Chi,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.txtSpecialFont02Chi,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.TextBox48,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.TextBox56,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.TextBox57,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.TextBox58,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.Picture1,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.TextBox59,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.TextBox61,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.TextBox69,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.TextBox65,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.TextBox21,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me,System.ComponentModel.ISupportInitialize).BeginInit
        '
        'Detail1
        '
        Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtPrintDateEng, Me.txtDearText, Me.txtHeaderEng, Me.txtDescriptionEng1, Me.txtDescriptionEng2, Me.txtDearSPNameChi, Me.txtHeaderChi, Me.txtDescriptionChi1, Me.txtDescriptionChi2, Me.txtDescriptionEng3, Me.txtDescriptionChi3, Me.sreLetterHeaderEng, Me.PageBreak1, Me.sreLetterHeaderChi, Me.sreLetterEndingEng, Me.sreLetterEndingChi, Me.txtDescriptionEng4, Me.txtDescriptionChi4, Me.txtReplaceReasonEng, Me.txtDescriptionEng5, Me.txtSpecialFont01Eng, Me.txtSpecialFont02Eng, Me.txtReplaceReason_Chi, Me.txtDescriptionChi5, Me.txtSpecialFont01Chi, Me.txtSpecialFont02Chi})
        Me.Detail1.Height = 10.40625!
        Me.Detail1.KeepTogether = true
        Me.Detail1.Name = "Detail1"
        '
        'txtPrintDateEng
        '
        Me.txtPrintDateEng.Height = 0.1875!
        Me.txtPrintDateEng.Left = 0!
        Me.txtPrintDateEng.LineSpacing = 1!
        Me.txtPrintDateEng.Name = "txtPrintDateEng"
        Me.txtPrintDateEng.Style = "font-family: Arial; font-size: 10pt; text-align: right; vertical-align: top; ddo-"& _ 
    "char-set: 1"
        Me.txtPrintDateEng.Text = "<CURRENT_DATE>"
        Me.txtPrintDateEng.Top = 0.34375!
        Me.txtPrintDateEng.Width = 6.5625!
        '
        'txtDearText
        '
        Me.txtDearText.Height = 0.21875!
        Me.txtDearText.Left = 0!
        Me.txtDearText.Name = "txtDearText"
        Me.txtDearText.Style = "font-family: Arial; font-size: 10pt; vertical-align: top; ddo-char-set: 1"
        Me.txtDearText.Text = "Dear Sir / Madam,"
        Me.txtDearText.Top = 0.53125!
        Me.txtDearText.Width = 1.84375!
        '
        'txtHeaderEng
        '
        Me.txtHeaderEng.Height = 0.1875!
        Me.txtHeaderEng.Left = 0!
        Me.txtHeaderEng.Name = "txtHeaderEng"
        Me.txtHeaderEng.Style = "font-family: Arial; font-size: 10pt; font-weight: bold; text-align: center; text-"& _ 
    "decoration: underline; vertical-align: top; ddo-char-set: 1"
        Me.txtHeaderEng.Text = "Replacement of eHealth System (Subsidies) Authentication Token"
        Me.txtHeaderEng.Top = 0.78125!
        Me.txtHeaderEng.Width = 6.5625!
        '
        'txtDescriptionEng1
        '
        Me.txtDescriptionEng1.Height = 0.52!
        Me.txtDescriptionEng1.Left = 0!
        Me.txtDescriptionEng1.Name = "txtDescriptionEng1"
        Me.txtDescriptionEng1.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd"& _ 
    "o-char-set: 1"
        Me.txtDescriptionEng1.Text = resources.GetString("txtDescriptionEng1.Text")
        Me.txtDescriptionEng1.Top = 1.125!
        Me.txtDescriptionEng1.Width = 6.5625!
        '
        'txtDescriptionEng2
        '
        Me.txtDescriptionEng2.Height = 0.19!
        Me.txtDescriptionEng2.Left = 0!
        Me.txtDescriptionEng2.Name = "txtDescriptionEng2"
        Me.txtDescriptionEng2.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd"& _ 
    "o-char-set: 1"
        Me.txtDescriptionEng2.Text = "Your Service Provider ID / Username and Password remain unchanged."
        Me.txtDescriptionEng2.Top = 2.08!
        Me.txtDescriptionEng2.Width = 6.5625!
        '
        'txtDearSPNameChi
        '
        Me.txtDearSPNameChi.Height = 0.1875!
        Me.txtDearSPNameChi.Left = 0!
        Me.txtDearSPNameChi.Name = "txtDearSPNameChi"
        Me.txtDearSPNameChi.Style = "font-family: 新細明體; font-size: 11.25pt; vertical-align: top; ddo-char-set: 1"
        Me.txtDearSPNameChi.Text = "先生 / 女士："
        Me.txtDearSPNameChi.Top = 5.8125!
        Me.txtDearSPNameChi.Width = 6.5625!
        '
        'txtHeaderChi
        '
        Me.txtHeaderChi.Height = 0.21875!
        Me.txtHeaderChi.Left = 0!
        Me.txtHeaderChi.Name = "txtHeaderChi"
        Me.txtHeaderChi.Style = "font-family: 新細明體; font-size: 14.25pt; font-weight: normal; text-align: center; t"& _ 
    "ext-decoration: underline; vertical-align: top; ddo-char-set: 1"
        Me.txtHeaderChi.Text = "更換「醫健通(資助)系統」認證保安編碼器"
        Me.txtHeaderChi.Top = 6!
        Me.txtHeaderChi.Width = 6.5625!
        '
        'txtDescriptionChi1
        '
        Me.txtDescriptionChi1.Height = 0.375!
        Me.txtDescriptionChi1.Left = 0!
        Me.txtDescriptionChi1.LineSpacing = 1!
        Me.txtDescriptionChi1.Name = "txtDescriptionChi1"
        Me.txtDescriptionChi1.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; "& _ 
    "ddo-char-set: 1"
        Me.txtDescriptionChi1.Text = "現隨函附上你的新認證保安編碼器 (編碼編號：<TOKEN_SN_NEW>)，以替代你現正使用的編碼器 (編碼編號：<TOKEN_SN_OLD>)。更換編碼器的原因"& _ 
    "如下："
        Me.txtDescriptionChi1.Top = 6.375!
        Me.txtDescriptionChi1.Width = 6.5625!
        '
        'txtDescriptionChi2
        '
        Me.txtDescriptionChi2.Height = 0.19!
        Me.txtDescriptionChi2.Left = 0!
        Me.txtDescriptionChi2.LineSpacing = 1!
        Me.txtDescriptionChi2.Name = "txtDescriptionChi2"
        Me.txtDescriptionChi2.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; "& _ 
    "ddo-char-set: 1"
        Me.txtDescriptionChi2.Text = "你的服務提供者號碼 / 使用者名稱及密碼維持不變。"
        Me.txtDescriptionChi2.Top = 7.27!
        Me.txtDescriptionChi2.Width = 6.5625!
        '
        'txtDescriptionEng3
        '
        Me.txtDescriptionEng3.Height = 0.19!
        Me.txtDescriptionEng3.Left = 0!
        Me.txtDescriptionEng3.Name = "txtDescriptionEng3"
        Me.txtDescriptionEng3.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd"& _ 
    "o-char-set: 1"
        Me.txtDescriptionEng3.Text = "Please activate your new authentication token by following the steps shown at App"& _ 
    "endix."
        Me.txtDescriptionEng3.Top = 2.39!
        Me.txtDescriptionEng3.Width = 6.5625!
        '
        'txtDescriptionChi3
        '
        Me.txtDescriptionChi3.Height = 0.19!
        Me.txtDescriptionChi3.Left = 0!
        Me.txtDescriptionChi3.LineSpacing = 1!
        Me.txtDescriptionChi3.Name = "txtDescriptionChi3"
        Me.txtDescriptionChi3.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; "& _ 
    "ddo-char-set: 1"
        Me.txtDescriptionChi3.Text = "請依照附件的有關程序，啟動你的新編碼器。"
        Me.txtDescriptionChi3.Top = 7.62!
        Me.txtDescriptionChi3.Width = 6.5625!
        '
        'sreLetterHeaderEng
        '
        Me.sreLetterHeaderEng.CloseBorder = false
        Me.sreLetterHeaderEng.Height = 0.1875!
        Me.sreLetterHeaderEng.Left = 0!
        Me.sreLetterHeaderEng.Name = "sreLetterHeaderEng"
        Me.sreLetterHeaderEng.Report = Nothing
        Me.sreLetterHeaderEng.ReportName = "SubReport1"
        Me.sreLetterHeaderEng.Top = 0!
        Me.sreLetterHeaderEng.Width = 6.5625!
        '
        'PageBreak1
        '
        Me.PageBreak1.Height = 0.03125!
        Me.PageBreak1.Left = 0!
        Me.PageBreak1.Name = "PageBreak1"
        Me.PageBreak1.Size = New System.Drawing.SizeF(6.5!, 0.03125!)
        Me.PageBreak1.Top = 5.40625!
        Me.PageBreak1.Width = 6.5!
        '
        'sreLetterHeaderChi
        '
        Me.sreLetterHeaderChi.CloseBorder = false
        Me.sreLetterHeaderChi.Height = 0.1875!
        Me.sreLetterHeaderChi.Left = 0!
        Me.sreLetterHeaderChi.Name = "sreLetterHeaderChi"
        Me.sreLetterHeaderChi.Report = Nothing
        Me.sreLetterHeaderChi.ReportName = "SubReport1"
        Me.sreLetterHeaderChi.Top = 5.40625!
        Me.sreLetterHeaderChi.Width = 6.625!
        '
        'sreLetterEndingEng
        '
        Me.sreLetterEndingEng.CloseBorder = false
        Me.sreLetterEndingEng.Height = 0.1875!
        Me.sreLetterEndingEng.Left = 0!
        Me.sreLetterEndingEng.Name = "sreLetterEndingEng"
        Me.sreLetterEndingEng.Report = Nothing
        Me.sreLetterEndingEng.ReportName = "SubReport1"
        Me.sreLetterEndingEng.Top = 4.3!
        Me.sreLetterEndingEng.Width = 6.5625!
        '
        'sreLetterEndingChi
        '
        Me.sreLetterEndingChi.CloseBorder = false
        Me.sreLetterEndingChi.Height = 0.1875!
        Me.sreLetterEndingChi.Left = 0!
        Me.sreLetterEndingChi.Name = "sreLetterEndingChi"
        Me.sreLetterEndingChi.Report = Nothing
        Me.sreLetterEndingChi.ReportName = "SubReport1"
        Me.sreLetterEndingChi.Top = 9.5!
        Me.sreLetterEndingChi.Width = 6.5625!
        '
        'txtDescriptionEng4
        '
        Me.txtDescriptionEng4.CanGrow = false
        Me.txtDescriptionEng4.Height = 0.375!
        Me.txtDescriptionEng4.Left = 0!
        Me.txtDescriptionEng4.LineSpacing = 1!
        Me.txtDescriptionEng4.Name = "txtDescriptionEng4"
        Me.txtDescriptionEng4.Style = "font-family: Arial; font-size: 10pt; text-align: justify; vertical-align: top; dd"& _ 
    "o-char-set: 1"
        Me.txtDescriptionEng4.Text = resources.GetString("txtDescriptionEng4.Text")
        Me.txtDescriptionEng4.Top = 2.7!
        Me.txtDescriptionEng4.Width = 6.625!
        '
        'txtDescriptionChi4
        '
        Me.txtDescriptionChi4.Height = 0.375!
        Me.txtDescriptionChi4.Left = 0!
        Me.txtDescriptionChi4.LineSpacing = 1!
        Me.txtDescriptionChi4.Name = "txtDescriptionChi4"
        Me.txtDescriptionChi4.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; vertical-align: top; "& _ 
    "ddo-char-set: 1"
        Me.txtDescriptionChi4.Text = "請小心保管你的保安編碼器。如有遺失，申請補發編碼器須繳付所需行政費用 (現時費用為每個編碼器$<LOST_TOKEN_FEE>)。"
        Me.txtDescriptionChi4.Top = 7.97!
        Me.txtDescriptionChi4.Width = 6.5625!
        '
        'txtReplaceReasonEng
        '
        Me.txtReplaceReasonEng.Height = 0.19!
        Me.txtReplaceReasonEng.Left = 0.5!
        Me.txtReplaceReasonEng.Name = "txtReplaceReasonEng"
        Me.txtReplaceReasonEng.Style = "font-family: Arial; font-size: 10pt; vertical-align: top; ddo-char-set: 1"
        Me.txtReplaceReasonEng.Text = "<REPLACE_REASON>"
        Me.txtReplaceReasonEng.Top = 1.77!
        Me.txtReplaceReasonEng.Width = 6.0625!
        '
        'txtDescriptionEng5
        '
        Me.txtDescriptionEng5.CanGrow = false
        Me.txtDescriptionEng5.Height = 0.75!
        Me.txtDescriptionEng5.Left = 0!
        Me.txtDescriptionEng5.LineSpacing = 1!
        Me.txtDescriptionEng5.Name = "txtDescriptionEng5"
        Me.txtDescriptionEng5.Style = "font-family: Arial; font-size: 10pt; text-align: justify; text-justify: distribut"& _ 
    "e; vertical-align: top; ddo-char-set: 1"
        Me.txtDescriptionEng5.Text = resources.GetString("txtDescriptionEng5.Text")
        Me.txtDescriptionEng5.Top = 3.2!
        Me.txtDescriptionEng5.Width = 6.625!
        '
        'txtSpecialFont01Eng
        '
        Me.txtSpecialFont01Eng.CanGrow = false
        Me.txtSpecialFont01Eng.Height = 0.1875!
            Me.txtSpecialFont01Eng.Left = 1.884!
            Me.txtSpecialFont01Eng.MultiLine = False
            Me.txtSpecialFont01Eng.Name = "txtSpecialFont01Eng"
            Me.txtSpecialFont01Eng.Style = "font-family: Arial; font-size: 10pt; font-weight: normal; text-align: left; text-" & _
        "decoration: underline; vertical-align: top; white-space: nowrap; ddo-char-set: 1" & _
        ""
            Me.txtSpecialFont01Eng.Text = "hcvd@dh.gov.hk"
            Me.txtSpecialFont01Eng.Top = 3.375!
            Me.txtSpecialFont01Eng.Width = 1.06!
            '
            'txtSpecialFont02Eng
            '
            Me.txtSpecialFont02Eng.CanGrow = False
            Me.txtSpecialFont02Eng.Height = 0.1875!
            Me.txtSpecialFont02Eng.Left = 1.764!
            Me.txtSpecialFont02Eng.MultiLine = False
            Me.txtSpecialFont02Eng.Name = "txtSpecialFont02Eng"
            Me.txtSpecialFont02Eng.Style = "font-family: Arial; font-size: 10pt; font-weight: normal; text-align: left; text-" & _
        "decoration: underline; vertical-align: top; white-space: nowrap; ddo-char-set: 1" & _
        ""
            Me.txtSpecialFont02Eng.Text = "vacs@dh.gov.hk"
            Me.txtSpecialFont02Eng.Top = 3.542!
            Me.txtSpecialFont02Eng.Width = 1.06!
            '
            'txtReplaceReason_Chi
            '
            Me.txtReplaceReason_Chi.Height = 0.1875!
            Me.txtReplaceReason_Chi.Left = 0.5!
            Me.txtReplaceReason_Chi.Name = "txtReplaceReason_Chi"
            Me.txtReplaceReason_Chi.Style = "font-family: 新細明體; font-size: 11.25pt; vertical-align: top; ddo-char-set: 1"
            Me.txtReplaceReason_Chi.Text = "<REPLACE_REASON>"
            Me.txtReplaceReason_Chi.Top = 6.92!
            Me.txtReplaceReason_Chi.Width = 6.06!
            '
            'txtDescriptionChi5
            '
            Me.txtDescriptionChi5.Height = 0.57!
            Me.txtDescriptionChi5.Left = 0.0!
            Me.txtDescriptionChi5.LineSpacing = 1.0!
            Me.txtDescriptionChi5.Name = "txtDescriptionChi5"
            Me.txtDescriptionChi5.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: justify; text-justify: distrib" & _
        "ute; vertical-align: top; ddo-char-set: 1"
            Me.txtDescriptionChi5.Text = "如果你對上述安排有任何查詢，請向醫療券事務科 (電話：0000 0001；電郵：                           ) 或項目管理及疫苗計劃科 " & _
        "(電話：0000 0002；電郵：                          ) 查詢。再次多謝你對長者醫療券計劃及 / 或疫苗資助計劃的支持。"
            Me.txtDescriptionChi5.Top = 8.5!
            Me.txtDescriptionChi5.Width = 6.5625!
            '
            'txtSpecialFont01Chi
            '
            Me.txtSpecialFont01Chi.CanGrow = False
            Me.txtSpecialFont01Chi.Height = 0.1875!
            Me.txtSpecialFont01Chi.Left = 5.332!
            Me.txtSpecialFont01Chi.MultiLine = False
            Me.txtSpecialFont01Chi.Name = "txtSpecialFont01Chi"
            Me.txtSpecialFont01Chi.Style = "font-family: Arial; font-size: 10pt; font-weight: normal; text-align: left; text-" & _
        "decoration: underline; vertical-align: bottom; white-space: nowrap; ddo-char-set" & _
        ": 1"
            Me.txtSpecialFont01Chi.Text = "hcvd@dh.gov.hk"
            Me.txtSpecialFont01Chi.Top = 8.5!
            Me.txtSpecialFont01Chi.Width = 1.0625!
            '
            'txtSpecialFont02Chi
            '
            Me.txtSpecialFont02Chi.CanGrow = False
            Me.txtSpecialFont02Chi.Height = 0.1875!
            Me.txtSpecialFont02Chi.Left = 3.625!
        Me.txtSpecialFont02Chi.MultiLine = false
        Me.txtSpecialFont02Chi.Name = "txtSpecialFont02Chi"
        Me.txtSpecialFont02Chi.Style = "font-family: Arial; font-size: 10pt; font-weight: normal; text-align: left; text-"& _ 
    "decoration: underline; vertical-align: bottom; white-space: nowrap; ddo-char-set"& _ 
    ": 1"
        Me.txtSpecialFont02Chi.Text = "vacs@dh.gov.hk"
        Me.txtSpecialFont02Chi.Top = 8.700001!
        Me.txtSpecialFont02Chi.Width = 1.06!
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
        Me.TextBox48.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: center; vertic"& _ 
    "al-align: middle; ddo-char-set: 136"
        Me.TextBox48.Text = "香港特別行政區政府"
        Me.TextBox48.Top = 0!
        Me.TextBox48.Visible = false
        Me.TextBox48.Width = 2.5625!
        '
        'TextBox56
        '
        Me.TextBox56.Height = 0.375!
        Me.TextBox56.Left = 0.0625!
        Me.TextBox56.Name = "TextBox56"
        Me.TextBox56.Style = "font-family: 新細明體; font-size: 12pt; font-weight: bold; text-align: center; ddo-ch"& _ 
    "ar-set: 136"
        Me.TextBox56.Text = "衞 生 署"
        Me.TextBox56.Top = 0.375!
        Me.TextBox56.Visible = false
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
        Me.TextBox57.Visible = false
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
        Me.TextBox58.Visible = false
        Me.TextBox58.Width = 2.5625!
        '
        'Picture1
        '
        Me.Picture1.Height = 1.25!
        Me.Picture1.ImageData = CType(resources.GetObject("Picture1.ImageData"),System.IO.Stream)
        Me.Picture1.Left = 2.625!
        Me.Picture1.Name = "Picture1"
        Me.Picture1.Top = 0!
        Me.Picture1.Visible = false
        Me.Picture1.Width = 1!
        '
        'TextBox59
        '
        Me.TextBox59.Height = 0.3125!
        Me.TextBox59.Left = 3.625!
        Me.TextBox59.Name = "TextBox59"
        Me.TextBox59.Style = "font-family: Arial; font-size: 9pt; font-weight: bold; text-align: center; vertic"& _ 
    "al-align: bottom; ddo-char-set: 0"
        Me.TextBox59.Text = "THE GOVERNMENT OF THE HONG KONG "
        Me.TextBox59.Top = 0!
        Me.TextBox59.Visible = false
        Me.TextBox59.Width = 2.75!
        '
        'TextBox61
        '
        Me.TextBox61.Height = 0.1875!
        Me.TextBox61.Left = 3.625!
        Me.TextBox61.Name = "TextBox61"
        Me.TextBox61.Style = "font-family: Arial; font-size: 9pt; font-weight: bold; text-align: center; vertic"& _ 
    "al-align: top; ddo-char-set: 0"
        Me.TextBox61.Text = "SPECIAL ADMINISTRATIVE REGION"
        Me.TextBox61.Top = 0.3125!
        Me.TextBox61.Visible = false
        Me.TextBox61.Width = 2.75!
        '
        'TextBox69
        '
        Me.TextBox69.Height = 0.1875!
        Me.TextBox69.Left = 3.625!
        Me.TextBox69.Name = "TextBox69"
        Me.TextBox69.Style = "font-family: Arial; font-size: 9pt; font-weight: bold; text-align: center; vertic"& _ 
    "al-align: top; ddo-char-set: 0"
        Me.TextBox69.Text = "DEPARTMENT OF HEALTH"
        Me.TextBox69.Top = 0.5!
        Me.TextBox69.Visible = false
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
        Me.TextBox65.Visible = false
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
        Me.TextBox21.Visible = false
        Me.TextBox21.Width = 2.75!
        '
        'PageFooter1
        '
        Me.PageFooter1.Height = 0.3!
        Me.PageFooter1.Name = "PageFooter1"
        '
        'ReportHeader1
        '
        Me.ReportHeader1.Height = 0!
        Me.ReportHeader1.Name = "ReportHeader1"
        '
        'ReportFooter1
        '
        Me.ReportFooter1.Height = 0!
        Me.ReportFooter1.Name = "ReportFooter1"
        '
        'TokenReplacementLetter
        '
        Me.MasterReport = false
        Me.PageSettings.Margins.Bottom = 0!
        Me.PageSettings.Margins.Top = 0!
        Me.PageSettings.PaperHeight = 11.69!
        Me.PageSettings.PaperWidth = 8.27!
        Me.PrintWidth = 6.65625!
        Me.ScriptLanguage = "VB.NET"
        Me.Sections.Add(Me.ReportHeader1)
        Me.Sections.Add(Me.PageHeader1)
        Me.Sections.Add(Me.Detail1)
        Me.Sections.Add(Me.PageFooter1)
        Me.Sections.Add(Me.ReportFooter1)
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma"& _ 
            "l; font-size: 10pt; color: Black; ddo-char-set: 204", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ddo-char-set: 204", "Heading1", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita"& _ 
            "lic", "Heading2", "Normal"))
        Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
        CType(Me.txtPrintDateEng,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtDearText,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtHeaderEng,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtDescriptionEng1,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtDescriptionEng2,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtDearSPNameChi,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtHeaderChi,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtDescriptionChi1,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtDescriptionChi2,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtDescriptionEng3,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtDescriptionChi3,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtDescriptionEng4,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtDescriptionChi4,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtReplaceReasonEng,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtDescriptionEng5,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtSpecialFont01Eng,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtSpecialFont02Eng,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtReplaceReason_Chi,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtDescriptionChi5,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtSpecialFont01Chi,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.txtSpecialFont02Chi,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.TextBox48,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.TextBox56,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.TextBox57,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.TextBox58,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.Picture1,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.TextBox59,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.TextBox61,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.TextBox69,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.TextBox65,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.TextBox21,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me,System.ComponentModel.ISupportInitialize).EndInit

End Sub
        Friend WithEvents txtPrintDateEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDearText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtHeaderEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionEng1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionEng2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDearSPNameChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtHeaderChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents PageHeader1 As GrapeCity.ActiveReports.SectionReportModel.PageHeader
        Friend WithEvents PageFooter1 As GrapeCity.ActiveReports.SectionReportModel.PageFooter
        Friend WithEvents txtDescriptionEng3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
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
        Friend WithEvents sreLetterHeaderEng As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents PageBreak1 As GrapeCity.ActiveReports.SectionReportModel.PageBreak
        Friend WithEvents sreLetterHeaderChi As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents ReportHeader1 As GrapeCity.ActiveReports.SectionReportModel.ReportHeader
        Friend WithEvents ReportFooter1 As GrapeCity.ActiveReports.SectionReportModel.ReportFooter
        Friend WithEvents sreLetterEndingEng As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents sreLetterEndingChi As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents txtDescriptionEng4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtReplaceReasonEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionEng5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSpecialFont01Eng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSpecialFont02Eng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtReplaceReason_Chi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDescriptionChi5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSpecialFont01Chi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSpecialFont02Chi As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace