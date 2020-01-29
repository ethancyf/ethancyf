Module Module1

    Sub Main()

        MailMgr.GetInstance().ProcessEMails()

        ' Testing
        'TestingHttpRequest()
        'TestingWSEmail()
        'While True
        'End While

    End Sub

    Private Sub TestingHttpRequest()
        Try

            Dim httpRequest As System.Net.WebRequest = System.Net.WebRequest.Create("https://xmlfw.egisdctr.hksarg:8443/")
            httpRequest.Method = "POST"
            httpRequest.Proxy = Nothing


            Dim callback As New System.Net.Security.RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
            System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

            ' Request Stream
            Dim stream As System.IO.Stream = httpRequest.GetRequestStream()
            stream.Close()

            ' Get Response
            Dim httpResponse As System.Net.HttpWebResponse = httpRequest.GetResponse()
            stream = httpResponse.GetResponseStream()
            Dim streamReader As New System.IO.StreamReader(stream)

            MailLogger.LogLine(streamReader.ReadToEnd())
            streamReader.Close()

        Catch ex As Exception
            MailLogger.LogLine(ex.ToString())
        End Try
    End Sub

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As Net.Security.SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Return True
    End Function

    Private Sub TestingWSEmail()

        WSSetting.InitParameters()

        Dim email As MailBuilder.Email = ConstructTestingEmail()
        MailLogger.LogLine(email.ToString())

        Try
            Dim blnSuccess As Boolean = SendMailUtil.GetInstance().SendMail(email)
            If blnSuccess Then
                MailLogger.LogLine("MailSent Success")
            Else
                MailLogger.LogLine("MailSent Fail")
            End If
        Catch ex As Exception
            MailLogger.LogLine(ex.ToString())
        End Try
        
    End Sub

    Private Function ConstructTestingEmail() As MailBuilder.Email
        Dim email As New MailBuilder.Email()

        'email.EmailAddress = ""
        'email.Subject = "Testing Email"
        'email.Content = "Testing Content: " & DateTime.Now.ToString()

        Return email

    End Function

End Module
