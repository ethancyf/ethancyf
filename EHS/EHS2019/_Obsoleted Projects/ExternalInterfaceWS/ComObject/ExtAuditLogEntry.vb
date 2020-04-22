Imports Common.ComObject
Imports ExternalInterfaceWS.Component.ErrorInfo

Namespace ComObject

    Public Class ExtAuditLogEntry
        Inherits Common.ComObject.AuditLogEntry


#Region "Constructors"

        Public Sub New(ByVal strFunctionCode As String)
            MyBase.New(strFunctionCode)
        End Sub

        Public Sub New(ByVal strFunctionCode As String, ByVal strDBFlag As String)
            MyBase.New(strFunctionCode, strDBFlag)
        End Sub

#End Region

        Protected _strMessageID As String = Nothing
        Public Property MessageID() As String
            Get
                Return _strMessageID
            End Get
            Set(ByVal value As String)
                _strMessageID = value
            End Set
        End Property

        Protected _strUserID As String = Nothing
        Public Property UserID() As String
            Get
                Return _strUserID
            End Get
            Set(ByVal value As String)
                _strUserID = value
            End Set
        End Property

        Protected _strSystemID As String = Nothing
        Public Property IncomingSystemID() As String
            Get
                Return _strSystemID
            End Get
            Set(ByVal value As String)
                _strSystemID = value
            End Set
        End Property


#Region "Extended functions"

        'Public Sub WriteStartLog_Ext(ByVal strLogID As String)
        '    MyBase.WriteStartLog(strLogID, ExtAuditLogMaster.Messages(strLogID), _strUserID, _strSystemID, _strMessageID)
        'End Sub

        Public Sub WriteEndLog_Ext(ByVal strLogID As String, ByVal strMessageCode As String)

            MyBase.WriteEndLog(strLogID, ExtAuditLogMaster.Messages(strLogID), _strUserID, _strSystemID, _strMessageID, strMessageCode)
        End Sub

        Public Sub WriteLog_Ext(ByVal strLogID As String)
            'MyBase.ResetActionTime()
            MyBase.WriteLog(strLogID, ExtAuditLogMaster.Messages(strLogID), _strUserID, _strSystemID, _strMessageID)
        End Sub

        Public Sub AddDescripton_Ext(ByVal strField As String, ByVal strValue As String)
            MyBase.AddDescripton(strField, strValue)
        End Sub

        Public Sub WriteLogData_Ext(ByVal strLogID As String, ByVal strData As String)
            'MyBase.ResetActionTime()
            WriteLogData(strLogID, ExtAuditLogMaster.Messages(strLogID), strData, _strUserID, _strSystemID, _strMessageID)
        End Sub

        Public Sub WriteLogWithErrorList_Ext(ByVal strLogID As String, ByVal udtErrorCodeList As ErrorInfoModelCollection)

            If udtErrorCodeList.Count > 0 Then
                Dim strErrorList As String = String.Empty
                For Each udtError As ErrorInfoModel In udtErrorCodeList
                    If strErrorList <> String.Empty Then
                        strErrorList = strErrorList + " ,"
                    End If
                    strErrorList = strErrorList + udtError.ErrorMessage
                Next

                Me.AddDescripton("ErrorList", strErrorList)
                Me.WriteLog_Ext(strLogID)
            Else
                Me.WriteLog_Ext(strLogID)
            End If

        End Sub


#End Region


    End Class

End Namespace


