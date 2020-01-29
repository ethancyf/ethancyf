Imports Common.ComObject
Imports Common.Component
Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Export.Pdf
Imports System.IO

' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
' -----------------------------------------------------------------------------------------

Imports System.Reflection
Imports Common.Component.VersionControl

' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

Partial Public Class GenCSF
    Inherits System.Web.UI.Page

#Region "Private class"

    Private Class SESS
        Public Const CFInfo As String = "CFInfo"
        Public Const Title As String = "Title"
    End Class

    Private Class ViewIndex
        Public Const InputPassword As Integer = 0
        Public Const ErrorPage As Integer = 1
    End Class

    Private Class AuditLogDescription
        Public Const ProcessFormDataStart As String = "Process Form Data start" ' 00001
        Public Const ProcessFormDataSucceed As String = "Process Form Data succeed" ' 00002
        Public Const ProcessFormDataFail As String = "Process Form Data fail" ' 00003
        Public Const DisplayPasswordInputWindow As String = "Display Password Input Window" ' 00004
        Public Const SubmitPasswordStart As String = "Submit Password start" ' 00005
        Public Const SubmitPasswordSucceed As String = "Submit Password succeed" ' 00006
        Public Const SubmitPasswordFail As String = "Submit Password fail" ' 00007
        Public Const GenerateConsentFormFileStart As String = "Generate Consent Form File start" ' 00008
        Public Const GenerateConsentFormFileSucceed As String = "Generate Consent Form File succeed" ' 00009
        Public Const GenerateConsentFormFileFail As String = "Generate Consent Form File fail" ' 00010
        Public Const OpenConsentFormFile As String = "Open Consent Form File" ' 00011
    End Class

    Private Class ErrorMessage
        Public Const MissingData As String = "Missing Data!"
        Public Const InvalidData As String = "Invalid Data!"
        Public Const InvalidDataLength As String = "Invalid Data Length!"
    End Class

    Private Const FunctionCode As String = "080101"

#End Region

