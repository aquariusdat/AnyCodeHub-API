using FluentValidation;

namespace AnyCodeHub.Contract.Services.V1.Authentication.Validators
{

    public class LoginValidator : AbstractValidator<Contract.Services.V1.Authentication.Query.Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty().NotNull();
        }
    }
}
