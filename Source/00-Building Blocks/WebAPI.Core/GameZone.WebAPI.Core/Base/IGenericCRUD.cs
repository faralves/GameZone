using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.Domain.Contracts.Base
{
    public interface IGenericCRUD<T, Y, R>
    {
        Task<R?> Create(T entity);
        Task<R?> GetById(Y id);
        Task<IEnumerable<R>> GetAll();
        Task<R?> Update(R entity);
        Task Delete(Y id);
    }
}
