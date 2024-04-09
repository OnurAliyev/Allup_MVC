using MVC_MiniProject_Allup.Models;
using System.Linq.Expressions;

namespace MVC_MiniProject_Allup.Business.Interfaces;

public interface IProductService
{
    public Task<List<Product>> GetAllProductsAsync(Expression<Func<Product, bool>>? expression = null,params string[] includes);
    public Task<Product> GetSingleAsync(Expression<Func<Product,bool>>? expression = null);
    public Task<Product> GetByIdAsync(int id);
    public Task CreateAsync(Product product);
    public Task UpdateAsync(Product product);
    public Task DeleteAsync(int id);
}
