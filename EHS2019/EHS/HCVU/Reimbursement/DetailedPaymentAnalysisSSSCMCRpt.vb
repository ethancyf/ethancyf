Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Format
Imports common.ComFunction
Imports Common.Validation
Imports Common.Component

Public Class DetailedPaymentAnalysisSSSCMCRpt

    Private m_dsData As DataSet
    Public intRecCount As Integer
    Dim udtFormatter As New Common.Format.Formatter()

    Public Sub New(ByVal dsData As DataSet, Optional ByVal bWithWatermark As Boolean = False)
        intRecCount = 1
        Me.m_dsData = dsData

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.DataSource = Nothing

        If Not bWithWatermark Then
            Me.Watermark.Dispose()
            Me.Watermark = Nothing
        End If
    End Sub

    Private Sub DetailedPaymentAnalysisRmbRpt_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
        Me.DataSource = Me.m_dsData.Tables(0)

        Dim dtmCutoffDate As DateTime = Me.m_dsData.Tables(1).Rows(0)("CutOffDate")

        Me.txtCutoffDate.Text = udtFormatter.formatDisplayDate(dtmCutoffDate, CultureLanguage.SimpChinese)
        Me.lblReimburseID.Text = "(" & Me.m_dsData.Tables(1).Rows(0)("ReimburseID").ToString().Trim() & "-" & Me.m_dsData.Tables(1).Rows(0)("SchemeCode").ToString().Trim() & ")"
        Me.lblReportDate.Text = udtFormatter.convertDateTime(Me.m_dsData.Tables(0).Rows(0)("Report_Date").ToString().Trim(), CultureLanguage.SimpChinese)
        Me.lblSchemeText.Text = Me.m_dsData.Tables(1).Rows(0)("SchemeCode").ToString().Trim()
        Me.rinfoPrintedOn.FormatString = "For internal use: printed by " & Me.m_dsData.Tables(1).Rows(0)("UserName").ToString().ToUpper().Trim() & " on {RunDateTime:dd MMM yyyy hh:mm}"

        ' Report code
        Dim udtGeneralFunction As New GeneralFunction
        Dim strSchemeCode As String = CStr(m_dsData.Tables(1).Rows(0)("Scheme_Code")).Trim

        udtGeneralFunction.getSystemParameter("DetailedPaymentAnalysisReportCode", lblRptNo.Text, String.Empty, strSchemeCode)

        'Set report orientation to Landscape
        Me.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape
    End Sub

    Private Sub Detail1_Format(ByVal sender As Object, ByVal e As System.EventArgs) Handles Detail1.Format

        'header
        lblAmountText.Html = "<br>从特别支援计划户口扣除的金额 (&yen;)"
        lblAmountText.SelectionStart = 0
        lblAmountText.SelectionLength = lblAmountText.Text.Length
        lblAmountText.SelectionColor = Drawing.Color.Black
        lblAmountText.SelectionFont = New System.Drawing.Font("新細明體", 10)
        lblAmountText.SelectionAlignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Left

        lblAmountReductionText.Html = "<br>由特别支援计划支付的減免费用 (&yen;)"
        lblAmountReductionText.SelectionStart = 0
        lblAmountReductionText.SelectionLength = lblAmountReductionText.Text.Length
        lblAmountReductionText.SelectionColor = Drawing.Color.Black
        lblAmountReductionText.SelectionFont = New System.Drawing.Font("新細明體", 10)
        lblAmountReductionText.SelectionAlignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Left

        lblSupportFeeText.Html = "<br>由特别支援计划承担的总额 (&yen;)"
        lblSupportFeeText.SelectionStart = 0
        lblSupportFeeText.SelectionLength = lblSupportFeeText.Text.Length
        lblSupportFeeText.SelectionColor = Drawing.Color.Black
        lblSupportFeeText.SelectionFont = New System.Drawing.Font("新細明體", 10)
        lblSupportFeeText.SelectionAlignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Left

        'summary
        lblTotalAmountText.Html = "从特别支援计划户口扣除的总金额 (&yen;)："
        lblTotalAmountText.SelectionStart = 0
        lblTotalAmountText.SelectionLength = lblTotalAmountText.Text.Length
        lblTotalAmountText.SelectionFont = New System.Drawing.Font("新細明體", 10.0!, Drawing.FontStyle.Bold)
        lblTotalAmountText.SelectionAlignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right
        txtTotalAmount.Text = udtFormatter.formatMoneyRMB(txtTotalAmount.Text, True)

        lblTotalAmountReductionText.Html = "由特别支援计划支付的減免总费用 (&yen;)："
        lblTotalAmountReductionText.SelectionStart = 0
        lblTotalAmountReductionText.SelectionLength = lblTotalAmountReductionText.Text.Length
        lblTotalAmountReductionText.SelectionFont = New System.Drawing.Font("新細明體", 10.0!, Drawing.FontStyle.Bold)
        lblTotalAmountReductionText.SelectionAlignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right
        txtTotalReductionFee.Text = udtFormatter.formatMoneyRMB(txtTotalReductionFee.Text, True)

        lblTotalSupportFeeText.Html = "由特别支援计划承担的总金额 (&yen;)："
        lblTotalSupportFeeText.SelectionStart = 0
        lblTotalSupportFeeText.SelectionLength = lblTotalSupportFeeText.Text.Length
        lblTotalSupportFeeText.SelectionFont = New System.Drawing.Font("新細明體", 10.0!, Drawing.FontStyle.Bold)
        lblTotalSupportFeeText.SelectionAlignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Right
        txtTotalSupportFee.Text = udtFormatter.formatMoneyRMB(txtTotalSupportFee.Text, True)

        Me.txtRecCount.Text = Me.intRecCount.ToString
        Me.txtTotalRecCount.Text = Me.intRecCount.ToString
        intRecCount = intRecCount + 1


    End Sub

End Class
