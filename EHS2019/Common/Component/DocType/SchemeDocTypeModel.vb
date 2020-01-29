Imports System.Data.SqlClient

Namespace Component.DocType
    <Serializable()> Public Class SchemeDocTypeModel

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

#Region "Schema"
        'Scheme_Code	char(10)	Unchecked
        'Doc_Code	char(20)	
        'Major_Doc	char(1)
#End Region

#Region "Private Member"

        Private _strScheme_Code As String
        Private _strDoc_Code As String
        Private _strMajor_Doc As String

        Private _intAge_LowerLimit As Nullable(Of Integer)
        Private _strAge_LowerLimitUnit As String
        Private _intAge_UpperLimit As Nullable(Of Integer)
        Private _strAge_UpperLimitUnit As String
        Private _strAge_CalMethod As String
#End Region

#Region "SQL Data Type"

        Public Const Scheme_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Scheme_Code_DataSize As Integer = 10

        Public Const Doc_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Doc_Code_DataSize As Integer = 20

        Public Const Major_Doc_DataType As SqlDbType = SqlDbType.Char
        Public Const Major_Doc_DataSize As Integer = 1

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

        Public Property DocCode() As String
            Get
                Return Me._strDoc_Code
            End Get
            Set(ByVal value As String)
                Me._strDoc_Code = value
            End Set
        End Property

        Public Property IsMajorDoc() As Boolean
            Get
                If Me._strMajor_Doc.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strMajor_Doc = strYES
                Else
                    Me._strMajor_Doc = strNO
                End If
            End Set
        End Property

        Public Property AgeLowerLimit() As Nullable(Of Integer)
            Get
                Return Me._intAge_LowerLimit
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intAge_LowerLimit = value
            End Set
        End Property

        Public Property AgeLowerLimitUnit() As String
            Get
                Return Me._strAge_LowerLimitUnit
            End Get
            Set(ByVal value As String)
                Me._strAge_LowerLimitUnit = value
            End Set
        End Property

        Public Property AgeUpperLimit() As Nullable(Of Integer)
            Get
                Return Me._intAge_UpperLimit
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intAge_UpperLimit = value
            End Set
        End Property

        Public Property AgeUpperLimitUnit() As String
            Get
                Return Me._strAge_UpperLimitUnit
            End Get
            Set(ByVal value As String)
                Me._strAge_UpperLimitUnit = value
            End Set
        End Property

        Public Property AgeCalMethod() As String
            Get
                Return Me._strAge_CalMethod
            End Get
            Set(ByVal value As String)
                Me._strAge_CalMethod = value
            End Set
        End Property
#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSchemeDocTypeModel As SchemeDocTypeModel)

            Me._strScheme_Code = udtSchemeDocTypeModel._strScheme_Code
            Me._strDoc_Code = udtSchemeDocTypeModel._strDoc_Code
            Me._strMajor_Doc = udtSchemeDocTypeModel._strMajor_Doc

            Me._intAge_LowerLimit = udtSchemeDocTypeModel._intAge_LowerLimit
            Me._strAge_LowerLimitUnit = udtSchemeDocTypeModel._strAge_LowerLimitUnit
            Me._intAge_UpperLimit = udtSchemeDocTypeModel._intAge_UpperLimit
            Me._strAge_UpperLimitUnit = udtSchemeDocTypeModel._strAge_UpperLimitUnit
            Me._strAge_CalMethod = udtSchemeDocTypeModel._strAge_CalMethod

        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal strDocCode As String, ByVal strMajorDoc As String, _
            ByVal intAgeLowerLimit As Nullable(Of Integer), ByVal strAgeLowerLimitUnit As String, ByVal intAgeUpperLimit As Nullable(Of Integer), _
            ByVal strAgeUpperLimitUnit As String, ByVal strAgeCalMethod As String)

            Me._intAge_LowerLimit = intAgeLowerLimit
            Me._strAge_LowerLimitUnit = strAgeLowerLimitUnit
            Me._intAge_UpperLimit = intAgeUpperLimit
            Me._strAge_UpperLimitUnit = strAgeUpperLimitUnit
            Me._strAge_CalMethod = strAgeCalMethod

            Me._strScheme_Code = strSchemeCode
            Me._strDoc_Code = strDocCode
            Me._strMajor_Doc = strMajorDoc
        End Sub

#End Region
    End Class
End Namespace