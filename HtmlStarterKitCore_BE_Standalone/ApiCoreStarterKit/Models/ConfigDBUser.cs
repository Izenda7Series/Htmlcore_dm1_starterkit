namespace ApiCoreStarterKit.Models
{
    public class ConfigDBUser
    {
        #region Properties
        public string UserName { get; }

        public string TenantName { get; }

        public bool IsActive { get; }
        #endregion

        #region CTOR
        public ConfigDBUser(string userName, string tenantName, bool isActive)
        {
            UserName = userName;
            TenantName = tenantName;
            IsActive = isActive;
        } 
        #endregion
    }
}
