namespace DentalClinicManagementSystem.BLL.DTOs.Patients
{
    public class PatientSearchDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}