using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Request
{
    [DataContract]
    public class ReqHashSignDTO
    {
        [DataMember(Name = "businessID")]
        public string BusinessID { get; set; }

        [DataMember(Name = "accessToken")]
        public string AccessToken { get; set; }

        [DataMember(Name = "openID")]
        public string OpenID { get; set; }

        [DataMember(Name = "source")]
        public string Source { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "hashCode")]
        public string HashCode { get; set; }

        [DataMember(Name = "redirectURI")]
        public string RedirectURI { get; set; }

        [DataMember(Name = "department")]
        public string Department { get; set; }

        [DataMember(Name = "documentName")]
        public string DocumentName { get; set; }

        [DataMember(Name = "serviceName")]
        public string ServiceName { get; set; }

        [DataMember(Name = "HKICHash")]
        public string HKICHash { get; set; }


    }
}
