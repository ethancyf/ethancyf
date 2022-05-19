Imports Common.ComObject

Namespace Log

    Public Class AuditLogService
        Inherits AuditLogEntry

#Region "Constructors"

        Public Sub New(ByVal strFunctionCode As String)
            MyBase.New(strFunctionCode)

        End Sub

#End Region

#Region "Fields and Properties"

        Public Property OS() As String
            Get
                Return ServiceSessionHandler.Base.OS
            End Get
            Set(value As String)
                ServiceSessionHandler.Base.OS = value
            End Set
        End Property

        Public Property Browser() As String
            Get
                Return ServiceSessionHandler.Base.Browser
            End Get
            Set(value As String)
                ServiceSessionHandler.Base.Browser = value
            End Set
        End Property

        Public Property UndefinedUserAgent() As String
            Get
                Return ServiceSessionHandler.Base.UndefinedUserAgent
            End Get
            Set(value As String)
                ServiceSessionHandler.Base.UndefinedUserAgent = value
            End Set
        End Property

#End Region

    End Class

End Namespace