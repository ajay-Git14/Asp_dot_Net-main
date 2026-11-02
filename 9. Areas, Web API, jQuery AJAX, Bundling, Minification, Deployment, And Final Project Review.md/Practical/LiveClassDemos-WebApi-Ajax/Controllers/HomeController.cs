using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "AjaxDemo");
    public IActionResult Error() => View();
}
