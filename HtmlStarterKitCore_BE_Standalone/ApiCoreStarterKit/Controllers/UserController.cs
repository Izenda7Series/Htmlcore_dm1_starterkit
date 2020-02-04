using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiCoreStarterKit.Models;
using System.Security.Claims;
using System.Web.Http.Results;
//using Microsoft.AspNetCore.Mvc;

namespace ApiCoreStarterKit.Controllers
{   
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("GenerateToken")]
        public IHttpActionResult GenerateToken()
        {
            var identity = ClaimsPrincipal.Current;
            var username = identity.FindFirst(ClaimsIdentity.DefaultNameClaimType).ToString();
            var tenantName = identity.FindFirst("tenant").ToString();
            var user = new Models.UserInfo { UserName = username, TenantUniqueName = tenantName };
            var token = IzendaBoundary.IzendaTokenAuthorization.GetToken(user);

            return Ok(new {token});

            //var token = "i7di+WoXTvjk47YhJGhictiBOqsUGIkbgd5B8XizEJ56DC4Ark8TO9YWUs50BH+HFnukB2H1pFZfza4psZCDOA==";
            //return Ok(new { token });
        }

        //[System.Web.Http.HttpGet]
        //[Authorize]
        //public JsonResult GenerateToken()
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    var username = identity.FindFirst(ClaimsIdentity.DefaultNameClaimType);
        //    var tenantName = identity.FindFirst("tenantName");

        //    var user = new Models.UserInfo { UserName = username?.Value, TenantUniqueName = tenantName?.Value };
        //    var token = IzendaBoundary.IzendaTokenAuthorization.GetToken(user);
        //    return Json(new { token });
        //}

        //[HttpGet]
        //[Route("GenerateToken")]
        //public IHttpActionResult GenerateToken()
        //{
        //    var username = User.Identity.GetUserName();
        //    var tenantName = ((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirstValue("tenant");
        //    var user = new Models.UserInfo { UserName = username, TenantUniqueName = tenantName };
        //    var token = IzendaBoundary.IzendaTokenAuthorization.GetToken(user);

        //    return token;

        //    //var token = "i7di+WoXTvjk47YhJGhictiBOqsUGIkbgd5B8XizEJ56DC4Ark8TO9YWUs50BH+HFnukB2H1pFZfza4psZCDOA==";
        //    //return Ok(new { token });
        //}
    }
}