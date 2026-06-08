namespace DentalClinicManagementSystem.BLL.DTOs.Visits
{
    public class VisitDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public string Treatment { get; set; } = string.Empty;
        public string? DoctorNotes { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}