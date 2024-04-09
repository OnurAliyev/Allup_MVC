using Microsoft.EntityFrameworkCore;
using MVC_MiniProject_Allup.Business.Interfaces;
using MVC_MiniProject_Allup.CustomExceptions.CommonExceptions;
using MVC_MiniProject_Allup.CustomExceptions.ProductExceptions;
using MVC_MiniProject_Allup.DataAccesLayer;
using MVC_MiniProject_Allup.Extensions;
using MVC_MiniProject_Allup.Models;
using System.Linq.Expressions;

namespace MVC_MiniProject_Allup.Business.Implementations;

public class ProductService : IProductService
{
    private readonly AllupDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ProductService(AllupDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    public async Task CreateAsync(Product product)
    {
        if (product.CoverImageFile is null) throw new ImageNullException("CoverImageFile", "Image is required!");
        if (product.CoverImageFile.ContentType != "image/jpeg" && product.CoverImageFile.ContentType != "image/png")
        {
            throw new InvalidContentTypeException("CoverImageFile", "Please select only jpeg and png format images");
        }
        if (product.CoverImageFile.Length > 3145728)
        {
            throw new SizeOfFileException("CoverImageFile", "Please select only photos with a size of less than 3mb");
        }
        ProductImages coverImage = new ProductImages() 
        {
            Product=product,
            ImageUrl=product.CoverImageFile.SaveFile(_env.WebRootPath,"uploads/products"),
            IsCover=true
        };
        await _context.ProductImages.AddAsync(coverImage);
        await _context.SaveChangesAsync();

        if (product.HoverImageFile is null) throw new ImageNullException("HoverImageFile", "Image is required!");
        if (product.HoverImageFile.ContentType != "image/jpeg" && product.HoverImageFile.ContentType != "image/png")
        {
            throw new InvalidContentTypeException("HoverImageFile", "Please select only jpeg and png format images");
        }
        if (product.HoverImageFile.Length > 3145728)
        {
            throw new SizeOfFileException("HoverImageFile", "Please select only photos with a size of less than 3mb");
        }
        ProductImages hoverImage = new ProductImages()
        {
            Product=product,
            ImageUrl =product.HoverImageFile.SaveFile(_env.WebRootPath,"uploads/products"),
            IsCover=false
        };
        await _context.ProductImages.AddAsync(hoverImage);
        await _context.SaveChangesAsync();

        if (product.ImageFiles is null) throw new ImageNullException("ImageFiles", "Image is required!");
        foreach(var file in product.ImageFiles)
        {
            if( file.ContentType != "image/jpeg" && file.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("ImageFiles", "Please select only jpeg and png format images");
            }
            if(file.Length> 3145728)
            {
                throw new SizeOfFileException("ImageFiles", "Please select only photos with a size of less than 3mb");
            }
            ProductImages images = new ProductImages()
            {
                Product=product,
                ImageUrl=file.SaveFile(_env.WebRootPath,"uploads/products"),
                IsCover=null
            };
            await _context.ProductImages.AddAsync(images);
            await _context.SaveChangesAsync();
        }
        
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Product product)
    {
        var existProduct = await _context.Products.FirstOrDefaultAsync(p=>p.Id == product.Id);
        if (existProduct is null) throw new ProductNotFoundException("Product not found!");
        if((await _context.Products.FirstOrDefaultAsync
            (p=>p.Name == product.Name) is not null) && (product.Name != existProduct.Name))
        {
            throw new AlreadyExistException("Name", "Product with this name already exist!");
        }
        if(product.CoverImageFile is not null)
        {
            if(product.CoverImageFile.ContentType !="image/jpeg" && product.CoverImageFile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("CoverImageFile", "Please select only jpeg and png format images");
            }
            if(product.CoverImageFile.Length> 3145728)
            {
                throw new SizeOfFileException("CoverImageFile", "Please select only photos with a size of less than 3mb");
            }
            
            ProductImages? coverImage= await _context.ProductImages
                .Where(ci=>ci.IsCover==true&& ci.ProductId == existProduct.Id)
                .FirstOrDefaultAsync();
            if (coverImage is not null)
            {
                FileManager.DeleteFile(_env.WebRootPath, "uploads/products", coverImage.ImageUrl);
                _context.ProductImages.Remove(coverImage);
            }
            ProductImages newCoverImage = new ProductImages() 
            {
                Product=product,
                ImageUrl=product.CoverImageFile.SaveFile(_env.WebRootPath,"uploads/products"),
                IsCover=true,
            };
            await _context.ProductImages.AddRangeAsync(newCoverImage);
            await _context.SaveChangesAsync();
        }
        if(product.HoverImageFile is not null)
        {
            if (product.HoverImageFile.ContentType != "image/jpeg" && product.HoverImageFile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("HoverImageFile", "Please select only jpeg and png format images");
            }
            if (product.HoverImageFile.Length > 3145728)
            {
                throw new SizeOfFileException("HoverImageFile", "Please select only photos with a size of less than 3mb");
            }

            ProductImages? hoverImage = await _context.ProductImages
                .Where(ci => ci.IsCover == false && ci.ProductId == existProduct.Id)
                .FirstOrDefaultAsync();
            if (hoverImage is not null)
            {
                FileManager.DeleteFile(_env.WebRootPath, "uploads/products", hoverImage.ImageUrl);
                _context.ProductImages.Remove(hoverImage);
            }
            ProductImages newHoverImage = new ProductImages()
            {
                Product = product,
                ImageUrl = product.HoverImageFile.SaveFile(_env.WebRootPath, "uploads/products"),
                IsCover = true,
            };
            await _context.ProductImages.AddRangeAsync(newHoverImage);
            await _context.SaveChangesAsync();
        }
        if(product.ImageFiles is not null)
        {
            foreach (var file in product.ImageFiles)
            {
                existProduct.ProductImages.RemoveAll(pi => !product.ProductImageIds.Contains(pi.Id) && pi.IsCover == null);
                if(file.ContentType != "image/jpeg" && file.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException("ImageFiles", "Please select only jpeg and png format images");
                }
                if (file.Length > 3145728)
                {
                    throw new SizeOfFileException("ImageFiles", "Please select only photos with a size of less than 3mb");
                }
                ProductImages images = new ProductImages()
                {
                    Product = product,
                    ImageUrl=file.SaveFile(_env.WebRootPath,"uploads/products"),
                    IsCover=null
                };
                await _context.ProductImages.AddAsync(images);
                await _context.SaveChangesAsync();
            }
        }
        existProduct.Name= product.Name;
        existProduct.Description= product.Description;
        existProduct.CostPrice= product.CostPrice;
        existProduct.SalePrice= product.SalePrice;
        existProduct.DiscountPercent= product.DiscountPercent;
        existProduct.ModifiedDate = DateTime.UtcNow.AddHours(4);
        existProduct.CategoryId= product.CategoryId;
        existProduct.BrandId= product.BrandId;
        existProduct.IsBestSeller= product.IsBestSeller;
        existProduct.IsNew= product.IsNew;
        existProduct.IsFeatured= product.IsFeatured;
        existProduct.IsDeleted= product.IsDeleted;
        existProduct.ProductCode= product.ProductCode;
        existProduct.StockCount= product.StockCount;
        await _context.SaveChangesAsync();

    }
    public async Task DeleteAsync(int id)
    {
        var foundedProduct = await _context.Products.FindAsync(id);
        if (foundedProduct is null) throw new ProductNotFoundException("Product not found!");

        ProductImages? coverImage = await _context.ProductImages
            .Where(pi => pi.IsCover == true && pi.ProductId == foundedProduct.Id)
            .FirstOrDefaultAsync();
        FileManager.DeleteFile(_env.WebRootPath, "uploads/products",coverImage.ImageUrl);
        _context.ProductImages.Remove(coverImage);

        ProductImages? hoverImage = await _context.ProductImages
            .Where(pi => pi.IsCover == false && pi.ProductId == foundedProduct.Id)
            .FirstOrDefaultAsync();
        FileManager.DeleteFile(_env.WebRootPath, "uploads/products", hoverImage.ImageUrl);
        _context.ProductImages.Remove(hoverImage);

        List<ProductImages> productImages = await _context.ProductImages
            .Where(pi=>pi.IsCover ==null && pi.ProductId ==foundedProduct.Id)
            .ToListAsync();
        foreach (var file in productImages)
        {
            FileManager.DeleteFile(_env.WebRootPath, "uploads/products", file.ImageUrl);
            _context.ProductImages.RemoveRange(file);
        }
        _context.Products.Remove(foundedProduct);
    }
    public async Task<Product> GetByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) throw new ProductNotFoundException("Product not found!");
        return product;
    }
    public async Task<List<Product>> GetAllProductsAsync(Expression<Func<Product,bool>>? expression =null, params string[] includes)
    {
        var query = _context.Products.AsQueryable();
        query = _getIncludes(query, includes);
        return expression is not null
            ? await query.Where(expression).ToListAsync() 
            : await query.ToListAsync();
    }
    public async Task<Product> GetSingleAsync(Expression<Func<Product,bool>>? expression = null,params string[] includes)
    {
        var query = _context.Products.AsQueryable();
        query = _getIncludes(query, includes);
        return expression is not null
            ? await query.Where(expression).FirstOrDefaultAsync()
            : await query.FirstOrDefaultAsync();
    }

    private IQueryable<Product> _getIncludes(IQueryable<Product> query, params string[] includes)
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
