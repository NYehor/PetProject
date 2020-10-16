using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PetProject.Models;
using Dapper;

namespace PetProject.Repositories
{
    public class UserRepository : IUserRepository, IRepository<User>
    {
        private string _connectionString;
        public UserRepository(string connectionString) 
        {
            _connectionString = connectionString;
        }

        public void Create(User item)
        {
            Console.WriteLine("Work!");

            using (IDbConnection db = new SqlConnection(_connectionString))
            {

                string sqlQuery = "INSERT INTO Users (Username, Email, Password, FirstName, LastName, RegistrationDate) VALUES (@Username, @Email, @Password, @FirstName, @LastName, @RegistrationDate)";
                db.Execute(sqlQuery, item);
            }
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
