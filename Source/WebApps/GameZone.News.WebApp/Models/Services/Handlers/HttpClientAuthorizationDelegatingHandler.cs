using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using GameZone.WebAPI.Core.Usuario;

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

        private string GetToken()
        {
            return _httpContextAccesor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "JWT")?.Value;
        }

        //private readonly IAspNetUser _user;

        //public HttpClientAuthorizationDelegatingHandler(IAspNetUser user)
        //{
        //    _user = user;
        //}

        //protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    var authorizationHeader = _user.ObterHttpContext().Request.Headers["Authorization"];

        //    if (!string.IsNullOrEmpty(authorizationHeader))
        //    {
        //        request.Headers.Add("Authorization", new List<string>() { authorizationHeader });
        //    }

        //    var token = _user.ObterUserToken();

        //    if (token != null)
        //    {
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    }

        //    return base.SendAsync(request, cancellationToken);
        //}
    }
}