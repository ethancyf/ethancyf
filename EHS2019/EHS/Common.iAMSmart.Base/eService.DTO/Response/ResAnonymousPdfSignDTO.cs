using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;

namespace eService.DTO.Response
{
    [DataContract]
    public class ResAnonymousPdfSignDTO
    {
        [DataMember(Name = "businessID")]
        public string BusinessID { get; set; }

        [DataMember(Name = "department")]
        public string Department { get; set; }

        [DataMember(Name = "serviceName")]
        public string ServiceName { get; set; }

        [DataMember(Name = "documentName")]
        public string DocumentName { get; set; }
        
        [DataMember(Name = "redirectURI")]
        public string RedirectURI { get; set; }

        [DataMember(Name = "signCode")]
        public string SignCode { get; set; }

        [DataMember(Name = "authByQR")]
        public bool AuthByQR { get; set; }

        [DataMember(Name = "ticketID")]
        public string TicketID { get; set; }
    }
}
