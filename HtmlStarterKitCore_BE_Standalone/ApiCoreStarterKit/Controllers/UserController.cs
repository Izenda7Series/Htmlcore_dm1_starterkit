using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using ApiCoreStarterKit.Models;
using System.Security.Claims;
using System.Web.Http.Results;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ApiCoreStarterKit.Controllers
{   
    [System.Web.Http.RoutePrefix("api/user")]
    public class UserController : Controller
    {
        [EnableCors("AllowOrigin")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GenerateToken")]

        public JsonResult GenerateToken()
        {
            string userName = "IzendaAdmin@system.com";
            string tenant = null;
            var user = new UserInfo { UserName = userName, TenantUniqueName = tenant };
            string token = IzendaBoundary.IzendaTokenAuthorization.GetToken(user);
            return Json(new { token });
        }
    }
}