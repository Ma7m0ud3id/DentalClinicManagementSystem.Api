using BCrypt.Net;
using DentalClinicManagementSystem.BLL.DTOs.Auth;
using DentalClinicManagementSystem.BLL.Security;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using DentalClinicManagementSystem.DAL.Data;
using DentalClinicManagementSystem.DAL.Entities;
using DentalClinicManagementSystem.DAL.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DentalClinicManagementSystem.BLL.Services.Implementations
{
    public class PatientAuthService : IPatientAuthService
    {
        private readonly AppDbContext _context;
        private readonly IValidator<PatientRegisterDto> _registerValidator;
        private readonly IValidator<PatientLoginDto> _loginValidator;
        private readonly JwtSettings _jwtSettings;

        public PatientAuthService(
            AppDbContext context,
            IValidator<PatientRegisterDto> registerValidator,
            IValidator<PatientLoginDto> loginValidator,
            IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<PatientLoginResponseDto> RegisterAsync(PatientRegisterDto dto)
        {
            // Validate
            await _registerValidator.ValidateAndThrowAsync(dto);

            // Check username uniqueness in Patients
            if (await _context.Patients.AnyAsync(p => p.Username == dto.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Check username uniqueness in Users (staff)
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Look for existing patient by phone
            var existingPatient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Phone == dto.Phone && !p.IsDeleted);

            Patient patientToUse;

            if (existingPatient != null)
            {
                // Existing patient found
                if (!string.IsNullOrEmpty(existingPatient.Username))
                {
                    throw new InvalidOperationException("A patient account already exists for this phone number. Please login instead.");
                }

                // Merge case: Add credentials to existing patient
                existingPatient.Username = dto.Username;
                //existingPatient.PasswordHash = BCrypt.HashPassword(dto.Password);
                existingPatient.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                existingPatient.Email = dto.Email ?? existingPatient.Email;

                if (existingPatient.DateOfBirth == null)
                    existingPatient.DateOfBirth = dto.DateOfBirth;
                if (string.IsNullOrEmpty(existingPatient.Address))
                    existingPatient.Address = dto.Address;
                if (string.IsNullOrEmpty(existingPatient.MedicalNotes))
                    existingPatient.MedicalNotes = dto.MedicalNotes;

                await _context.SaveChangesAsync();
                patientToUse = existingPatient;
            }
            else
            {
                // Create new patient
                var newPatient = new Patient
                {
                    FullName = dto.FullName,
                    Phone = dto.Phone,
                    Email = dto.Email,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = (Gender)dto.Gender,
                    Address = dto.Address,
                    MedicalNotes = dto.MedicalNotes,
                    Username = dto.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Patients.Add(newPatient);
                await _context.SaveChangesAsync();
                patientToUse = newPatient;
            }

            // Generate token
            var token = JwtTokenGenerator.GeneratePatientToken(patientToUse, _jwtSettings);

            return new PatientLoginResponseDto
            {
                Token = token,
                PatientId = patientToUse.Id,
                FullName = patientToUse.FullName,
                Username = patientToUse.Username ?? string.Empty,
                UserType = "Patient"
            };
        }

        public async Task<PatientLoginResponseDto> LoginAsync(PatientLoginDto dto)
        {
            // Validate
            await _loginValidator.ValidateAndThrowAsync(dto);

            // Find patient by username
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Username == dto.Username && !p.IsDeleted);

            if (patient == null || string.IsNullOrEmpty(patient.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            // Verify password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, patient.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            // Generate token
            var token = JwtTokenGenerator.GeneratePatientToken(patient, _jwtSettings);

            return new PatientLoginResponseDto
            {
                Token = token,
                PatientId = patient.Id,
                FullName = patient.FullName,
                Username = patient.Username ?? string.Empty,
                UserType = "Patient"
            };
        }
    }
}