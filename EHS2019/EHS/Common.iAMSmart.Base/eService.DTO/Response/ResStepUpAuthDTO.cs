using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Response
{
    [DataContract]
    public class ResStepUpAuthDTO
    {
        [DataMember(Name = "businessID")]
        public string BusinessID { get; set; }

        [DataMember(Name = "authByQR")]
        public bool AuthByQR { get; set; }

        [DataMember(Name = "ticketID")]
        public string TicketID { get; set; }
    }
}
