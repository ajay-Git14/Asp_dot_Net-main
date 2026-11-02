using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LiveClassDemos.Models;

namespace LiveClassDemos.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        List<Complaint> test = new List<Complaint>{ new Complaint
        {
            Id="10",
            Title="Hello",
            Description="This is description",
            Priority="High"
        },
        new Complaint
        {
            Id="10",
            Title="Hello",
            Priority="Low",
            Description="This is description"
        },
        new Complaint
        {
            Id="10",
            Title="Hello",
            Description="This is description",
            Priority="Medium"

        }
        };
        return View(test);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
