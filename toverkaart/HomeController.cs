using Microsoft.AspNetCore.Mvc;

namespace toverkaart
{
    public class HomeController : Controller
    {
        private readonly DatabaseService _databaseService;
        public HomeController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        public IActionResult Index()
        {
            _databaseService.GetConnection();

            return View();
        }
    }
}
