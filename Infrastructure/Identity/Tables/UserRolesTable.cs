using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Infrastructure.Identity.Tables
{
    public class UserRolesTable<TKey> : BaseTable
        where TKey : IEquatable<TKey>
    {
        public UserRolesTable(SqlConnection connection) : base(connection) { }

        public async Task<IEnumerable<IdentityRole<TKey>>> GetRolesAsync(TKey userId)
        {
            const string sqlQueary = "SELECT * FROM [dbo].[Roles] AS [r] INNER JOIN [dbo].[UserRoles] AS [ur]" +
                "ON [ur].[RoleId] = [r].[Id] WHERE [ur].[UserId] = @UserId";

            return await _connection.QueryAsync<IdentityRole<TKey>>(sqlQueary, new { UserId = userId });
        }

        public async Task<IdentityUserRole<TKey>> FindUsersRoleAsync(TKey userId, TKey roleId)
        {
            const string sqlQueary = "SELECT * FROM [dbo].[UsersRoles] WHERE [UserId] = @UserId, [RoleId] = @RoleId";

            return await _connection.QueryFirstAsync<IdentityUserRole<TKey>>(sqlQueary, new { 
                UserId = userId,
                RoleId = roleId
            });
        }
    }
}
