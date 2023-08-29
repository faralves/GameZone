using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace GameZone.News.WebApp.Models.Services
{
    public abstract class Service
    {
        protected HttpContent ObterConteudo(object dado)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task<T> DeserializarObjetoResponseNewtonsoft<T>(HttpResponseMessage responseMessage)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
