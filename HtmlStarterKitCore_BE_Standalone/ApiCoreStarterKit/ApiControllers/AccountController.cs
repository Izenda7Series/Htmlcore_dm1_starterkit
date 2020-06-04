using ApiCoreStarterKit.Models;
using Mvc5StarterKit.IzendaBoundary;
using System;
using System.Web.Http;

namespace ApiCoreStarterKit.ApiControllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        #region Methods
        [HttpGet]
        [Route("validateIzendaAuthToken")]
        public UserInfo ValidateIzendaAuthToken(string access_token)
        {
            var userInfo = IzendaTokenAuthorization.GetUserInfo(access_token);
            Console.WriteLine(userInfo);

            return userInfo;
        }

        [HttpGet]
        [Route("GetIzendaAccessToken")]
        public IHttpActionResult GetIzendaAccessToken(string message)
        {
            var userInfo = IzendaTokenAuthorization.DecryptIzendaAuthenticationMessage(message);
            var token = IzendaTokenAuthorization.GetToken(userInfo);

            return Ok(token);
        } 
        #endregion
    }
}
