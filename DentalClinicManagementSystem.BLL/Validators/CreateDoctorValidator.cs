using DentalClinicManagementSystem.BLL.DTOs.Doctors;
using FluentValidation;

namespace DentalClinicManagementSystem.BLL.Validators
{
    public class CreateDoctorValidator : AbstractValidator<CreateDoctorDto>
    {
        public CreateDoctorValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^01[0-2,5]{1}[0-9]{8}$")
                .WithMessage("Phone number must be in Egyptian format (e.g., 01012345678)");

            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Specialization is required")
                .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters");
        }
    }
}