Imports Common.ComFunction
Imports GrapeCity.ActiveReports.Export.Pdf.Section

Partial Public Class TokenSharingConsent_RV
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim rpt As PrintOut.TokenSharingConsent.TokenSharingConsent = New PrintOut.TokenSharingConsent.TokenSharingConsent(Me.Session(ReportFunction.SessionName.strApplicatName))

        Try
            rpt.Document.Printer.PrinterName = ""
            rpt.Run(False)
        Catch eRunReport As GrapeCity.ActiveReports.ReportException
            Throw New Exception(eRunReport.Message)
            Return
        End Try

        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "inline; filename=MyPDF.PDF")

        Dim pdf As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport()
        Dim memStream As New System.IO.MemoryStream()
        pdf.Export(rpt.Document, memStream)
        Response.BinaryWrite(memStream.ToArray())
        Response.End()

    End Sub

End Class