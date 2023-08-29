using System.Security.Claims;

namespace GameZone.News.WebApp.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            try
            {
                if (principal == null)
                {
                    throw new ArgumentException(nameof(principal));
                }

                var claim = principal.FindFirst(ClaimTypes.NameIdentifier);

                if (claim == null)
                    claim = principal.FindFirst("sub");

                return claim?.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            try
            {
                if (principal == null)
                {
                    throw new ArgumentException(nameof(principal));
                }

                var claim = principal.FindFirst("email");
                return claim?.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetUserToken(this ClaimsPrincipal principal)
        {
            try
            {
                if (principal == null)
                {
                    throw new ArgumentException(nameof(principal));
                }

                var claim = principal.FindFirst("JWT");
                return claim?.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetUserRefreshToken(this ClaimsPrincipal principal)
        {
            try
            {
                if (principal == null)
                {
                    throw new ArgumentException(nameof(principal));
                }

                var claim = principal.FindFirst("RefreshToken");
                return claim?.Value;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}