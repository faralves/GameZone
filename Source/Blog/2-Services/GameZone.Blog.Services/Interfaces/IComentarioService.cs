﻿using GameZone.Blog.Domain.Entities;
using GameZone.Domain.Contracts.Base;

namespace GameZone.Blog.Services.Interfaces
{
    public interface IComentarioService : IGenericRepoCRUD<Comentarios, int>
    {
        Task<IEnumerable<Comentarios>> GetByNoticiaId(int id);
    }
}
