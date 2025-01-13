using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace toverkaart
{
    public class Account
    {
        private DatabaseService _databaseService;

        public int Id { get; private set; }
        public string Voornaam { get; set; } = string.Empty;
        public string Achternaam { get; set; } = string.Empty;

        private string _email = string.Empty;
        public string Email 
        {
            get => _email;
            private set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("vul een geldig email addres in.");
                _email = value;
            }
        }

        private string _wachtwoord = string.Empty;
        public string Wachtwoord
        {
            get => _wachtwoord;
            private set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 6)
                    throw new ArgumentException("wachtwoord moet 6 tekens lang zijn");
                _wachtwoord = value;
            }
        }

        public string Rol { get; private set; }

        public Account() { }

        public Account(DatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentException(nameof(databaseService));
        }

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
                return new Account(_databaseService)
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
            var user = GetUserByEmail(email);

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(wachtwoord))
            {
                errorMessage = "Vul beide velden in.";
                return false;
            }
            else if (user != null && user.Wachtwoord == wachtwoord)
            {
                errorMessage = string.Empty;
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

        public bool CreateAccountCheck(string voornaam, string achternaam, string email, string wachtwoord, string herhaalWachtwoord, out string errorMessage)
        {
            if (string.IsNullOrEmpty(voornaam) || string.IsNullOrEmpty(achternaam) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(wachtwoord) ||
                string.IsNullOrEmpty(herhaalWachtwoord))
            {
                errorMessage = "Vul alle velden in.";
                return true;
            }
            errorMessage = string.Empty;
            return false;
        }

        public bool CreateAccountCheck(string wachtwoord, string herhaalWachtwoord, out string errorMessage)
        {
            if (wachtwoord != herhaalWachtwoord)
            {
                errorMessage = "Wachtwoorden komen niet overeen.";
                return true;
            }
            errorMessage = string.Empty;
            return false;
        }

        public bool CreateAccountCheck(string email, out string errorMessage)
        {
            var existingEmail = GetUserByEmail(email);
            if (existingEmail != null)
            {
                errorMessage = "E-mail adres is al gelinkt aan een account.";
                return true;
            }
            errorMessage = string.Empty;
            return false;
        }
    }
}
