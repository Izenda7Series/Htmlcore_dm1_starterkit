using System.Data.SqlClient;

namespace ApiCoreStarterKit.Services
{
    //This class is currently verifying the user's password against the IzendaUser tabke of the configuration database for example purposes
    public class LoginManager
    {
        private string connectionString;
        private const string userInfoQuery = "SELECT UserName, PasswordHash FROM IzendaUser";

        public LoginManager(string connString)
        {
            connectionString = connString;
        }

        //Currently this validates the login based on the username and password
        public bool ValidateLogin(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string queryString = userInfoQuery + $" WHERE UserName = '{username}';";

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Only one record should be returned
                reader.Read();
                string dbPassword = reader["PasswordHash"].ToString();
                reader.Close();

                // Decrypt password for check
                string userPass = IzendaBoundary.IzendaTokenAuthorization.GetPassword(dbPassword);                
                if (password.Equals(userPass))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
