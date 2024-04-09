using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using MVC_MiniProject_Allup.Business.Interfaces;
using MVC_MiniProject_Allup.CustomExceptions.CommonExceptions;
using MVC_MiniProject_Allup.CustomExceptions.SliderExceptions;
using MVC_MiniProject_Allup.DataAccesLayer;
using MVC_MiniProject_Allup.Extensions;
using MVC_MiniProject_Allup.Models;
using System.Linq.Expressions;

namespace MVC_MiniProject_Allup.Business.Implementations
{ // TODO: servis
    public class SliderService : ISliderService
    {
        private readonly AllupDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderService(AllupDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _env = environment;
        }
        public async Task<Slider> GetByIdAsync(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider is null) throw new SliderNotFoundException("Slider not found!");
            return slider;
        }
        public async Task<Slider> GetSingleAsync(Expression<Func<Slider, bool>>? expression = null)
        {
            var query = _context.Sliders.AsQueryable();
            return expression is not null
                ? await query.Where(expression).FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync();
        }
        public async Task<List<Slider>> GetSliderListAsync(Expression<Func<Slider, bool>>? expression = null, params string[] includes)
        {
            var query = _context.Sliders.AsQueryable();
            query = _getIncludes(query, includes);
            return expression is not null
                ? await query.Where(expression).ToListAsync()
                : await query.ToListAsync();
        }
        public async Task CreateAsync(Slider slider)
        {
            if (slider.ImageFile is null)
            {
                throw new ImageNullException("ImageFile", "Image is required!");
            }
            if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
            {
                throw new InvalidContentTypeException("ImageFile", "Please select only jpeg and png format images");
            }

            if (slider.ImageFile.Length > 3145728)
            {
                throw new SizeOfFileException("ImageFile", "Please select only photos with a size of less than 3mb");
            }

            slider.ImageUrl = slider.ImageFile.SaveFile(_env.WebRootPath, "uploads/sliders");
            slider.CreatedDate = DateTime.UtcNow.AddHours(4);
            slider.ModifiedDate = DateTime.UtcNow.AddHours(4);

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

        }
        public async Task UpdateAsync(Slider slider)
        {
            var existSlider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == slider.Id);
            if (existSlider is null)
            {
                throw new SliderNotFoundException("Slider not found!");
            }
            if (slider.ImageFile is not null)
            {
                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                {
                    throw new InvalidContentTypeException("ImageFile", "Please select only jpeg and png format images");
                }

                if (slider.ImageFile.Length > 3145728)
                {
                    throw new SizeOfFileException("ImageFile", "Please select only photos with a size of less than 3mb");
                }
                FileManager.DeleteFile(_env.WebRootPath, "uploads/sliders", existSlider.ImageUrl);
                slider.ImageUrl = slider.ImageFile.SaveFile(_env.WebRootPath, "uploads/sliders");
            }
            existSlider.ModifiedDate = DateTime.UtcNow.AddHours(4);
            existSlider.FirstTitle = slider.FirstTitle;
            existSlider.SecondTitle=slider.SecondTitle;
            existSlider.Description = slider.Description;
            existSlider.IsDeleted = slider.IsDeleted;
            existSlider.RedirectUrl = slider.RedirectUrl;
            existSlider.RedirectUrlText = slider.RedirectUrlText;
            await _context.SaveChangesAsync();

        }
        public async Task DeleteAsync(int id)
        {
            var existSlider = await _context.Sliders.FindAsync(id);
            if (existSlider is null) throw new SliderNotFoundException("Slider not found!");

            FileManager.DeleteFile(_env.WebRootPath, "uploads/sliders", existSlider.ImageUrl);

            _context.Remove(existSlider);
            await _context.SaveChangesAsync();
        }
        public async Task SoftDeleteAsync(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider is null) throw new SliderNotFoundException("Slider not found!");
            slider.ModifiedDate = DateTime.UtcNow.AddHours(4);
            slider.IsDeleted = !slider.IsDeleted;
        }

        private IQueryable<Slider> _getIncludes(IQueryable<Slider> query, params string[] includes)
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
}
