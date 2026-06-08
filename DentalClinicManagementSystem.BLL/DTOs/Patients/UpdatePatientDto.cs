namespace DentalClinicManagementSystem.BLL.DTOs.Patients
{
    public class UpdatePatientDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string? Address { get; set; }
        public string? MedicalNotes { get; set; }
    }
}