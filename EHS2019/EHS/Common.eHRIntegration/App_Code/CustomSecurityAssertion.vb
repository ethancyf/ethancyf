Imports System.Security.Cryptography.X509Certificates
Imports Microsoft.Web.Services3
Imports Microsoft.Web.Services3.Design
Imports Microsoft.Web.Services3.Security
Imports Microsoft.Web.Services3.Security.Tokens

Public Class CustomSecurityAssertion
    Inherits SecurityPolicyAssertion

    Private _strCertificateThumbprint As String

    Public Sub New(strCertificateThumbprint As String)
        _strCertificateThumbprint = strCertificateThumbprint
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
        Return New CustomSecurityClientOutputFilter(Me, _strCertificateThumbprint)
    End Function

End Class

Public Class CustomSecurityClientOutputFilter
    Inherits SendSecurityFilter

    Private clientToken As SecurityToken

    Public Sub New(ByVal parentAssertion As CustomSecurityAssertion, strCertificateThumbprint As String)
        MyBase.New(parentAssertion.ServiceActor, True)
       
        clientToken = GetSecurityToken(strCertificateThumbprint)

    End Sub

    Public Overrides Sub SecureMessage(ByVal envelope As SoapEnvelope, ByVal security As Microsoft.Web.Services3.Security.Security)
        security.Tokens.Add(clientToken)
        security.Elements.Add(New MessageSignature(clientToken))

        envelope.Header.RemoveChild(envelope.Header("wsa:Action"))
        envelope.Header.RemoveChild(envelope.Header("wsa:MessageID"))
        envelope.Header.RemoveChild(envelope.Header("wsa:ReplyTo"))
        envelope.Header.RemoveChild(envelope.Header("wsa:To"))

    End Sub

    Private Function GetSecurityToken(strThumbprint As String) As SecurityToken
        Dim securityToken As X509SecurityToken = Nothing
        Dim store As New X509Store(StoreName.My, StoreLocation.LocalMachine)

        store.Open(OpenFlags.ReadOnly)

        Try
            Dim certs As X509Certificate2Collection = store.Certificates.Find(X509FindType.FindByThumbprint, strThumbprint, False)

            If certs.Count = 1 Then
                securityToken = New X509SecurityToken(certs(0))
            Else
                securityToken = Nothing
            End If

        Catch ex As Exception
            securityToken = Nothing

        Finally
            If Not (store Is Nothing) Then
                store.Close()
            End If

        End Try

        Return securityToken

    End Function

End Class
