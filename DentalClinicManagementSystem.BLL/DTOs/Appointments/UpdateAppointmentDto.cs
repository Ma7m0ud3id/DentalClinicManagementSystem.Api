namespace DentalClinicManagementSystem.BLL.DTOs
{
    public class UpdateAppointmentDto
    {
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; }
        public string? Notes { get; set; }
    }
}