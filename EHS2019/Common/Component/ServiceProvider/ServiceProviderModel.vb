Imports Common.Component.Address
Imports Common.Component.Practice
Imports Common.Component.BankAcct
Imports Common.Component.ERNProcessed
Imports Common.Component.UserAC
Imports Common.Component.SchemeInformation
Imports Common.Component.MedicalOrganization
Imports Common.Component.ThirdParty
Imports System.ComponentModel
Imports Common.Format

' ----- Model Structure Change 07 May 2009 -----
' 1  Remove Delist Status
' 2  Remove Delist Dtm
' 3  Add Token return dtm
' ----- End 07 May 2009 ------------------------
'May 12
'Add MO List

' CRE16-018 Display SP tentative email in HCVU [Winnie]
' Add tentative email

Namespace Component.ServiceProvider
    <Serializable()> Public Class ServiceProviderModel
        Inherits UserACModel

#Region "Constants"

        Public Enum RecordStatusEnumClass
            <Description("A")> Active
            <Description("S")> Suspended
            <Description("D")> Delisted
        End Enum

#End Region

#Region "Private Members"
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
        Private _strTentativeEmail As String
        Private _strEmailChanged As String
        Private _strRecordStatus As String
        Private _strRemark As String
        Private _strSubmitMethod As String
        Private _strAlreadyJoinedEHR As String

        'Integration Start

        Private _strJoinPCD As String
        Private _strPCDAccountStatus As String
        Private _strPCDEnrolmentStatus As String
        Private _strPCDProfessional As String
        Private _dtmPCDStatusLastCheckDtm As Nullable(Of DateTime)

        'Integration End

        Private _strApplicationPrinted As String
        Private _strUnderModification As String
        Private _dtmEffectiveDtm As Nullable(Of DateTime)
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strCreateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _dtmTokenReturnDtm As Nullable(Of DateTime)
        Private _strActivation_Code As String
        Private _byteTSMP As Byte()

        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Private _strDataInputBy As String
        Private _dtmDataInputEffectDtm As Nullable(Of DateTime)
        ' INT13-0028 - SP Amendment Report [End][Tommy L]

        ' Lists
        Private _udtSchemeInfoList As SchemeInformationModelCollection
        Private _practiceList As PracticeModelCollection
        Private _udtMOList As MedicalOrganizationModelCollection
        Private _udtERNProcessedList As ERNProcessedModelCollection
        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Private _udtThirdPartyAdditionalFieldEnrolmentList As ThirdPartyAdditionalFieldEnrolmentCollection
        ' CRE12-001 eHS and PCD integration [End][Koala]
#End Region

#Region "DB Data Mapping"
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

        Public Const TentativeEmailDataType As SqlDbType = SqlDbType.VarChar
        Public Const TentativeEmailDataSize As Integer = 255

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

        Public Const AlreadyJoinedEHRDataType As SqlDbType = SqlDbType.Char
        Public Const AlreadyJoinedEHRDataSize As Integer = 1

        'Integration Start

        Public Const JoinPCDDataType As SqlDbType = SqlDbType.Char
        Public Const JoinPCDDataSize As Integer = 1

        'Integration End

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

        Public Const TokenReturnDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const TokenReturnDtmDataSize As Integer = 8

        Public Const EffectiveDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const EffectiveDtmDataSize As Integer = 8

        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Public Const DataInputByDataType As SqlDbType = SqlDbType.VarChar
        Public Const DataInputByDataSize As Integer = 20

        Public Const DataInputEffectDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const DataInputEffectDtmDataSize As Integer = 8
        ' INT13-0028 - SP Amendment Report [End][Tommy L]

        ' CRE17-016 (Checking of PCD status during VSS enrolment) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Const PCDAccountStatusDataType As SqlDbType = SqlDbType.Char
        Public Const PCDAccountStatusDataSize As Integer = 1

        Public Const PCDEnrolmentStatusDataType As SqlDbType = SqlDbType.Char
        Public Const PCDEnrolmentStatusDataSize As Integer = 1

        Public Const PCDProfessionalDataType As SqlDbType = SqlDbType.VarChar
        Public Const PCDProfessionalDataSize As Integer = 20

        Public Const LastCheckDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const LastCheckDtmDataSize As Integer = 8
        ' CRE17-016 (Checking of PCD status during VSS enrolment) [End][Chris YIM]
