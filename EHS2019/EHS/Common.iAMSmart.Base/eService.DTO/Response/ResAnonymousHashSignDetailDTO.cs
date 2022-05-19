using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;

namespace eService.DTO.Response
{
    [DataContract]
    public class ResAnonymousHashSignDetailDTO
    {
        [DataMember(Name = "hashCode")]
        public string HashCode { get; set; }

        [DataMember(Name = "timestamp")]
        public long Timestamp { get; set; }

        [DataMember(Name = "signature")]
        public string Signature { get; set; }

        [DataMember(Name = "cert")]
        public string Cert { get; set; }
        
    }
}
