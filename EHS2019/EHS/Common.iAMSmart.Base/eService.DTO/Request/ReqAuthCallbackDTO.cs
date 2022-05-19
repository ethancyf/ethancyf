using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Request
{
    [DataContract]
    public class ReqAuthCallBackDTO
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "errorCode")]
        public string ErrorCode { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "businessID")]
        public string BusinessID { get; set; }
    }
}
