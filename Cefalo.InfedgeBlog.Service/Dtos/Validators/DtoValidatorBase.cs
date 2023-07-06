using Cefalo.InfedgeBlog.Service.CustomExceptions;
using FluentValidation;

namespace Cefalo.InfedgeBlog.Service.Dtos.Validators
{
    public class DtoValidatorBase <T> : AbstractValidator<T>
    {
        virtual public void ValidateDto(T dto)
        {
            var ValidationResult = this.Validate(dto);

            if (!ValidationResult.IsValid)
            {
                string ErrorMessage = "";

                foreach (var error in ValidationResult.Errors)
                {
                    ErrorMessage += error.ToString();
                }

                throw new BadRequestException(ErrorMessage);
            }
        }
    }
}
