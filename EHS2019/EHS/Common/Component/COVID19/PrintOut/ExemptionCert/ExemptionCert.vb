Imports GrapeCity.ActiveReports
Imports GrapeCity.ActiveReports.Document
Imports GrapeCity.ActiveReports.SectionReportModel
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.COVID19.PrintOut.Common
Imports Common.Component.COVID19.PrintOut.Common.QrCodeFormatter
Imports Common.Component.COVID19.PrintOut.Common.Format.Formatter
Imports Common.Component.COVID19.PrintOut.Common.PrintoutHelper
'Imports HCSP.BLL


Namespace Component.COVID19.PrintOut.ExemptionCert
    Public Class ExemptionCert

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtSchemeClaim As SchemeClaimModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtSP As ServiceProviderModel

        ' Helper class
        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction
        Private _udtGeneralFunction As GeneralFunction
        Private _udtPrintoutHelper As PrintoutHelper = New PrintoutHelper()

        'Date for QR code printing
        Private _udtPrintTime As Date
        'Setting for blank sample of vaccination card
        Private _blnIsSample As Boolean


#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Initialize helper object
            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction
            _udtGeneralFunction = New GeneralFunction
            _udtPrintTime = Date.Now
            '_udtPrintTime = "2022-01-25 14:30"

        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel,
                       ByRef udtEHSAccount As EHSAccountModel)

            Me.New()

            ' Init variable
            _udtEHSTransaction = udtEHSTransaction
            _udtEHSAccount = udtEHSAccount
            _udtSP = Nothing
            '_udtSmartIDContent = udtSmartIDContent

            'Setting for blank sample of vaccination card true = printSample, false = print normal form
            _blnIsSample = False

            LoadReport()
            ChkIsSample()

        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel,
                       ByRef udtEHSAccount As EHSAccountModel,
                       ByVal udtSP As ServiceProviderModel)

            Me.New()

            ' Init variable
            _udtEHSTransaction = udtEHSTransaction
            _udtEHSAccount = udtEHSAccount
            _udtSP = udtSP
            '_udtSmartIDContent = udtSmartIDContent

            'Setting for blank sample of vaccination card true = printSample, false = print normal form
            _blnIsSample = False

            LoadReport()
            ChkIsSample()

        End Sub
