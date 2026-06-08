using DentalClinicManagementSystem.BLL.DTOs.Dashboard;

namespace DentalClinicManagementSystem.BLL.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetStatsAsync(int? currentUserId, string? currentUserRole);
    }
}