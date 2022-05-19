using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using log4net;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using ServiceStack.WebHost.Endpoints.Support.Markdown;
using System.Security.Cryptography.X509Certificates;

namespace eService.Common
{
    public class EncryptUtils
    {
        private static log4net.ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// KEY 结构体
        /// </summary>
        protected struct RSAKEY
        {
            /// <summary>
            /// 公钥
            /// </summary>
            public string PublicKey
            {
                get;
                set;
            }
            /// <summary>
            /// 私钥
            /// </summary>
            public string PrivateKey
            {
                get;
                set;
            }
        }

        /// <summary>
        /// RSA私钥加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptByPrivateKey(string s, string key)
        {
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //加密
            try
            {
                engine.Init(true, GetPrivateKeyParameter(key));
                byte[] byteData = System.Text.Encoding.UTF8.GetBytes(s);
                var resultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return CommonHelper.EncodeBase64(resultData);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// RSA私钥加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] EncryptByPrivateKey(byte[] byteData, string key)
        {
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //加密
            try
            {
                engine.Init(true, GetPrivateKeyParameter(key));
                //byte[] byteData = System.Text.Encoding.UTF8.GetBytes(s);
                var resultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return resultData;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// RSA公钥解密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DecryptByPublicKey(string s, string key)
        {
            s = s.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //解密
            try
            {
                engine.Init(false, GetPublicKeyParameter(key));
                byte[] byteData = Convert.FromBase64String(s);
                var resultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return CommonHelper.EncodeBase64(resultData);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// RSA公钥加密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncryptByPublicKey(string s, string key)
        {
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //加密
            try
            {
                engine.Init(true, GetPublicKeyParameter(key));
                byte[] byteData = System.Text.Encoding.UTF8.GetBytes(s);
                var resultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return CommonHelper.EncodeBase64(resultData);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// RSA私钥解密
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DecryptByPrivateKey(string s, string key)
        {
            s = s.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            //非对称加密算法，加解密用  
            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());

            //解密
            try
            {
                engine.Init(false, GetPrivateKeyParameter(key));
                byte[] byteData = Convert.FromBase64String(s);
                var resultData = engine.ProcessBlock(byteData, 0, byteData.Length);
                return CommonHelper.EncodeBase64(resultData);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }

        }

        #region 加签    
        /// <summary>
        /// 基于BouncyCastle的RSA签名
        /// </summary>
        /// <param name="data"></param>
        /// <param name="privateKeyJava"></param>
        /// <param name="hashAlgorithm">JAVA的和.NET的不一样，如：MD5(.NET)等同于MD5withRSA(JAVA)</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string RSASignJavaBouncyCastle(string data, string privateKeyJava, string hashAlgorithm = "MD5withRSA", string encoding = "UTF-8")
        {
            RsaKeyParameters privateKeyParam = (RsaKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKeyJava));
            ISigner signer = SignerUtilities.GetSigner(hashAlgorithm);
            signer.Init(true, privateKeyParam);//参数为true验签，参数为false加签
            var dataByte = Encoding.GetEncoding(encoding).GetBytes(data);
            signer.BlockUpdate(dataByte, 0, dataByte.Length);
            //return Encoding.GetEncoding(encoding).GetString(signer.GenerateSignature()); //签名结果 非Base64String
            return Convert.ToBase64String(signer.GenerateSignature());
        }
        #endregion

        #region 验签
        /// <summary>
        /// 基于BouncyCastle的RSA签名
        /// </summary>
        /// <param name="data">源数据</param>
        /// <param name="publicKeyJava"></param>
        /// <param name="signature">base64签名</param>
        /// <param name="hashAlgorithm">JAVA的和.NET的不一样，如：MD5(.NET)等同于MD5withRSA(JAVA)</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static bool VerifyJavaBouncyCastle(string data, string publicKeyJava, string signature, string hashAlgorithm = "MD5withRSA", string encoding = "UTF-8")
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKeyJava));
            ISigner signer = SignerUtilities.GetSigner(hashAlgorithm);
            signer.Init(false, publicKeyParam);
            byte[] dataByte = Encoding.GetEncoding(encoding).GetBytes(data);
            signer.BlockUpdate(dataByte, 0, dataByte.Length);
            //byte[] signatureByte = Encoding.GetEncoding(encoding).GetBytes(signature);// 非Base64String
            byte[] signatureByte = Convert.FromBase64String(signature);
            return signer.VerifySignature(signatureByte);
        }
        #endregion

        #region RSA私钥转换

        /// <summary>  
        /// RSA私钥格式转换，java->.net  
        /// </summary>  
        /// <param name="privateKey">java生成的RSA私钥</param>  
        /// <returns></returns>  
        public static string RSAPrivateKeyJava2DotNet(byte[] privateKeyByte)
        {
            RsaPrivateCrtKeyParameters privateKeyParam =
                (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(privateKeyByte);

            return string.Format(
                "<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        #endregion

        #region SHA256加密
        /// <summary>
        /// SHA256加密，返回byte数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SHA256EncryptToByte(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var byteData = GetKeyByteArray(data);
                return SHA256EncryptToByte(byteData);
            }
            return null;
        }

        /// <summary>
        /// SHA256加密byte数组，返回hash
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SHA256EncryptToByte(byte[] byteData)
        {
            if (byteData != null)
            {
                SHA256 sha256 = new SHA256Managed();

                var tmpByte = sha256.ComputeHash(byteData);
                sha256.Clear();

                return tmpByte;
            }
            return null;
        }

        /// <summary>
        /// SHA256加密流，返回hash
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] SHA256EncryptToByte(Stream stream)
        {
            if (stream != null)
            {
                SHA256 sha256 = new SHA256Managed();

                byte[] hashValue = sha256.ComputeHash(stream);
                sha256.Clear();

                return hashValue;
            }
            return null;
        }
        #endregion

        #region SHA512加密
        /// <summary>
        /// SHA512加密，返回byte数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SHA512EncryptToByte(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var byteData = GetKeyByteArray(data);
                return SHA512EncryptToByte(byteData);
            }
            return null;
        }

        /// <summary>
        /// SHA512加密，返回byte数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SHA512EncryptToByte(byte[] byteData)
        {
            if (byteData != null)
            {
                SHA512 sha512 = new SHA512Managed();

                var tmpByte = sha512.ComputeHash(byteData);
                sha512.Clear();

                return tmpByte;
            }
            return null;
        }

        #endregion

        #region MD5加密
        /// <summary>
        /// MD5，返回Byte数组
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <returns></returns>
        public static byte[] MD5EncryptToByte(byte[] dataBytes)
        {
            if (dataBytes != null)
            {
                byte[] tmpByte;
                MD5 md5 = new MD5CryptoServiceProvider();
                tmpByte = md5.ComputeHash(dataBytes);
                md5.Clear();
                return tmpByte;
            }
            return null;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取Key的Byte数组
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        private static byte[] GetKeyByteArray(string strKey)
        {

            //ASCIIEncoding ascii = new ASCIIEncoding();

            int tmpStrLen = strKey.Length;
            byte[] tmpByte = new byte[tmpStrLen - 1];

            tmpByte = Encoding.UTF8.GetBytes(strKey);

            return tmpByte;

        }

        private RSAKEY GetKey()
        {
            //RSA密钥对的构造器  
            RsaKeyPairGenerator keyGenerator = new RsaKeyPairGenerator();

            //RSA密钥构造器的参数  
            RsaKeyGenerationParameters param = new RsaKeyGenerationParameters(
                Org.BouncyCastle.Math.BigInteger.ValueOf(3),
                new Org.BouncyCastle.Security.SecureRandom(),
                1024,   //密钥长度  
                25);
            //用参数初始化密钥构造器  
            keyGenerator.Init(param);
            //产生密钥对  
            AsymmetricCipherKeyPair keyPair = keyGenerator.GenerateKeyPair();
            //获取公钥和密钥  
            AsymmetricKeyParameter publicKey = keyPair.Public;
            AsymmetricKeyParameter privateKey = keyPair.Private;

            SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);


            Asn1Object asn1ObjectPublic = subjectPublicKeyInfo.ToAsn1Object();

            byte[] publicInfoByte = asn1ObjectPublic.GetEncoded("UTF-8");
            Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();
            byte[] privateInfoByte = asn1ObjectPrivate.GetEncoded("UTF-8");

            RSAKEY item = new RSAKEY()
            {
                PublicKey = Convert.ToBase64String(publicInfoByte),
                PrivateKey = Convert.ToBase64String(privateInfoByte)
            };
            return item;
        }

        private static AsymmetricKeyParameter GetPublicKeyParameter(string s)
        {
            s = s.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            byte[] publicInfoByte = Convert.FromBase64String(s);
            Asn1Object pubKeyObj = Asn1Object.FromByteArray(publicInfoByte);//这里也可以从流中读取，从本地导入   
            AsymmetricKeyParameter pubKey = PublicKeyFactory.CreateKey(publicInfoByte);
            return pubKey;
        }

        private static AsymmetricKeyParameter GetPrivateKeyParameter(string s)
        {
            s = s.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            byte[] privateInfoByte = Convert.FromBase64String(s);
            // Asn1Object priKeyObj = Asn1Object.FromByteArray(privateInfoByte);//这里也可以从流中读取，从本地导入   
            // PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);
            AsymmetricKeyParameter priKey = PrivateKeyFactory.CreateKey(privateInfoByte);

            return priKey;
        }
        #endregion

        #region AES加解密
        private static readonly SecureRandom Random = new SecureRandom();

        //Preconfigured Encryption Parameters
        public static readonly int NonceBitSize = 128;
        public static readonly int MacBitSize = 128;
        public static readonly int KeyBitSize = 256;  //加密秘钥采用256位

        //Preconfigured Password Key Derivation Parameters
        public static readonly int SaltBitSize = 128;
        public static readonly int Iterations = 10000;
        public static readonly int MinPasswordLength = 12;

        /// <summary>
        /// Helper that generates a random new key on each call.
        /// </summary>
        /// <returns></returns>
        public static byte[] NewKey()
        {
            var key = new byte[KeyBitSize / 8];
            Random.NextBytes(key);
            return key;
        }

        /// <summary>
        /// Simple Encryption And Authentication (AES-GCM) of a UTF8 string.
        /// </summary>
        /// <param name="secretMessage">The secret message.</param>
        /// <param name="key">The key.</param>
        /// <param name="nonSecretPayload">Optional non-secret payload.</param>
        /// <returns>
        /// Encrypted Message
        /// </returns>
        /// <exception cref="System.ArgumentException">Secret Message Required!;secretMessage</exception>
        /// <remarks>
        /// Adds overhead of (Optional-Payload + BlockSize(16) + Message +  HMac-Tag(16)) * 1.33 Base64
        /// </remarks>
        public static string AESEncrypt(string secretMessage, string key, byte[] nonSecretPayload = null)
        {
            if (string.IsNullOrEmpty(secretMessage))
                throw new ArgumentException("Secret Message Required!", "secretMessage");

            var plainText = Encoding.UTF8.GetBytes(secretMessage);
            var keyByte = CommonHelper.DecodeBase64ToByte(key);
            var cipherText = AESEncrypt(plainText, keyByte, nonSecretPayload);
            var result = string.Empty;

            return Convert.ToBase64String(cipherText);
        }


        /// <summary>
        /// Simple Decryption & Authentication (AES-GCM) of a UTF8 Message
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message.</param>
        /// <param name="key">The key.</param>
        /// <param name="nonSecretPayloadLength">Length of the optional non-secret payload.</param>
        /// <returns>Decrypted Message</returns>
        public static string AESDecrypt(string encryptedMessage, string key, int nonSecretPayloadLength = 0)
        {
            if (string.IsNullOrEmpty(encryptedMessage))
                throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

            var cipherText = Convert.FromBase64String(encryptedMessage);
            var keyByte = CommonHelper.DecodeBase64ToByte(key);
            var plainText = AESDecrypt(cipherText, keyByte, nonSecretPayloadLength);
            return plainText == null ? null : Encoding.UTF8.GetString(plainText);
        }

        /// <summary>
        /// Simple Encryption And Authentication (AES-GCM) of a UTF8 String
        /// using key derived from a password (PBKDF2).
        /// </summary>
        /// <param name="secretMessage">The secret message.</param>
        /// <param name="password">The password.</param>
        /// <param name="nonSecretPayload">The non secret payload.</param>
        /// <returns>
        /// Encrypted Message
        /// </returns>
        /// <remarks>
        /// Significantly less secure than using random binary keys.
        /// Adds additional non secret payload for key generation parameters.
        /// </remarks>
        public static string AESEncryptWithPassword(string secretMessage, string password, byte[] nonSecretPayload = null)
        {
            if (string.IsNullOrEmpty(secretMessage))
                throw new ArgumentException("Secret Message Required!", "secretMessage");

            var plainText = Encoding.UTF8.GetBytes(secretMessage);
            var cipherText = AESEncryptWithPassword(plainText, password, nonSecretPayload);
            return Convert.ToBase64String(cipherText);
        }


        /// <summary>
        /// Simple Decryption and Authentication (AES-GCM) of a UTF8 message
        /// using a key derived from a password (PBKDF2)
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message.</param>
        /// <param name="password">The password.</param>
        /// <param name="nonSecretPayloadLength">Length of the non secret payload.</param>
        /// <returns>
        /// Decrypted Message
        /// </returns>
        /// <exception cref="System.ArgumentException">Encrypted Message Required!;encryptedMessage</exception>
        /// <remarks>
        /// Significantly less secure than using random binary keys.
        /// </remarks>
        public static string AESDecryptWithPassword(string encryptedMessage, string password, int nonSecretPayloadLength = 0)
        {
            if (string.IsNullOrEmpty(encryptedMessage))
                throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

            var cipherText = Convert.FromBase64String(encryptedMessage);
            var plainText = AESDecryptWithPassword(cipherText, password, nonSecretPayloadLength);
            return plainText == null ? null : Encoding.UTF8.GetString(plainText);
        }

        private static byte[] AESEncrypt(byte[] secretMessage, byte[] key, byte[] nonSecretPayload = null)
        {
            //User Error Checks
            if (key == null || key.Length != KeyBitSize / 8)
                throw new ArgumentException(String.Format("Key needs to be {0} bit!", KeyBitSize), "key");

            if (secretMessage == null || secretMessage.Length == 0)
                throw new ArgumentException("Secret Message Required!", "secretMessage");

            //Using random nonce large enough not to repeat
            //var nonce = new byte[NonceBitSize / 8];
            //Random.NextBytes(nonce, 0, nonce.Length);
            var nonce = new byte[12];
            Random.NextBytes(nonce, 0, nonce.Length);

            //Non-secret Payload Optional
            //nonSecretPayload = nonSecretPayload ?? new byte[] { };
            nonSecretPayload = nonSecretPayload ?? CommonHelper.ConvertIntToBytes(nonce.Length);

            var cipher = new GcmBlockCipher(new AesFastEngine());
            var parameters = new AeadParameters(new KeyParameter(key), MacBitSize, nonce);
            cipher.Init(true, parameters);

            //Generate Cipher Text With Auth Tag
            var cipherText = new byte[cipher.GetOutputSize(secretMessage.Length)];
            var len = cipher.ProcessBytes(secretMessage, 0, secretMessage.Length, cipherText, 0);
            cipher.DoFinal(cipherText, len);

            //Assemble Message
            using (var combinedStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(combinedStream))
                {
                    //Prepend Authenticated Payload
                    binaryWriter.Write(nonSecretPayload);
                    //Prepend Nonce
                    binaryWriter.Write(nonce);
                    //Write Cipher Text
                    binaryWriter.Write(cipherText);
                }
                return combinedStream.ToArray();
            }
        }

