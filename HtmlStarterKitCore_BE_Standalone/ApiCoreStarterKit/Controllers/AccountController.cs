using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiCoreStarterKit.Models;

namespace ApiCoreStarterKit.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        //The below method validates whether the access_token is valid or not
        [HttpGet]
        [Route("validateIzendaAuthToken")]
        public UserInfo ValidateIzendaAuthToken(string access_token)
        {
            var userInfo = IzendaBoundary.IzendaTokenAuthorization.GetUserInfo(access_token);
            Console.WriteLine(userInfo);

            return userInfo;
        }

        //The below method grabs the current user's token
        [HttpGet]
        [Route("GetIzendaAccessToken")]
        public IHttpActionResult GetIzendaAccessToken(string message)
        {
            var userInfo = IzendaBoundary.IzendaTokenAuthorization.DecryptIzendaAuthenticationMessage(message);
            var token = IzendaBoundary.IzendaTokenAuthorization.GetToken(userInfo);

            return Ok(token);
        }
    }
}
