using GameZone.Blog.Domain.Entities;
using GameZone.Domain.Contracts.Base;

namespace GameZone.Blog.Services.Interfaces
{
    public interface INoticiaService : IGenericRepoCRUD<Noticia, int>
    {
        string UploadBase64ImageBlobStorage(string database64Content, string containerBlobStorage, string urlImagem);
        bool DeleteImageBlobStorage(Uri urlBlobStorage, string containerBlobStorage);
    }
}
