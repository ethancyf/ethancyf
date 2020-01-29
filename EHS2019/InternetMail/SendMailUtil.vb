
Public Class SendMailUtil

    Private Shared _sendMailUtil As SendMailUtil

#Region "Constructor"

    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As SendMailUtil
        If _sendMailUtil Is Nothing Then _sendMailUtil = New SendMailUtil()
        Return _sendMailUtil
    End Function

#End Region

    Protected Shared _SenderDomain1 As Boolean = True

    Private wsSender_1 As NotificationSender = Nothing
    Private wsSender_2 As NotificationSender = Nothing

    Protected ReadOnly Property WSNotificationSender_1() As NotificationSender
        Get
            If Me.wsSender_1 Is Nothing Then

                ' Init WebService
                Me.wsSender_1 = New NotificationSender()
                Me.wsSender_1.Url = WSSetting.GetURLNotificationSender_1()

                ' Init Call Back For Cert Prompt
                Dim callback As New System.Net.Security.RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
                System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

                ' Init Cert
                Dim store As New Security.Cryptography.X509Certificates.X509Store(Security.Cryptography.X509Certificates.StoreName.My, Security.Cryptography.X509Certificates.StoreLocation.LocalMachine)
                store.Open(Security.Cryptography.X509Certificates.OpenFlags.ReadOnly)

                Dim col As Security.Cryptography.X509Certificates.X509Certificate2Collection = store.Certificates.Find(Security.Cryptography.X509Certificates.X509FindType.FindByThumbprint, WSSetting.GetThumbprintNo.Trim().ToUpper(), False)


                If col.Count > 0 Then
                    MailLogger.LogLine("CertFind:" + col(0).Thumbprint.ToString())
                    Me.wsSender_1.ClientCertificates.Add(CType(col(0), Security.Cryptography.X509Certificates.X509Certificate))
                End If
            End If
            Return wsSender_1

        End Get
    End Property

    Protected ReadOnly Property WSNotificationSender_2() As NotificationSender
        Get
            If Me.wsSender_2 Is Nothing Then

                ' Init WebService
                Me.wsSender_2 = New NotificationSender()
                Me.wsSender_2.Url = WSSetting.GetURLNotificationSender_2()

                ' Init Call Back For Cert Prompt
                Dim callback As New System.Net.Security.RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
                System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

                ' Init Cert
                Dim store As New Security.Cryptography.X509Certificates.X509Store(Security.Cryptography.X509Certificates.StoreName.My, Security.Cryptography.X509Certificates.StoreLocation.LocalMachine)
                store.Open(Security.Cryptography.X509Certificates.OpenFlags.ReadOnly)

                Dim col As Security.Cryptography.X509Certificates.X509Certificate2Collection = store.Certificates.Find(Security.Cryptography.X509Certificates.X509FindType.FindByThumbprint, WSSetting.GetThumbprintNo.Trim().ToUpper(), False)

                If col.Count > 0 Then
                    MailLogger.LogLine("CertFind:" + col(0).Thumbprint.ToString())
                    Me.wsSender_2.ClientCertificates.Add(CType(col(0), Security.Cryptography.X509Certificates.X509Certificate))
                End If

                'For Each cert As Security.Cryptography.X509Certificates.X509Certificate In store.Certificates
                '    If cert.GetSerialNumberString.Trim() = WSSetting.GetCertSerialNo().Trim() Then
                '        Me.wsSender_2.ClientCertificates.Add(cert)
                '        'Me.wsSender.PreAuthenticate = True
                '        Exit For
                '    End If
                'Next

            End If

            Return wsSender_2

        End Get
    End Property

    ''' <summary>
    ''' Send the Email To EGIG Server, Failover implement in Code Level
    ''' 1: Domain 1, 2: Domain 2
    ''' </summary>
    ''' <param name="email"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SendMail(ByVal email As MailBuilder.Email) As Boolean
        ' URL (Example:https://xmlfw.egisdctr.hksarg:8444/sst/runtime.asvc/dh.dhhcv.app001-ogcio.egis.nt001-NotiSender-SslClient-Pub"

        Try
            Dim NotificationRequest As New NotificationSenderRequest()
            Dim request As NotificationSenderRequest = Me.ConstructNotificationRequest(email)

            Dim response As NotificationSenderResponse = Nothing
            Dim blnRetry As Boolean = False

            ' ---- Send the Email through WebService
            Try
                If _SenderDomain1 Then
                    response = Me.WSNotificationSender_1.CallNotificationSender(request)
                Else
                    response = Me.WSNotificationSender_2.CallNotificationSender(request)
                End If
            Catch soapEx As Web.Services.Protocols.SoapException
                blnRetry = True
                _SenderDomain1 = Not _SenderDomain1
                MailLogger.LogLine(soapEx.ToString())
                MailLogger.ErrorLog(soapEx)
            Catch ex As Exception
                blnRetry = True
                _SenderDomain1 = Not _SenderDomain1
                MailLogger.LogLine(ex.ToString())
                MailLogger.ErrorLog(ex)
            End Try

            ' ---- Retry
            If blnRetry Then
                MailLogger.LogLine("Retry: Domain1=" + _SenderDomain1.ToString())
                MailLogger.Log("FailOver", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<Domain1:" + _SenderDomain1.ToString() + ">")
                Try
                    If _SenderDomain1 Then
                        response = Me.WSNotificationSender_1.CallNotificationSender(request)
                    Else
                        response = Me.WSNotificationSender_2.CallNotificationSender(request)
                    End If
                Catch soapEx As Web.Services.Protocols.SoapException
                    blnRetry = True
                    _SenderDomain1 = Not _SenderDomain1
                    MailLogger.LogLine(soapEx.ToString())
                    MailLogger.ErrorLog(soapEx)
                Catch ex As Exception
                    blnRetry = True
                    _SenderDomain1 = Not _SenderDomain1
                    MailLogger.LogLine(ex.ToString())
                    MailLogger.ErrorLog(ex)
                End Try
            End If
            ' --- End of Retry

            If Not response Is Nothing Then
                MailLogger.LogLine("response.Status:" + response.Status)

                If response.Status = WSSetting.NotificationSenderResponse_Status.Success Then
                    MailLogger.Log("SendMail", Common.Component.ScheduleJobLogStatus.Success, "<Response.Status:" + response.Status + ">", "<MailAddress:" + email.EmailAddress + "><MailSubject:" + email.Subject + "><MailContent:" + email.Content + ">")
                    Return True
                Else
                    Dim strReturn As String = ""

                    For Each noticeStatus As NotiStatus In response.NotificationStatus
                        strReturn = strReturn + "<response.NotificationStatus:" + noticeStatus.ChanAddr + "," + noticeStatus.ResultCd + "," + noticeStatus.ResultMesg + ">"
                    Next
                    MailLogger.LogLine(strReturn)
                    MailLogger.Log("SendMail", Common.Component.ScheduleJobLogStatus.Fail, "<Response.Status:" + response.Status + ">" + strReturn, "<MailAddress:" + email.EmailAddress + "><MailSubject:" + email.Subject + "><MailContent:" + email.Content + ">")

                    Return False
                End If

                Return True
            End If

        Catch soapEx As Web.Services.Protocols.SoapException
            MailLogger.LogLine(soapEx.ToString())
            MailLogger.ErrorLog(soapEx)
            Return False
        Catch ex As Exception
            MailLogger.LogLine(ex.ToString())
            MailLogger.ErrorLog(ex)
            Return False
        End Try
    End Function

    Protected Function ConstructNotificationRequest(ByVal email As MailBuilder.Email) As NotificationSenderRequest
        Dim request As New NotificationSenderRequest()
        request.ChanType = email.ChannelType

        Dim recipient As New Recipient()
        recipient.ChanAddr = email.EmailAddress

        request.RecipientDetail = New Recipient() {recipient}

        Dim content As New Content()
        content.CharSet = email.CharacterSet
        content.ContentType = email.ContentType
        content.Subject = email.Subject
        content.Content1 = email.Content

        request.ContentDetail = content
        Return request

    End Function

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As Net.Security.SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Return True
    End Function
End Class
