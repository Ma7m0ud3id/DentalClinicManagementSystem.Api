namespace DentalClinicManagementSystem.BLL.Services.Interfaces;

using DentalClinicManagementSystem.BLL.DTOs.Auth;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
    Task<CurrentUserDto?> GetCurrentUserAsync(int userId);
}