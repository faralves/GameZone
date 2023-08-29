using System.Net.Http.Headers;

namespace GameZone.News.WebApp.Models.Services.Handlers
{
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccesor = httpContextAccesor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var authorizationHeader = _httpContextAccesor.HttpContext
                    .Request.Headers["Authorization"];

                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
                }

                var token = GetToken();

                if (token != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetToken()
        {
            try
            {
                return _httpContextAccesor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "JWT")?.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}