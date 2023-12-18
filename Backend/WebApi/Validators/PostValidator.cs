using FluentValidation;
using WebApi.Models;

namespace WebApi.Validators
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(post => post.Title)
                .NotEmpty().WithMessage("El título es requerido")
                .Length(1, 255).WithMessage("La longitud del título debe estar entre 1 y 255 caracteres");

            RuleFor(post => post.Body)
                .NotEmpty().WithMessage("El cuerpo es requerido");
        }
    }
}
