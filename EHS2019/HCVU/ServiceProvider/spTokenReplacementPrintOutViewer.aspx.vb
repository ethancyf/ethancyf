Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.Component.Token

Partial Public Class spTokenReplacementPrintOutViewer
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim rpt As GrapeCity.ActiveReports.SectionReport = Nothing

        Dim udtSPModel As ServiceProviderModel = Session("ServiceProvider")
        Dim udtToken As TokenModel = Session(spPrintFunction.SESS_SPPrintFunctionToken)

        'add Audit log description
        Dim udtAuditLogEntry As AuditLogEntry = CType(Session(spPrintFunction.SESS_PrintOutAuditLogEntry), AuditLogEntry)

        'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'udtAuditLogEntry.AddDescripton("SPID(PrintOutViewer)", udtSPModel.SPID)
        'udtAuditLogEntry.AddDescripton("Letter Type(PrintOutViewer)", "Token Replacement")
        '' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        '' -----------------------------------------------------------------------------------------
        ''udtAuditLogEntry.AddDescripton("Use PPI-ePR Token(PrintOutViewer)", (udtToken.Project = TokenProjectType.PPIEPR).ToString())
        ''udtAuditLogEntry.AddDescripton("Old Token No(PrintOutViewer)", udtToken.TokenSerialNo)
        ''udtAuditLogEntry.AddDescripton("New Token No(PrintOutViewer)", IIf(IsNothing(udtToken.TokenSerialNoReplacement), String.Empty, udtToken.TokenSerialNoReplacement))
        'udtAuditLogEntry.AddDescripton("Use PPI-ePR Token (Existing)(PrintOutViewer)", (udtToken.Project = TokenProjectType.PPIEPR).ToString())
        'udtAuditLogEntry.AddDescripton("Token No (Existing)(PrintOutViewer)", udtToken.TokenSerialNo)
        'udtAuditLogEntry.AddDescripton("Is Share Token (Existing)(PrintOutViewer)", udtToken.IsShareToken.ToString())
        'udtAuditLogEntry.AddDescripton("Use PPI-ePR Token (New)(PrintOutViewer)", (udtToken.ProjectReplacement = TokenProjectType.PPIEPR).ToString())
        'udtAuditLogEntry.AddDescripton("Token No (New)(PrintOutViewer)", udtToken.TokenSerialNoReplacement)
        'udtAuditLogEntry.AddDescripton("Is Share Token (New)(PrintOutViewer)", udtToken.IsShareTokenReplacement.Value.ToString())
        '' CRE14-002 - PPI-ePR Migration [End][Tommy L]

        udtAuditLogEntry.AddDescripton("SPID(PrintOutViewer)", udtSPModel.SPID)
        udtAuditLogEntry.AddDescripton("Letter Type(PrintOutViewer)", "Token Replacement")
        udtAuditLogEntry.AddDescripton("Use EHRSS Token (Existing)(PrintOutViewer)", (udtToken.Project = TokenProjectType.EHR).ToString())
        udtAuditLogEntry.AddDescripton("Token No (Existing)(PrintOutViewer)", udtToken.TokenSerialNo)
        udtAuditLogEntry.AddDescripton("Is Share Token (Existing)(PrintOutViewer)", udtToken.IsShareToken.ToString())
        udtAuditLogEntry.AddDescripton("Use EHRSS Token (New)(PrintOutViewer)", (udtToken.ProjectReplacement = TokenProjectType.EHR).ToString())
        udtAuditLogEntry.AddDescripton("Token No (New)(PrintOutViewer)", udtToken.TokenSerialNoReplacement)
        udtAuditLogEntry.AddDescripton("Is Share Token (New)(PrintOutViewer)", udtToken.IsShareTokenReplacement.Value.ToString())
        'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]

        rpt = New PrintOut.ConfirmationLetter.TokenReplacementLetter(udtSPModel, udtToken)

        rpt.Document.Printer.PrinterName = ""

        Dim pdfExport As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport()

        ' Run and view the report
        Try
            'AuditLog
            udtAuditLogEntry.WriteStartLog(LogID.LOG00098, "Print Letter", New AuditLogInfo(udtSPModel.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

            rpt.Run(False)

            udtAuditLogEntry.WriteEndLog(LogID.LOG00099, "Print Letter Successful", New AuditLogInfo(udtSPModel.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

            pdfExport.Security.Permissions = GrapeCity.ActiveReports.Export.Pdf.Section.PdfPermissions.AllowPrint

            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "inline; filename=MyPDF.PDF")

            Dim memStream As New System.IO.MemoryStream()

            pdfExport.Export(rpt.Document, memStream)
            Response.BinaryWrite(memStream.ToArray())
            Response.End()

            Session.Clear()

        Catch ex As GrapeCity.ActiveReports.ReportException
            udtAuditLogEntry.WriteEndLog(LogID.LOG00100, "Print Letter Failed", New AuditLogInfo(udtSPModel.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
        End Try

    End Sub

End Class
