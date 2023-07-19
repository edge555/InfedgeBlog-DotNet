using FluentValidation;

namespace Cefalo.InfedgeBlog.Service.Dtos.Validators
{
    public class UserPostDtoValidator : DtoValidatorBase<UserPostDto>
    {
        public UserPostDtoValidator() 
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username can not be empty.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name can not be empty.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email can not be empty.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can not be empty.");
        }
    }
}
