Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Format
Imports common.ComFunction
Imports Common.Validation

Public Class DetailedPaymentAnalysisRpt 
    Private Reimburse_ID As String
    Private m_dsData As DataSet
    Public intRecCount As Integer
    Dim udtFormatter As New Common.Format.Formatter()

    Public Sub New(ByVal dsData As DataSet, Optional ByVal bWithWatermark As Boolean = False)

        intRecCount = 1
        'ByVal strReimburseID As String,
        'Reimburse_ID = strReimburseID
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

    Private Sub DetailedPaymentAnalysisRpt_FetchData(ByVal sender As Object, ByVal eArgs As GrapeCity.ActiveReports.SectionReport.FetchEventArgs) Handles Me.FetchData


    End Sub

    Private Sub DetailedPaymentAnalysisRpt_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        'If Me.m_dsData.Tables.Count > 0 Then
        '    Me.DataSource = Me.m_dsData.Tables(0).Select("", "SP_ID_PRACTICE ASC")
        'End If
        Me.DataSource = Me.m_dsData.Tables(0)
        ' CRE12-007 - Enhancement on DPAR [Start][Tommy L]
        ' -------------------------------------------------------------
        'Dim udtFormatter As New Formatter
        ' CRE12-007 - Enhancement on DPAR [End][Tommy L]
        Me.txtCutoffDate.Text = Me.m_dsData.Tables(1).Rows(0)("CutOffDate").ToString().Trim()
        Me.lblReimburseID.Text = "(" & Me.m_dsData.Tables(1).Rows(0)("ReimburseID").ToString().Trim() & "-" & Me.m_dsData.Tables(1).Rows(0)("SchemeCode").ToString().Trim() & ")"
        Me.lblReportDate.Text = udtFormatter.convertDateTime(Me.m_dsData.Tables(0).Rows(0)("Report_Date").ToString().Trim(), "en")
        Me.lblSchemeText.Text = Me.m_dsData.Tables(1).Rows(0)("SchemeCode").ToString().Trim()

        ' Report code
        Dim udtGeneralFunction As New GeneralFunction
        Dim strSchemeCode As String = CStr(m_dsData.Tables(1).Rows(0)("Scheme_Code")).Trim

        udtGeneralFunction.getSystemParameter("DetailedPaymentAnalysisReportCode", lblRptNo.Text, String.Empty, strSchemeCode)

        'Dim udtReimbursementBll As New ReimbursementBLL()
        'Dim dt As New DataTable
        'Dim txtAppLink As String = String.Empty
        'Dim txtEmail As String = String.Empty
        'Dim txtActivationWeeks As String = String.Empty
        'Dim GeneralFunction As GeneralFunction = New GeneralFunction
        'dt = udtReimbursementBll.GetDetailedAnalysisReportByReimburseID(Reimburse_ID)
        'Me.DataSource = dt

        'Me.txtCutoffDate.Text = "28-Oct-2008 (" & Reimburse_ID & ")"   '@cutoff_Date_str

        'Set report orientation to Landscape
        Me.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape
    End Sub

    ' CRE12-007 - Enhancement on DPAR [Start][Tommy L]
    ' -------------------------------------------------------------
    'Private Sub GroupHeaderServiceProvider_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupHeaderServiceProvider.Format
    'Me.txtRecCount.Text = Me.intRecCount.ToString
    'Me.txtTotalRecCount.Text = Me.intRecCount.ToString
    'intRecCount = intRecCount + 1
    'Me.TextBox3.Text = udtFormatter.maskBankAccount(Me.TextBox3.Text)
    'End Sub

    Private Sub Detail1_Format(ByVal sender As Object, ByVal e As System.EventArgs) Handles Detail1.Format
        Me.txtRecCount.Text = Me.intRecCount.ToString
        Me.txtTotalRecCount.Text = Me.intRecCount.ToString
        intRecCount = intRecCount + 1
        Me.txtBankAccNo.Text = udtFormatter.maskBankAccount(Me.txtBankAccNo.Text)
    End Sub
    ' CRE12-007 - Enhancement on DPAR [End][Tommy L]

    Private Sub PageHeader1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageHeader1.Format

    End Sub

    Private Sub ReportFooter1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReportFooter1.Format

    End Sub
End Class
