namespace WebApi.Validators
{
    using FluentValidation;
    using WebApi.Models;
    using WebApi.Utilities;

    public class FamilyGroupValidator : AbstractValidator<FamilyGroup>
    {
        private readonly DatabaseConnection dataAccess;

        public FamilyGroupValidator(DatabaseConnection dataAccess)
        {
            this.dataAccess = dataAccess;

            // Valida que la cédula no sea nula o vacía y que sea única
            RuleFor(familyGroup => familyGroup.Cedula)
                .NotNull().WithMessage("La cédula es requerida")
                .NotEmpty().WithMessage("La cédula no puede estar vacía")
                .Must(cedula => BeUniqueCedula(cedula)).WithMessage("La cédula ya está registrada.");

            // Valida que los campos Usuario, Nombres, Apellidos y Edad no sean nulos o vacíos
            RuleFor(familyGroup => familyGroup.UserID).NotNull().WithMessage("El usuario es requerido");
            RuleFor(familyGroup => familyGroup.Nombres).NotEmpty().WithMessage("Los nombres son requeridos");
            RuleFor(familyGroup => familyGroup.Apellidos).NotEmpty().WithMessage("Los apellidos son requeridos");
            RuleFor(familyGroup => familyGroup.Edad).NotNull().WithMessage("La edad es requerida");

            // Valida la fecha de nacimiento si es menor de edad
            RuleFor(familyGroup => familyGroup.FechaNacimiento)
                .NotEmpty()
                .When(familyGroup => familyGroup.MenorEdad)
                .WithMessage("La fecha de nacimiento es requerida para menores de edad");
        }

        private bool BeUniqueCedula(string? cedula)
        {
            var existingMember = dataAccess.Query<FamilyGroup>(
                "SELECT * FROM FamilyGroup WHERE Cedula = @Cedula", new { Cedula = cedula });

            return existingMember == null || !existingMember.Any();
        }
    }
}
