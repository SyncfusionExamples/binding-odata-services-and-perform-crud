using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ODataServiceProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.OData.Batch;
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData.Extensions;

namespace ODataServiceProject
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<OrdersDetailsContext>(option => 
                option.UseSqlServer(Configuration.GetConnectionString("OrdersDetailsDatabase")));
            services.AddOData();
            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            var books = builder.EntitySet<Orders>("Orders");
            FunctionConfiguration myFirstFunction = books.EntityType.Collection.Function("MyFirstFunction");
            myFirstFunction.ReturnsCollectionFromEntitySet<Orders>("Orders");
            return builder.GetEdmModel();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.Count().Filter().OrderBy().Expand().Select().MaxTop(null);
                routes.MapODataServiceRoute(
                 "odata",
                 "odata",
                 model: GetEdmModel()
               );
            });
        }
    }
}
