using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_MiniProject_Allup.Business.Interfaces;
using MVC_MiniProject_Allup.CustomExceptions.CategoryExceptions;
using MVC_MiniProject_Allup.CustomExceptions.CommonExceptions;
using MVC_MiniProject_Allup.Models;

namespace MVC_MiniProject_Allup.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    public async Task< IActionResult> Index() => View(await _categoryService.GetAllCategoriesAsync(c=>c.IsDeleted==false));
    public IActionResult Create() => View();
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid) return View();
        try
        {
            await _categoryService.CreateAsync(category);
        }
        catch (AlreadyExistException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch(SizeOfFileException ex)
        {
            ModelState.AddModelError(ex.PropertyName , ex.Message);
            return View();
        }
        catch(InvalidContentTypeException ex)
        {
            ModelState.AddModelError (ex.PropertyName , ex.Message);
            return View();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("",ex.Message);
            return View();
        }
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Update(int id) => View(await _categoryService.GetByIdAsync(id));
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Update(Category category)
    {
        if (!ModelState.IsValid) return View();
        try
        {
            await _categoryService.UpdateAsync(category);
        }
        catch (CategoryNotFoundException ex)
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
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _categoryService.DeleteAsync(id);
        }
        catch (CategoryNotFoundException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return NotFound();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("",ex.Message);
            return NotFound();
        }
        return Ok();
    }
}
