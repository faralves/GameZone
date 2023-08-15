using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Blog.API.Authorization
{
    public class EditarNoticiaAuthorization : TypeFilterAttribute
    {
        public EditarNoticiaAuthorization() : base(typeof(PodeEditarNoticiaFilter))
        {
        }

        private class PodeEditarNoticiaFilter : IAsyncAuthorizationFilter
        {
            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.User;
                if (user.Identity?.IsAuthenticated == true && user.HasClaim(c => c.Type == "PodeEditarNoticia" && c.Value == "true"))
                {
                    // A claim "PodeInserirUsuario" com valor "true" está presente no token JWT.
                    // Permite a continuação da requisição.
                }
                else
                {
                    // A claim não está presente ou não possui o valor "true".
                    // Negar o acesso com um erro de autorização.
                    context.Result = new ForbidResult();
                }

                await Task.CompletedTask;
            }
        }
    }
}
