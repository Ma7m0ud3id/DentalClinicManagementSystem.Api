namespace DentalClinicManagementSystem.BLL.DTOs.Visits
{
    public class CreateVisitDto
    {
        public int AppointmentId { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Treatment { get; set; } = string.Empty;
        public string? DoctorNotes { get; set; }
        public DateTime VisitDate { get; set; }
    }
}