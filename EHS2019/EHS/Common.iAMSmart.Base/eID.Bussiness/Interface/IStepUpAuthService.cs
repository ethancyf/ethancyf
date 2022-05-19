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
    public interface IStepUpAuthService
    {
        /// <summary>
        /// 请求增强认证接口
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="source"></param>
        /// <param name="state"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        ResponseDTO<ResStepUpAuthDTO> RequestStepUpAuth(AccessTokenDTO dto, string source, string state, string lang);

    }
}
