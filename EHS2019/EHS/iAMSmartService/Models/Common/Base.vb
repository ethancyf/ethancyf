'Imports PHF
'Imports PHF.DataAccess
'Imports PHF.Constants
'Imports PHF.SetupBLL.UserAgentMappingSetupBLL
'Imports PHF.Setup

Imports Common
Imports Common.ComObject

'Imports Newtonsoft.Json.Linq

Namespace ServiceSessionHandler

    Public Class Base

#Region "Constants"

        Private Const FunctionCode As String = Common.Component.FunctCode.FUNT120101
        Shared _strOS As String
        Shared _strBrowser As String
        Shared _strUndefinedUserAgent As String
#End Region

#Region "Enums"

        Private Enum EnumSessionKey
            OS
            Browser
            UndefinedUserAgent
        End Enum

#End Region

#Region "Properties"

        Public Shared Property OS() As String
            Get
                Return UserAgentInfoMapping.GetOS()
            End Get
            Set(ByVal value As String)
                _strOS = value
            End Set
        End Property

        Public Shared Property Browser() As String
            Get
                Return UserAgentInfoMapping.GetBrowser()
            End Get
            Set(ByVal value As String)
                _strBrowser = value
            End Set
        End Property

        Public Shared Property UndefinedUserAgent() As String
            Get
                Return _strUndefinedUserAgent
            End Get
            Set(ByVal value As String)
                _strUndefinedUserAgent = value
            End Set
        End Property

        'Public Shared ReadOnly Property AllEmpty() As Boolean
        '    Get
        '        If IsNothing(OS) AndAlso IsNothing(Browser) AndAlso IsNothing(UndefinedUserAgent) Then
        '            Return True
        '        Else
        '            Return False
        '        End If
        '    End Get
        'End Property

#End Region

#Region "Methods"

        Public Shared Sub ClearAllSession()
            For Each item In [Enum].GetValues(GetType(EnumSessionKey))
                ClearSession(item)
            Next
        End Sub

        '
        Private Shared Function GetSession(ByVal strKey As EnumSessionKey)
            Return HttpContext.Current.Session(String.Format("{0}_{1}", FunctionCode, strKey.ToString))
        End Function

        Private Shared Sub AddSession(ByVal strKey As EnumSessionKey, ByVal objValue As Object)
            HttpContext.Current.Session(String.Format("{0}_{1}", FunctionCode, strKey.ToString)) = objValue
        End Sub

        Private Shared Sub ClearSession(ByVal strKey As EnumSessionKey)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", FunctionCode, strKey.ToString))
        End Sub

#End Region

    End Class

End Namespace
