using ApiCoreStarterKit.Models;
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
        #endregion

        #region CTOR
        public LoginManager(string connString) => _connectionString = connString;
        #endregion

        #region Methods
        public bool ValidateLogin(string username, string password, string tenant)
        {
            var users = GetPotentialUsers(username);

            // invalid user input
            if (!users.Any()) { return false; }

            // find specific user by tenant
            var currentUser = users.FirstOrDefault(u => u.TenantUniqueName == tenant);

            // no matching user + tenant found
            if (currentUser == null) { return false; }

            // check if password matches
            return password.Equals(IzendaBoundary.IzendaTokenAuthorization.GetPassword(currentUser.Password));
        }

        private IEnumerable<UserInfo> GetPotentialUsers(string username)
        {
            var users = new List<UserInfo>();
            var tenants = GetTenants();

            using (var connection = new SqlConnection(_connectionString))
            {
                var queryString = $"SELECT UserName, PasswordHash, TenantId FROM IzendaUser WHERE UserName = '{username}';";

                var command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                var reader = command.ExecuteReader();

                // Get potential list of users
                while (reader.Read())
                {
                    var tenantId = reader["TenantId"].ToString();
                    var user = new UserInfo
                    {
                        UserName = reader["UserName"].ToString(),
                        Password = reader["PasswordHash"].ToString()
                    };

                    if (string.IsNullOrEmpty(tenantId)) // is system level
                        user.TenantUniqueName = null;
                    else // is tenant level
                    {
                        var tenant = tenants.FirstOrDefault(t => t.Id == tenantId);
                        user.TenantUniqueName = tenant?.TenantId ?? string.Empty;
                    }

                    users.Add(user);
                }
                reader.Close();
            }

            return users;
        }

        private IEnumerable<TenantInfo> GetTenants()
        {
            List<TenantInfo> tenants = new List<TenantInfo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var queryString = $"SELECT Id, TenantID, Name FROM IzendaTenant;";

                var command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tenants.Add(new TenantInfo
                    {
                        Id = reader["Id"].ToString(),
                        TenantId = reader["TenantID"].ToString(),
                        Name = reader["Name"].ToString()
                    });
                }
                reader.Close();
            }

            return tenants;
        } 
        #endregion
    }
}
