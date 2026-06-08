using DentalClinicManagementSystem.BLL.DTOs.Visits;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using DentalClinicManagementSystem.DAL.Data;
using DentalClinicManagementSystem.DAL.Entities;
using DentalClinicManagementSystem.DAL.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicManagementSystem.BLL.Services.Implementations
{
    public class VisitService : IVisitService
    {
        private readonly AppDbContext _context;
        private readonly IValidator<CreateVisitDto> _createValidator;
        private readonly IValidator<UpdateVisitDto> _updateValidator;

        public VisitService(
            AppDbContext context,
            IValidator<CreateVisitDto> createValidator,
            IValidator<UpdateVisitDto> updateValidator)
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IEnumerable<VisitDto>> GetAllAsync(int? currentUserId, string? currentUserRole)
        {
            var query = _context.Visits
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .AsQueryable();

            if (IsDoctor(currentUserRole) && currentUserId.HasValue)
            {
                query = query.Where(v => v.DoctorId == currentUserId.Value);
            }

            var visits = await query
                .OrderByDescending(v => v.VisitDate)
                .ToListAsync();

            return visits.Select(MapToDto);
        }

        public async Task<VisitDto?> GetByIdAsync(int id, int? currentUserId, string? currentUserRole)
        {
            var visit = await _context.Visits
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visit == null)
            {
                return null;
            }

            if (IsDoctor(currentUserRole) && visit.DoctorId != currentUserId)
            {
                throw new UnauthorizedAccessException("Access denied");
            }

            return MapToDto(visit);
        }

        public async Task<IEnumerable<VisitDto>> GetByPatientIdAsync(int patientId, int? currentUserId, string? currentUserRole)
        {
            var query = _context.Visits
                .Where(v => v.PatientId == patientId)
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .AsQueryable();

            if (IsDoctor(currentUserRole) && currentUserId.HasValue)
            {
                query = query.Where(v => v.DoctorId == currentUserId.Value);
            }

            var visits = await query
                .OrderByDescending(v => v.VisitDate)
                .ToListAsync();

            return visits.Select(MapToDto);
        }

        public async Task<VisitDto> CreateAsync(CreateVisitDto dto, int? currentUserId, string? currentUserRole)
        {
            // Validate
            await _createValidator.ValidateAndThrowAsync(dto);

            // Find appointment
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == dto.AppointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found");
            }

            // Check if appointment is cancelled
            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot create visit for cancelled appointment");
            }

            // Check if visit already exists for this appointment
            var visitExists = await _context.Visits
                .AnyAsync(v => v.AppointmentId == dto.AppointmentId);

            if (visitExists)
            {
                throw new InvalidOperationException("Visit already exists for this appointment");
            }

            // Check doctor authorization
            if (IsDoctor(currentUserRole) && appointment.DoctorId != currentUserId)
            {
                throw new UnauthorizedAccessException("You can only create visits for your own appointments");
            }

            // Create visit (derive PatientId and DoctorId from appointment)
            var newVisit = new Visit
            {
                AppointmentId = appointment.Id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                Diagnosis = dto.Diagnosis,
                Treatment = dto.Treatment,
                DoctorNotes = dto.DoctorNotes,
                VisitDate = dto.VisitDate,
                CreatedAt = DateTime.UtcNow
            };

            _context.Visits.Add(newVisit);

            // Auto-complete the appointment
            appointment.Status = AppointmentStatus.Completed;

            await _context.SaveChangesAsync();

            // Load related entities for mapping
            await _context.Entry(newVisit).Reference(v => v.Patient).LoadAsync();
            await _context.Entry(newVisit).Reference(v => v.Doctor).LoadAsync();

            return MapToDto(newVisit);
        }

        public async Task<VisitDto> UpdateAsync(int id, UpdateVisitDto dto, int? currentUserId, string? currentUserRole)
        {
            // Validate
            await _updateValidator.ValidateAndThrowAsync(dto);

            // Find visit
            var visit = await _context.Visits
                .Include(v => v.Patient)
                .Include(v => v.Doctor)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visit == null)
            {
                throw new KeyNotFoundException("Visit not found");
            }

            // Check doctor authorization
            if (IsDoctor(currentUserRole) && visit.DoctorId != currentUserId)
            {
                throw new UnauthorizedAccessException("You can only update your own visits");
            }

            // Update fields
            visit.Diagnosis = dto.Diagnosis;
            visit.Treatment = dto.Treatment;
            visit.DoctorNotes = dto.DoctorNotes;
            visit.VisitDate = dto.VisitDate;

            await _context.SaveChangesAsync();

            return MapToDto(visit);
        }

        private static bool IsDoctor(string? role) => role == "Doctor";

        private static VisitDto MapToDto(Visit visit)
        {
            return new VisitDto
            {
                Id = visit.Id,
                AppointmentId = visit.AppointmentId,
                PatientId = visit.PatientId,
                PatientName = visit.Patient?.FullName ?? string.Empty,
                DoctorId = visit.DoctorId,
                DoctorName = visit.Doctor?.FullName ?? string.Empty,
                Diagnosis = visit.Diagnosis,
                Treatment = visit.Treatment,
                DoctorNotes = visit.DoctorNotes,
                VisitDate = visit.VisitDate,
                CreatedAt = visit.CreatedAt
            };
        }
    }
}