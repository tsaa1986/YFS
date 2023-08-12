using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using YFS.Core.Mappings;
using YFS.Core.Models;
using YFS.Service.Interfaces;
using YFS.Service.Services;
using YFS.Repo.Data;
using YFS.Service.Filters.ActionFilters;

namespace YFS.Extension
{
    public static class ServiceExtension
    {
        public static void ConfigureRepositoryManager(this IServiceCollection services)
    => services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureMapping(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
            var mapperConfig = new MapperConfiguration(map =>
            {
                map.AddProfile<UserMappingProfile>();
                map.AddProfile<AccountGroupMappingProfile>();
                map.AddProfile<AccountTypeMappingProfile>();
                map.AddProfile<AccountMappingProfile>();
                map.AddProfile<AccountMonthlyBalanceMappingProfile>();
                map.AddProfile<UserAccountMappingProfile>();
                map.AddProfile<CurrencyMappingProfile>();
                map.AddProfile<CategoryMappingProfile>();
                map.AddProfile<OperationMappingProfile>();
            });
            services.AddSingleton(mapperConfig.CreateMapper());
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("jwtConfig");
            var secretKey = jwtConfig["secret"];
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["validIssuer"],
                    ValidAudience = jwtConfig["validAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountMonthlyBalanceService, AccountMonthlyBalanceService>();
            services.AddScoped<IAccountTypesService, AccountTypesService>();
            services.AddScoped<IAccountGroupsService, AccountGroupsService>();
            services.AddScoped<IOperationsService, OperationsService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}
