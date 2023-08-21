using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using GameZone.News.WebApp.Models.DTO.Request;
using GameZone.News.WebApp.Models.DTO.Response;
using GameZone.News.WebApp.Models.Interfaces;
using GameZone.WebAPI.Core.Usuario;
using Newtonsoft.Json;

namespace GameZone.News.WebApp.Models.Services
{
    public class NewsService : Service, INewsService
    {
        private readonly IConfiguration _configuration;
        private HttpClient _httpClient;
        private HttpClient _httpClientComentario;
        private IAutenticacaoService _autenticacaoService;
        private readonly IAspNetUser _user;
        private static bool _local_execution = false;
        private static string _local_path_file_images = "";

        private string _url_base_address = string.Empty;
        private string _url_base_comentario_address = string.Empty;

        public NewsService(IConfiguration configuration, IAspNetUser user, HttpClient httpClient, HttpClient httpClientComentario, IAutenticacaoService autenticacaoService)
        {
            _configuration = configuration;
            _url_base_address = _configuration.GetSection("BlogUrl").Value;
            _url_base_address += "/api/Noticias";

            httpClient.BaseAddress = new Uri(_url_base_address);
            _httpClient = httpClient;

            _url_base_comentario_address = _configuration.GetSection("BlogUrl").Value;
            _url_base_comentario_address += "/api/Comentario";
            httpClientComentario.BaseAddress = new Uri(_url_base_comentario_address);
            _httpClientComentario = httpClientComentario;

            _user = user;

            _local_execution = Boolean.Parse(configuration.GetSection("EnableLocalExecution").Value);
            _local_path_file_images = configuration.GetSection("LocalPathImages").Value;
            _autenticacaoService = autenticacaoService;
        }

        public async Task<IEnumerable<DTO.Response.CreateNewsDTO>> GetAllNewsAsync()
        {
            string endpoint = $"{_url_base_address}";
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var newsJson = await response.Content.ReadAsStringAsync();
                var news = JsonConvert.DeserializeObject<List<DTO.Response.CreateNewsDTO>>(newsJson);
                return news;
            }
            else
            {
                return new List<DTO.Response.CreateNewsDTO>();
            }
        }
        public async Task CreateNewsAsync(DTO.Request.CreateNewsDTO createNewsDto)
        {
            createNewsDto = AtualizarInfosImagem(createNewsDto);

            var newsContent = ObterConteudo(createNewsDto);
            string endpoint = $"{_url_base_address}";

            var response = await _httpClient.PostAsync(endpoint, newsContent);
        }


        private DTO.Request.CreateNewsDTO AtualizarInfosImagem(DTO.Request.CreateNewsDTO createNewsDto)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "ArquivosRecebidos");

            if (createNewsDto.Arquivo != null)
            {
                FileInfo fileInfo = new FileInfo(createNewsDto.Arquivo.FileName);
                string fileName = createNewsDto.Arquivo.FileName;

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
                        createNewsDto.Arquivo.CopyTo(stream);
                    }
                }

                createNewsDto.UrlImagem = fileNameWithPath;
            }
            return createNewsDto;
        }

        public async Task<DTO.Request.UpdateNewsDTO?> GetNewsByIdAsync(int id)
        {
            //var client = await GetHttpClientAsync(false);
            string endpoint = $"{_url_base_address}/{id}";
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var consultaJson = await response.Content.ReadAsStringAsync();
                var consulta = JsonConvert.DeserializeObject<DTO.Request.UpdateNewsDTO>(consultaJson);
                return consulta;
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateNewsAsync(DTO.Request.UpdateNewsDTO updateNewsDto)
        {
            var newsContent = ObterConteudo(updateNewsDto);
            string endpoint = $"{_url_base_address}";

            var response = await _httpClient.PutAsync(endpoint, newsContent);
        }

        public async Task DeleteNewsAsync(int id)
        {
            string endpoint = $"{_url_base_address}/{id}";
            var response = await _httpClient.DeleteAsync(endpoint);
        }

        public async Task<DTO.Response.NewsDto> GetById(int id)
        {
            string endpoint = $"{_url_base_address}/{id}";
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var consultaJson = await response.Content.ReadAsStringAsync();
                var consulta = JsonConvert.DeserializeObject<DTO.Response.NewsDto>(consultaJson);
                if(consulta != null)
                {
                    consulta.Comentarios = await GetCommentByNoticiaId(id);

                    if(consulta.Comentarios != null)
                    {
                        foreach (var comentario in consulta.Comentarios)
                        {
                            var usuario = await _autenticacaoService.GetUserDto(comentario.AspNetUsersId);
                            comentario.NomeUsuario = usuario.Name;
                        }
                    }
                }

                return consulta;
            }
            else
            {
                NewsDto news = null;
                return news;
            }
        }
        public async Task<ICollection<CommentDTO>> GetCommentByNoticiaId(int id)
        {
            _url_base_comentario_address += "/GetByNoticiaId";

            string endpoint = $"{_url_base_comentario_address}/{id}";
            var response = await _httpClientComentario.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var consultaJson = await response.Content.ReadAsStringAsync();
                var listComment = JsonConvert.DeserializeObject<ICollection<CommentDTO>> (consultaJson);
                return listComment;
            }
            else
            {
                ICollection<CommentDTO> listComment = null;
                return listComment;
            }
        }

        public async Task CreateCommentAsync(DTO.Request.CreateCommentDTO createCommentDto)
        {
            _httpClient.BaseAddress = new Uri(_url_base_comentario_address); 

            var newsContent = ObterConteudo(createCommentDto);
            string endpoint = $"{_url_base_comentario_address}";

            var response = await _httpClient.PostAsync(endpoint, newsContent);
        }
    }
}
