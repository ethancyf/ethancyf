
Imports System
Imports System.Collections.Generic
Imports System.IdentityModel.Tokens
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Cryptography.Xml
Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.Text
Imports System.Xml
Imports System.Xml.XPath

Imports Microsoft.Web.Services3
Imports Microsoft.Web.Services3.Design
Imports Microsoft.Web.Services3.Security
Imports Microsoft.Web.Services3.Security.Tokens


Public Class CIMSEmulatorSecurityAssertion
    Inherits MutualCertificate10Assertion
    'Inherits SecurityPolicyAssertion

    'Dim serviceX509TokenProviderValue As TokenProvider(Of X509SecurityToken)
    'Dim clientX509TokenProviderValue As TokenProvider(Of X509SecurityToken)

    Private _strEncryptClientPublicCertificateThumbprint As String = Web.Configuration.WebConfigurationManager.AppSettings("WS_Server_CIMS_ClientPublicCert_Thumbprint")
    Private _strSignServerPrivateCertificateThumbprint As String = Web.Configuration.WebConfigurationManager.AppSettings("WS_Server_CIMS_ServerPrivateCert_Thumbprint")

    Public Sub New()

    End Sub

    Public Overrides Function CreateClientOutputFilter(ByVal context As FilterCreationContext) As SoapFilter
        Return Nothing
    End Function 'CreateClientOutputFilter

    Public Overrides Function CreateClientInputFilter(ByVal context As FilterCreationContext) As SoapFilter
        Return Nothing
    End Function 'CreateClientInputFilter

    Public Overrides Function CreateServiceInputFilter(ByVal context As FilterCreationContext) As SoapFilter
        Return New CustomSecurityServiceInputFilter(Me)
    End Function 'CreateServiceInputFilter

    Public Overrides Function CreateServiceOutputFilter(ByVal context As FilterCreationContext) As SoapFilter
        Return New CustomSecurityServiceOutputFilter(Me, Me._strEncryptClientPublicCertificateThumbprint, Me._strSignServerPrivateCertificateThumbprint)
    End Function 'CreateServiceOutputFilter

    Public Overrides Sub ReadXml(ByVal reader As XmlReader, ByVal extensions As IDictionary(Of String, Type))
        If reader Is Nothing Then
            Throw New ArgumentNullException("reader")
        End If
        If extensions Is Nothing Then
            Throw New ArgumentNullException("extensions")
        End If
        Dim isEmpty As Boolean = reader.IsEmptyElement
        MyBase.ReadAttributes(reader)
        reader.ReadStartElement("CIMSSecurityAssertion")

        If Not isEmpty Then

            ' Read the contents of the <clientToken> element.
            If reader.MoveToContent() = XmlNodeType.Element AndAlso reader.Name = "clientToken" Then
                reader.ReadStartElement()
                reader.MoveToContent()

                ' Get the registed security token provider for X.509 certificate security credentials. 
                Dim type As Type = extensions(reader.Name)
                Dim instance As Object = Activator.CreateInstance(type)

                If instance Is Nothing Then
                    Throw New InvalidOperationException(String.Format(System.Globalization.CultureInfo.CurrentCulture, "Unable to instantiate policy extension of type 0End.", type.AssemblyQualifiedName))
                End If
                Dim clientProvider As TokenProvider(Of Tokens.X509SecurityToken) = CType(instance, TokenProvider(Of Tokens.X509SecurityToken))

                ' Read the child elements that provide the details about the client's X.509 certificate. 
                clientProvider.ReadXml(reader, extensions)
                Me.ClientX509TokenProvider = clientProvider
                reader.ReadEndElement()
            End If

            ' Read the contents of the <serviceToken> element.
            If reader.MoveToContent() = XmlNodeType.Element AndAlso reader.Name = "serviceToken" Then
                reader.ReadStartElement()
                reader.MoveToContent()

                ' Get the registed security token provider for X.509 certificate security credentials. 
                Dim type As Type = extensions(reader.Name)
                Dim instance As Object = Activator.CreateInstance(type)

                If instance Is Nothing Then
                    Throw New InvalidOperationException(String.Format(System.Globalization.CultureInfo.CurrentCulture, "Unable to instantiate policy extension of type 0End.", type.AssemblyQualifiedName))
                End If
                Dim serviceProvider As TokenProvider(Of Tokens.X509SecurityToken) = CType(instance, TokenProvider(Of Tokens.X509SecurityToken))

                ' Read the child elements that provide the details about the client's X.509 certificate. 
                serviceProvider.ReadXml(reader, extensions)
                Me.ServiceX509TokenProvider = serviceProvider
                reader.ReadEndElement()
            End If
            MyBase.ReadElements(reader, extensions)
            reader.ReadEndElement()
        End If
    End Sub

