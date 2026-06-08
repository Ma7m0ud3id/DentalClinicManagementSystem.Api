namespace DentalClinicManagementSystem.BLL.DTOs.Appointments
{
    public class CreateAppointmentDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DurationMinutes { get; set; }
        public string? Notes { get; set; }
    }
}