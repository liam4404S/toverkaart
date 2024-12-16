using System;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
namespace toverkaart
{
    public class DatabaseService
    {
        private readonly IConfiguration _configuration;

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConnectToDatabase()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine(connectionString);
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection successful!");

                var command = new MySqlCommand("SELECT * FROM gebieden", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["soort"]);
                    }
                }
            }
        }
    }
}
