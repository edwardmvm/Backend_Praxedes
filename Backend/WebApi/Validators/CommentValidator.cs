namespace WebApi.Validators
{
    using FluentValidation;
    using WebApi.Models;

    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(comment => comment.Body)
                .NotNull().WithMessage("El cuerpo del comentario es requerido")
                .NotEmpty().WithMessage("El cuerpo del comentario no puede estar vacío");
        }
    }


}
