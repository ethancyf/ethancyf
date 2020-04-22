Imports Common.Component
Imports Common.DataAccess
Imports CommonScheduleJob.Component.ScheduleJobSuspend
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO

Module Core

    Sub Main()
        ' CRE11-006
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

End Module

''' <summary>
''' CRE11-006
''' </summary>
''' <remarks></remarks>
Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

    ''' <summary>
    ''' CRE11-006
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return ""
        End Get
    End Property

    ''' <summary>
    ''' CRE11-006
    ''' Main process of schedule job
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Process()

        Dim strApplicationServer As String = System.Net.Dns.GetHostName
        Dim aryCachePath As ArrayList = GetCachePath()

        For Each dr As DataRow In GetClearCache(strApplicationServer).Rows
            If IsDBNull(dr("Cache_File")) Then
                ' Clear all
                For Each strCachePath As String In aryCachePath
                    For Each udtFI As FileInfo In (New DirectoryInfo(strCachePath)).GetFiles("*.txt")
                        File.Delete(udtFI.FullName)
                    Next
                Next

            Else
                Dim aryCacheFile As ArrayList = GetCacheFile(dr("Cache_File"))

                For Each strCachePath As String In aryCachePath
                    For Each udtFI As FileInfo In (New DirectoryInfo(strCachePath)).GetFiles("*.txt")
                        If aryCacheFile.Contains(udtFI.Name) Then
                            File.Delete(udtFI.FullName)
                        End If
                    Next
                Next

            End If

            ' Update row
            DeleteClearCache(dr("Request_ID").ToString.Trim)

        Next

    End Sub

    Private Function GetCachePath() As ArrayList
        Dim aryCachePath As New ArrayList

        For Each strCachePath As String In ConfigurationManager.AppSettings("CachePath").Split("|".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            strCachePath = strCachePath.Trim

            If Not strCachePath.EndsWith("\") Then strCachePath += "\"

            aryCachePath.Add(strCachePath)
        Next

        Return aryCachePath

    End Function

    Private Function GetCacheFile(ByVal strCacheFile As String) As ArrayList
        Dim aryCacheFile As New ArrayList

        For Each strNode As String In strCacheFile.Trim.Split("|".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            aryCacheFile.Add(strNode.Trim + ".txt")
        Next

        Return aryCacheFile

    End Function


    Private Function GetClearCache(Optional ByVal strApplicationServer As String = Nothing) As DataTable
        Dim udtDB As New Database
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Application_Server", SqlDbType.VarChar, 20, IIf(IsNothing(strApplicationServer), DBNull.Value, strApplicationServer)) _
        }

        udtDB.RunProc("proc_ClearCache_Get", prams, dt)

        Return dt

    End Function

    Private Sub DeleteClearCache(ByVal strRequestID As String)
        Dim udtDB As New Database

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Request_ID", SqlDbType.VarChar, 8, strRequestID) _
        }

        udtDB.RunProc("proc_ClearCache_Delete_ByRequestID", prams)

    End Sub

End Class
