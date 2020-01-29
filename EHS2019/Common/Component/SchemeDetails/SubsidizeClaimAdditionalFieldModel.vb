Imports System.Data.SqlClient

Namespace Component.SchemeDetails
    <Serializable()> Public Class SubsidizeClaimAdditionalFieldModel

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

#Region "Schema"

        'Scheme_Code	char(10)	Unchecked
        'Scheme_Seq	smallint	Unchecked
        'Subsidize_Code	char(10)	Unchecked
        'Display_Seq	smallint	Unchecked
        'AdditionalFieldID	varchar(20)	Unchecked
        'AdditionalFieldType	varchar(50)	Checked
        'Display_Name	varchar(100)	Checked
        'Display_Name_Chi	varchar(100)	Checked
        'List_Column	varchar(30)	Checked
        'Mandatory	char(1)	Checked

#End Region

#Region "Private Member"

        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String
        Private _intDisplay_Seq As Integer
        Private _strAdditionalFieldID As String

        Private _strAdditionalFieldType As String
        Private _strDisplay_Name As String
        Private _strDisplay_Name_Chi As String
        Private _strList_Column As String
        Private _strMandatory As String
#End Region

#Region "SQL Data Type"

        Public Const Scheme_CodeDataType As SqlDbType = SqlDbType.Char
        Public Const Scheme_CodeDataSize As Integer = 10

        Public Const Scheme_SeqDataType As SqlDbType = SqlDbType.SmallInt
        Public Const Scheme_SeqDataSize As Integer = 2

        Public Const Subsidize_CodeDataType As SqlDbType = SqlDbType.Char
        Public Const Subsidize_CodeDataSize As Integer = 10

        Public Const Display_SeqDataType As SqlDbType = SqlDbType.SmallInt
        Public Const Display_SeqDataSize As Integer = 2

        Public Const AdditionalFieldIDDataType As SqlDbType = SqlDbType.VarChar
        Public Const AdditionalFieldIDDataSize As Integer = 20

        Public Const AdditionalFieldTypeDataType As SqlDbType = SqlDbType.VarChar
        Public Const AdditionalFieldTypeDataSize As Integer = 50

        Public Const Display_NameDataType As SqlDbType = SqlDbType.VarChar
        Public Const Display_NameDataSize As Integer = 100

        Public Const Display_Name_ChiDataType As SqlDbType = SqlDbType.NVarChar
        Public Const Display_Name_ChiDataSize As Integer = 100

        Public Const MandatoryDataType As SqlDbType = SqlDbType.Char
        Public Const MandatoryDataSize As Integer = 1

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

        Public Property DisplaySeq() As Integer
            Get
                Return Me._intDisplay_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intDisplay_Seq = value
            End Set
        End Property

        Public Property AdditionalFieldID() As String
            Get
                Return Me._strAdditionalFieldID
            End Get
            Set(ByVal value As String)
                Me._strAdditionalFieldID = value
            End Set
        End Property

        Public Property AdditionalFieldType() As String
            Get
                Return Me._strAdditionalFieldType
            End Get
            Set(ByVal value As String)
                Me._strAdditionalFieldType = value
            End Set
        End Property

        Public Property DisplayName() As String
            Get
                Return Me._strDisplay_Name
            End Get
            Set(ByVal value As String)
                Me._strDisplay_Name = value
            End Set
        End Property

        Public Property DisplayNameChi() As String
            Get
                Return Me._strDisplay_Name_Chi
            End Get
            Set(ByVal value As String)
                Me._strDisplay_Name_Chi = value
            End Set
        End Property

        Public Property ListColumn() As String
            Get
                Return Me._strList_Column
            End Get
            Set(ByVal value As String)
                Me._strList_Column = value
            End Set
        End Property

        Public Property Mandatory() As Boolean
            Get
                If Me._strMandatory.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strMandatory = strYES
                Else
                    Me._strMandatory = strNO
                End If
            End Set
        End Property

#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSubsidizeClaimAdditionalFieldModel As SubsidizeClaimAdditionalFieldModel)
            Me._strScheme_Code = udtSubsidizeClaimAdditionalFieldModel._strScheme_Code
            Me._intScheme_Seq = udtSubsidizeClaimAdditionalFieldModel._intScheme_Seq
            Me._strSubsidize_Code = udtSubsidizeClaimAdditionalFieldModel._strSubsidize_Code
            Me._intDisplay_Seq = udtSubsidizeClaimAdditionalFieldModel._intDisplay_Seq
            Me._strAdditionalFieldID = udtSubsidizeClaimAdditionalFieldModel._strAdditionalFieldID

            Me._strAdditionalFieldType = udtSubsidizeClaimAdditionalFieldModel._strAdditionalFieldType
            Me._strDisplay_Name = udtSubsidizeClaimAdditionalFieldModel._strDisplay_Name
            Me._strDisplay_Name_Chi = udtSubsidizeClaimAdditionalFieldModel._strDisplay_Name_Chi
            Me._strList_Column = udtSubsidizeClaimAdditionalFieldModel._strList_Column
            Me._strMandatory = udtSubsidizeClaimAdditionalFieldModel._strMandatory

        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
            ByVal _intDisplay_Seq As Integer, ByVal strAdditionalFieldID As String, ByVal strAdditionalFieldType As String, _
            ByVal strDisplayName As String, ByVal strDisplayNameChi As String, ByVal strListColumn As String, ByVal strMandatory As String)

            Me._strScheme_Code = strSchemeCode
            Me._intScheme_Seq = intSchemeSeq
            Me._strSubsidize_Code = strSubsidizeCode
            Me._intDisplay_Seq = _intDisplay_Seq
            Me._strAdditionalFieldID = strAdditionalFieldID

            Me._strAdditionalFieldType = strAdditionalFieldType
            Me._strDisplay_Name = strDisplayName
            Me._strDisplay_Name_Chi = strDisplayNameChi
            Me._strList_Column = strListColumn
            Me._strMandatory = strMandatory
        End Sub

#End Region
    End Class
End Namespace
