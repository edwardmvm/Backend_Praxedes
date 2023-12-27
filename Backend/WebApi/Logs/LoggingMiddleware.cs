using WebApi.Utilities;

namespace WebApi.Logs
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DatabaseConnection databaseConnection)
        {
            // Crear el objeto de log
            RequestLogEntry logEntry = new()
            {
                RequestType = context.Request.Method,
                RequestURL = context.Request.Path,
                Timestamp = DateTime.UtcNow,
                // Captura los parámetros de ruta para las peticiones GET:
                RequestDetails = context.Request.Method == HttpMethods.Get ? context.Request.QueryString.Value : await ReadRequestBody(context)
            };

            try
            {
                await _next(context);
                logEntry.IsSuccessful = context.Response.StatusCode < 400;
            }
            catch (Exception ex)
            {
                logEntry.IsSuccessful = false;
                logEntry.FailureReason = ex.Message;
                throw;
            }
            finally
            {
                // Registra la petición aquí
                databaseConnection.LogRequest(logEntry);
            }
        }

        private static async Task<string> ReadRequestBody(HttpContext context)
        {
            context.Request.EnableBuffering();
            var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
            context.Request.Body.Position = 0; // Restablecer la posición para el model binding
            return body;
        }
    }

}
