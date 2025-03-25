using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
