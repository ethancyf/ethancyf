Imports System.Data.SqlClient
Imports Common.Component.Address
Imports Common.Component.BankAcct
Imports Common.Component.Professional
Imports Common.Component.StaticData

Namespace Component.PracticeT
    <Serializable()> Public Class PracticeModel
        Private _strSPID As String
        Private _strEnrolRefNo As String
        Private _intDisplaySeq As Integer
        Private _strPracticeName As String
        Private _strPracticeType As String
        Private _strPracticeTypeDesc As String
        Private _strPracticeTypeDescChi As String
        Private _Address As AddressModel
        'Private _strHealthProfCode As String
        'Private _strHealthProfCodeDesc As String
        'Private _strHealthProfCodeDescChi As String
        'Private _strRegCode As String
        Private _intProfessionalSeq As Integer
        Private _strRecordStatus As String
        Private _strRemark As String
        Private _strSubmitMethod As String
        Private _strDelistStatus As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strCreateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _dtmDelistDtm As Nullable(Of DateTime)
        Private _dtmEffectiveDtm As Nullable(Of DateTime)
        Private _byteTSMP As Byte()

        Private _udtProfessionalModel As ProfessionalModel
        Private _udtBankAcctModel As BankAcctModel

        Private _strPracticeNameChi As String
        Private _strPhoneDaytime As String

        Private _intMODisplaySeq As Integer

        Private _intEnrolmentForm_ServiceFeeFrom As Nullable(Of Integer)
        Private _intEnrolmentForm_ServiceFeeTo As Nullable(Of Integer)
        Private _strPracticeTypeRemarks As String

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

        'Public Const UnderModificationDataType As SqlDbType = SqlDbType.Char
        'Public Const UnderModificationDataSize As Integer = 1

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

        Public Const EnrolmentForm_ServiceFeeFromDataType As SqlDbType = SqlDbType.SmallInt
        Public Const EnrolmentForm_ServiceFeeFromDataSize As Integer = 2

        Public Const EnrolmentForm_ServiceFeeToDataType As SqlDbType = SqlDbType.SmallInt
        Public Const EnrolmentForm_ServiceFeeToDataSize As Integer = 2

        Public Const PracticeTypeRemarksDataType As SqlDbType = SqlDbType.NVarChar
        Public Const PracticeTypeRemarksDataSize As Integer = 255

        'Public ReadOnly Property EnrolRefCodeType() As SqlDbType
        '    Get
        '        Return SqlDbType.Char
        '    End Get
        'End Property

        'Public ReadOnly Property EnrolRefCodeSize() As Integer
        '    Get
        '        Return _intEnrolRefCodeSize
        '    End Get
        'End Property

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

        Public Property PracticeName() As String
            Get
                Return _strPracticeName
            End Get
            Set(ByVal value As String)
                _strPracticeName = value
            End Set
        End Property

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

        Public Property PracticeTypeDesc() As String
            Get
                Return _strPracticeTypeDesc
            End Get
            Set(ByVal value As String)
                _strPracticeTypeDesc = value
            End Set
        End Property

        Public Property PracticeTypeDescChi() As String
            Get
                Return _strPracticeTypeDescChi
            End Get
            Set(ByVal value As String)
                _strPracticeTypeDescChi = value
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

        'Public Property HealthProfCode() As String
        '    Get
        '        Return _strHealthProfCode
        '    End Get
        '    Set(ByVal value As String)
        '        _strHealthProfCode = value

        '        Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
        '        Dim udtStaticDataModel As StaticDataModel
        '        udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PROFESSION", _strHealthProfCode)
        '        _strHealthProfCodeDesc = udtStaticDataModel.DataValue
        '        _strHealthProfCodeDescChi = udtStaticDataModel.DataValueChi

        '    End Set
        'End Property

        'Public Property HealthProfCodeDesc() As String
        '    Get
        '        Return _strHealthProfCodeDesc
        '    End Get
        '    Set(ByVal value As String)
        '        _strHealthProfCodeDesc = value
        '    End Set
        'End Property

        'Public Property RegCode() As String
        '    Get
        '        Return _strRegCode
        '    End Get
        '    Set(ByVal value As String)
        '        _strRegCode = value
        '    End Set
        'End Property

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

        Public Property DelistStatus() As String
            Get
                Return _strDelistStatus
            End Get
            Set(ByVal value As String)
                _strDelistStatus = value
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

        Public Property EffectiveDtm() As Nullable(Of DateTime)
            Get
                Return _dtmEffectiveDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmEffectiveDtm = value
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

        Public Property EnrolmentForm_ServiceFeeFrom() As Nullable(Of Integer)
            Get
                Return _intEnrolmentForm_ServiceFeeFrom
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _intEnrolmentForm_ServiceFeeFrom = value
            End Set
        End Property

        Public Property EnrolmentForm_ServiceFeeTo() As Nullable(Of Integer)
            Get
                Return _intEnrolmentForm_ServiceFeeTo
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _intEnrolmentForm_ServiceFeeTo = value
            End Set
        End Property

        Public Property PracticeTypeRemarks() As String
            Get
                Return _strPracticeTypeRemarks
            End Get
            Set(ByVal value As String)
                _strPracticeTypeRemarks = value
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

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtPracticeModel As PracticeModel)
            _strSPID = udtPracticeModel.SPID
            _strEnrolRefNo = udtPracticeModel.EnrolRefNo
            _intDisplaySeq = udtPracticeModel.DisplaySeq
            _strPracticeName = udtPracticeModel.PracticeName
            '_strPracticeType = objPracticeModel.PracticeType
            PracticeType = udtPracticeModel.PracticeType
            _Address = udtPracticeModel.PracticeAddress
            '_strHealthProfCode = objPracticeModel.HealthProfCode
            'HealthProfCode = udtPracticeModel.HealthProfCode
            '_strRegCode = udtPracticeModel.RegCode
            _intProfessionalSeq = udtPracticeModel.ProfessionalSeq
            _strRecordStatus = udtPracticeModel.RecordStatus
            _strSubmitMethod = udtPracticeModel.SubmitMethod
            _strDelistStatus = udtPracticeModel.DelistStatus
            _strRemark = udtPracticeModel.Remark
            _dtmCreateDtm = udtPracticeModel.CreateDtm
            _strCreateBy = udtPracticeModel.CreateBy
            _dtmUpdateDtm = udtPracticeModel.UpdateDtm
            _strUpdateBy = udtPracticeModel.UpdateBy
            _dtmEffectiveDtm = udtPracticeModel.EffectiveDtm
            _dtmDelistDtm = udtPracticeModel.DelistDtm
            _byteTSMP = udtPracticeModel.TSMP

            _udtProfessionalModel = udtPracticeModel.Professional
            _udtBankAcctModel = udtPracticeModel.BankAcct

            'Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
            'Dim udtStaticDataModel As StaticDataModel
            'udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PROFESSION", _strHealthProfCode)
            '_strHealthProfCodeDesc = udtStaticDataModel.DataValue
            '_strHealthProfCodeDescChi = udtStaticDataModel.DataValueChi

            'udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PRACTICETYPE", _strPracticeType)
            '_strPracticeTypeDesc = udtStaticDataModel.DataValue
            '_strPracticeTypeDescChi = udtStaticDataModel.DataValueChi

        End Sub

        Public Sub New(ByVal strSPID As String, ByVal strEnrolRefNo As String, ByVal intDisplaySeq As Integer, _
                        ByVal strPracticeName As String, ByVal strPracticeType As String, ByVal udtAddress As AddressModel, _
                        ByVal intProfessionalSeq As Integer, ByVal strRecordStatus As String, ByVal strDelistStatus As String, _
                        ByVal strSubmitMethod As String, ByVal strRemark As String, ByVal dtmCreateDtm As Nullable(Of DateTime), _
                        ByVal strCreateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal strUpdateBy As String, ByVal dtmEffectiveDtm As Nullable(Of DateTime), _
                        ByVal dtmDelistDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte(), ByVal udtBankAcctModel As BankAcctModel, _
                        ByVal udtProfessionalModel As ProfessionalModel)

            _strSPID = strSPID
            _strEnrolRefNo = strEnrolRefNo
            _intDisplaySeq = intDisplaySeq
            _strPracticeName = strPracticeName
            '_strPracticeType = PracticeType
            PracticeType = strPracticeType
            _Address = udtAddress
            '_strHealthProfCode = HealthProfCode
            'HealthProfCode = strHealthProfCode
            '_strRegCode = strRegCode
            _intProfessionalSeq = intProfessionalSeq
            _strRecordStatus = strRecordStatus
            _strSubmitMethod = strSubmitMethod
            _strDelistStatus = strDelistStatus
            _strRemark = strRemark
            _dtmCreateDtm = dtmCreateDtm
            _strCreateBy = strCreateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strUpdateBy = strUpdateBy
            _dtmEffectiveDtm = dtmEffectiveDtm
            _dtmDelistDtm = dtmDelistDtm
            _byteTSMP = byteTSMP
            _udtBankAcctModel = udtBankAcctModel
            _udtProfessionalModel = udtProfessionalModel

            'Dim udtStaticDataBLL As StaticDataBLL = New StaticDataBLL
            'Dim udtStaticDataModel As StaticDataModel
            'udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PROFESSION", _strHealthProfCode)
            '_strHealthProfCodeDesc = udtStaticDataModel.DataValue
            '_strHealthProfCodeDescChi = udtStaticDataModel.DataValueChi

            'udtStaticDataModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PRACTICETYPE", _strPracticeType)
            '_strPracticeTypeDesc = udtStaticDataModel.DataValue
            '_strPracticeTypeDescChi = udtStaticDataModel.DataValueChi

        End Sub


    End Class
End Namespace