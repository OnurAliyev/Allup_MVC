using Microsoft.AspNetCore.Mvc;
using MVC_MiniProject_Allup.Business.Interfaces;
using MVC_MiniProject_Allup.CustomExceptions.BrandExceptions;
using MVC_MiniProject_Allup.CustomExceptions.CategoryExceptions;
using MVC_MiniProject_Allup.CustomExceptions.CommonExceptions;
using MVC_MiniProject_Allup.Models;

namespace MVC_MiniProject_Allup.Areas.Admin.Controllers;
[Area("Admin")]
public class BrandController : Controller
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }
    public async Task<IActionResult> Index() => View(await _brandService.GetAllBrandsAsync(c => c.IsDeleted == false));
    public IActionResult Create() => View();
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Brand brand)
    {
        if (!ModelState.IsValid) return View();
        try
        {
            await _brandService.CreateAsync(brand);
        }
        catch (AlreadyExistException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch (SizeOfFileException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch (InvalidContentTypeException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Update(int id) => View(await _brandService.GetByIdAsync(id));
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Brand brand)
    {
        if (!ModelState.IsValid) return View();
        try
        {
            await _brandService.UpdateAsync(brand);
        }
        catch (BrandNotFoundException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
        catch (SizeOfFileException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch (InvalidContentTypeException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _brandService.DeleteAsync(id);
        }
        catch (CategoryNotFoundException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return NotFound();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return NotFound();
        }
        return Ok();
    }
}
