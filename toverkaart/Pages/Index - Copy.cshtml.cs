using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mysqlx;
using System.Security.Cryptography;
using toverkaart;

namespace toverkaart.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty] public string Voornaam { get; set; } = string.Empty;
        [BindProperty] public string Achternaam { get; set; } = string.Empty;
        [BindProperty] public string Email { get; set; } = string.Empty;
        [BindProperty] public string Wachtwoord { get; set; } = string.Empty;
        [BindProperty] public string HerhaalWachtwoord { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public IndexModel(DatabaseService databaseService, ILogger<IndexModel> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }
        public IActionResult OnPostLogin()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Wachtwoord))
            {
                ErrorMessage = "Vul beide velden in.";
                return Page();
            }

            var mens = new Account(_databaseService);

            if (mens.Correctlogin(Email, Wachtwoord, out string errorMessage))
            {
                var user = mens.GetUserByEmail(Email);
                _logger.LogInformation($"User logged in successfully: {user.Id}, {user.Voornaam}, {user.Achternaam}, {Email}, {Wachtwoord}, {user.Rol}.");
                return RedirectToPage("/kaart");
            }
            else
            {
                ErrorMessage = errorMessage;
                return Page();
            }
        }
        public IActionResult OnPostCreateAccount()
        {
            try
            {
                // Log received data
                _logger.LogInformation($"Received: {Voornaam}, {Achternaam}, {Email}, {Wachtwoord}, {HerhaalWachtwoord}");

                // Validation
                if (string.IsNullOrEmpty(Voornaam) || string.IsNullOrEmpty(Achternaam) ||
                    string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Wachtwoord) ||
                    string.IsNullOrEmpty(HerhaalWachtwoord))
                {
                    return BadRequest(new { success = false, message = "Vul alle velden in." });
                }

                if (Wachtwoord != HerhaalWachtwoord)
                {
                    return BadRequest(new { success = false, message = "Wachtwoorden komen niet overeen." });
                }

                var account = new Account(_databaseService);
                var existingUser = account.GetUserByEmail(Email);

                if (existingUser != null)
                {
                    return BadRequest(new { success = false, message = "E-mail adres is al gelinkt aan een account." });
                }

                // Log successful account creation
                _logger.LogInformation($"Account aangemaakt voor {Voornaam} {Achternaam}");

                return new JsonResult(new { success = true, message = "Account aangemaakt!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating account: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Interne serverfout." });
            }
        }
    }
}
