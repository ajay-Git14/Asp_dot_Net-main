using SmartSocietyManagment.Interface;
using SmartSocietyManagment.Models;
using SmartSocietyManagment.ViewModels;

namespace SmartSocietyManagment.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly ILogger<ComplaintService> _logger;

        public ComplaintService(ILogger<ComplaintService> logger)
        {
            _logger = logger;
        }

        public Task<List<Complaint>> GetAllComplaintsAsync()
        {
            _logger.LogInformation("Fetching all complaints");

            var complaints = new List<Complaint>();

            return Task.FromResult(complaints);
        }

        public Task<Complaint?> GetComplaintByIdAsync(int id)
        {
            _logger.LogInformation("Fetching complaint details for ComplaintId: {ComplaintId}", id);

            Complaint? complaint = null;

            return Task.FromResult(complaint);
        }

        public Task CreateComplaintAsync(ComplaintCreateViewModel model)
        {
            _logger.LogInformation("Creating complaint with title: {Title}", model.Title);

            return Task.CompletedTask;
        }
    }
}
