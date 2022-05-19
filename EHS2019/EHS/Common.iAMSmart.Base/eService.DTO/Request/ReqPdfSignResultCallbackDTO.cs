using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Request
{
    [DataContract]
    public class ReqPdfSignResultCallbackDTO
    {
        [DataMember(Name = "businessID")]
        public string BusinessID { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "docDigest")]
        public string DocDigest { get; set; }

        //[DataMember(Name = "timestamp")]
        //public string Timestamp { get; set; }

        [DataMember(Name = "pdfSignature")]
        public string PdfSignature { get; set; }

        //[DataMember(Name = "cert")]
        //public string Cert { get; set; }

    }
}