#End Region

#Region "Properties"
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

        Public Property TentativeEmail() As String
            Get
                Return _strTentativeEmail
            End Get
            Set(ByVal value As String)
                _strTentativeEmail = value
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

        Public Property RecordStatusEnum() As RecordStatusEnumClass
            Get
                Return Formatter.StringToEnum(GetType(RecordStatusEnumClass), _strRecordStatus)
            End Get
            Set(value As RecordStatusEnumClass)
                _strRecordStatus = Formatter.EnumToString(value)
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

        Public Property AlreadyJoinEHR() As String
            Get
                Return _strAlreadyJoinedEHR
            End Get
            Set(ByVal value As String)
                _strAlreadyJoinedEHR = value
            End Set
        End Property

        'Integration Start

        Public Property JoinPCD() As String
            Get
                Return _strJoinPCD
            End Get
            Set(ByVal value As String)
                _strJoinPCD = value
            End Set
        End Property

        Public Property PCDAccountStatus() As String
            Get
                Return _strPCDAccountStatus
            End Get
            Set(ByVal value As String)
                _strPCDAccountStatus = value
            End Set
        End Property

        Public Property PCDEnrolmentStatus() As String
            Get
                Return _strPCDEnrolmentStatus
            End Get
            Set(ByVal value As String)
                _strPCDEnrolmentStatus = value
            End Set
        End Property

        Public Property PCDProfessional() As String
            Get
                Return _strPCDProfessional
            End Get
            Set(ByVal value As String)
                _strPCDProfessional = value
            End Set
        End Property

        Public Property PCDStatusLastCheckDtm() As Nullable(Of DateTime)
            Get
                Return _dtmPCDStatusLastCheckDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmPCDStatusLastCheckDtm = value
            End Set
        End Property

        'Integration End

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

        Public Property TokenReturnDtm() As Nullable(Of DateTime)
            Get
                Return _dtmTokenReturnDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmTokenReturnDtm = value
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

        Public Property ERNProcessedList() As ERNProcessedModelCollection
            Get
                Return _udtERNProcessedList
            End Get
            Set(ByVal value As ERNProcessedModelCollection)
                _udtERNProcessedList = value
            End Set
        End Property

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Property ThirdPartyAdditionalFieldEnrolmentList() As ThirdPartyAdditionalFieldEnrolmentCollection
            Get
                Return _udtThirdPartyAdditionalFieldEnrolmentList
            End Get
            Set(ByVal value As ThirdPartyAdditionalFieldEnrolmentCollection)
                _udtThirdPartyAdditionalFieldEnrolmentList = value
            End Set
        End Property
        ' CRE12-001 eHS and PCD integration [End][Koala]
#End Region

