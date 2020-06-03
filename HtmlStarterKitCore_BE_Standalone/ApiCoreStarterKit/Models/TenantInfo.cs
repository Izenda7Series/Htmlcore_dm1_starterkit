namespace ApiCoreStarterKit.Models
{
    public class TenantInfo
    {
        #region Properties
        public string Id { get; set; }

        public string Name { get; set; }
        #endregion

        #region CTOR
        public TenantInfo()
        { }

        public TenantInfo(string id, string name)
        {
            Id = id;
            Name = name;
        } 
        #endregion
    }
}
