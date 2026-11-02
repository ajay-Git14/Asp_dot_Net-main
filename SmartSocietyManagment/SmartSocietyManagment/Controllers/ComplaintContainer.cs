using SmartSocietyManagment.Interface;
using Microsoft.AspNetCore.Mvc;

namespace SmartSocietyManagment.Controllers
{
        public class ComplaintsController : Controller
        {
            private readonly IComplaintService _complaintService;

            public ComplaintsController(IComplaintService complaintService)
            {
                _complaintService = complaintService;
            }

            public async Task<IActionResult> Index()
            {
                var complaints = await _complaintService.GetAllComplaintsAsync();

                return View(complaints);
            }

            public async Task<IActionResult> Details(int id)
            {
                var complaint = await _complaintService.GetComplaintByIdAsync(id);

                if (complaint == null)
                {
                    return NotFound();
                }

                return View(complaint);
            }

            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }
        }
    
}
