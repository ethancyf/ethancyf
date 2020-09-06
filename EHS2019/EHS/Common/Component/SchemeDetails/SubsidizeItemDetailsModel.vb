Imports System.Data.SqlClient

Namespace Component.SchemeDetails
    <Serializable()> Public Class SubsidizeItemDetailsModel

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

        Public Class ItemCodeClass
            Public Const VACCINE = "VACCINE"
            Public Const EVOUCHER = "EVOUCHER"
            Public Const X_DOSE = "DOSE"
        End Class

        Public Class DoseCode
            Public Const VACCINE = "VACCINE"
            Public Const FirstDOSE = "1STDOSE"
            Public Const SecondDOSE = "2NDDOSE"
            Public Const ONLYDOSE = "ONLYDOSE"
            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Public Const ThirdDOSE = "3RDDOSE"
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        End Class

#Region "Schema"

        'Subsidize_Item_Code	char(10)	Unchecked
        'Display_Seq	smallint	Unchecked
        'Available_Item_Code	char(20)	Unchecked
        'Available_Item_Desc	varchar(100)	Unchecked
        'Available_Item_Desc_Chi	nvarchar(100)	Unchecked
        'Internal_Use   char(1) Unchecked
        'Create_By	varchar(20)	Unchecked
        'Create_Dtm	datetime	Unchecked
        'Update_By	varchar(20)	Unchecked
        'Update_Dtm	datetime	Unchecked
        'Record_Status	char(1)	Unchecked

#End Region

#Region "Private Member"

        Private _strSubsidize_Item_Code As String
        Private _intDisplay_Seq As Integer
        Private _strAvailable_Item_Code As String
        Private _strAvailable_Item_Desc As String
        Private _strAvailable_Item_Desc_Chi As String
        Private _strAvailable_Item_Desc_CN As String

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Private _intAvailable_Item_Num As Integer
        Private _strInternal_Use As String
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Private _strCreate_By As String
        Private _dtmCreate_Dtm As DateTime
        Private _strUpdate_By As String
        Private _dtmUpdate_Dtm As DateTime
        Private _strRecord_Status As String

#End Region

#Region "SQL Data Type"

        Public Const Subsidize_Item_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Subsidize_Item_Code_DataSize As Integer = 10

        Public Const Display_Seq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const Display_Seq_DataSize As Integer = 2

        Public Const Available_Item_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Available_Item_Code_DataSize As Integer = 20

        Public Const Available_Item_Desc_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Available_Item_Desc_DataSize As Integer = 100

        Public Const Available_Item_Desc_Chi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const Available_Item_Desc_Chi_DataSize As Integer = 100

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Const Available_Item_Num_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const Available_Item_Num_DataSize As Integer = 2

        Public Const Internal_Use_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Internal_Use_DataSize As Integer = 1
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Public Const Create_By_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Create_By_DataSize As Integer = 20

        Public Const Create_Dtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Create_Dtm_DataSize As Integer = 8

        Public Const Update_By_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Update_By_DataSize As Integer = 20

        Public Const Update_Dtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Update_Dtm_DataSize As Integer = 8

        Public Const Record_Status_DataType As SqlDbType = SqlDbType.Char
        Public Const Record_Status_DataSize As Integer = 1


#End Region

