using GameZone.WebAPI.Core.Extensions;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameZone.WebAPI.Core
{
    public abstract class Service
    {
        protected StringContent ObterConteudo(object dado)
        {
            return new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json");
        }

        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var retorno = JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
            return retorno;
        }

        protected async Task<T> DeserializarObjeto<T>(object objeto)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var objetoJson = JsonSerializer.Serialize(objeto);

            var retorno =  JsonSerializer.Deserialize<T>(objetoJson, options);
            return retorno;
        }

        protected async Task<T> DeserializarObjetoResponseNewtonsoft<T>(HttpResponseMessage responseMessage)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync());
        }


        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
