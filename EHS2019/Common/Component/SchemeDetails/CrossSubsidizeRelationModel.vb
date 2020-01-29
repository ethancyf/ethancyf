Imports System.Data.SqlClient

Namespace Component.SchemeDetails
    <Serializable()> Public Class CrossSubsidizeRelationModel

#Region "Private Member"
        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String

        Private _strRelate_Scheme_Code As String
        Private _intRelate_Scheme_Seq As Integer
        Private _strRelate_Subsidize_Code As String

#End Region

#Region "Property"

        Public Property SchemeCode() As String
            Get
                Return Me._strScheme_Code
            End Get
            Set(ByVal value As String)
                Me._strScheme_Code = value
            End Set
        End Property

        Public Property SchemeSeq() As Integer
            Get
                Return Me._intScheme_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intScheme_Seq = value
            End Set
        End Property

        Public Property SubsidizeCode() As String
            Get
                Return Me._strSubsidize_Code
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Code = value
            End Set
        End Property

        Public Property RelateSchemeCode() As String
            Get
                Return Me._strRelate_Scheme_Code
            End Get
            Set(ByVal value As String)
                Me._strRelate_Scheme_Code = value
            End Set
        End Property

        Public Property RelateSchemeSeq() As Integer
            Get
                Return Me._intRelate_Scheme_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intRelate_Scheme_Seq = value
            End Set
        End Property

        Public Property RelateSubsidizeCode() As String
            Get
                Return Me._strRelate_Subsidize_Code
            End Get
            Set(ByVal value As String)
                Me._strRelate_Subsidize_Code = value
            End Set
        End Property
#End Region

#Region "Constructor"

        Public Sub New()
        End Sub

        Public Sub New(ByVal udtCrossSubsidizeRelation As CrossSubsidizeRelationModel)
            Me._strScheme_Code = udtCrossSubsidizeRelation._strScheme_Code
            Me._intScheme_Seq = udtCrossSubsidizeRelation._intScheme_Seq
            Me._strSubsidize_Code = udtCrossSubsidizeRelation._strSubsidize_Code
            Me._strRelate_Scheme_Code = udtCrossSubsidizeRelation._strRelate_Scheme_Code
            Me._intRelate_Scheme_Seq = udtCrossSubsidizeRelation._intRelate_Scheme_Seq
            Me._strRelate_Subsidize_Code = udtCrossSubsidizeRelation._strRelate_Subsidize_Code
        End Sub

#End Region

    End Class
End Namespace