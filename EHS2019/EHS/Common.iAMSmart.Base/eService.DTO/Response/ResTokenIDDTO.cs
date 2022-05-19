using System.Runtime.Serialization;

namespace eService.DTO.Response
{
    [DataContract]
    public class ResTokenIDDTO
    {
        [DataMember(Name = "tokenID")]
        public string TokenID { get; set; }

        [DataMember(Name = "profileExist")]
        public bool IsProfileExist { get; set; }
    }
}
