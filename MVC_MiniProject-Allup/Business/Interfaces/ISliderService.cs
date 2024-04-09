using MVC_MiniProject_Allup.Models;
using System.Linq.Expressions;

namespace MVC_MiniProject_Allup.Business.Interfaces;

public interface ISliderService
{
    Task<Slider> GetByIdAsync(int id);
    Task<List<Slider>> GetSliderListAsync(Expression<Func<Slider, bool>>? expression = null, params string[] includes);
    Task<Slider> GetSingleAsync(Expression<Func<Slider, bool>>? expression = null);
    Task CreateAsync(Slider slider);
    Task UpdateAsync(Slider slider);
    Task DeleteAsync(int id);
    Task SoftDeleteAsync(int id);
}