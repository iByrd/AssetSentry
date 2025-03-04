using AssetSentry.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AssetSentry.Controllers
{
    public class LoansController : Controller
    {
        private AssetSentryContext _context;

        public LoansController(AssetSentryContext context) => _context = context;

        [Authorize]
        public IActionResult LoanList(string searchString)
        {
            LoanViewModel loanViewModel = new LoanViewModel();

            IQueryable<Device> deviceQuery = _context.Devices.Include(x => x.Status).AsNoTracking();
            loanViewModel.Devices = deviceQuery.OrderBy(x => x.Id).ToList();

            IQueryable<Loan> loanQuery = _context.Loans.Include(x => x.Device).AsNoTracking();
            loanViewModel.Loans = loanQuery.OrderBy(x => x.Id).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                var foundLoans = loanViewModel.Loans.Where(s => s.Device.Name!.ToUpper().Contains(searchString.ToUpper())
                || s.Student!.ToUpper().Contains(searchString.ToUpper())).ToList();

                loanViewModel.Loans = foundLoans;
            }

            return View(loanViewModel);
        }

        [Authorize]
        public IActionResult AddLoan(int deviceId)
        {
            LoanViewModel loanViewModel = new LoanViewModel();
            loanViewModel.NewLoan.DeviceId = deviceId;
            loanViewModel.NewLoan.Device = _context.Devices.Single(x => x.Id == deviceId);
            return View(loanViewModel);
        }

        [HttpPost]
        public IActionResult AddLoan(LoanViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Loans.Add(model.NewLoan);
                Device loanDevice = _context.Devices.Single(x => x.Id == model.NewLoan.DeviceId);
                if (model.NewLoan.IsActive == true)
                {
                    loanDevice.StatusId = "rented";
                }
              

                _context.SaveChanges();
                return RedirectToAction("LoanList");
            }
            else
            {
                // finally fixed this... server-side validation was not ever working
                //model.Devices = _context.Devices.ToList();
                model.NewLoan.Device = _context.Devices.Single(x => x.Id == model.NewLoan.DeviceId);
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult EndLoan(int id)
        {
            var loan = _context.Loans.Find(id);
            if (loan != null)
            {
                loan.IsActive = false;
                var deviceIdToChange = loan.DeviceId;
                var device = _context.Devices.Find(deviceIdToChange);
                device.StatusId = "available";
                _context.Loans.Update(loan);
            }

            _context.SaveChanges();
            return RedirectToAction("LoanList");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
