using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace GameZone.News.WebApp.Models.Services
{
    public abstract class Service
    {
        protected HttpContent ObterConteudo(object dado)
        {
            if (dado == null)
                throw new Exception("Objeto Informado Nulo, não é possível realizar a conversão de tipo de dados");

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(dado);
            byte[] data = Encoding.UTF8.GetBytes(json);
            ByteArrayContent byteContent = new ByteArrayContent(data);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpContent content = byteContent;
            return content;
        }

        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected async Task<T> DeserializarObjetoResponseNewtonsoft<T>(HttpResponseMessage responseMessage)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync());
        }
    }
}
