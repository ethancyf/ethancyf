Public Class PPIEPRUtil

    Private Shared st_IPAddress As String = ""
    Private Shared st_HostName As String = ""

    Public Shared Function GetHostName() As String
        If st_HostName = "" Then
            Dim strHostName As String = System.Net.Dns.GetHostName()
            st_HostName = strHostName
        End If
        Return st_HostName
    End Function

    Public Shared Function GetIPAddress() As String
        If st_IPAddress = "" Then
            Dim strHostName As String = System.Net.Dns.GetHostName()
            Dim ipHostEntry As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(strHostName)
            Dim ipAddress() As System.Net.IPAddress = ipHostEntry.AddressList
            If ipAddress.Length > 0 Then
                st_IPAddress = ipAddress(0).ToString()
            End If
        End If
        Return st_IPAddress
    End Function

End Class
