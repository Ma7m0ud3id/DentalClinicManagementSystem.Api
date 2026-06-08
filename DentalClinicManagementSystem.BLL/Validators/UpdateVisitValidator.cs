using DentalClinicManagementSystem.BLL.DTOs.Visits;
using FluentValidation;

namespace DentalClinicManagementSystem.BLL.Validators
{
    public class UpdateVisitValidator : AbstractValidator<UpdateVisitDto>
    {
        public UpdateVisitValidator()
        {
            RuleFor(x => x.Diagnosis)
                .NotEmpty().WithMessage("Diagnosis is required")
                .MaximumLength(500).WithMessage("Diagnosis cannot exceed 500 characters");

            RuleFor(x => x.Treatment)
                .NotEmpty().WithMessage("Treatment is required")
                .MaximumLength(500).WithMessage("Treatment cannot exceed 500 characters");

            RuleFor(x => x.DoctorNotes)
                .MaximumLength(1000).WithMessage("Doctor notes cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.DoctorNotes));

            RuleFor(x => x.VisitDate)
                .NotEmpty().WithMessage("Visit date is required");
        }
    }
}