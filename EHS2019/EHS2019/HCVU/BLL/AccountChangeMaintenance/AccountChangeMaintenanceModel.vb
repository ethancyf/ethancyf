Namespace AccountChangeMaintenance
    <Serializable()> Public Class AccountChangeMaintenanceModel

        Private _strSPID As String
        Private _strUpdType As String
        Private _strSystemDtm As Nullable(Of DateTime)
        Private _strRemark As String
        Private _strTokenSerialNo As String
        Private _strTokenRemark As String
        Private _strSPPracticeDisplaySeq As Integer
        Private _strDelistStatus As String
        Private _strUpdateBy As String
        Private _strConfirmedBy As String
        Private _strConfirmDtm As Nullable(Of DateTime)
        Private _strRecordStatus As String
        Private _strSchemeCode As String
        Private _byteTSMP As Byte()
        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Private _strDataInputBy As String
        ' INT13-0028 - SP Amendment Report [End][Tommy L]
        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Private _strProject As String
        Private _blnIsShareToken As Nullable(Of Boolean)
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private _strTokenSerialNoReplacement As String
        Private _strProjectReplacement As String
        Private _blnIsShareTokenReplacement As Nullable(Of Boolean)
        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]


        Public Const SPIDDataType As SqlDbType = SqlDbType.Char
        Public Const SPIDDataSize As Integer = 8

        Public Const UpdTypeDataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdTypeDataSize As Integer = 2

        Public Const SystemDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const SystemDtmDataSize As Integer = 8

        Public Const RemarkDataType As SqlDbType = SqlDbType.NVarChar
        Public Const RemarkDataSize As Integer = 255

        Public Const TokenSerialNoDataType As SqlDbType = SqlDbType.VarChar
        Public Const TokenSerialNoDataSize As Integer = 20

        Public Const TokenRemarkDataType As SqlDbType = SqlDbType.Char
        Public Const TokenRemarkDataSize As Integer = 5

        Public Const SPPracticeDisplaySeqDataType As SqlDbType = SqlDbType.Int
        Public Const SPPracticeDisplaySeqDataSize As Integer = 3

        Public Const DelistStatusDataType As SqlDbType = SqlDbType.Char
        Public Const DelistStatusDataSize As Integer = 1

        Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdateByDataSize As Integer = 20

        Public Const ConfirmedByDataType As SqlDbType = SqlDbType.VarChar
        Public Const ConfirmedByDataSize As Integer = 20

        Public Const ConfirmDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const ConfirmDtmDataSize As Integer = 8

        Public Const RecordStatusDataType As SqlDbType = SqlDbType.Char
        Public Const RecordStatusDataSize As Integer = 1

        Public Const SchemeCodeDataType As SqlDbType = SqlDbType.Char
        Public Const SchemeCodeDataSize As Integer = 10

        Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
        Public Const TSMPDataSize As Integer = 8

        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Public Const DataInputByDataType As SqlDbType = SqlDbType.VarChar
        Public Const DataInputByDataSize As Integer = 20
        ' INT13-0028 - SP Amendment Report [End][Tommy L]

        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Public Const ProjectDataType As SqlDbType = SqlDbType.Char
        Public Const ProjectDataSize As Integer = 10

        Public Const IsShareTokenDataType As SqlDbType = SqlDbType.Char
        Public Const IsShareTokenDataSize As Integer = 1
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Const TokenSerialNoReplacementDataType As SqlDbType = SqlDbType.VarChar
        Public Const TokenSerialNoReplacementDataSize As Integer = 20

        Public Const ProjectReplacementDataType As SqlDbType = SqlDbType.Char
        Public Const ProjectReplacementDataSize As Integer = 10

        Public Const IsShareTokenReplacementDataType As SqlDbType = SqlDbType.Char
        Public Const IsShareTokenReplacementDataSize As Integer = 1
        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value
            End Set
        End Property

        Public Property UpdType() As String
            Get
                Return _strUpdType
            End Get
            Set(ByVal value As String)
                _strUpdType = value
            End Set
        End Property

        Public Property SystemDtm() As Nullable(Of DateTime)
            Get
                Return _strSystemDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _strSystemDtm = value
            End Set
        End Property

        Public Property Remark() As String
            Get
                Return _strRemark
            End Get
            Set(ByVal value As String)
                _strRemark = value
            End Set
        End Property

        Public Property TokenSerialNo() As String
            Get
                Return _strTokenSerialNo
            End Get
            Set(ByVal value As String)
                _strTokenSerialNo = value
            End Set
        End Property

        Public Property TokenRemark() As String
            Get
                Return _strTokenRemark
            End Get
            Set(ByVal value As String)
                _strTokenRemark = value
            End Set
        End Property

        Public Property SPPracticeDisplaySeq() As Integer
            Get
                Return _strSPPracticeDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _strSPPracticeDisplaySeq = value
            End Set
        End Property

        Public Property DelistStatus() As String
            Get
                Return _strDelistStatus
            End Get
            Set(ByVal value As String)
                _strDelistStatus = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return _strUpdateBy
            End Get
            Set(ByVal value As String)
                _strUpdateBy = value
            End Set
        End Property

        Public Property ConfirmedBy() As String
            Get
                Return _strConfirmedBy
            End Get
            Set(ByVal value As String)
                _strConfirmedBy = value
            End Set
        End Property

        Public Property ConfirmDtm() As Nullable(Of DateTime)
            Get
                Return _strConfirmDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _strConfirmDtm = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return _strRecordStatus
            End Get
            Set(ByVal value As String)
                _strRecordStatus = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
            Set(ByVal value As String)
                _strSchemeCode = value
            End Set
        End Property

        Public Property TSMP() As Byte()
            Get
                Return _byteTSMP
            End Get
            Set(ByVal value As Byte())
                _byteTSMP = value
            End Set
        End Property

        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Public Property DataInputBy() As String
            Get
                Return _strDataInputBy
            End Get
            Set(ByVal value As String)
                _strDataInputBy = value
            End Set
        End Property
        ' INT13-0028 - SP Amendment Report [End][Tommy L]

        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Public Property Project() As String
            Get
                Return _strProject
            End Get
            Set(ByVal value As String)
                _strProject = value
            End Set
        End Property

        Public Property IsShareToken() As Nullable(Of Boolean)
            Get
                Return _blnIsShareToken
            End Get
            Set(ByVal value As Nullable(Of Boolean))
                _blnIsShareToken = value
            End Set
        End Property
        ' CRE14-002 - PPI-ePR Migration [End][Tommy L]

        'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Property TokenSerialNoReplacement() As String
            Get
                Return _strTokenSerialNoReplacement
            End Get
            Set(ByVal value As String)
                _strTokenSerialNoReplacement = value
            End Set
        End Property

        Public Property ProjectReplacement() As String
            Get
                Return _strProjectReplacement
            End Get
            Set(ByVal value As String)
                _strProjectReplacement = value
            End Set
        End Property

        Public Property IsShareTokenReplacement() As Nullable(Of Boolean)
            Get
                Return _blnIsShareTokenReplacement
            End Get
            Set(ByVal value As Nullable(Of Boolean))
                _blnIsShareTokenReplacement = value
            End Set
        End Property
        'CRE14-002 - PPI-ePR Migration [End][Chris YIM]


        Public Sub New()

        End Sub

        Public Sub New(ByVal udtAccountChangeMaintenanceModel As AccountChangeMaintenanceModel)
            _strSPID = udtAccountChangeMaintenanceModel.SPID
            _strUpdType = udtAccountChangeMaintenanceModel.UpdType
            _strSystemDtm = udtAccountChangeMaintenanceModel.SystemDtm
            _strRemark = udtAccountChangeMaintenanceModel.Remark
            _strTokenSerialNo = udtAccountChangeMaintenanceModel.TokenSerialNo
            _strTokenRemark = udtAccountChangeMaintenanceModel.TokenRemark
            _strSPPracticeDisplaySeq = udtAccountChangeMaintenanceModel.SPPracticeDisplaySeq
            _strDelistStatus = udtAccountChangeMaintenanceModel.DelistStatus
            _strUpdateBy = udtAccountChangeMaintenanceModel.UpdateBy
            _strConfirmedBy = udtAccountChangeMaintenanceModel.ConfirmedBy
            _strConfirmDtm = udtAccountChangeMaintenanceModel.ConfirmDtm
            _strRecordStatus = udtAccountChangeMaintenanceModel.RecordStatus
            _strSchemeCode = udtAccountChangeMaintenanceModel.SchemeCode
            _byteTSMP = udtAccountChangeMaintenanceModel.TSMP
            ' INT13-0028 - SP Amendment Report [Start][Tommy L]
            ' -------------------------------------------------------------------------
            _strDataInputBy = udtAccountChangeMaintenanceModel.DataInputBy
            ' INT13-0028 - SP Amendment Report [End][Tommy L]
            ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            _strProject = udtAccountChangeMaintenanceModel.Project
            _blnIsShareToken = udtAccountChangeMaintenanceModel.IsShareToken
            ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _strTokenSerialNoReplacement = udtAccountChangeMaintenanceModel.TokenSerialNoReplacement
            _strProjectReplacement = udtAccountChangeMaintenanceModel.ProjectReplacement
            _blnIsShareTokenReplacement = udtAccountChangeMaintenanceModel.IsShareTokenReplacement
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

        End Sub

        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Public Sub New(ByVal strSPID As String, ByVal strUpdType As String, ByVal strSystemDtm As Nullable(Of DateTime), _
        '                ByVal strRemark As String, ByVal strTokenSerialNo As String, ByVal strTokenRemark As String, _
        '                ByVal intSPPracticeDisplaySeq As Integer, ByVal strDelistStatus As String, ByVal strUpdateBy As String, ByVal strConfirmedBy As String, _
        '                ByVal strConfirmDtm As Nullable(Of DateTime), ByVal strRecordStatus As String, ByVal strSchemeCode As String, ByVal byteTSMP As Byte())
        ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Public Sub New(ByVal strSPID As String, ByVal strUpdType As String, ByVal strSystemDtm As Nullable(Of DateTime), _
        '        ByVal strRemark As String, ByVal strTokenSerialNo As String, ByVal strTokenRemark As String, _
        '        ByVal intSPPracticeDisplaySeq As Integer, ByVal strDelistStatus As String, ByVal strUpdateBy As String, ByVal strConfirmedBy As String, _
        '        ByVal strConfirmDtm As Nullable(Of DateTime), ByVal strRecordStatus As String, ByVal strSchemeCode As String, ByVal byteTSMP As Byte(), _
        '        ByVal strDataInputBy As String)
        Public Sub New(ByVal strSPID As String, ByVal strUpdType As String, ByVal strSystemDtm As Nullable(Of DateTime), _
                       ByVal strRemark As String, ByVal strTokenSerialNo As String, ByVal strTokenRemark As String, _
                       ByVal intSPPracticeDisplaySeq As Integer, ByVal strDelistStatus As String, ByVal strUpdateBy As String, ByVal strConfirmedBy As String, _
                       ByVal strConfirmDtm As Nullable(Of DateTime), ByVal strRecordStatus As String, ByVal strSchemeCode As String, ByVal byteTSMP As Byte(), _
                       ByVal strDataInputBy As String, ByVal strProject As String, ByVal blnIsShareToken As Nullable(Of Boolean), _
                       ByVal strTokenSerialNoReplacement As String, ByVal strProjectReplacement As String, ByVal blnIsShareTokenReplacement As Nullable(Of Boolean))
            ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
            ' INT13-0028 - SP Amendment Report [End][Tommy L]

            _strSPID = strSPID
            _strUpdType = strUpdType
            _strSystemDtm = strSystemDtm
            _strRemark = strRemark
            _strTokenSerialNo = strTokenSerialNo
            _strTokenRemark = strTokenRemark
            _strSPPracticeDisplaySeq = intSPPracticeDisplaySeq
            _strDelistStatus = strDelistStatus
            _strUpdateBy = strUpdateBy
            _strConfirmedBy = strConfirmedBy
            _strConfirmDtm = strConfirmDtm
            _strRecordStatus = strRecordStatus
            _strSchemeCode = strSchemeCode
            _byteTSMP = byteTSMP
            ' INT13-0028 - SP Amendment Report [Start][Tommy L]
            ' -------------------------------------------------------------------------
            _strDataInputBy = strDataInputBy
            ' INT13-0028 - SP Amendment Report [End][Tommy L]
            ' CRE14-002 - PPI-ePR Migration [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            _strProject = strProject
            _blnIsShareToken = blnIsShareToken
            ' CRE14-002 - PPI-ePR Migration [End][Tommy L]
            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _strTokenSerialNoReplacement = strTokenSerialNoReplacement
            _strProjectReplacement = strProjectReplacement
            _blnIsShareTokenReplacement = blnIsShareTokenReplacement
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
        End Sub

    End Class
End Namespace
