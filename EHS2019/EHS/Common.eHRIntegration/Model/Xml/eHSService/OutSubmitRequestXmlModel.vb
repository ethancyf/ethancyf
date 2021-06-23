Imports System.Xml
Imports System.Xml.Serialization

Namespace Model.Xml.eHSService

#Region "Enum Constants"

    Public Enum eHSResultCode
        NA
        R1000_Success = 1000
        R1001_NoTokenAssigned = 1001
        R1002_TokenNotMatch = 1002
        R1005_ExistingTokenNotIssuedBySenderParty = 1005
        R1006_NewTokenNotAvailable = 1006
        R2001_LoginAliasNotSet = 2001
        R9000_InvalidXmlElement = 9000
        R9001_InvalidParameter = 9001
        R9002_UserNotFound = 9002
        R9999_UnexpectedFailure = 9999
    End Enum

    Public Enum eHSPatientPortalResultCode
        NA
        R1000_Success = 1000
        R1001_PatientNotFound = 1001
        R1002_PatientNotEligible = 1002
        R9000_InvalidXmlElement = 9000
        R9001_InvalidParameter = 9001
        R9990_InternalError = 9990
        R9999_UnexpectedFailure = 9999
    End Enum

    'CRE20-006 DHC [Start][Nichole]
    Public Enum eHSDHCNSPResultCode
        NA
        R1000_Success = 1000
        R1001_ProfessionalNotFound = 1001
        R1002_ProfessionalDelisted = 1002
        R9000_InvalidXmlElement = 9000
        R9001_InvalidParameter = 9001
        R9999_UnexpectedFailure = 9999
    End Enum

    Public Enum eHSDHCClaimAccessResultCode
        NA
        R1000_Success = 1000
        R1001_ProfessionalNotFound = 1001
        R1002_ProfessionalDelisted = 1002
        R1003_ProfessionalDisabled = 1003
        R9000_InvalidXmlElement = 9000
        R9001_InvalidParameter = 9001
        R9999_UnexpectedFailure = 9999
    End Enum
    'CRE20-16 DHC [End][Nichole]
