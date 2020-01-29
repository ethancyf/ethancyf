Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Web
Imports Common.ComObject
Imports Common.Component

Public Class AuditLogBase
    Inherits Common.ComObject.AuditLogEntry

    Protected _hashMessage As Hashtable

    Public Overrides ReadOnly Property Platform() As String
        Get
            Return EVSPlatform.InterfaceExternal
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

    Public Sub WriteSystemLog(ex As Exception, strFunctionCode As String, strUserID As String)
        Dim objDatabase As New Common.DataAccess.Database(Common.Component.DBFlagStr.DBFlag2)
        Dim tmpMessage As String = "GetBaseException Message: " & ex.Message.ToString
        Dim tmpStackTrace As String = String.Empty

        If Not IsNothing(ex.StackTrace) Then
            tmpStackTrace = "GetBaseException StackTrace: " & ex.StackTrace.ToString
        End If

        If Not ex.InnerException Is Nothing Then
            tmpMessage = tmpMessage & vbCrLf & vbCrLf & " InnerException Message: " & ex.Message.ToString
            tmpStackTrace = tmpStackTrace & vbCrLf & vbCrLf & " InnerException StackTrace: " & ex.StackTrace.ToString
        End If

        Dim strSeverityCode As String = String.Empty

        If TypeOf ex Is SqlException Then
            strSeverityCode = Chr(ErrorHandler.EnumSeverityCode.Data)
        Else
            strSeverityCode = Chr(ErrorHandler.EnumSeverityCode.Unknown)
        End If

        Dim strPlatform As String = ConfigurationManager.AppSettings("Platform")

        If strPlatform = EVSPlatform.InterfaceInternal OrElse strPlatform = EVSPlatform.InterfaceExternal Then
            ErrorHandler.Log(objDatabase, MyBase.FunctionCode, strSeverityCode, "99999", String.Empty, _
                       System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName).AddressList(0).ToString, strUserID, tmpMessage & vbCrLf & vbCrLf & tmpStackTrace)

        Else
            ErrorHandler.Log(strFunctionCode, strSeverityCode, "99999", HttpContext.Current.Request.PhysicalPath, _
                             HttpContext.Current.Request.UserHostAddress, ex.Message)

        End If

    End Sub

End Class
