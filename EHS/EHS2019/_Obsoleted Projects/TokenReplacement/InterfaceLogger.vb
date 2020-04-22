Imports Common.ComObject

Public Class InterfaceLogger
    Inherits AuditLogEntry

    Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
        MyBase.New(strFunctionCode, strDBFlag)
    End Sub

    Public Overloads Sub AddDescription(ByVal strField As String, ByVal strValue As String)
        MyBase.AddDescripton(strField, strValue)
    End Sub

    '

    Public Overloads Sub WriteLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strMessageID As String)
        MyBase.WriteLog(strLogID, strDescription, Nothing, Nothing, strMessageID)
    End Sub

    '

    Public Overloads Sub WriteStartLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strMessageID As String)
        MyBase.WriteStartLog(strLogID, strDescription, Nothing, Nothing, strMessageID)
    End Sub

    '

    Public Overloads Sub WriteEndLog(ByVal strLogID As String, ByVal strDescription As String, ByVal strMessageID As String)
        MyBase.WriteEndLog(strLogID, strDescription, Nothing, Nothing, strMessageID)
    End Sub

    '

    Public Overloads ReadOnly Property ActionKey() As String
        Get
            Return MyBase.ActionKey
        End Get
    End Property

End Class
