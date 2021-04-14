using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Dapper;

namespace Infrastructure.Identity.Tables
{
    public class UserLoginsTable<TKey> : BaseTable
        where TKey : IEquatable<TKey>
    {

        public UserLoginsTable(SqlConnection connection) : base(connection) { }

        public async Task<IEnumerable<IdentityUserLogin<TKey>>> GetLoginsAsync(TKey userId) 
        {
            const string sqlQuery = "SELECT * FROM [dbo].[UserLogins] WHERE [UserId] = @UserId";

            var userLogins = await _connection.QueryAsync<IdentityUserLogin<TKey>>(sqlQuery, new { UserId = userId });
            return userLogins;
        }

        public async Task<IdentityUserLogin<TKey>> FindAsync(TKey userId, string loginProvider, string providerKey)
        {
            const string sqlQuery = "SELECT * FROM [dbo].[UserLogins] WHERE [LoginProvider] = @LoginProvider" +
                " AND [ProviderKey] = @ProviderKey AND [UserId] = @UserId";

            var userLogins = await _connection.QueryFirstAsync<IdentityUserLogin<TKey>>(sqlQuery, new
            {
                LoginProvider = loginProvider,
                ProviderKey = providerKey,
                UserId = userId
            });
            return userLogins;
        }

        public async Task<IdentityUserLogin<TKey>> FindAsync(string loginProvider, string providerKey)
        {
            const string sqlQuery = "SELECT * FROM [dbo].[UserLogins] WHERE [LoginProvider] = @LoginProvider" +
                " AND [ProviderKey] = @ProviderKey";

            var userLogins = await _connection.QueryFirstAsync<IdentityUserLogin<TKey>>(sqlQuery, new {
                LoginProvider = loginProvider,
                ProviderKey = providerKey
            });

            return userLogins;
        }
    }
}
