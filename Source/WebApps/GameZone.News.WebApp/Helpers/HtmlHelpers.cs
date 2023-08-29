using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.News.WebApp
{
    public static class HtmlHelpers
    {

        public static string IsSelected(this IHtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {
            try
            {
                if (String.IsNullOrEmpty(cssClass))
                    cssClass = "active";

                string currentAction = (string)html.ViewContext.RouteData.Values["action"];
                string currentController = (string)html.ViewContext.RouteData.Values["controller"];

                if (String.IsNullOrEmpty(controller))
                    controller = currentController;

                if (String.IsNullOrEmpty(action))
                    action = currentAction;

                return controller == currentController && action == currentAction ?
                    cssClass : String.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string PageClass(this IHtmlHelper htmlHelper)
        {
            try
            {
                string currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"];
                return currentAction;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
