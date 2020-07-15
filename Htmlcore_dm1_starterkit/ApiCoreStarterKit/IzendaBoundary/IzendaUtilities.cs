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
                    var tenant = new Tenant(int.Parse(reader["Id"]?.ToString() ?? string.Empty), reader["Name"].ToString() ?? string.Empty);

                    _tenantNameList.Add(tenant);
                }
                reader.Close();

                return _tenantNameList;
            }
        }

        public static IEnumerable<UserInfo> GetUserList(string username, string connectionString)
        {
            var users = new List<UserInfo>();

            using (var connection = new SqlConnection(connectionString))
            {
                var queryString = $"SELECT UserName, PasswordHash, Tenant_Id FROM ApplicationUsers WHERE UserName = '{username}';";

                var command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                var reader = command.ExecuteReader();

                // Get potential list of users
                while (reader.Read())
                {
                    int? tenantId = null;

                    var id = reader["Tenant_Id"].ToString();
                    if (!string.IsNullOrEmpty(id))
                    {
                        tenantId = int.Parse(id);
                    }

                    var user = new UserInfo
                    {
                        UserName = reader["UserName"].ToString(),
                        Password = reader["PasswordHash"].ToString()
                    };

                    if (tenantId == null) // if tenantId is null, it is system level
                        user.TenantUniqueName = null;
                    else // otherwise tenant level
                        user.TenantUniqueName = GetAllTenants(connectionString).FirstOrDefault(t => t.Id == tenantId)?.Name ?? null;

                    users.Add(user);
                }
                reader.Close();
            }

            return users;
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

        // SAVE user into cliend DB
        public static async Task SaveUserAsync(string userName, string email, int? tenant_Id, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var queryString = tenant_Id != null ? $"INSERT INTO ApplicationUsers (id, UserName, Email, Tenant_Id, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)" +
                    $"VALUES(@param0, @param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8)"
                        :$"INSERT INTO ApplicationUsers (id, UserName, Email, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)" +
                        $"VALUES(@param0, @param1, @param2, @param4, @param5, @param6, @param7, @param8)";

                using (SqlCommand cmd = new SqlCommand(queryString, connection))
                {
                    cmd.Parameters.Add("@param0", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                    cmd.Parameters.Add("@param1", SqlDbType.NVarChar, 256).Value = userName;
                    cmd.Parameters.Add("@param2", SqlDbType.NVarChar, 256).Value = email;

                    if (tenant_Id != null)
                        cmd.Parameters.Add("@param3", SqlDbType.Int, int.MaxValue).Value = tenant_Id;

                    cmd.Parameters.Add("@param4", SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param5", SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param6", SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@param7", SqlDbType.Bit).Value = true;
                    cmd.Parameters.Add("@param8", SqlDbType.Bit).Value = false;

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
        /// For more information, please refer to https://www.izenda.com/docs/ref/api_user.html#post-external-user
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
                SystemAdmin = isAdmin
            };

            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var izendaRole = await CreateRole(roleName, izendaTenant, authToken);
                izendaUser.Roles.Add(izendaRole);
            }

            bool success = await WebAPIService.Instance.PostReturnBooleanAsync("external/user", izendaUser, authToken);

            return success;
        }

        public static async Task<TenantDetail> GetIzendaTenantByName(string tenantName, string authToken)
        {
            var tenants = await WebAPIService.Instance.GetAsync<IList<TenantDetail>>("/tenant/allTenants", authToken);
            if (tenants != null)
                return tenants.FirstOrDefault(x => x.TenantId.Equals(tenantName, StringComparison.InvariantCultureIgnoreCase));

            return null;
        }

        /// <summary>
        /// Get a matched role from the list of Izenda Roles under the selected tenant
        /// </summary>
        private static async Task<RoleDetail> GetIzendaRoleByTenantAndName(Guid? tenantId, string roleName, string authToken)
        {
            var roles = await GetAllIzendaRoleByTenant(tenantId, authToken);

            if (roles.Any())
                return roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));

            return null;
        }

        /// <summary>
        /// Get all Izenda Roles by tenant
        /// For more information, please refer to https://www.izenda.com/docs/ref/api_role.html#get-role-all-tenant-id
        /// </summary>
        public static async Task<IList<RoleDetail>> GetAllIzendaRoleByTenant(Guid? tenantId, string authToken)
        {
            var roleList = await WebAPIService.Instance.GetAsync<IList<RoleDetail>>("/role/all/" + (tenantId.HasValue ? tenantId.ToString() : null), authToken);

            return roleList;
        }
        #endregion
    }
}
