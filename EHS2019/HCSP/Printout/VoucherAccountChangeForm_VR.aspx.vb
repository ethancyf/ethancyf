Imports GrapeCity.ActiveReports.SectionReportModel.Export.Pdf
Imports Common.ComFunction
Imports HCSP.BLL
Imports Common.Component.ServiceProvider
Imports Common.Component.ClaimTrans
Imports Common.Component.EHSAccount
Imports common.Component.DocType

Partial Public Class VoucherAccountChangeForm_VR
    Inherits System.Web.UI.Page

    Public Const FunctCode As String = Common.Component.FunctCode.FUNT020201


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtSessionHandler As SessionHandler = New SessionHandler
        Dim udtClaimVoucherBLL As ClaimVoucherBLL = New ClaimVoucherBLL
        Dim strFunctCode As String = FunctCode
        Dim strSessionFunctCode As String = udtSessionHandler.EHSClaimPrintoutFunctionCodeGetFromSession()
        Dim udtSP As ServiceProviderModel = Nothing
        udtSessionHandler.CurrentUserGetFromSession(udtSP, Nothing)
        If Not String.IsNullOrEmpty(strSessionFunctCode) Then
            strFunctCode = strSessionFunctCode
        End If
        Dim udtSmartIDContent As SmartIDContentModel = udtSessionHandler.SmartIDContentGetFormSession(strFunctCode)


        Dim rpt As PrintOut.VoucherAccountChnageForm.SmartIDChangePersonalPaticularForm
        rpt = New PrintOut.VoucherAccountChnageForm.SmartIDChangePersonalPaticularForm(Me.DiffOfChangePaticular(udtSmartIDContent.EHSAccount, udtSmartIDContent.EHSValidatedAccount), udtSP)
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
        ' Export the report to PDF:
        pdf.Export(rpt.Document, memStream)
        'pdf.Export(rpt.Document, "c:\\EVS\\TEMP\\mypdf.pdf")

        ' Write the PDF stream out
        Response.BinaryWrite(memStream.ToArray())
        ' Send all buffered content to the client
        Response.End()
    End Sub

    Private Function DiffOfChangePaticular(ByVal udtSmartIDEHSAccount As EHSAccountModel, ByVal udtOriginalEHSAccount As EHSAccountModel) As EHSAccountModel.EHSPersonalInformationModel
        Dim udtSmartIDEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtSmartIDEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)
        Dim udtOriginalEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtOriginalEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)
        Dim udtDiffEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = New EHSAccountModel.EHSPersonalInformationModel

        If udtSmartIDEHSPersonalInfo.ENameFirstName Is Nothing Then udtSmartIDEHSPersonalInfo.ENameFirstName = String.Empty
        If udtSmartIDEHSPersonalInfo.ENameSurName Is Nothing Then udtSmartIDEHSPersonalInfo.ENameSurName = String.Empty
        If udtSmartIDEHSPersonalInfo.CName Is Nothing Then udtSmartIDEHSPersonalInfo.CName = String.Empty
        If udtSmartIDEHSPersonalInfo.CCCode1 Is Nothing Then udtSmartIDEHSPersonalInfo.CCCode1 = String.Empty
        If udtSmartIDEHSPersonalInfo.CCCode2 Is Nothing Then udtSmartIDEHSPersonalInfo.CCCode2 = String.Empty
        If udtSmartIDEHSPersonalInfo.CCCode3 Is Nothing Then udtSmartIDEHSPersonalInfo.CCCode3 = String.Empty
        If udtSmartIDEHSPersonalInfo.CCCode4 Is Nothing Then udtSmartIDEHSPersonalInfo.CCCode4 = String.Empty
        If udtSmartIDEHSPersonalInfo.CCCode5 Is Nothing Then udtSmartIDEHSPersonalInfo.CCCode5 = String.Empty
        If udtSmartIDEHSPersonalInfo.CCCode6 Is Nothing Then udtSmartIDEHSPersonalInfo.CCCode6 = String.Empty
        If udtSmartIDEHSPersonalInfo.Gender Is Nothing Then udtSmartIDEHSPersonalInfo.Gender = String.Empty

        If udtOriginalEHSPersonalInfo.ENameFirstName Is Nothing Then udtOriginalEHSPersonalInfo.ENameFirstName = String.Empty
        If udtOriginalEHSPersonalInfo.ENameSurName Is Nothing Then udtOriginalEHSPersonalInfo.ENameSurName = String.Empty
        If udtOriginalEHSPersonalInfo.CName Is Nothing Then udtOriginalEHSPersonalInfo.CName = String.Empty
        If udtOriginalEHSPersonalInfo.CCCode1 Is Nothing Then udtOriginalEHSPersonalInfo.CCCode1 = String.Empty
        If udtOriginalEHSPersonalInfo.CCCode2 Is Nothing Then udtOriginalEHSPersonalInfo.CCCode2 = String.Empty
        If udtOriginalEHSPersonalInfo.CCCode3 Is Nothing Then udtOriginalEHSPersonalInfo.CCCode3 = String.Empty
        If udtOriginalEHSPersonalInfo.CCCode4 Is Nothing Then udtOriginalEHSPersonalInfo.CCCode4 = String.Empty
        If udtOriginalEHSPersonalInfo.CCCode5 Is Nothing Then udtOriginalEHSPersonalInfo.CCCode5 = String.Empty
        If udtOriginalEHSPersonalInfo.CCCode6 Is Nothing Then udtOriginalEHSPersonalInfo.CCCode6 = String.Empty
        If udtOriginalEHSPersonalInfo.Gender Is Nothing Then udtOriginalEHSPersonalInfo.Gender = String.Empty

        If Not udtSmartIDEHSPersonalInfo.ENameFirstName.Equals(udtOriginalEHSPersonalInfo.ENameFirstName) OrElse Not udtSmartIDEHSPersonalInfo.ENameSurName.Equals(udtOriginalEHSPersonalInfo.ENameSurName) Then
            udtDiffEHSPersonalInfo.ENameFirstName = udtSmartIDEHSPersonalInfo.ENameFirstName
            udtDiffEHSPersonalInfo.ENameSurName = udtSmartIDEHSPersonalInfo.ENameSurName
        End If

        If Not udtSmartIDEHSPersonalInfo.CName.Equals(udtOriginalEHSPersonalInfo.CName) Then
            udtDiffEHSPersonalInfo.CName = udtSmartIDEHSPersonalInfo.CName
        End If

        If Not udtSmartIDEHSPersonalInfo.CCCode1.Equals(udtOriginalEHSPersonalInfo.CCCode1) Then
            udtDiffEHSPersonalInfo.CCCode1 = udtSmartIDEHSPersonalInfo.CCCode1
        End If

        If Not udtSmartIDEHSPersonalInfo.CCCode2.Equals(udtOriginalEHSPersonalInfo.CCCode2) Then
            udtDiffEHSPersonalInfo.CCCode2 = udtSmartIDEHSPersonalInfo.CCCode2
        End If

        If Not udtSmartIDEHSPersonalInfo.CCCode3.Equals(udtOriginalEHSPersonalInfo.CCCode3) Then
            udtDiffEHSPersonalInfo.CCCode3 = udtSmartIDEHSPersonalInfo.CCCode3
        End If

        If Not udtSmartIDEHSPersonalInfo.CCCode4.Equals(udtOriginalEHSPersonalInfo.CCCode4) Then
            udtDiffEHSPersonalInfo.CCCode4 = udtSmartIDEHSPersonalInfo.CCCode4
        End If

        If Not udtSmartIDEHSPersonalInfo.CCCode5.Equals(udtOriginalEHSPersonalInfo.CCCode5) Then
            udtDiffEHSPersonalInfo.CCCode5 = udtSmartIDEHSPersonalInfo.CCCode5
        End If

        If Not udtSmartIDEHSPersonalInfo.CCCode6.Equals(udtOriginalEHSPersonalInfo.CCCode6) Then
            udtDiffEHSPersonalInfo.CCCode6 = udtSmartIDEHSPersonalInfo.CCCode6
        End If

        If Not udtSmartIDEHSPersonalInfo.DateofIssue.Equals(udtOriginalEHSPersonalInfo.DateofIssue) Then
            udtDiffEHSPersonalInfo.DateofIssue = udtSmartIDEHSPersonalInfo.DateofIssue
        End If

        If Not udtSmartIDEHSPersonalInfo.DOB.Equals(udtOriginalEHSPersonalInfo.DOB) OrElse Not udtSmartIDEHSPersonalInfo.ExactDOB.Equals(udtOriginalEHSPersonalInfo.ExactDOB) Then
            udtDiffEHSPersonalInfo.DOB = udtSmartIDEHSPersonalInfo.DOB
            udtDiffEHSPersonalInfo.ExactDOB = udtSmartIDEHSPersonalInfo.ExactDOB
        End If

        If Not udtSmartIDEHSPersonalInfo.Gender.Equals(udtOriginalEHSPersonalInfo.Gender) Then
            udtDiffEHSPersonalInfo.Gender = udtSmartIDEHSPersonalInfo.Gender
        End If

        Return udtDiffEHSPersonalInfo
    End Function

End Class