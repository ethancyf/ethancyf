using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;
using System.Collections;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;


namespace DecryptIdeas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            string m_decryptCertStoreName = DecryptIdeas.Properties.Settings.Default.IdeasRM_DecryptCertStoreName;
            string m_decryptCertThumbprint = DecryptIdeas.Properties.Settings.Default.IdeasRM_DecryptCertThumbprint;
            StoreLocation m_decryptCertStoreLocation = StoreLocation.LocalMachine;

            if (DecryptIdeas.Properties.Settings.Default.IdeasRM_DecryptCertStoreLocation == 1)

                m_decryptCertStoreLocation = StoreLocation.LocalMachine;

            else if (DecryptIdeas.Properties.Settings.Default.IdeasRM_DecryptCertStoreLocation == 2)

                m_decryptCertStoreLocation = StoreLocation.CurrentUser;

            else

                throw new ConfigurationErrorsException("IdeasRM_DecryptCertStoreLocation should be either 1 (Local Machine) or 2 (Current User)");


            X509Store decryptStore = null;
            X509Certificate2 decryptCertificate = null;
            decryptStore = new X509Store(m_decryptCertStoreName, m_decryptCertStoreLocation);

            decryptStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificates = (X509Certificate2Collection)decryptStore.Certificates;
            certificates = certificates.Find(X509FindType.FindByThumbprint, m_decryptCertThumbprint, false);
            decryptCertificate = (X509Certificate2)certificates[0];

            

            

            XmlDocument document = new XmlDocument();

            document.PreserveWhitespace = true;

            document.LoadXml(txtIdeasResponse.Text);

            IdeasEncryptedXml ideasEncryptedXML = new IdeasEncryptedXml(document);

            ideasEncryptedXML.AddKeyNameMapping("dummy", decryptCertificate.PrivateKey);

            ideasEncryptedXML.DecryptDocument();

            decryptCertificate.Reset(); decryptCertificate = null;

            decryptStore.Close(); decryptStore = null;

            txtResult.Text = document.OuterXml;
        }
    }


    [global::System.Reflection.ObfuscationAttribute(Exclude = false, StripAfterObfuscation = true)]

    internal class IdeasEncryptedXml : EncryptedXml
    {

        public IdeasEncryptedXml(XmlDocument doc)
            : base(doc)
        {

        }



        private RSA m_rsa;



        new public void AddKeyNameMapping(string keyName, object keyObject)
        {

            base.AddKeyNameMapping(keyName, keyObject);

            m_rsa = (RSA)keyObject;

        }



        public override SymmetricAlgorithm GetDecryptionKey(EncryptedData encryptedData, string symmetricAlgorithmUri)
        {

            if (symmetricAlgorithmUri == null)
            {

                EncryptionMethod encryptionMethod = encryptedData.EncryptionMethod;

                if (encryptionMethod == null) throw new CryptographicException("EncryptionMethod element for symmetric operation not found");



                symmetricAlgorithmUri = encryptionMethod.KeyAlgorithm;

                if (symmetricAlgorithmUri == null || symmetricAlgorithmUri.Trim().Length == 0) throw new CryptographicException("Algorithm attribute of EncryptionMethod element for symmetric operation is empty");

            }



            IEnumerator enumerator = encryptedData.KeyInfo.GetEnumerator(typeof(KeyInfoEncryptedKey));

            if (!enumerator.MoveNext()) throw new CryptographicException("EncryptedKey element not found");

            KeyInfoEncryptedKey keyInfo = (KeyInfoEncryptedKey)enumerator.Current;



            SymmetricAlgorithm symmetricAlgorithm = null;

            if (symmetricAlgorithmUri.Equals(EncryptedXml.XmlEncAES256Url))
            {

                symmetricAlgorithm = new RijndaelManaged();

                symmetricAlgorithm.KeySize = 256;

            }

            else if (symmetricAlgorithmUri.Equals(EncryptedXml.XmlEncAES192Url))
            {

                symmetricAlgorithm = new RijndaelManaged();

                symmetricAlgorithm.KeySize = 192;

            }

            else if (symmetricAlgorithmUri.Equals(EncryptedXml.XmlEncAES128Url))
            {

                symmetricAlgorithm = new RijndaelManaged();

                symmetricAlgorithm.KeySize = 128;

            }

            else if (symmetricAlgorithmUri.Equals(EncryptedXml.XmlEncTripleDESUrl))

                symmetricAlgorithm = new TripleDESCryptoServiceProvider();

            else if (symmetricAlgorithmUri.Equals(EncryptedXml.XmlEncDESUrl))

                symmetricAlgorithm = new DESCryptoServiceProvider();

            else

                throw new CryptographicException("EncryptionMethod [" + symmetricAlgorithmUri + "] is not support");



            if (keyInfo.EncryptedKey.EncryptionMethod == null) throw new CryptographicException("EncryptionMethod element for encrypted key not found");

            string assymmetricAlgorithmUri = keyInfo.EncryptedKey.EncryptionMethod.KeyAlgorithm;

            if (assymmetricAlgorithmUri == null || assymmetricAlgorithmUri.Trim().Length == 0) throw new CryptographicException("Algorithm attribute of EncryptionMethod element for encrypted key is empty");



            if (assymmetricAlgorithmUri.Equals(EncryptedXml.XmlEncRSA15Url))

                symmetricAlgorithm.Key = EncryptedXml.DecryptKey(keyInfo.EncryptedKey.CipherData.CipherValue, m_rsa, false);

            else if (assymmetricAlgorithmUri.Equals(EncryptedXml.XmlEncRSAOAEPUrl))

                symmetricAlgorithm.Key = EncryptedXml.DecryptKey(keyInfo.EncryptedKey.CipherData.CipherValue, m_rsa, true);

            else

                throw new CryptographicException("EncryptionMethod [" + assymmetricAlgorithmUri + "] is not support");



            return symmetricAlgorithm;

        }

    }
}
