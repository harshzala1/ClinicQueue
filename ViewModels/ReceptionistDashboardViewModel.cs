using ClinicQueue.Models;

namespace ClinicQueue.ViewModels
{
    public class ReceptionistDashboardViewModel
    {
        public List<QueueEntry> QueueEntries { get; set; } = new();
        public string SelectedDate { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
    }
}
