using DbUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YFS.Data;
using YFS.Data.Models;

namespace YFS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString =
                Configuration.GetConnectionString("DefaultConnection");

            // TODO - Create and configure an instance of the
            EnsureDatabase.For.SqlDatabase(connectionString);

            // DbUp upgrader
            var upgrader = DeployChanges.To
                 .SqlDatabase(connectionString, null)
                 .WithScriptsEmbeddedInAssembly(
                 System.Reflection.Assembly.GetExecutingAssembly()
                 )
                 .WithTransaction()
                 .Build();

            // TODO - Do a database migration if there are any 
            if (upgrader.IsUpgradeRequired())
            {
                upgrader.PerformUpgrade();
            }

            //swagger api
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {Title="My Api",Version="v1" }));


            // pending SQL 
            //Scripts

            services.AddRazorPages();

            //make the data repository available for dependency injection
            //The AddScoped method means that only one instance of the DataRepository class is created in a given HTTP request.This means
            //the lifetime of the class that is created lasts for the whole HTTP request
            services.AddControllers();
            services.AddScoped<IAccountGroupRepository, AccountGroupRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