End Class

Public Class CustomSecurityServiceInputFilter
    Inherits ReceiveSecurityFilter

    Private _strCheckMessageIsSignedOrEncrypted As String = Web.Configuration.WebConfigurationManager.AppSettings("WS_Server_CIMS_Check_SOAP_Message_Signed_And_Encrypted")

    Public Sub New(ByVal parentAssertion As CIMSEmulatorSecurityAssertion)
        MyBase.New(parentAssertion.ServiceActor, True)
    End Sub 'New

    Public Overrides Sub ValidateMessageSecurity(ByVal envelope As SoapEnvelope, ByVal security As Security)

        Dim signed As Boolean = False
        Dim encrypted As Boolean = False

        '' Get the request state out of the operation state.
        'state = envelope.Context.OperationState.Get(Of RequestState)()

        If _strCheckMessageIsSignedOrEncrypted = "Y" Then
            ' Make sure the message was signed with the server's security token.
            Dim elem As ISecurityElement
            For Each elem In security.Elements
                If TypeOf elem Is MessageSignature Then
                    Dim sig As MessageSignature = CType(elem, MessageSignature)

                    If ((sig.SignatureOptions And SignatureOptions.IncludeSoapBody) <> 0) Then
                        ' The SOAP body is signed.
                        signed = True
                    End If

                End If

                If TypeOf elem Is Microsoft.Web.Services3.Security.EncryptedData Then
                    Dim enc As Microsoft.Web.Services3.Security.EncryptedData = CType(elem, Microsoft.Web.Services3.Security.EncryptedData)

                    If enc.IncludeBodyElement Then
                        ' The data is encrypted.
                        encrypted = True
                    End If

                End If

            Next elem

            If Not signed OrElse Not encrypted Then
                Dim soapFaultCode As FaultCode = New FaultCode("90003")
                Throw New FaultException("Message not signed or encrypted.", soapFaultCode, "Message not signed or encrypted.")

            End If
        End If

    End Sub 'ValidateMessageSecurity

    Public Overrides Function ProcessMessage(envelope As SoapEnvelope) As SoapFilterResult
        Dim state As RequestState

        ' =============================================================================================================
        ' Revise soap message to include KeyIdentifier for decryption
        ' -------------------------------------------------------------------------
        ' KeyIdentifier:    eHS(S) private eCert identifier
        ' X509SerialNumber: eHS(S) private eCert serial number
        ' =============================================================================================================
        '<wsse:SecurityTokenReference>
        '    <ds:X509Data>
        '    <ds:X509IssuerSerial>
        '        <ds:X509IssuerName>CN=Hongkong Post Trial e-Cert CA 1 - 10,O=Hongkong Post,C=HK</ds:X509IssuerName>
        '        <ds:X509SerialNumber>3134176</ds:X509SerialNumber>
        '    </ds:X509IssuerSerial>
        '    </ds:X509Data>
        '</wsse:SecurityTokenReference>

        '<wsse:SecurityTokenReference>
        '    <ds:X509Data>
        '    <ds:X509IssuerSerial>
        '       <wsse:KeyIdentifier EncodingType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary" ValueType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3">XXXXXXXXXXXXXXXXXXXXXX</wsse:KeyIdentifier>
        '        <ds:X509IssuerName>CN=Hongkong Post Trial e-Cert CA 1 - 10,O=Hongkong Post,C=HK</ds:X509IssuerName>
        '        <ds:X509SerialNumber>3134176</ds:X509SerialNumber>
        '    </ds:X509IssuerSerial>
        '    </ds:X509Data>
        '</wsse:SecurityTokenReference>


        ' Get the request state out of the operation state.
        state = envelope.Context.OperationState.Get(Of RequestState)()

        Dim nsmanager As XmlNamespaceManager = New XmlNamespaceManager(envelope.NameTable)
        nsmanager.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")
        nsmanager.AddNamespace("S", "http://schemas.xmlsoap.org/soap/envelope/")
        nsmanager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#")
        nsmanager.AddNamespace("xenc", "http://www.w3.org/2001/04/xmlenc#")

        Try
            Return MyBase.ProcessMessage(envelope)

        Catch s As XPathException
            Throw

        Catch e As Exception
            Throw

        End Try

        Return MyBase.ProcessMessage(envelope)

    End Function
