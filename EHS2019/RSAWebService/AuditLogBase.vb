Imports common.Component
Imports Common.ComObject

Public Class AuditLogBase
    Inherits Common.ComObject.AuditLogEntry

    Protected _hashMessage As Hashtable

    '

    Public Overloads Sub WriteStartLog(ByVal strLogID As String)
        MyBase.WriteStartLog(strLogID, GetMessages(strLogID))
    End Sub


    Public Overloads Sub WriteEndLog(ByVal strLogID As String)
        MyBase.WriteEndLog(strLogID, GetMessages(strLogID))
    End Sub

    '

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Private Function GetMessages(ByVal strLogID As String)
        If _hashMessage Is Nothing Then
            CreateMessage()
        End If

        Return _hashMessage(strLogID)
    End Function

    Protected Overridable Sub CreateMessage()
        _hashMessage = New Hashtable(0)
    End Sub

End Class
