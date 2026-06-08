using DentalClinicManagementSystem.BLL.DTOs;
using DentalClinicManagementSystem.BLL.DTOs.Appointments;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using DentalClinicManagementSystem.DAL.Data;
using DentalClinicManagementSystem.DAL.Entities;
using DentalClinicManagementSystem.DAL.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicManagementSystem.BLL.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppDbContext _context;
        private readonly IValidator<CreateAppointmentDto> _createValidator;
        private readonly IValidator<UpdateAppointmentDto> _updateValidator;

        public AppointmentService(
            AppDbContext context,
            IValidator<CreateAppointmentDto> createValidator,
            IValidator<UpdateAppointmentDto> updateValidator)
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync(DateTime? date, int? doctorId, int? status, int? currentUserId, string? currentUserRole)
        {
            var query = _context.Appointments.AsQueryable();

            if (date.HasValue)
            {
                query = query.Where(a => a.AppointmentDate.Date == date.Value.Date);
            }

            if (doctorId.HasValue && currentUserRole != "Doctor")
            {
                query = query.Where(a => a.DoctorId == doctorId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(a => (int)a.Status == status.Value);
            }

            if (currentUserRole == "Doctor" && currentUserId.HasValue)
            {
                query = query.Where(a => a.DoctorId == currentUserId.Value);
            }

            var appointments = await query
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();

            return appointments.Select(MapToDto);
        }

        public async Task<IEnumerable<AppointmentDto>> GetTodayAsync(int? currentUserId, string? currentUserRole)
        {
            var today = DateTime.Today;
            return await GetAllAsync(today, null, null, currentUserId, currentUserRole);
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);

            return appointment == null ? null : MapToDto(appointment);
        }

        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
        {
            // Validate
            await _createValidator.ValidateAndThrowAsync(dto);

            // Check if appointment date is in the past
            if (dto.AppointmentDate < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot create appointment in the past");
            }

            // Check doctor exists, is active, and has Doctor role
            var doctor = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == dto.DoctorId && u.Role == UserRole.Doctor && u.IsActive);

            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found or is inactive");
            }

            // Check patient exists and is not deleted
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == dto.PatientId && !p.IsDeleted);

            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found");
            }

            // Set default duration if needed
            var duration = dto.DurationMinutes <= 0 ? 30 : dto.DurationMinutes;

            // Check for overlapping appointments
            var newStart = dto.AppointmentDate;
            var newEnd = newStart.AddMinutes(duration);

            var hasOverlap = await _context.Appointments
                .Where(a => a.DoctorId == dto.DoctorId)
                .Where(a => a.Status != AppointmentStatus.Cancelled)
                .Where(a => a.AppointmentDate < newEnd && a.AppointmentDate.AddMinutes(a.DurationMinutes) > newStart)
                .AnyAsync();

            if (hasOverlap)
            {
                throw new InvalidOperationException("Doctor has overlapping appointment");
            }

            // Create new appointment
            var newAppointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                DurationMinutes = duration,
                Status = AppointmentStatus.Scheduled,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.Appointments.Add(newAppointment);
            await _context.SaveChangesAsync();

            // Reload to get related entities
            await _context.Entry(newAppointment).Reference(a => a.Patient).LoadAsync();
            await _context.Entry(newAppointment).Reference(a => a.Doctor).LoadAsync();

            return MapToDto(newAppointment);
        }

        public async Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            // Validate
            await _updateValidator.ValidateAndThrowAsync(dto);

            // Find existing appointment
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {id} not found");
            }

            // Check if appointment is completed or cancelled
            if (appointment.Status == AppointmentStatus.Completed || appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot modify completed or cancelled appointment");
            }

            // Check if appointment date is in the past
            if (dto.AppointmentDate < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Cannot move appointment to the past");
            }

            // Check doctor exists, is active, and has Doctor role
            var doctor = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == dto.DoctorId && u.Role == UserRole.Doctor && u.IsActive);

            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found or is inactive");
            }

            // Set default duration if needed
            var duration = dto.DurationMinutes <= 0 ? 30 : dto.DurationMinutes;

            // Check for overlapping appointments (excluding current)
            var newStart = dto.AppointmentDate;
            var newEnd = newStart.AddMinutes(duration);

            var hasOverlap = await _context.Appointments
                .Where(a => a.Id != id)
                .Where(a => a.DoctorId == dto.DoctorId)
                .Where(a => a.Status != AppointmentStatus.Cancelled)
                .Where(a => a.AppointmentDate < newEnd && a.AppointmentDate.AddMinutes(a.DurationMinutes) > newStart)
                .AnyAsync();

            if (hasOverlap)
            {
                throw new InvalidOperationException("Doctor has overlapping appointment");
            }

            // Update fields
            appointment.DoctorId = dto.DoctorId;
            appointment.AppointmentDate = dto.AppointmentDate;
            appointment.DurationMinutes = duration;
            appointment.Notes = dto.Notes;

            await _context.SaveChangesAsync();

            // Reload to get latest data
            await _context.Entry(appointment).Reference(a => a.Patient).LoadAsync();
            await _context.Entry(appointment).Reference(a => a.Doctor).LoadAsync();

            return MapToDto(appointment);
        }

        public async Task<AppointmentDto> CancelAsync(int id)
        {
            // Find existing appointment
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {id} not found");
            }

            // Check if already cancelled
            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Already cancelled");
            }

            // Check if completed
            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("Cannot cancel completed appointment");
            }

            // Cancel appointment
            appointment.Status = AppointmentStatus.Cancelled;

            await _context.SaveChangesAsync();

            return MapToDto(appointment);
        }

        public async Task<AppointmentDto> CompleteAsync(int id)
        {
            // Find existing appointment
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {id} not found");
            }

            // Check if already completed
            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("Already completed");
            }

            // Check if cancelled
            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot complete cancelled appointment");
            }

            // Complete appointment
            appointment.Status = AppointmentStatus.Completed;

            await _context.SaveChangesAsync();

            return MapToDto(appointment);
        }

        private static AppointmentDto MapToDto(Appointment appointment)
        {
            return new AppointmentDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient?.FullName ?? string.Empty,
                DoctorId = appointment.DoctorId,
                DoctorName = appointment.Doctor?.FullName ?? string.Empty,
                AppointmentDate = appointment.AppointmentDate,
                DurationMinutes = appointment.DurationMinutes,
                Status = appointment.Status.ToString(),
                Notes = appointment.Notes,
                CreatedAt = appointment.CreatedAt
            };
        }
    }
}