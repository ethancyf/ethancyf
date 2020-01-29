Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security







Public Class ASPHTTP

    Private _URL As String = String.Empty
    Private _FormField() As FormField = Nothing
    Private _UploadFile As String = String.Empty
    Private _UploadFileField As String = String.Empty

    Public Sub New()
    End Sub

    Public Property URL() As String
        Get
            Return _URL
        End Get
        Set(ByVal value As String)
            _URL = value
        End Set
    End Property

    Public Property FormField() As FormField()
        Get
            Return _FormField
        End Get
        Set(ByVal value() As FormField)
            _FormField = value
        End Set
    End Property

    Public Property UploadFile() As String
        Get
            Return _UploadFile
        End Get
        Set(ByVal value As String)
            _UploadFile = value
        End Set
    End Property

    Public Property UploadFileField() As String
        Get
            Return _UploadFileField
        End Get
        Set(ByVal value As String)
            _UploadFileField = value
        End Set
    End Property

    Public Function getResult() As String
        Dim resp As HttpWebResponse = Nothing
        Try
            Dim req As HttpWebRequest = CType(WebRequest.Create(Me.URL), HttpWebRequest)

            req.Credentials = CredentialCache.DefaultCredentials
            Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
            System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

            req.Method = "POST"
            'Dim boundary As String = Guid.NewGuid().ToString().Replace("-", "")
            'req.ContentType = "multipart/form-data; boundary=" + boundary
            req.ContentType = "multipart/form-data;"

            Dim postData As MemoryStream = New MemoryStream
            'Dim newLine As String = "\r\n"
            Dim sw As StreamWriter = New StreamWriter(postData)

            Dim ff As FormField

            If Not Me.FormField Is Nothing Then
                For Each ff In Me.FormField
                    'sw.Write("--" + boundary + newLine)
                    'sw.Write("Content-Disposition: form-data; name=""{0}""{1}{1}{2}{1}", ff.fieldName, newLine, ff.fieldValue)
                    sw.Write("{0}={1}&", ff.fieldName, ff.fieldValue)
                Next
            End If

            'If Not Me.UploadFile Is String.Empty Then
            '    sw.Write("--" + boundary + newLine)
            '    sw.Write("Content-Disposition: form-data; name=""{0}""; filename=""{1}""{2}", Me.UploadFileField, Me.UploadFile, newLine)
            '    sw.Write("Content-Type: application/octet-stream" + newLine + newLine)
            '    sw.Flush()

            '    Dim inFile As FileStream = New FileStream(Me.UploadFile, FileMode.Open)
            '    Using inFile
            '        Dim inByte(inFile.Length) As Byte
            '        inFile.Read(inByte, 0, inFile.Length)
            '        postData.Write(inByte, 0, inByte.Length)
            '    End Using

            '    sw.Write(newLine)
            'End If

            'sw.Write("--{0}--{1}", boundary, newLine)
            sw.Flush()
            req.ContentLength = postData.Length

            'Dim s As Stream = req.GetRequestStream
            'Using s
            '    postData.WriteTo(s)
            'End Using

            postData.WriteTo(req.GetRequestStream)
            postData.Close()

            'Dim resp As HttpWebResponse = CType(req.GetResponse, HttpWebResponse)
            resp = CType(req.GetResponse, HttpWebResponse)


            'Dim count As Integer = 0
            'Do Until Not a = resp.GetResponseStream.Length
            '    count = count + 1
            'Loop

            Dim receiveStream As Stream = resp.GetResponseStream
            Dim readstream As StreamReader = New StreamReader(receiveStream, Encoding.UTF8)

            Dim result As String = ""
            'Do While readstream.Peek >= 0
            '    result = result + Convert.ToChar(readstream.Read)
            'Loop
            result = readstream.ReadToEnd

            'Dim result As String = readstream.ReadToEnd
            resp.Close()
            readstream.Close()
            Return result
        Catch we As System.Net.WebException
            Throw New System.Exception( _
                "Unable to execute URL.", _
                we)
        Finally
            If (Not IsNothing(resp)) Then
                resp.Close()
            End If
        End Try

        Return ""
    End Function

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Return True
    End Function

End Class


Public Class FormField
    Private _fieldName As String
    Private _fieldValue As String

    Public Sub New(ByVal fieldName As String, ByVal fieldValue As String)
        _fieldName = fieldName
        _fieldValue = fieldValue
    End Sub

    Public Property fieldName() As String
        Get
            Return _fieldName
        End Get
        Set(ByVal value As String)
            _fieldName = value
        End Set
    End Property

    Public Property fieldValue() As String
        Get
            Return _fieldValue
        End Get
        Set(ByVal value As String)
            _fieldValue = value
        End Set
    End Property
End Class
