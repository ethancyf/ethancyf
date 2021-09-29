

Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Covid19VaccinationCard
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
        Private WithEvents detCovid19VaccinationCard As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Covid19VaccinationCard))
            Me.srCovid19DoseTable = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.detCovid19VaccinationCard = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.srCovid19VaccinationCardFooter = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.srCovid19PatientName = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Picture2 = New GrapeCity.ActiveReports.SectionReportModel.Picture()
            Me.Line1 = New GrapeCity.ActiveReports.SectionReportModel.Line()
            Me.Picture3 = New GrapeCity.ActiveReports.SectionReportModel.Picture()
            Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.qrCode = New GrapeCity.ActiveReports.SectionReportModel.Barcode()
            Me.qrCodeLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label4 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtTransactionNumber = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtPrintDate = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label13 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label5 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader()
            Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter()
            CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Picture2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Picture3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.qrCodeLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label13, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'srCovid19DoseTable
            '
            Me.srCovid19DoseTable.CloseBorder = False
            Me.srCovid19DoseTable.Height = 6.87!
            Me.srCovid19DoseTable.Left = 0.0!
            Me.srCovid19DoseTable.Name = "srCovid19DoseTable"
            Me.srCovid19DoseTable.Report = Nothing
            Me.srCovid19DoseTable.ReportName = "srCovid19DoseTable"
            Me.srCovid19DoseTable.Top = 4.124!
            Me.srCovid19DoseTable.Width = 8.0!
            '
            'detCovid19VaccinationCard
            '
            Me.detCovid19VaccinationCard.CanGrow = False
            Me.detCovid19VaccinationCard.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.srCovid19DoseTable, Me.srCovid19VaccinationCardFooter, Me.srCovid19PatientName, Me.Label6, Me.Picture2, Me.Line1, Me.Picture3, Me.Label2, Me.Label1, Me.Label3, Me.qrCode, Me.qrCodeLabel, Me.Label4, Me.txtTransactionNumber, Me.txtPrintDate, Me.Label13, Me.Label5})
            Me.detCovid19VaccinationCard.Height = 11.447!
            Me.detCovid19VaccinationCard.Name = "detCovid19VaccinationCard"
            '
            'srCovid19VaccinationCardFooter
            '
            Me.srCovid19VaccinationCardFooter.CloseBorder = False
            Me.srCovid19VaccinationCardFooter.Height = 2.008!
            Me.srCovid19VaccinationCardFooter.Left = 0.0!
            Me.srCovid19VaccinationCardFooter.Name = "srCovid19VaccinationCardFooter"
            Me.srCovid19VaccinationCardFooter.Report = Nothing
            Me.srCovid19VaccinationCardFooter.ReportName = "srCovid19VaccinationCardFooter"
            Me.srCovid19VaccinationCardFooter.Top = 9.066!
            Me.srCovid19VaccinationCardFooter.Width = 8.0!
            '
            'srCovid19PatientName
            '
            Me.srCovid19PatientName.CloseBorder = False
            Me.srCovid19PatientName.Height = 1.532!
            Me.srCovid19PatientName.Left = 0.0!
            Me.srCovid19PatientName.Name = "srCovid19PatientName"
            Me.srCovid19PatientName.Report = Nothing
            Me.srCovid19PatientName.ReportName = "srCovid19PatientName"
            Me.srCovid19PatientName.Top = 2.53!
            Me.srCovid19PatientName.Width = 8.0!
            '
            'Label6
            '
            Me.Label6.Height = 0.335!
            Me.Label6.HyperLink = Nothing
            Me.Label6.Left = 1.059!
            Me.Label6.Name = "Label6"
            Me.Label6.Style = "color: #085296; font-family: Times New Roman; font-size: 18.75pt; font-weight: bo" & _
        "ld; text-align: center; ddo-char-set: 1"
            Me.Label6.Text = "COVID-19 Vaccination Record"
            Me.Label6.Top = 2.08!
            Me.Label6.Width = 5.882!
            '
            'Picture2
            '
            Me.Picture2.Height = 0.716!
            Me.Picture2.ImageData = CType(resources.GetObject("Picture2.ImageData"), System.IO.Stream)
            Me.Picture2.Left = 3.616!
            Me.Picture2.Name = "Picture2"
            Me.Picture2.SizeMode = GrapeCity.ActiveReports.SectionReportModel.SizeModes.Stretch
            Me.Picture2.Top = 0.0!
            Me.Picture2.Width = 0.768!
            '
            'Line1
            '
            Me.Line1.Height = 0.02024937!
            Me.Line1.Left = 0.000001430512!
            Me.Line1.LineStyle = GrapeCity.ActiveReports.SectionReportModel.LineStyle.Dot
            Me.Line1.LineWeight = 1.0!
            Me.Line1.Name = "Line1"
            Me.Line1.Top = 8.873751!
            Me.Line1.Width = 7.879999!
            Me.Line1.X1 = 7.88!
            Me.Line1.X2 = 0.000001430512!
            Me.Line1.Y1 = 8.894!
            Me.Line1.Y2 = 8.873751!
            '
            'Picture3
            '
            Me.Picture3.Height = 0.1654996!
            Me.Picture3.ImageData = CType(resources.GetObject("Picture3.ImageData"), System.IO.Stream)
            Me.Picture3.Left = 3.875!
            Me.Picture3.Name = "Picture3"
            Me.Picture3.SizeMode = GrapeCity.ActiveReports.SectionReportModel.SizeModes.Stretch
            Me.Picture3.Top = 8.801!
            Me.Picture3.Width = 0.1870003!
            '
            'Label2
            '
            Me.Label2.DataField = ""
            Me.Label2.Height = 0.335!
            Me.Label2.HyperLink = Nothing
            Me.Label2.Left = 1.045!
            Me.Label2.Name = "Label2"
            Me.Label2.Style = "color: #085296; font-family: PMingLiU; font-size: 18.75pt; font-weight: bold; tex" & _
        "t-align: center; text-decoration: none; ddo-char-set: 1"
            Me.Label2.Text = "2019 冠狀病毒病疫苗接種紀錄" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            Me.Label2.Top = 1.075!
            Me.Label2.Width = 5.91!
            '
            'Label1
            '
            Me.Label1.Height = 0.335!
            Me.Label1.HyperLink = Nothing
            Me.Label1.Left = 1.878!
            Me.Label1.Name = "Label1"
            Me.Label1.Style = "color: #085296; font-family: PMingLiU; font-size: 18.75pt; font-weight: bold; tex" & _
        "t-align: center; text-decoration: none; ddo-char-set: 1"
            Me.Label1.Text = "香港特別行政區政府衞生署" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            Me.Label1.Top = 0.74!
            Me.Label1.Width = 4.244!
            '
            'Label3
            '
            Me.Label3.Height = 0.335!
            Me.Label3.HyperLink = Nothing
            Me.Label3.Left = 1.059!
            Me.Label3.Name = "Label3"
            Me.Label3.Style = "color: #085296; font-family: Times New Roman; font-size: 18.75pt; font-weight: bo" & _
        "ld; text-align: center; ddo-char-set: 1"
            Me.Label3.Text = "Department of Health"
            Me.Label3.Top = 1.41!
            Me.Label3.Width = 5.882!
            '
            'qrCode
            '
            Me.qrCode.Font = New System.Drawing.Font("Courier New", 8.0!)
            Me.qrCode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(8, Byte), Integer), CType(CType(82, Byte), Integer), CType(CType(150, Byte), Integer))
            Me.qrCode.Height = 1.218056!
            Me.qrCode.Left = 6.538473!
            Me.qrCode.Name = "qrCode"
            Me.qrCode.QuietZoneBottom = 0.0!
            Me.qrCode.QuietZoneLeft = 0.0!
            Me.qrCode.QuietZoneRight = 0.0!
            Me.qrCode.QuietZoneTop = 0.0!
            Me.qrCode.Style = GrapeCity.ActiveReports.SectionReportModel.BarCodeStyle.QRCode
            Me.qrCode.Text = "Barcode1"
            Me.qrCode.Top = 0.3569999!
            Me.qrCode.Width = 1.218056!
            '
            'qrCodeLabel
            '
            Me.qrCodeLabel.Height = 0.1389999!
            Me.qrCodeLabel.HyperLink = Nothing
            Me.qrCodeLabel.Left = 6.48!
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
            Me.Label4.Left = 0.094!
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
            Me.txtTransactionNumber.Left = 0.01!
            Me.txtTransactionNumber.Name = "txtTransactionNumber"
            Me.txtTransactionNumber.Style = "font-family: Times New Roman; font-size: 13.75pt; text-align: left; ddo-char-set:" & _
        " 1"
            Me.txtTransactionNumber.Text = ""
            Me.txtTransactionNumber.Top = 11.043!
            Me.txtTransactionNumber.Width = 2.343!
            '
            'txtPrintDate
            '
            Me.txtPrintDate.Height = 0.25!
            Me.txtPrintDate.HyperLink = Nothing
            Me.txtPrintDate.Left = 4.635!
            Me.txtPrintDate.Name = "txtPrintDate"
            Me.txtPrintDate.Style = "font-family: Times New Roman; font-size: 13.75pt; font-weight: normal; text-align" & _
        ": right; ddo-char-set: 1"
            Me.txtPrintDate.Text = ""
            Me.txtPrintDate.Top = 11.043!
            Me.txtPrintDate.Width = 3.27!
            '
            'Label13
            '
            Me.Label13.Height = 0.25!
            Me.Label13.HyperLink = Nothing
            Me.Label13.Left = 3.27!
            Me.Label13.Name = "Label13"
            Me.Label13.Style = "font-family: Times New Roman; font-size: 13.75pt; font-weight: bold; text-align: " & _
        "center; ddo-char-set: 1"
            Me.Label13.Text = "Keep this record properly"
            Me.Label13.Top = 11.043!
            Me.Label13.Width = 2.201!
            '
            'Label5
            '
            Me.Label5.Height = 0.25!
            Me.Label5.HyperLink = Nothing
            Me.Label5.Left = 2.1!
            Me.Label5.Name = "Label5"
            Me.Label5.Style = "font-family: PMingLiU; font-size: 14.25pt; font-weight: bold; text-align: center;" & _
        " ddo-char-set: 0"
            Me.Label5.Text = "請妥善保存"
            Me.Label5.Top = 11.043!
            Me.Label5.Width = 1.17!
            '
            'PageHeader1
            '
            Me.PageHeader1.Height = 0.03125!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'PageFooter1
            '
            Me.PageFooter1.Height = 0.0!
            Me.PageFooter1.Name = "PageFooter1"
            '
            'Covid19VaccinationCard
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
            Me.PrintWidth = 8.0!
            Me.Sections.Add(Me.PageHeader1)
            Me.Sections.Add(Me.detCovid19VaccinationCard)
            Me.Sections.Add(Me.PageFooter1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
                "color: Black; font-family: ""PMingLiU""; ddo-char-set: 136", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-weight: bold; font-family: ""PMingLiU""; ddo-char-set: 136; font-size: 48pt", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic; ddo-char-set: 204", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 128", "Heading3", "Normal"))
            CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Picture2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Picture3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.qrCodeLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTransactionNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label13, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents srCovid19DoseTable As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents Picture2 As GrapeCity.ActiveReports.SectionReportModel.Picture
        Private WithEvents srCovid19PatientName As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents Line1 As GrapeCity.ActiveReports.SectionReportModel.Line
        Private WithEvents Picture3 As GrapeCity.ActiveReports.SectionReportModel.Picture
        Private WithEvents srCovid19VaccinationCardFooter As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents PageHeader1 As GrapeCity.ActiveReports.SectionReportModel.PageHeader
        Private WithEvents PageFooter1 As GrapeCity.ActiveReports.SectionReportModel.PageFooter
        Private WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label1 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label3 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents qrCode As GrapeCity.ActiveReports.SectionReportModel.Barcode
        Private WithEvents qrCodeLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label4 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtTransactionNumber As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtPrintDate As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label13 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label5 As GrapeCity.ActiveReports.SectionReportModel.Label

    End Class
End Namespace
