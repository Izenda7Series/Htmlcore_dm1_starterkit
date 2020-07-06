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

namespace ApiCoreStarterKit.Controllers
{
    [Route("api/user")]
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
        [HttpGet]
        [Route("GenerateToken")]
        public JsonResult GenerateToken(string tenant, string email, string password)
        {
            bool useADlogin;
            var adLoginSetting = _configuration.GetValue<string>("useADlogin:useADlogin"); // if you want to enable active directory login, then set this boolean value to true (appsettings.json). Default is false.
            bool.TryParse(adLoginSetting, out useADlogin);

            var defaultConnectionString = _configuration.GetConnectionString("DefaultConnection");
            var success = false;

            var loginManager = new LoginManager(defaultConnectionString);

            if (!string.IsNullOrEmpty(tenant) && useADlogin) // if tenant is null, then assume that it is system level login. Go to the ValidateLogin which is used for regular login process first
            {
                // If we allow AD authentication, then email / password field are not required because it can be retrieved from active directory information.
                // You can remove those fields from front-end UI. 
                // However, tenant field is required because it is used for GetToken.
                success = loginManager.ValidateActiveDirectoryLogin(tenant);
            }
            else
                success = loginManager.ValidateLogin(email, password, tenant);

            // Login failed
            if (!success)
                return null;

            // Login success, create UserInfo to GetToken
            var user = new UserInfo
            {
                UserName = email,
                TenantUniqueName = tenant,
                Password = password
            };

            // get token from constructed user information
            var token = IzendaTokenAuthorization.GetToken(user);

            return Json(new { token });
        }

        [EnableCors("AllowOrigin")]
        [HttpGet]
        [Route("CreateUser")]
        public async Task<JsonResult> CreateUser(bool isAdmin, string selectedRole, string selectedTenant, string userId, string firstName, string lastName)
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

            var assignedRole = !string.IsNullOrEmpty(selectedRole) ? selectedRole : "Employee"; // set default role if required

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
        [HttpGet]
        [Route("GetRoleList")]
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

        [EnableCors("AllowOrigin")]
        [HttpGet]
        [Route("GetCurrentUserInfo")]
        public string GetCurrentUserInfo(string token)
        {
            var userInfo = IzendaTokenAuthorization.GetUserInfo(token);
            var result = JsonConvert.SerializeObject(userInfo.UserName);

            return result;
        }
        #endregion
    }
}
