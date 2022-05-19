using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Response
{
    [DataContract]
    public class ResUploadDTO
    {
        [DataMember(Name = "fileIds")]
        public string[] FileIds { get; set; }
    }
}
