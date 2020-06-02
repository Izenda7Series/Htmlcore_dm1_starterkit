namespace ApiCoreStarterKit.Models
{
    public class Tenant
    {
        #region Properties
        public string Id { get; set; }

        public string Name { get; set; }
        #endregion

        #region CTOR
        public Tenant()
        { }

        public Tenant(string id, string name)
        {
            Id = id;
            Name = name;
        } 
        #endregion
    }
}
