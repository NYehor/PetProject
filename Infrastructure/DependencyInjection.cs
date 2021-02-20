using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection;
using Aplication.Interfaces;
using System.Data.SqlClient;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<SqlConnection>(e => new SqlConnection(connectionString));
            services.AddTransient<ApplicationDb>();

            return services;
        }
    }
}