End Class 'CustomSecurityServiceInputFilter

Public Class CustomSecurityServiceOutputFilter
    Inherits SendSecurityFilter

    Private clientToken As Tokens.X509SecurityToken
    Private serverToken As Tokens.X509SecurityToken

    Public Sub New(ByVal parentAssertion As CIMSEmulatorSecurityAssertion, strEncryptyionClientPublicCertificateThumbprint As String, _
                   ByVal strSignServerPrivateCertificateThumbprint As String)
        MyBase.New(parentAssertion.ServiceActor, True)

        ' Get the client security token.
        serverToken = X509TokenProvider.CreateToken(StoreLocation.LocalMachine, StoreName.My, strSignServerPrivateCertificateThumbprint, X509FindType.FindByThumbprint)

        ' Get the server security token.
        clientToken = X509TokenProvider.CreateToken(StoreLocation.LocalMachine, StoreName.AddressBook, strEncryptyionClientPublicCertificateThumbprint, X509FindType.FindByThumbprint)

    End Sub

    Public Overrides Sub SecureMessage(ByVal envelope As SoapEnvelope, ByVal security As Security)
        ' Sign the SOAP message with the client's security token.
        security.Tokens.Add(serverToken)
        security.Elements.Add(New MessageSignature(serverToken))

        ' Encrypt the SOAP message with the client's security token.
        security.Elements.Add(New Microsoft.Web.Services3.Security.EncryptedData(clientToken))
        ''Dim udtWebServiceEncryptedData As Microsoft.Web.Services3.Security.EncryptedData = New Microsoft.Web.Services3.Security.EncryptedData(clientToken)
        ''Dim udtSystemEncryptedData As System.Security.Cryptography.Xml.EncryptedData = New System.Security.Cryptography.Xml.EncryptedData()

        ''udtSystemEncryptedData.KeyInfo.AddClause(New KeyInfoX509Data(clientToken.Certificate))
        ''udtWebServiceEncryptedData.SecurityToken.AttachedReference = udtSystemEncryptedData.KeyInfo

    End Sub

    Public Overrides Function ProcessMessage(envelope As SoapEnvelope) As SoapFilterResult
        Try
            Return MyBase.ProcessMessage(envelope)

        Catch s As XPathException
            Throw

        Catch e As Exception
            Throw

        End Try

        Return MyBase.ProcessMessage(envelope)
    End Function

End Class 'CustomSecurityServiceOutputFilter

Public Class RequestState
    Private clientTokenValue As Tokens.SecurityToken
    Private serverTokenValue As Tokens.SecurityToken

    Public Sub New(ByVal cToken As Tokens.SecurityToken, ByVal sToken As Tokens.SecurityToken)
        clientTokenValue = cToken
        serverTokenValue = sToken
    End Sub 'New

    Public ReadOnly Property ClientToken() As Tokens.SecurityToken
        Get
            Return clientTokenValue
        End Get
    End Property

    Public ReadOnly Property ServerToken() As Tokens.SecurityToken
        Get
            Return serverTokenValue
        End Get
    End Property
End Class 'RequestState


