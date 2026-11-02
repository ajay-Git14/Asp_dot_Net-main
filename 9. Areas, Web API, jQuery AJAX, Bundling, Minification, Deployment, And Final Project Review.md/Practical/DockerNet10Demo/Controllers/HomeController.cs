using System.Diagnostics;
using DockerNet10Demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace DockerNet10Demo.Controllers;

public class HomeController : Controller
{
    private readonly IWebHostEnvironment _environment;

    public HomeController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public IActionResult Index()
    {
        ViewBag.EnvironmentName = _environment.EnvironmentName;
        ViewBag.MachineName = Environment.MachineName;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
