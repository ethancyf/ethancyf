Public Class WSSetting

    Private Shared _strDomain_1 As String
    Private Shared _strDomain_2 As String
    Private Shared _strWSNotificationSender As String
    Private Shared _strWSNotificationAction As String
    Private Shared _strCertSerialNo As String
    Private Shared _strThumbprint As String

    Public Shared Function GetURLNotificationSender_1() As String
        'Return _strDomain + _strWSNotificationSender + "/" + _strWSNotificationAction
        Return _strDomain_1 + _strWSNotificationSender
    End Function

    Public Shared Function GetURLNotificationSender_2() As String
        Return _strDomain_2 + _strWSNotificationSender
    End Function

    Public Shared Function GetCertSerialNo() As String
        Return _strCertSerialNo
    End Function

    Public Shared Function GetThumbprintNo() As String
        Return _strThumbprint
    End Function


    Public Shared Sub InitParameters()

        _strDomain_1 = Configuration.ConfigurationManager.AppSettings("EGISDomain_X509Certificate_1").ToString()
        _strDomain_2 = Configuration.ConfigurationManager.AppSettings("EGISDomain_X509Certificate_2").ToString()

        _strWSNotificationSender = Configuration.ConfigurationManager.AppSettings("NOTI_SENDER_END_POINT").ToString()
        _strWSNotificationAction = Configuration.ConfigurationManager.AppSettings("NOTI_SENDER_SOAP_ACTION").ToString()

        _strCertSerialNo = Configuration.ConfigurationManager.AppSettings("Cert_SerialNo").ToString()
        _strThumbprint = Configuration.ConfigurationManager.AppSettings("Thumbprint").ToString()

    End Sub

    Public Class NotificationSenderResponse_Status
        Public Const Success As String = "S"
        Public Const Fail As String = "F"
    End Class

    Public Class WSEmail

        Public Const ChannelType As String = "EM"

        Public Const CharSet As String = "UTF-8"

        Public Class ContentType
            Public Const HTML As String = "text/html"
            Public Const PLAIN As String = "text/plain"
        End Class

        Public Const SubjectMaxLength As Integer = 85

        Public Const ContentMaxSize As String = "20k Bytes"

    End Class
End Class
