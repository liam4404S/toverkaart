using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;

namespace toverkaart.Pages
{
    public class KaartModel : PageModel
    {
        private readonly ILogger<KaartModel> _logger;

        public KaartModel(ILogger<KaartModel> logger)
        {
            _logger = logger;
        }
    }
}
