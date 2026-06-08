using BCrypt.Net;
using DentalClinicManagementSystem.BLL.DTOs.Patients;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using DentalClinicManagementSystem.DAL.Data;
using DentalClinicManagementSystem.DAL.Entities;
using DentalClinicManagementSystem.DAL.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicManagementSystem.BLL.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly AppDbContext _context;
        private readonly IValidator<CreatePatientDto> _createValidator;
        private readonly IValidator<UpdatePatientDto> _updateValidator;

        public PatientService(
            AppDbContext context,
            IValidator<CreatePatientDto> createValidator,
            IValidator<UpdatePatientDto> updateValidator)
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IEnumerable<PatientSearchDto>> GetAllAsync(string? search = null)
        {
            var query = _context.Patients
                .Where(p => !p.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.FullName.Contains(search) || p.Phone.Contains(search));
            }

            var patients = await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return patients.Select(MapToSearchDto);
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            return patient == null ? null : MapToDto(patient);
        }

        public async Task<IEnumerable<PatientSearchDto>> GetByPatientIdAsync(int patientId, int? currentUserId, string? currentUserRole)
        {
            var query = _context.Patients
                .Where(p => p.Id == patientId && !p.IsDeleted)
                .AsQueryable();

            var patients = await query.ToListAsync();
            return patients.Select(MapToSearchDto);
        }

        public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
        {
            // Validate
            await _createValidator.ValidateAndThrowAsync(dto);

            // Check phone uniqueness
            var phoneExists = await _context.Patients
                .AnyAsync(p => p.Phone == dto.Phone && !p.IsDeleted);

            if (phoneExists)
            {
                throw new InvalidOperationException("Phone already exists");
            }

            // If username is provided, check uniqueness
            if (!string.IsNullOrEmpty(dto.Username))
            {
                var usernameExistsInPatients = await _context.Patients
                    .AnyAsync(p => p.Username == dto.Username);

                var usernameExistsInUsers = await _context.Users
                    .AnyAsync(u => u.Username == dto.Username);

                if (usernameExistsInPatients || usernameExistsInUsers)
                {
                    throw new InvalidOperationException("Username already exists");
                }
            }

            // Create new patient
            var newPatient = new Patient
            {
                FullName = dto.FullName,
                Phone = dto.Phone,
                DateOfBirth = dto.DateOfBirth,
                Gender = (Gender)dto.Gender,
                Address = dto.Address,
                MedicalNotes = dto.MedicalNotes,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            // Add authentication if provided
            if (!string.IsNullOrEmpty(dto.Username) && !string.IsNullOrEmpty(dto.Password))
            {
                newPatient.Username = dto.Username;
                newPatient.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                newPatient.Email = dto.Email;
            }

            _context.Patients.Add(newPatient);
            await _context.SaveChangesAsync();

            return MapToDto(newPatient);
        }

        public async Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto)
        {
            // Validate
            await _updateValidator.ValidateAndThrowAsync(dto);

            // Find existing patient
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (patient == null)
            {
                throw new KeyNotFoundException($"Patient with ID {id} not found");
            }

            // Check phone uniqueness if phone changed
            if (patient.Phone != dto.Phone)
            {
                var phoneExists = await _context.Patients
                    .AnyAsync(p => p.Phone == dto.Phone && !p.IsDeleted && p.Id != id);

                if (phoneExists)
                {
                    throw new InvalidOperationException("Phone already exists");
                }
            }

            // Update fields
            patient.FullName = dto.FullName;
            patient.Phone = dto.Phone;
            patient.DateOfBirth = dto.DateOfBirth;
            patient.Gender = (Gender)dto.Gender;
            patient.Address = dto.Address;
            patient.MedicalNotes = dto.MedicalNotes;

            await _context.SaveChangesAsync();

            return MapToDto(patient);
        }

        public async Task SoftDeleteAsync(int id)
        {
            // Find existing patient
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (patient == null)
            {
                throw new KeyNotFoundException($"Patient with ID {id} not found");
            }

            // Soft delete
            patient.IsDeleted = true;

            await _context.SaveChangesAsync();
        }

        private static PatientDto MapToDto(Patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id,
                FullName = patient.FullName,
                Phone = patient.Phone,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender.ToString(),
                Address = patient.Address,
                MedicalNotes = patient.MedicalNotes,
                CreatedAt = patient.CreatedAt
            };
        }

        private static PatientSearchDto MapToSearchDto(Patient patient)
        {
            return new PatientSearchDto
            {
                Id = patient.Id,
                FullName = patient.FullName,
                Phone = patient.Phone,
                CreatedAt = patient.CreatedAt
            };
        }
    }
}