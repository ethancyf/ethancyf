Imports System.Data.SqlClient

Namespace Component.Scheme
    <Serializable()> Public Class SchemeBackOfficeModel

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

#Region "Schema"
        'Scheme_Code	char(10)	Unchecked
        'Scheme_Seq	smallint	Unchecked
        'Scheme_Desc	varchar(100)	Checked
        'Scheme_Desc_Chi	nvarchar(100)	Checked
        'Display_Code	char(25)	Unchecked
        'Display_Seq	smallint	Unchecked
        'ReturnLogo_Enabled	char(1)	Checked
        'Eligible_Professional	varchar(100)	Checked
        'Effective_Dtm datetime
        'Expiry_Dtm datetime
        'Display_Subsidize_Desc	char(1)	Unchecked
        'Create_By	varchar(20)	Unchecked
        'Create_Dtm	datetime	Unchecked
        'Update_By	varchar(20)	Unchecked
        'Update_Dtm	datetime	Unchecked
        'Record_Status	char(1)	Unchecked

#End Region

#Region "Private Member"

        Private _strSchemeCode As String
        Private _intSchemeSeq As Integer
        Private _strSchemeDesc As String
        Private _strSchemeDescChi As String
        Private _strSchemeDescCN As String
        Private _strDisplayCode As String
        Private _intDisplaySeq As Integer
        Private _strReturnLogoEnabled As String
        Private _strEligibleProfesional As String
        Private _dtmEffectiveDtm As DateTime
        Private _dtmExpiryDtm As DateTime
        Private _strDisplaySubsidizeDesc As String
        Private _strCreateBy As String
        Private _dtmCreateDtm As DateTime
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As DateTime
        Private _strRecordStatus As String
        Private _strAllowFreeTextBankACNo As String
        Private _blnAllowNonClinicSetting As Boolean
        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private _strJoinPCDCompulsory As String
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
#End Region

#Region "SQL Data Type"
        Public Const SchemeCode_DataType As SqlDbType = SqlDbType.Char
        Public Const SchemeCode_DataSize As Integer = 10

        Public Const SchemeSeq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const SchemeSeq_DataSize As Integer = 2

        Public Const SchemeDesc_DataType As SqlDbType = SqlDbType.VarChar
        Public Const SchemeDesc_DataSize As Integer = 100

        Public Const SchemeDescChi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const SchemeDescChi_DataSize As Integer = 100

        Public Const DisplayCode_DataType As SqlDbType = SqlDbType.Char
        Public Const DisplayCode_DataSize As Integer = 25

        Public Const DisplaySeq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const DisplaySeq_DataSize As Integer = 2

        Public Const ReturnLogoEnabled_DataType As SqlDbType = SqlDbType.Char
        Public Const ReturnLogoEnabled_DataSize As Integer = 1

        Public Const EligibleProfessional_DataType As SqlDbType = SqlDbType.VarChar
        Public Const EligibleProfessional_DataSize As Integer = 100

        Public Const EffectiveDtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const EffectiveDtm_DataSize As Integer = 8

        Public Const ExpiryDtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const ExpiryDtm_DataSize As Integer = 8

        Public Const DisplaySubsidizeDesc_DataType As SqlDbType = SqlDbType.Char
        Public Const DisplaySubsidizeDesc_DataSize As Integer = 1

        Public Const CreateBy_DataType As SqlDbType = SqlDbType.VarChar
        Public Const CreateBy_DataSize As Integer = 20

        Public Const CreateDtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const CreateDtm_DataSize As Integer = 8

        Public Const UpdateBy_DataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdateBy_DataSize As Integer = 20

        Public Const UpdateDtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const UpdateDtm_DataSize As Integer = 8

        Public Const RecordStatus_DataType As SqlDbType = SqlDbType.Char
        Public Const RecordStatus_DataSize As Integer = 1

        Public Const AllowFreeTextBankACNo_DataType As SqlDbType = SqlDbType.Char
        Public Const AllowFreeTextBankACNo_DataSize As Integer = 1
#End Region

