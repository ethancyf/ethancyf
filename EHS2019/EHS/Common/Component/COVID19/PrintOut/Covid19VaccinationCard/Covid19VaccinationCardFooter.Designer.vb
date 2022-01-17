Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Covid19VaccinationCardFooter
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Covid19VaccinationCardFooter))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.Shape1 = New GrapeCity.ActiveReports.SectionReportModel.Shape()
            Me.txtHKID = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Shape2 = New GrapeCity.ActiveReports.SectionReportModel.Shape()
            Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label5 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Picture1 = New GrapeCity.ActiveReports.SectionReportModel.Picture()
            Me.Label4 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtName = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label14 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtDocType = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Line1 = New GrapeCity.ActiveReports.SectionReportModel.Line()
            Me.Line2 = New GrapeCity.ActiveReports.SectionReportModel.Line()
            Me.txtNameChi = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.txtDocTypeChi = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label18 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.srCovid19FooterDoseTable = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.txtEngNameOnly = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.qrCode = New GrapeCity.ActiveReports.SectionReportModel.Barcode()
            Me.qrCodeLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            CType(Me.txtHKID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Picture1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label14, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDocType, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDocTypeChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label18, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtEngNameOnly, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.qrCodeLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Shape1, Me.txtHKID, Me.Shape2, Me.Label6, Me.Label5, Me.Picture1, Me.Label4, Me.Label3, Me.txtName, Me.Label14, Me.txtDocType, Me.Line1, Me.Line2, Me.txtNameChi, Me.txtDocTypeChi, Me.Label18, Me.srCovid19FooterDoseTable, Me.txtEngNameOnly, Me.qrCode, Me.qrCodeLabel})
            Me.Detail.Height = 2.007833!
            Me.Detail.Name = "Detail"
            '
            'Shape1
            '
            Me.Shape1.Height = 1.992!
            Me.Shape1.Left = 0.0!
            Me.Shape1.Name = "Shape1"
            Me.Shape1.RoundingRadius = New GrapeCity.ActiveReports.Controls.CornersRadius(10.0!, Nothing, Nothing, Nothing, Nothing)
            Me.Shape1.Top = 0.0!
            Me.Shape1.Width = 3.938!
            '
            'txtHKID
            '
            Me.txtHKID.Height = 0.2120001!
            Me.txtHKID.HyperLink = Nothing
            Me.txtHKID.Left = 1.416!
            Me.txtHKID.Name = "txtHKID"
            Me.txtHKID.ShrinkToFit = True
            Me.txtHKID.Style = "font-family: Times New Roman; font-size: 10pt; ddo-char-set: 1; ddo-shrink-to-fit" & _
        ": true"
            Me.txtHKID.Text = ""
            Me.txtHKID.Top = 1.726!
            Me.txtHKID.Width = 2.476!
            '
            'Shape2
            '
            Me.Shape2.Height = 1.992!
            Me.Shape2.Left = 3.938!
            Me.Shape2.Name = "Shape2"
            Me.Shape2.RoundingRadius = New GrapeCity.ActiveReports.Controls.CornersRadius(10.0!, Nothing, Nothing, Nothing, Nothing)
            Me.Shape2.Top = 0.0!
            Me.Shape2.Width = 3.937!
            '
            'Label6
            '
            Me.Label6.Height = 0.123!
            Me.Label6.HyperLink = Nothing
            Me.Label6.Left = 1.527!
            Me.Label6.Name = "Label6"
            Me.Label6.Style = "color: #085296; font-family: Times New Roman; font-size: 9.25pt; font-weight: bol" & _
        "d; text-align: left; vertical-align: middle; ddo-char-set: 1"
            Me.Label6.Text = "COVID-19 Vaccination Record"
            Me.Label6.Top = 0.203!
            Me.Label6.Width = 2.362!
            '
            'Label5
            '
            Me.Label5.Height = 0.164!
            Me.Label5.HyperLink = Nothing
            Me.Label5.Left = 1.527!
            Me.Label5.Name = "Label5"
            Me.Label5.Style = "color: #085296; font-family: PMingLiU; font-size: 10.2pt; font-weight: bold; text" & _
        "-align: left; vertical-align: top; ddo-char-set: 1"
            Me.Label5.Text = "2019 冠狀病毒病疫苗接種紀錄"
            Me.Label5.Top = 0.0453!
            Me.Label5.Width = 2.39!
            '
            'Picture1
            '
            Me.Picture1.Height = 0.248!
            Me.Picture1.HyperLink = Nothing
            Me.Picture1.ImageData = CType(resources.GetObject("Picture1.ImageData"), System.IO.Stream)
            Me.Picture1.Left = 1.24!
            Me.Picture1.Name = "Picture1"
            Me.Picture1.SizeMode = GrapeCity.ActiveReports.SectionReportModel.SizeModes.Stretch
            Me.Picture1.Top = 0.04!
            Me.Picture1.Width = 0.2870007!
            '
            'Label4
            '
            Me.Label4.Height = 0.1619999!
            Me.Label4.HyperLink = Nothing
            Me.Label4.Left = 1.417!
            Me.Label4.Name = "Label4"
            Me.Label4.Style = "font-family: PMingLiU; font-size: 8.5pt; text-align: left; ddo-char-set: 1"
            Me.Label4.Text = "身份證明文件類別及號碼"
            Me.Label4.Top = 1.033!
            Me.Label4.Width = 1.335!
            '
            'Label3
            '
            Me.Label3.Height = 0.193!
            Me.Label3.HyperLink = Nothing
            Me.Label3.Left = 1.719!
            Me.Label3.Name = "Label3"
            Me.Label3.Style = "font-family: Times New Roman; font-size: 8.5pt; text-align: left; vertical-align:" & _
        " bottom; ddo-char-set: 1"
            Me.Label3.Text = "Name"
            Me.Label3.Top = 0.32!
            Me.Label3.Width = 0.4380001!
            '
            'txtName
            '
            Me.txtName.Height = 0.3859999!
            Me.txtName.HyperLink = Nothing
            Me.txtName.Left = 2.437!
            Me.txtName.Name = "txtName"
            Me.txtName.ShrinkToFit = True
            Me.txtName.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "bottom; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.txtName.Text = ""
            Me.txtName.Top = 0.522!
            Me.txtName.Width = 1.48!
            '
            'Label14
            '
            Me.Label14.Height = 0.168!
            Me.Label14.HyperLink = Nothing
            Me.Label14.Left = 2.752!
            Me.Label14.Name = "Label14"
            Me.Label14.Style = "font-family: Times New Roman; font-size: 8.5pt; text-align: left; ddo-char-set: 1" & _
        ""
            Me.Label14.Text = "Document Type & No. "
            Me.Label14.Top = 1.033!
            Me.Label14.Width = 1.251!
            '
            'txtDocType
            '
            Me.txtDocType.Height = 0.4889997!
            Me.txtDocType.HyperLink = Nothing
            Me.txtDocType.Left = 2.437!
            Me.txtDocType.Name = "txtDocType"
            Me.txtDocType.ShrinkToFit = True
            Me.txtDocType.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "bottom; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.txtDocType.Text = ""
            Me.txtDocType.Top = 1.237!
            Me.txtDocType.Width = 1.48!
            '
            'Line1
            '
            Me.Line1.Height = 0.0!
            Me.Line1.Left = 1.416!
            Me.Line1.LineWeight = 1.0!
            Me.Line1.Name = "Line1"
            Me.Line1.Top = 1.938!
            Me.Line1.Width = 2.501!
            Me.Line1.X1 = 1.416!
            Me.Line1.X2 = 3.917!
            Me.Line1.Y1 = 1.938!
            Me.Line1.Y2 = 1.938!
            '
            'Line2
            '
            Me.Line2.Height = 0.001999915!
            Me.Line2.Left = 1.416!
            Me.Line2.LineWeight = 1.0!
            Me.Line2.Name = "Line2"
            Me.Line2.Top = 0.9080001!
            Me.Line2.Width = 2.501!
            Me.Line2.X1 = 1.416!
            Me.Line2.X2 = 3.917!
            Me.Line2.Y1 = 0.91!
            Me.Line2.Y2 = 0.9080001!
            '
            'txtNameChi
            '
            Me.txtNameChi.Height = 0.3859998!
            Me.txtNameChi.HyperLink = Nothing
            Me.txtNameChi.Left = 1.417!
            Me.txtNameChi.Name = "txtNameChi"
            Me.txtNameChi.ShrinkToFit = True
            Me.txtNameChi.Style = resources.GetString("txtNameChi.Style")
            Me.txtNameChi.Text = ""
            Me.txtNameChi.Top = 0.522!
            Me.txtNameChi.Width = 0.9579999!
            '
            'txtDocTypeChi
            '
            Me.txtDocTypeChi.Height = 0.4889998!
            Me.txtDocTypeChi.HyperLink = Nothing
            Me.txtDocTypeChi.Left = 1.417!
            Me.txtDocTypeChi.Name = "txtDocTypeChi"
            Me.txtDocTypeChi.ShrinkToFit = True
            Me.txtDocTypeChi.Style = "font-family: PMingLiU; font-size: 10pt; text-align: left; vertical-align: bottom;" & _
        " ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.txtDocTypeChi.Text = ""
            Me.txtDocTypeChi.Top = 1.237!
            Me.txtDocTypeChi.Width = 0.9579999!
            '
            'Label18
            '
            Me.Label18.Height = 0.1929998!
            Me.Label18.HyperLink = Nothing
            Me.Label18.Left = 1.416!
            Me.Label18.Name = "Label18"
            Me.Label18.Style = "font-family: PMingLiU; font-size: 8.5pt; text-align: left; vertical-align: bottom" & _
        "; ddo-char-set: 1"
            Me.Label18.Text = "姓名"
            Me.Label18.Top = 0.326!
            Me.Label18.Width = 0.3030001!
            '
            'srCovid19FooterDoseTable
            '
            Me.srCovid19FooterDoseTable.CloseBorder = False
            Me.srCovid19FooterDoseTable.Height = 1.992!
            Me.srCovid19FooterDoseTable.Left = 3.938!
            Me.srCovid19FooterDoseTable.Name = "srCovid19FooterDoseTable"
            Me.srCovid19FooterDoseTable.Report = Nothing
            Me.srCovid19FooterDoseTable.ReportName = "srCovid19FooterDoseTable"
            Me.srCovid19FooterDoseTable.Top = 0.0!
            Me.srCovid19FooterDoseTable.Width = 3.937!
            '
            'txtEngNameOnly
            '
            Me.txtEngNameOnly.Height = 0.3859999!
            Me.txtEngNameOnly.HyperLink = Nothing
            Me.txtEngNameOnly.Left = 1.416!
            Me.txtEngNameOnly.Name = "txtEngNameOnly"
            Me.txtEngNameOnly.ShrinkToFit = True
            Me.txtEngNameOnly.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "bottom; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.txtEngNameOnly.Text = ""
            Me.txtEngNameOnly.Top = 0.522!
            Me.txtEngNameOnly.Width = 2.501!
            '
            'qrCode
            '
            Me.qrCode.Font = New System.Drawing.Font("Courier New", 8.0!)
            Me.qrCode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(8, Byte), Integer), CType(CType(82, Byte), Integer), CType(CType(150, Byte), Integer))
            Me.qrCode.Height = 1.35!
            Me.qrCode.Left = 0.03150001!
            Me.qrCode.Name = "qrCode"
            Me.qrCode.QuietZoneBottom = 0.0!
            Me.qrCode.QuietZoneLeft = 0.0!
            Me.qrCode.QuietZoneRight = 0.0!
            Me.qrCode.QuietZoneTop = 0.0!
            Me.qrCode.Style = GrapeCity.ActiveReports.SectionReportModel.BarCodeStyle.QRCode
            Me.qrCode.Text = "Barcode1"
            Me.qrCode.Top = 0.326!
            Me.qrCode.Width = 1.35!
            '
            'qrCodeLabel
            '
            Me.qrCodeLabel.Height = 0.212!
            Me.qrCodeLabel.HyperLink = Nothing
            Me.qrCodeLabel.Left = 0.039!
            Me.qrCodeLabel.Name = "qrCodeLabel"
            Me.qrCodeLabel.Style = "font-family: PMingLiU; font-size: 7.5pt; text-align: center; ddo-char-set: 1"
            Me.qrCodeLabel.Text = "二維碼紀錄 QR Code Record"
            Me.qrCodeLabel.Top = 1.726!
            Me.qrCodeLabel.Width = 1.335!
            '
            'Covid19VaccinationCardFooter
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.0!
            Me.PageSettings.PaperWidth = 8.5!
            Me.PrintWidth = 7.889!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
                "color: Black; font-family: ""PMingLiU""; ddo-char-set: 136", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ddo-char-set: 0", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic; ddo-char-set: 0", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ddo-char-set: 0", "Heading3", "Normal"))
            CType(Me.txtHKID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Picture1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label14, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDocType, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDocTypeChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label18, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtEngNameOnly, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.qrCodeLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents Shape1 As GrapeCity.ActiveReports.SectionReportModel.Shape
        Private WithEvents Shape2 As GrapeCity.ActiveReports.SectionReportModel.Shape
        Private WithEvents Picture1 As GrapeCity.ActiveReports.SectionReportModel.Picture
        Private WithEvents Label4 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label3 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtName As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label5 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label14 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtDocType As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Line2 As GrapeCity.ActiveReports.SectionReportModel.Line
        Private WithEvents txtNameChi As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents txtDocTypeChi As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Line1 As GrapeCity.ActiveReports.SectionReportModel.Line
        Private WithEvents txtHKID As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label18 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents srCovid19FooterDoseTable As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Private WithEvents txtEngNameOnly As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents qrCode As GrapeCity.ActiveReports.SectionReportModel.Barcode
        Private WithEvents qrCodeLabel As GrapeCity.ActiveReports.SectionReportModel.Label
    End Class
End Namespace