using FluentValidation;

namespace Cefalo.InfedgeBlog.Service.Dtos.Validators
{
    public class StoryPostDtoValidator : DtoValidatorBase<StoryPostDto>
    {
        public StoryPostDtoValidator() 
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title can not be empty");
            RuleFor(x => x.Body).NotEmpty().WithMessage("Body can not be empty");
        }
    }
}
