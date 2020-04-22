Imports TestWSforHKMA.Cryptography
Imports TestWSforHKMA
Imports System.Xml


Namespace Cryptography

    Public Class CryptographyMgr

#Region "Private Functions"

        'Encrypt --> Sign Signature (-->Encrypt)
        Private Shared Function GenerateSecuredXML(ByVal strPlainXML As String, ByVal objDSPara As Cryptography.DigitalSignatureParameter, ByVal objEncryptPara As Cryptography.EncryptionParameter) As String

            Dim strFinalXML As String = ""
            strFinalXML = strPlainXML

            'Encrypt 
            If objEncryptPara IsNot Nothing Then
                If objEncryptPara.Enable = True Then
                    If objEncryptPara.XMLEncryptElementName Is Nothing Then

                        objEncryptPara.XMLEncryptElementName = "Input"
                    End If

                    strFinalXML = XMLEncryptionMgr.encryptXML(objEncryptPara.CertificateThumbprint, strFinalXML, objEncryptPara.XMLEncryptElementName)
                End If
            End If

            'Sign Signature
            If objDSPara IsNot Nothing Then
                If objDSPara.Enable = True Then
                    If objDSPara.SignatureAttachmentElementId Is Nothing Then
                        objDSPara.SignatureAttachmentElementId = "Request"
                    End If

                    If objDSPara.XMLSignElementId Is Nothing Then
                        objDSPara.XMLSignElementId = ""
                    End If

                    strFinalXML = XMLDigitialSignatureMgr.signXML(objDSPara.CertificateThumbprint, strFinalXML, objDSPara.XMLSignElementId, objDSPara.SignatureAttachmentElementId)
                End If
            End If

            'Encrypt 
            If objEncryptPara IsNot Nothing Then
                If objEncryptPara.Enable = True Then
                    'If objEncryptPara.XMLEncryptElementName Is Nothing Then

                    objEncryptPara.XMLEncryptElementName = "Request"
                    'End If

                    strFinalXML = XMLEncryptionMgr.encryptXML(objEncryptPara.CertificateThumbprint, strFinalXML, objEncryptPara.XMLEncryptElementName)
                End If
            End If

            Return strFinalXML
        End Function

        'Decrypt --> Verify Signature 
        Private Shared Function resolveSecuredXML(ByVal strSecuredXML As String, ByVal objDSPara As Cryptography.DigitalSignatureParameter, ByVal objEncryptPara As Cryptography.EncryptionParameter) As ResolveResult
            Dim blnDSVerfiedResult As Boolean = False
            Dim objResolveResult As ResolveResult = Nothing

            Dim strFinalXML As String = strSecuredXML

            'Decrypt
            If objEncryptPara IsNot Nothing Then
                If objResolveResult Is Nothing Then
                    objResolveResult = New ResolveResult()
                End If

                If objEncryptPara.Enable = True Then
                    Try
                        strFinalXML = XMLEncryptionMgr.decryptXML(objEncryptPara.CertificateThumbprint, strFinalXML)
                    Catch objEx As Exception
                        Throw objEx
                    End Try

                    objResolveResult.SuccessfulDecrypted = True

                    objResolveResult.PlainXML = strFinalXML
                Else
                    objResolveResult.SuccessfulDecrypted = True
                    objResolveResult.PlainXML = strFinalXML
                End If
            End If

            'Verify signature
            If objDSPara IsNot Nothing Then
                If objDSPara.Enable = True Then

                    If objResolveResult Is Nothing Then
                        objResolveResult = New ResolveResult()
                    End If

                    If objDSPara.XMLSignElementId Is Nothing Then
                        objDSPara.XMLSignElementId = ""
                    End If

                    Try
                        blnDSVerfiedResult = XMLDigitialSignatureMgr.verifySignedXML(objDSPara.CertificateThumbprint, strFinalXML, objDSPara.XMLSignElementId)
                    Catch objEx As Exception
                        Throw objEx
                    End Try

                    objResolveResult.SuccessfulVerified = blnDSVerfiedResult
                Else
                    objResolveResult.SuccessfulVerified = True
                End If
            End If



            Return objResolveResult
        End Function

#End Region

#Region "Public Functions"

        Public Shared Function ExtractContentFromSecuredXML(ByVal strSecuredXML As String, ByRef strPlainXML As String, ByVal strSystemName As String) As Boolean

            Dim strCertificateThumbprintToVerifyDigitalSignature As String = AppConfigMgr.getCertificateThumbprintToVerifyDigitalSignature(strSystemName)
            Dim strCertificateThumbprintForDecryption As String = AppConfigMgr.getCertificateThumbprintForDecryption()

            Dim objDigitalSignatureParameter As New DigitalSignatureParameter(strCertificateThumbprintToVerifyDigitalSignature, True, Nothing, Nothing)
            Dim objDecryptionParameter As New EncryptionParameter(strCertificateThumbprintForDecryption, True, Nothing)

            Dim blnValid As Boolean = False
            Dim objResolveResult As ResolveResult = Nothing

            Try
                'Verify the signature & decrypt the content
                objResolveResult = resolveSecuredXML(strSecuredXML, objDigitalSignatureParameter, objDecryptionParameter)

                'WriteLog.WriteAuditLogToTextFile("", "(Result) Decrypt: " + objResolveResult.SuccessfulDecrypted.ToString() + " | Signature: " + objResolveResult.SuccessfulVerified.ToString())
                If objResolveResult.SuccessfulDecrypted And objResolveResult.SuccessfulVerified Then
                    blnValid = True
                End If

                'Return objResolveResult.PlainXML
                strPlainXML = objResolveResult.PlainXML
                Return blnValid

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Shared Function CreateSecuredXMLFromPlainXML(ByVal strPlainXML As String, ByVal strSystemName As String) As String

            Try
                Dim strOuput As String = strPlainXML

                Dim strCertificateThumbprintToPerformDigitalSignature As String = AppConfigMgr.getCertificateThumbprintToPerformDigitalSignature()
                Dim strCertificateThumbprintForEncryption As String = AppConfigMgr.getSSOCertificateThumbprintForEncryption(strSystemName)

                Dim objPerformDigitalSignatureParameter As New DigitalSignatureParameter(strCertificateThumbprintToPerformDigitalSignature, True, Nothing, Nothing)
                Dim objEncryptionParameter As New EncryptionParameter(strCertificateThumbprintForEncryption, True, Nothing)

                'Encrypt the content & sign the signature
                strOuput = GenerateSecuredXML(strOuput, objPerformDigitalSignatureParameter, objEncryptionParameter)

                Return strOuput
            Catch ex As Exception
                Throw ex
            End Try

        End Function

#End Region

    End Class

End Namespace


