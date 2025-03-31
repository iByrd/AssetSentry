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
        private readonly IEmailSender _emailSender;

        public LoansController(AssetSentryContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

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
                searchString = searchString.ToUpper();

                var foundLoans = loanViewModel.Loans.Where(s =>
                    (s.Device.Name != null && s.Device.Name.ToUpper().Contains(searchString)) ||
                    (s.Student != null && s.Student.ToUpper().Contains(searchString)) ||
                    (s.Email != null && s.Email.ToUpper().Contains(searchString)) ||
                    (s.IsActive ? "ACTIVE" : "CLOSED").Contains(searchString)
                ).ToList();

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

        [HttpPost]
        public async Task<IActionResult> SendLoanReminder(int loanId)
        {
            string returnSentence;
            var loan = await _context.Loans.Include(l => l.Device).FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
            {
                return NotFound();
            }

            string status = loan.EndDate < DateTime.Today ? "Overdue" :
                            (loan.EndDate == DateTime.Today ? "Due Today" : "Due Soon");

            if (status == "Overdue")
            {
                returnSentence = "Return it as soon as possible to avoid further penalties.";
            }
            else if(status == "Due Today")
            {
                returnSentence = "Return it by the end of today to avoid penalties.";
            }
            else
            {
                returnSentence = $"Return it by {loan.EndDate.ToShortDateString()} to avoid penalties.";
            }

            string emailBody = $@"
                <h2>Loan Reminder</h2>
                <p>Hello {loan.Student},</p>
                <p>Your loan for <b>{loan.Device?.Name}</b> is <b>{status}</b>.</p>
                <p>{returnSentence}</p>
                <p>Thank you!</p>";

            await _emailSender.SendEmailAsync(loan.Email, $"Loan Reminder: {status}", emailBody);

            TempData["SuccessMessage"] = $"Reminder email sent to {loan.Email}.";
            return RedirectToAction("LoanList");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
