using System;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
namespace toverkaart
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(IConfiguration configuration, ILogger<DatabaseService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new InvalidOperationException("Connection string not found.");

            _logger = logger;
        }

        public MySqlConnection GetConnection()
        {
            try
            {
                var connection = new MySqlConnection(_connectionString);
                connection.Open();
                _logger.LogInformation("Database connection successful.");
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection failed.");
                throw;
            }
        }

        public DataTable ExecuteQuery(string query, params MySqlParameter[] parameters)
        {
            using (var connection = GetConnection())
            {
                using (var command = new MySqlCommand(query, connection))
                {

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        DataTable result = new DataTable();
                        adapter.Fill(result);
                        return result;
                    }
                }
            }
        }
    }
}
