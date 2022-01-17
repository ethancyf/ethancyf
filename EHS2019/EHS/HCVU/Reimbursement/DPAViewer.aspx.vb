Imports System.Data.SqlClient
Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel
Imports Common.Format
Imports Common.Component
Imports Common.Component.HCVUUser


Partial Public Class DPAViewer
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Change to read the data from Session variables
        'Dim strRID As String = Page.Request.QueryString("RID").ToString.Trim
        'Dim strCutoffDate As String = Page.Request.QueryString("strCutoffDate").ToString.Trim
        'Dim bWatermark As Boolean = Page.Request.QueryString("bWatermark").ToString.Trim.Equals("Y")
        Dim strRID As String = Session("RID").ToString.Trim
        Dim strCutoffDate As String = Session("strCutoffDate").ToString.Trim
        Dim bWatermark As Boolean = Session("bWatermark").ToString.Trim.Equals("Y")
        Dim strDPAScheme As String = Session("DPAScheme").ToString.Trim
        Dim strReportSelected As String = Session("ReportSelected")
        Dim strReportFileName As String = "" 'CRE17-004 Generate a new DPAR on EHCP basis [Martin]

        ' CRE17-004 Generate a new DPAR on EHCP basis [Star][Dickson]
        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Dim rpt As Object = Nothing

        If (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(strDPAScheme).ReimbursementCurrency = EnumReimbursementCurrency.HKD Then
            If strReportSelected = DPAReportType.EHCP Then
                Dim DSEHCPbasis As DataSet = Me.GetEHCPBasisReportSource(strRID, strCutoffDate, strDPAScheme)
                rpt = New DetailedPaymentAnalysisRptEHCPBasis(DSEHCPbasis, bWatermark)
                strReportFileName = "DetailedPaymentAnalysisRptEHCP.PDF"
            Else
                rpt = New DetailedPaymentAnalysisRpt(Me.GetReportSource(strRID, strCutoffDate, strDPAScheme), bWatermark)
                strReportFileName = "DetailedPaymentAnalysisRptPractice.PDF"
            End If
            'CRE20-015-02 (Special Support Scheme) [Start][Martin]
        ElseIf (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(strDPAScheme).ReimbursementCurrency = EnumReimbursementCurrency.RMB Then
            rpt = New DetailedPaymentAnalysisSSSCMCRpt(Me.GetReportSource(strRID, strCutoffDate, strDPAScheme), bWatermark)
            strReportFileName = "DetailedPaymentAnalysisRptPractice.PDF"
            'CRE20-015-02 (Special Support Scheme) [End][Martin]
        Else
            If strReportSelected = DPAReportType.EHCP Then
                rpt = New DetailedPaymentAnalysisRmbRptEHCPBasis(Me.GetEHCPBasisReportSource(strRID, strCutoffDate, strDPAScheme), bWatermark)
                strReportFileName = "DetailedPaymentAnalysisRptEHCP.PDF"
            Else
                rpt = New DetailedPaymentAnalysisRmbRpt(Me.GetReportSource(strRID, strCutoffDate, strDPAScheme), bWatermark)
                strReportFileName = "DetailedPaymentAnalysisRptPractice.PDF"
            End If
        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        Try
            rpt.Run(False)
        Catch eRunReport As GrapeCity.ActiveReports.ReportException
            ' Failure running report, just report the error to the user:
            Response.Clear()
            Response.Write("<h1>Error running report:</h1>")
            Response.Write(eRunReport.ToString())
            Return
        End Try

        ' CRE17-004 Generate a new DPAR on EHCP basis [End][Dickson]

        ' Tell the browser this is a PDF document so it will use an appropriate viewer.
        ' If the report has been exported in a different format, the content-type will 
        ' need to be changed as noted in the following table:
        '  ExportType  ContentType
        '    PDF       "application/pdf"  (needs to be in lowercase)
        Response.ContentType = "application/pdf"

        ' IE & Acrobat seam to require "content-disposition" header being in the response.  If you don't add it, the doc still works most of the time, but not always.
        'this makes a new window appear: Response.AddHeader("content-disposition","attachment; filename=MyPDF.PDF");
        Response.AddHeader("content-disposition", "inline; filename=" & strReportFileName) 'CRE17-004 Generate a new DPAR on EHCP basis [Martin]

        ' Create the PDF export object
        Dim pdf As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport()

        ' Set fallback font
        pdf.FontFallback = Common.Component.Printout.PrintoutBLL.FallbackFont()

        'CRE17-004 Generate a new DPAR on EHCP basis [Start][Martin]
        pdf.Security.Encrypt = True
        pdf.Security.UserPassword = Session("ReportPassword").ToString.Trim
        pdf.Security.Use128Bit = True
        'CRE17-004 Generate a new DPAR on EHCP basis [End][Martin]

        ' Create a new memory stream that will hold the pdf output
        Dim memStream As New System.IO.MemoryStream()
        ' Export the report to PDF:
        pdf.Export(rpt.Document, memStream)
        ' Write the PDF stream out
        Response.BinaryWrite(memStream.ToArray())
        ' Send all buffered content to the client
        Response.End()
    End Sub

    'CRE20-015-02 (Special Support Scheme) [Start][Martin]
    Private Function GetReportSource(ByVal strRimbeID As String, ByVal strCutoffDate As String, ByVal strSchemeCode As String) As DataSet
        Dim dsData As New DataSet()

        Dim udtDB As New Common.DataAccess.Database()
        udtDB.CommandTimeout = 300

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser



        Dim params() As SqlParameter = New SqlParameter() { _
            udtDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strRimbeID), _
            udtDB.MakeInParam("@cutoff_Date_str", SqlDbType.Char, 11, strCutoffDate), _
            udtDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode), _
            udtDB.MakeInParam("@User_ID", SqlDbType.Char, 20, udtHCVUUser.UserID)}
        udtDB.RunProc("proc_DPAReport_get", params, dsData)

        ' Format Total Amount RMB
        Dim udtFormatter As New Formatter

        dsData.Tables(0).Columns.Add("Total_Amount_RMB_Text", GetType(String))
        dsData.Tables(0).Columns.Add("Total_Reduction_Fee_Text", GetType(String))
        dsData.Tables(0).Columns.Add("Total_Support_Fee_Text", GetType(String))

        For Each dr As DataRow In dsData.Tables(0).Rows
            If Not IsDBNull(dr("Total_Amount_RMB")) Then
                dr("Total_Amount_RMB_Text") = udtFormatter.formatMoneyRMB(dr("Total_Amount_RMB"), False)
                dr.AcceptChanges()
            End If

            If Not IsDBNull(dr("Total_Reduction_Fee")) Then
                dr("Total_Reduction_Fee_Text") = udtFormatter.formatMoneyRMB(dr("Total_Reduction_Fee"), False)
                dr.AcceptChanges()
            End If

            If Not IsDBNull(dr("Total_Support_Fee")) Then
                dr("Total_Support_Fee_Text") = udtFormatter.formatMoneyRMB(dr("Total_Support_Fee"), False)
                dr.AcceptChanges()
            End If

        Next

        Return dsData

    End Function
    'CRE20-015-02 (Special Support Scheme) [End][Martin]

    ' CRE17-004 Generate a new DPAR on EHCP basis [Start][Dickson]
    Private Function GetEHCPBasisReportSource(ByVal strRimbeID As String, ByVal strCutoffDate As String, ByVal strSchemeCode As String) As DataSet
        Dim dsData As New DataSet()

        Dim udtDB As New Common.DataAccess.Database()
        udtDB.CommandTimeout = 300

        Dim params() As SqlParameter = New SqlParameter() { _
            udtDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strRimbeID), _
            udtDB.MakeInParam("@cutoff_Date_str", SqlDbType.Char, 11, strCutoffDate), _
            udtDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}
        udtDB.RunProc("proc_DPAReport_EHCP_get", params, dsData)

        ' Format Total Amount RMB
        Dim udtFormatter As New Formatter

        dsData.Tables(0).Columns.Add("Total_Amount_RMB_Text", GetType(String))

        For Each dr As DataRow In dsData.Tables(0).Rows
            If Not IsDBNull(dr("Total_Amount_RMB")) Then
                dr("Total_Amount_RMB_Text") = udtFormatter.formatMoneyRMB(dr("Total_Amount_RMB"), False)
                dr.AcceptChanges()
            End If
        Next

        Return dsData

    End Function
    ' CRE17-004 Generate a new DPAR on EHCP basis [End][Dickson]
End Class