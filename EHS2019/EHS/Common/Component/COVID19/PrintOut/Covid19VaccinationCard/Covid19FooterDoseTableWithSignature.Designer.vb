
Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Covid19FooterDoseTableWithSignature
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Covid19FooterDoseTableWithSignature))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.qrcode2 = New GrapeCity.ActiveReports.SectionReportModel.Barcode()
            Me.qrcode3 = New GrapeCity.ActiveReports.SectionReportModel.Barcode()
            Me.qrcode4 = New GrapeCity.ActiveReports.SectionReportModel.Barcode()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.qrcode2, Me.qrcode3, Me.qrcode4})
            Me.Detail.Height = 1.98!
            Me.Detail.Name = "Detail"
            '
            'qrcode2
            '
            Me.qrcode2.Font = New System.Drawing.Font("Courier New", 8.0!)
            Me.qrcode2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.qrcode2.Height = 0.8!
            Me.qrcode2.Left = 1.928!
            Me.qrcode2.Name = "qrcode2"
            Me.qrcode2.QuietZoneBottom = 0.0!
            Me.qrcode2.QuietZoneLeft = 0.0!
            Me.qrcode2.QuietZoneRight = 0.0!
            Me.qrcode2.QuietZoneTop = 0.0!
            Me.qrcode2.Style = GrapeCity.ActiveReports.SectionReportModel.BarCodeStyle.QRCode
            Me.qrcode2.Text = "Barcode1"
            Me.qrcode2.Top = 0.647!
            Me.qrcode2.Width = 0.8!
            '
            'qrcode3
            '
            Me.qrcode3.Font = New System.Drawing.Font("Courier New", 8.0!)
            Me.qrcode3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.qrcode3.Height = 0.6!
            Me.qrcode3.Left = 1.149!
            Me.qrcode3.Name = "qrcode3"
            Me.qrcode3.QuietZoneBottom = 0.0!
            Me.qrcode3.QuietZoneLeft = 0.0!
            Me.qrcode3.QuietZoneRight = 0.0!
            Me.qrcode3.QuietZoneTop = 0.0!
            Me.qrcode3.Style = GrapeCity.ActiveReports.SectionReportModel.BarCodeStyle.QRCode
            Me.qrcode3.Text = "Barcode1"
            Me.qrcode3.Top = 0.8470001!
            Me.qrcode3.Width = 0.6!
            '
            'qrcode4
            '
            Me.qrcode4.Font = New System.Drawing.Font("Courier New", 8.0!)
            Me.qrcode4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.qrcode4.Height = 0.5!
            Me.qrcode4.Left = 0.45!
            Me.qrcode4.Name = "qrcode4"
            Me.qrcode4.QuietZoneBottom = 0.0!
            Me.qrcode4.QuietZoneLeft = 0.0!
            Me.qrcode4.QuietZoneRight = 0.0!
            Me.qrcode4.QuietZoneTop = 0.0!
            Me.qrcode4.Style = GrapeCity.ActiveReports.SectionReportModel.BarCodeStyle.QRCode
            Me.qrcode4.Text = "Barcode1"
            Me.qrcode4.Top = 0.947!
            Me.qrcode4.Width = 0.5!
            '
            'Covid19FooterDoseTableWithSignature
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.0!
            Me.PageSettings.PaperWidth = 8.5!
            Me.PrintWidth = 4.003!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black; ddo-char-set: 204", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents qrcode2 As GrapeCity.ActiveReports.SectionReportModel.Barcode
        Private WithEvents qrcode3 As GrapeCity.ActiveReports.SectionReportModel.Barcode
        Private WithEvents qrcode4 As GrapeCity.ActiveReports.SectionReportModel.Barcode
    End Class
End Namespace
