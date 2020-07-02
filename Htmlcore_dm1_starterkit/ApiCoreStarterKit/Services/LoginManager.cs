using ApiCoreStarterKit.Models;
using Mvc5StarterKit.IzendaBoundary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace ApiCoreStarterKit.Services
{
    /// <summary>
    /// This class is currently verifying the user's password against the IzendaUser tabke of the configuration database for example purposes
    /// </summary>
    public class LoginManager
    {
        #region Variables
        private readonly string _defaultConnectionString;

        // we don't have to expose this. Encapsulate
        private readonly List<Tenant> _tenantNameList = new List<Tenant>();
        #endregion

        #region CTOR
        public LoginManager(string defaultConnectionString)
        {
            _defaultConnectionString = defaultConnectionString;

            // It is a better idea to create a list of tenant from tenants table 
            // This list is immutable and does not have to be created everytime for looking up the tenant name
            GetTenantNameList();
        }
        #endregion

        #region Methods
        private void GetTenantNameList()
        {
            using (var connection = new SqlConnection(_defaultConnectionString))
            {
                var queryString = $"SELECT Id, Name FROM Tenants;";

                var command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var tenant = new Tenant(int.Parse(reader["Id"]?.ToString() ?? string.Empty), reader["Name"].ToString() ?? string.Empty);

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
            return password?.Equals(IzendaTokenAuthorization.GetPassword(currentUser.Password)) ?? false;
        }

        /// <summary>
        /// Login with Active Directory information.
        /// Please refer to the following link to get more information on Active Directory 
        /// https://docs.microsoft.com/en-us/windows-server/identity/ad-ds/get-started/virtual-dc/active-directory-domain-services-overview
        /// </summary>
        public bool ValidateActiveDirectoryLogin(string tenant)
        {
            var userName = Environment.UserName; 
            var userDomainName = Environment.UserDomainName;
            var authenticationType = ContextType.Domain;

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userDomainName))
            {
                using (var context = new PrincipalContext(authenticationType, Environment.UserDomainName))
                {
                    var userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName);

                    if (userPrincipal != null)
                    {
                        var email = userPrincipal.EmailAddress;
                        var users = GetUserList(email); // get user list from DB

                        // if matches with tenant information, then authentication is successfull.
                        return users != null ? users.FirstOrDefault(u => u.TenantUniqueName == tenant) != null : false;
                    }
                }
            }
               
            return false;
        }

        private IEnumerable<UserInfo> GetUserList(string username)
        {
            var users = new List<UserInfo>();

            using (var connection = new SqlConnection(_defaultConnectionString))
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
                        user.TenantUniqueName = _tenantNameList.FirstOrDefault(t => t.Id == tenantId)?.Name ?? null;

                    users.Add(user);
                }
                reader.Close();
            }

            return users;
        }
        #endregion
    }
}
