Imports Common.Component.Scheme
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.ClaimRules
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.Practice
Imports Common.Component.MedicalOrganization
Imports Common.Component.UserAC
Imports HCSP.BLL
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.ConsentFormInformation
'Imports ConsentFormEHS
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Public MustInherit Class BasePrintoutForm
    Inherits System.Web.UI.Page

    Private udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()

    Private FunctCode As String = Common.Component.FunctCode.FUNT020201

    Public MustOverride ReadOnly Property Language() As String

    Public MustOverride ReadOnly Property FormStyle() As String

    Protected Sub LoadReport()

        ' Check session expired
        UserACBLL.GetUserAC()

        Dim udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
        Dim strSessionFunctCode As String = udtSessionHandler.EHSClaimPrintoutFunctionCodeGetFromSession()
        Dim strFunctCode As String = Common.Component.FunctCode.FUNT020201
        If Not String.IsNullOrEmpty(strSessionFunctCode) Then
            strFunctCode = strSessionFunctCode
        End If

        Dim rpt As GrapeCity.ActiveReports.SectionReport = Nothing
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Select Case udtSessionHandler.SchemeSelectedGetFromSession(strFunctCode).SchemeCode.Trim
            Case SchemeClaimModel.CIVSS
                rpt = GetReport()

            Case SchemeClaimModel.EVSS
                rpt = GetReport()

            Case SchemeClaimModel.HCVS, SchemeClaimModel.HCVSCHN, SchemeClaimModel.HCVSDHC
                rpt = ConsentFormInformationBLL.GetReport(BulidConsentFormInformation(strFunctCode))

            Case SchemeClaimModel.HSIVSS
                rpt = GetReport()

            Case SchemeClaimModel.RVP
                rpt = GetReport()

            Case SchemeClaimModel.VSS
                rpt = GetReport()

            Case SchemeClaimModel.ENHVSSO
                rpt = GetReport()

                ' CRE20-015 (HA Scheme) [Start][Winnie]
            Case SchemeClaimModel.SSSCMC
                rpt = ConsentFormInformationBLL.GetReport(BulidConsentFormInformation(strFunctCode))
                ' CRE20-015 (HA Scheme) [End][Winnie]

                ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
            Case SchemeClaimModel.COVID19CVC, SchemeClaimModel.COVID19RVP, _
                SchemeClaimModel.COVID19DH, SchemeClaimModel.COVID19OR, _
                SchemeClaimModel.COVID19SR, SchemeClaimModel.COVID19SB
                rpt = GetReport()
                ' CRE20-0022 (Immu record) [End][Winnie SUEN]

            Case Else
                rpt = Nothing

        End Select
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        If rpt Is Nothing Then

            Dim udtEHSTransaction As EHSTransactionModel = udtSessionHandler.EHSTransactionGetFromSession(strFunctCode)
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

            ' Set fallback font
            pdf.FontFallback = Common.Component.Printout.PrintoutBLL.FallbackFont()

            ' Create a new memory stream that will hold the pdf output
            Dim memStream As New System.IO.MemoryStream()
            ' Export the report to PDF, Write the PDF stream out and Send all buffered content to the client
            pdf.Export(rpt.Document, memStream)
            Response.BinaryWrite(memStream.ToArray())
            Response.End()
        End If
    End Sub

    MustOverride Function GetReport() As GrapeCity.ActiveReports.SectionReport

    ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
    ' -----------------------------------------------------------------------------------------
    'Private Function BulidConsentFormInformation(ByVal strFunctCode As String) As ConsentFormEHS.ConsentFormInformationModel
    Private Function BulidConsentFormInformation(ByVal strFunctCode As String) As ConsentFormInformationModel
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
        Dim udtFormatter As New Common.Format.Formatter

        ' Get required object from session

        Dim udtSchemeClaim As SchemeClaimModel = udtSessionHandler.SchemeSelectedGetFromSession(strFunctCode)
        Dim udtEHSAccount As EHSAccountModel = udtSessionHandler.EHSAccountGetFromSession(strFunctCode)
        Dim udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode)
        Dim udtEHSTransaction As EHSTransactionModel = udtSessionHandler.EHSTransactionGetFromSession(strFunctCode)
        Dim udtSmartIDContent As BLL.SmartIDContentModel = udtSessionHandler.SmartIDContentGetFormSession(strFunctCode)
        Dim udtSP As ServiceProviderModel = Nothing

        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Dim udtSubsidizeFeeModel As SubsidizeFeeModel = Nothing

        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        udtSessionHandler.CurrentUserGetFromSession(udtSP, Nothing)

        ' Build Model
        Dim obj As New ConsentFormInformationModel
        obj.Platform = ConsentFormInformationModel.EnumPlatform.HCSP
        obj.FormType = udtSchemeClaim.SchemeCode

        obj.Language = Me.Language
        obj.FormStyle = Me.FormStyle
        obj.NeedPassword = ConsentFormInformationModel.NeedPasswordClass.No
        obj.DocType = udtEHSPersonalInformation.DocCode

        Select Case Me.Language
            Case ConsentFormInformationModel.LanguageClassInternal.English
                obj.SPName = udtSP.EnglishName

            Case ConsentFormInformationModel.LanguageClassInternal.Chinese,
                ConsentFormInformationModel.LanguageClassInternal.SimpChinese
                If udtSP.ChineseName.Trim <> String.Empty Then
                    obj.SPName = udtSP.ChineseName
                Else
                    obj.SPName = udtSP.EnglishName
                End If

            Case Else
                Throw New Exception(String.Format("BasePrintoutForm unhandled Language({0})", Me.Language))
        End Select

        obj.RecipientCName = udtEHSPersonalInformation.CName
        obj.RecipientEName = udtEHSPersonalInformation.EName

        If Not udtSmartIDContent Is Nothing AndAlso udtSmartIDContent.IsReadSmartID Then
            obj.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Yes
        Else
            obj.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.No
        End If

        obj.DocNo = udtEHSPersonalInformation.IdentityNum.Trim()
        Select Case obj.DocType
            Case ConsentFormInformationModel.DocTypeClass.HKIC
                obj.DOI = udtEHSPersonalInformation.DateofIssue
            Case ConsentFormInformationModel.DocTypeClass.EC

                obj.ECSerialNo = udtEHSPersonalInformation.ECSerialNo
                obj.ECReferenceNo = udtEHSPersonalInformation.ECReferenceNo
                obj.DOI = udtEHSPersonalInformation.DateofIssue

            Case ConsentFormInformationModel.DocTypeClass.HKBC
                ' Do Nothing
            Case ConsentFormInformationModel.DocTypeClass.REPMT

                obj.DOI = udtEHSPersonalInformation.DateofIssue

            Case ConsentFormInformationModel.DocTypeClass.DocI

                obj.DOI = udtEHSPersonalInformation.DateofIssue

            Case ConsentFormInformationModel.DocTypeClass.ID235B

                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'obj.PermitUntil = udtFormatter.formatDate(udtEHSPersonalInformation.PermitToRemainUntil.Value, Me.Language)
                obj.PermitUntil = udtFormatter.formatID235BPermittedToRemainUntil(udtEHSPersonalInformation.PermitToRemainUntil.Value)
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            Case ConsentFormInformationModel.DocTypeClass.VISA
                ' Do Nothing
            Case ConsentFormInformationModel.DocTypeClass.ADOPC
                ' Do Nothing
        End Select

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtSubPlatformBLL As New SubPlatformBLL
        'obj.ServiceDate = udtFormatter.formatDate(udtEHSTransaction.ServiceDate, Me.Language)
        'obj.SignDate = udtFormatter.formatDate(udtEHSTransaction.TransactionDtm, Me.Language)
        obj.ServiceDate = udtFormatter.formatDisplayDate(udtEHSTransaction.ServiceDate, Me.Language)
        obj.SignDate = udtFormatter.formatDisplayDate(udtEHSTransaction.TransactionDtm, Me.Language)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Select Case obj.FormType
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Case ConsentFormInformationModel.FormTypeClass.HCVS, _
                ConsentFormInformationModel.FormTypeClass.HCVSDHC

                'Get Practice Name
                Dim udtPractice As PracticeModel = udtSP.PracticeList.Item(udtEHSTransaction.PracticeID)

                Select Case Me.Language
                    Case ConsentFormInformationModel.LanguageClassInternal.English
                        obj.PracticeName = udtPractice.PracticeName

                        Select Case udtPractice.Professional.ServiceCategoryCode.Trim
                            Case "DIT"
                                obj.ProfessionDesc = HttpContext.GetGlobalResourceObject("Text", "Dietitian", _
                                                                                         New System.Globalization.CultureInfo(Common.Component.CultureLanguage.English))
                            Case "POD"
                                obj.ProfessionDesc = HttpContext.GetGlobalResourceObject("Text", "Podiatrist", _
                                                                                         New System.Globalization.CultureInfo(Common.Component.CultureLanguage.English))
                            Case "SPT"
                                obj.ProfessionDesc = HttpContext.GetGlobalResourceObject("Text", "SpeechTherapist", _
                                                                                         New System.Globalization.CultureInfo(Common.Component.CultureLanguage.English))
                            Case Else
                                obj.ProfessionDesc = udtPractice.Professional.ServiceCategoryDesc
                        End Select

                    Case ConsentFormInformationModel.LanguageClassInternal.Chinese,
                        ConsentFormInformationModel.LanguageClassInternal.SimpChinese
                        If udtPractice.PracticeNameChi.Trim <> String.Empty Then
                            obj.PracticeName = udtPractice.PracticeNameChi
                        Else
                            obj.PracticeName = udtPractice.PracticeName

                        End If
                        obj.ProfessionDesc = udtPractice.Professional.ServiceCategoryDescChi

                End Select

                If obj.FormType = ConsentFormInformationModel.FormTypeClass.HCVSDHC Then
                    obj.DisplayPracticeName = True
                End If
                ' CRE19-006 (DHC) [End][Winnie]

                ' Voucher Claim
                obj.VoucherClaim = udtEHSTransaction.VoucherClaim.ToString
                obj.VoucherAfterRedeem = udtEHSTransaction.VoucherAfterRedeem.ToString
                obj.VoucherBeforeRedeem = udtEHSTransaction.VoucherBeforeRedeem.ToString

                ' Co-Payment Fee
                If udtEHSTransaction.TransactionAdditionFields.CoPaymentFee.HasValue Then
                    obj.CoPaymentFee = udtEHSTransaction.TransactionAdditionFields.CoPaymentFee.Value
                Else
                    obj.CoPaymentFee = String.Empty
                End If

                ' Subsidize Fee
                udtSubsidizeFeeModel = udtSchemeClaimBLL.getAllSubsidizeFee().Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher)

                If udtSubsidizeFeeModel.SubsidizeFee.HasValue Then
                    obj.SubsidizeFee = udtSubsidizeFeeModel.SubsidizeFee.Value
                End If

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Case ConsentFormInformationModel.FormTypeClass.HCVSC
                ' Voucher Claim
                obj.VoucherClaim = udtEHSTransaction.VoucherClaim.ToString
                obj.VoucherAfterRedeem = udtEHSTransaction.VoucherAfterRedeem.ToString
                obj.VoucherBeforeRedeem = udtEHSTransaction.VoucherBeforeRedeem.ToString

                obj.VoucherClaimRMB = udtEHSTransaction.VoucherClaimRMB.ToString

                If udtEHSTransaction.ExchangeRate.HasValue Then
                    obj.ExchangeRate = udtEHSTransaction.ExchangeRate.Value
                End If

                'Get MO Name
                Dim udtPractice As PracticeModel = udtSP.PracticeList.Item(udtEHSTransaction.PracticeID)
                Dim strMODisplaySeq As String = udtPractice.MODisplaySeq

                Dim udtMO As MedicalOrganizationModel = udtSP.MOList.Item(udtPractice.MODisplaySeq)


                Select Case Me.Language
                    Case ConsentFormInformationModel.LanguageClassInternal.English
                        obj.MOName = udtMO.MOEngName

                    Case ConsentFormInformationModel.LanguageClassInternal.Chinese,
                        ConsentFormInformationModel.LanguageClassInternal.SimpChinese
                        If udtMO.MOChiName.Trim <> String.Empty Then
                            obj.MOName = udtMO.MOChiName
                        Else
                            obj.MOName = udtMO.MOEngName
                        End If
                End Select

                ' Co-Payment Fee RMB
                If udtEHSTransaction.TransactionAdditionFields.CoPaymentFeeRMB.HasValue Then
                    obj.CoPaymentFeeRMB = udtEHSTransaction.TransactionAdditionFields.CoPaymentFeeRMB.Value
                Else
                    obj.CoPaymentFeeRMB = String.Empty
                End If

                ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                ' Subsidize Fee
                udtSubsidizeFeeModel = udtSchemeClaimBLL.getAllSubsidizeFee().Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher)

                If udtSubsidizeFeeModel.SubsidizeFee.HasValue Then
                    obj.SubsidizeFee = udtSubsidizeFeeModel.SubsidizeFee.Value
                End If
                ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
                'CRE13-019-02 Extend HCVS to China [End][Winnie]

                ' CRE20-0XX (HA Scheme) [Start][Winnie]
            Case ConsentFormInformationModel.FormTypeClass.SSSCMC
                ' Voucher Claim
                obj.VoucherClaimRMB = udtEHSTransaction.VoucherClaimRMB.ToString
                obj.VoucherAfterRedeem = udtEHSTransaction.TransactionAdditionFields.SubsidyAfterClaim
                obj.VoucherBeforeRedeem = udtEHSTransaction.TransactionAdditionFields.SubsidyBeforeClaim

                'MO Name
                Dim udtPractice As PracticeModel = udtSP.PracticeList.Item(udtEHSTransaction.PracticeID)
                Dim strMODisplaySeq As String = udtPractice.MODisplaySeq

                Dim udtMO As MedicalOrganizationModel = udtSP.MOList.Item(udtPractice.MODisplaySeq)

                Select Case Me.Language
                    Case ConsentFormInformationModel.LanguageClassInternal.English
                        obj.MOName = udtMO.MOEngName

                    Case ConsentFormInformationModel.LanguageClassInternal.Chinese,
                        ConsentFormInformationModel.LanguageClassInternal.SimpChinese
                        If udtMO.MOChiName.Trim <> String.Empty Then
                            obj.MOName = udtMO.MOChiName
                        Else
                            obj.MOName = udtMO.MOEngName
                        End If
                End Select

                ' Co-Payment Fee & Subsidy Fee RMB
                Select Case udtEHSTransaction.TransactionDetails(0).SubsidizeCode.Trim
                    Case SubsidizeGroupClaimModel.SubsidizeCodeClass.HAS_A
                        obj.CoPaymentFeeRMB = udtEHSTransaction.TransactionAdditionFields.RegistrationFeeRMB
                        obj.SubsidyFeeRMB = 0

                    Case SubsidizeGroupClaimModel.SubsidizeCodeClass.HAS_B
                        obj.CoPaymentFeeRMB = 0
                        obj.SubsidyFeeRMB = udtEHSTransaction.TransactionAdditionFields.RegistrationFeeRMB
                End Select
        ' CRE20-0XX (HA Scheme) [End][Winnie]

            Case ConsentFormInformationModel.FormTypeClass.CIVSS

                ' Dose
                Select Case udtEHSTransaction.TransactionDetails(0).AvailableItemCode
                    Case Common.Component.SchemeDetails.SubsidizeItemDetailsModel.DoseCode.FirstDOSE
                        obj.SubsidyInfo = ConsentFormInformationModel.CIVSSSubsidyInfoClass.Dose1
                    Case Common.Component.SchemeDetails.SubsidizeItemDetailsModel.DoseCode.SecondDOSE
                        obj.SubsidyInfo = ConsentFormInformationModel.CIVSSSubsidyInfoClass.Dose2
                    Case Common.Component.SchemeDetails.SubsidizeItemDetailsModel.DoseCode.ONLYDOSE
                        obj.SubsidyInfo = ConsentFormInformationModel.CIVSSSubsidyInfoClass.DoseOnly
                End Select

                ' Pre School
                If udtEHSTransaction.PreSchool = "Y" Then
                    obj.Preschool = ConsentFormInformationModel.PreschoolClass.Preschool
                Else
                    obj.Preschool = ConsentFormInformationModel.PreschoolClass.NonPreschool
                End If

                If obj.SubsidyInfo <> ConsentFormInformationModel.CIVSSSubsidyInfoClass.Dose1 Then
                    obj.Preschool = ConsentFormInformationModel.PreschoolClass.Not1stDose
                ElseIf obj.SubsidyInfo = String.Empty Then
                    obj.Preschool = ConsentFormInformationModel.PreschoolClass.Unknown
                End If

            Case ConsentFormInformationModel.FormTypeClass.EVSS
                For Each udtTranDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
                    If obj.SubsidyInfo.Length > 0 Then
                        obj.SubsidyInfo += ","
                    End If

                    For Each udtSubsidy As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList
                        If udtSubsidy.SubsidizeCode = udtTranDetail.SubsidizeCode Then
                            obj.SubsidyInfo = udtSubsidy.SubsidizeDisplayCode
                            Continue For
                        End If
                    Next

                Next

            Case ConsentFormInformationModel.FormTypeClass.VSS
                If udtEHSTransaction.TransactionDetails(0).SubsidizeItemCode = SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19 Then

                Else
                    For Each udtTranDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
                        If obj.SubsidyInfo.Length > 0 Then
                            obj.SubsidyInfo += ","
                        End If

                        For Each udtSubsidy As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList
                            If udtSubsidy.SubsidizeCode = udtTranDetail.SubsidizeCode Then
                                obj.SubsidyInfo = udtSubsidy.SubsidizeDisplayCode
                                Continue For
                            End If
                        Next

                    Next
                End If

        End Select

        obj.Gender = udtEHSPersonalInformation.Gender
        obj.DOB = udtFormatter.formatDOB(udtEHSPersonalInformation.DOB, udtEHSPersonalInformation.ExactDOB, Me.Language, udtEHSPersonalInformation.ECAge, udtEHSPersonalInformation.ECDateOfRegistration)

        Return obj
    End Function

End Class
