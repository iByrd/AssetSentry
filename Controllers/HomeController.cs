using AssetSentry.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AssetSentry.Controllers
{
    public class HomeController : Controller
    {
        private AssetSentryContext _context;

        public HomeController(AssetSentryContext context) => _context = context;

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public List<int> GetTotalCountOfEachStatus()
        {
            List<int> data = new List<int>();
            int totalAvailable = _context.Devices.Where(x => x.StatusId == "available").Count();
            int totalOverdue = _context.Devices.Where(x => x.StatusId == "overdue").Count();
            int totalRented = _context.Devices.Where(x => x.StatusId == "rented").Count();

            data.Add(totalAvailable);
            data.Add(totalOverdue);
            data.Add(totalRented);
            return data;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