#Region "Page event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session(SESS.Title)) Then Me.Title = Session(SESS.Title)

        If Not Page.IsPostBack Then
            Session.Clear()

            Dim udtAuditLog As AuditLogEntry = LogProcessFormDataStart(Request)

            Dim udtCFInfo As ConsentFormInformationModel = New ConsentFormInformationBLL(Request).FillConsentFormInformation()

            ' Set title
            Dim udtGeneralFunction As New GeneralFunction

            Dim strLanguage As String = ConsentFormInformationModel.LanguageClassInternal.English
            If udtCFInfo.Language = ConsentFormInformationModel.LanguageClassInternal.Chinese Then
                strLanguage = ConsentFormInformationModel.LanguageClassInternal.Chinese
            End If

            Session(SESS.Title) = udtGeneralFunction.GetSystemResource("Text", "EHealthSystemConsentFormGenerationService", strLanguage)
            Me.Title = Session(SESS.Title)

            ' Validation
            If Validation(udtCFInfo, udtAuditLog) = False Then
                ' Display error in EHS

                imgBannerError.ImageUrl = udtGeneralFunction.GetSystemResource("ImageUrl", "FGSBanner", strLanguage)

                lblValidationFail.Text = udtGeneralFunction.GetSystemResource("Text", "ValidationFail", strLanguage)
                lblInvalidInputInformation.Text = udtGeneralFunction.GetSystemResource("Text", "InvalidInputInformation", strLanguage)

                mvGenerate.ActiveViewIndex = ViewIndex.ErrorPage

                Dim sb As New StringBuilder
                sb.Append("<script>")
                sb.Append("resizeMyselfError();")
                sb.Append("</script>")
                ClientScript.RegisterStartupScript(Me.GetType, "test", sb.ToString())

                Return
            End If

            ' Check need password
            If udtCFInfo.NeedPassword = ConsentFormInformationModel.NeedPasswordClass.Yes Then
                Session(SESS.CFInfo) = udtCFInfo

                imgBannerPassword.ImageUrl = udtGeneralFunction.GetSystemResource("ImageUrl", "FGSBanner", strLanguage)

                lblPassword.Text = udtGeneralFunction.GetSystemResource("Text", "PleaseEnterConsentFormPassword", udtCFInfo.Language)
                ibtnGenerate.Text = udtGeneralFunction.GetSystemResource("Text", "Submit", udtCFInfo.Language)
                lblPasswordError.Text = udtGeneralFunction.GetSystemResource("Text", "PasswordCannotBeEmpty", udtCFInfo.Language)

                lblPasswordError.Visible = False
                mvGenerate.ActiveViewIndex = ViewIndex.InputPassword

                LogDisplayPasswordInputWindow()

                Dim sb As New StringBuilder
                sb.Append("<script>")
                sb.Append("resizeMyselfPassword();")
                sb.Append("</script>")
                ClientScript.RegisterStartupScript(Me.GetType, "test", sb.ToString())

                Return
            End If

            LoadReport(udtCFInfo)

        End If

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ' Setting the secure flag in the ASP.NET Session id cookie
        Request.Cookies("ASP.NET_SessionId").Secure = True
    End Sub

    Private Function Validation(ByVal udtCFInfo As ConsentFormInformationModel, ByVal udtAuditLog As AuditLogEntry) As Boolean
        ' ----- Control Information -----

        ' FormType is mandatory
        If udtCFInfo.FormType = String.Empty Then
            LogProcessFormDataFail(udtAuditLog, "FormType", ErrorMessage.MissingData)
            Return False
        End If

        ' FormType is not accepted
        If udtCFInfo.FormType <> ConsentFormInformationModel.FormTypeClass.HCVS _
                AndAlso udtCFInfo.FormType <> ConsentFormInformationModel.FormTypeClass.CIVSS _
                AndAlso udtCFInfo.FormType <> ConsentFormInformationModel.FormTypeClass.EVSS Then
            LogProcessFormDataFail(udtAuditLog, "FormType", ErrorMessage.InvalidData)
            Return False
        End If

        ' Language is mandatory
        If udtCFInfo.Language = String.Empty Then
            LogProcessFormDataFail(udtAuditLog, "Language", ErrorMessage.MissingData)
            Return False
        End If

        ' Language is not accepted
        If udtCFInfo.Language <> ConsentFormInformationModel.LanguageClassInternal.Chinese _
                AndAlso udtCFInfo.Language <> ConsentFormInformationModel.LanguageClassInternal.English Then
            LogProcessFormDataFail(udtAuditLog, "Language", ErrorMessage.InvalidData)
            Return False
        End If

        ' FormStyle is not accepted
        If udtCFInfo.FormStyle <> ConsentFormInformationModel.FormStyleClass.Full _
                AndAlso udtCFInfo.FormStyle <> ConsentFormInformationModel.FormStyleClass.Condensed Then
            LogProcessFormDataFail(udtAuditLog, "FormStyle", ErrorMessage.InvalidData)
            Return False
        End If

        ' NeedPassword is not accepted
        If udtCFInfo.NeedPassword <> String.Empty _
                AndAlso udtCFInfo.NeedPassword <> ConsentFormInformationModel.NeedPasswordClass.Yes _
                AndAlso udtCFInfo.NeedPassword <> ConsentFormInformationModel.NeedPasswordClass.No Then
            LogProcessFormDataFail(udtAuditLog, "NeedPassword", ErrorMessage.InvalidData)
            Return False
        End If

        ' DocType is mandatory
        If udtCFInfo.DocType = String.Empty Then
            LogProcessFormDataFail(udtAuditLog, "DocType", ErrorMessage.MissingData)
            Return False
        End If

        ' DocType is not accepted
        If udtCFInfo.DocType <> ConsentFormInformationModel.DocTypeClass.ADOPC _
                AndAlso udtCFInfo.DocType <> ConsentFormInformationModel.DocTypeClass.DocI _
                AndAlso udtCFInfo.DocType <> ConsentFormInformationModel.DocTypeClass.EC _
                AndAlso udtCFInfo.DocType <> ConsentFormInformationModel.DocTypeClass.HKBC _
                AndAlso udtCFInfo.DocType <> ConsentFormInformationModel.DocTypeClass.HKIC _
                AndAlso udtCFInfo.DocType <> ConsentFormInformationModel.DocTypeClass.ID235B _
                AndAlso udtCFInfo.DocType <> ConsentFormInformationModel.DocTypeClass.REPMT _
                AndAlso udtCFInfo.DocType <> ConsentFormInformationModel.DocTypeClass.VISA Then
            LogProcessFormDataFail(udtAuditLog, "DocType", ErrorMessage.InvalidData)
            Return False
        End If

        ' DocType and FormType not matched
        Dim blnDocTypeFormTypeOK As Boolean = True

        Select Case udtCFInfo.DocType
            Case ConsentFormInformationModel.DocTypeClass.HKBC
                If udtCFInfo.FormType <> ConsentFormInformationModel.FormTypeClass.CIVSS Then blnDocTypeFormTypeOK = False

            Case ConsentFormInformationModel.DocTypeClass.REPMT
                If udtCFInfo.FormType <> ConsentFormInformationModel.FormTypeClass.CIVSS Then blnDocTypeFormTypeOK = False

            Case ConsentFormInformationModel.DocTypeClass.DocI
                If udtCFInfo.FormType <> ConsentFormInformationModel.FormTypeClass.CIVSS Then blnDocTypeFormTypeOK = False

            Case ConsentFormInformationModel.DocTypeClass.ID235B
                If udtCFInfo.FormType <> ConsentFormInformationModel.FormTypeClass.CIVSS Then blnDocTypeFormTypeOK = False

            Case ConsentFormInformationModel.DocTypeClass.VISA
                If udtCFInfo.FormType <> ConsentFormInformationModel.FormTypeClass.CIVSS Then blnDocTypeFormTypeOK = False

            Case ConsentFormInformationModel.DocTypeClass.ADOPC
                If udtCFInfo.FormType <> ConsentFormInformationModel.FormTypeClass.CIVSS Then blnDocTypeFormTypeOK = False

            Case ConsentFormInformationModel.DocTypeClass.EC
                If udtCFInfo.FormType <> ConsentFormInformationModel.FormTypeClass.HCVS _
                        AndAlso udtCFInfo.FormType <> ConsentFormInformationModel.FormTypeClass.EVSS Then
                    blnDocTypeFormTypeOK = False
                End If

        End Select

        If Not blnDocTypeFormTypeOK Then
            LogProcessFormDataFail(udtAuditLog, "DocType + FormType", ErrorMessage.InvalidData)
            Return False
        End If


        ' ----- SP Information -----

        ' Service Provider Name
        If udtCFInfo.SPName.Length > 40 Then
            LogProcessFormDataFail(udtAuditLog, "SPName", ErrorMessage.InvalidDataLength)
            Return False
        End If


        ' ----- Recipient Information -----

        ' Recipient English Name
        If udtCFInfo.RecipientEName.Length > 40 Then
            LogProcessFormDataFail(udtAuditLog, "RecpName", ErrorMessage.InvalidDataLength)
            Return False
        End If

        ' Recipient Chinese Name
        If udtCFInfo.RecipientCName.Length > 6 Then
            LogProcessFormDataFail(udtAuditLog, "RecpNameChi", ErrorMessage.InvalidDataLength)
            Return False
        End If

        ' Recipient DOB
        If udtCFInfo.DOB.Length > 40 Then
            LogProcessFormDataFail(udtAuditLog, "RecpDOBStr", ErrorMessage.InvalidDataLength)
            Return False
        End If

        ' Gender
        If udtCFInfo.Gender.Length > 1 Then
            LogProcessFormDataFail(udtAuditLog, "RecpGender", ErrorMessage.InvalidDataLength)
            Return False
        End If

        ' Gender is not accepted
        If udtCFInfo.Gender <> String.Empty _
                AndAlso udtCFInfo.Gender <> ConsentFormInformationModel.GenderClass.Male _
                AndAlso udtCFInfo.Gender <> ConsentFormInformationModel.GenderClass.Female Then
            LogProcessFormDataFail(udtAuditLog, "Gender", ErrorMessage.InvalidData)
            Return False
        End If

        ' Document No.
        If udtCFInfo.DocNo.Length > 25 Then
            LogProcessFormDataFail(udtAuditLog, "DocumentNo", ErrorMessage.InvalidDataLength)
            Return False
        End If

        ' Date of Issue
        If udtCFInfo.DOI.Length > 25 Then
            LogProcessFormDataFail(udtAuditLog, "DOI", ErrorMessage.InvalidDataLength)
            Return False
        End If

        ' Remain Until
        If udtCFInfo.PermitUntil.Length > 25 Then
            LogProcessFormDataFail(udtAuditLog, "ID235BRemainUntil", ErrorMessage.InvalidDataLength)
            Return False
        End If

        ' Passport No.
        If udtCFInfo.PassportNo.Length > 25 Then
            LogProcessFormDataFail(udtAuditLog, "PassportNo", ErrorMessage.InvalidDataLength)
            Return False
        End If

        ' EC Serial No.
        If udtCFInfo.ECSerialNo.Length > 25 Then
            LogProcessFormDataFail(udtAuditLog, "ECSerialNo", ErrorMessage.InvalidDataLength)
            Return False
        End If

        ' EC Reference No.
        If udtCFInfo.ECReferenceNo.Length > 25 Then
            LogProcessFormDataFail(udtAuditLog, "ECRefNo", ErrorMessage.InvalidDataLength)
            Return False
        End If


        ' ----- Claim Information -----

        ' Service Date
        If udtCFInfo.ServiceDate.Length > 25 Then
            LogProcessFormDataFail(udtAuditLog, "ServiceDateStr", ErrorMessage.InvalidDataLength)
            Return False
        End If

        ' Sign Date
        If udtCFInfo.SignDate.Length > 25 Then
            LogProcessFormDataFail(udtAuditLog, "SignDateStr", ErrorMessage.InvalidDataLength)
            Return False
        End If

        ' Use Smart IC
        If udtCFInfo.ReadSmartID <> ConsentFormInformationModel.ReadSmartIDClass.Yes _
                AndAlso udtCFInfo.ReadSmartID <> ConsentFormInformationModel.ReadSmartIDClass.No _
                AndAlso udtCFInfo.ReadSmartID <> ConsentFormInformationModel.ReadSmartIDClass.Unknown Then
            LogProcessFormDataFail(udtAuditLog, "UseSmartIC", ErrorMessage.InvalidData)
            Return False
        End If

        ' No. of Vouchers
        If udtCFInfo.VoucherClaim.Length > 3 Then
            LogProcessFormDataFail(udtAuditLog, "VoucherClaimed", ErrorMessage.InvalidDataLength)
            Return False
        End If

        ' No. of Vouchers (Content)
        If udtCFInfo.VoucherClaim <> String.Empty Then
            Try
                Dim intVoucherClaim As Integer = CInt(udtCFInfo.VoucherClaim)

                If intVoucherClaim <= 0 OrElse intVoucherClaim > 999 Then
                    LogProcessFormDataFail(udtAuditLog, "VoucherClaimed", ErrorMessage.InvalidData)
                    Return False
                End If

            Catch ex As Exception
                LogProcessFormDataFail(udtAuditLog, "VoucherClaimed", ErrorMessage.InvalidData)
                Return False
            End Try

        End If

        ' Subsidy Code
        Select Case udtCFInfo.FormType
            Case ConsentFormInformationModel.FormTypeClass.CIVSS
                If udtCFInfo.SubsidyInfo.Length > 20 Then
                    LogProcessFormDataFail(udtAuditLog, "SubsidyCode", ErrorMessage.InvalidDataLength)
                    Return False
                End If

                If udtCFInfo.SubsidyInfo <> String.Empty _
                        AndAlso udtCFInfo.SubsidyInfo <> ConsentFormInformationModel.CIVSSSubsidyInfoClass.Dose1 _
                        AndAlso udtCFInfo.SubsidyInfo <> ConsentFormInformationModel.CIVSSSubsidyInfoClass.Dose2 _
                        AndAlso udtCFInfo.SubsidyInfo <> ConsentFormInformationModel.CIVSSSubsidyInfoClass.DoseOnly Then
                    LogProcessFormDataFail(udtAuditLog, "SubsidyCode", ErrorMessage.InvalidData)
                    Return False
                End If

                If udtCFInfo.SubsidyInfo = ConsentFormInformationModel.CIVSSSubsidyInfoClass.Dose1 _
                        OrElse udtCFInfo.SubsidyInfo = ConsentFormInformationModel.CIVSSSubsidyInfoClass.DoseOnly Then
                    If udtCFInfo.Preschool <> String.Empty _
                            AndAlso udtCFInfo.Preschool <> ConsentFormInformationModel.PreschoolClass.Preschool _
                            AndAlso udtCFInfo.Preschool <> ConsentFormInformationModel.PreschoolClass.NonPreschool _
                            AndAlso udtCFInfo.Preschool <> ConsentFormInformationModel.PreschoolClass.Not1stDose Then
                        LogProcessFormDataFail(udtAuditLog, "CIVSSPreSchool", ErrorMessage.InvalidData)
                        Return False
                    End If
                End If

            Case ConsentFormInformationModel.FormTypeClass.EVSS
                Dim blnValid As Boolean = True

                If udtCFInfo.SubsidyInfo <> String.Empty Then
                    For Each strSubsidyCode As String In udtCFInfo.SubsidyInfo.Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                        If strSubsidyCode.Length > 20 Then
                            LogProcessFormDataFail(udtAuditLog, "SubsidyCode", ErrorMessage.InvalidDataLength)
                            Return False
                        End If

                        If strSubsidyCode.Trim <> String.Empty _
                                AndAlso strSubsidyCode.Trim <> ConsentFormInformationModel.EVSSSubsidyInfoClass.PV _
                                AndAlso strSubsidyCode.Trim <> ConsentFormInformationModel.EVSSSubsidyInfoClass.SIV Then
                            blnValid = False
                        End If
                    Next

                    If blnValid = False Then
                        LogProcessFormDataFail(udtAuditLog, "SubsidyCode", ErrorMessage.InvalidData)
                        Return False
                    End If

                End If

        End Select

        LogProcessFormDataSucceed(udtAuditLog)

        Return True

    End Function

