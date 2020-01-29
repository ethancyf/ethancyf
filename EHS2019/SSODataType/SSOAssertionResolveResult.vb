' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : User Define DataType 
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

Public Class SSOAssertionResolveResult

#Region "Memeber"

    Private blnSuccessfulVerified As Boolean = False
    Private blnSuccessfulDecrypted As Boolean = False
    Private objSSOAssertion As SSODataType.SSOAssertion = Nothing

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

    Public Property SSOAssertion() As SSODataType.SSOAssertion
        Get
            Return objSSOAssertion
        End Get
        Set(ByVal value As SSODataType.SSOAssertion)
            objSSOAssertion = value
        End Set
    End Property

#End Region

End Class
