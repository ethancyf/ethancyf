Public Class TokenSettingModel

    Public UserID As String
    Public TokenSerial As String
    Public TokenReplaceSerial As String

    Public Sub New(ByVal strUserID As String, ByVal strTokenSerial As String, ByVal strTokenReplaceSerial As String)
        UserID = strUserID.Trim
        TokenSerial = strTokenSerial.Trim
        TokenReplaceSerial = strTokenReplaceSerial.Trim
    End Sub
End Class
