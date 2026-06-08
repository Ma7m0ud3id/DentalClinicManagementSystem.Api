namespace DentalClinicManagementSystem.BLL.DTOs.Doctors
{
    public class CreateDoctorDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
    }
}