using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Aplication.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public class ApplicationDb : IApplicationDb
    {
        public IUserRepository Users { get; }
        public IThingRepository Things { get; }

        public ApplicationDb(SqlConnection connection)
        {
            Users = new UserRepository(connection);
        }
    }
}
