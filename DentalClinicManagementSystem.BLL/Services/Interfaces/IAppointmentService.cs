using DentalClinicManagementSystem.BLL.DTOs;
using DentalClinicManagementSystem.BLL.DTOs.Appointments;

namespace DentalClinicManagementSystem.BLL.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAsync(DateTime? date, int? doctorId, int? status, int? currentUserId, string? currentUserRole);
        Task<IEnumerable<AppointmentDto>> GetTodayAsync(int? currentUserId, string? currentUserRole);
        Task<AppointmentDto?> GetByIdAsync(int id);
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
        Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto);
        Task<AppointmentDto> CancelAsync(int id);
        Task<AppointmentDto> CompleteAsync(int id);
    }
}