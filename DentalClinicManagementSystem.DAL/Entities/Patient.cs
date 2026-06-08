using DentalClinicManagementSystem.DAL.Enums;

namespace DentalClinicManagementSystem.DAL.Entities;

public class Patient
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Address { get; set; }
    public string? MedicalNotes { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }

    // Authentication
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? Email { get; set; }

    // Navigation
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
}