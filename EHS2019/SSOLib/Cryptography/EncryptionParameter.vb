' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : DateType
' Detail            : XML Encryption parameter
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------

Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace Cryptography
    Public Class EncryptionParameter
        Private strCertificateThumbprint As String = ""
        Private blnEnable As Boolean = False
        Private strXMLEncryptElementName As String = ""

        Public Sub New()
        End Sub


        Public Sub New(ByVal strCertificateThumbprint As String, ByVal blnEnable As Boolean, ByVal strXMLEncryptElementName As String)
            Me.strCertificateThumbprint = strCertificateThumbprint
            Me.blnEnable = blnEnable
            Me.strXMLEncryptElementName = strXMLEncryptElementName
        End Sub

        Public Property CertificateThumbprint() As String
            Get
                Return strCertificateThumbprint
            End Get
            Set(ByVal value As String)
                strCertificateThumbprint = value
            End Set
        End Property

        Public Property XMLEncryptElementName() As String

            Get
                Return strXMLEncryptElementName
            End Get
            Set(ByVal value As String)
                strXMLEncryptElementName = value
            End Set
        End Property

        Public Property Enable() As Boolean
            Get
                Return blnEnable
            End Get
            Set(ByVal value As Boolean)
                blnEnable = value
            End Set
        End Property
    End Class
End Namespace