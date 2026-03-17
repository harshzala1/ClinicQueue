using ClinicQueue.Models;

namespace ClinicQueue.ViewModels
{
    public class AdminDashboardViewModel
    {
        public ClinicInfo? Clinic { get; set; }
        public List<User> Users { get; set; } = new();
        public string? SearchQuery { get; set; }
    }
}
