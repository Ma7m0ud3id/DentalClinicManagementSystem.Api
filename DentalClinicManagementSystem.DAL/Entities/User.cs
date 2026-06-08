namespace DentalClinicManagementSystem.DAL.Entities;

using DentalClinicManagementSystem.DAL.Enums;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public UserRole Role { get; set; }
    public string? Specialization { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public ICollection<Appointment> DoctorAppointments { get; set; } = new List<Appointment>();
    public ICollection<Visit> DoctorVisits { get; set; } = new List<Visit>();
}