Imports System.Web.Mvc
Imports Newtonsoft.Json
Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.Component

Namespace Controllers
    <Localization>
    Public Class ValueController
        Inherits BaseController

        ' GET: Value
        <HttpGet>
        <ActionName("systemmsg")>
        Function GetSystemMessageList() As String
            Dim strLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower

            Dim dtSystemMessage As DataTable
            If XMLMain.DBLink Then
                dtSystemMessage = GetSystemMessageDT(strLang)
            Else
                dtSystemMessage = XMLMain.GetSystemMessageXML(strLang)
            End If

            Dim lstSystemMessage = New List(Of SystemMessageModel)
            For Each Item As DataRow In dtSystemMessage.Rows
                lstSystemMessage.Add(New SystemMessageModel With {
                                     .code = CheckNull(Item("resourceKey")),
                                     .desc = CheckNull(Item("resourceValue"))
                                     })
            Next
            Return JsonConvert.SerializeObject(lstSystemMessage)
        End Function

        Private Function GetSystemMessageDT(strLang As String) As DataTable
            Dim udtDB As Database = New Database()
            Dim dtSystemMessage = New DataTable()
            Dim platform As String = ConfigurationManager.AppSettings("Platform")
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@platform", SqlDbType.VarChar, 20, platform)}
            Select Case strLang.ToLower()
                Case CultureLanguage.TradChinese
                    udtDB.RunProc("proc_SystemMessage_get_cache_zh_tw", prams, dtSystemMessage)
                Case Else
                    udtDB.RunProc("proc_SystemMessage_get_cache", prams, dtSystemMessage)
            End Select
            Return dtSystemMessage.Copy
        End Function

        ' Check Object If DbNull or String Null Return Empty
        Private Function CheckNull(ByVal str As Object) As String
            If IsDBNull(str) Then
                Return ""
            End If
            If (String.IsNullOrEmpty(str)) Then
                Return ""
            End If
            Return str.ToString().Trim
        End Function
    End Class
End Namespace