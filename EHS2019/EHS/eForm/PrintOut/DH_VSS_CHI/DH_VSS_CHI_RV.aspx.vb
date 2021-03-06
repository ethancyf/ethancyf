Imports Common.Component.ServiceProvider

Partial Public Class DH_VSS_CHI_RV
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim rpt As GrapeCity.ActiveReports.SectionReport = Nothing


        Dim udtServiceProvider As ServiceProviderModel = New ServiceProviderModel
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL


        udtServiceProvider = CType(Me.Session(Common.ComFunction.ReportFunction.SessionName.udtServiceProvider), ServiceProviderModel)

        rpt = New PrintOut.DH_VSS_CHI.DH_VSS(udtServiceProvider)

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

        ' Set fallback font
        pdf.FontFallback = Common.Component.Printout.PrintoutBLL.FallbackFont()

        ' Create a new memory stream that will hold the pdf output
        Dim memStream As New System.IO.MemoryStream()
        ' Export the report to PDF:
        pdf.Export(rpt.Document, memStream)
        ' Write the PDF stream out
        Response.BinaryWrite(memStream.ToArray())
        ' Send all buffered content to the client
        Response.End()

    End Sub

End Class