Imports System.Data.SqlClient
Imports System.IO
Imports System.Threading

Namespace ComObject
    Public Class CacheHandler
        Private Shared alCache As ArrayList

        Public Shared Sub InsertCache(ByVal key As String, ByVal value As Object, ByRef dependency As SqlCacheDependency)
            HttpContext.Current.Cache.Insert(key, value, dependency)
        End Sub

        Public Shared Function InsertCache(ByVal strKey As String, ByVal oValue As Object) As Boolean

            If alCache Is Nothing Then
                alCache = New ArrayList
            End If

            Dim strFilePath As String '= HttpContext.Current.Server.MapPath("./").ToLower()
            strFilePath = ConfigurationManager.AppSettings("CacheFile")
            strFilePath += strKey.Trim + ".txt"
            'Insert cache
            Dim sw As TextWriter
            Dim safesw As TextWriter = Nothing
            Dim index As Integer
            Try
                index = alCache.IndexOf(strKey)
                If (index >= 0) Then
                    If (Not Monitor.TryEnter(alCache.Item(index), 5000)) Then
                        'ErrorHandler.Log("Cache lock request time out - InsertCache(), index >= 0")
                        Return False
                    End If
                Else
                    Dim tempString As New String(strKey.ToCharArray())
                    index = alCache.Add(tempString)
                    If (Not Monitor.TryEnter(alCache.Item(index), 5000)) Then
                        'ErrorHandler.Log("Cache lock request time out - InsertCache(), index < 0")
                        Return False
                    End If
                End If
                'Change file
                If (Not File.Exists(strFilePath)) Then
                    sw = New StreamWriter(strFilePath)
                    safesw = TextWriter.Synchronized(sw)
                    safesw.WriteLine(DateTime.Now)
                    If (Not safesw Is Nothing) Then
                        safesw.Close()
                        safesw = Nothing
                    End If
                    If (Not sw Is Nothing) Then
                        sw.Close()
                        sw = Nothing
                    End If
                End If
                Dim dependency As New CacheDependency(strFilePath)
                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                ' CHange HttpContext to HttpRuntime for compatiable with console apps too
                HttpRuntime.Cache.Insert(strKey, oValue, dependency)
                ' CRE12-001 eHS and PCD integration [End][Koala]
            Catch ex As Exception
                'ErrorHandler.Log(ex)
                Throw ex
            Finally
                Monitor.Exit(alCache.Item(index))
                If (Not safesw Is Nothing) Then
                    safesw.Close()
                    safesw = Nothing
                End If
                If (Not sw Is Nothing) Then
                    sw.Close()
                    sw = Nothing
                End If
            End Try
        End Function

        Public Shared Sub RemoveCache(ByVal key As String)
            'HttpContext.Current.Cache.Remove(key)
        End Sub

    End Class
End Namespace

