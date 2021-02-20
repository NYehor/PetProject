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
    public class UserRepository : IUserRepository, IRepository<User>
    {
        private readonly SqlConnection _connection;

        public UserRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public void Create(User item)
        {
            string sqlQuery = "INSERT INTO Users (Username, Email, Password, FirstName, LastName, RegistrationDate) " +
                "VALUES (@Username, @Email, @Password, @FirstName, @LastName, @RegistrationDate)";

            _connection.Execute(sqlQuery, item);        
        }

        public void Delete(User item)
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
