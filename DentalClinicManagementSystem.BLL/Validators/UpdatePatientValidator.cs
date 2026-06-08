using DentalClinicManagementSystem.BLL.DTOs.Patients;
using FluentValidation;

namespace DentalClinicManagementSystem.BLL.Validators
{
    public class UpdatePatientValidator : AbstractValidator<UpdatePatientDto>
    {
        public UpdatePatientValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\d{11}$").WithMessage("Phone number must be 11 digits");

            RuleFor(x => x.Gender)
                .Must(g => g == 1 || g == 2)
                .WithMessage("Gender must be 1 (Male) or 2 (Female)");

            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.MedicalNotes)
                .MaximumLength(1000).WithMessage("Medical notes cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.MedicalNotes));
        }
    }
}