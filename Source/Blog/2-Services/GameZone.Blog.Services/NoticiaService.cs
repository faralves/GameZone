using Azure.Storage.Blobs;
using GameZone.Blog.Domain.Entities;
using GameZone.Blog.Infra.Interfaces;
using GameZone.Blog.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace GameZone.Blog.Services
{
    public class NoticiaService : INoticiaService
    {
        private readonly INoticiaRepository _noticiaRepository;
        private readonly IConfiguration _configuration;
        private static bool _local_execution = false;
        private static string conectionStorageAccount = string.Empty;

        public NoticiaService(INoticiaRepository noticiaRepository, IConfiguration configuration)
        {
            _noticiaRepository = noticiaRepository;
            _configuration = configuration;
            _local_execution = Boolean.Parse(_configuration.GetSection("EnableLocalExecution").Value);
            conectionStorageAccount = _configuration.GetSection("ConfigsAzure:ConnectionStorageAccount").Value;
        }

        public async Task Create(Noticia noticia)
        {            
            await _noticiaRepository.Create(noticia); 
        }

        public Task Delete(int id)
        {
            return _noticiaRepository.Delete(id);
        }

        public async Task<IEnumerable<Noticia>> GetAll()
        {
            return await _noticiaRepository.GetAll();
        }

        public async Task<Noticia> GetById(int id)
        {
            return await _noticiaRepository.GetById(id);
        }

        public async Task Update(Noticia noticia)
        {
            await _noticiaRepository.Update(noticia);
        }

        public string UploadBase64ImageBlobStorage(string base64Image, string container, string urlImagem)
        {
            if (_local_execution)
                return urlImagem;

            FileInfo fileInfo = new FileInfo(urlImagem);

            // Gera um nome randomico para imagem
            var fileName = Guid.NewGuid().ToString() + fileInfo.Extension;

            // Limpa o hash enviado
            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image, "");

            // Gera um array de Bytes
            byte[] imageBytes = Convert.FromBase64String(data);

            // Define o BLOB no qual a imagem será armazenada
            var blobClient = new BlobClient(conectionStorageAccount, container, fileName);

            // Envia a imagem
            using (var stream = new MemoryStream(imageBytes))
            {
                var uploadFile = blobClient.Upload(stream);
            }

            if (File.Exists(urlImagem))
                File.Delete(urlImagem);

            // Retorna a URL da imagem
            return blobClient.Uri.AbsoluteUri;
        }

        public bool DeleteImageBlobStorage(Uri urlBlobStorage, string containerBlobStorage)
        {
            if (_local_execution)
                return true;

            var fileName = urlBlobStorage.LocalPath.Replace("/" + containerBlobStorage + "/", "");

            // Define o BLOB no qual a imagem está armazenada
            var blobClient = new BlobClient(conectionStorageAccount, containerBlobStorage, fileName);

            var deleted = blobClient.DeleteIfExistsAsync().Result;

            return deleted;
        }

    }
}
