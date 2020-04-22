Imports Common.Component.Address
Imports Common.Component.PracticeT
Imports Common.Component.BankAcct
Imports Common.Component.UserAC
Imports Common.Component.SchemeInformation
Imports Common.Component.MedicalOrganizationT

Namespace Component.ServiceProviderT
    <Serializable()> Public Class ServiceProviderModel
        Inherits UserACModel

        Private _strEnrolRefNo As String
        Private _dtmEnrolDate As Nullable(Of DateTime)
        Private _strSPID As String
        Private _strAliasAccount As String
        Private _strHKID As String
        Private _strEName As String
        Private _strCName As String
        Private _Address As AddressModel
        Private _strPhone As String
        Private _strFax As String
        Private _strEmail As String
        Private _strEmailChanged As String
        Private _strRecordStatus As String
        Private _strDelistStatus As String
        Private _strRemark As String
        Private _strSubmitMethod As String
        Private _strAlreadyJoinedHAPPI As String
        Private _strJoinHAPPI As String
        Private _strApplicationPrinted As String
        Private _strUnderModification As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strCreateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _dtmEffectiveDtm As Nullable(Of DateTime)
        Private _dtmDelistDtm As Nullable(Of DateTime)
        Private _byteTSMP As Byte()
        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Private _strDataInputBy As String
        Private _dtmDataInputEffectDtm As Nullable(Of DateTime)
        ' INT13-0028 - SP Amendment Report [End][Tommy L]
        Private _udtSchemeInfoList As SchemeInformationModelCollection

        Private _udtMOList As MedicalOrganizationModelCollection

        ' Newly Add
        Private _strActivation_Code As String


        Private _practiceList As PracticeModelCollection
        'Private _bankAcctList As BankAcctModelCollection

        Public Const EnrolRefNoDataType As SqlDbType = SqlDbType.Char
        Public Const EnrolRefNoDataSize As Integer = 15

        Public Const EnrolDateDataType As SqlDbType = SqlDbType.DateTime
        Public Const EnrolDateDataSize As Integer = 8

        Public Const SPIDDataType As SqlDbType = SqlDbType.Char
        Public Const SPIDDataSize As Integer = 8

        Public Const HKIDDataType As SqlDbType = SqlDbType.Char
        Public Const HKIDDataSize As Integer = 9

        Public Const ENameDataType As SqlDbType = SqlDbType.VarChar
        Public Const ENameDataSize As Integer = 40

        Public Const CNameDataType As SqlDbType = SqlDbType.NVarChar
        Public Const CNameDataSize As Integer = 6

        Public Const PhoneDataType As SqlDbType = SqlDbType.VarChar
        Public Const PhoneDataSize As Integer = 20

        Public Const FaxDataType As SqlDbType = SqlDbType.VarChar
        Public Const FaxDataSize As Integer = 20

        Public Const EmailDataType As SqlDbType = SqlDbType.VarChar
        Public Const EmailDataSize As Integer = 255

        Public Const EmailChangedDataType As SqlDbType = SqlDbType.Char
        Public Const EmailChangedDataSize As Integer = 1

        Public Const RecordStatusDataType As SqlDbType = SqlDbType.Char
        Public Const RecordStatusDataSize As Integer = 1

        Public Const DelistStatusDataType As SqlDbType = SqlDbType.Char
        Public Const DelistStatusDataSize As Integer = 1

        Public Const RemarkDataType As SqlDbType = SqlDbType.NVarChar
        Public Const RemarkDataSize As Integer = 255

        Public Const SubmissionMethodDataType As SqlDbType = SqlDbType.Char
        Public Const SubmissionMethodDataSize As Integer = 1

        Public Const UnderModificationDataType As SqlDbType = SqlDbType.Char
        Public Const UnderModificationDataSize As Integer = 1

        Public Const AlreadyJoinedHAPPIDataType As SqlDbType = SqlDbType.Char
        Public Const AlreadyJoinedHAPPIDataSize As Integer = 1

        Public Const JoinHAPPIDataType As SqlDbType = SqlDbType.Char
        Public Const JoinHAPPIDataSize As Integer = 1

        Public Const ApplicationPrintedDataType As SqlDbType = SqlDbType.Char
        Public Const ApplicationPrintedDataSize As Integer = 1

        Public Const CreateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const CreateByDataSize As Integer = 20

        Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdateByDataSize As Integer = 20

        Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
        Public Const TSMPDataSize As Integer = 8

        Public Const Activation_CodeDataType As SqlDbType = SqlDbType.VarChar
        Public Const Activation_CodeDataSize As Integer = 100

        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Public Const DataInputByDataType As SqlDbType = SqlDbType.VarChar
        Public Const DataInputByDataSize As Integer = 20

        Public Const DataInputEffectDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const DataInputEffectDtmDataSize As Integer = 8
        ' INT13-0028 - SP Amendment Report [End][Tommy L]

        Public Property EnrolRefNo() As String
            Get
                Return _strEnrolRefNo
            End Get
            Set(ByVal value As String)
                _strEnrolRefNo = value
            End Set
        End Property

        Public Property EnrolDate() As Nullable(Of DateTime)
            Get
                Return _dtmEnrolDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmEnrolDate = value
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

        Public Property AliasAccount() As String
            Get
                Return _strAliasAccount
            End Get
            Set(ByVal value As String)
                _strAliasAccount = value
            End Set
        End Property

        Public Property HKID() As String
            Get
                Return _strHKID
            End Get
            Set(ByVal value As String)
                _strHKID = value
            End Set
        End Property

        Public Property EnglishName() As String
            Get
                Return _strEName
            End Get
            Set(ByVal value As String)
                _strEName = value
            End Set
        End Property

        Public Property ChineseName() As String
            Get
                Return _strCName
            End Get
            Set(ByVal value As String)
                _strCName = value
            End Set
        End Property

        Public Property SpAddress() As AddressModel
            Get
                Return _Address
            End Get
            Set(ByVal value As AddressModel)
                _Address = value
            End Set
        End Property

        Public Property Phone() As String
            Get
                Return _strPhone
            End Get
            Set(ByVal value As String)
                _strPhone = value
            End Set
        End Property

        Public Property Fax() As String
            Get
                Return _strFax
            End Get
            Set(ByVal value As String)
                _strFax = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return _strEmail
            End Get
            Set(ByVal value As String)
                _strEmail = value
            End Set
        End Property

        Public Property EmailChanged() As String
            Get
                Return _strEmailChanged
            End Get
            Set(ByVal value As String)
                _strEmailChanged = value
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

        Public Property DelistStatus() As String
            Get
                Return _strDelistStatus
            End Get
            Set(ByVal value As String)
                _strDelistStatus = value
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

        Public Property SubmitMethod() As String
            Get
                Return _strSubmitMethod
            End Get
            Set(ByVal value As String)
                _strSubmitMethod = value
            End Set
        End Property

        Public Property AlreadyJoinHAPPI() As String
            Get
                Return _strAlreadyJoinedHAPPI
            End Get
            Set(ByVal value As String)
                _strAlreadyJoinedHAPPI = value
            End Set
        End Property

        Public Property JoinHAPPI() As String
            Get
                Return _strJoinHAPPI
            End Get
            Set(ByVal value As String)
                _strJoinHAPPI = value
            End Set
        End Property

        Public Property ApplicationPrinted() As String
            Get
                Return _strApplicationPrinted
            End Get
            Set(ByVal value As String)
                _strApplicationPrinted = value
            End Set
        End Property

        Public Property UnderModification() As String
            Get
                Return _strUnderModification
            End Get
            Set(ByVal value As String)
                _strUnderModification = value
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

        Public Property ActivationCode() As String
            Get
                Return Me._strActivation_Code
            End Get
            Set(ByVal value As String)
                Me._strActivation_Code = value
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

        Public Property DataInputEffectDtm() As Nullable(Of DateTime)
            Get
                Return _dtmDataInputEffectDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmDataInputEffectDtm = value
            End Set
        End Property
        ' INT13-0028 - SP Amendment Report [End][Tommy L]

        Public Property PracticeList() As PracticeModelCollection
            Get
                Return _practiceList
            End Get
            Set(ByVal value As PracticeModelCollection)
                _practiceList = value
            End Set
        End Property

        Public Property SchemeInfoList() As SchemeInformationModelCollection
            Get
                Return _udtSchemeInfoList
            End Get
            Set(ByVal value As SchemeInformationModelCollection)
                _udtSchemeInfoList = value
            End Set
        End Property

        Public Property MOList() As MedicalOrganizationModelCollection
            Get
                Return _udtMOList
            End Get
            Set(ByVal value As MedicalOrganizationModelCollection)
                _udtMOList = value
            End Set
        End Property

        'Public Property BankAcctList() As BankAcctModelCollection
        '    Get
        '        Return _bankAcctList
        '    End Get
        '    Set(ByVal value As BankAcctModelCollection)
        '        _bankAcctList = value
        '    End Set
        'End Property


        Public Sub New()

        End Sub

        Public Sub New(ByVal udtServiceProviderModel As ServiceProviderModel)
            _strEnrolRefNo = udtServiceProviderModel.EnrolRefNo
            _dtmEnrolDate = udtServiceProviderModel.EnrolDate
            _strSPID = udtServiceProviderModel.SPID
            _strAliasAccount = udtServiceProviderModel.AliasAccount
            _strHKID = udtServiceProviderModel.HKID
            _strEName = udtServiceProviderModel.EnglishName
            _strCName = udtServiceProviderModel.ChineseName
            _Address = udtServiceProviderModel.SpAddress
            _strPhone = udtServiceProviderModel.Phone
            _strFax = udtServiceProviderModel.Fax
            _strEmail = udtServiceProviderModel.Email
            _strEmailChanged = udtServiceProviderModel.EmailChanged
            _strRecordStatus = udtServiceProviderModel.RecordStatus
            _strDelistStatus = udtServiceProviderModel.DelistStatus
            _strRemark = udtServiceProviderModel.Remark
            _strSubmitMethod = udtServiceProviderModel.SubmitMethod
            _strAlreadyJoinedHAPPI = udtServiceProviderModel.AlreadyJoinHAPPI
            _strJoinHAPPI = udtServiceProviderModel.JoinHAPPI
            _strUnderModification = udtServiceProviderModel.UnderModification
            _strApplicationPrinted = udtServiceProviderModel.ApplicationPrinted
            _dtmCreateDtm = udtServiceProviderModel.CreateDtm
            _strCreateBy = udtServiceProviderModel.CreateBy
            _dtmUpdateDtm = udtServiceProviderModel.UpdateDtm
            _strUpdateBy = udtServiceProviderModel.UpdateBy
            _dtmEffectiveDtm = udtServiceProviderModel.EffectiveDtm
            _dtmDelistDtm = udtServiceProviderModel.DelistDtm
            _byteTSMP = udtServiceProviderModel.TSMP

            _practiceList = udtServiceProviderModel.PracticeList
            '_bankAcctList = objServiceProviderModel.BankAcctList

            ' INT13-0028 - SP Amendment Report [Start][Tommy L]
            ' -------------------------------------------------------------------------
            _strDataInputBy = udtServiceProviderModel.DataInputBy
            _dtmDataInputEffectDtm = udtServiceProviderModel.DataInputEffectDtm
            ' INT13-0028 - SP Amendment Report [End][Tommy L]
        End Sub

        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Public Sub New(ByVal strEnrolRefNo As String, ByVal dtmEnrolDate As Nullable(Of DateTime), ByVal strSPID As String, ByVal strAliasAccount As String, ByVal strHKID As String, _
        '               ByVal strEName As String, ByVal strCName As String, ByVal Address As AddressModel, ByVal strPhone As String, _
        '               ByVal strFax As String, ByVal strEmail As String, ByVal strEmailChanged As String, ByVal strRecordStatus As String, ByVal strDelistStatus As String, _
        '               ByVal strRemark As String, ByVal strSubmitMethod As String, ByVal strAlreadyJoinedHAPPI As String, ByVal strJoinHAPPI As String, _
        '               ByVal strUnderModification As String, ByVal strApplicationPrinted As String, ByVal dtmCreateDtm As Nullable(Of DateTime), ByVal strCreateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), _
        '               ByVal strUpdateBy As String, ByVal dtmEffectiveDtm As Nullable(Of DateTime), ByVal dtmDelistDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte(), ByVal udtPracticeModelCollection As PracticeModelCollection)
        Public Sub New(ByVal strEnrolRefNo As String, ByVal dtmEnrolDate As Nullable(Of DateTime), ByVal strSPID As String, ByVal strAliasAccount As String, ByVal strHKID As String, _
                       ByVal strEName As String, ByVal strCName As String, ByVal Address As AddressModel, ByVal strPhone As String, _
                       ByVal strFax As String, ByVal strEmail As String, ByVal strEmailChanged As String, ByVal strRecordStatus As String, ByVal strDelistStatus As String, _
                       ByVal strRemark As String, ByVal strSubmitMethod As String, ByVal strAlreadyJoinedHAPPI As String, ByVal strJoinHAPPI As String, _
                       ByVal strUnderModification As String, ByVal strApplicationPrinted As String, ByVal dtmCreateDtm As Nullable(Of DateTime), ByVal strCreateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), _
                       ByVal strUpdateBy As String, ByVal dtmEffectiveDtm As Nullable(Of DateTime), ByVal dtmDelistDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte(), ByVal udtPracticeModelCollection As PracticeModelCollection, _
                       ByVal strDataInputBy As String, ByVal dtmDataInputEffectDtm As Nullable(Of DateTime))

            _strDataInputBy = strDataInputBy
            _dtmDataInputEffectDtm = dtmDataInputEffectDtm
            ' INT13-0028 - SP Amendment Report [End][Tommy L]

            _strEnrolRefNo = strEnrolRefNo
            _dtmEnrolDate = dtmEnrolDate
            _strSPID = strSPID
            _strAliasAccount = strAliasAccount
            _strHKID = strHKID
            _strEName = strEName
            _strCName = strCName
            _Address = Address
            _strPhone = strPhone
            _strFax = strFax
            _strEmail = strEmail
            _strEmailChanged = strEmailChanged
            _strRecordStatus = strRecordStatus
            _strDelistStatus = strDelistStatus
            _strRemark = strRemark
            _strSubmitMethod = strSubmitMethod
            _strAlreadyJoinedHAPPI = strAlreadyJoinedHAPPI
            _strJoinHAPPI = strJoinHAPPI
            _strUnderModification = strUnderModification
            _strApplicationPrinted = strApplicationPrinted
            _dtmCreateDtm = dtmCreateDtm
            _strCreateBy = strCreateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strUpdateBy = strUpdateBy
            _dtmEffectiveDtm = dtmEffectiveDtm
            _dtmDelistDtm = dtmDelistDtm
            _byteTSMP = byteTSMP

            _practiceList = udtPracticeModelCollection
            '_bankAcctList = bankAcctList

        End Sub

    End Class
End Namespace
