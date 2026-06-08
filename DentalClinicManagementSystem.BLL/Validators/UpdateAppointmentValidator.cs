using DentalClinicManagementSystem.BLL.DTOs;
using DentalClinicManagementSystem.BLL.DTOs.Appointments;
using FluentValidation;

namespace DentalClinicManagementSystem.BLL.Validators
{
    public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentDto>
    {
        public UpdateAppointmentValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("Doctor ID must be greater than 0");

            RuleFor(x => x.AppointmentDate)
                .NotEmpty().WithMessage("Appointment date is required");

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("Duration must be greater than 0 minutes");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));
        }
    }
}