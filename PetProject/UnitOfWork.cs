using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetProject.Repositories;

namespace PetProject
{
    public class UnitOfWork: IUnitOfWork
    {
        public IUserRepository Users { get; }
        public IThingRepository Things { get; }

        public UnitOfWork(string connectionString)
        {
            Users = new UserRepository(connectionString);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
