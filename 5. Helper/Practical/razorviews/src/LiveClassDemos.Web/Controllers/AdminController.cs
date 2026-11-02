using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult DashBoard()
        {
            return View();
        }
    }
}
