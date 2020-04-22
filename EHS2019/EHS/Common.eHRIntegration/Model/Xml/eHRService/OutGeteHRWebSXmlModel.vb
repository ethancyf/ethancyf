Imports System.Xml
Imports System.Xml.Serialization

Namespace Model.Xml.eHRService

    <XmlRoot("root")>
    Public Class OutGeteHRWebSXmlModel

        Public VerificationPass As String
        Public SysID As String
        Public servicecode As String
        Public data As String

    End Class

    <XmlRoot("geteHRSSTokenInfo")>
    Public Class OutGeteHRSSTokenInfoXmlModel

        Public HKID As String
        Public Timestamp As String

    End Class

    <XmlRoot("seteHRSSTokenShared")>
    Public Class OutSeteHRSSTokenSharedXmlModel

        Public HKID As String
        Public ExistingTokenID As String
        Public NewTokenID As String
        Public [Shared] As String
        Public Timestamp As String

    End Class

    <XmlRoot("replaceeHRSSToken")>
    Public Class OutReplaceeHRSSTokenXmlModel

        Public HKID As String
        Public ExistingTokenID As String
        Public NewTokenID As String
        Public ReplaceReasonCode As String
        Public Timestamp As String

    End Class

    <XmlRoot("notifyeHRSSTokenDeactivated")>
    Public Class OutNotifyeHRSSTokenDeactivatedXmlModel

        Public HKID As String
        Public ExistingTokenID As String
        Public NewTokenID As String
        Public DeactivateReasonCode As String
        Public Timestamp As String

    End Class

    <XmlRoot("geteHRSSLoginAlias")>
    Public Class OutGeteHRSSLoginAliasXmlModel

        Public HKID As String
        Public Timestamp As String

    End Class

    <XmlRoot("healthCheckeHRSS")>
    Public Class OutHealthCheckeHRSSXmlModel

        Public Timestamp As String

    End Class

End Namespace
