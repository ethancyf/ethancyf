using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Response
{
    [DataContract]
    public class ResProfileDetailDTO
    {
        [DataMember(Name = "businessID")]
        public string BusinessID { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "idNo")]
        public IdNoDTO IDNo { get; set; }

        [DataMember(Name = "enName")]
        public EnNameDTO EnName { get; set; }

        [DataMember(Name = "chName")]
        public ChNameDTO ChName { get; set; }

        [DataMember(Name = "gender")]
        public string Gender { get; set; }

        [DataMember(Name = "birthDate")]
        public string BirthDate { get; set; }

        [DataMember(Name = "chNameVerified")]
        public string ChNameVerified { get; set; }
    }
}
