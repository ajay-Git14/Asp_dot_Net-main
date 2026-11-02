using LiveClassDemos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LiveClassDemos.Controllers
{
    public class StudentController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public StudentController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }


            Response.Cookies.Append("StudentAge", student.Age.ToString());


            HttpContext.Session.SetString("StudentEmail", student.Email ?? string.Empty);
            HttpContext.Session.SetString("StudentName", student.Name);

            return RedirectToAction("Dashboard");
            //return View("Success", student);
        }

        [HttpGet]
        public IActionResult CreateManual()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Dashboard()
        {

            ViewBag.StudentName = Request.Cookies["StudentAge"] ?? "0";
            ViewBag.StudentEmail = HttpContext.Session.GetString("StudentEmail");

            ViewData["StudentCourse"] = HttpContext.Session.GetString("StudentName");
            return View();
        }


        [HttpGet]
        public IActionResult ClearSate()
        {

            
            ViewBag.StudentEmail = HttpContext.Session.GetString("StudentEmail");

             HttpContext.Session.Remove("StudentName");
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> CreateManual(IFormCollection form)
        {
            IFormFile? imageFile = form.Files["ImageFile"];

            string? savedImagePath = await SaveFileAsync(imageFile);

            Student student = new Student
            {
                Email = form["Email"].ToString(),
                Name = form["Name"].ToString(),
                Course = form["Course"].ToString(),
                Age = Convert.ToInt32(form["Age"].ToString()),
                ImagePath = savedImagePath
            };
            ViewBag.StudentName = student.Name;
            ViewBag.StudentEmail = student.Email;

            ViewData["StudentCourse"] = student.Course;



           

            return View("ManualSuccess", student);
        }

        private async Task<string?> SaveFileAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string extension = Path.GetExtension(file.FileName);
            string fileName = Guid.NewGuid().ToString() + extension;
            string filePath = Path.Combine(uploadsFolder, fileName);

            using FileStream stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            return "/uploads/" + fileName;
        }
    }
}