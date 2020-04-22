' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : DataType
' Detail            : Signature parameter
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
    Public Class DigitalSignatureParameter
        Private strCertificateThumbprint As String = ""
        Private blnEnable As Boolean = False
        Private strXMLSignElementId As String = ""
        Private strSignatureAttachmentElementId As String = ""


        Public Sub New()
        End Sub

        Public Sub New(ByVal strCertificateThumbprint As String, ByVal blnEnable As Boolean, ByVal strXMLSignElementId As String, ByVal strSignatureAttachmentElementId As String)
            Me.strCertificateThumbprint = strCertificateThumbprint
            Me.blnEnable = blnEnable
            Me.strXMLSignElementId = strXMLSignElementId
            Me.strSignatureAttachmentElementId = strSignatureAttachmentElementId
        End Sub

        Public Property CertificateThumbprint() As String

            Get
                Return strCertificateThumbprint
            End Get


            Set(ByVal value As String)
                strCertificateThumbprint = value
            End Set
        End Property


        Public Property XMLSignElementId() As String
            Get
                Return strXMLSignElementId
            End Get

            Set(ByVal value As String)

                strXMLSignElementId = value
            End Set
        End Property



        Public Property SignatureAttachmentElementId() As String
            Get
                Return strSignatureAttachmentElementId
            End Get

            Set(ByVal value As String)
                strSignatureAttachmentElementId = value
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
