Imports System.Web
Imports System.Web.Mvc
Imports [Public].LanguageCollection
Imports System.Globalization
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Format
Imports System.Threading.Tasks
Imports System.IO
Imports System.Speech.Synthesis

Namespace Controllers
    <Localization>
    Public Class VBEController
        Inherits BaseController

        Private vbeBLL As VBEBLL = New VBEBLL()

        Private CaptchaDuration As Integer = ConfigurationManager.AppSettings("CaptchaDuration")
        Private CaptchaFasterCheck As Integer = ConfigurationManager.AppSettings("CaptchaFasterCheck")
        Private CaptchaLength As Integer = ConfigurationManager.AppSettings("CaptchaLength")
        ReadOnly _PageTimeout As Integer = ConfigurationManager.AppSettings("PageTimeout")
        ReadOnly _CaptchaAudioFolderForWeb As String = ConfigurationManager.AppSettings("CaptchaAudioFolderForWeb")
        Private FunctionCode As String = FunctCode.FUNT030101

        ' GET: VBE
        Function Index() As ActionResult
            Return View()
        End Function

        Function Guide() As ActionResult
            Return View()
        End Function

        <HttpPost>
        Function Search() As ActionResult
            Dim validateResult As VBEValidateResult = New VBEValidateResult()
            Dim requestData As VBERequest = vbeBLL.EncapsulateVBERequest(Request)
            validateResult.VBERequestData = requestData
            ViewBag.PageTimeout = _PageTimeout
            ViewBag.CaptchaAudioFolder = _CaptchaAudioFolderForWeb
            Return View("Search", validateResult)
        End Function

        Function Search(validateResult As VBEValidateResult) As ActionResult
            vbeBLL.WritePageLoadAuditLog()
            Me.WakeUpAudioCaptcha()
            validateResult = CType(TempData("VBEValidateResult"), VBEValidateResult)
            ViewBag.PageTimeout = _PageTimeout
            ViewBag.CaptchaAudioFolder = _CaptchaAudioFolderForWeb
            Return View(validateResult)
        End Function

        <HttpPost>
        Function Result(form As FormCollection) As ActionResult
            Dim requestData As VBERequest = vbeBLL.EncapsulateVBERequest(Request)
            Dim validateResult As VBEValidateResult = Nothing
            Dim udtVBEResult As VBEResult = Nothing

            'validateResult = vbeBLL.VBERequestValidate(requestData, Session("ValidateCode").ToString(), Convert.ToDateTime(Session("ValidateTime")))
            validateResult = vbeBLL.VBERequestValidate(requestData, Session("ValidateCode").ToString(), Convert.ToDateTime(Session("ValidateTime")), udtVBEResult)

            'Dim validateResult As VBEValidateResult = New VBEValidateResult()
            '*********************IMPORTANTANCE**********************************************
            'It is for Debug, please remove below line when the backend validation is finished.
            'If you want to test for nopass of backend logic, please remove the code "validateResult.returnValue = True"
            'validateResult.returnValue = True
            '*********************IMPORTANTANCE**********************************************

            If Not validateResult.returnValue Then
                TempData("VBEValidateResult") = validateResult
                Return RedirectToAction("Search", "VBE", validateResult)
            End If

            udtVBEResult.VBERequestData = requestData
            ViewBag.PageTimeout = _PageTimeout
            ViewBag.CaptchaAudioFolder = _CaptchaAudioFolderForWeb
            Return View(udtVBEResult)
        End Function

        Function Result() As ActionResult
            Dim strCulture As String = Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower
            Dim strLang As String = "en"

            If strCulture <> Common.Component.CultureLanguage.English Then
                strLang = "tc"
            End If

            Return New RedirectResult("~\" & strLang & "\VBE\Search\")
        End Function

        <AllowAnonymous>
        Function GetValidateCode(Speech As String) As ActionResult
            Dim code As String = ValidateCode.CreateValidateCode(CaptchaLength)

            Dim blnEnableCaptcha As Boolean = IIf(ConfigurationManager.AppSettings("EnableCaptcha").ToString() = YesNo.Yes, True, False)

            If Not blnEnableCaptcha Then
                code = "00000"
            End If

            Dim vCode As ValidateCode = New ValidateCode(code)
            Session("ValidateCode") = code
            Session("ValidateTime") = DateTime.Now
            Dim bytes As Byte() = vCode.Generate() 'CreateValidateGraphic(code)
            GetValidateCode = File(bytes, "image/jpeg")
        End Function

        <HttpPost>
        Function GenerateAudioCaptcha(audioId As String) As JsonResult
            Dim rtn As Integer = 0
            Dim code As String = Session("ValidateCode")
            Try
                Dim CaptchaSpeechLocation As String = ConfigurationManager.AppSettings("CaptchaSpeechLocation").ToString()
                If CaptchaSpeechLocation.ToUpper() = "LOCAL" Then
                    Dim t As Task = Task.Factory.StartNew(Sub()
                                                                 ValidateCode.GenerateSpeech(audioId, code)
                                                             End Sub)
                    t.Wait(200)
                Else
                    Dim speechUrl As String = ConfigurationManager.AppSettings("GenCaptchSpeechUrl").ToString()
                    Dim queryparm As String = "?speech={0}&captcha={1}&language={2}"
                    queryparm = String.Format(queryparm, audioId, code, Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower())
                    HttpUtil.GetContent(speechUrl + queryparm, System.Text.Encoding.UTF8)
                End If
                Threading.Thread.Sleep(200)
            Catch ex As Exception
                rtn = 1
            End Try

            Dim obj As Object = New With {.Rtn = rtn}
            GenerateAudioCaptcha = Json(obj, JsonRequestBehavior.AllowGet)
        End Function

        Private Sub WakeUpAudioCaptcha()
            Try
                Dim CaptchaSpeechLocation As String = ConfigurationManager.AppSettings("CaptchaSpeechLocation").ToString()

                If CaptchaSpeechLocation.ToUpper() = "LOCAL" Then
                    'Nothing to do
                Else
                    Dim speechUrl As String = ConfigurationManager.AppSettings("GenCaptchSpeechStreamUrl").ToString()
                    HttpUtil.GetContent(speechUrl + "WakeUp", System.Text.Encoding.UTF8)
                End If
                Threading.Thread.Sleep(200)
            Catch ex As Exception
                Throw
            End Try

        End Sub

        Public Function GetCaptchaStream(audioId As String) As ActionResult
            Dim code As String = Session("ValidateCode")
            Try
                Dim CaptchaSpeechLocation As String = ConfigurationManager.AppSettings("CaptchaSpeechLocation").ToString()
                If CaptchaSpeechLocation.ToUpper() = "LOCAL" Then
                    Dim t As Task = Task.Factory.StartNew(Sub()
                                                              ValidateCode.GenerateSpeech(audioId, code)
                                                          End Sub)
                    t.Wait(200)
                Else
                    Dim speechUrl As String = ConfigurationManager.AppSettings("GenCaptchSpeechStreamUrl").ToString()
                    Dim CaptchaDuration As String = ConfigurationManager.AppSettings("CaptchaDuration").ToString()
                    Dim queryparm As String = "?speech={0}&captcha={1}&language={2}"
                    queryparm = String.Format(queryparm, audioId, code, Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower())
                    Return File(HttpUtil.GetMP3(speechUrl + queryparm), "audio/mp3")

                End If
            Catch ex As Exception
                'LogHelper.WriteLineToday(ex.Message + "Source:" + ex.Source + "StackTrace:" + ex.StackTrace)
                Throw
            End Try
        End Function

        Private Function StreamToBytes(ByVal stream As Stream) As Byte()
            Dim bytes As Byte() = New Byte(stream.Length - 1) {}
            stream.Read(bytes, 0, bytes.Length)
            stream.Seek(0, SeekOrigin.Begin)
            Return bytes
        End Function

        <HttpPost>
        Function VerifyCaptcha(code As String, submitTime As DateTime) As JsonResult
            Dim sessionCaptcha As String = ""
            Dim generateTime As DateTime
            submitTime = DateTime.Now
            generateTime = Session("ValidateTime")
            Dim rtn As Integer = 0
            Dim rtnDesc As String = "Matched"

            If Session("ValidateCode") <> Nothing Then
                sessionCaptcha = Session("ValidateCode")
            End If

            'Input is not correct, return value = 1
            If sessionCaptcha <> code Then
                rtn = 1
                rtnDesc = "Not Matched"
            End If

            'The code is expired, return value = 2
            If submitTime > generateTime.AddSeconds(CaptchaDuration) Then
                rtn = 2
                rtnDesc = "Expired"
            End If

            'The code is expired, return value = 3
            If generateTime.AddSeconds(CaptchaFasterCheck) > submitTime Then
                rtn = 3
                rtnDesc = String.Format("Input too fast (<{0}s)", CaptchaFasterCheck)
            End If

            vbeBLL.WirteClientVerifyCaptchaAuditLog(Session("ValidateCode"), code, rtnDesc)

            Dim obj As Object = New With {.Rtn = rtn}
            VerifyCaptcha = Json(obj, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost>
        Function ErrorLog(HKID As String, _
                            DocCode As Integer, _
                            DOB As String, _
                            ECAge As String, _
                            DOR_Year As String, _
                            DOR_Month As String, _
                            DOR_Day As String, _
                            errMsg As String) As JsonResult

            Dim strHKID As String = HKID
            Dim strDocCode As String = IIf(DocCode = 1, DocType.DocTypeModel.DocTypeCode.HKIC, DocType.DocTypeModel.DocTypeCode.EC)
            Dim strDOB As String = DOB
            Dim strECAge As String = ECAge
            Dim strDOR_Year As String = DOR_Year
            Dim strDOR_Month As String = DOR_Month
            Dim strDOR_Day As String = DOR_Day

            Dim strErrorMsg As String = errMsg
            Dim obj As Object = New With {.Rtn = 0}

            Dim lstErrMsg As List(Of String) = New List(Of String)

            If errMsg.Length > 0 Then
                Dim arrErrMsg() As String = Split(Mid(errMsg, 1, Len(errMsg) - Len("<br>")), "<br>")

                For i As Integer = 0 To arrErrMsg.Length - 1
                    lstErrMsg.Add(arrErrMsg(i))
                Next
            End If

            If lstErrMsg.Count > 0 Then
                vbeBLL.WirteClientValidationFailAuditLog(strHKID, strDocCode, strDOB, strECAge, strDOR_Year, strDOR_Month, strDOR_Day, lstErrMsg)
            End If

            ErrorLog = Json(obj, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost>
        Function RefreshCaptchaLog() As JsonResult

            Dim obj As Object = New With {.Rtn = 0}

            vbeBLL.WirteClientRefreshCaptchaAuditLog(Session("ValidateCode"))

            RefreshCaptchaLog = Json(obj, JsonRequestBehavior.AllowGet)
        End Function

        <HttpPost>
        Function PlayAudioCaptchaLog() As JsonResult

            Dim obj As Object = New With {.Rtn = 0}

            vbeBLL.WirteClientPlayAudioCaptchaAuditLog(Session("ValidateCode"))

            PlayAudioCaptchaLog = Json(obj, JsonRequestBehavior.AllowGet)
        End Function

    End Class
End Namespace