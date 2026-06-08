namespace DentalClinicManagementSystem.BLL.DTOs.Users
{
    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int Role { get; set; }
        public string? Specialization { get; set; }
    }
}