using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IRepository<T> where T: class
    {
        T GetById(int id);
        void Create(T item);
        void Update(T item);
        void Delete(T item);
        void Save();
    }
}
