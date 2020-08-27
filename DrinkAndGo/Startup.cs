using DrinkAndGo.Data;
using DrinkAndGo.Data.Interfaces;
using DrinkAndGo.Data.Mocks;
using DrinkAndGo.Data.Models;
using DrinkAndGo.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DrinkAndGo
{
    public class Startup
    {
        private IConfigurationRoot _configurationRoot; //entrypoint for configuration data

        public Startup(IWebHostEnvironment hostingEnvironment)
        {
            _configurationRoot = new ConfigurationBuilder().SetBasePath(hostingEnvironment.ContentRootPath)
               .AddJsonFile("appsettings.json")
               .Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //server configuration
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_configurationRoot.GetConnectionString("DefaultConnection")));

            services.AddTransient<IDrinkRepository, DrinkRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sp => ShoppingCart.GetCart(sp));

            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseDeveloperExceptionPage(); // used to render the exception during the development environment
            app.UseStatusCodePages(); // used to show the error page for tha http status code
            app.UseStaticFiles(); // used to add support the application for css, image and js
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "categoryFilter", template: "Drink/{action}/{category?}", defaults: new { Controller = "Drink", action = "List" });
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{Id?}");
            });  // add default for the mvc
            DbInitializer.Seed(serviceProvider);
        }
    }
}
