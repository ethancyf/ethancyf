Namespace Component.FileGeneration

    <Serializable()> Public Class FileGenerationUserControlModel

#Region "Schema"

        ' File_ID	        varchar(30)     NULL
        ' Display_Seq       int             NOT NULL
        ' UC_ID             varchar(100)    NOT NULL
        ' UC_Setting        ntext           NULL
        ' Parameter_Suffix  varchar(30)     NULL

#End Region

#Region "Private Member"

        Private _strFileID As String
        Private _intDisplaySeq As Integer
        Private _strUserControlID As String
        Private _dicUserControlSetting As Dictionary(Of String, String)
        Private _strParameterSuffix As String

        'CRE13-003 Token Replacement [Start][Karl]
        Private _strXmlSetting As String
        'CRE13-003 Token Replacement [End][Karl]

#End Region

#Region "Property"

        Public ReadOnly Property FileID() As String
            Get
                Return _strFileID
            End Get
        End Property

        Public ReadOnly Property DisplaySeq() As String
            Get
                Return _intDisplaySeq
            End Get
        End Property

        Public ReadOnly Property UserControlID() As String
            Get
                Return _strUserControlID
            End Get
        End Property

        Public ReadOnly Property UserControlSetting() As Dictionary(Of String, String)
            Get
                Return _dicUserControlSetting
            End Get
        End Property

        Public ReadOnly Property ParameterSuffix() As String
            Get
                Return _strParameterSuffix
            End Get
        End Property

        'CRE13-003 Token Replacement [Start][Karl]
        Public ReadOnly Property XmlSetting() As String
            Get
                Return _strXmlSetting
            End Get
        End Property
        'CRE13-003 Token Replacement [End][Karl]


#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal strFileID As String, ByVal intDisplaySeq As Integer, ByVal strUserControlID As String, _
                        ByVal dicUserControlSetting As Dictionary(Of String, String), ByVal strParameterSuffix As String)
            _strFileID = strFileID
            _intDisplaySeq = intDisplaySeq
            _strUserControlID = strUserControlID
            _dicUserControlSetting = dicUserControlSetting
            _strParameterSuffix = strParameterSuffix
        End Sub

        'CRE13-003 Token Replacement [Start][Karl]
        Public Sub New(ByVal strFileID As String, ByVal intDisplaySeq As Integer, ByVal strUserControlID As String, _
                        ByVal dicUserControlSetting As Dictionary(Of String, String), ByVal strParameterSuffix As String, ByVal strXmlSetting As String)
            _strFileID = strFileID
            _intDisplaySeq = intDisplaySeq
            _strUserControlID = strUserControlID
            _dicUserControlSetting = dicUserControlSetting
            _strParameterSuffix = strParameterSuffix
            _strXmlSetting = strXmlSetting
        End Sub
        'CRE13-003 Token Replacement [End][Karl]

#End Region

    End Class

End Namespace

