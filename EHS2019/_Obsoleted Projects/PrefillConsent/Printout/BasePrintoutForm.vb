Public MustInherit Class BasePrintoutForm
    'Inherits System.Web.UI.Page
Inherits Common.ComObject.MasterPage
    Protected Sub LoadReport()

        Dim rpt As GrapeCity.ActiveReports.SectionReport = GetReport()
        If rpt Is Nothing Then
            ' ToDo: Redirect to error page?
            If Not HttpContext.Current.Session Is Nothing AndAlso Not HttpContext.Current.Session("language") Is Nothing Then
                If HttpContext.Current.Session("language") = "zh-tw" Then
                    Response.Redirect("reporttimeout_chi.aspx")
                Else
                    Response.Redirect("reporttimeout.aspx")
                End If
            ElseIf Request.UserLanguages.Length > 0 Then
                Dim strLanguage As String = Request.UserLanguages(0)
                If strLanguage.IndexOf("zh") = 0 Then
                    Response.Redirect("reporttimeout_chi.aspx")
                End If
            Else
                Response.Redirect("reporttimeout.aspx")
            End If
        Else
            rpt.Document.Printer.PrinterName = ""
            Try
                rpt.Run(False)
            Catch eRunReport As GrapeCity.ActiveReports.ReportException
                ' Failure running report, just report the error to the user:
                Throw New Exception(eRunReport.Message)
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
            Response.AddHeader("content-disposition", "inline; filename=MyPDF.PDF")

            ' Create the PDF export object
            Dim pdf As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport()
            ' Create a new memory stream that will hold the pdf output
            Dim memStream As New System.IO.MemoryStream()
            ' Export the report to PDF, Write the PDF stream out and Send all buffered content to the client
            pdf.Export(rpt.Document, memStream)
            Response.BinaryWrite(memStream.ToArray())            ' 
            Response.End()
        End If
    End Sub

    MustOverride Function GetReport() As GrapeCity.ActiveReports.SectionReport

End Class
