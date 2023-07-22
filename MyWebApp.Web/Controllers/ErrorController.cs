using Microsoft.AspNetCore.Mvc;

namespace MyWebApp.Web.Controllers
{
    [Route("ErrorPage/{StatusCode}")]
    public class ErrorController : Controller
    {
        public IActionResult Index(int StatusCode)
        {
            switch (StatusCode)
            {
                case 404:
                    ViewData["Error"] = "Page Not Found";
                    break;
                default:
                    break;
            }
            return View("ErrorPage");
        }
    }
}
