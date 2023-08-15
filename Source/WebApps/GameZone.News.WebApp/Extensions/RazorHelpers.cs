using Microsoft.AspNetCore.Mvc.Razor;

namespace GameZone.News.WebApp.Extensions
{
    public static class RazorHelpers
    {
        public static string FormatoMoeda(this RazorPage page, decimal valor)
        {
            return FormatoMoeda(valor);
        }

        private static string FormatoMoeda(decimal valor)
        {
            return string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", valor);
        }
    }
}