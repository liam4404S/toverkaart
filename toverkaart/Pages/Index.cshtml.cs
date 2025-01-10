using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;

namespace toverkaart.Pages
{
    public class IndexModel : PageModel
    {
        private DatabaseService _databaseService;
        private ILogger<IndexModel> _logger;
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
            var persoon = new Account(_databaseService);

            var succesLogin = persoon.Correctlogin(Email, Wachtwoord, out string errorMessage);
            if (succesLogin)
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

        public IActionResult? OnPostCreateAccount()
        {
            try
            {
                _logger.LogInformation($"Received: {CreateVoornaam}, {CreateAchternaam}, {CreateEmail}, {CreateWachtwoord}, {CreateHerhaalWachtwoord}");

                var persoon = new Account(_databaseService);

                var emptyField = persoon.CreateAccountCheck(CreateVoornaam, CreateAchternaam, CreateEmail, CreateWachtwoord, CreateHerhaalWachtwoord, out string errorMessage);
                if (emptyField)
                {
                    ErrorMessage = errorMessage;
                    return null;
                }
                
                var passwordDifference = persoon.CreateAccountCheck(CreateWachtwoord, CreateHerhaalWachtwoord, out errorMessage);
                if (passwordDifference)
                {
                    ErrorMessage = errorMessage;
                    return null;
                }

                var existingUser = persoon.CreateAccountCheck(CreateEmail, out errorMessage);

                if (existingUser)
                {
                    ErrorMessage = errorMessage;
                    return null;
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
