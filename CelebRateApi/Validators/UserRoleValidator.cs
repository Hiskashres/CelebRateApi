using CelebRateApi.DTOs;
using FluentValidation;

namespace CelebRateApi.Validators
{
    public class UserRoleValidator : AbstractValidator<UserRoleDTO>
    {
        public UserRoleValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Role)
                .NotEmpty();
        }
    }
}