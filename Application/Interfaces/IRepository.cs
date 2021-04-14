using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IRepository<T> where T: class
    {
        Task<T> FindByIdAsync(Guid id);
        Task<int> CreateAsync(T item);
        Task<int> UpdateAsync(T item);
        Task<int> DeleteAsync(T item);
    }
}
