Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Cryptography
    Public Class ResolveResult

#Region "Memeber"

        Private blnSuccessfulVerified As Boolean = False
        Private blnSuccessfulDecrypted As Boolean = False
        Private strPlainXML As String = Nothing

#End Region

#Region "Property"

        Public Property SuccessfulVerified() As Boolean
            Get
                Return blnSuccessfulVerified
            End Get
            Set(ByVal value As Boolean)

                blnSuccessfulVerified = value
            End Set
        End Property

        Public Property SuccessfulDecrypted() As Boolean
            Get
                Return blnSuccessfulDecrypted
            End Get
            Set(ByVal value As Boolean)
                blnSuccessfulDecrypted = value
            End Set
        End Property

        Public Property PlainXML() As String
            Get
                Return strPlainXML
            End Get
            Set(ByVal value As String)
                strPlainXML = value
            End Set
        End Property

#End Region

    End Class
End Namespace