#Region "Property"

        Public Property SchemeCode() As String
            Get
                Return Me._strSchemeCode
            End Get
            Set(ByVal value As String)
                Me._strSchemeCode = value
            End Set
        End Property

        Public Property SchemeSeq() As Integer
            Get
                Return Me._intSchemeSeq
            End Get
            Set(ByVal value As Integer)
                Me._intSchemeSeq = value
            End Set
        End Property

        Public Property SchemeDesc() As String
            Get
                Return Me._strSchemeDesc
            End Get
            Set(ByVal value As String)
                Me._strSchemeDesc = value
            End Set
        End Property

        Public Property SchemeDescChi() As String
            Get
                Return Me._strSchemeDescChi
            End Get
            Set(ByVal value As String)
                Me._strSchemeDescChi = value
            End Set
        End Property

        Public Property SchemeDescCN() As String
            Get
                Return Me._strSchemeDescCN
            End Get
            Set(ByVal value As String)
                Me._strSchemeDescCN = value
            End Set
        End Property

        Public ReadOnly Property SchemeDesc(ByVal strLanguage As String) As String
            Get
                Select Case strLanguage
                    Case CultureLanguage.English
                        Return Me.SchemeDesc
                    Case CultureLanguage.TradChinese
                        Return Me.SchemeDescChi
                    Case CultureLanguage.SimpChinese
                        Return Me.SchemeDescCN
                    Case Else
                        Throw New Exception(String.Format("SchemeBackOfficeModel.SchemeDesc: Unexpected value (strLanguage={0})", strLanguage))
                End Select
            End Get
        End Property

        Public Property DisplayCode() As String
            Get
                Return Me._strDisplayCode
            End Get
            Set(ByVal value As String)
                Me._strDisplayCode = value
            End Set
        End Property

        Public Property DisplaySeq() As Integer
            Get
                Return Me._intDisplaySeq
            End Get
            Set(ByVal value As Integer)
                Me._intDisplaySeq = value
            End Set
        End Property


        Public Property ReturnLogoEnabled() As Boolean
            Get
                If Me._strReturnLogoEnabled.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strReturnLogoEnabled = strYES
                Else
                    Me._strReturnLogoEnabled = strNO
                End If
            End Set
        End Property

        Public ReadOnly Property EligibleProfesional(ByVal strProf As String) As Boolean
            Get
                Dim blnRes As Boolean = False
                If EligibleProfesionalString.Trim.Equals(ServiceCategoryCode.ALL) Then
                    blnRes = True
                Else
                    Dim strProfList As String()
                    strProfList = EligibleProfesionalString.Split(",")

                    For i As Integer = 0 To strProfList.Length - 1
                        If strProfList(i).Trim.Equals(strProf.Trim) Then
                            blnRes = True
                        End If
                    Next
                End If

                Return blnRes
            End Get

        End Property

        Public Property EligibleProfesionalString() As String
            Get
                Return Me._strEligibleProfesional
            End Get
            Set(ByVal value As String)
                Me._strEligibleProfesional = value
            End Set
        End Property

        Public Property EffectiveDtm() As DateTime
            Get
                Return Me._dtmEffectiveDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmEffectiveDtm = value
            End Set
        End Property

        Public Property ExpiryDtm() As DateTime
            Get
                Return Me._dtmExpiryDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmExpiryDtm = value
            End Set
        End Property

        Public Property DisplaySubsidizeDesc() As Boolean
            Get
                If Me._strDisplaySubsidizeDesc.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strDisplaySubsidizeDesc = strYES
                Else
                    Me._strDisplaySubsidizeDesc = strNO
                End If
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return Me._strCreateBy
            End Get
            Set(ByVal value As String)
                Me._strCreateBy = value
            End Set
        End Property

        Public Property CreateDtm() As DateTime
            Get
                Return Me._dtmCreateDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmCreateDtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return Me._strUpdateBy
            End Get
            Set(ByVal value As String)
                Me._strUpdateBy = value
            End Set
        End Property

        Public Property UpdateDtm() As DateTime
            Get
                Return Me._dtmUpdateDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmUpdateDtm = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return Me._strRecordStatus
            End Get
            Set(ByVal value As String)
                Me._strRecordStatus = value
            End Set
        End Property

        Public Property AllowFreeTextBankACNo() As String
            Get
                Return Me._strAllowFreeTextBankACNo
            End Get
            Set(ByVal value As String)
                Me._strAllowFreeTextBankACNo = value
            End Set
        End Property

        Public Property AllowNonClinicSetting() As Boolean
            Get
                Return _blnAllowNonClinicSetting
            End Get
            Set(ByVal value As Boolean)
                _blnAllowNonClinicSetting = value
            End Set
        End Property

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Property JoinPCDCompulsory() As String
            Get
                Return Me._strJoinPCDCompulsory
            End Get
            Set(ByVal value As String)
                Me._strJoinPCDCompulsory = value
            End Set
        End Property
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

