Imports System.Web.Mvc
Imports [Public].LanguageCollection
Imports System.Globalization
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Format

Namespace Controllers
    <Localization>
    Public Class VBEController
        Inherits BaseController

        Private vbeBLL As VBEBLL = New VBEBLL()

        Private CaptchaDuration As Integer = ConfigurationManager.AppSettings("CaptchaDuration")
        'Private CaptchaFasterCheck As Integer = 2
        'Private CaptchaLength As Integer = 5
        Private CaptchaFasterCheck As Integer = ConfigurationManager.AppSettings("CaptchaFasterCheck")
        Private CaptchaLength As Integer = ConfigurationManager.AppSettings("CaptchaLength")
        ReadOnly _PageTimeout As Integer = ConfigurationManager.AppSettings("PageTimeout")

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
            Return View("Search", validateResult)
        End Function

        Function Search(validateResult As VBEValidateResult) As ActionResult
            validateResult = CType(TempData("VBEValidateResult"), VBEValidateResult)
            ViewBag.PageTimeout = _PageTimeout
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
            Return View(udtVBEResult)
        End Function

        <AllowAnonymous>
        Function GetValidateCode() As ActionResult
            Dim code As String = ValidateCode.CreateValidateCode(CaptchaLength)
            Dim vCode As ValidateCode = New ValidateCode(code)
            Session("ValidateCode") = code
            Session("ValidateTime") = DateTime.Now
            Dim bytes As Byte() = vCode.Generate() 'CreateValidateGraphic(code)
            GetValidateCode = File(bytes, "image/jpeg")
        End Function

        <HttpPost>
        Function VerifyCaptcha(code As String, submitTime As DateTime) As JsonResult
            Dim sessionCaptcha As String = ""
            Dim generateTime As DateTime
            submitTime = DateTime.Now
            generateTime = Session("ValidateTime")
            Dim rtn As Integer = 0
            If Session("ValidateCode") <> Nothing Then
                sessionCaptcha = Session("ValidateCode")
            End If
            'Input is not correct, return value = 1
            If sessionCaptcha <> code Then
                rtn = 1
            End If
            'The code is expired, return value = 2
            If submitTime > generateTime.AddSeconds(CaptchaDuration) Then
                rtn = 2
            End If
            'The code is expired, return value = 3
            If generateTime.AddSeconds(CaptchaFasterCheck) > submitTime Then
                rtn = 3
            End If
            Dim obj As Object = New With {.Rtn = rtn}
            VerifyCaptcha = Json(obj, JsonRequestBehavior.AllowGet)
        End Function

    End Class
End Namespace