Imports System.Web
Imports common.Component
Imports Common.ComObject

Public Class AuditLogBase
    Inherits Common.ComObject.AuditLogEntry

    Protected _hashMessage As Hashtable

    Public Overrides ReadOnly Property Platform() As String
        Get
            Return "06"
        End Get
    End Property

    Public Overrides Sub WriteLog(ByVal strLogID As String)
        MyBase.WriteLog(strLogID, GetMessages(strLogID))
    End Sub

    Public Sub WriteLogAndMessageID(ByVal strLogID As String, ByVal strMessageID As String)
        MyBase.WriteLog(strLogID, GetMessages(strLogID), Nothing, Nothing, strMessageID)
    End Sub

    ''' <summary>
    ''' Write log with raw data, e.g. Interface module log input and output xml
    ''' </summary>
    ''' <param name="strLogID"></param>
    ''' <param name="strData"></param>
    ''' <remarks></remarks>
    Public Overloads Sub WriteLogData(ByVal strLogID As String, ByVal strData As String)
        MyBase.WriteLogData(strLogID, GetMessages(strLogID), strData)
    End Sub

    '

    Public Overloads Sub WriteStartLog(ByVal strLogID As String)
        MyBase.WriteStartLog(strLogID, GetMessages(strLogID))
    End Sub

    Public Overloads Sub WriteStartLog(ByVal strLogID As String, ByVal strUserID As String)
        MyBase.WriteStartLog(strLogID, GetMessages(strLogID), strUserID)
    End Sub

    Public Overloads Sub WriteStartLog(ByVal strLogID As String, ByVal strUserID As String, ByVal strMessageID As String)
        MyBase.WriteStartLog(strLogID, GetMessages(strLogID), strUserID, Nothing, strMessageID)
    End Sub

    '

    Public Overloads Sub WriteEndLog(ByVal strLogID As String)
        MyBase.WriteEndLog(strLogID, GetMessages(strLogID))
    End Sub

    Public Overloads Sub WriteEndLog(ByVal strLogID As String, ByVal strUserID As String)
        MyBase.WriteEndLog(strLogID, GetMessages(strLogID), strUserID)
    End Sub

    Public Overloads Sub WriteEndLog(ByVal strLogID As String, ByVal strUserID As String, ByVal strMessageID As String)
        MyBase.WriteEndLog(strLogID, GetMessages(strLogID), strUserID, Nothing, strMessageID)
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

    Public Sub WriteSystemLog(ByVal ex As Exception, ByVal strUserID As String)
        Dim objDatabase As New Common.DataAccess.Database(Common.Component.DBFlagStr.DBFlag2)
        Dim tmpMessage As String = "GetBaseException Message: " & ex.Message.ToString
        Dim tmpStackTrace As String = "GetBaseException StackTrace: " & ex.StackTrace.ToString

        If Not ex.InnerException Is Nothing Then
            tmpMessage = tmpMessage & vbCrLf & vbCrLf & " InnerException Message: " & ex.Message.ToString
            tmpStackTrace = tmpStackTrace & vbCrLf & vbCrLf & " InnerException StackTrace: " & ex.StackTrace.ToString
        End If

        Dim strSeverityCode As String = String.Empty
        If TypeOf ex Is System.Data.SqlClient.SqlException Then
            strSeverityCode = Chr(ErrorHandler.EnumSeverityCode.Data)
        Else
            strSeverityCode = Chr(ErrorHandler.EnumSeverityCode.Unknown)
        End If

        If HttpContext.Current Is Nothing Then
            ErrorHandler.Log(objDatabase, MyBase.FunctionCode, strSeverityCode, ComConfig.SystemLog.MessageCode, String.Empty, String.Empty, strUserID, tmpMessage & vbCrLf & vbCrLf & tmpStackTrace)
        Else
            ErrorHandler.Log(objDatabase, MyBase.FunctionCode, strSeverityCode, ComConfig.SystemLog.MessageCode, HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, strUserID, tmpMessage & vbCrLf & vbCrLf & tmpStackTrace)
        End If

    End Sub
End Class
