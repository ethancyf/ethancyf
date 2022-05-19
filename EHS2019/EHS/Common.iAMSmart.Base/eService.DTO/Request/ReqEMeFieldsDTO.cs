using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Request
{
    [DataContract]
    public class ReqEMeFieldsDTO
    {
        [DataMember(Name = "passportNumber")]
        public string PassportNumber { get; set; }

        [DataMember(Name = "homeTelNumber")]
        public string HomeTelNumber { get; set; }

        [DataMember(Name = "propertyOwner")]
        public string PropertyOwner { get; set; }

        [DataMember(Name = "gender")]
        public string Gender { get; set; }

        [DataMember(Name = "prefix")]
        public string Prefix { get; set; }

        [DataMember(Name = "mobileNumber")]
        public string MobileNumber { get; set; }

        [DataMember(Name = "employmentStatus")]
        public string EmploymentStatus { get; set; }

        [DataMember(Name = "idNo")]
        public string IdNo { get; set; }

        [DataMember(Name = "birthDate")]
        public string BirthDate { get; set; }

        [DataMember(Name = "passportExpiry")]
        public string PassportExpiry { get; set; }

        [DataMember(Name = "officeTelNumber")]
        public string OfficeTelNumber { get; set; }

        [DataMember(Name = "emailAddress")]
        public string EmailAddress { get; set; }

        [DataMember(Name = "postalAddress")]
        public string PostalAddress { get; set; }

        [DataMember(Name = "educationLevel")]
        public string EducationLevel { get; set; }

        [DataMember(Name = "carOwner")]
        public string CarOwner { get; set; }

        [DataMember(Name = "residentialAddress")]
        public string ResidentialAddress { get; set; }

        [DataMember(Name = "enName")]
        public string EnName { get; set; }

        [DataMember(Name = "chName")]
        public string ChName { get; set; }

        [DataMember(Name = "maritalStatus")]
        public string MaritalStatus { get; set; }

    }
}
