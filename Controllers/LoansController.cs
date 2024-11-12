using AssetSentry.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AssetSentry.Controllers
{
    public class LoansController : Controller
    {
        private AssetSentryContext _context;

        public LoansController(AssetSentryContext context) => _context = context;

        public IActionResult LoanList(string searchString)
        {
            LoanViewModel loanViewModel = new LoanViewModel();

            IQueryable<Device> deviceQuery = _context.Devices.Include(x => x.Status);
            loanViewModel.Devices = deviceQuery.OrderBy(x => x.Id).ToList();

            IQueryable<Loan> loanQuery = _context.Loans.Include(x => x.Device);
            loanViewModel.Loans = loanQuery.OrderBy(x => x.Id).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                var foundLoans = loanViewModel.Loans.Where(s => s.Device.Name!.ToUpper().Contains(searchString.ToUpper())
                || s.Student!.ToUpper().Contains(searchString.ToUpper())).ToList();

                loanViewModel.Loans = foundLoans;
            }

            return View(loanViewModel);
        }

        public IActionResult AddLoan()
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
