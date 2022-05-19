using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace eService.DTO.Request
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
        private PostBoxAddressDTO _postBoxAddress;
        private ChiPremisesAddressDTO _chiPremisesAddress;
        private FreeFormatAddressDTO _freeFormatAddress;
        private EngPremisesAddressDTO _engPremisesAddress;

        /*邮递地址*/
        [DataMember(Name = "PostBoxAddress")]
        public PostBoxAddressDTO PostBoxAddress
        {
            get
            {
                if (_postBoxAddress == null)
                {
                    return new PostBoxAddressDTO();
                }
                return _postBoxAddress;
            }
            set { _postBoxAddress = value;}
        }

        /*地段地址*/ /*香港标准地址*/
        [DataMember(Name = "ChiPremisesAddress")]
        public ChiPremisesAddressDTO ChiPremisesAddress
        {
            get
            {
                if (_chiPremisesAddress == null)
                {
                    return new ChiPremisesAddressDTO();
                }
                return _chiPremisesAddress;
            }
            set {_chiPremisesAddress = value;}
        }

        /*境外地址*/
        [DataMember(Name = "FreeFormatAddress")]
        public FreeFormatAddressDTO FreeFormatAddress
        {
            get
            {
                if (_freeFormatAddress == null)
                {
                    return new FreeFormatAddressDTO();
                }
                return _freeFormatAddress;
            }
            set { _freeFormatAddress = value;}
        }

        /*香港标准地址*/
        [DataMember(Name = "EngPremisesAddress")]
        public EngPremisesAddressDTO EngPremisesAddress
        {
            get
            {
                if (_engPremisesAddress == null)
                {
                    return new EngPremisesAddressDTO();
                }
                return _engPremisesAddress;
            }
            set { _engPremisesAddress = value;}
        }
    }

    [DataContract]
    public class PostBoxAddressDTO
    {
        private ChiPostBoxDTO _chiPostBox;
        private EngPostBoxDTO _engPostBox;

        [DataMember(Name = "ChiPostBox")]
        public ChiPostBoxDTO ChiPostBox
        {
            get
            {
                if (_chiPostBox == null)
                {
                    return new ChiPostBoxDTO();
                }
                return _chiPostBox;
            }
            set { _chiPostBox = value;}
        }

        [DataMember(Name = "EngPostBox")]
        public EngPostBoxDTO EngPostBox
        {
            get
            {
                if (_engPostBox == null)
                {
                    return new EngPostBoxDTO();
                }
                return _engPostBox;
            }
            set { _engPostBox = value;}
        }
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
        private ChiPremisesAddressDTO _chiPremisesAddress;
        private FreeFormatAddressDTO _freeFormatAddress;
        private EngPremisesAddressDTO _engPremisesAddress;

        /*境外地址*/
        [DataMember(Name = "ChiPremisesAddress")]
        public ChiPremisesAddressDTO ChiPremisesAddress
        {
            get
            {
                if (_chiPremisesAddress == null)
                {
                    return new ChiPremisesAddressDTO();
                }
                return _chiPremisesAddress;
            }
            set { _chiPremisesAddress = value;}
        }

        /*地段地址*/ /*香港标准地址*/ /*邮递地址*/
        [DataMember(Name = "FreeFormatAddress")]
        public FreeFormatAddressDTO FreeFormatAddress
        {
            get
            {
                if (_freeFormatAddress == null)
                {
                    return new FreeFormatAddressDTO();
                }
                return _freeFormatAddress;
            }
            set { _freeFormatAddress = value;}
        }

        /*境外地址*/
        [DataMember(Name = "EngPremisesAddress")]
        public EngPremisesAddressDTO EngPremisesAddress
        {
            get
            {
                if (_engPremisesAddress == null)
                {
                    return new EngPremisesAddressDTO();
                }
                return _engPremisesAddress;
            }
            set { _engPremisesAddress = value;}
        }
    }

    [DataContract]
    public class EngPremisesAddressDTO
    {
        private EngEstateDTO _engEstate;
        private EngBlockDTO _engBlock;
        private EngStreetDTO _engStreet;
        private Eng3dAddressDTO _eng3dAddress;
        private EngLotDTO _engLot;

        [DataMember(Name = "EngDistrict")]
        public EngDistrictDTO EngDistrict { get; set; }
        [DataMember(Name = "EngEstate")]
        public EngEstateDTO EngEstate
        {
            get
            {
                if (_engEstate == null)
                {
                    return new EngEstateDTO();
                }
                return _engEstate;
            }
            set { _engEstate = value;}
        }
        [DataMember(Name = "BuildingName")]
        public string BuildingName { get; set; }
        [DataMember(Name = "EngBlock")]
        public EngBlockDTO EngBlock
        {
            get
            {
                if (_engBlock == null)
                {
                    return new EngBlockDTO();
                }
                return _engBlock;
            }
            set { _engBlock = value;}
        }
        [DataMember(Name = "Region")]
        public string Region { get; set; }
        [DataMember(Name = "EngStreet")]
        public EngStreetDTO EngStreet
        {
            get
            {
                if (_engStreet == null)
                {
                    return new EngStreetDTO();
                }
                return _engStreet;
            }
            set { _engStreet = value;}
        }
        [DataMember(Name = "Eng3dAddress")]
        public Eng3dAddressDTO Eng3dAddress
        {
            get
            {
                if (_eng3dAddress == null)
                {
                    return new Eng3dAddressDTO();
                }
                return _eng3dAddress;
            }
            set { _eng3dAddress = value;}
        }
        [DataMember(Name = "EngLot")]
        public EngLotDTO EngLot
        {
            get
            {
                if (_engLot == null)
                {
                    return new EngLotDTO();
                }
                return _engLot;
            }
            set { _engLot = value;}
        }
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
    public class PremisesAddressDTO
    {
        private ChiPremisesAddressDTO _chiPremisesAddress;

        [DataMember(Name = "ChiPremisesAddress")]
        public ChiPremisesAddressDTO ChiPremisesAddress
        {
            get
            {
                if (_chiPremisesAddress == null)
                {
                    return new ChiPremisesAddressDTO();
                }
                return _chiPremisesAddress;
            }
            set { _chiPremisesAddress = value;}
        }
    }

    [DataContract]
    public class ChiPremisesAddressDTO
    {
        private ChiEstateDTO _chiEstate;
        private Chi3dAddressDTO _chi3dAddress;
        private ChiBlockDTO _chiBlock;
        private ChiDistrictDTO _chiDistrict;
        private ChiStreetDTO _chiStreet;
        private ChiLotDTO _chiLot;
        private ChiOutsideDTO _chiOutside;

        [DataMember(Name = "Chi3dAddress")]
        public Chi3dAddressDTO Chi3dAddress
        {
            get
            {
                if (_chi3dAddress == null)
                {
                    return new Chi3dAddressDTO();
                }
                return _chi3dAddress;
            }
            set { _chi3dAddress = value;}
        }
        [DataMember(Name = "ChiBlock")]
        public ChiBlockDTO ChiBlock
        {
            get
            {
                if (_chiBlock == null)
                {
                    return new ChiBlockDTO();
                }
                return _chiBlock;
            }
            set { _chiBlock = value;}
        }
        [DataMember(Name = "BuildingName")]
        public string BuildingName { get; set; }
        [DataMember(Name = "ChiDistrict")]
        public ChiDistrictDTO ChiDistrict
        {
            get
            {
                if (_chiDistrict == null)
                {
                    return new ChiDistrictDTO();
                }
                return _chiDistrict;
            }
            set { _chiDistrict = value;}
        }
        [DataMember(Name = "Region")]
        public string Region { get; set; }
        [DataMember(Name = "ChiEstate")]
        public ChiEstateDTO ChiEstate
        {
            get
            {
                if (_chiEstate == null)
                {
                    return new ChiEstateDTO();
                }
                return _chiEstate;
            }
            set {_chiEstate = value;}
        }
        [DataMember(Name = "ChiStreet")]
        public ChiStreetDTO ChiStreet
        {
            get
            {
                if (_chiStreet == null)
                {
                    return new ChiStreetDTO();
                }
                return _chiStreet;
            }
            set { _chiStreet = value;}
        }

        /* Type=2 */
        [DataMember(Name = "ChiLot")]
        public ChiLotDTO ChiLot
        {
            get
            {
                if (_chiLot == null)
                {
                    return new ChiLotDTO();
                }
                return _chiLot;
            }
            set { _chiLot = value;}
        }

        /* Type=3 */
        [DataMember(Name = "ChiOutside")]
        public ChiOutsideDTO ChiOutside
        {
            get
            {
                if (_chiOutside == null)
                {
                    return new ChiOutsideDTO();
                }
                return _chiOutside;
            }
            set { _chiOutside = value;}
        }
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
        private EngPhaseDTO _engPhase;

        [DataMember(Name = "EstateName")]
        public string EstateName { get; set; }
        [DataMember(Name = "EngPhase")]
        public EngPhaseDTO EngPhase
        {
            get
            {
                if (_engPhase == null)
                {
                    return new EngPhaseDTO();
                }
                return _engPhase;
            }
            set { _engPhase = value;}
        }
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
        private EngFloorDTO _engFloor;
        private EngUnitDTO _engUnit;

        [DataMember(Name = "EngFloor")]
        public EngFloorDTO EngFloor
        {
            get
            {
                if (_engFloor == null)
                {
                    return new EngFloorDTO();
                }
                return _engFloor;
            }
            set { _engFloor = value;}
        }
        [DataMember(Name = "EngUnit")]
        public EngUnitDTO EngUnit
        {
            get
            {
                if (_engUnit == null)
                {
                    return new EngUnitDTO();
                }
                return _engUnit;
            }
            set { _engUnit = value;}
        }
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
        private EngStructureLotDTO _engStructureLot;

        [DataMember(Name = "EngStructureLot")]
        public EngStructureLotDTO EngStructureLot
        {
            get
            {
                if (_engStructureLot == null)
                {
                    return new EngStructureLotDTO();
                }
                return _engStructureLot;
            }
            set { _engStructureLot = value;}
        }
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
        private ChiUnitDTO _chiUnit;
        private ChiFloorDTO _chiFloor;

        [DataMember(Name = "ChiUnit")]
        public ChiUnitDTO ChiUnit
        {
            get
            {
                if (_chiUnit == null)
                {
                    return new ChiUnitDTO();
                }
                return _chiUnit;
            }
            set { _chiUnit = value;}
        }
        [DataMember(Name = "ChiFloor")]
        public ChiFloorDTO ChiFloor
        {
            get
            {
                if (_chiFloor == null)
                {
                    return new ChiFloorDTO();
                }
                return _chiFloor;
            }
            set { _chiFloor = value;}
        }
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
        private ChiPhaseDTO _chiPhase;

        [DataMember(Name = "EstateName")]
        public string EstateName { get; set; }
        [DataMember(Name = "ChiPhase")]
        public ChiPhaseDTO ChiPhase
        {
            get
            {
                if (_chiPhase == null)
                {
                    return new ChiPhaseDTO();
                }
                return _chiPhase;
            }
            set { _chiPhase = value;}
        }
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
        private ChiStructuredLotDTO _chiStructuredLot;

        [DataMember(Name = "ChiStructuredLot")]
        public ChiStructuredLotDTO ChiStructuredLot
        {
            get
            {
                if (_chiStructuredLot == null)
                {
                    return new ChiStructuredLotDTO();
                }
                return _chiStructuredLot;
            }
            set { _chiStructuredLot = value;}
        }
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
        private ChiStructuredOutsideDTO _chiStructuredOutside;

        [DataMember(Name = "ChiStructuredOutside")]
        public ChiStructuredOutsideDTO ChiStructuredOutside
        {
            get
            {
                if (_chiStructuredOutside == null)
                {
                    return new ChiStructuredOutsideDTO();
                }
                return _chiStructuredOutside;
            }
            set { _chiStructuredOutside = value;}
        }
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
    public class ReqEMeCallbackDTO
    {
        private HomeTelNumberDTO _homeTelNumber;
        private MobileNumberDTO _mobileNumber;
        private IdNoDTO _idNo;
        private OfficeTelNumberDTO _officeTelNumber;
        private PostalAddressDTO _postalAddress;
        private ResidentialAddressDTO _residentialAddress;
        private EnNameDTO _enName;
        private ChNameDTO _chName;

        [DataMember(Name = "passportNumber")]
        public string PassportNumber { get; set; }

        [DataMember(Name = "homeTelNumber")]
        public HomeTelNumberDTO HomeTelNumber
        {
            get
            {
                if (_homeTelNumber == null)
                {
                    return new HomeTelNumberDTO();
                }
                return _homeTelNumber;
            }
            set { _homeTelNumber = value;}
        }

        [DataMember(Name = "propertyOwner")]
        public string PropertyOwner { get; set; }

        [DataMember(Name = "gender")]
        public string Gender { get; set; }

        [DataMember(Name = "prefix")]
        public string Prefix { get; set; }

        [DataMember(Name = "mobileNumber")]
        public MobileNumberDTO MobileNumber
        {
            get
            {
                if (_mobileNumber == null)
                {
                    return new MobileNumberDTO();
                }
                return _mobileNumber;
            }
            set { _mobileNumber = value;}
        }

        [DataMember(Name = "businessID")]
        public string BusinessID { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "employmentStatus")]
        public string EmploymentStatus { get; set; }

        [DataMember(Name = "birthDate")]
        public string BirthDate { get; set; }

        [DataMember(Name = "idNo")]
        public IdNoDTO IdNo
        {
            get
            {
                if (_idNo == null)
                {
                    return new IdNoDTO();
                }
                return _idNo;
            }
            set { _idNo = value;}
        }

        [DataMember(Name = "passportExpiry")]
        public string PassportExpiry { get; set; }

        [DataMember(Name = "officeTelNumber")]
        public OfficeTelNumberDTO OfficeTelNumber
        {
            get
            {
                if (_officeTelNumber == null)
                {
                    return new OfficeTelNumberDTO();
                }
                return _officeTelNumber;
            }
            set { _officeTelNumber = value;}
        }

        [DataMember(Name = "emailAddress")]
        public string EmailAddress { get; set; }

        public string Type { get; set; }

        [DataMember(Name = "postalAddress")]
        public PostalAddressDTO PostalAddress
        {
            get
            {
                if (_postalAddress == null)
                {
                    return new PostalAddressDTO();
                }
                return _postalAddress;
            }
            set { _postalAddress = value;}
        }

        public string Language { get; set; }

        [DataMember(Name = "educationLevel")]
        public string EducationLevel { get; set; }

        [DataMember(Name = "carOwner")]
        public string CarOwner { get; set; }

        [DataMember(Name = "residentialAddress")]
        public ResidentialAddressDTO ResidentialAddress
        {
            get
            {
                if (_residentialAddress == null)
                {
                    return new ResidentialAddressDTO();
                }
                return _residentialAddress;
            }
            set { _residentialAddress = value;}
        }

        [DataMember(Name = "enName")]
        public EnNameDTO EnName
        {
            get
            {
                if (_enName == null)
                {
                    return new EnNameDTO();
                }
                return _enName;
            }
            set { _enName = value;}
        }

        [DataMember(Name = "chName")]
        public ChNameDTO ChName
        {
            get
            {
                if (_chName == null)
                {
                    return new ChNameDTO();
                }
                return _chName;
            }
            set { _chName = value; }
        }

        [DataMember(Name = "maritalStatus")]
        public string MaritalStatus { get; set; }

        [DataMember(Name = "chNameVerified")]
        public string ChNameVerified { get; set; }
    }
}
