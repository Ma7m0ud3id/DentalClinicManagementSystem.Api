namespace DentalClinicManagementSystem.DAL.Entities;

public class Visit
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public string Diagnosis { get; set; } = null!;
    public string Treatment { get; set; } = null!;
    public string? DoctorNotes { get; set; }
    public DateTime VisitDate { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Appointment Appointment { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public User Doctor { get; set; } = null!;
}