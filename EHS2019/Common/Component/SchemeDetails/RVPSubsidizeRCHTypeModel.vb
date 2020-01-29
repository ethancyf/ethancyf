Imports System.Data.SqlClient

Namespace Component.SchemeDetails
    <Serializable()> Public Class RVPSubsidizeRCHTypeModel

#Region "Private Member"
        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String
        Private _stRCH_Type As String

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

        Public Property RCHType() As String
            Get
                Return Me._stRCH_Type
            End Get
            Set(ByVal value As String)
                Me._stRCH_Type = value
            End Set
        End Property

#End Region


#Region "Constructor"
        Public Sub New()
        End Sub

        Public Sub New(ByVal udtRVPSubsidizeRCHTypeModel As RVPSubsidizeRCHTypeModel)
            Me._strScheme_Code = udtRVPSubsidizeRCHTypeModel._strScheme_Code
            Me._intScheme_Seq = udtRVPSubsidizeRCHTypeModel._intScheme_Seq
            Me._strSubsidize_Code = udtRVPSubsidizeRCHTypeModel._strSubsidize_Code
            Me._stRCH_Type = udtRVPSubsidizeRCHTypeModel._stRCH_Type
        End Sub
#End Region
    End Class
End Namespace