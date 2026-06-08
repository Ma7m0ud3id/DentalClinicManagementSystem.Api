using DentalClinicManagementSystem.BLL.DTOs.Dashboard;
using DentalClinicManagementSystem.BLL.Services.Interfaces;
using DentalClinicManagementSystem.DAL.Data;
using DentalClinicManagementSystem.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace DentalClinicManagementSystem.BLL.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetStatsAsync(int? currentUserId, string? currentUserRole)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            // Total Patients (always global)
            var totalPatients = await _context.Patients
                .CountAsync(p => !p.IsDeleted);

            // Total Doctors (always global)
            var totalDoctors = await _context.Users
                .CountAsync(u => u.Role == UserRole.Doctor && u.IsActive);

            // Today Appointments
            var appointmentsQuery = _context.Appointments
                .Where(a => a.AppointmentDate >= today && a.AppointmentDate < tomorrow)
                .Where(a => a.Status != AppointmentStatus.Cancelled)
                .AsQueryable();

            if (IsDoctor(currentUserRole) && currentUserId.HasValue)
            {
                appointmentsQuery = appointmentsQuery.Where(a => a.DoctorId == currentUserId.Value);
            }

            var todayAppointments = await appointmentsQuery.CountAsync();

            // Completed Visits Today
            var visitsQuery = _context.Visits
                .Where(v => v.VisitDate >= today && v.VisitDate < tomorrow)
                .AsQueryable();

            if (IsDoctor(currentUserRole) && currentUserId.HasValue)
            {
                visitsQuery = visitsQuery.Where(v => v.DoctorId == currentUserId.Value);
            }

            var completedVisitsToday = await visitsQuery.CountAsync();

            return new DashboardStatsDto
            {
                TotalPatients = totalPatients,
                TotalDoctors = totalDoctors,
                TodayAppointments = todayAppointments,
                CompletedVisitsToday = completedVisitsToday
            };
        }

        private static bool IsDoctor(string? role) => role == "Doctor";
    }
}