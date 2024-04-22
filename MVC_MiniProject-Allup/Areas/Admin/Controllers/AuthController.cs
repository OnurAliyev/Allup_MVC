using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_MiniProject_Allup.Areas.Admin.ViewModels;
using MVC_MiniProject_Allup.Models;

namespace MVC_MiniProject_Allup.Areas.Admin.Controllers;
[Area("Admin")]
public class AuthController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(AdminViewModel adminViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        AppUser admin = null;
        admin = await _userManager.FindByNameAsync(adminViewModel.UserName);
        if (admin is null)
        {
            ModelState.AddModelError("", "Wrong username or password!");
            return View();
            var result = await _signInManager.PasswordSignInAsync(admin, adminViewModel.Password,false,false);
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Wrong username or password!");
                return View();
            }
        }
            return RedirectToAction("Index", "Dashboard");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("login");
    }
}
