using CelebRateApi.DTOs;
using FluentValidation;

namespace CelebRateApi.Validators
{
    public class UserValidator : AbstractValidator<UserDTO>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Age)
                .NotEmpty()
                .InclusiveBetween(15, 99);

            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .Matches(@"^\d{5}(-\d{4})?$")
                .WithMessage("ZipCode must be exactly 5 digits");
        }
    }
}