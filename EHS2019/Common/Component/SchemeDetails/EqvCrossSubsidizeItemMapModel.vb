Imports System.Data.SqlClient

Namespace Component.SchemeDetails
    <Serializable()> Public Class EqvCrossSubsidizeItemMapModel

#Region "Private Member"
        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String

        Private _strEqv_Scheme_Code As String
        Private _intEqv_Scheme_Seq As Integer
        Private _strEqv_Subsidize_Code As String

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

        Public Property EqvSubsidizeCode() As String
            Get
                Return Me._strEqv_Subsidize_Code
            End Get
            Set(ByVal value As String)
                Me._strEqv_Subsidize_Code = value
            End Set
        End Property
#End Region


#Region "Constructor"
        Public Sub New()
        End Sub

        Public Sub New(ByVal udtEqvCrossSubsidizeItemMapModel As EqvCrossSubsidizeItemMapModel)
            Me._strScheme_Code = udtEqvCrossSubsidizeItemMapModel._strScheme_Code
            Me._intScheme_Seq = udtEqvCrossSubsidizeItemMapModel._intScheme_Seq
            Me._strSubsidize_Code = udtEqvCrossSubsidizeItemMapModel._strSubsidize_Code


            Me._strEqv_Scheme_Code = udtEqvCrossSubsidizeItemMapModel._strEqv_Scheme_Code
            Me._intEqv_Scheme_Seq = udtEqvCrossSubsidizeItemMapModel._intEqv_Scheme_Seq
            Me._strEqv_Subsidize_Code = udtEqvCrossSubsidizeItemMapModel._strEqv_Subsidize_Code
        End Sub
#End Region
    End Class
End Namespace