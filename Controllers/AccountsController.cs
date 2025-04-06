using AssetSentry.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AssetSentry.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AssetSentryContext _context;
        private readonly IEmailSender _emailSender;

        public AccountsController(AssetSentryContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult AccountList()
        {
            return View(_context.UserAccounts.ToList());
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult Registration()
        {
            return View();
        }

        // source: https://stackoverflow.com/questions/3984138/hash-string-in-c-sharp
        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        //Source: https://www.youtube.com/watch?v=712G-iQ1zzg
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserAccount account = new UserAccount();
                account.FirstName = model.FirstName;
                account.LastName = model.LastName;
                account.Email = model.Email;
                account.Password = GetHashString(model.Password);
                if (model.IsAdmin == "true")
                {
                    account.IsAdmin = true;
                } else
                {
                    account.IsAdmin = false;
                }

                try
                {
                    _context.UserAccounts.Add(account);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = $"{account.FirstName} {account.LastName} registered successfully.";
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Please enter unique Email and Password.");
                    return View(model);
                }
                return View();
            }
            return View(model);
        }

        public async Task<IActionResult> SendEmail()
        {
            await _emailSender.SendEmailAsync("byrdj7@mail.uc.edu", "AssetSentry Test", "Test 2 from our c# app");

            return View("Login");
            //string subject = "This is a AssetSentry Test";
            //string sender = "assetsentry@yahoo.com";
            //string pass = "";
            //using(MailMessage messg = new MailMessage(sender, "byrdj7@mail.uc.edu"))
            //{
            //    messg.Subject = subject;
            //    messg.Body = "This is a test that I hope works from our C# app";
            //    messg.IsBodyHtml = false;
            //    using(SmtpClient smtp = new SmtpClient())
            //    {
            //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //        smtp.Host = "smtp.mail.yahoo.com";
            //        smtp.EnableSsl = true;
            //        NetworkCredential cred = new NetworkCredential(sender, pass);
            //        smtp.UseDefaultCredentials = false;
            //        smtp.Credentials = cred;
            //        smtp.Port = 587;

            //        smtp.Send(messg);
            //    }
            //}
            //return View("Login");
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserAccounts.Where(x => x.Email == model.Email && x.Password == GetHashString(model.Password)).FirstOrDefault();
                if (user != null)
                {
                    //Success, create cookie
                    var claims = new List<Claim> 
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.FirstName),
                        //new Claim(ClaimTypes.Role, "User")
                    };
                    //check if user is admin
                    if (user.IsAdmin)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    } else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is not correct.");
                }
            }
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Accounts");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.UserAccounts.FindAsync(id);
            if (account != null)
            {
                _context.UserAccounts.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("AccountList");
        }
    }
}
