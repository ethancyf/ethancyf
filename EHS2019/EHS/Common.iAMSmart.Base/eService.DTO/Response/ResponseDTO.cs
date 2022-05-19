using eService.DTO.Enum;
using System;
using System.Runtime.Serialization;

namespace eService.DTO.Response
{
    [DataContract]
    public class ResponseDTO<T>
    {
        [DataMember(Name = "txID")]
        public string TxID { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "content")]
        public T Content { get; set; }

        public ResponseDTO(string txID)
        {
            TxID = txID;
            Code = "D00000";
            Message = ReturnCode.SUCCESS.GetDescription();
        }

        public ResponseDTO(string txID, T content)
        {
            TxID = txID;
            Code = "D00000";
            Message = ReturnCode.SUCCESS.GetDescription();
            Content = content;
        }

        public ResponseDTO(string txID, string code, string message, T content)
        {
            TxID = txID;
            Code = code == null ? "" : code.Contains("D") ? code : "D" + code;
            Message = message;
            Content = content;
        }

        public ResponseDTO()
        {

        }
    }
}
