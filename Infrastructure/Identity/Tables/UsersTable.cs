using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Dapper;
using System.Security.Claims;

namespace Infrastructure.Identity.Tables
{
    public class UsersTable<TKey> : BaseTable
        where TKey : IEquatable<TKey>
    {
        public UsersTable(SqlConnection connection) : base(connection) { }

        public async Task<IdentityResult> CreateAsync(ApplicationUser<TKey> user)
        {
            const string sqlQuery = "INSERT INTO [dbo].[Users] VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail," +
                "@EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, " +
                "@TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount, @FirstName, @SecondName, @RegistrationDate)";

            var rows = await _connection.ExecuteAsync(sqlQuery, user);

            if (rows > 0)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Email}" });
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser<TKey> user)
        {
            string sqlQuery = "DELETE FROM [dbo].[Users] WHERE Id = @Id";

            var rows = await _connection.ExecuteAsync(sqlQuery, new { user.Id });

            if (rows > 0)
                return IdentityResult.Success;
            else
                return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {user.Email}" });
        }

        public async Task<ApplicationUser<TKey>> FindByEmailAsync(string normalizedEmail)
        {
            string sqlQuery = "SELECT * FROM [dbo].[Users] WHERE NormalizedEmail = @NormalizedEmail";

            return  await _connection.QuerySingleOrDefaultAsync<ApplicationUser<TKey>>(sqlQuery, new { NormalizedEmail = normalizedEmail });
        }

        public async Task<ApplicationUser<TKey>> FindByIdAsync(TKey userId)
        {
            string sqlQuery = "SELECT * FROM [dbo].[Users] WHERE Id = @Id";

            return await _connection.QuerySingleOrDefaultAsync(sqlQuery, new { Id =  userId });
        }

        public async Task<ApplicationUser<TKey>> FindByUserNameAsync(string normalizedUserName)
        {
            string sqlQuery = "SELECT * FROM [dbo].[Users] WHERE NormalizedUserName = @NormalizedUserName";

            return await _connection.QuerySingleOrDefaultAsync<ApplicationUser<TKey>>(sqlQuery, new { NormalizedUserName = normalizedUserName });
        }

        public async Task<IEnumerable<ApplicationUser<TKey>>> GetUsersForClaimAsync(Claim claim)
        {
            string sqlQuery = "SELECT * FROM [dbo].[Users] AS [u] INNER JOIN [dbo].[UserClaims] AS [uc] " +
                        "ON [u].[Id] = [uc].[UserId] WHERE ClaimType = @ClaimType, ClaimValue = @ClaimValue";

            return await _connection.QueryAsync<ApplicationUser<TKey>>(sqlQuery, new { ClaimType = claim.Type, ClaimValue = claim.Value });
        }

        public async Task<IEnumerable<ApplicationUser<TKey>>> GetUsersInRoleAsync(string normalizedRoleName)
        {
            const string sqlQueary = "SELECT * FROM [dbo].[Roles] AS [r] INNER JOIN [dbo].[UserRoles] AS [ur]" +
                "ON [ur].[RoleId] = [r].[Id] WHERE [ur].[UserId] = @UserId";

            return await _connection.QueryAsync<ApplicationUser<TKey>>(sqlQueary, new { NormalizedRoleName = normalizedRoleName });
        }

        public async Task<bool> UpdateAsync(ApplicationUser<TKey> user, IList<IdentityUserClaim<TKey>> claims, IList<IdentityUserRole<TKey>> roles, IList<IdentityUserLogin<TKey>> logins, IList<IdentityUserToken<TKey>> tokens)
        {
            const string updateUserSql = "UPDATE [dbo].[Users] SET UserName = @UserName, NormalizedUserName = @NormalizedUserName, Email = @Email, NormalizedEmail = @NormalizedEmail," +
                "EmailConfirmed = @EmailConfirmed, PasswordHash = @PasswordHash, SecurityStamp = @SecurityStamp, ConcurrencyStamp = @ConcurrencyStamp, PhoneNumber = @PhoneNumber, PhoneNumberConfirmed = @PhoneNumberConfirmed, " +
                "TwoFactorEnabled = @TwoFactorEnabled, LockoutEnd = @LockoutEnd, LockoutEnabled = @LockoutEnabled, AccessFailedCount = @AccessFailedCount, FirstName = @FirstName, SecondName = @SecondName" +
                "WHERE [Id] = @Id;";

            using (var transaction = _connection.BeginTransaction())
            {
                await _connection.ExecuteAsync(updateUserSql, user, transaction);

                if (claims?.Count() > 0)
                {
                    const string deleteClaimSql = "DELETE FROM [dbo].[UserClaims] WHERE [UserId] = @userId";
                    await _connection.ExecuteAsync(deleteClaimSql, new { UserId = user.Id}, transaction);

                    const string insertClaimSql = "INSER INTO [dbo].[UserClaims] (UserId, ClaimType, ClaimValues) VALUES (@UserId, @ClaimType, @ClaimValues)";
                    await _connection.ExecuteAsync(insertClaimSql, claims, transaction);
                }

                if (roles?.Count() > 0)
                {
                    const string deleteRoleSql = "DELETE FROM [dbo].[UserRoles] WHERE [UserId] = @userId";
                    await _connection.ExecuteAsync(deleteRoleSql, new { UserId = user.Id }, transaction);

                    const string insertRoleSql = "INSER INTO [dbo].[UserRoles] (RoleId, ClaimType, ClaimValues) VALUES (@RoleId, @ClaimType, @ClaimValues)";
                    await _connection.ExecuteAsync(insertRoleSql, roles, transaction);
                }

                if (tokens?.Count() > 0)
                {
                    const string deleteTokenSql = "DELETE FROM [dbo].[UserTokens] WHERE [UserId] = @userId";
                    await _connection.ExecuteAsync(deleteTokenSql, new { UserId = user.Id }, transaction);

                    const string insertTokenSql = "INSER INTO [dbo].[UserToken] VALUES (@UserId, @LoginProvider, @Name, @Value);";
                    await _connection.ExecuteAsync(insertTokenSql, tokens, transaction);
                }

                if (logins?.Count() > 0)
                {
                    const string deleteLoginSql = "DELETE FROM [dbo].[UserLogins] WHERE [UserId] = @userId";
                    await _connection.ExecuteAsync(deleteLoginSql, new { UserId = user.Id }, transaction);

                    const string insertTokenSql = "INSER INTO [dbo].[UserToken] VALUES (@LoginProvider, @ProviderKey, @ProviderDisplayName, @UserId);";
                    await _connection.ExecuteAsync(insertTokenSql, tokens, transaction);
                }

                try
                {
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }

            return true;
        }
    }
}
