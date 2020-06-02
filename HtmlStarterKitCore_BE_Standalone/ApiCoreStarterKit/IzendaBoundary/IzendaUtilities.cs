using ApiCoreStarterKit.IzendaBoundary.Models;
using ApiCoreStarterKit.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCoreStarterKit.IzendaBoundary
{
    public static class IzendaUtilities
    {
        #region Methods
        public static Tenant GetTenantByName(string name, string connectionString)
        {
            return GetAllTenants(connectionString).FirstOrDefault(t => t.Name == name);
        }

        public static List<Tenant> GetAllTenants(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var _tenantNameList = new List<Tenant>();
                var queryString = $"SELECT * FROM Tenants;";

                var command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var tenant = new Tenant(reader["Id"]?.ToString() ?? string.Empty, reader["Name"].ToString() ?? string.Empty);

                    _tenantNameList.Add(tenant);
                }
                reader.Close();

                return _tenantNameList;
            }
        }

        // SAVE tenant into client DB
        public static async Task SaveTenantAsync(Tenant tenant, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var queryString = $"INSERT INTO Tenants (Name) VALUES(@param1)";

                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.Add("@param1", SqlDbType.NVarChar, 1000).Value = tenant.Name;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Create a tenant
        /// For more information, please refer to https://www.izenda.com/docs/ref/api_tenant.html#tenant-apis
        /// </summary>
        public static async Task<bool> CreateTenant(string tenantName, string tenantId, string authToken)
        {
            var existingTenant = await GetIzendaTenantByName(tenantName, authToken);
            if (existingTenant != null)
                return false;

            var tenantDetail = new TenantDetail
            {
                Active = true,
                Disable = false,
                Name = tenantName,
                TenantId = tenantId
            };

            // For more information, please refer to https://www.izenda.com/docs/ref/api_tenant.html#post-tenant
            return await WebAPIService.Instance.PostReturnBooleanAsync("tenant", tenantDetail, authToken);
        }

        public static async Task<RoleDetail> CreateRole(string roleName, TenantDetail izendaTenant, string authToken)
        {
            var role = await GetIzendaRoleByTenantAndName(izendaTenant != null ? (Guid?)izendaTenant.Id : null, roleName, authToken);

            if (role == null)
            {
                role = new RoleDetail
                {
                    Active = true,
                    Deleted = false,
                    NotAllowSharing = false,
                    Name = roleName,
                    TenantId = izendaTenant != null ? (Guid?)izendaTenant.Id : null
                };

                var response = await WebAPIService.Instance.PostReturnValueAsync<AddRoleResponeMessage, RoleDetail>("role", role, authToken);
                role = response.Role;
            }

            return role;
        }

        /// <summary>
        /// Create a user
        /// For more information, please refer to https://www.izenda.com/docs/ref/api_user.html#post-user
        /// ATTN: please don't use this deprecated end point https://www.izenda.com/docs/ref/api_user.html#post-user-integration-saveuser
        /// </summary>
        public static async Task<bool> CreateIzendaUser(string tenant, string userID, string lastName, string firstName, bool isAdmin, string roleName, string authToken)
        {
            var izendaTenant = !string.IsNullOrEmpty(tenant) ? await GetIzendaTenantByName(tenant, authToken) : null;

            var izendaUser = new UserDetail
            {
                Username = userID,
                TenantId = izendaTenant != null ? (Guid?)izendaTenant.Id : null,
                LastName = lastName,
                FirstName = firstName,
                TenantDisplayId = izendaTenant != null ? izendaTenant.Name : string.Empty,
                InitPassword = false,
                SystemAdmin = isAdmin,
                Active = true
            };

            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var izendaRole = await CreateRole(roleName, izendaTenant, authToken);
                izendaUser.Roles.Add(izendaRole);
            }

            bool success = await WebAPIService.Instance.PostReturnBooleanAsync("user", izendaUser, authToken);

            return success;
        }

        private static async Task<TenantDetail> GetIzendaTenantByName(string tenantName, string authToken)
        {
            var tenants = await WebAPIService.Instance.GetAsync<IList<TenantDetail>>("/tenant/allTenants", authToken);
            if (tenants != null)
                return tenants.FirstOrDefault(x => x.Name.Equals(tenantName, StringComparison.InvariantCultureIgnoreCase));

            return null;
        }

        private static async Task<RoleDetail> GetIzendaRoleByTenantAndName(Guid? tenantId, string roleName, string authToken)
        {
            var roles = await WebAPIService.Instance.GetAsync<IList<RoleDetail>>("/role/all/" + (tenantId.HasValue ? tenantId.ToString() : null), authToken);

            if (roles != null)
                return roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));

            return null;
        }
        #endregion
    }
}
