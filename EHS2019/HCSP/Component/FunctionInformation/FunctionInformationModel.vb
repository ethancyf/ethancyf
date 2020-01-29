Namespace Component.FunctionInformation
    Public Class FunctionInformationModel

#Region "Schema"
        'Path	varchar(255)	Unchecked
        'Function_Code	char(6)	Unchecked
        'Description	varchar(100)	Checked
        'Role	char(1)	Checked
        'Scheme_Code	char(10)	Unchecked
        'Effective_Date	datetime	Unchecked
        'Expiry_Date	datetime	Checked
        'Record_Status	char(1)	Unchecked
#End Region

#Region "Private Memeber"
        Private _strPath As String
        Private _strFunctionCode As String
        'Private _strDescription As String
        Private _strRole As String

        Private _strScheme_Code As String
        Private _dtmEffective_Date As DateTime
        Private _dtmExpiry_Date As Nullable(Of DateTime)
        'Private _strRecord_Status As String
#End Region


        Public Property Path() As String
            Get
                Return _strPath
            End Get
            Set(ByVal value As String)
                _strPath = value
            End Set
        End Property

        Public Property FunctionCode() As String
            Get
                Return _strFunctionCode
            End Get
            Set(ByVal value As String)
                _strFunctionCode = value
            End Set
        End Property

        Public Property Role() As String
            Get
                Return _strRole
            End Get
            Set(ByVal value As String)
                _strRole = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return Me._strScheme_Code
            End Get
            Set(ByVal value As String)
                Me._strScheme_Code = value
            End Set
        End Property

        Public Property EffectiveDate() As DateTime
            Get
                Return Me._dtmEffective_Date
            End Get
            Set(ByVal value As DateTime)
                Me._dtmEffective_Date = value
            End Set
        End Property

        Public Property ExpiryDate() As Nullable(Of DateTime)
            Get
                Return Me._dtmExpiry_Date
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmExpiry_Date = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtFunctionInformationModel As FunctionInformationModel)
            Me._strPath = udtFunctionInformationModel._strPath
            Me._strFunctionCode = udtFunctionInformationModel._strFunctionCode
            Me._strRole = udtFunctionInformationModel._strRole
            Me._strScheme_Code = udtFunctionInformationModel._strScheme_Code
            Me._dtmEffective_Date = udtFunctionInformationModel._dtmEffective_Date
            Me._dtmExpiry_Date = udtFunctionInformationModel._dtmExpiry_Date
        End Sub
    End Class
End Namespace

