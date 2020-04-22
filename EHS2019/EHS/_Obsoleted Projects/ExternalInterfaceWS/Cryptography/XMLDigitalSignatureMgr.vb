Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml
Imports System.Security.Cryptography
Imports System.Security.Cryptography.Xml
Imports System.Security.Cryptography.X509Certificates


Namespace Cryptography
    Public Class XMLDigitialSignatureMgr

        Public Shared Function signXML(ByVal strX509CertificateThumbprint As String, ByVal strUnsignedXML As String, ByVal strXMLSignElementId As String, ByVal strSignatureAttachmentElementId As String) As String
            Dim objCertificate As X509Certificate2 = CertificateMgr.findCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, strX509CertificateThumbprint)


            If objCertificate Is Nothing Then
                Throw New Exception("Certificate, " + strX509CertificateThumbprint + ", not found at XMLDigitialSignatureMgr.signXML().")
            End If

            Return signXML(objCertificate, strUnsignedXML, strXMLSignElementId, strSignatureAttachmentElementId)

        End Function

        Private Shared Function signXML(ByVal objCertificate As X509Certificate2, ByVal strUnsignedXML As String, ByVal strXMLSignElementId As String, ByVal strSignatureAttachmentElementId As String) As String
            Dim rsaKey As System.Security.Cryptography.RSACryptoServiceProvider = DirectCast(objCertificate.PrivateKey, System.Security.Cryptography.RSACryptoServiceProvider)

            ' Create a new XML document.
            Dim xmlDoc As New XmlDocument()

            ' Load an XML file into the XmlDocument object.
            xmlDoc.PreserveWhitespace = True

            xmlDoc.LoadXml(strUnsignedXML)

            ' Create a SignedXml object.
            Dim signedXml As New SignedXml(xmlDoc)

            ' Add the key to the SignedXml document.
            signedXml.SigningKey = rsaKey

            ' Create a reference to be signed.
            Dim reference As New Reference()
            reference.Uri = strXMLSignElementId

            ' Add an enveloped transformation to the reference.
            Dim env As New XmlDsigEnvelopedSignatureTransform()
            reference.AddTransform(env)

            ' Add the reference to the SignedXml object.
            signedXml.AddReference(reference)

            ' Compute the signature.
            signedXml.ComputeSignature()

            ' Get the XML representation of the signature and save
            ' it to an XmlElement object.
            Dim xmlDigitalSignature As XmlElement = signedXml.GetXml()

            ' Append the element to the XML document.
            'xmlDoc.GetElementsByTagName("Assertion_Content")[0].AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
            xmlDoc.GetElementsByTagName(strSignatureAttachmentElementId)(0).AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, True))
         
            Return xmlDoc.InnerXml

        End Function

        Public Shared Function verifySignedXML(ByVal strX509CertificateThumbprint As String, ByVal strSignedXML As String, ByVal strXMLSignElementId As String) As Boolean
            Dim objCertificate As X509Certificate2 = CertificateMgr.findCertificate(StoreLocation.LocalMachine, StoreName.AddressBook, X509FindType.FindByThumbprint, strX509CertificateThumbprint)

            If objCertificate Is Nothing Then
                Throw New Exception("Certificate, " + strX509CertificateThumbprint + ", not found at XMLDigitialSignatureMgr.verifySignedXML().")
            End If

            Return verifySignedXML(objCertificate, strSignedXML, strXMLSignElementId)

        End Function

        Public Shared Function verifySignedXML(ByVal strX509CertificateThumbprint As String, ByVal strSignedXML As String, ByVal strXMLSignElementId As String, ByVal blnSignByOwn As Boolean) As Boolean

            Dim objCertificate As X509Certificate2 = Nothing
            If blnSignByOwn = True Then
                objCertificate = CertificateMgr.findCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindByThumbprint, strX509CertificateThumbprint)
            Else
                objCertificate = CertificateMgr.findCertificate(StoreLocation.LocalMachine, StoreName.AddressBook, X509FindType.FindByThumbprint, strX509CertificateThumbprint)
            End If

            If objCertificate Is Nothing Then
                Throw New Exception("Certificate, " + strX509CertificateThumbprint + ", not found at XMLDigitialSignatureMgr.verifySignedXML().")
            End If

            Return verifySignedXML(objCertificate, strSignedXML, strXMLSignElementId)

        End Function

        Private Shared Function verifySignedXML(ByVal objCertificate As X509Certificate2, ByVal strSignedXML As String, ByVal strXMLSignElementId As String) As Boolean
            'System.Security.Cryptography.RSACryptoServiceProvider rsaKey = (System.Security.Cryptography.RSACryptoServiceProvider)objCertificate.PrivateKey;
            'System.Security.Cryptography.RSACryptoServiceProvider rsaKey = (System.Security.Cryptography.RSACryptoServiceProvider)objCertificate.PublicKey;

            ' Create a new XML document.
            Dim xmlDoc As New XmlDocument()


            'If objCertificate Is Nothing Then
            '    Throw New CryptographicException("The X.509 certificate could not be found at XMLDigitialSignatureMgr.verifySignedXML().")
            'End If

            ' Load an XML file into the XmlDocument object.
            xmlDoc.PreserveWhitespace = True
            xmlDoc.LoadXml(strSignedXML)


            ' Create a new SignedXml object and pass it
            ' the XML document class.
            Dim signedXml As New SignedXml(xmlDoc)


            ' Find the "Signature" node and create a new
            ' XmlNodeList object.
            Dim nodeList As XmlNodeList = xmlDoc.GetElementsByTagName("Signature")

            ' Throw an exception if no signature was found.
            If nodeList.Count <= 0 Then
                Throw New CryptographicException("Verification failed: No Signature was found in the document.")
            End If

            ' This example only supports one signature for
            ' the entire XML document.  Throw an exception 
            ' if more than one signature was found.
            If nodeList.Count >= 2 Then
                Throw New CryptographicException("Verification failed: More that one signature was found for the document.")
            End If

            ' Load the first <signature> node.  
            signedXml.LoadXml(DirectCast(nodeList(0), XmlElement))


            Return signedXml.CheckSignature(objCertificate, True)

        End Function
    End Class
End Namespace