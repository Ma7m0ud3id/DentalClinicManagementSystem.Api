namespace DentalClinicManagementSystem.BLL.DTOs.Doctors
{
    public class UpdateDoctorDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}