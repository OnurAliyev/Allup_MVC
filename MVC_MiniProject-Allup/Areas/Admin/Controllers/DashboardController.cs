using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_MiniProject_Allup.DataAccesLayer;
using MVC_MiniProject_Allup.Models;

namespace MVC_MiniProject_Allup.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin,SuperAdmin")]

public class DashboardController : Controller
{
    private readonly AllupDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DashboardController(AllupDbContext context,UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public IActionResult Index()
    {
        return View();
    }
    //public async Task<IActionResult> CreateAdmin()
    //{
    //    AppUser SuperAdmin = new AppUser()
    //    {
    //        FullName = "Onur Aliyev",
    //        UserName="onuraliyev",
    //        Email="onurda@code.edu.az"
    //    };
    //    string password = "Onur123@";
    //    var result = await _userManager.CreateAsync(SuperAdmin, password);                           
    //    return Ok(result);
    //}
    //public async Task<IActionResult> CreateRole()
    //{
    //    IdentityRole roleSuperAdmin = new IdentityRole("SuperAdmin");
    //    IdentityRole roleAdmin = new IdentityRole("Admin");
    //    IdentityRole roleMember = new IdentityRole("Member");

    //    await _roleManager.CreateAsync(roleSuperAdmin);  
    //    await _roleManager.CreateAsync(roleAdmin);  
    //    await _roleManager.CreateAsync(roleMember);
    //    return Ok();
    //}
    //public async Task<IActionResult> AddRole()
    //{
    //    AppUser superAdmin = await _userManager.FindByNameAsync("onuraliyev");
    //    var result = await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
    //    return Ok(result);
    //}
}
