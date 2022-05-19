using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Request
{
    [DataContract]
    public class ReqStepUpAuthCallbackDTO
    {
        [DataMember(Name = "businessID")]
        public string BusinessID { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "isPassed")]
        public string IsPassed { get; set; }

    }
}
