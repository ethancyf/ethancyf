Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml
Imports System.Security.Cryptography
Imports System.Security.Cryptography.Xml
Imports System.Security.Cryptography.X509Certificates


Namespace Cryptography
    Public Class XMLEncryptionMgr

        Public Shared Function encryptXML(ByVal strX509CertificateThumbprint As String, ByVal strXML As String, ByVal strXMLEncryptElementName As String) As String
            Dim objCertificate As X509Certificate2 = CertificateMgr.findCertificate(StoreLocation.LocalMachine, StoreName.AddressBook, X509FindType.FindByThumbprint, strX509CertificateThumbprint)


            If objCertificate Is Nothing Then
                Throw New Exception("Certificate, " + strX509CertificateThumbprint + ", not found at XMLEncryptionMgr.encryptXML().")
            End If


            Dim blnChkRst As Boolean = Cryptography.CertificateMgr.VerifyCert(objCertificate)

            If blnChkRst = False Then
                Throw New Exception("Certificate " + strX509CertificateThumbprint + ", not valid at XMLEncryptionMgr.encryptXML().")
            End If

            Return encryptXML(objCertificate, strXML, strXMLEncryptElementName)

        End Function

        Public Shared Function encryptXML(ByVal objCertificate As X509Certificate2, ByVal strXML As String, ByVal strXMLEncryptElementName As String) As String
            Try
                ' Create an XmlDocument object.
                Dim xmlDoc As New XmlDocument()

                ' Load an XML file into the XmlDocument object.
                xmlDoc.PreserveWhitespace = True
                xmlDoc.LoadXml(strXML)

                'X509Certificate2 objCertificate = CertificateMgr.findCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint,
                '                    strX509CertificateThumbprint);

                If objCertificate Is Nothing Then
                    Throw New CryptographicException("The X.509 certificate could not be found at XMLEncryptionMgr.encryptXML().")
                End If

                '-------------------------------------------------
                ' Find the specified element in the XmlDocument
                ' object and create a new XmlElemnt object.
                '-------------------------------------------------

                Dim elementToEncrypt As XmlElement = TryCast(xmlDoc.GetElementsByTagName(strXMLEncryptElementName)(0), XmlElement)
                ' Throw an XmlException if the element was not found.
                If elementToEncrypt Is Nothing Then

                    Throw New XmlException("The specified element was not found")
                End If


                '-------------------------------------------------
                ' Create a new instance of the EncryptedXml class 
                ' and use it to encrypt the XmlElement with the 
                ' X.509 Certificate.
                '-------------------------------------------------

                'EncryptedXml eXml = new EncryptedXml();

                ' Encrypt the element.
                'EncryptedData edElement = eXml.Encrypt(elementToEncrypt, objCertificate);

                '-------------------------------------------------
                ' Replace the element from the original XmlDocument
                ' object with the EncryptedData element.
                '-------------------------------------------------
                'EncryptedXml.ReplaceElement(elementToEncrypt, edElement, false);
                'EncryptedXml.ReplaceElement(xmlDoc, edElement, true);

                Dim eXml As New EncryptedXml()


                Dim objRSACryptoServiceProvider As RSACryptoServiceProvider = DirectCast(objCertificate.PublicKey.Key, RSACryptoServiceProvider)
                eXml.AddKeyNameMapping("RSAKey", objRSACryptoServiceProvider)

                ' Encrypt the element.
                Dim edElement As EncryptedData = eXml.Encrypt(elementToEncrypt, "RSAKey")
                '-------------------------------------------------
                ' Replace the element from the original XmlDocument
                ' object with the EncryptedData element.
                '-------------------------------------------------
                EncryptedXml.ReplaceElement(elementToEncrypt, edElement, False)


                Return xmlDoc.InnerXml
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Shared Function decryptXML(ByVal strX509CertificateThumbprint As String, ByVal strEncryptedXML As String) As String
            'X509Certificate2 objCertificate = CertificateMgr.findCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint,
            Dim objCertificate As X509Certificate2 = CertificateMgr.findCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, strX509CertificateThumbprint)


            If objCertificate Is Nothing Then
                Throw New Exception("Certificate, " + strX509CertificateThumbprint + ", not found at XMLEncryptionMgr.decryptXML().")
            End If

            'certificate verification
            Dim blnChkRst As Boolean = CertificateMgr.VerifyCert(objCertificate)

            If blnChkRst = False Then
                Throw New Exception("Certificate " + strX509CertificateThumbprint + ", not valid at XMLEncryptionMgr.encryptXML().")
            End If


            Return decryptXML(objCertificate, strEncryptedXML)


        End Function

        Public Shared Function decryptXML(ByVal objCertificate As X509Certificate2, ByVal strEncryptedXML As String) As String
            Try
                ' Create an XmlDocument object.
                Dim xmlDoc As New XmlDocument()

                ' Load an XML file into the XmlDocument object.
                xmlDoc.PreserveWhitespace = True
                xmlDoc.LoadXml(strEncryptedXML)

                'X509Certificate2 objCertificate = CertificateMgr.findCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint,
                '                    strX509CertificateThumbprint);

                If objCertificate Is Nothing Then
                    Throw New CryptographicException("The X.509 certificate could not be found at XMLEncryptionMgr.decryptXML().")
                End If


                ' Create a new EncryptedXml object.
                Dim exml As New EncryptedXml(xmlDoc)
                'EncryptedXml exml = new EncryptedXml(Doc);

                ' Decrypt the XML document.
                'exml.DecryptDocument();


                Dim objCSPParams As New CspParameters()
                '------------------------------------------------------------------------------
                'objCSPParams.Flags = CspProviderFlags.UseDefaultKeyContainer
                objCSPParams.Flags = CspProviderFlags.UseMachineKeyStore
                objCSPParams.KeyContainerName = "KeyContainer"
                '------------------------------------------------------------------------------
                Dim objRSAKey As New RSACryptoServiceProvider(objCSPParams)
                objRSAKey.FromXmlString(objCertificate.PrivateKey.ToXmlString(True))
                exml.AddKeyNameMapping("RSAKey", objRSAKey)
                exml.DecryptDocument()


                Return xmlDoc.InnerXml
            Catch ex As Exception
                Throw ex
            End Try
        End Function
    End Class
End Namespace