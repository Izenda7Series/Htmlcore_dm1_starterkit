using ApiCoreStarterKit.Models;
using ApiCoreStarterKit.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Web.Http;

namespace ApiCoreStarterKit.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : Controller
    {
        #region variables
        private readonly IConfiguration _configuration;
        #endregion

        #region CTOR
        public UserController(IConfiguration configuration) => _configuration = configuration;
        #endregion

        #region Methods
        [EnableCors("AllowOrigin")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GenerateToken")]
        public JsonResult GenerateToken(string tenant, string email, string password)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Check if valid login from IzendaUser table
            var loginManager = new LoginManager(connectionString);
            var success = loginManager.ValidateLogin(email, password, tenant);

            // Login failed
            if (!success)
                return null;

            // Login success, encrypt and send token
            var user = new UserInfo 
            { 
                UserName = email, 
                TenantUniqueName = tenant, 
                Password = password 
            };

            var token = IzendaBoundary.IzendaTokenAuthorization.GetToken(user);

            return Json(new { token });
        } 
        #endregion
    }
}