#End Region

#Region "Audit Log Function"

    Private Function LogProcessFormDataStart(ByVal request As HttpRequest) As AuditLogEntry
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, DBFlag.dbEVS_InterfaceLog)

        ' Concat the request
        Dim sbRequest As New StringBuilder

        For Each strKey As String In request.Form.AllKeys
            sbRequest.Append(String.Format("<{0}", strKey))

            If Not IsNothing(request.Form(strKey)) AndAlso request.Form(strKey) <> String.Empty Then
                sbRequest.Append(String.Format(": {0}", request.Form(strKey)))
            End If

            sbRequest.Append(">")
        Next

        udtAuditLog.WriteLogData(LogID.LOG00001, AuditLogDescription.ProcessFormDataStart, sbRequest.ToString)

        Return udtAuditLog

    End Function

    Private Sub LogProcessFormDataSucceed(ByVal udtAuditLog As AuditLogEntry)
        udtAuditLog.WriteEndLog(LogID.LOG00002, AuditLogDescription.ProcessFormDataSucceed)
    End Sub

    Private Sub LogProcessFormDataFail(ByVal udtAuditLog As AuditLogEntry, ByVal strFormData As String, ByVal strError As String)
        udtAuditLog.AddDescripton("FormData", strFormData)
        udtAuditLog.AddDescripton("ErrorMessage", strError)

        udtAuditLog.WriteEndLog(LogID.LOG00003, AuditLogDescription.ProcessFormDataFail)

    End Sub

    Private Sub LogDisplayPasswordInputWindow()
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, DBFlag.dbEVS_InterfaceLog)
        udtAuditLog.WriteLog(LogID.LOG00004, AuditLogDescription.DisplayPasswordInputWindow)

    End Sub

    Private Function LogSubmitPasswordStart(ByVal strPassword As String) As AuditLogEntry
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, DBFlag.dbEVS_InterfaceLog)
        udtAuditLog.AddDescripton("Password", strPassword)
        udtAuditLog.WriteStartLog(LogID.LOG00005, AuditLogDescription.SubmitPasswordStart)

        Return udtAuditLog

    End Function

    Private Sub LogSubmitPasswordSucceed(ByVal udtAuditLog As AuditLogEntry)
        udtAuditLog.WriteEndLog(LogID.LOG00006, AuditLogDescription.SubmitPasswordSucceed)
    End Sub

    Private Sub LogSubmitPasswordFail(ByVal udtAuditLog As AuditLogEntry)
        udtAuditLog.WriteEndLog(LogID.LOG00007, AuditLogDescription.SubmitPasswordFail)
    End Sub

    Private Function LogGenerateConsentFormFileStart(ByVal strFormType As String, ByVal strLanguage As String, ByVal strFormStyle As String, ByVal strFileName As String) As AuditLogEntry
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, DBFlag.dbEVS_InterfaceLog)

        udtAuditLog.AddDescripton("FormType", strFormType)
        udtAuditLog.AddDescripton("Language", strLanguage)
        udtAuditLog.AddDescripton("FormStyle", strFormStyle)
        udtAuditLog.AddDescripton("FileName", strFileName)

        udtAuditLog.WriteStartLog(LogID.LOG00008, AuditLogDescription.GenerateConsentFormFileStart)

        Return udtAuditLog

    End Function

    Private Sub LogGenerateConsentFormFileSucceed(ByVal udtAuditLog As AuditLogEntry)
        udtAuditLog.WriteEndLog(LogID.LOG00009, AuditLogDescription.GenerateConsentFormFileSucceed)
    End Sub

    Private Sub LogGenerateConsentFormFileFail(ByVal udtAuditLog As AuditLogEntry)
        udtAuditLog.WriteEndLog(LogID.LOG00010, AuditLogDescription.GenerateConsentFormFileFail)
    End Sub

    Private Sub LogOpenConsentFormFile(ByVal strFormType As String, ByVal strLanguage As String, ByVal strFormStyle As String, ByVal strFileName As String, ByVal strURI As String)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, DBFlag.dbEVS_InterfaceLog)

        udtAuditLog.AddDescripton("FormType", strFormType)
        udtAuditLog.AddDescripton("Language", strLanguage)
        udtAuditLog.AddDescripton("FormStyle", strFormStyle)
        udtAuditLog.AddDescripton("FileName", strFileName)
        udtAuditLog.AddDescripton("URI", strURI)

        udtAuditLog.WriteLog(LogID.LOG00011, AuditLogDescription.OpenConsentFormFile)

    End Sub

