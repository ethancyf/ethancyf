Imports System.IO
Imports System.Text
Imports Org.BouncyCastle.Pkcs
Imports Org.BouncyCastle.X509
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Security
Imports Org.BouncyCastle.Crypto.EC
Imports Org.BouncyCastle.Crypto.Parameters
Imports Org.BouncyCastle.Crypto.Generators
Imports Org.BouncyCastle.Asn1.X9
Imports Org.BouncyCastle.Asn1.Pkcs
Imports Org.BouncyCastle.Asn1.Sec
Imports Org.BouncyCastle.Asn1.X509

Public Class Signature

    Private _objKeyPair As AsymmetricCipherKeyPair = Nothing
    Private _objPrivateKey As ECPrivateKeyParameters = Nothing
    Private _objPublicKey As ECPublicKeyParameters = Nothing

    Public Sub GeneratePrivatePublicKeyPair()

        _objKeyPair = GenerateKeyPair()
        _objPrivateKey = _objKeyPair.Private
        _objPublicKey = _objKeyPair.Public

        ''Get PrivateKey String
        'Dim strPrivateKey As String = FormatKeyString(privateKey)
        'Console.WriteLine(strPrivateKey)

        ''Get Publickey String
        'Dim StrPublickey As String = FormatKeyString(publicKey)
        'Console.WriteLine(StrPublickey)

        'Console.WriteLine("Enter text:")
        'Dim newData As String = Console.ReadLine()

        'Console.WriteLine("")

        ''Sign
        'Dim strSignature As String = Sign(newData, privateKey)
        'Console.WriteLine("Signature:")
        'Console.WriteLine(strSignature)


        ''Verify
        'Console.WriteLine("")
        'Console.WriteLine("Verify Signature:")
        'Console.WriteLine(VerifySignature(newData, publicKey, strSignature))

        'Console.Read()

    End Sub

    Private Shared Function GenerateKeyPair() As AsymmetricCipherKeyPair
        'GenerateKeyPair

        Dim secureRandom As SecureRandom = New SecureRandom()

        'Specified Curve 
        'Dim curve As X9ECParameters = ECNamedCurveTable.GetByName("secp256r1")  'secp256r1,secp256k1
        'Dim domainParams As ECDomainParameters = New ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H)
        'Dim keyParams = New ECKeyGenerationParameters(domainParams, secureRandom)

        'Named Curve

        Dim udtKeyParams = New ECKeyGenerationParameters(SecObjectIdentifiers.SecP256r1, secureRandom)
        Dim udtGenerator = New ECKeyPairGenerator("ECDSA")
        Dim udtKeyPair As AsymmetricCipherKeyPair = Nothing

        udtGenerator.Init(udtKeyParams)

        udtKeyPair = udtGenerator.GenerateKeyPair()

        Return udtKeyPair

    End Function

    Function FormatKeyToString(obj As ECKeyParameters) As String
        Dim textWriter As TextWriter = New StringWriter()
        Dim pemWriter As Org.BouncyCastle.OpenSsl.PemWriter = New Org.BouncyCastle.OpenSsl.PemWriter(textWriter)

        pemWriter.WriteObject(obj)
        pemWriter.Writer.Flush()

        Return textWriter.ToString()

    End Function

    Public Shared Function GetPublicKeyParameter(ByVal objPublicKeyParameter As ECPublicKeyParameters) As AsymmetricKeyParameter
        Dim objPublicKey As AsymmetricKeyParameter = Nothing
        Dim objPublicKeyInfo As SubjectPublicKeyInfo = Nothing

        'Generate keyInfo object by input object
        objPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(objPublicKeyParameter)

        'Create key object from keyInfo object
        objPublicKey = PublicKeyFactory.CreateKey(objPublicKeyInfo)

        Return objPublicKey
    End Function

    Public Shared Function GetECPublicKeyParameters(ByVal strPrivateKey As String, ByVal blnAddHeaderFooter As Boolean) As ECPublicKeyParameters
        Dim strPrivateKeyPara As String = String.Empty

        If blnAddHeaderFooter Then
            strPrivateKeyPara = "-----BEGIN EC PRIVATE KEY-----" + vbNewLine + strPrivateKey + vbNewLine + "-----END EC PRIVATE KEY-----"
        Else
            strPrivateKeyPara = strPrivateKey
        End If

        Dim privateKeyTextReader As TextReader = New StringReader(strPrivateKeyPara)
        Dim pr As Org.BouncyCastle.OpenSsl.PemReader = New Org.BouncyCastle.OpenSsl.PemReader(privateKeyTextReader)
        Dim keyPair As AsymmetricCipherKeyPair = pr.ReadObject()

        Return keyPair.Public

    End Function

    Public Shared Function GetPrivateKeyParameter(ByVal objPrivateKeyParameter As ECPrivateKeyParameters) As AsymmetricKeyParameter
        Dim objPrivateKey As AsymmetricKeyParameter = Nothing
        Dim objPrivateKeyInfo As PrivateKeyInfo = Nothing

        'Generate keyInfo object by input object
        objPrivateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(objPrivateKeyParameter)

        'Create key object from keyInfo object
        objPrivateKey = PrivateKeyFactory.CreateKey(objPrivateKeyInfo)

        Return objPrivateKey

    End Function

    Public Shared Function GetECPrivateKeyParameters(ByVal strPrivateKey As String, ByVal blnAddHeaderFooter As Boolean) As ECPrivateKeyParameters
        Dim strPrivateKeyPara As String = String.Empty

        If blnAddHeaderFooter Then
            strPrivateKeyPara = "-----BEGIN EC PRIVATE KEY-----" + vbNewLine + strPrivateKey + vbNewLine + "-----END EC PRIVATE KEY-----"
        Else
            strPrivateKeyPara = strPrivateKey
        End If

        Dim privateKeyTextReader As TextReader = New StringReader(strPrivateKeyPara)
        Dim pr As Org.BouncyCastle.OpenSsl.PemReader = New Org.BouncyCastle.OpenSsl.PemReader(privateKeyTextReader)
        Dim keyPair As AsymmetricCipherKeyPair = pr.ReadObject()

        Return keyPair.Private

    End Function

    'Public Shared Function Test(ByVal strPrivateKey As String) As ECPrivateKeyParameters
    '    Dim objPrivateKey As ECPrivateKeyParameters = Nothing
    '    Dim bytPrivateKeyInfo As Byte() = Nothing

    '    'Remove unnecessary character
    '    strPrivateKey = strPrivateKey.Replace("\r", "").Replace("\n", "").Replace(" ", "")

    '    'Convert Base64 string to byte array
    '    bytPrivateKeyInfo = Convert.FromBase64String(strPrivateKey)

    '    'Specified Curve 
    '    Dim curve As X9ECParameters = CustomNamedCurves.GetByName("secp256r1")
    '    Dim ecSpec As ECDomainParameters = New ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H)

    '    'Dim secureRandom As SecureRandom = New SecureRandom()
    '    'Dim udtKeyParams = New ECKeyGenerationParameters(SecObjectIdentifiers.SecP256r1, secureRandom)

    '    Dim biPrivateKey As Org.BouncyCastle.Math.BigInteger = New Org.BouncyCastle.Math.BigInteger(1, bytPrivateKeyInfo)

    '    objPrivateKey = New ECPrivateKeyParameters(biPrivateKey, ecSpec)

    '    Return objPrivateKey

    'End Function

    'Public Shared Function GetPrivateKeyParameter(ByVal strPrivateKey As String) As AsymmetricKeyParameter
    '    Dim objPrivateKey As AsymmetricKeyParameter = Nothing
    '    Dim bytPrivateKeyInfo As Byte() = Nothing

    '    'Remove unnecessary character
    '    strPrivateKey = strPrivateKey.Replace("\r", "").Replace("\n", "").Replace(" ", "")

    '    'Convert Base64 string to byte array
    '    bytPrivateKeyInfo = Convert.FromBase64String(strPrivateKey)

    '    'Create key object from byte array
    '    objPrivateKey = PrivateKeyFactory.CreateKey(bytPrivateKeyInfo)

    '    Return objPrivateKey

    'End Function

    Public Shared Function Sign(ByVal strData As String, _
                                ByVal objPrivateKeyParameter As ECKeyParameters, _
                                Optional ByVal strHashAlgorithm As String = "SHA256withECDSA", _
                                Optional ByVal strEncoding As String = "UTF-8") As String

        Dim objPrivateKey As AsymmetricKeyParameter = GetPrivateKeyParameter(objPrivateKeyParameter)
        Dim bytData As Byte() = System.Text.Encoding.GetEncoding(strEncoding).GetBytes(strData)
        Dim objSigner As ISigner = SignerUtilities.GetSigner(strHashAlgorithm)
        Dim strSignature As String = String.Empty

        'Import private key
        objSigner.Init(True, objPrivateKey)

        'Import raw data
        objSigner.BlockUpdate(bytData, 0, bytData.Length)

        'Generate digital signature and convert to Base64 string
        strSignature = Convert.ToBase64String(objSigner.GenerateSignature())

        Return strSignature

    End Function


    'Public Shared Function Sign(ByVal strData As String, _
    '                            ByVal strPrivateKeyParameter As String, _
    '                            Optional ByVal strHashAlgorithm As String = "SHA256withECDSA", _
    '                            Optional ByVal strEncoding As String = "UTF-8") As String

    '    Dim objPrivateKey As AsymmetricKeyParameter = GetPrivateKeyParameter(strPrivateKeyParameter)
    '    Dim bytData As Byte() = System.Text.Encoding.GetEncoding(strEncoding).GetBytes(strData)
    '    Dim objSigner As ISigner = SignerUtilities.GetSigner(strHashAlgorithm)
    '    Dim strSignature As String = String.Empty

    '    'Import private key
    '    objSigner.Init(True, objPrivateKey)

    '    'Import raw data
    '    objSigner.BlockUpdate(bytData, 0, bytData.Length)

    '    'Generate digital signature and convert to Base64 string
    '    strSignature = Convert.ToBase64String(objSigner.GenerateSignature())

    '    Return strSignature

    'End Function

    Public Shared Function VerifySignature(ByVal strData As String, _
                                            ByVal objPublicKeyParameter As ECKeyParameters, _
                                            ByVal strSignedText As String, _
                                            Optional ByVal strHashAlgorithm As String = "SHA256withECDSA", _
                                            Optional ByVal strEncoding As String = "UTF-8") As Boolean

        Dim objPublickey As AsymmetricKeyParameter = GetPublicKeyParameter(objPublicKeyParameter)
        Dim bytData As Byte() = System.Text.Encoding.GetEncoding(strEncoding).GetBytes(strData)
        Dim objSigner As ISigner = SignerUtilities.GetSigner(strHashAlgorithm)

        'Import public key
        objSigner.Init(False, objPublickey)

        'Import raw data
        objSigner.BlockUpdate(bytData, 0, bytData.Length)

        'Convert digital signature to byte array and verify to compare the raw data and decrypted digital signature byte array 
        Return objSigner.VerifySignature(Convert.FromBase64String(strSignedText))

    End Function

End Class

