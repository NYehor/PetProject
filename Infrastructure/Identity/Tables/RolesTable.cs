using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Infrastructure.Identity.Tables
{
    public class RolesTable<TKey> : BaseTable
        where TKey : IEquatable<TKey>
    {

        public RolesTable(SqlConnection connection) : base(connection) { }

        public async Task<IdentityRole<TKey>> FindByUserNameASync(string normalizedRoleName)
        {
            const string sqlQueary = "SELECT * FROM [dbo].[Roles] WHERE [NormalizedRoleName] = @NormalizedRoleName";

            return await _connection.QueryFirstAsync<IdentityRole<TKey>>(sqlQueary, new { NormalizedRoleName = normalizedRoleName });
        }

        public async Task<bool> CreateAsync(IdentityRole<TKey> role)
        {
            const string sqlQueary = "INSERT INTO [dbo].[Roles] (Name, NormalizedName, ConcurrencyStamp) VALUE (@Name, @NormalizedName, @ConcurrencyStamp)";

            int rows = await _connection.ExecuteAsync(sqlQueary, role);

            if(rows > 0)
                return true;

            return false;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole<TKey> role)
        {
            string sqlQuery = "DELETE FROM [dbo].[Roles] WHERE Id = @Id";

            var rows = await _connection.ExecuteAsync(sqlQuery, new { role.Id });

            if (rows > 0)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed(new IdentityError { Description = $"Could not delete role {role.Name}" });
        }

        public async Task<IdentityRole<TKey>> FindByIdAsync(string id)
        {
            const string sqlQueary = "SELECT * FROM [dbo].[Roles] WHERE Id = @Id";

            return await _connection.QueryFirstAsync<IdentityRole<TKey>>(sqlQueary, new { Id = id});
        }

        public async Task<IdentityRole<TKey>> FindByNameAsync(string normalizedName)
        {
            const string sqlQueary = "SELECT * FROM [dbo].[Roles] WHERE NormalizedRoleName = @NormalizedRoleName";

            return await _connection.QueryFirstAsync<IdentityRole<TKey>>(sqlQueary, new { NormalizedRoleName = normalizedName });
        }

        public async Task<bool> UpdateAsync(IdentityRole<TKey> role, IList<IdentityRoleClaim<TKey>> claims)
        {
            const string updateRoleSql = "UPDATE [dbo].[Roles] SET [Name] = @Name, [NormalizedName] = @NormalizedName, [ConcurrencyStamp] = @ConcurrencyStamp" +
                "WHERE [Id] = @Id";

            using (var transaction = _connection.BeginTransaction())
            {
                await _connection.ExecuteAsync(updateRoleSql, role, transaction);

                if (claims?.Count > 0)
                {
                    const string deleteClaimSql = "DELETE FROM [dbo].[RoleClaims] WHERE [RoleId] = @RoleId";
                    await _connection.ExecuteAsync(deleteClaimSql, new { RoleId = role.Id }, transaction);

                    const string insertClaimSql = "INSER INTO [dbo].[RoleClaims] (RoleId, ClaimType, ClaimValues) VALUES (@RoleId, @ClaimType, @ClaimValues)";
                    await _connection.ExecuteAsync(insertClaimSql, claims, transaction);
                }

                try
                {
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }

            return true;
        }
    }
}
