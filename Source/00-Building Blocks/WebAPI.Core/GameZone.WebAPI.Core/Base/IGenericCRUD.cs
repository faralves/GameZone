using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.Domain.Contracts.Base
{
    public interface IGenericCRUD<T, Y, R, S>
    {
        Task<S?> Create(T entity, string? idUsuarioClaim);
        Task<S?> GetById(Y id);
        Task<IEnumerable<S>> GetAll();
        Task<S?> Update(R entity, string idUsuario);
        Task Delete(Y id);
    }
}
