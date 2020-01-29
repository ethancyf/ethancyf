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

End Namespace
