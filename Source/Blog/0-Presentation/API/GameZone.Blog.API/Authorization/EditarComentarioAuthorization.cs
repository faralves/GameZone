using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Blog.API.Authorization
{
    public class EditarComentarioAuthorization : TypeFilterAttribute
    {
        public EditarComentarioAuthorization() : base(typeof(PodeEditarComentarioFilter))
        {
        }

        private class PodeEditarComentarioFilter : IAsyncAuthorizationFilter
        {
            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.User;
                if (user.Identity?.IsAuthenticated == true && user.HasClaim(c => c.Type == "PodeEditarComentario" && c.Value == "true"))
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
