using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class CommentsController : ControllerBase
    {
        private readonly DatabaseConnection dataAccess;
        private readonly IValidator<Comment> commentValidator;

        public CommentsController(IConfiguration configuration, IValidator<Comment> commentValidator)
        {
            dataAccess = new DatabaseConnection(configuration);
            this.commentValidator = commentValidator;
        }

        // GET: api/Comments
        [HttpGet]
        public IActionResult Get()
        {
            var comments = dataAccess.Query<Comment>("SELECT * FROM Comments").ToList();
            return Ok(comments);
        }

        // GET: api/Comments/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // Obtención del comentario por ID
            var comment = dataAccess.Query<Comment>("SELECT * FROM Comments WHERE CommentId = @CommentId", new { CommentId = id }).FirstOrDefault();
            if (comment == null)
            {
                return NotFound("Comentario no encontrado.");
            }

            return Ok(comment);
        }

        // POST: api/Comments/AutoGenerate
        [HttpPost("AutoGenerate")]
        public IActionResult AutoGenerateComments()
        {
            // Obtener todos los Posts y Usuarios
            var posts = dataAccess.Query<Post>("SELECT * FROM Posts").ToList();
            var users = dataAccess.Query<User>("SELECT * FROM Users").ToList();

            foreach (var post in posts)
            {
                foreach (var user in users)
                {
                    var comment = new Comment
                    {
                        PostId = post.PostId,
                        Name = user.Username,
                        Email = user.Email,
                        Body = post.Body
                    };

                    // Validación del comentario
                    var validationResult = commentValidator.Validate(comment);
                    if (!validationResult.IsValid)
                    {
                        // Continuar con el siguiente si la validación falla
                        continue;
                    }

                    // Inserción del comentario en la base de datos
                    try
                    {
                        dataAccess.Execute(
                            "INSERT INTO Comments (PostId, Name, Email, Body) VALUES (@PostId, @Name, @Email, @Body)",
                            comment
                        );
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"No se pudo insertar el comentario en la base de datos: {ex.Message}");
                    }
                }
            }

            return Ok("Comentarios generados automáticamente para todos los posts y usuarios.");
        }

        // PUT: api/Comments/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Comment comment)
        {
            var existingComment = dataAccess.Query<Comment>("SELECT * FROM Comments WHERE CommentId = @CommentId", new { CommentId = id }).FirstOrDefault();
            if (existingComment == null)
            {
                return NotFound("Comentario no encontrado.");
            }

            var validationResult = commentValidator.Validate(comment);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            int rowsAffected = dataAccess.Execute(
                "UPDATE Comments SET PostId = @PostId, Name = @Name, Email = @Email, Body = @Body WHERE CommentId = @CommentId",
                new { comment.PostId, comment.Name, comment.Email, comment.Body, CommentId = id }
            );

            if (rowsAffected > 0)
            {
                return Ok("Comentario actualizado con éxito.");
            }
            else
            {
                return StatusCode(500, "No se pudo actualizar el comentario en la base de datos.");
            }
        }

        // DELETE: api/Comments/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            int rowsAffected = dataAccess.Execute("DELETE FROM Comments WHERE CommentId = @CommentId", new { CommentId = id });
            if (rowsAffected > 0)
            {
                return Ok("Comentario eliminado con éxito.");
            }
            else
            {
                return StatusCode(500, "No se pudo eliminar el comentario de la base de datos.");
            }
        }

    }

}
