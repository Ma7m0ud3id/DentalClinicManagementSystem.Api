using DentalClinicManagementSystem.BLL.DTOs.Auth;
using FluentValidation;

namespace DentalClinicManagementSystem.BLL.Validators
{
    public class PatientRegisterValidator : AbstractValidator<PatientRegisterDto>
    {
        public PatientRegisterValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\d{11}$").WithMessage("Phone number must be 11 digits");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Gender)
                .InclusiveBetween(1, 2).WithMessage("Gender must be 1 (Male) or 2 (Female)");

            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.MedicalNotes)
                .MaximumLength(1000).WithMessage("Medical notes cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.MedicalNotes));
        }
    }
}