using DentalClinicManagementSystem.BLL.DTOs.Auth;

namespace DentalClinicManagementSystem.BLL.Services.Interfaces
{
    public interface IPatientAuthService
    {
        Task<PatientLoginResponseDto> RegisterAsync(PatientRegisterDto dto);
        Task<PatientLoginResponseDto> LoginAsync(PatientLoginDto dto);
    }
}