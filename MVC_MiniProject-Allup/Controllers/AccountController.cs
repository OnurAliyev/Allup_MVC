using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_MiniProject_Allup.DataAccesLayer;
using MVC_MiniProject_Allup.Models;
using MVC_MiniProject_Allup.ViewModels;

namespace MVC_MiniProject_Allup.Controllers
{
    public class AccountController : Controller
    {
        private readonly AllupDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(AllupDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = null;
            user = await _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user is null)
            {
                ModelState.AddModelError("", "Wrong username or password!");
                return View();
            }
            var resul = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
            if (!resul.Succeeded)
            {
                ModelState.AddModelError("", "Wrong username or password!");
                return View();
            }
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if (user is not null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetTokenLink = Url.Action("ResetPassword", "Account", new { email = forgotPasswordViewModel.Email, token = token }, Request.Scheme);

                return View("succesPage");
            }
            else
            {
                ModelState.AddModelError("Email", "Email is not found!");
                return View();
            }

        }
        public IActionResult ResetPassword(string email, string token)
        {
            if (email is null || token is null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ReserPasswrodViewModel reserPasswrodViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(reserPasswrodViewModel.Email);
            if (user is not null)
            {
                var result = await _userManager.ResetPasswordAsync(user, reserPasswrodViewModel.Token, reserPasswrodViewModel.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        return View();
                    }
                }
            }
            else
            {
                return NotFound("Email is not found!");
            }
            return RedirectToAction("Login", "Account");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (await _context.Users.AnyAsync(u => u.NormalizedUserName == registerViewModel.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "Username already exist!");
                return View();
            }
            if (await _context.Users.AnyAsync(u => u.NormalizedEmail == registerViewModel.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "Email already exist!");
                return View();
            }
            if (await _context.Users.AnyAsync(u => u.PhoneNumber == registerViewModel.PhoneNumber.ToUpper()))
            {
                ModelState.AddModelError("PhoneNumber", "Phonenumber already exist!");
                return View();
            }
            AppUser user = new AppUser()
            {
                FullName = registerViewModel.FullName,
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                PhoneNumber = registerViewModel.PhoneNumber,
            };
            string password = registerViewModel.Password;
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }
            return RedirectToAction("Login");
        }
    }
}
