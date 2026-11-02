using SmartSocietyManagment.Models;
using SmartSocietyManagment.ViewModels;

namespace SmartSocietyManagment.Interface
{
    
        public interface IComplaintService
        {
            Task<List<Complaint>> GetAllComplaintsAsync();

            Task<Complaint?> GetComplaintByIdAsync(int id);

            Task CreateComplaintAsync(ComplaintCreateViewModel model);
        }
    
}
