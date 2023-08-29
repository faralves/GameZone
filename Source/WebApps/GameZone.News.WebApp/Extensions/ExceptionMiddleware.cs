using GameZone.News.WebApp.Models.Interfaces;
using Polly.CircuitBreaker;
using Refit;
using System.Net;

namespace GameZone.News.WebApp.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private static IAutenticacaoService _autenticacaoService;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;

            try
            {
                await _next(httpContext);
            }
            catch (CustomHttpRequestException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (ValidationApiException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (ApiException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (BrokenCircuitException)
            {
                HandleCircuitBreakerExceptionAsync(httpContext);
            }
        }

        private static void HandleRequestExceptionAsync(HttpContext context, HttpStatusCode statusCode)
        {
            try
            {
                if (statusCode == HttpStatusCode.Unauthorized)
                {
                    if (_autenticacaoService.TokenExpirado())
                    {
                        if (_autenticacaoService.RefreshTokenValido().Result)
                        {
                            context.Response.Redirect(context.Request.Path);
                            return;
                        }
                    }

                    _autenticacaoService.Logout();
                    context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
                    return;
                }

                context.Response.StatusCode = (int)statusCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void HandleCircuitBreakerExceptionAsync(HttpContext context)
        {
            context.Response.Redirect("/sistema-indisponivel");
        }
    }
}
