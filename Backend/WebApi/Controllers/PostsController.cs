using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApi.Models;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class PostsController : ControllerBase
    {
        private readonly DatabaseConnection dataAccess;
        private readonly IValidator<Post> postValidator;

        public PostsController(IConfiguration configuration, IValidator<Post> postValidator)
        {
            dataAccess = new DatabaseConnection(configuration);
            this.postValidator = postValidator;
        }

        // GET: api/Posts
        [HttpGet]
        public IActionResult Get()
        {
            var posts = dataAccess.Query<Post>("SELECT * FROM Posts");
            return Ok(posts);
        }

        // POST: api/Posts
        [HttpPost]
        public IActionResult Post([FromBody] Post post)
        {
            var validationResult = postValidator.Validate(post);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // Procede con la inserción en la base de datos
            int postId;
            try
            {
                postId = dataAccess.Execute(
                    "INSERT INTO Posts (UserId, Title, Body) VALUES (@UserId, @Title, @Body); SELECT CAST(SCOPE_IDENTITY() as int)",
                    new { post.UserId, post.Title, post.Body }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"No se pudo insertar el post en la base de datos: {ex.Message}");
            }

            if (postId > 0)
            {
                // Retorna el ID del nuevo post
                return Ok(postId);
            }
            else
            {
                return StatusCode(500, "No se pudo insertar el post en la base de datos.");
            }
        }

        // GET: api/Posts/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var post = dataAccess.Query<Post>("SELECT * FROM Posts WHERE PostId = @PostId", new { PostId = id }).FirstOrDefault();
            if (post == null)
            {
                return NotFound("Post no encontrado.");
            }

            return Ok(post);
        }

        // POST: api/BulkInsertPosts
        [HttpPost("bulkInsert")]
        public async Task<IActionResult> BulkInsertPosts()
        {
            // Crear un cliente HTTP para consumir la API de JSONPlaceholder
            var client = new HttpClient();
            var response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error al obtener los datos desde JSONPlaceholder");
            }

            // Configuración para deserializar correctamente el JSON
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Deserializar la respuesta JSON a una lista de objetos Post
            var contentStream = await response.Content.ReadAsStreamAsync();
            var postsFromJsonPlaceholder = await JsonSerializer.DeserializeAsync<List<Post>>(contentStream, options);

            // Comprobar si la lista deserializada es nula
            if (postsFromJsonPlaceholder == null)
            {
                return StatusCode(500, "Error al deserializar los datos JSON.");
            }

            // Llamar a un método para realizar la inserción masiva
            var rowsAffected = BulkInsertPostsIntoDatabase(postsFromJsonPlaceholder);

            if (rowsAffected > 0)
            {
                return Ok($"{rowsAffected} posts han sido insertados con éxito.");
            }
            else
            {
                return StatusCode(500, "No se pudo insertar los posts en la base de datos.");
            }
        }
        private int BulkInsertPostsIntoDatabase(List<Post> posts)
        {
            var sql = "INSERT INTO Posts (UserID, Title, Body) VALUES (@userId, @Title, @Body)";
            return dataAccess.Execute(sql, posts);
        }

        // PUT: api/Posts/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Post post)
        {
            var postToUpdate = dataAccess.Query<Post>("SELECT * FROM Posts WHERE PostId = @PostId", new { PostId = id }).FirstOrDefault();
            if (postToUpdate == null)
            {
                return NotFound("Post no encontrado.");
            }

            // Realiza la actualización según los campos que se deseen modificar
            int rowsAffected;
            try
            {
                rowsAffected = dataAccess.Execute(
                    "UPDATE Posts SET Title = @Title, Body = @Body WHERE PostId = @PostId",
                    new { post.Title, post.Body, PostId = id }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"No se pudo actualizar el post en la base de datos: {ex.Message}");
            }

            if (rowsAffected > 0)
            {
                return Ok("Post actualizado con éxito.");
            }
            else
            {
                return StatusCode(500, "No se pudo actualizar el post en la base de datos.");
            }
        }

        // DELETE: api/Posts/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var rowsAffected = dataAccess.Execute("DELETE FROM Posts WHERE PostId = @PostId", new { PostId = id });
            if (rowsAffected > 0)
            {
                return Ok("Post eliminado con éxito.");
            }
            else
            {
                return StatusCode(500, "No se pudo eliminar el post de la base de datos.");
            }
        }
    }
}
