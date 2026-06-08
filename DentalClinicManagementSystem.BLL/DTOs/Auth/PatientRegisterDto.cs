namespace DentalClinicManagementSystem.BLL.DTOs.Auth
{
    public class PatientRegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string? Address { get; set; }
        public string? MedicalNotes { get; set; }
    }
}