#End Region

#Region "Constructor"
        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSchemeBackOfficeModel As SchemeBackOfficeModel)
            With udtSchemeBackOfficeModel
                _strSchemeCode = .SchemeCode
                _intSchemeSeq = .SchemeSeq
                _strSchemeDesc = .SchemeDesc
                _strSchemeDescChi = .SchemeDescChi
                _strSchemeDescCN = .SchemeDescCN
                _strDisplayCode = .DisplayCode
                _intDisplaySeq = .DisplaySeq
                ReturnLogoEnabled = .ReturnLogoEnabled
                _strEligibleProfesional = .EligibleProfesionalString
                _dtmEffectiveDtm = .EffectiveDtm
                _dtmExpiryDtm = .ExpiryDtm
                DisplaySubsidizeDesc = .DisplaySubsidizeDesc
                _strCreateBy = .CreateBy
                _dtmCreateDtm = .CreateDtm
                _strUpdateBy = .UpdateBy
                _dtmUpdateDtm = .UpdateDtm
                _strRecordStatus = .RecordStatus
                _strAllowFreeTextBankACNo = .AllowFreeTextBankACNo
                _blnAllowNonClinicSetting = .AllowNonClinicSetting
                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                _strJoinPCDCompulsory = .JoinPCDCompulsory
                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
            End With
        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSchemeDesc As String, ByVal strSchemeDescChi As String, ByVal strSchemeDescCN As String, _
                        ByVal strDisplayCode As String, ByVal intDisplaySeq As Integer, ByVal strReturnLogoEnabled As String, ByVal strEligibleProfesional As String, _
                        ByVal dtmEffectiveDtm As DateTime, ByVal dtmExpiryDtm As DateTime, ByVal strDisplaySubsidizeDesc As String, ByVal strCreateBy As String, _
                        ByVal dtmCreateDtm As DateTime, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal strRecordStatus As String, ByVal strAllowFreeTextBankACNo As String, _
                        ByVal blnAllowNonClinicSetting As Boolean, ByVal strJoinPCDCompulsory As String)
            _strSchemeCode = strSchemeCode
            _intSchemeSeq = intSchemeSeq
            _strSchemeDesc = strSchemeDesc
            _strSchemeDescChi = strSchemeDescChi
            _strSchemeDescCN = strSchemeDescCN
            _strDisplayCode = strDisplayCode
            _intDisplaySeq = intDisplaySeq
            _strReturnLogoEnabled = strReturnLogoEnabled
            _strEligibleProfesional = strEligibleProfesional
            _dtmEffectiveDtm = dtmEffectiveDtm
            _dtmExpiryDtm = dtmExpiryDtm
            _strDisplaySubsidizeDesc = strDisplaySubsidizeDesc
            _strCreateBy = strCreateBy
            _dtmCreateDtm = dtmCreateDtm
            _strUpdateBy = strUpdateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strRecordStatus = strRecordStatus
            _strAllowFreeTextBankACNo = strAllowFreeTextBankACNo
            _blnAllowNonClinicSetting = blnAllowNonClinicSetting
            _strAllowFreeTextBankACNo = strAllowFreeTextBankACNo
            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            _strJoinPCDCompulsory = strJoinPCDCompulsory
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
        End Sub
#End Region

#Region "Addition Memeber"
        Private _udtSubsidizeGroupBackOfficeModelList As SubsidizeGroupBackOfficeModelCollection

        Public Property SubsidizeGroupBackOfficeList() As SubsidizeGroupBackOfficeModelCollection
            Get
                Return Me._udtSubsidizeGroupBackOfficeModelList
            End Get
            Set(ByVal value As SubsidizeGroupBackOfficeModelCollection)
                Me._udtSubsidizeGroupBackOfficeModelList = value
            End Set
        End Property
#End Region

    End Class
End Namespace

