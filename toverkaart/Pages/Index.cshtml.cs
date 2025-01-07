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
        [BindProperty] public string Email { get; set; } = string.Empty;
        [BindProperty] public string Wachtwoord { get; set; } = string.Empty;
        [BindProperty] public string CreateVoornaam { get; set; } = string.Empty;
        [BindProperty] public string CreateAchternaam { get; set; } = string.Empty;
        [BindProperty] public string CreateEmail { get; set; } = string.Empty;
        [BindProperty] public string CreateWachtwoord { get; set; } = string.Empty;
        [BindProperty] public string CreateHerhaalWachtwoord { get; set; } = string.Empty;

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

            var persoon = new Account(_databaseService);

            if (persoon.Correctlogin(Email, Wachtwoord, out string errorMessage))
            {
                var user = persoon.GetUserByEmail(Email);
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
                _logger.LogInformation($"Received: {CreateVoornaam}, {CreateAchternaam}, {CreateEmail}, {CreateWachtwoord}, {CreateHerhaalWachtwoord}");

                if (string.IsNullOrEmpty(CreateVoornaam) || string.IsNullOrEmpty(CreateAchternaam) ||
                    string.IsNullOrEmpty(CreateEmail) || string.IsNullOrEmpty(CreateWachtwoord) ||
                    string.IsNullOrEmpty(CreateHerhaalWachtwoord))
                {
                    ErrorMessage = "Vul alle velden in.";
                    return Page();
                }

                if (CreateWachtwoord != CreateHerhaalWachtwoord)
                {
                    ErrorMessage = "Wachtwoorden komen niet overeen.";
                    return Page();
                }

                var persoon = new Account(_databaseService);
                var existingUser = persoon.GetUserByEmail(CreateEmail);

                if (existingUser != null)
                {
                    ErrorMessage = "E-mail adres is al gelinkt aan een account.";
                    return Page();
                }

                bool isAccountCreated = persoon.CreateAccount(CreateVoornaam, CreateAchternaam, CreateEmail, CreateWachtwoord, "Bezoeker");

                if (isAccountCreated)
                {
                    _logger.LogInformation($"Account aangemaakt voor {CreateVoornaam} {CreateAchternaam}");
                    return RedirectToPage("/Kaart");
                }
                else
                {
                    ErrorMessage = "Er is iets misgegaan bij het aanmaken van het account.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating account: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Interne serverfout." });
            }
        }
    }
}
