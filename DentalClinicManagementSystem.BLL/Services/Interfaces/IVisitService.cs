using DentalClinicManagementSystem.BLL.DTOs.Visits;

namespace DentalClinicManagementSystem.BLL.Services.Interfaces
{
    public interface IVisitService
    {
        Task<IEnumerable<VisitDto>> GetAllAsync(int? currentUserId, string? currentUserRole);
        Task<VisitDto?> GetByIdAsync(int id, int? currentUserId, string? currentUserRole);
        Task<IEnumerable<VisitDto>> GetByPatientIdAsync(int patientId, int? currentUserId, string? currentUserRole);
        Task<VisitDto> CreateAsync(CreateVisitDto dto, int? currentUserId, string? currentUserRole);
        Task<VisitDto> UpdateAsync(int id, UpdateVisitDto dto, int? currentUserId, string? currentUserRole);
    }
}