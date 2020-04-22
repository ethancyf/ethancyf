
Imports System
Imports System.Collections.Generic
Imports System.Security.Cryptography.X509Certificates
Imports System.Security.Cryptography.Xml
Imports System.Text
Imports System.Xml
Imports System.Xml.XPath

Imports Microsoft.Web.Services3
Imports Microsoft.Web.Services3.Design
Imports Microsoft.Web.Services3.Security
Imports Microsoft.Web.Services3.Security.Tokens



Public Class HKIPVRSecurityAssertion
    Inherits SecurityPolicyAssertion
    'Dim serviceX509TokenProviderValue As TokenProvider(Of X509SecurityToken)
    'Dim clientX509TokenProviderValue As TokenProvider(Of X509SecurityToken)

    Private _strEncryptCertificateThumbprint As String
    Private _strSignCertificateThumbprint As String

    'Public Property ClientX509TokenProvider() As TokenProvider(Of X509SecurityToken)
    '    Get
    '        Return clientX509TokenProviderValue
    '    End Get
    '    Set(ByVal value As TokenProvider(Of X509SecurityToken))
    '        clientX509TokenProviderValue = value
    '    End Set
    'End Property

    'Public Property ServiceX509TokenProvider() As TokenProvider(Of X509SecurityToken)
    '    Get
    '        Return serviceX509TokenProviderValue
    '    End Get
    '    Set(ByVal value As TokenProvider(Of X509SecurityToken))
    '        serviceX509TokenProviderValue = value
    '    End Set
    'End Property



    Public Property EncryptCertificateThumbprint() As String
        Get
            Return _strEncryptCertificateThumbprint
        End Get
        Set(ByVal value As String)
            _strEncryptCertificateThumbprint = value
        End Set
    End Property

    Public Property SignCertificateThumbprint() As String
        Get
            Return _strSignCertificateThumbprint
        End Get
        Set(ByVal value As String)
            _strSignCertificateThumbprint = value
        End Set
    End Property


    Public Sub New(ByVal strEncryptCertificateThumbprint As String, ByVal strSignCertificateThumbprint As String)
        _strEncryptCertificateThumbprint = strEncryptCertificateThumbprint
        _strSignCertificateThumbprint = strSignCertificateThumbprint
    End Sub

    Public Overrides Function CreateClientOutputFilter(ByVal context As FilterCreationContext) As SoapFilter
        Return New CustomSecurityClientOutputFilter(Me, _strEncryptCertificateThumbprint, _strSignCertificateThumbprint)
    End Function 'CreateClientOutputFilter

    Public Overrides Function CreateClientInputFilter(ByVal context As FilterCreationContext) As SoapFilter
        Return New CustomSecurityClientInputFilter(Me)
        'Return Nothing
    End Function 'CreateClientInputFilter

    Public Overrides Function CreateServiceInputFilter(ByVal context As FilterCreationContext) As SoapFilter
        Return Nothing
    End Function 'CreateServiceInputFilter

    Public Overrides Function CreateServiceOutputFilter(ByVal context As FilterCreationContext) As SoapFilter
        Return Nothing
    End Function 'CreateServiceOutputFilter

    'Public Overrides Sub ReadXml(ByVal reader As XmlReader, ByVal extensions As IDictionary(Of String, Type))
    '    If reader Is Nothing Then
    '        Throw New ArgumentNullException("reader")
    '    End If
    '    If extensions Is Nothing Then
    '        Throw New ArgumentNullException("extensions")
    '    End If
    '    Dim isEmpty As Boolean = reader.IsEmptyElement
    '    MyBase.ReadAttributes(reader)
    '    reader.ReadStartElement("CustomSecurityAssertion")

    '    If Not isEmpty Then
    '        ' Read the contents of the <clientToken> element.
    '        If reader.MoveToContent() = XmlNodeType.Element AndAlso reader.Name = "clientToken" Then
    '            reader.ReadStartElement()
    '            reader.MoveToContent()

    '            ' Get the registed security token provider for X.509 certificate security credentials. 
    '            Dim type As Type = extensions(reader.Name)
    '            Dim instance As Object = Activator.CreateInstance(type)

    '            If instance Is Nothing Then
    '                Throw New InvalidOperationException(String.Format(System.Globalization.CultureInfo.CurrentCulture, "Unable to instantiate policy extension of type 0End.", type.AssemblyQualifiedName))
    '            End If
    '            Dim clientProvider As TokenProvider(Of X509SecurityToken) = CType(instance, TokenProvider(Of X509SecurityToken))

    '            ' Read the child elements that provide the details about the client's X.509 certificate. 
    '            clientProvider.ReadXml(reader, extensions)
    '            Me.ClientX509TokenProvider = clientProvider
    '            reader.ReadEndElement()
    '        End If
    '        ' Read the contents of the <serviceToken> element.
    '        If reader.MoveToContent() = XmlNodeType.Element AndAlso reader.Name = "serviceToken" Then
    '            reader.ReadStartElement()
    '            reader.MoveToContent()

    '            ' Get the registed security token provider for X.509 certificate security credentials. 
    '            Dim type As Type = extensions(reader.Name)
    '            Dim instance As Object = Activator.CreateInstance(type)

    '            If instance Is Nothing Then
    '                Throw New InvalidOperationException(String.Format(System.Globalization.CultureInfo.CurrentCulture, "Unable to instantiate policy extension of type 0End.", type.AssemblyQualifiedName))
    '            End If
    '            Dim serviceProvider As TokenProvider(Of X509SecurityToken) = CType(instance, TokenProvider(Of X509SecurityToken))

    '            ' Read the child elements that provide the details about the client's X.509 certificate. 
    '            serviceProvider.ReadXml(reader, extensions)
    '            Me.ServiceX509TokenProvider = serviceProvider
    '            reader.ReadEndElement()
    '        End If
    '        MyBase.ReadElements(reader, extensions)
    '        reader.ReadEndElement()
    '    End If
    'End Sub
    'Public Overrides Function GetExtensions() As IEnumerable(Of KeyValuePair(Of String, Type))
    '    ' Add the CustomSecurityAssertion custom policy assertion to the list of registered
    '    ' policy extensions.
    '    Dim extensions As New List(Of KeyValuePair(Of String, Type))
    '    extensions.Add(New KeyValuePair(Of String, Type)("CustomSecurityAssertion", Me.GetType()))
    '    If (Not serviceX509TokenProviderValue Is Nothing) Then
    '        If (Not serviceX509TokenProviderValue Is Nothing) Then


    '            ' Add any policy extensions that read child elements of the <serviceToken> element
    '            ' to the list of registered policy extensions.
    '            Dim innerExtensions As IEnumerable(Of KeyValuePair(Of String, Type)) = serviceX509TokenProviderValue.GetExtensions()
    '            If (Not innerExtensions Is Nothing) Then
    '                If (Not innerExtensions Is Nothing) Then
    '                    Dim extension As KeyValuePair(Of String, Type)
    '                    For Each extension In innerExtensions
    '                        extensions.Add(extension)
    '                    Next
    '                End If
    '            End If
    '        End If
    '    End If
    '    If (Not clientX509TokenProviderValue Is Nothing) Then
    '        If (Not clientX509TokenProviderValue Is Nothing) Then


    '            ' Add any policy extensions that read child elements of the <clientToken> element
    '            ' Add any policy extensions that read child elements of the <clientToken> element
    '            ' to the list of registered policy extensions.
    '            ' to the list of registered policy extensions.
    '            Dim innerExtensions As IEnumerable(Of KeyValuePair(Of String, Type)) = clientX509TokenProviderValue.GetExtensions()

    '            If (Not innerExtensions Is Nothing) Then
    '                If (Not innerExtensions Is Nothing) Then
    '                    Dim extension As KeyValuePair(Of String, Type)
    '                    For Each extension In innerExtensions
    '                        extensions.Add(extension)
    '                    Next
    '                End If
    '            End If
    '        End If
    '    End If
    '    Return extensions
    'End Function