        private static byte[] AESDecrypt(byte[] encryptedMessage, byte[] key, int nonSecretPayloadLength = 0)
        {
            //User Error Checks
            if (key == null || key.Length != KeyBitSize / 8)
                throw new ArgumentException(String.Format("Key needs to be {0} bit!", KeyBitSize), "key");

            if (encryptedMessage == null || encryptedMessage.Length == 0)
                throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

            using (var cipherStream = new MemoryStream(encryptedMessage))
            using (var cipherReader = new BinaryReader(cipherStream))
            {
                //Grab Payload
                var nonSecretPayload = cipherReader.ReadBytes(nonSecretPayloadLength);

                //Grab Nonce
                //var nonce = cipherReader.ReadBytes(NonceBitSize / 8);
                var nonce = cipherReader.ReadBytes(12);

                var cipher = new GcmBlockCipher(new AesFastEngine());
                var parameters = new AeadParameters(new KeyParameter(key), MacBitSize, nonce);
                cipher.Init(false, parameters);

                //Decrypt Cipher Text
                var cipherText = cipherReader.ReadBytes(encryptedMessage.Length - nonSecretPayloadLength - nonce.Length);
                var plainText = new byte[cipher.GetOutputSize(cipherText.Length)];

                try
                {
                    var len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);
                    cipher.DoFinal(plainText, len);

                }
                catch (InvalidCipherTextException ex)
                {
                    //Return null if it doesn't authenticate
                    log.Error(ex.Message);
                    throw;
                }

                return plainText;
            }

        }

