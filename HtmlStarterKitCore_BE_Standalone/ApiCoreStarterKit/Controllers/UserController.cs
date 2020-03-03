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
using ApiCoreStarterKit.Services;
using Microsoft.Extensions.Configuration;

namespace ApiCoreStarterKit.Controllers
{   
    [System.Web.Http.RoutePrefix("api/user")]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [EnableCors("AllowOrigin")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GenerateToken")]

        public JsonResult GenerateToken(string tenant, string email, string password)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Check if valid login from IzendaUser table
            LoginManager loginManger = new LoginManager(connectionString);
            var status = loginManger.ValidateLogin(email, password);

            // Login failed
            if (!status)
            {
                return null;
            }

            // Login success, encrypt and send token
            var user = new UserInfo { UserName = email, TenantUniqueName = tenant, Password = password };
            string token = IzendaBoundary.IzendaTokenAuthorization.GetToken(user);
            return Json(new { token });
        }
    }
}