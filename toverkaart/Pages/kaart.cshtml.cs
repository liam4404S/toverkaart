using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;

namespace toverkaart.Pages
{
    public class Kaart : PageModel
    {
        private readonly ILogger<Kaart> _logger;

        public Kaart(ILogger<Kaart> logger)
        {
            _logger = logger;
        }
    }
}
