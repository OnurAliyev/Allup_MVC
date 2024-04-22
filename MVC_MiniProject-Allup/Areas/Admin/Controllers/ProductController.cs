using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_MiniProject_Allup.Business.Interfaces;
using MVC_MiniProject_Allup.CustomExceptions.CommonExceptions;
using MVC_MiniProject_Allup.CustomExceptions.ProductExceptions;
using MVC_MiniProject_Allup.Models;

namespace MVC_MiniProject_Allup.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin,SuperAdmin")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IBrandService _brandService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService,IBrandService brandService , ICategoryService categoryService)
    {
        _productService = productService;
        _brandService = brandService;
        _categoryService = categoryService;
    }
    public async Task<IActionResult> Index() => View(await _productService.GetAllProductsAsync(null,"Category","Brand","ProductImages"));
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Brands = await _brandService.GetAllBrandsAsync();
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Create(Product product)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Brands = await _brandService.GetAllBrandsAsync();
        if (!ModelState.IsValid) return View();
        try
        {
            await _productService.CreateAsync(product);
        }
        catch (InvalidContentTypeException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch(SizeOfFileException ex)
        {
            ModelState.AddModelError(ex.PropertyName,ex.Message);
            return View();
        }
        catch(ImageNullException ex)
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
    public async Task<IActionResult> Update(int id)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Brands = await _brandService.GetAllBrandsAsync();
        Product? product=null;
        try
        {
            product = await _productService.GetSingleAsync(p => p.Id == id, "Category", "Brand", "ProductImages");
        }
        catch (ProductNotFoundException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        return View(product);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Update(Product product)
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Brands = await _brandService.GetAllBrandsAsync();
        if (!ModelState.IsValid) return View();
        try
        {

        }
        catch (SizeOfFileException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch (InvalidContentTypeException ex)
        {
            ModelState.AddModelError(ex.PropertyName,ex.Message);
            return View();
        }
        catch(AlreadyExistException ex)
        {
            ModelState.AddModelError(ex.PropertyName, ex.Message);
            return View();
        }
        catch(ProductNotFoundException ex)
        {
            ModelState.AddModelError("",ex.Message);
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

        }
        catch (ProductNotFoundException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return NotFound();
        }
        catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
        return Ok();
    }
}
