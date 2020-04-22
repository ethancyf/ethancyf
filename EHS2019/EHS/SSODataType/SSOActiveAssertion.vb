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

Public Class SSOActiveAssertion

#Region "Memeber"

    Private strTxnId As String = Nothing
    Private strArtifact As String = Nothing
    Private strAssertion As String = Nothing
    Private intReadCount As Integer = -1
    Private dtCreationDateTime As DateTime

#End Region

#Region "Constructor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal strTxnId As String, ByVal strArtifact As String, ByVal strAssertion As String, ByVal intReadCount As Integer, ByVal dtCreationDateTime As DateTime)

        Me.strTxnId = strTxnId
        Me.strArtifact = strArtifact
        Me.strAssertion = strAssertion
        Me.intReadCount = intReadCount
        Me.dtCreationDateTime = dtCreationDateTime
    End Sub

#End Region

#Region "Property"

    Public Property TnxId() As String

        Get
            Return strTxnId
        End Get

        Set(ByVal value As String)
            strTxnId = value
        End Set
    End Property

    Public Property Artifact() As String

        Get
            Return strArtifact
        End Get

        Set(ByVal value As String)
            strArtifact = value
        End Set
    End Property

    Public Property Assertion() As String

        Get
            Return strAssertion
        End Get

        Set(ByVal value As String)
            strAssertion = value
        End Set
    End Property

    Public Property ReadCount() As Integer

        Get
            Return intReadCount
        End Get

        Set(ByVal value As Integer)
            intReadCount = value
        End Set
    End Property

    Public Property CreationDateTime() As DateTime

        Get
            Return dtCreationDateTime
        End Get

        Set(ByVal value As DateTime)
            dtCreationDateTime = value
        End Set
    End Property

#End Region

End Class
