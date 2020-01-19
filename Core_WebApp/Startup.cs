using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core_WebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core_WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 1. Database context 
        /// --EF Core context
        /// 2. MVC Options
        ///     -- Filters
        ///     -- Formatters
        /// 3. Security
        ///     -- Authisation for users based on JSON Web Tokans (JWT)
        ///     -- Authorisation
        ///         Base on ROle
        ///             Role base on policies
        /// 4. Cookes
        /// 5. CORS (communication across 2 domain) Policies
        ///     -- Web API
        /// 6. Customer Services
        ///     -- Domain Base Service classes (Business Logic)
        /// 7. Sessions
        ///     -- Session base
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AppDbConnection"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Conttracts : IApplicationBuilder and IWebHostEnvironment
        /// IApplicationBuilder
        ///     -- Manage HTTP request for Middlewares
        /// IWebHostEnvironment
        ///     -- Detect the host thing enviroment for execution whather it is PROD or Development.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
