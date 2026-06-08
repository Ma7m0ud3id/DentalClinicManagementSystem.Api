using DentalClinicManagementSystem.BLL.DTOs.Users;

namespace DentalClinicManagementSystem.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync(int? role, bool? isActive);
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(CreateUserDto dto);
        Task<UserDto> UpdateAsync(int id, UpdateUserDto dto);
        Task<UserDto> ToggleStatusAsync(int id, int currentUserId);
        Task ChangePasswordAsync(int id, ChangePasswordDto dto);
    }
}