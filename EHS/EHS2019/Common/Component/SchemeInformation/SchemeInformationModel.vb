
' ----- Model Structure Change 07 May 2009 -----
' 1  Remove Service Fee From
' 2  Remove Service Fee To
' 3  Add Record Status
' 3  Add Delist Status
' 4  Add Delist Dtm
' 5  Add Effective Dtm
' 6  Add Logo return dtm
' 7  Add Remark
' ----- End 07 May 2009 ------------------------

Namespace Component.SchemeInformation
    <Serializable()> Public Class SchemeInformationModel

        Private _strEnrolRefNo As String
        Private _strSPID As String
        Private _strSchemeCode As String
        Private _strRecordStatus As String
        Private _strRemark As String
        Private _strDelistStatus As String
        Private _dtmDelistDtm As Nullable(Of DateTime)
        Private _dtmEffectiveDtm As Nullable(Of DateTime)
        Private _dtmLogoReturnDtm As Nullable(Of DateTime)
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strCreateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _byteTSMP As Byte()
        Private _intSchemeDisplaySeq As Integer

        Public Const SchemeCodeDataType As SqlDbType = SqlDbType.Char
        Public Const SchemeCodemDataSize As Integer = 10

        Public Const RecordStatusDataType As SqlDbType = SqlDbType.Char
        Public Const RecordStatusDataSize As Integer = 1

        Public Const RemarkDataType As SqlDbType = SqlDbType.NVarChar
        Public Const RemarkDataSize As Integer = 255

        Public Const DelistStatusDataType As SqlDbType = SqlDbType.Char
        Public Const DelistStatusDataSize As Integer = 1

        Public Const LogoReturnDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const LogoReturnDtmDatasize As Integer = 8

        Public Const DelistDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const DelistDtmDatasize As Integer = 8

        Public Const EffectiveDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const EffectiveDtmDataSize As Integer = 8

        Public Const CreateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const CreateByDataSize As Integer = 20

        Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdateByDataSize As Integer = 20

        Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
        Public Const TSMPDataSize As Integer = 8

        Public Property EnrolRefNo() As String
            Get
                Return _strEnrolRefNo
            End Get
            Set(ByVal value As String)
                _strEnrolRefNo = value
            End Set
        End Property

        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value
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

        Public Property RecordStatus() As String
            Get
                Return _strRecordStatus
            End Get
            Set(ByVal value As String)
                _strRecordStatus = value
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

        Public Property DelistStatus() As String
            Get
                Return _strDelistStatus
            End Get
            Set(ByVal value As String)
                _strDelistStatus = value
            End Set
        End Property

        Public Property LogoReturnDtm() As Nullable(Of DateTime)
            Get
                Return _dtmLogoReturnDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmLogoReturnDtm = value
            End Set
        End Property

        Public Property DelistDtm() As Nullable(Of DateTime)
            Get
                Return _dtmDelistDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmDelistDtm = value
            End Set
        End Property

        Public Property EffectiveDtm() As Nullable(Of DateTime)
            Get
                Return _dtmEffectiveDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmEffectiveDtm = value
            End Set
        End Property

        Public Property CreateDtm() As Nullable(Of DateTime)
            Get
                Return _dtmCreateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmCreateDtm = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return _strCreateBy
            End Get
            Set(ByVal value As String)
                _strCreateBy = value
            End Set
        End Property

        Public Property UpdateDtm() As Nullable(Of DateTime)
            Get
                Return _dtmUpdateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmUpdateDtm = value
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

        Public Property TSMP() As Byte()
            Get
                Return _byteTSMP
            End Get
            Set(ByVal value As Byte())
                _byteTSMP = value
            End Set
        End Property

        Public Property SchemeDisplaySeq() As Integer
            Get
                Return _intSchemeDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intSchemeDisplaySeq = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtSchemeInformation As SchemeInformationModel)
            _strEnrolRefNo = udtSchemeInformation.EnrolRefNo
            _strSPID = udtSchemeInformation.SPID
            _strSchemeCode = udtSchemeInformation.SchemeCode
            _strRecordStatus = udtSchemeInformation.RecordStatus
            _strRemark = udtSchemeInformation.Remark
            _strDelistStatus = udtSchemeInformation.DelistStatus
            _dtmDelistDtm = udtSchemeInformation.DelistDtm
            _dtmEffectiveDtm = udtSchemeInformation.EffectiveDtm
            _dtmLogoReturnDtm = udtSchemeInformation.LogoReturnDtm
            _dtmCreateDtm = udtSchemeInformation.CreateDtm
            _strCreateBy = udtSchemeInformation.CreateBy
            _dtmUpdateDtm = udtSchemeInformation.UpdateDtm
            _strUpdateBy = udtSchemeInformation.UpdateBy
            _byteTSMP = udtSchemeInformation.TSMP
            _intSchemeDisplaySeq = udtSchemeInformation.SchemeDisplaySeq
        End Sub

        Public Sub New(ByVal strEnrolRefNo As String, ByVal strSPID As String, ByVal strSchemeCode As String, _
                       ByVal strRecordStatus As String, ByVal strRemark As String, ByVal strDelistStatus As String, _
                       ByVal dtmDelistDtm As Nullable(Of DateTime), ByVal dtmEffectiveDtm As Nullable(Of DateTime), _
                       ByVal dtmLogoReturnDtm As Nullable(Of DateTime), _
                       ByVal dtmCreateDtm As Nullable(Of DateTime), ByVal strCreateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), _
                       ByVal strUpdateBy As String, ByVal byteTSMP As Byte(), ByVal intSchemeDisplaySeq As Integer)
            _strEnrolRefNo = strEnrolRefNo
            _strSPID = strSPID
            _strSchemeCode = strSchemeCode
            _strRecordStatus = strRecordStatus
            _strRemark = strRemark
            _strDelistStatus = strDelistStatus
            _dtmDelistDtm = dtmDelistDtm
            _dtmEffectiveDtm = dtmEffectiveDtm
            _dtmLogoReturnDtm = dtmLogoReturnDtm
            _dtmCreateDtm = dtmCreateDtm
            _strCreateBy = strCreateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strUpdateBy = strUpdateBy
            _byteTSMP = byteTSMP
            _intSchemeDisplaySeq = intSchemeDisplaySeq
        End Sub

    End Class
End Namespace
