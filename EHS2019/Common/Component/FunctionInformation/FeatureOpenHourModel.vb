Imports Common.ComFunction

Namespace Component.FunctionInformation

    <Serializable()> Public Class FeatureOpenHourModel
        Inherits BaseModel

#Region "DB Table Schema - [FeatureOpenHour_FTOH]"
        Public Class DBTable

            '[FTOH_Feature_Code] [char](50) NOT NULL,
            '[FTOH_From_Time] [varchar](5) NOT NULL,
            '[FTOH_To_Time] [varchar](5)  NOT NULL,
            '[FTOH_Record_Status] [char](1) NOT NULL,
            '[FTOH_Create_Dtm] [datetime] NOT NULL,
            '[FTOH_Create_By] [varchar](20) NOT NULL,

            Public Const TableName As String = "FeatureOpenHour_FTOH"

            Public Class FTOH_Feature_Code
                Public Const Name As String = "FTOH_Feature_Code"
                Public Const DataType As SqlDbType = SqlDbType.VarChar
                Public Const DataSize As Integer = 50
            End Class

            Public Class FTOH_From_Time
                Public Const Name As String = "FTOH_From_Time"
                Public Const DataType As SqlDbType = SqlDbType.VarChar
                Public Const DataSize As Integer = 5
            End Class

            Public Class FTOH_To_Time
                Public Const Name As String = "FTOH_To_Time"
                Public Const DataType As SqlDbType = SqlDbType.VarChar
                Public Const DataSize As Integer = 5
            End Class

            Public Class FTOH_Record_Status
                Public Const Name As String = "FTOH_Record_Status"
                Public Const DataType As SqlDbType = SqlDbType.Char
                Public Const DataSize As Integer = 1
            End Class

            Public Class FTOH_Create_Dtm
                Public Const Name As String = "FTOH_Create_Dtm"
                Public Const DataType As SqlDbType = SqlDbType.DateTime
                Public Const DataSize As Integer = 8
            End Class


            Public Class FTOH_Create_By
                Public Const Name As String = "FTOH_Create_By"
                Public Const DataType As SqlDbType = SqlDbType.VarChar
                Public Const DataSize As Integer = 20
            End Class

        End Class
#End Region

#Region "[Enum_Class] of DB Table - [StatusData]"
        ' To map the display text of the Field - [FunctionFeature_FNFT]
        'Public Const STATUS_DATA_CLASS As String = "FunctionFeatureStatus"

#End Region


#Region "Property"

        Private _enumFeatureCode As FunctionFeatureModel.EnumFeatureCode
        Public Property FeatureCode() As FunctionFeatureModel.EnumFeatureCode
            Get
                Return _enumFeatureCode
            End Get
            Set(ByVal value As FunctionFeatureModel.EnumFeatureCode)
                _enumFeatureCode = value
            End Set
        End Property

        Private _strFromTime As String
        Public Property FromTime() As String
            Get
                Return _strFromTime
            End Get
            Set(ByVal value As String)
                _strFromTime = value
            End Set
        End Property

        Private _strToTime As String
        Public Property ToTime() As String
            Get
                Return _strToTime
            End Get
            Set(ByVal value As String)
                _strToTime = value
            End Set
        End Property

        Private _enumRecordStatus As FunctionFeatureModel.EnumRecordStatus
        Public Property RecordStatus() As FunctionFeatureModel.EnumRecordStatus
            Get
                Return _enumRecordStatus
            End Get
            Set(ByVal value As FunctionFeatureModel.EnumRecordStatus)
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

#End Region

        Public Sub New(ByVal drRaw As DataRow)
            Fill(drRaw)
        End Sub

        ''' <summary>
        ''' Fill data from raw datarow to property
        ''' </summary>
        ''' <param name="drRaw"></param>
        ''' <remarks></remarks>
        Private Sub Fill(ByVal drRaw As DataRow)
            Me._enumFeatureCode = Me.ReadDataRowEnumName(Of FunctionFeatureModel.EnumFeatureCode)(drRaw, DBTable.FTOH_Feature_Code.Name)
            Me._strFromTime = Me.ReadDataRowString(drRaw, DBTable.FTOH_From_Time.Name)
            Me._strToTime = Me.ReadDataRowString(drRaw, DBTable.FTOH_To_Time.Name)
            Me._enumRecordStatus = Me.ReadDataRowEnumValue(Of FunctionFeatureModel.EnumRecordStatus)(drRaw, DBTable.FTOH_Record_Status.Name)
            Me._dtmCreateDtm = Me.ReadDataRowDatetime(drRaw, DBTable.FTOH_Create_Dtm.Name)
            Me._strCreateBy = Me.ReadDataRowString(drRaw, DBTable.FTOH_Create_Dtm.Name)
        End Sub

        ''' <summary>
        ''' Check is this model setting meet the opening hour
        ''' </summary>
        ''' <param name="dtmCheckTime"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsOpeningHour(Optional ByVal dtmCheckTime As DateTime = Nothing) As Boolean
            Get
                Select Case RecordStatus
                    Case FunctionFeatureModel.EnumRecordStatus.Active
                        Dim dtmNow As DateTime = dtmCheckTime

                        ' Use system datetime if not specified
                        If dtmNow = Nothing Then
                            Dim udtGeneralFunction As New GeneralFunction
                            dtmNow = udtGeneralFunction.GetSystemDateTime()
                        End If

                        ' From & To time is empty
                        If FromTime = String.Empty And ToTime = String.Empty Then
                            Return False
                        End If

                        If FromTime < ToTime Then ' 04:00 - 17:00
                            If dtmNow.ToString("HH:mm") >= FromTime AndAlso _
                               dtmNow.ToString("HH:mm") <= ToTime Then
                                Return True
                            Else
                                Return False
                            End If
                        ElseIf FromTime > ToTime Then ' 17:00 - 04:00
                            If dtmNow.ToString("HH:mm") >= FromTime OrElse _
                               dtmNow.ToString("HH:mm") <= ToTime Then
                                Return True
                            Else
                                Return False
                            End If
                        ElseIf FromTime = ToTime Then ' 04:00 - 04:00
                            Return True
                        Else
                            Return False
                        End If
                    Case FunctionFeatureModel.EnumRecordStatus.Inactive
                        Return False
                End Select
            End Get
        End Property

    End Class
End Namespace

