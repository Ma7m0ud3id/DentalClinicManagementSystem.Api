using DentalClinicManagementSystem.BLL.DTOs.Users;
using FluentValidation;

namespace DentalClinicManagementSystem.BLL.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\d{11}$").WithMessage("Phone number must be 11 digits");

            RuleFor(x => x.Specialization)
                .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Specialization));
        }
    }
}