using MySql.Data.MySqlClient;
using System.Data;

namespace toverkaart
{
    public class Mensen
    {
        private DatabaseService _databaseService;

        public Mensen() { }

        public Mensen(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Wachtwoord { get; set; } = string.Empty;
        public string Rol { get; set; }

        public Mensen? GetUserByEmail(string email)
        {
            string query = "SELECT * FROM mensen WHERE email = @Email";

            var parameters = new[]
            {
                new MySqlParameter("@Email", email)
            };

            var result = _databaseService.ExecuteQuery(query, parameters);
            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                return new Mensen()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Email = row["email"].ToString(),
                    Wachtwoord = row["wachtwoord"].ToString(),
                    Rol = row["rol"].ToString()
                };
            }
            return null;
        }
    }
}
