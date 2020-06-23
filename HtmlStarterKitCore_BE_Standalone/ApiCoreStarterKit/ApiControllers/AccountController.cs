using ApiCoreStarterKit.Models;
using Mvc5StarterKit.IzendaBoundary;
using System;
using Microsoft.AspNetCore.Mvc;

namespace ApiCoreStarterKit.ApiControllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
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
        public ActionResult<string> GetIzendaAccessToken(string message)
        {
            var userInfo = IzendaTokenAuthorization.DecryptIzendaAuthenticationMessage(message);
            var token = IzendaTokenAuthorization.GetToken(userInfo);

            return Ok(new { Token = token });
        }
        #endregion
    }
}
