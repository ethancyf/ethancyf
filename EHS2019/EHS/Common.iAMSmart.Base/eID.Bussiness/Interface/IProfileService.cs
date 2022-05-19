using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eService.DTO;
using eService.DTO.Request;
using eService.DTO.Response;
using Newtonsoft.Json.Linq;

namespace eID.Bussiness.Interface
{
    public interface IProfileService
    {
        /// <summary>
        /// eMe-profile接口
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="source"></param>
        /// <param name="state"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        ResponseDTO<ResProfileDTO> RequestProfile(AccessTokenDTO dto, string source, string state, string lang, string businessID, List<string> profilelist, Common.ComObject.AuditLogEntry auditlog);

    }
}
