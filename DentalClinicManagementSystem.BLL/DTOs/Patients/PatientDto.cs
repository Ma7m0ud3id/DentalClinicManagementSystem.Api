namespace DentalClinicManagementSystem.BLL.DTOs.Patients
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? MedicalNotes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}