#End Region

    Private Sub ibtnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ibtnGenerate.Click
        Dim strPassword As String = txtPassword.Text

        Dim udtAuditLog As AuditLogEntry = LogSubmitPasswordStart(strPassword)

        If txtPassword.Text.Trim = String.Empty Then
            lblPasswordError.Visible = True
            LogSubmitPasswordFail(udtAuditLog)

            Return
        End If

        LogSubmitPasswordSucceed(udtAuditLog)

        LoadReport(Session(SESS.CFInfo), txtPassword.Text.Trim)

    End Sub

    '

    Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel, Optional ByVal strPassword As String = "")
        ' Randomize a new file name
        Dim strFileName As String = GenerateReportName(udtCFInfo)

        Dim udtAuditLog As AuditLogEntry = LogGenerateConsentFormFileStart(udtCFInfo.FormType, udtCFInfo.Language, udtCFInfo.FormStyle, strFileName)

        Dim rpt As ActiveReport3 = SelectReport(udtCFInfo)

        rpt.Document.Printer.PrinterName = String.Empty

        Try
            rpt.Run(False)
        Catch ex As ReportException
            udtAuditLog.AddDescripton("Exception Location", "rpt.Run")
            udtAuditLog.AddDescripton("ReportException", ex.Message)
            LogGenerateConsentFormFileFail(udtAuditLog)
            Return
        End Try

        Dim pdf As New PdfExport()

        If strPassword <> String.Empty Then
            pdf.Security.UserPassword = strPassword
            pdf.Security.Encrypt = True
        End If

        Dim strReportSavingLocation As String = ConfigurationManager.AppSettings("ReportSavingLocation")

        ' Save the file
        Try
            pdf.Export(rpt.Document, String.Format("{0}{1}", strReportSavingLocation, strFileName))
        Catch ex As Exception
            udtAuditLog.AddDescripton("Exception Location", "pdf.Export")
            udtAuditLog.AddDescripton("Exception", ex.Message)
            LogGenerateConsentFormFileFail(udtAuditLog)
            Throw ex
        End Try

        LogGenerateConsentFormFileSucceed(udtAuditLog)

        LogOpenConsentFormFile(udtCFInfo.FormType, udtCFInfo.Language, udtCFInfo.FormStyle, strFileName, ConfigurationManager.AppSettings("ReportSavingLink") + strFileName)

        Dim sb As New StringBuilder
        sb.Append("<script>")
        sb.Append("resizeMyselfConsentForm();")
        sb.Append("window.location = '" + ConfigurationManager.AppSettings("ReportSavingLink") + strFileName + "';")
        sb.Append("</script>")
        ClientScript.RegisterStartupScript(Me.GetType, "test2", sb.ToString())

    End Sub

    Private Function SelectReport(ByVal udtCFInfo As ConsentFormInformationModel) As ActiveReport3
        Dim strOption As String = String.Empty

        Select Case udtCFInfo.FormType
            Case ConsentFormInformationModel.FormTypeClass.HCVS
                strOption += "H"
            Case ConsentFormInformationModel.FormTypeClass.CIVSS
                strOption += "C"
            Case ConsentFormInformationModel.FormTypeClass.EVSS
                strOption += "E"
        End Select

        Select Case udtCFInfo.Language
            Case ConsentFormInformationModel.LanguageClassInternal.Chinese
                strOption += "C"
            Case ConsentFormInformationModel.LanguageClassInternal.English
                strOption += "E"
        End Select

        Select Case udtCFInfo.FormStyle
            Case ConsentFormInformationModel.FormStyleClass.Full
                strOption += "F"
            Case ConsentFormInformationModel.FormStyleClass.Condensed
                strOption += "C"
        End Select

        Select Case strOption

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------

            Case "HCF"
                'Return New PrintOut.VoucherConsentForm_CHI.VoucherConsentForm_CHI(udtCFInfo)
                Return LoadReportVersion(udtCFInfo, "VoucherConsentForm_CHI")
            Case "HCC"
                'Return New PrintOut.VoucherConsentForm_CHI.VoucherConsentCondensedForm_CHI(udtCFInfo)
                Return LoadReportVersion(udtCFInfo, "VoucherConsentCondensedForm_CHI")
            Case "HEF"
                'Return New PrintOut.VoucherConsentForm.VoucherConsentForm(udtCFInfo)
                Return LoadReportVersion(udtCFInfo, "VoucherConsentForm")
            Case "HEC"
                'Return New PrintOut.VoucherConsentForm.VoucherConsentCondensedForm(udtCFInfo)
                Return LoadReportVersion(udtCFInfo, "VoucherConsentCondensedForm")

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            Case "CCF"
                Return New PrintOut.CIVSSConsentForm_CHI.CIVSSConsentForm_CHI(udtCFInfo)
            Case "CCC"
                Return New PrintOut.CIVSSConsentForm_CHI.CIVSSConsentCondensedForm_CHI(udtCFInfo)
            Case "CEF"
                Return New PrintOut.CIVSSConsentForm.CIVSSConsentForm(udtCFInfo)
            Case "CEC"
                Return New PrintOut.CIVSSConsentForm.CIVSSConsentCondensedForm(udtCFInfo)
            Case "ECF"
                Return New PrintOut.EVSSConsentForm_CHI.EVSSConsentForm_CHI(udtCFInfo)
            Case "ECC"
                Return New PrintOut.EVSSConsentForm_CHI.EVSSConsentCondensedForm_CHI(udtCFInfo)
            Case "EEF"
                Return New PrintOut.EVSSConsentForm.EVSSConsentForm(udtCFInfo)
            Case "EEC"
                Return New PrintOut.EVSSConsentForm.EVSSConsentCondensedForm(udtCFInfo)
            Case Else
                Throw New Exception("Unknown report")

        End Select

        Return Nothing

    End Function

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Public Function LoadReportVersion(ByVal udtCFInfo As ConsentFormInformationModel, ByVal strReportName As String) As ActiveReport3
        Dim ass As Assembly
        Dim assType As Type
        Dim strFullName As String
        Dim db As New Common.DataAccess.Database(DBFlag.dbEVS_InterfaceLog)

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
        'Fix for getting too many DB datetime
        Dim dtmSystem As DateTime = (New Common.ComFunction.GeneralFunction).GetSystemDateTime()
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

        ass = [Assembly].GetExecutingAssembly
        For Each assType In ass.GetTypes
            If assType.IsClass Then
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
                'Fix for getting too many DB datetime
                If assType.FullName.EndsWith(VersionControlBLL.GetVersionControlListByLogicalName(strReportName, db, dtmSystem).PhysicalName) Then
                    'If assType.FullName.EndsWith(VersionControlBLL.GetVersionControlListByLogicalName(strReportName, db).PhysicalName) Then
                    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

                    strFullName = assType.FullName
                    Return Activator.CreateInstance(assType, New Object() {udtCFInfo})
                    Exit For
                End If
                End If
        Next
        Return Nothing
    End Function

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Private Function GenerateReportName(ByVal udtCFInfo As ConsentFormInformationModel) As String
        Dim strReportName As String = String.Empty

        ' Format: [FormType]_[Language][FormStyle]_[Timestamp]_[Serial].pdf
        ' e.g.: HCVS_EF_634183494552244210_0001.pdf

        strReportName += udtCFInfo.FormType

        strReportName += "_"

        strReportName += IIf(udtCFInfo.Language = ConsentFormInformationModel.LanguageClassInternal.Chinese, "C", "E")
        strReportName += IIf(udtCFInfo.FormStyle = ConsentFormInformationModel.FormStyleClass.Full, "F", "C")

        strReportName += "_"

        strReportName += Date.Now.Ticks.ToString

        strReportName += "_"

        strReportName += GetSerialNo()

        strReportName += ".pdf"

        Return strReportName

    End Function

    Private Function GetSerialNo() As String
        Return (New GeneralFunction).GetSystemProfile("CFSN", String.Empty, New ConsentFormEHS.Database(DBFlag.dbEVS_InterfaceLog)).ToString.PadLeft(4, "0")
    End Function

End Class