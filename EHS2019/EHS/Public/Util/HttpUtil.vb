Imports System.Net
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security

Public Class HttpUtil
    Public Shared Function GetContent(ByVal uri As String, ByVal coding As Encoding) As String

        System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        Dim request As WebRequest = WebRequest.Create(uri)
        Dim resp As WebResponse = request.GetResponse()
        Dim stream As Stream = resp.GetResponseStream()
        Dim sr As StreamReader = New StreamReader(stream, coding)
        Dim result As String = sr.ReadToEnd()
        stream.Close()
        sr.Close()
        Return result
    End Function

    Public Shared Function GetMP3(ByVal uri As String) As MemoryStream

        System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        Dim request As WebRequest = WebRequest.Create(uri)
        Dim resp As WebResponse = request.GetResponse()
        Dim connectStream As Stream = resp.GetResponseStream()
        Dim ioStream As MemoryStream = New MemoryStream()
        connectStream.CopyTo(ioStream)
        ioStream.Position = 0
        Return ioStream
    End Function

    Private Shared Function ValidateRemoteCertificate(sender As Object, certification As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function

End Class
