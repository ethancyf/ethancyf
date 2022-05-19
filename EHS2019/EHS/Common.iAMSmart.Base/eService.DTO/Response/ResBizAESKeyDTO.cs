using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;

namespace eService.DTO.Response
{
    [DataContract]
    public class ResBizAESKeyDTO
    {
        /// <summary>
        /// e-service上传公钥证书A加密的AES256秘钥
        /// </summary>
        [DataMember(Name = "secretKey")]
        public string SecretKey { get; set; }

        /// <summary>
        /// 通过RSA公钥得知对应的私钥，然后用私钥对secretKey进行解密得到AES key
        /// </summary>
        [DataMember(Name = "pubKey")]
        public string PublicKey { get; set; }
        
    }
}