#End Region

    <XmlRoot("root")>
    Public Class OutSubmitRequestXmlModel

        Public data As String

    End Class

    <XmlRoot("geteHSSTokenInfoResult")>
    Public Class OutGeteHSSTokenInfoXmlModel

        Public HKID As String
        Public Timestamp As String
        Public IsCommonUser As String
        Public ExistingTokenID As String
        Public ExistingTokenIssuer As String
        Public IsExistingTokenShared As String
        Public NewTokenID As String
        Public NewTokenIssuer As String
        Public IsNewTokenShared As String
        Public ResultDescription As String
        Public ResultCode As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutGeteHSSTokenInfoXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode, strHKID As String, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("seteHSSTokenSharedResult")>
    Public Class OutSeteHSSTokenSharedXmlModel

        Public HKID As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutSeteHSSTokenSharedXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode, strHKID As String, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("replaceeHRSSTokenResult")>
    Public Class OutReplaceeHRSSTokenXmlModel

        Public HKID As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutReplaceeHRSSTokenXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode, strHKID As String, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("notifyeHRSSTokenDeactivatedResult")>
    Public Class OutNotifyeHRSSTokenDeactivatedXmlModel

        Public HKID As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutNotifyeHRSSTokenDeactivatedXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode, strHKID As String, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("geteHSSLoginAliasResult")>
    Public Class OutGeteHSSLoginAliasXmlModel

        Public HKID As String
        Public LoginAlias As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutGeteHSSLoginAliasXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode, strHKID As String, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("healthCheckeHSSResult")>
    Public Class OutHealthCheckeHSSXmlModel

        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutHealthCheckeHSSXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            Timestamp = strTimestamp

        End Sub

    End Class

    ' CRE18-XXX (Provide data to eHR Portal) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    <XmlRoot("geteHSSDoctorListResult")>
    Public Class OutGeteHSSDoctorListXmlModel
        Private _strResult As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHSPatientPortalResultCode
            Get
                For Each e As eHSPatientPortalResultCode In System.Enum.GetValues(GetType(eHSPatientPortalResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutGeteHSSDoctorListXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Public Property Result As String
            Get
                Return _strResult
            End Get
            Set(value As String)
                _strResult = value
            End Set
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSPatientPortalResultCode, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            Timestamp = strTimestamp

        End Sub

    End Class
    ' CRE18-XXX (Provide data to eHR Portal) [End][Chris YIM]

    ' CRE20-16 Declare the output of DHC NSP & Claim Access XML model [Start][Nichole] 
    <XmlRoot("seteHSSDHCNSPResult")>
    Public Class OutSeteHSSDHCNSPXmlModel
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String
        <XmlElement("ProfList")>
        Public ProfList As ProfListClass

        Private Sub New()

        End Sub

        Public Sub New(enumeHSResultCode As eHSDHCNSPResultCode, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            Timestamp = strTimestamp
            ProfList = New ProfListClass

        End Sub


#Region "Sub Class ProfListClass"
        Public Class ProfListClass
            Private _ProfCount As Integer
            <XmlElement("Prof")>
            Public Prof() As ProfClass


            Public Property ProfCount As Integer
                Get
                    Return _ProfCount
                End Get
                Set(value As Integer)
                    _ProfCount = value
                End Set
            End Property
        End Class
#End Region

#Region "Sub Class ProfClass"
        Public Class ProfClass
            Private _ProfDistrictCode As String = String.Empty
            Private _ProfCode As String = String.Empty
            Private _ProfRegNo As String = String.Empty
            Private _EnrolledInEHS As String = String.Empty
            Public Sub New()

            End Sub
            Public Property ProfDistrictCode As String
                Get
                    Return _ProfDistrictCode
                End Get
                Set(value As String)
                    _ProfDistrictCode = value
                End Set
            End Property
            Public Property ProfCode As String
                Get
                    Return _ProfCode
                End Get
                Set(value As String)
                    _ProfCode = value
                End Set
            End Property
            Public Property ProfRegNo As String
                Get
                    Return _ProfRegNo
                End Get
                Set(value As String)
                    _ProfRegNo = value
                End Set
            End Property
            Public Property EnrolledInEHS As String
                Get
                    Return _EnrolledInEHS
                End Get
                Set(value As String)
                    _EnrolledInEHS = value
                End Set
            End Property
        End Class
#End Region
    End Class

    <XmlRoot("geteHSSDHCClaimAccessResult")>
    Public Class OutgeteHSSDHCClaimAccessXmlModel
        Public ResultDescription As String
        Public ResultCode As String
        Public EHSLoginURL As String
        Public Timestamp As String

        Private Sub New()

        End Sub

        Public Sub New(enumeHSResultCode As eHSDHCClaimAccessResultCode, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            Timestamp = strTimestamp


        End Sub

        Public Sub New(enumeHSResultCode As eHSDHCClaimAccessResultCode, strTimestamp As String, strEHSLoginURL As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            Timestamp = strTimestamp
            EHSLoginURL = strEHSLoginURL

        End Sub
    End Class
    ' CRE20-16  Declare the output of DHC NSP & Claim Access XML model  [End][Nichole]
    ' CRE18-XXX (Provide data to eHR Portal) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    <XmlRoot("geteHSSVoucherBalanceResult")>
    Public Class OutGeteHSSVoucherBalanceXmlModel
        Public ResultDescription As String
        Public ResultCode As String
        Public VoucherBalanceAmt As String
        Public MaxAccumulativeAmt As String
        Public QuotaList() As Quota
        Public Forfeit As ForfeitInfo
        Public TransactionHistory() As Transaction
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHSPatientPortalResultCode
            Get
                For Each e As eHSPatientPortalResultCode In System.Enum.GetValues(GetType(eHSPatientPortalResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutGeteHSSVoucherBalanceInfoXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSPatientPortalResultCode, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            Timestamp = strTimestamp

        End Sub

#Region "Sub Class Quota"
        Public Class Quota
            Private _ProfCode As String = String.Empty
            Private _Remain As String = String.Empty
            Private _MaxUsableRemain As String = String.Empty
            Private _RemainEndDate As String = String.Empty

            Public Property QuotaProfCode As String
                Get
                    Return _ProfCode
                End Get
                Set(value As String)
                    _ProfCode = value
                End Set
            End Property

            Public Property QuotaRemain As String
                Get
                    Return _Remain
                End Get
                Set(value As String)
                    _Remain = value
                End Set
            End Property

            Public Property QuotaMaxUsableRemain As String
                Get
                    Return _MaxUsableRemain
                End Get
                Set(value As String)
                    _MaxUsableRemain = value
                End Set
            End Property

            Public Property QuotaRemainEndDate As String
                Get
                    Return _RemainEndDate
                End Get
                Set(value As String)
                    _RemainEndDate = value
                End Set
            End Property

        End Class
#End Region

#Region "Sub Class Forfeit"
        Public Class ForfeitInfo
            Private _Deposit As String = String.Empty
            Private _Capping As String = String.Empty
            Private _ForfeitDate As String = String.Empty
            Private _Forfeit As String = String.Empty

            Public Property NextDepositAmount As String
                Get
                    Return _Deposit
                End Get
                Set(value As String)
                    _Deposit = value
                End Set
            End Property

            Public Property NextCappingAmount As String
                Get
                    Return _Capping
                End Get
                Set(value As String)
                    _Capping = value
                End Set
            End Property

            Public Property NextForfeitDate As String
                Get
                    Return _ForfeitDate
                End Get
                Set(value As String)
                    _ForfeitDate = value
                End Set
            End Property

            Public Property NextForfeitAmount As String
                Get
                    Return _Forfeit
                End Get
                Set(value As String)
                    _Forfeit = value
                End Set
            End Property

        End Class
#End Region

#Region "Sub Class Transaction"
        Public Class Transaction
            Private _ServiceDate As String = String.Empty
            Private _UsedVoucherAmt As String = String.Empty
            Private _ProfCode As String = String.Empty
            Private _SPEngName As String = String.Empty
            Private _SPChiName As String = String.Empty
            Private _PracticeEngName As String = String.Empty
            Private _PracticeChiName As String = String.Empty
            Private _PracticeEngAddress As String = String.Empty
            Private _PracticeChiAddress As String = String.Empty
            Private _PracticePhoneNumber As String = String.Empty

            Public Property ServiceDate As String
                Get
                    Return _ServiceDate
                End Get
                Set(value As String)
                    _ServiceDate = value
                End Set
            End Property

            Public Property UsedVoucherAmt As String
                Get
                    Return _UsedVoucherAmt
                End Get
                Set(value As String)
                    _UsedVoucherAmt = value
                End Set
            End Property

            Public Property ProfCode As String
                Get
                    Return _ProfCode
                End Get
                Set(value As String)
                    _ProfCode = value
                End Set
            End Property

            Public Property SPName_en As String
                Get
                    Return _SPEngName
                End Get
                Set(value As String)
                    _SPEngName = value
                End Set
            End Property

            Public Property SPName_tc As String
                Get
                    Return _SPChiName
                End Get
                Set(value As String)
                    _SPChiName = value
                End Set
            End Property

            Public Property PracticeName_en As String
                Get
                    Return _PracticeEngName
                End Get
                Set(value As String)
                    _PracticeEngName = value
                End Set
            End Property

            Public Property PracticeName_tc As String
                Get
                    Return _PracticeChiName
                End Get
                Set(value As String)
                    _PracticeChiName = value
                End Set
            End Property

            Public Property PracticeAddr_en As String
                Get
                    Return _PracticeEngAddress
                End Get
                Set(value As String)
                    _PracticeEngAddress = value
                End Set
            End Property

            Public Property PracticeAddr_tc As String
                Get
                    Return _PracticeChiAddress
                End Get
                Set(value As String)
                    _PracticeChiAddress = value
                End Set
            End Property

            Public Property PracticeTelNo As String
                Get
                    Return _PracticePhoneNumber
                End Get
                Set(value As String)
                    _PracticePhoneNumber = value
                End Set
            End Property


        End Class
#End Region

    End Class
    ' CRE18-XXX (Provide data to eHR Portal) [End][Chris YIM]

    <XmlRoot("errorResult")>
    Public Class OutErrorResultModel

        Public ResultDescription As String
        Public ResultCode As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutErrorResultModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)

        End Sub

    End Class

    '

    Public Class SupportFunction

        Public Shared Sub ConvertResultCode(enumeHSResultCode As eHSResultCode, ByRef strResultCode As String, ByRef strResultDescription As String)
            Select Case enumeHSResultCode
                Case eHSResultCode.NA

                Case eHSResultCode.R1000_Success
                    strResultCode = "1000"
                    strResultDescription = "Success"

                Case eHSResultCode.R1001_NoTokenAssigned
                    strResultCode = "1001"
                    strResultDescription = "No token is assigned"

                Case eHSResultCode.R1002_TokenNotMatch
                    strResultCode = "1002"
                    strResultDescription = "Token not match"

                Case eHSResultCode.R1005_ExistingTokenNotIssuedBySenderParty
                    strResultCode = "1005"
                    strResultDescription = "Existing token is not issued by sender party"

                Case eHSResultCode.R1006_NewTokenNotAvailable
                    strResultCode = "1006"
                    strResultDescription = "New token is not available"

                Case eHSResultCode.R2001_LoginAliasNotSet
                    strResultCode = "2001"
                    strResultDescription = "No username is set"

                Case eHSResultCode.R9000_InvalidXmlElement
                    strResultCode = "9000"
                    strResultDescription = "Invalid XML Element"

                Case eHSResultCode.R9001_InvalidParameter
                    strResultCode = "9001"
                    strResultDescription = "Invalid Parameter"

                Case eHSResultCode.R9002_UserNotFound
                    strResultCode = "9002"
                    strResultDescription = "User is not found"

                Case eHSResultCode.R9999_UnexpectedFailure
                    strResultCode = "9999"
                    strResultDescription = "Unexpected Failure"

                Case Else
                    Throw New Exception(String.Format("OutSubmitRequestXmlModel.SupportFunction.ConvertResultCode: Unexepcted value in eHSResultCode (enumeHSResultCode={0})", enumeHSResultCode.ToString))

            End Select

        End Sub

        Public Shared Sub ConvertResultCode(enumeHSResultCode As eHSPatientPortalResultCode, ByRef strResultCode As String, ByRef strResultDescription As String)
            Select Case enumeHSResultCode
                Case eHSPatientPortalResultCode.NA

                Case eHSPatientPortalResultCode.R1000_Success
                    strResultCode = "1000"
                    strResultDescription = "Success"

                Case eHSPatientPortalResultCode.R1001_PatientNotFound
                    strResultCode = "1001"
                    strResultDescription = "Patient Not Found"

                Case eHSPatientPortalResultCode.R1002_PatientNotEligible
                    strResultCode = "1002"
                    strResultDescription = "Patient Not Eligible"

                Case eHSPatientPortalResultCode.R9000_InvalidXmlElement
                    strResultCode = "9000"
                    strResultDescription = "Invalid XML Element"

                Case eHSPatientPortalResultCode.R9001_InvalidParameter
                    strResultCode = "9001"
                    strResultDescription = "Invalid Parameter"

                Case eHSPatientPortalResultCode.R9990_InternalError
                    strResultCode = "9990"
                    strResultDescription = "Internal Error"

                Case eHSPatientPortalResultCode.R9999_UnexpectedFailure
                    strResultCode = "9999"
                    strResultDescription = "Unexpected Failure"

                Case Else
                    Throw New Exception(String.Format("OutSubmitRequestXmlModel.SupportFunction.ConvertResultCode: Unexepcted value in eHSPatientPortalResultCode (enumeHSResultCode={0})", enumeHSResultCode.ToString))

            End Select

        End Sub
        'CRE20-16 DHC NSp [Start][Nichole]
        Public Shared Sub ConvertResultCode(enumeHSResultCode As eHSDHCNSPResultCode, ByRef strResultCode As String, ByRef strResultDescription As String)
            Select Case enumeHSResultCode
                Case eHSDHCNSPResultCode.NA

                Case eHSDHCNSPResultCode.R1000_Success
                    strResultCode = "1000"
                    strResultDescription = "Success"

                Case eHSDHCNSPResultCode.R1001_ProfessionalNotFound
                    strResultCode = "1001"
                    strResultDescription = "Professional not found"

                Case eHSDHCNSPResultCode.R1002_ProfessionalDelisted
                    strResultCode = "1002"
                    strResultDescription = "Professional has been suspended/delisted"

                Case eHSDHCNSPResultCode.R9000_InvalidXmlElement
                    strResultCode = "9000"
                    strResultDescription = "Invalid XML Element"

                Case eHSDHCNSPResultCode.R9001_InvalidParameter
                    strResultCode = "9001"
                    strResultDescription = "Invalid Parameter"

                Case eHSDHCNSPResultCode.R9999_UnexpectedFailure
                    strResultCode = "9999"
                    strResultDescription = "Unexpected Failure"

                Case Else
                    Throw New Exception(String.Format("OutSubmitRequestXmlModel.SupportFunction.ConvertResultCode: Unexepcted value in eHSResultCode (enumeHSResultCode={0})", enumeHSResultCode.ToString))

            End Select

        End Sub
        'CRE20-006 DHC NSp [Start][Nichole]
        'CRE20-006 DHC Claim Access [Start][Nichole]
        Public Shared Sub ConvertResultCode(enumeHSResultCode As eHSDHCClaimAccessResultCode, ByRef strResultCode As String, ByRef strResultDescription As String)
            Select Case enumeHSResultCode
                Case eHSDHCClaimAccessResultCode.NA

                Case eHSDHCClaimAccessResultCode.R1000_Success
                    strResultCode = "1000"
                    strResultDescription = "Success"

                Case eHSDHCClaimAccessResultCode.R1001_ProfessionalNotFound
                    strResultCode = "1001"
                    strResultDescription = "Professional not found"

                Case eHSDHCClaimAccessResultCode.R1002_ProfessionalDelisted
                    strResultCode = "1002"
                    strResultDescription = "Professional has been suspended/delisted"

                Case eHSDHCClaimAccessResultCode.R1003_ProfessionalDisabled
                    strResultCode = "1003"
                    strResultDescription = "Professional is not a enabled NSP"

                Case eHSDHCClaimAccessResultCode.R9000_InvalidXmlElement
                    strResultCode = "9000"
                    strResultDescription = "Invalid XML Element"

                Case eHSDHCClaimAccessResultCode.R9001_InvalidParameter
                    strResultCode = "9001"
                    strResultDescription = "Invalid Parameter"

                Case eHSDHCClaimAccessResultCode.R9999_UnexpectedFailure
                    strResultCode = "9999"
                    strResultDescription = "Unexpected Failure"

                Case Else
                    Throw New Exception(String.Format("OutSubmitRequestXmlModel.SupportFunction.ConvertResultCode: Unexepcted value in eHSResultCode (enumeHSResultCode={0})", enumeHSResultCode.ToString))

            End Select

        End Sub
        'CRE20-006 DHC Claim Access [Start][Nichole]
    End Class

End Namespace
