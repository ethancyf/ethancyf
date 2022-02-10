

Namespace Component.COVID19.PrintOut.ExemptionCert
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class ExemptionCert
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
        Private WithEvents detExemptionCert As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ExemptionCert))
            Me.detExemptionCert = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.Label7 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label14 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label19 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtNotSuitableCOVID19 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtNotSuitableCOVID19Chi = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label35 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label37 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label39 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label40 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtValidDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtValidDateChi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.srExemptionPaitentName = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.Label17 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label9 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label10 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtPractitionerName = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
            Me.txtPractice = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
            Me.txtIssueDate = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
            Me.Label3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label12 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Line1 = New GrapeCity.ActiveReports.SectionReportModel.Line()
            Me.Line2 = New GrapeCity.ActiveReports.SectionReportModel.Line()
            Me.Label8 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label11 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.qrCode = New GrapeCity.ActiveReports.SectionReportModel.Barcode()
            Me.qrCodeLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label4 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtPrintDate = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label13 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label5 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
            Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
            CType(Me.Label7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label14, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label19, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNotSuitableCOVID19, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNotSuitableCOVID19Chi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label35, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label37, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label39, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label40, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtValidDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtValidDateChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label17, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label9, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label10, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label12, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label8, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label11, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.qrCodeLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label13, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'detExemptionCert
            '
            Me.detExemptionCert.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Label7, Me.Label14, Me.Label19, Me.txtNotSuitableCOVID19, Me.txtNotSuitableCOVID19Chi, Me.Label35, Me.Label37, Me.Label39, Me.Label40, Me.txtValidDate, Me.txtValidDateChi, Me.srExemptionPaitentName, Me.Label17, Me.Label9, Me.Label10, Me.txtPractitionerName, Me.txtPractice, Me.txtIssueDate, Me.Label3, Me.Label12, Me.Line1, Me.Line2})
            Me.detExemptionCert.Height = 6.796333!
            Me.detExemptionCert.Name = "detExemptionCert"
            '
            'Label7
            '
            Me.Label7.Height = 0.22!
            Me.Label7.HyperLink = Nothing
            Me.Label7.Left = 6.486001!
            Me.Label7.Name = "Label7"
            Me.Label7.Style = "font-family: Times New Roman; font-size: 12pt; text-align: center; vertical-align" & _
        ": middle; ddo-char-set: 1"
            Me.Label7.Text = "Date of Issue"
            Me.Label7.Top = 6.571!
            Me.Label7.Width = 0.9369998!
            '
            'Label14
            '
            Me.Label14.Height = 0.22!
            Me.Label14.HyperLink = Nothing
            Me.Label14.Left = 0.6120003!
            Me.Label14.Name = "Label14"
            Me.Label14.Style = "color: Black; font-family: Times New Roman; font-size: 12pt; text-align: left; te" & _
        "xt-justify: auto; vertical-align: top; white-space: inherit; ddo-char-set: 1; dd" & _
        "o-wrap-mode: inherit"
            Me.Label14.Text = "This is to certify that the following person"
            Me.Label14.Top = 1.045!
            Me.Label14.Width = 2.883!
            '
            'Label19
            '
            Me.Label19.Height = 0.2!
            Me.Label19.HyperLink = Nothing
            Me.Label19.Left = 0.612!
            Me.Label19.Name = "Label19"
            Me.Label19.Style = "font-family: PMingLiU; font-size: 12pt; text-align: left; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.Label19.Text = "兹證明以下人士"
            Me.Label19.Top = 0.8450001!
            Me.Label19.Width = 1.257!
            '
            'txtNotSuitableCOVID19
            '
            Me.txtNotSuitableCOVID19.Height = 0.5960001!
            Me.txtNotSuitableCOVID19.HyperLink = Nothing
            Me.txtNotSuitableCOVID19.Left = 0.612!
            Me.txtNotSuitableCOVID19.Name = "txtNotSuitableCOVID19"
            Me.txtNotSuitableCOVID19.Style = "color: Black; font-family: Times New Roman; font-size: 12pt; text-align: left; te" & _
        "xt-justify: auto; vertical-align: top; white-space: inherit; ddo-char-set: 1; dd" & _
        "o-wrap-mode: inherit"
            Me.txtNotSuitableCOVID19.Text = ""
            Me.txtNotSuitableCOVID19.Top = 3.546!
            Me.txtNotSuitableCOVID19.Width = 6.811!
            '
            'txtNotSuitableCOVID19Chi
            '
            Me.txtNotSuitableCOVID19Chi.Height = 0.417!
            Me.txtNotSuitableCOVID19Chi.HyperLink = Nothing
            Me.txtNotSuitableCOVID19Chi.Left = 0.612!
            Me.txtNotSuitableCOVID19Chi.Name = "txtNotSuitableCOVID19Chi"
            Me.txtNotSuitableCOVID19Chi.Style = "font-family: PMingLiU; font-size: 12pt; text-align: left; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.txtNotSuitableCOVID19Chi.Text = ""
            Me.txtNotSuitableCOVID19Chi.Top = 3.169!
            Me.txtNotSuitableCOVID19Chi.Width = 6.811!
            '
            'Label35
            '
            Me.Label35.Height = 0.22!
            Me.Label35.HyperLink = Nothing
            Me.Label35.Left = 0.612!
            Me.Label35.Name = "Label35"
            Me.Label35.Style = "color: Black; font-family: Times New Roman; font-size: 12pt; text-align: left; te" & _
        "xt-justify: auto; vertical-align: middle; white-space: inherit; ddo-char-set: 1;" & _
        " ddo-wrap-mode: inherit"
            Me.Label35.Text = "This certification remains valid until"
            Me.Label35.Top = 4.524!
            Me.Label35.Width = 2.48!
            '
            'Label37
            '
            Me.Label37.Height = 0.22!
            Me.Label37.HyperLink = Nothing
            Me.Label37.Left = 0.612!
            Me.Label37.Name = "Label37"
            Me.Label37.Style = "font-family: PMingLiU; font-size: 12pt; text-align: left; vertical-align: top; dd" & _
        "o-char-set: 1"
            Me.Label37.Text = "此證明書的有效期直至"
            Me.Label37.Top = 4.323999!
            Me.Label37.Width = 1.731!
            '
            'Label39
            '
            Me.Label39.Height = 0.22!
            Me.Label39.HyperLink = Nothing
            Me.Label39.Left = 4.6!
            Me.Label39.Name = "Label39"
            Me.Label39.Style = "color: Black; font-family: Times New Roman; font-size: 12pt; text-align: center; " & _
        "text-justify: auto; vertical-align: top; white-space: inherit; ddo-char-set: 1; " & _
        "ddo-wrap-mode: inherit"
            Me.Label39.Text = "Name of Registered Medical Practitioner"
            Me.Label39.Top = 5.359999!
            Me.Label39.Width = 2.798!
            '
            'Label40
            '
            Me.Label40.Height = 0.2200005!
            Me.Label40.HyperLink = Nothing
            Me.Label40.Left = 3.555!
            Me.Label40.Name = "Label40"
            Me.Label40.Style = "font-family: PMingLiU; font-size: 12pt; text-align: center; vertical-align: top; " & _
        "ddo-char-set: 1"
            Me.Label40.Text = "註冊醫生姓名"
            Me.Label40.Top = 5.36!
            Me.Label40.Width = 1.045001!
            '
            'txtValidDate
            '
            Me.txtValidDate.Height = 0.2!
            Me.txtValidDate.Left = 3.092!
            Me.txtValidDate.Name = "txtValidDate"
            Me.txtValidDate.Style = "font-family: Times New Roman; font-size: 12pt; ddo-char-set: 0"
            Me.txtValidDate.Text = Nothing
            Me.txtValidDate.Top = 4.524!
            Me.txtValidDate.Width = 1.634!
            '
            'txtValidDateChi
            '
            Me.txtValidDateChi.Height = 0.2!
            Me.txtValidDateChi.Left = 2.301!
            Me.txtValidDateChi.Name = "txtValidDateChi"
            Me.txtValidDateChi.Style = "font-size: 12pt; text-align: left; ddo-char-set: 1"
            Me.txtValidDateChi.Text = Nothing
            Me.txtValidDateChi.Top = 4.323999!
            Me.txtValidDateChi.Width = 2.0!
            '
            'srExemptionPaitentName
            '
            Me.srExemptionPaitentName.CloseBorder = False
            Me.srExemptionPaitentName.Height = 1.56!
            Me.srExemptionPaitentName.Left = 0.612!
            Me.srExemptionPaitentName.Name = "srExemptionPaitentName"
            Me.srExemptionPaitentName.Report = Nothing
            Me.srExemptionPaitentName.ReportName = "srExemptionPaitentName"
            Me.srExemptionPaitentName.Top = 1.408!
            Me.srExemptionPaitentName.Width = 6.811!
            '
            'Label17
            '
            Me.Label17.Height = 0.22!
            Me.Label17.HyperLink = Nothing
            Me.Label17.Left = 5.786!
            Me.Label17.Name = "Label17"
            Me.Label17.Style = "font-family: PMingLiU; font-size: 12pt; text-align: center; vertical-align: top; " & _
        "ddo-char-set: 1"
            Me.Label17.Text = "發出日期"
            Me.Label17.Top = 6.571!
            Me.Label17.Width = 0.7000003!
            '
            'Label9
            '
            Me.Label9.Height = 0.22!
            Me.Label9.HyperLink = Nothing
            Me.Label9.Left = 6.032!
            Me.Label9.Name = "Label9"
            Me.Label9.Style = "font-family: Times New Roman; font-size: 12pt; text-align: center; vertical-align" & _
        ": middle; ddo-char-set: 1"
            Me.Label9.Text = "Healthcare Provider"
            Me.Label9.Top = 5.972!
            Me.Label9.Width = 1.390999!
            '
            'Label10
            '
            Me.Label10.Height = 0.22!
            Me.Label10.HyperLink = Nothing
            Me.Label10.Left = 5.316!
            Me.Label10.Name = "Label10"
            Me.Label10.Style = "font-family: PMingLiU; font-size: 12pt; text-align: center; vertical-align: top; " & _
        "ddo-char-set: 1"
            Me.Label10.Text = "醫護機構 "
            Me.Label10.Top = 5.972!
            Me.Label10.Width = 0.7160001!
            '
            'txtPractitionerName
            '
            Me.txtPractitionerName.AutoReplaceFields = True
            Me.txtPractitionerName.Font = New System.Drawing.Font("Arial", 10.0!)
            Me.txtPractitionerName.Height = 0.22!
            Me.txtPractitionerName.Left = 0.6120015!
            Me.txtPractitionerName.Name = "txtPractitionerName"
            Me.txtPractitionerName.RTF = resources.GetString("txtPractitionerName.RTF")
            Me.txtPractitionerName.Top = 5.14!
            Me.txtPractitionerName.Width = 6.795!
            '
            'txtPractice
            '
            Me.txtPractice.AutoReplaceFields = True
            Me.txtPractice.Font = New System.Drawing.Font("Arial", 10.0!)
            Me.txtPractice.Height = 0.22!
            Me.txtPractice.Left = 0.611999!
            Me.txtPractice.Name = "txtPractice"
            Me.txtPractice.RTF = resources.GetString("txtPractice.RTF")
            Me.txtPractice.Top = 5.752001!
            Me.txtPractice.Width = 6.811!
            '
            'txtIssueDate
            '
            Me.txtIssueDate.AutoReplaceFields = True
            Me.txtIssueDate.Font = New System.Drawing.Font("Arial", 10.0!)
            Me.txtIssueDate.Height = 0.22!
            Me.txtIssueDate.Left = 4.758!
            Me.txtIssueDate.Name = "txtIssueDate"
            Me.txtIssueDate.RTF = resources.GetString("txtIssueDate.RTF")
            Me.txtIssueDate.Top = 6.351!
            Me.txtIssueDate.Width = 2.665!
            '
            'Label3
            '
            Me.Label3.Height = 0.272!
            Me.Label3.HyperLink = Nothing
            Me.Label3.Left = 0.612!
            Me.Label3.Name = "Label3"
            Me.Label3.Style = resources.GetString("Label3.Style")
            Me.Label3.Text = "COVID-19 Vaccination Medical Exemption Certificate"
            Me.Label3.Top = 0.305!
            Me.Label3.Width = 6.811!
            '
            'Label12
            '
            Me.Label12.Height = 0.284!
            Me.Label12.HyperLink = Nothing
            Me.Label12.Left = 0.612!
            Me.Label12.Name = "Label12"
            Me.Label12.Style = resources.GetString("Label12.Style")
            Me.Label12.Text = "新冠疫苗接種醫學豁免證明書"
            Me.Label12.Top = 0.06299996!
            Me.Label12.Width = 6.811!
            '
            'Line1
            '
            Me.Line1.Height = 0.0!
            Me.Line1.Left = 2.722!
            Me.Line1.LineWeight = 3.0!
            Me.Line1.Name = "Line1"
            Me.Line1.Top = 0.283!
            Me.Line1.Width = 2.594!
            Me.Line1.X1 = 2.722!
            Me.Line1.X2 = 5.316!
            Me.Line1.Y1 = 0.283!
            Me.Line1.Y2 = 0.283!
            '
            'Line2
            '
            Me.Line2.Height = 0.0!
            Me.Line2.Left = 1.752!
            Me.Line2.LineWeight = 3.0!
            Me.Line2.Name = "Line2"
            Me.Line2.Top = 0.517!
            Me.Line2.Width = 4.542001!
            Me.Line2.X1 = 1.752!
            Me.Line2.X2 = 6.294001!
            Me.Line2.Y1 = 0.517!
            Me.Line2.Y2 = 0.517!
            '
            'Label8
            '
            Me.Label8.Height = 0.2309997!
            Me.Label8.HyperLink = Nothing
            Me.Label8.Left = 0.687!
            Me.Label8.Name = "Label8"
            Me.Label8.Style = "font-family: PMingLiU; font-size: 12pt; text-align: center; vertical-align: top; " & _
        "ddo-char-set: 1"
            Me.Label8.Text = "本函由電腦系統編印，毋需簽署。"
            Me.Label8.Top = 0.0!
            Me.Label8.Width = 6.811!
            '
            'Label11
            '
            Me.Label11.Height = 0.2420008!
            Me.Label11.HyperLink = Nothing
            Me.Label11.Left = 0.687!
            Me.Label11.Name = "Label11"
            Me.Label11.Style = "color: Black; font-family: Times New Roman; font-size: 12pt; text-align: center; " & _
        "text-justify: auto; vertical-align: middle; white-space: inherit; ddo-char-set: " & _
        "1; ddo-wrap-mode: inherit"
            Me.Label11.Text = "This is a computer-generated print-out. No signature is required."
            Me.Label11.Top = 0.2309999!
            Me.Label11.Width = 6.811!
            '
            'Label6
            '
            Me.Label6.Height = 0.335!
            Me.Label6.HyperLink = Nothing
            Me.Label6.Left = 1.05!
            Me.Label6.Name = "Label6"
            Me.Label6.Style = "color: #085296; font-family: Times New Roman; font-size: 18.75pt; font-weight: bo" & _
        "ld; text-align: center; ddo-char-set: 1"
            Me.Label6.Text = "COVID-19 Vaccination Programme"
            Me.Label6.Top = 2.08!
            Me.Label6.Width = 5.882!
            '
            'Label2
            '
            Me.Label2.DataField = ""
            Me.Label2.Height = 0.335!
            Me.Label2.HyperLink = Nothing
            Me.Label2.Left = 1.05!
            Me.Label2.Name = "Label2"
            Me.Label2.Style = "color: #085296; font-family: PMingLiU; font-size: 18.75pt; font-weight: bold; tex" & _
        "t-align: center; text-decoration: none; ddo-char-set: 1"
            Me.Label2.Text = "2019 冠狀病毒病疫苗接種計劃" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            Me.Label2.Top = 1.41!
            Me.Label2.Width = 5.91!
            '
            'Label1
            '
            Me.Label1.Height = 0.335!
            Me.Label1.HyperLink = Nothing
            Me.Label1.Left = 1.883!
            Me.Label1.Name = "Label1"
            Me.Label1.Style = "color: #085296; font-family: PMingLiU; font-size: 18.75pt; font-weight: bold; tex" & _
        "t-align: center; text-decoration: none; ddo-char-set: 1"
            Me.Label1.Text = "香港特別行政區政府" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            Me.Label1.Top = 1.075!
            Me.Label1.Width = 4.244!
            '
            'qrCode
            '
            Me.qrCode.Font = New System.Drawing.Font("Courier New", 8.0!)
            Me.qrCode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(8, Byte), Integer), CType(CType(82, Byte), Integer), CType(CType(150, Byte), Integer))
            Me.qrCode.Height = 1.218056!
            Me.qrCode.Left = 6.529473!
            Me.qrCode.Name = "qrCode"
            Me.qrCode.QuietZoneBottom = 0.0!
            Me.qrCode.QuietZoneLeft = 0.0!
            Me.qrCode.QuietZoneRight = 0.0!
            Me.qrCode.QuietZoneTop = 0.0!
            Me.qrCode.Style = GrapeCity.ActiveReports.SectionReportModel.BarCodeStyle.QRCode
            Me.qrCode.Text = "Barcode1"
            Me.qrCode.Top = 0.357!
            Me.qrCode.Width = 1.218056!
            '
            'qrCodeLabel
            '
            Me.qrCodeLabel.Height = 0.1389999!
            Me.qrCodeLabel.HyperLink = Nothing
            Me.qrCodeLabel.Left = 6.471!
            Me.qrCodeLabel.Name = "qrCodeLabel"
            Me.qrCodeLabel.Style = "font-family: PMingLiU; font-size: 7.5pt; text-align: center; vertical-align: top;" & _
        " ddo-char-set: 1"
            Me.qrCodeLabel.Text = "二維碼紀錄 QR Code Record"
            Me.qrCodeLabel.Top = 1.606!
            Me.qrCodeLabel.Width = 1.335!
            '
            'Label4
            '
            Me.Label4.Height = 0.335!
            Me.Label4.HyperLink = Nothing
            Me.Label4.Left = 0.08500013!
            Me.Label4.Name = "Label4"
            Me.Label4.Style = "color: #085296; font-family: Times New Roman; font-size: 18.75pt; font-weight: bo" & _
        "ld; text-align: center; ddo-char-set: 1"
            Me.Label4.Text = "The Government of the Hong Kong Special Administrative Region"
            Me.Label4.Top = 1.745!
            Me.Label4.Width = 7.811!
            '
            'txtTransactionNumber
            '
            Me.txtTransactionNumber.Height = 0.25!
            Me.txtTransactionNumber.HyperLink = Nothing
            Me.txtTransactionNumber.Left = 0.0!
            Me.txtTransactionNumber.Name = "txtTransactionNumber"
            Me.txtTransactionNumber.Style = "font-family: Times New Roman; font-size: 13.75pt; text-align: left; ddo-char-set:" & _
        " 1"
            Me.txtTransactionNumber.Text = ""
            Me.txtTransactionNumber.Top = 0.691!
            Me.txtTransactionNumber.Visible = False
            Me.txtTransactionNumber.Width = 2.343!
            '
            'txtPrintDate
            '
            Me.txtPrintDate.Height = 0.25!
            Me.txtPrintDate.HyperLink = Nothing
            Me.txtPrintDate.Left = 4.625!
            Me.txtPrintDate.Name = "txtPrintDate"
            Me.txtPrintDate.Style = "font-family: Times New Roman; font-size: 13.75pt; font-weight: normal; text-align" & _
        ": right; ddo-char-set: 1"
            Me.txtPrintDate.Text = ""
            Me.txtPrintDate.Top = 0.691!
            Me.txtPrintDate.Width = 3.27!
            '
            'Label13
            '
            Me.Label13.Height = 0.25!
            Me.Label13.HyperLink = Nothing
            Me.Label13.Left = 3.26!
            Me.Label13.Name = "Label13"
            Me.Label13.Style = "font-family: Times New Roman; font-size: 13.75pt; font-weight: bold; text-align: " & _
        "center; ddo-char-set: 1"
            Me.Label13.Text = "Keep this record properly"
            Me.Label13.Top = 0.691!
            Me.Label13.Width = 2.201!
            '
            'Label5
            '
            Me.Label5.Height = 0.25!
            Me.Label5.HyperLink = Nothing
            Me.Label5.Left = 2.09!
            Me.Label5.Name = "Label5"
            Me.Label5.Style = "font-family: PMingLiU; font-size: 14.25pt; font-weight: bold; text-align: center;" & _
        " ddo-char-set: 0"
            Me.Label5.Text = "請妥善保存"
            Me.Label5.Top = 0.691!
            Me.Label5.Width = 1.17!
            '
            'PageHeader1
            '
            Me.PageHeader1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Label6, Me.Label2, Me.Label1, Me.qrCode, Me.qrCodeLabel, Me.Label4})
            Me.PageHeader1.Height = 2.551582!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'PageFooter1
            '
            Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTransactionNumber, Me.txtPrintDate, Me.Label13, Me.Label5, Me.Label8, Me.Label11})
            Me.PageFooter1.Height = 1.1605!
            Me.PageFooter1.Name = "PageFooter1"
            '
            'ExemptionCert
            '
            Me.MasterReport = False
            Me.PageSettings.DefaultPaperSize = False
            Me.PageSettings.Margins.Bottom = 0.0!
            Me.PageSettings.Margins.Left = 0.185!
            Me.PageSettings.Margins.Right = 0.0!
            Me.PageSettings.Margins.Top = 0.1!
            Me.PageSettings.PaperHeight = 11.69291!
            Me.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4
            Me.PageSettings.PaperWidth = 8.267716!
            Me.PrintWidth = 8.184!
            Me.Sections.Add(Me.PageHeader1)
            Me.Sections.Add(Me.detExemptionCert)
            Me.Sections.Add(Me.PageFooter1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
                "color: Black; font-family: ""PMingLiU""; ddo-char-set: 136", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-weight: bold; font-family: ""PMingLiU""; ddo-char-set: 136; font-size: 48pt", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic; ddo-char-set: 204", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"))
            CType(Me.Label7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label14, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label19, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNotSuitableCOVID19, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNotSuitableCOVID19Chi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label35, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label37, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label39, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label40, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtValidDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtValidDateChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label17, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label9, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label10, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label12, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label8, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label11, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.qrCodeLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label13, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents PageHeader1 As GrapeCity.ActiveReports.SectionReportModel.PageHeader
        Private WithEvents PageFooter1 As GrapeCity.ActiveReports.SectionReportModel.PageFooter
        Private WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label1 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents qrCode As GrapeCity.ActiveReports.SectionReportModel.Barcode
        Private WithEvents qrCodeLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label4 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtTransactionNumber As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtPrintDate As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label13 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label5 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label7 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label14 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label19 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtNotSuitableCOVID19 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtNotSuitableCOVID19Chi As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label35 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label37 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label39 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label40 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtValidDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtValidDateChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents srExemptionPaitentName As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents Label17 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label8 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label11 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label9 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label10 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtPractitionerName As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
        Private WithEvents txtPractice As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
        Private WithEvents txtIssueDate As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
        Private WithEvents Label3 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label12 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Line1 As GrapeCity.ActiveReports.SectionReportModel.Line
        Private WithEvents Line2 As GrapeCity.ActiveReports.SectionReportModel.Line
    End Class
End Namespace
