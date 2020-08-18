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

End Namespace
