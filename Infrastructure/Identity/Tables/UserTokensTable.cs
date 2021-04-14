using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Infrastructure.Identity.Tables
{
    public class UserTokensTable<TKey> : BaseTable
        where TKey : IEquatable<TKey>
    {
        public UserTokensTable(SqlConnection connection) : base(connection) { }

        public async Task<IEnumerable<IdentityUserToken<TKey>>> GetTokensAsync(TKey userId)
        {
            string sqlQuery = "SELECT * FROM [dbo].[UserTokens] WHERE UserId = @UserId";

            return await _connection.QueryAsync<IdentityUserToken<TKey>>(sqlQuery, new { UserId = userId});
        }

        public async Task<IdentityResult> DeleteAsync(IdentityUserToken<TKey> token)
        {
            string sqlQuery = "DELETE FROM [dbo].[UserTokens] WHERE UserId = @UserId, " +
                                "LoginProvider = @LoginProvider, Name = @Name";

            var rows = await _connection.ExecuteAsync(sqlQuery, token);

            if (rows > 0)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user token {token.Name}" });
        }

        public async Task<IdentityUserToken<TKey>> FindTokenAsync(TKey userId, string loginProvider, string name)
        {
            string sqlQuery = "SELECT * FROM [dbo].[UserTokens] WHERE UserId = @UserId, LoginProvider = @LoginProvider, Name = @Name";

            return await _connection.QueryFirstAsync<IdentityUserToken<TKey>>(sqlQuery, new { 
                UserId = userId,
                LoginProvider = loginProvider,
                Namme = name
            });
        }
    }
}
