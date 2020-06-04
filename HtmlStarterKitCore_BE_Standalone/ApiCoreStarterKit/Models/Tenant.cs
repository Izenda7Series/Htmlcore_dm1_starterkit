namespace ApiCoreStarterKit.Models
{
    public class Tenant
    {
        #region Properties
        public int Id { get; set; }

        public string Name { get; set; }
        #endregion

        #region CTOR
        public Tenant()
        { }

        public Tenant(int id, string name)
        {
            Id = id;
            Name = name;
        } 
        #endregion
    }
}
