using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApiCoreStarterKit
{
    public class Startup
    {
        #region Constants
        private readonly string jquery = "https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js";
        private readonly string bootstrapjs = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js";
        private readonly string bootstrapcss = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css";
        private readonly string copyright = DateTime.Now.Year + " Izenda Integrated BI Platform"; 
        #endregion

        #region Properties
        public IConfiguration Configuration { get; }
        #endregion

        #region CTOR
        public Startup(IConfiguration configuration) => Configuration = configuration;
        #endregion

        #region Methods
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCors("AllowOrigin");

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync(
                    "<!DOCTYPE html>"+
                    "<html>"+
                    "<head>"+
                    "<title>Izenda Landing</title>"+
                     "<script src=" +
                    jquery + ">" +
                    "</script>" +
                    "<script src=" +
                    bootstrapjs + ">" +
                    "</script>" +

                    "<link rel=" +
                    "stylesheet" + " " +
                    "href=" +
                    bootstrapcss + ">" +

                    "</head>" +
                    "<body>"+
                    
                    "<h3>This is the API used by the HTML Core project, the izenda API should be deployed separately. See the readme.txt file for more details.</h3>" +
                 
                    copyright +
                   
                    "</body>"+
                    "</html>");
            });
        }
        #endregion
    }
}
