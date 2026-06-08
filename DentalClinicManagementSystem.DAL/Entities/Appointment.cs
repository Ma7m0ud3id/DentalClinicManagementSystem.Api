namespace DentalClinicManagementSystem.DAL.Entities;

using DentalClinicManagementSystem.DAL.Enums;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public AppointmentStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public User Doctor { get; set; } = null!;
    public Visit? Visit { get; set; }
}