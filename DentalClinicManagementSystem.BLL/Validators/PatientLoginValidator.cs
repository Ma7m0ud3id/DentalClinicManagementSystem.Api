using DentalClinicManagementSystem.BLL.DTOs.Auth;
using FluentValidation;

namespace DentalClinicManagementSystem.BLL.Validators
{
    public class PatientLoginValidator : AbstractValidator<PatientLoginDto>
    {
        public PatientLoginValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}