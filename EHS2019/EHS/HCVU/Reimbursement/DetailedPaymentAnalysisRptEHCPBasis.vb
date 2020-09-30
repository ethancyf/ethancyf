Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Format
Imports common.ComFunction
Imports Common.Validation
Imports Common.Component.Scheme.SchemeClaimModel

Public Class DetailedPaymentAnalysisRptEHCPBasis
    ' CRE17-004 Generate a new DPAR on EHCP basis [Start][Dickson]
    Private Reimburse_ID As String
    Private m_dsData As DataSet
    Dim udtFormatter As New Common.Format.Formatter()

    Public Sub New(ByVal dsData As DataSet, Optional ByVal bWithWatermark As Boolean = False)

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
        Me.DataSource = Me.m_dsData.Tables(0)
        Me.txtCutoffDate.Text = Me.m_dsData.Tables(1).Rows(0)("CutOffDate").ToString().Trim()
        Me.lblReimburseID.Text = "(" & Me.m_dsData.Tables(1).Rows(0)("ReimburseID").ToString().Trim() & "-" & Me.m_dsData.Tables(1).Rows(0)("SchemeCode").ToString().Trim() & ")"
        Me.lblReportDate.Text = udtFormatter.convertDateTime(Me.m_dsData.Tables(1).Rows(0)("ReportDate").ToString().Trim(), "en")
        Me.lblSchemeText.Text = Me.m_dsData.Tables(1).Rows(0)("SchemeCode").ToString().Trim()
        Me.txtTotalVerCases.Text = Me.m_dsData.Tables(1).Rows(0)("TotalVerCases").ToString().Trim()
        Me.txtTotalSP.Text = Me.m_dsData.Tables(1).Rows(0)("TotalSP").ToString().Trim()
        Me.txtTotalSpidPractice.Text = Me.m_dsData.Tables(1).Rows(0)("TotalSPPractice").ToString().Trim()
        ' Report code
        Dim udtGeneralFunction As New GeneralFunction
        Dim strSchemeCode As String = CStr(m_dsData.Tables(1).Rows(0)("Scheme_Code")).Trim

        udtGeneralFunction.getSystemParameter("DetailedPaymentAnalysisReportEHCPCode", lblRptNo.Text, String.Empty, strSchemeCode)

        'CRE17-004 Generate a new DPAR on EHCP basis [Start][Martin]
        If Me.m_dsData.Tables(1).Rows(0)("SchemeCode").ToString().Trim() = HCVSDHC Then
            Me.lblVerificationCase.Visible = False
            Me.txtVerificationCase.Visible = False
            Me.lblTotalVerCasesText.Visible = False
            Me.txtTotalVerCases.Visible = False
        End If
        'CRE17-004 Generate a new DPAR on EHCP basis [End][Martin]
        Me.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape
    End Sub


    Private Sub Detail1_Format(ByVal sender As Object, ByVal e As System.EventArgs) Handles Detail1.Format

    End Sub

    Private Sub PageHeader1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageHeader1.Format

    End Sub

    Private Sub ReportFooter1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReportFooter1.Format

    End Sub
    ' CRE17-004 Generate a new DPAR on EHCP basis [End][Dickson]
End Class
