Imports System.Data.SqlClient

Namespace Component.Scheme
    <Serializable()> Public Class SchemeEFormModel

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

#Region "Schema"
        'Scheme_Code	char(10)	Unchecked
        'Scheme_Seq	smallint	Unchecked
        'Scheme_Desc	varchar(100)	Checked
        'Scheme_Desc_Chi	nvarchar(100)	Checked
        'Display_Code	char(25)	Unchecked
        'Display_Seq	smallint	Unchecked
        'Service_Fee_Enabled	char(1)	Checked
        'Eligible_Professional	varchar(100)	Checked
        'Display_Subsidize_Desc	char(1)	Unchecked
        'Enrol_Period_From	datetime	Unchecked
        'Enrol_Period_To	datetime	Unchecked
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
        Private _strDisplayCode As String
        Private _intDisplaySeq As Integer
        Private _strServiceFeeEnabled As String
        Private _strEligibleProfesional As String
        Private _strDisplaySubsidizeDesc As String
        Private _dtmEnrolPeriodFrom As DateTime
        Private _dtmEnrolPeriodTo As DateTime
        Private _strCreateBy As String
        Private _dtmCreateDtm As DateTime
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As DateTime
        Private _strRecordStatus As String
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private _strAllowNonClinicSetting As String
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

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

        Public Const ServiceFeeEnabled_DataType As SqlDbType = SqlDbType.Char
        Public Const ServiceFeeEnabled_DataSize As Integer = 1

        Public Const EligibleProfessional_DataType As SqlDbType = SqlDbType.VarChar
        Public Const EligibleProfessional_DataSize As Integer = 100

        Public Const DisplaySubsidizeDesc_DataType As SqlDbType = SqlDbType.Char
        Public Const DisplaySubsidizeDesc_DataSize As Integer = 1

        Public Const EnrolPeriodFrom_DataType As SqlDbType = SqlDbType.DateTime
        Public Const EnrolPeriodFrom_DataSize As Integer = 8

        Public Const EnrolPeriodTo_DataType As SqlDbType = SqlDbType.DateTime
        Public Const EnrolPeriodTo_DataSize As Integer = 8

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

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Const AllowNonClinicSetting_DataType As SqlDbType = SqlDbType.Char
        Public Const AllowNonClinicSetting_DataSize As Integer = 1
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

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


        Public Property ServiceFeeEnabled() As Boolean
            Get
                If Me._strServiceFeeEnabled.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strServiceFeeEnabled = strYES
                Else
                    Me._strServiceFeeEnabled = strNO
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

        Public Property EnrolPeriodFrom() As DateTime
            Get
                Return Me._dtmEnrolPeriodFrom
            End Get
            Set(ByVal value As DateTime)
                Me._dtmEnrolPeriodFrom = value
            End Set
        End Property

        Public Property EnrolPeriodTo() As DateTime
            Get
                Return Me._dtmEnrolPeriodTo
            End Get
            Set(ByVal value As DateTime)
                Me._dtmEnrolPeriodTo = value
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

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Property AllowNonClinicSetting() As String
            Get
                Return Me._strAllowNonClinicSetting
            End Get
            Set(ByVal value As String)
                Me._strAllowNonClinicSetting = value
            End Set
        End Property
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

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

        Public Sub New(ByVal udtSchemeEFormModel As SchemeEFormModel)
            _strSchemeCode = udtSchemeEFormModel.SchemeCode
            _intSchemeSeq = udtSchemeEFormModel.SchemeSeq
            _strSchemeDesc = udtSchemeEFormModel.SchemeDesc
            _strSchemeDescChi = udtSchemeEFormModel.SchemeDescChi
            _strDisplayCode = udtSchemeEFormModel.DisplayCode
            _intDisplaySeq = udtSchemeEFormModel.DisplaySeq
            ServiceFeeEnabled = udtSchemeEFormModel.ServiceFeeEnabled
            EligibleProfesionalString = udtSchemeEFormModel.EligibleProfesionalString
            DisplaySubsidizeDesc = udtSchemeEFormModel.DisplaySubsidizeDesc
            _dtmEnrolPeriodFrom = udtSchemeEFormModel.EnrolPeriodFrom
            _dtmEnrolPeriodTo = udtSchemeEFormModel.EnrolPeriodTo
            _strCreateBy = udtSchemeEFormModel.CreateBy
            _dtmCreateDtm = udtSchemeEFormModel.CreateDtm
            _strUpdateBy = udtSchemeEFormModel.UpdateBy
            _dtmUpdateDtm = udtSchemeEFormModel.UpdateDtm
            _strRecordStatus = udtSchemeEFormModel.RecordStatus
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _strAllowNonClinicSetting = udtSchemeEFormModel.AllowNonClinicSetting
            'CRE16-002 (Revamp VSS) [End][Chris YIM]

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            _strJoinPCDCompulsory = udtSchemeEFormModel.JoinPCDCompulsory
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSchemeDesc As String, ByVal strSchemeDescChi As String, _
                        ByVal strDisplayCode As String, ByVal intDisplaySeq As Integer, ByVal strServiceFeeEnabled As String, ByVal strEligibleProfesional As String, _
                        ByVal strDisplaySubsidizeDesc As String, ByVal dtmEnrolPeriodFrom As DateTime, ByVal dtmEnrolPeriodTo As DateTime, ByVal strCreateBy As String, _
                        ByVal dtmCreateDtm As DateTime, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal strRecordStatus As String, _
                        ByVal strAllowNonClinicSetting As String, ByVal strJoinPCDCompulsory As String)
            _strSchemeCode = strSchemeCode
            _intSchemeSeq = intSchemeSeq
            _strSchemeDesc = strSchemeDesc
            _strSchemeDescChi = strSchemeDescChi
            _strDisplayCode = strDisplayCode
            _intDisplaySeq = intDisplaySeq
            _strServiceFeeEnabled = strServiceFeeEnabled
            _strEligibleProfesional = strEligibleProfesional
            _strDisplaySubsidizeDesc = strDisplaySubsidizeDesc
            _dtmEnrolPeriodFrom = dtmEnrolPeriodFrom
            _dtmEnrolPeriodTo = dtmEnrolPeriodTo
            _strCreateBy = strCreateBy
            _dtmCreateDtm = dtmCreateDtm
            _strUpdateBy = strUpdateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strRecordStatus = strRecordStatus
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _strAllowNonClinicSetting = strAllowNonClinicSetting
            'CRE16-002 (Revamp VSS) [End][Chris YIM]

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            _strJoinPCDCompulsory = strJoinPCDCompulsory
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
        End Sub
#End Region

#Region "Addition Memeber"
        Private _udtSubsidizeGroupEFormModelList As SubsidizeGroupEFormModelCollection

        Public Property SubsidizeGroupEFormList() As SubsidizeGroupEFormModelCollection
            Get
                Return Me._udtSubsidizeGroupEFormModelList
            End Get
            Set(ByVal value As SubsidizeGroupEFormModelCollection)
                Me._udtSubsidizeGroupEFormModelList = value
            End Set
        End Property
#End Region

    End Class
End Namespace

