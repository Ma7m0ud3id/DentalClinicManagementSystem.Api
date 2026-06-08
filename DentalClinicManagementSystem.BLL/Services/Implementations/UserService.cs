using BCrypt.Net;
using DentalClinicManagementSystem.BLL.DTOs.Users;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using DentalClinicManagementSystem.DAL.Data;
using DentalClinicManagementSystem.DAL.Entities;
using DentalClinicManagementSystem.DAL.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicManagementSystem.BLL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IValidator<CreateUserDto> _createValidator;
        private readonly IValidator<UpdateUserDto> _updateValidator;
        private readonly IValidator<ChangePasswordDto> _changePasswordValidator;

        public UserService(
            AppDbContext context,
            IValidator<CreateUserDto> createValidator,
            IValidator<UpdateUserDto> updateValidator,
            IValidator<ChangePasswordDto> changePasswordValidator)
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _changePasswordValidator = changePasswordValidator;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(int? role, bool? isActive)
        {
            var query = _context.Users.AsQueryable();

            if (role.HasValue)
            {
                query = query.Where(u => (int)u.Role == role.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(u => u.IsActive == isActive.Value);
            }

            var users = await query
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return users.Select(MapToDto);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user == null ? null : MapToDto(user);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            // Validate
            await _createValidator.ValidateAndThrowAsync(dto);

            // Check username uniqueness
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Check phone uniqueness
            if (await _context.Users.AnyAsync(u => u.Phone == dto.Phone))
            {
                throw new InvalidOperationException("Phone already exists");
            }

            // Hash password
            //var hashedPassword = BCrypt.HashPassword(dto.Password);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Create user
            var newUser = new User
            {
                Username = dto.Username,
                PasswordHash = hashedPassword,
                FullName = dto.FullName,
                Phone = dto.Phone,
                Role = (UserRole)dto.Role,
                Specialization = dto.Role == 2 ? dto.Specialization : null,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return MapToDto(newUser);
        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
        {
            // Validate
            await _updateValidator.ValidateAndThrowAsync(dto);

            // Find user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Check phone uniqueness if changed
            if (user.Phone != dto.Phone)
            {
                if (await _context.Users.AnyAsync(u => u.Phone == dto.Phone && u.Id != id))
                {
                    throw new InvalidOperationException("Phone already exists");
                }
            }

            // Update fields
            user.FullName = dto.FullName;
            user.Phone = dto.Phone;
            user.IsActive = dto.IsActive;

            // Update specialization only for doctors
            if (user.Role == UserRole.Doctor)
            {
                user.Specialization = dto.Specialization;
            }

            await _context.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task<UserDto> ToggleStatusAsync(int id, int currentUserId)
        {
            // Find user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Prevent deactivating own account
            if (id == currentUserId)
            {
                throw new InvalidOperationException("You cannot deactivate your own account");
            }

            // Toggle status
            user.IsActive = !user.IsActive;

            await _context.SaveChangesAsync();

            return MapToDto(user);
        }

        public async Task ChangePasswordAsync(int id, ChangePasswordDto dto)
        {
            // Validate
            await _changePasswordValidator.ValidateAndThrowAsync(dto);

            // Find user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Hash and update password
            //user.PasswordHash = BCrypt.HashPassword(dto.NewPassword);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _context.SaveChangesAsync();
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Phone = user.Phone,
                Role = user.Role.ToString(),
                Specialization = user.Specialization,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}