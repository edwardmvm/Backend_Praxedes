using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FamilyGroupsController : ControllerBase
    {
        private readonly DatabaseConnection dataAccess;
        private readonly IValidator<FamilyGroup> familyGroupValidator;

        public FamilyGroupsController(IConfiguration configuration, IValidator<FamilyGroup> familyGroupValidator)
        {
            dataAccess = new DatabaseConnection(configuration);
            this.familyGroupValidator = familyGroupValidator;
        }

        // GET: api/FamilyGroups
        [HttpGet]
        public IActionResult Get()
        {
            var familyGroups = dataAccess.Query<FamilyGroup>("SELECT * FROM FamilyGroup");
            return Ok(familyGroups);
        }

        // GET: api/FamilyGroups/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var familyGroup = dataAccess.Query<FamilyGroup>("SELECT * FROM FamilyGroup WHERE MemberID = @MemberID", new { MemberID = id }).FirstOrDefault();
            if (familyGroup == null)
            {
                return NotFound("Grupo familiar no encontrado.");
            }

            return Ok(familyGroup);
        }

        // POST: api/FamilyGroups
        [HttpPost]
        public IActionResult Post([FromBody] FamilyGroup familyGroup)
        {
            // Determina si es menor de edad basado en la fecha de nacimiento.
            if (familyGroup.FechaNacimiento.HasValue)
            {
                var age = DateTime.Today.Year - familyGroup.FechaNacimiento.Value.Year;
                if (familyGroup.FechaNacimiento > DateTime.Today.AddYears(-age)) age--;
                familyGroup.MenorEdad = age < 18;
            }
            else
            {
                familyGroup.MenorEdad = false; // O define una lógica para cuando la fecha de nacimiento no se proporcione
            }

            // Si es mayor de edad, establece la fecha de nacimiento en nulo.
            if (!familyGroup.MenorEdad)
            {
                familyGroup.FechaNacimiento = null;
            }

            // Validación del objeto
            var validationResult = familyGroupValidator.Validate(familyGroup);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            int rowsAffected = dataAccess.Execute(
                "INSERT INTO FamilyGroup (UserID, Cedula, Nombres, Apellidos, Genero, Parentesco, Edad, MenorEdad, FechaNacimiento) VALUES (@UserID, @Cedula, @Nombres, @Apellidos, @Genero, @Parentesco, @Edad, @MenorEdad, @FechaNacimiento)",
                new
                {
                    familyGroup.UserID,
                    familyGroup.Cedula,
                    familyGroup.Nombres,
                    familyGroup.Apellidos,
                    familyGroup.Genero,
                    familyGroup.Parentesco,
                    familyGroup.Edad,
                    familyGroup.MenorEdad,
                    FechaNacimiento = familyGroup.MenorEdad ? familyGroup.FechaNacimiento : null
                }
            );

            if (rowsAffected > 0)
            {
                return Ok("Grupo familiar agregado con éxito.");
            }
            else
            {
                return StatusCode(500, "No se pudo insertar el grupo familiar en la base de datos.");
            }
        }

        // PUT: api/FamilyGroups/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] FamilyGroup familyGroup)
        {
            // Asignar MenorEdad basado en la edad
            familyGroup.MenorEdad = familyGroup.Edad < 18;

            var familyGroupToUpdate = dataAccess.Query<FamilyGroup>("SELECT * FROM FamilyGroup WHERE MemberID = @MemberID", new { MemberID = id }).FirstOrDefault();
            if (familyGroupToUpdate == null)
            {
                return NotFound("Grupo familiar no encontrado.");
            }

            var rowsAffected = dataAccess.Execute(
                "UPDATE FamilyGroup SET UserID = @UserID, Cedula = @Cedula, Nombres = @Nombres, Apellidos = @Apellidos, Genero = @Genero, Parentesco = @Parentesco, Edad = @Edad, MenorEdad = @MenorEdad, FechaNacimiento = @FechaNacimiento WHERE MemberID = @MemberID",
                new { familyGroup.UserID, familyGroup.Cedula, familyGroup.Nombres, familyGroup.Apellidos, familyGroup.Genero, familyGroup.Parentesco, familyGroup.Edad, familyGroup.MenorEdad, familyGroup.FechaNacimiento, MemberID = id }
            );

            if (rowsAffected > 0)
            {
                return Ok("Grupo familiar actualizado con éxito.");
            }
            else
            {
                return StatusCode(500, "No se pudo actualizar el grupo familiar en la base de datos.");
            }
        }

        // DELETE: api/FamilyGroups/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var rowsAffected = dataAccess.Execute("DELETE FROM FamilyGroup WHERE MemberID = @MemberID", new { MemberID = id });
            if (rowsAffected > 0)
            {
                return Ok("Grupo familiar eliminado con éxito.");
            }
            else
            {
                return StatusCode(500, "No se pudo eliminar el grupo familiar de la base de datos.");
            }
        }
    }
}
