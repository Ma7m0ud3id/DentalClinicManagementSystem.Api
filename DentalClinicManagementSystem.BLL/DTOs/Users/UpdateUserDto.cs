namespace DentalClinicManagementSystem.BLL.DTOs.Users
{
    public class UpdateUserDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Specialization { get; set; }
        public bool IsActive { get; set; }
    }
}