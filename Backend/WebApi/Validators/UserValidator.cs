namespace WebApi.Validators
{
    using FluentValidation;
    using WebApi.Models;

    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty().WithMessage("El nombre de usuario es requerido");
        }
    }

}
