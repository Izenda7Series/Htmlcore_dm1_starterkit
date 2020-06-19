using ApiCoreStarterKit.IzendaBoundary;
using ApiCoreStarterKit.Models;
using ApiCoreStarterKit.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mvc5StarterKit.IzendaBoundary;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiCoreStarterKit.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : BaseController
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
            var defaultConnectionString = _configuration.GetConnectionString("DefaultConnection");

            // Check if valid login from IzendaUser table
            var loginManager = new LoginManager(defaultConnectionString);
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

            var token = IzendaTokenAuthorization.GetToken(user);

            return Json(new { token });
        }

        [EnableCors("AllowOrigin")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("CreateUser")]
        public async Task<JsonResult> CreateUser(bool isAdmin, string selectedTenant, string userId, string firstName, string lastName)
        {
            var connectString = _configuration.GetConnectionString("DefaultConnection");
            var izendaAdminAuthToken = IzendaTokenAuthorization.GetIzendaAdminToken();
            int? tenantId = null;

            if (selectedTenant != "Select Tenant") // tenant level
            {
                tenantId = IzendaUtilities.GetTenantByName(selectedTenant, connectString)?.Id;
                isAdmin = false;

                if (tenantId == null)
                    return AddJsonResult(false);
            }

            var users = IzendaUtilities.GetUserList(userId, connectString);

            // invalid user input
            if (users.Any())
                return AddJsonResult(false);

            // save user into client DB
            await IzendaUtilities.SaveUserAsync(userId, userId, tenantId, connectString);

            var assignedRole = "Employee"; // set default role if required

            var success = await IzendaUtilities.CreateIzendaUser(
                selectedTenant,
                userId,
                lastName,
                firstName,
                isAdmin,
                assignedRole, izendaAdminAuthToken);

            if (success)
                return AddJsonResult(true);
            else
                return AddJsonResult(false);
        }


        [EnableCors("AllowOrigin")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetRoleList")]
        public async Task<string> GetRoleList(string selectedTenant)
        {
            var selectList = new List<string>();
            var adminToken = IzendaTokenAuthorization.GetIzendaAdminToken();

            var izendaTenant = await IzendaUtilities.GetIzendaTenantByName(selectedTenant, adminToken);
            var roleDetailsByTenant = await IzendaUtilities.GetAllIzendaRoleByTenant(izendaTenant?.Id ?? null, adminToken);

            foreach (var roleDetail in roleDetailsByTenant)
            {
                selectList.Add(roleDetail.Name);
            }
            var result = JsonConvert.SerializeObject(selectList);

            return result;
        }
        #endregion
    }
}
