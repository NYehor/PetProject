using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Domain.Entities;
using Aplication.Interfaces;

namespace Infrastructure.Repositories
{
    public class ThingRepository : IThingRepository
    {
        public Task<int> CreateAsync(Thing item)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Thing item)
        {
            throw new NotImplementedException();
        }

        public Task<Thing> FindByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Thing item)
        {
            throw new NotImplementedException();
        }
    }
}
