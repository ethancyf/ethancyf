using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Response
{
    [DataContract]
    public class ResSignDownloadDTO
    {
        [DataMember(Name = "fileContent")]
        public string FileContent { get; set; }
    }
}
