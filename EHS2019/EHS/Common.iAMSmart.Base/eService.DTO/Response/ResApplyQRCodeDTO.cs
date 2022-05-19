using System;
using System.Runtime.Serialization;

namespace eService.DTO.Response
{
    [DataContract]
    public class ResApplyQRCodeDTO
    {
        [DataMember(Name = "redirectUrl")]
        public string RedirectUrl{ get; set; }

    }
}
