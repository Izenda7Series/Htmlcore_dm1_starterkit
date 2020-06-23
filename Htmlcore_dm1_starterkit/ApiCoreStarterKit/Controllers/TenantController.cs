using ApiCoreStarterKit.IzendaBoundary;
using ApiCoreStarterKit.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Mvc5StarterKit.IzendaBoundary;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCoreStarterKit.Controllers
{
    [Route("api/tenant")]
    public class TenantController : BaseController
    {
        #region Variables
        private readonly IConfiguration _configuration;
        #endregion

        #region CTOR
        public TenantController(IConfiguration configuration) => _configuration = configuration;
        #endregion

        #region Methods
        [EnableCors("AllowOrigin")]
        [HttpGet]
        [Route("CreateTenant")]
        public async Task<JsonResult> CreateTenant(string tenantID, string tenantName)
        {
            if (string.IsNullOrEmpty(tenantID) || string.IsNullOrEmpty(tenantName))
                return AddJsonResult(false);
            else
            {
                var izendaAdminAuthToken = IzendaTokenAuthorization.GetIzendaAdminToken();
                var connectString = _configuration.GetConnectionString("DefaultConnection");

                var isTenantExist = IzendaUtilities.GetTenantByName(tenantID, connectString); // check user DB first

                if (isTenantExist == null)
                {
                    var success = await IzendaUtilities.CreateTenant(tenantName, tenantID, izendaAdminAuthToken);

                    if (success)
                    {
                        // save a new tenant at user DB
                        var newTenant = new Tenant() { Name = tenantID };
                        await IzendaUtilities.SaveTenantAsync(newTenant, connectString);

                        return AddJsonResult(true);
                    }
                    else
                        return AddJsonResult(false);
                }
                else
                    return AddJsonResult(false);
            }
        }

        [EnableCors("AllowOrigin")]
        [HttpGet]
        [Route("GetTenantList")]
        public string GetTenantList()
        {
            // todo
            var connectString = _configuration.GetConnectionString("DefaultConnection");
            var tenantList = IzendaUtilities.GetAllTenants(connectString);

            var list = new List<string>();
            list.Add("Select Tenant");

            foreach (var tenant in tenantList)
            {
                list.Add(tenant.Name);
            }

            var result = JsonConvert.SerializeObject(list);

            return result;
        }
        #endregion
    }
}
