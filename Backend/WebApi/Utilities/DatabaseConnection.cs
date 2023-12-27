namespace WebApi.Utilities
{
    using Dapper;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Data;
    using WebApi.Logs;

    public class DatabaseConnection
    {
        private readonly string connectionString;

        public DatabaseConnection(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public IEnumerable<T> Query<T>(string sql, object? param = null)
        {
            using var connection = new SqlConnection(connectionString);
            return connection.Query<T>(sql, param);
        }

        public int Execute(string sql, object? param = null)
        {
            using var connection = new SqlConnection(connectionString);
            return connection.Execute(sql, param); // Dapper's Execute returns the number of rows affected
        }

        //Manejo de logs
        public void LogRequest(RequestLogEntry logEntry)
        {
            string sql = @"INSERT INTO RequestLog (RequestType, RequestURL, RequestDetails, IsSuccessful, FailureReason, Timestamp) 
                   VALUES (@RequestType, @RequestURL, @RequestDetails, @IsSuccessful, @FailureReason, @Timestamp)";
            using var connection = new SqlConnection(connectionString);
            connection.Execute(sql, logEntry);
        }

        // Método para probar la conexión a la base de datos
        public bool TestConnection()
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                connection.Open();
                return connection.State == ConnectionState.Open;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al conectar a la base de datos: {ex.Message}");
                return false;
            }
        }

    }

}
