using Microsoft.AspNetCore.Mvc;

namespace MVC_MiniProject_Allup.Areas.Admin.Controllers;
[Area("Admin")]

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
