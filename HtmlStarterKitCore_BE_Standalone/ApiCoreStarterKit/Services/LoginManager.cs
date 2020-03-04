using ApiCoreStarterKit.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ApiCoreStarterKit.Services
{
    //This class is currently verifying the user's password against the IzendaUser tabke of the configuration database for example purposes
    public class LoginManager
    {
        private string connectionString;

        public LoginManager(string connString)
        {
            connectionString = connString;
        }

        public bool ValidateLogin(string username, string password, string tenant)
        {
            var users = GetPotentialUsers(username);

            // invalid user input
            if (users.Count == 0) { return false; }

            // find specific user by tenant
            var currentUser = users.FirstOrDefault(u => u.TenantUniqueName == tenant);

            // no matching user + tenant found
            if (currentUser == null) { return false; }

            // check if password matches
            var userPass = IzendaBoundary.IzendaTokenAuthorization.GetPassword(currentUser.Password);
            if (password.Equals(userPass))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<UserInfo> GetPotentialUsers(string username)
        {
            List<UserInfo> users = new List<UserInfo>();
            var tenants = GetTenants();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryString = $"SELECT UserName, PasswordHash, TenantId FROM IzendaUser WHERE UserName = '{username}';";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Get potential list of users
                while (reader.Read())
                {
                    var user = new UserInfo();
                    user.UserName = reader["UserName"].ToString();
                    user.Password = reader["PasswordHash"].ToString();

                    var tenantId = reader["TenantId"].ToString();
                    if(tenantId == "") // is system level
                    {
                        user.TenantUniqueName = null;
                    }
                    else // is tenant level
                    {
                        var tenant = tenants.FirstOrDefault(t => t.Id == tenantId);
                        user.TenantUniqueName = tenant?.TenantId;
                    }

                    users.Add(user);
                }
                reader.Close();
            }

            return users;
        }

        private List<TenantInfo> GetTenants()
        {
            List<TenantInfo> tenants = new List<TenantInfo>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryString = $"SELECT Id, TenantID, Name FROM IzendaTenant;";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

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
    }
}
