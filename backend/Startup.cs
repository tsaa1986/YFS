using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using YFS.Extension;
using YFS.Repo.Data;
using Microsoft.Extensions.Logging;
using Serilog;

namespace YFS
{
    public class Startup
    {
        string ConfigCors = "_ConfigCors";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var connectionString =
            //Configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging(true));//,ServiceLifetime.Transient);

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<RepositoryContext>(options =>
                options.UseNpgsql(connectionString,
                        x => x.MigrationsAssembly("YFS.Repo"))
                        .EnableSensitiveDataLogging(true));

            ServiceExtension.ConfigureRepositoryManager(services);
            ServiceExtension.ConfigureIdentity(services);
            ServiceExtension.RegisterDependencies(services);
            services.AddAuthentication();
            //overview securite rules for cors
            services.AddCors(options => options
            .AddPolicy(name: ConfigCors,
                      policy =>
                      {
                          policy
                          //.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                          //.SetIsOriginAllowed("http://localhost:3000")npm 
                          //.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                          .WithOrigins("http://localhost:3000", "https://localhost:3000", "http://localhost:3001", "https://localhost:3001", 
                                "http://localhost:5001", "http://localhost:5000", "http://10.10.10.20", 
                                "https://10.10.10.20").AllowAnyMethod().AllowAnyHeader().AllowCredentials();                          
                      }));
            ServiceExtension.ConfigureMapping(services);
            ServiceExtension.ConfigureJWT(services, Configuration);

            //swagger api
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {Title="My Api",Version="v1" }));

            //make the data repository available for dependency injection
            //The AddScoped method means that only one instance of the DataRepository class is created in a given HTTP request.This means
            //the lifetime of the class that is created lasts for the whole HTTP request
            services.AddControllers();

            services.AddLogging(builder =>
            {
                //builder.AddSerilog(Log.Logger);
                builder.AddConfiguration(Configuration.GetSection("Logging"));
                builder.AddConsole();
                builder.AddDebug();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(ConfigCors);

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            //SeedData.EnsurePopulated(app);
            //IdentitySeedData.EnsurePopulated(app);
            // handle DB seeding
            await SeedDb.Initialize(app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);

        }

    }
}
