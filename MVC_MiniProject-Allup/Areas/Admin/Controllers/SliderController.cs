using Microsoft.AspNetCore.Mvc;
using MVC_MiniProject_Allup.Business.Interfaces;
using MVC_MiniProject_Allup.CustomExceptions.CommonExceptions;
using MVC_MiniProject_Allup.CustomExceptions.SliderExceptions;
using MVC_MiniProject_Allup.Models;

namespace MVC_MiniProject_Allup.Areas.Admin.Controllers;
[Area("Admin")]

public class SliderController : Controller
{
    private readonly ISliderService _sliderService;

    public SliderController(ISliderService sliderService)
    {
        _sliderService = sliderService;
    }
    public async Task<IActionResult> Index() => View(await _sliderService.GetSliderListAsync(s => s.IsDeleted == false));
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Slider slider)
    {
        if (!ModelState.IsValid) return View();
        try
        {
            await _sliderService.CreateAsync(slider);
        }
        catch (ImageNullException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch(InvalidContentTypeException ex)
        {
            ModelState.AddModelError(ex.PropertyName,ex.Message);
            return View();
        }
        catch(SizeOfFileException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("",ex.Message);
            return View();
        }
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Update(int id) => View(await _sliderService.GetByIdAsync(id));
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Slider slider)
    {
        if (!ModelState.IsValid) return View();
        try
        {
           await _sliderService.UpdateAsync(slider);
        }
        catch (SliderNotFoundException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
        catch(InvalidContentTypeException ex)
        {
            ModelState.AddModelError(ex.PropertyName,ex.Message);
        }
        catch(SizeOfFileException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch(Exception ex)
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
            await _sliderService.DeleteAsync(id);
        }
        catch (SliderNotFoundException ex)
        {
            ModelState.AddModelError("",ex.Message);
            return NotFound();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return NotFound();
        }
        return Ok();
    }
}
