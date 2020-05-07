namespace ApiCoreStarterKit.Models
{
    public class Tenant
    {
        #region Properties
        public string Id { get; }
        public string Name { get; }
        #endregion

        #region CTOR
        public Tenant(string id, string name)
        {
            Id = id;
            Name = name;
        } 
        #endregion
    }
}
