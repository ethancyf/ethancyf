Imports System.ComponentModel
Imports System.Data.SqlClient
Imports Common.Component.Address
Imports Common.Component.BankAcct
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Professional
Imports Common.Component.StaticData
Imports Common.Format

Namespace Component.Practice
    <Serializable()> Public Class PracticeModel

#Region "Constants"

        Public Enum RecordStatusEnumClass
            <Description("A")> Active
            <Description("S")> Suspended
            <Description("D")> Delisted
        End Enum

#End Region

        Private _strPracticeType As String

        Public Property PracticeType() As String
            Get
                Return _strPracticeType
            End Get
            Set(ByVal value As String)
                _strPracticeType = value

                Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
                Dim udtStaticDataModel As StaticDataModel
                udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PRACTICETYPE", _strPracticeType)
                _strPracticeTypeDesc = udtStaticDataModel.DataValue
                _strPracticeTypeDescChi = udtStaticDataModel.DataValueChi
            End Set
        End Property

        Private _strSPID As String
        Private _strEnrolRefNo As String
        Private _intDisplaySeq As Integer
        Private _strPracticeName As String
        Private _strPracticeTypeDesc As String
        Private _strPracticeTypeDescChi As String
        Private _Address As AddressModel
        Private _intProfessionalSeq As Integer
        Private _strRecordStatus As String
        Private _strRemark As String
        Private _strSubmitMethod As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strCreateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _byteTSMP As Byte()

        Private _udtProfessionalModel As ProfessionalModel
        Private _udtBankAcctModel As BankAcctModel
        Private _udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection

        Private _strPracticeNameChi As String
        Private _strPhoneDaytime As String
        Private _intMODisplaySeq As Integer

        Private _blnIsduplicated As Boolean

        ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
        Private _strMobileClinic As String
        Private _strRemarksDesc As String
        Private _strRemarksDescChi As String
        ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

        Public Const DisplaySeqDataType As SqlDbType = SqlDbType.SmallInt
        Public Const DisplaySeqDataSize As Integer = 2

        Public Const PracticeNameDataType As SqlDbType = SqlDbType.NVarChar
        Public Const PracticeNameDataSize As Integer = 100

        Public Const PracticeTypeDataType As SqlDbType = SqlDbType.Char
        Public Const PracticeTypeDataSize As Integer = 1

        Public Const RecordStatusDataType As SqlDbType = SqlDbType.Char
        Public Const RecordStatusDataSize As Integer = 1

        Public Const DelistStatusDataType As SqlDbType = SqlDbType.Char
        Public Const DelistStatusDataSize As Integer = 1

        Public Const RemarkDataType As SqlDbType = SqlDbType.NVarChar
        Public Const RemarkDataSize As Integer = 255

        Public Const SubmissionMethodDataType As SqlDbType = SqlDbType.Char
        Public Const SubmissionMethodDataSize As Integer = 1

        Public Const CreateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const CreateByDataSize As Integer = 20

        Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdateByDataSize As Integer = 20

        Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
        Public Const TSMPDataSize As Integer = 8

        Public Const PracticeNameChiDataType As SqlDbType = SqlDbType.NVarChar
        Public Const PracticeNameChiDataSize As Integer = 100

        Public Const PhoneDaytimeDataType As SqlDbType = SqlDbType.VarChar
        Public Const PhoneDaytimeDataSize As Integer = 20

        ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
        ' ------------------------------------------------------------------------
        Public Const MobileClinicDataType As SqlDbType = SqlDbType.Char
        Public Const MobileClinicDataSize As Integer = 1

        Public Const RemarksDescDataType As SqlDbType = SqlDbType.NVarChar
        Public Const RemarksDescDataSize As Integer = 200

        Public Const RemarksDescChiDataType As SqlDbType = SqlDbType.NVarChar
        Public Const RemarksDescChiDataSize As Integer = 200
        ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value
            End Set
        End Property

        Public Property EnrolRefNo() As String
            Get
                Return _strEnrolRefNo
            End Get
            Set(ByVal value As String)
                _strEnrolRefNo = value
            End Set
        End Property

        Public Property DisplaySeq() As Integer
            Get
                Return _intDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intDisplaySeq = value
            End Set
        End Property

        Public Property MODisplaySeq() As Integer
            Get
                Return _intMODisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intMODisplaySeq = value
            End Set
        End Property

        Public Property PracticeName() As String
            Get
                Return _strPracticeName
            End Get
            Set(ByVal value As String)
                _strPracticeName = value
            End Set
        End Property

        Public Property PracticeAddress() As AddressModel
            Get
                Return _Address
            End Get
            Set(ByVal value As AddressModel)
                _Address = value
            End Set
        End Property

        Public Property ProfessionalSeq() As Integer
            Get
                Return _intProfessionalSeq
            End Get
            Set(ByVal value As Integer)
                _intProfessionalSeq = value
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

        Public Property RecordStatusEnum() As RecordStatusEnumClass
            Get
                Return Formatter.StringToEnum(GetType(RecordStatusEnumClass), _strRecordStatus)
            End Get
            Set(value As RecordStatusEnumClass)
                _strRecordStatus = Formatter.EnumToString(value)
            End Set
        End Property

        Public Property SubmitMethod() As String
            Get
                Return _strSubmitMethod
            End Get
            Set(ByVal value As String)
                _strSubmitMethod = value
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

        Public Property BankAcct() As BankAcctModel
            Get
                Return _udtBankAcctModel
            End Get
            Set(ByVal value As BankAcctModel)
                _udtBankAcctModel = value
            End Set
        End Property

        Public Property Professional() As ProfessionalModel
            Get
                Return _udtProfessionalModel
            End Get
            Set(ByVal value As ProfessionalModel)
                _udtProfessionalModel = value
            End Set
        End Property

        Public Property PracticeNameChi() As String
            Get
                Return _strPracticeNameChi
            End Get
            Set(ByVal value As String)
                _strPracticeNameChi = value
            End Set
        End Property

        Public Property PhoneDaytime() As String
            Get
                Return _strPhoneDaytime
            End Get
            Set(ByVal value As String)
                _strPhoneDaytime = value
            End Set
        End Property

        Public Property PracticeSchemeInfoList() As PracticeSchemeInfoModelCollection
            Get
                Return _udtPracticeSchemeInfoList
            End Get
            Set(ByVal value As PracticeSchemeInfoModelCollection)
                _udtPracticeSchemeInfoList = value
            End Set
        End Property

        Public Property IsDuplicated() As Boolean
            Get
                Return _blnIsduplicated
            End Get
            Set(ByVal value As Boolean)
                _blnIsduplicated = value
            End Set
        End Property

        ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
        ' ------------------------------------------------------------------------
        Public Property MobileClinic() As String
            Get
                Return _strMobileClinic
            End Get
            Set(ByVal value As String)
                _strMobileClinic = value
            End Set
        End Property

        Public Property RemarksDesc() As String
            Get
                Return _strRemarksDesc
            End Get
            Set(ByVal value As String)
                _strRemarksDesc = value
            End Set
        End Property

        Public Property RemarksDescChi() As String
            Get
                Return _strRemarksDescChi
            End Get
            Set(ByVal value As String)
                _strRemarksDescChi = value
            End Set
        End Property
        ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtPracticeModel As PracticeModel)
            _strSPID = udtPracticeModel.SPID
            _strEnrolRefNo = udtPracticeModel.EnrolRefNo
            _intDisplaySeq = udtPracticeModel.DisplaySeq
            _intMODisplaySeq = udtPracticeModel.MODisplaySeq
            _strPracticeName = udtPracticeModel.PracticeName
            _strPracticeNameChi = udtPracticeModel.PracticeNameChi
            _Address = udtPracticeModel.PracticeAddress
            _intProfessionalSeq = udtPracticeModel.ProfessionalSeq
            _strRecordStatus = udtPracticeModel.RecordStatus
            _strSubmitMethod = udtPracticeModel.SubmitMethod
            _strRemark = udtPracticeModel.Remark
            _dtmCreateDtm = udtPracticeModel.CreateDtm
            _strCreateBy = udtPracticeModel.CreateBy
            _dtmUpdateDtm = udtPracticeModel.UpdateDtm
            _strUpdateBy = udtPracticeModel.UpdateBy
            _byteTSMP = udtPracticeModel.TSMP

            _strPhoneDaytime = udtPracticeModel.PhoneDaytime

            _udtProfessionalModel = udtPracticeModel.Professional
            _udtBankAcctModel = udtPracticeModel.BankAcct
            _udtPracticeSchemeInfoList = udtPracticeModel.PracticeSchemeInfoList

            ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
            _strMobileClinic = udtPracticeModel.MobileClinic
            _strRemarksDesc = udtPracticeModel.RemarksDesc
            _strRemarksDescChi = udtPracticeModel.RemarksDescChi
            ' CRE16-022 (Add optional field "Remarks") [End][Winnie]
        End Sub

        ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
        ' Add [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
        Public Sub New(ByVal strSPID As String, ByVal strEnrolRefNo As String, ByVal intDisplaySeq As Integer, _
                        ByVal intMODisplaySeq As Integer, ByVal strPracticeName As String, ByVal strPracticeNameChi As String, ByVal udtAddress As AddressModel, _
                        ByVal intProfessionalSeq As Integer, ByVal strRecordStatus As String, _
                        ByVal strSubmitMethod As String, ByVal strRemark As String, ByVal strPhoneDaytime As String, ByVal dtmCreateDtm As Nullable(Of DateTime), _
                        ByVal strCreateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal strUpdateBy As String, _
                        ByVal byteTSMP As Byte(), ByVal strMobileClinic As String, ByVal strRemarksDesc As String, ByVal strRemarksDescChi As String, _
                        ByVal udtBankAcctModel As BankAcctModel, _
                        ByVal udtProfessionalModel As ProfessionalModel, ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection)
            ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

            _strSPID = strSPID
            _strEnrolRefNo = strEnrolRefNo
            _intDisplaySeq = intDisplaySeq
            _intMODisplaySeq = intMODisplaySeq
            _strPracticeName = strPracticeName
            _strPracticeNameChi = strPracticeNameChi
            _Address = udtAddress
            _intProfessionalSeq = intProfessionalSeq
            _strRecordStatus = strRecordStatus
            _strSubmitMethod = strSubmitMethod
            _strRemark = strRemark
            _strPhoneDaytime = strPhoneDaytime
            _dtmCreateDtm = dtmCreateDtm
            _strCreateBy = strCreateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strUpdateBy = strUpdateBy
            _byteTSMP = byteTSMP
            ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
            _strMobileClinic = strMobileClinic
            _strRemarksDesc = strRemarksDesc
            _strRemarksDescChi = strRemarksDescChi
            ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

            _udtBankAcctModel = udtBankAcctModel
            _udtProfessionalModel = udtProfessionalModel

            _udtPracticeSchemeInfoList = udtPracticeSchemeInfoList

        End Sub

        Public Class PracticeRecordStatus
            Public Const Active As String = "A"
            Public Const Delisted As String = "D"

        End Class

        Public Function IsAllowJoinPCD() As Boolean
            Dim blnIsAllowJoinPCD As Boolean = False

            ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            '' Handle Status in function [FilterByPCD]
            'If _strRecordStatus = PracticeRecordStatus.Active Or _
            '    _strRecordStatus = PracticeStagingStatus.Active Or _
            '    _strRecordStatus = PracticeStagingStatus.Existing Or _
            '    _strRecordStatus = PracticeStagingStatus.Update Then
            ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

            If Common.Component.Profession.ProfessionBLL.GetProfessionListByServiceCategoryCode(_udtProfessionalModel.ServiceCategoryCode).AllowJoinPCD Then
                blnIsAllowJoinPCD = True
            End If

            Return blnIsAllowJoinPCD
        End Function

    End Class

End Namespace