End Class

Public Class CustomSecurityClientInputFilter
    Inherits ReceiveSecurityFilter

    Public Sub New(ByVal parentAssertion As HKIPVRSecurityAssertion)
        MyBase.New(parentAssertion.ServiceActor, True)
    End Sub 'New

    Public Overrides Sub ValidateMessageSecurity(ByVal envelope As SoapEnvelope, ByVal security As Security)

        'Dim state As RequestState
        'Dim signed As Boolean = False
        'Dim encrypted As Boolean = False

        '' Get the request state out of the operation state.
        'state = envelope.Context.OperationState.Get(Of RequestState)()

        '' Make sure the message was signed with the server's security token.
        'Dim elem As ISecurityElement
        'For Each elem In security.Elements
        '    If TypeOf elem Is MessageSignature Then
        '        Dim sig As MessageSignature = CType(elem, MessageSignature)

        '        If sig.SigningToken.Equals(state.ServerToken) Then
        '            signed = True
        '        End If
        '    End If
        '    If TypeOf elem Is EncryptedData Then
        '        Dim enc As EncryptedData = CType(elem, EncryptedData)

        '        If enc.SecurityToken.Equals(state.ClientToken) Then
        '            encrypted = True
        '        End If
        '    End If
        'Next elem
        'If Not signed OrElse Not encrypted Then
        '    Throw New Exception("Response message does not meet security requirements")
        'End If
    End Sub 'ValidateMessageSecurity

    Public Overrides Function ProcessMessage(envelope As SoapEnvelope) As SoapFilterResult
        'Dim state As RequestState

        '' Get the request state out of the operation state.
        'state = envelope.Context.OperationState.Get(Of RequestState)()

        Dim nsmanager As XmlNamespaceManager = New XmlNamespaceManager(envelope.NameTable)
        nsmanager.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")
        nsmanager.AddNamespace("S", "http://schemas.xmlsoap.org/soap/envelope/")
        nsmanager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#")
        nsmanager.AddNamespace("xenc", "http://www.w3.org/2001/04/xmlenc#")

        Dim FaultNodeList As XmlNodeList = envelope.SelectNodes("/S:Envelope/S:Body/S:Fault", nsmanager)
        If FaultNodeList.Count > 0 Then
            Dim FaultCodeList As XmlNodeList = FaultNodeList(0).SelectNodes("faultcode")
            Dim FaultStringList As XmlNodeList = FaultNodeList(0).SelectNodes("faultstring")
            Dim FaultFactorList As XmlNodeList = FaultNodeList(0).SelectNodes("faultactor")

            Throw New Exception(String.Format("({0}|{1}|{2})", FaultCodeList(0).InnerText, FaultStringList(0).InnerText, FaultFactorList(0).InnerText))

        End If

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

        Try
            'Dim KeyInfoNodeList As XmlNodeList = envelope.SelectNodes("/S:Envelope/S:Header/wsse:Security/xenc:EncryptedKey/ds:KeyInfo", nsmanager)
            'Dim SecurityTokenReferenceNodeList As XmlNodeList = KeyInfoNodeList(0).SelectNodes("wsse:SecurityTokenReference", nsmanager)
            ''Dim EncryptedKeyNodeList As XmlNodeList = envelope.SelectNodes("/S:Envelope/S:Header/wsse:Security/xenc:EncryptedKey", nsmanager)
            'Dim mySecurityTokenReference As SecurityTokenReference = New SecurityTokenReference(SecurityTokenReferenceNodeList(0))
            'Dim myKeyIdentifier As KeyIdentifier = New KeyIdentifier(state.ClientToken.KeyIdentifier.Identifier) '"0DDPRtJ7/mQkj1xH3NxzoGUOikI=")
            'mySecurityTokenReference.KeyIdentifier = myKeyIdentifier
            'KeyInfoNodeList(0).RemoveAll()
            'KeyInfoNodeList(0).AppendChild(mySecurityTokenReference.GetXml(envelope))

            Dim KeyInfoNodeList As XmlNodeList = envelope.SelectNodes("/S:Envelope/S:Header/wsse:Security/xenc:EncryptedKey/ds:KeyInfo", nsmanager)
            Dim myKeyInfo As KeyInfo = New KeyInfo()
            myKeyInfo.LoadXml(CType(KeyInfoNodeList(0), XmlElement))

            Dim mySecurityTokenReference As SecurityTokenReference = New SecurityTokenReference(myKeyInfo.GetXml.SelectNodes("wsse:SecurityTokenReference", nsmanager)(0))

            If CType(mySecurityTokenReference.GetXml, XmlElement).SelectNodes("ds:X509Data", nsmanager).Count > 0 Then
                Dim myKeyInfoX509Data As KeyInfoX509Data = New KeyInfoX509Data()
                myKeyInfoX509Data.LoadXml(CType(mySecurityTokenReference.GetXml, XmlElement).FirstChild)

                Dim Token As SecurityToken = X509TokenProvider.CreateToken(StoreLocation.LocalMachine, StoreName.My, CType(myKeyInfoX509Data.IssuerSerials(0), X509IssuerSerial).SerialNumber, X509FindType.FindBySerialNumber)

                Dim myKeyIdentifier As KeyIdentifier = New KeyIdentifier(Token.KeyIdentifier.Identifier) '"0DDPRtJ7/mQkj1xH3NxzoGUOikI=")
                mySecurityTokenReference.KeyIdentifier = myKeyIdentifier

                KeyInfoNodeList(0).RemoveAll()
                KeyInfoNodeList(0).AppendChild(mySecurityTokenReference.GetXml(envelope))

            End If

            'Dim SignKeyInfoNodeList As XmlNodeList = envelope.SelectNodes("/S:Envelope/S:Header/wsse:Security/ds:Signature/ds:KeyInfo", nsmanager)
            'Dim SignSecurityTokenReferenceNodeList As XmlNodeList = SignKeyInfoNodeList(0).SelectNodes("wsse:SecurityTokenReference", nsmanager)
            ''Dim EncryptedKeyNodeList As XmlNodeList = envelope.SelectNodes("/S:Envelope/S:Header/wsse:Security/xenc:EncryptedKey", nsmanager)
            'Dim mySignSecurityTokenReference As SecurityTokenReference = New SecurityTokenReference(SignSecurityTokenReferenceNodeList(0))
            'Dim mySignKeyIdentifier As KeyIdentifier = New KeyIdentifier(state.ServerToken.KeyIdentifier.Identifier) '"0DDPRtJ7/mQkj1xH3NxzoGUOikI=")
            'mySignSecurityTokenReference.KeyIdentifier = mySignKeyIdentifier
            'SignKeyInfoNodeList(0).RemoveAll()
            'SignKeyInfoNodeList(0).AppendChild(mySignSecurityTokenReference.GetXml(envelope))

            Dim SignKeyInfoNodeList As XmlNodeList = envelope.SelectNodes("/S:Envelope/S:Header/wsse:Security/ds:Signature/ds:KeyInfo", nsmanager)
            Dim SignKeyInfo As KeyInfo = New KeyInfo()
            SignKeyInfo.LoadXml(CType(SignKeyInfoNodeList(0), XmlElement))

            Dim SignSecurityTokenReference As SecurityTokenReference = New SecurityTokenReference(SignKeyInfo.GetXml.SelectNodes("wsse:SecurityTokenReference", nsmanager)(0))

            If CType(SignSecurityTokenReference.GetXml, XmlElement).SelectNodes("ds:X509Data", nsmanager).Count > 0 Then
                Dim SignKeyInfoX509Data As KeyInfoX509Data = New KeyInfoX509Data()
                SignKeyInfoX509Data.LoadXml(CType(SignSecurityTokenReference.GetXml, XmlElement).FirstChild)

                Dim Token As SecurityToken = X509TokenProvider.CreateToken(StoreLocation.LocalMachine, StoreName.My, CType(SignKeyInfoX509Data.IssuerSerials(0), X509IssuerSerial).SerialNumber, X509FindType.FindBySerialNumber)

                Dim SignKeyIdentifier As KeyIdentifier = New KeyIdentifier(Token.KeyIdentifier.Identifier) '"0DDPRtJ7/mQkj1xH3NxzoGUOikI=")
                SignSecurityTokenReference.KeyIdentifier = SignKeyIdentifier

                SignKeyInfoNodeList(0).RemoveAll()
                SignKeyInfoNodeList(0).AppendChild(SignSecurityTokenReference.GetXml(envelope))

            End If

            Return MyBase.ProcessMessage(envelope)

        Catch s As XPathException
            Throw

        Catch e As Exception
            Throw

        End Try

        Return MyBase.ProcessMessage(envelope)
    End Function
