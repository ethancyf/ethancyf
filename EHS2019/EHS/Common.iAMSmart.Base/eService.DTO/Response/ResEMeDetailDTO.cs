using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Response
{
    #region child class
    [DataContract]
    public class MobileNumberDTO
    {
        [DataMember(Name = "NationalDestinationCode")]
        public string NationalDestinationCode { get; set; }
        [DataMember(Name = "CountryCode")]
        public string CountryCode { get; set; }
        [DataMember(Name = "SubscriberNumber")]
        public string SubscriberNumber { get; set; }
    }

    [DataContract]
    public class HomeTelNumberDTO
    {
        [DataMember(Name = "NationalDestinationCode")]
        public string NationalDestinationCode { get; set; }
        [DataMember(Name = "CountryCode")]
        public string CountryCode { get; set; }
        [DataMember(Name = "SubscriberNumber")]
        public string SubscriberNumber { get; set; }
    }

    [DataContract]
    public class IdNoDTO
    {
        [DataMember(Name = "Identification")]
        public string Identification { get; set; }
        [DataMember(Name = "CheckDigit")]
        public string CheckDigit { get; set; }
    }

    [DataContract]
    public class OfficeTelNumberDTO
    {
        [DataMember(Name = "NationalDestinationCode")]
        public string NationalDestinationCode { get; set; }
        [DataMember(Name = "CountryCode")]
        public string CountryCode { get; set; }
        [DataMember(Name = "SubscriberNumber")]
        public string SubscriberNumber { get; set; }
    }

    [DataContract]
    public class PostalAddressDTO
    {
        /*邮递地址*/
        [DataMember(Name = "PostBoxAddress")]
        public PostBoxAddressDTO PostBoxAddress { get; set; }

        /*地段地址*/ /*香港标准地址*/
        [DataMember(Name = "ChiPremisesAddress")]
        public ChiPremisesAddressDTO ChiPremisesAddress { get; set; }

        /*境外地址*/
        [DataMember(Name = "FreeFormatAddress")]
        public FreeFormatAddressDTO FreeFormatAddress { get; set; }

        /*香港标准地址*/
        [DataMember(Name = "EngPremisesAddress")]
        public EngPremisesAddressDTO EngPremisesAddress { get; set; }
    }

    [DataContract]
    public class PostBoxAddressDTO
    {
        [DataMember(Name = "ChiPostBox")]
        public ChiPostBoxDTO ChiPostBox { get; set; }

        [DataMember(Name = "EngPostBox")]
        public EngPostBoxDTO EngPostBox { get; set; }
    }

    [DataContract]
    public class ChiPostBoxDTO
    {
        [DataMember(Name = "PostOfficeRegion")]
        public string PostOfficeRegion { get; set; }
        [DataMember(Name = "PostOffice")]
        public string PostOffice { get; set; }
        [DataMember(Name = "PoBoxNo")]
        public string PoBoxNo { get; set; }
    }

    [DataContract]
    public class EngPostBoxDTO
    {
        [DataMember(Name = "PostOfficeRegion")]
        public string PostOfficeRegion { get; set; }
        [DataMember(Name = "PostOffice")]
        public string PostOffice { get; set; }
        [DataMember(Name = "PoBoxNo")]
        public string PoBoxNo { get; set; }
    }

    [DataContract]
    public class ResidentialAddressDTO
    {
        /*境外地址*/
        [DataMember(Name = "ChiPremisesAddress")]
        public ChiPremisesAddressDTO ChiPremisesAddress { get; set; }

        /*地段地址*/ /*香港标准地址*/ /*邮递地址*/
        [DataMember(Name = "FreeFormatAddress")]
        public FreeFormatAddressDTO FreeFormatAddress { get; set; }

        /*境外地址*/
        [DataMember(Name = "EngPremisesAddress")]
        public EngPremisesAddressDTO EngPremisesAddress { get; set; }

    }

    [DataContract]
    public class EngPremisesAddressDTO
    {
        [DataMember(Name = "EngDistrict")]
        public EngDistrictDTO EngDistrict { get; set; }
        [DataMember(Name = "EngEstate")]
        public EngEstateDTO EngEstate { get; set; }
        [DataMember(Name = "BuildingName")]
        public string BuildingName { get; set; }
        [DataMember(Name = "EngBlock")]
        public EngBlockDTO EngBlock { get; set; }
        [DataMember(Name = "Region")]
        public string Region { get; set; }
        [DataMember(Name = "EngStreet")]
        public EngStreetDTO EngStreet { get; set; }
        [DataMember(Name = "Eng3dAddress")]
        public Eng3dAddressDTO Eng3dAddress { get; set; }
        [DataMember(Name = "EngLot")]
        public EngLotDTO EngLot { get; set; }
    }

    [DataContract]
    public class FreeFormatAddressDTO
    {
        [DataMember(Name = "AddressLine1")]
        public string AddressLine1 { get; set; }
        [DataMember(Name = "AddressLine2")]
        public string AddressLine2 { get; set; }
        [DataMember(Name = "AddressLine3")]
        public string AddressLine3 { get; set; }
        [DataMember(Name = "LanguageCode")]
        public string LanguageCode { get; set; }
    }

    [DataContract]
    public class ChiPremisesAddressDTO
    {
        [DataMember(Name = "Chi3dAddress")]
        public Chi3dAddressDTO Chi3dAddress { get; set; }
        [DataMember(Name = "ChiBlock")]
        public ChiBlockDTO ChiBlock { get; set; }
        [DataMember(Name = "BuildingName")]
        public string BuildingName { get; set; }
        [DataMember(Name = "ChiDistrict")]
        public ChiDistrictDTO ChiDistrict { get; set; }
        [DataMember(Name = "Region")]
        public string Region { get; set; }
        [DataMember(Name = "ChiEstate")]
        public ChiEstateDTO ChiEstate { get; set; }
        [DataMember(Name = "ChiStreet")]
        public ChiStreetDTO ChiStreet { get; set; }

        /* Type=2 */
        [DataMember(Name = "ChiLot")]
        public ChiLotDTO ChiLot { get; set; }

        /* Type=3 */
        [DataMember(Name = "ChiOutside")]
        public ChiOutsideDTO ChiOutside { get; set; }

    }

    [DataContract]
    public class EngDistrictDTO
    {
        [DataMember(Name = "DcDistrict")]
        public string DcDistrict { get; set; }
        [DataMember(Name = "Sub-district")]
        public string SubDistrict { get; set; }
    }

    [DataContract]
    public class EngEstateDTO
    {
        [DataMember(Name = "EstateName")]
        public string EstateName { get; set; }
        [DataMember(Name = "EngPhase")]
        public EngPhaseDTO EngPhase { get; set; }
    }

    [DataContract]
    public class EngPhaseDTO
    {
        [DataMember(Name = "PhaseName")]
        public string PhaseName { get; set; }
    }

    [DataContract]
    public class EngBlockDTO
    {
        [DataMember(Name = "BlockDescriptor")]
        public string BlockDescriptor { get; set; }
        [DataMember(Name = "BlockNo")]
        public string BlockNo { get; set; }
    }

    [DataContract]
    public class EngStreetDTO
    {
        [DataMember(Name = "StreetName")]
        public string StreetName { get; set; }
        [DataMember(Name = "BuildingNoFrom")]
        public string BuildingNoFrom { get; set; }
    }

    [DataContract]
    public class Eng3dAddressDTO
    {
        [DataMember(Name = "EngFloor")]
        public EngFloorDTO EngFloor { get; set; }
        [DataMember(Name = "EngUnit")]
        public EngUnitDTO EngUnit { get; set; }
    }

    [DataContract]
    public class EngUnitDTO
    {
        [DataMember(Name = "UnitDescriptor")]
        public string UnitDescriptor { get; set; }
        [DataMember(Name = "UnitNo")]
        public string UnitNo { get; set; }
    }

    [DataContract]
    public class EngFloorDTO
    {
        [DataMember(Name = "Floor")]
        public string Floor { get; set; }
    }

    [DataContract]
    public class EngLotDTO
    {
        [DataMember(Name = "EngStructureLot")]
        public EngStructureLotDTO EngStructureLot { get; set; }
    }

    [DataContract]
    public class EngStructureLotDTO
    {
        [DataMember(Name = "DdNo")]
        public string DdNo { get; set; }
        [DataMember(Name = "LotNo")]
        public string LotNo { get; set; }
        [DataMember(Name = "LotSection2")]
        public string LotSection2 { get; set; }
        [DataMember(Name = "LotType")]
        public string LotType { get; set; }
        [DataMember(Name = "LotSection3")]
        public string LotSection3 { get; set; }
        [DataMember(Name = "LotSection1")]
        public string LotSection1 { get; set; }
        [DataMember(Name = "DdType")]
        public string DdType { get; set; }
        [DataMember(Name = "LotSubsection1")]
        public string LotSubsection1 { get; set; }
        [DataMember(Name = "LotSubsection2")]
        public string LotSubsection2 { get; set; }
        [DataMember(Name = "LotExtendPortionCode")]
        public string LotExtendPortionCode { get; set; }
        [DataMember(Name = "LotSubsection3")]
        public string LotSubsection3 { get; set; }
    }

    [DataContract]
    public class Chi3dAddressDTO
    {
        [DataMember(Name = "ChiUnit")]
        public ChiUnitDTO ChiUnit { get; set; }
        [DataMember(Name = "ChiFloor")]
        public ChiFloorDTO ChiFloor { get; set; }
    }

    [DataContract]
    public class ChiUnitDTO
    {
        [DataMember(Name = "UnitDescriptor")]
        public string UnitDescriptor { get; set; }
        [DataMember(Name = "UnitNo")]
        public string UnitNo { get; set; }
    }

    [DataContract]
    public class ChiFloorDTO
    {
        [DataMember(Name = "Floor")]
        public string Floor { get; set; }
    }

    [DataContract]
    public class ChiBlockDTO
    {
        [DataMember(Name = "BlockDescriptor")]
        public string BlockDescriptor { get; set; }
        [DataMember(Name = "BlockNo")]
        public string BlockNo { get; set; }
    }

    [DataContract]
    public class ChiDistrictDTO
    {
        [DataMember(Name = "DcDistrict")]
        public string DcDistrict { get; set; }
        [DataMember(Name = "Sub-district")]
        public string SubDistrict { get; set; }
    }

    [DataContract]
    public class ChiEstateDTO
    {
        [DataMember(Name = "EstateName")]
        public string EstateName { get; set; }
        [DataMember(Name = "ChiPhase")]
        public ChiPhaseDTO ChiPhase { get; set; }
    }

    [DataContract]
    public class ChiPhaseDTO
    {
        [DataMember(Name = "PhaseName")]
        public string PhaseName { get; set; }
    }

    [DataContract]
    public class ChiStreetDTO
    {
        [DataMember(Name = "StreetName")]
        public string StreetName { get; set; }
        [DataMember(Name = "BuildingNoFrom")]
        public string BuildingNoFrom { get; set; }
    }

    public class ChiLotDTO
    {
        [DataMember(Name = "ChiStructuredLot")]
        public ChiStructuredLotDTO ChiStructuredLot { get; set; }
    }

    public class ChiStructuredLotDTO
    {
        [DataMember(Name = "DdNo")]
        public string DdNo { get; set; }
        [DataMember(Name = "LotNo")]
        public string LotNo { get; set; }
        [DataMember(Name = "LotSection2")]
        public string LotSection2 { get; set; }
        [DataMember(Name = "LotType")]
        public string LotType { get; set; }
        [DataMember(Name = "LotSection3")]
        public string LotSection3 { get; set; }
        [DataMember(Name = "LotSection1")]
        public string LotSection1 { get; set; }
        [DataMember(Name = "DdType")]
        public string DdType { get; set; }
        [DataMember(Name = "LotSection4")]
        public string LotSection4 { get; set; }
        [DataMember(Name = "LotSubsection1")]
        public string LotSubsection1 { get; set; }
        [DataMember(Name = "LotSubsection2")]
        public string LotSubsection2 { get; set; }
        [DataMember(Name = "LotExtendPortionCode")]
        public string LotExtendPortionCode { get; set; }
        [DataMember(Name = "LotSubsection3")]
        public string LotSubsection3 { get; set; }
        [DataMember(Name = "LotSubsection4")]
        public string LotSubsection4 { get; set; }
    }

    public class ChiOutsideDTO
    {
        [DataMember(Name = "ChiStructuredOutside")]
        public ChiStructuredOutsideDTO ChiStructuredOutside { get; set; }
    }

    public class ChiStructuredOutsideDTO
    {
        [DataMember(Name = "LineOne")]
        public string LineOne { get; set; }
        [DataMember(Name = "LineTwo")]
        public string LineTwo { get; set; }
        [DataMember(Name = "LineThree")]
        public string LineThree { get; set; }
    }

    [DataContract]
    public class EnNameDTO
    {
        [DataMember(Name = "UnstructuredName")]
        public string UnstructuredName { get; set; }
    }

    [DataContract]
    public class ChNameDTO
    {
        [DataMember(Name = "ChineseName")]
        public string ChineseName { get; set; }
    }

    #endregion


    [DataContract]
    public class ResEMeDetailDTO
    {
        [DataMember(Name = "passportNumber")]
        public string PassportNumber { get; set; }

        [DataMember(Name = "homeTelNumber")]
        public HomeTelNumberDTO HomeTelNumber { get; set; }

        [DataMember(Name = "propertyOwner")]
        public string PropertyOwner { get; set; }

        [DataMember(Name = "gender")]
        public string Gender { get; set; }

        [DataMember(Name = "prefix")]
        public string Prefix { get; set; }

        [DataMember(Name = "mobileNumber")]
        public MobileNumberDTO MobileNumber { get; set; }

        [DataMember(Name = "businessID")]
        public string BusinessID { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "employmentStatus")]
        public string EmploymentStatus { get; set; }

        [DataMember(Name = "birthDate")]
        public string BirthDate { get; set; }

        [DataMember(Name = "idNo")]
        public IdNoDTO IdNo { get; set; }

        [DataMember(Name = "passportExpiry")]
        public string PassportExpiry { get; set; }

        [DataMember(Name = "officeTelNumber")]
        public OfficeTelNumberDTO OfficeTelNumber { get; set; }

        [DataMember(Name = "emailAddress")]
        public string EmailAddress { get; set; }

        public string Type { get; set; }

        [DataMember(Name = "postalAddress")]
        public PostalAddressDTO PostalAddress { get; set; }

        public string Language { get; set; }

        [DataMember(Name = "educationLevel")]
        public string EducationLevel { get; set; }

        [DataMember(Name = "carOwner")]
        public string CarOwner { get; set; }

        [DataMember(Name = "residentialAddress")]
        public ResidentialAddressDTO ResidentialAddress { get; set; }

        [DataMember(Name = "enName")]
        public EnNameDTO EnName { get; set; }

        [DataMember(Name = "chName")]
        public ChNameDTO ChName { get; set; }

        [DataMember(Name = "maritalStatus")]
        public string MaritalStatus { get; set; }

        [DataMember(Name = "chNameVerified")]
        public string ChNameVerified { get; set; }
    }
}
