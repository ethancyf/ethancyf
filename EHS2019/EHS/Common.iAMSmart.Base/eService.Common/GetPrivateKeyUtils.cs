using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Common.ComFunction;

namespace eService.Common
{
    public class GetPrivateKeyUtils
    {
        //private const string strFunctionCode = PHF.Constants.FunctionCode.FUNC_070101;
        //private static AuditLogWeb AuditLogWeb = new AuditLogWeb(strFunctionCode);

        public static string GetPrivateKey()
        {
            string strPrivateKey = string.Empty;

            //AuditLogWeb AuditLogWeb = new AuditLogWeb(strFunctionCode);
            //AuditLogWeb.StartLog(BaseAuditLog.LogIDEnumClass.LogID_00108);

            //if ((new SystemParameterBLL()).GetSystemParameter("IAMSmartKPKMode") == PHF.Constants.IAMSmartKPKMode.PK)
            //    return (new SystemParameterBLL()).GetSystemParameter("IAMSmartKEKPK");

            X509Store stordce = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            stordce.Open(OpenFlags.OpenExistingOnly);

            string strIAMSmartKEKThumbprint = (new GeneralFunction()).GetSystemParameterParmValue1("IAMSmartKEKThumbprint");

            X509Certificate2Collection udtCertList = stordce.Certificates.Find(X509FindType.FindByThumbprint, strIAMSmartKEKThumbprint, false);

            foreach (X509Certificate2 cert in udtCertList)
            {
                if (cert.HasPrivateKey)
                {
                    RSACryptoServiceProvider prov = (RSACryptoServiceProvider)cert.PrivateKey;

                    RSAParameters parameters = prov.ExportParameters(true);

                    var KeyPair = Org.BouncyCastle.Security.DotNetUtilities.GetKeyPair(cert.PrivateKey);

                    AsymmetricKeyParameter privateKey = KeyPair.Private;

                    PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);

                    Asn1Object asn1ObjectPrivate = privateKeyInfo.ToAsn1Object();

                    byte[] privateInfoByte = asn1ObjectPrivate.GetEncoded("UTF-8");

                    strPrivateKey = Convert.ToBase64String(privateInfoByte);
                }
            }

            stordce.Close();


            if (string.IsNullOrEmpty(strPrivateKey))
            {
                //AuditLogWeb.AddData("Message", "PrivateKey is null or empty");
                //AuditLogWeb.EndLog(BaseAuditLog.LogIDEnumClass.LogID_00110);

            }
            else
            {
                //AuditLogWeb.EndLog(BaseAuditLog.LogIDEnumClass.LogID_00109);
            }

            return strPrivateKey;
        }

    }
}