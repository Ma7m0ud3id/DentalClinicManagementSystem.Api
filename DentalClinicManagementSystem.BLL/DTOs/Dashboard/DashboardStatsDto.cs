namespace DentalClinicManagementSystem.BLL.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TodayAppointments { get; set; }
        public int CompletedVisitsToday { get; set; }
    }
}