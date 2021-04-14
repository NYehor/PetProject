using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Dapper;

namespace Infrastructure.Identity.Tables
{
    public class RoleClaimsTable<TKey> : BaseTable
        where TKey : IEquatable<TKey>
    {

        public RoleClaimsTable(SqlConnection connection) : base(connection) { }

        public async Task<IEnumerable<IdentityRoleClaim<TKey>>> GetClaimsAsync(TKey roleId)
        {
            const string sqlQuery = "SELECT * FROM [dbo].[RoleClaims] WHERE [RoleId] = @RoleId";
            
            return await _connection.QueryAsync<IdentityRoleClaim<TKey>>(sqlQuery, new { RoleId = roleId});
        }
    }
}