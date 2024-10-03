using AssetSentry.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AssetSentry.Controllers
{
    public class LoansController : Controller
    {
        private readonly ILogger<LoansController> _logger;

        public LoansController(ILogger<LoansController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
