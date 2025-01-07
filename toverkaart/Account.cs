using MySql.Data.MySqlClient;
using System.Data;

namespace toverkaart
{
    public class Account
    {
        private DatabaseService _databaseService;

        public Account() { }

        public Account(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public int Id { get; set; }
        public string Voornaam { get; set; } = string.Empty;
        public string Achternaam { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Wachtwoord { get; set; } = string.Empty;
        public string HerhaalWachtwoord { get; set; } = string.Empty;
        public string Rol { get; set; }

        public Account? GetUserByEmail(string email)
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
                return new Account()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Voornaam = row["voornaam"].ToString(),
                    Achternaam = row["achternaam"].ToString(),
                    Email = row["email"].ToString(),
                    Wachtwoord = row["wachtwoord"].ToString(),
                    Rol = row["rol"].ToString()
                };
            }
            return null;
        }

        public bool Correctlogin(string email,string wachtwoord, out string errorMessage)
        {
            errorMessage = string.Empty;

            var user = GetUserByEmail(email);

            if (user != null && user.Wachtwoord == wachtwoord)
            {
                return true;
            }

            errorMessage = "ongeldig e-mail address of wachtwoord.";
            return false;
        }

        public bool CreateAccount(string voornaam, string achternaam, string email, string wachtwoord, string rol)
        {
            try
            {
                string query = "INSERT INTO mensen (Voornaam, Achternaam, Email, Wachtwoord, Rol) VALUES (@Voornaam, @Achternaam, @Email, @Wachtwoord, @Rol)";

                var parameters = new MySqlParameter[]
                {
                new MySqlParameter("@Voornaam", MySqlDbType.VarChar) { Value = voornaam },
                new MySqlParameter("@Achternaam", MySqlDbType.VarChar) { Value = achternaam },
                new MySqlParameter("@Email", MySqlDbType.VarChar) { Value = email },
                new MySqlParameter("@Wachtwoord", MySqlDbType.VarChar) { Value = wachtwoord }, 
                new MySqlParameter("@Rol", MySqlDbType.VarChar) {Value = rol}
                };

                var result = _databaseService.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating account: {ex.Message}");
                return false;
            }
        }
    }
}
