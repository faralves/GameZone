using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.Domain.Contracts.Base
{
    public interface IGenericRepoCRUD<T, Y>
    {
        Task Create(T entity);
        Task<T?> GetById(Y id);
        Task<IEnumerable<T>> GetAll();
        Task Update(T entity);
        Task Delete(Y id);
    }
}
