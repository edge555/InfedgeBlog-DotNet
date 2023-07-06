using FluentValidation;

namespace Cefalo.InfedgeBlog.Service.Dtos.Validators
{
    public class SignupDtoValidator : DtoValidatorBase<SignupDto>
    {
        public SignupDtoValidator() 
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username can not be empty")
                .MinimumLength(6).WithMessage("Username length must be at least 6");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name can not be empty")
                .MinimumLength(6).WithMessage("Name length must be at least 6");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password can not be empty")
                .MinimumLength(6).WithMessage("Password length must be at least 6");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid Email address");
        }
    }
}
