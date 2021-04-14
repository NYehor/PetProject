using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Aplication.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> FindByUserNameAsync(string userName);
    }
}
