Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Export.Pdf.Section
Imports Common.Component.ServiceProvider
Imports Common.PCD.Printout.EnrolmentInformation.Component
Imports Common.ComObject
Imports Common.Component

Partial Public Class PCD_EnrolmentForm
    Inherits System.Web.UI.Page

    Private Const LocalFunctionCode As String = FunctCode.FUNT020101

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strPCD_ERN As String = GetSessionStringByName("PCD_ERN")
        Dim strPCD_SubmissionTime As String = GetSessionStringByName("PCD_SubmissionTime")
        Dim strLang As String = Page.Request.QueryString("lang")

        Dim udtAuditLogEntry As New AuditLogEntry(LocalFunctionCode)
        udtAuditLogEntry.AddDescripton("PCD_ERN", strPCD_ERN)
        udtAuditLogEntry.AddDescripton("PCD_SubmissionTime", strPCD_SubmissionTime)
        udtAuditLogEntry.AddDescripton("Language", strLang)
        udtAuditLogEntry.WriteLog(LogID.LOG00072, "Print PCD Enrolment Form Start")

        Dim udtProvider As ServiceProviderModel = Session("SP")

        If Not IsNothing(udtProvider) Then

            Dim rpt As GrapeCity.ActiveReports.SectionReport = New Core(udtProvider, strLang, strPCD_ERN, strPCD_SubmissionTime)

            Try
                rpt.Run(False)
            Catch eRunReport As GrapeCity.ActiveReports.ReportException
                ' Failure running report, just report the error to the user:
                Response.Clear()

                Return
            End Try

            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "inline; filename=MyPDF.PDF")

            Dim pdf As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport()
            pdf.NeverEmbedFonts = ""

            Dim memStream As New System.IO.MemoryStream()
            pdf.Export(rpt.Document, memStream)

            Response.BinaryWrite(memStream.ToArray())

            udtAuditLogEntry.WriteLog(LogID.LOG00073, "Print PCD Enrolment Form End")

            Response.End()

        Else
            Response.Redirect("~/main.aspx")

            udtAuditLogEntry.WriteLog(LogID.LOG00074, "Print PCD Enrolment Form Fail")
        End If


    End Sub

    Function GetSessionStringByName(ByVal strName As String) As String
        Dim strValue As String = String.Empty
        If Not IsNothing(Session(strName)) Then
            strValue = Session(strName)
        End If
        Return strValue
    End Function

End Class