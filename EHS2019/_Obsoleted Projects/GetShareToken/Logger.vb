Imports Common.ComObject
Imports Common.Component

Public Class Logger

#Region "Private Class"

    Public Enum EnumLogAction
        Start
        [End]
        Initialization
        ProcessQueue
        Finalizer
    End Enum

    Public Enum EnumLogStatus
        Information
        Success
        Fail
    End Enum

#End Region

    ''' <summary>
    ''' Log to Console only
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <remarks></remarks>
    Public Shared Sub Log(ByVal strText As String)
        Console.WriteLine(String.Format("<{0}> {1}", Now.ToString("yyyy-MM-dd HH:mm:ss"), strText))
    End Sub

    ''' <summary>
    ''' Log to Console and Database
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <param name="eLogAction"></param>
    ''' <param name="eLogStatus"></param>
    ''' <remarks></remarks>
    Public Shared Sub Log(ByVal strText As String, ByVal eLogAction As EnumLogAction, ByVal eLogStatus As EnumLogStatus)
        Log(strText)

        ScheduleJobLogEntry.WriteLog(DateTime.Now, GetIPAddress(), ScheduleJobID.TokenReplacement, eLogAction.ToString, eLogStatus.ToString, Nothing, strText)

    End Sub

    '

    Public Shared Function GetIPAddress() As String
        Dim strIPAddress As String = String.Empty

        Dim ipAddress() As System.Net.IPAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList

        If ipAddress.Length > 0 Then
            strIPAddress = ipAddress(0).ToString
        End If

        Return strIPAddress

    End Function

End Class
