Imports System.Xml
Imports System.Xml.Serialization

Namespace Model.Xml.eHSService

    <XmlRoot("root")>
    Public Class InSubmitRequestXmlModel

        Public data As String

    End Class

    <XmlRoot("geteHSSTokenInfo")>
    Public Class InGeteHSSTokenInfoXmlModel

        Public HKID As String
        Public Timestamp As String

    End Class

    <XmlRoot("seteHSSTokenShared")>
    Public Class InSeteHSSTokenSharedXmlModel

        Public HKID As String
        Public ExistingTokenID As String
        Public NewTokenID As String
        Public [Shared] As String
        Public Timestamp As String

    End Class

    <XmlRoot("replaceeHSSToken")>
    Public Class InReplaceeHSSTokenXmlModel

        Public HKID As String
        Public ExistingTokenID As String
        Public NewTokenID As String
        Public ReplaceReasonCode As String
        Public Timestamp As String

    End Class

    <XmlRoot("notifyeHSSTokenDeactivated")>
    Public Class InNotifyeHSSTokenDeactivatedXmlModel

        Public HKID As String
        Public ExistingTokenID As String
        Public NewTokenID As String
        Public DeactivateReasonCode As String
        Public Timestamp As String

    End Class

    <XmlRoot("geteHSSLoginAlias")>
    Public Class InGeteHSSLoginAliasXmlModel

        Public HKID As String
        Public Timestamp As String

    End Class

    <XmlRoot("healthCheckeHSS")>
    Public Class InHealthCheckeHSSXmlModel

        Public Timestamp As String

    End Class

    ' CRE18-XXX (Provide data to eHR Portal) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    <XmlRoot("geteHSSDoctorList")>
    Public Class InGeteHSSDoctorListXmlModel
        Public Timestamp As String

    End Class
    ' CRE18-XXX (Provide data to eHR Portal) [End][Chris YIM]

    ' CRE18-XXX (Provide data to eHR Portal) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    <XmlRoot("geteHSSVoucherBalance")>
    Public Class InGeteHSSVoucherBalanceXmlModel
        Public HKID As String
        Public DocType As String
        Public DOBFormat As String
        Public DOB As String
        Public EC_Age As String
        Public EC_RegDate As String
        Public Timestamp As String

    End Class
    ' CRE18-XXX (Provide data to eHR Portal) [End][Chris YIM]

    ' CRE20-006 Set the DHC NSP & Claim access [Start][Nichole]
    ' --------------------------------------------------------------------------------------
    <XmlRoot("seteHSSDHCNSP")>
    Public Class InSeteHSSDHCNSPXmlModel
        Public Timestamp As String
        Public UploadDistrictCode As String
        <XmlElement("ProfList")>
        Public ProfList As ProfListClass


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

            Private _ProfDistrictCode As String
            Private _ProfCode As String
            Private _ProfRegNo As String
            'Private _EnrolledInEHS As String


            'Public Property EnrolledInEHS As String
            '    Get
            '        Return _EnrolledInEHS
            '    End Get
            '    Set(value As String)
            '        _EnrolledInEHS = value
            '    End Set
            'End Property

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

        End Class
#End Region
    End Class

    <XmlRoot("geteHSSDHCClaimAccess")>
    Public Class InGeteHSSDHCClaimAccessXMLModel
        <XmlElement("ServiceProvider")>
        Public ServiceProvider As ServiceProviderClass
        <XmlElement("Patient")>
        Public Patient As PatientClass
        <XmlElement("VoucherClaim")>
        Public VoucherClaim As VoucherClaimClass
        Public Timestamp As String

#Region "Sub Class ServiceProviderClass"
        Public Class ServiceProviderClass
            Private _ProfCode As String
            Private _ProfRegNo As String

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
        End Class
#End Region

#Region "Sub Class Patient"
        Public Class PatientClass
            Private _HKID As String
            Private _DocType As String
            Private _HKICSymbol As String
            Private _DOBFormat As String
            Private _DOB As String
            Private _DHCDistrictCode As String

            Public Property HKID As String
                Get
                    Return _HKID
                End Get
                Set(value As String)
                    _HKID = value
                End Set
            End Property

            Public Property DocType As String
                Get
                    Return _DocType
                End Get
                Set(value As String)
                    _DocType = value
                End Set
            End Property

            Public Property HKICSymbol As String
                Get
                    Return _HKICSymbol
                End Get
                Set(value As String)
                    _HKICSymbol = value
                End Set
            End Property

            Public Property DOBFormat As String
                Get
                    Return _DOBFormat
                End Get
                Set(value As String)
                    _DOBFormat = value
                End Set
            End Property

            Public Property DOB As String
                Get
                    Return _DOB
                End Get
                Set(value As String)
                    _DOB = value
                End Set
            End Property

            Public Property DHCDistrictCode As String
                Get
                    Return _DHCDistrictCode
                End Get
                Set(value As String)
                    _DHCDistrictCode = value
                End Set
            End Property
        End Class
#End Region

#Region "Sub Class VoucherClaimClass"
        Public Class VoucherClaimClass
            Private _ClaimAmount As String


            Public Property ClaimAmount As String
                Get
                    Return _ClaimAmount
                End Get
                Set(value As String)
                    _ClaimAmount = value
                End Set
            End Property
        End Class
#End Region
    End Class
    'CRE20-006 Set the DHC NSP & Claim Access model [End][Nichole]
End Namespace
