using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection;
using Aplication.Interfaces;
using System.Data.SqlClient;
using Infrastructure.Identity;
using Infrastructure.Identity.Tables;
using Microsoft.AspNetCore.Authentication;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<SqlConnection>(e => new SqlConnection(connectionString));
            services.AddTransient<IApplicationDb, ApplicationDb>();

            services.AddTransient<IUserStore<ApplicationUser<string>>, UserStore<string>>();
            services.AddTransient<IRoleStore<IdentityRole<string>>, RoleStore<string>>();

            services.AddTransient<RoleClaimsTable<string>>();
            services.AddTransient<RolesTable<string>>();
            services.AddTransient<UserClaimsTable<string>>();
            services.AddTransient<UserLoginsTable<string>>();
            services.AddTransient<UserRolesTable<string>>();
            services.AddTransient<UsersTable<string>>();
            services.AddTransient<UserTokensTable<string>>();

            services.AddIdentity<ApplicationUser<string>, IdentityRole<string>>()
                .AddUserStore<UserStore<string>>()
                .AddRoleStore<RoleStore<string>>()
                .AddDefaultTokenProviders();

            //services.AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser<string>, ApplicationDbContext>();

            services.AddIdentityServer().AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser<string>>();

            services.AddAuthentication()
                    //.AddGoogle(googleOptions => {
                    //    IConfigurationSection googleAuthNSection =
                    //    configuration.GetSection("Authentication:Google");

                    //    googleOptions.ClientId = googleAuthNSection["ClientId"];
                    //    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                    //})
                    .AddIdentityServerJwt();

            return services;
        }
    }
}
