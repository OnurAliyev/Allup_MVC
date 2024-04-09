using Microsoft.EntityFrameworkCore;
using MVC_MiniProject_Allup.Business.Interfaces;
using MVC_MiniProject_Allup.CustomExceptions.CategoryExceptions;
using MVC_MiniProject_Allup.CustomExceptions.CommonExceptions;
using MVC_MiniProject_Allup.DataAccesLayer;
using MVC_MiniProject_Allup.Extensions;
using MVC_MiniProject_Allup.Models;
using System.Linq.Expressions;

namespace MVC_MiniProject_Allup.Business.Implementations;

public class CategoryService : ICategoryService
{
    private readonly AllupDbContext _context;
    private readonly IWebHostEnvironment _env;

    public CategoryService(AllupDbContext context,IWebHostEnvironment env)
    {
        _context=context;
        _env=env;
    }
    public async Task<List<Category>> GetAllCategoriesAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
    {
       var query=_context.Categories.AsQueryable();
        query=_getIncludes(query, includes);
        return expression is not null
            ? await query.Where(expression).ToListAsync()
            : await query.ToListAsync();
    }
    public async Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes)
    {
        var query = _context.Categories.AsQueryable();
        query=_getIncludes(query, includes);
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
    public async Task CreateAsync(Category category)
    {
        var _category = await _context.Categories.FirstOrDefaultAsync(c=>c.Name == category.Name);
        if (_category is not null)
        {
            throw new AlreadyExistException("Name", "Category with this name already exist!");
        }
        if(category.ImageFile is null)
        {
            throw new ImageNullException("ImageFile", "Image is required!");
        }
        if(category.ImageFile.ContentType !="image/jpeg" && category.ImageFile.ContentType!="image/png")
        {
            throw new InvalidContentTypeException("ImageFile", "Please select only jpeg and png format images");
        }
        if (category.ImageFile.Length>3145728)
        {
            throw new SizeOfFileException("ImageFile", "Please select only photos with a size of less than 3mb");
        }
        category.ImageUrl=category.ImageFile.SaveFile(_env.WebRootPath, "uploads/categories");
        category.CreatedDate=DateTime.UtcNow.AddHours(4);
        category.ModifiedDate=DateTime.UtcNow.AddHours(4);
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Category category)
    {
        var existCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id==category.Id);
        if (existCategory is null) throw new CategoryNotFoundException("Category not found!");
        if(category.ImageFile is not null)
        {
            if (category.ImageFile.ContentType !="image/jpeg"&&category.ImageFile.ContentType!="image/png")
            {
                throw new InvalidContentTypeException("ImageFile", "Please select only jpeg and png format images");
            }
            if (category.ImageFile.Length>3145728)
            {
                throw new SizeOfFileException("ImageFile", "Please select only photos with a size of less than 3mb");
            }
            FileManager.DeleteFile(_env.WebRootPath, "uploads/categories", existCategory.ImageUrl);
            category.ImageUrl = category.ImageFile.SaveFile(_env.WebRootPath, "uploads/categories");
        }
        existCategory.ModifiedDate=DateTime.UtcNow.AddHours(4);
        existCategory.Name=category.Name;
        existCategory.IsDeleted=category.IsDeleted;
        existCategory.ImageUrl=category.ImageUrl;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(int id)
    {
        var existCategory = await _context.Categories.FindAsync(id);
        if (existCategory is null) throw new CategoryNotFoundException("Category not found!");

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