        public static byte[] AESEncryptWithPassword(byte[] secretMessage, string password, byte[] nonSecretPayload = null)
        {
            nonSecretPayload = nonSecretPayload ?? new byte[] { };

            //User Error Checks
            if (string.IsNullOrEmpty(password) || password.Length < MinPasswordLength)
                throw new ArgumentException(String.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");

            if (secretMessage == null || secretMessage.Length == 0)
                throw new ArgumentException("Secret Message Required!", "secretMessage");

            var generator = new Pkcs5S2ParametersGenerator();

            //Use Random Salt to minimize pre-generated weak password attacks.
            var salt = new byte[SaltBitSize / 8];
            Random.NextBytes(salt);

            generator.Init(
              PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()),
              salt,
              Iterations);

            //Generate Key
            var key = (KeyParameter)generator.GenerateDerivedMacParameters(KeyBitSize);

            //Create Full Non Secret Payload
            var payload = new byte[salt.Length + nonSecretPayload.Length];
            Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length);
            Array.Copy(salt, 0, payload, nonSecretPayload.Length, salt.Length);

            return AESEncrypt(secretMessage, key.GetKey(), payload);
        }

        public static byte[] AESDecryptWithPassword(byte[] encryptedMessage, string password, int nonSecretPayloadLength = 0)
        {
            //User Error Checks
            if (string.IsNullOrEmpty(password) || password.Length < MinPasswordLength)
                throw new ArgumentException(String.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");

            if (encryptedMessage == null || encryptedMessage.Length == 0)
                throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");

            var generator = new Pkcs5S2ParametersGenerator();

            //Grab Salt from Payload
            var salt = new byte[SaltBitSize / 8];
            Array.Copy(encryptedMessage, nonSecretPayloadLength, salt, 0, salt.Length);

            generator.Init(
              PbeParametersGenerator.Pkcs5PasswordToBytes(password.ToCharArray()),
              salt,
              Iterations);

            //Generate Key
            var key = (KeyParameter)generator.GenerateDerivedMacParameters(KeyBitSize);

            return AESDecrypt(encryptedMessage, key.GetKey(), salt.Length + nonSecretPayloadLength);
        }

        #endregion
    }
}
