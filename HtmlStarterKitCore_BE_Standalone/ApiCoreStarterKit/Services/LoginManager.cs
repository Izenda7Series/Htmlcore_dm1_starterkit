using ApiCoreStarterKit.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ApiCoreStarterKit.Services
{
    /// <summary>
    /// This class is currently verifying the user's password against the IzendaUser tabke of the configuration database for example purposes
    /// </summary>
    public class LoginManager
    {
        #region Variables
        private readonly string _connectionString;

        private List<Tenant> _tenantNameList = new List<Tenant>();
        #endregion

        #region CTOR
        public LoginManager(string connectionString)
        {
            _connectionString = connectionString;

            // It is a better idea to create a list of tenant from tenants table 
            // This list is immutable and does not have to be created everytime for looking up the tenant name
            GetTenantNameList(); 
        }
        #endregion

        #region Methods
        private void GetTenantNameList()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var queryString = $"SELECT Id, Name FROM Tenants;";

                var command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var tenant = new Tenant(reader["Id"]?.ToString() ?? string.Empty , reader["Name"].ToString() ?? string.Empty);
                   
                    _tenantNameList.Add(tenant);
                }
                reader.Close();
            }
        }

        public bool ValidateLogin(string username, string password, string tenant)
        {
            var users = GetUserList(username);

            // invalid user input
            if (!users.Any())
                return false; 

            // find specific user by tenant
            var currentUser = users.FirstOrDefault(u => u.TenantUniqueName == tenant);

            // no matching user + tenant found
            if (currentUser == null)
                return false;

            // check if password matches
            return password?.Equals(IzendaBoundary.IzendaTokenAuthorization.GetPassword(currentUser.Password)) ?? false;
        }

        private IEnumerable<UserInfo> GetUserList(string username)
        {
            var users = new List<UserInfo>();
            var tenantInfoList = GetTenantInfoList();

            using (var connection = new SqlConnection(_connectionString))
            {
                var queryString = $"SELECT UserName, PasswordHash, Tenant_Id FROM ApplicationUsers WHERE UserName = '{username}';";

                var command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                var reader = command.ExecuteReader();

                // Get potential list of users
                while (reader.Read())
                {
                    var tenantId = reader["Tenant_Id"].ToString();
                    var user = new UserInfo
                    {
                        UserName = reader["UserName"].ToString(),
                        Password = reader["PasswordHash"].ToString()
                    };

                    if (string.IsNullOrEmpty(tenantId)) // if tenantId is null, it is system level
                        user.TenantUniqueName = null;
                    else // otherwise tenant level
                        user.TenantUniqueName = _tenantNameList.FirstOrDefault(t => t.Id == tenantId)?.Name ?? null;

                    users.Add(user);
                }
                reader.Close();
            }

            return users;
        }

        private IEnumerable<TenantInfo> GetTenantInfoList()
        {
            var tenants = new List<TenantInfo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var queryString = $"SELECT Id, Tenant_Id, UserName FROM ApplicationUsers;";

                var command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tenants.Add(new TenantInfo
                    {
                        Id = reader["Id"].ToString(),
                        TenantId = reader["Tenant_Id"].ToString(),
                        Name = reader["UserName"].ToString()
                    });
                }
                reader.Close();
            }

            return tenants;
        }
        #endregion
    }
}
