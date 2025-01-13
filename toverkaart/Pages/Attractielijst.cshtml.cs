using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace toverkaart.Pages
{
    public class AttractielijstModel : PageModel
    {
        private DatabaseService _databaseService;
        private ILogger<AttractielijstModel> _logger;
        [BindProperty] public string AttractieNaam { get; set; } = string.Empty;
        public List<Attractie> GevondenAttracties { get; set; } = new List<Attractie>();

        public AttractielijstModel(DatabaseService databaseService, ILogger<AttractielijstModel> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        public IActionResult? OnPostSearch()
        {
            var attractie = new Attractie(_databaseService);
            // Get the list of all attracties that match the search term
            GevondenAttracties = attractie.GetAllAttracties(AttractieNaam);

            // If no attracties are found, add a model error
            if (GevondenAttracties.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Geen attractie gevonden.");
            }

            return Page();
        }
    }
}
