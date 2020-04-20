namespace OfficeManager.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using OfficeManager.Data;
    using OfficeManager.Models;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            this._logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult HttpError(int statusCode)
        {
            return this.View(statusCode);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
