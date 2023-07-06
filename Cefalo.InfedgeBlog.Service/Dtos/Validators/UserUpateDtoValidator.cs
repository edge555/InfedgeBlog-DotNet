using FluentValidation;

namespace Cefalo.InfedgeBlog.Service.Dtos.Validators
{
    public class UserUpateDtoValidator : DtoValidatorBase<UserUpdateDto>
    {
        public UserUpateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name can not be empty");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can not be empty");
        }
    }
}
