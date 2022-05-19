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
    public interface ISignService
    {
        /// <summary>
        /// 请求Hash签名接口
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResponseDTO<ResHashSignDTO> RequestHashSign(ReqHashSignDTO dto);

        /// <summary>
        /// 轮询获取Hash签名结果接口
        /// </summary>
        /// <param name="businessID"></param>
        /// <param name="signingResult"></param>
        /// <returns></returns>
        ResponseDTO<string> GetHashAckResult(string businessID, string signingResult);

        /// <summary>
        /// 请求Pdf签名接口
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResponseDTO<ResPdfSignDTO> RequestPdfSign(ReqPdfSignDTO dto);

        /// <summary>
        /// 轮询获取Pdf签名结果接口
        /// </summary>
        /// <param name="businessID"></param>
        /// <param name="signingResult"></param>
        /// <returns></returns>
        ResponseDTO<string> GetPdfAckResult(string businessID, string signingResult);

        /// <summary>
        /// 请求匿名Hash签名接口
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResponseDTO<ResAnonymousHashSignDTO> RequestAnonymousHashSign(ReqAnonymousHashSignDTO dto);

        /// <summary>
        /// 获取匿名Hash签名结果接口
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        ResponseDTO<ResAnonymousHashSignDetailDTO> GetAnonymousHashAckResult(string accessToken, string openID);

        /// <summary>
        /// 请求匿名Pdf签名接口
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResponseDTO<ResAnonymousPdfSignDTO> RequestAnonymousPdfSign(ReqAnonymousPdfSignDTO dto);

        /// <summary>
        /// 获取匿名Pdf签名结果接口
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        ResponseDTO<ResAnonymousPdfSignDetailDTO> GetAnonymousPdfAckResult(string accessToken, string openID);
    }
}
