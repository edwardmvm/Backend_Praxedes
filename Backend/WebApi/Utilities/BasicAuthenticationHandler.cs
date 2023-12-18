using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using WebApi.Models;

namespace WebApi.Utilities
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly DatabaseConnection dataAccess;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            DatabaseConnection dataAccess)
            : base(options, logger, encoder, clock)
        {
            this.dataAccess = dataAccess;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            try
            {
                var authorizationHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authorizationHeader.Parameter!);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                var user = await Task.Run(() => dataAccess.Query<User>("SELECT * FROM Users WHERE Username = @Username AND Password = @Password",
                    new { Username = username, Password = password }).FirstOrDefault());

                if (user == null)
                    return AuthenticateResult.Fail("Invalid Username or Password");

                // Si las credenciales son válidas, crear un ClaimsPrincipal con el nombre de usuario.
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    // Agregar más claims.
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail("Error al procesar las credenciales: " + ex.Message);
            }
        }
    }
}