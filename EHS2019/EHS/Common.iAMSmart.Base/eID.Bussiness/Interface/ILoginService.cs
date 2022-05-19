using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eService.DTO.Response;

namespace eID.Bussiness.Interface
{
    public interface ILoginService
    {
        /// <summary>
        /// 獲取AccessToken接口
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        ResponseDTO<AccessTokenDTO> GetAccessToken(string code, string state, Common.ComObject.AuditLogEntry auditlog);
    }
}
