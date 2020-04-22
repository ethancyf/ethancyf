Imports System.Data.SqlClient
Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel
Imports Common.Format

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

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Dim rpt As Object = Nothing

        If (New SchemeClaimBLL).getAllDistinctSchemeClaim.Filter(strDPAScheme).ReimbursementCurrency = EnumReimbursementCurrency.HKD Then
            rpt = New DetailedPaymentAnalysisRpt(Me.GetReportSource(strRID, strCutoffDate, strDPAScheme), bWatermark)
        Else
            rpt = New DetailedPaymentAnalysisRmbRpt(Me.GetReportSource(strRID, strCutoffDate, strDPAScheme), bWatermark)
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

        ' Tell the browser this is a PDF document so it will use an appropriate viewer.
        ' If the report has been exported in a different format, the content-type will 
        ' need to be changed as noted in the following table:
        '  ExportType  ContentType
        '    PDF       "application/pdf"  (needs to be in lowercase)
        Response.ContentType = "application/pdf"

        ' IE & Acrobat seam to require "content-disposition" header being in the response.  If you don't add it, the doc still works most of the time, but not always.
        'this makes a new window appear: Response.AddHeader("content-disposition","attachment; filename=MyPDF.PDF");
        Response.AddHeader("content-disposition", "inline; filename=DetailedPaymentAnalysisRpt.PDF")

        ' Create the PDF export object
        Dim pdf As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport()
        ' Create a new memory stream that will hold the pdf output
        Dim memStream As New System.IO.MemoryStream()
        ' Export the report to PDF:
        pdf.Export(rpt.Document, memStream)
        ' Write the PDF stream out
        Response.BinaryWrite(memStream.ToArray())
        ' Send all buffered content to the client
        Response.End()
    End Sub

    Private Function GetReportSource(ByVal strRimbeID As String, ByVal strCutoffDate As String, ByVal strSchemeCode As String) As DataSet
        Dim dsData As New DataSet()

        Dim udtDB As New Common.DataAccess.Database()
        udtDB.CommandTimeout = 300

        Dim params() As SqlParameter = New SqlParameter() { _
            udtDB.MakeInParam("@reimburse_id", SqlDbType.Char, 15, strRimbeID), _
            udtDB.MakeInParam("@cutoff_Date_str", SqlDbType.Char, 11, strCutoffDate), _
            udtDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}
        udtDB.RunProc("proc_DPAReport_get", params, dsData)

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

End Class