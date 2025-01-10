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

        //public AttractielijstModel(DatabaseService databaseService, ILogger<IndexModel> logger)
        //{
        //    _databaseService = databaseService;
        //    _logger = logger;
        //}

        public IActionResult? OnPostSearch()
        {
            
            return null;
        }
    }
}