#Region "Constructors"

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
            _strTentativeEmail = udtServiceProviderModel.TentativeEmail
            _strEmailChanged = udtServiceProviderModel.EmailChanged
            _strRecordStatus = udtServiceProviderModel.RecordStatus

            _strRemark = udtServiceProviderModel.Remark
            _strSubmitMethod = udtServiceProviderModel.SubmitMethod
            _strAlreadyJoinedEHR = udtServiceProviderModel.AlreadyJoinEHR
            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' ==========================================================
            _strJoinPCD = udtServiceProviderModel.JoinPCD
            ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
            _strUnderModification = udtServiceProviderModel.UnderModification
            _strApplicationPrinted = udtServiceProviderModel.ApplicationPrinted
            _dtmCreateDtm = udtServiceProviderModel.CreateDtm
            _strCreateBy = udtServiceProviderModel.CreateBy
            _dtmUpdateDtm = udtServiceProviderModel.UpdateDtm
            _strUpdateBy = udtServiceProviderModel.UpdateBy

            _dtmEffectiveDtm = udtServiceProviderModel.EffectiveDtm

            _dtmTokenReturnDtm = udtServiceProviderModel.TokenReturnDtm
            _byteTSMP = udtServiceProviderModel.TSMP

            _practiceList = udtServiceProviderModel.PracticeList
            _udtSchemeInfoList = udtServiceProviderModel.SchemeInfoList
            '_bankAcctList = objServiceProviderModel.BankAcctList
            _udtMOList = udtServiceProviderModel.MOList
            _udtERNProcessedList = udtServiceProviderModel.ERNProcessedList
            ' CRE12-001 eHS and PCD integration [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            _udtThirdPartyAdditionalFieldEnrolmentList = udtServiceProviderModel.ThirdPartyAdditionalFieldEnrolmentList
            ' CRE12-001 eHS and PCD integration [End][Koala]

            ' INT13-0028 - SP Amendment Report [Start][Tommy L]
            ' -------------------------------------------------------------------------
            _strDataInputBy = udtServiceProviderModel.DataInputBy
            _dtmDataInputEffectDtm = udtServiceProviderModel.DataInputEffectDtm
            ' INT13-0028 - SP Amendment Report [End][Tommy L]

            ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
            _strPCDAccountStatus = udtServiceProviderModel.PCDAccountStatus
            _strPCDEnrolmentStatus = udtServiceProviderModel.PCDEnrolmentStatus
            _strPCDProfessional = udtServiceProviderModel.PCDProfessional
            _dtmPCDStatusLastCheckDtm = udtServiceProviderModel.PCDStatusLastCheckDtm
            ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---
        End Sub

        'Integration Start

        Public Sub New(ByVal strEnrolRefNo As String, ByVal dtmEnrolDate As Nullable(Of DateTime), ByVal strSPID As String, ByVal strAliasAccount As String, ByVal strHKID As String, _
                       ByVal strEName As String, ByVal strCName As String, ByVal Address As AddressModel, ByVal strPhone As String, _
                       ByVal strFax As String, ByVal strEmail As String, ByVal strTentativeEmail As String, ByVal strEmailChanged As String, ByVal strRecordStatus As String, _
                       ByVal strRemark As String, ByVal strSubmitMethod As String, ByVal strAlreadyJoinedEHR As String, ByVal strJoinPCD As String, _
                       ByVal strUnderModification As String, ByVal strApplicationPrinted As String, ByVal dtmCreateDtm As Nullable(Of DateTime), ByVal strCreateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), _
                       ByVal strUpdateBy As String, ByVal dtmEffectiveDtm As Nullable(Of DateTime), ByVal dtmTokenReturnDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte(), ByVal udtPracticeModelCollection As PracticeModelCollection, _
                       ByVal udtSchemeInfoList As SchemeInformationModelCollection, ByVal udtMOList As MedicalOrganizationModelCollection, _
                       ByVal strDataInputBy As String, ByVal dtmDataInputEffectDtm As Nullable(Of DateTime),
                       ByVal strPCDAccountStatus As String, ByVal strPCDEnrolmentStatus As String, ByVal strPCDProfessional As String, ByVal dtmPCDStatusLastCheckDtm As Nullable(Of DateTime))

            _strDataInputBy = strDataInputBy
            _dtmDataInputEffectDtm = dtmDataInputEffectDtm

            ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
            _strPCDAccountStatus = strPCDAccountStatus
            _strPCDEnrolmentStatus = strPCDEnrolmentStatus
            _strPCDProfessional = strPCDProfessional
            _dtmPCDStatusLastCheckDtm = dtmPCDStatusLastCheckDtm
            ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---

            Constructor(strEnrolRefNo, dtmEnrolDate, strSPID, strAliasAccount, strHKID, _
                        strEName, strCName, Address, strPhone, _
                        strFax, strEmail, strTentativeEmail, strEmailChanged, strRecordStatus, _
                        strRemark, strSubmitMethod, strAlreadyJoinedEHR, strJoinPCD, _
                        strUnderModification, strApplicationPrinted, dtmCreateDtm, strCreateBy, dtmUpdateDtm, _
                        strUpdateBy, dtmEffectiveDtm, dtmTokenReturnDtm, byteTSMP, udtPracticeModelCollection, _
                        udtSchemeInfoList, udtMOList)
        End Sub

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
        ' ==========================================================
        Public Sub Constructor(ByVal strEnrolRefNo As String, ByVal dtmEnrolDate As Nullable(Of DateTime), ByVal strSPID As String, ByVal strAliasAccount As String, ByVal strHKID As String, _
                       ByVal strEName As String, ByVal strCName As String, ByVal Address As AddressModel, ByVal strPhone As String, _
                       ByVal strFax As String, ByVal strEmail As String, ByVal strTentativeEmail As String, ByVal strEmailChanged As String, ByVal strRecordStatus As String, _
                       ByVal strRemark As String, ByVal strSubmitMethod As String, ByVal strAlreadyJoinedEHR As String, ByVal strJoinPCD As String, _
                       ByVal strUnderModification As String, ByVal strApplicationPrinted As String, ByVal dtmCreateDtm As Nullable(Of DateTime), ByVal strCreateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), _
                       ByVal strUpdateBy As String, ByVal dtmEffectiveDtm As Nullable(Of DateTime), ByVal dtmTokenReturnDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte(), ByVal udtPracticeModelCollection As PracticeModelCollection, _
                       ByVal udtSchemeInfoList As SchemeInformationModelCollection, ByVal udtMOList As MedicalOrganizationModelCollection)

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
            _strTentativeEmail = strTentativeEmail
            _strEmailChanged = strEmailChanged
            _strRecordStatus = strRecordStatus
            _strRemark = strRemark
            _strSubmitMethod = strSubmitMethod
            _strAlreadyJoinedEHR = strAlreadyJoinedEHR
            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' ==========================================================
            _strJoinPCD = strJoinPCD
            ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
            _strUnderModification = strUnderModification
            _strApplicationPrinted = strApplicationPrinted
            _dtmCreateDtm = dtmCreateDtm
            _strCreateBy = strCreateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strUpdateBy = strUpdateBy
            _dtmEffectiveDtm = dtmEffectiveDtm
            _dtmTokenReturnDtm = dtmTokenReturnDtm
            _byteTSMP = byteTSMP

            _practiceList = udtPracticeModelCollection
            _udtSchemeInfoList = udtSchemeInfoList
            '_bankAcctList = bankAcctList
            _udtMOList = udtMOList


        End Sub
        ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
#End Region

#Region "Functions"

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Public Sub FilterByHCSPSubPlatform(ByVal enumSubPlatform As EnumHCSPSubPlatform)
            Me.SchemeInfoList = Me.SchemeInfoList.FilterByHCSPSubPlatform(enumSubPlatform)

            Dim udtPracticeList As New PracticeModelCollection

            For Each udtPractice As PracticeModel In Me.PracticeList.Values
                udtPractice.PracticeSchemeInfoList = udtPractice.PracticeSchemeInfoList.FilterByHCSPSubPlatform(enumSubPlatform)

                If udtPractice.PracticeSchemeInfoList.Count > 0 Then
                    udtPracticeList.Add(New PracticeModel(udtPractice))
                End If

            Next

            Me.PracticeList = udtPracticeList

        End Sub
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

#End Region

    End Class
End Namespace
