using FluentValidation;
using static AnyCodeHub.Contract.Services.V1.Authentication.Command;

namespace AnyCodeHub.Contract.Services.V1.Authentication.Validators;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(t => t.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(t => t.FirstName).NotEmpty();
        RuleFor(t => t.LastName).NotEmpty();
        RuleFor(t => t.BirthOfDate).NotEmpty();

        RuleFor(p => p.Password).NotEmpty().WithMessage("Your password cannot be empty.")
                    .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                    .MaximumLength(30).WithMessage("Your password length must not exceed 30.")
                    .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                    .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                    .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                    //.Matches(@"[\!\?\*\.\@\)]+").WithMessage("Your password must contain at least one (!? *.).")
                    .Matches(@"(?=.*\W)").WithMessage("Your password must contain at least one special character.")
            ;

        RuleFor(t => t.Password).NotEmpty().NotNull();
        RuleFor(t => t.PasswordConfirmed).NotEmpty().WithMessage("Please enter the password confirmation.");
        RuleFor(t => t.Password).Equal(t => t.PasswordConfirmed).WithMessage($"The password confirmation doesn't match.");
    }
}
