using Microsoft.AspNetCore.Mvc;
using MVC_MiniProject_Allup.ViewModels;

namespace MVC_MiniProject_Allup.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        HomeViewModel viewModel= new HomeViewModel() 
        {
        };
        return View(viewModel);
    }

}
