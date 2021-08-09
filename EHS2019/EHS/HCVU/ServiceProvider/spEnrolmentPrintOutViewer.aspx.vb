Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.Component.Scheme
Imports Common.ComObject

Partial Public Class spEnrolmentPrintOutViewer
    Inherits System.Web.UI.Page

#Region "Fields"

    Private udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
    Private udtSchemeBackOfficeModelCollection As New SchemeBackOfficeModelCollection
    Private strSchemeCodeArrayList As ArrayList = Nothing
    Private strSchemeCodeList As String = String.Empty
#End Region

#Region "Constants"

    Public Const SESS_PrintSchemeCodeArrayList As String = "PrintSchemeCodeArrayList"

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ApplicantType As Integer = Session("ApplicantType")
        Dim udtSPModel As ServiceProviderModel = Session("ServiceProvider")
        Dim btnSPPermanent As Boolean = Session("SPPermanent")
        Dim rpt As GrapeCity.ActiveReports.SectionReport = Nothing

        If Not IsNothing(Session(SESS_PrintSchemeCodeArrayList)) Then
            strSchemeCodeArrayList = CType(Session(SESS_PrintSchemeCodeArrayList), ArrayList)
            'Session.Remove(SESS_PrintSchemeCodeArrayList) 'CRE20-023-57 (Fix bug for Some information are missing when download Scheme Enrolment Letter via Chrome/Edge) [Martin]
        End If

        If IsNothing(strSchemeCodeArrayList) Then
            strSchemeCodeArrayList = New ArrayList()
            If udtSchemeBackOfficeBLL.ExistSession_SchemeBackOfficeWithSubsidizeGroup Then
                udtSchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetSession_SchemeBackOfficeWithSubsidizeGroup
                For Each udtSchemeBackOfficeModel As SchemeBackOfficeModel In udtSchemeBackOfficeModelCollection
                    If Not IsNothing(Session(udtSchemeBackOfficeModel.SchemeCode.Trim)) AndAlso Not Session(udtSchemeBackOfficeModel.SchemeCode.Trim).ToString().Equals(String.Empty) Then
                        If Session(udtSchemeBackOfficeModel.SchemeCode.Trim).ToString().Trim = "Y" Then
                            strSchemeCodeArrayList.Add(udtSchemeBackOfficeModel.SchemeCode.Trim)
                        End If
                    End If
                Next
            End If
        End If

        'Selection of letters
        'New enrolment (eHS)    DH_eHS001
        'New enrolment (reuse PPI-ePR token)   DH_eHS004
        'Scheme enrolment   DH_eHS002

        'add Audit log description
        Dim udtAuditLogEntry As AuditLogEntry = CType(Session(spPrintFunction.SESS_PrintOutAuditLogEntry), AuditLogEntry)
        udtAuditLogEntry.AddDescripton("SPID(PrintOutViewer)", udtSPModel.SPID)
        udtAuditLogEntry.AddDescripton("Token No(PrintOutViewer)", IIf(IsNothing(udtSPModel.TokenSerialNo), String.Empty, udtSPModel.TokenSerialNo))
        For Each strSchemeCode As String In strSchemeCodeArrayList
            strSchemeCodeList += strSchemeCode.Trim + " "
        Next
        udtAuditLogEntry.AddDescripton("SchemeCodeList(PrintOutViewer)", strSchemeCodeList)
        udtAuditLogEntry.AddDescripton("ApplicantType(PrintOutViewer)", ApplicantType.ToString())

        'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Select Case ApplicantType
        '    Case Common.Component.ApplicantType.Applicant_of_fresh
        '        rpt = New PrintOut.ConfirmationLetter.DH_eHS001(udtSPModel.EnrolRefNo, udtSPModel.TokenSerialNo, strSchemeCodeArrayList, udtSPModel, ApplicantType, btnSPPermanent)
        '    Case Common.Component.ApplicantType.Applicant_of_eHS_PPiePR
        '        rpt = New PrintOut.ConfirmationLetter.DH_eHS002(udtSPModel, strSchemeCodeArrayList, ApplicantType, False, btnSPPermanent)
        '    Case Common.Component.ApplicantType.Applicant_of_eHS
        '        rpt = New PrintOut.ConfirmationLetter.DH_eHS002(udtSPModel, strSchemeCodeArrayList, ApplicantType, False, btnSPPermanent)
        '    Case Common.Component.ApplicantType.Applicant_of_PPiePR
        '        rpt = New PrintOut.ConfirmationLetter.DH_eHS004(udtSPModel.EnrolRefNo, udtSPModel.TokenSerialNo, strSchemeCodeArrayList, udtSPModel, ApplicantType, btnSPPermanent)
        'End Select

        Select Case ApplicantType
            Case Common.Component.ApplicantType.NewEnrolment_EHSS_Token
                rpt = New PrintOut.ConfirmationLetter.DH_eHS001(udtSPModel.EnrolRefNo, udtSPModel.TokenSerialNo, strSchemeCodeArrayList, udtSPModel, ApplicantType, btnSPPermanent)
            Case Common.Component.ApplicantType.SchemeEnrolment_EHRSS_Token
                rpt = New PrintOut.ConfirmationLetter.DH_eHS002(udtSPModel, strSchemeCodeArrayList, ApplicantType, False, btnSPPermanent)
            Case Common.Component.ApplicantType.SchemeEnrolment_EHSS_Token
                rpt = New PrintOut.ConfirmationLetter.DH_eHS002(udtSPModel, strSchemeCodeArrayList, ApplicantType, False, btnSPPermanent)
            Case Common.Component.ApplicantType.NewEnrolment_EHRSS_Token
                rpt = New PrintOut.ConfirmationLetter.DH_eHS004(udtSPModel.EnrolRefNo, udtSPModel.TokenSerialNo, strSchemeCodeArrayList, udtSPModel, ApplicantType, btnSPPermanent)
        End Select
        'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]

        rpt.Document.Printer.PrinterName = ""

        Dim pdfExport As New GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport()

        ' Run and view the report
        Try
            'AuditLog
            If btnSPPermanent Then
                udtAuditLogEntry.AddDescripton("SP table location", TableLocation.Permanent)
            Else
                udtAuditLogEntry.AddDescripton("SP table location", TableLocation.Staging)
            End If
            udtAuditLogEntry.AddDescripton("Applicant Type", ApplicantType.ToString)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00101, "Print Letter for Enrolment", New AuditLogInfo(udtSPModel.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

            rpt.Run(False)

            udtAuditLogEntry.WriteEndLog(LogID.LOG00102, "Print Letter for Enrolment successful", New AuditLogInfo(udtSPModel.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))

            pdfExport.Security.Permissions = GrapeCity.ActiveReports.Export.Pdf.Section.PdfPermissions.AllowPrint

            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "inline; filename=MyPDF.PDF")

            Dim memStream As New System.IO.MemoryStream()
            pdfExport.Export(rpt.Document, memStream)
            Response.BinaryWrite(memStream.ToArray())
            Response.End()

            Session.Clear()

        Catch ex As GrapeCity.ActiveReports.ReportException
            udtAuditLogEntry.WriteEndLog(LogID.LOG00103, "Print Letter for Enrolment fail", New AuditLogInfo(udtSPModel.SPID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
        End Try

    End Sub

End Class