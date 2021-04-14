using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Security.Claims;

namespace Infrastructure.Identity.Tables
{
    public class UserClaimsTable<TKey>: BaseTable
        where TKey : IEquatable<TKey>
    {

        public UserClaimsTable(SqlConnection connection) : base(connection) { }

        public async Task<IEnumerable<IdentityUserClaim<TKey>>> GetClaimsAsync(TKey userId)
        {
            const string sqlQuery = "SELECT * FROM [dbo].[UserClaims] WHERE [UserId] = @UserId";

             return await _connection.QueryAsync<IdentityUserClaim<TKey>>(sqlQuery, new { UserId = userId});
        }
    }
}
