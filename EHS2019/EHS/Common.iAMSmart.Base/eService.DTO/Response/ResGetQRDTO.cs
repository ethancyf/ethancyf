using System;
using System.Runtime.Serialization;

namespace eService.DTO.Response
{
    [DataContract]
    public class ResGetQRDTO
    {
        [DataMember(Name = "redirectUrl")]
        public string RedirectUrl{ get; set; }

        [DataMember(Name = "loginSerialNum")]
        public string LoginSerialNum { get; set; }
    }
}
