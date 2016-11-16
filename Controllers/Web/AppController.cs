using Microsoft.AspNetCore.Mvc;

namespace TripPlanner.Controllers.Web
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}