Imports Common.Format
Imports System.Web.Mvc
Imports System.Xml
Imports System.Web.Helpers
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json
Imports Common.ComFunction

Namespace Controllers
    <Localization>
    Public Class SPSController
        Inherits BaseController

        Private spBLL As SPBLL = New SPBLL()

        ' GET: Search
        <HttpGet>
        Function Search() As ActionResult
            Dim strLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower
            Dim vm As SPSViewModel = New SPSViewModel()
            Dim codeList = spBLL.GetCodeList(vm, strLang)
            spBLL.WritePageLoadAuditLog()
            Return View(codeList)
        End Function

        'Change Language
        <HttpPost>
        Function Search(form As FormCollection) As ActionResult
            Dim strLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower
            Dim vw = spBLL.ServiceProviderSearch(form, strLang)
            Return View(vw)
        End Function

        Public Function GetResult(dataModel As String) As ActionResult
            Dim strLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower
            Dim strQueryLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower

            Dim validateResult As SPSValidateResult = Nothing

            ' Encapsulate Request Data
            Dim spRequest As SPRequest = spBLL.EncapsulateSPSRequest(dataModel, strQueryLang)

            Dim vm As SPSViewModel = New SPSViewModel()
            ' Request Data Validate and Get Result
            validateResult = spBLL.SPSRequestValidate(spRequest, strLang, strQueryLang, vm)

            'Last update date
            Dim lastDate As DateTime = DateTime.Now
            If XMLMain.DBLink Then
                lastDate = spBLL.GetLastUpdate
            Else
                lastDate = DateTime.Now
            End If

            vm.LastUpdateDate = String.Format("{0}", Resource.Text("UpdateDate").Replace("%s", ": " & (New Formatter).formatDisplayDate(spBLL.GetLastUpdate, strLang)))

            If Not validateResult.returnValue And validateResult.lstErrCodes.Count > 0 Then
                vm.lstErrorCodes.AddRange(validateResult.lstErrCodes)
                vm.IsValid = False
            Else
                vm.IsValid = True
            End If

            Return PartialView("ResultList", vm)
        End Function

        Public Function GetPageSizeList(hasvss As Boolean) As JsonResult
            Dim strPageSize As String = "10;20;30;40;50"
            If XMLMain.DBLink Then
                If hasvss Then
                    strPageSize = (New GeneralFunction).getSystemParameterValue1("SDIR_ddlResultPerPage_VSS")
                Else
                    strPageSize = (New GeneralFunction).getSystemParameterValue1("SDIR_ddlResultPerPage_HCVU")
                End If
            End If
            Return Json(strPageSize.Split(";"))

        End Function

        ' INT20-0026 (Fix full width issue on public site) [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function PopUpDetail(JsonDetailHex As String) As ActionResult
            'Public Function PopUpDetail(JsonDetail As String) As ActionResult
            ' INT20-0026 (Fix full width issue on public site) [End][Koala]
            Dim strLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower

            ' INT20-0026 (Fix full width issue on public site) [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Dim JsonDetail As String = FromHexString(JsonDetailHex)
            ' INT20-0026 (Fix full width issue on public site) [End][Koala]
            Dim vm As SPSViewModel = spBLL.PopUpDetail(JsonDetail, strLang)

            spBLL.WirtePracticeDetailPopupAuditLog()

            Return PartialView("PracticeDetail", vm)
        End Function

        Private Function FromHexString(ByVal hexString As String) As String
            Dim bytes As Byte() = New Byte(hexString.Length / 2 - 1) {}
            For i = 0 To bytes.Length - 1
                bytes(i) = Convert.ToByte(hexString.Substring(i * 2, 2), 16)
            Next
            Return Encoding.BigEndianUnicode.GetString(bytes)
        End Function

        Public Function OrderQuery(selectedScheme As String) As ActionResult
            Dim strLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower
            Dim queryList As List(Of AreaList) = spBLL.OrderQuery(strLang)
            Return PartialView("OrderQuery", queryList)

        End Function

        <HttpPost>
        Function ErrorLog(errMsg As String) As JsonResult
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
                spBLL.WirteClientValidationFailAuditLog(lstErrMsg)
            End If

            ErrorLog = Json(obj, JsonRequestBehavior.AllowGet)
        End Function
    End Class
End Namespace

