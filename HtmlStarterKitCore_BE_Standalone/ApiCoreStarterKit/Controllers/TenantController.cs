using ApiCoreStarterKit.IzendaBoundary;
using ApiCoreStarterKit.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mvc5StarterKit.IzendaBoundary;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiCoreStarterKit.ApiControllers
{
    [RoutePrefix("api/tenant")]
    public class TenantController : Controller
    {
        #region Variables
        private readonly IConfiguration _configuration;
        #endregion

        #region CTOR
        public TenantController(IConfiguration configuration) => _configuration = configuration;
        #endregion

        #region Methods
        [EnableCors("AllowOrigin")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("CreateTenant")]
        public async Task<JsonResult> CreateTenant(string tenantID, string tenantName)
        {
            if (string.IsNullOrEmpty(tenantID) || string.IsNullOrEmpty(tenantName))
                return CreateEntityResult(false);
            else
            {
                var izendaAdminAuthToken = IzendaTokenAuthorization.GetIzendaAdminToken();
                var connectString = _configuration.GetConnectionString("DefaultConnection");

                var isTenantExist = IzendaUtilities.GetTenantByName(tenantName, connectString); // check user DB first

                if (isTenantExist == null)
                {
                    //TODO: Create tenant now

                    var success = await IzendaUtilities.CreateTenant(tenantName, tenantID, izendaAdminAuthToken);

                    if (success)
                    {
                        // save a new tenant at user DB
                        var newTenant = new Tenant() { Name = tenantName };
                        await IzendaUtilities.SaveTenantAsync(newTenant, connectString);

                        return CreateEntityResult(true);
                    }
                    else
                        return CreateEntityResult(false);
                }
                else
                    return CreateEntityResult(false);
            }
        }

        private JsonResult CreateEntityResult(bool success)
        {
            var resultMessage = success ? "You have created a tenant successfully" : "You have failed to create a tenant";

            return Json(new { resultMessage });
        } 
        #endregion
    }
}
