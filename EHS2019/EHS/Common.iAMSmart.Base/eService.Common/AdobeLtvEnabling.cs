using iTextSharp.text;
using iTextSharp.text.error_messages;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace eService.Common
{
    public class AdobeLtvEnabling
    {
        public static readonly LogUtils LogUtils = new LogUtils(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AdobeLtvEnabling(PdfStamper pdfStamper)
        {
            this.pdfStamper = pdfStamper;
        }

        /// <summary>
        /// Call this method to have LTV information added to the {@link PdfStamper} given in the constructor.
        /// </summary>
        /// <param name="ocspClient"></param>
        /// <param name="crlClient"></param>
        public void Enable(IOcspClient ocspClient, ICrlClient crlClient)
        {
            AcroFields fields = pdfStamper.AcroFields;
            bool encrypted = pdfStamper.Reader.IsEncrypted();

            List<string> names = fields.GetSignatureNames();
            foreach (string name in names)
            {
                PdfPKCS7 pdfPKCS7 = fields.VerifySignature(name);
                PdfDictionary signatureDictionary = fields.GetSignatureDictionary(name);
                X509Certificate certificate = pdfPKCS7.SigningCertificate;
                AddLtvForChain(certificate, ocspClient, crlClient, GetSignatureHashKey(signatureDictionary, encrypted));
            }

            OutputDss();
        }

        /// <summary>
        /// the actual LTV enabling methods
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="ocspClient"></param>
        /// <param name="crlClient"></param>
        /// <param name="key"></param>
        private void AddLtvForChain(X509Certificate certificate, IOcspClient ocspClient, ICrlClient crlClient, PdfName key)
        {
            if (SeenCertificates.Contains(certificate))
            {
                return;
            }
            SeenCertificates.Add(certificate);
            ValidationData validationData = new ValidationData();
            while (certificate != null)
            {
                LogUtils.Info("subjectDN:" + certificate.SubjectDN);
                X509Certificate issuer = GetIssuerCertificate(certificate);
                validationData.certs.Add(certificate.GetEncoded());
                byte[] ocspResponse = ocspClient.GetEncoded(certificate, issuer, null);
                if (ocspResponse != null)
                {
                    LogUtils.Info("  with OCSP response");
                    validationData.ocsps.Add(ocspResponse);
                    X509Certificate ocspSigner = GetOcspSignerCertificate(ocspResponse);
                    if (ocspSigner != null)
                    {
                        LogUtils.Info(string.Format("  signed by {0}\n", ocspSigner.SubjectDN));
                    }
                    AddLtvForChain(ocspSigner, ocspClient, crlClient, GetOcspHashKey(ocspResponse));
                }
                else
                {
                    ICollection<byte[]> crl = crlClient.GetEncoded(certificate, null);
                    if (crl != null && crl.Count > 0)
                    {
                        LogUtils.Info(string.Format("  with {0} CRLs\n", crl.Count));
                        foreach (byte[] crlBytes in crl)
                        {
                            validationData.crls.Add(crlBytes);
                            AddLtvForChain(null, ocspClient, crlClient, GetCrlHashKey(crlBytes));
                        }
                    }
                }
                certificate = issuer;
            }
            validated[key] = validationData;
        }

        private void OutputDss()
        {
            PdfWriter writer = pdfStamper.Writer;
            PdfReader reader = pdfStamper.Reader;

            PdfDictionary dss = new PdfDictionary();
            PdfDictionary vrim = new PdfDictionary();
            PdfArray ocsps = new PdfArray();
            PdfArray crls = new PdfArray();
            PdfArray certs = new PdfArray();

            writer.AddDeveloperExtension(PdfDeveloperExtension.ESIC_1_7_EXTENSIONLEVEL5);
            writer.AddDeveloperExtension(new PdfDeveloperExtension(PdfName.ADBE, new PdfName("1.7"), 8));

            PdfDictionary catalog = reader.Catalog;
            pdfStamper.MarkUsed(catalog);
            foreach (PdfName vkey in validated.Keys)
            {
                PdfArray ocsp = new PdfArray();
                PdfArray crl = new PdfArray();
                PdfArray cert = new PdfArray();
                PdfDictionary vri = new PdfDictionary();
                foreach (byte[] b in validated[vkey].crls)
                {
                    PdfStream ps = new PdfStream(b);
                    ps.FlateCompress();
                    PdfIndirectReference iref = writer.AddToBody(ps, false).IndirectReference;
                    crl.Add(iref);
                    crls.Add(iref);
                }
                foreach (byte[] b in validated[vkey].ocsps)
                {
                    PdfStream ps = new PdfStream(BuildOCSPResponse(b));
                    ps.FlateCompress();
                    PdfIndirectReference iref = writer.AddToBody(ps, false).IndirectReference;
                    ocsp.Add(iref);
                    ocsps.Add(iref);
                }
                foreach (byte[] b in validated[vkey].certs)
                {
                    PdfStream ps = new PdfStream(b);
                    ps.FlateCompress();
                    PdfIndirectReference iref = writer.AddToBody(ps, false).IndirectReference;
                    cert.Add(iref);
                    certs.Add(iref);
                }
                if (ocsp.Length > 0)
                    vri.Put(PdfName.OCSP, writer.AddToBody(ocsp, false).IndirectReference);
                if (crl.Length > 0)
                    vri.Put(PdfName.CRL, writer.AddToBody(crl, false).IndirectReference);
                if (cert.Length > 0)
                    vri.Put(PdfName.CERT, writer.AddToBody(cert, false).IndirectReference);
                vri.Put(PdfName.TU, new PdfDate());
                vrim.Put(vkey, writer.AddToBody(vri, false).IndirectReference);
            }
            dss.Put(PdfName.VRI, writer.AddToBody(vrim, false).IndirectReference);
            if (ocsps.Length > 0)
                dss.Put(PdfName.OCSPS, writer.AddToBody(ocsps, false).IndirectReference);
            if (crls.Length > 0)
                dss.Put(PdfName.CRLS, writer.AddToBody(crls, false).IndirectReference);
            if (certs.Length > 0)
                dss.Put(PdfName.CERTS, writer.AddToBody(certs, false).IndirectReference);
            catalog.Put(PdfName.DSS, writer.AddToBody(dss, false).IndirectReference);
        }

        /// <summary>
        /// VRI signature hash key calculation
        /// </summary>
        /// <param name="crlBytes"></param>
        /// <returns></returns>
        private static PdfName GetCrlHashKey(byte[] crlBytes)
        {
            X509Crl crl = new X509Crl(CertificateList.GetInstance(crlBytes));
            byte[] signatureBytes = crl.GetSignature();
            DerOctetString octetString = new DerOctetString(signatureBytes);
            byte[] octetBytes = octetString.GetEncoded();
            byte[] octetHash = HashBytesSha1(octetBytes);
            PdfName octetName = new PdfName(Utilities.ConvertToHex(octetHash));
            return octetName;
        }

        private static PdfName GetOcspHashKey(byte[] basicResponseBytes)
        {
            BasicOcspResponse basicResponse = BasicOcspResponse.GetInstance(Asn1Sequence.GetInstance(basicResponseBytes));
            byte[] signatureBytes = basicResponse.Signature.GetBytes();
            DerOctetString octetString = new DerOctetString(signatureBytes);
            byte[] octetBytes = octetString.GetEncoded();
            byte[] octetHash = HashBytesSha1(octetBytes);
            PdfName octetName = new PdfName(Utilities.ConvertToHex(octetHash));
            return octetName;
        }

        private static PdfName GetSignatureHashKey(PdfDictionary dic, bool encrypted)
        {
            PdfString contents = dic.GetAsString(PdfName.CONTENTS);
            byte[] bc = contents.GetOriginalBytes();
            if (PdfName.ETSI_RFC3161.Equals(PdfReader.GetPdfObject(dic.Get(PdfName.SUBFILTER))))
            {
                using (Asn1InputStream din = new Asn1InputStream(bc))
                {
                    Asn1Object pkcs = din.ReadObject();
                    bc = pkcs.GetEncoded();
                }
            }
            byte[] bt = HashBytesSha1(bc);
            return new PdfName(Utilities.ConvertToHex(bt));
        }

        private static byte[] HashBytesSha1(byte[] b)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            return sha.ComputeHash(b);
        }

        /// <summary>
        /// OCSP response helpers
        /// </summary>
        /// <param name="basicResponseBytes"></param>
        /// <returns></returns>
        private static X509Certificate GetOcspSignerCertificate(byte[] basicResponseBytes)
        {
            BasicOcspResponse borRaw = BasicOcspResponse.GetInstance(Asn1Sequence.GetInstance(basicResponseBytes));
            BasicOcspResp bor = new BasicOcspResp(borRaw);

            foreach (X509Certificate x509Certificate in bor.GetCerts())
            {
                if (bor.Verify(x509Certificate.GetPublicKey()))
                {
                    return x509Certificate;
                }
            }
            return null;
        }

        private static byte[] BuildOCSPResponse(byte[] basicOCSPResponse)
        {
            DerOctetString doctet = new DerOctetString(basicOCSPResponse);
            Asn1EncodableVector v2 = new Asn1EncodableVector();
            v2.Add(OcspObjectIdentifiers.PkixOcspBasic);
            v2.Add(doctet);
            DerEnumerated den = new DerEnumerated(0);
            Asn1EncodableVector v3 = new Asn1EncodableVector();
            v3.Add(den);
            v3.Add(new DerTaggedObject(true, 0, new DerSequence(v2)));
            DerSequence seq = new DerSequence(v3);
            return seq.GetEncoded();
        }

        private static X509Certificate GetIssuerCertificate(X509Certificate certificate)
        {
            string url = GetCACURL(certificate);
            if (url != null && url.Length > 0)
            {
                HttpWebRequest con = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)con.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new IOException(MessageLocalization.GetComposedMessage("invalid.http.response.1", (int)response.StatusCode));
                }

                //Get Response
                Stream inp = response.GetResponseStream();
                byte[] buf = new byte[1024];
                MemoryStream bout = new MemoryStream();
                while (true)
                {
                    int n = inp.Read(buf, 0, buf.Length);
                    if (n <= 0)
                    {
                        break;
                    }
                    bout.Write(buf, 0, n);
                }
                inp.Close();

                var cert2 = new System.Security.Cryptography.X509Certificates.X509Certificate2(bout.ToArray());

                return new X509Certificate(X509CertificateStructure.GetInstance(cert2.GetRawCertData()));
            }

            try
            {
                certificate.Verify(certificate.GetPublicKey());
                return null;
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
            }

            foreach (X509Certificate candidate in extraCertificates)
            {
                try
                {
                    certificate.Verify(candidate.GetPublicKey());
                    return candidate;
                }
                catch (Exception ex)
                {
                    LogUtils.Error(ex.Message);
                }
            }
            return null;
        }

        private static string GetCACURL(X509Certificate certificate)
        {
            try
            {
                Asn1Object obj = GetExtensionValue(certificate, X509Extensions.AuthorityInfoAccess.Id);
                if (obj == null)
                {
                    return null;
                }

                Asn1Sequence AccessDescriptions = (Asn1Sequence)obj;
                for (int i = 0; i < AccessDescriptions.Count; i++)
                {
                    Asn1Sequence AccessDescription = (Asn1Sequence)AccessDescriptions[i];
                    if (AccessDescription.Count != 2)
                    {
                        continue;
                    }
                    else
                    {
                        if ((AccessDescription[0] is DerObjectIdentifier) && ((DerObjectIdentifier)AccessDescription[0]).Id.Equals("1.3.6.1.5.5.7.48.2"))
                        {
                            string accessLocation = GetStringFromGeneralName((Asn1Object)AccessDescription[1]);
                            return accessLocation == null ? "" : accessLocation;
                        }
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        private static Asn1Object GetExtensionValue(X509Certificate certificate, string oid)
        {
            byte[] bytes = certificate.GetExtensionValue(new DerObjectIdentifier(oid)).GetDerEncoded();
            if (bytes == null)
            {
                return null;
            }
            Asn1InputStream aIn = new Asn1InputStream(new MemoryStream(bytes));
            Asn1OctetString octs = (Asn1OctetString)aIn.ReadObject();
            aIn = new Asn1InputStream(new MemoryStream(octs.GetOctets()));
            return aIn.ReadObject();
        }

        private static string GetStringFromGeneralName(Asn1Object names)
        {
            Asn1TaggedObject taggedObject = (Asn1TaggedObject)names;
            return Encoding.GetEncoding(1252).GetString(Asn1OctetString.GetInstance(taggedObject, false).GetOctets());
        }

        //
        // inner class
        //
        class ValidationData
        {
            public IList<byte[]> crls = new List<byte[]>();
            public IList<byte[]> ocsps = new List<byte[]>();
            public IList<byte[]> certs = new List<byte[]>();
        }

        //
        // member variables
        //
        PdfStamper pdfStamper;
        HashSet<X509Certificate> SeenCertificates = new HashSet<X509Certificate>();
        IDictionary<PdfName, ValidationData> validated = new Dictionary<PdfName, ValidationData>();

        public static List<X509Certificate> extraCertificates = new List<X509Certificate>();
    }
}
