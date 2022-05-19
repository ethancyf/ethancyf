using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Request
{
    [DataContract]
    public class ReqEncryptBizPostDTO
    {
        /// <summary>
        /// 加密后数据
        /// </summary>
        [DataMember(Name = "content")]
        public string Content { get; set; }
    }
}
