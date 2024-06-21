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
using System;
using Microsoft.AspNetCore.DataProtection;
using YFS.Repo.Repository;

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
                map.AddProfile<ApiTokenMappingProfile>();
            });
            services.AddSingleton(mapperConfig.CreateMapper());
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequiredLength = 12;
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = true;
                o.User.RequireUniqueEmail = true;
                o.Lockout.MaxFailedAccessAttempts = 10;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddDefaultTokenProviders();
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("jwtConfig");
            var secretKey = jwtConfig["secret"];

            if (string.IsNullOrEmpty(secretKey) || Encoding.UTF8.GetByteCount(secretKey) < 32)
            {
                throw new InvalidOperationException("JWT secret key must be at least 256 bits (32 bytes) long.");
            }

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
                    //IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey)),
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
            services.AddScoped<IBanksSyncService, BanksSyncService>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddHttpClient();
            services.AddScoped<IMonoIntegrationApiService, MonoIntegrationApiService>();
            services.AddScoped<IMonoSyncedTransactionRepository, MonoSyncedTransactionRepository>();
            services.AddScoped<IMonoSyncedTransactionService, MonoSyncedTransactionService>();
            services.AddScoped<LanguageScopedService>();
        }
    }
}
