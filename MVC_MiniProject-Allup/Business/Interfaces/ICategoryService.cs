using MVC_MiniProject_Allup.Models;
using System.Linq.Expressions;

namespace MVC_MiniProject_Allup.Business.Interfaces;

public interface ICategoryService
{
    public Task<List<Category>> GetAllCategoriesAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes);
    public Task<Category> GetSingleAsync(Expression<Func<Category, bool>>? expression = null, params string[] includes);
    public Task<Category> GetByIdAsync(int id);
    public Task CreateAsync(Category category);
    public Task UpdateAsync(Category category);
    public Task DeleteAsync(int id);
}
