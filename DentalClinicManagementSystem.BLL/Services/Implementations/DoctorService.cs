using BCrypt.Net;
using DentalClinicManagementSystem.BLL.DTOs.Doctors;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using DentalClinicManagementSystem.DAL.Data;
using DentalClinicManagementSystem.DAL.Entities;
using DentalClinicManagementSystem.DAL.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicManagementSystem.BLL.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext _context;
        private readonly IValidator<CreateDoctorDto> _createValidator;
        private readonly IValidator<UpdateDoctorDto> _updateValidator;

        public DoctorService(
            AppDbContext context,
            IValidator<CreateDoctorDto> createValidator,
            IValidator<UpdateDoctorDto> updateValidator)
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            var doctors = await _context.Users
                .Where(u => u.Role == UserRole.Doctor)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return doctors.Select(MapToDto);
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            var doctor = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.Role == UserRole.Doctor);

            return doctor == null ? null : MapToDto(doctor);
        }

        public async Task<DoctorDto> CreateAsync(CreateDoctorDto dto)
        {
            // Validate
            await _createValidator.ValidateAndThrowAsync(dto);

            // Check if username exists in any user
            var usernameExists = await _context.Users
                .AnyAsync(u => u.Username == dto.Username);

            if (usernameExists)
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Check if phone exists in any user
            var phoneExists = await _context.Users
                .AnyAsync(u => u.Phone == dto.Phone);

            if (phoneExists)
            {
                throw new InvalidOperationException("Phone already exists");
            }

            // Create new doctor user
            var newDoctor = new User
            {
                FullName = dto.FullName,
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone,
                Specialization = dto.Specialization,
                Role = UserRole.Doctor,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newDoctor);
            await _context.SaveChangesAsync();

            return MapToDto(newDoctor);
        }

        public async Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            // Validate
            await _updateValidator.ValidateAndThrowAsync(dto);

            // Find existing doctor
            var doctor = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.Role == UserRole.Doctor);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"Doctor with ID {id} not found");
            }

            // Check phone uniqueness if phone changed
            if (doctor.Phone != dto.Phone)
            {
                var phoneExists = await _context.Users
                    .AnyAsync(u => u.Phone == dto.Phone && u.Id != id);

                if (phoneExists)
                {
                    throw new InvalidOperationException("Phone already exists");
                }
            }

            // Update fields
            doctor.FullName = dto.FullName;
            doctor.Phone = dto.Phone;
            doctor.Specialization = dto.Specialization;
            doctor.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return MapToDto(doctor);
        }

        public async Task<DoctorDto> ToggleStatusAsync(int id)
        {
            // Find existing doctor
            var doctor = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.Role == UserRole.Doctor);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"Doctor with ID {id} not found");
            }

            // Toggle status
            doctor.IsActive = !doctor.IsActive;

            await _context.SaveChangesAsync();

            return MapToDto(doctor);
        }

        private static DoctorDto MapToDto(User user)
        {
            return new DoctorDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.Username,
                Phone = user.Phone,
                Specialization = user.Specialization ?? string.Empty,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}