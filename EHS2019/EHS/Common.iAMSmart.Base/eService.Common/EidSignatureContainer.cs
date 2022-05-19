using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace eService.Common
{
    public class EidSignatureContainer : IExternalSignatureContainer
    {
        private byte[] PdfSignature;

        public EidSignatureContainer(byte[] pdfSignature)
        {
            this.PdfSignature = pdfSignature;
        }

        public void ModifySigningDictionary(PdfDictionary signDic)
        {
        }

        public byte[] Sign(Stream data)
        {
            return PdfSignature;
        }
    }
}