End Class 'CustomSecurityClientInputFilter

Public Class CustomSecurityClientOutputFilter
    Inherits SendSecurityFilter
    Private clientToken As SecurityToken
    Private serverToken As SecurityToken

    Public Sub New(ByVal parentAssertion As HKIPVRSecurityAssertion, strEncryptyionCertificateThumbprint As String, _
                   ByVal strSignCertificateThumbprint As String)
        MyBase.New(parentAssertion.ServiceActor, True)

        ' Get the client security token.
        clientToken = X509TokenProvider.CreateToken(StoreLocation.LocalMachine, StoreName.My, strSignCertificateThumbprint, X509FindType.FindByThumbprint)

        ' Get the server security token.
        serverToken = X509TokenProvider.CreateToken(StoreLocation.LocalMachine, StoreName.AddressBook, strEncryptyionCertificateThumbprint, X509FindType.FindByThumbprint)
    End Sub 'New

    Public Overrides Sub SecureMessage(ByVal envelope As SoapEnvelope, ByVal security As Security)
        ' Sign the SOAP message with the client's security token.
        security.Tokens.Add(clientToken)
        security.Elements.Add(New MessageSignature(clientToken))

        ' Encrypt the SOAP message with the client's security token.
        security.Elements.Add(New Microsoft.Web.Services3.Security.EncryptedData(serverToken))

        ' Encrypt the client's security token with the server's security token.
        'security.Elements.Add(New EncryptedData(serverToken, "#" + clientToken.Id))

        ' Store the client and server security tokens in the request state.
        Dim state As New RequestState(clientToken, serverToken)

        ' Store the request state in the proxy's operation state. 
        ' This makes these tokens accessible when SOAP responses are 
        ' verified to have sufficient security requirements.
        envelope.Context.OperationState.Set(state)
    End Sub 'SecureMessage
End Class 'CustomSecurityClientOutputFilter

Public Class RequestState
    Private clientTokenValue As SecurityToken
    Private serverTokenValue As SecurityToken


    Public Sub New(ByVal cToken As SecurityToken, ByVal sToken As SecurityToken)
        clientTokenValue = cToken
        serverTokenValue = sToken
    End Sub 'New


    Public ReadOnly Property ClientToken() As SecurityToken
        Get
            Return clientTokenValue
        End Get
    End Property

    Public ReadOnly Property ServerToken() As SecurityToken
        Get
            Return serverTokenValue
        End Get
    End Property
End Class 'RequestState
