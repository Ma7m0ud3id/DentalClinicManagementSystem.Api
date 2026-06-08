using DentalClinicManagementSystem.BLL.DTOs.Doctors;
using FluentValidation;

namespace DentalClinicManagementSystem.BLL.Validators
{
    public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorDto>
    {
        public UpdateDoctorValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

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