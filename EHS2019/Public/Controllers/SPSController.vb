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

        Public Function PopUpDetail(JsonDetail As String) As ActionResult
            Dim strLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower
            Dim vm As SPSViewModel = spBLL.PopUpDetail(JsonDetail, strLang)

            Return PartialView("PracticeDetail", vm)
        End Function

        Public Function OrderQuery(selectedScheme As String) As ActionResult
            Dim strLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower
            Dim queryList As List(Of AreaList) = spBLL.OrderQuery(strLang)
            Return PartialView("OrderQuery", queryList)

        End Function
    End Class
End Namespace

