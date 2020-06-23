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

// If you use .Net Framework, please replace with following codes.
// Dot Net Core is not supposed to use a controller inherited from ApiController (System.Web.Http) but ControllerBase (Microsoft.AspNetCore.Mvc).
// HttpGet, Route follow the same rule.
/*
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
}*/
