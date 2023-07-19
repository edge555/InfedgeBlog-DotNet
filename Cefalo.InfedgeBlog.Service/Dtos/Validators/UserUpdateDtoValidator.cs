using FluentValidation;

namespace Cefalo.InfedgeBlog.Service.Dtos.Validators
{
    public class UserUpdateDtoValidator : DtoValidatorBase<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name can not be empty.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can not be empty.");
        }
    }
}