#End Region

        Private Sub LoadReport()
      
            'Header
            qrCode.Text = (New QrcodeFormatter).GenerateQRCodeStringForExemptionRecord(_udtEHSTransaction, _udtEHSAccount, _udtPrintTime)
            'qrCode.Text = "1586416616aberg1451eg6e4rs1t6ru16rt1u/r6t1u15dshf456cxbxjfjdjt4655347854757646444764dfgjdhdfghfgh4652rhfhg54ew61te5r1g3d0g5dr1y6er1t1e51g15g6e1r6e61e61g6e1h61jht6rs16r1h6r1jh6fr1h6r537634"

            'Sub Report
            srExemptionPaitentName.Report = New Covid19VaccinationCard.Covid19PatientName(_udtEHSAccount, _blnIsSample, FormType.Exemption)

            'Details

            Me.txtNotSuitableCOVID19.Text = HttpContext.GetGlobalResourceObject("Text", "ExemptionCertNotSuitableCOVID19", New System.Globalization.CultureInfo(CultureLanguage.English))
            Me.txtNotSuitableCOVID19Chi.Text = HttpContext.GetGlobalResourceObject("Text", "ExemptionCertNotSuitableCOVID19", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))


            Me.txtValidDateChi.Text = FormatDisplayDateChinese(_udtEHSTransaction.TransactionAdditionFields.ValidUntil) & "。"
            Me.txtValidDate.Text = FormatDisplayDate(_udtEHSTransaction.TransactionAdditionFields.ValidUntil) & "."

            'Me.txtValidDateChi.Text = FormatDisplayDateChinese("2022-04-01") & "。"
            'Me.txtValidDate.Text = FormatDisplayDate("2022-04-01") & "."

            If _udtSP IsNot Nothing Then
                txtPractitionerName.Html = String.Format("<span style='font-family:MingLiU_HKSCS-ExtB'>{0}</span>", _udtSP.ChineseName)
                txtPractitionerName.Html += String.Format("&nbsp<span style='font-family:Times New Roman'>{0}</span>", _udtSP.EnglishName)
            Else
                txtPractitionerName.Html = String.Format("<span style='font-family:MingLiU_HKSCS-ExtB'>{0}</span>", _udtEHSTransaction.ServiceProviderNameChi)
                txtPractitionerName.Html += String.Format("&nbsp<span style='font-family:Times New Roman'>{0}</span>", _udtEHSTransaction.ServiceProviderName)
            End If

            'txtPractitionerName.Html = String.Format("<span style='font-family:MingLiU_HKSCS-ExtB'>{0}</span>", "陳大文")
            'txtPractitionerName.Html += String.Format("&nbsp<span style='font-family:Times New Roman'>{0}</span>", "CHAN, TAI MAN")

            txtPractitionerName.SelectionStart = 0
            txtPractitionerName.SelectionLength = txtPractitionerName.Text.Length
            txtPractitionerName.SelectionColor = Drawing.Color.Black
            txtPractitionerName.SelectionAlignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right

            'Me.txtPracticeChi.Text = ""
            'Me.txtPractice.Text = _udtEHSTransaction.PracticeNameChi & "  " & _udtEHSTransaction.PracticeName

            txtPractice.Html = String.Format("<span style='font-family:MingLiU_HKSCS-ExtB'>{0}</span>", _udtEHSTransaction.PracticeNameChi)
            txtPractice.Html += String.Format("&nbsp<span style='font-family:Times New Roman'>{0}</span>", _udtEHSTransaction.PracticeName)

            'txtPractice.Html = String.Format("<span style='font-family:MingLiU_HKSCS-ExtB'>{0}</span>", "陳大文醫務所")
            'txtPractice.Html += String.Format("&nbsp<span style='font-family:Times New Roman'>{0}</span>", "CHAN TAI MAN Clinic")

            txtPractice.SelectionStart = 0
            txtPractice.SelectionLength = txtPractice.Text.Length
            txtPractice.SelectionColor = Drawing.Color.Black
            txtPractice.SelectionAlignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right

            'Me.txtIssueDateChi.Text = ""
            txtIssueDate.Html = String.Format("<span style='font-family:PMingLiU'>{0}</span>", FormatDisplayDateChinese(_udtEHSTransaction.ServiceDate))
            txtIssueDate.Html += String.Format("&nbsp<span style='font-family:Times New Roman'>{0}</span>", FormatDisplayDate(_udtEHSTransaction.ServiceDate))
            txtIssueDate.SelectionStart = 0
            txtIssueDate.SelectionLength = txtIssueDate.Text.Length
            txtIssueDate.SelectionColor = Drawing.Color.Black
            txtIssueDate.SelectionAlignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right


            'Footer
            Me.txtPrintDate.Text = "Printed on " + FormatDisplayClock(_udtPrintTime)

            'Transaction No.
            Me.txtTransactionNumber.Text = "Ref: " + _udtFormatter.formatSystemNumber(_udtEHSTransaction.TransactionID)

            If _udtPrintoutHelper.DisplayPrintoutRefNo(PrintoutHelper.FormType.Exemption) Then
                Me.txtTransactionNumber.Visible = True
            Else
                Me.txtTransactionNumber.Visible = False
            End If

        End Sub

        Private Sub ChkIsSample()
            If (_blnIsSample) Then
                qrCode.Visible = False
                qrCodeLabel.Visible = False
                Me.txtPrintDate.Visible = False
                Me.txtTransactionNumber.Visible = False
            End If

        End Sub
#Region "Report Event"
        Private Sub Covid19VaccinationCard_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            'Me.Document.Printer.PrinterName = ""
        End Sub
#End Region

        Private Sub PageFooter1_Format(sender As Object, e As EventArgs) Handles PageFooter1.Format

        End Sub
    End Class
End Namespace

