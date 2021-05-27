Imports System.Data.SqlClient
Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel
Imports Common.Format
Imports Common.Component
Imports HCVU.BLL
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount
Imports Common.Component.HCVUUser

Partial Public MustInherit Class BasePrintoutForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadReport()

    End Sub

    Protected Sub LoadReport()

        ' Check session expired
        'UserACBLL.GetUserAC()

        'CRE20-023 add session on the reprint form [Start][Nichole]
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser()
        'CRE20-023 add session on the reprint form [End][Nichole]


        Dim udtSessionHandler As BLL.SessionHandlerBLL = New BLL.SessionHandlerBLL()
        Dim strSessionFunctCode As String = udtSessionHandler.EHSClaimPrintoutFunctionCodeGetFromSession()
        Dim strFunctCode As String = Common.Component.FunctCode.FUNT020201
        If Not String.IsNullOrEmpty(strSessionFunctCode) Then
            strFunctCode = strSessionFunctCode
        End If

        Dim udtEHSTransaction As EHSTransactionModel = udtSessionHandler.EHSTransactionGetFromSession(strFunctCode)

        Dim udtDischargeResult As COVID19.DischargeResultModel = udtSessionHandler.ClaimCOVID19DischargeRecordGetFromSession(strFunctCode)
        Dim blnDischarge As Boolean = False

        If udtDischargeResult IsNot Nothing AndAlso _
            (udtDischargeResult.DemographicResult = COVID19.DischargeResultModel.Result.ExactMatch OrElse _
            udtDischargeResult.DemographicResult = COVID19.DischargeResultModel.Result.PartialMatch) Then
            blnDischarge = True
        End If

        Dim rpt As GrapeCity.ActiveReports.SectionReport = Nothing

        Select Case udtEHSTransaction.SchemeCode.Trim
            Case SchemeClaimModel.CIVSS
                'rpt = GetReport()

            Case SchemeClaimModel.EVSS
                'rpt = GetReport()

            Case SchemeClaimModel.HCVS, SchemeClaimModel.HCVSCHN, SchemeClaimModel.HCVSDHC
                'rpt = ConsentFormInformationBLL.GetReport(BulidConsentFormInformation(strFunctCode))

            Case SchemeClaimModel.HSIVSS
                'rpt = GetReport()

            Case SchemeClaimModel.RVP
                If udtEHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19).Count > 0 Then
                    'EHS Record for covid19 printing
                    Dim udtEHSAccount As EHSAccountModel = udtSessionHandler.EHSAccountGetFromSession(strFunctCode)
                    Dim udtVaccinationRecord As TransactionDetailVaccineModel = udtSessionHandler.ClaimCOVID19VaccinationCardGetFromSession(strFunctCode)

                    rpt = New COVID19.PrintOut.Covid19VaccinationCard.Covid19VaccinationCard(udtEHSTransaction, udtEHSAccount, udtVaccinationRecord, blnDischarge)
                End If

            Case SchemeClaimModel.VSS
                If udtEHSTransaction.TransactionDetails.FilterBySubsidizeItemDetail(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19).Count > 0 Then
                    'EHS Record for covid19 printing
                    Dim udtEHSAccount As EHSAccountModel = udtSessionHandler.EHSAccountGetFromSession(strFunctCode)
                    Dim udtVaccinationRecord As TransactionDetailVaccineModel = udtSessionHandler.ClaimCOVID19VaccinationCardGetFromSession(strFunctCode)

                    rpt = New COVID19.PrintOut.Covid19VaccinationCard.Covid19VaccinationCard(udtEHSTransaction, udtEHSAccount, udtVaccinationRecord, blnDischarge)
                End If

                'rpt = GetReport()

            Case SchemeClaimModel.ENHVSSO
                'rpt = GetReport()

                ' CRE20-015 (HA Scheme) [Start][Winnie]
            Case SchemeClaimModel.SSSCMC
                'rpt = ConsentFormInformationBLL.GetReport(BulidConsentFormInformation(strFunctCode))
                ' CRE20-015 (HA Scheme) [End][Winnie]

                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
            Case SchemeClaimModel.COVID19CVC, SchemeClaimModel.COVID19DH, SchemeClaimModel.COVID19OR, SchemeClaimModel.COVID19SR, SchemeClaimModel.COVID19SB
                'EHS Record for covid19 printing
                Dim udtEHSAccount As EHSAccountModel = udtSessionHandler.EHSAccountGetFromSession(strFunctCode)
                Dim udtVaccinationRecord As TransactionDetailVaccineModel = udtSessionHandler.ClaimCOVID19VaccinationCardGetFromSession(strFunctCode)

                rpt = New COVID19.PrintOut.Covid19VaccinationCard.Covid19VaccinationCard(udtEHSTransaction, udtEHSAccount, udtVaccinationRecord, blnDischarge)
                ' CRE20-0022 (Immu record) [End][Winnie SUEN]
            Case SchemeClaimModel.COVID19RVP

                Dim udtEHSAccount As EHSAccountModel = udtSessionHandler.EHSAccountGetFromSession(strFunctCode)
                Dim udtVaccinationRecord As TransactionDetailVaccineModel = udtSessionHandler.ClaimCOVID19VaccinationCardGetFromSession(strFunctCode)

                rpt = New COVID19.PrintOut.Covid19VaccinationCard.Covid19VaccinationCard(udtEHSTransaction, udtEHSAccount, udtVaccinationRecord, blnDischarge)

            Case Else
                rpt = Nothing

        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        If rpt Is Nothing Then

            'Dim udtEHSTransaction As EHSTransactionModel = udtSessionHandler.EHSTransactionGetFromSession(strFunctCode)
            Dim strDocumentCode As String = "null"
            Dim strSchemeCode As String = "null"
            If Not udtEHSTransaction Is Nothing Then
                strDocumentCode = udtEHSTransaction.DocCode
                strSchemeCode = udtEHSTransaction.SchemeCode
            End If

            Throw New ArgumentNullException(String.Format("BasePrintoutForm, Get Report Fail: (Transaction DocCode:{0} SchemeCode:{1})", strDocumentCode, strSchemeCode))
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
            Response.BinaryWrite(memStream.ToArray())
            Response.End()
        End If
    End Sub



End Class