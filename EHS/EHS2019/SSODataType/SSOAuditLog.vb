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

Public Class SSOAuditLog

#Region "Memeber"

    Private strTxnId As String
    Private strMsgType As String
    Private strSourceSite As String
    Private strTargetSite As String
    Private strArtifact As String
    Private strPlainAssertion As String
    Private strSecuredAssertion As String
    Private strPlainArtifactResolveReq As String
    Private strSecuredArtifactResolveReq As String
    Private dtCreationDatetime As DateTime

#End Region

#Region "Constructor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal strTxnId As String, ByVal strMsgType As String, ByVal strSourceSite As String, ByVal strTargetSite As String, ByVal strArtifact As String, ByVal strPlainAssertion As String, _
    ByVal strSecuredAssertion As String, ByVal strPlainArtifactResolveReq As String, ByVal strSecuredArtifactResolveReq As String, ByVal dtCreationDatetime As DateTime)
        Me.strTxnId = strTxnId
        Me.strMsgType = strMsgType
        Me.strSourceSite = strSourceSite
        Me.strTargetSite = strTargetSite
        Me.strArtifact = strArtifact
        Me.strPlainAssertion = strPlainAssertion
        Me.strSecuredAssertion = strSecuredAssertion
        Me.strPlainArtifactResolveReq = strPlainArtifactResolveReq
        Me.strSecuredArtifactResolveReq = strSecuredArtifactResolveReq
        Me.dtCreationDatetime = dtCreationDatetime
    End Sub

#End Region

#Region "Property"

    Public Property TxnId() As String
        Get
            Return strTxnId
        End Get
        Set(ByVal value As String)
            strTxnId = value
        End Set
    End Property

    Public Property MsgType() As String
        Get
            Return strMsgType
        End Get
        Set(ByVal value As String)
            strMsgType = value
        End Set
    End Property

    Public Property SourceSite() As String
        Get
            Return strSourceSite
        End Get
        Set(ByVal value As String)
            strSourceSite = value
        End Set
    End Property

    Public Property TargetSite() As String
        Get
            Return strTargetSite
        End Get
        Set(ByVal value As String)
            strTargetSite = value
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

    Public Property PlainAssertion() As String
        Get
            Return strPlainAssertion
        End Get
        Set(ByVal value As String)
            strPlainAssertion = value
        End Set
    End Property

    Public Property SecuredAssertion() As String
        Get
            Return strSecuredAssertion
        End Get
        Set(ByVal value As String)
            strSecuredAssertion = value
        End Set
    End Property

    Public Property PlainArtifactResolveReq() As String
        Get
            Return strPlainArtifactResolveReq
        End Get
        Set(ByVal value As String)
            strPlainArtifactResolveReq = value
        End Set
    End Property

    Public Property SecuredArtifactResolveReq() As String
        Get
            Return strSecuredArtifactResolveReq
        End Get
        Set(ByVal value As String)
            strSecuredArtifactResolveReq = value
        End Set
    End Property

    Public Property CreationDatetime() As DateTime
        Get
            Return dtCreationDatetime
        End Get
        Set(ByVal value As DateTime)
            dtCreationDatetime = value
        End Set
    End Property

#End Region

End Class
