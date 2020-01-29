Imports System.Data.SqlClient
Imports System.IO
Imports System.Threading

Public Class CacheHandler

    Private Shared aryCache As ArrayList

    Public Shared Sub InsertCache(ByVal key As String, ByVal value As Object, ByRef dependency As SqlCacheDependency)
        HttpContext.Current.Cache.Insert(key, value, dependency)
    End Sub

    Public Shared Function InsertCache(ByVal strKey As String, ByVal objValue As Object) As Boolean
        If aryCache Is Nothing Then aryCache = New ArrayList

        Dim strFilePath As String = ConfigurationManager.AppSettings("CacheFile").Trim
        If Not strFilePath.EndsWith("/") Then strFilePath += "/"
        strFilePath += strKey.Trim + ".txt"

        ' Insert cache
        Dim sw As TextWriter = Nothing
        Dim safesw As TextWriter = Nothing
        Dim intIndex As Integer = 0

        Try
            intIndex = aryCache.IndexOf(strKey)

            If intIndex >= 0 Then
                If Not Monitor.TryEnter(aryCache.Item(intIndex), 5000) Then
                    Return False
                End If

            Else
                intIndex = aryCache.Add(New String(strKey.ToCharArray))
                If Not Monitor.TryEnter(aryCache.Item(intIndex), 5000) Then
                    Return False
                End If

            End If

            ' Change file
            If Not File.Exists(strFilePath) Then
                sw = New StreamWriter(strFilePath)
                safesw = TextWriter.Synchronized(sw)
                safesw.WriteLine(DateTime.Now)

                If Not IsNothing(safesw) Then
                    safesw.Close()
                    safesw = Nothing
                End If

                If Not IsNothing(sw) Then
                    sw.Close()
                    sw = Nothing
                End If

            End If

            Dim dependency As New CacheDependency(strFilePath)

            HttpContext.Current.Cache.Insert(strKey, objValue, dependency)

        Catch ex As Exception
            Throw ex

        Finally
            Monitor.Exit(aryCache.Item(intIndex))

            If Not IsNothing(safesw) Then
                safesw.Close()
                safesw = Nothing
            End If

            If Not IsNothing(sw) Then
                sw.Close()
                sw = Nothing
            End If

        End Try

    End Function

End Class

