using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models;
using WebApi.Utilities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseConnection dataAccess;
        private readonly IValidator<User> userValidator;
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration, IValidator<User> userValidator)
        {
            dataAccess = new DatabaseConnection(configuration);
            this.userValidator = userValidator;
            _configuration = configuration;
        }

        // GET: api/Users
        [HttpGet]
        public IActionResult Get()
        {
            var users = dataAccess.Query<User>("SELECT * FROM Users");
            return Ok(users);
        }
        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = dataAccess.Query<User>("SELECT * FROM Users WHERE UserID = @UserID", new { UserID = id }).FirstOrDefault();
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Ok(user);
        }

        // POST: api/Users/AutoCreate
        [HttpPost("AutoCreate")]
        public IActionResult AutoCreateUsers(int start, int count)
        {
            var users = GenerateUsers(start, count);
            var createdUsers = new List<User>();
            var existingUsers = new List<string>();

            foreach (var user in users)
            {
                if (UserExists(user.Username, user.Email))
                {
                    existingUsers.Add(user.Username);
                    continue;
                }

                var validationResult = userValidator.Validate(user);
                if (!validationResult.IsValid)
                {
                    // Manejar la validación fallida
                    continue;
                }

                dataAccess.Execute(
                    "INSERT INTO Users (Username, Password, Email, IsActive) VALUES (@Username, @Password, @Email, @IsActive)",
                    user
                );
                createdUsers.Add(user);
            }

            return Ok(new { CreatedUsers = createdUsers, ExistingUsers = existingUsers });
        }
        private bool UserExists(string username, string email)
        {
            var userByUsername = dataAccess.Query<User>("SELECT * FROM Users WHERE Username = @Username", new { Username = username }).FirstOrDefault();
            var userByEmail = dataAccess.Query<User>("SELECT * FROM Users WHERE Email = @Email", new { Email = email }).FirstOrDefault();
            return userByUsername != null || userByEmail != null;
        }
        private static IEnumerable<User> GenerateUsers(int start, int count)
        {
            for (int i = start; i < start + count; i++)
            {
                yield return new User
                {
                    Username = $"Usuario{i}",
                    Password = $"contrasennia{i}",
                    Email = $"email{i}@ejemplo.com",
                    IsActive = true
                };
            }
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User user)
        {
            var userToUpdate = dataAccess.Query<User>("SELECT * FROM Users WHERE UserID = @UserID", new { UserID = id }).FirstOrDefault();
            if (userToUpdate == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Realiza la actualización según los campos que se deseen modificar
            var rowsAffected = dataAccess.Execute(
                "UPDATE Users SET Username = @Username, Password = @Password, Email = @Email, IsActive = @IsActive WHERE UserID = @UserID",
                new { user.Username, user.Password, user.Email, user.IsActive, UserID = id }
            );

            if (rowsAffected > 0)
            {
                return Ok("Usuario actualizado con éxito.");
            }
            else
            {
                return StatusCode(500, "No se pudo actualizar el usuario en la base de datos.");
            }
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var rowsAffected = dataAccess.Execute("DELETE FROM Users WHERE UserID = @UserID", new { UserID = id });
            if (rowsAffected > 0)
            {
                return Ok("Usuario eliminado con éxito.");
            }
            else
            {
                return StatusCode(500, "No se pudo eliminar el usuario de la base de datos.");
            }
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User userLogin)
        {
            var user = AuthenticateUser(userLogin.Username, userLogin.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var tokenString = GenerateJwtToken(user);
            return Ok(new { Token = tokenString });
        }

        private User AuthenticateUser(string username, string password)
        {
            // Aquí es donde buscarás al usuario en la base de datos. 
            // En un escenario real, es importante almacenar las contraseñas de manera segura (por ejemplo, usando hash y sal).
            var user = dataAccess.Query<User>("SELECT * FROM Users WHERE Username = @Username AND Password = @Password",
                        new { Username = username, Password = password }).FirstOrDefault();

            if (user != null)
            {
                // Si encuentras al usuario y la contraseña coincide, devuelves el usuario.
                return user;
            }

            // Si no, devuelves null.
            return null!;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:SecretKey"]!); // Ahora esto debería funcionar

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }

}
