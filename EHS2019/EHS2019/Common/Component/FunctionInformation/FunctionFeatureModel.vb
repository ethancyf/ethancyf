Imports Common.Component.EHSTransaction

Namespace Component.FunctionInformation
    <Serializable()> Public Class FunctionFeatureModel
        Inherits BaseModel

#Region "DB Table Schema - [FunctionFeature_FNFT]"
        Public Class DBTable

            '[FNFT_Function_Code] [char](6) NOT NULL,
            '[FNFT_Feature_Code] [varchar](50) NOT NULL,
            '[FNFT_Record_Status] [char](1) NOT NULL,
            '[FNFT_Create_Dtm] [datetime] NOT NULL,
            '[FNFT_Create_By] [varchar](20) NOT NULL,

            Public Const TableName As String = "FunctionFeature_FNFT"

            Public Class FNFT_Function_Code
                Public Const Name As String = "FNFT_Function_Code"
                Public Const DataType As SqlDbType = SqlDbType.Char
                Public Const DataSize As Integer = 6
            End Class

            Public Class FNFT_Feature_Code
                Public Const Name As String = "FNFT_Feature_Code"
                Public Const DataType As SqlDbType = SqlDbType.VarChar
                Public Const DataSize As Integer = 50
            End Class

            Public Class FNFT_Record_Status
                Public Const Name As String = "FNFT_Record_Status"
                Public Const DataType As SqlDbType = SqlDbType.Char
                Public Const DataSize As Integer = 1
            End Class

            Public Class FNFT_Create_Dtm
                Public Const Name As String = "FNFT_Create_Dtm"
                Public Const DataType As SqlDbType = SqlDbType.DateTime
                Public Const DataSize As Integer = 8
            End Class


            Public Class FNFT_Create_By
                Public Const Name As String = "FNFT_Create_By"
                Public Const DataType As SqlDbType = SqlDbType.VarChar
                Public Const DataSize As Integer = 20
            End Class

        End Class
#End Region

#Region "[Enum_Class] of DB Table - [StatusData]"
        ' To map the display text of the Field - [FunctionFeature_FNFT]
        'Public Const STATUS_DATA_CLASS As String = "FunctionFeatureStatus"

        Public Enum EnumRecordStatus
            Active = 65 'A
            Inactive = 73 'I
        End Enum

        Public Enum EnumFeatureCode
            SPECIFIC_OPENHOUR
            RESULT_LIMIT_1ST_ENABLE
            RESULT_LIMIT_OVERRIDE_ENABLE
        End Enum
#End Region

#Region "Property"

        Private _strFunctionCode As String
        Public ReadOnly Property FunctionCode() As String
            Get
                Return _strFunctionCode
            End Get
        End Property

        Private _enumFeatureCode As EnumFeatureCode
        Public Property FeatureCode() As EnumFeatureCode
            Get
                Return _enumFeatureCode
            End Get
            Set(ByVal value As EnumFeatureCode)
                _enumFeatureCode = value
            End Set
        End Property

        Private _enumRecordStatus As EnumRecordStatus
        Public Property RecordStatus() As EnumRecordStatus
            Get
                Return _enumRecordStatus
            End Get
            Set(ByVal value As EnumRecordStatus)
                _enumRecordStatus = value
            End Set
        End Property

        Private _dtmCreateDtm As DateTime
        Public Property CreateDtm() As DateTime
            Get
                Return _dtmCreateDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmCreateDtm = value
            End Set
        End Property

        Private _strCreateBy As String
        Public Property CreateBy() As String
            Get
                Return _strCreateBy
            End Get
            Set(ByVal value As String)
                _strCreateBy = value
            End Set
        End Property

        Private _cllnFeatureOpenHours As FeatureOpenHourModelCollection = Nothing
        Public ReadOnly Property FeatureOpenHours() As FeatureOpenHourModelCollection
            Get
                If _cllnFeatureOpenHours Is Nothing Then _cllnFeatureOpenHours = FunctionInformationBLL.GetFeatureOpenHourModelCollection(Me.FeatureCode)

                Return _cllnFeatureOpenHours
            End Get
        End Property

        ''' <summary>
        ''' Check all opening hour under this feature
        ''' </summary>
        ''' <param name="dtmCheckTime"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsOpeningHour(Optional ByVal dtmCheckTime As DateTime = Nothing) As Boolean
            Get
                Select Case Me.RecordStatus
                    Case EnumRecordStatus.Active
                        ' Feature active and no open hour setting = always opening hour
                        If FeatureOpenHours Is Nothing OrElse FeatureOpenHours.Count = 0 Then Return True

                        ' Feature active and any open hour setting match
                        Return FeatureOpenHours.IsOpeningHour(dtmCheckTime)
                    Case EnumRecordStatus.Inactive
                        Return False
                End Select

            End Get
        End Property
#End Region

#Region "Constructor"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="drRaw">Raw datarow of FunctionFeature_FNFT</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal drRaw As DataRow)
            Fill(drRaw)
        End Sub

#End Region

#Region "Init By Raw Data"

        ''' <summary>
        ''' Fill data from raw datarow to property
        ''' </summary>
        ''' <param name="drRaw"></param>
        ''' <remarks></remarks>
        Private Sub Fill(ByVal drRaw As DataRow)
            Me._strFunctionCode = Me.ReadDataRowString(drRaw, DBTable.FNFT_Function_Code.Name)
            Me._enumFeatureCode = Me.ReadDataRowEnumName(Of EnumFeatureCode)(drRaw, DBTable.FNFT_Feature_Code.Name)
            Me._enumRecordStatus = Me.ReadDataRowEnumValue(Of EnumRecordStatus)(drRaw, DBTable.FNFT_Record_Status.Name)
            Me._dtmCreateDtm = Me.ReadDataRowDatetime(drRaw, DBTable.FNFT_Create_Dtm.Name)
            Me._strCreateBy = Me.ReadDataRowString(drRaw, DBTable.FNFT_Create_By.Name)

        End Sub
#End Region

    End Class
End Namespace

