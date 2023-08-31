using GameZone.News.WebApp.Models.DTO.Request;
using GameZone.News.WebApp.Models.DTO.Response;
using GameZone.News.WebApp.Models.Interfaces;
using GameZone.WebAPI.Core.Usuario;
using Newtonsoft.Json;

namespace GameZone.News.WebApp.Models.Services
{
    public class NewsService : Service, INewsService
    {
        private HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private IAutenticacaoService _autenticacaoService;
        private static bool _local_execution = false;
        private static string _local_path_file_images = "";

        private string _endPointNoticia = string.Empty;
        private string _endPointComentario = string.Empty;

        public NewsService(IConfiguration configuration, HttpClient httpClient, IAutenticacaoService autenticacaoService)
        {
            //try
            //{
                _configuration = configuration;

                _httpClient = httpClient;
                _autenticacaoService = autenticacaoService;

                string urlAddress = _configuration.GetSection("BlogUrl").Value;
                httpClient.BaseAddress = new Uri(urlAddress);

                _endPointNoticia = $"{urlAddress}/api/Noticias";
                _endPointComentario = $"{urlAddress}/api/Comentario";

                _local_execution = Boolean.Parse(configuration.GetSection("EnableLocalExecution").Value);
                _local_path_file_images = configuration.GetSection("LocalPathImages").Value;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }


        public async Task<IEnumerable<DTO.Response.CreateNewsDTO>> GetAllNewsAsync()
        {
            //try
            //{
                var news = new List<DTO.Response.CreateNewsDTO>();
                using (var response = await _httpClient.GetAsync(_endPointNoticia))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var newsJson = await response.Content.ReadAsStringAsync();
                        news = JsonConvert.DeserializeObject<List<DTO.Response.CreateNewsDTO>>(newsJson);
                    }
                }
                return news;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task CreateNewsAsync(DTO.Request.CreateNewsDTO createNewsDto)
        {
            //try
            //{
                createNewsDto.UrlImagem = GetUrlImagemServidor(createNewsDto.Arquivo);

                var newsContent = ObterConteudo(createNewsDto);

                using (var response = await _httpClient.PostAsync(_endPointNoticia, newsContent))
                {
                    response.EnsureSuccessStatusCode();
                }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }


        private string GetUrlImagemServidor(IFormFile? arquivo)
        {
            string urlImagem = string.Empty;
            //try
            //{
                string path = Path.Combine(Directory.GetCurrentDirectory(), "ArquivosRecebidos");

                if (arquivo != null)
                {
                    FileInfo fileInfo = new FileInfo(arquivo.FileName);
                    string fileName = arquivo.FileName;

                    string fileNameWithPath = string.Empty;

                    if (_local_execution)
                        path = _local_path_file_images;

                    fileNameWithPath = Path.Combine(path, fileName);

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    if (!File.Exists(fileNameWithPath))
                    {
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            arquivo.CopyTo(stream);
                        }
                    }

                    urlImagem = fileNameWithPath;
                }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

            return urlImagem;
        }

        public async Task<DTO.Request.UpdateNewsDTO?> GetNewsByIdAsync(int id)
        {
            //try
            //{
                DTO.Request.UpdateNewsDTO consulta = null;

                string endpoint = $"{_endPointNoticia}/{id}";
                using (var response = await _httpClient.GetAsync(endpoint))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var consultaJson = await response.Content.ReadAsStringAsync();
                        consulta = JsonConvert.DeserializeObject<DTO.Request.UpdateNewsDTO>(consultaJson);
                    }
                }
                return consulta;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task UpdateNewsAsync(DTO.Request.UpdateNewsDTO updateNewsDto)
        {
            //try
            //{
                string urlImagem = GetUrlImagemServidor(updateNewsDto.Arquivo);
                if (!string.IsNullOrEmpty(urlImagem))
                    updateNewsDto.UrlImagem = urlImagem;

                updateNewsDto = new UpdateNewsDTO().AtualizarInfosImagem(updateNewsDto);

                var newsContent = ObterConteudo(updateNewsDto);

                string endpoint = $"{_endPointNoticia}/{updateNewsDto.Id}";

                using (var response = await _httpClient.PutAsync(endpoint, newsContent))
                {
                    response.EnsureSuccessStatusCode();
                }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task DeleteNewsAsync(int id)
        {
            //try
            //{
                string endpoint = $"{_endPointNoticia}/{id}";
                using (var response = await _httpClient.DeleteAsync(endpoint)) 
                { 
                    response.EnsureSuccessStatusCode(); 
                }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task<DTO.Response.NewsDto> GetById(int id)
        {
            //try
            //{
                DTO.Response.NewsDto consulta = null;

                string endpoint = $"{_endPointNoticia}/{id}";
                using (var response = await _httpClient.GetAsync(endpoint))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var consultaJson = await response.Content.ReadAsStringAsync();
                        consulta = JsonConvert.DeserializeObject<DTO.Response.NewsDto>(consultaJson);
                    }
                }

                if (consulta != null)
                {
                    var usuarioAutor = await _autenticacaoService.GetUserDto(consulta.AspNetUsersId);
                    consulta.Autor = usuarioAutor.Name;

                    consulta.Comentarios = await GetCommentByNoticiaId(id);

                    if (consulta.Comentarios != null)
                    {
                        foreach (var comentario in consulta.Comentarios)
                        {
                                var usuario = await _autenticacaoService.GetUserDto(comentario.AspNetUsersId);
                                comentario.NomeUsuario = usuario.Name;
                        }
                    }
                }
                return consulta;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
        public async Task<ICollection<CommentDTO>> GetCommentByNoticiaId(int id)
        {
            //try
            //{
                ICollection<CommentDTO> listComment = null;

                string endpoint = $"{_endPointComentario}/GetByNoticiaId/{id}";
                using (var response = await _httpClient.GetAsync(endpoint))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var consultaJson = await response.Content.ReadAsStringAsync();
                        listComment = JsonConvert.DeserializeObject<ICollection<CommentDTO>>(consultaJson);
                    }
                }

                return listComment;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public async Task CreateCommentAsync(DTO.Request.CreateCommentDTO createCommentDto)
        {
            //try
            //{
                var newsContent = ObterConteudo(createCommentDto);

                using (var response = await _httpClient.PostAsync(_endPointComentario, newsContent))
                {
                    response.EnsureSuccessStatusCode();
                }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }
    }
}