#Region "Property"

        Public Property SubsidizeItemCode() As String
            Get
                Return Me._strSubsidize_Item_Code
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Item_Code = value
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

        Public Property AvailableItemCode() As String
            Get
                Return Me._strAvailable_Item_Code
            End Get
            Set(ByVal value As String)
                Me._strAvailable_Item_Code = value
            End Set
        End Property

        Public Property AvailableItemDesc() As String
            Get
                Return Me._strAvailable_Item_Desc
            End Get
            Set(ByVal value As String)
                Me._strAvailable_Item_Desc = value
            End Set
        End Property

        Public Property AvailableItemDescChi() As String
            Get
                Return Me._strAvailable_Item_Desc_Chi
            End Get
            Set(ByVal value As String)
                Me._strAvailable_Item_Desc_Chi = value
            End Set
        End Property

        Public Property AvailableItemDescCN() As String
            Get
                Return Me._strAvailable_Item_Desc_CN
            End Get
            Set(ByVal value As String)
                Me._strAvailable_Item_Desc_CN = value
            End Set
        End Property

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Property AvailableItemNum() As Integer
            Get
                Return Me._intAvailable_Item_Num
            End Get
            Set(ByVal value As Integer)
                Me._intAvailable_Item_Num = value
            End Set
        End Property

        Public Property InternalUse() As Boolean
            Get
                If Me._strInternal_Use.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strInternal_Use = strYES
                Else
                    Me._strInternal_Use = strNO
                End If
            End Set
        End Property
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Public Property CreateBy() As String
            Get
                Return Me._strCreate_By
            End Get
            Set(ByVal value As String)
                Me._strCreate_By = value
            End Set
        End Property

        Public Property CreateDtm() As DateTime
            Get
                Return Me._dtmCreate_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmCreate_Dtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return Me._strUpdate_By
            End Get
            Set(ByVal value As String)
                Me._strUpdate_By = value
            End Set
        End Property

        Public Property UpdateDtm() As DateTime
            Get
                Return Me._dtmUpdate_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmUpdate_Dtm = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return Me._strRecord_Status
            End Get
            Set(ByVal value As String)
                Me._strRecord_Status = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSubsidizeItemDetailsModel As SubsidizeItemDetailsModel)

            Me._strSubsidize_Item_Code = udtSubsidizeItemDetailsModel._strSubsidize_Item_Code
            Me._intDisplay_Seq = udtSubsidizeItemDetailsModel._intDisplay_Seq
            Me._strAvailable_Item_Code = udtSubsidizeItemDetailsModel._strAvailable_Item_Code
            Me._strAvailable_Item_Desc = udtSubsidizeItemDetailsModel._strAvailable_Item_Desc
            Me._strAvailable_Item_Desc_Chi = udtSubsidizeItemDetailsModel._strAvailable_Item_Desc_Chi
            Me._strAvailable_Item_Desc_CN = udtSubsidizeItemDetailsModel._strAvailable_Item_Desc_CN

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            Me._intAvailable_Item_Num = udtSubsidizeItemDetailsModel._intAvailable_Item_Num
            Me._strInternal_Use = udtSubsidizeItemDetailsModel._strInternal_Use
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            Me._strCreate_By = udtSubsidizeItemDetailsModel._strCreate_By
            Me._dtmCreate_Dtm = udtSubsidizeItemDetailsModel._dtmCreate_Dtm
            Me._strUpdate_By = udtSubsidizeItemDetailsModel._strUpdate_By
            Me._dtmUpdate_Dtm = udtSubsidizeItemDetailsModel._dtmUpdate_Dtm
            Me._strRecord_Status = udtSubsidizeItemDetailsModel._strRecord_Status

        End Sub

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Sub New(ByVal strSubsidizeItemCode As String, ByVal intDisplaySeq As Integer, _
                        ByVal strAvailableItemCode As String, ByVal strAvailableItemDesc As String, ByVal strAvailableItemDescChi As String, _
                        ByVal strAvailableItemDescCN As String, ByVal intAvailableItemNum As Integer, ByVal strInternalUse As String, _
                        ByVal strCreateBy As String, ByVal dtmCreateDtm As String, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As String, _
                        ByVal strRecordStatus As String)

            Me._strSubsidize_Item_Code = strSubsidizeItemCode
            Me._intDisplay_Seq = intDisplaySeq
            Me._strAvailable_Item_Code = strAvailableItemCode
            Me._strAvailable_Item_Desc = strAvailableItemDesc
            Me._strAvailable_Item_Desc_Chi = strAvailableItemDescChi
            Me._strAvailable_Item_Desc_CN = strAvailableItemDescCN

            Me._intAvailable_Item_Num = intAvailableItemNum
            Me._strInternal_Use = strInternalUse

            Me._strCreate_By = strCreateBy
            Me._dtmCreate_Dtm = dtmCreateDtm
            Me._strUpdate_By = strUpdateBy
            Me._dtmUpdate_Dtm = dtmUpdateDtm
            Me._strRecord_Status = strRecordStatus

        End Sub
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

#End Region

    End Class
End Namespace
