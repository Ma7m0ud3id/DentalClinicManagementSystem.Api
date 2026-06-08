namespace DentalClinicManagementSystem.BLL.DTOs.Patients
{
    public class CreatePatientDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string? Address { get; set; }
        public string? MedicalNotes { get; set; }
        
        // Optional authentication
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}