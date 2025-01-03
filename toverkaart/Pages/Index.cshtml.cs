using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using toverkaart;

namespace toverkaart.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Wachtwoord { get; set; } = string.Empty;
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

            var mensen = new Mensen(_databaseService);

            var user = mensen.GetUserByEmail(Email);

            if (user != null && user.Wachtwoord == Wachtwoord)
            {
                _logger.LogInformation($"user {Email} logged in successfully");
                return RedirectToPage("/kaart");
            }
            else
            {
                ErrorMessage = "Ongeldig e-mail address of wachtwoord.";
                return Page();
            }
        }
    }
}
