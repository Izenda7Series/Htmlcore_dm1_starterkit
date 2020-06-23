using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ApiCoreStarterKit
{
    public class Program
    {
        #region CTOR
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Run();
        }
        #endregion

        #region Methods
        public static IWebHost CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseIISIntegration()
               .UseStartup<Startup>() //This calls the Startup.cs class
               .Build(); 
        #endregion
    }
}
