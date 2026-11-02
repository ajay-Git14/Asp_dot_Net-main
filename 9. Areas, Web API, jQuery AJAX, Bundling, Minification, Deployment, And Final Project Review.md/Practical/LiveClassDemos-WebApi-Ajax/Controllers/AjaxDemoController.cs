using Microsoft.AspNetCore.Mvc;

namespace LiveClassDemos.Controllers;

public class AjaxDemoController : Controller
{
    public IActionResult Index() => View();
}
