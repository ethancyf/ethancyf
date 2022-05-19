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
    public interface IEMeInfoService
    {
        /// <summary>
        /// 请求EME信息接口
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="source"></param>
        /// <param name="eMeFields"></param>
        /// <param name="state"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        ResponseDTO<ResEmeInfoDTO> RequestEMeInfo(AccessTokenDTO dto, string source, string[] eMeFields, string state, string lang);

        /// <summary>
        /// 请求匿名EME信息接口
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eMeFields"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        ResponseDTO<ResAnonymousEmeInfoDTO> RequestAnonymousEMeInfo(string source, string[] eMeFields, string lang);

        /// <summary>
        /// 获取匿名EME信息接口
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        ResponseDTO<ResEMeDetailDTO> GetAnonymousEMeDetail(string accessToken, string openID);
    }
}
