using Microsoft.AspNetCore.Mvc;

namespace GameZone.News.WebApp.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}