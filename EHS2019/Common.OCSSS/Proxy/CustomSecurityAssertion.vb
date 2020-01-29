Imports System.Security.Cryptography.X509Certificates
Imports Microsoft.Web.Services3
Imports Microsoft.Web.Services3.Design
Imports Microsoft.Web.Services3.Security
Imports Microsoft.Web.Services3.Security.Tokens
Imports System.Xml

Public Class CustomSecurityAssertion
    Inherits SecurityPolicyAssertion

    Private _strAuthToken As String

    Public Sub New(strAuthToken As String)
        _strAuthToken = strAuthToken
    End Sub

    Public Overrides Function CreateServiceInputFilter(ByVal context As FilterCreationContext) As SoapFilter
        Return Nothing
    End Function

    Public Overrides Function CreateServiceOutputFilter(ByVal context As FilterCreationContext) As SoapFilter
        Return Nothing
    End Function

    Public Overrides Function CreateClientInputFilter(ByVal context As FilterCreationContext) As SoapFilter
        Return Nothing
    End Function

    Public Overrides Function CreateClientOutputFilter(ByVal context As FilterCreationContext) As SoapFilter
        Return New CustomSecurityClientOutputFilter(Me, _strAuthToken)
    End Function

End Class

Public Class CustomSecurityClientOutputFilter
    Inherits SendSecurityFilter

    Private clientToken As SecurityToken
    Private _strAuthToken As String

    Public Sub New(ByVal parentAssertion As CustomSecurityAssertion, ByVal strAuthToken As String)
        MyBase.New(parentAssertion.ServiceActor, True)

        _strAuthToken = strAuthToken
    End Sub

    Public Overrides Sub SecureMessage(ByVal envelope As SoapEnvelope, ByVal security As Microsoft.Web.Services3.Security.Security)

        ' Setup Soap Header for OCSSS
        ' =============================================================================
        ' <soap:Header>
        '       <wsse:Security xmlns:wsse="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
        '           <wsse:UsernameToken>
        '               <wsse:AuthToken>9908abd7d63d2fc15eb77754abc384f87b2608aba054f8cb549e7ee4da529758</wsse:AuthToken>
        '           </wsse:UsernameToken>
        '       </wsse:Security>
        '</soap:Header>
        ' =============================================================================
        Dim strNamespaceURL As String = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"
        Dim oHeaderSecurity As XmlElement = envelope.CreateElement("wsse", "Security", strNamespaceURL)
        Dim oHeaderUsernameToken As XmlElement = envelope.CreateElement("wsse", "UsernameToken", strNamespaceURL)
        Dim oHeaderAuthToken As XmlElement = envelope.CreateElement("wsse", "AuthToken", strNamespaceURL)
        oHeaderAuthToken.InnerText = _strAuthToken
        oHeaderSecurity.AppendChild(oHeaderUsernameToken)
        oHeaderUsernameToken.AppendChild(oHeaderAuthToken)
        envelope.Header.AppendChild(oHeaderSecurity)

        ' Remove unuse Soap Header
        envelope.Header.RemoveChild(envelope.Header("wsa:Action"))
        envelope.Header.RemoveChild(envelope.Header("wsa:MessageID"))
        envelope.Header.RemoveChild(envelope.Header("wsa:ReplyTo"))
        envelope.Header.RemoveChild(envelope.Header("wsa:To"))
    End Sub



End Class
