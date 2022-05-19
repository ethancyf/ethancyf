using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Response
{
    [DataContract]
    [Serializable]
    public class AccessTokenDTO
    {
        [DataMember(Name = "accessToken")]
        public string AccessToken { get; set; }

        [DataMember(Name = "tokenType")]
        public string TokenType { get; set; }

        [DataMember(Name = "issueAt")]
        public string IssueAt { get; set; }

        [DataMember(Name = "expireIn")]
        public string ExpiresIn { get; set; }

        [DataMember(Name = "openID")]
        public string OpenID { get; set; }

        [DataMember(Name = "userType")]
        public string UserType { get; set; }

        [DataMember(Name = "scope")]
        public string Scope { get; set; }

        [DataMember(Name = "userID")]
        public string UserID { get; set; }

    }
}
