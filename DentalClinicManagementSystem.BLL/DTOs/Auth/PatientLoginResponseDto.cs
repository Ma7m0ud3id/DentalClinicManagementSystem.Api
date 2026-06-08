namespace DentalClinicManagementSystem.BLL.DTOs.Auth
{
    public class PatientLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int PatientId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string UserType { get; set; } = "Patient";
    }
}