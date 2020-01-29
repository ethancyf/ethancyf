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

<Serializable()> Public Class SSOAppInfo

#Region "Memeber"

    Private strSSOAppId As String
    Private strSSOAppName As String
    Private strSSOAppWinName As String

#End Region

#Region "Constructor"

    Public Sub New()

    End Sub


    Public Sub New(ByVal strSSOAppId As String, ByVal strSSOAppName As String, ByVal strSSOAppWinName As String)
        Me.strSSOAppId = strSSOAppId
        Me.strSSOAppName = strSSOAppName

        Me.strSSOAppWinName = strSSOAppWinName
    End Sub

#End Region

#Region "Property"

    Public Property SSOAppId() As String
        Get
            Return strSSOAppId
        End Get

        Set(ByVal value As String)
            strSSOAppId = value
        End Set
    End Property

    Public Property SSOAppName() As String
        Get
            Return strSSOAppName
        End Get

        Set(ByVal value As String)
            strSSOAppName = value
        End Set
    End Property

    Public Property SSOAppWinName() As String
        Get
            Return strSSOAppWinName
        End Get

        Set(ByVal value As String)
            strSSOAppWinName = value
        End Set
    End Property

#End Region

End Class
