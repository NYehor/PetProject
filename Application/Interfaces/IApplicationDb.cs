using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IApplicationDb
    {
        IUserRepository Users { get; }
        IThingRepository Things { get; }
    }
}
