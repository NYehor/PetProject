using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Domain.Entities;
using Aplication.Interfaces;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnection _connection;

        public UserRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> CreateAsync(User item)
        {
            const string sqlQuery = "INSERT INTO [dbo].[Users] VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail," +
                "@EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, " +
                "@TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount, @FirstName, @SecondName)";


            return await _connection.ExecuteAsync(sqlQuery, item);        
        }

        public async Task<int> DeleteAsync(User item)
        {
            string sql = "DELETE FROM Users WHERE Id = @Id";

            return await _connection.ExecuteAsync(sql, new { item.Id});
        }

        public async Task<int> UpdateAsync(User item)
        {
            string sqlQuery = "UPDATE Users" +
                 "SET (@Username, @Email, @PasswordHash, @FirstName, @LastName, @RegistrationDate)" +
                 "WHERE Id = @Id";

            return await _connection.ExecuteAsync(sqlQuery, item);
        }

        public async Task<User> FindByIdAsync(Guid id)
        {
            string sql = "SELECT * FROM Users WHERE Id = @Id";

            return await _connection.QuerySingleOrDefaultAsync(sql, new { Id = id });
        }

        public async Task<User> FindByUserNameAsync(string userName)
        {
            string sql = "SELECT * FROM Users WHERE UserName = @UserName;";

            var tmp = await _connection.QuerySingleOrDefaultAsync<User>(sql, new
            {
                UserName = userName
            });

            return await _connection.QuerySingleOrDefaultAsync<User>(sql, new
            {
                UserName = userName
            });
        }
    }
}
