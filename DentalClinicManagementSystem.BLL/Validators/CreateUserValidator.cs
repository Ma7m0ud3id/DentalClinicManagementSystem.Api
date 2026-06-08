using DentalClinicManagementSystem.BLL.DTOs.Users;
using FluentValidation;

namespace DentalClinicManagementSystem.BLL.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
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

            RuleFor(x => x.Role)
                .InclusiveBetween(1, 3).WithMessage("Role must be 1 (Admin), 2 (Doctor), or 3 (Receptionist)");

            When(x => x.Role == 2, () =>
            {
                RuleFor(x => x.Specialization)
                    .NotEmpty().WithMessage("Specialization is required for doctors")
                    .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters");
            });

            When(x => x.Role != 2, () =>
            {
                RuleFor(x => x.Specialization)
                    .Empty().WithMessage("Specialization is only for doctors");
            });
        }
    }
}