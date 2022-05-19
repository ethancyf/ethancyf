using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;

namespace eService.DTO.Response
{
    [DataContract]
    public class ResAnonymousPdfSignDetailDTO
    {
        [DataMember(Name = "docDigest")]
        public string DocDigest { get; set; }

        [DataMember(Name = "pdfSignature")]
        public string PdfSignature { get; set; }
        
    }
}
