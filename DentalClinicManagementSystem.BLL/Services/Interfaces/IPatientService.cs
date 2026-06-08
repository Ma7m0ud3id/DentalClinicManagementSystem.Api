using DentalClinicManagementSystem.BLL.DTOs.Patients;

namespace DentalClinicManagementSystem.BLL.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientSearchDto>> GetAllAsync(string? search = null);
        Task<PatientDto?> GetByIdAsync(int id);
        Task<PatientDto> CreateAsync(CreatePatientDto dto);
        Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto);
        Task SoftDeleteAsync(int id);
    }
}