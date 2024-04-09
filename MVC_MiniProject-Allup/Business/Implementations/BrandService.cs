﻿using Microsoft.EntityFrameworkCore;
using MVC_MiniProject_Allup.Business.Interfaces;
using MVC_MiniProject_Allup.CustomExceptions.CategoryExceptions;
using MVC_MiniProject_Allup.CustomExceptions.CommonExceptions;
using MVC_MiniProject_Allup.CustomExceptions.SliderExceptions;
using MVC_MiniProject_Allup.DataAccesLayer;
using MVC_MiniProject_Allup.Extensions;
using MVC_MiniProject_Allup.Models;
using System.Linq.Expressions;
using System.Linq;

namespace MVC_MiniProject_Allup.Business.Implementations;

public class BrandService:IBrandService
{
    private readonly AllupDbContext _context;
    private readonly IWebHostEnvironment _env;
    public BrandService(AllupDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    public async Task<List<Brand>> GetAllBrandsAsync(Expression<Func<Brand, bool>>? expression = null, params string[] includes)
    {
        var query = _context.Categories.AsQueryable();
        query = _getIncludes(query, includes);
        return expression is not null
            ? await query.Where(expression).ToListAsync()
            : await query.ToListAsync();
    }
    public async Task<Brand> GetSingleAsync(Expression<Func<Brand, bool>>? expression = null, params string[] includes)
    {
        var query = _context.Categories.AsQueryable();
        query = _getIncludes(query, includes);
        return expression is not null
            ? await query.Where(expression).FirstOrDefaultAsync()
            : await query.FirstOrDefaultAsync();

    }
    public async Task<Category> GetByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category is null) throw new CategoryNotFoundException("Category not found!");
        return category;
    }
    public async Task CreateAsync(Brand brand)
    {
        var _brand = await _context.Brands.FirstOrDefaultAsync(b => b.Name == brand.Name);
        if (_brand is not null)
        {
            throw new AlreadyExistException("Name", "Brand with this name already exist!");
        }
        if (brand.LogoImageFile is null)
        {
            throw new ImageNullException("LogoImageFile", "Image is required!");
        }
        if (brand.LogoImageFile.ContentType != "image/jpeg" && brand.LogoImageFile.ContentType != "image/png")
        {
            throw new InvalidContentTypeException("LogoImageFile", "Please select only jpeg and png format images");
        }
        if (brand.LogoImageFile.Length > 3145728)
        {
            throw new SizeOfFileException("LogoImageFile", "Please select only photos with a size of less than 3mb");
        }
        brand.BrandLogoUrl = brand.LogoImageFile.SaveFile(_env.WebRootPath, "uploads/brands");
        brand.CreatedDate = DateTime.UtcNow.AddHours(4);
        brand.ModifiedDate = DateTime.UtcNow.AddHours(4);
        await _context.Brands.AddAsync(brand);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Brand brand)
    {
        var existBrand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == brand.Id);
        if (existBrand is null) throw new CategoryNotFoundException("Category not found!");
        if (brand.LogoImageFile is not null)
        {
            if (brand.LogoImageFile.ContentType != "image/jpeg" && brand.LogoImageFile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("LogoImageFile", "Please select only jpeg and png format images");
            }
            if (brand.LogoImageFile.Length > 3145728)
            {
                throw new SizeOfFileException("LogoImageFile", "Please select only photos with a size of less than 3mb");
            }
            FileManager.DeleteFile(_env.WebRootPath, "uploads/sliders", existBrand.BrandLogoUrl);
            brand.BrandLogoUrl = brand.LogoImageFile.SaveFile(_env.WebRootPath, "uploads/sliders");
        }
        existBrand.ModifiedDate = DateTime.UtcNow.AddHours(4);
        existBrand.Name = brand.Name;
        existBrand.IsDeleted = brand.IsDeleted;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var existCategory = await _context.Categories.FindAsync(id);
        if (existCategory is null) throw new SliderNotFoundException("Slider not found!");

        FileManager.DeleteFile(_env.WebRootPath, "uploads/categories", existCategory.ImageUrl);

        _context.Remove(existCategory);
        await _context.SaveChangesAsync();
    }

    private IQueryable<Category> _getIncludes(IQueryable<Category> query, params string[] includes)
    {
        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query;
    }
}