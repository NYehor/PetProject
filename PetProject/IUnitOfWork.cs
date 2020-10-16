using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetProject.Repositories;

namespace PetProject
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IThingRepository Things { get; }
    }
}
