Imports System.Data.SqlClient

Namespace Component.SchemeDetails
    <Serializable()> Public Class EqvSubsidizeMapModel

#Region "Schema"

        'Scheme_Code	char(10)	Unchecked
        'Scheme_Seq	smallint	Unchecked
        'Subsidize_Code	char(10)	Unchecked
        'Subsidize_Item_Code	char(10)	Unchecked
        'Available_Item_Code	char(10)	Unchecked
        'Eqv_Scheme_Code	char(10)	Unchecked
        'Eqv_Scheme_Seq	smallint	Unchecked
        'Eqv_Subsidize_Code	char(10)	Unchecked
        'Eqv_Subsidize_Item_Code	char(10)	Unchecked
        'Eqv_Available_Item_Code	char(10)	Unchecked

#End Region

#Region "Private Member"
        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Item_Code As String

        Private _strEqv_Scheme_Code As String
        Private _intEqv_Scheme_Seq As Integer
        Private _strEqv_Subsidize_Item_Code As String
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

        Public Property SubsidizeItemCode() As String
            Get
                Return Me._strSubsidize_Item_Code
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Item_Code = value
            End Set
        End Property

        Public Property EqvSchemeCode() As String
            Get
                Return Me._strEqv_Scheme_Code
            End Get
            Set(ByVal value As String)
                Me._strEqv_Scheme_Code = value
            End Set
        End Property

        Public Property EqvSchemeSeq() As Integer
            Get
                Return Me._intEqv_Scheme_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intEqv_Scheme_Seq = value
            End Set
        End Property

        Public Property EqvSubsidizeItemCode() As String
            Get
                Return Me._strEqv_Subsidize_Item_Code
            End Get
            Set(ByVal value As String)
                Me._strEqv_Subsidize_Item_Code = value
            End Set
        End Property
       
#End Region


#Region "Constructor"
        Public Sub New()
        End Sub

        Public Sub New(ByVal udtEqvSubsidizeMapModel As EqvSubsidizeMapModel)
            Me._strScheme_Code = udtEqvSubsidizeMapModel._strScheme_Code
            Me._intScheme_Seq = udtEqvSubsidizeMapModel._intScheme_Seq
            Me._strSubsidize_Item_Code = udtEqvSubsidizeMapModel._strSubsidize_Item_Code

            Me._strEqv_Scheme_Code = udtEqvSubsidizeMapModel._strEqv_Scheme_Code
            Me._intEqv_Scheme_Seq = udtEqvSubsidizeMapModel._intEqv_Scheme_Seq
            Me._strEqv_Subsidize_Item_Code = udtEqvSubsidizeMapModel._strEqv_Subsidize_Item_Code
        End Sub
#End Region
    End Class
End Namespace