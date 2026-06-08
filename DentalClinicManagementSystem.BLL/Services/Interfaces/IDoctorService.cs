using DentalClinicManagementSystem.BLL.DTOs.Doctors;

namespace DentalClinicManagementSystem.BLL.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllAsync();
        Task<DoctorDto?> GetByIdAsync(int id);
        Task<DoctorDto> CreateAsync(CreateDoctorDto dto);
        Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto);
        Task<DoctorDto> ToggleStatusAsync(int id);
    }
}